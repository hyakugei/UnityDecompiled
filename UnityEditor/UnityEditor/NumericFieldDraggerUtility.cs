using System;
using UnityEngine;

namespace UnityEditor
{
	internal class NumericFieldDraggerUtility
	{
		private static bool s_UseYSign = false;

		private const float kDragSensitivity = 0.03f;

		internal static float Acceleration(bool shiftPressed, bool altPressed)
		{
			return (float)((!shiftPressed) ? 1 : 4) * ((!altPressed) ? 1f : 0.25f);
		}

		internal static float NiceDelta(Vector2 deviceDelta, float acceleration)
		{
			deviceDelta.y = -deviceDelta.y;
			if (Mathf.Abs(Mathf.Abs(deviceDelta.x) - Mathf.Abs(deviceDelta.y)) / Mathf.Max(Mathf.Abs(deviceDelta.x), Mathf.Abs(deviceDelta.y)) > 0.1f)
			{
				if (Mathf.Abs(deviceDelta.x) > Mathf.Abs(deviceDelta.y))
				{
					NumericFieldDraggerUtility.s_UseYSign = false;
				}
				else
				{
					NumericFieldDraggerUtility.s_UseYSign = true;
				}
			}
			float result;
			if (NumericFieldDraggerUtility.s_UseYSign)
			{
				result = Mathf.Sign(deviceDelta.y) * deviceDelta.magnitude * acceleration;
			}
			else
			{
				result = Mathf.Sign(deviceDelta.x) * deviceDelta.magnitude * acceleration;
			}
			return result;
		}

		internal static double CalculateFloatDragSensitivity(double value)
		{
			double result;
			if (double.IsInfinity(value) || double.IsNaN(value))
			{
				result = 0.0;
			}
			else
			{
				result = Math.Max(1.0, Math.Pow(Math.Abs(value), 0.5)) * 0.029999999329447746;
			}
			return result;
		}

		internal static long CalculateIntDragSensitivity(long value)
		{
			return (long)Math.Max(1.0, Math.Pow(Math.Abs((double)value), 0.5) * 0.029999999329447746);
		}
	}
}
