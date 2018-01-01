using System;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.Experimental.UIElements.StyleEnums;

namespace UnityEditor.Experimental.UIElements.GraphView
{
	public class Dragger : MouseManipulator
	{
		private Vector2 m_Start;

		protected bool m_Active;

		public Vector2 panSpeed
		{
			get;
			set;
		}

		public bool clampToParentEdges
		{
			get;
			set;
		}

		public Dragger()
		{
			base.activators.Add(new ManipulatorActivationFilter
			{
				button = MouseButton.LeftMouse
			});
			this.panSpeed = new Vector2(1f, 1f);
			this.clampToParentEdges = false;
			this.m_Active = false;
		}

		protected Rect CalculatePosition(float x, float y, float width, float height)
		{
			Rect result = new Rect(x, y, width, height);
			if (this.clampToParentEdges)
			{
				if (result.x < base.target.parent.layout.xMin)
				{
					result.x = base.target.parent.layout.xMin;
				}
				else if (result.xMax > base.target.parent.layout.xMax)
				{
					result.x = base.target.parent.layout.xMax - result.width;
				}
				if (result.y < base.target.parent.layout.yMin)
				{
					result.y = base.target.parent.layout.yMin;
				}
				else if (result.yMax > base.target.parent.layout.yMax)
				{
					result.y = base.target.parent.layout.yMax - result.height;
				}
				result.width = width;
				result.height = height;
			}
			return result;
		}

		protected override void RegisterCallbacksOnTarget()
		{
			base.target.RegisterCallback<MouseDownEvent>(new EventCallback<MouseDownEvent>(this.OnMouseDown), Capture.NoCapture);
			base.target.RegisterCallback<MouseMoveEvent>(new EventCallback<MouseMoveEvent>(this.OnMouseMove), Capture.NoCapture);
			base.target.RegisterCallback<MouseUpEvent>(new EventCallback<MouseUpEvent>(this.OnMouseUp), Capture.NoCapture);
		}

		protected override void UnregisterCallbacksFromTarget()
		{
			base.target.UnregisterCallback<MouseDownEvent>(new EventCallback<MouseDownEvent>(this.OnMouseDown), Capture.NoCapture);
			base.target.UnregisterCallback<MouseMoveEvent>(new EventCallback<MouseMoveEvent>(this.OnMouseMove), Capture.NoCapture);
			base.target.UnregisterCallback<MouseUpEvent>(new EventCallback<MouseUpEvent>(this.OnMouseUp), Capture.NoCapture);
		}

		protected void OnMouseDown(MouseDownEvent e)
		{
			if (this.m_Active)
			{
				e.StopImmediatePropagation();
			}
			else
			{
				GraphElement graphElement = e.target as GraphElement;
				if (graphElement == null || graphElement.IsMovable())
				{
					if (base.CanStartManipulation(e))
					{
						this.m_Start = e.localMousePosition;
						this.m_Active = true;
						base.target.TakeMouseCapture();
						e.StopPropagation();
					}
				}
			}
		}

		protected void OnMouseMove(MouseMoveEvent e)
		{
			GraphElement graphElement = e.target as GraphElement;
			if (graphElement == null || graphElement.IsMovable())
			{
				if (this.m_Active)
				{
					if (base.target.style.positionType == PositionType.Manual)
					{
						Vector2 vector = e.localMousePosition - this.m_Start;
						base.target.layout = this.CalculatePosition(base.target.layout.x + vector.x, base.target.layout.y + vector.y, base.target.layout.width, base.target.layout.height);
					}
					e.StopPropagation();
				}
			}
		}

		protected void OnMouseUp(MouseUpEvent e)
		{
			GraphElement graphElement = e.target as GraphElement;
			if (graphElement == null || graphElement.IsMovable())
			{
				if (this.m_Active)
				{
					if (base.CanStopManipulation(e))
					{
						GraphElement graphElement2 = base.target as GraphElement;
						if (graphElement2 != null)
						{
							graphElement2.UpdatePresenterPosition();
						}
						this.m_Active = false;
						base.target.ReleaseMouseCapture();
						e.StopPropagation();
					}
				}
			}
		}
	}
}
