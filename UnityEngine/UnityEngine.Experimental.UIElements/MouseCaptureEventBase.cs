using System;

namespace UnityEngine.Experimental.UIElements
{
	public abstract class MouseCaptureEventBase<T> : EventBase<T>, IMouseCaptureEvent, IPropagatableEvent where T : MouseCaptureEventBase<T>, new()
	{
		protected MouseCaptureEventBase()
		{
			this.Init();
		}

		protected override void Init()
		{
			base.Init();
			base.flags = (EventBase.EventFlags.Bubbles | EventBase.EventFlags.Capturable);
		}

		public static T GetPooled(IEventHandler target)
		{
			T pooled = EventBase<T>.GetPooled();
			pooled.target = target;
			return pooled;
		}
	}
}
