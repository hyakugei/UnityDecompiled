using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	internal class GUIDebugger
	{
		public static extern bool active
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public static void LogLayoutEntry(Rect rect, RectOffset margins, GUIStyle style)
		{
			GUIDebugger.LogLayoutEntry_Injected(ref rect, margins, style);
		}

		public static void LogLayoutGroupEntry(Rect rect, RectOffset margins, GUIStyle style, bool isVertical)
		{
			GUIDebugger.LogLayoutGroupEntry_Injected(ref rect, margins, style, isVertical);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void LogLayoutEndGroup();

		public static void LogBeginProperty(string targetTypeAssemblyQualifiedName, string path, Rect position)
		{
			GUIDebugger.LogBeginProperty_Injected(targetTypeAssemblyQualifiedName, path, ref position);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void LogEndProperty();

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void LogLayoutEntry_Injected(ref Rect rect, RectOffset margins, GUIStyle style);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void LogLayoutGroupEntry_Injected(ref Rect rect, RectOffset margins, GUIStyle style, bool isVertical);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void LogBeginProperty_Injected(string targetTypeAssemblyQualifiedName, string path, ref Rect position);
	}
}
