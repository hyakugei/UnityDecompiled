using System;
using System.Runtime.CompilerServices;

namespace UnityEngine.CrashReportHandler
{
	public class CrashReportHandler
	{
		public static extern bool enableCaptureExceptions
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		private CrashReportHandler()
		{
		}
	}
}
