using System;
using UnityEngine;

namespace UnityEditor.Experimental.UIElements.GraphView
{
	[Serializable]
	public class SearchTreeEntry : IComparable<SearchTreeEntry>
	{
		public int level;

		public GUIContent content;

		public object userData;

		public string name
		{
			get
			{
				return this.content.text;
			}
		}

		public SearchTreeEntry(GUIContent content)
		{
			this.content = content;
		}

		public int CompareTo(SearchTreeEntry o)
		{
			return this.name.CompareTo(o.name);
		}
	}
}
