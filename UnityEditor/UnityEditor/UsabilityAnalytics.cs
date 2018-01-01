using System;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;

namespace UnityEditor
{
	internal static class UsabilityAnalytics
	{
		public static void Track(string page)
		{
			UsabilityAnalytics.TrackPageView("editor.analytics.unity3d.com", page, "", false);
		}

		public static void Event(string category, string action, string label, int value)
		{
			UsabilityAnalytics.TrackEvent(category, action, label, value, false);
		}

		internal static void SendEvent(string subType, DateTime startTime, TimeSpan duration, bool isBlocking, object parameters)
		{
			if (startTime.Kind == DateTimeKind.Local)
			{
				throw new ArgumentException("Local DateTimes are not supported, use UTC instead.");
			}
			UsabilityAnalytics.SendUsabilityEvent(subType, startTime.Ticks, duration.Ticks, isBlocking, parameters);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SendUsabilityEvent(string subType, long startTimeTicks, long durationTicks, bool isBlocking, object parameters);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void TrackPageView(string hostname, string path, [DefaultValue("")] string referrer, [DefaultValue("false")] bool forceRequest);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void TrackEvent(string category, string action, [DefaultValue("")] string label, [DefaultValue("0")] int value, [DefaultValue("false")] bool forceRequest);
	}
}
