using System;
using UnityEditor;
using UnityEngine;

namespace UnityEditorInternal.Profiling
{
	internal abstract class ProfilerDetailedView
	{
		protected static class Styles
		{
			public static GUIContent emptyText;

			public static GUIContent selectLineText;

			public static readonly GUIStyle expandedArea;

			public static readonly GUIStyle callstackScroll;

			public static readonly GUIStyle callstackTextArea;

			static Styles()
			{
				ProfilerDetailedView.Styles.emptyText = new GUIContent("");
				ProfilerDetailedView.Styles.selectLineText = EditorGUIUtility.TrTextContent("Select Line for the detailed information", null, null);
				ProfilerDetailedView.Styles.expandedArea = GUIStyle.none;
				ProfilerDetailedView.Styles.callstackScroll = GUIStyle.none;
				ProfilerDetailedView.Styles.callstackTextArea = EditorStyles.textArea;
				ProfilerDetailedView.Styles.expandedArea.stretchWidth = true;
				ProfilerDetailedView.Styles.expandedArea.stretchHeight = true;
				ProfilerDetailedView.Styles.expandedArea.padding = new RectOffset(0, 0, 0, 0);
				ProfilerDetailedView.Styles.callstackScroll.padding = new RectOffset(5, 5, 5, 5);
				ProfilerDetailedView.Styles.callstackTextArea.margin = new RectOffset(0, 0, 0, 0);
				ProfilerDetailedView.Styles.callstackTextArea.padding = new RectOffset(3, 3, 3, 3);
				ProfilerDetailedView.Styles.callstackTextArea.wordWrap = false;
				ProfilerDetailedView.Styles.callstackTextArea.stretchWidth = true;
				ProfilerDetailedView.Styles.callstackTextArea.stretchHeight = true;
			}
		}

		protected static readonly string kNoneText = LocalizationDatabase.GetLocalizedString("None");

		protected FrameDataView m_FrameDataView;

		protected int m_SelectedID = -1;

		protected void DrawEmptyPane(GUIStyle headerStyle)
		{
			GUILayout.Box(ProfilerDetailedView.Styles.emptyText, headerStyle, new GUILayoutOption[0]);
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.FlexibleSpace();
			GUILayout.BeginVertical(new GUILayoutOption[0]);
			GUILayout.FlexibleSpace();
			GUILayout.Label(ProfilerDetailedView.Styles.selectLineText, EditorStyles.wordWrappedLabel, new GUILayoutOption[0]);
			GUILayout.FlexibleSpace();
			GUILayout.EndVertical();
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
		}
	}
}
