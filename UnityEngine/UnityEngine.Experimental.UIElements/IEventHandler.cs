using System;

namespace UnityEngine.Experimental.UIElements
{
	public interface IEventHandler
	{
		void HandleEvent(EventBase evt);

		bool HasCaptureHandlers();

		bool HasBubbleHandlers();
	}
}
