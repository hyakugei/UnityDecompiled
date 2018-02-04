using System;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;

namespace UnityEditor
{
	[CustomEditor(typeof(RenderSettings))]
	internal class LightingEditor : Editor
	{
		internal static class Styles
		{
			public static readonly GUIContent env_top;

			public static readonly GUIContent env_skybox_mat;

			public static readonly GUIContent env_skybox_sun;

			public static readonly GUIContent env_amb_top;

			public static readonly GUIContent env_amb_src;

			public static readonly GUIContent env_amb_int;

			public static readonly GUIContent env_refl_top;

			public static readonly GUIContent env_refl_src;

			public static readonly GUIContent env_refl_res;

			public static readonly GUIContent env_refl_cmp;

			public static readonly GUIContent env_refl_int;

			public static readonly GUIContent env_refl_bnc;

			public static readonly GUIContent skyboxWarning;

			public static readonly GUIContent createLight;

			public static readonly GUIContent ambientUp;

			public static readonly GUIContent ambientMid;

			public static readonly GUIContent ambientDown;

			public static readonly GUIContent ambient;

			public static readonly GUIContent customReflection;

			public static readonly GUIContent AmbientLightingMode;

			public static readonly GUIContent[] kFullAmbientSource;

			public static readonly GUIContent[] AmbientLightingModes;

			public static readonly int[] kFullAmbientSourceValues;

			static Styles()
			{
				LightingEditor.Styles.env_top = EditorGUIUtility.TrTextContent("Environment", null, null);
				LightingEditor.Styles.env_skybox_mat = EditorGUIUtility.TrTextContent("Skybox Material", "Specifies the material that is used to simulate the sky or other distant background in the Scene.", null);
				LightingEditor.Styles.env_skybox_sun = EditorGUIUtility.TrTextContent("Sun Source", "Specifies the directional light that is used to indicate the direction of the sun when a procedural skybox is used. If set to None, the brightest directional light in the Scene is used to represent the sun.", null);
				LightingEditor.Styles.env_amb_top = EditorGUIUtility.TrTextContent("Environment Lighting", null, null);
				LightingEditor.Styles.env_amb_src = EditorGUIUtility.TrTextContent("Source", "Specifies whether to use a skybox, gradient, or color for ambient light contributed to the Scene.", null);
				LightingEditor.Styles.env_amb_int = EditorGUIUtility.TrTextContent("Intensity Multiplier", "Controls the brightness of the skybox lighting in the Scene.", null);
				LightingEditor.Styles.env_refl_top = EditorGUIUtility.TrTextContent("Environment Reflections", null, null);
				LightingEditor.Styles.env_refl_src = EditorGUIUtility.TrTextContent("Source", "Specifies whether to use the skybox or a custom cube map for reflection effects in the Scene.", null);
				LightingEditor.Styles.env_refl_res = EditorGUIUtility.TrTextContent("Resolution", "Controls the resolution for the cube map assigned to the skybox material for reflection effects in the Scene.", null);
				LightingEditor.Styles.env_refl_cmp = EditorGUIUtility.TrTextContent("Compression", "Controls how Unity compresses the reflection cube maps. Options are Auto, Compressed, and Uncompressed. Auto compresses the cube maps if the compression format is suitable.", null);
				LightingEditor.Styles.env_refl_int = EditorGUIUtility.TrTextContent("Intensity Multiplier", "Controls how much the skybox or custom cubemap affects reflections in the Scene. A value of 1 produces physically correct results.", null);
				LightingEditor.Styles.env_refl_bnc = EditorGUIUtility.TrTextContent("Bounces", "Controls how many times a reflection includes other reflections. A value of 1 results in the Scene being rendered once so mirrored reflections will be black. A value of 2 results in mirrored reflections being visible in the Scene.", null);
				LightingEditor.Styles.skyboxWarning = EditorGUIUtility.TrTextContent("Shader of this material does not support skybox rendering.", null, null);
				LightingEditor.Styles.createLight = EditorGUIUtility.TrTextContent("Create Light", null, null);
				LightingEditor.Styles.ambientUp = EditorGUIUtility.TrTextContent("Sky Color", "Controls the color of light emitted from the sky in the Scene.", null);
				LightingEditor.Styles.ambientMid = EditorGUIUtility.TrTextContent("Equator Color", "Controls the color of light emitted from the sides of the Scene.", null);
				LightingEditor.Styles.ambientDown = EditorGUIUtility.TrTextContent("Ground Color", "Controls the color of light emitted from the ground of the Scene.", null);
				LightingEditor.Styles.ambient = EditorGUIUtility.TrTextContent("Ambient Color", "Controls the color of the ambient light contributed to the Scene.", null);
				LightingEditor.Styles.customReflection = EditorGUIUtility.TrTextContent("Cubemap", "Specifies the custom cube map used for reflection effects in the Scene.", null);
				LightingEditor.Styles.AmbientLightingMode = EditorGUIUtility.TrTextContent("Ambient Mode", "Specifies the Global Illumination mode that should be used for handling ambient light in the Scene. Options are Realtime or Baked. This property is not editable unless both Realtime Global Illumination and Baked Global Illumination are enabled for the scene.", null);
				LightingEditor.Styles.kFullAmbientSource = new GUIContent[]
				{
					EditorGUIUtility.TrTextContent("Skybox", null, null),
					EditorGUIUtility.TrTextContent("Gradient", null, null),
					EditorGUIUtility.TrTextContent("Color", null, null)
				};
				LightingEditor.Styles.AmbientLightingModes = new GUIContent[]
				{
					EditorGUIUtility.TrTextContent("Realtime", null, null),
					EditorGUIUtility.TrTextContent("Baked", null, null)
				};
				LightingEditor.Styles.kFullAmbientSourceValues = new int[]
				{
					0,
					1,
					3
				};
			}
		}

		protected SerializedProperty m_EnabledBakedGI;

		protected SerializedProperty m_EnabledRealtimeGI;

		protected SerializedProperty m_Sun;

		protected SerializedProperty m_AmbientSource;

		protected SerializedProperty m_AmbientSkyColor;

		protected SerializedProperty m_AmbientEquatorColor;

		protected SerializedProperty m_AmbientGroundColor;

		protected SerializedProperty m_AmbientIntensity;

		protected SerializedProperty m_AmbientLightingMode;

		protected SerializedProperty m_ReflectionIntensity;

		protected SerializedProperty m_ReflectionBounces;

		protected SerializedProperty m_SkyboxMaterial;

		protected SerializedProperty m_DefaultReflectionMode;

		protected SerializedProperty m_DefaultReflectionResolution;

		protected SerializedProperty m_CustomReflection;

		protected SerializedProperty m_ReflectionCompression;

		protected SerializedObject m_LightmapSettings;

		private bool m_bShowEnvironment;

		private const string kShowEnvironment = "ShowEnvironment";

		public virtual void OnEnable()
		{
			this.m_Sun = base.serializedObject.FindProperty("m_Sun");
			this.m_AmbientSource = base.serializedObject.FindProperty("m_AmbientMode");
			this.m_AmbientSkyColor = base.serializedObject.FindProperty("m_AmbientSkyColor");
			this.m_AmbientEquatorColor = base.serializedObject.FindProperty("m_AmbientEquatorColor");
			this.m_AmbientGroundColor = base.serializedObject.FindProperty("m_AmbientGroundColor");
			this.m_AmbientIntensity = base.serializedObject.FindProperty("m_AmbientIntensity");
			this.m_ReflectionIntensity = base.serializedObject.FindProperty("m_ReflectionIntensity");
			this.m_ReflectionBounces = base.serializedObject.FindProperty("m_ReflectionBounces");
			this.m_SkyboxMaterial = base.serializedObject.FindProperty("m_SkyboxMaterial");
			this.m_DefaultReflectionMode = base.serializedObject.FindProperty("m_DefaultReflectionMode");
			this.m_DefaultReflectionResolution = base.serializedObject.FindProperty("m_DefaultReflectionResolution");
			this.m_CustomReflection = base.serializedObject.FindProperty("m_CustomReflection");
			this.m_LightmapSettings = new SerializedObject(LightmapEditorSettings.GetLightmapSettings());
			this.m_ReflectionCompression = this.m_LightmapSettings.FindProperty("m_LightmapEditorSettings.m_ReflectionCompression");
			this.m_AmbientLightingMode = this.m_LightmapSettings.FindProperty("m_GISettings.m_EnvironmentLightingMode");
			this.m_EnabledBakedGI = this.m_LightmapSettings.FindProperty("m_GISettings.m_EnableBakedLightmaps");
			this.m_EnabledRealtimeGI = this.m_LightmapSettings.FindProperty("m_GISettings.m_EnableRealtimeLightmaps");
			this.m_bShowEnvironment = SessionState.GetBool("ShowEnvironment", true);
		}

		public virtual void OnDisable()
		{
			SessionState.SetBool("ShowEnvironment", this.m_bShowEnvironment);
		}

		private void DrawGUI()
		{
			Material material = this.m_SkyboxMaterial.objectReferenceValue as Material;
			this.m_bShowEnvironment = EditorGUILayout.FoldoutTitlebar(this.m_bShowEnvironment, LightingEditor.Styles.env_top, true);
			if (this.m_bShowEnvironment)
			{
				EditorGUI.indentLevel++;
				EditorGUILayout.PropertyField(this.m_SkyboxMaterial, LightingEditor.Styles.env_skybox_mat, new GUILayoutOption[0]);
				if (material && !EditorMaterialUtility.IsBackgroundMaterial(material))
				{
					EditorGUILayout.HelpBox(LightingEditor.Styles.skyboxWarning.text, MessageType.Warning);
				}
				EditorGUILayout.PropertyField(this.m_Sun, LightingEditor.Styles.env_skybox_sun, new GUILayoutOption[0]);
				EditorGUILayout.Space();
				EditorGUILayout.LabelField(LightingEditor.Styles.env_amb_top, new GUILayoutOption[0]);
				EditorGUI.indentLevel++;
				EditorGUILayout.IntPopup(this.m_AmbientSource, LightingEditor.Styles.kFullAmbientSource, LightingEditor.Styles.kFullAmbientSourceValues, LightingEditor.Styles.env_amb_src, new GUILayoutOption[0]);
				AmbientMode intValue = (AmbientMode)this.m_AmbientSource.intValue;
				if (intValue != AmbientMode.Trilight)
				{
					if (intValue != AmbientMode.Flat)
					{
						if (intValue == AmbientMode.Skybox)
						{
							if (material == null)
							{
								EditorGUI.BeginChangeCheck();
								Color colorValue = EditorGUILayout.ColorField(LightingEditor.Styles.ambient, this.m_AmbientSkyColor.colorValue, true, false, true, new GUILayoutOption[0]);
								if (EditorGUI.EndChangeCheck())
								{
									this.m_AmbientSkyColor.colorValue = colorValue;
								}
							}
							else
							{
								EditorGUILayout.Slider(this.m_AmbientIntensity, 0f, 8f, LightingEditor.Styles.env_amb_int, new GUILayoutOption[0]);
							}
						}
					}
					else
					{
						EditorGUI.BeginChangeCheck();
						Color colorValue2 = EditorGUILayout.ColorField(LightingEditor.Styles.ambient, this.m_AmbientSkyColor.colorValue, true, false, true, new GUILayoutOption[0]);
						if (EditorGUI.EndChangeCheck())
						{
							this.m_AmbientSkyColor.colorValue = colorValue2;
						}
					}
				}
				else
				{
					EditorGUI.BeginChangeCheck();
					Color colorValue3 = EditorGUILayout.ColorField(LightingEditor.Styles.ambientUp, this.m_AmbientSkyColor.colorValue, true, false, true, new GUILayoutOption[0]);
					Color colorValue4 = EditorGUILayout.ColorField(LightingEditor.Styles.ambientMid, this.m_AmbientEquatorColor.colorValue, true, false, true, new GUILayoutOption[0]);
					Color colorValue5 = EditorGUILayout.ColorField(LightingEditor.Styles.ambientDown, this.m_AmbientGroundColor.colorValue, true, false, true, new GUILayoutOption[0]);
					if (EditorGUI.EndChangeCheck())
					{
						this.m_AmbientSkyColor.colorValue = colorValue3;
						this.m_AmbientEquatorColor.colorValue = colorValue4;
						this.m_AmbientGroundColor.colorValue = colorValue5;
					}
				}
				bool flag = SupportedRenderingFeatures.IsLightmapBakeTypeSupported(LightmapBakeType.Realtime);
				bool flag2 = SupportedRenderingFeatures.IsLightmapBakeTypeSupported(LightmapBakeType.Baked);
				if ((this.m_EnabledBakedGI.boolValue || this.m_EnabledRealtimeGI.boolValue) && (flag2 || flag))
				{
					int[] optionValues = new int[]
					{
						0,
						1
					};
					if (this.m_EnabledBakedGI.boolValue && this.m_EnabledRealtimeGI.boolValue)
					{
						using (new EditorGUI.DisabledScope((this.m_AmbientLightingMode.intValue == 0 && flag && !flag2) || (this.m_AmbientLightingMode.intValue == 1 && flag2 && !flag)))
						{
							EditorGUILayout.IntPopup(this.m_AmbientLightingMode, LightingEditor.Styles.AmbientLightingModes, optionValues, LightingEditor.Styles.AmbientLightingMode, new GUILayoutOption[0]);
						}
						if ((this.m_AmbientLightingMode.intValue == 0 && !flag) || (this.m_AmbientLightingMode.intValue == 1 && !flag2))
						{
							EditorGUILayout.HelpBox("The following mode is not supported and will fallback on " + ((this.m_AmbientLightingMode.intValue != 0 || flag) ? "Realtime" : "Baked"), MessageType.Warning);
						}
					}
					else if ((this.m_EnabledBakedGI.boolValue && flag2) || (this.m_EnabledRealtimeGI.boolValue && flag))
					{
						using (new EditorGUI.DisabledScope(true))
						{
							EditorGUILayout.IntPopup(LightingEditor.Styles.AmbientLightingMode, (!this.m_EnabledBakedGI.boolValue) ? 0 : 1, LightingEditor.Styles.AmbientLightingModes, optionValues, new GUILayoutOption[0]);
						}
					}
				}
				EditorGUI.indentLevel--;
				EditorGUILayout.Space();
				EditorGUILayout.LabelField(LightingEditor.Styles.env_refl_top, new GUILayoutOption[0]);
				EditorGUI.indentLevel++;
				EditorGUILayout.PropertyField(this.m_DefaultReflectionMode, LightingEditor.Styles.env_refl_src, new GUILayoutOption[0]);
				DefaultReflectionMode intValue2 = (DefaultReflectionMode)this.m_DefaultReflectionMode.intValue;
				if (intValue2 != DefaultReflectionMode.FromSkybox)
				{
					if (intValue2 == DefaultReflectionMode.Custom)
					{
						EditorGUILayout.PropertyField(this.m_CustomReflection, LightingEditor.Styles.customReflection, new GUILayoutOption[0]);
					}
				}
				else
				{
					int[] optionValues2 = null;
					GUIContent[] displayedOptions = null;
					ReflectionProbeEditor.GetResolutionArray(ref optionValues2, ref displayedOptions);
					EditorGUILayout.IntPopup(this.m_DefaultReflectionResolution, displayedOptions, optionValues2, LightingEditor.Styles.env_refl_res, new GUILayoutOption[]
					{
						GUILayout.MinWidth(40f)
					});
				}
				EditorGUILayout.PropertyField(this.m_ReflectionCompression, LightingEditor.Styles.env_refl_cmp, new GUILayoutOption[0]);
				EditorGUILayout.Slider(this.m_ReflectionIntensity, 0f, 1f, LightingEditor.Styles.env_refl_int, new GUILayoutOption[0]);
				EditorGUILayout.IntSlider(this.m_ReflectionBounces, 1, 5, LightingEditor.Styles.env_refl_bnc, new GUILayoutOption[0]);
				EditorGUI.indentLevel--;
				EditorGUI.indentLevel--;
				EditorGUILayout.Space();
			}
		}

		public override void OnInspectorGUI()
		{
			base.serializedObject.Update();
			this.m_LightmapSettings.Update();
			this.DrawGUI();
			base.serializedObject.ApplyModifiedProperties();
			this.m_LightmapSettings.ApplyModifiedProperties();
		}
	}
}
