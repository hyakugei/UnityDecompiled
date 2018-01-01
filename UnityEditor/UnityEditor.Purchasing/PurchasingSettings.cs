using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityEditor.Purchasing
{
	public static class PurchasingSettings
	{
		[ThreadAndSerializationSafe]
		public static extern bool enabled
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
		internal static extern void ApplyEnableSettings(BuildTarget target);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void SetEnabledServiceWindow(bool enabled);
	}
}
