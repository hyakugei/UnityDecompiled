using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.AnimatedValues;
using UnityEditor.Build;
using UnityEditor.Experimental.AssetImporters;
using UnityEditor.Modules;
using UnityEngine;
using UnityEngine.Events;

namespace UnityEditor
{
	[CanEditMultipleObjects, CustomEditor(typeof(TextureImporter))]
	internal class TextureImporterInspector : AssetImporterEditor
	{
		[Flags]
		private enum TextureInspectorGUIElement
		{
			None = 0,
			PowerOfTwo = 1,
			Readable = 2,
			AlphaHandling = 4,
			ColorSpace = 8,
			MipMaps = 16,
			NormalMap = 32,
			Sprite = 64,
			Cookie = 128,
			CubeMapConvolution = 256,
			CubeMapping = 512,
			SingleChannelComponent = 2048
		}

		private struct TextureInspectorTypeGUIProperties
		{
			public TextureImporterInspector.TextureInspectorGUIElement commonElements;

			public TextureImporterInspector.TextureInspectorGUIElement advancedElements;

			public TextureImporterShape shapeCaps;

			public TextureInspectorTypeGUIProperties(TextureImporterInspector.TextureInspectorGUIElement _commonElements, TextureImporterInspector.TextureInspectorGUIElement _advancedElements, TextureImporterShape _shapeCaps)
			{
				this.commonElements = _commonElements;
				this.advancedElements = _advancedElements;
				this.shapeCaps = _shapeCaps;
			}
		}

		private delegate void GUIMethod(TextureImporterInspector.TextureInspectorGUIElement guiElements);

		private enum CookieMode
		{
			Spot,
			Directional,
			Point
		}

		internal class Styles
		{
			public readonly GUIContent textureTypeTitle = EditorGUIUtility.TrTextContent("Texture Type", "What will this texture be used for?", null);

			public readonly GUIContent[] textureTypeOptions = new GUIContent[]
			{
				EditorGUIUtility.TrTextContent("Default", "Texture is a normal image such as a diffuse texture or other.", null),
				EditorGUIUtility.TrTextContent("Normal map", "Texture is a bump or normal map.", null),
				EditorGUIUtility.TrTextContent("Editor GUI and Legacy GUI", "Texture is used for a GUI element.", null),
				EditorGUIUtility.TrTextContent("Sprite (2D and UI)", "Texture is used for a sprite.", null),
				EditorGUIUtility.TrTextContent("Cursor", "Texture is used for a cursor.", null),
				EditorGUIUtility.TrTextContent("Cookie", "Texture is a cookie you put on a light.", null),
				EditorGUIUtility.TrTextContent("Lightmap", "Texture is a lightmap.", null),
				EditorGUIUtility.TrTextContent("Single Channel", "Texture is a one component texture.", null)
			};

			public readonly int[] textureTypeValues = new int[]
			{
				0,
				1,
				2,
				8,
				7,
				4,
				6,
				10
			};

			public readonly GUIContent textureShape = EditorGUIUtility.TrTextContent("Texture Shape", "What shape is this texture?", null);

			private readonly GUIContent textureShape2D = EditorGUIUtility.TrTextContent("2D", "Texture is 2D.", null);

			private readonly GUIContent textureShapeCube = EditorGUIUtility.TrTextContent("Cube", "Texture is a Cubemap.", null);

			public readonly Dictionary<TextureImporterShape, GUIContent[]> textureShapeOptionsDictionnary = new Dictionary<TextureImporterShape, GUIContent[]>();

			public readonly Dictionary<TextureImporterShape, int[]> textureShapeValuesDictionnary = new Dictionary<TextureImporterShape, int[]>();

			public readonly GUIContent filterMode = EditorGUIUtility.TrTextContent("Filter Mode", null, null);

			public readonly GUIContent[] filterModeOptions = new GUIContent[]
			{
				EditorGUIUtility.TrTextContent("Point (no filter)", null, null),
				EditorGUIUtility.TrTextContent("Bilinear", null, null),
				EditorGUIUtility.TrTextContent("Trilinear", null, null)
			};

			public readonly GUIContent cookieType = EditorGUIUtility.TrTextContent("Light Type", null, null);

			public readonly GUIContent[] cookieOptions = new GUIContent[]
			{
				EditorGUIUtility.TrTextContent("Spotlight", null, null),
				EditorGUIUtility.TrTextContent("Directional", null, null),
				EditorGUIUtility.TrTextContent("Point", null, null)
			};

			public readonly GUIContent generateFromBump = EditorGUIUtility.TrTextContent("Create from Grayscale", "The grayscale of the image is used as a heightmap for generating the normal map.", null);

			public readonly GUIContent bumpiness = EditorGUIUtility.TrTextContent("Bumpiness", null, null);

			public readonly GUIContent bumpFiltering = EditorGUIUtility.TrTextContent("Filtering", null, null);

			public readonly GUIContent[] bumpFilteringOptions = new GUIContent[]
			{
				EditorGUIUtility.TrTextContent("Sharp", null, null),
				EditorGUIUtility.TrTextContent("Smooth", null, null)
			};

			public readonly GUIContent cubemap = EditorGUIUtility.TrTextContent("Mapping", null, null);

			public readonly GUIContent[] cubemapOptions = new GUIContent[]
			{
				EditorGUIUtility.TrTextContent("Auto", null, null),
				EditorGUIUtility.TrTextContent("6 Frames Layout (Cubic Environment)", "Texture contains 6 images arranged in one of the standard cubemap layouts - cross or sequence (+x,-x, +y, -y, +z, -z). Texture can be in vertical or horizontal orientation.", null),
				EditorGUIUtility.TrTextContent("Latitude-Longitude Layout (Cylindrical)", "Texture contains an image of a ball unwrapped such that latitude and longitude are mapped to horizontal and vertical dimensions (as on a globe).", null),
				EditorGUIUtility.TrTextContent("Mirrored Ball (Spheremap)", "Texture contains an image of a mirrored ball.", null)
			};

			public readonly int[] cubemapValues2 = new int[]
			{
				6,
				5,
				2,
				1
			};

			public readonly GUIContent cubemapConvolution = EditorGUIUtility.TrTextContent("Convolution Type", null, null);

			public readonly GUIContent[] cubemapConvolutionOptions = new GUIContent[]
			{
				EditorGUIUtility.TrTextContent("None", null, null),
				EditorGUIUtility.TrTextContent("Specular (Glossy Reflection)", "Convolve cubemap for specular reflections with varying smoothness (Glossy Reflections).", null),
				EditorGUIUtility.TrTextContent("Diffuse (Irradiance)", "Convolve cubemap for diffuse-only reflection (Irradiance Cubemap).", null)
			};

			public readonly int[] cubemapConvolutionValues = new int[]
			{
				0,
				1,
				2
			};

			public readonly GUIContent seamlessCubemap = EditorGUIUtility.TrTextContent("Fixup Edge Seams", "Enable if this texture is used for glossy reflections.", null);

			public readonly GUIContent textureFormat = EditorGUIUtility.TrTextContent("Format", null, null);

			public readonly GUIContent defaultPlatform = EditorGUIUtility.TrTextContent("Default", null, null);

			public readonly GUIContent mipmapFadeOutToggle = EditorGUIUtility.TrTextContent("Fadeout Mip Maps", null, null);

			public readonly GUIContent mipmapFadeOut = EditorGUIUtility.TrTextContent("Fade Range", null, null);

			public readonly GUIContent readWrite = EditorGUIUtility.TrTextContent("Read/Write Enabled", "Enable to be able to access the raw pixel data from code.", null);

			public readonly GUIContent alphaSource = EditorGUIUtility.TrTextContent("Alpha Source", "How is the alpha generated for the imported texture.", null);

			public readonly GUIContent[] alphaSourceOptions = new GUIContent[]
			{
				EditorGUIUtility.TrTextContent("None", "No Alpha will be used.", null),
				EditorGUIUtility.TrTextContent("Input Texture Alpha", "Use Alpha from the input texture if one is provided.", null),
				EditorGUIUtility.TrTextContent("From Gray Scale", "Generate Alpha from image gray scale.", null)
			};

			public readonly int[] alphaSourceValues = new int[]
			{
				0,
				1,
				2
			};

			public readonly GUIContent singleChannelComponent = EditorGUIUtility.TrTextContent("Channel", "As which color/alpha component the single channel texture is treated.", null);

			public readonly GUIContent[] singleChannelComponentOptions = new GUIContent[]
			{
				EditorGUIUtility.TrTextContent("Alpha", "Use the alpha channel (compression not supported).", null),
				EditorGUIUtility.TrTextContent("Red", "Use the red color component.", null)
			};

			public readonly int[] singleChannelComponentValues = new int[]
			{
				0,
				1
			};

			public readonly GUIContent generateMipMaps = EditorGUIUtility.TrTextContent("Generate Mip Maps", null, null);

			public readonly GUIContent sRGBTexture = EditorGUIUtility.TrTextContent("sRGB (Color Texture)", "Texture content is stored in gamma space. Non-HDR color textures should enable this flag (except if used for IMGUI).", null);

			public readonly GUIContent borderMipMaps = EditorGUIUtility.TrTextContent("Border Mip Maps", null, null);

			public readonly GUIContent mipMapsPreserveCoverage = EditorGUIUtility.TrTextContent("Mip Maps Preserve Coverage", "The alpha channel of generated Mip Maps will preserve coverage during the alpha test.", null);

			public readonly GUIContent alphaTestReferenceValue = EditorGUIUtility.TrTextContent("Alpha Cutoff Value", "The reference value used during the alpha test. Controls Mip Map coverage.", null);

			public readonly GUIContent mipMapFilter = EditorGUIUtility.TrTextContent("Mip Map Filtering", null, null);

			public readonly GUIContent[] mipMapFilterOptions = new GUIContent[]
			{
				EditorGUIUtility.TrTextContent("Box", null, null),
				EditorGUIUtility.TrTextContent("Kaiser", null, null)
			};

			public readonly GUIContent npot = EditorGUIUtility.TrTextContent("Non Power of 2", "How non-power-of-two textures are scaled on import.", null);

			public readonly GUIContent generateCubemap = EditorGUIUtility.TrTextContent("Generate Cubemap", null, null);

			public readonly GUIContent compressionQuality = EditorGUIUtility.TrTextContent("Compressor Quality", null, null);

			public readonly GUIContent compressionQualitySlider = EditorGUIUtility.TrTextContent("Compressor Quality", "Use the slider to adjust compression quality from 0 (Fastest) to 100 (Best)", null);

			public readonly GUIContent[] mobileCompressionQualityOptions = new GUIContent[]
			{
				EditorGUIUtility.TrTextContent("Fast", null, null),
				EditorGUIUtility.TrTextContent("Normal", null, null),
				EditorGUIUtility.TrTextContent("Best", null, null)
			};

			public readonly GUIContent spriteMode = EditorGUIUtility.TrTextContent("Sprite Mode", null, null);

			public readonly GUIContent[] spriteModeOptions = new GUIContent[]
			{
				EditorGUIUtility.TrTextContent("Single", null, null),
				EditorGUIUtility.TrTextContent("Multiple", null, null),
				EditorGUIUtility.TrTextContent("Polygon", null, null)
			};

			public readonly GUIContent[] spriteMeshTypeOptions = new GUIContent[]
			{
				EditorGUIUtility.TrTextContent("Full Rect", null, null),
				EditorGUIUtility.TrTextContent("Tight", null, null)
			};

			public readonly GUIContent spritePackingTag = EditorGUIUtility.TrTextContent("Packing Tag", "Tag for the Sprite Packing system.", null);

			public readonly GUIContent spritePixelsPerUnit = EditorGUIUtility.TrTextContent("Pixels Per Unit", "How many pixels in the sprite correspond to one unit in the world.", null);

			public readonly GUIContent spriteExtrude = EditorGUIUtility.TrTextContent("Extrude Edges", "How much empty area to leave around the sprite in the generated mesh.", null);

			public readonly GUIContent spriteMeshType = EditorGUIUtility.TrTextContent("Mesh Type", "Type of sprite mesh to generate.", null);

			public readonly GUIContent spriteAlignment = EditorGUIUtility.TrTextContent("Pivot", "Sprite pivot point in its localspace. May be used for syncing animation frames of different sizes.", null);

			public readonly GUIContent[] spriteAlignmentOptions = new GUIContent[]
			{
				EditorGUIUtility.TrTextContent("Center", null, null),
				EditorGUIUtility.TrTextContent("Top Left", null, null),
				EditorGUIUtility.TrTextContent("Top", null, null),
				EditorGUIUtility.TrTextContent("Top Right", null, null),
				EditorGUIUtility.TrTextContent("Left", null, null),
				EditorGUIUtility.TrTextContent("Right", null, null),
				EditorGUIUtility.TrTextContent("Bottom Left", null, null),
				EditorGUIUtility.TrTextContent("Bottom", null, null),
				EditorGUIUtility.TrTextContent("Bottom Right", null, null),
				EditorGUIUtility.TrTextContent("Custom", null, null)
			};

			public readonly GUIContent alphaIsTransparency = EditorGUIUtility.TrTextContent("Alpha Is Transparency", "If the provided alpha channel is transparency, enable this to pre-filter the color to avoid texture filtering artifacts. This is not supported for HDR textures.", null);

			public readonly GUIContent etc1Compression = EditorGUIUtility.TrTextContent("Compress using ETC1 (split alpha channel)", "Alpha for this texture will be preserved by splitting the alpha channel to another texture, and both resulting textures will be compressed using ETC1.", null);

			public readonly GUIContent crunchedCompression = EditorGUIUtility.TrTextContent("Use Crunch Compression", "Texture is crunch-compressed to save space on disk when applicable.", null);

			public readonly GUIContent showAdvanced = EditorGUIUtility.TrTextContent("Advanced", "Show advanced settings.", null);

			public Styles()
			{
				GUIContent[] value = new GUIContent[]
				{
					this.textureShape2D
				};
				GUIContent[] value2 = new GUIContent[]
				{
					this.textureShapeCube
				};
				GUIContent[] value3 = new GUIContent[]
				{
					this.textureShape2D,
					this.textureShapeCube
				};
				this.textureShapeOptionsDictionnary.Add(TextureImporterShape.Texture2D, value);
				this.textureShapeOptionsDictionnary.Add(TextureImporterShape.TextureCube, value2);
				this.textureShapeOptionsDictionnary.Add(TextureImporterShape.Texture2D | TextureImporterShape.TextureCube, value3);
				int[] value4 = new int[]
				{
					1
				};
				int[] value5 = new int[]
				{
					2
				};
				int[] value6 = new int[]
				{
					1,
					2
				};
				this.textureShapeValuesDictionnary.Add(TextureImporterShape.Texture2D, value4);
				this.textureShapeValuesDictionnary.Add(TextureImporterShape.TextureCube, value5);
				this.textureShapeValuesDictionnary.Add(TextureImporterShape.Texture2D | TextureImporterShape.TextureCube, value6);
			}
		}

		public static string s_DefaultPlatformName = "DefaultTexturePlatform";

		private SerializedProperty m_TextureType;

		private Dictionary<TextureImporterInspector.TextureInspectorGUIElement, TextureImporterInspector.GUIMethod> m_GUIElementMethods = new Dictionary<TextureImporterInspector.TextureInspectorGUIElement, TextureImporterInspector.GUIMethod>();

		[SerializeField]
		internal List<TextureImportPlatformSettings> m_PlatformSettings;

		internal static int[] s_TextureFormatsValueAll;

		internal static int[] s_NormalFormatsValueAll;

		internal static readonly TextureImporterFormat[] kFormatsWithCompressionSettings = new TextureImporterFormat[]
		{
			TextureImporterFormat.DXT1Crunched,
			TextureImporterFormat.DXT5Crunched,
			TextureImporterFormat.ETC_RGB4Crunched,
			TextureImporterFormat.ETC2_RGBA8Crunched,
			TextureImporterFormat.PVRTC_RGB2,
			TextureImporterFormat.PVRTC_RGB4,
			TextureImporterFormat.PVRTC_RGBA2,
			TextureImporterFormat.PVRTC_RGBA4,
			TextureImporterFormat.ETC_RGB4,
			TextureImporterFormat.ETC2_RGB4,
			TextureImporterFormat.ETC2_RGB4_PUNCHTHROUGH_ALPHA,
			TextureImporterFormat.ETC2_RGBA8,
			TextureImporterFormat.ASTC_RGB_4x4,
			TextureImporterFormat.ASTC_RGB_5x5,
			TextureImporterFormat.ASTC_RGB_6x6,
			TextureImporterFormat.ASTC_RGB_8x8,
			TextureImporterFormat.ASTC_RGB_10x10,
			TextureImporterFormat.ASTC_RGB_12x12,
			TextureImporterFormat.ASTC_RGBA_4x4,
			TextureImporterFormat.ASTC_RGBA_5x5,
			TextureImporterFormat.ASTC_RGBA_6x6,
			TextureImporterFormat.ASTC_RGBA_8x8,
			TextureImporterFormat.ASTC_RGBA_10x10,
			TextureImporterFormat.ASTC_RGBA_12x12
		};

		internal static string[] s_TextureFormatStringsAll;

		internal static string[] s_TextureFormatStringsPSP2;

		internal static string[] s_TextureFormatStringsSwitch;

		internal static string[] s_TextureFormatStringsWebGL;

		internal static string[] s_TextureFormatStringsApplePVR;

		internal static string[] s_TextureFormatStringsAndroid;

		internal static string[] s_TextureFormatStringsTizen;

		internal static string[] s_TextureFormatStringsSingleChannel;

		internal static string[] s_TextureFormatStringsDefault;

		internal static string[] s_NormalFormatStringsDefault;

		private readonly AnimBool m_ShowBumpGenerationSettings = new AnimBool();

		private readonly AnimBool m_ShowCubeMapSettings = new AnimBool();

		private readonly AnimBool m_ShowGenericSpriteSettings = new AnimBool();

		private readonly AnimBool m_ShowMipMapSettings = new AnimBool();

		private readonly AnimBool m_ShowSpriteMeshTypeOption = new AnimBool();

		private readonly GUIContent m_EmptyContent = new GUIContent(" ");

		private readonly int[] m_FilterModeOptions = (int[])Enum.GetValues(typeof(FilterMode));

		private string m_ImportWarning = null;

		internal static TextureImporterInspector.Styles s_Styles;

		private TextureImporterInspector.TextureInspectorTypeGUIProperties[] m_TextureTypeGUIElements = new TextureImporterInspector.TextureInspectorTypeGUIProperties[Enum.GetValues(typeof(TextureImporterType)).Length];

		private List<TextureImporterInspector.TextureInspectorGUIElement> m_GUIElementsDisplayOrder = new List<TextureImporterInspector.TextureInspectorGUIElement>();

		private SerializedProperty m_AlphaSource;

		private SerializedProperty m_ConvertToNormalMap;

		private SerializedProperty m_HeightScale;

		private SerializedProperty m_NormalMapFilter;

		private SerializedProperty m_GenerateCubemap;

		private SerializedProperty m_CubemapConvolution;

		private SerializedProperty m_SeamlessCubemap;

		private SerializedProperty m_BorderMipMap;

		private SerializedProperty m_MipMapsPreserveCoverage;

		private SerializedProperty m_AlphaTestReferenceValue;

		private SerializedProperty m_NPOTScale;

		private SerializedProperty m_IsReadable;

		private SerializedProperty m_sRGBTexture;

		private SerializedProperty m_EnableMipMap;

		private SerializedProperty m_MipMapMode;

		private SerializedProperty m_FadeOut;

		private SerializedProperty m_MipMapFadeDistanceStart;

		private SerializedProperty m_MipMapFadeDistanceEnd;

		private SerializedProperty m_Aniso;

		private SerializedProperty m_FilterMode;

		private SerializedProperty m_WrapU;

		private SerializedProperty m_WrapV;

		private SerializedProperty m_WrapW;

		private SerializedProperty m_SpritePackingTag;

		private SerializedProperty m_SpritePixelsToUnits;

		private SerializedProperty m_SpriteExtrude;

		private SerializedProperty m_SpriteMeshType;

		private SerializedProperty m_Alignment;

		private SerializedProperty m_SpritePivot;

		private SerializedProperty m_AlphaIsTransparency;

		private SerializedProperty m_TextureShape;

		private SerializedProperty m_SpriteMode;

		private SerializedProperty m_SingleChannelComponent;

		private bool m_ShowAdvanced = false;

		private int m_TextureWidth = 0;

		private int m_TextureHeight = 0;

		private bool m_IsPOT = false;

		private bool m_ShowPerAxisWrapModes = false;

		internal TextureImporterType textureType
		{
			get
			{
				TextureImporterType result;
				if (this.m_TextureType.hasMultipleDifferentValues)
				{
					result = TextureImporterType.Default;
				}
				else
				{
					result = (TextureImporterType)this.m_TextureType.intValue;
				}
				return result;
			}
		}

		internal bool textureTypeHasMultipleDifferentValues
		{
			get
			{
				return this.m_TextureType.hasMultipleDifferentValues;
			}
		}

		public override bool showImportedObject
		{
			get
			{
				return false;
			}
		}

		internal static int[] TextureFormatsValueAll
		{
			get
			{
				int[] result;
				if (TextureImporterInspector.s_TextureFormatsValueAll != null)
				{
					result = TextureImporterInspector.s_TextureFormatsValueAll;
				}
				else
				{
					bool flag = false;
					bool flag2 = false;
					bool flag3 = false;
					bool flag4 = false;
					BuildPlatform[] buildPlayerValidPlatforms = TextureImporterInspector.GetBuildPlayerValidPlatforms();
					BuildPlatform[] array = buildPlayerValidPlatforms;
					for (int i = 0; i < array.Length; i++)
					{
						BuildPlatform buildPlatform = array[i];
						BuildTarget defaultTarget = buildPlatform.defaultTarget;
						if (defaultTarget != BuildTarget.iOS)
						{
							if (defaultTarget != BuildTarget.Android)
							{
								if (defaultTarget != BuildTarget.Tizen)
								{
									if (defaultTarget == BuildTarget.tvOS)
									{
										flag2 = true;
										flag4 = true;
									}
								}
								else
								{
									flag = true;
								}
							}
							else
							{
								flag2 = true;
								flag = true;
								flag3 = true;
								flag4 = true;
							}
						}
						else
						{
							flag2 = true;
							flag = true;
							flag3 = true;
						}
					}
					List<int> list = new List<int>();
					list.AddRange(new int[]
					{
						10,
						12
					});
					if (flag)
					{
						list.Add(34);
					}
					if (flag2)
					{
						list.AddRange(new int[]
						{
							30,
							31,
							32,
							33
						});
					}
					if (flag3)
					{
						list.AddRange(new int[]
						{
							45,
							46,
							47
						});
					}
					if (flag4)
					{
						list.AddRange(new int[]
						{
							48,
							49,
							50,
							51,
							52,
							53,
							54,
							55,
							56,
							57,
							58,
							59
						});
					}
					list.AddRange(new int[]
					{
						7,
						2,
						13,
						3,
						1,
						5,
						4,
						17,
						24,
						25,
						28,
						29,
						64,
						65
					});
					TextureImporterInspector.s_TextureFormatsValueAll = list.ToArray();
					result = TextureImporterInspector.s_TextureFormatsValueAll;
				}
				return result;
			}
		}

		internal static int[] NormalFormatsValueAll
		{
			get
			{
				bool flag = false;
				bool flag2 = false;
				bool flag3 = false;
				bool flag4 = false;
				BuildPlatform[] buildPlayerValidPlatforms = TextureImporterInspector.GetBuildPlayerValidPlatforms();
				BuildPlatform[] array = buildPlayerValidPlatforms;
				int i = 0;
				while (i < array.Length)
				{
					BuildPlatform buildPlatform = array[i];
					BuildTarget defaultTarget = buildPlatform.defaultTarget;
					if (defaultTarget == BuildTarget.iOS)
					{
						goto IL_64;
					}
					if (defaultTarget != BuildTarget.Android)
					{
						if (defaultTarget != BuildTarget.Tizen)
						{
							if (defaultTarget == BuildTarget.tvOS)
							{
								goto IL_64;
							}
						}
						else
						{
							flag = true;
						}
					}
					else
					{
						flag2 = true;
						flag = true;
						flag3 = true;
						flag4 = true;
					}
					IL_76:
					i++;
					continue;
					IL_64:
					flag2 = true;
					flag = true;
					flag3 = true;
					goto IL_76;
				}
				List<int> list = new List<int>();
				list.AddRange(new int[]
				{
					12
				});
				if (flag2)
				{
					list.AddRange(new int[]
					{
						30,
						31,
						32,
						33
					});
				}
				if (flag)
				{
					list.AddRange(new int[]
					{
						34
					});
				}
				if (flag3)
				{
					list.AddRange(new int[]
					{
						45,
						46,
						47
					});
				}
				if (flag4)
				{
					list.AddRange(new int[]
					{
						48,
						49,
						50,
						51,
						52,
						53,
						54,
						55,
						56,
						57,
						58,
						59
					});
				}
				list.AddRange(new int[]
				{
					2,
					13,
					4,
					29
				});
				TextureImporterInspector.s_NormalFormatsValueAll = list.ToArray();
				return TextureImporterInspector.s_NormalFormatsValueAll;
			}
		}

		internal SpriteImportMode spriteImportMode
		{
			get
			{
				return (SpriteImportMode)this.m_SpriteMode.intValue;
			}
		}

		public new void OnDisable()
		{
			base.OnDisable();
			EditorPrefs.SetBool("TextureImporterShowAdvanced", this.m_ShowAdvanced);
		}

		public static bool IsCompressedDXTTextureFormat(TextureImporterFormat format)
		{
			return format == TextureImporterFormat.DXT1 || format == TextureImporterFormat.DXT5;
		}

		internal static bool IsGLESMobileTargetPlatform(BuildTarget target)
		{
			return target == BuildTarget.iOS || target == BuildTarget.tvOS || target == BuildTarget.Android || target == BuildTarget.Tizen;
		}

		private void UpdateImportWarning()
		{
			TextureImporter textureImporter = base.target as TextureImporter;
			this.m_ImportWarning = ((!textureImporter) ? null : textureImporter.GetImportWarnings());
		}

		private void ToggleFromInt(SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginChangeCheck();
			EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
			int intValue = (!EditorGUILayout.Toggle(label, property.intValue > 0, new GUILayoutOption[0])) ? 0 : 1;
			EditorGUI.showMixedValue = false;
			if (EditorGUI.EndChangeCheck())
			{
				property.intValue = intValue;
			}
		}

		private void EnumPopup(SerializedProperty property, Type type, GUIContent label)
		{
			EditorGUILayout.IntPopup(property, EditorGUIUtility.TempContent(Enum.GetNames(type)), Enum.GetValues(type) as int[], label, new GUILayoutOption[0]);
		}

		private void CacheSerializedProperties()
		{
			this.m_AlphaSource = base.serializedObject.FindProperty("m_AlphaUsage");
			this.m_ConvertToNormalMap = base.serializedObject.FindProperty("m_ConvertToNormalMap");
			this.m_HeightScale = base.serializedObject.FindProperty("m_HeightScale");
			this.m_NormalMapFilter = base.serializedObject.FindProperty("m_NormalMapFilter");
			this.m_GenerateCubemap = base.serializedObject.FindProperty("m_GenerateCubemap");
			this.m_SeamlessCubemap = base.serializedObject.FindProperty("m_SeamlessCubemap");
			this.m_BorderMipMap = base.serializedObject.FindProperty("m_BorderMipMap");
			this.m_MipMapsPreserveCoverage = base.serializedObject.FindProperty("m_MipMapsPreserveCoverage");
			this.m_AlphaTestReferenceValue = base.serializedObject.FindProperty("m_AlphaTestReferenceValue");
			this.m_NPOTScale = base.serializedObject.FindProperty("m_NPOTScale");
			this.m_IsReadable = base.serializedObject.FindProperty("m_IsReadable");
			this.m_sRGBTexture = base.serializedObject.FindProperty("m_sRGBTexture");
			this.m_EnableMipMap = base.serializedObject.FindProperty("m_EnableMipMap");
			this.m_MipMapMode = base.serializedObject.FindProperty("m_MipMapMode");
			this.m_FadeOut = base.serializedObject.FindProperty("m_FadeOut");
			this.m_MipMapFadeDistanceStart = base.serializedObject.FindProperty("m_MipMapFadeDistanceStart");
			this.m_MipMapFadeDistanceEnd = base.serializedObject.FindProperty("m_MipMapFadeDistanceEnd");
			this.m_Aniso = base.serializedObject.FindProperty("m_TextureSettings.m_Aniso");
			this.m_FilterMode = base.serializedObject.FindProperty("m_TextureSettings.m_FilterMode");
			this.m_WrapU = base.serializedObject.FindProperty("m_TextureSettings.m_WrapU");
			this.m_WrapV = base.serializedObject.FindProperty("m_TextureSettings.m_WrapV");
			this.m_WrapW = base.serializedObject.FindProperty("m_TextureSettings.m_WrapW");
			this.m_CubemapConvolution = base.serializedObject.FindProperty("m_CubemapConvolution");
			this.m_SpriteMode = base.serializedObject.FindProperty("m_SpriteMode");
			this.m_SpritePackingTag = base.serializedObject.FindProperty("m_SpritePackingTag");
			this.m_SpritePixelsToUnits = base.serializedObject.FindProperty("m_SpritePixelsToUnits");
			this.m_SpriteExtrude = base.serializedObject.FindProperty("m_SpriteExtrude");
			this.m_SpriteMeshType = base.serializedObject.FindProperty("m_SpriteMeshType");
			this.m_Alignment = base.serializedObject.FindProperty("m_Alignment");
			this.m_SpritePivot = base.serializedObject.FindProperty("m_SpritePivot");
			this.m_AlphaIsTransparency = base.serializedObject.FindProperty("m_AlphaIsTransparency");
			this.m_TextureType = base.serializedObject.FindProperty("m_TextureType");
			this.m_TextureShape = base.serializedObject.FindProperty("m_TextureShape");
			this.m_SingleChannelComponent = base.serializedObject.FindProperty("m_SingleChannelComponent");
		}

		private void InitializeGUI()
		{
			TextureImporterShape shapeCaps = TextureImporterShape.Texture2D | TextureImporterShape.TextureCube;
			this.m_TextureTypeGUIElements[0] = new TextureImporterInspector.TextureInspectorTypeGUIProperties(TextureImporterInspector.TextureInspectorGUIElement.AlphaHandling | TextureImporterInspector.TextureInspectorGUIElement.ColorSpace | TextureImporterInspector.TextureInspectorGUIElement.CubeMapConvolution | TextureImporterInspector.TextureInspectorGUIElement.CubeMapping, TextureImporterInspector.TextureInspectorGUIElement.PowerOfTwo | TextureImporterInspector.TextureInspectorGUIElement.Readable | TextureImporterInspector.TextureInspectorGUIElement.MipMaps, shapeCaps);
			this.m_TextureTypeGUIElements[1] = new TextureImporterInspector.TextureInspectorTypeGUIProperties(TextureImporterInspector.TextureInspectorGUIElement.NormalMap | TextureImporterInspector.TextureInspectorGUIElement.CubeMapping, TextureImporterInspector.TextureInspectorGUIElement.PowerOfTwo | TextureImporterInspector.TextureInspectorGUIElement.Readable | TextureImporterInspector.TextureInspectorGUIElement.MipMaps, shapeCaps);
			this.m_TextureTypeGUIElements[8] = new TextureImporterInspector.TextureInspectorTypeGUIProperties(TextureImporterInspector.TextureInspectorGUIElement.Sprite, TextureImporterInspector.TextureInspectorGUIElement.PowerOfTwo | TextureImporterInspector.TextureInspectorGUIElement.Readable | TextureImporterInspector.TextureInspectorGUIElement.AlphaHandling | TextureImporterInspector.TextureInspectorGUIElement.ColorSpace | TextureImporterInspector.TextureInspectorGUIElement.MipMaps, TextureImporterShape.Texture2D);
			this.m_TextureTypeGUIElements[4] = new TextureImporterInspector.TextureInspectorTypeGUIProperties(TextureImporterInspector.TextureInspectorGUIElement.AlphaHandling | TextureImporterInspector.TextureInspectorGUIElement.Cookie | TextureImporterInspector.TextureInspectorGUIElement.CubeMapping, TextureImporterInspector.TextureInspectorGUIElement.PowerOfTwo | TextureImporterInspector.TextureInspectorGUIElement.Readable | TextureImporterInspector.TextureInspectorGUIElement.MipMaps, TextureImporterShape.Texture2D | TextureImporterShape.TextureCube);
			this.m_TextureTypeGUIElements[10] = new TextureImporterInspector.TextureInspectorTypeGUIProperties(TextureImporterInspector.TextureInspectorGUIElement.AlphaHandling | TextureImporterInspector.TextureInspectorGUIElement.CubeMapping | TextureImporterInspector.TextureInspectorGUIElement.SingleChannelComponent, TextureImporterInspector.TextureInspectorGUIElement.PowerOfTwo | TextureImporterInspector.TextureInspectorGUIElement.Readable | TextureImporterInspector.TextureInspectorGUIElement.MipMaps, shapeCaps);
			this.m_TextureTypeGUIElements[2] = new TextureImporterInspector.TextureInspectorTypeGUIProperties(TextureImporterInspector.TextureInspectorGUIElement.None, TextureImporterInspector.TextureInspectorGUIElement.PowerOfTwo | TextureImporterInspector.TextureInspectorGUIElement.Readable | TextureImporterInspector.TextureInspectorGUIElement.AlphaHandling | TextureImporterInspector.TextureInspectorGUIElement.MipMaps, TextureImporterShape.Texture2D);
			this.m_TextureTypeGUIElements[7] = new TextureImporterInspector.TextureInspectorTypeGUIProperties(TextureImporterInspector.TextureInspectorGUIElement.None, TextureImporterInspector.TextureInspectorGUIElement.PowerOfTwo | TextureImporterInspector.TextureInspectorGUIElement.Readable | TextureImporterInspector.TextureInspectorGUIElement.AlphaHandling | TextureImporterInspector.TextureInspectorGUIElement.MipMaps, TextureImporterShape.Texture2D);
			this.m_TextureTypeGUIElements[6] = new TextureImporterInspector.TextureInspectorTypeGUIProperties(TextureImporterInspector.TextureInspectorGUIElement.None, TextureImporterInspector.TextureInspectorGUIElement.PowerOfTwo | TextureImporterInspector.TextureInspectorGUIElement.Readable | TextureImporterInspector.TextureInspectorGUIElement.MipMaps, TextureImporterShape.Texture2D);
			this.m_GUIElementMethods.Clear();
			this.m_GUIElementMethods.Add(TextureImporterInspector.TextureInspectorGUIElement.PowerOfTwo, new TextureImporterInspector.GUIMethod(this.POTScaleGUI));
			this.m_GUIElementMethods.Add(TextureImporterInspector.TextureInspectorGUIElement.Readable, new TextureImporterInspector.GUIMethod(this.ReadableGUI));
			this.m_GUIElementMethods.Add(TextureImporterInspector.TextureInspectorGUIElement.ColorSpace, new TextureImporterInspector.GUIMethod(this.ColorSpaceGUI));
			this.m_GUIElementMethods.Add(TextureImporterInspector.TextureInspectorGUIElement.AlphaHandling, new TextureImporterInspector.GUIMethod(this.AlphaHandlingGUI));
			this.m_GUIElementMethods.Add(TextureImporterInspector.TextureInspectorGUIElement.MipMaps, new TextureImporterInspector.GUIMethod(this.MipMapGUI));
			this.m_GUIElementMethods.Add(TextureImporterInspector.TextureInspectorGUIElement.NormalMap, new TextureImporterInspector.GUIMethod(this.BumpGUI));
			this.m_GUIElementMethods.Add(TextureImporterInspector.TextureInspectorGUIElement.Sprite, new TextureImporterInspector.GUIMethod(this.SpriteGUI));
			this.m_GUIElementMethods.Add(TextureImporterInspector.TextureInspectorGUIElement.Cookie, new TextureImporterInspector.GUIMethod(this.CookieGUI));
			this.m_GUIElementMethods.Add(TextureImporterInspector.TextureInspectorGUIElement.CubeMapping, new TextureImporterInspector.GUIMethod(this.CubemapMappingGUI));
			this.m_GUIElementsDisplayOrder.Clear();
			this.m_GUIElementsDisplayOrder.Add(TextureImporterInspector.TextureInspectorGUIElement.CubeMapping);
			this.m_GUIElementsDisplayOrder.Add(TextureImporterInspector.TextureInspectorGUIElement.CubeMapConvolution);
			this.m_GUIElementsDisplayOrder.Add(TextureImporterInspector.TextureInspectorGUIElement.Cookie);
			this.m_GUIElementsDisplayOrder.Add(TextureImporterInspector.TextureInspectorGUIElement.ColorSpace);
			this.m_GUIElementsDisplayOrder.Add(TextureImporterInspector.TextureInspectorGUIElement.AlphaHandling);
			this.m_GUIElementsDisplayOrder.Add(TextureImporterInspector.TextureInspectorGUIElement.NormalMap);
			this.m_GUIElementsDisplayOrder.Add(TextureImporterInspector.TextureInspectorGUIElement.Sprite);
			this.m_GUIElementsDisplayOrder.Add(TextureImporterInspector.TextureInspectorGUIElement.PowerOfTwo);
			this.m_GUIElementsDisplayOrder.Add(TextureImporterInspector.TextureInspectorGUIElement.Readable);
			this.m_GUIElementsDisplayOrder.Add(TextureImporterInspector.TextureInspectorGUIElement.MipMaps);
		}

		public override void OnEnable()
		{
			TextureImporterInspector.s_DefaultPlatformName = TextureImporter.defaultPlatformName;
			this.m_ShowAdvanced = EditorPrefs.GetBool("TextureImporterShowAdvanced", this.m_ShowAdvanced);
			this.CacheSerializedProperties();
			this.m_ShowBumpGenerationSettings.valueChanged.AddListener(new UnityAction(base.Repaint));
			this.m_ShowCubeMapSettings.valueChanged.AddListener(new UnityAction(base.Repaint));
			this.m_ShowCubeMapSettings.value = (this.m_TextureShape.intValue == 2);
			this.m_ShowGenericSpriteSettings.valueChanged.AddListener(new UnityAction(base.Repaint));
			this.m_ShowGenericSpriteSettings.value = (this.m_SpriteMode.intValue != 0);
			this.m_ShowSpriteMeshTypeOption.valueChanged.AddListener(new UnityAction(base.Repaint));
			this.m_ShowSpriteMeshTypeOption.value = this.ShouldShowSpriteMeshTypeOption();
			this.m_ShowMipMapSettings.valueChanged.AddListener(new UnityAction(base.Repaint));
			this.m_ShowMipMapSettings.value = this.m_EnableMipMap.boolValue;
			this.InitializeGUI();
			TextureImporter textureImporter = base.target as TextureImporter;
			if (!(textureImporter == null))
			{
				textureImporter.GetWidthAndHeight(ref this.m_TextureWidth, ref this.m_TextureHeight);
				this.m_IsPOT = (TextureImporterInspector.IsPowerOfTwo(this.m_TextureWidth) && TextureImporterInspector.IsPowerOfTwo(this.m_TextureHeight));
				TextureImporterInspector.InitializeTextureFormatStrings();
			}
		}

		private void SetSerializedPropertySettings(TextureImporterSettings settings)
		{
			this.m_AlphaSource.intValue = (int)settings.alphaSource;
			this.m_ConvertToNormalMap.intValue = ((!settings.convertToNormalMap) ? 0 : 1);
			this.m_HeightScale.floatValue = settings.heightmapScale;
			this.m_NormalMapFilter.intValue = (int)settings.normalMapFilter;
			this.m_GenerateCubemap.intValue = (int)settings.generateCubemap;
			this.m_CubemapConvolution.intValue = (int)settings.cubemapConvolution;
			this.m_SeamlessCubemap.intValue = ((!settings.seamlessCubemap) ? 0 : 1);
			this.m_BorderMipMap.intValue = ((!settings.borderMipmap) ? 0 : 1);
			this.m_MipMapsPreserveCoverage.intValue = ((!settings.mipMapsPreserveCoverage) ? 0 : 1);
			this.m_AlphaTestReferenceValue.floatValue = settings.alphaTestReferenceValue;
			this.m_NPOTScale.intValue = (int)settings.npotScale;
			this.m_IsReadable.intValue = ((!settings.readable) ? 0 : 1);
			this.m_EnableMipMap.intValue = ((!settings.mipmapEnabled) ? 0 : 1);
			this.m_sRGBTexture.intValue = ((!settings.sRGBTexture) ? 0 : 1);
			this.m_MipMapMode.intValue = (int)settings.mipmapFilter;
			this.m_FadeOut.intValue = ((!settings.fadeOut) ? 0 : 1);
			this.m_MipMapFadeDistanceStart.intValue = settings.mipmapFadeDistanceStart;
			this.m_MipMapFadeDistanceEnd.intValue = settings.mipmapFadeDistanceEnd;
			this.m_SpriteMode.intValue = settings.spriteMode;
			this.m_SpritePixelsToUnits.floatValue = settings.spritePixelsPerUnit;
			this.m_SpriteExtrude.intValue = (int)settings.spriteExtrude;
			this.m_SpriteMeshType.intValue = (int)settings.spriteMeshType;
			this.m_Alignment.intValue = settings.spriteAlignment;
			this.m_WrapU.intValue = (int)settings.wrapMode;
			this.m_WrapV.intValue = (int)settings.wrapMode;
			this.m_FilterMode.intValue = (int)settings.filterMode;
			this.m_Aniso.intValue = settings.aniso;
			this.m_AlphaIsTransparency.intValue = ((!settings.alphaIsTransparency) ? 0 : 1);
			this.m_TextureType.intValue = (int)settings.textureType;
			this.m_TextureShape.intValue = (int)settings.textureShape;
			this.m_SingleChannelComponent.intValue = (int)settings.singleChannelComponent;
		}

		internal TextureImporterSettings GetSerializedPropertySettings()
		{
			return this.GetSerializedPropertySettings(new TextureImporterSettings());
		}

		internal TextureImporterSettings GetSerializedPropertySettings(TextureImporterSettings settings)
		{
			if (!this.m_AlphaSource.hasMultipleDifferentValues)
			{
				settings.alphaSource = (TextureImporterAlphaSource)this.m_AlphaSource.intValue;
			}
			if (!this.m_ConvertToNormalMap.hasMultipleDifferentValues)
			{
				settings.convertToNormalMap = (this.m_ConvertToNormalMap.intValue > 0);
			}
			if (!this.m_HeightScale.hasMultipleDifferentValues)
			{
				settings.heightmapScale = this.m_HeightScale.floatValue;
			}
			if (!this.m_NormalMapFilter.hasMultipleDifferentValues)
			{
				settings.normalMapFilter = (TextureImporterNormalFilter)this.m_NormalMapFilter.intValue;
			}
			if (!this.m_GenerateCubemap.hasMultipleDifferentValues)
			{
				settings.generateCubemap = (TextureImporterGenerateCubemap)this.m_GenerateCubemap.intValue;
			}
			if (!this.m_CubemapConvolution.hasMultipleDifferentValues)
			{
				settings.cubemapConvolution = (TextureImporterCubemapConvolution)this.m_CubemapConvolution.intValue;
			}
			if (!this.m_SeamlessCubemap.hasMultipleDifferentValues)
			{
				settings.seamlessCubemap = (this.m_SeamlessCubemap.intValue > 0);
			}
			if (!this.m_BorderMipMap.hasMultipleDifferentValues)
			{
				settings.borderMipmap = (this.m_BorderMipMap.intValue > 0);
			}
			if (!this.m_MipMapsPreserveCoverage.hasMultipleDifferentValues)
			{
				settings.mipMapsPreserveCoverage = (this.m_MipMapsPreserveCoverage.intValue > 0);
			}
			if (!this.m_AlphaTestReferenceValue.hasMultipleDifferentValues)
			{
				settings.alphaTestReferenceValue = this.m_AlphaTestReferenceValue.floatValue;
			}
			if (!this.m_NPOTScale.hasMultipleDifferentValues)
			{
				settings.npotScale = (TextureImporterNPOTScale)this.m_NPOTScale.intValue;
			}
			if (!this.m_IsReadable.hasMultipleDifferentValues)
			{
				settings.readable = (this.m_IsReadable.intValue > 0);
			}
			if (!this.m_sRGBTexture.hasMultipleDifferentValues)
			{
				settings.sRGBTexture = (this.m_sRGBTexture.intValue > 0);
			}
			if (!this.m_EnableMipMap.hasMultipleDifferentValues)
			{
				settings.mipmapEnabled = (this.m_EnableMipMap.intValue > 0);
			}
			if (!this.m_MipMapMode.hasMultipleDifferentValues)
			{
				settings.mipmapFilter = (TextureImporterMipFilter)this.m_MipMapMode.intValue;
			}
			if (!this.m_FadeOut.hasMultipleDifferentValues)
			{
				settings.fadeOut = (this.m_FadeOut.intValue > 0);
			}
			if (!this.m_MipMapFadeDistanceStart.hasMultipleDifferentValues)
			{
				settings.mipmapFadeDistanceStart = this.m_MipMapFadeDistanceStart.intValue;
			}
			if (!this.m_MipMapFadeDistanceEnd.hasMultipleDifferentValues)
			{
				settings.mipmapFadeDistanceEnd = this.m_MipMapFadeDistanceEnd.intValue;
			}
			if (!this.m_SpriteMode.hasMultipleDifferentValues)
			{
				settings.spriteMode = this.m_SpriteMode.intValue;
			}
			if (!this.m_SpritePixelsToUnits.hasMultipleDifferentValues)
			{
				settings.spritePixelsPerUnit = this.m_SpritePixelsToUnits.floatValue;
			}
			if (!this.m_SpriteExtrude.hasMultipleDifferentValues)
			{
				settings.spriteExtrude = (uint)this.m_SpriteExtrude.intValue;
			}
			if (!this.m_SpriteMeshType.hasMultipleDifferentValues)
			{
				settings.spriteMeshType = (SpriteMeshType)this.m_SpriteMeshType.intValue;
			}
			if (!this.m_Alignment.hasMultipleDifferentValues)
			{
				settings.spriteAlignment = this.m_Alignment.intValue;
			}
			if (!this.m_SpritePivot.hasMultipleDifferentValues)
			{
				settings.spritePivot = this.m_SpritePivot.vector2Value;
			}
			if (!this.m_WrapU.hasMultipleDifferentValues)
			{
				settings.wrapModeU = (TextureWrapMode)this.m_WrapU.intValue;
			}
			if (!this.m_WrapV.hasMultipleDifferentValues)
			{
				settings.wrapModeU = (TextureWrapMode)this.m_WrapV.intValue;
			}
			if (!this.m_WrapW.hasMultipleDifferentValues)
			{
				settings.wrapModeU = (TextureWrapMode)this.m_WrapW.intValue;
			}
			if (!this.m_FilterMode.hasMultipleDifferentValues)
			{
				settings.filterMode = (FilterMode)this.m_FilterMode.intValue;
			}
			if (!this.m_Aniso.hasMultipleDifferentValues)
			{
				settings.aniso = this.m_Aniso.intValue;
			}
			if (!this.m_AlphaIsTransparency.hasMultipleDifferentValues)
			{
				settings.alphaIsTransparency = (this.m_AlphaIsTransparency.intValue > 0);
			}
			if (!this.m_TextureType.hasMultipleDifferentValues)
			{
				settings.textureType = (TextureImporterType)this.m_TextureType.intValue;
			}
			if (!this.m_TextureShape.hasMultipleDifferentValues)
			{
				settings.textureShape = (TextureImporterShape)this.m_TextureShape.intValue;
			}
			if (!this.m_SingleChannelComponent.hasMultipleDifferentValues)
			{
				settings.singleChannelComponent = (TextureImporterSingleChannelComponent)this.m_SingleChannelComponent.intValue;
			}
			return settings;
		}

		private void CookieGUI(TextureImporterInspector.TextureInspectorGUIElement guiElements)
		{
			EditorGUI.BeginChangeCheck();
			TextureImporterInspector.CookieMode cookieMode;
			if (this.m_BorderMipMap.intValue > 0)
			{
				cookieMode = TextureImporterInspector.CookieMode.Spot;
			}
			else if (this.m_TextureShape.intValue == 2)
			{
				cookieMode = TextureImporterInspector.CookieMode.Point;
			}
			else
			{
				cookieMode = TextureImporterInspector.CookieMode.Directional;
			}
			cookieMode = (TextureImporterInspector.CookieMode)EditorGUILayout.Popup(TextureImporterInspector.s_Styles.cookieType, (int)cookieMode, TextureImporterInspector.s_Styles.cookieOptions, new GUILayoutOption[0]);
			if (EditorGUI.EndChangeCheck())
			{
				this.SetCookieMode(cookieMode);
			}
			if (cookieMode == TextureImporterInspector.CookieMode.Point)
			{
				this.m_TextureShape.intValue = 2;
			}
			else
			{
				this.m_TextureShape.intValue = 1;
			}
		}

		private void CubemapMappingGUI(TextureImporterInspector.TextureInspectorGUIElement guiElements)
		{
			this.m_ShowCubeMapSettings.target = (this.m_TextureShape.intValue == 2);
			if (EditorGUILayout.BeginFadeGroup(this.m_ShowCubeMapSettings.faded))
			{
				if (this.m_TextureShape.intValue == 2)
				{
					using (new EditorGUI.DisabledScope(!this.m_IsPOT && this.m_NPOTScale.intValue == 0))
					{
						EditorGUI.showMixedValue = (this.m_GenerateCubemap.hasMultipleDifferentValues || this.m_SeamlessCubemap.hasMultipleDifferentValues);
						EditorGUI.BeginChangeCheck();
						int intValue = EditorGUILayout.IntPopup(TextureImporterInspector.s_Styles.cubemap, this.m_GenerateCubemap.intValue, TextureImporterInspector.s_Styles.cubemapOptions, TextureImporterInspector.s_Styles.cubemapValues2, new GUILayoutOption[0]);
						if (EditorGUI.EndChangeCheck())
						{
							this.m_GenerateCubemap.intValue = intValue;
						}
						EditorGUI.indentLevel++;
						if (this.ShouldDisplayGUIElement(guiElements, TextureImporterInspector.TextureInspectorGUIElement.CubeMapConvolution))
						{
							EditorGUILayout.IntPopup(this.m_CubemapConvolution, TextureImporterInspector.s_Styles.cubemapConvolutionOptions, TextureImporterInspector.s_Styles.cubemapConvolutionValues, TextureImporterInspector.s_Styles.cubemapConvolution, new GUILayoutOption[0]);
						}
						this.ToggleFromInt(this.m_SeamlessCubemap, TextureImporterInspector.s_Styles.seamlessCubemap);
						EditorGUI.indentLevel--;
						EditorGUI.showMixedValue = false;
						EditorGUILayout.Space();
					}
				}
			}
			EditorGUILayout.EndFadeGroup();
		}

		private void ColorSpaceGUI(TextureImporterInspector.TextureInspectorGUIElement guiElements)
		{
			this.ToggleFromInt(this.m_sRGBTexture, TextureImporterInspector.s_Styles.sRGBTexture);
		}

		private void POTScaleGUI(TextureImporterInspector.TextureInspectorGUIElement guiElements)
		{
			using (new EditorGUI.DisabledScope(this.m_IsPOT))
			{
				this.EnumPopup(this.m_NPOTScale, typeof(TextureImporterNPOTScale), TextureImporterInspector.s_Styles.npot);
			}
		}

		private void ReadableGUI(TextureImporterInspector.TextureInspectorGUIElement guiElements)
		{
			this.ToggleFromInt(this.m_IsReadable, TextureImporterInspector.s_Styles.readWrite);
		}

		private void AlphaHandlingGUI(TextureImporterInspector.TextureInspectorGUIElement guiElements)
		{
			bool flag = true;
			if (this.ShouldDisplayGUIElement(guiElements, TextureImporterInspector.TextureInspectorGUIElement.SingleChannelComponent))
			{
				EditorGUI.showMixedValue = this.m_SingleChannelComponent.hasMultipleDifferentValues;
				EditorGUI.BeginChangeCheck();
				int intValue = EditorGUILayout.IntPopup(TextureImporterInspector.s_Styles.singleChannelComponent, this.m_SingleChannelComponent.intValue, TextureImporterInspector.s_Styles.singleChannelComponentOptions, TextureImporterInspector.s_Styles.singleChannelComponentValues, new GUILayoutOption[0]);
				EditorGUI.showMixedValue = false;
				if (EditorGUI.EndChangeCheck())
				{
					this.m_SingleChannelComponent.intValue = intValue;
				}
				flag = (this.m_SingleChannelComponent.intValue == 0);
			}
			if (flag)
			{
				int num = 0;
				int num2 = 0;
				bool flag2 = TextureImporterInspector.CountImportersWithAlpha(base.targets, out num);
				flag2 = (flag2 && TextureImporterInspector.CountImportersWithHDR(base.targets, out num2));
				EditorGUI.showMixedValue = this.m_AlphaSource.hasMultipleDifferentValues;
				EditorGUI.BeginChangeCheck();
				int intValue2 = EditorGUILayout.IntPopup(TextureImporterInspector.s_Styles.alphaSource, this.m_AlphaSource.intValue, TextureImporterInspector.s_Styles.alphaSourceOptions, TextureImporterInspector.s_Styles.alphaSourceValues, new GUILayoutOption[0]);
				EditorGUI.showMixedValue = false;
				if (EditorGUI.EndChangeCheck())
				{
					this.m_AlphaSource.intValue = intValue2;
				}
				bool flag3 = flag2 && this.m_AlphaSource.intValue != 0 && num2 == 0;
				using (new EditorGUI.DisabledScope(!flag3))
				{
					this.ToggleFromInt(this.m_AlphaIsTransparency, TextureImporterInspector.s_Styles.alphaIsTransparency);
				}
			}
		}

		private bool ShouldShowSpriteMeshTypeOption()
		{
			return this.m_SpriteMode.intValue != 3 && !this.m_SpriteMode.hasMultipleDifferentValues;
		}

		private void SpriteGUI(TextureImporterInspector.TextureInspectorGUIElement guiElements)
		{
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.IntPopup(this.m_SpriteMode, TextureImporterInspector.s_Styles.spriteModeOptions, new int[]
			{
				1,
				2,
				3
			}, TextureImporterInspector.s_Styles.spriteMode, new GUILayoutOption[0]);
			if (EditorGUI.EndChangeCheck())
			{
				GUIUtility.keyboardControl = 0;
			}
			EditorGUI.indentLevel++;
			this.m_ShowGenericSpriteSettings.target = (this.m_SpriteMode.intValue != 0);
			if (EditorGUILayout.BeginFadeGroup(this.m_ShowGenericSpriteSettings.faded))
			{
				EditorGUILayout.PropertyField(this.m_SpritePackingTag, TextureImporterInspector.s_Styles.spritePackingTag, new GUILayoutOption[0]);
				EditorGUILayout.PropertyField(this.m_SpritePixelsToUnits, TextureImporterInspector.s_Styles.spritePixelsPerUnit, new GUILayoutOption[0]);
				this.m_ShowSpriteMeshTypeOption.target = this.ShouldShowSpriteMeshTypeOption();
				if (EditorGUILayout.BeginFadeGroup(this.m_ShowSpriteMeshTypeOption.faded))
				{
					EditorGUILayout.IntPopup(this.m_SpriteMeshType, TextureImporterInspector.s_Styles.spriteMeshTypeOptions, new int[]
					{
						0,
						1
					}, TextureImporterInspector.s_Styles.spriteMeshType, new GUILayoutOption[0]);
				}
				EditorGUILayout.EndFadeGroup();
				EditorGUILayout.IntSlider(this.m_SpriteExtrude, 0, 32, TextureImporterInspector.s_Styles.spriteExtrude, new GUILayoutOption[0]);
				if (this.m_SpriteMode.intValue == 1)
				{
					EditorGUILayout.Popup(this.m_Alignment, TextureImporterInspector.s_Styles.spriteAlignmentOptions, TextureImporterInspector.s_Styles.spriteAlignment, new GUILayoutOption[0]);
					if (this.m_Alignment.intValue == 9)
					{
						GUILayout.BeginHorizontal(new GUILayoutOption[0]);
						EditorGUILayout.PropertyField(this.m_SpritePivot, this.m_EmptyContent, new GUILayoutOption[0]);
						GUILayout.EndHorizontal();
					}
				}
				using (new EditorGUI.DisabledScope(base.targets.Length != 1))
				{
					GUILayout.BeginHorizontal(new GUILayoutOption[0]);
					GUILayout.FlexibleSpace();
					if (GUILayout.Button("Sprite Editor", new GUILayoutOption[0]))
					{
						if (this.HasModified())
						{
							string text = "Unapplied import settings for '" + ((TextureImporter)base.target).assetPath + "'.\n";
							text += "Apply and continue to sprite editor or cancel.";
							if (EditorUtility.DisplayDialog("Unapplied import settings", text, "Apply", "Cancel"))
							{
								base.ApplyAndImport();
								SpriteEditorWindow.GetWindow();
								GUIUtility.ExitGUI();
							}
						}
						else
						{
							SpriteEditorWindow.GetWindow();
						}
					}
					GUILayout.EndHorizontal();
				}
			}
			EditorGUILayout.EndFadeGroup();
			EditorGUI.indentLevel--;
		}

		private void MipMapGUI(TextureImporterInspector.TextureInspectorGUIElement guiElements)
		{
			this.ToggleFromInt(this.m_EnableMipMap, TextureImporterInspector.s_Styles.generateMipMaps);
			this.m_ShowMipMapSettings.target = (this.m_EnableMipMap.boolValue && !this.m_EnableMipMap.hasMultipleDifferentValues);
			if (EditorGUILayout.BeginFadeGroup(this.m_ShowMipMapSettings.faded))
			{
				EditorGUI.indentLevel++;
				this.ToggleFromInt(this.m_BorderMipMap, TextureImporterInspector.s_Styles.borderMipMaps);
				EditorGUILayout.Popup(this.m_MipMapMode, TextureImporterInspector.s_Styles.mipMapFilterOptions, TextureImporterInspector.s_Styles.mipMapFilter, new GUILayoutOption[0]);
				this.ToggleFromInt(this.m_MipMapsPreserveCoverage, TextureImporterInspector.s_Styles.mipMapsPreserveCoverage);
				if (this.m_MipMapsPreserveCoverage.intValue != 0 && !this.m_MipMapsPreserveCoverage.hasMultipleDifferentValues)
				{
					EditorGUI.indentLevel++;
					EditorGUILayout.PropertyField(this.m_AlphaTestReferenceValue, TextureImporterInspector.s_Styles.alphaTestReferenceValue, new GUILayoutOption[0]);
					EditorGUI.indentLevel--;
				}
				this.ToggleFromInt(this.m_FadeOut, TextureImporterInspector.s_Styles.mipmapFadeOutToggle);
				if (this.m_FadeOut.intValue > 0)
				{
					EditorGUI.indentLevel++;
					EditorGUI.BeginChangeCheck();
					float f = (float)this.m_MipMapFadeDistanceStart.intValue;
					float f2 = (float)this.m_MipMapFadeDistanceEnd.intValue;
					EditorGUILayout.MinMaxSlider(TextureImporterInspector.s_Styles.mipmapFadeOut, ref f, ref f2, 0f, 10f, new GUILayoutOption[0]);
					if (EditorGUI.EndChangeCheck())
					{
						this.m_MipMapFadeDistanceStart.intValue = Mathf.RoundToInt(f);
						this.m_MipMapFadeDistanceEnd.intValue = Mathf.RoundToInt(f2);
					}
					EditorGUI.indentLevel--;
				}
				EditorGUI.indentLevel--;
			}
			EditorGUILayout.EndFadeGroup();
		}

		private void BumpGUI(TextureImporterInspector.TextureInspectorGUIElement guiElements)
		{
			EditorGUI.BeginChangeCheck();
			this.ToggleFromInt(this.m_ConvertToNormalMap, TextureImporterInspector.s_Styles.generateFromBump);
			this.m_ShowBumpGenerationSettings.target = (this.m_ConvertToNormalMap.intValue > 0);
			if (EditorGUILayout.BeginFadeGroup(this.m_ShowBumpGenerationSettings.faded))
			{
				EditorGUI.indentLevel++;
				EditorGUILayout.Slider(this.m_HeightScale, 0f, 0.3f, TextureImporterInspector.s_Styles.bumpiness, new GUILayoutOption[0]);
				EditorGUILayout.Popup(this.m_NormalMapFilter, TextureImporterInspector.s_Styles.bumpFilteringOptions, TextureImporterInspector.s_Styles.bumpFiltering, new GUILayoutOption[0]);
				EditorGUI.indentLevel--;
			}
			EditorGUILayout.EndFadeGroup();
			if (EditorGUI.EndChangeCheck())
			{
				this.SyncPlatformSettings();
			}
		}

		private void TextureSettingsGUI()
		{
			EditorGUI.BeginChangeCheck();
			bool isVolumeTexture = false;
			TextureInspector.WrapModePopup(this.m_WrapU, this.m_WrapV, this.m_WrapW, isVolumeTexture, ref this.m_ShowPerAxisWrapModes);
			if (this.m_NPOTScale.intValue == 0 && (this.m_WrapU.intValue == 0 || this.m_WrapV.intValue == 0) && !ShaderUtil.hardwareSupportsFullNPOT)
			{
				bool flag = false;
				UnityEngine.Object[] targets = base.targets;
				for (int i = 0; i < targets.Length; i++)
				{
					UnityEngine.Object @object = targets[i];
					int value = -1;
					int value2 = -1;
					TextureImporter textureImporter = (TextureImporter)@object;
					textureImporter.GetWidthAndHeight(ref value, ref value2);
					if (!Mathf.IsPowerOfTwo(value) || !Mathf.IsPowerOfTwo(value2))
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					GUIContent gUIContent = EditorGUIUtility.TrTextContent("Graphics device doesn't support Repeat wrap mode on NPOT textures. Falling back to Clamp.", null, null);
					EditorGUILayout.HelpBox(gUIContent.text, MessageType.Warning, true);
				}
			}
			EditorGUI.BeginChangeCheck();
			EditorGUI.showMixedValue = this.m_FilterMode.hasMultipleDifferentValues;
			FilterMode filterMode = (FilterMode)this.m_FilterMode.intValue;
			if (filterMode == (FilterMode)(-1))
			{
				if (this.m_FadeOut.intValue > 0 || this.m_ConvertToNormalMap.intValue > 0)
				{
					filterMode = FilterMode.Trilinear;
				}
				else
				{
					filterMode = FilterMode.Bilinear;
				}
			}
			filterMode = (FilterMode)EditorGUILayout.IntPopup(TextureImporterInspector.s_Styles.filterMode, (int)filterMode, TextureImporterInspector.s_Styles.filterModeOptions, this.m_FilterModeOptions, new GUILayoutOption[0]);
			EditorGUI.showMixedValue = false;
			if (EditorGUI.EndChangeCheck())
			{
				this.m_FilterMode.intValue = (int)filterMode;
			}
			bool flag2 = this.m_FilterMode.intValue != 0 && this.m_EnableMipMap.intValue > 0 && this.m_TextureShape.intValue != 2;
			using (new EditorGUI.DisabledScope(!flag2))
			{
				EditorGUI.BeginChangeCheck();
				EditorGUI.showMixedValue = this.m_Aniso.hasMultipleDifferentValues;
				int num = this.m_Aniso.intValue;
				if (num == -1)
				{
					num = 1;
				}
				num = EditorGUILayout.IntSlider("Aniso Level", num, 0, 16, new GUILayoutOption[0]);
				EditorGUI.showMixedValue = false;
				if (EditorGUI.EndChangeCheck())
				{
					this.m_Aniso.intValue = num;
				}
				TextureInspector.DoAnisoGlobalSettingNote(num);
			}
			if (EditorGUI.EndChangeCheck())
			{
				this.ApplySettingsToTexture();
			}
		}

		public override void OnInspectorGUI()
		{
			if (TextureImporterInspector.s_Styles == null)
			{
				TextureImporterInspector.s_Styles = new TextureImporterInspector.Styles();
			}
			bool enabled = GUI.enabled;
			EditorGUILayout.Space();
			EditorGUI.BeginChangeCheck();
			EditorGUI.showMixedValue = this.m_TextureType.hasMultipleDifferentValues;
			int num = EditorGUILayout.IntPopup(TextureImporterInspector.s_Styles.textureTypeTitle, this.m_TextureType.intValue, TextureImporterInspector.s_Styles.textureTypeOptions, TextureImporterInspector.s_Styles.textureTypeValues, new GUILayoutOption[0]);
			EditorGUI.showMixedValue = false;
			if (EditorGUI.EndChangeCheck() && this.m_TextureType.intValue != num)
			{
				TextureImporterSettings serializedPropertySettings = this.GetSerializedPropertySettings();
				serializedPropertySettings.ApplyTextureType((TextureImporterType)num);
				this.m_TextureType.intValue = num;
				this.SetSerializedPropertySettings(serializedPropertySettings);
				this.SyncPlatformSettings();
				this.ApplySettingsToTexture();
			}
			int[] array = TextureImporterInspector.s_Styles.textureShapeValuesDictionnary[this.m_TextureTypeGUIElements[num].shapeCaps];
			using (new EditorGUI.DisabledScope(array.Length == 1 || this.m_TextureType.intValue == 4))
			{
				EditorGUI.BeginChangeCheck();
				EditorGUI.showMixedValue = this.m_TextureShape.hasMultipleDifferentValues;
				int intValue = EditorGUILayout.IntPopup(TextureImporterInspector.s_Styles.textureShape, this.m_TextureShape.intValue, TextureImporterInspector.s_Styles.textureShapeOptionsDictionnary[this.m_TextureTypeGUIElements[num].shapeCaps], TextureImporterInspector.s_Styles.textureShapeValuesDictionnary[this.m_TextureTypeGUIElements[num].shapeCaps], new GUILayoutOption[0]);
				EditorGUI.showMixedValue = false;
				if (EditorGUI.EndChangeCheck())
				{
					this.m_TextureShape.intValue = intValue;
				}
			}
			if (Array.IndexOf<int>(array, this.m_TextureShape.intValue) == -1)
			{
				this.m_TextureShape.intValue = array[0];
			}
			EditorGUILayout.Space();
			if (!this.m_TextureType.hasMultipleDifferentValues)
			{
				this.DoGUIElements(this.m_TextureTypeGUIElements[num].commonElements, this.m_GUIElementsDisplayOrder);
				if (this.m_TextureTypeGUIElements[num].advancedElements != TextureImporterInspector.TextureInspectorGUIElement.None)
				{
					EditorGUILayout.Space();
					this.m_ShowAdvanced = EditorGUILayout.Foldout(this.m_ShowAdvanced, TextureImporterInspector.s_Styles.showAdvanced, true);
					if (this.m_ShowAdvanced)
					{
						EditorGUI.indentLevel++;
						this.DoGUIElements(this.m_TextureTypeGUIElements[num].advancedElements, this.m_GUIElementsDisplayOrder);
						EditorGUI.indentLevel--;
					}
				}
			}
			EditorGUILayout.Space();
			this.TextureSettingsGUI();
			this.ShowPlatformSpecificSettings();
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.FlexibleSpace();
			base.ApplyRevertGUI();
			GUILayout.EndHorizontal();
			this.UpdateImportWarning();
			if (this.m_ImportWarning != null)
			{
				EditorGUILayout.HelpBox(this.m_ImportWarning, MessageType.Warning);
			}
			GUI.enabled = enabled;
		}

		private bool ShouldDisplayGUIElement(TextureImporterInspector.TextureInspectorGUIElement guiElements, TextureImporterInspector.TextureInspectorGUIElement guiElement)
		{
			return (guiElements & guiElement) == guiElement;
		}

		private void DoGUIElements(TextureImporterInspector.TextureInspectorGUIElement guiElements, List<TextureImporterInspector.TextureInspectorGUIElement> guiElementsDisplayOrder)
		{
			foreach (TextureImporterInspector.TextureInspectorGUIElement current in guiElementsDisplayOrder)
			{
				if (this.ShouldDisplayGUIElement(guiElements, current) && this.m_GUIElementMethods.ContainsKey(current))
				{
					this.m_GUIElementMethods[current](guiElements);
				}
			}
		}

		private void ApplySettingsToTexture()
		{
			UnityEngine.Object[] targets = base.targets;
			for (int i = 0; i < targets.Length; i++)
			{
				AssetImporter assetImporter = (AssetImporter)targets[i];
				Texture texture = AssetDatabase.LoadMainAssetAtPath(assetImporter.assetPath) as Texture;
				if (texture != null)
				{
					if (this.m_Aniso.intValue != -1)
					{
						TextureUtil.SetAnisoLevelNoDirty(texture, this.m_Aniso.intValue);
					}
					if (this.m_FilterMode.intValue != -1)
					{
						TextureUtil.SetFilterModeNoDirty(texture, (FilterMode)this.m_FilterMode.intValue);
					}
					if ((this.m_WrapU.intValue != -1 || this.m_WrapV.intValue != -1 || this.m_WrapW.intValue != -1) && !this.m_WrapU.hasMultipleDifferentValues && !this.m_WrapV.hasMultipleDifferentValues && !this.m_WrapW.hasMultipleDifferentValues)
					{
						TextureUtil.SetWrapModeNoDirty(texture, (TextureWrapMode)this.m_WrapU.intValue, (TextureWrapMode)this.m_WrapV.intValue, (TextureWrapMode)this.m_WrapW.intValue);
					}
				}
			}
			SceneView.RepaintAll();
		}

		private static bool CountImportersWithAlpha(UnityEngine.Object[] importers, out int count)
		{
			bool result;
			try
			{
				count = 0;
				for (int i = 0; i < importers.Length; i++)
				{
					UnityEngine.Object @object = importers[i];
					if ((@object as TextureImporter).DoesSourceTextureHaveAlpha())
					{
						count++;
					}
				}
				result = true;
			}
			catch
			{
				count = importers.Length;
				result = false;
			}
			return result;
		}

		private static bool CountImportersWithHDR(UnityEngine.Object[] importers, out int count)
		{
			bool result;
			try
			{
				count = 0;
				for (int i = 0; i < importers.Length; i++)
				{
					UnityEngine.Object @object = importers[i];
					if ((@object as TextureImporter).IsSourceTextureHDR())
					{
						count++;
					}
				}
				result = true;
			}
			catch
			{
				count = importers.Length;
				result = false;
			}
			return result;
		}

		private void SetCookieMode(TextureImporterInspector.CookieMode cm)
		{
			if (cm != TextureImporterInspector.CookieMode.Spot)
			{
				if (cm != TextureImporterInspector.CookieMode.Point)
				{
					if (cm == TextureImporterInspector.CookieMode.Directional)
					{
						this.m_BorderMipMap.intValue = 0;
						SerializedProperty arg_EB_0 = this.m_WrapU;
						int num = 0;
						this.m_WrapW.intValue = num;
						num = num;
						this.m_WrapV.intValue = num;
						arg_EB_0.intValue = num;
						this.m_GenerateCubemap.intValue = 6;
						this.m_TextureShape.intValue = 1;
					}
				}
				else
				{
					this.m_BorderMipMap.intValue = 0;
					SerializedProperty arg_9A_0 = this.m_WrapU;
					int num = 1;
					this.m_WrapW.intValue = num;
					num = num;
					this.m_WrapV.intValue = num;
					arg_9A_0.intValue = num;
					this.m_GenerateCubemap.intValue = 1;
					this.m_TextureShape.intValue = 2;
				}
			}
			else
			{
				this.m_BorderMipMap.intValue = 1;
				SerializedProperty arg_49_0 = this.m_WrapU;
				int num = 1;
				this.m_WrapW.intValue = num;
				num = num;
				this.m_WrapV.intValue = num;
				arg_49_0.intValue = num;
				this.m_GenerateCubemap.intValue = 6;
				this.m_TextureShape.intValue = 1;
			}
		}

		private void SyncPlatformSettings()
		{
			foreach (TextureImportPlatformSettings current in this.m_PlatformSettings)
			{
				current.Sync();
			}
		}

		internal static string[] BuildTextureStrings(int[] texFormatValues)
		{
			string[] array = new string[texFormatValues.Length];
			for (int i = 0; i < texFormatValues.Length; i++)
			{
				int format = texFormatValues[i];
				array[i] = " " + TextureUtil.GetTextureFormatString((TextureFormat)format);
			}
			return array;
		}

		internal static void InitializeTextureFormatStrings()
		{
			if (TextureImporterInspector.s_TextureFormatStringsApplePVR == null)
			{
				TextureImporterInspector.s_TextureFormatStringsApplePVR = TextureImporterInspector.BuildTextureStrings(TextureImportPlatformSettings.kTextureFormatsValueApplePVR);
			}
			if (TextureImporterInspector.s_TextureFormatStringsAndroid == null)
			{
				TextureImporterInspector.s_TextureFormatStringsAndroid = TextureImporterInspector.BuildTextureStrings(TextureImportPlatformSettings.kTextureFormatsValueAndroid);
			}
			if (TextureImporterInspector.s_TextureFormatStringsTizen == null)
			{
				TextureImporterInspector.s_TextureFormatStringsTizen = TextureImporterInspector.BuildTextureStrings(TextureImportPlatformSettings.kTextureFormatsValueTizen);
			}
			if (TextureImporterInspector.s_TextureFormatStringsWebGL == null)
			{
				TextureImporterInspector.s_TextureFormatStringsWebGL = TextureImporterInspector.BuildTextureStrings(TextureImportPlatformSettings.kTextureFormatsValueWebGL);
			}
			if (TextureImporterInspector.s_TextureFormatStringsPSP2 == null)
			{
				TextureImporterInspector.s_TextureFormatStringsPSP2 = TextureImporterInspector.BuildTextureStrings(TextureImportPlatformSettings.kTextureFormatsValuePSP2);
			}
			if (TextureImporterInspector.s_TextureFormatStringsSwitch == null)
			{
				TextureImporterInspector.s_TextureFormatStringsSwitch = TextureImporterInspector.BuildTextureStrings(TextureImportPlatformSettings.kTextureFormatsValueSwitch);
			}
			if (TextureImporterInspector.s_TextureFormatStringsDefault == null)
			{
				TextureImporterInspector.s_TextureFormatStringsDefault = TextureImporterInspector.BuildTextureStrings(TextureImportPlatformSettings.kTextureFormatsValueDefault);
			}
			if (TextureImporterInspector.s_NormalFormatStringsDefault == null)
			{
				TextureImporterInspector.s_NormalFormatStringsDefault = TextureImporterInspector.BuildTextureStrings(TextureImportPlatformSettings.kNormalFormatsValueDefault);
			}
			if (TextureImporterInspector.s_TextureFormatStringsSingleChannel == null)
			{
				TextureImporterInspector.s_TextureFormatStringsSingleChannel = TextureImporterInspector.BuildTextureStrings(TextureImportPlatformSettings.kTextureFormatsValueSingleChannel);
			}
		}

		internal static bool IsFormatRequireCompressionSetting(TextureImporterFormat format)
		{
			return ArrayUtility.Contains<TextureImporterFormat>(TextureImporterInspector.kFormatsWithCompressionSettings, format);
		}

		protected void ShowPlatformSpecificSettings()
		{
			BuildPlatform[] array = TextureImporterInspector.GetBuildPlayerValidPlatforms().ToArray<BuildPlatform>();
			GUILayout.Space(10f);
			int num = EditorGUILayout.BeginPlatformGrouping(array, TextureImporterInspector.s_Styles.defaultPlatform);
			TextureImportPlatformSettings textureImportPlatformSettings = this.m_PlatformSettings[num + 1];
			if (!textureImportPlatformSettings.isDefault)
			{
				EditorGUI.BeginChangeCheck();
				EditorGUI.showMixedValue = textureImportPlatformSettings.overriddenIsDifferent;
				string label = "Override for " + array[num].title.text;
				bool overriddenForAll = EditorGUILayout.ToggleLeft(label, textureImportPlatformSettings.overridden, new GUILayoutOption[0]);
				EditorGUI.showMixedValue = false;
				if (EditorGUI.EndChangeCheck())
				{
					textureImportPlatformSettings.SetOverriddenForAll(overriddenForAll);
					this.SyncPlatformSettings();
				}
			}
			bool disabled = !textureImportPlatformSettings.isDefault && !textureImportPlatformSettings.allAreOverridden;
			using (new EditorGUI.DisabledScope(disabled))
			{
				ITextureImportSettingsExtension textureImportSettingsExtension = ModuleManager.GetTextureImportSettingsExtension(textureImportPlatformSettings.m_Target);
				textureImportSettingsExtension.ShowImportSettings(this, textureImportPlatformSettings);
				this.SyncPlatformSettings();
			}
			EditorGUILayout.EndPlatformGrouping();
		}

		private static bool IsPowerOfTwo(int f)
		{
			return (f & f - 1) == 0;
		}

		public static BuildPlatform[] GetBuildPlayerValidPlatforms()
		{
			List<BuildPlatform> validPlatforms = BuildPlatforms.instance.GetValidPlatforms();
			return validPlatforms.ToArray();
		}

		public virtual void BuildTargetList()
		{
			BuildPlatform[] buildPlayerValidPlatforms = TextureImporterInspector.GetBuildPlayerValidPlatforms();
			this.m_PlatformSettings = new List<TextureImportPlatformSettings>();
			this.m_PlatformSettings.Add(new TextureImportPlatformSettings(TextureImporterInspector.s_DefaultPlatformName, BuildTarget.StandaloneWindows, this));
			BuildPlatform[] array = buildPlayerValidPlatforms;
			for (int i = 0; i < array.Length; i++)
			{
				BuildPlatform buildPlatform = array[i];
				this.m_PlatformSettings.Add(new TextureImportPlatformSettings(buildPlatform.name, buildPlatform.defaultTarget, this));
			}
		}

		public override bool HasModified()
		{
			bool result;
			if (base.HasModified())
			{
				result = true;
			}
			else
			{
				foreach (TextureImportPlatformSettings current in this.m_PlatformSettings)
				{
					if (current.HasChanged())
					{
						result = true;
						return result;
					}
				}
				result = false;
			}
			return result;
		}

		public static void SelectMainAssets(UnityEngine.Object[] targets)
		{
			ArrayList arrayList = new ArrayList();
			for (int i = 0; i < targets.Length; i++)
			{
				AssetImporter assetImporter = (AssetImporter)targets[i];
				Texture texture = AssetDatabase.LoadMainAssetAtPath(assetImporter.assetPath) as Texture;
				if (texture)
				{
					arrayList.Add(texture);
				}
			}
			if (arrayList.Count > 0)
			{
				Selection.objects = (arrayList.ToArray(typeof(UnityEngine.Object)) as UnityEngine.Object[]);
			}
		}

		protected override void ResetValues()
		{
			base.ResetValues();
			this.CacheSerializedProperties();
			this.BuildTargetList();
			this.ApplySettingsToTexture();
			TextureImporterInspector.SelectMainAssets(base.targets);
		}

		protected override void Apply()
		{
			base.Apply();
			this.SyncPlatformSettings();
			foreach (TextureImportPlatformSettings current in this.m_PlatformSettings)
			{
				current.Apply();
			}
		}
	}
}
