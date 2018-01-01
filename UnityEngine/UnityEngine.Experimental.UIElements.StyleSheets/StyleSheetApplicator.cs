using System;
using UnityEngine.StyleSheets;

namespace UnityEngine.Experimental.UIElements.StyleSheets
{
	internal static class StyleSheetApplicator
	{
		internal delegate CursorStyle CreateDefaultCursorStyleFunction(StyleSheet sheet, StyleValueHandle handle);

		public static class Shorthand
		{
			private static void ReadFourSidesArea(StyleSheet sheet, StyleValueHandle[] handles, out float top, out float right, out float bottom, out float left)
			{
				top = 0f;
				right = 0f;
				bottom = 0f;
				left = 0f;
				switch (handles.Length)
				{
				case 0:
					break;
				case 1:
					top = (right = (bottom = (left = sheet.ReadFloat(handles[0]))));
					break;
				case 2:
					top = (bottom = sheet.ReadFloat(handles[0]));
					left = (right = sheet.ReadFloat(handles[1]));
					break;
				case 3:
					top = sheet.ReadFloat(handles[0]);
					left = (right = sheet.ReadFloat(handles[1]));
					bottom = sheet.ReadFloat(handles[2]);
					break;
				default:
					top = sheet.ReadFloat(handles[0]);
					right = sheet.ReadFloat(handles[1]);
					bottom = sheet.ReadFloat(handles[2]);
					left = sheet.ReadFloat(handles[3]);
					break;
				}
			}

			public static void ApplyBorderRadius(StyleSheet sheet, StyleValueHandle[] handles, int specificity, VisualElementStylesData styleData)
			{
				float val;
				float val2;
				float val3;
				float val4;
				StyleSheetApplicator.Shorthand.ReadFourSidesArea(sheet, handles, out val, out val2, out val3, out val4);
				StyleSheetApplicator.Apply<float>(val, specificity, ref styleData.borderTopLeftRadius);
				StyleSheetApplicator.Apply<float>(val2, specificity, ref styleData.borderTopRightRadius);
				StyleSheetApplicator.Apply<float>(val4, specificity, ref styleData.borderBottomLeftRadius);
				StyleSheetApplicator.Apply<float>(val3, specificity, ref styleData.borderBottomRightRadius);
			}

			public static void ApplyMargin(StyleSheet sheet, StyleValueHandle[] handles, int specificity, VisualElementStylesData styleData)
			{
				float val;
				float val2;
				float val3;
				float val4;
				StyleSheetApplicator.Shorthand.ReadFourSidesArea(sheet, handles, out val, out val2, out val3, out val4);
				StyleSheetApplicator.Apply<float>(val, specificity, ref styleData.marginTop);
				StyleSheetApplicator.Apply<float>(val2, specificity, ref styleData.marginRight);
				StyleSheetApplicator.Apply<float>(val3, specificity, ref styleData.marginBottom);
				StyleSheetApplicator.Apply<float>(val4, specificity, ref styleData.marginLeft);
			}

			public static void ApplyPadding(StyleSheet sheet, StyleValueHandle[] handles, int specificity, VisualElementStylesData styleData)
			{
				float val;
				float val2;
				float val3;
				float val4;
				StyleSheetApplicator.Shorthand.ReadFourSidesArea(sheet, handles, out val, out val2, out val3, out val4);
				StyleSheetApplicator.Apply<float>(val, specificity, ref styleData.paddingTop);
				StyleSheetApplicator.Apply<float>(val2, specificity, ref styleData.paddingRight);
				StyleSheetApplicator.Apply<float>(val3, specificity, ref styleData.paddingBottom);
				StyleSheetApplicator.Apply<float>(val4, specificity, ref styleData.paddingLeft);
			}
		}

		internal static StyleSheetApplicator.CreateDefaultCursorStyleFunction createDefaultCursorStyleFunc = null;

		private static void Apply<T>(T val, int specificity, ref StyleValue<T> property)
		{
			property.Apply(new StyleValue<T>(val, specificity), StylePropertyApplyMode.CopyIfEqualOrGreaterSpecificity);
		}

		public static void ApplyDefault<T>(int specificity, ref StyleValue<T> property)
		{
			StyleSheetApplicator.Apply<T>(default(T), specificity, ref property);
		}

		public static void ApplyBool(StyleSheet sheet, StyleValueHandle[] handles, int specificity, ref StyleValue<bool> property)
		{
			bool val = sheet.ReadKeyword(handles[0]) == StyleValueKeyword.True;
			StyleSheetApplicator.Apply<bool>(val, specificity, ref property);
		}

		public static void ApplyFloat(StyleSheet sheet, StyleValueHandle[] handles, int specificity, ref StyleValue<float> property)
		{
			float val = sheet.ReadFloat(handles[0]);
			StyleSheetApplicator.Apply<float>(val, specificity, ref property);
		}

		public static void ApplyInt(StyleSheet sheet, StyleValueHandle[] handles, int specificity, ref StyleValue<int> property)
		{
			int val = (int)sheet.ReadFloat(handles[0]);
			StyleSheetApplicator.Apply<int>(val, specificity, ref property);
		}

		public static void ApplyEnum<T>(StyleSheet sheet, StyleValueHandle[] handles, int specificity, ref StyleValue<int> property)
		{
			int enumValue = StyleSheetCache.GetEnumValue<T>(sheet, handles[0]);
			StyleSheetApplicator.Apply<int>(enumValue, specificity, ref property);
		}

		public static void ApplyColor(StyleSheet sheet, StyleValueHandle[] handles, int specificity, ref StyleValue<Color> property)
		{
			Color val = sheet.ReadColor(handles[0]);
			StyleSheetApplicator.Apply<Color>(val, specificity, ref property);
		}

		public static void ApplyCursor(StyleSheet sheet, StyleValueHandle[] handles, int specificity, ref StyleValue<CursorStyle> property)
		{
			StyleValueHandle handle = handles[0];
			bool flag = handle.valueType == StyleValueType.ResourcePath;
			if (flag)
			{
				string pathName = sheet.ReadResourcePath(handles[0]);
				Texture2D texture2D = Panel.loadResourceFunc(pathName, typeof(Texture2D)) as Texture2D;
				if (texture2D != null)
				{
					Vector2 zero = Vector2.zero;
					sheet.TryReadFloat(handles, 1, out zero.x);
					sheet.TryReadFloat(handles, 2, out zero.y);
					CursorStyle val = new CursorStyle
					{
						texture = texture2D,
						hotspot = zero
					};
					StyleSheetApplicator.Apply<CursorStyle>(val, specificity, ref property);
				}
			}
			else if (StyleSheetApplicator.createDefaultCursorStyleFunc != null)
			{
				CursorStyle val2 = StyleSheetApplicator.createDefaultCursorStyleFunc(sheet, handle);
				StyleSheetApplicator.Apply<CursorStyle>(val2, specificity, ref property);
			}
		}

		public static void ApplyResource<T>(StyleSheet sheet, StyleValueHandle[] handles, int specificity, ref StyleValue<T> property) where T : UnityEngine.Object
		{
			StyleValueHandle handle = handles[0];
			if (handle.valueType == StyleValueType.Keyword && handle.valueIndex == 5)
			{
				StyleSheetApplicator.Apply<T>((T)((object)null), specificity, ref property);
			}
			else
			{
				T t = (T)((object)null);
				string text = sheet.ReadResourcePath(handle);
				if (!string.IsNullOrEmpty(text))
				{
					t = (Panel.loadResourceFunc(text, typeof(T)) as T);
					if (t != null)
					{
						StyleSheetApplicator.Apply<T>(t, specificity, ref property);
					}
					else
					{
						if (typeof(T) == typeof(Texture2D))
						{
							t = (Panel.loadResourceFunc("d_console.warnicon", typeof(T)) as T);
							StyleSheetApplicator.Apply<T>(t, specificity, ref property);
						}
						Debug.LogWarning(string.Format("{0} resource/file not found for path: {1}", typeof(T).Name, text));
					}
				}
			}
		}
	}
}
