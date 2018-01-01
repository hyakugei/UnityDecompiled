using System;
using System.Collections.Generic;

namespace UnityEditor.Collaboration
{
	internal interface ICollabHistoryWindow
	{
		bool revisionActionsEnabled
		{
			get;
			set;
		}

		int itemsPerPage
		{
			set;
		}

		string errMessage
		{
			set;
		}

		string inProgressRevision
		{
			get;
			set;
		}

		PageChangeAction OnPageChangeAction
		{
			set;
		}

		RevisionAction OnGoBackAction
		{
			set;
		}

		RevisionAction OnUpdateAction
		{
			set;
		}

		RevisionAction OnRestoreAction
		{
			set;
		}

		ShowBuildAction OnShowBuildAction
		{
			set;
		}

		Action OnShowServicesAction
		{
			set;
		}

		void UpdateState(HistoryState state, bool force);

		void UpdateRevisions(IEnumerable<RevisionData> items, string tip, int totalRevisions);
	}
}
