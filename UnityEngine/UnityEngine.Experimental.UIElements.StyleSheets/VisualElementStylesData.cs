using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Experimental.UIElements.StyleEnums;
using UnityEngine.StyleSheets;

namespace UnityEngine.Experimental.UIElements.StyleSheets
{
	internal class VisualElementStylesData : ICustomStyle
	{
		public static VisualElementStylesData none = new VisualElementStylesData(true);

		internal readonly bool isShared;

		private Dictionary<string, CustomProperty> m_CustomProperties;

		internal StyleValue<float> width;

		internal StyleValue<float> height;

		internal StyleValue<float> maxWidth;

		internal StyleValue<float> maxHeight;

		internal StyleValue<float> minWidth;

		internal StyleValue<float> minHeight;

		internal StyleValue<float> flex;

		internal StyleValue<int> overflow;

		internal StyleValue<float> positionLeft;

		internal StyleValue<float> positionTop;

		internal StyleValue<float> positionRight;

		internal StyleValue<float> positionBottom;

		internal StyleValue<float> marginLeft;

		internal StyleValue<float> marginTop;

		internal StyleValue<float> marginRight;

		internal StyleValue<float> marginBottom;

		internal StyleValue<float> borderLeft;

		internal StyleValue<float> borderTop;

		internal StyleValue<float> borderRight;

		internal StyleValue<float> borderBottom;

		internal StyleValue<float> paddingLeft;

		internal StyleValue<float> paddingTop;

		internal StyleValue<float> paddingRight;

		internal StyleValue<float> paddingBottom;

		internal StyleValue<int> positionType;

		internal StyleValue<int> alignSelf;

		internal StyleValue<int> textAlignment;

		internal StyleValue<int> fontStyle;

		internal StyleValue<int> textClipping;

		internal StyleValue<Font> font;

		internal StyleValue<int> fontSize;

		internal StyleValue<bool> wordWrap;

		internal StyleValue<Color> textColor;

		internal StyleValue<int> flexDirection;

		internal StyleValue<Color> backgroundColor;

		internal StyleValue<Color> borderColor;

		internal StyleValue<Texture2D> backgroundImage;

		internal StyleValue<int> backgroundSize;

		internal StyleValue<int> alignItems;

		internal StyleValue<int> alignContent;

		internal StyleValue<int> justifyContent;

		internal StyleValue<int> flexWrap;

		internal StyleValue<float> borderLeftWidth;

		internal StyleValue<float> borderTopWidth;

		internal StyleValue<float> borderRightWidth;

		internal StyleValue<float> borderBottomWidth;

		internal StyleValue<float> borderTopLeftRadius;

		internal StyleValue<float> borderTopRightRadius;

		internal StyleValue<float> borderBottomRightRadius;

		internal StyleValue<float> borderBottomLeftRadius;

		internal StyleValue<int> sliceLeft;

		internal StyleValue<int> sliceTop;

		internal StyleValue<int> sliceRight;

		internal StyleValue<int> sliceBottom;

		internal StyleValue<float> opacity;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<int> <>f__mg$cache0;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<int> <>f__mg$cache1;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<int> <>f__mg$cache2;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<Texture2D> <>f__mg$cache3;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<float> <>f__mg$cache4;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<float> <>f__mg$cache5;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<float> <>f__mg$cache6;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<float> <>f__mg$cache7;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<float> <>f__mg$cache8;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<Font> <>f__mg$cache9;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<int> <>f__mg$cacheA;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<int> <>f__mg$cacheB;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<int> <>f__mg$cacheC;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<int> <>f__mg$cacheD;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<float> <>f__mg$cacheE;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<int> <>f__mg$cacheF;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<float> <>f__mg$cache10;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<float> <>f__mg$cache11;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<float> <>f__mg$cache12;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<float> <>f__mg$cache13;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<float> <>f__mg$cache14;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<float> <>f__mg$cache15;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<float> <>f__mg$cache16;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<float> <>f__mg$cache17;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<int> <>f__mg$cache18;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<float> <>f__mg$cache19;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<float> <>f__mg$cache1A;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<float> <>f__mg$cache1B;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<float> <>f__mg$cache1C;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<int> <>f__mg$cache1D;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<float> <>f__mg$cache1E;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<float> <>f__mg$cache1F;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<float> <>f__mg$cache20;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<float> <>f__mg$cache21;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<int> <>f__mg$cache22;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<int> <>f__mg$cache23;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<Color> <>f__mg$cache24;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<float> <>f__mg$cache25;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<bool> <>f__mg$cache26;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<Color> <>f__mg$cache27;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<int> <>f__mg$cache28;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<Color> <>f__mg$cache29;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<float> <>f__mg$cache2A;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<float> <>f__mg$cache2B;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<float> <>f__mg$cache2C;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<float> <>f__mg$cache2D;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<float> <>f__mg$cache2E;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<float> <>f__mg$cache2F;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<float> <>f__mg$cache30;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<float> <>f__mg$cache31;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<int> <>f__mg$cache32;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<int> <>f__mg$cache33;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<int> <>f__mg$cache34;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<int> <>f__mg$cache35;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<float> <>f__mg$cache36;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<float> <>f__mg$cache37;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<float> <>f__mg$cache38;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<float> <>f__mg$cache39;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<float> <>f__mg$cache3A;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<float> <>f__mg$cache3B;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<int> <>f__mg$cache3C;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<bool> <>f__mg$cache3D;

		[CompilerGenerated]
		private static HandlesApplicatorFunction<Color> <>f__mg$cache3E;

		public VisualElementStylesData(bool isShared)
		{
			this.isShared = isShared;
		}

		public void Apply(VisualElementStylesData other, StylePropertyApplyMode mode)
		{
			this.m_CustomProperties = other.m_CustomProperties;
			this.width.Apply(other.width, mode);
			this.height.Apply(other.height, mode);
			this.maxWidth.Apply(other.maxWidth, mode);
			this.maxHeight.Apply(other.maxHeight, mode);
			this.minWidth.Apply(other.minWidth, mode);
			this.minHeight.Apply(other.minHeight, mode);
			this.flex.Apply(other.flex, mode);
			this.overflow.Apply(other.overflow, mode);
			this.positionLeft.Apply(other.positionLeft, mode);
			this.positionTop.Apply(other.positionTop, mode);
			this.positionRight.Apply(other.positionRight, mode);
			this.positionBottom.Apply(other.positionBottom, mode);
			this.marginLeft.Apply(other.marginLeft, mode);
			this.marginTop.Apply(other.marginTop, mode);
			this.marginRight.Apply(other.marginRight, mode);
			this.marginBottom.Apply(other.marginBottom, mode);
			this.borderLeft.Apply(other.borderLeft, mode);
			this.borderTop.Apply(other.borderTop, mode);
			this.borderRight.Apply(other.borderRight, mode);
			this.borderBottom.Apply(other.borderBottom, mode);
			this.paddingLeft.Apply(other.paddingLeft, mode);
			this.paddingTop.Apply(other.paddingTop, mode);
			this.paddingRight.Apply(other.paddingRight, mode);
			this.paddingBottom.Apply(other.paddingBottom, mode);
			this.positionType.Apply(other.positionType, mode);
			this.alignSelf.Apply(other.alignSelf, mode);
			this.textAlignment.Apply(other.textAlignment, mode);
			this.fontStyle.Apply(other.fontStyle, mode);
			this.textClipping.Apply(other.textClipping, mode);
			this.fontSize.Apply(other.fontSize, mode);
			this.font.Apply(other.font, mode);
			this.wordWrap.Apply(other.wordWrap, mode);
			this.textColor.Apply(other.textColor, mode);
			this.flexDirection.Apply(other.flexDirection, mode);
			this.backgroundColor.Apply(other.backgroundColor, mode);
			this.borderColor.Apply(other.borderColor, mode);
			this.backgroundImage.Apply(other.backgroundImage, mode);
			this.backgroundSize.Apply(other.backgroundSize, mode);
			this.alignItems.Apply(other.alignItems, mode);
			this.alignContent.Apply(other.alignContent, mode);
			this.justifyContent.Apply(other.justifyContent, mode);
			this.flexWrap.Apply(other.flexWrap, mode);
			this.borderLeftWidth.Apply(other.borderLeftWidth, mode);
			this.borderTopWidth.Apply(other.borderTopWidth, mode);
			this.borderRightWidth.Apply(other.borderRightWidth, mode);
			this.borderBottomWidth.Apply(other.borderBottomWidth, mode);
			this.borderTopLeftRadius.Apply(other.borderTopLeftRadius, mode);
			this.borderTopRightRadius.Apply(other.borderTopRightRadius, mode);
			this.borderBottomRightRadius.Apply(other.borderBottomRightRadius, mode);
			this.borderBottomLeftRadius.Apply(other.borderBottomLeftRadius, mode);
			this.sliceLeft.Apply(other.sliceLeft, mode);
			this.sliceTop.Apply(other.sliceTop, mode);
			this.sliceRight.Apply(other.sliceRight, mode);
			this.sliceBottom.Apply(other.sliceBottom, mode);
			this.opacity.Apply(other.opacity, mode);
		}

		public void WriteToGUIStyle(GUIStyle style)
		{
			style.alignment = (TextAnchor)this.textAlignment.GetSpecifiedValueOrDefault((int)style.alignment);
			style.wordWrap = this.wordWrap.GetSpecifiedValueOrDefault(style.wordWrap);
			style.clipping = (TextClipping)this.textClipping.GetSpecifiedValueOrDefault((int)style.clipping);
			if (this.font.value != null)
			{
				style.font = this.font.value;
			}
			style.fontSize = this.fontSize.GetSpecifiedValueOrDefault(style.fontSize);
			style.fontStyle = (FontStyle)this.fontStyle.GetSpecifiedValueOrDefault((int)style.fontStyle);
			this.AssignRect(style.margin, ref this.marginLeft, ref this.marginTop, ref this.marginRight, ref this.marginBottom);
			this.AssignRect(style.padding, ref this.paddingLeft, ref this.paddingTop, ref this.paddingRight, ref this.paddingBottom);
			this.AssignRect(style.border, ref this.sliceLeft, ref this.sliceTop, ref this.sliceRight, ref this.sliceBottom);
			this.AssignState(style.normal);
			this.AssignState(style.focused);
			this.AssignState(style.hover);
			this.AssignState(style.active);
			this.AssignState(style.onNormal);
			this.AssignState(style.onFocused);
			this.AssignState(style.onHover);
			this.AssignState(style.onActive);
		}

		private void AssignState(GUIStyleState state)
		{
			state.textColor = this.textColor.GetSpecifiedValueOrDefault(state.textColor);
			if (this.backgroundImage.value != null)
			{
				state.background = this.backgroundImage.value;
				if (state.scaledBackgrounds == null || state.scaledBackgrounds.Length < 1 || state.scaledBackgrounds[0] != this.backgroundImage.value)
				{
					state.scaledBackgrounds = new Texture2D[]
					{
						this.backgroundImage.value
					};
				}
			}
		}

		private void AssignRect(RectOffset rect, ref StyleValue<int> left, ref StyleValue<int> top, ref StyleValue<int> right, ref StyleValue<int> bottom)
		{
			rect.left = left.GetSpecifiedValueOrDefault(rect.left);
			rect.top = top.GetSpecifiedValueOrDefault(rect.top);
			rect.right = right.GetSpecifiedValueOrDefault(rect.right);
			rect.bottom = bottom.GetSpecifiedValueOrDefault(rect.bottom);
		}

		private void AssignRect(RectOffset rect, ref StyleValue<float> left, ref StyleValue<float> top, ref StyleValue<float> right, ref StyleValue<float> bottom)
		{
			rect.left = (int)left.GetSpecifiedValueOrDefault((float)rect.left);
			rect.top = (int)top.GetSpecifiedValueOrDefault((float)rect.top);
			rect.right = (int)right.GetSpecifiedValueOrDefault((float)rect.right);
			rect.bottom = (int)bottom.GetSpecifiedValueOrDefault((float)rect.bottom);
		}

		internal void ApplyRule(StyleSheet registry, int specificity, StyleRule rule, StylePropertyID[] propertyIDs)
		{
			int i = 0;
			while (i < rule.properties.Length)
			{
				StyleProperty styleProperty = rule.properties[i];
				StylePropertyID stylePropertyID = propertyIDs[i];
				StyleValueHandle[] values = styleProperty.values;
				switch (stylePropertyID)
				{
				case StylePropertyID.MarginLeft:
				{
					StyleValueHandle[] arg_43B_1 = values;
					if (VisualElementStylesData.<>f__mg$cache10 == null)
					{
						VisualElementStylesData.<>f__mg$cache10 = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
					}
					registry.Apply(arg_43B_1, specificity, ref this.marginLeft, VisualElementStylesData.<>f__mg$cache10);
					break;
				}
				case StylePropertyID.MarginTop:
				{
					StyleValueHandle[] arg_46B_1 = values;
					if (VisualElementStylesData.<>f__mg$cache11 == null)
					{
						VisualElementStylesData.<>f__mg$cache11 = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
					}
					registry.Apply(arg_46B_1, specificity, ref this.marginTop, VisualElementStylesData.<>f__mg$cache11);
					break;
				}
				case StylePropertyID.MarginRight:
				{
					StyleValueHandle[] arg_49B_1 = values;
					if (VisualElementStylesData.<>f__mg$cache12 == null)
					{
						VisualElementStylesData.<>f__mg$cache12 = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
					}
					registry.Apply(arg_49B_1, specificity, ref this.marginRight, VisualElementStylesData.<>f__mg$cache12);
					break;
				}
				case StylePropertyID.MarginBottom:
				{
					StyleValueHandle[] arg_4CB_1 = values;
					if (VisualElementStylesData.<>f__mg$cache13 == null)
					{
						VisualElementStylesData.<>f__mg$cache13 = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
					}
					registry.Apply(arg_4CB_1, specificity, ref this.marginBottom, VisualElementStylesData.<>f__mg$cache13);
					break;
				}
				case StylePropertyID.PaddingLeft:
				{
					StyleValueHandle[] arg_5EB_1 = values;
					if (VisualElementStylesData.<>f__mg$cache19 == null)
					{
						VisualElementStylesData.<>f__mg$cache19 = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
					}
					registry.Apply(arg_5EB_1, specificity, ref this.paddingLeft, VisualElementStylesData.<>f__mg$cache19);
					break;
				}
				case StylePropertyID.PaddingTop:
				{
					StyleValueHandle[] arg_61B_1 = values;
					if (VisualElementStylesData.<>f__mg$cache1A == null)
					{
						VisualElementStylesData.<>f__mg$cache1A = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
					}
					registry.Apply(arg_61B_1, specificity, ref this.paddingTop, VisualElementStylesData.<>f__mg$cache1A);
					break;
				}
				case StylePropertyID.PaddingRight:
				{
					StyleValueHandle[] arg_64B_1 = values;
					if (VisualElementStylesData.<>f__mg$cache1B == null)
					{
						VisualElementStylesData.<>f__mg$cache1B = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
					}
					registry.Apply(arg_64B_1, specificity, ref this.paddingRight, VisualElementStylesData.<>f__mg$cache1B);
					break;
				}
				case StylePropertyID.PaddingBottom:
				{
					StyleValueHandle[] arg_67B_1 = values;
					if (VisualElementStylesData.<>f__mg$cache1C == null)
					{
						VisualElementStylesData.<>f__mg$cache1C = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
					}
					registry.Apply(arg_67B_1, specificity, ref this.paddingBottom, VisualElementStylesData.<>f__mg$cache1C);
					break;
				}
				case StylePropertyID.BorderLeft:
				{
					StyleValueHandle[] arg_1FB_1 = values;
					if (VisualElementStylesData.<>f__mg$cache4 == null)
					{
						VisualElementStylesData.<>f__mg$cache4 = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
					}
					registry.Apply(arg_1FB_1, specificity, ref this.borderLeft, VisualElementStylesData.<>f__mg$cache4);
					break;
				}
				case StylePropertyID.BorderTop:
				{
					StyleValueHandle[] arg_22B_1 = values;
					if (VisualElementStylesData.<>f__mg$cache5 == null)
					{
						VisualElementStylesData.<>f__mg$cache5 = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
					}
					registry.Apply(arg_22B_1, specificity, ref this.borderTop, VisualElementStylesData.<>f__mg$cache5);
					break;
				}
				case StylePropertyID.BorderRight:
				{
					StyleValueHandle[] arg_25B_1 = values;
					if (VisualElementStylesData.<>f__mg$cache6 == null)
					{
						VisualElementStylesData.<>f__mg$cache6 = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
					}
					registry.Apply(arg_25B_1, specificity, ref this.borderRight, VisualElementStylesData.<>f__mg$cache6);
					break;
				}
				case StylePropertyID.BorderBottom:
				{
					StyleValueHandle[] arg_28B_1 = values;
					if (VisualElementStylesData.<>f__mg$cache7 == null)
					{
						VisualElementStylesData.<>f__mg$cache7 = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
					}
					registry.Apply(arg_28B_1, specificity, ref this.borderBottom, VisualElementStylesData.<>f__mg$cache7);
					break;
				}
				case StylePropertyID.PositionType:
				{
					StyleValueHandle[] arg_6AB_1 = values;
					if (VisualElementStylesData.<>f__mg$cache1D == null)
					{
						VisualElementStylesData.<>f__mg$cache1D = new HandlesApplicatorFunction<int>(StyleSheetApplicator.ApplyEnum<PositionType>);
					}
					registry.Apply(arg_6AB_1, specificity, ref this.positionType, VisualElementStylesData.<>f__mg$cache1D);
					break;
				}
				case StylePropertyID.PositionLeft:
				{
					StyleValueHandle[] arg_73B_1 = values;
					if (VisualElementStylesData.<>f__mg$cache20 == null)
					{
						VisualElementStylesData.<>f__mg$cache20 = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
					}
					registry.Apply(arg_73B_1, specificity, ref this.positionLeft, VisualElementStylesData.<>f__mg$cache20);
					break;
				}
				case StylePropertyID.PositionTop:
				{
					StyleValueHandle[] arg_6DB_1 = values;
					if (VisualElementStylesData.<>f__mg$cache1E == null)
					{
						VisualElementStylesData.<>f__mg$cache1E = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
					}
					registry.Apply(arg_6DB_1, specificity, ref this.positionTop, VisualElementStylesData.<>f__mg$cache1E);
					break;
				}
				case StylePropertyID.PositionRight:
				{
					StyleValueHandle[] arg_76B_1 = values;
					if (VisualElementStylesData.<>f__mg$cache21 == null)
					{
						VisualElementStylesData.<>f__mg$cache21 = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
					}
					registry.Apply(arg_76B_1, specificity, ref this.positionRight, VisualElementStylesData.<>f__mg$cache21);
					break;
				}
				case StylePropertyID.PositionBottom:
				{
					StyleValueHandle[] arg_70B_1 = values;
					if (VisualElementStylesData.<>f__mg$cache1F == null)
					{
						VisualElementStylesData.<>f__mg$cache1F = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
					}
					registry.Apply(arg_70B_1, specificity, ref this.positionBottom, VisualElementStylesData.<>f__mg$cache1F);
					break;
				}
				case StylePropertyID.Width:
				{
					StyleValueHandle[] arg_82B_1 = values;
					if (VisualElementStylesData.<>f__mg$cache25 == null)
					{
						VisualElementStylesData.<>f__mg$cache25 = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
					}
					registry.Apply(arg_82B_1, specificity, ref this.width, VisualElementStylesData.<>f__mg$cache25);
					break;
				}
				case StylePropertyID.Height:
				{
					StyleValueHandle[] arg_3DB_1 = values;
					if (VisualElementStylesData.<>f__mg$cacheE == null)
					{
						VisualElementStylesData.<>f__mg$cacheE = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
					}
					registry.Apply(arg_3DB_1, specificity, ref this.height, VisualElementStylesData.<>f__mg$cacheE);
					break;
				}
				case StylePropertyID.MinWidth:
				{
					StyleValueHandle[] arg_58B_1 = values;
					if (VisualElementStylesData.<>f__mg$cache17 == null)
					{
						VisualElementStylesData.<>f__mg$cache17 = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
					}
					registry.Apply(arg_58B_1, specificity, ref this.minWidth, VisualElementStylesData.<>f__mg$cache17);
					break;
				}
				case StylePropertyID.MinHeight:
				{
					StyleValueHandle[] arg_55B_1 = values;
					if (VisualElementStylesData.<>f__mg$cache16 == null)
					{
						VisualElementStylesData.<>f__mg$cache16 = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
					}
					registry.Apply(arg_55B_1, specificity, ref this.minHeight, VisualElementStylesData.<>f__mg$cache16);
					break;
				}
				case StylePropertyID.MaxWidth:
				{
					StyleValueHandle[] arg_52B_1 = values;
					if (VisualElementStylesData.<>f__mg$cache15 == null)
					{
						VisualElementStylesData.<>f__mg$cache15 = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
					}
					registry.Apply(arg_52B_1, specificity, ref this.maxWidth, VisualElementStylesData.<>f__mg$cache15);
					break;
				}
				case StylePropertyID.MaxHeight:
				{
					StyleValueHandle[] arg_4FB_1 = values;
					if (VisualElementStylesData.<>f__mg$cache14 == null)
					{
						VisualElementStylesData.<>f__mg$cache14 = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
					}
					registry.Apply(arg_4FB_1, specificity, ref this.maxHeight, VisualElementStylesData.<>f__mg$cache14);
					break;
				}
				case StylePropertyID.Flex:
				{
					StyleValueHandle[] arg_2BB_1 = values;
					if (VisualElementStylesData.<>f__mg$cache8 == null)
					{
						VisualElementStylesData.<>f__mg$cache8 = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
					}
					registry.Apply(arg_2BB_1, specificity, ref this.flex, VisualElementStylesData.<>f__mg$cache8);
					break;
				}
				case StylePropertyID.BorderLeftWidth:
				{
					StyleValueHandle[] arg_91B_1 = values;
					if (VisualElementStylesData.<>f__mg$cache2A == null)
					{
						VisualElementStylesData.<>f__mg$cache2A = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
					}
					registry.Apply(arg_91B_1, specificity, ref this.borderLeftWidth, VisualElementStylesData.<>f__mg$cache2A);
					break;
				}
				case StylePropertyID.BorderTopWidth:
				{
					StyleValueHandle[] arg_94B_1 = values;
					if (VisualElementStylesData.<>f__mg$cache2B == null)
					{
						VisualElementStylesData.<>f__mg$cache2B = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
					}
					registry.Apply(arg_94B_1, specificity, ref this.borderTopWidth, VisualElementStylesData.<>f__mg$cache2B);
					break;
				}
				case StylePropertyID.BorderRightWidth:
				{
					StyleValueHandle[] arg_97B_1 = values;
					if (VisualElementStylesData.<>f__mg$cache2C == null)
					{
						VisualElementStylesData.<>f__mg$cache2C = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
					}
					registry.Apply(arg_97B_1, specificity, ref this.borderRightWidth, VisualElementStylesData.<>f__mg$cache2C);
					break;
				}
				case StylePropertyID.BorderBottomWidth:
				{
					StyleValueHandle[] arg_9AB_1 = values;
					if (VisualElementStylesData.<>f__mg$cache2D == null)
					{
						VisualElementStylesData.<>f__mg$cache2D = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
					}
					registry.Apply(arg_9AB_1, specificity, ref this.borderBottomWidth, VisualElementStylesData.<>f__mg$cache2D);
					break;
				}
				case StylePropertyID.BorderTopLeftRadius:
				{
					StyleValueHandle[] arg_9DB_1 = values;
					if (VisualElementStylesData.<>f__mg$cache2E == null)
					{
						VisualElementStylesData.<>f__mg$cache2E = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
					}
					registry.Apply(arg_9DB_1, specificity, ref this.borderTopLeftRadius, VisualElementStylesData.<>f__mg$cache2E);
					break;
				}
				case StylePropertyID.BorderTopRightRadius:
				{
					StyleValueHandle[] arg_A0B_1 = values;
					if (VisualElementStylesData.<>f__mg$cache2F == null)
					{
						VisualElementStylesData.<>f__mg$cache2F = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
					}
					registry.Apply(arg_A0B_1, specificity, ref this.borderTopRightRadius, VisualElementStylesData.<>f__mg$cache2F);
					break;
				}
				case StylePropertyID.BorderBottomRightRadius:
				{
					StyleValueHandle[] arg_A3B_1 = values;
					if (VisualElementStylesData.<>f__mg$cache30 == null)
					{
						VisualElementStylesData.<>f__mg$cache30 = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
					}
					registry.Apply(arg_A3B_1, specificity, ref this.borderBottomRightRadius, VisualElementStylesData.<>f__mg$cache30);
					break;
				}
				case StylePropertyID.BorderBottomLeftRadius:
				{
					StyleValueHandle[] arg_A6B_1 = values;
					if (VisualElementStylesData.<>f__mg$cache31 == null)
					{
						VisualElementStylesData.<>f__mg$cache31 = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
					}
					registry.Apply(arg_A6B_1, specificity, ref this.borderBottomLeftRadius, VisualElementStylesData.<>f__mg$cache31);
					break;
				}
				case StylePropertyID.FlexDirection:
				{
					StyleValueHandle[] arg_37B_1 = values;
					if (VisualElementStylesData.<>f__mg$cacheC == null)
					{
						VisualElementStylesData.<>f__mg$cacheC = new HandlesApplicatorFunction<int>(StyleSheetApplicator.ApplyEnum<FlexDirection>);
					}
					registry.Apply(arg_37B_1, specificity, ref this.flexDirection, VisualElementStylesData.<>f__mg$cacheC);
					break;
				}
				case StylePropertyID.FlexWrap:
				{
					StyleValueHandle[] arg_3AB_1 = values;
					if (VisualElementStylesData.<>f__mg$cacheD == null)
					{
						VisualElementStylesData.<>f__mg$cacheD = new HandlesApplicatorFunction<int>(StyleSheetApplicator.ApplyEnum<Wrap>);
					}
					registry.Apply(arg_3AB_1, specificity, ref this.flexWrap, VisualElementStylesData.<>f__mg$cacheD);
					break;
				}
				case StylePropertyID.JustifyContent:
				{
					StyleValueHandle[] arg_40B_1 = values;
					if (VisualElementStylesData.<>f__mg$cacheF == null)
					{
						VisualElementStylesData.<>f__mg$cacheF = new HandlesApplicatorFunction<int>(StyleSheetApplicator.ApplyEnum<Justify>);
					}
					registry.Apply(arg_40B_1, specificity, ref this.justifyContent, VisualElementStylesData.<>f__mg$cacheF);
					break;
				}
				case StylePropertyID.AlignContent:
				{
					StyleValueHandle[] arg_13B_1 = values;
					if (VisualElementStylesData.<>f__mg$cache0 == null)
					{
						VisualElementStylesData.<>f__mg$cache0 = new HandlesApplicatorFunction<int>(StyleSheetApplicator.ApplyEnum<Align>);
					}
					registry.Apply(arg_13B_1, specificity, ref this.alignContent, VisualElementStylesData.<>f__mg$cache0);
					break;
				}
				case StylePropertyID.AlignSelf:
				{
					StyleValueHandle[] arg_19B_1 = values;
					if (VisualElementStylesData.<>f__mg$cache2 == null)
					{
						VisualElementStylesData.<>f__mg$cache2 = new HandlesApplicatorFunction<int>(StyleSheetApplicator.ApplyEnum<Align>);
					}
					registry.Apply(arg_19B_1, specificity, ref this.alignSelf, VisualElementStylesData.<>f__mg$cache2);
					break;
				}
				case StylePropertyID.AlignItems:
				{
					StyleValueHandle[] arg_16B_1 = values;
					if (VisualElementStylesData.<>f__mg$cache1 == null)
					{
						VisualElementStylesData.<>f__mg$cache1 = new HandlesApplicatorFunction<int>(StyleSheetApplicator.ApplyEnum<Align>);
					}
					registry.Apply(arg_16B_1, specificity, ref this.alignItems, VisualElementStylesData.<>f__mg$cache1);
					break;
				}
				case StylePropertyID.TextAlignment:
				{
					StyleValueHandle[] arg_79B_1 = values;
					if (VisualElementStylesData.<>f__mg$cache22 == null)
					{
						VisualElementStylesData.<>f__mg$cache22 = new HandlesApplicatorFunction<int>(StyleSheetApplicator.ApplyEnum<TextAnchor>);
					}
					registry.Apply(arg_79B_1, specificity, ref this.textAlignment, VisualElementStylesData.<>f__mg$cache22);
					break;
				}
				case StylePropertyID.TextClipping:
				{
					StyleValueHandle[] arg_7CB_1 = values;
					if (VisualElementStylesData.<>f__mg$cache23 == null)
					{
						VisualElementStylesData.<>f__mg$cache23 = new HandlesApplicatorFunction<int>(StyleSheetApplicator.ApplyEnum<TextClipping>);
					}
					registry.Apply(arg_7CB_1, specificity, ref this.textClipping, VisualElementStylesData.<>f__mg$cache23);
					break;
				}
				case StylePropertyID.Font:
				{
					StyleValueHandle[] arg_2EB_1 = values;
					if (VisualElementStylesData.<>f__mg$cache9 == null)
					{
						VisualElementStylesData.<>f__mg$cache9 = new HandlesApplicatorFunction<Font>(StyleSheetApplicator.ApplyResource<Font>);
					}
					registry.Apply(arg_2EB_1, specificity, ref this.font, VisualElementStylesData.<>f__mg$cache9);
					break;
				}
				case StylePropertyID.FontSize:
				{
					StyleValueHandle[] arg_31B_1 = values;
					if (VisualElementStylesData.<>f__mg$cacheA == null)
					{
						VisualElementStylesData.<>f__mg$cacheA = new HandlesApplicatorFunction<int>(StyleSheetApplicator.ApplyInt);
					}
					registry.Apply(arg_31B_1, specificity, ref this.fontSize, VisualElementStylesData.<>f__mg$cacheA);
					break;
				}
				case StylePropertyID.FontStyle:
				{
					StyleValueHandle[] arg_34B_1 = values;
					if (VisualElementStylesData.<>f__mg$cacheB == null)
					{
						VisualElementStylesData.<>f__mg$cacheB = new HandlesApplicatorFunction<int>(StyleSheetApplicator.ApplyEnum<FontStyle>);
					}
					registry.Apply(arg_34B_1, specificity, ref this.fontStyle, VisualElementStylesData.<>f__mg$cacheB);
					break;
				}
				case StylePropertyID.BackgroundSize:
				{
					StyleValueHandle[] arg_8BB_1 = values;
					if (VisualElementStylesData.<>f__mg$cache28 == null)
					{
						VisualElementStylesData.<>f__mg$cache28 = new HandlesApplicatorFunction<int>(StyleSheetApplicator.ApplyInt);
					}
					registry.Apply(arg_8BB_1, specificity, ref this.backgroundSize, VisualElementStylesData.<>f__mg$cache28);
					break;
				}
				case StylePropertyID.WordWrap:
				{
					StyleValueHandle[] arg_85B_1 = values;
					if (VisualElementStylesData.<>f__mg$cache26 == null)
					{
						VisualElementStylesData.<>f__mg$cache26 = new HandlesApplicatorFunction<bool>(StyleSheetApplicator.ApplyBool);
					}
					registry.Apply(arg_85B_1, specificity, ref this.wordWrap, VisualElementStylesData.<>f__mg$cache26);
					break;
				}
				case StylePropertyID.BackgroundImage:
				{
					StyleValueHandle[] arg_1CB_1 = values;
					if (VisualElementStylesData.<>f__mg$cache3 == null)
					{
						VisualElementStylesData.<>f__mg$cache3 = new HandlesApplicatorFunction<Texture2D>(StyleSheetApplicator.ApplyResource<Texture2D>);
					}
					registry.Apply(arg_1CB_1, specificity, ref this.backgroundImage, VisualElementStylesData.<>f__mg$cache3);
					break;
				}
				case StylePropertyID.TextColor:
				{
					StyleValueHandle[] arg_7FB_1 = values;
					if (VisualElementStylesData.<>f__mg$cache24 == null)
					{
						VisualElementStylesData.<>f__mg$cache24 = new HandlesApplicatorFunction<Color>(StyleSheetApplicator.ApplyColor);
					}
					registry.Apply(arg_7FB_1, specificity, ref this.textColor, VisualElementStylesData.<>f__mg$cache24);
					break;
				}
				case StylePropertyID.BackgroundColor:
				{
					StyleValueHandle[] arg_88B_1 = values;
					if (VisualElementStylesData.<>f__mg$cache27 == null)
					{
						VisualElementStylesData.<>f__mg$cache27 = new HandlesApplicatorFunction<Color>(StyleSheetApplicator.ApplyColor);
					}
					registry.Apply(arg_88B_1, specificity, ref this.backgroundColor, VisualElementStylesData.<>f__mg$cache27);
					break;
				}
				case StylePropertyID.BorderColor:
				{
					StyleValueHandle[] arg_8EB_1 = values;
					if (VisualElementStylesData.<>f__mg$cache29 == null)
					{
						VisualElementStylesData.<>f__mg$cache29 = new HandlesApplicatorFunction<Color>(StyleSheetApplicator.ApplyColor);
					}
					registry.Apply(arg_8EB_1, specificity, ref this.borderColor, VisualElementStylesData.<>f__mg$cache29);
					break;
				}
				case StylePropertyID.Overflow:
				{
					StyleValueHandle[] arg_5BB_1 = values;
					if (VisualElementStylesData.<>f__mg$cache18 == null)
					{
						VisualElementStylesData.<>f__mg$cache18 = new HandlesApplicatorFunction<int>(StyleSheetApplicator.ApplyEnum<Overflow>);
					}
					registry.Apply(arg_5BB_1, specificity, ref this.overflow, VisualElementStylesData.<>f__mg$cache18);
					break;
				}
				case StylePropertyID.SliceLeft:
				{
					StyleValueHandle[] arg_A9B_1 = values;
					if (VisualElementStylesData.<>f__mg$cache32 == null)
					{
						VisualElementStylesData.<>f__mg$cache32 = new HandlesApplicatorFunction<int>(StyleSheetApplicator.ApplyInt);
					}
					registry.Apply(arg_A9B_1, specificity, ref this.sliceLeft, VisualElementStylesData.<>f__mg$cache32);
					break;
				}
				case StylePropertyID.SliceTop:
				{
					StyleValueHandle[] arg_ACB_1 = values;
					if (VisualElementStylesData.<>f__mg$cache33 == null)
					{
						VisualElementStylesData.<>f__mg$cache33 = new HandlesApplicatorFunction<int>(StyleSheetApplicator.ApplyInt);
					}
					registry.Apply(arg_ACB_1, specificity, ref this.sliceTop, VisualElementStylesData.<>f__mg$cache33);
					break;
				}
				case StylePropertyID.SliceRight:
				{
					StyleValueHandle[] arg_AFB_1 = values;
					if (VisualElementStylesData.<>f__mg$cache34 == null)
					{
						VisualElementStylesData.<>f__mg$cache34 = new HandlesApplicatorFunction<int>(StyleSheetApplicator.ApplyInt);
					}
					registry.Apply(arg_AFB_1, specificity, ref this.sliceRight, VisualElementStylesData.<>f__mg$cache34);
					break;
				}
				case StylePropertyID.SliceBottom:
				{
					StyleValueHandle[] arg_B2B_1 = values;
					if (VisualElementStylesData.<>f__mg$cache35 == null)
					{
						VisualElementStylesData.<>f__mg$cache35 = new HandlesApplicatorFunction<int>(StyleSheetApplicator.ApplyInt);
					}
					registry.Apply(arg_B2B_1, specificity, ref this.sliceBottom, VisualElementStylesData.<>f__mg$cache35);
					break;
				}
				case StylePropertyID.Opacity:
				{
					StyleValueHandle[] arg_B5B_1 = values;
					if (VisualElementStylesData.<>f__mg$cache36 == null)
					{
						VisualElementStylesData.<>f__mg$cache36 = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
					}
					registry.Apply(arg_B5B_1, specificity, ref this.opacity, VisualElementStylesData.<>f__mg$cache36);
					break;
				}
				case StylePropertyID.BorderRadius:
				{
					StyleValueHandle[] arg_B8B_1 = values;
					if (VisualElementStylesData.<>f__mg$cache37 == null)
					{
						VisualElementStylesData.<>f__mg$cache37 = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
					}
					registry.Apply(arg_B8B_1, specificity, ref this.borderTopLeftRadius, VisualElementStylesData.<>f__mg$cache37);
					StyleValueHandle[] arg_BB6_1 = values;
					if (VisualElementStylesData.<>f__mg$cache38 == null)
					{
						VisualElementStylesData.<>f__mg$cache38 = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
					}
					registry.Apply(arg_BB6_1, specificity, ref this.borderTopRightRadius, VisualElementStylesData.<>f__mg$cache38);
					StyleValueHandle[] arg_BE1_1 = values;
					if (VisualElementStylesData.<>f__mg$cache39 == null)
					{
						VisualElementStylesData.<>f__mg$cache39 = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
					}
					registry.Apply(arg_BE1_1, specificity, ref this.borderBottomLeftRadius, VisualElementStylesData.<>f__mg$cache39);
					StyleValueHandle[] arg_C0C_1 = values;
					if (VisualElementStylesData.<>f__mg$cache3A == null)
					{
						VisualElementStylesData.<>f__mg$cache3A = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
					}
					registry.Apply(arg_C0C_1, specificity, ref this.borderBottomRightRadius, VisualElementStylesData.<>f__mg$cache3A);
					break;
				}
				case StylePropertyID.Margin:
				case StylePropertyID.Padding:
					goto IL_C91;
				case StylePropertyID.Custom:
				{
					if (this.m_CustomProperties == null)
					{
						this.m_CustomProperties = new Dictionary<string, CustomProperty>();
					}
					CustomProperty value = default(CustomProperty);
					if (!this.m_CustomProperties.TryGetValue(styleProperty.name, out value) || specificity >= value.specificity)
					{
						value.handles = values;
						value.data = registry;
						value.specificity = specificity;
						this.m_CustomProperties[styleProperty.name] = value;
					}
					break;
				}
				default:
					goto IL_C91;
				}
				i++;
				continue;
				IL_C91:
				throw new ArgumentException(string.Format("Non exhaustive switch statement (value={0})", stylePropertyID));
			}
		}

		public void ApplyCustomProperty(string propertyName, ref StyleValue<float> target)
		{
			StyleValueType arg_22_3 = StyleValueType.Float;
			if (VisualElementStylesData.<>f__mg$cache3B == null)
			{
				VisualElementStylesData.<>f__mg$cache3B = new HandlesApplicatorFunction<float>(StyleSheetApplicator.ApplyFloat);
			}
			this.ApplyCustomProperty<float>(propertyName, ref target, arg_22_3, VisualElementStylesData.<>f__mg$cache3B);
		}

		public void ApplyCustomProperty(string propertyName, ref StyleValue<int> target)
		{
			StyleValueType arg_22_3 = StyleValueType.Float;
			if (VisualElementStylesData.<>f__mg$cache3C == null)
			{
				VisualElementStylesData.<>f__mg$cache3C = new HandlesApplicatorFunction<int>(StyleSheetApplicator.ApplyInt);
			}
			this.ApplyCustomProperty<int>(propertyName, ref target, arg_22_3, VisualElementStylesData.<>f__mg$cache3C);
		}

		public void ApplyCustomProperty(string propertyName, ref StyleValue<bool> target)
		{
			StyleValueType arg_22_3 = StyleValueType.Keyword;
			if (VisualElementStylesData.<>f__mg$cache3D == null)
			{
				VisualElementStylesData.<>f__mg$cache3D = new HandlesApplicatorFunction<bool>(StyleSheetApplicator.ApplyBool);
			}
			this.ApplyCustomProperty<bool>(propertyName, ref target, arg_22_3, VisualElementStylesData.<>f__mg$cache3D);
		}

		public void ApplyCustomProperty(string propertyName, ref StyleValue<Color> target)
		{
			StyleValueType arg_22_3 = StyleValueType.Color;
			if (VisualElementStylesData.<>f__mg$cache3E == null)
			{
				VisualElementStylesData.<>f__mg$cache3E = new HandlesApplicatorFunction<Color>(StyleSheetApplicator.ApplyColor);
			}
			this.ApplyCustomProperty<Color>(propertyName, ref target, arg_22_3, VisualElementStylesData.<>f__mg$cache3E);
		}

		public void ApplyCustomProperty<T>(string propertyName, ref StyleValue<T> target) where T : UnityEngine.Object
		{
			this.ApplyCustomProperty<T>(propertyName, ref target, StyleValueType.ResourcePath, new HandlesApplicatorFunction<T>(StyleSheetApplicator.ApplyResource<T>));
		}

		public void ApplyCustomProperty(string propertyName, ref StyleValue<string> target)
		{
			StyleValue<string> other = new StyleValue<string>(string.Empty);
			CustomProperty customProperty;
			if (this.m_CustomProperties != null && this.m_CustomProperties.TryGetValue(propertyName, out customProperty))
			{
				other.value = customProperty.data.ReadAsString(customProperty.handles[0]);
				other.specificity = customProperty.specificity;
			}
			target.Apply(other, StylePropertyApplyMode.CopyIfNotInline);
		}

		internal void ApplyCustomProperty<T>(string propertyName, ref StyleValue<T> target, StyleValueType valueType, HandlesApplicatorFunction<T> applicatorFunc)
		{
			StyleValue<T> other = default(StyleValue<T>);
			CustomProperty customProperty;
			if (this.m_CustomProperties != null && this.m_CustomProperties.TryGetValue(propertyName, out customProperty))
			{
				StyleValueHandle styleValueHandle = customProperty.handles[0];
				if (styleValueHandle.valueType == valueType)
				{
					customProperty.data.Apply(customProperty.handles, customProperty.specificity, ref other, applicatorFunc);
				}
				else
				{
					Debug.LogWarning(string.Format("Trying to read value as {0} while parsed type is {1}", valueType, styleValueHandle.valueType));
				}
			}
			target.Apply(other, StylePropertyApplyMode.CopyIfNotInline);
		}
	}
}
