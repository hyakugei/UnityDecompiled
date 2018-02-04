using System;
using System.Collections.Generic;

namespace UnityEditor.Experimental.UIElements.GraphView
{
	public struct GraphViewChange
	{
		public List<GraphElement> elementsToRemove;

		public List<Edge> edgesToCreate;

		public List<GraphElement> movedElements;
	}
}
