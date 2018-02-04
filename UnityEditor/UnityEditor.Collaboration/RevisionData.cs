using System;
using System.Collections.Generic;

namespace UnityEditor.Collaboration
{
	internal struct RevisionData
	{
		public string id;

		public int index;

		public DateTime timeStamp;

		public string authorName;

		public string comment;

		public bool obtained;

		public bool current;

		public bool inProgress;

		public bool enabled;

		public BuildState buildState;

		public int buildFailures;

		public ICollection<ChangeData> changes;

		public int changesTotal;

		public bool changesTruncated;
	}
}
