using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace UnityEditor.Experimental.UIElements.GraphView
{
	internal class GroupNodeDropArea : VisualElement, IDropTarget
	{
		public GroupNodeDropArea()
		{
			base.name = "GroupNodeDropAreaName";
		}

		public bool CanAcceptDrop(List<ISelectable> selection)
		{
			bool result;
			if (selection.Count == 0)
			{
				result = false;
			}
			else
			{
				foreach (ISelectable current in selection)
				{
					GraphElement graphElement = current as GraphElement;
					if (graphElement == null || graphElement is GroupNode)
					{
						result = false;
						return result;
					}
				}
				result = true;
			}
			return result;
		}

		public EventPropagation DragExited()
		{
			base.RemoveFromClassList("dragEntered");
			return EventPropagation.Continue;
		}

		public EventPropagation DragPerform(IMGUIEvent evt, IEnumerable<ISelectable> selection, IDropTarget dropTarget)
		{
			GroupNode firstAncestorOfType = base.parent.GetFirstAncestorOfType<GroupNode>();
			foreach (ISelectable current in selection)
			{
				if (current != firstAncestorOfType)
				{
					GraphElement element = current as GraphElement;
					if (!firstAncestorOfType.ContainsElement(element) && element.GetContainingGroupNode() == null)
					{
						firstAncestorOfType.AddElement(element);
					}
				}
			}
			base.RemoveFromClassList("dragEntered");
			return EventPropagation.Stop;
		}

		public EventPropagation DragUpdated(IMGUIEvent evt, IEnumerable<ISelectable> selection, IDropTarget dropTarget)
		{
			GroupNode firstAncestorOfType = base.parent.GetFirstAncestorOfType<GroupNode>();
			bool flag = false;
			foreach (ISelectable current in selection)
			{
				if (current != firstAncestorOfType)
				{
					GraphElement element = current as GraphElement;
					Event imguiEvent = evt.imguiEvent;
					if (imguiEvent.shift)
					{
						if (firstAncestorOfType.ContainsElement(element))
						{
							firstAncestorOfType.RemoveElement(element);
						}
					}
					else if (!firstAncestorOfType.ContainsElement(element) && element.GetContainingGroupNode() == null)
					{
						flag = true;
					}
				}
			}
			if (flag)
			{
				base.AddToClassList("dragEntered");
			}
			else
			{
				base.RemoveFromClassList("dragEntered");
			}
			return EventPropagation.Stop;
		}
	}
}
