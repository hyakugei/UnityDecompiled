using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.XR.WSA
{
	[MovedFrom("UnityEngine.VR.WSA")]
	public sealed class HolographicSettings
	{
		public enum HolographicReprojectionMode
		{
			PositionAndOrientation,
			OrientationOnly,
			Disabled
		}

		public static extern bool IsContentProtectionEnabled
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern HolographicSettings.HolographicReprojectionMode ReprojectionMode
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		[Obsolete("Support for toggling latent frame presentation has been removed, and IsLatentFramePresentation will always return true", false)]
		public static bool IsLatentFramePresentation
		{
			get
			{
				return true;
			}
		}

		public static extern bool IsDisplayOpaque
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		[Obsolete("Support for toggling latent frame presentation has been removed", true)]
		public static void ActivateLatentFramePresentation(bool activated)
		{
		}

		public static void SetFocusPointForFrame(Vector3 position)
		{
			HolographicSettings.InternalSetFocusPointForFrame(position);
		}

		private static void InternalSetFocusPointForFrame(Vector3 position)
		{
			HolographicSettings.INTERNAL_CALL_InternalSetFocusPointForFrame(ref position);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_InternalSetFocusPointForFrame(ref Vector3 position);

		public static void SetFocusPointForFrame(Vector3 position, Vector3 normal)
		{
			HolographicSettings.InternalSetFocusPointForFrameWithNormal(position, normal);
		}

		private static void InternalSetFocusPointForFrameWithNormal(Vector3 position, Vector3 normal)
		{
			HolographicSettings.INTERNAL_CALL_InternalSetFocusPointForFrameWithNormal(ref position, ref normal);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_InternalSetFocusPointForFrameWithNormal(ref Vector3 position, ref Vector3 normal);

		public static void SetFocusPointForFrame(Vector3 position, Vector3 normal, Vector3 velocity)
		{
			HolographicSettings.InternalSetFocusPointForFrameWithNormalVelocity(position, normal, velocity);
		}

		private static void InternalSetFocusPointForFrameWithNormalVelocity(Vector3 position, Vector3 normal, Vector3 velocity)
		{
			HolographicSettings.INTERNAL_CALL_InternalSetFocusPointForFrameWithNormalVelocity(ref position, ref normal, ref velocity);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_InternalSetFocusPointForFrameWithNormalVelocity(ref Vector3 position, ref Vector3 normal, ref Vector3 velocity);
	}
}
