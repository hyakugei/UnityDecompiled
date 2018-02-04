using System;
using UnityEngine.Bindings;

namespace UnityEditor
{
	[NativeType(Header = "Editor/Src/EditorUserBuildSettings.h")]
	public enum PSP2BuildSubtarget
	{
		PCHosted,
		Package
	}
}
