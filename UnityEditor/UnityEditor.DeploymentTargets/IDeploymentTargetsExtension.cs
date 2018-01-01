using System;
using System.Collections.Generic;

namespace UnityEditor.DeploymentTargets
{
	internal interface IDeploymentTargetsExtension
	{
		List<DeploymentTargetIdAndStatus> GetKnownTargets(ProgressHandler progressHandler = null);

		IDeploymentTargetInfo GetTargetInfo(DeploymentTargetId targetId, ProgressHandler progressHandler = null);

		void LaunchBuildOnTarget(BuildProperties buildProperties, DeploymentTargetId targetId, ProgressHandler progressHandler = null);
	}
}
