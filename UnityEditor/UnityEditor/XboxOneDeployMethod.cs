using System;
using UnityEngine.Bindings;

namespace UnityEditor
{
	[NativeType(Header = "Runtime/Serialize/BuildTarget.h")]
	public enum XboxOneDeployMethod
	{
		Push,
		Pull,
		RunFromPC,
		Package,
		PackageStreaming
	}
}
