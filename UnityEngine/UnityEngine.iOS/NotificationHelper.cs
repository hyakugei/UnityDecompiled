using System;
using System.Runtime.CompilerServices;

namespace UnityEngine.iOS
{
	internal sealed class NotificationHelper
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern IntPtr CreateLocal();

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void DestroyLocal(IntPtr target);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void DestroyRemote(IntPtr target);
	}
}
