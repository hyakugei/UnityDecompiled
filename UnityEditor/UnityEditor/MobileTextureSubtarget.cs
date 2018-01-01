using System;
using UnityEngine.Bindings;

namespace UnityEditor
{
	[NativeType(Header = "Runtime/Serialize/BuildTarget.h")]
	public enum MobileTextureSubtarget
	{
		Generic,
		DXT,
		PVRTC,
		[Obsolete("UnityEditor.MobileTextureSubtarget.ATC has been deprecated. Use UnityEditor.MobileTextureSubtarget.ETC instead (UnityUpgradable) -> UnityEditor.MobileTextureSubtarget.ETC", true)]
		ATC,
		ETC,
		ETC2,
		ASTC
	}
}
