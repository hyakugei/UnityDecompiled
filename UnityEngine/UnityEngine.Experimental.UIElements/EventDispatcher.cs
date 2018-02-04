using System;
using System.Collections.Generic;

namespace UnityEngine.Experimental.UIElements
{
	internal class EventDispatcher : IEventDispatcher
	{
		private struct PropagationPaths : IDisposable
		{
			public readonly List<VisualElement> capturePath;

			public readonly List<VisualElement> bubblePath;

			public PropagationPaths(int initialSize)
			{
				this.capturePath = new List<VisualElement>(initialSize);
				this.bubblePath = new List<VisualElement>(initialSize);
			}

			public void Dispose()
			{
				EventDispatcher.PropagationPathsPool.Release(this);
			}

			public void Clear()
			{
				this.bubblePath.Clear();
				this.capturePath.Clear();
			}
		}

		private static class PropagationPathsPool
		{
			private static readonly List<EventDispatcher.PropagationPaths> s_Available = new List<EventDispatcher.PropagationPaths>();

			public static EventDispatcher.PropagationPaths Acquire()
			{
				EventDispatcher.PropagationPaths result;
				if (EventDispatcher.PropagationPathsPool.s_Available.Count != 0)
				{
					EventDispatcher.PropagationPaths propagationPaths = EventDispatcher.PropagationPathsPool.s_Available[0];
					EventDispatcher.PropagationPathsPool.s_Available.RemoveAt(0);
					result = propagationPaths;
				}
				else
				{
					EventDispatcher.PropagationPaths propagationPaths2 = new EventDispatcher.PropagationPaths(16);
					result = propagationPaths2;
				}
				return result;
			}

			public static void Release(EventDispatcher.PropagationPaths po)
			{
				po.Clear();
				EventDispatcher.PropagationPathsPool.s_Available.Add(po);
			}
		}

		private VisualElement m_TopElementUnderMouse;

		private const int k_DefaultPropagationDepth = 16;

		private static readonly EventDispatcher.PropagationPaths k_EmptyPropagationPaths = new EventDispatcher.PropagationPaths(0);

		private void DispatchMouseEnterMouseLeave(VisualElement previousTopElementUnderMouse, VisualElement currentTopElementUnderMouse, Event triggerEvent)
		{
			if (previousTopElementUnderMouse != currentTopElementUnderMouse)
			{
				int i = 0;
				VisualElement visualElement;
				for (visualElement = previousTopElementUnderMouse; visualElement != null; visualElement = visualElement.shadow.parent)
				{
					i++;
				}
				int j = 0;
				VisualElement visualElement2;
				for (visualElement2 = currentTopElementUnderMouse; visualElement2 != null; visualElement2 = visualElement2.shadow.parent)
				{
					j++;
				}
				visualElement = previousTopElementUnderMouse;
				visualElement2 = currentTopElementUnderMouse;
				while (i > j)
				{
					using (MouseLeaveEvent pooled = MouseEventBase<MouseLeaveEvent>.GetPooled(triggerEvent))
					{
						pooled.target = visualElement;
						this.DispatchEvent(pooled, visualElement.panel);
					}
					i--;
					visualElement = visualElement.shadow.parent;
				}
				List<VisualElement> list = new List<VisualElement>(j);
				while (j > i)
				{
					list.Add(visualElement2);
					j--;
					visualElement2 = visualElement2.shadow.parent;
				}
				while (visualElement != visualElement2)
				{
					using (MouseLeaveEvent pooled2 = MouseEventBase<MouseLeaveEvent>.GetPooled(triggerEvent))
					{
						pooled2.target = visualElement;
						this.DispatchEvent(pooled2, visualElement.panel);
					}
					list.Add(visualElement2);
					visualElement = visualElement.shadow.parent;
					visualElement2 = visualElement2.shadow.parent;
				}
				for (int k = list.Count - 1; k >= 0; k--)
				{
					using (MouseEnterEvent pooled3 = MouseEventBase<MouseEnterEvent>.GetPooled(triggerEvent))
					{
						pooled3.target = list[k];
						this.DispatchEvent(pooled3, list[k].panel);
					}
				}
			}
		}

		private void DispatchMouseOverMouseOut(VisualElement previousTopElementUnderMouse, VisualElement currentTopElementUnderMouse, Event triggerEvent)
		{
			if (previousTopElementUnderMouse != currentTopElementUnderMouse)
			{
				if (previousTopElementUnderMouse != null)
				{
					using (MouseOutEvent pooled = MouseEventBase<MouseOutEvent>.GetPooled(triggerEvent))
					{
						pooled.target = previousTopElementUnderMouse;
						this.DispatchEvent(pooled, previousTopElementUnderMouse.panel);
					}
				}
				if (currentTopElementUnderMouse != null)
				{
					using (MouseOverEvent pooled2 = MouseEventBase<MouseOverEvent>.GetPooled(triggerEvent))
					{
						pooled2.target = currentTopElementUnderMouse;
						this.DispatchEvent(pooled2, currentTopElementUnderMouse.panel);
					}
				}
			}
		}

		public void DispatchEvent(EventBase evt, IPanel panel)
		{
			Event imguiEvent = evt.imguiEvent;
			if (imguiEvent == null || imguiEvent.type != EventType.Repaint)
			{
				if (panel != null && panel.panelDebug != null && panel.panelDebug.enabled && panel.panelDebug.interceptEvents != null && panel.panelDebug.interceptEvents(imguiEvent))
				{
					evt.StopPropagation();
				}
				else
				{
					bool flag = false;
					VisualElement visualElement = null;
					if ((evt is IMouseEvent || imguiEvent != null) && MouseCaptureController.mouseCapture != null)
					{
						visualElement = (MouseCaptureController.mouseCapture as VisualElement);
						if (visualElement != null && visualElement.panel == null)
						{
							Debug.Log(string.Format("Capture has no panel, forcing removal (capture={0} eventType={1})", MouseCaptureController.mouseCapture, (imguiEvent == null) ? "null" : imguiEvent.type.ToString()));
							MouseCaptureController.ReleaseMouseCapture();
							visualElement = null;
						}
						if (panel != null)
						{
							if (visualElement != null && visualElement.panel.contextType != panel.contextType)
							{
								return;
							}
						}
						flag = true;
						evt.dispatch = true;
						if (MouseCaptureController.mouseCapture != null)
						{
							evt.target = MouseCaptureController.mouseCapture;
							evt.currentTarget = MouseCaptureController.mouseCapture;
							evt.propagationPhase = PropagationPhase.AtTarget;
							MouseCaptureController.mouseCapture.HandleEvent(evt);
						}
						evt.propagationPhase = PropagationPhase.None;
						evt.currentTarget = null;
						evt.dispatch = false;
					}
					if (evt.isPropagationStopped)
					{
						if (evt.target == null && panel != null)
						{
							evt.target = panel.visualTree;
						}
						if (evt.target != null && evt.target != MouseCaptureController.mouseCapture)
						{
							evt.dispatch = true;
							evt.currentTarget = evt.target;
							evt.propagationPhase = PropagationPhase.AtTarget;
							evt.target.HandleEvent(evt);
							evt.propagationPhase = PropagationPhase.None;
							evt.currentTarget = null;
							evt.dispatch = false;
						}
					}
					if (!evt.isPropagationStopped)
					{
						if (evt is IKeyboardEvent && panel != null)
						{
							if (panel.focusController.focusedElement != null)
							{
								IMGUIContainer iMGUIContainer = panel.focusController.focusedElement as IMGUIContainer;
								flag = true;
								if (iMGUIContainer != null)
								{
									if (iMGUIContainer.HandleIMGUIEvent(evt.imguiEvent))
									{
										evt.StopPropagation();
										evt.PreventDefault();
									}
								}
								else
								{
									evt.target = panel.focusController.focusedElement;
									EventDispatcher.PropagateEvent(evt);
								}
							}
							else
							{
								evt.target = panel.visualTree;
								EventDispatcher.PropagateEvent(evt);
								flag = false;
							}
						}
						else if (evt.GetEventTypeId() == EventBase<MouseEnterEvent>.TypeId() || evt.GetEventTypeId() == EventBase<MouseLeaveEvent>.TypeId())
						{
							Debug.Assert(evt.target != null);
							flag = true;
							EventDispatcher.PropagateEvent(evt);
						}
						else if (evt is IMouseEvent || (imguiEvent != null && (imguiEvent.type == EventType.ContextClick || imguiEvent.type == EventType.DragUpdated || imguiEvent.type == EventType.DragPerform || imguiEvent.type == EventType.DragExited)))
						{
							VisualElement topElementUnderMouse = this.m_TopElementUnderMouse;
							if (evt.GetEventTypeId() == EventBase<MouseLeaveWindowEvent>.TypeId())
							{
								this.m_TopElementUnderMouse = null;
								this.DispatchMouseEnterMouseLeave(topElementUnderMouse, this.m_TopElementUnderMouse, imguiEvent);
								this.DispatchMouseOverMouseOut(topElementUnderMouse, this.m_TopElementUnderMouse, imguiEvent);
							}
							else if (evt is IMouseEvent || imguiEvent != null)
							{
								if (evt.target == null && panel != null)
								{
									if (evt is IMouseEvent)
									{
										this.m_TopElementUnderMouse = panel.Pick((evt as IMouseEvent).localMousePosition);
									}
									else if (imguiEvent != null)
									{
										this.m_TopElementUnderMouse = panel.Pick(imguiEvent.mousePosition);
									}
									evt.target = this.m_TopElementUnderMouse;
								}
								if (evt.target != null)
								{
									flag = true;
									EventDispatcher.PropagateEvent(evt);
								}
								if (evt.GetEventTypeId() == EventBase<MouseMoveEvent>.TypeId() || evt.GetEventTypeId() == EventBase<MouseEnterWindowEvent>.TypeId() || evt.GetEventTypeId() == EventBase<WheelEvent>.TypeId() || (imguiEvent != null && imguiEvent.type == EventType.DragUpdated))
								{
									this.DispatchMouseEnterMouseLeave(topElementUnderMouse, this.m_TopElementUnderMouse, imguiEvent);
									this.DispatchMouseOverMouseOut(topElementUnderMouse, this.m_TopElementUnderMouse, imguiEvent);
								}
							}
						}
						else if (panel != null && imguiEvent != null && (imguiEvent.type == EventType.ExecuteCommand || imguiEvent.type == EventType.ValidateCommand))
						{
							IMGUIContainer iMGUIContainer2 = panel.focusController.focusedElement as IMGUIContainer;
							if (iMGUIContainer2 != null)
							{
								flag = true;
								if (iMGUIContainer2.HandleIMGUIEvent(evt.imguiEvent))
								{
									evt.StopPropagation();
									evt.PreventDefault();
								}
							}
							else if (panel.focusController.focusedElement != null)
							{
								flag = true;
								evt.target = panel.focusController.focusedElement;
								EventDispatcher.PropagateEvent(evt);
							}
							else
							{
								flag = true;
								EventDispatcher.PropagateToIMGUIContainer(panel.visualTree, evt, visualElement);
							}
						}
						else if (evt is IPropagatableEvent || evt is IFocusEvent || evt is IChangeEvent || evt.GetEventTypeId() == EventBase<PostLayoutEvent>.TypeId() || evt.GetEventTypeId() == EventBase<InputEvent>.TypeId())
						{
							Debug.Assert(evt.target != null);
							flag = true;
							EventDispatcher.PropagateEvent(evt);
						}
					}
					if (!evt.isPropagationStopped && imguiEvent != null && panel != null)
					{
						if (!flag || (imguiEvent != null && (imguiEvent.type == EventType.MouseEnterWindow || imguiEvent.type == EventType.MouseLeaveWindow || imguiEvent.type == EventType.Used)))
						{
							EventDispatcher.PropagateToIMGUIContainer(panel.visualTree, evt, visualElement);
						}
					}
					if (evt.target == null && panel != null)
					{
						evt.target = panel.visualTree;
					}
					EventDispatcher.ExecuteDefaultAction(evt);
				}
			}
		}

		private static void PropagateToIMGUIContainer(VisualElement root, EventBase evt, VisualElement capture)
		{
			IMGUIContainer iMGUIContainer = root as IMGUIContainer;
			if (iMGUIContainer != null && (evt.imguiEvent.type == EventType.Used || root != capture))
			{
				if (iMGUIContainer.HandleIMGUIEvent(evt.imguiEvent))
				{
					evt.StopPropagation();
					evt.PreventDefault();
				}
			}
			else if (root != null)
			{
				for (int i = 0; i < root.shadow.childCount; i++)
				{
					EventDispatcher.PropagateToIMGUIContainer(root.shadow[i], evt, capture);
					if (evt.isPropagationStopped)
					{
						break;
					}
				}
			}
		}

		private static void PropagateEvent(EventBase evt)
		{
			if (!evt.dispatch)
			{
				using (EventDispatcher.PropagationPaths propagationPaths = EventDispatcher.BuildPropagationPath(evt.target as VisualElement))
				{
					evt.dispatch = true;
					if (evt.capturable && propagationPaths.capturePath.Count > 0)
					{
						evt.propagationPhase = PropagationPhase.Capture;
						for (int i = propagationPaths.capturePath.Count - 1; i >= 0; i--)
						{
							if (evt.isPropagationStopped)
							{
								break;
							}
							evt.currentTarget = propagationPaths.capturePath[i];
							evt.currentTarget.HandleEvent(evt);
						}
					}
					evt.propagationPhase = PropagationPhase.AtTarget;
					evt.currentTarget = evt.target;
					evt.currentTarget.HandleEvent(evt);
					if (evt.bubbles && propagationPaths.bubblePath.Count > 0)
					{
						evt.propagationPhase = PropagationPhase.BubbleUp;
						foreach (VisualElement current in propagationPaths.bubblePath)
						{
							if (evt.isPropagationStopped)
							{
								break;
							}
							evt.currentTarget = current;
							evt.currentTarget.HandleEvent(evt);
						}
					}
					evt.dispatch = false;
					evt.propagationPhase = PropagationPhase.None;
					evt.currentTarget = null;
				}
			}
		}

		private static void ExecuteDefaultAction(EventBase evt)
		{
			if (evt.target != null)
			{
				evt.dispatch = true;
				evt.currentTarget = evt.target;
				evt.propagationPhase = PropagationPhase.DefaultAction;
				evt.currentTarget.HandleEvent(evt);
				evt.propagationPhase = PropagationPhase.None;
				evt.currentTarget = null;
				evt.dispatch = false;
			}
		}

		private static EventDispatcher.PropagationPaths BuildPropagationPath(VisualElement elem)
		{
			EventDispatcher.PropagationPaths result;
			if (elem == null)
			{
				result = EventDispatcher.k_EmptyPropagationPaths;
			}
			else
			{
				EventDispatcher.PropagationPaths propagationPaths = EventDispatcher.PropagationPathsPool.Acquire();
				while (elem.shadow.parent != null)
				{
					if (elem.shadow.parent.enabledInHierarchy)
					{
						if (elem.shadow.parent.HasCaptureHandlers())
						{
							propagationPaths.capturePath.Add(elem.shadow.parent);
						}
						if (elem.shadow.parent.HasBubbleHandlers())
						{
							propagationPaths.bubblePath.Add(elem.shadow.parent);
						}
					}
					elem = elem.shadow.parent;
				}
				result = propagationPaths;
			}
			return result;
		}
	}
}
