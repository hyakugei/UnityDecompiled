using System;

namespace UnityEditor
{
	public enum GameViewSizeGroupType
	{
		Standalone,
		[Obsolete("WebPlayer has been removed in 5.4")]
		WebPlayer,
		iOS,
		Android,
		[Obsolete("PS3 has been removed in 5.5", false)]
		PS3,
		[Obsolete("Wii U support was removed in 2018.1", false)]
		WiiU,
		Tizen,
		[Obsolete("Windows Phone 8 was removed in 5.3", false)]
		WP8,
		N3DS,
		HMD
	}
}
