using System;
using System.Linq;
using UnityEngine;

namespace UnityEditor.Modules
{
	internal class DefaultTextureImportSettingsExtension : ITextureImportSettingsExtension
	{
		private static readonly string[] kMaxTextureSizeStrings = new string[]
		{
			"32",
			"64",
			"128",
			"256",
			"512",
			"1024",
			"2048",
			"4096",
			"8192"
		};

		private static readonly int[] kMaxTextureSizeValues = new int[]
		{
			32,
			64,
			128,
			256,
			512,
			1024,
			2048,
			4096,
			8192
		};

		private static readonly GUIContent maxSize = EditorGUIUtility.TrTextContent("Max Size", "Textures larger than this will be scaled down.", null);

		private static readonly string[] kResizeAlgorithmStrings = new string[]
		{
			"Mitchell",
			"Bilinear"
		};

		private static readonly int[] kResizeAlgorithmValues = new int[]
		{
			0,
			1
		};

		private static readonly GUIContent resizeAlgorithm = EditorGUIUtility.TrTextContent("Resize Algorithm", "Select algorithm to apply for textures when scaled down.", null);

		private static readonly GUIContent kTextureCompression = EditorGUIUtility.TrTextContent("Compression", "How will this texture be compressed?", null);

		private static readonly GUIContent[] kTextureCompressionOptions = new GUIContent[]
		{
			EditorGUIUtility.TrTextContent("None", "Texture is not compressed.", null),
			EditorGUIUtility.TrTextContent("Low Quality", "Texture compressed with low quality but high performance, high compression format.", null),
			EditorGUIUtility.TrTextContent("Normal Quality", "Texture is compressed with a standard format.", null),
			EditorGUIUtility.TrTextContent("High Quality", "Texture compressed with a high quality format.", null)
		};

		private static readonly int[] kTextureCompressionValues = new int[]
		{
			0,
			3,
			1,
			2
		};

		public virtual void ShowImportSettings(Editor baseEditor, TextureImportPlatformSettings platformSettings)
		{
			TextureImporterInspector textureImporterInspector = baseEditor as TextureImporterInspector;
			EditorGUI.BeginChangeCheck();
			EditorGUI.showMixedValue = (platformSettings.overriddenIsDifferent || platformSettings.maxTextureSizeIsDifferent);
			int maxTextureSizeForAll = EditorGUILayout.IntPopup(DefaultTextureImportSettingsExtension.maxSize.text, platformSettings.maxTextureSize, DefaultTextureImportSettingsExtension.kMaxTextureSizeStrings, DefaultTextureImportSettingsExtension.kMaxTextureSizeValues, new GUILayoutOption[0]);
			EditorGUI.showMixedValue = false;
			if (EditorGUI.EndChangeCheck())
			{
				platformSettings.SetMaxTextureSizeForAll(maxTextureSizeForAll);
			}
			EditorGUI.BeginChangeCheck();
			EditorGUI.showMixedValue = (platformSettings.overriddenIsDifferent || platformSettings.resizeAlgorithmIsDifferent);
			int resizeAlgorithmForAll = EditorGUILayout.IntPopup(DefaultTextureImportSettingsExtension.resizeAlgorithm.text, (int)platformSettings.resizeAlgorithm, DefaultTextureImportSettingsExtension.kResizeAlgorithmStrings, DefaultTextureImportSettingsExtension.kResizeAlgorithmValues, new GUILayoutOption[0]);
			EditorGUI.showMixedValue = false;
			if (EditorGUI.EndChangeCheck())
			{
				platformSettings.SetResizeAlgorithmForAll((TextureResizeAlgorithm)resizeAlgorithmForAll);
			}
			using (new EditorGUI.DisabledScope(platformSettings.overridden && !platformSettings.isDefault))
			{
				EditorGUI.BeginChangeCheck();
				EditorGUI.showMixedValue = (platformSettings.overriddenIsDifferent || platformSettings.textureCompressionIsDifferent || (platformSettings.overridden && !platformSettings.isDefault));
				TextureImporterCompression textureCompressionForAll = (TextureImporterCompression)EditorGUILayout.IntPopup(DefaultTextureImportSettingsExtension.kTextureCompression, (int)platformSettings.textureCompression, DefaultTextureImportSettingsExtension.kTextureCompressionOptions, DefaultTextureImportSettingsExtension.kTextureCompressionValues, new GUILayoutOption[0]);
				EditorGUI.showMixedValue = false;
				if (EditorGUI.EndChangeCheck())
				{
					platformSettings.SetTextureCompressionForAll(textureCompressionForAll);
				}
			}
			int[] array = null;
			string[] array2 = null;
			bool flag = false;
			int num = 0;
			for (int i = 0; i < textureImporterInspector.targets.Length; i++)
			{
				TextureImporter textureImporter = textureImporterInspector.targets[i] as TextureImporter;
				TextureImporterSettings settings = platformSettings.GetSettings(textureImporter);
				TextureImporterType textureImporterType = (!textureImporterInspector.textureTypeHasMultipleDifferentValues) ? textureImporterInspector.textureType : settings.textureType;
				int num2 = (int)platformSettings.format;
				int[] array3;
				string[] array4;
				if (platformSettings.isDefault)
				{
					num2 = -1;
					array3 = new int[]
					{
						-1
					};
					array4 = new string[]
					{
						"Auto"
					};
				}
				else if (!platformSettings.overridden)
				{
					num2 = (int)TextureImporter.FormatFromTextureParameters(settings, platformSettings.platformTextureSettings, textureImporterInspector.assetTarget && textureImporter.DoesSourceTextureHaveAlpha(), textureImporterInspector.assetTarget && textureImporter.IsSourceTextureHDR(), platformSettings.m_Target);
					array3 = new int[]
					{
						num2
					};
					array4 = new string[]
					{
						TextureUtil.GetTextureFormatString((TextureFormat)num2)
					};
				}
				else if (textureImporterType == TextureImporterType.Cookie || textureImporterType == TextureImporterType.SingleChannel)
				{
					array3 = TextureImportPlatformSettings.kTextureFormatsValueSingleChannel;
					array4 = TextureImporterInspector.s_TextureFormatStringsSingleChannel;
				}
				else if (TextureImporterInspector.IsGLESMobileTargetPlatform(platformSettings.m_Target))
				{
					if (platformSettings.m_Target == BuildTarget.iOS || platformSettings.m_Target == BuildTarget.tvOS)
					{
						array3 = TextureImportPlatformSettings.kTextureFormatsValueApplePVR;
						array4 = TextureImporterInspector.s_TextureFormatStringsApplePVR;
					}
					else
					{
						array3 = TextureImportPlatformSettings.kTextureFormatsValueAndroid;
						array4 = TextureImporterInspector.s_TextureFormatStringsAndroid;
					}
				}
				else if (textureImporterType == TextureImporterType.NormalMap)
				{
					array3 = TextureImportPlatformSettings.kNormalFormatsValueDefault;
					array4 = TextureImporterInspector.s_NormalFormatStringsDefault;
				}
				else if (platformSettings.m_Target == BuildTarget.WebGL)
				{
					array3 = TextureImportPlatformSettings.kTextureFormatsValueWebGL;
					array4 = TextureImporterInspector.s_TextureFormatStringsWebGL;
				}
				else if (platformSettings.m_Target == BuildTarget.PSP2)
				{
					array3 = TextureImportPlatformSettings.kTextureFormatsValuePSP2;
					array4 = TextureImporterInspector.s_TextureFormatStringsPSP2;
				}
				else if (platformSettings.m_Target == BuildTarget.Switch)
				{
					array3 = TextureImportPlatformSettings.kTextureFormatsValueSwitch;
					array4 = TextureImporterInspector.s_TextureFormatStringsSwitch;
				}
				else
				{
					array3 = TextureImportPlatformSettings.kTextureFormatsValueDefault;
					array4 = TextureImporterInspector.s_TextureFormatStringsDefault;
				}
				if (i == 0)
				{
					array = array3;
					array2 = array4;
					num = num2;
				}
				else if (!array3.SequenceEqual(array) || !array4.SequenceEqual(array2))
				{
					flag = true;
					break;
				}
			}
			using (new EditorGUI.DisabledScope(flag || array2.Length == 1))
			{
				EditorGUI.BeginChangeCheck();
				EditorGUI.showMixedValue = (flag || platformSettings.textureFormatIsDifferent);
				num = EditorGUILayout.IntPopup(TextureImporterInspector.s_Styles.textureFormat, num, EditorGUIUtility.TempContent(array2), array, new GUILayoutOption[0]);
				EditorGUI.showMixedValue = false;
				if (EditorGUI.EndChangeCheck())
				{
					platformSettings.SetTextureFormatForAll((TextureImporterFormat)num);
				}
				if (Array.IndexOf<int>(array, num) == -1)
				{
					platformSettings.SetTextureFormatForAll((TextureImporterFormat)array[0]);
				}
			}
			if (platformSettings.isDefault && platformSettings.textureCompression != TextureImporterCompression.Uncompressed)
			{
				EditorGUI.BeginChangeCheck();
				EditorGUI.showMixedValue = (platformSettings.overriddenIsDifferent || platformSettings.crunchedCompressionIsDifferent);
				bool crunchedCompressionForAll = EditorGUILayout.Toggle(TextureImporterInspector.s_Styles.crunchedCompression, platformSettings.crunchedCompression, new GUILayoutOption[0]);
				EditorGUI.showMixedValue = false;
				if (EditorGUI.EndChangeCheck())
				{
					platformSettings.SetCrunchedCompressionForAll(crunchedCompressionForAll);
				}
			}
			bool flag2 = false || num == 28 || num == 29 || num == 64 || num == 65;
			if ((platformSettings.isDefault && platformSettings.textureCompression != TextureImporterCompression.Uncompressed && platformSettings.crunchedCompression) || (!platformSettings.isDefault && flag2) || (!platformSettings.textureFormatIsDifferent && ArrayUtility.Contains<TextureImporterFormat>(TextureImporterInspector.kFormatsWithCompressionSettings, (TextureImporterFormat)num)))
			{
				EditorGUI.BeginChangeCheck();
				EditorGUI.showMixedValue = (platformSettings.overriddenIsDifferent || platformSettings.compressionQualityIsDifferent);
				int compressionQualityForAll = this.EditCompressionQuality(platformSettings.m_Target, platformSettings.compressionQuality, flag2);
				EditorGUI.showMixedValue = false;
				if (EditorGUI.EndChangeCheck())
				{
					platformSettings.SetCompressionQualityForAll(compressionQualityForAll);
				}
			}
			bool flag3 = TextureImporter.IsETC1SupportedByBuildTarget(BuildPipeline.GetBuildTargetByName(platformSettings.name));
			bool flag4 = textureImporterInspector.spriteImportMode != SpriteImportMode.None;
			bool flag5 = TextureImporter.IsTextureFormatETC1Compression((TextureFormat)num);
			if (flag3 && flag4 && flag5)
			{
				EditorGUI.BeginChangeCheck();
				EditorGUI.showMixedValue = (platformSettings.overriddenIsDifferent || platformSettings.allowsAlphaSplitIsDifferent);
				bool allowsAlphaSplitForAll = GUILayout.Toggle(platformSettings.allowsAlphaSplitting, TextureImporterInspector.s_Styles.etc1Compression, new GUILayoutOption[0]);
				if (EditorGUI.EndChangeCheck())
				{
					platformSettings.SetAllowsAlphaSplitForAll(allowsAlphaSplitForAll);
				}
			}
		}

		private int EditCompressionQuality(BuildTarget target, int compression, bool isCrunchedFormat)
		{
			bool flag = !isCrunchedFormat && (target == BuildTarget.iOS || target == BuildTarget.tvOS || target == BuildTarget.Android || target == BuildTarget.Tizen);
			int result;
			if (flag)
			{
				int selectedIndex = 1;
				if (compression == 0)
				{
					selectedIndex = 0;
				}
				else if (compression == 100)
				{
					selectedIndex = 2;
				}
				switch (EditorGUILayout.Popup(TextureImporterInspector.s_Styles.compressionQuality, selectedIndex, TextureImporterInspector.s_Styles.mobileCompressionQualityOptions, new GUILayoutOption[0]))
				{
				case 0:
					result = 0;
					break;
				case 1:
					result = 50;
					break;
				case 2:
					result = 100;
					break;
				default:
					result = 50;
					break;
				}
			}
			else
			{
				compression = EditorGUILayout.IntSlider(TextureImporterInspector.s_Styles.compressionQualitySlider, compression, 0, 100, new GUILayoutOption[0]);
				result = compression;
			}
			return result;
		}
	}
}
