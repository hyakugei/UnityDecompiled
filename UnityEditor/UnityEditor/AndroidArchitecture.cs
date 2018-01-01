using System;

namespace UnityEditor
{
	[Flags]
	public enum AndroidArchitecture : uint
	{
		None = 0u,
		ARMv7 = 1u,
		X86 = 4u,
		All = 4294967295u
	}
}
