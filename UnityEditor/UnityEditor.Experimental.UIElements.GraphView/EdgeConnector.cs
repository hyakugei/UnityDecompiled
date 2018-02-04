using System;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace UnityEditor.Experimental.UIElements.GraphView
{
	public abstract class EdgeConnector : MouseManipulator
	{
		public abstract EdgeDragHelper edgeDragHelper
		{
			get;
		}
	}
	public class EdgeConnector<TEdge> : EdgeConnector where TEdge : Edge, new()
	{
		private EdgeDragHelper m_EdgeDragHelper;

		private Edge m_EdgeCandidate;

		protected bool m_Active;

		public override EdgeDragHelper edgeDragHelper
		{
			get
			{
				return this.m_EdgeDragHelper;
			}
		}

		public EdgeConnector(IEdgeConnectorListener listener)
		{
			this.m_EdgeDragHelper = new EdgeDragHelper<TEdge>(listener);
			this.m_Active = false;
			base.activators.Add(new ManipulatorActivationFilter
			{
				button = MouseButton.LeftMouse
			});
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

		protected virtual void OnMouseDown(MouseDownEvent e)
		{
			if (this.m_Active)
			{
				e.StopImmediatePropagation();
			}
			else if (base.CanStartManipulation(e))
			{
				Port port = e.target as Port;
				if (port != null)
				{
					this.m_EdgeCandidate = Activator.CreateInstance<TEdge>();
					this.m_EdgeDragHelper.draggedPort = port;
					this.m_EdgeDragHelper.edgeCandidate = this.m_EdgeCandidate;
					if (this.m_EdgeDragHelper.HandleMouseDown(e))
					{
						this.m_Active = true;
						base.target.TakeMouseCapture();
						e.StopPropagation();
					}
					else
					{
						this.m_EdgeDragHelper.Reset(false);
						this.m_EdgeCandidate = null;
					}
				}
			}
		}

		protected virtual void OnMouseMove(MouseMoveEvent e)
		{
			if (this.m_Active)
			{
				this.m_EdgeDragHelper.HandleMouseMove(e);
				this.m_EdgeCandidate.candidatePosition = e.mousePosition;
				this.m_EdgeCandidate.UpdateEdgeControl();
				e.StopPropagation();
			}
		}

		protected virtual void OnMouseUp(MouseUpEvent e)
		{
			if (this.m_Active && base.CanStopManipulation(e))
			{
				this.m_EdgeDragHelper.HandleMouseUp(e);
				this.m_Active = false;
				base.target.ReleaseMouseCapture();
				e.StopPropagation();
			}
		}

		private void OnKeyDown(KeyDownEvent e)
		{
			if (e.keyCode == KeyCode.Escape && this.m_Active)
			{
				Port port = e.target as Port;
				GraphView firstAncestorOfType = port.GetFirstAncestorOfType<GraphView>();
				firstAncestorOfType.RemoveElement(this.m_EdgeCandidate);
				this.m_EdgeCandidate.input = null;
				this.m_EdgeCandidate.output = null;
				this.m_EdgeCandidate = null;
				this.m_EdgeDragHelper.Reset(false);
				this.m_Active = false;
				base.target.ReleaseMouseCapture();
				e.StopPropagation();
			}
		}
	}
}
