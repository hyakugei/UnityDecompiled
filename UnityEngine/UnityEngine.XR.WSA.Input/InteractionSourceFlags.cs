using System;

namespace UnityEngine.XR.WSA.Input
{
	internal enum InteractionSourceFlags
	{
		None,
		SupportsGrasp,
		SupportsMenu,
		SupportsPointing = 4,
		SupportsTouchpad = 8,
		SupportsThumbstick = 16
	}
}
