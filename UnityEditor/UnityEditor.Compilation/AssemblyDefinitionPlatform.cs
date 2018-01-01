using System;

namespace UnityEditor.Compilation
{
	public struct AssemblyDefinitionPlatform
	{
		public string Name
		{
			get;
			private set;
		}

		public string DisplayName
		{
			get;
			private set;
		}

		public BuildTarget BuildTarget
		{
			get;
			private set;
		}

		internal AssemblyDefinitionPlatform(string name, string displayName, BuildTarget buildTarget)
		{
			this = default(AssemblyDefinitionPlatform);
			this.Name = name;
			this.DisplayName = displayName;
			this.BuildTarget = buildTarget;
		}
	}
}
