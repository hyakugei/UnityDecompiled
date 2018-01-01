using System;
using UnityEngine.Bindings;

namespace UnityEditor
{
	[NativeType(Header = "Editor/Src/EditorUserBuildSettings.h")]
	public enum PS4HardwareTarget
	{
		BaseOnly,
		[Obsolete("Enum member PS4HardwareTarget.NeoAndBase has been deprecated. Use PS4HardwareTarget.ProAndBase instead (UnityUpgradable) -> ProAndBase", true)]
		NeoAndBase,
		ProAndBase = 1
	}
}
