using System;
using System.ComponentModel;
using UnityEngine.Bindings;

namespace UnityEditor
{
	[EditorBrowsable(EditorBrowsableState.Never), Obsolete("UnityEditor.AndroidBuildSubtarget has been deprecated. Use UnityEditor.MobileTextureSubtarget instead (UnityUpgradable)", true), NativeType(Header = "Editor/Src/EditorUserBuildSettings.h")]
	public enum AndroidBuildSubtarget
	{
		Generic = -1,
		DXT = -1,
		PVRTC = -1,
		ATC = -1,
		ETC = -1,
		ETC2 = -1,
		ASTC = -1
	}
}
