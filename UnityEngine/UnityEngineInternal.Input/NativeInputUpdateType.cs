using System;

namespace UnityEngineInternal.Input
{
	[Flags]
	public enum NativeInputUpdateType
	{
		Dynamic = 1,
		Fixed = 2,
		BeforeRender = 4,
		Editor = 8
	}
}
