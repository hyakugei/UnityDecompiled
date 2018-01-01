using System;
using System.Collections.Generic;

namespace UnityEditor.Experimental.UIElements.GraphView
{
	public interface ISearchWindowProvider
	{
		List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context);

		bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context);
	}
}
