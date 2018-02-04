using System;
using UnityEngine.Experimental.UIElements.StyleSheets;

namespace UnityEngine.Experimental.UIElements
{
	public class Image : VisualElement
	{
		private StyleValue<int> m_ScaleMode;

		private StyleValue<Texture> m_Image;

		private Rect m_UV;

		public StyleValue<Texture> image
		{
			get
			{
				return this.m_Image;
			}
			set
			{
				if (StyleValueUtils.ApplyAndCompare<Texture>(ref this.m_Image, value))
				{
					base.Dirty(ChangeType.Layout | ChangeType.Repaint);
					if (this.m_Image.value == null)
					{
						this.m_UV = new Rect(0f, 0f, 1f, 1f);
					}
				}
			}
		}

		public Rect sourceRect
		{
			get
			{
				return this.GetSourceRect();
			}
			set
			{
				this.CalculateUV(value);
			}
		}

		public Rect uv
		{
			get
			{
				return this.m_UV;
			}
			set
			{
				this.m_UV = value;
			}
		}

		public StyleValue<ScaleMode> scaleMode
		{
			get
			{
				return new StyleValue<ScaleMode>((ScaleMode)this.m_ScaleMode.value, this.m_ScaleMode.specificity);
			}
			set
			{
				if (StyleValueUtils.ApplyAndCompare(ref this.m_ScaleMode, new StyleValue<int>((int)value.value, value.specificity)))
				{
					base.Dirty(ChangeType.Layout);
				}
			}
		}

		public Image()
		{
			this.scaleMode = ScaleMode.ScaleAndCrop;
			this.m_UV = new Rect(0f, 0f, 1f, 1f);
		}

		protected internal override Vector2 DoMeasure(float width, VisualElement.MeasureMode widthMode, float height, VisualElement.MeasureMode heightMode)
		{
			float num = float.NaN;
			float num2 = float.NaN;
			Texture specifiedValueOrDefault = this.image.GetSpecifiedValueOrDefault(null);
			Vector2 result;
			if (specifiedValueOrDefault == null)
			{
				result = new Vector2(num, num2);
			}
			else
			{
				Rect sourceRect = this.sourceRect;
				bool flag = sourceRect != Rect.zero;
				num = ((!flag) ? ((float)specifiedValueOrDefault.width) : sourceRect.width);
				num2 = ((!flag) ? ((float)specifiedValueOrDefault.height) : sourceRect.height);
				if (widthMode == VisualElement.MeasureMode.AtMost)
				{
					num = Mathf.Min(num, width);
				}
				if (heightMode == VisualElement.MeasureMode.AtMost)
				{
					num2 = Mathf.Min(num2, height);
				}
				result = new Vector2(num, num2);
			}
			return result;
		}

		internal override void DoRepaint(IStylePainter painter)
		{
			base.DoRepaint(painter);
			Texture specifiedValueOrDefault = this.image.GetSpecifiedValueOrDefault(null);
			if (specifiedValueOrDefault == null)
			{
				Debug.LogWarning("null texture passed to GUI.DrawTexture");
			}
			else
			{
				TextureStylePainterParameters painterParams = new TextureStylePainterParameters
				{
					rect = base.contentRect,
					uv = this.uv,
					texture = specifiedValueOrDefault,
					color = GUI.color,
					scaleMode = this.scaleMode
				};
				painter.DrawTexture(painterParams);
			}
		}

		protected override void OnStyleResolved(ICustomStyle elementStyle)
		{
			base.OnStyleResolved(elementStyle);
			elementStyle.ApplyCustomProperty<Texture>("image", ref this.m_Image);
			elementStyle.ApplyCustomProperty("image-size", ref this.m_ScaleMode);
		}

		private void CalculateUV(Rect srcRect)
		{
			this.m_UV = new Rect(0f, 0f, 1f, 1f);
			Texture specifiedValueOrDefault = this.image.GetSpecifiedValueOrDefault(null);
			if (specifiedValueOrDefault != null)
			{
				int width = specifiedValueOrDefault.width;
				int height = specifiedValueOrDefault.height;
				this.m_UV.x = srcRect.x / (float)width;
				this.m_UV.width = srcRect.width / (float)width;
				this.m_UV.height = srcRect.height / (float)height;
				this.m_UV.y = 1f - this.m_UV.height - srcRect.y / (float)height;
			}
		}

		private Rect GetSourceRect()
		{
			Rect zero = Rect.zero;
			Texture specifiedValueOrDefault = this.image.GetSpecifiedValueOrDefault(null);
			if (specifiedValueOrDefault != null)
			{
				int width = specifiedValueOrDefault.width;
				int height = specifiedValueOrDefault.height;
				zero.x = this.uv.x * (float)width;
				zero.width = this.uv.width * (float)width;
				zero.y = (1f - this.uv.y - this.uv.height) * (float)height;
				zero.height = this.uv.height * (float)height;
			}
			return zero;
		}
	}
}
