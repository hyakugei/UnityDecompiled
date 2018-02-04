using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor.Presets
{
	[CustomEditor(typeof(PresetManager))]
	internal sealed class PresetManagerEditor : Editor
	{
		private static class Style
		{
			public static GUIContent managerIcon = EditorGUIUtility.IconContent("GameManager Icon");

			public static GUIStyle centerStyle = new GUIStyle
			{
				alignment = TextAnchor.MiddleCenter
			};
		}

		private Dictionary<string, List<Preset>> m_DiscoveredPresets = new Dictionary<string, List<Preset>>();

		private SerializedProperty m_DefaultPresets;

		private HashSet<string> m_AddedTypes = new HashSet<string>();

		private ReorderableList m_List;

		private GenericMenu m_AddingMenu;

		private static GUIContent s_DropIcon = null;

		private new PresetManager target
		{
			get
			{
				return base.target as PresetManager;
			}
		}

		internal override void OnHeaderIconGUI(Rect iconRect)
		{
			GUI.Label(iconRect, PresetManagerEditor.Style.managerIcon, PresetManagerEditor.Style.centerStyle);
		}

		internal override void OnHeaderTitleGUI(Rect titleRect, string header)
		{
			header = "PresetManager";
			base.OnHeaderTitleGUI(titleRect, header);
		}

		private void OnEnable()
		{
			this.m_DefaultPresets = base.serializedObject.FindProperty("m_DefaultList");
			this.m_List = new ReorderableList(base.serializedObject, this.m_DefaultPresets);
			this.m_List.draggable = false;
			this.m_List.drawHeaderCallback = delegate(Rect rect)
			{
				EditorGUI.LabelField(rect, GUIContent.Temp("Default Presets"));
			};
			this.m_List.drawElementCallback = new ReorderableList.ElementCallbackDelegate(this.DrawElementCallback);
			this.m_List.onAddDropdownCallback = new ReorderableList.AddDropdownCallbackDelegate(this.OnAddDropdownCallback);
			this.m_List.onRemoveCallback = new ReorderableList.RemoveCallbackDelegate(this.OnRemoveCallback);
			this.RefreshAddList();
			this.m_List.onCanAddCallback = ((ReorderableList list) => this.m_AddingMenu.GetItemCount() > 0);
		}

		private void OnRemoveCallback(ReorderableList list)
		{
			ReorderableList.defaultBehaviours.DoRemoveButton(list);
			this.RefreshAddList();
		}

		private void OnFocus()
		{
			this.RefreshAddList();
		}

		private void RefreshAddList()
		{
			this.m_AddedTypes.Clear();
			for (int i = 0; i < this.m_DefaultPresets.arraySize; i++)
			{
				this.m_AddedTypes.Add(this.target.GetPresetTypeNameAtIndex(i));
			}
			IEnumerable<Preset> enumerable = from a in AssetDatabase.FindAssets("t:Preset")
			select AssetDatabase.LoadAssetAtPath<Preset>(AssetDatabase.GUIDToAssetPath(a));
			this.m_DiscoveredPresets.Clear();
			foreach (Preset current in enumerable)
			{
				string targetFullTypeName = current.GetTargetFullTypeName();
				if (current.IsValid() && !Preset.IsPresetExcludedFromDefaultPresets(current))
				{
					if (!this.m_DiscoveredPresets.ContainsKey(targetFullTypeName))
					{
						this.m_DiscoveredPresets.Add(targetFullTypeName, new List<Preset>());
					}
					this.m_DiscoveredPresets[targetFullTypeName].Add(current);
				}
			}
			this.m_AddingMenu = new GenericMenu();
			foreach (KeyValuePair<string, List<Preset>> current2 in this.m_DiscoveredPresets)
			{
				if (!this.m_AddedTypes.Contains(current2.Key))
				{
					foreach (Preset current3 in current2.Value)
					{
						this.m_AddingMenu.AddItem(new GUIContent(current2.Key.Replace(".", "/") + "/" + current3.name), false, new GenericMenu.MenuFunction2(this.OnAddingPreset), current3);
					}
				}
			}
		}

		private void OnAddDropdownCallback(Rect buttonRect, ReorderableList list)
		{
			this.m_AddingMenu.DropDown(buttonRect);
		}

		private void OnAddingPreset(object userData)
		{
			base.serializedObject.ApplyModifiedProperties();
			Undo.RecordObject(this.target, "Inspector");
			this.target.SetAsDefaultInternal((Preset)userData);
			base.serializedObject.Update();
			this.RefreshAddList();
		}

		private static string FullTypeNameToFriendlyName(string fullTypeName)
		{
			string result;
			if (string.IsNullOrEmpty(fullTypeName))
			{
				result = "Unsupported Type";
			}
			else
			{
				int num = fullTypeName.LastIndexOf(".");
				if (num == -1)
				{
					result = fullTypeName;
				}
				else
				{
					result = string.Format("{0} ({1})", fullTypeName.Substring(num + 1), fullTypeName.Substring(0, num));
				}
			}
			return result;
		}

		private void DrawElementCallback(Rect rect, int index, bool isActive, bool isFocused)
		{
			rect.height = EditorGUIUtility.singleLineHeight;
			rect.y += 2f;
			SerializedProperty serializedProperty = this.m_DefaultPresets.GetArrayElementAtIndex(index).FindPropertyRelative("defaultPresets.Array.data[0].m_Preset");
			Preset preset = (Preset)serializedProperty.objectReferenceValue;
			string presetTypeNameAtIndex = this.target.GetPresetTypeNameAtIndex(index);
			GUIContent gUIContent = GUIContent.Temp(PresetManagerEditor.FullTypeNameToFriendlyName(presetTypeNameAtIndex));
			using (new EditorGUI.DisabledGroupScope(string.IsNullOrEmpty(presetTypeNameAtIndex)))
			{
				Rect position = new Rect(rect.xMax - rect.height, rect.y, rect.height, rect.height);
				EventType type = Event.current.type;
				if (type == EventType.MouseDown)
				{
					if (position.Contains(Event.current.mousePosition))
					{
						GenericMenu genericMenu = new GenericMenu();
						if (this.m_DiscoveredPresets.ContainsKey(presetTypeNameAtIndex))
						{
							foreach (Preset current in this.m_DiscoveredPresets[presetTypeNameAtIndex])
							{
								genericMenu.AddItem(new GUIContent(current.name), current == preset, new GenericMenu.MenuFunction2(this.OnAddingPreset), current);
							}
						}
						else
						{
							genericMenu.AddItem(new GUIContent("None"), false, null);
						}
						genericMenu.ShowAsContext();
						Event.current.Use();
					}
				}
				int controlID = GUIUtility.GetControlID(gUIContent, FocusType.Passive);
				Rect rect2 = EditorGUI.PrefixLabel(rect, gUIContent);
				Rect dropRect = rect2;
				dropRect.xMax -= rect2.height;
				EditorGUI.DoObjectField(rect2, dropRect, controlID, preset, typeof(Preset), serializedProperty, new EditorGUI.ObjectFieldValidator(this.PresetFieldDropValidator), false);
				position.x += 7f;
				position.width = 9f;
				position.height = 10f;
				position.y += 11f;
				EditorGUI.LabelField(position, PresetManagerEditor.s_DropIcon);
			}
		}

		private UnityEngine.Object PresetFieldDropValidator(UnityEngine.Object[] references, Type objType, SerializedProperty property, EditorGUI.ObjectFieldValidatorOptions options)
		{
			UnityEngine.Object result;
			if (references.Length == 1)
			{
				Preset preset = references[0] as Preset;
				string propertyPath = property.propertyPath;
				int num = propertyPath.IndexOf("[") + 1;
				int length = propertyPath.IndexOf("]") - num;
				int index = int.Parse(propertyPath.Substring(num, length));
				if (preset != null && this.target.GetPresetTypeNameAtIndex(index) == preset.GetTargetFullTypeName())
				{
					result = references[0];
					return result;
				}
			}
			result = null;
			return result;
		}

		public override void OnInspectorGUI()
		{
			if (Event.current.type == EventType.MouseEnterWindow)
			{
				this.RefreshAddList();
			}
			else
			{
				if (PresetManagerEditor.s_DropIcon == null)
				{
					PresetManagerEditor.s_DropIcon = EditorGUIUtility.IconContent("Icon Dropdown");
				}
				base.serializedObject.Update();
				this.m_List.DoLayoutList();
				base.serializedObject.ApplyModifiedProperties();
			}
		}

		[MenuItem("Edit/Project Settings/Preset Manager", false, 300)]
		private static void ShowManagerInspector()
		{
			Selection.activeObject = Resources.FindObjectsOfTypeAll<PresetManager>().First<PresetManager>();
		}
	}
}
