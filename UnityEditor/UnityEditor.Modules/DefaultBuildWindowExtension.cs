using System;
using UnityEngine;

namespace UnityEditor.Modules
{
	internal class DefaultBuildWindowExtension : IBuildWindowExtension
	{
		public virtual void ShowPlatformBuildOptions()
		{
		}

		public virtual void ShowPlatformBuildWarnings()
		{
		}

		public virtual void ShowInternalPlatformBuildOptions()
		{
		}

		public virtual bool EnabledBuildButton()
		{
			return true;
		}

		public virtual bool EnabledBuildAndRunButton()
		{
			return true;
		}

		public virtual void GetBuildButtonTitles(out GUIContent buildButtonTitle, out GUIContent buildAndRunButtonTitle)
		{
			buildButtonTitle = null;
			buildAndRunButtonTitle = null;
		}

		public virtual bool ShouldDrawScriptDebuggingCheckbox()
		{
			return true;
		}

		public virtual bool ShouldDrawProfilerCheckbox()
		{
			return true;
		}

		public virtual bool ShouldDrawDevelopmentPlayerCheckbox()
		{
			return true;
		}

		public virtual bool ShouldDrawExplicitNullCheckbox()
		{
			return false;
		}

		public virtual bool ShouldDrawExplicitDivideByZeroCheckbox()
		{
			return false;
		}

		public virtual bool ShouldDrawExplicitArrayBoundsCheckbox()
		{
			return false;
		}

		public virtual bool ShouldDrawForceOptimizeScriptsCheckbox()
		{
			return false;
		}
	}
}
