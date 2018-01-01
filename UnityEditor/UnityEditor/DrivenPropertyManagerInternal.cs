using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityEditor
{
	internal class DrivenPropertyManagerInternal
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool IsDriven(UnityEngine.Object target, string propertyPath);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool IsDriving(UnityEngine.Object driver, UnityEngine.Object target, string propertyPath);
	}
}
