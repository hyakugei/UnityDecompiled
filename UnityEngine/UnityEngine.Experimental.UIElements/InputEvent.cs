using System;

namespace UnityEngine.Experimental.UIElements
{
	public class InputEvent : EventBase<InputEvent>
	{
		public string previousData
		{
			get;
			protected set;
		}

		public string newData
		{
			get;
			protected set;
		}

		public InputEvent()
		{
			this.Init();
		}

		protected override void Init()
		{
			base.Init();
			base.flags = (EventBase.EventFlags.Bubbles | EventBase.EventFlags.Capturable);
			this.previousData = null;
			this.newData = null;
		}

		public static InputEvent GetPooled(string previousData, string newData)
		{
			InputEvent pooled = EventBase<InputEvent>.GetPooled();
			pooled.previousData = previousData;
			pooled.newData = newData;
			return pooled;
		}
	}
}
