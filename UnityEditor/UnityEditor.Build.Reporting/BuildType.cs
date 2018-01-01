using System;

namespace UnityEditor.Build.Reporting
{
	[Flags]
	internal enum BuildType
	{
		Player = 1,
		AssetBundle = 2
	}
}
