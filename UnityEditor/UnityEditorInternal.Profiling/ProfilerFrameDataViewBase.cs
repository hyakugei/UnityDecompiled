using System;
using System.Threading;
using UnityEditor;
using UnityEngine;

namespace UnityEditorInternal.Profiling
{
	[Serializable]
	internal class ProfilerFrameDataViewBase
	{
		protected static class BaseStyles
		{
			public static readonly GUIContent noData;

			public static GUIContent disabledSearchText;

			public static readonly GUIStyle header;

			public static readonly GUIStyle label;

			public static readonly GUIStyle toolbar;

			public static readonly GUIStyle tooltip;

			public static readonly GUIStyle tooltipArrow;

			public static readonly GUIStyle viewTypeToolbarDropDown;

			static BaseStyles()
			{
				ProfilerFrameDataViewBase.BaseStyles.noData = EditorGUIUtility.TextContent("No frame data available");
				ProfilerFrameDataViewBase.BaseStyles.disabledSearchText = EditorGUIUtility.TextContent("Showing search results are disabled while recording with deep profiling.\nStop recording to view search results.");
				ProfilerFrameDataViewBase.BaseStyles.header = "OL title";
				ProfilerFrameDataViewBase.BaseStyles.label = "OL label";
				ProfilerFrameDataViewBase.BaseStyles.toolbar = EditorStyles.toolbar;
				ProfilerFrameDataViewBase.BaseStyles.tooltip = "AnimationEventTooltip";
				ProfilerFrameDataViewBase.BaseStyles.tooltipArrow = "AnimationEventTooltipArrow";
				ProfilerFrameDataViewBase.BaseStyles.viewTypeToolbarDropDown = EditorStyles.toolbarDropDown;
				ProfilerFrameDataViewBase.BaseStyles.toolbar.padding.left = 0;
				ProfilerFrameDataViewBase.BaseStyles.viewTypeToolbarDropDown.stretchWidth = true;
			}
		}

		public delegate void ViewTypeChangedCallback(ProfilerViewType viewType);

		private static readonly GUIContent[] kCPUProfilerViewTypeNames = new GUIContent[]
		{
			EditorGUIUtility.TextContent("Hierarchy"),
			EditorGUIUtility.TextContent("Timeline"),
			EditorGUIUtility.TextContent("Raw Hierarchy")
		};

		private static readonly int[] kCPUProfilerViewTypes = new int[]
		{
			0,
			1,
			2
		};

		private static readonly GUIContent[] kGPUProfilerViewTypeNames = new GUIContent[]
		{
			EditorGUIUtility.TextContent("Hierarchy"),
			EditorGUIUtility.TextContent("Raw Hierarchy")
		};

		private static readonly int[] kGPUProfilerViewTypes = new int[]
		{
			0,
			2
		};

		public event ProfilerFrameDataViewBase.ViewTypeChangedCallback viewTypeChanged
		{
			add
			{
				ProfilerFrameDataViewBase.ViewTypeChangedCallback viewTypeChangedCallback = this.viewTypeChanged;
				ProfilerFrameDataViewBase.ViewTypeChangedCallback viewTypeChangedCallback2;
				do
				{
					viewTypeChangedCallback2 = viewTypeChangedCallback;
					viewTypeChangedCallback = Interlocked.CompareExchange<ProfilerFrameDataViewBase.ViewTypeChangedCallback>(ref this.viewTypeChanged, (ProfilerFrameDataViewBase.ViewTypeChangedCallback)Delegate.Combine(viewTypeChangedCallback2, value), viewTypeChangedCallback);
				}
				while (viewTypeChangedCallback != viewTypeChangedCallback2);
			}
			remove
			{
				ProfilerFrameDataViewBase.ViewTypeChangedCallback viewTypeChangedCallback = this.viewTypeChanged;
				ProfilerFrameDataViewBase.ViewTypeChangedCallback viewTypeChangedCallback2;
				do
				{
					viewTypeChangedCallback2 = viewTypeChangedCallback;
					viewTypeChangedCallback = Interlocked.CompareExchange<ProfilerFrameDataViewBase.ViewTypeChangedCallback>(ref this.viewTypeChanged, (ProfilerFrameDataViewBase.ViewTypeChangedCallback)Delegate.Remove(viewTypeChangedCallback2, value), viewTypeChangedCallback);
				}
				while (viewTypeChangedCallback != viewTypeChangedCallback2);
			}
		}

		public bool gpuView
		{
			get;
			set;
		}

		protected ProfilerFrameDataViewBase()
		{
		}

		protected void DrawViewTypePopup(ProfilerViewType viewType)
		{
			ProfilerViewType profilerViewType;
			if (!this.gpuView)
			{
				profilerViewType = (ProfilerViewType)EditorGUILayout.IntPopup((int)viewType, ProfilerFrameDataViewBase.kCPUProfilerViewTypeNames, ProfilerFrameDataViewBase.kCPUProfilerViewTypes, ProfilerFrameDataViewBase.BaseStyles.viewTypeToolbarDropDown, new GUILayoutOption[0]);
			}
			else
			{
				if (viewType == ProfilerViewType.Timeline)
				{
					viewType = ProfilerViewType.Hierarchy;
				}
				profilerViewType = (ProfilerViewType)EditorGUILayout.IntPopup((int)viewType, ProfilerFrameDataViewBase.kGPUProfilerViewTypeNames, ProfilerFrameDataViewBase.kGPUProfilerViewTypes, ProfilerFrameDataViewBase.BaseStyles.viewTypeToolbarDropDown, new GUILayoutOption[0]);
			}
			if (profilerViewType != viewType)
			{
				if (this.viewTypeChanged != null)
				{
					this.viewTypeChanged(profilerViewType);
				}
			}
		}

		protected void ShowLargeTooltip(Vector2 pos, Rect fullRect, string text)
		{
			GUIContent content = GUIContent.Temp(text);
			GUIStyle tooltip = ProfilerFrameDataViewBase.BaseStyles.tooltip;
			Vector2 vector = tooltip.CalcSize(content);
			Rect position = new Rect(pos.x - 32f, pos.y, 64f, 6f);
			Rect position2 = new Rect(pos.x, pos.y + 6f, vector.x, vector.y);
			if (position2.xMax > fullRect.xMax + 16f)
			{
				position2.x = fullRect.xMax - position2.width + 16f;
			}
			if (position.xMax > fullRect.xMax + 20f)
			{
				position.x = fullRect.xMax - position.width + 20f;
			}
			if (position2.xMin < fullRect.xMin + 30f)
			{
				position2.x = fullRect.xMin + 30f;
			}
			if (position.xMin < fullRect.xMin - 20f)
			{
				position.x = fullRect.xMin - 20f;
			}
			float num = 16f + position2.height + 2f * position.height;
			bool flag = pos.y + vector.y + 6f > fullRect.yMax && position2.y - num > 0f;
			if (flag)
			{
				position2.y -= num;
				position.y -= 16f + 2f * position.height;
			}
			GUI.BeginClip(position);
			Matrix4x4 matrix = GUI.matrix;
			if (flag)
			{
				GUIUtility.ScaleAroundPivot(new Vector2(1f, -1f), new Vector2(position.width * 0.5f, position.height));
			}
			GUI.Label(new Rect(0f, 0f, position.width, position.height), GUIContent.none, ProfilerFrameDataViewBase.BaseStyles.tooltipArrow);
			GUI.matrix = matrix;
			GUI.EndClip();
			GUI.Label(position2, content, tooltip);
		}

		public virtual void Clear()
		{
		}
	}
}
