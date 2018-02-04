using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine
{
	[VisibleToOtherModules(new string[]
	{
		"UnityEngine.UIElementsModule"
	}), UsedByNativeCode]
	[StructLayout(LayoutKind.Sequential)]
	internal sealed class StylePainter : IStylePainter
	{
		[NonSerialized]
		internal IntPtr m_Ptr;

		private Color m_OpacityColor = Color.white;

		public Matrix4x4 currentTransform
		{
			get;
			set;
		}

		public Vector2 mousePosition
		{
			get;
			set;
		}

		public Rect currentWorldClip
		{
			get;
			set;
		}

		public Event repaintEvent
		{
			get;
			set;
		}

		public float opacity
		{
			get
			{
				return this.m_OpacityColor.a;
			}
			set
			{
				this.m_OpacityColor.a = value;
			}
		}

		public StylePainter()
		{
			this.Init();
		}

		public StylePainter(Vector2 pos) : this()
		{
			this.mousePosition = pos;
		}

		public void DrawRect(RectStylePainterParameters painterParams)
		{
			Rect rect = painterParams.rect;
			Color color = painterParams.color;
			Vector4 widths = painterParams.border.GetWidths();
			Vector4 radiuses = painterParams.border.GetRadiuses();
			this.DrawRect_Internal(rect, color * this.m_OpacityColor, widths, radiuses);
		}

		public void DrawTexture(TextureStylePainterParameters painterParams)
		{
			Rect rect = painterParams.rect;
			Rect sourceRect = (!(painterParams.uv != Rect.zero)) ? new Rect(0f, 0f, 1f, 1f) : painterParams.uv;
			Texture texture = painterParams.texture;
			Color color = painterParams.color;
			ScaleMode scaleMode = painterParams.scaleMode;
			int sliceLeft = painterParams.sliceLeft;
			int sliceTop = painterParams.sliceTop;
			int sliceRight = painterParams.sliceRight;
			int sliceBottom = painterParams.sliceBottom;
			bool usePremultiplyAlpha = painterParams.usePremultiplyAlpha;
			Rect screenRect = rect;
			float num = (float)texture.width * sourceRect.width / ((float)texture.height * sourceRect.height);
			float num2 = rect.width / rect.height;
			if (scaleMode != ScaleMode.StretchToFill)
			{
				if (scaleMode != ScaleMode.ScaleAndCrop)
				{
					if (scaleMode == ScaleMode.ScaleToFit)
					{
						if (num2 > num)
						{
							float num3 = num / num2;
							screenRect = new Rect(rect.xMin + rect.width * (1f - num3) * 0.5f, rect.yMin, num3 * rect.width, rect.height);
						}
						else
						{
							float num4 = num2 / num;
							screenRect = new Rect(rect.xMin, rect.yMin + rect.height * (1f - num4) * 0.5f, rect.width, num4 * rect.height);
						}
					}
				}
				else if (num2 > num)
				{
					float num5 = sourceRect.height * (num / num2);
					float num6 = (sourceRect.height - num5) * 0.5f;
					sourceRect = new Rect(sourceRect.x, sourceRect.y + num6, sourceRect.width, num5);
				}
				else
				{
					float num7 = sourceRect.width * (num2 / num);
					float num8 = (sourceRect.width - num7) * 0.5f;
					sourceRect = new Rect(sourceRect.x + num8, sourceRect.y, num7, sourceRect.height);
				}
			}
			Vector4 widths = painterParams.border.GetWidths();
			Vector4 radiuses = painterParams.border.GetRadiuses();
			this.DrawTexture_Internal(screenRect, texture, sourceRect, color * this.m_OpacityColor, widths, radiuses, sliceLeft, sliceTop, sliceRight, sliceBottom, usePremultiplyAlpha);
		}

		public void DrawText(TextStylePainterParameters painterParams)
		{
			Rect rect = painterParams.rect;
			string text = painterParams.text;
			Font font = painterParams.font;
			int fontSize = painterParams.fontSize;
			FontStyle fontStyle = painterParams.fontStyle;
			Color fontColor = painterParams.fontColor;
			TextAnchor anchor = painterParams.anchor;
			bool wordWrap = painterParams.wordWrap;
			float wordWrapWidth = painterParams.wordWrapWidth;
			bool richText = painterParams.richText;
			TextClipping clipping = painterParams.clipping;
			this.DrawText_Internal(rect, text, font, fontSize, fontStyle, fontColor * this.m_OpacityColor, anchor, wordWrap, wordWrapWidth, richText, clipping);
		}

		public Vector2 GetCursorPosition(CursorPositionStylePainterParameters painterParams)
		{
			Font font = painterParams.font;
			Vector2 result;
			if (font == null)
			{
				Debug.LogError("StylePainter: Can't process a null font.");
				result = Vector2.zero;
			}
			else
			{
				string text = painterParams.text;
				int fontSize = painterParams.fontSize;
				FontStyle fontStyle = painterParams.fontStyle;
				TextAnchor anchor = painterParams.anchor;
				float wordWrapWidth = painterParams.wordWrapWidth;
				bool richText = painterParams.richText;
				Rect rect = painterParams.rect;
				int cursorIndex = painterParams.cursorIndex;
				result = this.GetCursorPosition_Internal(text, font, fontSize, fontStyle, anchor, wordWrapWidth, richText, rect, cursorIndex);
			}
			return result;
		}

		public float ComputeTextWidth(TextStylePainterParameters painterParams)
		{
			string text = painterParams.text;
			float wordWrapWidth = painterParams.wordWrapWidth;
			bool wordWrap = painterParams.wordWrap;
			Font font = painterParams.font;
			int fontSize = painterParams.fontSize;
			FontStyle fontStyle = painterParams.fontStyle;
			TextAnchor anchor = painterParams.anchor;
			bool richText = painterParams.richText;
			return this.ComputeTextWidth_Internal(text, wordWrapWidth, wordWrap, font, fontSize, fontStyle, anchor, richText);
		}

		public float ComputeTextHeight(TextStylePainterParameters painterParams)
		{
			string text = painterParams.text;
			float wordWrapWidth = painterParams.wordWrapWidth;
			bool wordWrap = painterParams.wordWrap;
			Font font = painterParams.font;
			int fontSize = painterParams.fontSize;
			FontStyle fontStyle = painterParams.fontStyle;
			TextAnchor anchor = painterParams.anchor;
			bool richText = painterParams.richText;
			return this.ComputeTextHeight_Internal(text, wordWrapWidth, wordWrap, font, fontSize, fontStyle, anchor, richText);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Init();

		internal void DrawRect_Internal(Rect screenRect, Color color, Vector4 borderWidths, Vector4 borderRadiuses)
		{
			StylePainter.INTERNAL_CALL_DrawRect_Internal(this, ref screenRect, ref color, ref borderWidths, ref borderRadiuses);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_DrawRect_Internal(StylePainter self, ref Rect screenRect, ref Color color, ref Vector4 borderWidths, ref Vector4 borderRadiuses);

		internal void DrawTexture_Internal(Rect screenRect, Texture texture, Rect sourceRect, Color color, Vector4 borderWidths, Vector4 borderRadiuses, int leftBorder, int topBorder, int rightBorder, int bottomBorder, bool usePremultiplyAlpha)
		{
			StylePainter.INTERNAL_CALL_DrawTexture_Internal(this, ref screenRect, texture, ref sourceRect, ref color, ref borderWidths, ref borderRadiuses, leftBorder, topBorder, rightBorder, bottomBorder, usePremultiplyAlpha);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_DrawTexture_Internal(StylePainter self, ref Rect screenRect, Texture texture, ref Rect sourceRect, ref Color color, ref Vector4 borderWidths, ref Vector4 borderRadiuses, int leftBorder, int topBorder, int rightBorder, int bottomBorder, bool usePremultiplyAlpha);

		internal void DrawText_Internal(Rect screenRect, string text, Font font, int fontSize, FontStyle fontStyle, Color fontColor, TextAnchor anchor, bool wordWrap, float wordWrapWidth, bool richText, TextClipping textClipping)
		{
			StylePainter.INTERNAL_CALL_DrawText_Internal(this, ref screenRect, text, font, fontSize, fontStyle, ref fontColor, anchor, wordWrap, wordWrapWidth, richText, textClipping);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_DrawText_Internal(StylePainter self, ref Rect screenRect, string text, Font font, int fontSize, FontStyle fontStyle, ref Color fontColor, TextAnchor anchor, bool wordWrap, float wordWrapWidth, bool richText, TextClipping textClipping);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern float ComputeTextWidth_Internal(string text, float width, bool wordWrap, Font font, int fontSize, FontStyle fontStyle, TextAnchor anchor, bool richText);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern float ComputeTextHeight_Internal(string text, float width, bool wordWrap, Font font, int fontSize, FontStyle fontStyle, TextAnchor anchor, bool richText);

		public Vector2 GetCursorPosition_Internal(string text, Font font, int fontSize, FontStyle fontStyle, TextAnchor anchor, float wordWrapWidth, bool richText, Rect screenRect, int cursorPosition)
		{
			Vector2 result;
			StylePainter.INTERNAL_CALL_GetCursorPosition_Internal(this, text, font, fontSize, fontStyle, anchor, wordWrapWidth, richText, ref screenRect, cursorPosition, out result);
			return result;
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_GetCursorPosition_Internal(StylePainter self, string text, Font font, int fontSize, FontStyle fontStyle, TextAnchor anchor, float wordWrapWidth, bool richText, ref Rect screenRect, int cursorPosition, out Vector2 value);
	}
}
