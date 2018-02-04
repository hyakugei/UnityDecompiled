using System;
using UnityEngine;

namespace UnityEditor.Experimental.UIElements
{
	public class IntegerField : TextValueField<long>
	{
		internal static string allowedCharacters
		{
			get
			{
				return EditorGUI.s_AllowedCharactersForInt;
			}
		}

		public IntegerField() : this(-1)
		{
		}

		public IntegerField(int maxLength) : base(maxLength)
		{
		}

		internal override bool AcceptCharacter(char c)
		{
			return c != '\0' && IntegerField.allowedCharacters.IndexOf(c) != -1;
		}

		public override void ApplyInputDeviceDelta(Vector3 delta, DeltaSpeed speed, long startValue)
		{
			double num = (double)NumericFieldDraggerUtility.CalculateIntDragSensitivity(startValue);
			float acceleration = NumericFieldDraggerUtility.Acceleration(speed == DeltaSpeed.Fast, speed == DeltaSpeed.Slow);
			long num2 = base.value;
			num2 += (long)Math.Round((double)NumericFieldDraggerUtility.NiceDelta(delta, acceleration) * num);
			base.SetValueAndNotify(num2);
		}

		protected override string ValueToString(long v)
		{
			return v.ToString(base.formatString);
		}

		protected override long StringToValue(string str)
		{
			long result;
			EditorGUI.StringToLong(this.text, out result);
			return result;
		}
	}
}
