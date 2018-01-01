using System;
using UnityEditor.Build;

namespace UnityEditor
{
	internal static class EditorUserBuildSettingsUtils
	{
		public static BuildTarget CalculateSelectedBuildTarget()
		{
			BuildTargetGroup selectedBuildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
			BuildTarget result;
			if (selectedBuildTargetGroup != BuildTargetGroup.Standalone)
			{
				if (selectedBuildTargetGroup != BuildTargetGroup.Facebook)
				{
					if (BuildPlatforms.instance == null)
					{
						throw new Exception("Build platforms are not initialized.");
					}
					BuildPlatform buildPlatform = BuildPlatforms.instance.BuildPlatformFromTargetGroup(selectedBuildTargetGroup);
					if (buildPlatform == null)
					{
						throw new Exception("Could not find build platform for target group " + selectedBuildTargetGroup);
					}
					result = buildPlatform.defaultTarget;
				}
				else
				{
					result = EditorUserBuildSettings.selectedFacebookTarget;
				}
			}
			else
			{
				result = DesktopStandaloneBuildWindowExtension.GetBestStandaloneTarget(EditorUserBuildSettings.selectedStandaloneTarget);
			}
			return result;
		}
	}
}
