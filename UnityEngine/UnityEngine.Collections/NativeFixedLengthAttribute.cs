using System;
using UnityEngine.Scripting;

namespace UnityEngine.Collections
{
	[AttributeUsage(AttributeTargets.Field), RequiredByNativeCode]
	public class NativeFixedLengthAttribute : Attribute
	{
		public int FixedLength;

		public NativeFixedLengthAttribute(int fixedLength)
		{
			this.FixedLength = fixedLength;
		}
	}
}
