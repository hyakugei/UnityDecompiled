using System;
using UnityEngine.Bindings;

namespace UnityEditor
{
	[NativeType(Header = "Runtime/Serialize/BuildTarget.h")]
	public enum AndroidETC2Fallback
	{
		Quality32Bit,
		Quality16Bit,
		Quality32BitDownscaled
	}
}
