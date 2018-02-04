using System;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace UnityEditor.Experimental.UIElements.GraphView
{
	public class EdgeManipulator : MouseManipulator
	{
		private bool m_Active;

		private Edge m_Edge;

		private EdgePresenter m_EdgePresenter;

		private Vector2 m_PressPos;

		private Port m_ConnectedPort;

		private EdgeDragHelper m_ConnectedEdgeDragHelper;

		private Port m_DetachedPort;

		private bool m_DetachedFromInputPort;

		private static int s_StartDragDistance = 10;

		private MouseDownEvent m_LastMouseDownEvent;

		public EdgeManipulator()
		{
			base.activators.Add(new ManipulatorActivationFilter
			{
				button = MouseButton.LeftMouse
			});
			this.Reset();
		}

		protected override void RegisterCallbacksOnTarget()
		{
			base.target.RegisterCallback<MouseDownEvent>(new EventCallback<MouseDownEvent>(this.OnMouseDown), Capture.NoCapture);
			base.target.RegisterCallback<MouseMoveEvent>(new EventCallback<MouseMoveEvent>(this.OnMouseMove), Capture.NoCapture);
			base.target.RegisterCallback<MouseUpEvent>(new EventCallback<MouseUpEvent>(this.OnMouseUp), Capture.NoCapture);
			base.target.RegisterCallback<KeyDownEvent>(new EventCallback<KeyDownEvent>(this.OnKeyDown), Capture.NoCapture);
		}

		protected override void UnregisterCallbacksFromTarget()
		{
			base.target.UnregisterCallback<MouseDownEvent>(new EventCallback<MouseDownEvent>(this.OnMouseDown), Capture.NoCapture);
			base.target.UnregisterCallback<MouseMoveEvent>(new EventCallback<MouseMoveEvent>(this.OnMouseMove), Capture.NoCapture);
			base.target.UnregisterCallback<MouseUpEvent>(new EventCallback<MouseUpEvent>(this.OnMouseUp), Capture.NoCapture);
			base.target.UnregisterCallback<KeyDownEvent>(new EventCallback<KeyDownEvent>(this.OnKeyDown), Capture.NoCapture);
		}

		private void Reset()
		{
			this.m_Active = false;
			this.m_Edge = null;
			this.m_ConnectedPort = null;
			this.m_ConnectedEdgeDragHelper = null;
			this.m_DetachedPort = null;
			this.m_DetachedFromInputPort = false;
		}

		protected void OnMouseDown(MouseDownEvent evt)
		{
			if (this.m_Active)
			{
				evt.StopImmediatePropagation();
			}
			else if (base.CanStartManipulation(evt))
			{
				this.m_Edge = ((evt.target as VisualElement).parent as Edge);
				if (this.m_Edge != null)
				{
					this.m_EdgePresenter = this.m_Edge.GetPresenter<EdgePresenter>();
				}
				this.m_PressPos = evt.mousePosition;
				base.target.TakeMouseCapture();
				evt.StopPropagation();
				this.m_LastMouseDownEvent = evt;
			}
		}

		protected void OnMouseMove(MouseMoveEvent evt)
		{
			if (this.m_Edge != null)
			{
				evt.StopPropagation();
				if (this.m_DetachedPort == null)
				{
					float magnitude = (evt.mousePosition - this.m_PressPos).magnitude;
					if (magnitude < (float)EdgeManipulator.s_StartDragDistance)
					{
						return;
					}
					Vector2 b = new Vector2(this.m_Edge.output.GetGlobalCenter().x, this.m_Edge.output.GetGlobalCenter().y);
					Vector2 b2 = new Vector2(this.m_Edge.input.GetGlobalCenter().x, this.m_Edge.input.GetGlobalCenter().y);
					float magnitude2 = (this.m_PressPos - b).magnitude;
					float magnitude3 = (this.m_PressPos - b2).magnitude;
					this.m_DetachedFromInputPort = (magnitude3 < magnitude2);
					if (this.m_DetachedFromInputPort)
					{
						this.m_ConnectedPort = this.m_Edge.output;
						this.m_DetachedPort = this.m_Edge.input;
						this.m_DetachedPort.Disconnect(this.m_Edge);
						this.m_Edge.input = null;
						if (this.m_EdgePresenter != null)
						{
							this.m_EdgePresenter.input = null;
						}
					}
					else
					{
						this.m_ConnectedPort = this.m_Edge.input;
						this.m_DetachedPort = this.m_Edge.output;
						this.m_DetachedPort.Disconnect(this.m_Edge);
						this.m_Edge.output = null;
						if (this.m_EdgePresenter != null)
						{
							this.m_EdgePresenter.output = null;
						}
					}
					this.m_ConnectedEdgeDragHelper = this.m_ConnectedPort.edgeConnector.edgeDragHelper;
					this.m_ConnectedEdgeDragHelper.draggedPort = this.m_ConnectedPort;
					this.m_ConnectedEdgeDragHelper.edgeCandidate = this.m_Edge;
					this.m_Edge.candidatePosition = evt.mousePosition;
					if (this.m_ConnectedEdgeDragHelper.HandleMouseDown(this.m_LastMouseDownEvent))
					{
						this.m_Active = true;
					}
					else
					{
						this.Reset();
					}
					this.m_LastMouseDownEvent = null;
				}
				if (this.m_Active)
				{
					if (this.m_EdgePresenter != null)
					{
						this.m_EdgePresenter.candidatePosition = evt.mousePosition;
					}
					this.m_ConnectedEdgeDragHelper.HandleMouseMove(evt);
				}
			}
		}

		protected void OnMouseUp(MouseUpEvent evt)
		{
			if (base.CanStopManipulation(evt))
			{
				if (this.m_Active)
				{
					GraphView firstAncestorOfType = this.m_Edge.GetFirstAncestorOfType<GraphView>();
					this.RestoreDetachedPort();
					this.m_ConnectedEdgeDragHelper.HandleMouseUp(evt);
					if (this.m_EdgePresenter != null && (this.m_EdgePresenter.input == null || this.m_EdgePresenter.output == null))
					{
						this.m_EdgePresenter.input = null;
						this.m_EdgePresenter.output = null;
						firstAncestorOfType.presenter.RemoveElement(this.m_EdgePresenter);
					}
				}
				this.Reset();
				if (base.target.HasMouseCapture())
				{
					base.target.ReleaseMouseCapture();
				}
				evt.StopPropagation();
			}
		}

		protected void OnKeyDown(KeyDownEvent evt)
		{
			if (this.m_Active)
			{
				if (evt.keyCode == KeyCode.Escape)
				{
					this.RestoreDetachedPort();
					this.m_ConnectedEdgeDragHelper.Reset(false);
					this.Reset();
					base.target.ReleaseMouseCapture();
					evt.StopPropagation();
				}
			}
		}

		private void RestoreDetachedPort()
		{
			if (this.m_DetachedFromInputPort)
			{
				this.m_Edge.input = this.m_DetachedPort;
				if (this.m_EdgePresenter)
				{
					this.m_EdgePresenter.input = this.m_DetachedPort.GetPresenter<PortPresenter>();
				}
				else
				{
					this.m_DetachedPort.Connect(this.m_Edge);
				}
			}
			else
			{
				this.m_Edge.output = this.m_DetachedPort;
				if (this.m_EdgePresenter)
				{
					this.m_EdgePresenter.output = this.m_DetachedPort.GetPresenter<PortPresenter>();
				}
				else
				{
					this.m_DetachedPort.Connect(this.m_Edge);
				}
			}
		}
	}
}
