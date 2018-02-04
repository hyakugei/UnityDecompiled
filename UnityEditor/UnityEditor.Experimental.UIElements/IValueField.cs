using System;
using UnityEngine;

namespace UnityEditor.Experimental.UIElements
{
	public interface IValueField<T>
	{
		T value
		{
			get;
			set;
		}

		void ApplyInputDeviceDelta(Vector3 delta, DeltaSpeed speed, T startValue);
	}
}
