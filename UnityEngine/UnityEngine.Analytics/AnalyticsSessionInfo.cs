using System;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine.Scripting;

namespace UnityEngine.Analytics
{
	[RequiredByNativeCode]
	public static class AnalyticsSessionInfo
	{
		public delegate void SessionStateChanged(AnalyticsSessionState sessionState, long sessionId, long sessionElapsedTime, bool sessionChanged);

		public static event AnalyticsSessionInfo.SessionStateChanged sessionStateChanged
		{
			add
			{
				/*
				AnalyticsSessionInfo.SessionStateChanged sessionStateChanged = AnalyticsSessionInfo.sessionStateChanged;
				AnalyticsSessionInfo.SessionStateChanged sessionStateChanged2;
				do
				{
					sessionStateChanged2 = sessionStateChanged;
					sessionStateChanged = Interlocked.CompareExchange<AnalyticsSessionInfo.SessionStateChanged>(ref AnalyticsSessionInfo.sessionStateChanged, (AnalyticsSessionInfo.SessionStateChanged)Delegate.Combine(sessionStateChanged2, value), sessionStateChanged);
				}
				while (sessionStateChanged != sessionStateChanged2);
				*/
			}
			remove
			{
				/*
				AnalyticsSessionInfo.SessionStateChanged sessionStateChanged = AnalyticsSessionInfo.sessionStateChanged;
				AnalyticsSessionInfo.SessionStateChanged sessionStateChanged2;
				do
				{
					sessionStateChanged2 = sessionStateChanged;
					sessionStateChanged = Interlocked.CompareExchange<AnalyticsSessionInfo.SessionStateChanged>(ref AnalyticsSessionInfo.sessionStateChanged, (AnalyticsSessionInfo.SessionStateChanged)Delegate.Remove(sessionStateChanged2, value), sessionStateChanged);
				}
				while (sessionStateChanged != sessionStateChanged2);
				*/
			}
		}

		public static extern AnalyticsSessionState sessionState
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public static extern long sessionId
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public static extern long sessionElapsedTime
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public static extern string userId
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		[RequiredByNativeCode]
		internal static void CallSessionStateChanged(AnalyticsSessionState sessionState, long sessionId, long sessionElapsedTime, bool sessionChanged)
		{
			/*
			AnalyticsSessionInfo.SessionStateChanged sessionStateChanged = AnalyticsSessionInfo.sessionStateChanged;
			if (sessionStateChanged != null)
			{
				sessionStateChanged(sessionState, sessionId, sessionElapsedTime, sessionChanged);
			}
			*/
		}
	}
}
