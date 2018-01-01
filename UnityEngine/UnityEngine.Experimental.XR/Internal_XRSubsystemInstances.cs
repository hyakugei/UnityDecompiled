using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace UnityEngine.Experimental.XR
{
	internal static class Internal_XRSubsystemInstances
	{
		internal static List<XRInstance> s_SubsystemInstances = new List<XRInstance>();

		[RequiredByNativeCode]
		internal static void Internal_InitializeManagedInstance(IntPtr ptr, XRInstance inst)
		{
			inst.m_Ptr = ptr;
			inst.SetHandle(inst);
			Internal_XRSubsystemInstances.s_SubsystemInstances.Add(inst);
		}

		[RequiredByNativeCode]
		internal static void Internal_ClearManagedInstances()
		{
			foreach (XRInstance current in Internal_XRSubsystemInstances.s_SubsystemInstances)
			{
				current.m_Ptr = IntPtr.Zero;
			}
			Internal_XRSubsystemInstances.s_SubsystemInstances.Clear();
		}

		[RequiredByNativeCode]
		internal static void Internal_RemoveInstanceByPtr(IntPtr ptr)
		{
			for (int i = Internal_XRSubsystemInstances.s_SubsystemInstances.Count - 1; i >= 0; i--)
			{
				if (Internal_XRSubsystemInstances.s_SubsystemInstances[i].m_Ptr == ptr)
				{
					Internal_XRSubsystemInstances.s_SubsystemInstances[i].m_Ptr = IntPtr.Zero;
					Internal_XRSubsystemInstances.s_SubsystemInstances.RemoveAt(i);
				}
			}
		}

		internal static XRInstance Internal_GetInstanceByPtr(IntPtr ptr)
		{
			XRInstance result;
			foreach (XRInstance current in Internal_XRSubsystemInstances.s_SubsystemInstances)
			{
				if (current.m_Ptr == ptr)
				{
					result = current;
					return result;
				}
			}
			result = null;
			return result;
		}
	}
}
