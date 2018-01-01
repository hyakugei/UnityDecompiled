using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityEditor
{
	internal class StandardParticlesShaderGUI : ShaderGUI
	{
		public enum BlendMode
		{
			Opaque,
			Cutout,
			Fade,
			Transparent,
			Additive,
			Subtractive,
			Modulate
		}

		public enum FlipbookMode
		{
			Simple,
			Blended
		}

		public enum ColorMode
		{
			Multiply,
			Additive,
			Subtractive,
			Overlay,
			Color,
			Difference
		}

		private static class Styles
		{
			public static GUIContent albedoText = EditorGUIUtility.TrTextContent("Albedo", "Albedo (RGB) and Transparency (A).", null);

			public static GUIContent alphaCutoffText = EditorGUIUtility.TrTextContent("Alpha Cutoff", "Threshold for alpha cutoff.", null);

			public static GUIContent metallicMapText = EditorGUIUtility.TrTextContent("Metallic", "Metallic (R) and Smoothness (A).", null);

			public static GUIContent smoothnessText = EditorGUIUtility.TrTextContent("Smoothness", "Smoothness value.", null);

			public static GUIContent smoothnessScaleText = EditorGUIUtility.TrTextContent("Smoothness", "Smoothness scale factor.", null);

			public static GUIContent normalMapText = EditorGUIUtility.TrTextContent("Normal Map", "Normal Map.", null);

			public static GUIContent emissionText = EditorGUIUtility.TrTextContent("Color", "Emission (RGB).", null);

			public static GUIContent renderingMode = EditorGUIUtility.TrTextContent("Rendering Mode", "Determines the transparency and blending method for drawing the object to the screen.", null);

			public static GUIContent[] blendNames = Array.ConvertAll<string, GUIContent>(Enum.GetNames(typeof(StandardParticlesShaderGUI.BlendMode)), (string item) => new GUIContent(item));

			public static GUIContent colorMode = EditorGUIUtility.TrTextContent("Color Mode", "Determines the blending mode between the particle color and the texture albedo.", null);

			public static GUIContent[] colorNames = Array.ConvertAll<string, GUIContent>(Enum.GetNames(typeof(StandardParticlesShaderGUI.ColorMode)), (string item) => new GUIContent(item));

			public static GUIContent flipbookMode = EditorGUIUtility.TrTextContent("Flip-Book Mode", "Determines the blending mode used for animated texture sheets.", null);

			public static GUIContent[] flipbookNames = Array.ConvertAll<string, GUIContent>(Enum.GetNames(typeof(StandardParticlesShaderGUI.FlipbookMode)), (string item) => new GUIContent(item));

			public static GUIContent twoSidedEnabled = EditorGUIUtility.TrTextContent("Two Sided", "Render both front and back faces of the particle geometry.", null);

			public static GUIContent distortionEnabled = EditorGUIUtility.TrTextContent("Enable Distortion", "Use a grab pass and normal map to simulate refraction.", null);

			public static GUIContent distortionStrengthText = EditorGUIUtility.TrTextContent("Strength", "Distortion Strength.", null);

			public static GUIContent distortionBlendText = EditorGUIUtility.TrTextContent("Blend", "Weighting between albedo and grab pass.", null);

			public static GUIContent softParticlesEnabled = EditorGUIUtility.TrTextContent("Enable Soft Particles", "Fade out particle geometry when it gets close to the surface of objects written into the depth buffer.", null);

			public static GUIContent softParticlesNearFadeDistanceText = EditorGUIUtility.TrTextContent("Near fade", "Soft Particles near fade distance.", null);

			public static GUIContent softParticlesFarFadeDistanceText = EditorGUIUtility.TrTextContent("Far fade", "Soft Particles far fade distance.", null);

			public static GUIContent cameraFadingEnabled = EditorGUIUtility.TrTextContent("Enable Camera Fading", "Fade out particle geometry when it gets close to the camera.", null);

			public static GUIContent cameraNearFadeDistanceText = EditorGUIUtility.TrTextContent("Near fade", "Camera near fade distance.", null);

			public static GUIContent cameraFarFadeDistanceText = EditorGUIUtility.TrTextContent("Far fade", "Camera far fade distance.", null);

			public static GUIContent emissionEnabled = EditorGUIUtility.TrTextContent("Emission", null, null);

			public static string blendingOptionsText = "Blending Options";

			public static string mainOptionsText = "Main Options";

			public static string mapsOptionsText = "Maps";

			public static string requiredVertexStreamsText = "Required Vertex Streams";

			public static string streamPositionText = "Position (POSITION.xyz)";

			public static string streamNormalText = "Normal (NORMAL.xyz)";

			public static string streamColorText = "Color (COLOR.xyzw)";

			public static string streamUVText = "UV (TEXCOORD0.xy)";

			public static string streamUV2Text = "UV2 (TEXCOORD0.zw)";

			public static string streamAnimBlendText = "AnimBlend (TEXCOORD1.x)";

			public static string streamAnimFrameText = "AnimFrame (INSTANCED0.x)";

			public static string streamTangentText = "Tangent (TANGENT.xyzw)";

			public static GUIContent streamApplyToAllSystemsText = EditorGUIUtility.TrTextContent("Apply to Systems", "Apply the vertex stream layout to all Particle Systems using this material", null);
		}

		private MaterialProperty blendMode = null;

		private MaterialProperty colorMode = null;

		private MaterialProperty flipbookMode = null;

		private MaterialProperty cullMode = null;

		private MaterialProperty distortionEnabled = null;

		private MaterialProperty distortionStrength = null;

		private MaterialProperty distortionBlend = null;

		private MaterialProperty albedoMap = null;

		private MaterialProperty albedoColor = null;

		private MaterialProperty alphaCutoff = null;

		private MaterialProperty metallicMap = null;

		private MaterialProperty metallic = null;

		private MaterialProperty smoothness = null;

		private MaterialProperty bumpScale = null;

		private MaterialProperty bumpMap = null;

		private MaterialProperty emissionEnabled = null;

		private MaterialProperty emissionColorForRendering = null;

		private MaterialProperty emissionMap = null;

		private MaterialProperty softParticlesEnabled = null;

		private MaterialProperty cameraFadingEnabled = null;

		private MaterialProperty softParticlesNearFadeDistance = null;

		private MaterialProperty softParticlesFarFadeDistance = null;

		private MaterialProperty cameraNearFadeDistance = null;

		private MaterialProperty cameraFarFadeDistance = null;

		private MaterialEditor m_MaterialEditor;

		private List<ParticleSystemRenderer> m_RenderersUsingThisMaterial = new List<ParticleSystemRenderer>();

		private bool m_FirstTimeApply = true;

		public void FindProperties(MaterialProperty[] props)
		{
			this.blendMode = ShaderGUI.FindProperty("_Mode", props);
			this.colorMode = ShaderGUI.FindProperty("_ColorMode", props, false);
			this.flipbookMode = ShaderGUI.FindProperty("_FlipbookMode", props);
			this.cullMode = ShaderGUI.FindProperty("_Cull", props);
			this.distortionEnabled = ShaderGUI.FindProperty("_DistortionEnabled", props);
			this.distortionStrength = ShaderGUI.FindProperty("_DistortionStrength", props);
			this.distortionBlend = ShaderGUI.FindProperty("_DistortionBlend", props);
			this.albedoMap = ShaderGUI.FindProperty("_MainTex", props);
			this.albedoColor = ShaderGUI.FindProperty("_Color", props);
			this.alphaCutoff = ShaderGUI.FindProperty("_Cutoff", props);
			this.metallicMap = ShaderGUI.FindProperty("_MetallicGlossMap", props, false);
			this.metallic = ShaderGUI.FindProperty("_Metallic", props, false);
			this.smoothness = ShaderGUI.FindProperty("_Glossiness", props, false);
			this.bumpScale = ShaderGUI.FindProperty("_BumpScale", props);
			this.bumpMap = ShaderGUI.FindProperty("_BumpMap", props);
			this.emissionEnabled = ShaderGUI.FindProperty("_EmissionEnabled", props);
			this.emissionColorForRendering = ShaderGUI.FindProperty("_EmissionColor", props);
			this.emissionMap = ShaderGUI.FindProperty("_EmissionMap", props);
			this.softParticlesEnabled = ShaderGUI.FindProperty("_SoftParticlesEnabled", props);
			this.cameraFadingEnabled = ShaderGUI.FindProperty("_CameraFadingEnabled", props);
			this.softParticlesNearFadeDistance = ShaderGUI.FindProperty("_SoftParticlesNearFadeDistance", props);
			this.softParticlesFarFadeDistance = ShaderGUI.FindProperty("_SoftParticlesFarFadeDistance", props);
			this.cameraNearFadeDistance = ShaderGUI.FindProperty("_CameraNearFadeDistance", props);
			this.cameraFarFadeDistance = ShaderGUI.FindProperty("_CameraFarFadeDistance", props);
		}

		public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] props)
		{
			this.FindProperties(props);
			this.m_MaterialEditor = materialEditor;
			Material material = materialEditor.target as Material;
			if (this.m_FirstTimeApply)
			{
				this.MaterialChanged(material);
				this.CacheRenderersUsingThisMaterial(material);
				this.m_FirstTimeApply = false;
			}
			this.ShaderPropertiesGUI(material);
		}

		public void ShaderPropertiesGUI(Material material)
		{
			EditorGUIUtility.labelWidth = 0f;
			EditorGUI.BeginChangeCheck();
			GUILayout.Label(StandardParticlesShaderGUI.Styles.blendingOptionsText, EditorStyles.boldLabel, new GUILayoutOption[0]);
			this.BlendModePopup();
			this.ColorModePopup();
			EditorGUILayout.Space();
			GUILayout.Label(StandardParticlesShaderGUI.Styles.mainOptionsText, EditorStyles.boldLabel, new GUILayoutOption[0]);
			this.FlipbookModePopup();
			this.TwoSidedPopup(material);
			this.FadingPopup(material);
			this.DistortionPopup(material);
			EditorGUILayout.Space();
			GUILayout.Label(StandardParticlesShaderGUI.Styles.mapsOptionsText, EditorStyles.boldLabel, new GUILayoutOption[0]);
			this.DoAlbedoArea(material);
			this.DoSpecularMetallicArea(material);
			this.DoNormalMapArea(material);
			this.DoEmissionArea(material);
			if (!this.flipbookMode.hasMixedValue && (int)this.flipbookMode.floatValue != 1)
			{
				EditorGUI.BeginChangeCheck();
				this.m_MaterialEditor.TextureScaleOffsetProperty(this.albedoMap);
				if (EditorGUI.EndChangeCheck())
				{
					this.emissionMap.textureScaleAndOffset = this.albedoMap.textureScaleAndOffset;
				}
			}
			if (EditorGUI.EndChangeCheck())
			{
				UnityEngine.Object[] targets = this.blendMode.targets;
				for (int i = 0; i < targets.Length; i++)
				{
					UnityEngine.Object @object = targets[i];
					this.MaterialChanged((Material)@object);
				}
			}
			EditorGUILayout.Space();
			GUILayout.Label(StandardParticlesShaderGUI.Styles.requiredVertexStreamsText, EditorStyles.boldLabel, new GUILayoutOption[0]);
			this.DoVertexStreamsArea(material);
		}

		public override void OnClosed(Material material)
		{
			material.SetShaderPassEnabled("Always", true);
		}

		public override void AssignNewShaderToMaterial(Material material, Shader oldShader, Shader newShader)
		{
			if (newShader.name.Contains("Unlit"))
			{
				material.SetFloat("_LightingEnabled", 0f);
			}
			else
			{
				material.SetFloat("_LightingEnabled", 1f);
			}
			if (material.HasProperty("_Emission"))
			{
				material.SetColor("_EmissionColor", material.GetColor("_Emission"));
			}
			base.AssignNewShaderToMaterial(material, oldShader, newShader);
			if (oldShader == null || !oldShader.name.Contains("Legacy Shaders/"))
			{
				StandardParticlesShaderGUI.SetupMaterialWithBlendMode(material, (StandardParticlesShaderGUI.BlendMode)material.GetFloat("_Mode"));
			}
			else
			{
				StandardParticlesShaderGUI.BlendMode blendMode = StandardParticlesShaderGUI.BlendMode.Opaque;
				if (oldShader.name.Contains("/Transparent/Cutout/"))
				{
					blendMode = StandardParticlesShaderGUI.BlendMode.Cutout;
				}
				else if (oldShader.name.Contains("/Transparent/"))
				{
					blendMode = StandardParticlesShaderGUI.BlendMode.Fade;
				}
				material.SetFloat("_Mode", (float)blendMode);
				this.MaterialChanged(material);
			}
		}

		private void BlendModePopup()
		{
			EditorGUI.showMixedValue = this.blendMode.hasMixedValue;
			StandardParticlesShaderGUI.BlendMode blendMode = (StandardParticlesShaderGUI.BlendMode)this.blendMode.floatValue;
			EditorGUI.BeginChangeCheck();
			blendMode = (StandardParticlesShaderGUI.BlendMode)EditorGUILayout.Popup(StandardParticlesShaderGUI.Styles.renderingMode, (int)blendMode, StandardParticlesShaderGUI.Styles.blendNames, new GUILayoutOption[0]);
			if (EditorGUI.EndChangeCheck())
			{
				this.m_MaterialEditor.RegisterPropertyChangeUndo("Rendering Mode");
				this.blendMode.floatValue = (float)blendMode;
			}
			EditorGUI.showMixedValue = false;
		}

		private void ColorModePopup()
		{
			if (this.colorMode != null)
			{
				EditorGUI.showMixedValue = this.colorMode.hasMixedValue;
				StandardParticlesShaderGUI.ColorMode colorMode = (StandardParticlesShaderGUI.ColorMode)this.colorMode.floatValue;
				EditorGUI.BeginChangeCheck();
				colorMode = (StandardParticlesShaderGUI.ColorMode)EditorGUILayout.Popup(StandardParticlesShaderGUI.Styles.colorMode, (int)colorMode, StandardParticlesShaderGUI.Styles.colorNames, new GUILayoutOption[0]);
				if (EditorGUI.EndChangeCheck())
				{
					this.m_MaterialEditor.RegisterPropertyChangeUndo("Color Mode");
					this.colorMode.floatValue = (float)colorMode;
				}
				EditorGUI.showMixedValue = false;
			}
		}

		private void FlipbookModePopup()
		{
			EditorGUI.showMixedValue = this.flipbookMode.hasMixedValue;
			StandardParticlesShaderGUI.FlipbookMode flipbookMode = (StandardParticlesShaderGUI.FlipbookMode)this.flipbookMode.floatValue;
			EditorGUI.BeginChangeCheck();
			flipbookMode = (StandardParticlesShaderGUI.FlipbookMode)EditorGUILayout.Popup(StandardParticlesShaderGUI.Styles.flipbookMode, (int)flipbookMode, StandardParticlesShaderGUI.Styles.flipbookNames, new GUILayoutOption[0]);
			if (EditorGUI.EndChangeCheck())
			{
				this.m_MaterialEditor.RegisterPropertyChangeUndo("Flip-Book Mode");
				this.flipbookMode.floatValue = (float)flipbookMode;
			}
			EditorGUI.showMixedValue = false;
		}

		private void TwoSidedPopup(Material material)
		{
			EditorGUI.showMixedValue = this.cullMode.hasMixedValue;
			bool flag = this.cullMode.floatValue == 0f;
			EditorGUI.BeginChangeCheck();
			flag = EditorGUILayout.Toggle(StandardParticlesShaderGUI.Styles.twoSidedEnabled, flag, new GUILayoutOption[0]);
			if (EditorGUI.EndChangeCheck())
			{
				this.m_MaterialEditor.RegisterPropertyChangeUndo("Two Sided Enabled");
				this.cullMode.floatValue = ((!flag) ? 2f : 0f);
			}
			EditorGUI.showMixedValue = false;
		}

		private void FadingPopup(Material material)
		{
			if (material.GetInt("_ZWrite") == 0)
			{
				EditorGUI.showMixedValue = this.softParticlesEnabled.hasMixedValue;
				float num = this.softParticlesEnabled.floatValue;
				EditorGUI.BeginChangeCheck();
				num = ((!EditorGUILayout.Toggle(StandardParticlesShaderGUI.Styles.softParticlesEnabled, num != 0f, new GUILayoutOption[0])) ? 0f : 1f);
				if (EditorGUI.EndChangeCheck())
				{
					this.m_MaterialEditor.RegisterPropertyChangeUndo("Soft Particles Enabled");
					this.softParticlesEnabled.floatValue = num;
				}
				if (num != 0f)
				{
					int labelIndent = 2;
					this.m_MaterialEditor.ShaderProperty(this.softParticlesNearFadeDistance, StandardParticlesShaderGUI.Styles.softParticlesNearFadeDistanceText, labelIndent);
					this.m_MaterialEditor.ShaderProperty(this.softParticlesFarFadeDistance, StandardParticlesShaderGUI.Styles.softParticlesFarFadeDistanceText, labelIndent);
				}
				EditorGUI.showMixedValue = this.cameraFadingEnabled.hasMixedValue;
				float num2 = this.cameraFadingEnabled.floatValue;
				EditorGUI.BeginChangeCheck();
				num2 = ((!EditorGUILayout.Toggle(StandardParticlesShaderGUI.Styles.cameraFadingEnabled, num2 != 0f, new GUILayoutOption[0])) ? 0f : 1f);
				if (EditorGUI.EndChangeCheck())
				{
					this.m_MaterialEditor.RegisterPropertyChangeUndo("Camera Fading Enabled");
					this.cameraFadingEnabled.floatValue = num2;
				}
				if (num2 != 0f)
				{
					int labelIndent2 = 2;
					this.m_MaterialEditor.ShaderProperty(this.cameraNearFadeDistance, StandardParticlesShaderGUI.Styles.cameraNearFadeDistanceText, labelIndent2);
					this.m_MaterialEditor.ShaderProperty(this.cameraFarFadeDistance, StandardParticlesShaderGUI.Styles.cameraFarFadeDistanceText, labelIndent2);
				}
				EditorGUI.showMixedValue = false;
			}
		}

		private void DistortionPopup(Material material)
		{
			if (material.GetInt("_ZWrite") == 0)
			{
				EditorGUI.showMixedValue = this.distortionEnabled.hasMixedValue;
				bool flag = this.distortionEnabled.floatValue != 0f;
				EditorGUI.BeginChangeCheck();
				flag = EditorGUILayout.Toggle(StandardParticlesShaderGUI.Styles.distortionEnabled, flag, new GUILayoutOption[0]);
				if (EditorGUI.EndChangeCheck())
				{
					this.m_MaterialEditor.RegisterPropertyChangeUndo("Distortion Enabled");
					this.distortionEnabled.floatValue = ((!flag) ? 0f : 1f);
				}
				if (flag)
				{
					int labelIndent = 2;
					this.m_MaterialEditor.ShaderProperty(this.distortionStrength, StandardParticlesShaderGUI.Styles.distortionStrengthText, labelIndent);
					this.m_MaterialEditor.ShaderProperty(this.distortionBlend, StandardParticlesShaderGUI.Styles.distortionBlendText, labelIndent);
				}
				EditorGUI.showMixedValue = false;
			}
		}

		private void DoAlbedoArea(Material material)
		{
			this.m_MaterialEditor.TexturePropertyWithHDRColor(StandardParticlesShaderGUI.Styles.albedoText, this.albedoMap, this.albedoColor, true);
			if ((int)material.GetFloat("_Mode") == 1)
			{
				this.m_MaterialEditor.ShaderProperty(this.alphaCutoff, StandardParticlesShaderGUI.Styles.alphaCutoffText, 2);
			}
		}

		private void DoEmissionArea(Material material)
		{
			EditorGUI.showMixedValue = this.emissionEnabled.hasMixedValue;
			bool flag = this.emissionEnabled.floatValue != 0f;
			EditorGUI.BeginChangeCheck();
			flag = EditorGUILayout.Toggle(StandardParticlesShaderGUI.Styles.emissionEnabled, flag, new GUILayoutOption[0]);
			if (EditorGUI.EndChangeCheck())
			{
				this.m_MaterialEditor.RegisterPropertyChangeUndo("Emission Enabled");
				this.emissionEnabled.floatValue = ((!flag) ? 0f : 1f);
			}
			if (flag)
			{
				bool flag2 = this.emissionMap.textureValue != null;
				this.m_MaterialEditor.TexturePropertyWithHDRColor(StandardParticlesShaderGUI.Styles.emissionText, this.emissionMap, this.emissionColorForRendering, false);
				float maxColorComponent = this.emissionColorForRendering.colorValue.maxColorComponent;
				if (this.emissionMap.textureValue != null && !flag2 && maxColorComponent <= 0f)
				{
					this.emissionColorForRendering.colorValue = Color.white;
				}
			}
		}

		private void DoSpecularMetallicArea(Material material)
		{
			if (this.metallicMap != null)
			{
				bool flag = material.GetFloat("_LightingEnabled") > 0f;
				if (flag)
				{
					bool flag2 = this.metallicMap.textureValue != null;
					this.m_MaterialEditor.TexturePropertySingleLine(StandardParticlesShaderGUI.Styles.metallicMapText, this.metallicMap, (!flag2) ? this.metallic : null);
					int labelIndent = 2;
					bool flag3 = flag2;
					this.m_MaterialEditor.ShaderProperty(this.smoothness, (!flag3) ? StandardParticlesShaderGUI.Styles.smoothnessText : StandardParticlesShaderGUI.Styles.smoothnessScaleText, labelIndent);
				}
			}
		}

		private void DoNormalMapArea(Material material)
		{
			bool flag = material.GetInt("_ZWrite") != 0;
			bool flag2 = material.GetFloat("_LightingEnabled") > 0f;
			bool flag3 = material.GetFloat("_DistortionEnabled") > 0f && !flag;
			if (flag2 || flag3)
			{
				this.m_MaterialEditor.TexturePropertySingleLine(StandardParticlesShaderGUI.Styles.normalMapText, this.bumpMap, (!(this.bumpMap.textureValue != null)) ? null : this.bumpScale);
			}
		}

		private void DoVertexStreamsArea(Material material)
		{
			bool flag = material.GetFloat("_LightingEnabled") > 0f;
			bool flag2 = material.GetFloat("_FlipbookMode") > 0f;
			bool flag3 = material.GetTexture("_BumpMap") && flag;
			bool flag4 = ShaderUtil.HasProceduralInstancing(material.shader);
			GUILayout.Label(StandardParticlesShaderGUI.Styles.streamPositionText, EditorStyles.label, new GUILayoutOption[0]);
			if (flag)
			{
				GUILayout.Label(StandardParticlesShaderGUI.Styles.streamNormalText, EditorStyles.label, new GUILayoutOption[0]);
			}
			GUILayout.Label(StandardParticlesShaderGUI.Styles.streamColorText, EditorStyles.label, new GUILayoutOption[0]);
			GUILayout.Label(StandardParticlesShaderGUI.Styles.streamUVText, EditorStyles.label, new GUILayoutOption[0]);
			if (flag3)
			{
				GUILayout.Label(StandardParticlesShaderGUI.Styles.streamTangentText, EditorStyles.label, new GUILayoutOption[0]);
			}
			if (flag4)
			{
				GUILayout.Label(StandardParticlesShaderGUI.Styles.streamAnimFrameText, EditorStyles.label, new GUILayoutOption[0]);
			}
			else if (flag2 && !flag4)
			{
				GUILayout.Label(StandardParticlesShaderGUI.Styles.streamUV2Text, EditorStyles.label, new GUILayoutOption[0]);
				GUILayout.Label(StandardParticlesShaderGUI.Styles.streamAnimBlendText, EditorStyles.label, new GUILayoutOption[0]);
			}
			List<ParticleSystemVertexStream> list = new List<ParticleSystemVertexStream>();
			list.Add(ParticleSystemVertexStream.Position);
			if (flag)
			{
				list.Add(ParticleSystemVertexStream.Normal);
			}
			list.Add(ParticleSystemVertexStream.Color);
			list.Add(ParticleSystemVertexStream.UV);
			if (flag3)
			{
				list.Add(ParticleSystemVertexStream.Tangent);
			}
			List<ParticleSystemVertexStream> list2 = new List<ParticleSystemVertexStream>(list);
			if (flag4)
			{
				list2.Add(ParticleSystemVertexStream.AnimFrame);
			}
			if (flag2)
			{
				list.Add(ParticleSystemVertexStream.UV2);
				list.Add(ParticleSystemVertexStream.AnimBlend);
			}
			if (GUILayout.Button(StandardParticlesShaderGUI.Styles.streamApplyToAllSystemsText, EditorStyles.miniButton, new GUILayoutOption[]
			{
				GUILayout.ExpandWidth(false)
			}))
			{
				foreach (ParticleSystemRenderer current in this.m_RenderersUsingThisMaterial)
				{
					if (flag4 && current.renderMode == ParticleSystemRenderMode.Mesh && current.supportsMeshInstancing)
					{
						current.SetActiveVertexStreams(list2);
					}
					else
					{
						current.SetActiveVertexStreams(list);
					}
				}
			}
			string text = "";
			List<ParticleSystemVertexStream> list3 = new List<ParticleSystemVertexStream>();
			foreach (ParticleSystemRenderer current2 in this.m_RenderersUsingThisMaterial)
			{
				current2.GetActiveVertexStreams(list3);
				bool flag5;
				if (flag4 && current2.renderMode == ParticleSystemRenderMode.Mesh && current2.supportsMeshInstancing)
				{
					flag5 = list3.SequenceEqual(list2);
				}
				else
				{
					flag5 = list3.SequenceEqual(list);
				}
				if (!flag5)
				{
					text = text + "  " + current2.name + "\n";
				}
			}
			if (text != "")
			{
				EditorGUILayout.HelpBox("The following Particle System Renderers are using this material with incorrect Vertex Streams:\n" + text + "Use the Apply to Systems button to fix this", MessageType.Warning, true);
			}
			EditorGUILayout.Space();
		}

		public static void SetupMaterialWithBlendMode(Material material, StandardParticlesShaderGUI.BlendMode blendMode)
		{
			switch (blendMode)
			{
			case StandardParticlesShaderGUI.BlendMode.Opaque:
				material.SetOverrideTag("RenderType", "");
				material.SetInt("_BlendOp", 0);
				material.SetInt("_SrcBlend", 1);
				material.SetInt("_DstBlend", 0);
				material.SetInt("_ZWrite", 1);
				material.DisableKeyword("_ALPHATEST_ON");
				material.DisableKeyword("_ALPHABLEND_ON");
				material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
				material.DisableKeyword("_ALPHAMODULATE_ON");
				material.renderQueue = -1;
				break;
			case StandardParticlesShaderGUI.BlendMode.Cutout:
				material.SetOverrideTag("RenderType", "TransparentCutout");
				material.SetInt("_BlendOp", 0);
				material.SetInt("_SrcBlend", 1);
				material.SetInt("_DstBlend", 0);
				material.SetInt("_ZWrite", 1);
				material.EnableKeyword("_ALPHATEST_ON");
				material.DisableKeyword("_ALPHABLEND_ON");
				material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
				material.DisableKeyword("_ALPHAMODULATE_ON");
				material.renderQueue = 2450;
				break;
			case StandardParticlesShaderGUI.BlendMode.Fade:
				material.SetOverrideTag("RenderType", "Transparent");
				material.SetInt("_BlendOp", 0);
				material.SetInt("_SrcBlend", 5);
				material.SetInt("_DstBlend", 10);
				material.SetInt("_ZWrite", 0);
				material.DisableKeyword("_ALPHATEST_ON");
				material.EnableKeyword("_ALPHABLEND_ON");
				material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
				material.DisableKeyword("_ALPHAMODULATE_ON");
				material.renderQueue = 3000;
				break;
			case StandardParticlesShaderGUI.BlendMode.Transparent:
				material.SetOverrideTag("RenderType", "Transparent");
				material.SetInt("_BlendOp", 0);
				material.SetInt("_SrcBlend", 1);
				material.SetInt("_DstBlend", 10);
				material.SetInt("_ZWrite", 0);
				material.DisableKeyword("_ALPHATEST_ON");
				material.DisableKeyword("_ALPHABLEND_ON");
				material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
				material.DisableKeyword("_ALPHAMODULATE_ON");
				material.renderQueue = 3000;
				break;
			case StandardParticlesShaderGUI.BlendMode.Additive:
				material.SetOverrideTag("RenderType", "Transparent");
				material.SetInt("_BlendOp", 0);
				material.SetInt("_SrcBlend", 5);
				material.SetInt("_DstBlend", 1);
				material.SetInt("_ZWrite", 0);
				material.DisableKeyword("_ALPHATEST_ON");
				material.EnableKeyword("_ALPHABLEND_ON");
				material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
				material.DisableKeyword("_ALPHAMODULATE_ON");
				material.renderQueue = 3000;
				break;
			case StandardParticlesShaderGUI.BlendMode.Subtractive:
				material.SetOverrideTag("RenderType", "Transparent");
				material.SetInt("_BlendOp", 2);
				material.SetInt("_SrcBlend", 5);
				material.SetInt("_DstBlend", 1);
				material.SetInt("_ZWrite", 0);
				material.DisableKeyword("_ALPHATEST_ON");
				material.EnableKeyword("_ALPHABLEND_ON");
				material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
				material.DisableKeyword("_ALPHAMODULATE_ON");
				material.renderQueue = 3000;
				break;
			case StandardParticlesShaderGUI.BlendMode.Modulate:
				material.SetOverrideTag("RenderType", "Transparent");
				material.SetInt("_BlendOp", 0);
				material.SetInt("_SrcBlend", 2);
				material.SetInt("_DstBlend", 10);
				material.SetInt("_ZWrite", 0);
				material.DisableKeyword("_ALPHATEST_ON");
				material.DisableKeyword("_ALPHABLEND_ON");
				material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
				material.EnableKeyword("_ALPHAMODULATE_ON");
				material.renderQueue = 3000;
				break;
			}
		}

		public static void SetupMaterialWithColorMode(Material material, StandardParticlesShaderGUI.ColorMode colorMode)
		{
			switch (colorMode)
			{
			case StandardParticlesShaderGUI.ColorMode.Multiply:
				material.DisableKeyword("_COLOROVERLAY_ON");
				material.DisableKeyword("_COLORCOLOR_ON");
				material.DisableKeyword("_COLORADDSUBDIFF_ON");
				break;
			case StandardParticlesShaderGUI.ColorMode.Additive:
				material.DisableKeyword("_COLOROVERLAY_ON");
				material.DisableKeyword("_COLORCOLOR_ON");
				material.EnableKeyword("_COLORADDSUBDIFF_ON");
				material.SetVector("_ColorAddSubDiff", new Vector4(1f, 0f, 0f, 0f));
				break;
			case StandardParticlesShaderGUI.ColorMode.Subtractive:
				material.DisableKeyword("_COLOROVERLAY_ON");
				material.DisableKeyword("_COLORCOLOR_ON");
				material.EnableKeyword("_COLORADDSUBDIFF_ON");
				material.SetVector("_ColorAddSubDiff", new Vector4(-1f, 0f, 0f, 0f));
				break;
			case StandardParticlesShaderGUI.ColorMode.Overlay:
				material.DisableKeyword("_COLORCOLOR_ON");
				material.DisableKeyword("_COLORADDSUBDIFF_ON");
				material.EnableKeyword("_COLOROVERLAY_ON");
				break;
			case StandardParticlesShaderGUI.ColorMode.Color:
				material.DisableKeyword("_COLOROVERLAY_ON");
				material.DisableKeyword("_COLORADDSUBDIFF_ON");
				material.EnableKeyword("_COLORCOLOR_ON");
				break;
			case StandardParticlesShaderGUI.ColorMode.Difference:
				material.DisableKeyword("_COLOROVERLAY_ON");
				material.DisableKeyword("_COLORCOLOR_ON");
				material.EnableKeyword("_COLORADDSUBDIFF_ON");
				material.SetVector("_ColorAddSubDiff", new Vector4(-1f, 1f, 0f, 0f));
				break;
			}
		}

		private void SetMaterialKeywords(Material material)
		{
			bool flag = material.GetInt("_ZWrite") != 0;
			bool flag2 = material.GetFloat("_LightingEnabled") > 0f;
			bool flag3 = material.GetFloat("_DistortionEnabled") > 0f && !flag;
			StandardParticlesShaderGUI.SetKeyword(material, "_NORMALMAP", material.GetTexture("_BumpMap") && (flag2 || flag3));
			StandardParticlesShaderGUI.SetKeyword(material, "_METALLICGLOSSMAP", material.GetTexture("_MetallicGlossMap") != null && flag2);
			material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.None;
			StandardParticlesShaderGUI.SetKeyword(material, "_EMISSION", material.GetFloat("_EmissionEnabled") > 0f);
			bool state = material.GetFloat("_FlipbookMode") > 0f;
			StandardParticlesShaderGUI.SetKeyword(material, "_REQUIRE_UV2", state);
			bool flag4 = material.GetFloat("_SoftParticlesEnabled") > 0f;
			bool flag5 = material.GetFloat("_CameraFadingEnabled") > 0f;
			float num = material.GetFloat("_SoftParticlesNearFadeDistance");
			float num2 = material.GetFloat("_SoftParticlesFarFadeDistance");
			float num3 = material.GetFloat("_CameraNearFadeDistance");
			float num4 = material.GetFloat("_CameraFarFadeDistance");
			if (num < 0f)
			{
				num = 0f;
				material.SetFloat("_SoftParticlesNearFadeDistance", 0f);
			}
			if (num2 < 0f)
			{
				num2 = 0f;
				material.SetFloat("_SoftParticlesFarFadeDistance", 0f);
			}
			if (num3 < 0f)
			{
				num3 = 0f;
				material.SetFloat("_CameraNearFadeDistance", 0f);
			}
			if (num4 < 0f)
			{
				num4 = 0f;
				material.SetFloat("_CameraFarFadeDistance", 0f);
			}
			bool state2 = (flag4 || flag5) && !flag;
			StandardParticlesShaderGUI.SetKeyword(material, "_FADING_ON", state2);
			if (flag4)
			{
				material.SetVector("_SoftParticleFadeParams", new Vector4(num, 1f / (num2 - num), 0f, 0f));
			}
			else
			{
				material.SetVector("_SoftParticleFadeParams", new Vector4(0f, 0f, 0f, 0f));
			}
			if (flag5)
			{
				material.SetVector("_CameraFadeParams", new Vector4(num3, 1f / (num4 - num3), 0f, 0f));
			}
			else
			{
				material.SetVector("_CameraFadeParams", new Vector4(0f, float.PositiveInfinity, 0f, 0f));
			}
			StandardParticlesShaderGUI.SetKeyword(material, "EFFECT_BUMP", flag3);
			material.SetShaderPassEnabled("Always", flag3);
			if (flag3)
			{
				material.SetFloat("_DistortionStrengthScaled", material.GetFloat("_DistortionStrength") * 0.1f);
			}
		}

		private void MaterialChanged(Material material)
		{
			StandardParticlesShaderGUI.SetupMaterialWithBlendMode(material, (StandardParticlesShaderGUI.BlendMode)material.GetFloat("_Mode"));
			if (this.colorMode != null)
			{
				StandardParticlesShaderGUI.SetupMaterialWithColorMode(material, (StandardParticlesShaderGUI.ColorMode)material.GetFloat("_ColorMode"));
			}
			this.SetMaterialKeywords(material);
		}

		private void CacheRenderersUsingThisMaterial(Material material)
		{
			this.m_RenderersUsingThisMaterial.Clear();
			ParticleSystemRenderer[] array = UnityEngine.Object.FindObjectsOfType(typeof(ParticleSystemRenderer)) as ParticleSystemRenderer[];
			ParticleSystemRenderer[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				ParticleSystemRenderer particleSystemRenderer = array2[i];
				if (particleSystemRenderer.sharedMaterial == material)
				{
					this.m_RenderersUsingThisMaterial.Add(particleSystemRenderer);
				}
			}
		}

		private static void SetKeyword(Material m, string keyword, bool state)
		{
			if (state)
			{
				m.EnableKeyword(keyword);
			}
			else
			{
				m.DisableKeyword(keyword);
			}
		}
	}
}
