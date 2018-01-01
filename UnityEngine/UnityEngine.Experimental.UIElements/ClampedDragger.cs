using System;
using System.Threading;

namespace UnityEngine.Experimental.UIElements
{
	internal class ClampedDragger : Clickable
	{
		[Flags]
		public enum DragDirection
		{
			None = 0,
			LowToHigh = 1,
			HighToLow = 2,
			Free = 4
		}

		public event Action dragging
		{
			add
			{
				Action action = this.dragging;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action>(ref this.dragging, (Action)Delegate.Combine(action2, value), action);
				}
				while (action != action2);
			}
			remove
			{
				Action action = this.dragging;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action>(ref this.dragging, (Action)Delegate.Remove(action2, value), action);
				}
				while (action != action2);
			}
		}

		public ClampedDragger.DragDirection dragDirection
		{
			get;
			set;
		}

		private Slider slider
		{
			get;
			set;
		}

		public Vector2 startMousePosition
		{
			get;
			private set;
		}

		public Vector2 delta
		{
			get
			{
				return base.lastMousePosition - this.startMousePosition;
			}
		}

		public ClampedDragger(Slider slider, Action clickHandler, Action dragHandler) : base(clickHandler, 250L, 30L)
		{
			this.dragDirection = ClampedDragger.DragDirection.None;
			this.slider = slider;
			this.dragging += dragHandler;
		}

		protected override void RegisterCallbacksOnTarget()
		{
			base.target.RegisterCallback<MouseDownEvent>(new EventCallback<MouseDownEvent>(this.OnMouseDown), Capture.NoCapture);
			base.target.RegisterCallback<MouseMoveEvent>(new EventCallback<MouseMoveEvent>(this.OnMouseMove), Capture.NoCapture);
			base.target.RegisterCallback<MouseUpEvent>(new EventCallback<MouseUpEvent>(base.OnMouseUp), Capture.NoCapture);
		}

		protected override void UnregisterCallbacksFromTarget()
		{
			base.target.UnregisterCallback<MouseDownEvent>(new EventCallback<MouseDownEvent>(this.OnMouseDown), Capture.NoCapture);
			base.target.UnregisterCallback<MouseMoveEvent>(new EventCallback<MouseMoveEvent>(this.OnMouseMove), Capture.NoCapture);
			base.target.UnregisterCallback<MouseUpEvent>(new EventCallback<MouseUpEvent>(base.OnMouseUp), Capture.NoCapture);
		}

		private new void OnMouseDown(MouseDownEvent evt)
		{
			if (base.CanStartManipulation(evt))
			{
				this.startMousePosition = evt.localMousePosition;
				this.dragDirection = ClampedDragger.DragDirection.None;
				base.OnMouseDown(evt);
			}
		}

		private new void OnMouseMove(MouseMoveEvent evt)
		{
			if (base.target.HasMouseCapture())
			{
				base.OnMouseMove(evt);
				if (this.dragDirection == ClampedDragger.DragDirection.None)
				{
					this.dragDirection = ClampedDragger.DragDirection.Free;
				}
				if (this.dragDirection == ClampedDragger.DragDirection.Free)
				{
					if (this.dragging != null)
					{
						this.dragging();
					}
				}
			}
		}
	}
}
