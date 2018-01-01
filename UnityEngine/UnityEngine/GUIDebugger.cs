using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
	internal class GUIDebugger
	{
		public static void LogLayoutEntry(Rect rect, RectOffset margins, GUIStyle style)
		{
			GUIDebugger.INTERNAL_CALL_LogLayoutEntry(ref rect, margins, style);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_LogLayoutEntry(ref Rect rect, RectOffset margins, GUIStyle style);

		public static void LogLayoutGroupEntry(Rect rect, RectOffset margins, GUIStyle style, bool isVertical)
		{
			GUIDebugger.INTERNAL_CALL_LogLayoutGroupEntry(ref rect, margins, style, isVertical);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_LogLayoutGroupEntry(ref Rect rect, RectOffset margins, GUIStyle style, bool isVertical);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void LogLayoutEndGroup();

		public static void LogBeginProperty(string targetTypeAssemblyQualifiedName, string path, Rect position)
		{
			GUIDebugger.INTERNAL_CALL_LogBeginProperty(targetTypeAssemblyQualifiedName, path, ref position);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_LogBeginProperty(string targetTypeAssemblyQualifiedName, string path, ref Rect position);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void LogEndProperty();
	}
}
