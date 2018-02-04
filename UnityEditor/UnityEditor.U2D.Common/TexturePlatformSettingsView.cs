using System;
using UnityEditor.U2D.Interface;
using UnityEngine;

namespace UnityEditor.U2D.Common
{
	internal class TexturePlatformSettingsView : ITexturePlatformSettingsView
	{
		private class Styles
		{
			public readonly GUIContent textureFormatLabel = EditorGUIUtility.TrTextContent("Format", null, null);

			public readonly GUIContent maxTextureSizeLabel = EditorGUIUtility.TrTextContent("Max Texture Size", "Maximum size of the packed texture.", null);

			public readonly GUIContent compressionLabel = EditorGUIUtility.TrTextContent("Compression", "How will this texture be compressed?", null);

			public readonly GUIContent useCrunchedCompressionLabel = EditorGUIUtility.TrTextContent("Use Crunch Compression", "Texture is crunch-compressed to save space on disk when applicable.", null);

			public readonly GUIContent compressionQualityLabel = EditorGUIUtility.TrTextContent("Compressor Quality", null, null);

			public readonly GUIContent compressionQualitySliderLabel = EditorGUIUtility.TrTextContent("Compressor Quality", "Use the slider to adjust compression quality from 0 (Fastest) to 100 (Best)", null);

			public readonly int[] kMaxTextureSizeValues = new int[]
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

			public readonly GUIContent[] kMaxTextureSizeStrings;

			public readonly GUIContent[] kTextureCompressionOptions = new GUIContent[]
			{
				EditorGUIUtility.TrTextContent("None", "Texture is not compressed.", null),
				EditorGUIUtility.TrTextContent("Low Quality", "Texture compressed with low quality but high performance, high compression format.", null),
				EditorGUIUtility.TrTextContent("Normal Quality", "Texture is compressed with a standard format.", null),
				EditorGUIUtility.TrTextContent("High Quality", "Texture compressed with a high quality format.", null)
			};

			public readonly int[] kTextureCompressionValues = new int[]
			{
				0,
				3,
				1,
				2
			};

			public readonly GUIContent[] kMobileCompressionQualityOptions = new GUIContent[]
			{
				EditorGUIUtility.TrTextContent("Fast", null, null),
				EditorGUIUtility.TrTextContent("Normal", null, null),
				EditorGUIUtility.TrTextContent("Best", null, null)
			};

			public Styles()
			{
				this.kMaxTextureSizeStrings = new GUIContent[this.kMaxTextureSizeValues.Length];
				for (int i = 0; i < this.kMaxTextureSizeValues.Length; i++)
				{
					this.kMaxTextureSizeStrings[i] = EditorGUIUtility.TextContent(string.Format("{0}", this.kMaxTextureSizeValues[i]));
				}
			}
		}

		private static TexturePlatformSettingsView.Styles s_Styles;

		public string buildPlatformTitle
		{
			get;
			set;
		}

		internal TexturePlatformSettingsView()
		{
			TexturePlatformSettingsView.s_Styles = (TexturePlatformSettingsView.s_Styles ?? new TexturePlatformSettingsView.Styles());
		}

		public virtual TextureImporterCompression DrawCompression(TextureImporterCompression defaultValue, bool isMixedValue, out bool changed)
		{
			EditorGUI.BeginChangeCheck();
			EditorGUI.showMixedValue = isMixedValue;
			defaultValue = (TextureImporterCompression)EditorGUILayout.IntPopup(TexturePlatformSettingsView.s_Styles.compressionLabel, (int)defaultValue, TexturePlatformSettingsView.s_Styles.kTextureCompressionOptions, TexturePlatformSettingsView.s_Styles.kTextureCompressionValues, new GUILayoutOption[0]);
			EditorGUI.showMixedValue = false;
			changed = EditorGUI.EndChangeCheck();
			return defaultValue;
		}

		public virtual bool DrawUseCrunchedCompression(bool defaultValue, bool isMixedValue, out bool changed)
		{
			EditorGUI.BeginChangeCheck();
			EditorGUI.showMixedValue = isMixedValue;
			defaultValue = EditorGUILayout.Toggle(TexturePlatformSettingsView.s_Styles.useCrunchedCompressionLabel, defaultValue, new GUILayoutOption[0]);
			EditorGUI.showMixedValue = false;
			changed = EditorGUI.EndChangeCheck();
			return defaultValue;
		}

		public virtual bool DrawOverride(bool defaultValue, bool isMixedValue, out bool changed)
		{
			EditorGUI.BeginChangeCheck();
			EditorGUI.showMixedValue = isMixedValue;
			defaultValue = EditorGUILayout.ToggleLeft(EditorGUIUtility.TempContent("Override for " + this.buildPlatformTitle), defaultValue, new GUILayoutOption[0]);
			EditorGUI.showMixedValue = false;
			changed = EditorGUI.EndChangeCheck();
			return defaultValue;
		}

		public virtual int DrawMaxSize(int defaultValue, bool isMixedValue, out bool changed)
		{
			EditorGUI.BeginChangeCheck();
			EditorGUI.showMixedValue = isMixedValue;
			defaultValue = EditorGUILayout.IntPopup(TexturePlatformSettingsView.s_Styles.maxTextureSizeLabel, defaultValue, TexturePlatformSettingsView.s_Styles.kMaxTextureSizeStrings, TexturePlatformSettingsView.s_Styles.kMaxTextureSizeValues, new GUILayoutOption[0]);
			EditorGUI.showMixedValue = false;
			changed = EditorGUI.EndChangeCheck();
			return defaultValue;
		}

		public virtual TextureImporterFormat DrawFormat(TextureImporterFormat defaultValue, int[] displayValues, string[] displayStrings, bool isMixedValue, bool isDisabled, out bool changed)
		{
			TextureImporterFormat result;
			using (new EditorGUI.DisabledScope(isDisabled))
			{
				EditorGUI.BeginChangeCheck();
				EditorGUI.showMixedValue = isMixedValue;
				defaultValue = (TextureImporterFormat)EditorGUILayout.IntPopup(TexturePlatformSettingsView.s_Styles.textureFormatLabel, (int)defaultValue, EditorGUIUtility.TempContent(displayStrings), displayValues, new GUILayoutOption[0]);
				EditorGUI.showMixedValue = false;
				changed = EditorGUI.EndChangeCheck();
				result = defaultValue;
			}
			return result;
		}

		public virtual int DrawCompressionQualityPopup(int defaultValue, bool isMixedValue, out bool changed)
		{
			EditorGUI.BeginChangeCheck();
			EditorGUI.showMixedValue = isMixedValue;
			defaultValue = EditorGUILayout.Popup(TexturePlatformSettingsView.s_Styles.compressionQualityLabel, defaultValue, TexturePlatformSettingsView.s_Styles.kMobileCompressionQualityOptions, new GUILayoutOption[0]);
			EditorGUI.showMixedValue = false;
			changed = EditorGUI.EndChangeCheck();
			return defaultValue;
		}

		public virtual int DrawCompressionQualitySlider(int defaultValue, bool isMixedValue, out bool changed)
		{
			EditorGUI.BeginChangeCheck();
			EditorGUI.showMixedValue = isMixedValue;
			defaultValue = EditorGUILayout.IntSlider(TexturePlatformSettingsView.s_Styles.compressionQualitySliderLabel, defaultValue, 0, 100, new GUILayoutOption[0]);
			EditorGUI.showMixedValue = false;
			changed = EditorGUI.EndChangeCheck();
			return defaultValue;
		}
	}
}
