using System;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace UnityEditor.Experimental.UIElements.GraphView
{
	public class ClickSelector : MouseManipulator
	{
		public ClickSelector()
		{
			base.activators.Add(new ManipulatorActivationFilter
			{
				button = MouseButton.LeftMouse
			});
			base.activators.Add(new ManipulatorActivationFilter
			{
				button = MouseButton.RightMouse
			});
			base.activators.Add(new ManipulatorActivationFilter
			{
				button = MouseButton.LeftMouse,
				modifiers = EventModifiers.Control
			});
			base.activators.Add(new ManipulatorActivationFilter
			{
				button = MouseButton.LeftMouse,
				modifiers = EventModifiers.Shift
			});
		}

		protected override void RegisterCallbacksOnTarget()
		{
			base.target.RegisterCallback<MouseDownEvent>(new EventCallback<MouseDownEvent>(this.OnMouseDown), Capture.NoCapture);
		}

		protected override void UnregisterCallbacksFromTarget()
		{
			base.target.UnregisterCallback<MouseDownEvent>(new EventCallback<MouseDownEvent>(this.OnMouseDown), Capture.NoCapture);
		}

		protected void OnMouseDown(MouseDownEvent e)
		{
			if (e.currentTarget is ISelectable)
			{
				if (base.CanStartManipulation(e))
				{
					if ((base.target as ISelectable).HitTest(e.localMousePosition))
					{
						GraphElement graphElement = e.currentTarget as GraphElement;
						if (graphElement != null)
						{
							VisualElement parent = graphElement.shadow.parent;
							while (parent != null && !(parent is GraphView))
							{
								parent = parent.shadow.parent;
							}
							GraphView selectionContainer = parent as GraphView;
							if (graphElement.IsSelected(selectionContainer) && e.ctrlKey)
							{
								graphElement.Unselect(selectionContainer);
							}
							else
							{
								graphElement.Select(selectionContainer, e.shiftKey || e.ctrlKey);
							}
						}
					}
				}
			}
		}
	}
}
