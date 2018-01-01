using System;

namespace UnityEngine
{
	public struct LightBakingOutput
	{
		public int probeOcclusionLightIndex;

		public int occlusionMaskChannel;

		public LightmapBakeType lightmapBakeType;

		public MixedLightingMode mixedLightingMode;

		public bool isBaked;
	}
}
