using System;

namespace UnityEditor.DeploymentTargets
{
	internal class DefaultDeploymentTargetInfo : IDeploymentTargetInfo
	{
		public virtual FlagSet<DeploymentTargetSupportFlags> GetSupportFlags()
		{
			return DeploymentTargetSupportFlags.None;
		}

		public virtual TargetCheckResult CheckTarget(DeploymentTargetRequirements targetRequirements)
		{
			return default(TargetCheckResult);
		}

		public string GetDisplayName()
		{
			return "";
		}
	}
}
