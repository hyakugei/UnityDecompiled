using System;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.Experimental.UIElements.StyleSheets;
using UnityEngine.StyleSheets;

namespace UnityEditor.Experimental.UIElements
{
	public static class UIElementsEditorUtility
	{
		internal static readonly string s_DefaultCommonStyleSheetPath = "StyleSheets/DefaultCommon.uss";

		internal static readonly string s_DefaultCommonDarkStyleSheetPath = "StyleSheets/DefaultCommonDark.uss";

		internal static readonly string s_DefaultCommonLightStyleSheetPath = "StyleSheets/DefaultCommonLight.uss";

		public static CursorStyle CreateDefaultCursorStyle(MouseCursor mouseCursor)
		{
			return new CursorStyle
			{
				texture = null,
				hotspot = Vector2.zero,
				defaultCursorId = (int)mouseCursor
			};
		}

		internal static CursorStyle CreateDefaultCursorStyle(StyleSheet sheet, StyleValueHandle handle)
		{
			int enumValue = StyleSheetCache.GetEnumValue<MouseCursor>(sheet, handle);
			return new CursorStyle
			{
				texture = null,
				hotspot = Vector2.zero,
				defaultCursorId = enumValue
			};
		}

		internal static void AddDefaultEditorStyleSheets(VisualElement p)
		{
			if (p.styleSheets == null)
			{
				p.AddStyleSheetPath(UIElementsEditorUtility.s_DefaultCommonStyleSheetPath);
				if (EditorGUIUtility.isProSkin)
				{
					p.AddStyleSheetPath(UIElementsEditorUtility.s_DefaultCommonDarkStyleSheetPath);
				}
				else
				{
					p.AddStyleSheetPath(UIElementsEditorUtility.s_DefaultCommonLightStyleSheetPath);
				}
			}
		}

		internal static void ForceDarkStyleSheet(VisualElement ele)
		{
			if (!EditorGUIUtility.isProSkin)
			{
				for (VisualElement visualElement = ele; visualElement != null; visualElement = visualElement.parent)
				{
					if (visualElement.HasStyleSheetPath(UIElementsEditorUtility.s_DefaultCommonLightStyleSheetPath))
					{
						visualElement.ReplaceStyleSheetPath(UIElementsEditorUtility.s_DefaultCommonLightStyleSheetPath, UIElementsEditorUtility.s_DefaultCommonDarkStyleSheetPath);
						break;
					}
				}
			}
		}
	}
}
