using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine.Windows
{
	public static class CrashReporting
	{
		[ThreadAndSerializationSafe]
		public static extern string crashReportFolder
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}
	}
}
