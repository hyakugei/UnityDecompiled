using System;
using UnityEditor.Build.Reporting;

namespace UnityEditor.Modules
{
	internal interface IBuildAnalyzer
	{
		void OnAddedExecutable(BuildReport report, int fileIndex);
	}
}
