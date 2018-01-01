using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public static class FrameTimingManager
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void CaptureFrameTimings();

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern uint GetLatestTimings(uint numFrames, FrameTiming[] timings);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern float GetVSyncsPerSecond();

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern ulong GetGpuTimerFrequency();

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern ulong GetCpuTimerFrequency();
	}
}
