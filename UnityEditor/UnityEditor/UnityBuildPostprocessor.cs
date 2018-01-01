using System;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityEditor
{
	internal class UnityBuildPostprocessor : IProcessScene, IOrderedCallback
	{
		public int callbackOrder
		{
			get
			{
				return 0;
			}
		}

		public void OnProcessScene(Scene scene, BuildReport report)
		{
			int num;
			int num2;
			PlayerSettings.GetBatchingForPlatform(EditorUserBuildSettings.activeBuildTarget, out num, out num2);
			if (num != 0)
			{
				InternalStaticBatchingUtility.Combine(null, true, true);
			}
		}
	}
}
