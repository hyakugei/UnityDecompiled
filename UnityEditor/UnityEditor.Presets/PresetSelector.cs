using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor.Experimental.AssetImporters;
using UnityEngine;

namespace UnityEditor.Presets
{
	public class PresetSelector : EditorWindow
	{
		private static class Style
		{
			public static GUIStyle bottomBarBg = "ProjectBrowserBottomBarBg";

			public static GUIStyle toolbarBack = "ObjectPickerToolbar";

			public static GUIContent presetIcon = EditorGUIUtility.IconContent("Preset.Context");
		}

		private string m_SearchField;

		private IEnumerable<Preset> m_Presets;

		private ObjectListAreaState m_ListAreaState;

		private ObjectListArea m_ListArea;

		private const float kMinTopSize = 250f;

		private const float kMinWidth = 200f;

		private const float kPreviewMargin = 5f;

		private const float kPreviewExpandedAreaHeight = 75f;

		private SavedInt m_StartGridSize = new SavedInt("PresetSelector.GridSize", 64);

		private bool m_CanCreateNew;

		private int m_ModalUndoGroup = -1;

		private UnityEngine.Object m_MainTarget;

		private static PresetSelector s_SharedPresetSelector = null;

		private PresetSelectorReceiver m_EventObject;

		internal static PresetSelector get
		{
			get
			{
				if (PresetSelector.s_SharedPresetSelector == null)
				{
					UnityEngine.Object[] array = Resources.FindObjectsOfTypeAll(typeof(PresetSelector));
					if (array != null && array.Length > 0)
					{
						PresetSelector.s_SharedPresetSelector = (PresetSelector)array[0];
					}
					if (PresetSelector.s_SharedPresetSelector == null)
					{
						PresetSelector.s_SharedPresetSelector = ScriptableObject.CreateInstance<PresetSelector>();
					}
				}
				return PresetSelector.s_SharedPresetSelector;
			}
		}

		[EditorHeaderItem(typeof(UnityEngine.Object), -1001)]
		public static bool DrawPresetButton(Rect rectangle, UnityEngine.Object[] targets)
		{
			UnityEngine.Object @object = targets[0];
			bool result;
			if (Preset.IsObjectExcludedFromPresets(@object) || (@object.hideFlags & HideFlags.NotEditable) != HideFlags.None)
			{
				result = false;
			}
			else
			{
				if (EditorGUI.DropdownButton(rectangle, PresetSelector.Style.presetIcon, FocusType.Passive, EditorStyles.iconButton))
				{
					PresetContextMenu.CreateAndShow(targets);
				}
				result = true;
			}
			return result;
		}

		public static void ShowSelector(UnityEngine.Object[] targets, Preset currentSelection, bool createNewAllowed)
		{
			DefaultPresetSelectorReceiver defaultPresetSelectorReceiver = ScriptableObject.CreateInstance<DefaultPresetSelectorReceiver>();
			defaultPresetSelectorReceiver.Init(targets);
			PresetSelector.ShowSelector(targets[0], currentSelection, createNewAllowed, defaultPresetSelectorReceiver);
		}

		public static void ShowSelector(UnityEngine.Object target, Preset currentSelection, bool createNewAllowed, PresetSelectorReceiver eventReceiver)
		{
			PresetSelector.get.Init(target, currentSelection, createNewAllowed, eventReceiver);
		}

		private void Init(UnityEngine.Object target, Preset currentSelection, bool createNewAllowed, PresetSelectorReceiver eventReceiver)
		{
			this.m_ModalUndoGroup = Undo.GetCurrentGroup();
			ContainerWindow.SetFreezeDisplay(true);
			this.m_SearchField = string.Empty;
			this.m_MainTarget = target;
			this.InitListArea();
			this.m_Presets = PresetSelector.FindAllPresetsForObject(target);
			this.UpdateSearchResult((!(currentSelection != null)) ? 0 : currentSelection.GetInstanceID());
			this.m_EventObject = eventReceiver;
			this.m_CanCreateNew = createNewAllowed;
			base.ShowWithMode(ShowMode.AuxWindow);
			base.titleContent = EditorGUIUtility.TrTextContent("Select Preset", null, null);
			Rect position = this.m_Parent.window.position;
			position.width = EditorPrefs.GetFloat("PresetSelectorWidth", 200f);
			position.height = EditorPrefs.GetFloat("PresetSelectorHeight", 390f);
			base.position = position;
			base.minSize = new Vector2(200f, 335f);
			base.maxSize = new Vector2(10000f, 10000f);
			base.Focus();
			ContainerWindow.SetFreezeDisplay(false);
			this.m_Parent.AddToAuxWindowList();
		}

		private static IEnumerable<Preset> FindAllPresetsForObject(UnityEngine.Object target)
		{
			return from a in AssetDatabase.FindAssets("t:Preset")
			select AssetDatabase.LoadAssetAtPath<Preset>(AssetDatabase.GUIDToAssetPath(a)) into preset
			where preset.CanBeAppliedTo(target)
			select preset;
		}

		private void InitListArea()
		{
			if (this.m_ListAreaState == null)
			{
				this.m_ListAreaState = new ObjectListAreaState();
			}
			if (this.m_ListArea == null)
			{
				this.m_ListArea = new ObjectListArea(this.m_ListAreaState, this, true);
				this.m_ListArea.allowDeselection = false;
				this.m_ListArea.allowDragging = false;
				this.m_ListArea.allowFocusRendering = false;
				this.m_ListArea.allowMultiSelect = false;
				this.m_ListArea.allowRenaming = false;
				this.m_ListArea.allowBuiltinResources = false;
				ObjectListArea expr_84 = this.m_ListArea;
				expr_84.repaintCallback = (Action)Delegate.Combine(expr_84.repaintCallback, new Action(base.Repaint));
				ObjectListArea expr_AB = this.m_ListArea;
				expr_AB.itemSelectedCallback = (Action<bool>)Delegate.Combine(expr_AB.itemSelectedCallback, new Action<bool>(this.ListAreaItemSelectedCallback));
				this.m_ListArea.gridSize = this.m_StartGridSize.value;
			}
		}

		private void UpdateSearchResult(int currentSelection)
		{
			int[] instanceIDs = (from p in this.m_Presets
			where p.name.ToLower().Contains(this.m_SearchField.ToLower())
			select p.GetInstanceID()).ToArray<int>();
			this.m_ListArea.ShowObjectsInList(instanceIDs);
			this.m_ListArea.InitSelection(new int[]
			{
				currentSelection
			});
		}

		private void ListAreaItemSelectedCallback(bool doubleClicked)
		{
			if (doubleClicked)
			{
				base.Close();
				GUIUtility.ExitGUI();
			}
			else if (this.m_EventObject != null)
			{
				this.m_EventObject.OnSelectionChanged(this.GetCurrentSelection());
			}
		}

		private void OnGUI()
		{
			this.m_ListArea.HandleKeyboard(false);
			this.HandleKeyInput();
			EditorGUI.FocusTextInControl("ComponentSearch");
			this.DrawSearchField();
			Rect controlRect = EditorGUILayout.GetControlRect(true, new GUILayoutOption[]
			{
				GUILayout.ExpandHeight(true)
			});
			int controlID = GUIUtility.GetControlID(FocusType.Keyboard);
			this.m_ListArea.OnGUI(new Rect(0f, controlRect.y, base.position.width, controlRect.height), controlID);
			using (new EditorGUILayout.HorizontalScope(PresetSelector.Style.bottomBarBg, new GUILayoutOption[]
			{
				GUILayout.MinHeight(24f)
			}))
			{
				if (this.m_CanCreateNew)
				{
					if (GUILayout.Button("Save current to...", new GUILayoutOption[0]))
					{
						PresetSelector.CreatePreset(this.m_MainTarget);
					}
				}
				GUILayout.FlexibleSpace();
				if (this.m_ListArea.CanShowThumbnails())
				{
					using (EditorGUI.ChangeCheckScope changeCheckScope = new EditorGUI.ChangeCheckScope())
					{
						int gridSize = (int)GUILayout.HorizontalSlider((float)this.m_ListArea.gridSize, (float)this.m_ListArea.minGridSize, (float)this.m_ListArea.maxGridSize, new GUILayoutOption[]
						{
							GUILayout.Width(55f)
						});
						if (changeCheckScope.changed)
						{
							this.m_ListArea.gridSize = gridSize;
						}
					}
				}
			}
		}

		private void DrawSearchField()
		{
			using (EditorGUI.ChangeCheckScope changeCheckScope = new EditorGUI.ChangeCheckScope())
			{
				GUI.SetNextControlName("ComponentSearch");
				Rect controlRect = EditorGUILayout.GetControlRect(false, 24f, PresetSelector.Style.toolbarBack, new GUILayoutOption[0]);
				controlRect.height = 40f;
				GUI.Label(controlRect, GUIContent.none, PresetSelector.Style.toolbarBack);
				this.m_SearchField = EditorGUI.SearchField(new Rect(5f, 5f, base.position.width - 10f, 15f), this.m_SearchField);
				if (changeCheckScope.changed)
				{
					this.UpdateSearchResult(0);
				}
			}
		}

		private void HandleKeyInput()
		{
			if (Event.current.type == EventType.KeyDown)
			{
				KeyCode keyCode = Event.current.keyCode;
				if (keyCode != KeyCode.Escape)
				{
					if (keyCode == KeyCode.KeypadEnter || keyCode == KeyCode.Return)
					{
						base.Close();
						Event.current.Use();
						GUIUtility.ExitGUI();
					}
				}
				else if (this.m_SearchField == string.Empty)
				{
					this.Cancel();
				}
			}
		}

		private void OnDisable()
		{
			if (this.m_ListArea != null)
			{
				this.m_StartGridSize.value = this.m_ListArea.gridSize;
			}
			if (this.m_EventObject != null)
			{
				this.m_EventObject.OnSelectionClosed(this.GetCurrentSelection());
			}
			Undo.CollapseUndoOperations(this.m_ModalUndoGroup);
		}

		private void OnDestroy()
		{
			if (this.m_ListArea != null)
			{
				this.m_ListArea.OnDestroy();
			}
		}

		private Preset GetCurrentSelection()
		{
			Preset result = null;
			if (this.m_ListArea != null)
			{
				int[] selection = this.m_ListArea.GetSelection();
				if (selection != null && selection.Length > 0)
				{
					result = (EditorUtility.InstanceIDToObject(selection[0]) as Preset);
				}
			}
			return result;
		}

		private void Cancel()
		{
			Undo.RevertAllDownToGroup(this.m_ModalUndoGroup);
			this.m_ListArea.InitSelection(new int[0]);
			base.Close();
			GUI.changed = true;
			GUIUtility.ExitGUI();
		}

		private static bool ApplyImportSettingsBeforeSavingPreset(ref Preset preset, UnityEngine.Object target)
		{
			InspectorWindow[] allInspectorWindows = InspectorWindow.GetAllInspectorWindows();
			bool result;
			for (int i = 0; i < allInspectorWindows.Length; i++)
			{
				InspectorWindow inspectorWindow = allInspectorWindows[i];
				ActiveEditorTracker tracker = inspectorWindow.tracker;
				Editor[] activeEditors = tracker.activeEditors;
				int j = 0;
				while (j < activeEditors.Length)
				{
					Editor editor = activeEditors[j];
					AssetImporterEditor assetImporterEditor = editor as AssetImporterEditor;
					if (assetImporterEditor != null && assetImporterEditor.target == target && assetImporterEditor.HasModified())
					{
						if (EditorUtility.DisplayDialog("Unapplied import settings", "Apply settings before creating a new preset", "Apply", "Cancel"))
						{
							assetImporterEditor.ApplyAndImport();
							preset.UpdateProperties(assetImporterEditor.target);
							result = false;
							return result;
						}
						result = true;
						return result;
					}
					else
					{
						j++;
					}
				}
			}
			result = false;
			return result;
		}

		private static string CreatePresetDialog(ref Preset preset, UnityEngine.Object target)
		{
			string result;
			if (target is AssetImporter && PresetSelector.ApplyImportSettingsBeforeSavingPreset(ref preset, target))
			{
				result = null;
			}
			else
			{
				result = EditorUtility.SaveFilePanelInProject("New Preset", preset.GetTargetTypeName(), "preset", "", ProjectWindowUtil.GetActiveFolderPath());
			}
			return result;
		}

		private static void CreatePreset(UnityEngine.Object target)
		{
			Preset preset = new Preset(target);
			string text = PresetSelector.CreatePresetDialog(ref preset, target);
			if (!string.IsNullOrEmpty(text))
			{
				Preset preset2 = AssetDatabase.LoadAssetAtPath<Preset>(text);
				if (preset2 != null)
				{
					EditorUtility.CopySerialized(preset, preset2);
					preset2.name = Path.GetFileNameWithoutExtension(text);
				}
				else
				{
					AssetDatabase.CreateAsset(preset, text);
				}
			}
		}
	}
}
