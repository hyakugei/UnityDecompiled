using System;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;
using UnityEngine.Scripting;

namespace UnityEngine
{
	public sealed class CubemapArray : Texture
	{
		public extern int cubemapCount
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public extern TextureFormat format
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public CubemapArray(int faceSize, int cubemapCount, TextureFormat format, bool mipmap, [DefaultValue("false")] bool linear)
		{
			CubemapArray.Internal_Create(this, faceSize, cubemapCount, format, mipmap, linear);
		}

		public CubemapArray(int faceSize, int cubemapCount, TextureFormat format, bool mipmap) : this(faceSize, cubemapCount, format, mipmap, false)
		{
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetPixels(Color[] colors, CubemapFace face, int arrayElement, [DefaultValue("0")] int miplevel);

		[ExcludeFromDocs]
		public void SetPixels(Color[] colors, CubemapFace face, int arrayElement)
		{
			int miplevel = 0;
			this.SetPixels(colors, face, arrayElement, miplevel);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetPixels32(Color32[] colors, CubemapFace face, int arrayElement, [DefaultValue("0")] int miplevel);

		[ExcludeFromDocs]
		public void SetPixels32(Color32[] colors, CubemapFace face, int arrayElement)
		{
			int miplevel = 0;
			this.SetPixels32(colors, face, arrayElement, miplevel);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern Color[] GetPixels(CubemapFace face, int arrayElement, [DefaultValue("0")] int miplevel);

		[ExcludeFromDocs]
		public Color[] GetPixels(CubemapFace face, int arrayElement)
		{
			int miplevel = 0;
			return this.GetPixels(face, arrayElement, miplevel);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern Color32[] GetPixels32(CubemapFace face, int arrayElement, [DefaultValue("0")] int miplevel);

		[ExcludeFromDocs]
		public Color32[] GetPixels32(CubemapFace face, int arrayElement)
		{
			int miplevel = 0;
			return this.GetPixels32(face, arrayElement, miplevel);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern bool IsReadable();

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool Internal_CreateImpl([Writable] CubemapArray mono, int ext, int count, TextureFormat format, bool mipmap, bool linear);

		private static void Internal_Create([Writable] CubemapArray mono, int ext, int count, TextureFormat format, bool mipmap, bool linear)
		{
			if (!CubemapArray.Internal_CreateImpl(mono, ext, count, format, mipmap, linear))
			{
				throw new UnityException("Failed to create cubemap array texture because of invalid parameters.");
			}
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void ApplyImpl(bool updateMipmaps, bool makeNoLongerReadable);

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
	}
}
