using System;

namespace UnityEngine.Experimental.UIElements
{
	public abstract class BaseTextElement : VisualElement
	{
		[SerializeField]
		private string m_Text;

		public virtual string text
		{
			get
			{
				return this.m_Text ?? string.Empty;
			}
			set
			{
				if (!(this.m_Text == value))
				{
					this.m_Text = value;
					base.Dirty(ChangeType.Layout);
					if (!string.IsNullOrEmpty(base.persistenceKey))
					{
						base.SavePersistentData();
					}
				}
			}
		}

		public override void DoRepaint()
		{
			IStylePainter stylePainter = base.elementPanel.stylePainter;
			stylePainter.DrawBackground(this);
			stylePainter.DrawBorder(this);
			stylePainter.DrawText(this);
		}

		protected internal override Vector2 DoMeasure(float width, VisualElement.MeasureMode widthMode, float height, VisualElement.MeasureMode heightMode)
		{
			float num = float.NaN;
			float num2 = float.NaN;
			Font font = base.style.font;
			Vector2 result;
			if (this.text == null || font == null)
			{
				result = new Vector2(num, num2);
			}
			else
			{
				IStylePainter stylePainter = base.elementPanel.stylePainter;
				if (widthMode == VisualElement.MeasureMode.Exactly)
				{
					num = width;
				}
				else
				{
					TextStylePainterParameters defaultTextParameters = stylePainter.GetDefaultTextParameters(this);
					defaultTextParameters.text = this.text;
					defaultTextParameters.font = font;
					defaultTextParameters.wordWrapWidth = 0f;
					defaultTextParameters.wordWrap = false;
					defaultTextParameters.richText = true;
					num = stylePainter.ComputeTextWidth(defaultTextParameters);
					if (widthMode == VisualElement.MeasureMode.AtMost)
					{
						num = Mathf.Min(num, width);
					}
				}
				if (heightMode == VisualElement.MeasureMode.Exactly)
				{
					num2 = height;
				}
				else
				{
					TextStylePainterParameters defaultTextParameters2 = stylePainter.GetDefaultTextParameters(this);
					defaultTextParameters2.text = this.text;
					defaultTextParameters2.font = font;
					defaultTextParameters2.wordWrapWidth = num;
					defaultTextParameters2.richText = true;
					num2 = stylePainter.ComputeTextHeight(defaultTextParameters2);
					if (heightMode == VisualElement.MeasureMode.AtMost)
					{
						num2 = Mathf.Min(num2, height);
					}
				}
				result = new Vector2(num, num2);
			}
			return result;
		}
	}
}
