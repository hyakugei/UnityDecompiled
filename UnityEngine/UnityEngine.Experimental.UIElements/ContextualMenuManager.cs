using System;

namespace UnityEngine.Experimental.UIElements
{
	internal abstract class ContextualMenuManager
	{
		public abstract void DisplayMenuIfEventMatches(EventBase evt, IEventHandler eventHandler);

		public void DisplayMenu(EventBase triggerEvent, IEventHandler target)
		{
			ContextualMenu contextualMenu = new ContextualMenu();
			bool flag;
			using (ContextualMenuPopulateEvent pooled = ContextualMenuPopulateEvent.GetPooled(triggerEvent, contextualMenu, target))
			{
				UIElementsUtility.eventDispatcher.DispatchEvent(pooled, null);
				flag = !pooled.isDefaultPrevented;
			}
			if (flag)
			{
				contextualMenu.PrepareForDisplay(triggerEvent);
				this.DoDisplayMenu(contextualMenu, triggerEvent);
			}
		}

		protected abstract void DoDisplayMenu(ContextualMenu menu, EventBase triggerEvent);
	}
}
