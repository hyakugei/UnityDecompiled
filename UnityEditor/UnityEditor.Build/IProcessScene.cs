using System;
using UnityEditor.Build.Reporting;
using UnityEngine.SceneManagement;

namespace UnityEditor.Build
{
	public interface IProcessScene : IOrderedCallback
	{
		void OnProcessScene(Scene scene, BuildReport report);
	}
}
