using System;
using UnityEngine.Bindings;

namespace UnityEditor
{
	[NativeType(Header = "Editor/Src/EditorUserBuildSettings.h")]
	public enum PS4BuildSubtarget
	{
		PCHosted,
		Package,
		Iso
	}
}
