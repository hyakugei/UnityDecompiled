using System;
using UnityEditor;

namespace UnityEditorInternal
{
	[CustomEditor(typeof(ProjectSettingsBase), true)]
	internal class ProjectSettingsBaseEditor : Editor
	{
		private string m_LocalizedTargetName;

		internal override string targetTitle
		{
			get
			{
				if (this.m_LocalizedTargetName == null)
				{
					this.m_LocalizedTargetName = L10n.Tr(base.target.name);
				}
				return this.m_LocalizedTargetName;
			}
		}

		protected override bool ShouldHideOpenButton()
		{
			return true;
		}
	}
}
