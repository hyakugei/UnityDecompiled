using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine.Experimental.XR
{
	[NativeType(Header = "Modules/XR/XRSubsystemManager.h")]
	public static class XRSubsystemManager
	{
		static XRSubsystemManager()
		{
			XRSubsystemManager.StaticConstructScriptingClassMap();
		}

		public static void GetSubsystemDescriptors<T>(List<T> descriptors) where T : IXRSubsystemDescriptor
		{
			descriptors.Clear();
			foreach (IXRSubsystemDescriptorImpl current in Internal_XRSubsystemDescriptors.s_SubsystemDescriptors)
			{
				if (current is T)
				{
					descriptors.Add((T)((object)current));
				}
			}
		}

		public static void GetInstances<T>(List<T> instances) where T : XRInstance
		{
			instances.Clear();
			foreach (XRInstance current in Internal_XRSubsystemInstances.s_SubsystemInstances)
			{
				if (current is T)
				{
					instances.Add((T)((object)current));
				}
			}
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void DestroyInstance_Internal(IntPtr instancePtr);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void StaticConstructScriptingClassMap();
	}
}
