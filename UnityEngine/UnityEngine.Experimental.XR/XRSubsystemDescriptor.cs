using System;
using System.Runtime.InteropServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.Experimental.XR
{
	[NativeType(Header = "Modules/XR/XRSubsystemDescriptor.h"), UsedByNativeCode("XRSubsystemDescriptor")]
	[StructLayout(LayoutKind.Sequential)]
	public class XRSubsystemDescriptor<TXRInstance> : XRSubsystemDescriptorBase where TXRInstance : XRInstance
	{
		public TXRInstance Create()
		{
			IntPtr ptr = Internal_XRSubsystemDescriptors.Create(this.m_Ptr);
			TXRInstance tXRInstance = (TXRInstance)((object)Internal_XRSubsystemInstances.Internal_GetInstanceByPtr(ptr));
			tXRInstance.m_subsystemDescriptor = this;
			return tXRInstance;
		}
	}
}
