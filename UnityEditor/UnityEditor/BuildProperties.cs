using System;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace UnityEditor
{
	internal abstract class BuildProperties : ScriptableObject
	{
		public static BuildProperties GetFromBuildReport(BuildReport report)
		{
			BuildProperties[] appendices = report.GetAppendices<BuildProperties>();
			if (appendices.Length > 0)
			{
				return appendices[0];
			}
			throw new MissingBuildPropertiesException();
		}

		public abstract DeploymentTargetRequirements GetTargetRequirements();
	}
}
