using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;
using UnityEngine.Rendering;
using UnityEngine.Scripting;

namespace UnityEngine
{
	[UsedByNativeCode]
	public class RenderTexture : Texture
	{
		public extern int depth
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public RenderBuffer colorBuffer
		{
			get
			{
				RenderBuffer result;
				this.GetColorBuffer(out result);
				return result;
			}
		}

		public RenderBuffer depthBuffer
		{
			get
			{
				RenderBuffer result;
				this.GetDepthBuffer(out result);
				return result;
			}
		}

		public static extern RenderTexture active
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		[Obsolete("RenderTexture.enabled is always now, no need to use it")]
		public static extern bool enabled
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public override extern int width
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public override extern int height
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public override extern TextureDimension dimension
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public extern bool useMipMap
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public extern bool sRGB
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public extern RenderTextureFormat format
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public extern VRTextureUsage vrUsage
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public extern RenderTextureMemoryless memorylessMode
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public extern bool autoGenerateMips
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public extern int volumeDepth
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public extern int antiAliasing
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public extern bool bindTextureMS
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public extern bool enableRandomWrite
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public extern bool useDynamicScale
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public bool isPowerOfTwo
		{
			get
			{
				return this.GetIsPowerOfTwo();
			}
			set
			{
			}
		}

		[Obsolete("Use RenderTexture.dimension instead.", false)]
		public bool isCubemap
		{
			get
			{
				return this.dimension == TextureDimension.Cube;
			}
			set
			{
				this.dimension = ((!value) ? TextureDimension.Tex2D : TextureDimension.Cube);
			}
		}

		[Obsolete("Use RenderTexture.dimension instead.", false)]
		public bool isVolume
		{
			get
			{
				return this.dimension == TextureDimension.Tex3D;
			}
			set
			{
				this.dimension = ((!value) ? TextureDimension.Tex2D : TextureDimension.Tex3D);
			}
		}

		public RenderTextureDescriptor descriptor
		{
			get
			{
				return this.GetDescriptor();
			}
			set
			{
				RenderTexture.ValidateRenderTextureDesc(value);
				this.SetRenderTextureDescriptor(value);
			}
		}

		[EditorBrowsable(EditorBrowsableState.Never), Obsolete("Use RenderTexture.autoGenerateMips instead (UnityUpgradable) -> autoGenerateMips", false)]
		public bool generateMips
		{
			get
			{
				return this.autoGenerateMips;
			}
			set
			{
				this.autoGenerateMips = value;
			}
		}

		protected internal RenderTexture()
		{
		}

		public RenderTexture(RenderTextureDescriptor desc)
		{
			RenderTexture.ValidateRenderTextureDesc(desc);
			RenderTexture.Internal_Create(this);
			this.SetRenderTextureDescriptor(desc);
		}

		public RenderTexture(RenderTexture textureToCopy)
		{
			if (textureToCopy == null)
			{
				throw new ArgumentNullException("textureToCopy");
			}
			RenderTexture.ValidateRenderTextureDesc(textureToCopy.descriptor);
			RenderTexture.Internal_Create(this);
			this.SetRenderTextureDescriptor(textureToCopy.descriptor);
		}

		public RenderTexture(int width, int height, int depth, [UnityEngine.Internal.DefaultValue("RenderTextureFormat.Default")] RenderTextureFormat format, [UnityEngine.Internal.DefaultValue("RenderTextureReadWrite.Default")] RenderTextureReadWrite readWrite)
		{
			RenderTexture.Internal_Create(this);
			this.width = width;
			this.height = height;
			this.depth = depth;
			this.format = format;
			bool flag = QualitySettings.activeColorSpace == ColorSpace.Linear;
			this.SetSRGBReadWrite((readWrite != RenderTextureReadWrite.Default) ? (readWrite == RenderTextureReadWrite.sRGB) : flag);
		}

		public RenderTexture(int width, int height, int depth, RenderTextureFormat format) : this(width, height, depth, format, RenderTextureReadWrite.Default)
		{
		}

		public RenderTexture(int width, int height, int depth) : this(width, height, depth, RenderTextureFormat.Default, RenderTextureReadWrite.Default)
		{
		}

		private void SetRenderTextureDescriptor(RenderTextureDescriptor desc)
		{
			RenderTexture.INTERNAL_CALL_SetRenderTextureDescriptor(this, ref desc);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_SetRenderTextureDescriptor(RenderTexture self, ref RenderTextureDescriptor desc);

		private RenderTextureDescriptor GetDescriptor()
		{
			RenderTextureDescriptor result;
			RenderTexture.INTERNAL_CALL_GetDescriptor(this, out result);
			return result;
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_GetDescriptor(RenderTexture self, out RenderTextureDescriptor value);

		private static RenderTexture GetTemporary_Internal(RenderTextureDescriptor desc)
		{
			return RenderTexture.INTERNAL_CALL_GetTemporary_Internal(ref desc);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern RenderTexture INTERNAL_CALL_GetTemporary_Internal(ref RenderTextureDescriptor desc);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void ReleaseTemporary(RenderTexture temp);

		public void ResolveAntiAliasedSurface()
		{
			this.Internal_ResolveAntiAliasedSurface(null);
		}

		public void ResolveAntiAliasedSurface(RenderTexture target)
		{
			this.Internal_ResolveAntiAliasedSurface(target);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Internal_ResolveAntiAliasedSurface(RenderTexture target);

		public void DiscardContents()
		{
			RenderTexture.INTERNAL_CALL_DiscardContents(this);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_DiscardContents(RenderTexture self);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void DiscardContents(bool discardColor, bool discardDepth);

		public void MarkRestoreExpected()
		{
			RenderTexture.INTERNAL_CALL_MarkRestoreExpected(this);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_MarkRestoreExpected(RenderTexture self);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetColorBuffer(out RenderBuffer res);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetDepthBuffer(out RenderBuffer res);

		public IntPtr GetNativeDepthBufferPtr()
		{
			IntPtr result;
			RenderTexture.INTERNAL_CALL_GetNativeDepthBufferPtr(this, out result);
			return result;
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_GetNativeDepthBufferPtr(RenderTexture self, out IntPtr value);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetGlobalShaderProperty(string propertyName);

		[Obsolete("GetTexelOffset always returns zero now, no point in using it.")]
		public Vector2 GetTexelOffset()
		{
			return Vector2.zero;
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool SupportsStencil(RenderTexture rt);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern VRTextureUsage GetActiveVRUsage();

		[Obsolete("SetBorderColor is no longer supported.", true)]
		public void SetBorderColor(Color color)
		{
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern bool GetIsPowerOfTwo();

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool Create();

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void Release();

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool IsCreated();

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void GenerateMips();

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void ConvertToEquirect(RenderTexture equirect, Camera.MonoOrStereoscopicEye eye = Camera.MonoOrStereoscopicEye.Mono);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void SetSRGBReadWrite(bool srgb);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_Create([Writable] RenderTexture rt);

		private static void ValidateRenderTextureDesc(RenderTextureDescriptor desc)
		{
			if (desc.width <= 0)
			{
				throw new ArgumentException("RenderTextureDesc width must be greater than zero.", "desc.width");
			}
			if (desc.height <= 0)
			{
				throw new ArgumentException("RenderTextureDesc height must be greater than zero.", "desc.height");
			}
			if (desc.volumeDepth <= 0)
			{
				throw new ArgumentException("RenderTextureDesc volumeDepth must be greater than zero.", "desc.volumeDepth");
			}
			if (desc.msaaSamples != 1 && desc.msaaSamples != 2 && desc.msaaSamples != 4 && desc.msaaSamples != 8)
			{
				throw new ArgumentException("RenderTextureDesc msaaSamples must be 1, 2, 4, or 8.", "desc.msaaSamples");
			}
			if (desc.depthBufferBits != 0 && desc.depthBufferBits != 16 && desc.depthBufferBits != 24)
			{
				throw new ArgumentException("RenderTextureDesc depthBufferBits must be 0, 16, or 24.", "desc.depthBufferBits");
			}
		}

		public static RenderTexture GetTemporary(RenderTextureDescriptor desc)
		{
			RenderTexture.ValidateRenderTextureDesc(desc);
			desc.createdFromScript = true;
			return RenderTexture.GetTemporary_Internal(desc);
		}

		private static RenderTexture GetTemporaryImpl(int width, int height, int depthBuffer = 0, RenderTextureFormat format = RenderTextureFormat.Default, RenderTextureReadWrite readWrite = RenderTextureReadWrite.Default, int antiAliasing = 1, RenderTextureMemoryless memorylessMode = RenderTextureMemoryless.None, VRTextureUsage vrUsage = VRTextureUsage.None, bool useDynamicScale = false)
		{
			return RenderTexture.GetTemporary(new RenderTextureDescriptor(width, height, format, depthBuffer)
			{
				sRGB = (readWrite != RenderTextureReadWrite.Linear),
				msaaSamples = antiAliasing,
				memoryless = memorylessMode,
				vrUsage = vrUsage,
				useDynamicScale = useDynamicScale
			});
		}

		public static RenderTexture GetTemporary(int width, int height, [UnityEngine.Internal.DefaultValue("0")] int depthBuffer, [UnityEngine.Internal.DefaultValue("RenderTextureFormat.Default")] RenderTextureFormat format, [UnityEngine.Internal.DefaultValue("RenderTextureReadWrite.Default")] RenderTextureReadWrite readWrite, [UnityEngine.Internal.DefaultValue("1")] int antiAliasing, [UnityEngine.Internal.DefaultValue("RenderTextureMemoryless.None")] RenderTextureMemoryless memorylessMode, [UnityEngine.Internal.DefaultValue("VRTextureUsage.None")] VRTextureUsage vrUsage, [UnityEngine.Internal.DefaultValue("false")] bool useDynamicScale)
		{
			return RenderTexture.GetTemporaryImpl(width, height, depthBuffer, format, readWrite, antiAliasing, memorylessMode, vrUsage, useDynamicScale);
		}

		public static RenderTexture GetTemporary(int width, int height, int depthBuffer, RenderTextureFormat format, RenderTextureReadWrite readWrite, int antiAliasing, RenderTextureMemoryless memorylessMode, VRTextureUsage vrUsage)
		{
			return RenderTexture.GetTemporaryImpl(width, height, depthBuffer, format, readWrite, antiAliasing, memorylessMode, vrUsage, false);
		}

		public static RenderTexture GetTemporary(int width, int height, int depthBuffer, RenderTextureFormat format, RenderTextureReadWrite readWrite, int antiAliasing, RenderTextureMemoryless memorylessMode)
		{
			return RenderTexture.GetTemporaryImpl(width, height, depthBuffer, format, readWrite, antiAliasing, memorylessMode, VRTextureUsage.None, false);
		}

		public static RenderTexture GetTemporary(int width, int height, int depthBuffer, RenderTextureFormat format, RenderTextureReadWrite readWrite, int antiAliasing)
		{
			return RenderTexture.GetTemporaryImpl(width, height, depthBuffer, format, readWrite, antiAliasing, RenderTextureMemoryless.None, VRTextureUsage.None, false);
		}

		public static RenderTexture GetTemporary(int width, int height, int depthBuffer, RenderTextureFormat format, RenderTextureReadWrite readWrite)
		{
			return RenderTexture.GetTemporaryImpl(width, height, depthBuffer, format, readWrite, 1, RenderTextureMemoryless.None, VRTextureUsage.None, false);
		}

		public static RenderTexture GetTemporary(int width, int height, int depthBuffer, RenderTextureFormat format)
		{
			return RenderTexture.GetTemporaryImpl(width, height, depthBuffer, format, RenderTextureReadWrite.Default, 1, RenderTextureMemoryless.None, VRTextureUsage.None, false);
		}

		public static RenderTexture GetTemporary(int width, int height, int depthBuffer)
		{
			return RenderTexture.GetTemporaryImpl(width, height, depthBuffer, RenderTextureFormat.Default, RenderTextureReadWrite.Default, 1, RenderTextureMemoryless.None, VRTextureUsage.None, false);
		}

		public static RenderTexture GetTemporary(int width, int height)
		{
			return RenderTexture.GetTemporaryImpl(width, height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Default, 1, RenderTextureMemoryless.None, VRTextureUsage.None, false);
		}
	}
}
