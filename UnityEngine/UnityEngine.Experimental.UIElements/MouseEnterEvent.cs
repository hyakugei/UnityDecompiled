using System;

namespace UnityEngine.Experimental.UIElements
{
	public class MouseEnterEvent : MouseEventBase<MouseEnterEvent>
	{
		public MouseEnterEvent()
		{
			this.Init();
		}

		protected override void Init()
		{
			base.Init();
			base.flags = EventBase.EventFlags.Capturable;
		}
	}
}
