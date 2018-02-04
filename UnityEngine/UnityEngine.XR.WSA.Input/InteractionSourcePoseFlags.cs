using System;

namespace UnityEngine.XR.WSA.Input
{
	internal enum InteractionSourcePoseFlags
	{
		None,
		HasGripPosition,
		HasGripRotation,
		HasPointerPosition = 4,
		HasPointerRotation = 8,
		HasVelocity = 16,
		HasAngularVelocity = 32
	}
}
