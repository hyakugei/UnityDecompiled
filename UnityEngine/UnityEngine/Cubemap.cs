using System;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;
using UnityEngine.Scripting;

namespace UnityEngine
{
	[ExcludeFromPreset]
	public sealed class Cubemap : Texture
	{
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

		internal Cubemap(int ext, TextureFormat format, bool mipmap, IntPtr nativeTex)
		{
			Cubemap.Internal_Create(this, ext, format, mipmap, nativeTex);
		}

		public Cubemap(int ext, TextureFormat format, bool mipmap) : this(ext, format, mipmap, IntPtr.Zero)
		{
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern Color[] GetPixels(CubemapFace face, [DefaultValue("0")] int miplevel);

		[ExcludeFromDocs]
		public Color[] GetPixels(CubemapFace face)
		{
			int miplevel = 0;
			return this.GetPixels(face, miplevel);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetPixels(Color[] colors, CubemapFace face, [DefaultValue("0")] int miplevel);

		[ExcludeFromDocs]
		public void SetPixels(Color[] colors, CubemapFace face)
		{
			int miplevel = 0;
			this.SetPixels(colors, face, miplevel);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool Internal_CreateImpl([Writable] Cubemap mono, int ext, TextureFormat format, bool mipmap, IntPtr nativeTex);

		private static void Internal_Create([Writable] Cubemap mono, int ext, TextureFormat format, bool mipmap, IntPtr nativeTex)
		{
			if (!Cubemap.Internal_CreateImpl(mono, ext, format, mipmap, nativeTex))
			{
				throw new UnityException("Failed to create texture because of invalid parameters.");
			}
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void ApplyImpl(bool updateMipmaps, bool makeNoLongerReadable);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern bool IsReadable();

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

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SmoothEdges([DefaultValue("1")] int smoothRegionWidthInPixels);

		public void SmoothEdges()
		{
			this.SmoothEdges(1);
		}

		public static Cubemap CreateExternalTexture(int ext, TextureFormat format, bool mipmap, IntPtr nativeTex)
		{
			if (nativeTex == IntPtr.Zero)
			{
				throw new ArgumentException("nativeTex can not be null");
			}
			return new Cubemap(ext, format, mipmap, nativeTex);
		}

		public void SetPixel(CubemapFace face, int x, int y, Color color)
		{
			if (!this.IsReadable())
			{
				throw base.CreateNonReadableException(this);
			}
			this.SetPixelImpl((int)face, x, y, color);
		}

		public Color GetPixel(CubemapFace face, int x, int y)
		{
			if (!this.IsReadable())
			{
				throw base.CreateNonReadableException(this);
			}
			return this.GetPixelImpl((int)face, x, y);
		}

		public void Apply([DefaultValue("true")] bool updateMipmaps, [DefaultValue("false")] bool makeNoLongerReadable)
		{
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

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetPixelImpl_Injected(int image, int x, int y, ref Color color);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetPixelImpl_Injected(int image, int x, int y, out Color ret);
	}
}
