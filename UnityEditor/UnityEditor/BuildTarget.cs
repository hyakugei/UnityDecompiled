using System;
using UnityEngine.Bindings;

namespace UnityEditor
{
	[NativeType("Runtime/Serialize/SerializationMetaFlags.h")]
	public enum BuildTarget
	{
		StandaloneOSX = 2,
		[Obsolete("Use StandaloneOSX instead (UnityUpgradable) -> StandaloneOSX", true)]
		StandaloneOSXUniversal = 2,
		[Obsolete("StandaloneOSXIntel has been removed in 2017.3")]
		StandaloneOSXIntel = 4,
		StandaloneWindows,
		[Obsolete("WebPlayer has been removed in 5.4", true)]
		WebPlayer,
		[Obsolete("WebPlayerStreamed has been removed in 5.4", true)]
		WebPlayerStreamed,
		iOS = 9,
		[Obsolete("PS3 has been removed in >=5.5")]
		PS3,
		[Obsolete("XBOX360 has been removed in 5.5")]
		XBOX360,
		Android = 13,
		StandaloneLinux = 17,
		StandaloneWindows64 = 19,
		WebGL,
		WSAPlayer,
		StandaloneLinux64 = 24,
		StandaloneLinuxUniversal,
		[Obsolete("Use WSAPlayer with Windows Phone 8.1 selected")]
		WP8Player,
		[Obsolete("StandaloneOSXIntel64 has been removed in 2017.3")]
		StandaloneOSXIntel64,
		[Obsolete("BlackBerry has been removed in 5.4")]
		BlackBerry,
		Tizen,
		PSP2,
		PS4,
		[Obsolete("warning PSM has been removed in >= 5.3")]
		PSM,
		XboxOne,
		[Obsolete("SamsungTV has been removed in 2017.3")]
		SamsungTV,
		N3DS,
		[Obsolete("Wii U support was removed in 2018.1")]
		WiiU,
		tvOS,
		Switch,
		[Obsolete("Use iOS instead (UnityUpgradable) -> iOS", true)]
		iPhone = -1,
		[Obsolete("BlackBerry has been removed in 5.4")]
		BB10 = -1,
		[Obsolete("Use WSAPlayer instead (UnityUpgradable) -> WSAPlayer", true)]
		MetroPlayer = -1,
		NoTarget = -2
	}
}
