using System;

namespace UnityEngine.XR.WSA.Input
{
	internal enum InteractionSourceStateFlags
	{
		None,
		Grasped,
		AnyPressed,
		TouchpadPressed = 4,
		ThumbstickPressed = 8,
		SelectPressed = 16,
		MenuPressed = 32,
		TouchpadTouched = 64
	}
}
