using System;
using UnityEngine;
using UnityEngine.Accessibility;

namespace UnityEditor
{
	internal class ColorPicker : EditorWindow
	{
		private enum ColorBoxMode
		{
			HSV,
			EyeDropper
		}

		private enum SliderMode
		{
			RGB,
			RGBFloat,
			HSV
		}

		private static class Styles
		{
			public const float fixedWindowWidth = 233f;

			public const float hexFieldWidth = 72f;

			public const float sliderModeFieldWidth = 72f;

			public const float channelSliderLabelWidth = 14f;

			public const float sliderTextFieldWidth = 45f;

			public const float extraVerticalSpacing = 6f;

			public const float highLuminanceThreshold = 0.5f;

			public static readonly float hueDialThumbSize;

			public static readonly RectOffset colorBoxPadding;

			public static readonly Color lowLuminanceContentColor;

			public static readonly Color highLuminanceContentColor;

			public static readonly GUIStyle originalColorSwatch;

			public static readonly GUIStyle currentColorSwatch;

			public static readonly GUIStyle colorBoxThumb;

			public static readonly GUIStyle hueDialBackground;

			public static readonly GUIStyle hueDialThumb;

			public static readonly GUIStyle sliderBackground;

			public static readonly GUIStyle sliderThumb;

			public static readonly GUIStyle background;

			public static readonly GUIStyle exposureSwatch;

			public static readonly GUIStyle selectedExposureSwatchStroke;

			public static readonly GUIContent eyeDropper;

			public static readonly GUIContent exposureValue;

			public static readonly GUIContent hexLabel;

			public static readonly GUIContent presetsToggle;

			public static readonly ScalableGUIContent originalColorSwatchFill;

			public static readonly ScalableGUIContent currentColorSwatchFill;

			public static readonly ScalableGUIContent hueDialThumbFill;

			public static readonly Texture2D alphaSliderCheckerBackground;

			public static readonly GUIContent[] sliderModeLabels;

			public static readonly int[] sliderModeValues;

			static Styles()
			{
				ColorPicker.Styles.colorBoxPadding = new RectOffset(6, 6, 6, 6);
				ColorPicker.Styles.lowLuminanceContentColor = Color.white;
				ColorPicker.Styles.highLuminanceContentColor = Color.black;
				ColorPicker.Styles.originalColorSwatch = "ColorPickerOriginalColor";
				ColorPicker.Styles.currentColorSwatch = "ColorPickerCurrentColor";
				ColorPicker.Styles.colorBoxThumb = "ColorPicker2DThumb";
				ColorPicker.Styles.hueDialBackground = "ColorPickerHueRing";
				ColorPicker.Styles.hueDialThumb = "ColorPickerHueRingThumb";
				ColorPicker.Styles.sliderBackground = "ColorPickerSliderBackground";
				ColorPicker.Styles.sliderThumb = "ColorPickerHorizThumb";
				ColorPicker.Styles.background = new GUIStyle("ColorPickerBackground");
				ColorPicker.Styles.exposureSwatch = "ColorPickerExposureSwatch";
				ColorPicker.Styles.selectedExposureSwatchStroke = "ColorPickerCurrentExposureSwatchBorder";
				ColorPicker.Styles.eyeDropper = EditorGUIUtility.TrIconContent("EyeDropper.Large", "Pick a color from the screen.");
				ColorPicker.Styles.exposureValue = EditorGUIUtility.TrTextContent("Intensity", "Number of stops to over- or under-expose the color.", null);
				ColorPicker.Styles.hexLabel = EditorGUIUtility.TrTextContent("Hexadecimal", null, null);
				ColorPicker.Styles.presetsToggle = EditorGUIUtility.TrTextContent("Swatches", null, null);
				ColorPicker.Styles.originalColorSwatchFill = new ScalableGUIContent(string.Empty, "The original color. Click this swatch to reset the color picker to this value.", "ColorPicker-OriginalColor");
				ColorPicker.Styles.currentColorSwatchFill = new ScalableGUIContent(string.Empty, "The new color.", "ColorPicker-CurrentColor");
				ColorPicker.Styles.hueDialThumbFill = new ScalableGUIContent("ColorPicker-HueRing-Thumb-Fill");
				ColorPicker.Styles.alphaSliderCheckerBackground = (EditorGUIUtility.LoadRequired("Previews/Textures/textureChecker.png") as Texture2D);
				ColorPicker.Styles.sliderModeLabels = new GUIContent[]
				{
					EditorGUIUtility.TrTextContent("RGB 0-255", null, null),
					EditorGUIUtility.TrTextContent("RGB 0-1.0", null, null),
					EditorGUIUtility.TrTextContent("HSV", null, null)
				};
				ColorPicker.Styles.sliderModeValues = new int[]
				{
					0,
					1,
					2
				};
				Vector2 vector = ColorPicker.Styles.hueDialThumb.CalcSize(ColorPicker.Styles.hueDialThumbFill);
				ColorPicker.Styles.hueDialThumbSize = Mathf.Max(vector.x, vector.y);
			}
		}

		private const float k_DefaultExposureSliderMax = 10f;

		[SerializeField]
		private bool m_HDR;

		[SerializeField]
		private ColorMutator m_Color;

		[SerializeField]
		private Texture2D m_ColorSlider;

		[SerializeField]
		private Color[] m_Colors;

		private const int kHueRes = 64;

		private const int kColorBoxSize = 32;

		[SerializeField]
		private Texture2D m_ColorBox;

		private static int s_Slider2Dhash = "Slider2D".GetHashCode();

		[SerializeField]
		private bool m_ShowPresets = true;

		[SerializeField]
		private bool m_IsOSColorPicker = false;

		[SerializeField]
		private bool m_ShowAlpha = true;

		[SerializeField]
		private Texture2D m_RTexture;

		private float m_RTextureG = -1f;

		private float m_RTextureB = -1f;

		[SerializeField]
		private Texture2D m_GTexture;

		private float m_GTextureR = -1f;

		private float m_GTextureB = -1f;

		[SerializeField]
		private Texture2D m_BTexture;

		private float m_BTextureR = -1f;

		private float m_BTextureG = -1f;

		[SerializeField]
		private Texture2D m_HueTexture;

		private float m_HueTextureS = -1f;

		private float m_HueTextureV = -1f;

		[SerializeField]
		private Texture2D m_SatTexture;

		private float m_SatTextureH = -1f;

		private float m_SatTextureV = -1f;

		[SerializeField]
		private Texture2D m_ValTexture;

		private float m_ValTextureH = -1f;

		private float m_ValTextureS = -1f;

		[NonSerialized]
		private int m_TextureColorBoxMode = -1;

		[SerializeField]
		private float m_LastConstant = -1f;

		[NonSerialized]
		private bool m_ColorSpaceBoxDirty;

		[SerializeField]
		private ColorPicker.ColorBoxMode m_ColorBoxMode = ColorPicker.ColorBoxMode.HSV;

		[SerializeField]
		private ColorPicker.SliderMode m_SliderMode = ColorPicker.SliderMode.HSV;

		[SerializeField]
		private Texture2D m_AlphaTexture;

		private float m_OldAlpha = -1f;

		[SerializeField]
		private GUIView m_DelegateView;

		private Action<Color> m_ColorChangedCallback;

		[SerializeField]
		private int m_ModalUndoGroup = -1;

		private float m_FloatSliderMaxOnMouseDown;

		private bool m_DraggingFloatSlider;

		private float m_ExposureSliderMax = 10f;

		private PresetLibraryEditor<ColorPresetLibrary> m_ColorLibraryEditor;

		private PresetLibraryEditorState m_ColorLibraryEditorState;

		private static ColorPicker s_Instance;

		private static Texture2D s_LeftGradientTexture;

		private static Texture2D s_RightGradientTexture;

		public static string presetsEditorPrefID
		{
			get
			{
				return "Color";
			}
		}

		public static Color color
		{
			get
			{
				return ColorPicker.instance.m_Color.exposureAdjustedColor;
			}
			set
			{
				ColorPicker.instance.SetColor(value);
				ColorPicker.instance.Repaint();
			}
		}

		public static bool visible
		{
			get
			{
				return ColorPicker.s_Instance != null;
			}
		}

		public static ColorPicker instance
		{
			get
			{
				if (!ColorPicker.s_Instance)
				{
					UnityEngine.Object[] array = Resources.FindObjectsOfTypeAll(typeof(ColorPicker));
					if (array != null && array.Length > 0)
					{
						ColorPicker.s_Instance = (ColorPicker)array[0];
					}
					if (!ColorPicker.s_Instance)
					{
						ColorPicker.s_Instance = ScriptableObject.CreateInstance<ColorPicker>();
						ColorPicker.s_Instance.wantsMouseMove = true;
					}
				}
				return ColorPicker.s_Instance;
			}
		}

		public static int originalKeyboardControl
		{
			get;
			private set;
		}

		public string currentPresetLibrary
		{
			get
			{
				this.InitializePresetsLibraryIfNeeded();
				return this.m_ColorLibraryEditor.currentLibraryWithoutExtension;
			}
			set
			{
				this.InitializePresetsLibraryIfNeeded();
				this.m_ColorLibraryEditor.currentLibraryWithoutExtension = value;
			}
		}

		private static void swap(ref float f1, ref float f2)
		{
			float num = f1;
			f1 = f2;
			f2 = num;
		}

		private Vector2 Slider2D(Rect rect, Vector2 value, Vector2 maxvalue, Vector2 minvalue, GUIStyle backStyle, GUIStyle thumbStyle)
		{
			Vector2 result;
			if (backStyle == null)
			{
				result = value;
			}
			else if (thumbStyle == null)
			{
				result = value;
			}
			else
			{
				int controlID = GUIUtility.GetControlID(ColorPicker.s_Slider2Dhash, FocusType.Passive);
				if (maxvalue.x < minvalue.x)
				{
					ColorPicker.swap(ref maxvalue.x, ref minvalue.x);
				}
				if (maxvalue.y < minvalue.y)
				{
					ColorPicker.swap(ref maxvalue.y, ref minvalue.y);
				}
				Event current = Event.current;
				switch (current.GetTypeForControl(controlID))
				{
				case EventType.MouseDown:
					if (rect.Contains(current.mousePosition))
					{
						GUIUtility.hotControl = controlID;
						GUIUtility.keyboardControl = 0;
						value.x = (current.mousePosition.x - rect.x) / rect.width * (maxvalue.x - minvalue.x);
						value.y = (current.mousePosition.y - rect.y) / rect.height * (maxvalue.y - minvalue.y);
						GUI.changed = true;
						Event.current.Use();
					}
					break;
				case EventType.MouseUp:
					if (GUIUtility.hotControl == controlID)
					{
						GUIUtility.hotControl = 0;
						current.Use();
					}
					break;
				case EventType.MouseDrag:
					if (GUIUtility.hotControl == controlID)
					{
						value.x = (current.mousePosition.x - rect.x) / rect.width * (maxvalue.x - minvalue.x);
						value.y = (current.mousePosition.y - rect.y) / rect.height * (maxvalue.y - minvalue.y);
						value.x = Mathf.Clamp(value.x, minvalue.x, maxvalue.x);
						value.y = Mathf.Clamp(value.y, minvalue.y, maxvalue.y);
						GUI.changed = true;
						Event.current.Use();
					}
					break;
				case EventType.Repaint:
				{
					backStyle.Draw(rect, GUIContent.none, controlID);
					Color color = GUI.color;
					GUI.color = ((VisionUtility.ComputePerceivedLuminance(ColorPicker.color) <= 0.5f) ? ColorPicker.Styles.lowLuminanceContentColor : ColorPicker.Styles.highLuminanceContentColor);
					Rect position = new Rect
					{
						size = thumbStyle.CalcSize(GUIContent.none),
						center = new Vector2(value.x / (maxvalue.x - minvalue.x) * rect.width + rect.x, value.y / (maxvalue.y - minvalue.y) * rect.height + rect.y)
					};
					thumbStyle.Draw(position, GUIContent.none, controlID);
					GUI.color = color;
					break;
				}
				}
				result = value;
			}
			return result;
		}

		private void RGBSliders()
		{
			float colorChannelNormalized = this.m_Color.GetColorChannelNormalized(RgbaChannel.R);
			float colorChannelNormalized2 = this.m_Color.GetColorChannelNormalized(RgbaChannel.G);
			float colorChannelNormalized3 = this.m_Color.GetColorChannelNormalized(RgbaChannel.B);
			this.m_RTexture = this.Update1DSlider(this.m_RTexture, 32, colorChannelNormalized2, colorChannelNormalized3, ref this.m_RTextureG, ref this.m_RTextureB, 0, false);
			this.m_GTexture = this.Update1DSlider(this.m_GTexture, 32, colorChannelNormalized, colorChannelNormalized3, ref this.m_GTextureR, ref this.m_GTextureB, 1, false);
			this.m_BTexture = this.Update1DSlider(this.m_BTexture, 32, colorChannelNormalized, colorChannelNormalized2, ref this.m_BTextureR, ref this.m_BTextureG, 2, false);
			this.RGBSlider("R", RgbaChannel.R, this.m_RTexture);
			GUILayout.Space(6f);
			this.RGBSlider("G", RgbaChannel.G, this.m_GTexture);
			GUILayout.Space(6f);
			this.RGBSlider("B", RgbaChannel.B, this.m_BTexture);
			GUILayout.Space(6f);
		}

		private void RGBSlider(string label, RgbaChannel channel, Texture2D sliderBackground)
		{
			ColorPicker.SliderMode sliderMode = this.m_SliderMode;
			if (sliderMode != ColorPicker.SliderMode.RGB)
			{
				if (sliderMode == ColorPicker.SliderMode.RGBFloat)
				{
					float num = this.m_Color.GetColorChannelHdr(channel);
					float maxColorComponent = this.m_Color.color.maxColorComponent;
					EventType type = Event.current.type;
					float num2 = (!this.m_HDR || this.m_Color.exposureAdjustedColor.maxColorComponent <= 1f) ? 1f : (this.m_Color.exposureAdjustedColor.maxColorComponent / maxColorComponent);
					float textFieldMax = (!this.m_HDR) ? 1f : 3.40282347E+38f;
					EditorGUI.BeginChangeCheck();
					num = EditorGUILayout.SliderWithTexture(GUIContent.Temp(label), num, 0f, (!this.m_DraggingFloatSlider) ? num2 : this.m_FloatSliderMaxOnMouseDown, EditorGUI.kFloatFieldFormatString, 0f, textFieldMax, sliderBackground, new GUILayoutOption[0]);
					if (type != EventType.MouseDown)
					{
						if (type == EventType.MouseUp)
						{
							this.m_DraggingFloatSlider = false;
						}
					}
					else
					{
						this.m_FloatSliderMaxOnMouseDown = num2;
						this.m_DraggingFloatSlider = true;
					}
					if (EditorGUI.EndChangeCheck())
					{
						this.m_Color.SetColorChannelHdr(channel, num);
						this.OnColorChanged(true);
					}
				}
			}
			else
			{
				float num = (float)this.m_Color.GetColorChannel(channel);
				EditorGUI.BeginChangeCheck();
				num = EditorGUILayout.SliderWithTexture(GUIContent.Temp(label), num, 0f, 255f, EditorGUI.kIntFieldFormatString, 0f, 255f, sliderBackground, new GUILayoutOption[0]);
				if (EditorGUI.EndChangeCheck())
				{
					this.m_Color.SetColorChannel(channel, num / 255f);
					this.OnColorChanged(true);
				}
				this.m_DraggingFloatSlider = false;
			}
		}

		private Texture2D Update1DSlider(Texture2D tex, int xSize, float const1, float const2, ref float oldConst1, ref float oldConst2, int idx, bool hsvSpace)
		{
			if (!tex || const1 != oldConst1 || const2 != oldConst2)
			{
				if (!tex)
				{
					tex = ColorPicker.MakeTexture(xSize, 2);
				}
				Color[] array = new Color[xSize * 2];
				Color topLeftColor = Color.black;
				Color black = Color.black;
				switch (idx)
				{
				case 0:
					topLeftColor = new Color(0f, const1, const2, 1f);
					black = new Color(1f, 0f, 0f, 0f);
					break;
				case 1:
					topLeftColor = new Color(const1, 0f, const2, 1f);
					black = new Color(0f, 1f, 0f, 0f);
					break;
				case 2:
					topLeftColor = new Color(const1, const2, 0f, 1f);
					black = new Color(0f, 0f, 1f, 0f);
					break;
				case 3:
					topLeftColor = this.m_Color.color;
					topLeftColor.a = 0f;
					black = new Color(0f, 0f, 0f, 1f);
					break;
				}
				ColorPicker.FillArea(xSize, 2, array, topLeftColor, black, new Color(0f, 0f, 0f, 0f));
				if (hsvSpace)
				{
					ColorPicker.HSVToRGBArray(array);
				}
				oldConst1 = const1;
				oldConst2 = const2;
				tex.SetPixels(array);
				tex.Apply();
			}
			return tex;
		}

		private void HSVSliders()
		{
			float num = this.m_Color.GetColorChannel(HsvChannel.H);
			float num2 = this.m_Color.GetColorChannel(HsvChannel.S);
			float num3 = this.m_Color.GetColorChannel(HsvChannel.V);
			this.m_HueTexture = this.Update1DSlider(this.m_HueTexture, 64, 1f, 1f, ref this.m_HueTextureS, ref this.m_HueTextureV, 0, true);
			this.m_SatTexture = this.Update1DSlider(this.m_SatTexture, 32, num, Mathf.Max(num3, 0.2f), ref this.m_SatTextureH, ref this.m_SatTextureV, 1, true);
			this.m_ValTexture = this.Update1DSlider(this.m_ValTexture, 32, num, num2, ref this.m_ValTextureH, ref this.m_ValTextureS, 2, true);
			EditorGUI.BeginChangeCheck();
			num = EditorGUILayout.SliderWithTexture(GUIContent.Temp("H"), num * 360f, 0f, 360f, EditorGUI.kIntFieldFormatString, this.m_HueTexture, new GUILayoutOption[0]);
			if (EditorGUI.EndChangeCheck())
			{
				this.m_Color.SetColorChannel(HsvChannel.H, num / 360f);
				this.OnColorChanged(true);
			}
			GUILayout.Space(6f);
			EditorGUI.BeginChangeCheck();
			num2 = EditorGUILayout.SliderWithTexture(GUIContent.Temp("S"), num2 * 100f, 0f, 100f, EditorGUI.kIntFieldFormatString, this.m_SatTexture, new GUILayoutOption[0]);
			if (EditorGUI.EndChangeCheck())
			{
				this.m_Color.SetColorChannel(HsvChannel.S, num2 / 100f);
				this.OnColorChanged(true);
			}
			GUILayout.Space(6f);
			EditorGUI.BeginChangeCheck();
			num3 = EditorGUILayout.SliderWithTexture(GUIContent.Temp("V"), num3 * 100f, 0f, 100f, EditorGUI.kIntFieldFormatString, this.m_ValTexture, new GUILayoutOption[0]);
			if (EditorGUI.EndChangeCheck())
			{
				this.m_Color.SetColorChannel(HsvChannel.V, num3 / 100f);
				this.OnColorChanged(true);
			}
			GUILayout.Space(6f);
		}

		private static void FillArea(int xSize, int ySize, Color[] retval, Color topLeftColor, Color rightGradient, Color downGradient)
		{
			Color b = new Color(0f, 0f, 0f, 0f);
			Color b2 = new Color(0f, 0f, 0f, 0f);
			if (xSize > 1)
			{
				b = rightGradient / (float)(xSize - 1);
			}
			if (ySize > 1)
			{
				b2 = downGradient / (float)(ySize - 1);
			}
			Color color = topLeftColor;
			int num = 0;
			for (int i = 0; i < ySize; i++)
			{
				Color color2 = color;
				for (int j = 0; j < xSize; j++)
				{
					retval[num++] = color2;
					color2 += b;
				}
				color += b2;
			}
		}

		private static void HSVToRGBArray(Color[] colors)
		{
			int num = colors.Length;
			for (int i = 0; i < num; i++)
			{
				Color color = colors[i];
				Color color2 = Color.HSVToRGB(color.r, color.g, color.b);
				color2.a = color.a;
				colors[i] = color2;
			}
		}

		public static Texture2D MakeTexture(int width, int height)
		{
			return new Texture2D(width, height, TextureFormat.RGBA32, false, true)
			{
				hideFlags = HideFlags.HideAndDontSave,
				wrapMode = TextureWrapMode.Clamp
			};
		}

		private void DrawColorSpaceBox(Rect colorBoxRect, float constantValue)
		{
			if (Event.current.type == EventType.Repaint)
			{
				if (this.m_ColorBoxMode != (ColorPicker.ColorBoxMode)this.m_TextureColorBoxMode)
				{
					int num = 32;
					int num2 = 32;
					if (this.m_ColorBox == null)
					{
						this.m_ColorBox = ColorPicker.MakeTexture(num, num2);
					}
					if (this.m_ColorBox.width != num || this.m_ColorBox.height != num2)
					{
						this.m_ColorBox.Resize(num, num2);
					}
				}
				if (this.m_ColorBoxMode != (ColorPicker.ColorBoxMode)this.m_TextureColorBoxMode || this.m_LastConstant != constantValue || this.m_ColorSpaceBoxDirty)
				{
					this.m_Colors = this.m_ColorBox.GetPixels(0);
					int width = this.m_ColorBox.width;
					int height = this.m_ColorBox.height;
					ColorPicker.FillArea(width, height, this.m_Colors, new Color(this.m_Color.GetColorChannel(HsvChannel.H), 0f, 0f, 1f), new Color(0f, 1f, 0f, 0f), new Color(0f, 0f, 1f, 0f));
					ColorPicker.HSVToRGBArray(this.m_Colors);
					this.m_ColorBox.SetPixels(this.m_Colors, 0);
					this.m_ColorBox.Apply(true);
					this.m_LastConstant = constantValue;
					this.m_TextureColorBoxMode = (int)this.m_ColorBoxMode;
				}
				Graphics.DrawTexture(colorBoxRect, this.m_ColorBox, new Rect(0.5f / (float)this.m_ColorBox.width, 0.5f / (float)this.m_ColorBox.height, 1f - 1f / (float)this.m_ColorBox.width, 1f - 1f / (float)this.m_ColorBox.height), 0, 0, 0, 0, Color.grey);
			}
		}

		private void InitializePresetsLibraryIfNeeded()
		{
			if (this.m_ColorLibraryEditorState == null)
			{
				this.m_ColorLibraryEditorState = new PresetLibraryEditorState(ColorPicker.presetsEditorPrefID);
				this.m_ColorLibraryEditorState.TransferEditorPrefsState(true);
			}
			if (this.m_ColorLibraryEditor == null)
			{
				ScriptableObjectSaveLoadHelper<ColorPresetLibrary> helper = new ScriptableObjectSaveLoadHelper<ColorPresetLibrary>("colors", SaveType.Text);
				this.m_ColorLibraryEditor = new PresetLibraryEditor<ColorPresetLibrary>(helper, this.m_ColorLibraryEditorState, new Action<int, object>(this.OnClickedPresetSwatch));
				this.m_ColorLibraryEditor.previewAspect = 1f;
				this.m_ColorLibraryEditor.minMaxPreviewHeight = new Vector2(14f, 14f);
				this.m_ColorLibraryEditor.settingsMenuRightMargin = 2f;
				this.m_ColorLibraryEditor.useOnePixelOverlappedGrid = true;
				this.m_ColorLibraryEditor.alwaysShowScrollAreaHorizontalLines = false;
				this.m_ColorLibraryEditor.marginsForGrid = new RectOffset(0, 0, 0, 0);
				this.m_ColorLibraryEditor.marginsForList = new RectOffset(0, 5, 2, 2);
				this.m_ColorLibraryEditor.InitializeGrid(233f - (float)(ColorPicker.Styles.background.padding.left + ColorPicker.Styles.background.padding.right));
			}
		}

		private void OnClickedPresetSwatch(int clickCount, object presetObject)
		{
			Color color = (Color)presetObject;
			if (!this.m_HDR && color.maxColorComponent > 1f)
			{
				color = new ColorMutator(color).color;
			}
			this.SetColor(color);
		}

		private void DoColorSwatchAndEyedropper()
		{
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			if (GUILayout.Button(ColorPicker.Styles.eyeDropper, GUIStyle.none, new GUILayoutOption[]
			{
				GUILayout.Width(40f),
				GUILayout.ExpandWidth(false)
			}))
			{
				GUIUtility.keyboardControl = 0;
				EyeDropper.Start(this.m_Parent, false);
				this.m_ColorBoxMode = ColorPicker.ColorBoxMode.EyeDropper;
				GUIUtility.ExitGUI();
			}
			Color exposureAdjustedColor = this.m_Color.exposureAdjustedColor;
			Rect rect = GUILayoutUtility.GetRect(ColorPicker.Styles.currentColorSwatchFill, ColorPicker.Styles.currentColorSwatch, new GUILayoutOption[]
			{
				GUILayout.ExpandWidth(true)
			});
			Vector2 size = ColorPicker.Styles.currentColorSwatch.CalcSize(ColorPicker.Styles.currentColorSwatchFill);
			Rect position = new Rect
			{
				size = size,
				y = rect.y,
				x = rect.xMax - size.x
			};
			Color backgroundColor = GUI.backgroundColor;
			Color contentColor = GUI.contentColor;
			int controlID = GUIUtility.GetControlID(FocusType.Passive);
			if (Event.current.type == EventType.Repaint)
			{
				GUI.backgroundColor = ((this.m_Color.exposureAdjustedColor.a != 1f) ? Color.white : Color.clear);
				GUI.contentColor = this.m_Color.exposureAdjustedColor;
				ColorPicker.Styles.currentColorSwatch.Draw(position, ColorPicker.Styles.currentColorSwatchFill, controlID);
			}
			position.x -= position.width;
			GUI.backgroundColor = ((this.m_Color.originalColor.a != 1f) ? Color.white : Color.clear);
			GUI.contentColor = this.m_Color.originalColor;
			if (GUI.Button(position, ColorPicker.Styles.originalColorSwatchFill, ColorPicker.Styles.originalColorSwatch))
			{
				this.m_Color.Reset();
				Event.current.Use();
				this.OnColorChanged(true);
			}
			GUI.backgroundColor = backgroundColor;
			GUI.contentColor = contentColor;
			GUILayout.EndHorizontal();
		}

		private void DoColorSpaceGUI()
		{
			Vector2 vector = ColorPicker.Styles.hueDialBackground.CalcSize(GUIContent.none);
			Rect rect = GUILayoutUtility.GetRect(vector.x, vector.y);
			ColorPicker.ColorBoxMode colorBoxMode = this.m_ColorBoxMode;
			if (colorBoxMode != ColorPicker.ColorBoxMode.HSV)
			{
				if (colorBoxMode == ColorPicker.ColorBoxMode.EyeDropper)
				{
					EyeDropper.DrawPreview(rect);
				}
			}
			else
			{
				rect.x += (rect.width - rect.height) * 0.5f;
				rect.width = rect.height;
				float num = this.m_Color.GetColorChannel(HsvChannel.H);
				Color contentColor = GUI.contentColor;
				GUI.contentColor = Color.HSVToRGB(num, 1f, 1f);
				EditorGUI.BeginChangeCheck();
				num = EditorGUI.AngularDial(rect, GUIContent.none, num * 360f, ColorPicker.Styles.hueDialThumbFill.image, ColorPicker.Styles.hueDialBackground, ColorPicker.Styles.hueDialThumb);
				if (EditorGUI.EndChangeCheck())
				{
					num = Mathf.Repeat(num, 360f) / 360f;
					this.m_Color.SetColorChannel(HsvChannel.H, num);
					this.OnColorChanged(true);
				}
				GUI.contentColor = contentColor;
				float num2 = rect.width * 0.5f - ColorPicker.Styles.hueDialThumbSize;
				int num3 = Mathf.FloorToInt(Mathf.Sqrt(2f) * num2);
				if ((num3 & 1) == 1)
				{
					num3++;
				}
				Rect rect2 = new Rect
				{
					size = Vector2.one * (float)num3,
					center = rect.center
				};
				rect2 = ColorPicker.Styles.colorBoxPadding.Remove(rect2);
				this.DrawColorSpaceBox(rect2, this.m_Color.GetColorChannel(HsvChannel.H));
				EditorGUI.BeginChangeCheck();
				Vector2 value = new Vector2(this.m_Color.GetColorChannel(HsvChannel.S), 1f - this.m_Color.GetColorChannel(HsvChannel.V));
				value = this.Slider2D(rect2, value, Vector2.zero, Vector2.one, GUIStyle.none, ColorPicker.Styles.colorBoxThumb);
				if (EditorGUI.EndChangeCheck())
				{
					this.m_Color.SetColorChannel(HsvChannel.S, value.x);
					this.m_Color.SetColorChannel(HsvChannel.V, 1f - value.y);
					this.OnColorChanged(true);
				}
			}
		}

		private void DoColorSliders(float availableWidth)
		{
			float labelWidth = EditorGUIUtility.labelWidth;
			float fieldWidth = EditorGUIUtility.fieldWidth;
			EditorGUIUtility.labelWidth = availableWidth - 72f;
			EditorGUIUtility.fieldWidth = 72f;
			this.m_SliderMode = (ColorPicker.SliderMode)EditorGUILayout.IntPopup(GUIContent.Temp(" "), (int)this.m_SliderMode, ColorPicker.Styles.sliderModeLabels, ColorPicker.Styles.sliderModeValues, new GUILayoutOption[0]);
			GUILayout.Space(6f);
			EditorGUIUtility.labelWidth = labelWidth;
			EditorGUIUtility.fieldWidth = fieldWidth;
			EditorGUIUtility.labelWidth = 14f;
			ColorPicker.SliderMode sliderMode = this.m_SliderMode;
			if (sliderMode != ColorPicker.SliderMode.HSV)
			{
				this.RGBSliders();
			}
			else
			{
				this.HSVSliders();
			}
			if (this.m_ShowAlpha)
			{
				this.m_AlphaTexture = this.Update1DSlider(this.m_AlphaTexture, 32, 0f, 0f, ref this.m_OldAlpha, ref this.m_OldAlpha, 3, false);
				float num = 1f;
				string formatString = EditorGUI.kFloatFieldFormatString;
				ColorPicker.SliderMode sliderMode2 = this.m_SliderMode;
				if (sliderMode2 != ColorPicker.SliderMode.HSV)
				{
					if (sliderMode2 == ColorPicker.SliderMode.RGB)
					{
						num = 255f;
						formatString = EditorGUI.kIntFieldFormatString;
					}
				}
				else
				{
					num = 100f;
					formatString = EditorGUI.kIntFieldFormatString;
				}
				Rect controlRect = EditorGUILayout.GetControlRect(true, new GUILayoutOption[0]);
				if (Event.current.type == EventType.Repaint)
				{
					Rect rect = controlRect;
					rect.xMin += EditorGUIUtility.labelWidth;
					rect.xMax -= EditorGUIUtility.fieldWidth + 5f;
					rect = ColorPicker.Styles.sliderBackground.padding.Remove(rect);
					Rect sourceRect = new Rect
					{
						x = 0f,
						y = 0f,
						width = rect.width / rect.height,
						height = 1f
					};
					Graphics.DrawTexture(rect, ColorPicker.Styles.alphaSliderCheckerBackground, sourceRect, 0, 0, 0, 0);
				}
				EditorGUI.BeginChangeCheck();
				float num2 = this.m_Color.GetColorChannelNormalized(RgbaChannel.A) * num;
				num2 = EditorGUI.SliderWithTexture(controlRect, GUIContent.Temp("A"), num2, 0f, num, formatString, this.m_AlphaTexture, new GUILayoutOption[0]);
				if (EditorGUI.EndChangeCheck())
				{
					this.m_Color.SetColorChannel(RgbaChannel.A, num2 / num);
					this.OnColorChanged(true);
				}
				GUILayout.Space(6f);
			}
			EditorGUIUtility.labelWidth = labelWidth;
		}

		private void DoHexField(float availableWidth)
		{
			float labelWidth = EditorGUIUtility.labelWidth;
			float fieldWidth = EditorGUIUtility.fieldWidth;
			EditorGUIUtility.labelWidth = availableWidth - 72f;
			EditorGUIUtility.fieldWidth = 72f;
			EditorGUI.BeginChangeCheck();
			Color32 color = EditorGUILayout.HexColorTextField(ColorPicker.Styles.hexLabel, this.m_Color.color, false, new GUILayoutOption[0]);
			if (EditorGUI.EndChangeCheck())
			{
				this.m_Color.SetColorChannel(RgbaChannel.R, color.r);
				this.m_Color.SetColorChannel(RgbaChannel.G, color.g);
				this.m_Color.SetColorChannel(RgbaChannel.B, color.b);
				this.OnColorChanged(true);
			}
			EditorGUIUtility.labelWidth = labelWidth;
			EditorGUIUtility.fieldWidth = fieldWidth;
		}

		private void DoExposureSlider()
		{
			float labelWidth = EditorGUIUtility.labelWidth;
			EditorGUIUtility.labelWidth = EditorStyles.label.CalcSize(ColorPicker.Styles.exposureValue).x + (float)EditorStyles.label.margin.right;
			Rect rect = GUILayoutUtility.GetRect(0f, EditorGUIUtility.singleLineHeight);
			EditorGUI.BeginChangeCheck();
			float exposureValue = EditorGUI.Slider(rect, ColorPicker.Styles.exposureValue, this.m_Color.exposureValue, -this.m_ExposureSliderMax, this.m_ExposureSliderMax, -3.40282347E+38f, 3.40282347E+38f);
			if (EditorGUI.EndChangeCheck())
			{
				this.m_Color.exposureValue = exposureValue;
				this.OnColorChanged(true);
			}
			EditorGUIUtility.labelWidth = labelWidth;
		}

		private void DoExposureSwatches()
		{
			Rect rect = GUILayoutUtility.GetRect(GUIContent.none, ColorPicker.Styles.exposureSwatch, new GUILayoutOption[]
			{
				GUILayout.ExpandWidth(true)
			});
			int num = 5;
			Rect position = new Rect
			{
				x = rect.x + (rect.width - (float)num * ColorPicker.Styles.exposureSwatch.fixedWidth) * 0.5f,
				y = rect.y,
				width = ColorPicker.Styles.exposureSwatch.fixedWidth,
				height = ColorPicker.Styles.exposureSwatch.fixedHeight
			};
			Color backgroundColor = GUI.backgroundColor;
			Color contentColor = GUI.contentColor;
			for (int i = 0; i < num; i++)
			{
				int num2 = i - num / 2;
				Color color = this.m_Color.exposureAdjustedColor * Mathf.Pow(2f, (float)num2);
				color.a = 1f;
				GUI.backgroundColor = color;
				GUI.contentColor = ((VisionUtility.ComputePerceivedLuminance(color) >= 0.5f) ? ColorPicker.Styles.highLuminanceContentColor : ColorPicker.Styles.lowLuminanceContentColor);
				if (GUI.Button(position, GUIContent.Temp((num2 != 0) ? ((num2 >= 0) ? string.Format("+{0}", num2) : num2.ToString()) : null), ColorPicker.Styles.exposureSwatch))
				{
					this.m_Color.exposureValue = Mathf.Clamp(this.m_Color.exposureValue + (float)num2, -this.m_ExposureSliderMax, this.m_ExposureSliderMax);
					this.OnColorChanged(true);
				}
				if (num2 == 0 && Event.current.type == EventType.Repaint)
				{
					GUI.backgroundColor = GUI.contentColor;
					ColorPicker.Styles.selectedExposureSwatchStroke.Draw(position, false, false, false, false);
				}
				position.x += position.width;
			}
			GUI.backgroundColor = backgroundColor;
			GUI.contentColor = contentColor;
		}

		private void DoPresetsGUI()
		{
			Rect rect = GUILayoutUtility.GetRect(ColorPicker.Styles.presetsToggle, EditorStyles.foldout);
			rect.xMax -= 17f;
			this.m_ShowPresets = EditorGUI.Foldout(rect, this.m_ShowPresets, ColorPicker.Styles.presetsToggle);
			if (this.m_ShowPresets)
			{
				GUILayout.Space(-(EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing));
				Rect rect2 = GUILayoutUtility.GetRect(0f, Mathf.Clamp(this.m_ColorLibraryEditor.contentHeight, 20f, 250f));
				this.m_ColorLibraryEditor.OnGUI(rect2, ColorPicker.color);
			}
		}

		private void OnGUI()
		{
			this.InitializePresetsLibraryIfNeeded();
			EventType type = Event.current.type;
			if (type == EventType.ExecuteCommand)
			{
				string commandName = Event.current.commandName;
				if (commandName != null)
				{
					if (!(commandName == "EyeDropperUpdate"))
					{
						if (!(commandName == "EyeDropperClicked"))
						{
							if (commandName == "EyeDropperCancelled")
							{
								this.OnEyedropperCancelled();
							}
						}
						else
						{
							this.m_ColorBoxMode = ColorPicker.ColorBoxMode.HSV;
							Color lastPickedColor = EyeDropper.GetLastPickedColor();
							this.m_Color.SetColorChannelHdr(RgbaChannel.R, lastPickedColor.r);
							this.m_Color.SetColorChannelHdr(RgbaChannel.G, lastPickedColor.g);
							this.m_Color.SetColorChannelHdr(RgbaChannel.B, lastPickedColor.b);
							this.m_Color.SetColorChannelHdr(RgbaChannel.A, lastPickedColor.a);
							this.OnColorChanged(true);
						}
					}
					else
					{
						base.Repaint();
					}
				}
			}
			Rect rect = EditorGUILayout.BeginVertical(ColorPicker.Styles.background, new GUILayoutOption[0]);
			float width = EditorGUILayout.GetControlRect(false, 1f, EditorStyles.numberField, new GUILayoutOption[0]).width;
			EditorGUIUtility.labelWidth = width - 45f;
			EditorGUIUtility.fieldWidth = 45f;
			GUILayout.Space(10f);
			this.DoColorSwatchAndEyedropper();
			GUILayout.Space(10f);
			this.DoColorSpaceGUI();
			GUILayout.Space(10f);
			this.DoColorSliders(width);
			this.DoHexField(width);
			GUILayout.Space(6f);
			if (this.m_HDR)
			{
				this.DoExposureSlider();
				GUILayout.Space(6f);
				this.DoExposureSwatches();
				GUILayout.Space(6f);
			}
			this.DoPresetsGUI();
			this.HandleCopyPasteEvents();
			EditorGUILayout.EndVertical();
			if (rect.height > 0f && Event.current.type == EventType.Repaint)
			{
				this.SetHeight(rect.height);
			}
			if (Event.current.type == EventType.KeyDown)
			{
				KeyCode keyCode = Event.current.keyCode;
				if (keyCode != KeyCode.Escape)
				{
					if (keyCode == KeyCode.Return || keyCode == KeyCode.KeypadEnter)
					{
						base.Close();
					}
				}
				else if (this.m_ColorBoxMode == ColorPicker.ColorBoxMode.EyeDropper)
				{
					EyeDropper.End();
					this.OnEyedropperCancelled();
				}
				else
				{
					Undo.RevertAllDownToGroup(this.m_ModalUndoGroup);
					this.m_Color.Reset();
					this.OnColorChanged(false);
					base.Close();
					GUIUtility.ExitGUI();
				}
			}
			if ((Event.current.type == EventType.MouseDown && Event.current.button != 1) || Event.current.type == EventType.ContextClick)
			{
				GUIUtility.keyboardControl = 0;
				base.Repaint();
			}
		}

		private void OnEyedropperCancelled()
		{
			base.Repaint();
			this.m_ColorBoxMode = ColorPicker.ColorBoxMode.HSV;
		}

		private void SetHeight(float newHeight)
		{
			if (newHeight != base.position.height)
			{
				base.minSize = new Vector2(233f, newHeight);
				base.maxSize = new Vector2(233f, newHeight);
			}
		}

		private void HandleCopyPasteEvents()
		{
			Event current = Event.current;
			EventType type = current.type;
			if (type != EventType.ValidateCommand)
			{
				if (type == EventType.ExecuteCommand)
				{
					string commandName = current.commandName;
					if (commandName != null)
					{
						if (!(commandName == "Copy"))
						{
							if (commandName == "Paste")
							{
								Color color;
								if (ColorClipboard.TryGetColor(this.m_HDR, out color))
								{
									if (!this.m_ShowAlpha)
									{
										color.a = this.m_Color.GetColorChannelNormalized(RgbaChannel.A);
									}
									this.SetColor(color);
									GUI.changed = true;
									current.Use();
								}
							}
						}
						else
						{
							ColorClipboard.SetColor(ColorPicker.color);
							current.Use();
						}
					}
				}
			}
			else
			{
				string commandName2 = current.commandName;
				if (commandName2 != null)
				{
					if (commandName2 == "Copy" || commandName2 == "Paste")
					{
						current.Use();
					}
				}
			}
		}

		public static Texture2D GetGradientTextureWithAlpha1To0()
		{
			Texture2D arg_51_0;
			if ((arg_51_0 = ColorPicker.s_LeftGradientTexture) == null)
			{
				arg_51_0 = (ColorPicker.s_LeftGradientTexture = ColorPicker.CreateGradientTexture("ColorPicker_1To0_Gradient", 4, 4, new Color(1f, 1f, 1f, 1f), new Color(1f, 1f, 1f, 0f)));
			}
			return arg_51_0;
		}

		public static Texture2D GetGradientTextureWithAlpha0To1()
		{
			Texture2D arg_51_0;
			if ((arg_51_0 = ColorPicker.s_RightGradientTexture) == null)
			{
				arg_51_0 = (ColorPicker.s_RightGradientTexture = ColorPicker.CreateGradientTexture("ColorPicker_0To1_Gradient", 4, 4, new Color(1f, 1f, 1f, 0f), new Color(1f, 1f, 1f, 1f)));
			}
			return arg_51_0;
		}

		private static Texture2D CreateGradientTexture(string name, int width, int height, Color leftColor, Color rightColor)
		{
			Texture2D texture2D = new Texture2D(width, height, TextureFormat.RGBA32, false, true);
			texture2D.name = name;
			texture2D.hideFlags = HideFlags.HideAndDontSave;
			Color[] array = new Color[width * height];
			for (int i = 0; i < width; i++)
			{
				Color color = Color.Lerp(leftColor, rightColor, (float)i / (float)(width - 1));
				for (int j = 0; j < height; j++)
				{
					array[j * width + i] = color;
				}
			}
			texture2D.SetPixels(array);
			texture2D.wrapMode = TextureWrapMode.Clamp;
			texture2D.Apply();
			return texture2D;
		}

		private void OnColorChanged(bool exitGUI = true)
		{
			this.m_OldAlpha = -1f;
			this.m_ColorSpaceBoxDirty = true;
			this.m_ExposureSliderMax = Mathf.Max(this.m_ExposureSliderMax, this.m_Color.exposureValue);
			if (this.m_DelegateView != null)
			{
				Event e = EditorGUIUtility.CommandEvent("ColorPickerChanged");
				if (!this.m_IsOSColorPicker)
				{
					base.Repaint();
				}
				this.m_DelegateView.SendEvent(e);
				if (!this.m_IsOSColorPicker && exitGUI)
				{
					GUIUtility.ExitGUI();
				}
			}
			if (this.m_ColorChangedCallback != null)
			{
				this.m_ColorChangedCallback(ColorPicker.color);
			}
		}

		private void SetColor(Color c)
		{
			if (this.m_IsOSColorPicker)
			{
				OSColorPicker.color = c;
			}
			else
			{
				this.m_Color.SetColorChannelHdr(RgbaChannel.R, c.r);
				this.m_Color.SetColorChannelHdr(RgbaChannel.G, c.g);
				this.m_Color.SetColorChannelHdr(RgbaChannel.B, c.b);
				this.m_Color.SetColorChannelHdr(RgbaChannel.A, c.a);
				this.OnColorChanged(true);
				base.Repaint();
			}
		}

		public static void Show(GUIView viewToUpdate, Color col, bool showAlpha = true, bool hdr = false)
		{
			ColorPicker.Show(viewToUpdate, null, col, showAlpha, hdr);
		}

		public static void Show(Action<Color> colorChangedCallback, Color col, bool showAlpha = true, bool hdr = false)
		{
			ColorPicker.Show(null, colorChangedCallback, col, showAlpha, hdr);
		}

		private static void Show(GUIView viewToUpdate, Action<Color> colorChangedCallback, Color col, bool showAlpha, bool hdr)
		{
			ColorPicker instance = ColorPicker.instance;
			instance.m_HDR = hdr;
			instance.m_Color = new ColorMutator(col);
			instance.m_ShowAlpha = showAlpha;
			instance.m_DelegateView = viewToUpdate;
			instance.m_ColorChangedCallback = colorChangedCallback;
			instance.m_ModalUndoGroup = Undo.GetCurrentGroup();
			instance.m_ExposureSliderMax = Mathf.Max(instance.m_ExposureSliderMax, instance.m_Color.exposureValue);
			ColorPicker.originalKeyboardControl = GUIUtility.keyboardControl;
			if (instance.m_HDR)
			{
				instance.m_IsOSColorPicker = false;
			}
			if (instance.m_IsOSColorPicker)
			{
				OSColorPicker.Show(showAlpha);
			}
			else
			{
				instance.titleContent = ((!hdr) ? EditorGUIUtility.TrTextContent("Color", null, null) : EditorGUIUtility.TrTextContent("HDR Color", null, null));
				float y = (float)EditorPrefs.GetInt("CPickerHeight", (int)instance.position.height);
				instance.minSize = new Vector2(233f, y);
				instance.maxSize = new Vector2(233f, y);
				instance.InitializePresetsLibraryIfNeeded();
				instance.ShowAuxWindow();
			}
		}

		private void PollOSColorPicker()
		{
			if (this.m_IsOSColorPicker)
			{
				if (!OSColorPicker.visible || Application.platform != RuntimePlatform.OSXEditor)
				{
					UnityEngine.Object.DestroyImmediate(this);
				}
				else
				{
					Color color = OSColorPicker.color;
					if (this.m_Color.color != color)
					{
						this.m_Color.SetColorChannel(RgbaChannel.R, color.r);
						this.m_Color.SetColorChannel(RgbaChannel.G, color.g);
						this.m_Color.SetColorChannel(RgbaChannel.B, color.b);
						this.m_Color.SetColorChannel(RgbaChannel.A, color.a);
						this.OnColorChanged(true);
					}
				}
			}
		}

		private void OnEnable()
		{
			base.hideFlags = HideFlags.DontSave;
			this.m_IsOSColorPicker = EditorPrefs.GetBool("UseOSColorPicker");
			base.hideFlags = HideFlags.DontSave;
			EditorApplication.update = (EditorApplication.CallbackFunction)Delegate.Combine(EditorApplication.update, new EditorApplication.CallbackFunction(this.PollOSColorPicker));
			EditorGUIUtility.editingTextField = true;
			this.m_SliderMode = (ColorPicker.SliderMode)EditorPrefs.GetInt("CPSliderMode", 0);
			this.m_ShowPresets = (EditorPrefs.GetInt("CPPresetsShow", 1) != 0);
		}

		private void OnDisable()
		{
			EditorPrefs.SetInt("CPSliderMode", (int)this.m_SliderMode);
			EditorPrefs.SetInt("CPColorMode", (int)this.m_ColorBoxMode);
			EditorPrefs.SetInt("CPPresetsShow", (!this.m_ShowPresets) ? 0 : 1);
			EditorPrefs.SetInt("CPickerHeight", (int)base.position.height);
		}

		public void OnDestroy()
		{
			Undo.CollapseUndoOperations(this.m_ModalUndoGroup);
			if (this.m_ColorSlider)
			{
				UnityEngine.Object.DestroyImmediate(this.m_ColorSlider);
			}
			if (this.m_ColorBox)
			{
				UnityEngine.Object.DestroyImmediate(this.m_ColorBox);
			}
			if (this.m_RTexture)
			{
				UnityEngine.Object.DestroyImmediate(this.m_RTexture);
			}
			if (this.m_GTexture)
			{
				UnityEngine.Object.DestroyImmediate(this.m_GTexture);
			}
			if (this.m_BTexture)
			{
				UnityEngine.Object.DestroyImmediate(this.m_BTexture);
			}
			if (this.m_HueTexture)
			{
				UnityEngine.Object.DestroyImmediate(this.m_HueTexture);
			}
			if (this.m_SatTexture)
			{
				UnityEngine.Object.DestroyImmediate(this.m_SatTexture);
			}
			if (this.m_ValTexture)
			{
				UnityEngine.Object.DestroyImmediate(this.m_ValTexture);
			}
			if (this.m_AlphaTexture)
			{
				UnityEngine.Object.DestroyImmediate(this.m_AlphaTexture);
			}
			ColorPicker.s_Instance = null;
			if (this.m_IsOSColorPicker)
			{
				OSColorPicker.Close();
			}
			EditorApplication.update = (EditorApplication.CallbackFunction)Delegate.Remove(EditorApplication.update, new EditorApplication.CallbackFunction(this.PollOSColorPicker));
			if (this.m_ColorLibraryEditorState != null)
			{
				this.m_ColorLibraryEditorState.TransferEditorPrefsState(false);
			}
			if (this.m_ColorLibraryEditor != null)
			{
				this.m_ColorLibraryEditor.UnloadUsedLibraries();
			}
			GUIUtility.keyboardControl = ColorPicker.originalKeyboardControl;
		}
	}
}
