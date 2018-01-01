using System;
using System.Collections.Generic;

namespace UnityEditor.DeploymentTargets
{
	internal abstract class DefaultDeploymentTargetsExtension : IDeploymentTargetsExtension
	{
		public virtual List<DeploymentTargetIdAndStatus> GetKnownTargets(ProgressHandler progressHandler = null)
		{
			return new List<DeploymentTargetIdAndStatus>();
		}

		public virtual IDeploymentTargetInfo GetTargetInfo(DeploymentTargetId targetId, ProgressHandler progressHandler = null)
		{
			return new DefaultDeploymentTargetInfo();
		}

		public virtual void LaunchBuildOnTarget(BuildProperties buildProperties, DeploymentTargetId targetId, ProgressHandler progressHandler = null)
		{
			throw new NotSupportedException();
		}
	}
}
