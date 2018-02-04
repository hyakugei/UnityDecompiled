using System;
using UnityEditor.AnimatedValues;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;

namespace UnityEditor
{
	[CustomEditor(typeof(GraphicsSettings))]
	internal class GraphicsSettingsInspector : ProjectSettingsBaseEditor
	{
		internal class Styles
		{
			public static readonly GUIContent showEditorWindow = EditorGUIUtility.TrTextContent("Open Editor...", null, null);

			public static readonly GUIContent closeEditorWindow = EditorGUIUtility.TrTextContent("Close Editor", null, null);

			public static readonly GUIContent tierSettings = EditorGUIUtility.TrTextContent("Tier Settings", null, null);

			public static readonly GUIContent builtinSettings = EditorGUIUtility.TrTextContent("Built-in Shader Settings", null, null);

			public static readonly GUIContent shaderStrippingSettings = EditorGUIUtility.TrTextContent("Shader Stripping", null, null);

			public static readonly GUIContent shaderPreloadSettings = EditorGUIUtility.TrTextContent("Shader Preloading", null, null);

			public static readonly GUIContent cameraSettings = EditorGUIUtility.TrTextContent("Camera Settings", null, null);

			public static readonly GUIContent renderPipeSettings = EditorGUIUtility.TrTextContent("Scriptable Render Pipeline Settings", null, null);

			public static readonly GUIContent renderPipeLabel = EditorGUIUtility.TrTextContent("Scriptable Render Pipeline", null, null);
		}

		private Editor m_TierSettingsEditor;

		private Editor m_BuiltinShadersEditor;

		private Editor m_AlwaysIncludedShadersEditor;

		private Editor m_ShaderStrippingEditor;

		private Editor m_ShaderPreloadEditor;

		private SerializedProperty m_TransparencySortMode;

		private SerializedProperty m_TransparencySortAxis;

		private SerializedProperty m_ScriptableRenderLoop;

		private bool showTierSettingsUI = true;

		private AnimBool tierSettingsAnimator = null;

		private UnityEngine.Object graphicsSettings
		{
			get
			{
				return GraphicsSettings.GetGraphicsSettings();
			}
		}

		private Editor tierSettingsEditor
		{
			get
			{
				Editor.CreateCachedEditor(this.graphicsSettings, typeof(GraphicsSettingsWindow.TierSettingsEditor), ref this.m_TierSettingsEditor);
				((GraphicsSettingsWindow.TierSettingsEditor)this.m_TierSettingsEditor).verticalLayout = true;
				return this.m_TierSettingsEditor;
			}
		}

		private Editor builtinShadersEditor
		{
			get
			{
				Editor.CreateCachedEditor(this.graphicsSettings, typeof(GraphicsSettingsWindow.BuiltinShadersEditor), ref this.m_BuiltinShadersEditor);
				return this.m_BuiltinShadersEditor;
			}
		}

		private Editor alwaysIncludedShadersEditor
		{
			get
			{
				Editor.CreateCachedEditor(this.graphicsSettings, typeof(GraphicsSettingsWindow.AlwaysIncludedShadersEditor), ref this.m_AlwaysIncludedShadersEditor);
				return this.m_AlwaysIncludedShadersEditor;
			}
		}

		private Editor shaderStrippingEditor
		{
			get
			{
				Editor.CreateCachedEditor(this.graphicsSettings, typeof(GraphicsSettingsWindow.ShaderStrippingEditor), ref this.m_ShaderStrippingEditor);
				return this.m_ShaderStrippingEditor;
			}
		}

		private Editor shaderPreloadEditor
		{
			get
			{
				Editor.CreateCachedEditor(this.graphicsSettings, typeof(GraphicsSettingsWindow.ShaderPreloadEditor), ref this.m_ShaderPreloadEditor);
				return this.m_ShaderPreloadEditor;
			}
		}

		public void OnEnable()
		{
			this.m_TransparencySortMode = base.serializedObject.FindProperty("m_TransparencySortMode");
			this.m_TransparencySortAxis = base.serializedObject.FindProperty("m_TransparencySortAxis");
			this.m_ScriptableRenderLoop = base.serializedObject.FindProperty("m_CustomRenderPipeline");
		}

		private void HandleEditorWindowButton()
		{
			TierSettingsWindow instance = TierSettingsWindow.GetInstance();
			GUIContent content = (!(instance == null)) ? GraphicsSettingsInspector.Styles.closeEditorWindow : GraphicsSettingsInspector.Styles.showEditorWindow;
			if (GUILayout.Button(content, EditorStyles.miniButton, new GUILayoutOption[]
			{
				GUILayout.Width(110f)
			}))
			{
				if (instance)
				{
					instance.Close();
				}
				else
				{
					TierSettingsWindow.CreateWindow();
					TierSettingsWindow.GetInstance().Show();
				}
			}
		}

		private void TierSettingsGUI()
		{
			if (this.tierSettingsAnimator == null)
			{
				this.tierSettingsAnimator = new AnimBool(this.showTierSettingsUI, new UnityAction(base.Repaint));
			}
			bool enabled = GUI.enabled;
			GUI.enabled = true;
			EditorGUILayout.BeginVertical(GUI.skin.box, new GUILayoutOption[0]);
			EditorGUILayout.BeginHorizontal(new GUILayoutOption[0]);
			Rect rect = GUILayoutUtility.GetRect(20f, 18f);
			rect.x += 3f;
			rect.width += 6f;
			this.showTierSettingsUI = GUI.Toggle(rect, this.showTierSettingsUI, GraphicsSettingsInspector.Styles.tierSettings, EditorStyles.inspectorTitlebarText);
			this.HandleEditorWindowButton();
			EditorGUILayout.EndHorizontal();
			this.tierSettingsAnimator.target = this.showTierSettingsUI;
			GUI.enabled = enabled;
			if (EditorGUILayout.BeginFadeGroup(this.tierSettingsAnimator.faded) && TierSettingsWindow.GetInstance() == null)
			{
				this.tierSettingsEditor.OnInspectorGUI();
			}
			EditorGUILayout.EndFadeGroup();
			EditorGUILayout.EndVertical();
		}

		public override void OnInspectorGUI()
		{
			base.serializedObject.Update();
			GUILayout.Label(GraphicsSettingsInspector.Styles.renderPipeSettings, EditorStyles.boldLabel, new GUILayoutOption[0]);
			Rect controlRect = EditorGUILayout.GetControlRect(true, EditorGUI.GetPropertyHeight(this.m_ScriptableRenderLoop), new GUILayoutOption[0]);
			EditorGUI.BeginProperty(controlRect, GraphicsSettingsInspector.Styles.renderPipeLabel, this.m_ScriptableRenderLoop);
			this.m_ScriptableRenderLoop.objectReferenceValue = EditorGUI.ObjectField(controlRect, this.m_ScriptableRenderLoop.objectReferenceValue, typeof(RenderPipelineAsset), false);
			EditorGUI.EndProperty();
			EditorGUILayout.Space();
			bool flag = GraphicsSettings.renderPipelineAsset != null;
			if (flag)
			{
				EditorGUILayout.HelpBox("A Scriptable Render Pipeline is in use, some settings will not be used and are hidden", MessageType.Info);
			}
			if (!flag)
			{
				GUILayout.Label(GraphicsSettingsInspector.Styles.cameraSettings, EditorStyles.boldLabel, new GUILayoutOption[0]);
				EditorGUILayout.PropertyField(this.m_TransparencySortMode, new GUILayoutOption[0]);
				EditorGUILayout.PropertyField(this.m_TransparencySortAxis, new GUILayoutOption[0]);
				EditorGUILayout.Space();
			}
			this.TierSettingsGUI();
			GUILayout.Label(GraphicsSettingsInspector.Styles.builtinSettings, EditorStyles.boldLabel, new GUILayoutOption[0]);
			if (!flag)
			{
				this.builtinShadersEditor.OnInspectorGUI();
			}
			this.alwaysIncludedShadersEditor.OnInspectorGUI();
			EditorGUILayout.Space();
			GUILayout.Label(GraphicsSettingsInspector.Styles.shaderStrippingSettings, EditorStyles.boldLabel, new GUILayoutOption[0]);
			this.shaderStrippingEditor.OnInspectorGUI();
			EditorGUILayout.Space();
			GUILayout.Label(GraphicsSettingsInspector.Styles.shaderPreloadSettings, EditorStyles.boldLabel, new GUILayoutOption[0]);
			this.shaderPreloadEditor.OnInspectorGUI();
			base.serializedObject.ApplyModifiedProperties();
		}
	}
}
