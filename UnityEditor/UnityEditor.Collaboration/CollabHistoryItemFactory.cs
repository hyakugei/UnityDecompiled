using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace UnityEditor.Collaboration
{
	internal class CollabHistoryItemFactory : ICollabHistoryItemFactory
	{
		private const int k_MaxChangesPerRevision = 10;

		[DebuggerHidden]
		public IEnumerable<RevisionData> GenerateElements(IEnumerable<Revision> revisions, int totalRevisions, int startIndex, string tipRev, string inProgressRevision, bool revisionActionsEnabled, bool buildServiceEnabled, string currentUser)
		{
			CollabHistoryItemFactory.<GenerateElements>c__Iterator0 <GenerateElements>c__Iterator = new CollabHistoryItemFactory.<GenerateElements>c__Iterator0();
			<GenerateElements>c__Iterator.startIndex = startIndex;
			<GenerateElements>c__Iterator.revisions = revisions;
			<GenerateElements>c__Iterator.tipRev = tipRev;
			<GenerateElements>c__Iterator.buildServiceEnabled = buildServiceEnabled;
			<GenerateElements>c__Iterator.currentUser = currentUser;
			<GenerateElements>c__Iterator.totalRevisions = totalRevisions;
			<GenerateElements>c__Iterator.inProgressRevision = inProgressRevision;
			<GenerateElements>c__Iterator.revisionActionsEnabled = revisionActionsEnabled;
			CollabHistoryItemFactory.<GenerateElements>c__Iterator0 expr_44 = <GenerateElements>c__Iterator;
			expr_44.$PC = -2;
			return expr_44;
		}

		private static DateTime TimeStampToDateTime(double timeStamp)
		{
			DateTime result = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
			result = result.AddSeconds(timeStamp).ToLocalTime();
			return result;
		}
	}
}
