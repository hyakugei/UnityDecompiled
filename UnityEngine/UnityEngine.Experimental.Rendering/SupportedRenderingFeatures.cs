using System;
using UnityEngine.Scripting;

namespace UnityEngine.Experimental.Rendering
{
	public class SupportedRenderingFeatures
	{
		[Flags]
		public enum ReflectionProbeSupportFlags
		{
			None = 0,
			Rotation = 1
		}

		[Flags]
		public enum LightmapMixedBakeMode
		{
			None = 0,
			IndirectOnly = 1,
			Subtractive = 2,
			Shadowmask = 4
		}

		private static SupportedRenderingFeatures s_Active = new SupportedRenderingFeatures();

		public static SupportedRenderingFeatures active
		{
			get
			{
				if (SupportedRenderingFeatures.s_Active == null)
				{
					SupportedRenderingFeatures.s_Active = new SupportedRenderingFeatures();
				}
				return SupportedRenderingFeatures.s_Active;
			}
			set
			{
				SupportedRenderingFeatures.s_Active = value;
			}
		}

		public SupportedRenderingFeatures.ReflectionProbeSupportFlags reflectionProbeSupportFlags
		{
			get;
			set;
		}

		public SupportedRenderingFeatures.LightmapMixedBakeMode defaultMixedLightingMode
		{
			get;
			set;
		}

		public SupportedRenderingFeatures.LightmapMixedBakeMode supportedMixedLightingModes
		{
			get;
			set;
		}

		public LightmapBakeType supportedLightmapBakeTypes
		{
			get;
			set;
		}

		public LightmapsMode supportedLightmapsModes
		{
			get;
			set;
		}

		public bool rendererSupportsLightProbeProxyVolumes
		{
			get;
			set;
		}

		public bool rendererSupportsMotionVectors
		{
			get;
			set;
		}

		public bool rendererSupportsReceiveShadows
		{
			get;
			set;
		}

		public bool rendererSupportsReflectionProbes
		{
			get;
			set;
		}

		public SupportedRenderingFeatures()
		{
			this.<reflectionProbeSupportFlags>k__BackingField = SupportedRenderingFeatures.ReflectionProbeSupportFlags.None;
			this.<defaultMixedLightingMode>k__BackingField = SupportedRenderingFeatures.LightmapMixedBakeMode.None;
			this.<supportedMixedLightingModes>k__BackingField = (SupportedRenderingFeatures.LightmapMixedBakeMode.IndirectOnly | SupportedRenderingFeatures.LightmapMixedBakeMode.Subtractive | SupportedRenderingFeatures.LightmapMixedBakeMode.Shadowmask);
			this.<supportedLightmapBakeTypes>k__BackingField = (LightmapBakeType.Realtime | LightmapBakeType.Baked | LightmapBakeType.Mixed);
			this.<supportedLightmapsModes>k__BackingField = LightmapsMode.CombinedDirectional;
			this.<rendererSupportsLightProbeProxyVolumes>k__BackingField = true;
			this.<rendererSupportsMotionVectors>k__BackingField = true;
			this.<rendererSupportsReceiveShadows>k__BackingField = true;
			this.<rendererSupportsReflectionProbes>k__BackingField = true;
			base..ctor();
		}

		[RequiredByNativeCode]
		internal static MixedLightingMode FallbackMixedLightingMode()
		{
			MixedLightingMode result;
			if (SupportedRenderingFeatures.active.defaultMixedLightingMode != SupportedRenderingFeatures.LightmapMixedBakeMode.None && (SupportedRenderingFeatures.active.supportedMixedLightingModes & SupportedRenderingFeatures.active.defaultMixedLightingMode) == SupportedRenderingFeatures.active.defaultMixedLightingMode)
			{
				SupportedRenderingFeatures.LightmapMixedBakeMode defaultMixedLightingMode = SupportedRenderingFeatures.active.defaultMixedLightingMode;
				if (defaultMixedLightingMode != SupportedRenderingFeatures.LightmapMixedBakeMode.Shadowmask)
				{
					if (defaultMixedLightingMode != SupportedRenderingFeatures.LightmapMixedBakeMode.Subtractive)
					{
						result = MixedLightingMode.IndirectOnly;
					}
					else
					{
						result = MixedLightingMode.Subtractive;
					}
				}
				else
				{
					result = MixedLightingMode.Shadowmask;
				}
			}
			else if (SupportedRenderingFeatures.IsMixedLightingModeSupported(MixedLightingMode.Shadowmask))
			{
				result = MixedLightingMode.Shadowmask;
			}
			else if (SupportedRenderingFeatures.IsMixedLightingModeSupported(MixedLightingMode.Subtractive))
			{
				result = MixedLightingMode.Subtractive;
			}
			else
			{
				result = MixedLightingMode.IndirectOnly;
			}
			return result;
		}

		[RequiredByNativeCode]
		internal static bool IsMixedLightingModeSupported(MixedLightingMode mixedMode)
		{
			return (mixedMode == MixedLightingMode.IndirectOnly && (SupportedRenderingFeatures.active.supportedMixedLightingModes & SupportedRenderingFeatures.LightmapMixedBakeMode.IndirectOnly) == SupportedRenderingFeatures.LightmapMixedBakeMode.IndirectOnly) || (mixedMode == MixedLightingMode.Subtractive && (SupportedRenderingFeatures.active.supportedMixedLightingModes & SupportedRenderingFeatures.LightmapMixedBakeMode.Subtractive) == SupportedRenderingFeatures.LightmapMixedBakeMode.Subtractive) || (mixedMode == MixedLightingMode.Shadowmask && (SupportedRenderingFeatures.active.supportedMixedLightingModes & SupportedRenderingFeatures.LightmapMixedBakeMode.Shadowmask) == SupportedRenderingFeatures.LightmapMixedBakeMode.Shadowmask);
		}

		[RequiredByNativeCode]
		internal static bool IsLightmapBakeTypeSupported(LightmapBakeType bakeType)
		{
			return (SupportedRenderingFeatures.active.supportedLightmapBakeTypes & bakeType) == bakeType;
		}

		[RequiredByNativeCode]
		internal static bool IsLightmapsModeSupported(LightmapsMode mode)
		{
			return (SupportedRenderingFeatures.active.supportedLightmapsModes & mode) == mode;
		}
	}
}
