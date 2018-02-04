using System;
using UnityEngine.Experimental.UIElements;

namespace UnityEditor.Collaboration
{
	internal class CollabHistoryItem : VisualContainer
	{
		public static RevisionAction s_OnRestore;

		public static RevisionAction s_OnGoBack;

		public static RevisionAction s_OnUpdate;

		public static ShowBuildAction s_OnShowBuild;

		public static Action s_OnShowServices;

		private readonly string m_RevisionId;

		private readonly string m_FullDescription;

		private readonly DateTime m_TimeStamp;

		private readonly Button m_Button;

		private readonly HistoryProgressSpinner m_ProgressSpinner;

		private VisualContainer m_ActionsTray;

		private VisualElement m_Details;

		private Label m_Description;

		private Label m_TimeAgo;

		private readonly Button m_ExpandCollapseButton;

		private bool m_Expanded;

		private const int kMaxDescriptionChars = 500;

		public bool RevisionActionsEnabled
		{
			set
			{
				this.m_Button.SetEnabled(value);
			}
		}

		public DateTime timeStamp
		{
			get
			{
				return this.m_TimeStamp;
			}
		}

		public CollabHistoryItem(RevisionData data)
		{
			this.m_RevisionId = data.id;
			this.m_TimeStamp = data.timeStamp;
			base.name = "HistoryItem";
			this.m_ActionsTray = new VisualContainer
			{
				name = "HistoryItemActionsTray"
			};
			this.m_ProgressSpinner = new HistoryProgressSpinner();
			this.m_Details = new VisualElement
			{
				name = "HistoryDetail"
			};
			Label child = new Label(data.authorName)
			{
				name = "Author"
			};
			this.m_TimeAgo = new Label(TimeAgo.GetString(this.m_TimeStamp));
			this.m_FullDescription = data.comment;
			bool flag = this.ShouldTruncateDescription(this.m_FullDescription);
			if (flag)
			{
				this.m_Description = new Label(this.GetTruncatedDescription(this.m_FullDescription));
			}
			else
			{
				this.m_Description = new Label(this.m_FullDescription);
			}
			this.m_Description.name = "RevisionDescription";
			CollabHistoryDropDown child2 = new CollabHistoryDropDown(data.changes, data.changesTotal, data.changesTruncated);
			if (data.current)
			{
				this.m_Button = new Button(new Action(this.Restore))
				{
					name = "ActionButton",
					text = "Restore"
				};
			}
			else if (data.obtained)
			{
				this.m_Button = new Button(new Action(this.GoBackTo))
				{
					name = "ActionButton",
					text = "Go back to..."
				};
			}
			else
			{
				this.m_Button = new Button(new Action(this.UpdateTo))
				{
					name = "ActionButton",
					text = "Update"
				};
			}
			this.m_Button.SetEnabled(data.enabled);
			this.m_ProgressSpinner.ProgressEnabled = data.inProgress;
			this.m_ActionsTray.Add(this.m_ProgressSpinner);
			this.m_ActionsTray.Add(this.m_Button);
			this.m_Details.Add(child);
			this.m_Details.Add(this.m_TimeAgo);
			this.m_Details.Add(this.m_Description);
			if (flag)
			{
				this.m_ExpandCollapseButton = new Button(new Action(this.ToggleDescription))
				{
					name = "ToggleDescription",
					text = "Show More"
				};
				this.m_Details.Add(this.m_ExpandCollapseButton);
			}
			if (data.buildState != BuildState.None)
			{
				BuildStatusButton child3;
				if (data.buildState == BuildState.Configure)
				{
					child3 = new BuildStatusButton(new Action(this.ShowServicePage));
				}
				else
				{
					child3 = new BuildStatusButton(new Action(this.ShowBuildForCommit), data.buildState, data.buildFailures);
				}
				this.m_Details.Add(child3);
			}
			this.m_Details.Add(this.m_ActionsTray);
			this.m_Details.Add(child2);
			base.Add(this.m_Details);
			base.schedule.Execute(new Action(this.UpdateTimeAgo)).Every(20000L);
		}

		public static void SetUpCallbacks(RevisionAction Restore, RevisionAction GoBack, RevisionAction Update)
		{
			CollabHistoryItem.s_OnRestore = Restore;
			CollabHistoryItem.s_OnGoBack = GoBack;
			CollabHistoryItem.s_OnUpdate = Update;
		}

		public void SetInProgressStatus(string revisionIdInProgress)
		{
			if (string.IsNullOrEmpty(revisionIdInProgress))
			{
				this.m_Button.SetEnabled(true);
				this.m_ProgressSpinner.ProgressEnabled = false;
			}
			else
			{
				this.m_Button.SetEnabled(false);
				if (this.m_RevisionId.Equals(revisionIdInProgress))
				{
					this.m_ProgressSpinner.ProgressEnabled = true;
				}
			}
		}

		private void ShowBuildForCommit()
		{
			if (CollabHistoryItem.s_OnShowBuild != null)
			{
				CollabHistoryItem.s_OnShowBuild(this.m_RevisionId);
			}
		}

		private void ShowServicePage()
		{
			if (CollabHistoryItem.s_OnShowServices != null)
			{
				CollabHistoryItem.s_OnShowServices();
			}
		}

		private void Restore()
		{
			if (CollabHistoryItem.s_OnRestore != null)
			{
				CollabHistoryItem.s_OnRestore(this.m_RevisionId, false);
			}
		}

		private void GoBackTo()
		{
			if (CollabHistoryItem.s_OnGoBack != null)
			{
				CollabHistoryItem.s_OnGoBack(this.m_RevisionId, false);
			}
		}

		private void UpdateTo()
		{
			if (CollabHistoryItem.s_OnUpdate != null)
			{
				CollabHistoryItem.s_OnUpdate(this.m_RevisionId, true);
			}
		}

		private void UpdateTimeAgo()
		{
			this.m_TimeAgo.text = TimeAgo.GetString(this.m_TimeStamp);
		}

		private bool ShouldTruncateDescription(string description)
		{
			return description.Contains(Environment.NewLine) || description.Length > 500;
		}

		private string GetTruncatedDescription(string description)
		{
			string text = (!description.Contains(Environment.NewLine)) ? description : description.Substring(0, description.IndexOf(Environment.NewLine));
			if (text.Length > 500)
			{
				text = text.Substring(0, 500) + "...";
			}
			return text;
		}

		private void ToggleDescription()
		{
			if (this.m_Expanded)
			{
				this.m_Expanded = false;
				this.m_ExpandCollapseButton.text = "Show More";
				this.m_Description.text = this.GetTruncatedDescription(this.m_FullDescription);
			}
			else
			{
				this.m_Expanded = true;
				this.m_ExpandCollapseButton.text = "Show Less";
				this.m_Description.text = this.m_FullDescription;
			}
		}
	}
}
