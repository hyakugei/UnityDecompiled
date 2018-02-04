using System;
using UnityEditor;
using UnityEngine;

namespace UnityEditorInternal
{
	internal static class CurveBindingUtility
	{
		public static object GetCurrentValue(AnimationWindowState state, AnimationWindowCurve curve)
		{
			object result;
			if (state.previewing && curve.rootGameObject != null)
			{
				result = AnimationWindowUtility.GetCurrentValue(curve.rootGameObject, curve.binding);
			}
			else
			{
				result = curve.Evaluate(state.currentTime);
			}
			return result;
		}

		public static object GetCurrentValue(GameObject rootGameObject, EditorCurveBinding curveBinding)
		{
			object result;
			if (rootGameObject != null)
			{
				result = AnimationWindowUtility.GetCurrentValue(rootGameObject, curveBinding);
			}
			else if (curveBinding.isPPtrCurve)
			{
				result = null;
			}
			else
			{
				result = 0f;
			}
			return result;
		}
	}
}
