using System;
using UnityEngine;
using UnityEngine.Bindings;

namespace UnityEditor.Experimental.AssetImporters
{
	public struct TextureGenerationSettings
	{
		[NativeName("assetPath")]
		private string m_AssetPath;

		[NativeName("imageData")]
		private IntPtr m_ImageData;

		[NativeName("imageDataSize")]
		private int m_ImageDataSize;

		[NativeName("qualifyForSpritePacking")]
		private bool m_QualifyForSpritePacking;

		[NativeName("enablePostProcessor")]
		private bool m_EnablePostProcessor;

		[NativeName("tiSettings")]
		private TextureImporterSettings m_Settings;

		[NativeName("platformSettings")]
		private TextureImporterPlatformSettings m_PlatformSettings;

		[NativeName("sourceTextureInformation")]
		private SourceTextureInformation m_SourceTextureInformation;

		[NativeName("spriteSheetData")]
		private SpriteImportData[] m_SpriteImportData;

		[NativeName("spritePackingTag")]
		private string m_SpritePackingTag;

		public string assetPath
		{
			get
			{
				return this.m_AssetPath;
			}
			set
			{
				this.m_AssetPath = value;
			}
		}

		public IntPtr imageData
		{
			get
			{
				return this.m_ImageData;
			}
			set
			{
				this.m_ImageData = value;
			}
		}

		public int imageDataSize
		{
			get
			{
				return this.m_ImageDataSize;
			}
			set
			{
				this.m_ImageDataSize = value;
			}
		}

		public bool qualifyForSpritePacking
		{
			get
			{
				return this.m_QualifyForSpritePacking;
			}
			set
			{
				this.m_QualifyForSpritePacking = value;
			}
		}

		public bool enablePostProcessor
		{
			get
			{
				return this.m_EnablePostProcessor;
			}
			set
			{
				this.m_EnablePostProcessor = value;
			}
		}

		public TextureImporterSettings textureImporterSettings
		{
			get
			{
				return this.m_Settings;
			}
			set
			{
				this.m_Settings = value;
			}
		}

		public TextureImporterPlatformSettings platformSettings
		{
			get
			{
				return this.m_PlatformSettings;
			}
			set
			{
				this.m_PlatformSettings = value;
			}
		}

		public SourceTextureInformation sourceTextureInformation
		{
			get
			{
				return this.m_SourceTextureInformation;
			}
			set
			{
				this.m_SourceTextureInformation = value;
			}
		}

		public SpriteImportData[] spriteImportData
		{
			get
			{
				return this.m_SpriteImportData;
			}
			set
			{
				this.m_SpriteImportData = value;
			}
		}

		public string spritePackingTag
		{
			get
			{
				return this.m_SpritePackingTag;
			}
			set
			{
				this.m_SpritePackingTag = value;
			}
		}

		public TextureGenerationSettings(TextureImporterType type)
		{
			this.m_EnablePostProcessor = true;
			this.m_AssetPath = "";
			this.m_ImageDataSize = 0;
			this.m_QualifyForSpritePacking = false;
			this.m_SpritePackingTag = "";
			this.m_ImageData = 0;
			this.m_SpriteImportData = null;
			this.m_SourceTextureInformation = new SourceTextureInformation();
			SourceTextureInformation arg_62_0 = this.m_SourceTextureInformation;
			int num = 0;
			this.m_SourceTextureInformation.height = num;
			arg_62_0.width = num;
			this.m_SourceTextureInformation.containsAlpha = false;
			this.m_SourceTextureInformation.hdr = false;
			this.m_PlatformSettings = new TextureImporterPlatformSettings();
			this.m_PlatformSettings.overridden = false;
			this.m_PlatformSettings.format = TextureImporterFormat.Automatic;
			this.m_PlatformSettings.maxTextureSize = 2048;
			this.m_PlatformSettings.allowsAlphaSplitting = false;
			this.m_PlatformSettings.resizeAlgorithm = TextureResizeAlgorithm.Mitchell;
			this.m_PlatformSettings.compressionQuality = 50;
			this.m_PlatformSettings.crunchedCompression = false;
			this.m_PlatformSettings.name = TextureImporter.defaultPlatformName;
			this.m_Settings = new TextureImporterSettings();
			this.m_Settings.textureType = type;
			this.m_Settings.textureShape = TextureImporterShape.Texture2D;
			this.m_Settings.convertToNormalMap = false;
			this.m_Settings.mipmapEnabled = true;
			this.m_Settings.mipmapFilter = TextureImporterMipFilter.BoxFilter;
			this.m_Settings.sRGBTexture = true;
			this.m_Settings.borderMipmap = false;
			this.m_Settings.mipMapsPreserveCoverage = false;
			this.m_Settings.alphaTestReferenceValue = 0.5f;
			this.m_Settings.readable = false;
			this.m_Settings.fadeOut = false;
			this.m_Settings.mipmapFadeDistanceStart = 1;
			this.m_Settings.mipmapFadeDistanceEnd = 3;
			this.m_Settings.heightmapScale = 0.25f;
			this.m_Settings.normalMapFilter = TextureImporterNormalFilter.Standard;
			this.m_Settings.cubemapConvolution = TextureImporterCubemapConvolution.None;
			this.m_Settings.generateCubemap = TextureImporterGenerateCubemap.AutoCubemap;
			this.m_Settings.seamlessCubemap = false;
			this.m_Settings.npotScale = TextureImporterNPOTScale.ToNearest;
			this.m_Settings.spriteMode = 1;
			this.m_Settings.spriteExtrude = 1u;
			this.m_Settings.spriteMeshType = SpriteMeshType.Tight;
			this.m_Settings.spriteAlignment = 0;
			this.m_Settings.spritePivot = Vector2.one * 0.5f;
			this.m_Settings.spritePixelsPerUnit = 100f;
			this.m_Settings.spriteBorder = Vector4.zero;
			this.m_Settings.alphaSource = TextureImporterAlphaSource.FromInput;
			this.m_Settings.alphaIsTransparency = false;
			this.m_Settings.spriteTessellationDetail = -1f;
			TextureImporterSettings arg_2AD_0 = this.m_Settings;
			TextureWrapMode textureWrapMode = TextureWrapMode.Repeat;
			this.m_Settings.wrapModeW = textureWrapMode;
			textureWrapMode = textureWrapMode;
			this.m_Settings.wrapModeV = textureWrapMode;
			textureWrapMode = textureWrapMode;
			this.m_Settings.wrapModeU = textureWrapMode;
			arg_2AD_0.wrapMode = textureWrapMode;
			switch (type)
			{
			case TextureImporterType.Default:
				this.m_Settings.sRGBTexture = true;
				this.m_Settings.mipmapEnabled = true;
				break;
			case TextureImporterType.NormalMap:
				this.m_Settings.sRGBTexture = false;
				break;
			case TextureImporterType.GUI:
			{
				this.m_Settings.sRGBTexture = false;
				this.m_Settings.mipmapEnabled = false;
				this.m_Settings.alphaIsTransparency = true;
				this.m_Settings.npotScale = TextureImporterNPOTScale.None;
				this.m_Settings.aniso = 1;
				TextureImporterSettings arg_384_0 = this.m_Settings;
				textureWrapMode = TextureWrapMode.Clamp;
				this.m_Settings.wrapModeW = textureWrapMode;
				textureWrapMode = textureWrapMode;
				this.m_Settings.wrapModeV = textureWrapMode;
				textureWrapMode = textureWrapMode;
				this.m_Settings.wrapModeU = textureWrapMode;
				arg_384_0.wrapMode = textureWrapMode;
				break;
			}
			case TextureImporterType.Cookie:
			{
				this.m_Settings.borderMipmap = true;
				TextureImporterSettings arg_4B9_0 = this.m_Settings;
				textureWrapMode = TextureWrapMode.Clamp;
				this.m_Settings.wrapModeW = textureWrapMode;
				textureWrapMode = textureWrapMode;
				this.m_Settings.wrapModeV = textureWrapMode;
				textureWrapMode = textureWrapMode;
				this.m_Settings.wrapModeU = textureWrapMode;
				arg_4B9_0.wrapMode = textureWrapMode;
				this.m_Settings.aniso = 0;
				break;
			}
			case TextureImporterType.Lightmap:
				this.m_Settings.sRGBTexture = true;
				this.m_Settings.npotScale = TextureImporterNPOTScale.ToNearest;
				this.m_Settings.alphaIsTransparency = false;
				this.m_Settings.alphaSource = TextureImporterAlphaSource.None;
				break;
			case TextureImporterType.Cursor:
			{
				this.m_Settings.readable = true;
				this.m_Settings.alphaIsTransparency = true;
				this.m_Settings.mipmapEnabled = false;
				this.m_Settings.npotScale = TextureImporterNPOTScale.None;
				this.m_Settings.aniso = 1;
				TextureImporterSettings arg_472_0 = this.m_Settings;
				textureWrapMode = TextureWrapMode.Clamp;
				this.m_Settings.wrapModeW = textureWrapMode;
				textureWrapMode = textureWrapMode;
				this.m_Settings.wrapModeV = textureWrapMode;
				textureWrapMode = textureWrapMode;
				this.m_Settings.wrapModeU = textureWrapMode;
				arg_472_0.wrapMode = textureWrapMode;
				break;
			}
			case TextureImporterType.Sprite:
			{
				this.m_Settings.npotScale = TextureImporterNPOTScale.None;
				this.m_Settings.alphaIsTransparency = true;
				this.m_Settings.mipmapEnabled = false;
				this.m_Settings.sRGBTexture = true;
				TextureImporterSettings arg_3EF_0 = this.m_Settings;
				textureWrapMode = TextureWrapMode.Clamp;
				this.m_Settings.wrapModeW = textureWrapMode;
				textureWrapMode = textureWrapMode;
				this.m_Settings.wrapModeV = textureWrapMode;
				textureWrapMode = textureWrapMode;
				this.m_Settings.wrapModeU = textureWrapMode;
				arg_3EF_0.wrapMode = textureWrapMode;
				this.m_Settings.alphaSource = TextureImporterAlphaSource.FromInput;
				break;
			}
			case TextureImporterType.SingleChannel:
				this.m_Settings.sRGBTexture = false;
				break;
			}
		}
	}
}
