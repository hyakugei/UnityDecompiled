using System;

namespace UnityEngine.Experimental.UIElements
{
	public class ChangeEvent<T> : EventBase<ChangeEvent<T>>, IChangeEvent
	{
		public T previousValue
		{
			get;
			protected set;
		}

		public T newValue
		{
			get;
			protected set;
		}

		public ChangeEvent()
		{
			this.Init();
		}

		protected override void Init()
		{
			base.Init();
			base.flags = (EventBase.EventFlags.Bubbles | EventBase.EventFlags.Capturable);
			this.previousValue = default(T);
			this.newValue = default(T);
		}

		public static ChangeEvent<T> GetPooled(T previousValue, T newValue)
		{
			ChangeEvent<T> pooled = EventBase<ChangeEvent<T>>.GetPooled();
			pooled.previousValue = previousValue;
			pooled.newValue = newValue;
			return pooled;
		}
	}
}
