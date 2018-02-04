using System;
using System.Collections.Generic;
using UnityEditor.Build.Reporting;
using UnityEditor.Modules;

namespace UnityEditor.DeploymentTargets
{
	internal class DeploymentTargetManager
	{
		private const string kExtensionErrorMessage = "Platform does not implement DeploymentTargetsExtension";

		private static IDeploymentTargetsExtension GetExtension(BuildTargetGroup targetGroup, BuildTarget buildTarget)
		{
			IDeploymentTargetsExtension deploymentTargetsExtension = ModuleManager.GetDeploymentTargetsExtension(targetGroup, buildTarget);
			if (deploymentTargetsExtension == null)
			{
				throw new NotSupportedException("Platform does not implement DeploymentTargetsExtension");
			}
			return deploymentTargetsExtension;
		}

		public static bool IsExtensionSupported(BuildTargetGroup targetGroup, BuildTarget buildTarget)
		{
			return ModuleManager.GetDeploymentTargetsExtension(targetGroup, buildTarget) != null;
		}

		public static IDeploymentTargetInfo GetTargetInfo(BuildTargetGroup targetGroup, BuildTarget buildTarget, DeploymentTargetId targetId)
		{
			IDeploymentTargetsExtension extension = DeploymentTargetManager.GetExtension(targetGroup, buildTarget);
			return extension.GetTargetInfo(targetId, null);
		}

		public static bool SupportsLaunchBuild(IDeploymentTargetInfo info, BuildProperties buildProperties)
		{
			return info.GetSupportFlags().HasFlags(DeploymentTargetSupportFlags.Launch) && info.CheckTarget(buildProperties.GetTargetRequirements()).Passed();
		}

		public static bool SupportsLaunchBuild(IDeploymentTargetInfo info, BuildReport buildReport)
		{
			return DeploymentTargetManager.SupportsLaunchBuild(info, BuildProperties.GetFromBuildReport(buildReport));
		}

		public static void LaunchBuildOnTarget(BuildTargetGroup targetGroup, BuildTarget buildTarget, BuildProperties buildProperties, DeploymentTargetId targetId, ProgressHandler progressHandler = null)
		{
			IDeploymentTargetsExtension extension = DeploymentTargetManager.GetExtension(targetGroup, buildTarget);
			extension.LaunchBuildOnTarget(buildProperties, targetId, progressHandler);
		}

		public static void LaunchBuildOnTarget(BuildTargetGroup targetGroup, BuildReport buildReport, DeploymentTargetId targetId, ProgressHandler progressHandler = null)
		{
			DeploymentTargetManager.LaunchBuildOnTarget(targetGroup, buildReport.summary.platform, BuildProperties.GetFromBuildReport(buildReport), targetId, progressHandler);
		}

		public static List<DeploymentTargetIdAndStatus> GetKnownTargets(BuildTargetGroup targetGroup, BuildTarget buildTarget)
		{
			IDeploymentTargetsExtension extension = DeploymentTargetManager.GetExtension(targetGroup, buildTarget);
			return extension.GetKnownTargets(null);
		}

		public static List<DeploymentTargetId> FindValidTargetsForLaunchBuild(BuildTargetGroup targetGroup, BuildTarget buildTarget, BuildProperties buildProperties)
		{
			IDeploymentTargetsExtension extension = DeploymentTargetManager.GetExtension(targetGroup, buildTarget);
			List<DeploymentTargetId> list = new List<DeploymentTargetId>();
			List<DeploymentTargetIdAndStatus> knownTargets = extension.GetKnownTargets(null);
			foreach (DeploymentTargetIdAndStatus current in knownTargets)
			{
				if (current.status == DeploymentTargetStatus.Ready)
				{
					if (DeploymentTargetManager.SupportsLaunchBuild(extension.GetTargetInfo(current.id, null), buildProperties))
					{
						list.Add(current.id);
					}
				}
			}
			return list;
		}

		public static List<DeploymentTargetId> FindValidTargetsForLaunchBuild(BuildTargetGroup targetGroup, BuildReport buildReport)
		{
			return DeploymentTargetManager.FindValidTargetsForLaunchBuild(targetGroup, buildReport.summary.platform, BuildProperties.GetFromBuildReport(buildReport));
		}
	}
}
