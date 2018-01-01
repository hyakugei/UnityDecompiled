using System;
using UnityEngine;

namespace UnityEditor.Modules
{
	internal interface IBuildWindowExtension
	{
		void ShowPlatformBuildOptions();

		void ShowPlatformBuildWarnings();

		void ShowInternalPlatformBuildOptions();

		bool EnabledBuildButton();

		bool EnabledBuildAndRunButton();

		void GetBuildButtonTitles(out GUIContent buildButtonTitle, out GUIContent buildAndRunButtonTitle);

		bool ShouldDrawScriptDebuggingCheckbox();

		bool ShouldDrawProfilerCheckbox();

		bool ShouldDrawDevelopmentPlayerCheckbox();

		bool ShouldDrawExplicitNullCheckbox();

		bool ShouldDrawExplicitDivideByZeroCheckbox();

		bool ShouldDrawExplicitArrayBoundsCheckbox();

		bool ShouldDrawForceOptimizeScriptsCheckbox();
	}
}
