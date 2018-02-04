using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEditor.Experimental.AssetImporters;
using UnityEditor.VersionControl;
using UnityEditorInternal;
using UnityEditorInternal.VersionControl;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Profiling;

namespace UnityEditor
{
	[EditorWindowTitle(title = "Inspector", useTypeNameAsIconName = true)]
	public class InspectorWindow : EditorWindow, IHasCustomMenu, ISerializationCallbackReceiver
	{
		internal class Styles
		{
			public readonly GUIStyle preToolbar = "preToolbar";

			public readonly GUIStyle preToolbar2 = "preToolbar2";

			public readonly GUIStyle preDropDown = "preDropDown";

			public readonly GUIStyle dragHandle = "RL DragHandle";

			public readonly GUIStyle lockButton = "IN LockButton";

			public readonly GUIStyle insertionMarker = "InsertionMarker";

			public readonly GUIContent preTitle = EditorGUIUtility.TrTextContent("Preview", null, null);

			public readonly GUIContent labelTitle = EditorGUIUtility.TrTextContent("Asset Labels", null, null);

			public readonly GUIContent addComponentLabel = EditorGUIUtility.TrTextContent("Add Component", null, null);

			public GUIStyle preBackground = "preBackground";

			public GUIStyle addComponentArea = EditorStyles.inspectorTitlebar;

			public GUIStyle addComponentButtonStyle = "AC Button";

			public GUIStyle previewMiniLabel = new GUIStyle(EditorStyles.whiteMiniLabel);

			public GUIStyle typeSelection = new GUIStyle("PR Label");

			public GUIStyle lockedHeaderButton = "preButton";

			public GUIStyle stickyNote = new GUIStyle("VCS_StickyNote");

			public GUIStyle stickyNoteArrow = new GUIStyle("VCS_StickyNoteArrow");

			public GUIStyle stickyNotePerforce = new GUIStyle("VCS_StickyNoteP4");

			public GUIStyle stickyNoteLabel = new GUIStyle("VCS_StickyNoteLabel");

			public readonly GUIContent VCS_NotConnectedMessage = EditorGUIUtility.TrTextContent("VCS Plugin {0} is enabled but not connected", null, null);

			public Styles()
			{
				this.typeSelection.padding.left = 12;
			}
		}

		internal Vector2 m_ScrollPosition;

		internal InspectorMode m_InspectorMode = InspectorMode.Normal;

		private static readonly List<InspectorWindow> m_AllInspectors = new List<InspectorWindow>();

		private static bool s_AllOptimizedGUIBlocksNeedsRebuild;

		private const float kBottomToolbarHeight = 17f;

		internal const int kInspectorPaddingLeft = 14;

		internal const int kInspectorPaddingRight = 4;

		private const long delayRepaintWhilePlayingAnimation = 150L;

		private long s_LastUpdateWhilePlayingAnimation = 0L;

		private bool m_ResetKeyboardControl;

		protected ActiveEditorTracker m_Tracker;

		[SerializeField]
		protected List<UnityEngine.Object> m_ObjectsLockedBeforeSerialization = new List<UnityEngine.Object>();

		[SerializeField]
		protected List<int> m_InstanceIDsLockedBeforeSerialization = new List<int>();

		[SerializeField]
		private EditorGUIUtility.EditorLockTrackerWithActiveEditorTracker m_LockTracker = new EditorGUIUtility.EditorLockTrackerWithActiveEditorTracker();

		private Editor m_LastInteractedEditor;

		private bool m_IsOpenForEdit = false;

		private static InspectorWindow.Styles s_Styles;

		[SerializeField]
		private PreviewResizer m_PreviewResizer = new PreviewResizer();

		[SerializeField]
		private PreviewWindow m_PreviewWindow;

		private LabelGUI m_LabelGUI = new LabelGUI();

		private AssetBundleNameGUI m_AssetBundleNameGUI = new AssetBundleNameGUI();

		private TypeSelectionList m_TypeSelectionList = null;

		private double m_lastRenderedTime;

		private bool m_InvalidateGUIBlockCache = true;

		private List<IPreviewable> m_Previews;

		private IPreviewable m_SelectedPreview;

		private EditorDragging editorDragging;

		public static Action<Editor> OnPostHeaderGUI = null;

		internal static InspectorWindow s_CurrentInspectorWindow;

		[CompilerGenerated]
		private static Func<UnityEngine.Object, bool> <>f__mg$cache0;

		internal static InspectorWindow.Styles styles
		{
			get
			{
				InspectorWindow.Styles arg_18_0;
				if ((arg_18_0 = InspectorWindow.s_Styles) == null)
				{
					arg_18_0 = (InspectorWindow.s_Styles = new InspectorWindow.Styles());
				}
				return arg_18_0;
			}
		}

		public bool isLocked
		{
			get
			{
				this.m_LockTracker.tracker = this.tracker;
				return this.m_LockTracker.isLocked;
			}
			set
			{
				this.m_LockTracker.tracker = this.tracker;
				this.m_LockTracker.isLocked = value;
			}
		}

		internal ActiveEditorTracker tracker
		{
			get
			{
				this.CreateTracker();
				return this.m_Tracker;
			}
		}

		internal InspectorWindow()
		{
			this.editorDragging = new EditorDragging(this);
		}

		private void Awake()
		{
			if (!InspectorWindow.m_AllInspectors.Contains(this))
			{
				InspectorWindow.m_AllInspectors.Add(this);
			}
		}

		private void OnDestroy()
		{
			if (this.m_PreviewWindow != null)
			{
				this.m_PreviewWindow.Close();
			}
			if (this.m_Tracker != null && !this.m_Tracker.Equals(ActiveEditorTracker.sharedTracker))
			{
				this.m_Tracker.Destroy();
			}
		}

		protected virtual void OnEnable()
		{
			this.RefreshTitle();
			base.minSize = new Vector2(275f, 50f);
			if (!InspectorWindow.m_AllInspectors.Contains(this))
			{
				InspectorWindow.m_AllInspectors.Add(this);
			}
			this.m_PreviewResizer.Init("InspectorPreview");
			this.m_LabelGUI.OnEnable();
			this.CreateTracker();
			this.RestoreLockStateFromSerializedData();
			if (this.m_LockTracker == null)
			{
				this.m_LockTracker = new EditorGUIUtility.EditorLockTrackerWithActiveEditorTracker();
			}
			this.m_LockTracker.tracker = this.tracker;
			this.m_LockTracker.lockStateChanged.AddListener(new UnityAction<bool>(this.LockStateChanged));
			EditorApplication.projectWasLoaded = (UnityAction)Delegate.Combine(EditorApplication.projectWasLoaded, new UnityAction(this.OnProjectWasLoaded));
		}

		private void OnProjectWasLoaded()
		{
			if (this.m_InstanceIDsLockedBeforeSerialization.Count > 0)
			{
				for (int i = this.m_InstanceIDsLockedBeforeSerialization.Count - 1; i >= 0; i--)
				{
					for (int j = this.m_ObjectsLockedBeforeSerialization.Count - 1; j >= 0; j--)
					{
						if (this.m_ObjectsLockedBeforeSerialization[j] == null || this.m_ObjectsLockedBeforeSerialization[j].GetInstanceID() == this.m_InstanceIDsLockedBeforeSerialization[i])
						{
							this.m_ObjectsLockedBeforeSerialization.RemoveAt(j);
							break;
						}
					}
				}
				this.m_InstanceIDsLockedBeforeSerialization.Clear();
				this.RestoreLockStateFromSerializedData();
			}
		}

		protected virtual void OnDisable()
		{
			InspectorWindow.m_AllInspectors.Remove(this);
			if (this.m_LockTracker != null)
			{
				this.m_LockTracker.lockStateChanged.RemoveListener(new UnityAction<bool>(this.LockStateChanged));
			}
			EditorApplication.projectWasLoaded = (UnityAction)Delegate.Remove(EditorApplication.projectWasLoaded, new UnityAction(this.OnProjectWasLoaded));
		}

		private void OnLostFocus()
		{
			this.m_LabelGUI.OnLostFocus();
		}

		internal static void RepaintAllInspectors()
		{
			foreach (InspectorWindow current in InspectorWindow.m_AllInspectors)
			{
				current.Repaint();
			}
		}

		internal static List<InspectorWindow> GetInspectors()
		{
			return InspectorWindow.m_AllInspectors;
		}

		private void OnSelectionChange()
		{
			this.m_Previews = null;
			this.m_SelectedPreview = null;
			this.m_TypeSelectionList = null;
			this.m_Parent.ClearKeyboardControl();
			ScriptAttributeUtility.ClearGlobalCache();
			base.Repaint();
		}

		internal static InspectorWindow[] GetAllInspectorWindows()
		{
			return InspectorWindow.m_AllInspectors.ToArray();
		}

		private void OnInspectorUpdate()
		{
			this.tracker.VerifyModifiedMonoBehaviours();
			if (this.tracker.isDirty && this.ReadyToRepaint())
			{
				base.Repaint();
			}
		}

		public virtual void AddItemsToMenu(GenericMenu menu)
		{
			menu.AddItem(EditorGUIUtility.TrTextContent("Normal", null, null), this.m_InspectorMode == InspectorMode.Normal, new GenericMenu.MenuFunction(this.SetNormal));
			menu.AddItem(EditorGUIUtility.TrTextContent("Debug", null, null), this.m_InspectorMode == InspectorMode.Debug, new GenericMenu.MenuFunction(this.SetDebug));
			if (Unsupported.IsDeveloperMode())
			{
				menu.AddItem(EditorGUIUtility.TrTextContent("Debug-Internal", null, null), this.m_InspectorMode == InspectorMode.DebugInternal, new GenericMenu.MenuFunction(this.SetDebugInternal));
			}
			menu.AddSeparator(string.Empty);
			this.m_LockTracker.AddItemsToMenu(menu, false);
		}

		private void RefreshTitle()
		{
			string iconName = "UnityEditor.InspectorWindow";
			if (this.m_InspectorMode == InspectorMode.Normal)
			{
				base.titleContent = EditorGUIUtility.TrTextContentWithIcon("Inspector", iconName);
			}
			else
			{
				base.titleContent = EditorGUIUtility.TrTextContentWithIcon("Debug", iconName);
			}
		}

		private void SetMode(InspectorMode mode)
		{
			this.m_InspectorMode = mode;
			this.RefreshTitle();
			this.tracker.inspectorMode = mode;
			this.m_ResetKeyboardControl = true;
		}

		private void SetDebug()
		{
			this.SetMode(InspectorMode.Debug);
		}

		private void SetNormal()
		{
			this.SetMode(InspectorMode.Normal);
		}

		private void SetDebugInternal()
		{
			this.SetMode(InspectorMode.DebugInternal);
		}

		private void FlipLocked()
		{
			this.isLocked = !this.isLocked;
		}

		private static void DoInspectorDragAndDrop(Rect rect, UnityEngine.Object[] targets)
		{
			if (InspectorWindow.Dragging(rect))
			{
				DragAndDrop.visualMode = InternalEditorUtility.InspectorWindowDrag(targets, Event.current.type == EventType.DragPerform);
				if (Event.current.type == EventType.DragPerform)
				{
					DragAndDrop.AcceptDrag();
				}
			}
		}

		private static bool Dragging(Rect rect)
		{
			return (Event.current.type == EventType.DragUpdated || Event.current.type == EventType.DragPerform) && rect.Contains(Event.current.mousePosition);
		}

		protected virtual void CreateTracker()
		{
			if (this.m_Tracker != null)
			{
				this.m_Tracker.inspectorMode = this.m_InspectorMode;
			}
			else
			{
				ActiveEditorTracker sharedTracker = ActiveEditorTracker.sharedTracker;
				bool flag = InspectorWindow.m_AllInspectors.Any((InspectorWindow i) => i.m_Tracker != null && i.m_Tracker.Equals(sharedTracker));
				this.m_Tracker = ((!flag) ? ActiveEditorTracker.sharedTracker : new ActiveEditorTracker());
				this.m_Tracker.inspectorMode = this.m_InspectorMode;
				this.m_Tracker.RebuildIfNecessary();
			}
		}

		protected virtual void CreatePreviewables()
		{
			if (this.m_Previews == null)
			{
				this.m_Previews = new List<IPreviewable>();
				if (this.tracker.activeEditors.Length != 0)
				{
					Editor[] activeEditors = this.tracker.activeEditors;
					for (int i = 0; i < activeEditors.Length; i++)
					{
						Editor editor = activeEditors[i];
						IEnumerable<IPreviewable> previewsForType = this.GetPreviewsForType(editor);
						foreach (IPreviewable current in previewsForType)
						{
							this.m_Previews.Add(current);
						}
					}
				}
			}
		}

		private IEnumerable<IPreviewable> GetPreviewsForType(Editor editor)
		{
			List<IPreviewable> list = new List<IPreviewable>();
			foreach (Type current in EditorAssemblies.GetAllTypesWithInterface<IPreviewable>())
			{
				if (!typeof(Editor).IsAssignableFrom(current))
				{
					object[] customAttributes = current.GetCustomAttributes(typeof(CustomPreviewAttribute), false);
					object[] array = customAttributes;
					for (int i = 0; i < array.Length; i++)
					{
						object obj = array[i];
						CustomPreviewAttribute customPreviewAttribute = (CustomPreviewAttribute)obj;
						if (!(editor.target == null) && customPreviewAttribute.m_Type == editor.target.GetType())
						{
							IPreviewable previewable = Activator.CreateInstance(current) as IPreviewable;
							previewable.Initialize(editor.targets);
							list.Add(previewable);
						}
					}
				}
			}
			return list;
		}

		protected virtual void ShowButton(Rect r)
		{
			this.m_LockTracker.ShowButton(r, InspectorWindow.styles.lockButton, false);
		}

		private void LockStateChanged(bool lockeState)
		{
			this.tracker.RebuildIfNecessary();
		}

		protected virtual void OnGUI()
		{
			Profiler.BeginSample("InspectorWindow.OnGUI");
			this.CreatePreviewables();
			InspectorWindow.FlushAllOptimizedGUIBlocksIfNeeded();
			this.ResetKeyboardControl();
			this.m_ScrollPosition = EditorGUILayout.BeginVerticalScrollView(this.m_ScrollPosition, new GUILayoutOption[0]);
			if (Event.current.type == EventType.Repaint)
			{
				this.tracker.ClearDirty();
			}
			InspectorWindow.s_CurrentInspectorWindow = this;
			Editor[] activeEditors = this.tracker.activeEditors;
			this.AssignAssetEditor(activeEditors);
			Profiler.BeginSample("InspectorWindow.DrawEditors()");
			this.DrawEditors(activeEditors);
			Profiler.EndSample();
			if (this.tracker.hasComponentsWhichCannotBeMultiEdited)
			{
				if (activeEditors.Length == 0 && !this.tracker.isLocked && Selection.objects.Length > 0)
				{
					this.DrawSelectionPickerList();
				}
				else
				{
					Rect rect = GUILayoutUtility.GetRect(10f, 4f, EditorStyles.inspectorTitlebar);
					if (Event.current.type == EventType.Repaint)
					{
						this.DrawSplitLine(rect.y);
					}
					GUILayout.Label("Components that are only on some of the selected objects cannot be multi-edited.", EditorStyles.helpBox, new GUILayoutOption[0]);
					GUILayout.Space(4f);
				}
			}
			InspectorWindow.s_CurrentInspectorWindow = null;
			EditorGUI.indentLevel = 0;
			this.AddComponentButton(this.tracker.activeEditors);
			GUI.enabled = true;
			this.CheckDragAndDrop(this.tracker.activeEditors);
			this.MoveFocusOnKeyPress();
			EditorGUILayout.EndScrollView();
			Profiler.BeginSample("InspectorWindow.DrawPreviewAndLabels");
			this.DrawPreviewAndLabels();
			Profiler.EndSample();
			if (this.tracker.activeEditors.Length > 0)
			{
				this.DrawVCSShortInfo();
			}
			Profiler.EndSample();
		}

		internal virtual Editor GetLastInteractedEditor()
		{
			return this.m_LastInteractedEditor;
		}

		internal IPreviewable GetEditorThatControlsPreview(IPreviewable[] editors)
		{
			IPreviewable result;
			if (editors.Length == 0)
			{
				result = null;
			}
			else if (this.m_SelectedPreview != null)
			{
				result = this.m_SelectedPreview;
			}
			else
			{
				IPreviewable lastInteractedEditor = this.GetLastInteractedEditor();
				Type type = (lastInteractedEditor == null) ? null : lastInteractedEditor.GetType();
				IPreviewable previewable = null;
				IPreviewable previewable2 = null;
				for (int i = 0; i < editors.Length; i++)
				{
					IPreviewable previewable3 = editors[i];
					if (previewable3 != null && !(previewable3.target == null))
					{
						if (!EditorUtility.IsPersistent(previewable3.target) || !(AssetDatabase.GetAssetPath(previewable3.target) != AssetDatabase.GetAssetPath(editors[0].target)))
						{
							if (!(editors[0] is AssetImporterEditor) || previewable3 is AssetImporterEditor)
							{
								if (previewable3.HasPreviewGUI())
								{
									if (previewable3 == lastInteractedEditor)
									{
										result = previewable3;
										return result;
									}
									if (previewable2 == null && previewable3.GetType() == type)
									{
										previewable2 = previewable3;
									}
									if (previewable == null)
									{
										previewable = previewable3;
									}
								}
							}
						}
					}
				}
				if (previewable2 != null)
				{
					result = previewable2;
				}
				else if (previewable != null)
				{
					result = previewable;
				}
				else
				{
					result = null;
				}
			}
			return result;
		}

		internal IPreviewable[] GetEditorsWithPreviews(Editor[] editors)
		{
			IList<IPreviewable> list = new List<IPreviewable>();
			int num = -1;
			for (int i = 0; i < editors.Length; i++)
			{
				Editor editor = editors[i];
				num++;
				if (!(editor.target == null))
				{
					if (!EditorUtility.IsPersistent(editor.target) || !(AssetDatabase.GetAssetPath(editor.target) != AssetDatabase.GetAssetPath(editors[0].target)))
					{
						if (EditorUtility.IsPersistent(editors[0].target) || !EditorUtility.IsPersistent(editor.target))
						{
							if (!this.ShouldCullEditor(editors, num))
							{
								if (!(editors[0] is AssetImporterEditor) || editor is AssetImporterEditor)
								{
									if (editor.HasPreviewGUI())
									{
										list.Add(editor);
									}
								}
							}
						}
					}
				}
			}
			foreach (IPreviewable current in this.m_Previews)
			{
				if (current.HasPreviewGUI())
				{
					list.Add(current);
				}
			}
			return list.ToArray<IPreviewable>();
		}

		internal UnityEngine.Object GetInspectedObject()
		{
			Editor firstNonImportInspectorEditor = this.GetFirstNonImportInspectorEditor(this.tracker.activeEditors);
			UnityEngine.Object result;
			if (firstNonImportInspectorEditor == null)
			{
				result = null;
			}
			else
			{
				result = firstNonImportInspectorEditor.target;
			}
			return result;
		}

		private Editor GetFirstNonImportInspectorEditor(Editor[] editors)
		{
			Editor result;
			for (int i = 0; i < editors.Length; i++)
			{
				Editor editor = editors[i];
				if (!(editor.target is AssetImporter))
				{
					result = editor;
					return result;
				}
			}
			result = null;
			return result;
		}

		private void MoveFocusOnKeyPress()
		{
			KeyCode keyCode = Event.current.keyCode;
			if (Event.current.type == EventType.KeyDown && (keyCode == KeyCode.DownArrow || keyCode == KeyCode.UpArrow || keyCode == KeyCode.Tab))
			{
				if (keyCode != KeyCode.Tab)
				{
					EditorGUIUtility.MoveFocusAndScroll(keyCode == KeyCode.DownArrow);
				}
				else
				{
					EditorGUIUtility.ScrollForTabbing(!Event.current.shift);
				}
				Event.current.Use();
			}
		}

		private void ResetKeyboardControl()
		{
			if (this.m_ResetKeyboardControl)
			{
				GUIUtility.keyboardControl = 0;
				this.m_ResetKeyboardControl = false;
			}
		}

		private void CheckDragAndDrop(Editor[] editors)
		{
			Rect rect = GUILayoutUtility.GetRect(GUIContent.none, GUIStyle.none, new GUILayoutOption[]
			{
				GUILayout.ExpandHeight(true)
			});
			if (rect.Contains(Event.current.mousePosition))
			{
				Editor firstNonImportInspectorEditor = this.GetFirstNonImportInspectorEditor(editors);
				if (firstNonImportInspectorEditor != null)
				{
					InspectorWindow.DoInspectorDragAndDrop(rect, firstNonImportInspectorEditor.targets);
				}
				if (Event.current.type == EventType.MouseDown)
				{
					GUIUtility.keyboardControl = 0;
					Event.current.Use();
				}
			}
			this.editorDragging.HandleDraggingToBottomArea(rect, this.m_Tracker);
		}

		private static bool HasLabel(UnityEngine.Object target)
		{
			return InspectorWindow.HasLabel(target, AssetDatabase.GetAssetPath(target));
		}

		private static bool HasLabel(UnityEngine.Object target, string assetPath)
		{
			return EditorUtility.IsPersistent(target) && assetPath.StartsWith("assets", StringComparison.OrdinalIgnoreCase);
		}

		private UnityEngine.Object[] GetInspectedAssets()
		{
			Editor firstNonImportInspectorEditor = this.GetFirstNonImportInspectorEditor(this.tracker.activeEditors);
			UnityEngine.Object[] result;
			if (firstNonImportInspectorEditor != null && firstNonImportInspectorEditor.targets.Length == 1)
			{
				string assetPath = AssetDatabase.GetAssetPath(firstNonImportInspectorEditor.target);
				if (InspectorWindow.HasLabel(firstNonImportInspectorEditor.target, assetPath) && !Directory.Exists(assetPath))
				{
					result = firstNonImportInspectorEditor.targets;
					return result;
				}
			}
			IEnumerable<UnityEngine.Object> arg_85_0 = Selection.objects;
			if (InspectorWindow.<>f__mg$cache0 == null)
			{
				InspectorWindow.<>f__mg$cache0 = new Func<UnityEngine.Object, bool>(InspectorWindow.HasLabel);
			}
			result = arg_85_0.Where(InspectorWindow.<>f__mg$cache0).ToArray<UnityEngine.Object>();
			return result;
		}

		private void DrawPreviewAndLabels()
		{
			if (this.m_PreviewWindow && Event.current.type == EventType.Repaint)
			{
				this.m_PreviewWindow.Repaint();
			}
			IPreviewable[] editorsWithPreviews = this.GetEditorsWithPreviews(this.tracker.activeEditors);
			IPreviewable editorThatControlsPreview = this.GetEditorThatControlsPreview(editorsWithPreviews);
			bool flag = editorThatControlsPreview != null && editorThatControlsPreview.HasPreviewGUI() && this.m_PreviewWindow == null;
			UnityEngine.Object[] inspectedAssets = this.GetInspectedAssets();
			bool flag2 = inspectedAssets.Length > 0;
			bool flag3 = inspectedAssets.Any((UnityEngine.Object a) => !(a is MonoScript) && AssetDatabase.IsMainAsset(a));
			if (flag || flag2)
			{
				Event current = Event.current;
				Rect position = EditorGUILayout.BeginHorizontal(GUIContent.none, InspectorWindow.styles.preToolbar, new GUILayoutOption[]
				{
					GUILayout.Height(17f)
				});
				Rect position2 = default(Rect);
				GUILayout.FlexibleSpace();
				Rect lastRect = GUILayoutUtility.GetLastRect();
				GUIContent content;
				if (flag)
				{
					GUIContent previewTitle = editorThatControlsPreview.GetPreviewTitle();
					content = (previewTitle ?? InspectorWindow.styles.preTitle);
				}
				else
				{
					content = InspectorWindow.styles.labelTitle;
				}
				position2.x = lastRect.x + 3f;
				position2.y = lastRect.y + (17f - InspectorWindow.s_Styles.dragHandle.fixedHeight) / 2f + 1f;
				position2.width = lastRect.width - 6f;
				position2.height = InspectorWindow.s_Styles.dragHandle.fixedHeight;
				if (editorsWithPreviews.Length > 1)
				{
					Vector2 vector = InspectorWindow.styles.preDropDown.CalcSize(content);
					float a3 = position2.xMax - lastRect.xMin - 3f - 20f;
					float num = Mathf.Min(a3, vector.x);
					Rect position3 = new Rect(lastRect.x, lastRect.y, num, vector.y);
					lastRect.xMin += num;
					position2.xMin += num;
					GUIContent[] array = new GUIContent[editorsWithPreviews.Length];
					int selected = -1;
					for (int i = 0; i < editorsWithPreviews.Length; i++)
					{
						IPreviewable previewable = editorsWithPreviews[i];
						GUIContent gUIContent = previewable.GetPreviewTitle() ?? InspectorWindow.styles.preTitle;
						string text;
						if (gUIContent == InspectorWindow.styles.preTitle)
						{
							string str = ObjectNames.GetTypeName(previewable.target);
							if (previewable.target is MonoBehaviour)
							{
								str = MonoScript.FromMonoBehaviour(previewable.target as MonoBehaviour).GetClass().Name;
							}
							text = gUIContent.text + " - " + str;
						}
						else
						{
							text = gUIContent.text;
						}
						array[i] = new GUIContent(text);
						if (editorsWithPreviews[i] == editorThatControlsPreview)
						{
							selected = i;
						}
					}
					if (GUI.Button(position3, content, InspectorWindow.styles.preDropDown))
					{
						EditorUtility.DisplayCustomMenu(position3, array, selected, new EditorUtility.SelectMenuItemFunction(this.OnPreviewSelected), editorsWithPreviews);
					}
				}
				else
				{
					float a2 = position2.xMax - lastRect.xMin - 3f - 20f;
					float width = Mathf.Min(a2, InspectorWindow.styles.preToolbar2.CalcSize(content).x);
					Rect position4 = new Rect(lastRect.x, lastRect.y, width, lastRect.height);
					position2.xMin = position4.xMax + 3f;
					GUI.Label(position4, content, InspectorWindow.styles.preToolbar2);
				}
				if (flag && Event.current.type == EventType.Repaint)
				{
					InspectorWindow.s_Styles.dragHandle.Draw(position2, GUIContent.none, false, false, false, false);
				}
				if (flag && this.m_PreviewResizer.GetExpandedBeforeDragging())
				{
					editorThatControlsPreview.OnPreviewSettings();
				}
				EditorGUILayout.EndHorizontal();
				if (current.type == EventType.MouseUp && current.button == 1 && position.Contains(current.mousePosition) && this.m_PreviewWindow == null)
				{
					this.DetachPreview();
				}
				float height;
				if (flag)
				{
					Rect position5 = base.position;
					if (EditorSettings.externalVersionControl != ExternalVersionControl.Disabled && EditorSettings.externalVersionControl != ExternalVersionControl.AutoDetect && EditorSettings.externalVersionControl != ExternalVersionControl.Generic)
					{
						position5.height -= 17f;
					}
					height = this.m_PreviewResizer.ResizeHandle(position5, 100f, 100f, 17f, lastRect);
				}
				else
				{
					if (GUI.Button(position, GUIContent.none, GUIStyle.none))
					{
						this.m_PreviewResizer.ToggleExpanded();
					}
					height = 0f;
				}
				if (this.m_PreviewResizer.GetExpanded())
				{
					GUILayout.BeginVertical(InspectorWindow.styles.preBackground, new GUILayoutOption[]
					{
						GUILayout.Height(height)
					});
					if (flag)
					{
						editorThatControlsPreview.DrawPreview(GUILayoutUtility.GetRect(0f, 10240f, 64f, 10240f));
					}
					if (flag2)
					{
						using (new EditorGUI.DisabledScope(inspectedAssets.Any((UnityEngine.Object a) => EditorUtility.IsPersistent(a) && !Editor.IsAppropriateFileOpenForEdit(a))))
						{
							this.m_LabelGUI.OnLabelGUI(inspectedAssets);
						}
					}
					if (flag3)
					{
						this.m_AssetBundleNameGUI.OnAssetBundleNameGUI(inspectedAssets);
					}
					GUILayout.EndVertical();
				}
			}
		}

		internal UnityEngine.Object[] GetTargetsForPreview(IPreviewable previewEditor)
		{
			Editor editor = null;
			Editor[] activeEditors = this.tracker.activeEditors;
			for (int i = 0; i < activeEditors.Length; i++)
			{
				Editor editor2 = activeEditors[i];
				if (editor2.target.GetType() == previewEditor.target.GetType())
				{
					editor = editor2;
					break;
				}
			}
			return editor.targets;
		}

		private void OnPreviewSelected(object userData, string[] options, int selected)
		{
			IPreviewable[] array = userData as IPreviewable[];
			this.m_SelectedPreview = array[selected];
		}

		private void DetachPreview()
		{
			Event.current.Use();
			this.m_PreviewWindow = (ScriptableObject.CreateInstance(typeof(PreviewWindow)) as PreviewWindow);
			this.m_PreviewWindow.SetParentInspector(this);
			this.m_PreviewWindow.Show();
			base.Repaint();
			GUIUtility.ExitGUI();
		}

		protected virtual void DrawVCSSticky(float offset)
		{
			string text = "";
			Editor firstNonImportInspectorEditor = this.GetFirstNonImportInspectorEditor(this.tracker.activeEditors);
			if (!EditorPrefs.GetBool("vcssticky") && !Editor.IsAppropriateFileOpenForEdit(firstNonImportInspectorEditor.target, out text))
			{
				Rect position = new Rect(10f, base.position.height - 94f, base.position.width - 20f, 80f);
				position.y -= offset;
				if (Event.current.type == EventType.Repaint)
				{
					InspectorWindow.styles.stickyNote.Draw(position, false, false, false, false);
					Rect position2 = new Rect(position.x, position.y + position.height / 2f - 32f, 64f, 64f);
					if (EditorSettings.externalVersionControl == "Perforce")
					{
						InspectorWindow.styles.stickyNotePerforce.Draw(position2, false, false, false, false);
					}
					Rect position3 = new Rect(position.x + position2.width, position.y, position.width - position2.width, position.height);
					GUI.Label(position3, EditorGUIUtility.TrTextContent("<b>Under Version Control</b>\nCheck out this asset in order to make changes.", null, null), InspectorWindow.styles.stickyNoteLabel);
					Rect position4 = new Rect(position.x + position.width / 2f, position.y + 80f, 19f, 14f);
					InspectorWindow.styles.stickyNoteArrow.Draw(position4, false, false, false, false);
				}
			}
		}

		private void DrawVCSShortInfo()
		{
			if (Provider.enabled && EditorSettings.externalVersionControl != ExternalVersionControl.Disabled && EditorSettings.externalVersionControl != ExternalVersionControl.AutoDetect && EditorSettings.externalVersionControl != ExternalVersionControl.Generic)
			{
				Editor firstNonImportInspectorEditor = this.GetFirstNonImportInspectorEditor(this.tracker.activeEditors);
				string assetPath = AssetDatabase.GetAssetPath(firstNonImportInspectorEditor.target);
				Asset assetByPath = Provider.GetAssetByPath(assetPath);
				if (assetByPath != null && (assetByPath.path.StartsWith("Assets") || assetByPath.path.StartsWith("ProjectSettings")))
				{
					Asset assetByPath2 = Provider.GetAssetByPath(assetPath.Trim(new char[]
					{
						'/'
					}) + ".meta");
					string text = assetByPath.StateToString();
					string text2 = (assetByPath2 != null) ? assetByPath2.StateToString() : string.Empty;
					if (text == string.Empty && Provider.onlineState != OnlineState.Online)
					{
						text = string.Format(InspectorWindow.s_Styles.VCS_NotConnectedMessage.text, Provider.GetActivePlugin().name);
					}
					bool flag = assetByPath2 != null && (assetByPath2.state & ~Asset.States.MetaFile) != assetByPath.state;
					bool flag2 = text != "";
					float height = (!flag || !flag2) ? 17f : 34f;
					GUILayout.Label(GUIContent.none, InspectorWindow.styles.preToolbar, new GUILayoutOption[]
					{
						GUILayout.Height(height)
					});
					Rect lastRect = GUILayoutUtility.GetLastRect();
					bool flag3 = Event.current.type == EventType.Layout || Event.current.type == EventType.Repaint;
					if (flag2 && flag3)
					{
						Texture2D icon = AssetDatabase.GetCachedIcon(assetPath) as Texture2D;
						if (flag)
						{
							Rect rect = lastRect;
							rect.height = 17f;
							this.DrawVCSShortInfoAsset(assetByPath, this.BuildTooltip(assetByPath, null), rect, icon, text);
							Texture2D iconForFile = InternalEditorUtility.GetIconForFile(assetByPath2.path);
							rect.y += 17f;
							this.DrawVCSShortInfoAsset(assetByPath2, this.BuildTooltip(null, assetByPath2), rect, iconForFile, text2);
						}
						else
						{
							this.DrawVCSShortInfoAsset(assetByPath, this.BuildTooltip(assetByPath, assetByPath2), lastRect, icon, text);
						}
					}
					else if (text2 != "" && flag3)
					{
						Texture2D iconForFile2 = InternalEditorUtility.GetIconForFile(assetByPath2.path);
						this.DrawVCSShortInfoAsset(assetByPath2, this.BuildTooltip(assetByPath, assetByPath2), lastRect, iconForFile2, text2);
					}
					string text3 = "";
					if (!Editor.IsAppropriateFileOpenForEdit(firstNonImportInspectorEditor.target, out text3))
					{
						if (Provider.isActive)
						{
							float num = 80f;
							Rect position = new Rect(lastRect.x + lastRect.width - num, lastRect.y, num, lastRect.height);
							if (GUI.Button(position, "Check out", InspectorWindow.styles.lockedHeaderButton))
							{
								EditorPrefs.SetBool("vcssticky", true);
								Task task = Provider.Checkout(firstNonImportInspectorEditor.targets, CheckoutMode.Both);
								task.Wait();
								base.Repaint();
							}
						}
						this.DrawVCSSticky(lastRect.height / 2f);
					}
				}
			}
		}

		protected string BuildTooltip(Asset asset, Asset metaAsset)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (asset != null)
			{
				stringBuilder.AppendLine("Asset:");
				stringBuilder.AppendLine(asset.AllStateToString());
			}
			if (metaAsset != null)
			{
				stringBuilder.AppendLine("Meta file:");
				stringBuilder.AppendLine(metaAsset.AllStateToString());
			}
			return stringBuilder.ToString();
		}

		protected void DrawVCSShortInfoAsset(Asset asset, string tooltip, Rect rect, Texture2D icon, string currentState)
		{
			Rect rect2 = new Rect(rect.x, rect.y, 28f, 16f);
			Rect position = rect2;
			position.x += 6f;
			position.width = 16f;
			if (icon != null)
			{
				GUI.DrawTexture(position, icon);
			}
			Overlay.DrawOverlay(asset, rect2);
			Rect position2 = new Rect(rect.x + 26f, rect.y, rect.width - 31f, rect.height);
			GUIContent gUIContent = GUIContent.Temp(currentState);
			gUIContent.tooltip = tooltip;
			EditorGUI.LabelField(position2, gUIContent, InspectorWindow.styles.preToolbar2);
		}

		protected void AssignAssetEditor(Editor[] editors)
		{
			if (editors.Length > 1 && editors[0] is AssetImporterEditor)
			{
				(editors[0] as AssetImporterEditor).assetEditor = editors[1];
				for (int i = 0; i < editors.Length; i++)
				{
					if (editors[i].target == null)
					{
						editors[i].InternalSetTargets(editors[i].serializedObject.targetObjects);
					}
				}
			}
		}

		private void DrawEditors(Editor[] editors)
		{
			if (editors.Length != 0)
			{
				UnityEngine.Object inspectedObject = this.GetInspectedObject();
				string empty = string.Empty;
				GUILayout.Space(0f);
				if (inspectedObject is Material)
				{
					int num = 0;
					while (num <= 1 && num < editors.Length)
					{
						MaterialEditor materialEditor = editors[num] as MaterialEditor;
						if (materialEditor != null)
						{
							materialEditor.forceVisible = true;
							break;
						}
						num++;
					}
				}
				bool rebuildOptimizedGUIBlock = false;
				if (Event.current.type == EventType.Repaint)
				{
					if (inspectedObject != null && this.m_IsOpenForEdit != Editor.IsAppropriateFileOpenForEdit(inspectedObject, out empty))
					{
						this.m_IsOpenForEdit = !this.m_IsOpenForEdit;
						rebuildOptimizedGUIBlock = true;
					}
					if (this.m_InvalidateGUIBlockCache)
					{
						rebuildOptimizedGUIBlock = true;
						this.m_InvalidateGUIBlockCache = false;
					}
				}
				else if (Event.current.type == EventType.ExecuteCommand && Event.current.commandName == "EyeDropperUpdate")
				{
					rebuildOptimizedGUIBlock = true;
				}
				Editor.m_AllowMultiObjectAccess = true;
				bool flag = false;
				Rect position = default(Rect);
				for (int i = 0; i < editors.Length; i++)
				{
					if (this.ShouldCullEditor(editors, i))
					{
						if (Event.current.type == EventType.Repaint)
						{
							editors[i].isInspectorDirty = false;
						}
					}
					else
					{
						bool textFieldInput = GUIUtility.textFieldInput;
						this.DrawEditor(editors, i, rebuildOptimizedGUIBlock, ref flag, ref position);
						if (Event.current.type == EventType.Repaint && !textFieldInput && GUIUtility.textFieldInput)
						{
							InspectorWindow.FlushOptimizedGUIBlock(editors[i]);
						}
					}
				}
				EditorGUIUtility.ResetGUIState();
				if (position.height > 0f)
				{
					GUI.BeginGroup(position);
					GUI.Label(new Rect(0f, 0f, position.width, position.height), "Imported Object", "OL Title");
					GUI.EndGroup();
				}
			}
		}

		internal override void OnResized()
		{
			this.m_InvalidateGUIBlockCache = true;
		}

		private void DrawEditor(Editor[] editors, int editorIndex, bool rebuildOptimizedGUIBlock, ref bool showImportedObjectBarNext, ref Rect importedObjectBarRect)
		{
			Editor editor = editors[editorIndex];
			if (!(editor == null))
			{
				UnityEngine.Object target = editor.target;
				if (target || target.GetType() == typeof(MonoBehaviour))
				{
					GUIUtility.GetControlID(target.GetInstanceID(), FocusType.Passive);
					EditorGUIUtility.ResetGUIState();
					GUILayoutGroup topLevel = GUILayoutUtility.current.topLevel;
					int visible = this.tracker.GetVisible(editorIndex);
					bool flag;
					if (visible == -1)
					{
						flag = InternalEditorUtility.GetIsInspectorExpanded(target);
						this.tracker.SetVisible(editorIndex, (!flag) ? 0 : 1);
					}
					else
					{
						flag = (visible == 1);
					}
					rebuildOptimizedGUIBlock |= editor.isInspectorDirty;
					if (Event.current.type == EventType.Repaint)
					{
						editor.isInspectorDirty = false;
					}
					ScriptAttributeUtility.propertyHandlerCache = editor.propertyHandlerCache;
					bool flag2 = this.EditorHasLargeHeader(editorIndex, editors);
					if (flag2)
					{
						string empty = string.Empty;
						bool flag3 = editor.IsOpenForEdit(out empty);
						if (showImportedObjectBarNext)
						{
							showImportedObjectBarNext = false;
							GUILayout.Space(15f);
							importedObjectBarRect = GUILayoutUtility.GetRect(16f, 16f);
							importedObjectBarRect.height = 17f;
						}
						flag = true;
						using (new EditorGUI.DisabledScope(!flag3))
						{
							editor.DrawHeader();
						}
					}
					if (editor.target is AssetImporter)
					{
						showImportedObjectBarNext = true;
					}
					bool flag4 = false;
					if (editor is GenericInspector && CustomEditorAttributes.FindCustomEditorType(target, false) != null)
					{
						if (this.m_InspectorMode != InspectorMode.DebugInternal)
						{
							if (this.m_InspectorMode == InspectorMode.Normal)
							{
								flag4 = true;
							}
							else if (target is AssetImporter)
							{
								flag4 = true;
							}
						}
					}
					Rect dragRect = default(Rect);
					if (!flag2)
					{
						using (new EditorGUI.DisabledScope(!editor.IsEnabled()))
						{
							bool flag5 = EditorGUILayout.InspectorTitlebar(flag, editor.targets, editor.CanBeExpandedViaAFoldout());
							if (flag != flag5)
							{
								this.tracker.SetVisible(editorIndex, (!flag5) ? 0 : 1);
								InternalEditorUtility.SetIsInspectorExpanded(target, flag5);
								if (flag5)
								{
									this.m_LastInteractedEditor = editor;
								}
								else if (this.m_LastInteractedEditor == editor)
								{
									this.m_LastInteractedEditor = null;
								}
							}
						}
						dragRect = GUILayoutUtility.GetLastRect();
					}
					if (flag4 && flag)
					{
						GUILayout.Label("Multi-object editing not supported.", EditorStyles.helpBox, new GUILayoutOption[0]);
					}
					else
					{
						this.DisplayDeprecationMessageIfNecessary(editor);
						if (InspectorWindow.OnPostHeaderGUI != null)
						{
							InspectorWindow.OnPostHeaderGUI(editor);
						}
						EditorGUIUtility.ResetGUIState();
						Rect rect = default(Rect);
						bool flag6 = ModuleMetadata.GetModuleIncludeSettingForObject(target) == ModuleIncludeSetting.ForceExclude;
						if (flag6)
						{
							EditorGUILayout.HelpBox("The module which implements this component type has been force excluded in player settings. This object will be removed in play mode and from any builds you make.", MessageType.Warning);
						}
						using (new EditorGUI.DisabledScope(!editor.IsEnabled() || flag6))
						{
							GenericInspector genericInspector = editor as GenericInspector;
							if (genericInspector)
							{
								genericInspector.m_InspectorMode = this.m_InspectorMode;
							}
							EditorGUIUtility.hierarchyMode = true;
							EditorGUIUtility.wideMode = (base.position.width > 330f);
							ScriptAttributeUtility.propertyHandlerCache = editor.propertyHandlerCache;
							OptimizedGUIBlock optimizedGUIBlock;
							float num;
							if (editor.GetOptimizedGUIBlock(rebuildOptimizedGUIBlock, flag, out optimizedGUIBlock, out num))
							{
								rect = GUILayoutUtility.GetRect(0f, (!flag) ? 0f : num);
								this.HandleLastInteractedEditor(rect, editor);
								if (Event.current.type == EventType.Layout)
								{
									return;
								}
								if (optimizedGUIBlock.Begin(rebuildOptimizedGUIBlock, rect))
								{
									if (flag)
									{
										GUI.changed = false;
										editor.OnOptimizedInspectorGUI(rect);
									}
								}
								optimizedGUIBlock.End();
							}
							else
							{
								if (flag)
								{
									GUIStyle style = (!editor.UseDefaultMargins()) ? GUIStyle.none : EditorStyles.inspectorDefaultMargins;
									rect = EditorGUILayout.BeginVertical(style, new GUILayoutOption[0]);
									this.HandleLastInteractedEditor(rect, editor);
									GUI.changed = false;
									try
									{
										editor.OnInspectorGUI();
									}
									catch (Exception exception)
									{
										if (GUIUtility.ShouldRethrowException(exception))
										{
											throw;
										}
										Debug.LogException(exception);
									}
									EditorGUILayout.EndVertical();
								}
								if (Event.current.type == EventType.Used)
								{
									return;
								}
							}
						}
						this.editorDragging.HandleDraggingToEditor(editorIndex, dragRect, rect, this.m_Tracker);
						if (GUILayoutUtility.current.topLevel != topLevel)
						{
							if (!GUILayoutUtility.current.layoutGroups.Contains(topLevel))
							{
								Debug.LogError("Expected top level layout group missing! Too many GUILayout.EndScrollView/EndVertical/EndHorizontal?");
								GUIUtility.ExitGUI();
							}
							else
							{
								Debug.LogWarning("Unexpected top level layout group! Missing GUILayout.EndScrollView/EndVertical/EndHorizontal?");
								while (GUILayoutUtility.current.topLevel != topLevel)
								{
									GUILayoutUtility.EndLayoutGroup();
								}
							}
						}
						this.HandleComponentScreenshot(rect, editor);
					}
				}
			}
		}

		internal void RepaintImmediately(bool rebuildOptimizedGUIBlocks)
		{
			this.m_InvalidateGUIBlockCache = rebuildOptimizedGUIBlocks;
			base.RepaintImmediately();
		}

		internal bool EditorHasLargeHeader(int editorIndex, Editor[] trackerActiveEditors)
		{
			UnityEngine.Object target = trackerActiveEditors[editorIndex].target;
			return AssetDatabase.IsMainAsset(target) || AssetDatabase.IsSubAsset(target) || editorIndex == 0 || target is Material;
		}

		private void DisplayDeprecationMessageIfNecessary(Editor editor)
		{
			if (editor && editor.target)
			{
				ObsoleteAttribute obsoleteAttribute = (ObsoleteAttribute)Attribute.GetCustomAttribute(editor.target.GetType(), typeof(ObsoleteAttribute));
				if (obsoleteAttribute != null)
				{
					string message = (!string.IsNullOrEmpty(obsoleteAttribute.Message)) ? obsoleteAttribute.Message : "This component has been marked as obsolete.";
					EditorGUILayout.HelpBox(message, (!obsoleteAttribute.IsError) ? MessageType.Warning : MessageType.Error);
				}
			}
		}

		private void HandleComponentScreenshot(Rect contentRect, Editor editor)
		{
			if (ScreenShots.s_TakeComponentScreenshot)
			{
				contentRect.yMin -= 16f;
				if (contentRect.Contains(Event.current.mousePosition))
				{
					Rect contentRect2 = GUIClip.Unclip(contentRect);
					contentRect2.position += this.m_Parent.screenPosition.position;
					ScreenShots.ScreenShotComponent(contentRect2, editor.target);
				}
			}
		}

		internal bool ShouldCullEditor(Editor[] editors, int editorIndex)
		{
			bool result;
			if (editors[editorIndex].hideInspector)
			{
				result = true;
			}
			else
			{
				UnityEngine.Object target = editors[editorIndex].target;
				if (target is SubstanceImporter || target is ParticleSystemRenderer)
				{
					result = true;
				}
				else if (target != null && target.GetType() == typeof(AssetImporter))
				{
					result = true;
				}
				else
				{
					if (this.m_InspectorMode == InspectorMode.Normal && editorIndex != 0)
					{
						AssetImporterEditor assetImporterEditor = editors[0] as AssetImporterEditor;
						if (assetImporterEditor != null && !assetImporterEditor.showImportedObject)
						{
							result = true;
							return result;
						}
					}
					result = false;
				}
			}
			return result;
		}

		private void DrawSelectionPickerList()
		{
			if (this.m_TypeSelectionList == null)
			{
				this.m_TypeSelectionList = new TypeSelectionList(Selection.objects);
			}
			GUILayout.Space(0f);
			Editor.DrawHeaderGUI(null, Selection.objects.Length + " Objects");
			GUILayout.Label("Narrow the Selection:", EditorStyles.label, new GUILayoutOption[0]);
			GUILayout.Space(4f);
			Vector2 iconSize = EditorGUIUtility.GetIconSize();
			EditorGUIUtility.SetIconSize(new Vector2(16f, 16f));
			foreach (TypeSelection current in this.m_TypeSelectionList.typeSelections)
			{
				Rect rect = GUILayoutUtility.GetRect(16f, 16f, new GUILayoutOption[]
				{
					GUILayout.ExpandWidth(true)
				});
				if (GUI.Button(rect, current.label, InspectorWindow.styles.typeSelection))
				{
					Selection.objects = current.objects;
					Event.current.Use();
				}
				if (GUIUtility.hotControl == 0)
				{
					EditorGUIUtility.AddCursorRect(rect, MouseCursor.Link);
				}
				GUILayout.Space(4f);
			}
			EditorGUIUtility.SetIconSize(iconSize);
		}

		private void HandleLastInteractedEditor(Rect componentRect, Editor editor)
		{
			if (editor != this.m_LastInteractedEditor && Event.current.type == EventType.MouseDown && componentRect.Contains(Event.current.mousePosition))
			{
				this.m_LastInteractedEditor = editor;
				base.Repaint();
			}
		}

		private void AddComponentButton(Editor[] editors)
		{
			Editor firstNonImportInspectorEditor = this.GetFirstNonImportInspectorEditor(editors);
			if (firstNonImportInspectorEditor != null && firstNonImportInspectorEditor.target != null && firstNonImportInspectorEditor.target is GameObject && firstNonImportInspectorEditor.IsEnabled())
			{
				EditorGUILayout.BeginHorizontal(new GUILayoutOption[0]);
				GUILayout.FlexibleSpace();
				GUIContent addComponentLabel = InspectorWindow.s_Styles.addComponentLabel;
				Rect rect = GUILayoutUtility.GetRect(addComponentLabel, InspectorWindow.styles.addComponentButtonStyle);
				if (Event.current.type == EventType.Repaint)
				{
					this.DrawSplitLine(rect.y - 11f);
				}
				Event current = Event.current;
				bool flag = false;
				EventType type = current.type;
				if (type == EventType.ExecuteCommand)
				{
					string commandName = current.commandName;
					if (commandName == "OpenAddComponentDropdown")
					{
						flag = true;
						current.Use();
					}
				}
				if (EditorGUI.DropdownButton(rect, addComponentLabel, FocusType.Passive, InspectorWindow.styles.addComponentButtonStyle) || flag)
				{
					if (AddComponentWindow.Show(rect, (from o in firstNonImportInspectorEditor.targets
					select (GameObject)o).ToArray<GameObject>()))
					{
						GUIUtility.ExitGUI();
					}
				}
				GUILayout.FlexibleSpace();
				EditorGUILayout.EndHorizontal();
			}
		}

		private bool ReadyToRepaint()
		{
			bool result;
			if (AnimationMode.InAnimationPlaybackMode())
			{
				long num = DateTime.Now.Ticks / 10000L;
				if (num - this.s_LastUpdateWhilePlayingAnimation < 150L)
				{
					result = false;
					return result;
				}
				this.s_LastUpdateWhilePlayingAnimation = num;
			}
			result = true;
			return result;
		}

		private void DrawSplitLine(float y)
		{
			Rect position = new Rect(0f, y, this.m_Pos.width + 1f, 1f);
			Rect texCoords = new Rect(0f, 1f, 1f, 1f - 1f / (float)EditorStyles.inspectorTitlebar.normal.background.height);
			GUI.DrawTextureWithTexCoords(position, EditorStyles.inspectorTitlebar.normal.background, texCoords);
		}

		internal static void ShowWindow()
		{
			EditorWindow.GetWindow(typeof(InspectorWindow));
		}

		private static void FlushOptimizedGUI()
		{
			InspectorWindow.s_AllOptimizedGUIBlocksNeedsRebuild = true;
		}

		private static void FlushAllOptimizedGUIBlocksIfNeeded()
		{
			if (InspectorWindow.s_AllOptimizedGUIBlocksNeedsRebuild)
			{
				InspectorWindow.s_AllOptimizedGUIBlocksNeedsRebuild = false;
				foreach (InspectorWindow current in InspectorWindow.m_AllInspectors)
				{
					Editor[] activeEditors = current.tracker.activeEditors;
					for (int i = 0; i < activeEditors.Length; i++)
					{
						Editor editor = activeEditors[i];
						InspectorWindow.FlushOptimizedGUIBlock(editor);
					}
				}
			}
		}

		private static void FlushOptimizedGUIBlock(Editor editor)
		{
			if (!(editor == null))
			{
				OptimizedGUIBlock optimizedGUIBlock;
				float num;
				if (editor.GetOptimizedGUIBlock(false, false, out optimizedGUIBlock, out num))
				{
					optimizedGUIBlock.valid = false;
				}
			}
		}

		private void Update()
		{
			Editor[] activeEditors = this.tracker.activeEditors;
			if (activeEditors != null)
			{
				bool flag = false;
				Editor[] array = activeEditors;
				for (int i = 0; i < array.Length; i++)
				{
					Editor editor = array[i];
					if (editor.RequiresConstantRepaint() && !editor.hideInspector)
					{
						flag = true;
					}
				}
				if (flag && this.m_lastRenderedTime + 0.032999999821186066 < EditorApplication.timeSinceStartup)
				{
					this.m_lastRenderedTime = EditorApplication.timeSinceStartup;
					base.Repaint();
				}
			}
		}

		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
			this.m_ObjectsLockedBeforeSerialization.Clear();
			this.m_InstanceIDsLockedBeforeSerialization.Clear();
			if (this.m_Tracker != null && this.m_Tracker.isLocked)
			{
				this.m_Tracker.GetObjectsLockedByThisTracker(this.m_ObjectsLockedBeforeSerialization);
				for (int i = this.m_ObjectsLockedBeforeSerialization.Count - 1; i >= 0; i--)
				{
					if (!EditorUtility.IsPersistent(this.m_ObjectsLockedBeforeSerialization[i]))
					{
						this.m_InstanceIDsLockedBeforeSerialization.Add(this.m_ObjectsLockedBeforeSerialization[i].GetInstanceID());
						this.m_ObjectsLockedBeforeSerialization.RemoveAt(i);
					}
				}
			}
		}

		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
		}

		private void RestoreLockStateFromSerializedData()
		{
			if (this.m_Tracker != null)
			{
				if (this.m_InstanceIDsLockedBeforeSerialization.Count > 0)
				{
					for (int i = 0; i < this.m_InstanceIDsLockedBeforeSerialization.Count; i++)
					{
						UnityEngine.Object @object = EditorUtility.InstanceIDToObject(this.m_InstanceIDsLockedBeforeSerialization[i]);
						if (@object)
						{
							this.m_ObjectsLockedBeforeSerialization.Add(@object);
						}
					}
				}
				for (int j = this.m_ObjectsLockedBeforeSerialization.Count - 1; j >= 0; j--)
				{
					if (this.m_ObjectsLockedBeforeSerialization[j] == null)
					{
						this.m_ObjectsLockedBeforeSerialization.RemoveAt(j);
					}
				}
				this.m_Tracker.SetObjectsLockedByThisTracker(this.m_ObjectsLockedBeforeSerialization);
				this.m_Tracker.RebuildIfNecessary();
			}
		}
	}
}
