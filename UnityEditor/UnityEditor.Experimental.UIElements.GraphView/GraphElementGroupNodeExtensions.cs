using System;
using System.Collections.Generic;
using UnityEngine.Experimental.UIElements;

namespace UnityEditor.Experimental.UIElements.GraphView
{
	public static class GraphElementGroupNodeExtensions
	{
		public static GroupNode GetContainingGroupNode(this GraphElement element)
		{
			GroupNode result;
			if (element == null)
			{
				result = null;
			}
			else
			{
				GraphView firstAncestorOfType = element.GetFirstAncestorOfType<GraphView>();
				if (firstAncestorOfType == null)
				{
					result = null;
				}
				else
				{
					List<GroupNode> list = firstAncestorOfType.Query(null, null).ToList();
					foreach (GroupNode current in list)
					{
						if (current.ContainsElement(element))
						{
							result = current;
							return result;
						}
					}
					result = null;
				}
			}
			return result;
		}
	}
}
