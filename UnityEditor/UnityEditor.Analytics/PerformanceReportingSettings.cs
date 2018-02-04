using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityEditor.Analytics
{
	public static class PerformanceReportingSettings
	{
		[ThreadAndSerializationSafe]
		public static extern bool enabled
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}
	}
}
