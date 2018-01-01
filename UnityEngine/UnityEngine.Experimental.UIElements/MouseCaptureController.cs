using System;

namespace UnityEngine.Experimental.UIElements
{
	public static class MouseCaptureController
	{
		internal static IEventHandler mouseCapture
		{
			get;
			private set;
		}

		public static bool IsMouseCaptureTaken()
		{
			return MouseCaptureController.mouseCapture != null;
		}

		public static bool HasMouseCapture(this IEventHandler handler)
		{
			return MouseCaptureController.mouseCapture == handler;
		}

		public static void TakeMouseCapture(this IEventHandler handler)
		{
			if (MouseCaptureController.mouseCapture != handler)
			{
				if (GUIUtility.hotControl != 0)
				{
					Debug.Log("Should not be capturing when there is a hotcontrol");
				}
				else
				{
					MouseCaptureController.ReleaseMouseCapture();
					MouseCaptureController.mouseCapture = handler;
					using (MouseCaptureEvent pooled = MouseCaptureEventBase<MouseCaptureEvent>.GetPooled(MouseCaptureController.mouseCapture))
					{
						UIElementsUtility.eventDispatcher.DispatchEvent(pooled, null);
					}
				}
			}
		}

		public static void ReleaseMouseCapture(this IEventHandler handler)
		{
			Debug.Assert(handler == MouseCaptureController.mouseCapture, "Element releasing capture does not have capture");
			if (handler == MouseCaptureController.mouseCapture)
			{
				MouseCaptureController.ReleaseMouseCapture();
			}
		}

		public static void ReleaseMouseCapture()
		{
			if (MouseCaptureController.mouseCapture != null)
			{
				using (MouseCaptureOutEvent pooled = MouseCaptureEventBase<MouseCaptureOutEvent>.GetPooled(MouseCaptureController.mouseCapture))
				{
					UIElementsUtility.eventDispatcher.DispatchEvent(pooled, null);
				}
			}
			MouseCaptureController.mouseCapture = null;
		}
	}
}
