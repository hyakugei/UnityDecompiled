using System;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine.Internal;
using UnityEngine.Scripting;

namespace UnityEngine
{
	public static class RemoteSettings
	{
		public delegate void UpdatedEventHandler();

		public static event RemoteSettings.UpdatedEventHandler Updated
		{
			add
			{
				RemoteSettings.UpdatedEventHandler updatedEventHandler = RemoteSettings.Updated;
				RemoteSettings.UpdatedEventHandler updatedEventHandler2;
				do
				{
					updatedEventHandler2 = updatedEventHandler;
					updatedEventHandler = Interlocked.CompareExchange<RemoteSettings.UpdatedEventHandler>(ref RemoteSettings.Updated, (RemoteSettings.UpdatedEventHandler)Delegate.Combine(updatedEventHandler2, value), updatedEventHandler);
				}
				while (updatedEventHandler != updatedEventHandler2);
			}
			remove
			{
				RemoteSettings.UpdatedEventHandler updatedEventHandler = RemoteSettings.Updated;
				RemoteSettings.UpdatedEventHandler updatedEventHandler2;
				do
				{
					updatedEventHandler2 = updatedEventHandler;
					updatedEventHandler = Interlocked.CompareExchange<RemoteSettings.UpdatedEventHandler>(ref RemoteSettings.Updated, (RemoteSettings.UpdatedEventHandler)Delegate.Remove(updatedEventHandler2, value), updatedEventHandler);
				}
				while (updatedEventHandler != updatedEventHandler2);
			}
		}

		public static event Action BeforeFetchFromServer
		{
			add
			{
				Action action = RemoteSettings.BeforeFetchFromServer;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action>(ref RemoteSettings.BeforeFetchFromServer, (Action)Delegate.Combine(action2, value), action);
				}
				while (action != action2);
			}
			remove
			{
				Action action = RemoteSettings.BeforeFetchFromServer;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action>(ref RemoteSettings.BeforeFetchFromServer, (Action)Delegate.Remove(action2, value), action);
				}
				while (action != action2);
			}
		}

		[RequiredByNativeCode]
		internal static void RemoteSettingsUpdated(bool wasLastUpdatedFromServer)
		{
			RemoteSettings.UpdatedEventHandler updated = RemoteSettings.Updated;
			if (updated != null)
			{
				updated();
			}
		}

		[RequiredByNativeCode]
		internal static void RemoteSettingsBeforeFetchFromServer()
		{
			Action beforeFetchFromServer = RemoteSettings.BeforeFetchFromServer;
			if (beforeFetchFromServer != null)
			{
				beforeFetchFromServer();
			}
		}

		[Obsolete("Calling CallOnUpdate() is not necessary any more and should be removed. Use RemoteSettingsUpdated instead", true)]
		public static void CallOnUpdate()
		{
			throw new NotSupportedException("Calling CallOnUpdate() is not necessary any more and should be removed.");
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void ForceUpdate();

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool WasLastUpdatedFromServer();

		[ExcludeFromDocs]
		public static int GetInt(string key)
		{
			return RemoteSettings.GetInt(key, 0);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int GetInt(string key, [DefaultValue("0")] int defaultValue);

		[ExcludeFromDocs]
		public static long GetLong(string key)
		{
			return RemoteSettings.GetLong(key, 0L);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern long GetLong(string key, [DefaultValue("0")] long defaultValue);

		[ExcludeFromDocs]
		public static float GetFloat(string key)
		{
			return RemoteSettings.GetFloat(key, 0f);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern float GetFloat(string key, [DefaultValue("0.0F")] float defaultValue);

		[ExcludeFromDocs]
		public static string GetString(string key)
		{
			return RemoteSettings.GetString(key, "");
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern string GetString(string key, [DefaultValue("\"\"")] string defaultValue);

		[ExcludeFromDocs]
		public static bool GetBool(string key)
		{
			return RemoteSettings.GetBool(key, false);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool GetBool(string key, [DefaultValue("false")] bool defaultValue);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool HasKey(string key);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int GetCount();

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern string[] GetKeys();
	}
}
