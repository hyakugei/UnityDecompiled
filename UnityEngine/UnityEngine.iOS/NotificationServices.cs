using System;
using System.Runtime.CompilerServices;

namespace UnityEngine.iOS
{
	public sealed class NotificationServices
	{
		public static extern int localNotificationCount
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public static extern int remoteNotificationCount
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public static extern NotificationType enabledNotificationTypes
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public static extern string registrationError
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public static extern byte[] deviceToken
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public static LocalNotification[] localNotifications
		{
			get
			{
				int localNotificationCount = NotificationServices.localNotificationCount;
				LocalNotification[] array = new LocalNotification[localNotificationCount];
				for (int i = 0; i < localNotificationCount; i++)
				{
					array[i] = NotificationServices.GetLocalNotificationImpl(i);
				}
				return array;
			}
		}

		public static RemoteNotification[] remoteNotifications
		{
			get
			{
				int remoteNotificationCount = NotificationServices.remoteNotificationCount;
				RemoteNotification[] array = new RemoteNotification[remoteNotificationCount];
				for (int i = 0; i < remoteNotificationCount; i++)
				{
					array[i] = NotificationServices.GetRemoteNotificationImpl(i);
				}
				return array;
			}
		}

		public static extern LocalNotification[] scheduledLocalNotifications
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void ClearLocalNotifications();

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void ClearRemoteNotifications();

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void Internal_RegisterImpl(NotificationType notificationTypes, bool registerForRemote);

		public static void RegisterForNotifications(NotificationType notificationTypes)
		{
			NotificationServices.Internal_RegisterImpl(notificationTypes, true);
		}

		public static void RegisterForNotifications(NotificationType notificationTypes, bool registerForRemote)
		{
			NotificationServices.Internal_RegisterImpl(notificationTypes, registerForRemote);
		}

		public static void ScheduleLocalNotification(LocalNotification notification)
		{
			notification.Schedule();
		}

		public static void PresentLocalNotificationNow(LocalNotification notification)
		{
			notification.PresentNow();
		}

		public static void CancelLocalNotification(LocalNotification notification)
		{
			notification.Cancel();
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void CancelAllLocalNotifications();

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void UnregisterForRemoteNotifications();

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern LocalNotification GetLocalNotificationImpl(int index);

		public static LocalNotification GetLocalNotification(int index)
		{
			if (index < 0 || index >= NotificationServices.localNotificationCount)
			{
				throw new ArgumentOutOfRangeException("index", "Index out of bounds.");
			}
			return NotificationServices.GetLocalNotificationImpl(index);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern RemoteNotification GetRemoteNotificationImpl(int index);

		public static RemoteNotification GetRemoteNotification(int index)
		{
			if (index < 0 || index >= NotificationServices.remoteNotificationCount)
			{
				throw new ArgumentOutOfRangeException("index", "Index out of bounds.");
			}
			return NotificationServices.GetRemoteNotificationImpl(index);
		}
	}
}
