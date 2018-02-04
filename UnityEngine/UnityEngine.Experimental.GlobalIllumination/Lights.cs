using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine.Experimental.GlobalIllumination
{
	internal static class Lights
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void GetModified(List<Light> outLights);

		public static void SetFromScript(List<LightDataGI> lights)
		{
			Lights.SetFromScript_Internal((LightDataGI[])NoAllocHelpers.ExtractArrayFromList(lights), NoAllocHelpers.SafeLength<LightDataGI>(lights));
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetFromScript_Internal(LightDataGI[] lights, int count);

		[RequiredByNativeCode]
		private static void RequestLights_Internal()
		{
			Lightmapping.RequestLights();
		}
	}
}
