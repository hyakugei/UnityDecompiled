using System;
using System.Collections.Generic;
using UnityEditor.Connect;
using UnityEditor.Web;

namespace UnityEditor.Collaboration
{
	internal class CollabHistoryPresenter
	{
		private const int k_ItemsPerPage = 5;

		private ICollabHistoryWindow m_Window;

		private ICollabHistoryItemFactory m_Factory;

		private IRevisionsService m_Service;

		private ConnectInfo m_ConnectState;

		private CollabInfo m_CollabState;

		private int m_TotalRevisions;

		private int m_CurrentPage;

		private BuildAccess m_BuildAccess;

		private string m_ProgressRevision;

		public bool BuildServiceEnabled
		{
			get;
			set;
		}

		public CollabHistoryPresenter(ICollabHistoryWindow window, ICollabHistoryItemFactory factory, IRevisionsService service)
		{
			this.m_Window = window;
			this.m_Factory = factory;
			this.m_Service = service;
			this.m_CurrentPage = 0;
			this.m_BuildAccess = new BuildAccess();
		}

		public void OnWindowEnabled()
		{
			UnityConnect.instance.StateChanged += new UnityEditor.Connect.StateChangedDelegate(this.OnConnectStateChanged);
			Collab.instance.StateChanged += new StateChangedDelegate(this.OnCollabStateChanged);
			Collab.instance.RevisionUpdated += new StateChangedDelegate(this.OnCollabRevisionUpdated);
			Collab.instance.JobsCompleted += new StateChangedDelegate(this.OnCollabJobsCompleted);
			EditorApplication.playModeStateChanged += new Action<PlayModeStateChange>(this.OnPlayModeStateChanged);
			if (Collab.instance.IsConnected())
			{
				this.m_ConnectState = UnityConnect.instance.GetConnectInfo();
				this.m_CollabState = Collab.instance.GetCollabInfo();
			}
			this.m_Window.revisionActionsEnabled = !EditorApplication.isPlayingOrWillChangePlaymode;
			this.m_Window.OnPageChangeAction = new PageChangeAction(this.OnUpdatePage);
			this.m_Window.OnUpdateAction = new RevisionAction(this.OnUpdate);
			this.m_Window.OnRestoreAction = new RevisionAction(this.OnRestore);
			this.m_Window.OnGoBackAction = new RevisionAction(this.OnGoBack);
			this.m_Window.OnShowBuildAction = new ShowBuildAction(this.ShowBuildForCommit);
			this.m_Window.OnShowServicesAction = new Action(this.ShowServicePage);
			this.m_Window.itemsPerPage = 5;
			this.UpdateBuildServiceStatus();
			HistoryState historyState = this.RecalculateState();
			if (historyState == HistoryState.Ready)
			{
				this.OnUpdatePage(this.m_CurrentPage);
			}
			this.m_Window.UpdateState(historyState, true);
		}

		public void OnWindowDisabled()
		{
			UnityConnect.instance.StateChanged -= new UnityEditor.Connect.StateChangedDelegate(this.OnConnectStateChanged);
			Collab.instance.StateChanged -= new StateChangedDelegate(this.OnCollabStateChanged);
			Collab.instance.RevisionUpdated -= new StateChangedDelegate(this.OnCollabRevisionUpdated);
			Collab.instance.JobsCompleted -= new StateChangedDelegate(this.OnCollabJobsCompleted);
			EditorApplication.playModeStateChanged -= new Action<PlayModeStateChange>(this.OnPlayModeStateChanged);
		}

		private void OnConnectStateChanged(ConnectInfo state)
		{
			this.m_ConnectState = state;
			this.m_Window.UpdateState(this.RecalculateState(), false);
		}

		private void OnCollabStateChanged(CollabInfo state)
		{
			if (!CollabHistoryPresenter.CollabStatesEqual(this.m_CollabState, state))
			{
				if (this.m_CollabState.tip != state.tip)
				{
					this.OnUpdatePage(this.m_CurrentPage);
				}
				this.m_CollabState = state;
				this.m_Window.UpdateState(this.RecalculateState(), false);
				if (state.inProgress)
				{
					this.m_Window.inProgressRevision = this.m_ProgressRevision;
				}
				else
				{
					this.m_Window.inProgressRevision = null;
				}
			}
		}

		private void OnCollabRevisionUpdated(CollabInfo state)
		{
			this.OnUpdatePage(this.m_CurrentPage);
		}

		private void OnCollabJobsCompleted(CollabInfo state)
		{
			this.m_ProgressRevision = null;
		}

		private void OnPlayModeStateChanged(PlayModeStateChange stateChange)
		{
			if (stateChange == PlayModeStateChange.ExitingEditMode || stateChange == PlayModeStateChange.EnteredPlayMode)
			{
				this.m_Window.revisionActionsEnabled = false;
			}
			else if (stateChange == PlayModeStateChange.EnteredEditMode || stateChange == PlayModeStateChange.ExitingPlayMode)
			{
				this.m_Window.revisionActionsEnabled = true;
			}
		}

		private HistoryState RecalculateState()
		{
			HistoryState result;
			if (this.m_ConnectState.error)
			{
				this.m_Window.errMessage = this.m_ConnectState.lastErrorMsg;
				result = HistoryState.Error;
			}
			else if (!this.m_ConnectState.online)
			{
				result = HistoryState.Offline;
			}
			else if (this.m_ConnectState.maintenance || this.m_CollabState.maintenance)
			{
				result = HistoryState.Maintenance;
			}
			else if (!this.m_ConnectState.loggedIn)
			{
				result = HistoryState.LoggedOut;
			}
			else if (!this.m_CollabState.seat)
			{
				result = HistoryState.NoSeat;
			}
			else if (!Collab.instance.IsCollabEnabledForCurrentProject())
			{
				result = HistoryState.Disabled;
			}
			else if (!Collab.instance.IsConnected() || !this.m_CollabState.ready)
			{
				result = HistoryState.Waiting;
			}
			else
			{
				result = HistoryState.Ready;
			}
			return result;
		}

		private static bool CollabStatesEqual(CollabInfo c1, CollabInfo c2)
		{
			return c1.ready == c2.ready && c1.update == c2.update && c1.publish == c2.publish && c1.inProgress == c2.inProgress && c1.maintenance == c2.maintenance && c1.conflict == c2.conflict && c1.dirty == c2.dirty && c1.refresh == c2.refresh && c1.seat == c2.seat && c1.tip == c2.tip;
		}

		public void UpdateBuildServiceStatus()
		{
			UnityConnectServiceCollection.ServiceInfo[] allServiceInfos = UnityConnectServiceCollection.instance.GetAllServiceInfos();
			for (int i = 0; i < allServiceInfos.Length; i++)
			{
				UnityConnectServiceCollection.ServiceInfo serviceInfo = allServiceInfos[i];
				if (serviceInfo.name.Equals("Build"))
				{
					this.BuildServiceEnabled = serviceInfo.enabled;
				}
			}
		}

		public void ShowBuildForCommit(string revisionID)
		{
			this.m_BuildAccess.ShowBuildForCommit(revisionID);
		}

		public void ShowServicePage()
		{
			this.m_BuildAccess.ShowServicePage();
		}

		public void OnUpdatePage(int page)
		{
			RevisionsResult revisions = this.m_Service.GetRevisions(page * 5, 5);
			this.m_TotalRevisions = revisions.RevisionsInRepo;
			IEnumerable<RevisionData> items = this.m_Factory.GenerateElements(revisions.Revisions, this.m_TotalRevisions, page * 5, this.m_Service.tipRevision, this.m_Window.inProgressRevision, this.m_Window.revisionActionsEnabled, this.BuildServiceEnabled, this.m_Service.currentUser);
			this.m_Window.UpdateRevisions(items, this.m_Service.tipRevision, this.m_TotalRevisions);
		}

		private void OnRestore(string revisionId, bool updatetorevision)
		{
			this.m_ProgressRevision = revisionId;
			Collab.instance.ResyncToRevision(revisionId);
		}

		private void OnGoBack(string revisionId, bool updatetorevision)
		{
			this.m_ProgressRevision = revisionId;
			Collab.instance.GoBackToRevision(revisionId, false);
		}

		private void OnUpdate(string revisionId, bool updatetorevision)
		{
			this.m_ProgressRevision = revisionId;
			Collab.instance.Update(revisionId, updatetorevision);
		}
	}
}
