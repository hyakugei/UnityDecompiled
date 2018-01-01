using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Scripting;

namespace UnityEngine.iOS
{
	[UsedByNativeCode]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class OnDemandResourcesRequest : AsyncOperation, IDisposable
	{
		public extern string error
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public extern float loadingPriority
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		internal OnDemandResourcesRequest()
		{
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern string GetResourcePath(string resourceName);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void DestroyFromScript(IntPtr ptr);

		public void Dispose()
		{
			if (this.m_Ptr != IntPtr.Zero)
			{
				OnDemandResourcesRequest.DestroyFromScript(this.m_Ptr);
				this.m_Ptr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}

		~OnDemandResourcesRequest()
		{
			this.Dispose();
		}
	}
}
