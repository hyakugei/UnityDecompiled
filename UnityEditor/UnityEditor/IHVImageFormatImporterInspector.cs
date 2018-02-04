using System;
using UnityEditor.Experimental.AssetImporters;
using UnityEngine;

namespace UnityEditor
{
	[CanEditMultipleObjects, CustomEditor(typeof(IHVImageFormatImporter))]
	internal class IHVImageFormatImporterInspector : AssetImporterEditor
	{
		internal class Styles
		{
			public static readonly GUIContent readWrite = EditorGUIUtility.TrTextContent("Read/Write Enabled", "Enable to be able to access the raw pixel data from code.", null);

			public static readonly GUIContent sRGBTexture = EditorGUIUtility.TrTextContent("sRGB (Color Texture)", "Texture content is stored in gamma space. Non-HDR color textures should enable this flag (except if used for IMGUI).", null);

			public static readonly GUIContent wrapMode = EditorGUIUtility.TrTextContent("Wrap Mode", null, null);

			public static readonly GUIContent filterMode = EditorGUIUtility.TrTextContent("Filter Mode", null, null);

			public static readonly int[] filterModeValues = new int[]
			{
				0,
				1,
				2
			};

			public static readonly GUIContent[] filterModeOptions = new GUIContent[]
			{
				EditorGUIUtility.TextContent("Point (no filter)"),
				EditorGUIUtility.TextContent("Bilinear"),
				EditorGUIUtility.TrTextContent("Trilinear", null, null)
			};
		}

		private SerializedProperty m_IsReadable;

		private SerializedProperty m_sRGBTexture;

		private SerializedProperty m_FilterMode;

		private SerializedProperty m_WrapU;

		private SerializedProperty m_WrapV;

		private SerializedProperty m_WrapW;

		private bool m_ShowPerAxisWrapModes = false;

		public override bool showImportedObject
		{
			get
			{
				return false;
			}
		}

		public override void OnEnable()
		{
			this.m_IsReadable = base.serializedObject.FindProperty("m_IsReadable");
			this.m_sRGBTexture = base.serializedObject.FindProperty("m_sRGBTexture");
			this.m_FilterMode = base.serializedObject.FindProperty("m_TextureSettings.m_FilterMode");
			this.m_WrapU = base.serializedObject.FindProperty("m_TextureSettings.m_WrapU");
			this.m_WrapV = base.serializedObject.FindProperty("m_TextureSettings.m_WrapV");
			this.m_WrapW = base.serializedObject.FindProperty("m_TextureSettings.m_WrapW");
		}

		public void TextureSettingsGUI()
		{
			bool isVolumeTexture = false;
			TextureInspector.WrapModePopup(this.m_WrapU, this.m_WrapV, this.m_WrapW, isVolumeTexture, ref this.m_ShowPerAxisWrapModes);
			Rect controlRect = EditorGUILayout.GetControlRect(new GUILayoutOption[0]);
			EditorGUI.BeginProperty(controlRect, IHVImageFormatImporterInspector.Styles.filterMode, this.m_FilterMode);
			EditorGUI.BeginChangeCheck();
			FilterMode filterMode = (FilterMode)((this.m_FilterMode.intValue != -1) ? this.m_FilterMode.intValue : 1);
			filterMode = (FilterMode)EditorGUI.IntPopup(controlRect, IHVImageFormatImporterInspector.Styles.filterMode, (int)filterMode, IHVImageFormatImporterInspector.Styles.filterModeOptions, IHVImageFormatImporterInspector.Styles.filterModeValues);
			if (EditorGUI.EndChangeCheck())
			{
				this.m_FilterMode.intValue = (int)filterMode;
			}
			EditorGUI.EndProperty();
		}

		public override void OnInspectorGUI()
		{
			EditorGUILayout.PropertyField(this.m_IsReadable, IHVImageFormatImporterInspector.Styles.readWrite, new GUILayoutOption[0]);
			EditorGUILayout.PropertyField(this.m_sRGBTexture, IHVImageFormatImporterInspector.Styles.sRGBTexture, new GUILayoutOption[0]);
			EditorGUI.BeginChangeCheck();
			this.TextureSettingsGUI();
			if (EditorGUI.EndChangeCheck())
			{
				UnityEngine.Object[] targets = base.targets;
				for (int i = 0; i < targets.Length; i++)
				{
					AssetImporter assetImporter = (AssetImporter)targets[i];
					Texture tex = AssetDatabase.LoadMainAssetAtPath(assetImporter.assetPath) as Texture;
					if (this.m_FilterMode.intValue != -1)
					{
						TextureUtil.SetFilterModeNoDirty(tex, (FilterMode)this.m_FilterMode.intValue);
					}
					if ((this.m_WrapU.intValue != -1 || this.m_WrapV.intValue != -1 || this.m_WrapW.intValue != -1) && !this.m_WrapU.hasMultipleDifferentValues && !this.m_WrapV.hasMultipleDifferentValues && !this.m_WrapW.hasMultipleDifferentValues)
					{
						TextureUtil.SetWrapModeNoDirty(tex, (TextureWrapMode)this.m_WrapU.intValue, (TextureWrapMode)this.m_WrapV.intValue, (TextureWrapMode)this.m_WrapW.intValue);
					}
				}
				SceneView.RepaintAll();
			}
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.FlexibleSpace();
			base.ApplyRevertGUI();
			GUILayout.EndHorizontal();
		}
	}
}
