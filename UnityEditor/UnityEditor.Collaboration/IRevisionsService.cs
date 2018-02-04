using System;

namespace UnityEditor.Collaboration
{
	internal interface IRevisionsService
	{
		string tipRevision
		{
			get;
		}

		string currentUser
		{
			get;
		}

		RevisionsResult GetRevisions(int offset, int count);
	}
}
