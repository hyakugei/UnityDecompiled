using System;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEditor.Collaboration
{
	[UsedByNativeCode]
	internal struct Revision
	{
		[NativeName("m_CommitterName")]
		private string m_AuthorName;

		[NativeName("m_CommitterEmail")]
		private string m_Author;

		private string m_Comment;

		private string m_RevisionID;

		private string m_Reference;

		private ulong m_TimeStamp;

		private bool m_IsObtained;

		private ChangeAction[] m_Entries;

		private CloudBuildStatus[] m_BuildStatuses;

		public string authorName
		{
			get
			{
				return this.m_AuthorName;
			}
		}

		public string author
		{
			get
			{
				return this.m_Author;
			}
		}

		public string comment
		{
			get
			{
				return this.m_Comment;
			}
		}

		public string revisionID
		{
			get
			{
				return this.m_RevisionID;
			}
		}

		public string reference
		{
			get
			{
				return this.m_Reference;
			}
		}

		public ulong timeStamp
		{
			get
			{
				return this.m_TimeStamp;
			}
		}

		public bool isObtained
		{
			get
			{
				return this.m_IsObtained;
			}
		}

		public ChangeAction[] entries
		{
			get
			{
				return this.m_Entries;
			}
		}

		public CloudBuildStatus[] buildStatuses
		{
			get
			{
				return this.m_BuildStatuses;
			}
		}
	}
}
