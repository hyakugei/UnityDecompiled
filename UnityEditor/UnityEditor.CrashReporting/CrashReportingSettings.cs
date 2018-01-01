using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityEditor.CrashReporting
{
	public static class CrashReportingSettings
	{
		[ThreadAndSerializationSafe]
		public static extern bool enabled
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern bool captureEditorExceptions
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void SetEnabledServiceWindow(bool enabled);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern string GetEventUrl();

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void SetEventUrl(string eventUrl);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern string GetNativeEventUrl();

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void SetNativeEventUrl(string eventUrl);
	}
}
