using System;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor.Presets
{
	[CanEditMultipleObjects, CustomEditor(typeof(Preset))]
	internal class PresetEditor : Editor
	{
		private static class Style
		{
			public static GUIContent presetType;

			public static GUIStyle inspectorBig;

			public static GUIStyle centerStyle;

			static Style()
			{
				PresetEditor.Style.presetType = EditorGUIUtility.TrTextContent("Preset Type", "The Object type this Preset can be applied to.", null);
				PresetEditor.Style.inspectorBig = new GUIStyle(EditorStyles.inspectorBig);
				PresetEditor.Style.centerStyle = new GUIStyle
				{
					alignment = TextAnchor.MiddleCenter
				};
				PresetEditor.Style.inspectorBig.padding.bottom--;
			}
		}

		private class ReferenceCount
		{
			public int count;

			public UnityEngine.Object reference;
		}

		private static Dictionary<int, PresetEditor.ReferenceCount> s_References = new Dictionary<int, PresetEditor.ReferenceCount>();

		private bool m_DisplayErrorPreset;

		private string m_SelectedPresetTypeName;

		private Dictionary<string, List<UnityEngine.Object>> m_InspectedTypes = new Dictionary<string, List<UnityEngine.Object>>();

		private Editor m_InternalEditor = null;

		private string m_NotSupportedEditorName = null;

		private void OnEnable()
		{
			this.m_InspectedTypes.Clear();
			UnityEngine.Object[] targets = base.targets;
			for (int i = 0; i < targets.Length; i++)
			{
				UnityEngine.Object @object = targets[i];
				Preset preset = (Preset)@object;
				string targetFullTypeName = preset.GetTargetFullTypeName();
				if (!this.m_InspectedTypes.ContainsKey(targetFullTypeName))
				{
					this.m_InspectedTypes.Add(targetFullTypeName, new List<UnityEngine.Object>());
				}
				this.m_InspectedTypes[targetFullTypeName].Add(@object);
			}
			if (this.m_InspectedTypes.Count == 1)
			{
				Preset preset2 = (Preset)base.target;
				if (preset2.IsValid())
				{
					this.m_SelectedPresetTypeName = preset2.GetTargetFullTypeName();
					this.GenerateInternalEditor();
				}
				else
				{
					this.m_SelectedPresetTypeName = "Invalid";
					this.m_DisplayErrorPreset = true;
				}
			}
		}

		private void OnDisable()
		{
			this.DestroyInternalEditor();
		}

		public override void OnInspectorGUI()
		{
			if (this.m_DisplayErrorPreset)
			{
				EditorGUILayout.HelpBox("Unable to load this Preset, the type is not supported.", MessageType.Error);
			}
			else
			{
				this.DrawPresetData();
			}
		}

		private void DrawPresetData()
		{
			string text = (this.m_InspectedTypes.Count <= 1) ? this.m_SelectedPresetTypeName : EditorGUI.mixedValueContent.text;
			Rect position = EditorGUI.PrefixLabel(EditorGUILayout.GetControlRect(true, new GUILayoutOption[0]), PresetEditor.Style.presetType);
			EditorGUI.SelectableLabel(position, text);
			if (this.m_InternalEditor != null)
			{
				this.DrawInternalInspector();
			}
			else if (!string.IsNullOrEmpty(this.m_NotSupportedEditorName))
			{
				this.DrawUnsupportedInspector();
			}
		}

		internal override void OnHeaderControlsGUI()
		{
			using (new EditorGUI.DisabledScope(base.targets.Length != 1 || Preset.IsPresetExcludedFromDefaultPresets(base.target as Preset)))
			{
				Preset preset = (Preset)base.target;
				if (Preset.GetDefaultForPreset(preset) == preset)
				{
					if (GUILayout.Button(string.Format("Remove from {0} Default", preset.GetTargetTypeName()), EditorStyles.miniButton, new GUILayoutOption[]
					{
						GUILayout.ExpandWidth(false)
					}))
					{
						Preset.RemoveFromDefault(preset);
					}
				}
				else if (GUILayout.Button(string.Format("Set as {0} Default", preset.GetTargetTypeName()), EditorStyles.miniButton, new GUILayoutOption[]
				{
					GUILayout.ExpandWidth(false)
				}))
				{
					Preset.SetAsDefault(preset);
				}
			}
		}

		internal override void OnHeaderTitleGUI(Rect titleRect, string header)
		{
			if (this.m_InspectedTypes.Count > 1)
			{
				header = string.Format("Multiple Types Presets ({0})", base.targets.Length);
			}
			else if (base.targets.Length > 1)
			{
				header = string.Format("{0} Presets ({1})", this.m_SelectedPresetTypeName, base.targets.Length);
			}
			base.OnHeaderTitleGUI(titleRect, header);
		}

		public override bool UseDefaultMargins()
		{
			return false;
		}

		private void GenerateInternalEditor()
		{
			UnityEngine.Object[] array = new UnityEngine.Object[base.targets.Length];
			for (int i = 0; i < base.targets.Length; i++)
			{
				Preset preset = (Preset)base.targets[i];
				PresetEditor.ReferenceCount referenceCount = null;
				if (!PresetEditor.s_References.TryGetValue(preset.GetInstanceID(), out referenceCount))
				{
					referenceCount = new PresetEditor.ReferenceCount
					{
						count = 0,
						reference = preset.GetReferenceObject()
					};
					if (referenceCount.reference == null)
					{
						this.m_NotSupportedEditorName = preset.GetTargetTypeName();
						return;
					}
					PresetEditor.s_References.Add(preset.GetInstanceID(), referenceCount);
				}
				referenceCount.count++;
				array[i] = referenceCount.reference;
			}
			this.m_InternalEditor = Editor.CreateEditor(array);
		}

		private void DestroyInternalEditor()
		{
			if (this.m_InternalEditor != null)
			{
				UnityEngine.Object.DestroyImmediate(this.m_InternalEditor);
				for (int i = 0; i < base.targets.Length; i++)
				{
					Preset preset = (Preset)base.targets[i];
					if (--PresetEditor.s_References[preset.GetInstanceID()].count == 0)
					{
						if (PresetEditor.s_References[preset.GetInstanceID()].reference is Component)
						{
							GameObject gameObject = ((Component)PresetEditor.s_References[preset.GetInstanceID()].reference).gameObject;
							UnityEngine.Object.DestroyImmediate(gameObject);
						}
						else
						{
							UnityEngine.Object.DestroyImmediate(PresetEditor.s_References[preset.GetInstanceID()].reference);
						}
						PresetEditor.s_References.Remove(preset.GetInstanceID());
					}
				}
			}
		}

		private void DrawInternalInspector()
		{
			using (EditorGUI.ChangeCheckScope changeCheckScope = new EditorGUI.ChangeCheckScope())
			{
				this.m_InternalEditor.target.hideFlags = HideFlags.None;
				if (this.m_InternalEditor.target is Component)
				{
					bool isInspectorExpanded = InternalEditorUtility.GetIsInspectorExpanded(this.m_InternalEditor.target);
					bool flag = EditorGUILayout.InspectorTitlebar(isInspectorExpanded, this.m_InternalEditor.targets, this.m_InternalEditor.CanBeExpandedViaAFoldout());
					if (flag != isInspectorExpanded)
					{
						InternalEditorUtility.SetIsInspectorExpanded(this.m_InternalEditor.target, flag);
					}
				}
				else
				{
					this.m_InternalEditor.DrawHeader();
				}
				if (InternalEditorUtility.GetIsInspectorExpanded(this.m_InternalEditor.target))
				{
					EditorGUI.indentLevel++;
					this.m_InternalEditor.OnInspectorGUI();
					EditorGUI.indentLevel--;
				}
				this.m_InternalEditor.target.hideFlags = HideFlags.HideAndDontSave;
				if (changeCheckScope.changed || this.m_InternalEditor.isInspectorDirty)
				{
					for (int i = 0; i < this.m_InternalEditor.targets.Length; i++)
					{
						((Preset)base.targets[i]).UpdateProperties(this.m_InternalEditor.targets[i]);
					}
				}
			}
		}

		private void DrawUnsupportedInspector()
		{
			GUILayout.BeginHorizontal(PresetEditor.Style.inspectorBig, new GUILayoutOption[0]);
			GUILayout.Space(38f);
			GUILayout.BeginVertical(new GUILayoutOption[0]);
			GUILayout.Space(19f);
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			EditorGUILayout.GetControlRect(new GUILayoutOption[0]);
			GUILayout.EndHorizontal();
			GUILayout.EndVertical();
			GUILayout.EndHorizontal();
			Rect lastRect = GUILayoutUtility.GetLastRect();
			Rect position = new Rect(lastRect.x + 6f, lastRect.y + 6f, 32f, 32f);
			GUI.Label(position, AssetPreview.GetMiniTypeThumbnail(typeof(UnityEngine.Object)), PresetEditor.Style.centerStyle);
			Rect position2 = new Rect(lastRect.x + 44f, lastRect.y + 6f, lastRect.width - 86f, 16f);
			GUI.Label(position2, this.m_NotSupportedEditorName, EditorStyles.largeLabel);
			EditorGUILayout.HelpBox("Preset Inspectors are not supported for this type.", MessageType.Info);
		}
	}
}
