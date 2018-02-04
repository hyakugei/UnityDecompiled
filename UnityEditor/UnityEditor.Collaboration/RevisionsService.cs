using System;
using UnityEditor.Connect;

namespace UnityEditor.Collaboration
{
	internal class RevisionsService : IRevisionsService
	{
		protected Collab collab;

		protected UnityConnect connect;

		protected RevisionsResult history;

		protected int historyOffset = 0;

		public string tipRevision
		{
			get
			{
				return this.collab.collabInfo.tip;
			}
		}

		public string currentUser
		{
			get
			{
				return this.connect.GetUserInfo().userName;
			}
		}

		public RevisionsService(Collab collabInstance, UnityConnect connectInstance)
		{
			this.collab = collabInstance;
			this.connect = connectInstance;
			this.history = new RevisionsResult();
		}

		public RevisionsResult GetRevisions(int offset, int count)
		{
			this.history.Clear();
			RevisionsData revisionsData = this.collab.GetRevisionsData(true, offset, count);
			this.history.Revisions.AddRange(revisionsData.Revisions);
			this.history.RevisionsInRepo = revisionsData.RevisionsInRepo;
			this.historyOffset = revisionsData.RevisionOffset;
			return this.history;
		}
	}
}
