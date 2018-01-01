using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.Experimental.XR
{
	[NativeType(Header = "Modules/XR/XRInstance.h"), UsedByNativeCode]
	[StructLayout(LayoutKind.Sequential)]
	public class XRInstance
	{
		internal IntPtr m_Ptr;

		internal IXRSubsystemDescriptor m_subsystemDescriptor;

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void SetHandle(XRInstance inst);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void Start();

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void Stop();

		public void Destroy()
		{
			IntPtr ptr = this.m_Ptr;
			Internal_XRSubsystemInstances.Internal_RemoveInstanceByPtr(this.m_Ptr);
			XRSubsystemManager.DestroyInstance_Internal(ptr);
		}
	}
	[UsedByNativeCode("XRInstance_TXRSubsystemDescriptor")]
	public class XRInstance<TXRSubsystemDescriptor> : XRInstance where TXRSubsystemDescriptor : IXRSubsystemDescriptor
	{
		public TXRSubsystemDescriptor SubsystemDescriptor
		{
			get
			{
				return (TXRSubsystemDescriptor)((object)this.m_subsystemDescriptor);
			}
		}
	}
}
