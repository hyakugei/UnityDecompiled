using System;
using UnityEngine;

namespace UnityEditor.Experimental.UIElements.GraphView
{
	public interface IEdgeConnectorListener
	{
		void OnDropOutsidePort(Edge edge, Vector2 position);

		void OnDrop(GraphView graphView, Edge edge);
	}
}
