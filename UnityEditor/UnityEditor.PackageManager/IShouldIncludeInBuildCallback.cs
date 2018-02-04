using System;

namespace UnityEditor.PackageManager
{
	public interface IShouldIncludeInBuildCallback
	{
		string PackageName
		{
			get;
		}

		bool ShouldIncludeInBuild(string path);
	}
}
