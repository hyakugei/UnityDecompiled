using System;
using UnityEngine;

namespace UnityEditor
{
	internal static class StatelessAdvancedDropdown
	{
		private static AdvancedDropdown s_Instance;

		private static EditorWindow s_ParentWindow;

		private static bool m_WindowClosed;

		private static int m_Result;

		private static int s_CurrentControl;

		private static void SearchablePopup(Rect rect, string label, int selectedIndex, string[] displayedOptions, Action<string> onSelected)
		{
			StatelessAdvancedDropdown.ResetAndCreateWindow();
			StatelessAdvancedDropdown.InitWindow(rect, label, selectedIndex, displayedOptions);
			StatelessAdvancedDropdown.s_Instance.onSelected += delegate(AdvancedDropdownWindow w)
			{
				onSelected(w.GetIdOfSelectedItem());
			};
		}

		public static int SearchablePopup(Rect rect, int selectedIndex, string[] displayedOptions)
		{
			return StatelessAdvancedDropdown.SearchablePopup(rect, selectedIndex, displayedOptions, "MiniPullDown");
		}

		public static int SearchablePopup(Rect rect, int selectedIndex, string[] displayedOptions, GUIStyle style)
		{
			string text = null;
			if (selectedIndex >= 0)
			{
				text = displayedOptions[selectedIndex];
			}
			GUIContent gUIContent = new GUIContent(text);
			int controlID = GUIUtility.GetControlID("AdvancedDropdown".GetHashCode(), FocusType.Keyboard, rect);
			if (EditorGUI.DropdownButton(controlID, rect, gUIContent, "MiniPullDown"))
			{
				StatelessAdvancedDropdown.s_CurrentControl = controlID;
				StatelessAdvancedDropdown.ResetAndCreateWindow();
				StatelessAdvancedDropdown.InitWindow(rect, gUIContent.text, selectedIndex, displayedOptions);
				StatelessAdvancedDropdown.s_Instance.onSelected += delegate(AdvancedDropdownWindow w)
				{
					StatelessAdvancedDropdown.m_Result = w.GetSelectedIndex();
					StatelessAdvancedDropdown.m_WindowClosed = true;
				};
			}
			int result;
			if (StatelessAdvancedDropdown.m_WindowClosed && StatelessAdvancedDropdown.s_CurrentControl == controlID)
			{
				StatelessAdvancedDropdown.s_CurrentControl = 0;
				StatelessAdvancedDropdown.m_WindowClosed = false;
				result = StatelessAdvancedDropdown.m_Result;
			}
			else
			{
				result = selectedIndex;
			}
			return result;
		}

		private static void ResetAndCreateWindow()
		{
			if (StatelessAdvancedDropdown.s_Instance != null)
			{
				StatelessAdvancedDropdown.s_Instance.Close();
				StatelessAdvancedDropdown.s_Instance = null;
			}
			StatelessAdvancedDropdown.s_ParentWindow = EditorWindow.focusedWindow;
			StatelessAdvancedDropdown.s_Instance = ScriptableObject.CreateInstance<AdvancedDropdown>();
			StatelessAdvancedDropdown.m_WindowClosed = false;
		}

		private static void InitWindow(Rect rect, string label, int selectedIndex, string[] displayedOptions)
		{
			StatelessAdvancedDropdown.s_Instance.DisplayedOptions = displayedOptions;
			StatelessAdvancedDropdown.s_Instance.SelectedIndex = selectedIndex;
			StatelessAdvancedDropdown.s_Instance.Label = label;
			StatelessAdvancedDropdown.s_Instance.onSelected += delegate(AdvancedDropdownWindow w)
			{
				if (StatelessAdvancedDropdown.s_ParentWindow != null)
				{
					StatelessAdvancedDropdown.s_ParentWindow.Repaint();
				}
			};
			StatelessAdvancedDropdown.s_Instance.Init(rect);
		}
	}
}
