using System;
using UnityEditor.AnimatedValues;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

namespace UnityEditor
{
	[CanEditMultipleObjects, CustomEditor(typeof(Light))]
	public class LightEditor : Editor
	{
		public sealed class Settings
		{
			private class Styles
			{
				public readonly GUIContent Type = EditorGUIUtility.TrTextContent("Type", "Specifies the current type of light. Possible types are Directional, Spot, Point, and Area lights.", null);

				public readonly GUIContent Range = EditorGUIUtility.TrTextContent("Range", "Controls how far the light is emitted from the center of the object.", null);

				public readonly GUIContent SpotAngle = EditorGUIUtility.TrTextContent("Spot Angle", "Controls the angle in degrees at the base of a Spot light's cone.", null);

				public readonly GUIContent Color = EditorGUIUtility.TrTextContent("Color", "Controls the color being emitted by the light.", null);

				public readonly GUIContent UseColorTemperature = EditorGUIUtility.TrTextContent("Use color temperature mode", "Choose between RGB and temperature mode for light's color.", null);

				public readonly GUIContent ColorFilter = EditorGUIUtility.TrTextContent("Filter", "A colored gel can be put in front of the light source to tint the light.", null);

				public readonly GUIContent ColorTemperature = EditorGUIUtility.TrTextContent("Temperature", "Also known as CCT (Correlated color temperature). The color temperature of the electromagnetic radiation emitted from an ideal black body is defined as its surface temperature in Kelvin. White is 6500K", null);

				public readonly GUIContent Intensity = EditorGUIUtility.TrTextContent("Intensity", "Controls the brightness of the light. Light color is multiplied by this value.", null);

				public readonly GUIContent LightmappingMode = EditorGUIUtility.TrTextContent("Mode", "Specifies the light mode used to determine if and how a light will be baked. Possible modes are Baked, Mixed, and Realtime.", null);

				public readonly GUIContent LightBounceIntensity = EditorGUIUtility.TrTextContent("Indirect Multiplier", "Controls the intensity of indirect light being contributed to the scene. A value of 0 will cause Realtime lights to be removed from realtime global illumination and Baked and Mixed lights to no longer emit indirect lighting. Has no effect when both Realtime and Baked Global Illumination are disabled.", null);

				public readonly GUIContent ShadowType = EditorGUIUtility.TrTextContent("Shadow Type", "Specifies whether Hard Shadows, Soft Shadows, or No Shadows will be cast by the light.", null);

				public readonly GUIContent ShadowRealtimeSettings = EditorGUIUtility.TrTextContent("Realtime Shadows", "Settings for realtime direct shadows.", null);

				public readonly GUIContent ShadowStrength = EditorGUIUtility.TrTextContent("Strength", "Controls how dark the shadows cast by the light will be.", null);

				public readonly GUIContent ShadowResolution = EditorGUIUtility.TrTextContent("Resolution", "Controls the rendered resolution of the shadow maps. A higher resolution will increase the fidelity of shadows at the cost of GPU performance and memory usage.", null);

				public readonly GUIContent ShadowBias = EditorGUIUtility.TrTextContent("Bias", "Controls the distance at which the shadows will be pushed away from the light. Useful for avoiding false self-shadowing artifacts.", null);

				public readonly GUIContent ShadowNormalBias = EditorGUIUtility.TrTextContent("Normal Bias", "Controls distance at which the shadow casting surfaces will be shrunk along the surface normal. Useful for avoiding false self-shadowing artifacts.", null);

				public readonly GUIContent ShadowNearPlane = EditorGUIUtility.TrTextContent("Near Plane", "Controls the value for the near clip plane when rendering shadows. Currently clamped to 0.1 units or 1% of the lights range property, whichever is lower.", null);

				public readonly GUIContent BakedShadowRadius = EditorGUIUtility.TrTextContent("Baked Shadow Radius", "Controls the amount of artificial softening applied to the edges of shadows cast by the Point or Spot light.", null);

				public readonly GUIContent BakedShadowAngle = EditorGUIUtility.TrTextContent("Baked Shadow Angle", "Controls the amount of artificial softening applied to the edges of shadows cast by directional lights.", null);

				public readonly GUIContent Cookie = EditorGUIUtility.TrTextContent("Cookie", "Specifies the Texture mask to cast shadows, create silhouettes, or patterned illumination for the light.", null);

				public readonly GUIContent CookieSize = EditorGUIUtility.TrTextContent("Cookie Size", "Controls the size of the cookie mask currently assigned to the light.", null);

				public readonly GUIContent DrawHalo = EditorGUIUtility.TrTextContent("Draw Halo", "When enabled, draws a spherical halo of light with a radius equal to the lights range value.", null);

				public readonly GUIContent Flare = EditorGUIUtility.TrTextContent("Flare", "Specifies the flare object to be used by the light to render lens flares in the scene.", null);

				public readonly GUIContent RenderMode = EditorGUIUtility.TrTextContent("Render Mode", "Specifies the importance of the light which impacts lighting fidelity and performance. Options are Auto, Important, and Not Important. This only affects Forward Rendering", null);

				public readonly GUIContent CullingMask = EditorGUIUtility.TrTextContent("Culling Mask", "Specifies which layers will be affected or excluded from the light's effect on objects in the scene.", null);

				public readonly GUIContent AreaWidth = EditorGUIUtility.TrTextContent("Width", "Controls the width in units of the area light.", null);

				public readonly GUIContent AreaHeight = EditorGUIUtility.TrTextContent("Height", "Controls the height in units of the area light.", null);

				public readonly GUIContent BakingWarning = EditorGUIUtility.TrTextContent("Light mode is currently overridden to Realtime mode. Enable Baked Global Illumination to use Mixed or Baked light modes.", null, null);

				public readonly GUIContent IndirectBounceShadowWarning = EditorGUIUtility.TrTextContent("Realtime indirect bounce shadowing is not supported for Spot and Point lights.", null, null);

				public readonly GUIContent CookieWarning = EditorGUIUtility.TrTextContent("Cookie textures for spot lights should be set to clamp, not repeat, to avoid artifacts.", null, null);

				public readonly GUIContent[] LightmapBakeTypeTitles = new GUIContent[]
				{
					EditorGUIUtility.TrTextContent("Realtime", null, null),
					EditorGUIUtility.TrTextContent("Mixed", null, null),
					EditorGUIUtility.TrTextContent("Baked", null, null)
				};

				public readonly int[] LightmapBakeTypeValues = new int[]
				{
					4,
					1,
					2
				};
			}

			private SerializedObject m_SerializedObject;

			private Texture2D m_KelvinGradientTexture;

			private const float kMinKelvin = 1000f;

			private const float kMaxKelvin = 20000f;

			private const float kSliderPower = 2f;

			private static LightEditor.Settings.Styles s_Styles;

			public SerializedProperty lightType
			{
				get;
				private set;
			}

			public SerializedProperty range
			{
				get;
				private set;
			}

			public SerializedProperty spotAngle
			{
				get;
				private set;
			}

			public SerializedProperty cookieSize
			{
				get;
				private set;
			}

			public SerializedProperty color
			{
				get;
				private set;
			}

			public SerializedProperty intensity
			{
				get;
				private set;
			}

			public SerializedProperty bounceIntensity
			{
				get;
				private set;
			}

			public SerializedProperty colorTemperature
			{
				get;
				private set;
			}

			public SerializedProperty useColorTemperature
			{
				get;
				private set;
			}

			public SerializedProperty cookieProp
			{
				get;
				private set;
			}

			public SerializedProperty shadowsType
			{
				get;
				private set;
			}

			public SerializedProperty shadowsStrength
			{
				get;
				private set;
			}

			public SerializedProperty shadowsResolution
			{
				get;
				private set;
			}

			public SerializedProperty shadowsBias
			{
				get;
				private set;
			}

			public SerializedProperty shadowsNormalBias
			{
				get;
				private set;
			}

			public SerializedProperty shadowsNearPlane
			{
				get;
				private set;
			}

			public SerializedProperty halo
			{
				get;
				private set;
			}

			public SerializedProperty flare
			{
				get;
				private set;
			}

			public SerializedProperty renderMode
			{
				get;
				private set;
			}

			public SerializedProperty cullingMask
			{
				get;
				private set;
			}

			public SerializedProperty lightmapping
			{
				get;
				private set;
			}

			public SerializedProperty areaSizeX
			{
				get;
				private set;
			}

			public SerializedProperty areaSizeY
			{
				get;
				private set;
			}

			public SerializedProperty bakedShadowRadiusProp
			{
				get;
				private set;
			}

			public SerializedProperty bakedShadowAngleProp
			{
				get;
				private set;
			}

			public bool isRealtime
			{
				get
				{
					return this.lightmapping.intValue == 4;
				}
			}

			public bool isCompletelyBaked
			{
				get
				{
					return this.lightmapping.intValue == 2;
				}
			}

			public bool isBakedOrMixed
			{
				get
				{
					return !this.isRealtime;
				}
			}

			public Texture cookie
			{
				get
				{
					return this.cookieProp.objectReferenceValue as Texture;
				}
			}

			internal bool typeIsSame
			{
				get
				{
					return !this.lightType.hasMultipleDifferentValues;
				}
			}

			internal bool shadowTypeIsSame
			{
				get
				{
					return !this.shadowsType.hasMultipleDifferentValues;
				}
			}

			private bool lightmappingTypeIsSame
			{
				get
				{
					return !this.lightmapping.hasMultipleDifferentValues;
				}
			}

			public Light light
			{
				get
				{
					return this.m_SerializedObject.targetObject as Light;
				}
			}

			internal bool bounceWarningValue
			{
				get
				{
					return this.typeIsSame && (this.light.type == LightType.Point || this.light.type == LightType.Spot) && this.lightmappingTypeIsSame && this.isRealtime && !this.bounceIntensity.hasMultipleDifferentValues && this.bounceIntensity.floatValue > 0f;
				}
			}

			internal bool bakingWarningValue
			{
				get
				{
					return !Lightmapping.bakedGI && this.lightmappingTypeIsSame && this.isBakedOrMixed;
				}
			}

			internal bool cookieWarningValue
			{
				get
				{
					return this.typeIsSame && this.light.type == LightType.Spot && !this.cookieProp.hasMultipleDifferentValues && this.cookie && this.cookie.wrapMode != TextureWrapMode.Clamp;
				}
			}

			public Settings(SerializedObject so)
			{
				this.m_SerializedObject = so;
			}

			public void OnEnable()
			{
				this.lightType = this.m_SerializedObject.FindProperty("m_Type");
				this.range = this.m_SerializedObject.FindProperty("m_Range");
				this.spotAngle = this.m_SerializedObject.FindProperty("m_SpotAngle");
				this.cookieSize = this.m_SerializedObject.FindProperty("m_CookieSize");
				this.color = this.m_SerializedObject.FindProperty("m_Color");
				this.intensity = this.m_SerializedObject.FindProperty("m_Intensity");
				this.bounceIntensity = this.m_SerializedObject.FindProperty("m_BounceIntensity");
				this.colorTemperature = this.m_SerializedObject.FindProperty("m_ColorTemperature");
				this.useColorTemperature = this.m_SerializedObject.FindProperty("m_UseColorTemperature");
				this.cookieProp = this.m_SerializedObject.FindProperty("m_Cookie");
				this.shadowsType = this.m_SerializedObject.FindProperty("m_Shadows.m_Type");
				this.shadowsStrength = this.m_SerializedObject.FindProperty("m_Shadows.m_Strength");
				this.shadowsResolution = this.m_SerializedObject.FindProperty("m_Shadows.m_Resolution");
				this.shadowsBias = this.m_SerializedObject.FindProperty("m_Shadows.m_Bias");
				this.shadowsNormalBias = this.m_SerializedObject.FindProperty("m_Shadows.m_NormalBias");
				this.shadowsNearPlane = this.m_SerializedObject.FindProperty("m_Shadows.m_NearPlane");
				this.halo = this.m_SerializedObject.FindProperty("m_DrawHalo");
				this.flare = this.m_SerializedObject.FindProperty("m_Flare");
				this.renderMode = this.m_SerializedObject.FindProperty("m_RenderMode");
				this.cullingMask = this.m_SerializedObject.FindProperty("m_CullingMask");
				this.lightmapping = this.m_SerializedObject.FindProperty("m_Lightmapping");
				this.areaSizeX = this.m_SerializedObject.FindProperty("m_AreaSize.x");
				this.areaSizeY = this.m_SerializedObject.FindProperty("m_AreaSize.y");
				this.bakedShadowRadiusProp = this.m_SerializedObject.FindProperty("m_ShadowRadius");
				this.bakedShadowAngleProp = this.m_SerializedObject.FindProperty("m_ShadowAngle");
				if (this.m_KelvinGradientTexture == null)
				{
					this.m_KelvinGradientTexture = LightEditor.Settings.CreateKelvinGradientTexture("KelvinGradientTexture", 300, 16, 1000f, 20000f);
				}
			}

			public void OnDestroy()
			{
				if (this.m_KelvinGradientTexture != null)
				{
					UnityEngine.Object.DestroyImmediate(this.m_KelvinGradientTexture);
				}
			}

			private static Texture2D CreateKelvinGradientTexture(string name, int width, int height, float minKelvin, float maxKelvin)
			{
				Texture2D texture2D = new Texture2D(width, height, TextureFormat.ARGB32, false);
				texture2D.name = name;
				texture2D.hideFlags = HideFlags.HideAndDontSave;
				Color32[] array = new Color32[width * height];
				float num = Mathf.Pow(maxKelvin, 0.5f);
				float num2 = Mathf.Pow(minKelvin, 0.5f);
				for (int i = 0; i < width; i++)
				{
					float num3 = (float)i / (float)(width - 1);
					float f = (num - num2) * num3 + num2;
					float kelvin = Mathf.Pow(f, 2f);
					Color color = Mathf.CorrelatedColorTemperatureToRGB(kelvin);
					for (int j = 0; j < height; j++)
					{
						array[j * width + i] = color.gamma;
					}
				}
				texture2D.SetPixels32(array);
				texture2D.wrapMode = TextureWrapMode.Clamp;
				texture2D.Apply();
				return texture2D;
			}

			public void Update()
			{
				if (LightEditor.Settings.s_Styles == null)
				{
					LightEditor.Settings.s_Styles = new LightEditor.Settings.Styles();
				}
				this.m_SerializedObject.Update();
			}

			public void DrawLightType()
			{
				EditorGUILayout.PropertyField(this.lightType, LightEditor.Settings.s_Styles.Type, new GUILayoutOption[0]);
			}

			public void DrawRange(bool showAreaOptions)
			{
				if (showAreaOptions)
				{
					GUI.enabled = false;
					string tooltip = "For area lights " + this.range.displayName + " is computed from Width, Height and Intensity";
					GUIContent label = new GUIContent(this.range.displayName, tooltip);
					EditorGUILayout.FloatField(label, this.light.range, new GUILayoutOption[0]);
					GUI.enabled = true;
				}
				else
				{
					EditorGUILayout.PropertyField(this.range, LightEditor.Settings.s_Styles.Range, new GUILayoutOption[0]);
				}
			}

			public void DrawSpotAngle()
			{
				EditorGUILayout.Slider(this.spotAngle, 1f, 179f, LightEditor.Settings.s_Styles.SpotAngle, new GUILayoutOption[0]);
			}

			public void DrawArea()
			{
				EditorGUILayout.PropertyField(this.areaSizeX, LightEditor.Settings.s_Styles.AreaWidth, new GUILayoutOption[0]);
				EditorGUILayout.PropertyField(this.areaSizeY, LightEditor.Settings.s_Styles.AreaHeight, new GUILayoutOption[0]);
			}

			public void DrawColor()
			{
				if (GraphicsSettings.lightsUseLinearIntensity && GraphicsSettings.lightsUseColorTemperature)
				{
					EditorGUILayout.PropertyField(this.useColorTemperature, LightEditor.Settings.s_Styles.UseColorTemperature, new GUILayoutOption[0]);
					if (this.useColorTemperature.boolValue)
					{
						EditorGUILayout.LabelField(LightEditor.Settings.s_Styles.Color, new GUILayoutOption[0]);
						EditorGUI.indentLevel++;
						EditorGUILayout.PropertyField(this.color, LightEditor.Settings.s_Styles.ColorFilter, new GUILayoutOption[0]);
						EditorGUILayout.SliderWithTexture(LightEditor.Settings.s_Styles.ColorTemperature, this.colorTemperature, 1000f, 20000f, 2f, this.m_KelvinGradientTexture, new GUILayoutOption[0]);
						EditorGUI.indentLevel--;
					}
					else
					{
						EditorGUILayout.PropertyField(this.color, LightEditor.Settings.s_Styles.Color, new GUILayoutOption[0]);
					}
				}
				else
				{
					EditorGUILayout.PropertyField(this.color, LightEditor.Settings.s_Styles.Color, new GUILayoutOption[0]);
				}
			}

			public void DrawLightmapping()
			{
				EditorGUILayout.IntPopup(this.lightmapping, LightEditor.Settings.s_Styles.LightmapBakeTypeTitles, LightEditor.Settings.s_Styles.LightmapBakeTypeValues, LightEditor.Settings.s_Styles.LightmappingMode, new GUILayoutOption[0]);
				if (this.bakingWarningValue)
				{
					EditorGUILayout.HelpBox(LightEditor.Settings.s_Styles.BakingWarning.text, MessageType.Info);
				}
			}

			public void DrawIntensity()
			{
				EditorGUILayout.PropertyField(this.intensity, LightEditor.Settings.s_Styles.Intensity, new GUILayoutOption[0]);
			}

			public void DrawBounceIntensity()
			{
				EditorGUILayout.PropertyField(this.bounceIntensity, LightEditor.Settings.s_Styles.LightBounceIntensity, new GUILayoutOption[0]);
				if (this.bounceWarningValue)
				{
					EditorGUILayout.HelpBox(LightEditor.Settings.s_Styles.IndirectBounceShadowWarning.text, MessageType.Info);
				}
			}

			public void DrawCookie()
			{
				EditorGUILayout.PropertyField(this.cookieProp, LightEditor.Settings.s_Styles.Cookie, new GUILayoutOption[0]);
				if (this.cookieWarningValue)
				{
					EditorGUILayout.HelpBox(LightEditor.Settings.s_Styles.CookieWarning.text, MessageType.Warning);
				}
			}

			public void DrawCookieSize()
			{
				EditorGUILayout.PropertyField(this.cookieSize, LightEditor.Settings.s_Styles.CookieSize, new GUILayoutOption[0]);
			}

			public void DrawHalo()
			{
				EditorGUILayout.PropertyField(this.halo, LightEditor.Settings.s_Styles.DrawHalo, new GUILayoutOption[0]);
			}

			public void DrawFlare()
			{
				EditorGUILayout.PropertyField(this.flare, LightEditor.Settings.s_Styles.Flare, new GUILayoutOption[0]);
			}

			public void DrawRenderMode()
			{
				EditorGUILayout.PropertyField(this.renderMode, LightEditor.Settings.s_Styles.RenderMode, new GUILayoutOption[0]);
			}

			public void DrawCullingMask()
			{
				EditorGUILayout.PropertyField(this.cullingMask, LightEditor.Settings.s_Styles.CullingMask, new GUILayoutOption[0]);
			}

			public void ApplyModifiedProperties()
			{
				this.m_SerializedObject.ApplyModifiedProperties();
			}

			public void DrawShadowsType()
			{
				EditorGUILayout.Space();
				EditorGUILayout.PropertyField(this.shadowsType, LightEditor.Settings.s_Styles.ShadowType, new GUILayoutOption[0]);
			}

			public void DrawBakedShadowRadius()
			{
				using (new EditorGUI.DisabledScope(this.shadowsType.intValue != 2))
				{
					EditorGUILayout.PropertyField(this.bakedShadowRadiusProp, LightEditor.Settings.s_Styles.BakedShadowRadius, new GUILayoutOption[0]);
				}
			}

			public void DrawBakedShadowAngle()
			{
				using (new EditorGUI.DisabledScope(this.shadowsType.intValue != 2))
				{
					EditorGUILayout.Slider(this.bakedShadowAngleProp, 0f, 90f, LightEditor.Settings.s_Styles.BakedShadowAngle, new GUILayoutOption[0]);
				}
			}

			public void DrawRuntimeShadow()
			{
				EditorGUILayout.LabelField(LightEditor.Settings.s_Styles.ShadowRealtimeSettings, new GUILayoutOption[0]);
				EditorGUI.indentLevel++;
				EditorGUILayout.Slider(this.shadowsStrength, 0f, 1f, LightEditor.Settings.s_Styles.ShadowStrength, new GUILayoutOption[0]);
				EditorGUILayout.PropertyField(this.shadowsResolution, LightEditor.Settings.s_Styles.ShadowResolution, new GUILayoutOption[0]);
				EditorGUILayout.Slider(this.shadowsBias, 0f, 2f, LightEditor.Settings.s_Styles.ShadowBias, new GUILayoutOption[0]);
				EditorGUILayout.Slider(this.shadowsNormalBias, 0f, 3f, LightEditor.Settings.s_Styles.ShadowNormalBias, new GUILayoutOption[0]);
				float leftValue = Mathf.Min(0.01f * this.range.floatValue, 0.1f);
				EditorGUILayout.Slider(this.shadowsNearPlane, leftValue, 10f, LightEditor.Settings.s_Styles.ShadowNearPlane, new GUILayoutOption[0]);
				EditorGUI.indentLevel--;
			}
		}

		private class Style
		{
			public readonly GUIContent iconRemove = EditorGUIUtility.TrIconContent("Toolbar Minus", "Remove command buffer");

			public readonly GUIContent DisabledLightWarning = EditorGUIUtility.TrTextContent("Lighting has been disabled in at least one Scene view. Any changes applied to lights in the Scene will not be updated in these views until Lighting has been enabled again.", null, null);

			public readonly GUIStyle invisibleButton = "InvisibleButton";
		}

		private static LightEditor.Style s_Styles;

		private LightEditor.Settings m_Settings;

		private AnimBool m_AnimShowSpotOptions = new AnimBool();

		private AnimBool m_AnimShowPointOptions = new AnimBool();

		private AnimBool m_AnimShowDirOptions = new AnimBool();

		private AnimBool m_AnimShowAreaOptions = new AnimBool();

		private AnimBool m_AnimShowRuntimeOptions = new AnimBool();

		private AnimBool m_AnimShowShadowOptions = new AnimBool();

		private AnimBool m_AnimBakedShadowAngleOptions = new AnimBool();

		private AnimBool m_AnimBakedShadowRadiusOptions = new AnimBool();

		private AnimBool m_AnimShowLightBounceIntensity = new AnimBool();

		private bool m_CommandBuffersShown = true;

		internal static Color kGizmoLight = new Color(0.996078432f, 0.992156863f, 0.533333361f, 0.5019608f);

		internal static Color kGizmoDisabledLight = new Color(0.5294118f, 0.454901963f, 0.196078435f, 0.5019608f);

		protected LightEditor.Settings settings
		{
			get
			{
				if (this.m_Settings == null)
				{
					this.m_Settings = new LightEditor.Settings(base.serializedObject);
				}
				return this.m_Settings;
			}
		}

		private bool spotOptionsValue
		{
			get
			{
				return this.settings.typeIsSame && this.settings.light.type == LightType.Spot;
			}
		}

		private bool pointOptionsValue
		{
			get
			{
				return this.settings.typeIsSame && this.settings.light.type == LightType.Point;
			}
		}

		private bool dirOptionsValue
		{
			get
			{
				return this.settings.typeIsSame && this.settings.light.type == LightType.Directional;
			}
		}

		private bool areaOptionsValue
		{
			get
			{
				return this.settings.typeIsSame && this.settings.light.type == LightType.Area;
			}
		}

		private bool runtimeOptionsValue
		{
			get
			{
				return this.settings.typeIsSame && this.settings.light.type != LightType.Area && !this.settings.isCompletelyBaked;
			}
		}

		private bool bakedShadowRadius
		{
			get
			{
				return this.settings.typeIsSame && (this.settings.light.type == LightType.Point || this.settings.light.type == LightType.Spot) && this.settings.isBakedOrMixed;
			}
		}

		private bool bakedShadowAngle
		{
			get
			{
				return this.settings.typeIsSame && this.settings.light.type == LightType.Directional && this.settings.isBakedOrMixed;
			}
		}

		private bool shadowOptionsValue
		{
			get
			{
				return this.settings.shadowTypeIsSame && this.settings.light.shadows != LightShadows.None;
			}
		}

		private void SetOptions(AnimBool animBool, bool initialize, bool targetValue)
		{
			if (initialize)
			{
				animBool.value = targetValue;
				animBool.valueChanged.AddListener(new UnityAction(base.Repaint));
			}
			else
			{
				animBool.target = targetValue;
			}
		}

		private void UpdateShowOptions(bool initialize)
		{
			this.SetOptions(this.m_AnimShowSpotOptions, initialize, this.spotOptionsValue);
			this.SetOptions(this.m_AnimShowPointOptions, initialize, this.pointOptionsValue);
			this.SetOptions(this.m_AnimShowDirOptions, initialize, this.dirOptionsValue);
			this.SetOptions(this.m_AnimShowAreaOptions, initialize, this.areaOptionsValue);
			this.SetOptions(this.m_AnimShowShadowOptions, initialize, this.shadowOptionsValue);
			this.SetOptions(this.m_AnimShowRuntimeOptions, initialize, this.runtimeOptionsValue);
			this.SetOptions(this.m_AnimBakedShadowAngleOptions, initialize, this.bakedShadowAngle);
			this.SetOptions(this.m_AnimBakedShadowRadiusOptions, initialize, this.bakedShadowRadius);
			this.SetOptions(this.m_AnimShowLightBounceIntensity, initialize, true);
		}

		protected virtual void OnEnable()
		{
			this.settings.OnEnable();
			this.UpdateShowOptions(true);
		}

		protected virtual void OnDestroy()
		{
			if (this.m_Settings != null)
			{
				this.m_Settings.OnDestroy();
				this.m_Settings = null;
			}
		}

		private void CommandBufferGUI()
		{
			if (base.targets.Length == 1)
			{
				Light light = base.target as Light;
				if (!(light == null))
				{
					int commandBufferCount = light.commandBufferCount;
					if (commandBufferCount != 0)
					{
						this.m_CommandBuffersShown = GUILayout.Toggle(this.m_CommandBuffersShown, GUIContent.Temp(commandBufferCount + " command buffers"), EditorStyles.foldout, new GUILayoutOption[0]);
						if (this.m_CommandBuffersShown)
						{
							EditorGUI.indentLevel++;
							LightEvent[] array = (LightEvent[])Enum.GetValues(typeof(LightEvent));
							for (int i = 0; i < array.Length; i++)
							{
								LightEvent lightEvent = array[i];
								CommandBuffer[] commandBuffers = light.GetCommandBuffers(lightEvent);
								CommandBuffer[] array2 = commandBuffers;
								for (int j = 0; j < array2.Length; j++)
								{
									CommandBuffer commandBuffer = array2[j];
									using (new GUILayout.HorizontalScope(new GUILayoutOption[0]))
									{
										Rect rect = GUILayoutUtility.GetRect(GUIContent.none, EditorStyles.miniLabel);
										rect.xMin += EditorGUI.indent;
										Rect removeButtonRect = LightEditor.GetRemoveButtonRect(rect);
										rect.xMax = removeButtonRect.x;
										GUI.Label(rect, string.Format("{0}: {1} ({2})", lightEvent, commandBuffer.name, EditorUtility.FormatBytes(commandBuffer.sizeInBytes)), EditorStyles.miniLabel);
										if (GUI.Button(removeButtonRect, LightEditor.s_Styles.iconRemove, LightEditor.s_Styles.invisibleButton))
										{
											light.RemoveCommandBuffer(lightEvent, commandBuffer);
											SceneView.RepaintAll();
											GameView.RepaintAll();
											GUIUtility.ExitGUI();
										}
									}
								}
							}
							using (new GUILayout.HorizontalScope(new GUILayoutOption[0]))
							{
								GUILayout.FlexibleSpace();
								if (GUILayout.Button("Remove all", EditorStyles.miniButton, new GUILayoutOption[0]))
								{
									light.RemoveAllCommandBuffers();
									SceneView.RepaintAll();
									GameView.RepaintAll();
								}
							}
							EditorGUI.indentLevel--;
						}
					}
				}
			}
		}

		private static Rect GetRemoveButtonRect(Rect r)
		{
			Vector2 vector = LightEditor.s_Styles.invisibleButton.CalcSize(LightEditor.s_Styles.iconRemove);
			return new Rect(r.xMax - vector.x, r.y + (float)((int)(r.height / 2f - vector.y / 2f)), vector.x, vector.y);
		}

		public override void OnInspectorGUI()
		{
			if (LightEditor.s_Styles == null)
			{
				LightEditor.s_Styles = new LightEditor.Style();
			}
			this.settings.Update();
			this.UpdateShowOptions(false);
			this.settings.DrawLightType();
			EditorGUILayout.Space();
			using (new EditorGUILayout.FadeGroupScope(1f - this.m_AnimShowDirOptions.faded))
			{
				this.settings.DrawRange(this.m_AnimShowAreaOptions.target);
			}
			using (new EditorGUILayout.FadeGroupScope(this.m_AnimShowSpotOptions.faded))
			{
				this.settings.DrawSpotAngle();
			}
			using (new EditorGUILayout.FadeGroupScope(this.m_AnimShowAreaOptions.faded))
			{
				this.settings.DrawArea();
			}
			this.settings.DrawColor();
			EditorGUILayout.Space();
			using (new EditorGUILayout.FadeGroupScope(1f - this.m_AnimShowAreaOptions.faded))
			{
				this.settings.DrawLightmapping();
			}
			this.settings.DrawIntensity();
			using (new EditorGUILayout.FadeGroupScope(this.m_AnimShowLightBounceIntensity.faded))
			{
				this.settings.DrawBounceIntensity();
			}
			this.ShadowsGUI();
			using (new EditorGUILayout.FadeGroupScope(this.m_AnimShowRuntimeOptions.faded))
			{
				this.settings.DrawCookie();
			}
			using (new EditorGUILayout.FadeGroupScope(this.m_AnimShowRuntimeOptions.faded * this.m_AnimShowDirOptions.faded))
			{
				this.settings.DrawCookieSize();
			}
			this.settings.DrawHalo();
			this.settings.DrawFlare();
			this.settings.DrawRenderMode();
			this.settings.DrawCullingMask();
			EditorGUILayout.Space();
			if (SceneView.lastActiveSceneView != null && !SceneView.lastActiveSceneView.m_SceneLighting)
			{
				EditorGUILayout.HelpBox(LightEditor.s_Styles.DisabledLightWarning.text, MessageType.Warning);
			}
			this.CommandBufferGUI();
			this.settings.ApplyModifiedProperties();
			base.serializedObject.ApplyModifiedProperties();
		}

		private void ShadowsGUI()
		{
			float num = 1f - this.m_AnimShowAreaOptions.faded;
			using (new EditorGUILayout.FadeGroupScope(num))
			{
				this.settings.DrawShadowsType();
			}
			EditorGUI.indentLevel++;
			num *= this.m_AnimShowShadowOptions.faded;
			using (new EditorGUILayout.FadeGroupScope(num * this.m_AnimBakedShadowRadiusOptions.faded))
			{
				this.settings.DrawBakedShadowRadius();
			}
			using (new EditorGUILayout.FadeGroupScope(num * this.m_AnimBakedShadowAngleOptions.faded))
			{
				this.settings.DrawBakedShadowAngle();
			}
			using (new EditorGUILayout.FadeGroupScope(num * this.m_AnimShowRuntimeOptions.faded))
			{
				this.settings.DrawRuntimeShadow();
			}
			EditorGUI.indentLevel--;
			EditorGUILayout.Space();
		}

		protected virtual void OnSceneGUI()
		{
			Light light = base.target as Light;
			Color color = Handles.color;
			if (light.enabled)
			{
				Handles.color = LightEditor.kGizmoLight;
			}
			else
			{
				Handles.color = LightEditor.kGizmoDisabledLight;
			}
			float num = light.range;
			switch (light.type)
			{
			case LightType.Spot:
			{
				Color color2 = Handles.color;
				color2.a = Mathf.Clamp01(color.a * 2f);
				Handles.color = color2;
				Vector2 angleAndRange = new Vector2(light.spotAngle, light.range);
				angleAndRange = Handles.ConeHandle(light.transform.rotation, light.transform.position, angleAndRange, 1f, 1f, true);
				if (GUI.changed)
				{
					Undo.RecordObject(light, "Adjust Spot Light");
					light.spotAngle = angleAndRange.x;
					light.range = Mathf.Max(angleAndRange.y, 0.01f);
				}
				break;
			}
			case LightType.Point:
				num = Handles.RadiusHandle(Quaternion.identity, light.transform.position, num, true);
				if (GUI.changed)
				{
					Undo.RecordObject(light, "Adjust Point Light");
					light.range = num;
				}
				break;
			case LightType.Area:
			{
				EditorGUI.BeginChangeCheck();
				Vector2 areaSize = Handles.DoRectHandles(light.transform.rotation, light.transform.position, light.areaSize);
				if (EditorGUI.EndChangeCheck())
				{
					Undo.RecordObject(light, "Adjust Area Light");
					light.areaSize = areaSize;
				}
				break;
			}
			}
			Handles.color = color;
		}
	}
}
