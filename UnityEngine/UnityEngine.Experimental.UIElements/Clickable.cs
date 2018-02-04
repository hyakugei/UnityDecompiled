using System;
using System.Threading;

namespace UnityEngine.Experimental.UIElements
{
	public class Clickable : MouseManipulator
	{
		private readonly long m_Delay;

		private readonly long m_Interval;

		private IVisualElementScheduledItem m_Repeater;

		public event Action clicked
		{
			add
			{
				/*
				Action action = this.clicked;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action>(ref this.clicked, (Action)Delegate.Combine(action2, value), action);
				}
				while (action != action2);
				*/
			}
			remove
			{
				/*
				Action action = this.clicked;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action>(ref this.clicked, (Action)Delegate.Remove(action2, value), action);
				}
				while (action != action2);
				*/
			}
		}

		public Vector2 lastMousePosition
		{
			get;
			private set;
		}

		public Clickable(Action handler, long delay, long interval) : this(handler)
		{
			this.m_Delay = delay;
			this.m_Interval = interval;
		}

		public Clickable(Action handler)
		{
			//this.clicked = handler;
			base.activators.Add(new ManipulatorActivationFilter
			{
				button = MouseButton.LeftMouse
			});
		}

		private void OnTimer(TimerState timerState)
		{
			/*
			if (this.clicked != null && this.IsRepeatable())
			{
				if (base.target.ContainsPoint(this.lastMousePosition))
				{
					this.clicked();
					base.target.pseudoStates |= PseudoStates.Active;
				}
				else
				{
					base.target.pseudoStates &= ~PseudoStates.Active;
				}
			}
			*/
		}

		private bool IsRepeatable()
		{
			return this.m_Delay > 0L || this.m_Interval > 0L;
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

		protected void OnMouseDown(MouseDownEvent evt)
		{
			if (base.CanStartManipulation(evt))
			{
				base.target.TakeMouseCapture();
				this.lastMousePosition = evt.localMousePosition;
				if (this.IsRepeatable())
				{
					/*
					if (this.clicked != null && base.target.ContainsPoint(evt.localMousePosition))
					{
						this.clicked();
					}
					*/
					if (this.m_Repeater == null)
					{
						this.m_Repeater = base.target.schedule.Execute(new Action<TimerState>(this.OnTimer)).Every(this.m_Interval).StartingIn(this.m_Delay);
					}
					else
					{
						this.m_Repeater.ExecuteLater(this.m_Delay);
					}
				}
				base.target.pseudoStates |= PseudoStates.Active;
				evt.StopPropagation();
			}
		}

		protected void OnMouseMove(MouseMoveEvent evt)
		{
			if (base.target.HasMouseCapture())
			{
				this.lastMousePosition = evt.localMousePosition;
				evt.StopPropagation();
			}
		}

		protected void OnMouseUp(MouseUpEvent evt)
		{
			if (base.CanStopManipulation(evt))
			{
				base.target.ReleaseMouseCapture();
				if (this.IsRepeatable())
				{
					if (this.m_Repeater != null)
					{
						this.m_Repeater.Pause();
					}
				}
				/*
				else if (this.clicked != null && base.target.ContainsPoint(evt.localMousePosition))
				{
					this.clicked();
				}
				*/
				base.target.pseudoStates &= ~PseudoStates.Active;
				evt.StopPropagation();
			}
		}
	}
}
