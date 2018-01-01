using System;
using UnityEditor.Build.Reporting;

namespace UnityEditor.Build
{
	public interface IPostprocessBuild : IOrderedCallback
	{
		void OnPostprocessBuild(BuildReport report);
	}
}
