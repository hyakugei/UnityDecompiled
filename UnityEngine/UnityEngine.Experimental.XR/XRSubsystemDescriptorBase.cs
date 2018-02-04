using System;
using System.Runtime.InteropServices;
using UnityEngine.Scripting;

namespace UnityEngine.Experimental.XR
{
	[UsedByNativeCode("XRSubsystemDescriptorBase")]
	[StructLayout(LayoutKind.Sequential)]
	public class XRSubsystemDescriptorBase : IXRSubsystemDescriptor, IXRSubsystemDescriptorImpl
	{
		internal IntPtr m_Ptr;

		IntPtr IXRSubsystemDescriptorImpl.ptr
		{
			get
			{
				return this.m_Ptr;
			}
			set
			{
				this.m_Ptr = value;
			}
		}

		public string id
		{
			get
			{
				return Internal_XRSubsystemDescriptors.GetId(this.m_Ptr);
			}
		}
	}
}
