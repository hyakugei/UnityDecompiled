using System;

namespace UnityEngine.Experimental.UIElements
{
	public interface IEventDispatcher
	{
		void DispatchEvent(EventBase evt, IPanel panel);
	}
}
