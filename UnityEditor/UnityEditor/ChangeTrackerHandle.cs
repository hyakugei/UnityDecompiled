using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Scripting;

namespace UnityEditor
{
	[RequiredByNativeCode]
	internal struct ChangeTrackerHandle
	{
		private IntPtr m_Handle;

		internal static ChangeTrackerHandle AcquireTracker(UnityEngine.Object obj)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("Not a valid unity engine object");
			}
			return new ChangeTrackerHandle
			{
				m_Handle = ChangeTrackerHandle.Internal_AcquireTracker(obj)
			};
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr Internal_AcquireTracker(UnityEngine.Object o);

		internal void ReleaseTracker()
		{
			if (this.m_Handle == IntPtr.Zero)
			{
				throw new ArgumentNullException("Not a valid handle, has it been released already?");
			}
			ChangeTrackerHandle.Internal_ReleaseTracker(this.m_Handle);
			this.m_Handle = IntPtr.Zero;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_ReleaseTracker(IntPtr handle);

		internal bool PollForChanges()
		{
			if (this.m_Handle == IntPtr.Zero)
			{
				throw new ArgumentNullException("Not a valid handle, has it been released already?");
			}
			return ChangeTrackerHandle.Internal_PollChanges(this.m_Handle);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool Internal_PollChanges(IntPtr handle);

		internal void ForceDirtyNextPoll()
		{
			if (this.m_Handle == IntPtr.Zero)
			{
				throw new ArgumentNullException("Not a valid handle, has it been released already?");
			}
			ChangeTrackerHandle.Internal_ForceUpdate(this.m_Handle);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_ForceUpdate(IntPtr handle);
	}
}
