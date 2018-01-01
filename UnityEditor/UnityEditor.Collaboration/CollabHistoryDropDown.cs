using System;
using System.Collections.Generic;
using UnityEditor.Connect;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace UnityEditor.Collaboration
{
	internal class CollabHistoryDropDown : VisualElement
	{
		private readonly VisualElement m_FilesContainer;

		private readonly Label m_ToggleLabel;

		private int m_ChangesTotal;

		public CollabHistoryDropDown(ICollection<ChangeData> changes, int changesTotal, bool changesTruncated)
		{
			this.m_FilesContainer = new VisualElement();
			this.m_ChangesTotal = changesTotal;
			this.m_ToggleLabel = new Label(this.ToggleText(false));
			this.m_ToggleLabel.AddManipulator(new Clickable(new Action(this.ToggleDropdown)));
			base.Add(this.m_ToggleLabel);
			foreach (ChangeData current in changes)
			{
				this.m_FilesContainer.Add(new CollabHistoryDropDownItem(current.path, current.action));
			}
			if (changesTruncated)
			{
				this.m_FilesContainer.Add(new Button(new Action(this.ShowAllClick))
				{
					text = "Show all on dashboard"
				});
			}
		}

		private void ToggleDropdown()
		{
			if (base.Contains(this.m_FilesContainer))
			{
				base.Remove(this.m_FilesContainer);
				this.m_ToggleLabel.text = this.ToggleText(false);
			}
			else
			{
				base.Add(this.m_FilesContainer);
				this.m_ToggleLabel.text = this.ToggleText(true);
			}
		}

		private string ToggleText(bool open)
		{
			string arg = (!open) ? "▶" : "▼";
			string arg2 = (this.m_ChangesTotal != 1) ? "Changes" : "Change";
			return string.Format("{0} {1} Asset {2}", arg, this.m_ChangesTotal, arg2);
		}

		private void ShowAllClick()
		{
			string configurationURL = UnityConnect.instance.GetConfigurationURL(CloudConfigUrl.CloudCollab);
			string organizationForeignKey = UnityConnect.instance.GetOrganizationForeignKey();
			string projectGUID = UnityConnect.instance.GetProjectGUID();
			string url = string.Format("{0}/orgs/{1}/projects/{2}/assets", configurationURL, organizationForeignKey, projectGUID);
			Application.OpenURL(url);
		}
	}
}
