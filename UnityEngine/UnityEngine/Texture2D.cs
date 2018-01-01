using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;
using UnityEngine.Scripting;

namespace UnityEngine
{
	[UsedByNativeCode]
	public sealed class Texture2D : Texture
	{
		[Flags]
		public enum EXRFlags
		{
			None = 0,
			OutputAsFloat = 1,
			CompressZIP = 2,
			CompressRLE = 4,
			CompressPIZ = 8
		}

		public extern bool alphaIsTransparency
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public extern int mipmapCount
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public extern TextureFormat format
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public static extern Texture2D whiteTexture
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public static extern Texture2D blackTexture
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		internal Texture2D(int width, int height, TextureFormat format, bool mipmap, bool linear, IntPtr nativeTex)
		{
			Texture2D.Internal_Create(this, width, height, format, mipmap, linear, nativeTex);
		}

		public Texture2D(int width, int height, [DefaultValue("TextureFormat.RGBA32")] TextureFormat format, [DefaultValue("true")] bool mipmap, [DefaultValue("false")] bool linear) : this(width, height, format, mipmap, linear, IntPtr.Zero)
		{
		}

		public Texture2D(int width, int height, TextureFormat format, bool mipmap) : this(width, height, format, mipmap, false, IntPtr.Zero)
		{
		}

		public Texture2D(int width, int height) : this(width, height, TextureFormat.RGBA32, true, false, IntPtr.Zero)
		{
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void UpdateExternalTexture(IntPtr nativeTex);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetAllPixels32(Color32[] colors, int miplevel);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetBlockOfPixels32(int x, int y, int blockWidth, int blockHeight, Color32[] colors, int miplevel);

		[ExcludeFromDocs]
		public void SetPixels32(Color32[] colors)
		{
			int miplevel = 0;
			this.SetPixels32(colors, miplevel);
		}

		public void SetPixels32(Color32[] colors, [DefaultValue("0")] int miplevel)
		{
			this.SetAllPixels32(colors, miplevel);
		}

		[ExcludeFromDocs]
		public void SetPixels32(int x, int y, int blockWidth, int blockHeight, Color32[] colors)
		{
			int miplevel = 0;
			this.SetPixels32(x, y, blockWidth, blockHeight, colors, miplevel);
		}

		public void SetPixels32(int x, int y, int blockWidth, int blockHeight, Color32[] colors, [DefaultValue("0")] int miplevel)
		{
			this.SetBlockOfPixels32(x, y, blockWidth, blockHeight, colors, miplevel);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern byte[] GetRawTextureData();

		[ExcludeFromDocs]
		public Color[] GetPixels()
		{
			int miplevel = 0;
			return this.GetPixels(miplevel);
		}

		public Color[] GetPixels([DefaultValue("0")] int miplevel)
		{
			int num = this.width >> miplevel;
			if (num < 1)
			{
				num = 1;
			}
			int num2 = this.height >> miplevel;
			if (num2 < 1)
			{
				num2 = 1;
			}
			return this.GetPixels(0, 0, num, num2, miplevel);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern Color[] GetPixels(int x, int y, int blockWidth, int blockHeight, [DefaultValue("0")] int miplevel);

		[ExcludeFromDocs]
		public Color[] GetPixels(int x, int y, int blockWidth, int blockHeight)
		{
			int miplevel = 0;
			return this.GetPixels(x, y, blockWidth, blockHeight, miplevel);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern Color32[] GetPixels32([DefaultValue("0")] int miplevel);

		[ExcludeFromDocs]
		public Color32[] GetPixels32()
		{
			int miplevel = 0;
			return this.GetPixels32(miplevel);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool Resize(int width, int height, TextureFormat format, bool hasMipMap);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern Rect[] PackTextures(Texture2D[] textures, int padding, [DefaultValue("2048")] int maximumAtlasSize, [DefaultValue("false")] bool makeNoLongerReadable);

		[ExcludeFromDocs]
		public Rect[] PackTextures(Texture2D[] textures, int padding, int maximumAtlasSize)
		{
			bool makeNoLongerReadable = false;
			return this.PackTextures(textures, padding, maximumAtlasSize, makeNoLongerReadable);
		}

		[ExcludeFromDocs]
		public Rect[] PackTextures(Texture2D[] textures, int padding)
		{
			bool makeNoLongerReadable = false;
			int maximumAtlasSize = 2048;
			return this.PackTextures(textures, padding, maximumAtlasSize, makeNoLongerReadable);
		}

		public static bool GenerateAtlas(Vector2[] sizes, int padding, int atlasSize, List<Rect> results)
		{
			if (sizes == null)
			{
				throw new ArgumentException("sizes array can not be null");
			}
			if (results == null)
			{
				throw new ArgumentException("results list cannot be null");
			}
			if (padding < 0)
			{
				throw new ArgumentException("padding can not be negative");
			}
			if (atlasSize <= 0)
			{
				throw new ArgumentException("atlas size must be positive");
			}
			results.Clear();
			bool result;
			if (sizes.Length == 0)
			{
				result = true;
			}
			else
			{
				Texture2D.GenerateAtlasInternal(sizes, padding, atlasSize, results);
				result = (results.Count != 0);
			}
			return result;
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GenerateAtlasInternal(Vector2[] sizes, int padding, int atlasSize, object resultList);

		public void ReadPixels(Rect source, int destX, int destY, [DefaultValue("true")] bool recalculateMipMaps)
		{
			Texture2D.INTERNAL_CALL_ReadPixels(this, ref source, destX, destY, recalculateMipMaps);
		}

		[ExcludeFromDocs]
		public void ReadPixels(Rect source, int destX, int destY)
		{
			bool recalculateMipMaps = true;
			Texture2D.INTERNAL_CALL_ReadPixels(this, ref source, destX, destY, recalculateMipMaps);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_ReadPixels(Texture2D self, ref Rect source, int destX, int destY, bool recalculateMipMaps);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void Compress(bool highQuality);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool Internal_CreateImpl([Writable] Texture2D mono, int w, int h, TextureFormat format, bool mipmap, bool linear, IntPtr nativeTex);

		private static void Internal_Create([Writable] Texture2D mono, int w, int h, TextureFormat format, bool mipmap, bool linear, IntPtr nativeTex)
		{
			if (!Texture2D.Internal_CreateImpl(mono, w, h, format, mipmap, linear, nativeTex))
			{
				throw new UnityException("Failed to create texture because of invalid parameters.");
			}
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern bool IsReadable();

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void ApplyImpl(bool updateMipmaps, bool makeNoLongerReadable);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern bool ResizeImpl(int width, int height);

		private void SetPixelImpl(int image, int x, int y, Color color)
		{
			this.SetPixelImpl_Injected(image, x, y, ref color);
		}

		private Color GetPixelImpl(int image, int x, int y)
		{
			Color result;
			this.GetPixelImpl_Injected(image, x, y, out result);
			return result;
		}

		private Color GetPixelBilinearImpl(int image, float x, float y)
		{
			Color result;
			this.GetPixelBilinearImpl_Injected(image, x, y, out result);
			return result;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetPixelsImpl(int x, int y, int w, int h, Color[] pixel, int miplevel, int frame);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern bool LoadRawTextureDataImpl(IntPtr data, int size);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern bool LoadRawTextureDataImplArray(byte[] data);

		public static Texture2D CreateExternalTexture(int width, int height, TextureFormat format, bool mipmap, bool linear, IntPtr nativeTex)
		{
			if (nativeTex == IntPtr.Zero)
			{
				throw new ArgumentException("nativeTex can not be null");
			}
			return new Texture2D(width, height, format, mipmap, linear, nativeTex);
		}

		public void SetPixel(int x, int y, Color color)
		{
			if (!this.IsReadable())
			{
				throw base.CreateNonReadableException(this);
			}
			this.SetPixelImpl(0, x, y, color);
		}

		public void SetPixels(int x, int y, int blockWidth, int blockHeight, Color[] colors, [DefaultValue("0")] int miplevel)
		{
			if (!this.IsReadable())
			{
				throw base.CreateNonReadableException(this);
			}
			this.SetPixelsImpl(x, y, blockWidth, blockHeight, colors, miplevel, 0);
		}

		public void SetPixels(int x, int y, int blockWidth, int blockHeight, Color[] colors)
		{
			this.SetPixels(x, y, blockWidth, blockHeight, colors, 0);
		}

		public void SetPixels(Color[] colors, [DefaultValue("0")] int miplevel)
		{
			int num = this.width >> miplevel;
			if (num < 1)
			{
				num = 1;
			}
			int num2 = this.height >> miplevel;
			if (num2 < 1)
			{
				num2 = 1;
			}
			this.SetPixels(0, 0, num, num2, colors, miplevel);
		}

		public void SetPixels(Color[] colors)
		{
			this.SetPixels(0, 0, this.width, this.height, colors, 0);
		}

		public Color GetPixel(int x, int y)
		{
			if (!this.IsReadable())
			{
				throw base.CreateNonReadableException(this);
			}
			return this.GetPixelImpl(0, x, y);
		}

		public Color GetPixelBilinear(float x, float y)
		{
			if (!this.IsReadable())
			{
				throw base.CreateNonReadableException(this);
			}
			return this.GetPixelBilinearImpl(0, x, y);
		}

		public void LoadRawTextureData(IntPtr data, int size)
		{
			if (!this.IsReadable())
			{
				throw base.CreateNonReadableException(this);
			}
			if (data == IntPtr.Zero || size == 0)
			{
				Debug.LogError("No texture data provided to LoadRawTextureData", this);
			}
			else if (!this.LoadRawTextureDataImpl(data, size))
			{
				throw new UnityException("LoadRawTextureData: not enough data provided (will result in overread).");
			}
		}

		public void LoadRawTextureData(byte[] data)
		{
			if (!this.IsReadable())
			{
				throw base.CreateNonReadableException(this);
			}
			if (data == null || data.Length == 0)
			{
				Debug.LogError("No texture data provided to LoadRawTextureData", this);
			}
			else if (!this.LoadRawTextureDataImplArray(data))
			{
				throw new UnityException("LoadRawTextureData: not enough data provided (will result in overread).");
			}
		}

		public void Apply([DefaultValue("true")] bool updateMipmaps, [DefaultValue("false")] bool makeNoLongerReadable)
		{
			if (!this.IsReadable())
			{
				throw base.CreateNonReadableException(this);
			}
			this.ApplyImpl(updateMipmaps, makeNoLongerReadable);
		}

		public void Apply(bool updateMipmaps)
		{
			this.Apply(updateMipmaps, false);
		}

		public void Apply()
		{
			this.Apply(true, false);
		}

		public bool Resize(int width, int height)
		{
			if (!this.IsReadable())
			{
				throw base.CreateNonReadableException(this);
			}
			return this.ResizeImpl(width, height);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetPixelImpl_Injected(int image, int x, int y, ref Color color);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetPixelImpl_Injected(int image, int x, int y, out Color ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetPixelBilinearImpl_Injected(int image, float x, float y, out Color ret);
	}
}
