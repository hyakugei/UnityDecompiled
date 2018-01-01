using System;
using System.Runtime.CompilerServices;

namespace UnityEditor
{
	[AttributeUsage(AttributeTargets.Method)]
	public class ShaderIncludePathAttribute : Attribute
	{
		[RequiredSignature]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string[] GetIncludePaths();
	}
}
