using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngineInternal;

namespace UnityEditor
{
	internal class LightingWindowBakeSettings
	{
		private static class Styles
		{
			public static readonly int[] LightmapDirectionalModeValues = new int[]
			{
				0,
				1
			};

			public static readonly GUIContent[] LightmapDirectionalModeStrings = new GUIContent[]
			{
				EditorGUIUtility.TrTextContent("Non-Directional", null, null),
				EditorGUIUtility.TrTextContent("Directional", null, null)
			};

			public static readonly int[] LightmapSizeValues = new int[]
			{
				32,
				64,
				128,
				256,
				512,
				1024,
				2048,
				4096
			};

			public static readonly GUIContent[] LightmapSizeStrings = Array.ConvertAll<int, GUIContent>(LightingWindowBakeSettings.Styles.LightmapSizeValues, (int x) => new GUIContent(x.ToString()));

			public static readonly int[] ConcurrentJobsTypeValues = new int[]
			{
				0,
				1,
				2
			};

			public static readonly GUIContent[] ConcurrentJobsTypeStrings = new GUIContent[]
			{
				EditorGUIUtility.TrTextContent("Min", null, null),
				EditorGUIUtility.TrTextContent("Low", null, null),
				EditorGUIUtility.TrTextContent("High", null, null)
			};

			public static readonly int[] MixedModeValues = new int[]
			{
				0,
				1,
				2
			};

			public static readonly GUIContent[] MixedModeStrings = new GUIContent[]
			{
				EditorGUIUtility.TrTextContent("Baked Indirect", null, null),
				EditorGUIUtility.TrTextContent("Subtractive", null, null),
				EditorGUIUtility.TrTextContent("Shadowmask", null, null)
			};

			public static readonly int[] BouncesValues = new int[]
			{
				0,
				1,
				2,
				3,
				4
			};

			public static readonly GUIContent[] BouncesStrings = new GUIContent[]
			{
				EditorGUIUtility.TrTextContent("None", null, null),
				EditorGUIUtility.TextContent("1"),
				EditorGUIUtility.TextContent("2"),
				EditorGUIUtility.TextContent("3"),
				EditorGUIUtility.TextContent("4")
			};

			public static readonly GUIContent[] HelpStringsMixed = new GUIContent[]
			{
				EditorGUIUtility.TrTextContent("Mixed lights provide realtime direct lighting while indirect light is baked into lightmaps and light probes.", null, null),
				EditorGUIUtility.TrTextContent("Mixed lights provide baked direct and indirect lighting for static objects. Dynamic objects receive realtime direct lighting and cast shadows on static objects using the main directional light in the scene.", null, null),
				EditorGUIUtility.TrTextContent("Mixed lights provide realtime direct lighting. Indirect lighting gets baked into lightmaps and light probes. Shadowmasks and light probes occlusion get generated for baked shadows. The Shadowmask Mode used at run time can be set in the Quality Settings panel.", null, null)
			};

			public static readonly GUIContent EnableBaked = EditorGUIUtility.TextContent("Baked Global Illumination|Controls whether Mixed and Baked lights will use baked Global Illumination. If enabled, Mixed lights are baked using the specified Lighting Mode and Baked lights will be completely baked and not adjustable at runtime.");

			public static readonly GUIContent BounceScale = EditorGUIUtility.TextContent("Bounce Scale|Multiplier for indirect lighting. Use with care.");

			public static readonly GUIContent UpdateThreshold = EditorGUIUtility.TextContent("Update Threshold|Threshold for updating realtime GI. A lower value causes more frequent updates (default 1.0).");

			public static readonly GUIContent AlbedoBoost = EditorGUIUtility.TextContent("Albedo Boost|Controls the amount of light bounced between surfaces by intensifying the albedo of materials in the scene. Increasing this draws the albedo value towards white for indirect light computation. The default value is physically accurate.");

			public static readonly GUIContent IndirectOutputScale = EditorGUIUtility.TextContent("Indirect Intensity|Controls the brightness of indirect light stored in realtime and baked lightmaps. A value above 1.0 will increase the intensity of indirect light while a value less than 1.0 will reduce indirect light intensity.");

			public static readonly GUIContent LightmapDirectionalMode = EditorGUIUtility.TextContent("Directional Mode|Controls whether baked and realtime lightmaps will store directional lighting information from the lighting environment. Options are Directional and Non-Directional.");

			public static readonly GUIContent DefaultLightmapParameters = EditorGUIUtility.TextContent("Lightmap Parameters|Allows the adjustment of advanced parameters that affect the process of generating a lightmap for an object using global illumination.");

			public static readonly GUIContent RealtimeLightsLabel = EditorGUIUtility.TextContent("Realtime Lighting|Precompute Realtime indirect lighting for realtime lights and static objects. In this mode realtime lights, ambient lighting, materials of static objects (including emission) will generate indirect lighting at runtime. Only static objects are blocking and bouncing light, dynamic objects receive indirect lighting via light probes.");

			public static readonly GUIContent MixedLightsLabel = EditorGUIUtility.TextContent("Mixed Lighting|Bake Global Illumination for mixed lights and static objects. May bake both direct and/or indirect lighting based on settings. Only static objects are blocking and bouncing light, dynamic objects receive baked lighting via light probes.");

			public static readonly GUIContent GeneralLightmapLabel = EditorGUIUtility.TextContent("Lightmapping Settings|Settings that apply to both Global Illumination modes (Precomputed Realtime and Baked).");

			public static readonly GUIContent NoRealtimeGIInSM2AndGLES2 = EditorGUIUtility.TextContent("Realtime Global Illumination is not supported on SM2.0 hardware nor when using GLES2.0.");

			public static readonly GUIContent ConcurrentJobs = EditorGUIUtility.TextContent("Concurrent Jobs|The amount of simultaneously scheduled jobs.");

			public static readonly GUIContent ForceWhiteAlbedo = EditorGUIUtility.TextContent("Force White Albedo|Force white albedo during lighting calculations.");

			public static readonly GUIContent ForceUpdates = EditorGUIUtility.TextContent("Force Updates|Force continuous updates of runtime indirect lighting calculations.");

			public static readonly GUIStyle LabelStyle = EditorStyles.wordWrappedMiniLabel;

			public static readonly GUIContent IndirectResolution = EditorGUIUtility.TrTextContent("Indirect Resolution", "Sets the resolution in texels that are used per unit for objects being lit by indirect lighting. The larger the value, the more significant the impact will be on the time it takes to bake the lighting.", null);

			public static readonly GUIContent LightmapResolution = EditorGUIUtility.TrTextContent("Lightmap Resolution", "Sets the resolution in texels that are used per unit for objects being lit by baked global illumination. Larger values will result in increased time to calculate the baked lighting.", null);

			public static readonly GUIContent Padding = EditorGUIUtility.TrTextContent("Lightmap Padding", "Sets the separation in texels between shapes in the baked lightmap.", null);

			public static readonly GUIContent LightmapSize = EditorGUIUtility.TrTextContent("Lightmap Size", "Sets the resolution of the full lightmap Texture in pixels. Values are squared, so a setting of 1024 will produce a 1024x1024 pixel sized lightmap.", null);

			public static readonly GUIContent TextureCompression = EditorGUIUtility.TrTextContent("Compress Lightmaps", "Controls whether the baked lightmap is compressed or not. When enabled, baked lightmaps are compressed to reduce required storage space but some artifacting may be present due to compression.", null);

			public static readonly GUIContent AmbientOcclusion = EditorGUIUtility.TrTextContent("Ambient Occlusion", "Specifies whether to include ambient occlusion or not in the baked lightmap result. Enabling this results in simulating the soft shadows that occur in cracks and crevices of objects when light is reflected onto them.", null);

			public static readonly GUIContent AmbientOcclusionContribution = EditorGUIUtility.TrTextContent("Indirect Contribution", "Adjusts the contrast of ambient occlusion applied to indirect lighting. The larger the value, the more contrast is applied to the ambient occlusion for indirect lighting.", null);

			public static readonly GUIContent AmbientOcclusionContributionDirect = EditorGUIUtility.TrTextContent("Direct Contribution", "Adjusts the contrast of ambient occlusion applied to the direct lighting. The larger the value is, the more contrast is applied to the ambient occlusion for direct lighting. This effect is not physically accurate.", null);

			public static readonly GUIContent AOMaxDistance = EditorGUIUtility.TrTextContent("Max Distance", "Controls how far rays are cast in order to determine if an object is occluded or not. A larger value produces longer rays and contributes more shadows to the lightmap, while a smaller value produces shorter rays that contribute shadows only when objects are very close to one another. A value of 0 casts an infinitely long ray that has no maximum distance.", null);

			public static readonly GUIContent FinalGather = EditorGUIUtility.TrTextContent("Final Gather", "Specifies whether the final light bounce of the global illumination calculation is calculated at the same resolution as the baked lightmap. When enabled, visual quality is improved at the cost of additional time required to bake the lighting.", null);

			public static readonly GUIContent FinalGatherRayCount = EditorGUIUtility.TrTextContent("Ray Count", "Controls the number of rays emitted for every final gather point.", null);

			public static readonly GUIContent FinalGatherFiltering = EditorGUIUtility.TrTextContent("Denoising", "Controls whether a denoising filter is applied to the final gather output.", null);

			public static readonly GUIContent SubtractiveShadowColor = EditorGUIUtility.TrTextContent("Realtime Shadow Color", "The color used for mixing realtime shadows with baked lightmaps in Subtractive lighting mode. The color defines the darkest point of the realtime shadow.", null);

			public static readonly GUIContent MixedLightMode = EditorGUIUtility.TrTextContent("Lighting Mode", "Specifies which Scene lighting mode will be used for all Mixed lights in the Scene. Options are Baked Indirect, Shadowmask and Subtractive.", null);

			public static readonly GUIContent UseRealtimeGI = EditorGUIUtility.TrTextContent("Realtime Global Illumination", "Controls whether Realtime lights in the Scene contribute indirect light. If enabled, Realtime lights contribute both direct and indirect light. If disabled, Realtime lights only contribute direct light. This can be disabled on a per-light basis in the light component Inspector by setting Indirect Multiplier to 0.", null);

			public static readonly GUIContent BakedGIDisabledInfo = EditorGUIUtility.TrTextContent("All Baked and Mixed lights in the Scene are currently being overridden to Realtime light modes. Enable Baked Global Illumination to allow the use of Baked and Mixed light modes.", null, null);

			public static readonly GUIContent BakeBackend = EditorGUIUtility.TrTextContent("Lightmapper", "Specifies which baking system will be used to generate baked lightmaps.", null);

			public static readonly GUIContent PVRDirectSampleCount = EditorGUIUtility.TrTextContent("Direct Samples", "Controls the number of samples the lightmapper will use for direct lighting calculations. Increasing this value may improve the quality of lightmaps but increases the time required for baking to complete.", null);

			public static readonly GUIContent PVRIndirectSampleCount = EditorGUIUtility.TrTextContent("Indirect Samples", "Controls the number of samples the lightmapper will use for indirect lighting calculations. Increasing this value may improve the quality of lightmaps but increases the time required for baking to complete.", null);

			public static readonly GUIContent PVRBounces = EditorGUIUtility.TrTextContent("Bounces", "Controls the maximum number of bounces the lightmapper will compute for indirect light.", null);

			public static readonly GUIContent PVRFilteringMode = EditorGUIUtility.TrTextContent("Filtering", "Specifies the method used to reduce noise in baked lightmaps. Options are None, Automatic, or Advanced.", null);

			public static readonly GUIContent PVRFiltering = EditorGUIUtility.TrTextContent("Filtering", "Specifies the filter kernel used to reduce the amount of noise in baked lightmaps.", null);

			public static readonly GUIContent PVRFilteringAdvanced = EditorGUIUtility.TrTextContent("Advanced Filter Settings", "Show advanced settings to configure filtering on lightmaps.", null);

			public static readonly GUIContent PVRFilterTypeDirect = EditorGUIUtility.TrTextContent("Direct Filter", "Specifies the filter kernel applied to the direct light stored in the lightmap. Gaussian will blur the lightmap with some loss of detail. A-Trous will reduce noise based on a threshold while maintaining edge detail.", null);

			public static readonly GUIContent PVRFilterTypeIndirect = EditorGUIUtility.TrTextContent("Indirect Filter", "Specifies the filter kernel applied to the indirect light stored in the lightmap. Gaussian will blur the lightmap with some loss of detail. A-Trous will reduce noise based on a threshold while maintaining edge detail.", null);

			public static readonly GUIContent PVRFilterTypeAO = EditorGUIUtility.TrTextContent("Ambient Occlusion Filter", "Specifies the filter kernel applied to the ambient occlusion stored in the lightmap. Gaussian will blur the lightmap with some loss of detail. A-Trous will reduce noise based on a threshold while maintaining edge detail.", null);

			public static readonly GUIContent PVRFilteringGaussRadiusDirect = EditorGUIUtility.TrTextContent("Direct Radius", "Controls the radius of the filter for direct light stored in the lightmap. A higher value will increase the strength of the blur, reducing noise from direct light in the lightmap.", null);

			public static readonly GUIContent PVRFilteringGaussRadiusIndirect = EditorGUIUtility.TrTextContent("Indirect Radius", "Controls the radius of the filter for indirect light stored in the lightmap. A higher value will increase the strength of the blur, reducing noise from indirect light in the lightmap.", null);

			public static readonly GUIContent PVRFilteringGaussRadiusAO = EditorGUIUtility.TrTextContent("Ambient Occlusion Radius", "The radius of the filter for ambient occlusion in the lightmap. A higher radius will increase the blur strength, reducing sampling noise from ambient occlusion in the lightmap.", null);

			public static readonly GUIContent PVRFilteringAtrousPositionSigmaDirect = EditorGUIUtility.TrTextContent("Direct Sigma", "Controls the threshold of the filter for direct light stored in the lightmap. A higher value will increase the threshold, reducing noise in the direct layer of the lightmap. Too high of a value can cause a loss of detail in the lightmap.", null);

			public static readonly GUIContent PVRFilteringAtrousPositionSigmaIndirect = EditorGUIUtility.TrTextContent("Indirect Sigma", "Controls the threshold of the filter for indirect light stored in the lightmap. A higher value will increase the threshold, reducing noise in the indirect layer of the lightmap. Too high of a value can cause a loss of detail in the lightmap.", null);

			public static readonly GUIContent PVRFilteringAtrousPositionSigmaAO = EditorGUIUtility.TrTextContent("Ambient Occlusion Sigma", "Controls the threshold of the filter for ambient occlusion stored in the lightmap. A higher value will increase the threshold, reducing noise in the ambient occlusion layer of the lightmap. Too high of a value can cause a loss of detail in the lightmap.", null);

			public static readonly GUIContent PVRCulling = EditorGUIUtility.TrTextContent("Prioritize View", "Specifies whether the lightmapper should prioritize baking texels within the scene view. When disabled, objects outside the scene view will have the same priority as those in the scene view.", null);
		}

		private bool m_ShowRealtimeLightsSettings = true;

		private bool m_ShowMixedLightsSettings = true;

		private bool m_ShowGeneralLightmapSettings = true;

		private const string kShowRealtimeLightsSettingsKey = "ShowRealtimeLightsSettings";

		private const string kShowMixedLightsSettingsKey = "ShowMixedLightsSettings";

		private const string kShowGeneralLightmapSettingsKey = "ShowGeneralLightmapSettings";

		private SerializedObject m_LightmapSettingsSO;

		private UnityEngine.Object m_LightmapSettings;

		private SerializedObject m_RenderSettingsSO;

		private SerializedProperty m_EnableRealtimeGI;

		private SerializedProperty m_Resolution;

		private SerializedProperty m_MixedBakeMode;

		private SerializedProperty m_EnabledBakedGI;

		private SerializedProperty m_AlbedoBoost;

		private SerializedProperty m_IndirectOutputScale;

		private SerializedProperty m_LightmapParameters;

		private SerializedProperty m_LightmapDirectionalMode;

		private SerializedProperty m_BakeResolution;

		private SerializedProperty m_Padding;

		private SerializedProperty m_AmbientOcclusion;

		private SerializedProperty m_AOMaxDistance;

		private SerializedProperty m_CompAOExponent;

		private SerializedProperty m_CompAOExponentDirect;

		private SerializedProperty m_TextureCompression;

		private SerializedProperty m_FinalGather;

		private SerializedProperty m_FinalGatherRayCount;

		private SerializedProperty m_FinalGatherFiltering;

		private SerializedProperty m_LightmapSize;

		private SerializedProperty m_SubtractiveShadowColor;

		private SerializedProperty m_BakeBackend;

		private SerializedProperty m_PVRSampleCount;

		private SerializedProperty m_PVRDirectSampleCount;

		private SerializedProperty m_PVRBounces;

		private SerializedProperty m_PVRCulling;

		private SerializedProperty m_PVRFilteringMode;

		private SerializedProperty m_PVRFilterTypeDirect;

		private SerializedProperty m_PVRFilterTypeIndirect;

		private SerializedProperty m_PVRFilterTypeAO;

		private SerializedProperty m_PVRFilteringGaussRadiusDirect;

		private SerializedProperty m_PVRFilteringGaussRadiusIndirect;

		private SerializedProperty m_PVRFilteringGaussRadiusAO;

		private SerializedProperty m_PVRFilteringAtrousPositionSigmaDirect;

		private SerializedProperty m_PVRFilteringAtrousPositionSigmaIndirect;

		private SerializedProperty m_PVRFilteringAtrousPositionSigmaAO;

		private SerializedProperty m_BounceScale;

		private SerializedProperty m_UpdateThreshold;

		private static bool PlayerHasSM20Support()
		{
			GraphicsDeviceType[] graphicsAPIs = PlayerSettings.GetGraphicsAPIs(EditorUserBuildSettings.activeBuildTarget);
			return graphicsAPIs.Contains(GraphicsDeviceType.OpenGLES2) || graphicsAPIs.Contains(GraphicsDeviceType.N3DS);
		}

		private void InitSettings()
		{
			UnityEngine.Object renderSettings = RenderSettings.GetRenderSettings();
			SerializedObject serializedObject = this.m_RenderSettingsSO = new SerializedObject(renderSettings);
			this.m_SubtractiveShadowColor = serializedObject.FindProperty("m_SubtractiveShadowColor");
			this.m_LightmapSettings = LightmapEditorSettings.GetLightmapSettings();
			SerializedObject serializedObject2 = this.m_LightmapSettingsSO = new SerializedObject(this.m_LightmapSettings);
			this.m_EnableRealtimeGI = serializedObject2.FindProperty("m_GISettings.m_EnableRealtimeLightmaps");
			this.m_Resolution = serializedObject2.FindProperty("m_LightmapEditorSettings.m_Resolution");
			this.m_MixedBakeMode = serializedObject2.FindProperty("m_LightmapEditorSettings.m_MixedBakeMode");
			this.m_EnabledBakedGI = serializedObject2.FindProperty("m_GISettings.m_EnableBakedLightmaps");
			this.m_AlbedoBoost = serializedObject2.FindProperty("m_GISettings.m_AlbedoBoost");
			this.m_IndirectOutputScale = serializedObject2.FindProperty("m_GISettings.m_IndirectOutputScale");
			this.m_LightmapParameters = serializedObject2.FindProperty("m_LightmapEditorSettings.m_LightmapParameters");
			this.m_LightmapDirectionalMode = serializedObject2.FindProperty("m_LightmapEditorSettings.m_LightmapsBakeMode");
			this.m_BakeResolution = serializedObject2.FindProperty("m_LightmapEditorSettings.m_BakeResolution");
			this.m_Padding = serializedObject2.FindProperty("m_LightmapEditorSettings.m_Padding");
			this.m_AmbientOcclusion = serializedObject2.FindProperty("m_LightmapEditorSettings.m_AO");
			this.m_AOMaxDistance = serializedObject2.FindProperty("m_LightmapEditorSettings.m_AOMaxDistance");
			this.m_CompAOExponent = serializedObject2.FindProperty("m_LightmapEditorSettings.m_CompAOExponent");
			this.m_CompAOExponentDirect = serializedObject2.FindProperty("m_LightmapEditorSettings.m_CompAOExponentDirect");
			this.m_TextureCompression = serializedObject2.FindProperty("m_LightmapEditorSettings.m_TextureCompression");
			this.m_FinalGather = serializedObject2.FindProperty("m_LightmapEditorSettings.m_FinalGather");
			this.m_FinalGatherRayCount = serializedObject2.FindProperty("m_LightmapEditorSettings.m_FinalGatherRayCount");
			this.m_FinalGatherFiltering = serializedObject2.FindProperty("m_LightmapEditorSettings.m_FinalGatherFiltering");
			this.m_LightmapSize = serializedObject2.FindProperty("m_LightmapEditorSettings.m_AtlasSize");
			this.m_BakeBackend = serializedObject2.FindProperty("m_LightmapEditorSettings.m_BakeBackend");
			this.m_PVRSampleCount = serializedObject2.FindProperty("m_LightmapEditorSettings.m_PVRSampleCount");
			this.m_PVRDirectSampleCount = serializedObject2.FindProperty("m_LightmapEditorSettings.m_PVRDirectSampleCount");
			this.m_PVRBounces = serializedObject2.FindProperty("m_LightmapEditorSettings.m_PVRBounces");
			this.m_PVRCulling = serializedObject2.FindProperty("m_LightmapEditorSettings.m_PVRCulling");
			this.m_PVRFilteringMode = serializedObject2.FindProperty("m_LightmapEditorSettings.m_PVRFilteringMode");
			this.m_PVRFilterTypeDirect = serializedObject2.FindProperty("m_LightmapEditorSettings.m_PVRFilterTypeDirect");
			this.m_PVRFilterTypeIndirect = serializedObject2.FindProperty("m_LightmapEditorSettings.m_PVRFilterTypeIndirect");
			this.m_PVRFilterTypeAO = serializedObject2.FindProperty("m_LightmapEditorSettings.m_PVRFilterTypeAO");
			this.m_PVRFilteringGaussRadiusDirect = serializedObject2.FindProperty("m_LightmapEditorSettings.m_PVRFilteringGaussRadiusDirect");
			this.m_PVRFilteringGaussRadiusIndirect = serializedObject2.FindProperty("m_LightmapEditorSettings.m_PVRFilteringGaussRadiusIndirect");
			this.m_PVRFilteringGaussRadiusAO = serializedObject2.FindProperty("m_LightmapEditorSettings.m_PVRFilteringGaussRadiusAO");
			this.m_PVRFilteringAtrousPositionSigmaDirect = serializedObject2.FindProperty("m_LightmapEditorSettings.m_PVRFilteringAtrousPositionSigmaDirect");
			this.m_PVRFilteringAtrousPositionSigmaIndirect = serializedObject2.FindProperty("m_LightmapEditorSettings.m_PVRFilteringAtrousPositionSigmaIndirect");
			this.m_PVRFilteringAtrousPositionSigmaAO = serializedObject2.FindProperty("m_LightmapEditorSettings.m_PVRFilteringAtrousPositionSigmaAO");
			this.m_BounceScale = serializedObject2.FindProperty("m_GISettings.m_BounceScale");
			this.m_UpdateThreshold = serializedObject2.FindProperty("m_GISettings.m_TemporalCoherenceThreshold");
		}

		public void OnEnable()
		{
			this.InitSettings();
			this.m_ShowGeneralLightmapSettings = SessionState.GetBool("ShowGeneralLightmapSettings", true);
			this.m_ShowRealtimeLightsSettings = SessionState.GetBool("ShowRealtimeLightsSettings", true);
			this.m_ShowMixedLightsSettings = SessionState.GetBool("ShowMixedLightsSettings", true);
		}

		public void OnDisable()
		{
			SessionState.SetBool("ShowGeneralLightmapSettings", this.m_ShowGeneralLightmapSettings);
			SessionState.SetBool("ShowRealtimeLightsSettings", this.m_ShowRealtimeLightsSettings);
			SessionState.SetBool("ShowMixedLightsSettings", this.m_ShowMixedLightsSettings);
			this.m_LightmapSettingsSO.Dispose();
			this.m_LightmapSettings = null;
			this.m_RenderSettingsSO.Dispose();
		}

		private void Repaint()
		{
			InspectorWindow.RepaintAllInspectors();
		}

		private static void DrawResolutionField(SerializedProperty resolution, GUIContent label)
		{
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			EditorGUILayout.PropertyField(resolution, label, new GUILayoutOption[0]);
			GUILayout.Label(" texels per unit", LightingWindowBakeSettings.Styles.LabelStyle, new GUILayoutOption[0]);
			GUILayout.EndHorizontal();
		}

		private static void DrawFilterSettingField(SerializedProperty gaussSetting, SerializedProperty atrousSetting, GUIContent gaussLabel, GUIContent atrousLabel, LightmapEditorSettings.FilterType type)
		{
			if (type != LightmapEditorSettings.FilterType.None)
			{
				GUILayout.BeginHorizontal(new GUILayoutOption[0]);
				if (type == LightmapEditorSettings.FilterType.Gaussian)
				{
					EditorGUILayout.IntSlider(gaussSetting, 0, 5, gaussLabel, new GUILayoutOption[0]);
					GUILayout.Label(" texels", LightingWindowBakeSettings.Styles.LabelStyle, new GUILayoutOption[0]);
				}
				else if (type == LightmapEditorSettings.FilterType.ATrous)
				{
					EditorGUILayout.Slider(atrousSetting, 0f, 2f, atrousLabel, new GUILayoutOption[0]);
					GUILayout.Label(" sigma", LightingWindowBakeSettings.Styles.LabelStyle, new GUILayoutOption[0]);
				}
				GUILayout.EndHorizontal();
			}
		}

		private static bool isBuiltIn(SerializedProperty prop)
		{
			bool result;
			if (prop.objectReferenceValue != null)
			{
				LightmapParameters lightmapParameters = prop.objectReferenceValue as LightmapParameters;
				result = (lightmapParameters.hideFlags == HideFlags.NotEditable);
			}
			else
			{
				result = true;
			}
			return result;
		}

		private static bool LightmapParametersGUI(SerializedProperty prop, GUIContent content)
		{
			EditorGUILayout.BeginHorizontal(new GUILayoutOption[0]);
			EditorGUIInternal.AssetPopup<LightmapParameters>(prop, content, "giparams", "Default-Medium");
			string text = "Edit...";
			if (LightingWindowBakeSettings.isBuiltIn(prop))
			{
				text = "View";
			}
			bool result = false;
			if (prop.objectReferenceValue == null)
			{
				using (new EditorGUI.DisabledScope(true))
				{
					if (GUILayout.Button(text, EditorStyles.miniButton, new GUILayoutOption[]
					{
						GUILayout.ExpandWidth(false)
					}))
					{
						Selection.activeObject = null;
						result = true;
					}
				}
			}
			else if (GUILayout.Button(text, EditorStyles.miniButton, new GUILayoutOption[]
			{
				GUILayout.ExpandWidth(false)
			}))
			{
				Selection.activeObject = prop.objectReferenceValue;
				result = true;
			}
			EditorGUILayout.EndHorizontal();
			return result;
		}

		private void RealtimeLightingGUI()
		{
			if (SupportedRenderingFeatures.IsLightmapBakeTypeSupported(LightmapBakeType.Realtime))
			{
				this.m_ShowRealtimeLightsSettings = EditorGUILayout.FoldoutTitlebar(this.m_ShowRealtimeLightsSettings, LightingWindowBakeSettings.Styles.RealtimeLightsLabel, true);
				if (this.m_ShowRealtimeLightsSettings)
				{
					EditorGUI.indentLevel++;
					EditorGUILayout.PropertyField(this.m_EnableRealtimeGI, LightingWindowBakeSettings.Styles.UseRealtimeGI, new GUILayoutOption[0]);
					if (this.m_EnableRealtimeGI.boolValue && LightingWindowBakeSettings.PlayerHasSM20Support())
					{
						EditorGUILayout.HelpBox(LightingWindowBakeSettings.Styles.NoRealtimeGIInSM2AndGLES2.text, MessageType.Warning);
					}
					EditorGUI.indentLevel--;
					EditorGUILayout.Space();
				}
			}
		}

		private void MixedLightingGUI()
		{
			if (SupportedRenderingFeatures.IsLightmapBakeTypeSupported(LightmapBakeType.Baked))
			{
				this.m_ShowMixedLightsSettings = EditorGUILayout.FoldoutTitlebar(this.m_ShowMixedLightsSettings, LightingWindowBakeSettings.Styles.MixedLightsLabel, true);
				if (this.m_ShowMixedLightsSettings)
				{
					EditorGUI.indentLevel++;
					EditorGUILayout.PropertyField(this.m_EnabledBakedGI, LightingWindowBakeSettings.Styles.EnableBaked, new GUILayoutOption[0]);
					if (!this.m_EnabledBakedGI.boolValue)
					{
						EditorGUILayout.HelpBox(LightingWindowBakeSettings.Styles.BakedGIDisabledInfo.text, MessageType.Info);
					}
					using (new EditorGUI.DisabledScope(!this.m_EnabledBakedGI.boolValue))
					{
						bool flag = SupportedRenderingFeatures.IsLightmapBakeTypeSupported(LightmapBakeType.Mixed);
						if (flag)
						{
							EditorGUILayout.IntPopup(this.m_MixedBakeMode, LightingWindowBakeSettings.Styles.MixedModeStrings, LightingWindowBakeSettings.Styles.MixedModeValues, LightingWindowBakeSettings.Styles.MixedLightMode, new GUILayoutOption[0]);
							if (!SupportedRenderingFeatures.IsMixedLightingModeSupported((MixedLightingMode)this.m_MixedBakeMode.intValue))
							{
								string text = LightingWindowBakeSettings.Styles.MixedModeStrings[(int)SupportedRenderingFeatures.FallbackMixedLightingMode()].text;
								EditorGUILayout.HelpBox("The Mixed Mode is not supported by the current render pipline. Fallback mode is " + text, MessageType.Warning);
							}
							else if (this.m_EnabledBakedGI.boolValue)
							{
								EditorGUILayout.HelpBox(LightingWindowBakeSettings.Styles.HelpStringsMixed[this.m_MixedBakeMode.intValue].text, MessageType.Info);
							}
							if (this.m_MixedBakeMode.intValue == 1)
							{
								EditorGUILayout.PropertyField(this.m_SubtractiveShadowColor, LightingWindowBakeSettings.Styles.SubtractiveShadowColor, new GUILayoutOption[0]);
								this.m_RenderSettingsSO.ApplyModifiedProperties();
								EditorGUILayout.Space();
							}
						}
					}
					EditorGUI.indentLevel--;
					EditorGUILayout.Space();
				}
			}
		}

		public void DeveloperBuildSettingsGUI()
		{
			if (Unsupported.IsDeveloperMode())
			{
				Lightmapping.concurrentJobsType = (Lightmapping.ConcurrentJobsType)EditorGUILayout.IntPopup(LightingWindowBakeSettings.Styles.ConcurrentJobs, (int)Lightmapping.concurrentJobsType, LightingWindowBakeSettings.Styles.ConcurrentJobsTypeStrings, LightingWindowBakeSettings.Styles.ConcurrentJobsTypeValues, new GUILayoutOption[0]);
				Lightmapping.enlightenForceUpdates = EditorGUILayout.Toggle(LightingWindowBakeSettings.Styles.ForceUpdates, Lightmapping.enlightenForceUpdates, new GUILayoutOption[0]);
				Lightmapping.enlightenForceWhiteAlbedo = EditorGUILayout.Toggle(LightingWindowBakeSettings.Styles.ForceWhiteAlbedo, Lightmapping.enlightenForceWhiteAlbedo, new GUILayoutOption[0]);
				Lightmapping.filterMode = (FilterMode)EditorGUILayout.EnumPopup(EditorGUIUtility.TempContent("Filter Mode"), Lightmapping.filterMode, new GUILayoutOption[0]);
				EditorGUILayout.Slider(this.m_BounceScale, 0f, 10f, LightingWindowBakeSettings.Styles.BounceScale, new GUILayoutOption[0]);
				EditorGUILayout.Slider(this.m_UpdateThreshold, 0f, 1f, LightingWindowBakeSettings.Styles.UpdateThreshold, new GUILayoutOption[0]);
				if (GUILayout.Button("Clear disk cache", new GUILayoutOption[]
				{
					GUILayout.Width(150f)
				}))
				{
					Lightmapping.Clear();
					Lightmapping.ClearDiskCache();
				}
				if (GUILayout.Button("Print state to console", new GUILayoutOption[]
				{
					GUILayout.Width(150f)
				}))
				{
					Lightmapping.PrintStateToConsole();
				}
				if (GUILayout.Button("Reset albedo/emissive", new GUILayoutOption[]
				{
					GUILayout.Width(150f)
				}))
				{
					GIDebugVisualisation.ResetRuntimeInputTextures();
				}
				if (GUILayout.Button("Reset environment", new GUILayoutOption[]
				{
					GUILayout.Width(150f)
				}))
				{
					DynamicGI.UpdateEnvironment();
				}
			}
		}

		private void GeneralLightmapSettingsGUI()
		{
			bool flag = SupportedRenderingFeatures.IsLightmapBakeTypeSupported(LightmapBakeType.Baked);
			if (flag || SupportedRenderingFeatures.IsLightmapBakeTypeSupported(LightmapBakeType.Realtime))
			{
				this.m_ShowGeneralLightmapSettings = EditorGUILayout.FoldoutTitlebar(this.m_ShowGeneralLightmapSettings, LightingWindowBakeSettings.Styles.GeneralLightmapLabel, true);
				if (this.m_ShowGeneralLightmapSettings)
				{
					EditorGUI.indentLevel++;
					using (new EditorGUI.DisabledScope(!this.m_EnabledBakedGI.boolValue && !this.m_EnableRealtimeGI.boolValue))
					{
						if (flag)
						{
							using (new EditorGUI.DisabledScope(!this.m_EnabledBakedGI.boolValue))
							{
								EditorGUI.BeginChangeCheck();
								EditorGUILayout.PropertyField(this.m_BakeBackend, LightingWindowBakeSettings.Styles.BakeBackend, new GUILayoutOption[0]);
								if (EditorGUI.EndChangeCheck())
								{
									InspectorWindow.RepaintAllInspectors();
								}
								if (LightmapEditorSettings.lightmapper == LightmapEditorSettings.Lightmapper.PathTracer)
								{
									EditorGUI.indentLevel++;
									EditorGUILayout.PropertyField(this.m_PVRCulling, LightingWindowBakeSettings.Styles.PVRCulling, new GUILayoutOption[0]);
									if (LightmapEditorSettings.sampling != LightmapEditorSettings.Sampling.Auto)
									{
										EditorGUILayout.PropertyField(this.m_PVRDirectSampleCount, LightingWindowBakeSettings.Styles.PVRDirectSampleCount, new GUILayoutOption[0]);
										EditorGUILayout.PropertyField(this.m_PVRSampleCount, LightingWindowBakeSettings.Styles.PVRIndirectSampleCount, new GUILayoutOption[0]);
										if (this.m_PVRSampleCount.intValue < 10 || this.m_PVRSampleCount.intValue > 100000)
										{
											this.m_PVRSampleCount.intValue = Math.Max(Math.Min(this.m_PVRSampleCount.intValue, 100000), 10);
										}
									}
									EditorGUILayout.IntPopup(this.m_PVRBounces, LightingWindowBakeSettings.Styles.BouncesStrings, LightingWindowBakeSettings.Styles.BouncesValues, LightingWindowBakeSettings.Styles.PVRBounces, new GUILayoutOption[0]);
									EditorGUILayout.PropertyField(this.m_PVRFilteringMode, LightingWindowBakeSettings.Styles.PVRFilteringMode, new GUILayoutOption[0]);
									if (this.m_PVRFilteringMode.enumValueIndex == 2)
									{
										EditorGUI.indentLevel++;
										EditorGUILayout.PropertyField(this.m_PVRFilterTypeDirect, LightingWindowBakeSettings.Styles.PVRFilterTypeDirect, new GUILayoutOption[0]);
										LightingWindowBakeSettings.DrawFilterSettingField(this.m_PVRFilteringGaussRadiusDirect, this.m_PVRFilteringAtrousPositionSigmaDirect, LightingWindowBakeSettings.Styles.PVRFilteringGaussRadiusDirect, LightingWindowBakeSettings.Styles.PVRFilteringAtrousPositionSigmaDirect, LightmapEditorSettings.filterTypeDirect);
										EditorGUILayout.Space();
										EditorGUILayout.PropertyField(this.m_PVRFilterTypeIndirect, LightingWindowBakeSettings.Styles.PVRFilterTypeIndirect, new GUILayoutOption[0]);
										LightingWindowBakeSettings.DrawFilterSettingField(this.m_PVRFilteringGaussRadiusIndirect, this.m_PVRFilteringAtrousPositionSigmaIndirect, LightingWindowBakeSettings.Styles.PVRFilteringGaussRadiusIndirect, LightingWindowBakeSettings.Styles.PVRFilteringAtrousPositionSigmaIndirect, LightmapEditorSettings.filterTypeIndirect);
										using (new EditorGUI.DisabledScope(!this.m_AmbientOcclusion.boolValue))
										{
											EditorGUILayout.Space();
											EditorGUILayout.PropertyField(this.m_PVRFilterTypeAO, LightingWindowBakeSettings.Styles.PVRFilterTypeAO, new GUILayoutOption[0]);
											LightingWindowBakeSettings.DrawFilterSettingField(this.m_PVRFilteringGaussRadiusAO, this.m_PVRFilteringAtrousPositionSigmaAO, LightingWindowBakeSettings.Styles.PVRFilteringGaussRadiusAO, LightingWindowBakeSettings.Styles.PVRFilteringAtrousPositionSigmaAO, LightmapEditorSettings.filterTypeAO);
										}
										EditorGUI.indentLevel--;
									}
									EditorGUI.indentLevel--;
									EditorGUILayout.Space();
								}
							}
						}
						using (new EditorGUI.DisabledScope(LightmapEditorSettings.lightmapper == LightmapEditorSettings.Lightmapper.PathTracer && !this.m_EnableRealtimeGI.boolValue))
						{
							LightingWindowBakeSettings.DrawResolutionField(this.m_Resolution, LightingWindowBakeSettings.Styles.IndirectResolution);
						}
						if (flag)
						{
							using (new EditorGUI.DisabledScope(!this.m_EnabledBakedGI.boolValue))
							{
								LightingWindowBakeSettings.DrawResolutionField(this.m_BakeResolution, LightingWindowBakeSettings.Styles.LightmapResolution);
								GUILayout.BeginHorizontal(new GUILayoutOption[0]);
								EditorGUILayout.PropertyField(this.m_Padding, LightingWindowBakeSettings.Styles.Padding, new GUILayoutOption[0]);
								GUILayout.Label(" texels", LightingWindowBakeSettings.Styles.LabelStyle, new GUILayoutOption[0]);
								GUILayout.EndHorizontal();
								EditorGUILayout.IntPopup(this.m_LightmapSize, LightingWindowBakeSettings.Styles.LightmapSizeStrings, LightingWindowBakeSettings.Styles.LightmapSizeValues, LightingWindowBakeSettings.Styles.LightmapSize, new GUILayoutOption[0]);
								EditorGUILayout.PropertyField(this.m_TextureCompression, LightingWindowBakeSettings.Styles.TextureCompression, new GUILayoutOption[0]);
								EditorGUILayout.PropertyField(this.m_AmbientOcclusion, LightingWindowBakeSettings.Styles.AmbientOcclusion, new GUILayoutOption[0]);
								if (this.m_AmbientOcclusion.boolValue)
								{
									EditorGUI.indentLevel++;
									EditorGUILayout.PropertyField(this.m_AOMaxDistance, LightingWindowBakeSettings.Styles.AOMaxDistance, new GUILayoutOption[0]);
									if (this.m_AOMaxDistance.floatValue < 0f)
									{
										this.m_AOMaxDistance.floatValue = 0f;
									}
									EditorGUILayout.Slider(this.m_CompAOExponent, 0f, 10f, LightingWindowBakeSettings.Styles.AmbientOcclusionContribution, new GUILayoutOption[0]);
									EditorGUILayout.Slider(this.m_CompAOExponentDirect, 0f, 10f, LightingWindowBakeSettings.Styles.AmbientOcclusionContributionDirect, new GUILayoutOption[0]);
									EditorGUI.indentLevel--;
								}
								if (LightmapEditorSettings.lightmapper == LightmapEditorSettings.Lightmapper.Radiosity)
								{
									EditorGUILayout.PropertyField(this.m_FinalGather, LightingWindowBakeSettings.Styles.FinalGather, new GUILayoutOption[0]);
									if (this.m_FinalGather.boolValue)
									{
										EditorGUI.indentLevel++;
										EditorGUILayout.PropertyField(this.m_FinalGatherRayCount, LightingWindowBakeSettings.Styles.FinalGatherRayCount, new GUILayoutOption[0]);
										EditorGUILayout.PropertyField(this.m_FinalGatherFiltering, LightingWindowBakeSettings.Styles.FinalGatherFiltering, new GUILayoutOption[0]);
										EditorGUI.indentLevel--;
									}
								}
							}
						}
						bool flag2 = SupportedRenderingFeatures.IsLightmapsModeSupported(LightmapsMode.CombinedDirectional);
						if (flag2 || this.m_LightmapDirectionalMode.intValue == 1)
						{
							EditorGUILayout.IntPopup(this.m_LightmapDirectionalMode, LightingWindowBakeSettings.Styles.LightmapDirectionalModeStrings, LightingWindowBakeSettings.Styles.LightmapDirectionalModeValues, LightingWindowBakeSettings.Styles.LightmapDirectionalMode, new GUILayoutOption[0]);
							if (!flag2)
							{
								EditorGUILayout.HelpBox("Directional Mode is not supported. Fallback will be Non-Directional.", MessageType.Warning);
							}
						}
						else
						{
							using (new EditorGUI.DisabledScope(true))
							{
								EditorGUILayout.IntPopup(LightingWindowBakeSettings.Styles.LightmapDirectionalMode, 0, LightingWindowBakeSettings.Styles.LightmapDirectionalModeStrings, LightingWindowBakeSettings.Styles.LightmapDirectionalModeValues, new GUILayoutOption[0]);
							}
						}
						EditorGUILayout.Slider(this.m_IndirectOutputScale, 0f, 5f, LightingWindowBakeSettings.Styles.IndirectOutputScale, new GUILayoutOption[0]);
						EditorGUILayout.Slider(this.m_AlbedoBoost, 1f, 10f, LightingWindowBakeSettings.Styles.AlbedoBoost, new GUILayoutOption[0]);
						if (LightingWindowBakeSettings.LightmapParametersGUI(this.m_LightmapParameters, LightingWindowBakeSettings.Styles.DefaultLightmapParameters))
						{
							EditorWindow.FocusWindowIfItsOpen<InspectorWindow>();
						}
						EditorGUI.indentLevel--;
						EditorGUILayout.Space();
					}
				}
			}
		}

		public void OnGUI()
		{
			if (this.m_LightmapSettings == null || this.m_LightmapSettings != LightmapEditorSettings.GetLightmapSettings())
			{
				this.InitSettings();
			}
			this.m_LightmapSettingsSO.UpdateIfRequiredOrScript();
			this.RealtimeLightingGUI();
			this.MixedLightingGUI();
			this.GeneralLightmapSettingsGUI();
			this.m_LightmapSettingsSO.ApplyModifiedProperties();
		}
	}
}
