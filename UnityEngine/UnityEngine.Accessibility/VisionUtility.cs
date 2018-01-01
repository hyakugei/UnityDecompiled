using System;
using System.Linq;
using UnityEngine.Scripting;

namespace UnityEngine.Accessibility
{
	[UsedByNativeCode]
	public static class VisionUtility
	{
		private static readonly Color[] s_ColorBlindSafePalette = new Color[]
		{
			new Color32(0, 0, 0, 255),
			new Color32(73, 0, 146, 255),
			new Color32(7, 71, 81, 255),
			new Color32(0, 146, 146, 255),
			new Color32(182, 109, 255, 255),
			new Color32(255, 109, 182, 255),
			new Color32(109, 182, 255, 255),
			new Color32(36, 255, 36, 255),
			new Color32(255, 182, 219, 255),
			new Color32(182, 219, 255, 255),
			new Color32(255, 255, 109, 255)
		};

		private static readonly float[] s_ColorBlindSafePaletteLuminanceValues = (from c in VisionUtility.s_ColorBlindSafePalette
		select VisionUtility.ComputePerceivedLuminance(c)).ToArray<float>();

		internal static float ComputePerceivedLuminance(Color color)
		{
			color = color.linear;
			return Mathf.LinearToGammaSpace(0.2126f * color.r + 0.7152f * color.g + 0.0722f * color.b);
		}

		public static int GetColorBlindSafePalette(Color[] palette, float minimumLuminance, float maximumLuminance)
		{
			if (palette == null)
			{
				throw new ArgumentNullException("palette");
			}
			Color[] array = (from i in Enumerable.Range(0, VisionUtility.s_ColorBlindSafePalette.Length)
			where VisionUtility.s_ColorBlindSafePaletteLuminanceValues[i] >= minimumLuminance && VisionUtility.s_ColorBlindSafePaletteLuminanceValues[i] <= maximumLuminance
			select VisionUtility.s_ColorBlindSafePalette[i]).ToArray<Color>();
			int num = Mathf.Min(palette.Length, array.Length);
			if (num > 0)
			{
				int k = 0;
				int num2 = palette.Length;
				while (k < num2)
				{
					palette[k] = array[k % num];
					k++;
				}
			}
			else
			{
				int j = 0;
				int num3 = palette.Length;
				while (j < num3)
				{
					palette[j] = default(Color);
					j++;
				}
			}
			return num;
		}
	}
}
