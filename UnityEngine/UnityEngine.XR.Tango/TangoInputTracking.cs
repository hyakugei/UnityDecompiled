using System;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine.Scripting;

namespace UnityEngine.XR.Tango
{
	[UsedByNativeCode]
	internal static class TangoInputTracking
	{
		private enum TrackingStateEventType
		{
			TrackingAcquired,
			TrackingLost
		}

		internal static event Action<CoordinateFrame> trackingAcquired
		{
			add
			{
				Action<CoordinateFrame> action = TangoInputTracking.trackingAcquired;
				Action<CoordinateFrame> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action<CoordinateFrame>>(ref TangoInputTracking.trackingAcquired, (Action<CoordinateFrame>)Delegate.Combine(action2, value), action);
				}
				while (action != action2);
			}
			remove
			{
				Action<CoordinateFrame> action = TangoInputTracking.trackingAcquired;
				Action<CoordinateFrame> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action<CoordinateFrame>>(ref TangoInputTracking.trackingAcquired, (Action<CoordinateFrame>)Delegate.Remove(action2, value), action);
				}
				while (action != action2);
			}
		}

		internal static event Action<CoordinateFrame> trackingLost
		{
			add
			{
				Action<CoordinateFrame> action = TangoInputTracking.trackingLost;
				Action<CoordinateFrame> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action<CoordinateFrame>>(ref TangoInputTracking.trackingLost, (Action<CoordinateFrame>)Delegate.Combine(action2, value), action);
				}
				while (action != action2);
			}
			remove
			{
				Action<CoordinateFrame> action = TangoInputTracking.trackingLost;
				Action<CoordinateFrame> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action<CoordinateFrame>>(ref TangoInputTracking.trackingLost, (Action<CoordinateFrame>)Delegate.Remove(action2, value), action);
				}
				while (action != action2);
			}
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool Internal_TryGetPoseAtTime(double time, ScreenOrientation screenOrientation, CoordinateFrame baseFrame, CoordinateFrame targetFrame, out PoseData pose);

		internal static bool TryGetPoseAtTime(out PoseData pose, CoordinateFrame baseFrame, CoordinateFrame targetFrame, double time, ScreenOrientation screenOrientation)
		{
			return TangoInputTracking.Internal_TryGetPoseAtTime(time, screenOrientation, baseFrame, targetFrame, out pose);
		}

		internal static bool TryGetPoseAtTime(out PoseData pose, CoordinateFrame baseFrame, CoordinateFrame targetFrame, double time = 0.0)
		{
			return TangoInputTracking.Internal_TryGetPoseAtTime(time, Screen.orientation, baseFrame, targetFrame, out pose);
		}

		[UsedByNativeCode]
		private static void InvokeTangoTrackingEvent(TangoInputTracking.TrackingStateEventType eventType, CoordinateFrame frame)
		{
			Action<CoordinateFrame> action;
			if (eventType != TangoInputTracking.TrackingStateEventType.TrackingAcquired)
			{
				if (eventType != TangoInputTracking.TrackingStateEventType.TrackingLost)
				{
					throw new ArgumentException("TrackingEventHandler - Invalid EventType: " + eventType);
				}
				action = TangoInputTracking.trackingLost;
			}
			else
			{
				action = TangoInputTracking.trackingAcquired;
			}
			if (action != null)
			{
				action(frame);
			}
		}

		static TangoInputTracking()
		{
			// Note: this type is marked as 'beforefieldinit'.
			TangoInputTracking.trackingAcquired = null;
			TangoInputTracking.trackingLost = null;
		}
	}
}
