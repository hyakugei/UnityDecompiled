using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	internal class ScriptingRuntime
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern string[] GetAllUserAssemblies();
	}
}
