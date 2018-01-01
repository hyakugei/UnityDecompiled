using System;
using UnityEditor.Build.Reporting;

namespace UnityEditor.Build
{
	public interface IPreprocessBuild : IOrderedCallback
	{
		void OnPreprocessBuild(BuildReport report);
	}
}
