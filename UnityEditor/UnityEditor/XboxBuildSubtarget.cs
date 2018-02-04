using System;
using UnityEngine.Bindings;

namespace UnityEditor
{
	[NativeType(Header = "Runtime/Serialize/BuildTarget.h")]
	public enum XboxBuildSubtarget
	{
		Development,
		Master,
		Debug
	}
}
