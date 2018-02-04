using System;
using System.Collections.Generic;

namespace UnityEditor.Collaboration
{
	internal class RevisionsResult
	{
		public List<Revision> Revisions = new List<Revision>();

		public int RevisionsInRepo = -1;

		public int Count
		{
			get
			{
				return this.Revisions.Count;
			}
		}

		public void Clear()
		{
			this.Revisions.Clear();
			this.RevisionsInRepo = -1;
		}
	}
}
