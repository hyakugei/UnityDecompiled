using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityEditor.Advertisements
{
	public static class AdvertisementSettings
	{
		public static extern bool enabled
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern bool testMode
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern bool initializeOnStartup
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		internal static extern bool enabledForPlatform
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern string GetGameId(RuntimePlatform platform);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetGameId(RuntimePlatform platform, string gameId);

		[Obsolete("No longer supported and will always return true")]
		public static bool IsPlatformEnabled(RuntimePlatform platform)
		{
			return true;
		}

		[Obsolete("No longer supported and will do nothing")]
		public static void SetPlatformEnabled(RuntimePlatform platform, bool value)
		{
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern string GetPlatformGameId(string platformName);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetPlatformGameId(string platformName, string gameId);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void SetEnabledServiceWindow(bool enabled);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void ApplyEnableSettings(BuildTarget target);
	}
}
