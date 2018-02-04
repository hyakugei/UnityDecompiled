using System;

namespace UnityEngine.Experimental.UIElements
{
	public interface IUIElementDataWatch
	{
		IUIElementDataWatchRequest RegisterWatch(UnityEngine.Object toWatch, Action<UnityEngine.Object> watchNotification);

		void UnregisterWatch(IUIElementDataWatchRequest requested);
	}
}
