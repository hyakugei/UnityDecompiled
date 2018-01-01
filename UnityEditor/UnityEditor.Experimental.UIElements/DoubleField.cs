using System;
using UnityEngine;

namespace UnityEditor.Experimental.UIElements
{
	public class DoubleField : TextValueField<double>
	{
		internal static string allowedCharacters
		{
			get
			{
				return EditorGUI.s_AllowedCharactersForFloat;
			}
		}

		public DoubleField() : this(-1)
		{
		}

		public DoubleField(int maxLength) : base(maxLength)
		{
		}

		internal override bool AcceptCharacter(char c)
		{
			return c != '\0' && DoubleField.allowedCharacters.IndexOf(c) != -1;
		}

		public override void ApplyInputDeviceDelta(Vector3 delta, DeltaSpeed speed, double startValue)
		{
			double num = NumericFieldDraggerUtility.CalculateFloatDragSensitivity(startValue);
			float acceleration = NumericFieldDraggerUtility.Acceleration(speed == DeltaSpeed.Fast, speed == DeltaSpeed.Slow);
			double num2 = base.value;
			num2 += (double)NumericFieldDraggerUtility.NiceDelta(delta, acceleration) * num;
			num2 = MathUtils.RoundBasedOnMinimumDifference(num2, num);
			base.SetValueAndNotify(num2);
		}

		protected override string ValueToString(double v)
		{
			return v.ToString(base.formatString);
		}

		protected override double StringToValue(string str)
		{
			double result;
			EditorGUI.StringToDouble(str, out result);
			return result;
		}
	}
}
