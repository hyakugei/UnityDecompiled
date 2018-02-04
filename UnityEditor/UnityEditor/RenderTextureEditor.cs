using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace UnityEditor
{
	[CanEditMultipleObjects, CustomEditor(typeof(RenderTexture))]
	internal class RenderTextureEditor : TextureInspector
	{
		private class Styles
		{
			public readonly GUIContent size = EditorGUIUtility.TrTextContent("Size", "Size of the render texture in pixels.", null);

			public readonly GUIContent cross = EditorGUIUtility.TextContent("x");

			public readonly GUIContent antiAliasing = EditorGUIUtility.TrTextContent("Anti-Aliasing", "Number of anti-aliasing samples.", null);

			public readonly GUIContent colorFormat = EditorGUIUtility.TrTextContent("Color Format", "Format of the color buffer.", null);

			public readonly GUIContent depthBuffer = EditorGUIUtility.TrTextContent("Depth Buffer", "Format of the depth buffer.", null);

			public readonly GUIContent dimension = EditorGUIUtility.TrTextContent("Dimension", "Is the texture 2D, Cube or 3D?", null);

			public readonly GUIContent enableMipmaps = EditorGUIUtility.TrTextContent("Enable Mip Maps", "This render texture will have Mip Maps.", null);

			public readonly GUIContent bindMS = EditorGUIUtility.TrTextContent("Bind multisampled", "If enabled, the texture will not go through an AA resolve if bound to a shader.", null);

			public readonly GUIContent autoGeneratesMipmaps = EditorGUIUtility.TrTextContent("Auto generate Mip Maps", "This render texture automatically generate its Mip Maps.", null);

			public readonly GUIContent sRGBTexture = EditorGUIUtility.TrTextContent("sRGB (Color RenderTexture)", "RenderTexture content is stored in gamma space. Non-HDR color textures should enable this flag.", null);

			public readonly GUIContent useDynamicScale = EditorGUIUtility.TrTextContent("Dynamic Scaling", "Allow the texture to be automatically resized by ScalableBufferManager, to support dynamic resolution.", null);

			public readonly GUIContent[] renderTextureAntiAliasing = new GUIContent[]
			{
				EditorGUIUtility.TrTextContent("None", null, null),
				EditorGUIUtility.TrTextContent("2 samples", null, null),
				EditorGUIUtility.TrTextContent("4 samples", null, null),
				EditorGUIUtility.TrTextContent("8 samples", null, null)
			};

			public readonly int[] renderTextureAntiAliasingValues = new int[]
			{
				1,
				2,
				4,
				8
			};

			public readonly GUIContent[] dimensionStrings = new GUIContent[]
			{
				EditorGUIUtility.TextContent("2D"),
				EditorGUIUtility.TextContent("Cube"),
				EditorGUIUtility.TrTextContent("3D", null, null)
			};

			public readonly int[] dimensionValues = new int[]
			{
				2,
				4,
				3
			};
		}

		[Flags]
		protected enum GUIElements
		{
			RenderTargetNoneGUI = 0,
			RenderTargetDepthGUI = 2,
			RenderTargetAAGUI = 4
		}

		private static RenderTextureEditor.Styles s_Styles = null;

		private const RenderTextureEditor.GUIElements s_AllGUIElements = RenderTextureEditor.GUIElements.RenderTargetDepthGUI | RenderTextureEditor.GUIElements.RenderTargetAAGUI;

		private SerializedProperty m_Width;

		private SerializedProperty m_Height;

		private SerializedProperty m_Depth;

		private SerializedProperty m_ColorFormat;

		private SerializedProperty m_DepthFormat;

		private SerializedProperty m_AntiAliasing;

		private SerializedProperty m_EnableMipmaps;

		private SerializedProperty m_AutoGeneratesMipmaps;

		private SerializedProperty m_Dimension;

		private SerializedProperty m_sRGB;

		private SerializedProperty m_UseDynamicScale;

		private static RenderTextureEditor.Styles styles
		{
			get
			{
				if (RenderTextureEditor.s_Styles == null)
				{
					RenderTextureEditor.s_Styles = new RenderTextureEditor.Styles();
				}
				return RenderTextureEditor.s_Styles;
			}
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			this.m_Width = base.serializedObject.FindProperty("m_Width");
			this.m_Height = base.serializedObject.FindProperty("m_Height");
			this.m_Depth = base.serializedObject.FindProperty("m_VolumeDepth");
			this.m_AntiAliasing = base.serializedObject.FindProperty("m_AntiAliasing");
			this.m_ColorFormat = base.serializedObject.FindProperty("m_ColorFormat");
			this.m_DepthFormat = base.serializedObject.FindProperty("m_DepthFormat");
			this.m_EnableMipmaps = base.serializedObject.FindProperty("m_MipMap");
			this.m_AutoGeneratesMipmaps = base.serializedObject.FindProperty("m_GenerateMips");
			this.m_Dimension = base.serializedObject.FindProperty("m_Dimension");
			this.m_sRGB = base.serializedObject.FindProperty("m_SRGB");
			this.m_UseDynamicScale = base.serializedObject.FindProperty("m_UseDynamicScale");
		}

		public static bool IsHDRFormat(RenderTextureFormat format)
		{
			return format == RenderTextureFormat.ARGBHalf || format == RenderTextureFormat.RGB111110Float || format == RenderTextureFormat.RGFloat || format == RenderTextureFormat.ARGBFloat || format == RenderTextureFormat.ARGBFloat || format == RenderTextureFormat.RFloat || format == RenderTextureFormat.RGHalf || format == RenderTextureFormat.RHalf;
		}

		protected void OnRenderTextureGUI(RenderTextureEditor.GUIElements guiElements)
		{
			GUI.changed = false;
			bool flag = this.m_Dimension.intValue == 3;
			EditorGUILayout.IntPopup(this.m_Dimension, RenderTextureEditor.styles.dimensionStrings, RenderTextureEditor.styles.dimensionValues, RenderTextureEditor.styles.dimension, new GUILayoutOption[0]);
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			EditorGUILayout.PrefixLabel(RenderTextureEditor.styles.size, EditorStyles.popup);
			EditorGUILayout.DelayedIntField(this.m_Width, GUIContent.none, new GUILayoutOption[]
			{
				GUILayout.MinWidth(40f)
			});
			GUILayout.Label(RenderTextureEditor.styles.cross, new GUILayoutOption[0]);
			EditorGUILayout.DelayedIntField(this.m_Height, GUIContent.none, new GUILayoutOption[]
			{
				GUILayout.MinWidth(40f)
			});
			if (flag)
			{
				GUILayout.Label(RenderTextureEditor.styles.cross, new GUILayoutOption[0]);
				EditorGUILayout.DelayedIntField(this.m_Depth, GUIContent.none, new GUILayoutOption[]
				{
					GUILayout.MinWidth(40f)
				});
			}
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			if ((guiElements & RenderTextureEditor.GUIElements.RenderTargetAAGUI) != RenderTextureEditor.GUIElements.RenderTargetNoneGUI)
			{
				EditorGUILayout.IntPopup(this.m_AntiAliasing, RenderTextureEditor.styles.renderTextureAntiAliasing, RenderTextureEditor.styles.renderTextureAntiAliasingValues, RenderTextureEditor.styles.antiAliasing, new GUILayoutOption[0]);
			}
			EditorGUILayout.PropertyField(this.m_ColorFormat, RenderTextureEditor.styles.colorFormat, new GUILayoutOption[0]);
			if ((guiElements & RenderTextureEditor.GUIElements.RenderTargetDepthGUI) != RenderTextureEditor.GUIElements.RenderTargetNoneGUI)
			{
				EditorGUILayout.PropertyField(this.m_DepthFormat, RenderTextureEditor.styles.depthBuffer, new GUILayoutOption[0]);
			}
			bool disabled = RenderTextureEditor.IsHDRFormat((RenderTextureFormat)this.m_ColorFormat.intValue);
			using (new EditorGUI.DisabledScope(disabled))
			{
				EditorGUILayout.PropertyField(this.m_sRGB, RenderTextureEditor.styles.sRGBTexture, new GUILayoutOption[0]);
			}
			using (new EditorGUI.DisabledScope(flag))
			{
				EditorGUILayout.PropertyField(this.m_EnableMipmaps, RenderTextureEditor.styles.enableMipmaps, new GUILayoutOption[0]);
				using (new EditorGUI.DisabledScope(!this.m_EnableMipmaps.boolValue))
				{
					EditorGUILayout.PropertyField(this.m_AutoGeneratesMipmaps, RenderTextureEditor.styles.autoGeneratesMipmaps, new GUILayoutOption[0]);
				}
			}
			if (flag)
			{
				EditorGUILayout.HelpBox("3D RenderTextures do not support Mip Maps.", MessageType.Info);
			}
			EditorGUILayout.PropertyField(this.m_UseDynamicScale, RenderTextureEditor.styles.useDynamicScale, new GUILayoutOption[0]);
			RenderTexture renderTexture = base.target as RenderTexture;
			if (GUI.changed && renderTexture != null)
			{
				renderTexture.Release();
			}
			EditorGUILayout.Space();
			base.DoWrapModePopup();
			base.DoFilterModePopup();
			using (new EditorGUI.DisabledScope(this.RenderTextureHasDepth()))
			{
				base.DoAnisoLevelSlider();
			}
			if (this.RenderTextureHasDepth())
			{
				this.m_Aniso.intValue = 0;
				EditorGUILayout.HelpBox("RenderTextures with depth must have an Aniso Level of 0.", MessageType.Info);
			}
		}

		public override void OnInspectorGUI()
		{
			base.serializedObject.Update();
			this.OnRenderTextureGUI(RenderTextureEditor.GUIElements.RenderTargetDepthGUI | RenderTextureEditor.GUIElements.RenderTargetAAGUI);
			base.serializedObject.ApplyModifiedProperties();
		}

		private bool RenderTextureHasDepth()
		{
			return TextureUtil.IsDepthRTFormat((RenderTextureFormat)this.m_ColorFormat.enumValueIndex) || this.m_DepthFormat.enumValueIndex != 0;
		}

		public override string GetInfoString()
		{
			RenderTexture renderTexture = base.target as RenderTexture;
			string text = renderTexture.width + "x" + renderTexture.height;
			if (renderTexture.dimension == TextureDimension.Tex3D)
			{
				text = text + "x" + renderTexture.volumeDepth;
			}
			if (!renderTexture.isPowerOfTwo)
			{
				text += "(NPOT)";
			}
			if (QualitySettings.desiredColorSpace == ColorSpace.Linear)
			{
				bool flag = RenderTextureEditor.IsHDRFormat(renderTexture.format);
				bool flag2 = renderTexture.sRGB && !flag;
				text = text + " " + ((!flag2) ? "Linear" : "sRGB");
			}
			text = text + "  " + renderTexture.format;
			return text + "  " + EditorUtility.FormatBytes(TextureUtil.GetRuntimeMemorySizeLong(renderTexture));
		}
	}
}
