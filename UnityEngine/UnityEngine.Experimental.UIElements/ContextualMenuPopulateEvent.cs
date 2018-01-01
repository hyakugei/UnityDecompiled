using System;

namespace UnityEngine.Experimental.UIElements
{
	public class ContextualMenuPopulateEvent : MouseEventBase<ContextualMenuPopulateEvent>
	{
		public ContextualMenu menu
		{
			get;
			private set;
		}

		public ContextualMenuPopulateEvent()
		{
			this.Init();
		}

		public static ContextualMenuPopulateEvent GetPooled(EventBase triggerEvent, ContextualMenu menu, IEventHandler target)
		{
			ContextualMenuPopulateEvent pooled = EventBase<ContextualMenuPopulateEvent>.GetPooled();
			if (triggerEvent != null)
			{
				IMouseEvent mouseEvent = triggerEvent as IMouseEvent;
				if (mouseEvent != null)
				{
					pooled.modifiers = mouseEvent.modifiers;
					pooled.mousePosition = mouseEvent.mousePosition;
					pooled.localMousePosition = mouseEvent.mousePosition;
					pooled.mouseDelta = mouseEvent.mouseDelta;
					pooled.button = mouseEvent.button;
					pooled.clickCount = mouseEvent.clickCount;
				}
				pooled.target = target;
				pooled.menu = menu;
			}
			return pooled;
		}

		protected override void Init()
		{
			base.Init();
			this.menu = null;
		}
	}
}
