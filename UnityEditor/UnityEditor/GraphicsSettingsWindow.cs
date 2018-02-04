using System;
using System.Collections;
using UnityEditor.Build;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering;

namespace UnityEditor
{
	internal class GraphicsSettingsWindow
	{
		internal class BuiltinShaderSettings
		{
			internal enum BuiltinShaderMode
			{
				None,
				Builtin,
				Custom
			}

			private readonly SerializedProperty m_Mode;

			private readonly SerializedProperty m_Shader;

			private readonly GUIContent m_Label;

			internal BuiltinShaderSettings(string label, string name, SerializedObject serializedObject)
			{
				this.m_Mode = serializedObject.FindProperty(name + ".m_Mode");
				this.m_Shader = serializedObject.FindProperty(name + ".m_Shader");
				this.m_Label = EditorGUIUtility.TextContent(label);
			}

			internal void DoGUI()
			{
				EditorGUILayout.PropertyField(this.m_Mode, this.m_Label, new GUILayoutOption[0]);
				if (this.m_Mode.intValue == 2)
				{
					EditorGUILayout.PropertyField(this.m_Shader, new GUILayoutOption[0]);
				}
			}
		}

		internal class BuiltinShadersEditor : Editor
		{
			private GraphicsSettingsWindow.BuiltinShaderSettings m_Deferred;

			private GraphicsSettingsWindow.BuiltinShaderSettings m_DeferredReflections;

			private GraphicsSettingsWindow.BuiltinShaderSettings m_LegacyDeferred;

			private GraphicsSettingsWindow.BuiltinShaderSettings m_ScreenSpaceShadows;

			private GraphicsSettingsWindow.BuiltinShaderSettings m_DepthNormals;

			private GraphicsSettingsWindow.BuiltinShaderSettings m_MotionVectors;

			private GraphicsSettingsWindow.BuiltinShaderSettings m_LightHalo;

			private GraphicsSettingsWindow.BuiltinShaderSettings m_LensFlare;

			private string deferredString
			{
				get
				{
					return LocalizationDatabase.GetLocalizedString("Deferred|Shader used for Deferred Shading.");
				}
			}

			private string deferredReflString
			{
				get
				{
					return LocalizationDatabase.GetLocalizedString("Deferred Reflections|Shader used for Deferred reflection probes.");
				}
			}

			private string legacyDeferredString
			{
				get
				{
					return LocalizationDatabase.GetLocalizedString("Legacy Deferred|Shader used for Legacy (light prepass) Deferred Lighting.");
				}
			}

			private string screenShadowsString
			{
				get
				{
					return LocalizationDatabase.GetLocalizedString("Screen Space Shadows|Shader used for screen-space cascaded shadows.");
				}
			}

			private string depthNormalsString
			{
				get
				{
					return LocalizationDatabase.GetLocalizedString("Depth Normals|Shader used for depth and normals texture when enabled on a Camera.");
				}
			}

			private string motionVectorsString
			{
				get
				{
					return LocalizationDatabase.GetLocalizedString("Motion Vectors|Shader for generation of Motion Vectors when the rendering camera has renderMotionVectors set to true.");
				}
			}

			private string lightHaloString
			{
				get
				{
					return LocalizationDatabase.GetLocalizedString("Light Halo|Default Shader used for light halos.");
				}
			}

			private string lensFlareString
			{
				get
				{
					return LocalizationDatabase.GetLocalizedString("Lens Flare|Default Shader used for lens flares.");
				}
			}

			public void OnEnable()
			{
				this.m_Deferred = new GraphicsSettingsWindow.BuiltinShaderSettings(this.deferredString, "m_Deferred", base.serializedObject);
				this.m_DeferredReflections = new GraphicsSettingsWindow.BuiltinShaderSettings(this.deferredReflString, "m_DeferredReflections", base.serializedObject);
				this.m_LegacyDeferred = new GraphicsSettingsWindow.BuiltinShaderSettings(this.legacyDeferredString, "m_LegacyDeferred", base.serializedObject);
				this.m_ScreenSpaceShadows = new GraphicsSettingsWindow.BuiltinShaderSettings(this.screenShadowsString, "m_ScreenSpaceShadows", base.serializedObject);
				this.m_DepthNormals = new GraphicsSettingsWindow.BuiltinShaderSettings(this.depthNormalsString, "m_DepthNormals", base.serializedObject);
				this.m_MotionVectors = new GraphicsSettingsWindow.BuiltinShaderSettings(this.motionVectorsString, "m_MotionVectors", base.serializedObject);
				this.m_LightHalo = new GraphicsSettingsWindow.BuiltinShaderSettings(this.lightHaloString, "m_LightHalo", base.serializedObject);
				this.m_LensFlare = new GraphicsSettingsWindow.BuiltinShaderSettings(this.lensFlareString, "m_LensFlare", base.serializedObject);
			}

			public override void OnInspectorGUI()
			{
				base.serializedObject.Update();
				this.m_Deferred.DoGUI();
				EditorGUI.BeginChangeCheck();
				this.m_DeferredReflections.DoGUI();
				if (EditorGUI.EndChangeCheck())
				{
					ShaderUtil.ReloadAllShaders();
				}
				this.m_LegacyDeferred.DoGUI();
				this.m_ScreenSpaceShadows.DoGUI();
				this.m_DepthNormals.DoGUI();
				this.m_MotionVectors.DoGUI();
				this.m_LightHalo.DoGUI();
				this.m_LensFlare.DoGUI();
				base.serializedObject.ApplyModifiedProperties();
			}
		}

		internal class AlwaysIncludedShadersEditor : Editor
		{
			private SerializedProperty m_AlwaysIncludedShaders;

			public void OnEnable()
			{
				this.m_AlwaysIncludedShaders = base.serializedObject.FindProperty("m_AlwaysIncludedShaders");
				this.m_AlwaysIncludedShaders.isExpanded = true;
			}

			public override void OnInspectorGUI()
			{
				base.serializedObject.Update();
				EditorGUILayout.PropertyField(this.m_AlwaysIncludedShaders, true, new GUILayoutOption[0]);
				base.serializedObject.ApplyModifiedProperties();
			}
		}

		internal class ShaderStrippingEditor : Editor
		{
			internal class Styles
			{
				public static readonly GUIContent shaderSettings = EditorGUIUtility.TrTextContent("Platform shader settings", null, null);

				public static readonly GUIContent builtinSettings = EditorGUIUtility.TrTextContent("Built-in shader settings", null, null);

				public static readonly GUIContent shaderPreloadSettings = EditorGUIUtility.TrTextContent("Shader preloading", null, null);

				public static readonly GUIContent lightmapModes = EditorGUIUtility.TrTextContent("Lightmap Modes", null, null);

				public static readonly GUIContent lightmapPlain = EditorGUIUtility.TrTextContent("Baked Non-Directional", "Include support for baked non-directional lightmaps.", null);

				public static readonly GUIContent lightmapDirCombined = EditorGUIUtility.TrTextContent("Baked Directional", "Include support for baked directional lightmaps.", null);

				public static readonly GUIContent lightmapKeepShadowMask = EditorGUIUtility.TrTextContent("Baked Shadowmask", "Include support for baked shadow occlusion.", null);

				public static readonly GUIContent lightmapKeepSubtractive = EditorGUIUtility.TrTextContent("Baked Subtractive", "Include support for baked substractive lightmaps.", null);

				public static readonly GUIContent lightmapDynamicPlain = EditorGUIUtility.TrTextContent("Realtime Non-Directional", "Include support for realtime non-directional lightmaps.", null);

				public static readonly GUIContent lightmapDynamicDirCombined = EditorGUIUtility.TrTextContent("Realtime Directional", "Include support for realtime directional lightmaps.", null);

				public static readonly GUIContent lightmapFromScene = EditorGUIUtility.TrTextContent("Import From Current Scene", "Calculate lightmap modes used by the current scene.", null);

				public static readonly GUIContent fogModes = EditorGUIUtility.TrTextContent("Fog Modes", null, null);

				public static readonly GUIContent fogLinear = EditorGUIUtility.TrTextContent("Linear", "Include support for Linear fog.", null);

				public static readonly GUIContent fogExp = EditorGUIUtility.TrTextContent("Exponential", "Include support for Exponential fog.", null);

				public static readonly GUIContent fogExp2 = EditorGUIUtility.TrTextContent("Exponential Squared", "Include support for Exponential Squared fog.", null);

				public static readonly GUIContent fogFromScene = EditorGUIUtility.TrTextContent("Import From Current Scene", "Calculate fog modes used by the current scene.", null);

				public static readonly GUIContent instancingVariants = EditorGUIUtility.TrTextContent("Instancing Variants", null, null);

				public static readonly GUIContent shaderPreloadSave = EditorGUIUtility.TrTextContent("Save to asset...", "Save currently tracked shaders into a Shader Variant Manifest asset.", null);

				public static readonly GUIContent shaderPreloadClear = EditorGUIUtility.TrTextContent("Clear", "Clear currently tracked shader variant information.", null);
			}

			private SerializedProperty m_LightmapStripping;

			private SerializedProperty m_LightmapKeepPlain;

			private SerializedProperty m_LightmapKeepDirCombined;

			private SerializedProperty m_LightmapKeepDynamicPlain;

			private SerializedProperty m_LightmapKeepDynamicDirCombined;

			private SerializedProperty m_LightmapKeepShadowMask;

			private SerializedProperty m_LightmapKeepSubtractive;

			private SerializedProperty m_FogStripping;

			private SerializedProperty m_FogKeepLinear;

			private SerializedProperty m_FogKeepExp;

			private SerializedProperty m_FogKeepExp2;

			private SerializedProperty m_InstancingStripping;

			public void OnEnable()
			{
				this.m_LightmapStripping = base.serializedObject.FindProperty("m_LightmapStripping");
				this.m_LightmapKeepPlain = base.serializedObject.FindProperty("m_LightmapKeepPlain");
				this.m_LightmapKeepDirCombined = base.serializedObject.FindProperty("m_LightmapKeepDirCombined");
				this.m_LightmapKeepDynamicPlain = base.serializedObject.FindProperty("m_LightmapKeepDynamicPlain");
				this.m_LightmapKeepDynamicDirCombined = base.serializedObject.FindProperty("m_LightmapKeepDynamicDirCombined");
				this.m_LightmapKeepShadowMask = base.serializedObject.FindProperty("m_LightmapKeepShadowMask");
				this.m_LightmapKeepSubtractive = base.serializedObject.FindProperty("m_LightmapKeepSubtractive");
				this.m_FogStripping = base.serializedObject.FindProperty("m_FogStripping");
				this.m_FogKeepLinear = base.serializedObject.FindProperty("m_FogKeepLinear");
				this.m_FogKeepExp = base.serializedObject.FindProperty("m_FogKeepExp");
				this.m_FogKeepExp2 = base.serializedObject.FindProperty("m_FogKeepExp2");
				this.m_InstancingStripping = base.serializedObject.FindProperty("m_InstancingStripping");
			}

			public override void OnInspectorGUI()
			{
				base.serializedObject.Update();
				bool flag = false;
				bool flag2 = false;
				EditorGUILayout.PropertyField(this.m_LightmapStripping, GraphicsSettingsWindow.ShaderStrippingEditor.Styles.lightmapModes, new GUILayoutOption[0]);
				if (this.m_LightmapStripping.intValue != 0)
				{
					EditorGUI.indentLevel++;
					EditorGUILayout.PropertyField(this.m_LightmapKeepPlain, GraphicsSettingsWindow.ShaderStrippingEditor.Styles.lightmapPlain, new GUILayoutOption[0]);
					EditorGUILayout.PropertyField(this.m_LightmapKeepDirCombined, GraphicsSettingsWindow.ShaderStrippingEditor.Styles.lightmapDirCombined, new GUILayoutOption[0]);
					EditorGUILayout.PropertyField(this.m_LightmapKeepDynamicPlain, GraphicsSettingsWindow.ShaderStrippingEditor.Styles.lightmapDynamicPlain, new GUILayoutOption[0]);
					EditorGUILayout.PropertyField(this.m_LightmapKeepDynamicDirCombined, GraphicsSettingsWindow.ShaderStrippingEditor.Styles.lightmapDynamicDirCombined, new GUILayoutOption[0]);
					EditorGUILayout.PropertyField(this.m_LightmapKeepShadowMask, GraphicsSettingsWindow.ShaderStrippingEditor.Styles.lightmapKeepShadowMask, new GUILayoutOption[0]);
					EditorGUILayout.PropertyField(this.m_LightmapKeepSubtractive, GraphicsSettingsWindow.ShaderStrippingEditor.Styles.lightmapKeepSubtractive, new GUILayoutOption[0]);
					EditorGUILayout.Space();
					EditorGUILayout.BeginHorizontal(new GUILayoutOption[0]);
					EditorGUILayout.PrefixLabel(GUIContent.Temp(" "), EditorStyles.miniButton);
					if (GUILayout.Button(GraphicsSettingsWindow.ShaderStrippingEditor.Styles.lightmapFromScene, EditorStyles.miniButton, new GUILayoutOption[]
					{
						GUILayout.ExpandWidth(false)
					}))
					{
						flag = true;
					}
					EditorGUILayout.EndHorizontal();
					EditorGUI.indentLevel--;
					EditorGUILayout.Space();
				}
				EditorGUILayout.PropertyField(this.m_FogStripping, GraphicsSettingsWindow.ShaderStrippingEditor.Styles.fogModes, new GUILayoutOption[0]);
				if (this.m_FogStripping.intValue != 0)
				{
					EditorGUI.indentLevel++;
					EditorGUILayout.PropertyField(this.m_FogKeepLinear, GraphicsSettingsWindow.ShaderStrippingEditor.Styles.fogLinear, new GUILayoutOption[0]);
					EditorGUILayout.PropertyField(this.m_FogKeepExp, GraphicsSettingsWindow.ShaderStrippingEditor.Styles.fogExp, new GUILayoutOption[0]);
					EditorGUILayout.PropertyField(this.m_FogKeepExp2, GraphicsSettingsWindow.ShaderStrippingEditor.Styles.fogExp2, new GUILayoutOption[0]);
					EditorGUILayout.Space();
					EditorGUILayout.BeginHorizontal(new GUILayoutOption[0]);
					EditorGUILayout.PrefixLabel(GUIContent.Temp(" "), EditorStyles.miniButton);
					if (GUILayout.Button(GraphicsSettingsWindow.ShaderStrippingEditor.Styles.fogFromScene, EditorStyles.miniButton, new GUILayoutOption[]
					{
						GUILayout.ExpandWidth(false)
					}))
					{
						flag2 = true;
					}
					EditorGUILayout.EndHorizontal();
					EditorGUI.indentLevel--;
					EditorGUILayout.Space();
				}
				EditorGUILayout.PropertyField(this.m_InstancingStripping, GraphicsSettingsWindow.ShaderStrippingEditor.Styles.instancingVariants, new GUILayoutOption[0]);
				base.serializedObject.ApplyModifiedProperties();
				if (flag)
				{
					ShaderUtil.CalculateLightmapStrippingFromCurrentScene();
				}
				if (flag2)
				{
					ShaderUtil.CalculateFogStrippingFromCurrentScene();
				}
			}
		}

		internal class ShaderPreloadEditor : Editor
		{
			internal class Styles
			{
				public static readonly GUIContent shaderPreloadSave = EditorGUIUtility.TrTextContent("Save to asset...", "Save currently tracked shaders into a Shader Variant Manifest asset.", null);

				public static readonly GUIContent shaderPreloadClear = EditorGUIUtility.TrTextContent("Clear", "Clear currently tracked shader variant information.", null);
			}

			private SerializedProperty m_PreloadedShaders;

			public void OnEnable()
			{
				this.m_PreloadedShaders = base.serializedObject.FindProperty("m_PreloadedShaders");
				this.m_PreloadedShaders.isExpanded = true;
			}

			public override void OnInspectorGUI()
			{
				base.serializedObject.Update();
				base.serializedObject.ApplyModifiedProperties();
				EditorGUILayout.PropertyField(this.m_PreloadedShaders, true, new GUILayoutOption[0]);
				EditorGUILayout.Space();
				GUILayout.Label(string.Format("Currently tracked: {0} shaders {1} total variants", ShaderUtil.GetCurrentShaderVariantCollectionShaderCount(), ShaderUtil.GetCurrentShaderVariantCollectionVariantCount()), new GUILayoutOption[0]);
				EditorGUILayout.BeginHorizontal(new GUILayoutOption[0]);
				GUILayout.FlexibleSpace();
				if (GUILayout.Button(GraphicsSettingsWindow.ShaderPreloadEditor.Styles.shaderPreloadSave, EditorStyles.miniButton, new GUILayoutOption[0]))
				{
					string message = "Save shader variant collection";
					string text = EditorUtility.SaveFilePanelInProject("Save Shader Variant Collection", "NewShaderVariants", "shadervariants", message, ProjectWindowUtil.GetActiveFolderPath());
					if (!string.IsNullOrEmpty(text))
					{
						ShaderUtil.SaveCurrentShaderVariantCollection(text);
					}
					GUIUtility.ExitGUI();
				}
				if (GUILayout.Button(GraphicsSettingsWindow.ShaderPreloadEditor.Styles.shaderPreloadClear, EditorStyles.miniButton, new GUILayoutOption[0]))
				{
					ShaderUtil.ClearCurrentShaderVariantCollection();
				}
				EditorGUILayout.EndHorizontal();
				base.serializedObject.ApplyModifiedProperties();
			}
		}

		internal class TierSettingsEditor : Editor
		{
			internal class Styles
			{
				public static readonly GUIContent[] shaderQualityName = new GUIContent[]
				{
					EditorGUIUtility.TrTextContent("Low", null, null),
					EditorGUIUtility.TrTextContent("Medium", null, null),
					EditorGUIUtility.TrTextContent("High", null, null)
				};

				public static readonly int[] shaderQualityValue = new int[]
				{
					0,
					1,
					2
				};

				public static readonly GUIContent[] renderingPathName = new GUIContent[]
				{
					EditorGUIUtility.TrTextContent("Forward", null, null),
					EditorGUIUtility.TrTextContent("Deferred", null, null),
					EditorGUIUtility.TrTextContent("Legacy Vertex Lit", null, null),
					EditorGUIUtility.TrTextContent("Legacy Deferred (light prepass)", null, null)
				};

				public static readonly int[] renderingPathValue = new int[]
				{
					1,
					3,
					0,
					2
				};

				public static readonly GUIContent[] hdrModeName = new GUIContent[]
				{
					EditorGUIUtility.TrTextContent("FP16", null, null),
					EditorGUIUtility.TrTextContent("R11G11B10", null, null)
				};

				public static readonly int[] hdrModeValue = new int[]
				{
					1,
					2
				};

				public static readonly GUIContent[] realtimeGICPUUsageName = new GUIContent[]
				{
					EditorGUIUtility.TrTextContent("Low", null, null),
					EditorGUIUtility.TrTextContent("Medium", null, null),
					EditorGUIUtility.TrTextContent("High", null, null),
					EditorGUIUtility.TrTextContent("Unlimited", null, null)
				};

				public static readonly int[] realtimeGICPUUsageValue = new int[]
				{
					25,
					50,
					75,
					100
				};

				public static readonly GUIContent[] tierName = new GUIContent[]
				{
					EditorGUIUtility.TrTextContent("Low (Tier1)", null, null),
					EditorGUIUtility.TrTextContent("Medium (Tier 2)", null, null),
					EditorGUIUtility.TrTextContent("High (Tier 3)", null, null)
				};

				public static readonly GUIContent empty = EditorGUIUtility.TextContent("");

				public static readonly GUIContent autoSettings = EditorGUIUtility.TrTextContent("Use Defaults", null, null);

				public static readonly GUIContent standardShaderSettings = EditorGUIUtility.TrTextContent("Standard Shader", null, null);

				public static readonly GUIContent renderingSettings = EditorGUIUtility.TrTextContent("Rendering", null, null);

				public static readonly GUIContent standardShaderQuality = EditorGUIUtility.TrTextContent("Standard Shader Quality", null, null);

				public static readonly GUIContent reflectionProbeBoxProjection = EditorGUIUtility.TrTextContent("Reflection Probes Box Projection", null, null);

				public static readonly GUIContent reflectionProbeBlending = EditorGUIUtility.TrTextContent("Reflection Probes Blending", null, null);

				public static readonly GUIContent detailNormalMap = EditorGUIUtility.TrTextContent("Detail Normal Map", null, null);

				public static readonly GUIContent cascadedShadowMaps = EditorGUIUtility.TrTextContent("Cascaded Shadows", null, null);

				public static readonly GUIContent prefer32BitShadowMaps = EditorGUIUtility.TrTextContent("Prefer 32 bit shadow maps", null, null);

				public static readonly GUIContent semitransparentShadows = EditorGUIUtility.TrTextContent("Enable Semitransparent Shadows", null, null);

				public static readonly GUIContent enableLPPV = EditorGUIUtility.TrTextContent("Enable Light Probe Proxy Volume", null, null);

				public static readonly GUIContent renderingPath = EditorGUIUtility.TrTextContent("Rendering Path", null, null);

				public static readonly GUIContent useHDR = EditorGUIUtility.TrTextContent("Use HDR", null, null);

				public static readonly GUIContent hdrMode = EditorGUIUtility.TrTextContent("HDR Mode", null, null);

				public static readonly GUIContent realtimeGICPUUsage = EditorGUIUtility.TrTextContent("Realtime Global Illumination CPU Usage", "How many CPU worker threads to create for Realtime Global Illumination lighting calculations in the Player. Increasing this makes the system react faster to changes in lighting at a cost of using more CPU time. The higher the CPU Usage value, the more worker threads are created for solving Realtime GI.", null);
			}

			public bool verticalLayout = false;

			internal void OnFieldLabelsGUI(bool vertical)
			{
				if (!vertical)
				{
					EditorGUILayout.LabelField(GraphicsSettingsWindow.TierSettingsEditor.Styles.standardShaderSettings, EditorStyles.boldLabel, new GUILayoutOption[0]);
				}
				bool flag = GraphicsSettings.renderPipelineAsset != null;
				if (!flag)
				{
					EditorGUILayout.LabelField(GraphicsSettingsWindow.TierSettingsEditor.Styles.standardShaderQuality, new GUILayoutOption[0]);
					EditorGUILayout.LabelField(GraphicsSettingsWindow.TierSettingsEditor.Styles.reflectionProbeBoxProjection, new GUILayoutOption[0]);
					EditorGUILayout.LabelField(GraphicsSettingsWindow.TierSettingsEditor.Styles.reflectionProbeBlending, new GUILayoutOption[0]);
					EditorGUILayout.LabelField(GraphicsSettingsWindow.TierSettingsEditor.Styles.detailNormalMap, new GUILayoutOption[0]);
					EditorGUILayout.LabelField(GraphicsSettingsWindow.TierSettingsEditor.Styles.semitransparentShadows, new GUILayoutOption[0]);
				}
				EditorGUILayout.LabelField(GraphicsSettingsWindow.TierSettingsEditor.Styles.enableLPPV, new GUILayoutOption[0]);
				if (!vertical)
				{
					EditorGUILayout.LabelField(GraphicsSettingsWindow.TierSettingsEditor.Styles.empty, EditorStyles.boldLabel, new GUILayoutOption[0]);
					EditorGUILayout.LabelField(GraphicsSettingsWindow.TierSettingsEditor.Styles.renderingSettings, EditorStyles.boldLabel, new GUILayoutOption[0]);
				}
				if (!flag)
				{
					EditorGUILayout.LabelField(GraphicsSettingsWindow.TierSettingsEditor.Styles.cascadedShadowMaps, new GUILayoutOption[0]);
					EditorGUILayout.LabelField(GraphicsSettingsWindow.TierSettingsEditor.Styles.prefer32BitShadowMaps, new GUILayoutOption[0]);
					EditorGUILayout.LabelField(GraphicsSettingsWindow.TierSettingsEditor.Styles.useHDR, new GUILayoutOption[0]);
					EditorGUILayout.LabelField(GraphicsSettingsWindow.TierSettingsEditor.Styles.hdrMode, new GUILayoutOption[0]);
					EditorGUILayout.LabelField(GraphicsSettingsWindow.TierSettingsEditor.Styles.renderingPath, new GUILayoutOption[0]);
				}
				EditorGUILayout.LabelField(GraphicsSettingsWindow.TierSettingsEditor.Styles.realtimeGICPUUsage, new GUILayoutOption[0]);
			}

			internal ShaderQuality ShaderQualityPopup(ShaderQuality sq)
			{
				return (ShaderQuality)EditorGUILayout.IntPopup((int)sq, GraphicsSettingsWindow.TierSettingsEditor.Styles.shaderQualityName, GraphicsSettingsWindow.TierSettingsEditor.Styles.shaderQualityValue, new GUILayoutOption[0]);
			}

			internal RenderingPath RenderingPathPopup(RenderingPath rp)
			{
				return (RenderingPath)EditorGUILayout.IntPopup((int)rp, GraphicsSettingsWindow.TierSettingsEditor.Styles.renderingPathName, GraphicsSettingsWindow.TierSettingsEditor.Styles.renderingPathValue, new GUILayoutOption[0]);
			}

			internal CameraHDRMode HDRModePopup(CameraHDRMode mode)
			{
				return (CameraHDRMode)EditorGUILayout.IntPopup((int)mode, GraphicsSettingsWindow.TierSettingsEditor.Styles.hdrModeName, GraphicsSettingsWindow.TierSettingsEditor.Styles.hdrModeValue, new GUILayoutOption[0]);
			}

			internal RealtimeGICPUUsage RealtimeGICPUUsagePopup(RealtimeGICPUUsage usage)
			{
				return (RealtimeGICPUUsage)EditorGUILayout.IntPopup((int)usage, GraphicsSettingsWindow.TierSettingsEditor.Styles.realtimeGICPUUsageName, GraphicsSettingsWindow.TierSettingsEditor.Styles.realtimeGICPUUsageValue, new GUILayoutOption[0]);
			}

			internal void OnTierGUI(BuildTargetGroup platform, GraphicsTier tier, bool vertical)
			{
				TierSettings tierSettings = EditorGraphicsSettings.GetTierSettings(platform, tier);
				EditorGUI.BeginChangeCheck();
				if (!vertical)
				{
					EditorGUILayout.LabelField(GraphicsSettingsWindow.TierSettingsEditor.Styles.empty, EditorStyles.boldLabel, new GUILayoutOption[0]);
				}
				bool flag = GraphicsSettings.renderPipelineAsset != null;
				if (!flag)
				{
					tierSettings.standardShaderQuality = this.ShaderQualityPopup(tierSettings.standardShaderQuality);
					tierSettings.reflectionProbeBoxProjection = EditorGUILayout.Toggle(tierSettings.reflectionProbeBoxProjection, new GUILayoutOption[0]);
					tierSettings.reflectionProbeBlending = EditorGUILayout.Toggle(tierSettings.reflectionProbeBlending, new GUILayoutOption[0]);
					tierSettings.detailNormalMap = EditorGUILayout.Toggle(tierSettings.detailNormalMap, new GUILayoutOption[0]);
					tierSettings.semitransparentShadows = EditorGUILayout.Toggle(tierSettings.semitransparentShadows, new GUILayoutOption[0]);
				}
				tierSettings.enableLPPV = EditorGUILayout.Toggle(tierSettings.enableLPPV, new GUILayoutOption[0]);
				if (!vertical)
				{
					EditorGUILayout.LabelField(GraphicsSettingsWindow.TierSettingsEditor.Styles.empty, EditorStyles.boldLabel, new GUILayoutOption[0]);
					EditorGUILayout.LabelField(GraphicsSettingsWindow.TierSettingsEditor.Styles.empty, EditorStyles.boldLabel, new GUILayoutOption[0]);
				}
				if (!flag)
				{
					tierSettings.cascadedShadowMaps = EditorGUILayout.Toggle(tierSettings.cascadedShadowMaps, new GUILayoutOption[0]);
					tierSettings.prefer32BitShadowMaps = EditorGUILayout.Toggle(tierSettings.prefer32BitShadowMaps, new GUILayoutOption[0]);
					tierSettings.hdr = EditorGUILayout.Toggle(tierSettings.hdr, new GUILayoutOption[0]);
					tierSettings.hdrMode = this.HDRModePopup(tierSettings.hdrMode);
					tierSettings.renderingPath = this.RenderingPathPopup(tierSettings.renderingPath);
				}
				tierSettings.realtimeGICPUUsage = this.RealtimeGICPUUsagePopup(tierSettings.realtimeGICPUUsage);
				if (EditorGUI.EndChangeCheck())
				{
					EditorGraphicsSettings.RegisterUndoForGraphicsSettings();
					EditorGraphicsSettings.SetTierSettings(platform, tier, tierSettings);
				}
			}

			internal void OnGuiHorizontal(BuildTargetGroup platform)
			{
				EditorGUILayout.BeginHorizontal(new GUILayoutOption[0]);
				EditorGUILayout.BeginVertical(new GUILayoutOption[0]);
				EditorGUIUtility.labelWidth = 140f;
				EditorGUILayout.LabelField(GraphicsSettingsWindow.TierSettingsEditor.Styles.empty, EditorStyles.boldLabel, new GUILayoutOption[0]);
				this.OnFieldLabelsGUI(false);
				EditorGUILayout.LabelField(GraphicsSettingsWindow.TierSettingsEditor.Styles.empty, EditorStyles.boldLabel, new GUILayoutOption[0]);
				EditorGUILayout.LabelField(GraphicsSettingsWindow.TierSettingsEditor.Styles.autoSettings, EditorStyles.boldLabel, new GUILayoutOption[0]);
				EditorGUILayout.EndVertical();
				EditorGUIUtility.labelWidth = 50f;
				IEnumerator enumerator = Enum.GetValues(typeof(GraphicsTier)).GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						GraphicsTier graphicsTier = (GraphicsTier)enumerator.Current;
						bool flag = EditorGraphicsSettings.AreTierSettingsAutomatic(platform, graphicsTier);
						EditorGUILayout.BeginVertical(new GUILayoutOption[0]);
						EditorGUILayout.LabelField(GraphicsSettingsWindow.TierSettingsEditor.Styles.tierName[(int)graphicsTier], EditorStyles.boldLabel, new GUILayoutOption[0]);
						using (new EditorGUI.DisabledScope(flag))
						{
							this.OnTierGUI(platform, graphicsTier, false);
						}
						EditorGUILayout.LabelField(GraphicsSettingsWindow.TierSettingsEditor.Styles.empty, EditorStyles.boldLabel, new GUILayoutOption[0]);
						EditorGUI.BeginChangeCheck();
						flag = EditorGUILayout.Toggle(flag, new GUILayoutOption[0]);
						if (EditorGUI.EndChangeCheck())
						{
							EditorGraphicsSettings.RegisterUndoForGraphicsSettings();
							EditorGraphicsSettings.MakeTierSettingsAutomatic(platform, graphicsTier, flag);
							EditorGraphicsSettings.OnUpdateTierSettingsImpl(platform, true);
						}
						EditorGUILayout.EndVertical();
					}
				}
				finally
				{
					IDisposable disposable;
					if ((disposable = (enumerator as IDisposable)) != null)
					{
						disposable.Dispose();
					}
				}
				EditorGUIUtility.labelWidth = 0f;
				EditorGUILayout.EndHorizontal();
			}

			internal void OnGuiVertical(BuildTargetGroup platform)
			{
				IEnumerator enumerator = Enum.GetValues(typeof(GraphicsTier)).GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						GraphicsTier graphicsTier = (GraphicsTier)enumerator.Current;
						bool flag = EditorGraphicsSettings.AreTierSettingsAutomatic(platform, graphicsTier);
						EditorGUI.BeginChangeCheck();
						GUILayout.BeginHorizontal(new GUILayoutOption[0]);
						EditorGUIUtility.labelWidth = 80f;
						EditorGUILayout.LabelField(GraphicsSettingsWindow.TierSettingsEditor.Styles.tierName[(int)graphicsTier], EditorStyles.boldLabel, new GUILayoutOption[0]);
						GUILayout.FlexibleSpace();
						EditorGUIUtility.labelWidth = 75f;
						flag = EditorGUILayout.Toggle(GraphicsSettingsWindow.TierSettingsEditor.Styles.autoSettings, flag, new GUILayoutOption[0]);
						GUILayout.EndHorizontal();
						if (EditorGUI.EndChangeCheck())
						{
							EditorGraphicsSettings.RegisterUndoForGraphicsSettings();
							EditorGraphicsSettings.MakeTierSettingsAutomatic(platform, graphicsTier, flag);
							EditorGraphicsSettings.OnUpdateTierSettingsImpl(platform, true);
						}
						using (new EditorGUI.DisabledScope(flag))
						{
							EditorGUI.indentLevel++;
							EditorGUILayout.BeginHorizontal(new GUILayoutOption[0]);
							EditorGUILayout.BeginVertical(new GUILayoutOption[0]);
							EditorGUIUtility.labelWidth = 140f;
							this.OnFieldLabelsGUI(true);
							EditorGUILayout.EndVertical();
							EditorGUILayout.BeginVertical(new GUILayoutOption[0]);
							EditorGUIUtility.labelWidth = 50f;
							this.OnTierGUI(platform, graphicsTier, true);
							EditorGUILayout.EndVertical();
							GUILayout.EndHorizontal();
							EditorGUI.indentLevel--;
						}
					}
				}
				finally
				{
					IDisposable disposable;
					if ((disposable = (enumerator as IDisposable)) != null)
					{
						disposable.Dispose();
					}
				}
				EditorGUIUtility.labelWidth = 0f;
			}

			public override void OnInspectorGUI()
			{
				BuildPlatform[] array = BuildPlatforms.instance.GetValidPlatforms().ToArray();
				BuildTargetGroup targetGroup = array[EditorGUILayout.BeginPlatformGrouping(array, null, GUIStyle.none)].targetGroup;
				if (this.verticalLayout)
				{
					this.OnGuiVertical(targetGroup);
				}
				else
				{
					this.OnGuiHorizontal(targetGroup);
				}
				EditorGUILayout.EndPlatformGrouping();
			}
		}
	}
}
