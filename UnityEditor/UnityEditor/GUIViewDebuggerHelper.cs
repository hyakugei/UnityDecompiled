using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEditor
{
	[UsedByNativeCode]
	internal static class GUIViewDebuggerHelper
	{
		internal static Action onViewInstructionsChanged;

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void GetViews(List<GUIView> views);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void DebugWindow([Unmarshalled] GUIView view);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void StopDebugging();

		private static GUIContent CreateGUIContent(string text, Texture image, string tooltip)
		{
			return new GUIContent(text, image, tooltip);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void GetDrawInstructions(List<IMGUIDrawInstruction> drawInstructions);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void GetClipInstructions(List<IMGUIClipInstruction> clipInstructions);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void GetNamedControlInstructions(List<IMGUINamedControlInstruction> namedControlInstructions);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void GetPropertyInstructions(List<IMGUIPropertyInstruction> namedControlInstructions);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void GetLayoutInstructions(List<IMGUILayoutInstruction> layoutInstructions);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void GetUnifiedInstructions(List<IMGUIInstruction> layoutInstructions);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void ClearInstructions();

		[RequiredByNativeCode]
		private static void CallOnViewInstructionsChanged()
		{
			if (GUIViewDebuggerHelper.onViewInstructionsChanged != null)
			{
				GUIViewDebuggerHelper.onViewInstructionsChanged();
			}
		}
	}
}
