using System;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;
using UnityEngine.Scripting;

namespace UnityEngine
{
	public sealed class Texture2DArray : Texture
	{
		public extern int depth
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public extern TextureFormat format
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public Texture2DArray(int width, int height, int depth, TextureFormat format, bool mipmap, [DefaultValue("false")] bool linear)
		{
			Texture2DArray.Internal_Create(this, width, height, depth, format, mipmap, linear);
		}

		public Texture2DArray(int width, int height, int depth, TextureFormat format, bool mipmap) : this(width, height, depth, format, mipmap, false)
		{
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetPixels(Color[] colors, int arrayElement, [DefaultValue("0")] int miplevel);

		[ExcludeFromDocs]
		public void SetPixels(Color[] colors, int arrayElement)
		{
			int miplevel = 0;
			this.SetPixels(colors, arrayElement, miplevel);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetPixels32(Color32[] colors, int arrayElement, [DefaultValue("0")] int miplevel);

		[ExcludeFromDocs]
		public void SetPixels32(Color32[] colors, int arrayElement)
		{
			int miplevel = 0;
			this.SetPixels32(colors, arrayElement, miplevel);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern Color[] GetPixels(int arrayElement, [DefaultValue("0")] int miplevel);

		[ExcludeFromDocs]
		public Color[] GetPixels(int arrayElement)
		{
			int miplevel = 0;
			return this.GetPixels(arrayElement, miplevel);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern Color32[] GetPixels32(int arrayElement, [DefaultValue("0")] int miplevel);

		[ExcludeFromDocs]
		public Color32[] GetPixels32(int arrayElement)
		{
			int miplevel = 0;
			return this.GetPixels32(arrayElement, miplevel);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern bool IsReadable();

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool Internal_CreateImpl([Writable] Texture2DArray mono, int w, int h, int d, TextureFormat format, bool mipmap, bool linear);

		private static void Internal_Create([Writable] Texture2DArray mono, int w, int h, int d, TextureFormat format, bool mipmap, bool linear)
		{
			if (!Texture2DArray.Internal_CreateImpl(mono, w, h, d, format, mipmap, linear))
			{
				throw new UnityException("Failed to create 2D array texture because of invalid parameters.");
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
