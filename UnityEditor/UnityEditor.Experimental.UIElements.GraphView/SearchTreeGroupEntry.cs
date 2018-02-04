using System;
using UnityEngine;

namespace UnityEditor.Experimental.UIElements.GraphView
{
	[Serializable]
	public class SearchTreeGroupEntry : SearchTreeEntry
	{
		internal int selectedIndex;

		internal Vector2 scroll;

		public SearchTreeGroupEntry(GUIContent content, int level = 0) : base(content)
		{
			this.content = content;
			this.level = level;
		}
	}
}
