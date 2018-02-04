using System;

namespace UnityEngine.Experimental.GlobalIllumination
{
	public static class LightmapperUtils
	{
		public static LightMode Extract(LightmapBakeType baketype)
		{
			return (baketype != LightmapBakeType.Realtime) ? ((baketype != LightmapBakeType.Mixed) ? LightMode.Baked : LightMode.Mixed) : LightMode.Realtime;
		}

		public static LinearColor ExtractIndirect(Light l)
		{
			return LinearColor.Convert(l.color, l.intensity * l.bounceIntensity);
		}

		public static float ExtractInnerCone(Light l)
		{
			return 2f * Mathf.Atan(Mathf.Tan(l.spotAngle * 0.5f * 0.0174532924f) * 46f / 64f);
		}

		public static void Extract(Light l, ref DirectionalLight dir)
		{
			dir.instanceID = l.GetInstanceID();
			dir.mode = LightmapperUtils.Extract(l.lightmapBakeType);
			dir.shadow = (l.shadows != LightShadows.None);
			dir.direction = l.transform.forward;
			dir.color = LinearColor.Convert(l.color, l.intensity);
			dir.indirectColor = LightmapperUtils.ExtractIndirect(l);
			dir.penumbraWidthRadian = ((l.shadows != LightShadows.Soft) ? 0f : (0.0174532924f * l.shadowAngle));
		}

		public static void Extract(Light l, ref PointLight point)
		{
			point.instanceID = l.GetInstanceID();
			point.mode = LightmapperUtils.Extract(l.lightmapBakeType);
			point.shadow = (l.shadows != LightShadows.None);
			point.position = l.transform.position;
			point.color = LinearColor.Convert(l.color, l.intensity);
			point.indirectColor = LightmapperUtils.ExtractIndirect(l);
			point.range = l.range;
			point.sphereRadius = ((l.shadows != LightShadows.Soft) ? 0f : l.shadowRadius);
		}

		public static void Extract(Light l, ref SpotLight spot)
		{
			spot.instanceID = l.GetInstanceID();
			spot.mode = LightmapperUtils.Extract(l.lightmapBakeType);
			spot.shadow = (l.shadows != LightShadows.None);
			spot.position = l.transform.position;
			spot.orientation = l.transform.rotation;
			spot.color = LinearColor.Convert(l.color, l.intensity);
			spot.indirectColor = LightmapperUtils.ExtractIndirect(l);
			spot.range = l.range;
			spot.sphereRadius = ((l.shadows != LightShadows.Soft) ? 0f : l.shadowRadius);
			spot.coneAngle = l.spotAngle * 0.0174532924f;
			spot.innerConeAngle = LightmapperUtils.ExtractInnerCone(l);
		}

		public static void Extract(Light l, ref RectangleLight rect)
		{
			rect.instanceID = l.GetInstanceID();
			rect.mode = LightmapperUtils.Extract(l.lightmapBakeType);
			rect.shadow = (l.shadows != LightShadows.None);
			rect.position = l.transform.position;
			rect.orientation = l.transform.rotation;
			rect.color = LinearColor.Convert(l.color, l.intensity);
			rect.indirectColor = LightmapperUtils.ExtractIndirect(l);
			rect.range = l.range;
			rect.width = l.areaSize.x;
			rect.height = l.areaSize.y;
		}
	}
}
