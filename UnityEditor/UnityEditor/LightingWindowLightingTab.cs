using System;
using UnityEngine;

namespace UnityEditor
{
	internal class LightingWindowLightingTab
	{
		private static class Styles
		{
			public static readonly GUIContent OtherSettings = EditorGUIUtility.TrTextContent("Other Settings", null, null);

			public static readonly GUIContent DebugSettings = EditorGUIUtility.TrTextContent("Debug Settings", null, null);

			public static readonly GUIContent LightProbeVisualization = EditorGUIUtility.TrTextContent("Light Probe Visualization", null, null);
		}

		private Editor m_LightingEditor;

		private Editor m_FogEditor;

		private Editor m_OtherRenderingEditor;

		private bool m_ShowOtherSettings = true;

		private bool m_ShowDebugSettings = false;

		private bool m_ShowProbeDebugSettings = false;

		private bool m_ShouldUpdateStatistics = true;

		private const string kShowOtherSettings = "kShowOtherSettings";

		private const string kShowDebugSettings = "kShowDebugSettings";

		private const string kUpdateStatistics = "kUpdateStatistics";

		private UnityEngine.Object m_RenderSettings = null;

		private LightingWindowBakeSettings m_BakeSettings;

		private UnityEngine.Object renderSettings
		{
			get
			{
				if (this.m_RenderSettings == null)
				{
					this.m_RenderSettings = RenderSettings.GetRenderSettings();
				}
				return this.m_RenderSettings;
			}
		}

		private Editor lightingEditor
		{
			get
			{
				if (this.m_LightingEditor == null || this.m_LightingEditor.target == null)
				{
					Editor.CreateCachedEditor(this.renderSettings, typeof(LightingEditor), ref this.m_LightingEditor);
				}
				return this.m_LightingEditor;
			}
		}

		private Editor fogEditor
		{
			get
			{
				if (this.m_FogEditor == null || this.m_FogEditor.target == null)
				{
					Editor.CreateCachedEditor(this.renderSettings, typeof(FogEditor), ref this.m_FogEditor);
				}
				return this.m_FogEditor;
			}
		}

		private Editor otherRenderingEditor
		{
			get
			{
				if (this.m_OtherRenderingEditor == null || this.m_OtherRenderingEditor.target == null)
				{
					Editor.CreateCachedEditor(this.renderSettings, typeof(OtherRenderingEditor), ref this.m_OtherRenderingEditor);
				}
				return this.m_OtherRenderingEditor;
			}
		}

		public void OnEnable()
		{
			this.m_BakeSettings = new LightingWindowBakeSettings();
			this.m_BakeSettings.OnEnable();
			this.m_ShowOtherSettings = SessionState.GetBool("kShowOtherSettings", true);
			this.m_ShowDebugSettings = SessionState.GetBool("kShowDebugSettings", false);
			this.m_ShouldUpdateStatistics = SessionState.GetBool("kUpdateStatistics", false);
		}

		public void OnDisable()
		{
			this.m_BakeSettings.OnDisable();
			SessionState.SetBool("kShowOtherSettings", this.m_ShowOtherSettings);
			SessionState.SetBool("kShowDebugSettings", this.m_ShowDebugSettings);
			SessionState.SetBool("kUpdateStatistics", this.m_ShouldUpdateStatistics);
			this.ClearCachedProperties();
		}

		private void ClearCachedProperties()
		{
			if (this.m_LightingEditor != null)
			{
				UnityEngine.Object.DestroyImmediate(this.m_LightingEditor);
				this.m_LightingEditor = null;
			}
			if (this.m_FogEditor != null)
			{
				UnityEngine.Object.DestroyImmediate(this.m_FogEditor);
				this.m_FogEditor = null;
			}
			if (this.m_OtherRenderingEditor != null)
			{
				UnityEngine.Object.DestroyImmediate(this.m_OtherRenderingEditor);
				this.m_OtherRenderingEditor = null;
			}
		}

		private void DebugSettingsGUI()
		{
			this.m_ShowDebugSettings = EditorGUILayout.FoldoutTitlebar(this.m_ShowDebugSettings, LightingWindowLightingTab.Styles.DebugSettings, true);
			if (this.m_ShowDebugSettings)
			{
				EditorGUI.indentLevel++;
				this.m_ShowProbeDebugSettings = EditorGUILayout.Foldout(this.m_ShowProbeDebugSettings, LightingWindowLightingTab.Styles.LightProbeVisualization, true);
				if (this.m_ShowProbeDebugSettings)
				{
					EditorGUI.BeginChangeCheck();
					EditorGUI.indentLevel++;
					LightProbeVisualization.lightProbeVisualizationMode = (LightProbeVisualization.LightProbeVisualizationMode)EditorGUILayout.EnumPopup(LightProbeVisualization.lightProbeVisualizationMode, new GUILayoutOption[0]);
					LightProbeVisualization.showInterpolationWeights = EditorGUILayout.Toggle("Display Weights", LightProbeVisualization.showInterpolationWeights, new GUILayoutOption[0]);
					LightProbeVisualization.showOcclusions = EditorGUILayout.Toggle("Display Occlusion", LightProbeVisualization.showOcclusions, new GUILayoutOption[0]);
					EditorGUI.indentLevel--;
					if (EditorGUI.EndChangeCheck())
					{
						EditorApplication.SetSceneRepaintDirty();
					}
				}
				EditorGUILayout.Space();
				this.m_BakeSettings.DeveloperBuildSettingsGUI();
				EditorGUI.indentLevel--;
				EditorGUILayout.Space();
			}
		}

		private void OtherSettingsGUI()
		{
			this.m_ShowOtherSettings = EditorGUILayout.FoldoutTitlebar(this.m_ShowOtherSettings, LightingWindowLightingTab.Styles.OtherSettings, true);
			if (this.m_ShowOtherSettings)
			{
				EditorGUI.indentLevel++;
				this.fogEditor.OnInspectorGUI();
				this.otherRenderingEditor.OnInspectorGUI();
				EditorGUI.indentLevel--;
				EditorGUILayout.Space();
			}
		}

		public void OnGUI()
		{
			EditorGUIUtility.hierarchyMode = true;
			this.lightingEditor.OnInspectorGUI();
			this.m_BakeSettings.OnGUI();
			this.OtherSettingsGUI();
			this.DebugSettingsGUI();
		}
	}
}
