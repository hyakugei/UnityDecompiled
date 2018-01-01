using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Unity.Collections.LowLevel.Unsafe
{
	[StructLayout(LayoutKind.Sequential)]
	public sealed class DisposeSentinel
	{
		public delegate void DeallocateDelegate(IntPtr buffer, Allocator allocator);

		private IntPtr m_Ptr;

		private DisposeSentinel.DeallocateDelegate m_DeallocateDelegate;

		private Allocator m_Label;

		private AtomicSafetyHandle m_Safety;

		private StackFrame m_StackFrame;

		private DisposeSentinel()
		{
		}

		public static void Dispose(AtomicSafetyHandle safety, ref DisposeSentinel sentinel)
		{
			AtomicSafetyHandle.CheckDeallocateAndThrow(safety);
			AtomicSafetyHandle.Release(safety);
			DisposeSentinel.Clear(ref sentinel);
		}

		public static void Create(IntPtr ptr, Allocator label, out AtomicSafetyHandle safety, out DisposeSentinel sentinel, int callSiteStackDepth, DisposeSentinel.DeallocateDelegate deallocateDelegate = null)
		{
			safety = AtomicSafetyHandle.Create();
			if (NativeLeakDetection.Mode == NativeLeakDetectionMode.Enabled)
			{
				sentinel = new DisposeSentinel
				{
					m_StackFrame = new StackFrame(callSiteStackDepth + 2, true),
					m_Ptr = ptr,
					m_Label = label,
					m_DeallocateDelegate = deallocateDelegate,
					m_Safety = safety
				};
			}
			else
			{
				sentinel = null;
			}
		}

		protected override void Finalize()
		{
			try
			{
				if (this.m_Ptr != IntPtr.Zero)
				{
					string fileName = this.m_StackFrame.GetFileName();
					int fileLineNumber = this.m_StackFrame.GetFileLineNumber();
					string msg = string.Format("A Native Collection created with Allocator.{2} has not been disposed, resulting in a memory leak. It was allocated at {0}:{1}.", fileName, fileLineNumber, this.m_Label);
					UnsafeUtility.LogError(msg, fileName, fileLineNumber);
					AtomicSafetyHandle.EnforceAllBufferJobsHaveCompletedAndRelease(this.m_Safety);
					if (this.m_DeallocateDelegate != null)
					{
						this.m_DeallocateDelegate(this.m_Ptr, this.m_Label);
					}
					else
					{
						UnsafeUtility.Free(this.m_Ptr, this.m_Label);
					}
				}
			}
			finally
			{
				base.Finalize();
			}
		}

		public static void UpdateBufferPtr(DisposeSentinel sentinel, IntPtr ptr)
		{
			if (sentinel != null)
			{
				sentinel.m_Ptr = ptr;
			}
		}

		public static void Clear(ref DisposeSentinel sentinel)
		{
			if (sentinel != null)
			{
				sentinel.m_Ptr = IntPtr.Zero;
				sentinel = null;
			}
		}
	}
}
