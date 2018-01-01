using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Bindings;

namespace UnityEditor
{
	[StructLayout(LayoutKind.Sequential)]
	internal sealed class LogEntry
	{
		public string condition;

		public int errorNum;

		public string file;

		public int line;

		public int mode;

		public int instanceID;

		public int identifier;

		[Ignore]
		public int isWorldPlaying;

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void LogToConsoleEx(LogEntry outputEntry);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void RemoveLogEntriesByMode(int mode);
	}
}
