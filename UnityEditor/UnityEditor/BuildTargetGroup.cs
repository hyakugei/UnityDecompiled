using System;
using UnityEngine.Bindings;

namespace UnityEditor
{
	[NativeType(Header = "Editor/Src/BuildPipeline/BuildTargetPlatformSpecific.h")]
	public enum BuildTargetGroup
	{
		Unknown,
		Standalone,
		[Obsolete("WebPlayer was removed in 5.4, consider using WebGL", true)]
		WebPlayer,
		[Obsolete("Use iOS instead (UnityUpgradable) -> iOS", true)]
		iPhone = 4,
		iOS = 4,
		[Obsolete("PS3 has been removed in >=5.5")]
		PS3,
		[Obsolete("XBOX360 has been removed in 5.5")]
		XBOX360,
		Android,
		WebGL = 13,
		WSA,
		[Obsolete("Use WSA instead")]
		Metro = 14,
		[Obsolete("Use WSA instead")]
		WP8,
		[Obsolete("BlackBerry has been removed as of 5.4")]
		BlackBerry,
		Tizen,
		PSP2,
		PS4,
		[Obsolete("warning PSM has been removed in >= 5.3")]
		PSM,
		XboxOne,
		[Obsolete("SamsungTV has been removed as of 2017.3")]
		SamsungTV,
		N3DS,
		[Obsolete("Wii U support was removed in 2018.1")]
		WiiU,
		tvOS,
		Facebook,
		Switch
	}
}
