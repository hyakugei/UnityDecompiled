using System;
using UnityEngine;

namespace UnityEditor
{
	[Serializable]
	internal class ColorMutator
	{
		private const byte k_MaxByteForOverexposedColor = 191;

		[SerializeField]
		private Color m_OriginalColor;

		[SerializeField]
		private byte[] m_Color = new byte[4];

		[SerializeField]
		private float[] m_ColorHdr = new float[4];

		[SerializeField]
		private float[] m_Hsv = new float[3];

		[SerializeField]
		private float m_ExposureValue;

		public Color originalColor
		{
			get
			{
				return this.m_OriginalColor;
			}
		}

		public Color32 color
		{
			get
			{
				return new Color32(this.m_Color[0], this.m_Color[1], this.m_Color[2], this.m_Color[3]);
			}
		}

		public Color exposureAdjustedColor
		{
			get
			{
				return new Color(this.m_ColorHdr[0], this.m_ColorHdr[1], this.m_ColorHdr[2], this.m_ColorHdr[3]);
			}
		}

		public Vector3 colorHsv
		{
			get
			{
				return new Vector3(this.m_Hsv[0], this.m_Hsv[1], this.m_Hsv[2]);
			}
		}

		public float exposureValue
		{
			get
			{
				return this.m_ExposureValue;
			}
			set
			{
				if (this.m_ExposureValue != value)
				{
					this.m_ExposureValue = value;
					Color color = this.color * Mathf.Pow(2f, this.m_ExposureValue);
					this.m_ColorHdr[0] = color.r;
					this.m_ColorHdr[1] = color.g;
					this.m_ColorHdr[2] = color.b;
				}
			}
		}

		public ColorMutator(Color originalColor)
		{
			this.m_OriginalColor = originalColor;
			this.Reset();
		}

		public byte GetColorChannel(RgbaChannel channel)
		{
			return this.m_Color[(int)channel];
		}

		public float GetColorChannelNormalized(RgbaChannel channel)
		{
			return (float)this.m_Color[(int)channel] / 255f;
		}

		public void SetColorChannel(RgbaChannel channel, byte value)
		{
			if (this.m_Color[(int)channel] != value)
			{
				this.m_Color[(int)channel] = value;
				this.m_ColorHdr[(int)channel] = (float)value / 255f;
				if (channel != RgbaChannel.A)
				{
					this.m_ColorHdr[(int)channel] *= Mathf.Pow(2f, this.m_ExposureValue);
				}
				Color.RGBToHSV(this.color, out this.m_Hsv[0], out this.m_Hsv[1], out this.m_Hsv[2]);
			}
		}

		public void SetColorChannel(RgbaChannel channel, float normalizedValue)
		{
			this.SetColorChannel(channel, (byte)Mathf.RoundToInt(Mathf.Clamp01(normalizedValue) * 255f));
		}

		public float GetColorChannelHdr(RgbaChannel channel)
		{
			return this.m_ColorHdr[(int)channel];
		}

		public void SetColorChannelHdr(RgbaChannel channel, float value)
		{
			if (this.m_ColorHdr[(int)channel] != value)
			{
				this.m_ColorHdr[(int)channel] = value;
				this.OnRgbaHdrChannelChanged((int)channel);
			}
		}

		public float GetColorChannel(HsvChannel channel)
		{
			return this.m_Hsv[(int)channel];
		}

		public void SetColorChannel(HsvChannel channel, float value)
		{
			this.m_Hsv[(int)channel] = Mathf.Clamp01(value);
			Color a = Color.HSVToRGB(this.m_Hsv[0], this.m_Hsv[1], this.m_Hsv[2]);
			this.m_Color[0] = (byte)Mathf.CeilToInt(a.r * 255f);
			this.m_Color[1] = (byte)Mathf.CeilToInt(a.g * 255f);
			this.m_Color[2] = (byte)Mathf.CeilToInt(a.b * 255f);
			a *= Mathf.Pow(2f, this.m_ExposureValue);
			this.m_ColorHdr[0] = a.r;
			this.m_ColorHdr[1] = a.g;
			this.m_ColorHdr[2] = a.b;
		}

		public void Reset()
		{
			if (this.m_ColorHdr == null || this.m_ColorHdr.Length != 4)
			{
				this.m_ColorHdr = new float[4];
			}
			this.m_ColorHdr[0] = this.m_OriginalColor.r;
			this.m_ColorHdr[1] = this.m_OriginalColor.g;
			this.m_ColorHdr[2] = this.m_OriginalColor.b;
			this.m_ColorHdr[3] = this.m_OriginalColor.a;
			if (this.m_Color == null || this.m_Color.Length != 4)
			{
				this.m_Color = new byte[4];
			}
			this.OnRgbaHdrChannelChanged(-1);
		}

		private void OnRgbaHdrChannelChanged(int channel)
		{
			this.m_Color[3] = (byte)Mathf.RoundToInt(Mathf.Clamp01(this.m_ColorHdr[3]) * 255f);
			if (channel != 3)
			{
				Color exposureAdjustedColor = this.exposureAdjustedColor;
				float maxColorComponent = exposureAdjustedColor.maxColorComponent;
				if (maxColorComponent == 0f || (maxColorComponent <= 1f && maxColorComponent > 0.003921569f))
				{
					this.m_ExposureValue = 0f;
					this.m_Color[0] = (byte)Mathf.RoundToInt(exposureAdjustedColor.r * 255f);
					this.m_Color[1] = (byte)Mathf.RoundToInt(exposureAdjustedColor.g * 255f);
					this.m_Color[2] = (byte)Mathf.RoundToInt(exposureAdjustedColor.b * 255f);
				}
				else
				{
					float num = 191f / maxColorComponent;
					this.m_ExposureValue = Mathf.Log(255f / num) / Mathf.Log(2f);
					this.m_Color[0] = Math.Min(191, (byte)Mathf.CeilToInt(num * this.m_ColorHdr[0]));
					this.m_Color[1] = Math.Min(191, (byte)Mathf.CeilToInt(num * this.m_ColorHdr[1]));
					this.m_Color[2] = Math.Min(191, (byte)Mathf.CeilToInt(num * this.m_ColorHdr[2]));
				}
				Color.RGBToHSV(this.color, out this.m_Hsv[0], out this.m_Hsv[1], out this.m_Hsv[2]);
			}
		}
	}
}
