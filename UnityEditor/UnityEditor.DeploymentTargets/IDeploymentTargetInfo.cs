using System;

namespace UnityEditor.DeploymentTargets
{
	internal interface IDeploymentTargetInfo
	{
		FlagSet<DeploymentTargetSupportFlags> GetSupportFlags();

		TargetCheckResult CheckTarget(DeploymentTargetRequirements targetRequirements);

		string GetDisplayName();
	}
}
