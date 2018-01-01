using System;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine.Internal;
using UnityEngine.Scripting;

namespace UnityEngine.XR.Tango
{
	[RequiredByNativeCode]
	public static class TangoInputTracking
	{
		private enum TrackingStateEventType
		{
			TrackingAcquired,
			TrackingLost
		}

		public static event Action<CoordinateFrame> trackingAcquired
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

		public static event Action<CoordinateFrame> trackingLost
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
		public static extern bool TryGetPoseAtTime(CoordinateFrame baseFrame, CoordinateFrame targetFrame, out PoseData pose, [DefaultValue("0.0f")] double time);

		public static bool TryGetPoseAtTime(CoordinateFrame baseFrame, CoordinateFrame targetFrame, out PoseData pose)
		{
			return TangoInputTracking.TryGetPoseAtTime(baseFrame, targetFrame, out pose, 0.0);
		}

		[RequiredByNativeCode]
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
