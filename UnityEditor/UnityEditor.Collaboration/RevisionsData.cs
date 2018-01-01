using System;
using UnityEngine.Scripting;

namespace UnityEditor.Collaboration
{
	[UsedByNativeCode]
	internal struct RevisionsData
	{
		private int m_RevisionsInRepo;

		private int m_RevisionOffset;

		private int m_ReturnedRevisions;

		private Revision[] m_Revisions;

		public int RevisionsInRepo
		{
			get
			{
				return this.m_RevisionsInRepo;
			}
		}

		public int RevisionOffset
		{
			get
			{
				return this.m_RevisionOffset;
			}
		}

		public int ReturnedRevisions
		{
			get
			{
				return this.m_ReturnedRevisions;
			}
		}

		public Revision[] Revisions
		{
			get
			{
				return this.m_Revisions;
			}
		}
	}
}
