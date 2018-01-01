using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine.Experimental.XR
{
	internal static class Internal_XRSubsystemDescriptors
	{
		internal static List<IXRSubsystemDescriptorImpl> s_SubsystemDescriptors = new List<IXRSubsystemDescriptorImpl>();

		[RequiredByNativeCode]
		internal static void Internal_InitializeManagedDescriptor(IntPtr ptr, IXRSubsystemDescriptorImpl desc)
		{
			desc.ptr = ptr;
			Internal_XRSubsystemDescriptors.s_SubsystemDescriptors.Add(desc);
		}

		[RequiredByNativeCode]
		internal static void Internal_ClearManagedDescriptors()
		{
			foreach (IXRSubsystemDescriptorImpl current in Internal_XRSubsystemDescriptors.s_SubsystemDescriptors)
			{
				current.ptr = IntPtr.Zero;
			}
			Internal_XRSubsystemDescriptors.s_SubsystemDescriptors.Clear();
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern IntPtr Create(IntPtr descriptorPtr);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern string GetId(IntPtr descriptorPtr);
	}
}
