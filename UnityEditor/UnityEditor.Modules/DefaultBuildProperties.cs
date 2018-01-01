using System;

namespace UnityEditor.Modules
{
	internal class DefaultBuildProperties : BuildProperties
	{
		public override DeploymentTargetRequirements GetTargetRequirements()
		{
			return null;
		}
	}
}
