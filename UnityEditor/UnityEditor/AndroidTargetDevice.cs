using System;

namespace UnityEditor
{
	[Obsolete("Use AndroidArchitecture instead. (UnityUpgradable) -> AndroidArchitecture", false)]
	public enum AndroidTargetDevice
	{
		[Obsolete("Use AndroidArchitecture.All instead. (UnityUpgradable) -> AndroidArchitecture.All", false)]
		FAT,
		[Obsolete("Use AndroidArchitecture.ARMv7 instead. (UnityUpgradable) -> AndroidArchitecture.ARMv7", false)]
		ARMv7 = 3,
		[Obsolete("Use AndroidArchitecture.X86 instead. (UnityUpgradable) -> AndroidArchitecture.X86", false)]
		x86
	}
}
