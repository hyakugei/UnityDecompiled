using System;
using UnityEngine.Scripting;

namespace UnityEngine.Collections
{
	[AttributeUsage(AttributeTargets.Field), RequiredByNativeCode]
	public class WriteOnlyAttribute : Attribute
	{
	}
}
