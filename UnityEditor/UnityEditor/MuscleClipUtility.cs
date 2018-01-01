using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

namespace UnityEditor
{
	internal class MuscleClipUtility
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern MuscleClipQualityInfo GetMuscleClipQualityInfo(AnimationClip clip, float startTime, float stopTime);

		internal static void CalculateQualityCurves(AnimationClip clip, QualityCurvesTime time, Vector2[] poseCurve, Vector2[] rotationCurve, Vector2[] heightCurve, Vector2[] positionCurve)
		{
			if (poseCurve.Length != rotationCurve.Length || rotationCurve.Length != heightCurve.Length || heightCurve.Length != positionCurve.Length)
			{
				throw new ArgumentException(string.Format("poseCurve '{0}', rotationCurve '{1}', heightCurve '{2}' and positionCurve '{3}' Length must be equals.", new object[]
				{
					poseCurve.Length,
					rotationCurve.Length,
					heightCurve.Length,
					positionCurve.Length
				}));
			}
			MuscleClipUtility.CalculateQualityCurves(clip, time.fixedTime, time.variableEndStart, time.variableEndEnd, time.q, poseCurve, rotationCurve, heightCurve, positionCurve);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		protected static extern void CalculateQualityCurves(AnimationClip clip, float fixedTime, float variableEndStart, float variableEndEnd, int q, [Out] Vector2[] poseCurve, [Out] Vector2[] rotationCurve, [Out] Vector2[] heightCurve, [Out] Vector2[] positionCurve);
	}
}
