using System;
using System.Runtime.CompilerServices;

namespace UnityEditor.Analytics
{
	public static class AnalyticsSettings
	{
		public static extern bool enabled
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern bool testMode
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		internal static extern bool enabledForPlatform
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void SetEnabledServiceWindow(bool enabled);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void ApplyEnableSettings(BuildTarget target);
	}
}
