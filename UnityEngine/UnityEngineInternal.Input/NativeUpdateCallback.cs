using System;

namespace UnityEngineInternal.Input
{
	public delegate void NativeUpdateCallback(NativeInputUpdateType updateType, int eventCount, IntPtr eventData);
}
