using System;
using System.Runtime.InteropServices;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.XR.WSA.Input
{
	[Obsolete("InteractionSourceLocation is deprecated, and will be removed in a future release. Use InteractionSourcePose instead. (UnityUpgradable) -> InteractionSourcePose", true), MovedFrom("UnityEngine.VR.WSA.Input")]
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct InteractionSourceLocation
	{
		public bool TryGetVelocity(out Vector3 velocity)
		{
			velocity = Vector3.zero;
			return false;
		}

		public bool TryGetPosition(out Vector3 position)
		{
			position = Vector3.zero;
			return false;
		}
	}
}
