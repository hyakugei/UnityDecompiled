using System;

namespace UnityEditor.Scripting.ScriptCompilation
{
	internal struct PrecompiledAssembly
	{
		public string Path;

		public AssemblyFlags Flags;

		public OptionalUnityReferences OptionalUnityReferences;
	}
}
