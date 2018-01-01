using System;
using System.Collections.Generic;

namespace UnityEditor
{
	internal interface IPlatformIconProvider
	{
		Dictionary<PlatformIconKind, PlatformIcon[]> GetRequiredPlatformIcons();

		PlatformIconKind GetPlatformIconKindFromEnumValue(IconKind kind);
	}
}
