using System;
using System.Collections.Generic;

namespace UnityEngine.Experimental.GlobalIllumination
{
	public static class Lightmapping
	{
		public delegate void RequestLightsDelegate(List<Light> requests, List<LightDataGI> lightsOutput);

		private static readonly List<LightDataGI> s_Storage = new List<LightDataGI>();

		private static readonly List<Light> s_Requests = new List<Light>();

		private static readonly Lightmapping.RequestLightsDelegate s_DefaultDelegate = delegate(List<Light> requests, List<LightDataGI> lightsOutput)
		{
			DirectionalLight directionalLight = default(DirectionalLight);
			PointLight pointLight = default(PointLight);
			SpotLight spotLight = default(SpotLight);
			RectangleLight rectangleLight = default(RectangleLight);
			LightDataGI item = default(LightDataGI);
			foreach (Light current in requests)
			{
				switch (current.type)
				{
				case UnityEngine.LightType.Spot:
					LightmapperUtils.Extract(current, ref spotLight);
					item.Init(ref spotLight);
					break;
				case UnityEngine.LightType.Directional:
					LightmapperUtils.Extract(current, ref directionalLight);
					item.Init(ref directionalLight);
					break;
				case UnityEngine.LightType.Point:
					LightmapperUtils.Extract(current, ref pointLight);
					item.Init(ref pointLight);
					break;
				case UnityEngine.LightType.Area:
					LightmapperUtils.Extract(current, ref rectangleLight);
					item.Init(ref rectangleLight);
					break;
				default:
					item.InitNoBake(current.GetInstanceID());
					break;
				}
				lightsOutput.Add(item);
			}
		};

		private static Lightmapping.RequestLightsDelegate s_RequestLightsDelegate = Lightmapping.s_DefaultDelegate;

		public static void SetDelegate(Lightmapping.RequestLightsDelegate del)
		{
			Lightmapping.s_RequestLightsDelegate = ((del == null) ? Lightmapping.s_DefaultDelegate : del);
		}

		public static void ResetDelegate()
		{
			Lightmapping.s_RequestLightsDelegate = Lightmapping.s_DefaultDelegate;
		}

		public static void RequestLights()
		{
			Lightmapping.s_Requests.Clear();
			Lights.GetModified(Lightmapping.s_Requests);
			Lightmapping.s_Storage.Clear();
			Lightmapping.s_RequestLightsDelegate(Lightmapping.s_Requests, Lightmapping.s_Storage);
			Lights.SetFromScript(Lightmapping.s_Storage);
		}
	}
}
