using System;

namespace UnityEditor
{
	[Obsolete]
	[Serializable]
	public class ColorPickerHDRConfig
	{
		public float minBrightness;

		public float maxBrightness;

		public float minExposureValue;

		public float maxExposureValue;

		public ColorPickerHDRConfig(float minBrightness, float maxBrightness, float minExposureValue, float maxExposureValue)
		{
			this.minBrightness = minBrightness;
			this.maxBrightness = maxBrightness;
			this.minExposureValue = minExposureValue;
			this.maxExposureValue = maxExposureValue;
		}
	}
}
