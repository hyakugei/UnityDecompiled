using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEditor;
using UnityEditorInternal.Profiling;
using UnityEngine;

namespace UnityEditorInternal
{
	[Serializable]
	internal class ProfilerTimelineGUI : ProfilerFrameDataViewBase
	{
		internal class ThreadInfo
		{
			public float height;

			public float desiredWeight;

			public float weight;

			public int threadIndex;

			public string name;

			public bool alive;
		}

		internal class GroupInfo
		{
			public bool expanded;

			public string name;

			public float height;

			public List<ProfilerTimelineGUI.ThreadInfo> threads;
		}

		internal class Styles
		{
			public GUIStyle background = "OL Box";

			public GUIStyle tooltip = "AnimationEventTooltip";

			public GUIStyle tooltipArrow = "AnimationEventTooltipArrow";

			public GUIStyle bar = "ProfilerTimelineBar";

			public GUIStyle leftPane = "ProfilerTimelineLeftPane";

			public GUIStyle rightPane = "ProfilerRightPane";

			public GUIStyle foldout = "ProfilerTimelineFoldout";

			public GUIStyle profilerGraphBackground = new GUIStyle("ProfilerScrollviewBackground");

			public GUIStyle timelineTick = "AnimationTimelineTick";

			public GUIStyle rectangleToolSelection = "RectangleToolSelection";

			public Color frameDelimiterColor = Color.white.RGBMultiplied(0.4f);

			private Color m_RangeSelectionColorLight = new Color32(255, 255, 255, 90);

			private Color m_RangeSelectionColorDark = new Color32(200, 200, 200, 40);

			private Color m_OutOfRangeColorLight = new Color32(160, 160, 160, 127);

			private Color m_OutOfRangeColorDark = new Color32(40, 40, 40, 127);

			public Color rangeSelectionColor
			{
				[CompilerGenerated]
				get
				{
					return (!EditorGUIUtility.isProSkin) ? this.m_RangeSelectionColorLight : this.m_RangeSelectionColorDark;
				}
			}

			public Color outOfRangeColor
			{
				[CompilerGenerated]
				get
				{
					return (!EditorGUIUtility.isProSkin) ? this.m_OutOfRangeColorLight : this.m_OutOfRangeColorDark;
				}
			}

			internal Styles()
			{
				GUIStyleState arg_175_0 = this.bar.normal;
				Texture2D texture2D = EditorGUIUtility.whiteTexture;
				this.bar.active.background = texture2D;
				texture2D = texture2D;
				this.bar.hover.background = texture2D;
				arg_175_0.background = texture2D;
				GUIStyleState arg_1B0_0 = this.bar.normal;
				Color color = Color.black;
				this.bar.active.textColor = color;
				color = color;
				this.bar.hover.textColor = color;
				arg_1B0_0.textColor = color;
				this.profilerGraphBackground.overflow.left = -179;
				this.leftPane.padding.left = 15;
			}
		}

		private class EntryInfo
		{
			public int frameId = -1;

			public int threadId = -1;

			public int nativeIndex = -1;

			public float relativeYPos = 0f;

			public float time = 0f;

			public float duration = 0f;

			public string name = string.Empty;

			public bool IsValid()
			{
				return this.name.Length > 0;
			}

			public bool Equals(int frameId, int threadId, int nativeIndex)
			{
				return frameId == this.frameId && threadId == this.threadId && nativeIndex == this.nativeIndex;
			}

			public virtual void Reset()
			{
				this.frameId = -1;
				this.threadId = -1;
				this.nativeIndex = -1;
				this.relativeYPos = 0f;
				this.time = 0f;
				this.duration = 0f;
				this.name = string.Empty;
			}
		}

		private class SelectedEntryInfo : ProfilerTimelineGUI.EntryInfo
		{
			public int instanceId = -1;

			public string metaData = string.Empty;

			public float totalDuration = -1f;

			public int instanceCount = -1;

			public string callstackInfo = string.Empty;

			public override void Reset()
			{
				base.Reset();
				this.instanceId = -1;
				this.metaData = string.Empty;
				this.totalDuration = -1f;
				this.instanceCount = -1;
				this.callstackInfo = string.Empty;
			}
		}

		private struct RangeSelectionInfo
		{
			public static readonly int controlIDHint = "RangeSelection".GetHashCode();

			public bool active;

			public bool mouseDown;

			public float mouseDownTime;

			public float startTime;

			public float endTime;

			public float duration
			{
				[CompilerGenerated]
				get
				{
					return this.endTime - this.startTime;
				}
			}
		}

		private const float kTextFadeStartWidth = 50f;

		private const float kTextFadeOutWidth = 20f;

		private const float kLineHeight = 16f;

		private const float kGroupHeight = 20f;

		private static readonly float[] k_TickModulos = new float[]
		{
			0.001f,
			0.005f,
			0.01f,
			0.05f,
			0.1f,
			0.5f,
			1f,
			5f,
			10f,
			50f,
			100f,
			500f,
			1000f,
			5000f,
			10000f,
			30000f,
			60000f
		};

		private const string k_TickFormatMilliseconds = "{0}ms";

		private const string k_TickFormatSeconds = "{0}s";

		private const int k_TickLabelSeparation = 60;

		private float animationTime = 1f;

		private double lastScrollUpdate = 0.0;

		private List<ProfilerTimelineGUI.GroupInfo> groups = null;

		private static ProfilerTimelineGUI.Styles ms_Styles;

		[NonSerialized]
		private ZoomableArea m_TimeArea;

		private TickHandler m_HTicks;

		private IProfilerWindowController m_Window;

		private ProfilerTimelineGUI.SelectedEntryInfo m_SelectedEntry = new ProfilerTimelineGUI.SelectedEntryInfo();

		private float m_SelectedThreadY = 0f;

		private string m_LocalizedString_Total;

		private string m_LocalizedString_Instances;

		private ProfilerTimelineGUI.RangeSelectionInfo m_RangeSelection = default(ProfilerTimelineGUI.RangeSelectionInfo);

		private static ProfilerTimelineGUI.Styles styles
		{
			get
			{
				ProfilerTimelineGUI.Styles arg_18_0;
				if ((arg_18_0 = ProfilerTimelineGUI.ms_Styles) == null)
				{
					arg_18_0 = (ProfilerTimelineGUI.ms_Styles = new ProfilerTimelineGUI.Styles());
				}
				return arg_18_0;
			}
		}

		public ProfilerTimelineGUI(IProfilerWindowController window)
		{
			this.m_Window = window;
			this.groups = new List<ProfilerTimelineGUI.GroupInfo>(new ProfilerTimelineGUI.GroupInfo[]
			{
				new ProfilerTimelineGUI.GroupInfo
				{
					name = "",
					height = 0f,
					expanded = true,
					threads = new List<ProfilerTimelineGUI.ThreadInfo>()
				},
				new ProfilerTimelineGUI.GroupInfo
				{
					name = "Unity Job System",
					height = 20f,
					expanded = SessionState.GetBool("Unity Job System", false),
					threads = new List<ProfilerTimelineGUI.ThreadInfo>()
				},
				new ProfilerTimelineGUI.GroupInfo
				{
					name = "Loading",
					height = 20f,
					expanded = SessionState.GetBool("Loading", false),
					threads = new List<ProfilerTimelineGUI.ThreadInfo>()
				}
			});
			this.m_LocalizedString_Total = LocalizationDatabase.GetLocalizedString("Total");
			this.m_LocalizedString_Instances = LocalizationDatabase.GetLocalizedString("Instances");
			this.m_HTicks = new TickHandler();
			this.m_HTicks.SetTickModulos(ProfilerTimelineGUI.k_TickModulos);
		}

		private void CalculateBars(ref ProfilerFrameDataIterator iter, Rect r, int frameIndex, float time)
		{
			float num = 0f;
			iter.SetRoot(frameIndex, 0);
			int threadCount = iter.GetThreadCount(frameIndex);
			int i;
			for (i = 0; i < threadCount; i++)
			{
				iter.SetRoot(frameIndex, i);
				string groupname = iter.GetGroupName();
				ProfilerTimelineGUI.GroupInfo groupInfo = this.groups.Find((ProfilerTimelineGUI.GroupInfo g) => g.name == groupname);
				if (groupInfo == null)
				{
					groupInfo = new ProfilerTimelineGUI.GroupInfo();
					groupInfo.name = groupname;
					groupInfo.height = 20f;
					groupInfo.expanded = false;
					groupInfo.threads = new List<ProfilerTimelineGUI.ThreadInfo>();
					this.groups.Add(groupInfo);
				}
				List<ProfilerTimelineGUI.ThreadInfo> threads = groupInfo.threads;
				ProfilerTimelineGUI.ThreadInfo threadInfo = threads.Find((ProfilerTimelineGUI.ThreadInfo t) => t.threadIndex == i);
				if (threadInfo == null)
				{
					threadInfo = new ProfilerTimelineGUI.ThreadInfo();
					threadInfo.name = iter.GetThreadName();
					threadInfo.height = 0f;
					threadInfo.weight = (threadInfo.desiredWeight = (float)((!groupInfo.expanded) ? 0 : 1));
					threadInfo.threadIndex = i;
					groupInfo.threads.Add(threadInfo);
				}
				threadInfo.alive = true;
				if (threadInfo.weight != threadInfo.desiredWeight)
				{
					threadInfo.weight = threadInfo.desiredWeight * time + (1f - threadInfo.desiredWeight) * (1f - time);
				}
				num += threadInfo.weight;
			}
			float num2 = 0f;
			foreach (ProfilerTimelineGUI.GroupInfo current in this.groups)
			{
				if (current.threads.Count > 1)
				{
					num2 += (current.expanded ? Math.Max(current.height, (float)current.threads.Count * 2f) : current.height);
				}
			}
			float num3 = r.height - num2;
			float num4 = num3 / (num + 1f);
			foreach (ProfilerTimelineGUI.GroupInfo current2 in this.groups)
			{
				foreach (ProfilerTimelineGUI.ThreadInfo current3 in current2.threads)
				{
					current3.height = num4 * current3.weight;
				}
			}
			this.groups[0].expanded = true;
			this.groups[0].height = 0f;
			if (this.groups[0].threads.Count > 0)
			{
				this.groups[0].threads[0].height = 2f * num4;
			}
		}

		private void UpdateAnimatedFoldout()
		{
			double num = EditorApplication.timeSinceStartup - this.lastScrollUpdate;
			this.animationTime = Math.Min(1f, this.animationTime + (float)num);
			this.m_Window.Repaint();
			if (this.animationTime == 1f)
			{
				EditorApplication.update = (EditorApplication.CallbackFunction)Delegate.Remove(EditorApplication.update, new EditorApplication.CallbackFunction(this.UpdateAnimatedFoldout));
			}
		}

		private bool DrawBar(Rect r, float y, float height, string name, bool group, bool expanded, bool indent)
		{
			Rect position = new Rect(r.x - 180f, y, 180f, height);
			Rect position2 = new Rect(r.x, y, r.width, height);
			if (Event.current.type == EventType.Repaint)
			{
				ProfilerTimelineGUI.styles.rightPane.Draw(position2, false, false, false, false);
				bool flag = height < 25f;
				GUIContent content = GUIContent.Temp(name);
				if (flag)
				{
					ProfilerTimelineGUI.styles.leftPane.padding.top -= (int)(25f - height) / 2;
				}
				if (indent)
				{
					ProfilerTimelineGUI.styles.leftPane.padding.left += 10;
				}
				ProfilerTimelineGUI.styles.leftPane.Draw(position, content, false, false, false, false);
				if (indent)
				{
					ProfilerTimelineGUI.styles.leftPane.padding.left -= 10;
				}
				if (flag)
				{
					ProfilerTimelineGUI.styles.leftPane.padding.top += (int)(25f - height) / 2;
				}
			}
			bool result;
			if (group)
			{
				position.width -= 1f;
				position.xMin += 1f;
				result = GUI.Toggle(position, expanded, GUIContent.none, ProfilerTimelineGUI.styles.foldout);
			}
			else
			{
				result = false;
			}
			return result;
		}

		private void DrawBars(Rect r, int frameIndex)
		{
			bool flag = false;
			foreach (ProfilerTimelineGUI.GroupInfo current in this.groups)
			{
				foreach (ProfilerTimelineGUI.ThreadInfo current2 in current.threads)
				{
					if (current2 != null)
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					break;
				}
			}
			if (flag)
			{
				float num = r.y;
				foreach (ProfilerTimelineGUI.GroupInfo current3 in this.groups)
				{
					bool flag2 = current3.name == "";
					if (!flag2)
					{
						float height = current3.height;
						bool expanded = current3.expanded;
						current3.expanded = this.DrawBar(r, num, height, current3.name, true, expanded, false);
						if (current3.expanded != expanded)
						{
							SessionState.SetBool(current3.name, current3.expanded);
							this.animationTime = 0f;
							this.lastScrollUpdate = EditorApplication.timeSinceStartup;
							EditorApplication.update = (EditorApplication.CallbackFunction)Delegate.Combine(EditorApplication.update, new EditorApplication.CallbackFunction(this.UpdateAnimatedFoldout));
							foreach (ProfilerTimelineGUI.ThreadInfo current4 in current3.threads)
							{
								current4.desiredWeight = ((!current3.expanded) ? 0f : 1f);
							}
						}
						num += height;
					}
					foreach (ProfilerTimelineGUI.ThreadInfo current5 in current3.threads)
					{
						float height2 = current5.height;
						if (height2 != 0f)
						{
							this.DrawBar(r, num, height2, current5.name, false, true, !flag2);
						}
						num += height2;
					}
				}
			}
		}

		private void DoNativeProfilerTimeline(Rect r, int frameIndex, int threadIndex, float timeOffset, bool ghost)
		{
			Rect rect = r;
			float num = Math.Min(rect.height * 0.25f, 1f);
			float num2 = num + 1f;
			rect.y += num;
			rect.height -= num2;
			GUI.BeginGroup(rect);
			Rect threadRect = rect;
			threadRect.x = 0f;
			threadRect.y = 0f;
			if (Event.current.type == EventType.Repaint)
			{
				this.DrawNativeProfilerTimeline(threadRect, frameIndex, threadIndex, timeOffset, ghost);
			}
			else if (Event.current.type == EventType.MouseDown && !ghost)
			{
				this.HandleNativeProfilerTimelineInput(threadRect, frameIndex, threadIndex, timeOffset, num);
			}
			GUI.EndGroup();
		}

		private void DrawNativeProfilerTimeline(Rect threadRect, int frameIndex, int threadIndex, float timeOffset, bool ghost)
		{
			bool flag = this.m_SelectedEntry.threadId == threadIndex && this.m_SelectedEntry.frameId == frameIndex;
			NativeProfilerTimeline_DrawArgs nativeProfilerTimeline_DrawArgs = default(NativeProfilerTimeline_DrawArgs);
			nativeProfilerTimeline_DrawArgs.Reset();
			nativeProfilerTimeline_DrawArgs.frameIndex = frameIndex;
			nativeProfilerTimeline_DrawArgs.threadIndex = threadIndex;
			nativeProfilerTimeline_DrawArgs.timeOffset = timeOffset;
			nativeProfilerTimeline_DrawArgs.threadRect = threadRect;
			nativeProfilerTimeline_DrawArgs.shownAreaRect = this.m_TimeArea.shownArea;
			nativeProfilerTimeline_DrawArgs.selectedEntryIndex = ((!flag) ? -1 : this.m_SelectedEntry.nativeIndex);
			nativeProfilerTimeline_DrawArgs.mousedOverEntryIndex = -1;
			NativeProfilerTimeline.Draw(ref nativeProfilerTimeline_DrawArgs);
		}

		private void HandleNativeProfilerTimelineInput(Rect threadRect, int frameIndex, int threadIndex, float timeOffset, float topMargin)
		{
			if (threadRect.Contains(Event.current.mousePosition))
			{
				bool flag = Event.current.clickCount == 1 && Event.current.type == EventType.MouseDown;
				bool flag2 = Event.current.clickCount == 2 && Event.current.type == EventType.MouseDown;
				bool flag3 = (flag || flag2) && Event.current.button == 0;
				if (flag3)
				{
					NativeProfilerTimeline_GetEntryAtPositionArgs nativeProfilerTimeline_GetEntryAtPositionArgs = default(NativeProfilerTimeline_GetEntryAtPositionArgs);
					nativeProfilerTimeline_GetEntryAtPositionArgs.Reset();
					nativeProfilerTimeline_GetEntryAtPositionArgs.frameIndex = frameIndex;
					nativeProfilerTimeline_GetEntryAtPositionArgs.threadIndex = threadIndex;
					nativeProfilerTimeline_GetEntryAtPositionArgs.timeOffset = timeOffset;
					nativeProfilerTimeline_GetEntryAtPositionArgs.threadRect = threadRect;
					nativeProfilerTimeline_GetEntryAtPositionArgs.shownAreaRect = this.m_TimeArea.shownArea;
					nativeProfilerTimeline_GetEntryAtPositionArgs.position = Event.current.mousePosition;
					NativeProfilerTimeline.GetEntryAtPosition(ref nativeProfilerTimeline_GetEntryAtPositionArgs);
					int out_EntryIndex = nativeProfilerTimeline_GetEntryAtPositionArgs.out_EntryIndex;
					if (out_EntryIndex != -1)
					{
						bool flag4 = !this.m_SelectedEntry.Equals(frameIndex, threadIndex, out_EntryIndex);
						if (flag4)
						{
							NativeProfilerTimeline_GetEntryTimingInfoArgs nativeProfilerTimeline_GetEntryTimingInfoArgs = default(NativeProfilerTimeline_GetEntryTimingInfoArgs);
							nativeProfilerTimeline_GetEntryTimingInfoArgs.Reset();
							nativeProfilerTimeline_GetEntryTimingInfoArgs.frameIndex = frameIndex;
							nativeProfilerTimeline_GetEntryTimingInfoArgs.threadIndex = threadIndex;
							nativeProfilerTimeline_GetEntryTimingInfoArgs.entryIndex = out_EntryIndex;
							nativeProfilerTimeline_GetEntryTimingInfoArgs.calculateFrameData = true;
							NativeProfilerTimeline.GetEntryTimingInfo(ref nativeProfilerTimeline_GetEntryTimingInfoArgs);
							NativeProfilerTimeline_GetEntryInstanceInfoArgs nativeProfilerTimeline_GetEntryInstanceInfoArgs = default(NativeProfilerTimeline_GetEntryInstanceInfoArgs);
							nativeProfilerTimeline_GetEntryInstanceInfoArgs.Reset();
							nativeProfilerTimeline_GetEntryInstanceInfoArgs.frameIndex = frameIndex;
							nativeProfilerTimeline_GetEntryInstanceInfoArgs.threadIndex = threadIndex;
							nativeProfilerTimeline_GetEntryInstanceInfoArgs.entryIndex = out_EntryIndex;
							NativeProfilerTimeline.GetEntryInstanceInfo(ref nativeProfilerTimeline_GetEntryInstanceInfoArgs);
							this.m_Window.SetSelectedPropertyPath(nativeProfilerTimeline_GetEntryInstanceInfoArgs.out_Path);
							this.m_SelectedEntry.Reset();
							this.m_SelectedEntry.frameId = frameIndex;
							this.m_SelectedEntry.threadId = threadIndex;
							this.m_SelectedEntry.nativeIndex = out_EntryIndex;
							this.m_SelectedEntry.instanceId = nativeProfilerTimeline_GetEntryInstanceInfoArgs.out_Id;
							this.m_SelectedEntry.time = nativeProfilerTimeline_GetEntryTimingInfoArgs.out_LocalStartTime;
							this.m_SelectedEntry.duration = nativeProfilerTimeline_GetEntryTimingInfoArgs.out_Duration;
							this.m_SelectedEntry.totalDuration = nativeProfilerTimeline_GetEntryTimingInfoArgs.out_TotalDurationForFrame;
							this.m_SelectedEntry.instanceCount = nativeProfilerTimeline_GetEntryTimingInfoArgs.out_InstanceCountForFrame;
							this.m_SelectedEntry.relativeYPos = nativeProfilerTimeline_GetEntryAtPositionArgs.out_EntryYMaxPos + topMargin;
							this.m_SelectedEntry.name = nativeProfilerTimeline_GetEntryAtPositionArgs.out_EntryName;
							this.m_SelectedEntry.callstackInfo = nativeProfilerTimeline_GetEntryInstanceInfoArgs.out_CallstackInfo;
							this.m_SelectedEntry.metaData = nativeProfilerTimeline_GetEntryInstanceInfoArgs.out_MetaData;
						}
						Event.current.Use();
						this.UpdateSelectedObject(flag, flag2);
					}
					else if (flag3)
					{
						this.ClearSelection();
						Event.current.Use();
					}
				}
			}
		}

		private void UpdateSelectedObject(bool singleClick, bool doubleClick)
		{
			UnityEngine.Object @object = EditorUtility.InstanceIDToObject(this.m_SelectedEntry.instanceId);
			if (@object is Component)
			{
				@object = ((Component)@object).gameObject;
			}
			if (@object != null)
			{
				if (singleClick)
				{
					EditorGUIUtility.PingObject(@object.GetInstanceID());
				}
				else if (doubleClick)
				{
					Selection.objects = new List<UnityEngine.Object>
					{
						@object
					}.ToArray();
				}
			}
		}

		private void ClearSelection()
		{
			this.m_Window.ClearSelectedPropertyPath();
			this.m_SelectedEntry.Reset();
			this.m_RangeSelection.active = false;
		}

		private void PerformFrameSelected(float frameMS)
		{
			float num;
			float num2;
			if (this.m_RangeSelection.active)
			{
				num = this.m_RangeSelection.startTime;
				num2 = this.m_RangeSelection.duration;
			}
			else
			{
				num = this.m_SelectedEntry.time;
				num2 = this.m_SelectedEntry.duration;
				if (this.m_SelectedEntry.instanceId < 0 || num2 <= 0f)
				{
					num = 0f;
					num2 = frameMS;
				}
			}
			this.m_TimeArea.SetShownHRangeInsideMargins(num - num2 * 0.2f, num + num2 * 1.2f);
		}

		private void HandleFrameSelected(float frameMS)
		{
			Event current = Event.current;
			if (current.type == EventType.ValidateCommand || current.type == EventType.ExecuteCommand)
			{
				if (current.commandName == "FrameSelected")
				{
					bool flag = current.type == EventType.ExecuteCommand;
					if (flag)
					{
						this.PerformFrameSelected(frameMS);
					}
					current.Use();
				}
			}
		}

		private void DoProfilerFrame(int frameIndex, Rect fullRect, bool ghost, int threadCount, float offset)
		{
			ProfilerFrameDataIterator profilerFrameDataIterator = new ProfilerFrameDataIterator();
			int threadCount2 = profilerFrameDataIterator.GetThreadCount(frameIndex);
			if (!ghost || threadCount2 == threadCount)
			{
				profilerFrameDataIterator.SetRoot(frameIndex, 0);
				if (!ghost)
				{
					this.HandleFrameSelected(profilerFrameDataIterator.frameTimeMS);
				}
				float num = fullRect.y;
				foreach (ProfilerTimelineGUI.GroupInfo current in this.groups)
				{
					Rect r = fullRect;
					bool expanded = current.expanded;
					if (expanded)
					{
						num += current.height;
					}
					float num2 = num;
					int count = current.threads.Count;
					foreach (ProfilerTimelineGUI.ThreadInfo current2 in current.threads)
					{
						profilerFrameDataIterator.SetRoot(frameIndex, current2.threadIndex);
						r.y = num;
						r.height = ((!expanded) ? Math.Max(current.height / (float)count - 1f, 2f) : current2.height);
						this.DoNativeProfilerTimeline(r, frameIndex, current2.threadIndex, offset, ghost);
						bool flag = this.m_SelectedEntry.IsValid() && this.m_SelectedEntry.frameId == frameIndex && this.m_SelectedEntry.threadId == current2.threadIndex;
						if (flag)
						{
							this.m_SelectedThreadY = num;
						}
						num += r.height;
					}
					if (!expanded)
					{
						num = num2 + Math.Max(current.height, (float)(count * 2));
					}
				}
			}
		}

		private void DoSelectionTooltip(int frameIndex, Rect fullRect)
		{
			if (this.m_SelectedEntry.IsValid() && this.m_SelectedEntry.frameId == frameIndex)
			{
				string arg = string.Format(((double)this.m_SelectedEntry.duration < 1.0) ? "{0:f3}ms" : "{0:f2}ms", this.m_SelectedEntry.duration);
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(string.Format("{0}\n{1}", this.m_SelectedEntry.name, arg));
				if (this.m_SelectedEntry.instanceCount > 1)
				{
					string text = string.Format(((double)this.m_SelectedEntry.totalDuration < 1.0) ? "{0:f3}ms" : "{0:f2}ms", this.m_SelectedEntry.totalDuration);
					stringBuilder.Append(string.Format("\n{0}: {1} ({2} {3})", new object[]
					{
						this.m_LocalizedString_Total,
						text,
						this.m_SelectedEntry.instanceCount,
						this.m_LocalizedString_Instances
					}));
				}
				if (this.m_SelectedEntry.metaData.Length > 0)
				{
					stringBuilder.Append(string.Format("\n{0}", this.m_SelectedEntry.metaData));
				}
				if (this.m_SelectedEntry.callstackInfo.Length > 0)
				{
					stringBuilder.Append(string.Format("\n{0}", this.m_SelectedEntry.callstackInfo));
				}
				float y = fullRect.y + this.m_SelectedThreadY + this.m_SelectedEntry.relativeYPos;
				float x = this.m_TimeArea.TimeToPixel(this.m_SelectedEntry.time + this.m_SelectedEntry.duration * 0.5f, fullRect);
				base.ShowLargeTooltip(new Vector2(x, y), fullRect, stringBuilder.ToString());
			}
		}

		public void MarkDeadOrClearThread()
		{
			foreach (ProfilerTimelineGUI.GroupInfo current in this.groups)
			{
				for (int i = current.threads.Count - 1; i >= 0; i--)
				{
					if (current.threads[i].alive)
					{
						current.threads[i].alive = false;
					}
					else
					{
						current.threads.RemoveAt(i);
					}
				}
			}
		}

		private void PrepareTicks()
		{
			this.m_HTicks.SetRanges(this.m_TimeArea.shownArea.xMin, this.m_TimeArea.shownArea.xMax, this.m_TimeArea.drawRect.xMin, this.m_TimeArea.drawRect.xMax);
			this.m_HTicks.SetTickStrengths(3f, 80f, true);
		}

		private void DrawGrid(Rect rect, float frameTime)
		{
			if (this.m_TimeArea != null && Event.current.type == EventType.Repaint)
			{
				GUI.BeginClip(rect);
				float num = 0f;
				rect.y = num;
				rect.x = num;
				Color textColor = ProfilerTimelineGUI.styles.timelineTick.normal.textColor;
				textColor.a = 0.1f;
				HandleUtility.ApplyWireMaterial();
				if (Application.platform == RuntimePlatform.WindowsEditor)
				{
					GL.Begin(7);
				}
				else
				{
					GL.Begin(1);
				}
				this.PrepareTicks();
				for (int i = 0; i < this.m_HTicks.tickLevels; i++)
				{
					float num2 = this.m_HTicks.GetStrengthOfLevel(i) * 0.9f;
					if (num2 > 0.5f)
					{
						float[] ticksAtLevel = this.m_HTicks.GetTicksAtLevel(i, true);
						for (int j = 0; j < ticksAtLevel.Length; j++)
						{
							float time = ticksAtLevel[j];
							float x = this.m_TimeArea.TimeToPixel(time, rect);
							TimeArea.DrawVerticalLineFast(x, 0f, rect.height, textColor);
						}
					}
				}
				GL.End();
				GUI.EndClip();
			}
		}

		public void DoTimeRulerGUI(Rect rect, float frameTime)
		{
			if (this.m_TimeArea != null && Event.current.type == EventType.Repaint)
			{
				GUI.BeginClip(rect);
				float num = 0f;
				rect.y = num;
				rect.x = num;
				GUI.Box(rect, GUIContent.none, EditorStyles.toolbarButton);
				Color textColor = ProfilerTimelineGUI.styles.timelineTick.normal.textColor;
				textColor.a *= 0.75f;
				this.PrepareTicks();
				if (Event.current.type == EventType.Repaint)
				{
					HandleUtility.ApplyWireMaterial();
					if (Application.platform == RuntimePlatform.WindowsEditor)
					{
						GL.Begin(7);
					}
					else
					{
						GL.Begin(1);
					}
					for (int i = 0; i < this.m_HTicks.tickLevels; i++)
					{
						float num2 = this.m_HTicks.GetStrengthOfLevel(i) * 0.8f;
						float[] ticksAtLevel = this.m_HTicks.GetTicksAtLevel(i, true);
						for (int j = 0; j < ticksAtLevel.Length; j++)
						{
							float time = ticksAtLevel[j];
							float x = this.m_TimeArea.TimeToPixel(time, rect);
							float num3 = rect.height * Mathf.Min(1f, num2) * 0.7f;
							Color color = new Color(1f, 1f, 1f, num2 / 0.5f) * textColor;
							TimeArea.DrawVerticalLineFast(x, rect.height - num3 + 0.5f, rect.height - 0.5f, color);
						}
					}
					GL.End();
				}
				int num4 = 60;
				int levelWithMinSeparation = this.m_HTicks.GetLevelWithMinSeparation((float)num4);
				float[] ticksAtLevel2 = this.m_HTicks.GetTicksAtLevel(levelWithMinSeparation, false);
				for (int k = 0; k < ticksAtLevel2.Length; k++)
				{
					float time2 = ticksAtLevel2[k];
					float num5 = Mathf.Floor(this.m_TimeArea.TimeToPixel(time2, rect));
					string text = this.FormatTickLabel(time2, levelWithMinSeparation);
					GUI.Label(new Rect(num5 + 3f, -3f, (float)num4, 20f), text, ProfilerTimelineGUI.styles.timelineTick);
				}
				this.DrawOutOfRangeOverlay(rect, frameTime);
				this.DrawRangeSelectionOverlay(rect);
				GUI.EndClip();
			}
		}

		private string FormatTickLabel(float time, int level)
		{
			string format = "{0}ms";
			float periodOfLevel = this.m_HTicks.GetPeriodOfLevel(level);
			int num = Mathf.FloorToInt(Mathf.Log10(periodOfLevel));
			if (num >= 3)
			{
				time /= 1000f;
				format = "{0}s";
			}
			return string.Format(format, time.ToString("N" + Mathf.Max(0, -num)));
		}

		private void DrawOutOfRangeOverlay(Rect rect, float frameTime)
		{
			Color outOfRangeColor = ProfilerTimelineGUI.styles.outOfRangeColor;
			Color frameDelimiterColor = ProfilerTimelineGUI.styles.frameDelimiterColor;
			float num = this.m_TimeArea.TimeToPixel(0f, rect);
			float num2 = this.m_TimeArea.TimeToPixel(frameTime, rect);
			if (num > rect.xMin)
			{
				Rect rect2 = Rect.MinMaxRect(rect.xMin, rect.yMin, Mathf.Min(num, rect.xMax), rect.yMax);
				EditorGUI.DrawRect(rect2, outOfRangeColor);
				TimeArea.DrawVerticalLine(rect2.xMax, rect2.yMin, rect2.yMax, frameDelimiterColor);
			}
			if (num2 < rect.xMax)
			{
				Rect rect3 = Rect.MinMaxRect(Mathf.Max(num2, rect.xMin), rect.yMin, rect.xMax, rect.yMax);
				EditorGUI.DrawRect(rect3, outOfRangeColor);
				TimeArea.DrawVerticalLine(rect3.xMin, rect3.yMin, rect3.yMax, frameDelimiterColor);
			}
		}

		private void DrawRangeSelectionOverlay(Rect rect)
		{
			if (this.m_RangeSelection.active)
			{
				float num = this.m_TimeArea.TimeToPixel(this.m_RangeSelection.startTime, rect);
				float num2 = this.m_TimeArea.TimeToPixel(this.m_RangeSelection.endTime, rect);
				if (num <= rect.xMax && num2 >= rect.xMin)
				{
					Rect rect2 = Rect.MinMaxRect(Mathf.Max(rect.xMin, num), rect.yMin, Mathf.Min(rect.xMax, num2), rect.yMax);
					EditorGUI.DrawRect(rect2, ProfilerTimelineGUI.styles.rangeSelectionColor);
					string text = string.Format("{0}ms", this.m_RangeSelection.duration.ToString("N3"));
					Chart.DoLabel(num + (num2 - num) / 2f, rect.yMin + 3f, text, -0.5f);
				}
			}
		}

		public void DoGUI(FrameDataView frameDataView, float width, float ypos, float height)
		{
			if (frameDataView == null || !frameDataView.IsValid())
			{
				GUILayout.Label(ProfilerFrameDataViewBase.BaseStyles.noData, ProfilerFrameDataViewBase.BaseStyles.label, new GUILayoutOption[0]);
			}
			else
			{
				Rect drawRect = new Rect(0f, ypos - 1f, width, height + 1f);
				float num = 179f;
				if (Event.current.type == EventType.Repaint)
				{
					ProfilerTimelineGUI.styles.profilerGraphBackground.Draw(drawRect, false, false, false, false);
					EditorStyles.toolbar.Draw(new Rect(0f, ypos + height - 15f, num, 15f), false, false, false, false);
				}
				bool flag = false;
				if (this.m_TimeArea == null)
				{
					flag = true;
					this.m_TimeArea = new ZoomableArea();
					this.m_TimeArea.hRangeLocked = false;
					this.m_TimeArea.vRangeLocked = true;
					this.m_TimeArea.hSlider = true;
					this.m_TimeArea.vSlider = false;
					this.m_TimeArea.scaleWithWindow = true;
					this.m_TimeArea.rect = new Rect(drawRect.x + num - 1f, drawRect.y, drawRect.width - num, drawRect.height);
					this.m_TimeArea.margin = 10f;
				}
				if (flag)
				{
					NativeProfilerTimeline_InitializeArgs nativeProfilerTimeline_InitializeArgs = default(NativeProfilerTimeline_InitializeArgs);
					nativeProfilerTimeline_InitializeArgs.Reset();
					nativeProfilerTimeline_InitializeArgs.ghostAlpha = 0.3f;
					nativeProfilerTimeline_InitializeArgs.nonSelectedAlpha = 0.75f;
					nativeProfilerTimeline_InitializeArgs.guiStyle = ProfilerTimelineGUI.styles.bar.m_Ptr;
					nativeProfilerTimeline_InitializeArgs.lineHeight = 16f;
					nativeProfilerTimeline_InitializeArgs.textFadeOutWidth = 20f;
					nativeProfilerTimeline_InitializeArgs.textFadeStartWidth = 50f;
					NativeProfilerTimeline.Initialize(ref nativeProfilerTimeline_InitializeArgs);
				}
				ProfilerFrameDataIterator profilerFrameDataIterator = new ProfilerFrameDataIterator();
				int threadCount = profilerFrameDataIterator.GetThreadCount(frameDataView.frameIndex);
				profilerFrameDataIterator.SetRoot(frameDataView.frameIndex, 0);
				this.m_TimeArea.hBaseRangeMin = 0f;
				this.m_TimeArea.hBaseRangeMax = profilerFrameDataIterator.frameTimeMS;
				if (flag)
				{
					this.PerformFrameSelected(profilerFrameDataIterator.frameTimeMS);
				}
				this.m_TimeArea.rect = new Rect(drawRect.x + num, drawRect.y, drawRect.width - num, drawRect.height);
				this.m_TimeArea.BeginViewGUI();
				this.m_TimeArea.EndViewGUI();
				drawRect = this.m_TimeArea.drawRect;
				this.DrawGrid(drawRect, profilerFrameDataIterator.frameTimeMS);
				this.MarkDeadOrClearThread();
				this.CalculateBars(ref profilerFrameDataIterator, drawRect, frameDataView.frameIndex, this.animationTime);
				this.DrawBars(drawRect, frameDataView.frameIndex);
				this.DoRangeSelection(this.m_TimeArea.drawRect);
				GUI.BeginClip(this.m_TimeArea.drawRect);
				drawRect.x = 0f;
				drawRect.y = 0f;
				bool enabled = GUI.enabled;
				GUI.enabled = false;
				int num2 = (!this.m_Window.IsRecording()) ? 3 : 1;
				int num3 = num2;
				int num4 = frameDataView.frameIndex;
				float num5 = 0f;
				do
				{
					int previousFrameIndex = ProfilerDriver.GetPreviousFrameIndex(num4);
					if (previousFrameIndex == -1)
					{
						break;
					}
					profilerFrameDataIterator.SetRoot(previousFrameIndex, 0);
					num5 -= profilerFrameDataIterator.frameTimeMS;
					num4 = previousFrameIndex;
					num3--;
				}
				while (num5 > this.m_TimeArea.shownArea.x && num3 > 0);
				while (num4 != -1 && num4 != frameDataView.frameIndex)
				{
					profilerFrameDataIterator.SetRoot(num4, 0);
					this.DoProfilerFrame(num4, drawRect, true, threadCount, num5);
					num5 += profilerFrameDataIterator.frameTimeMS;
					num4 = ProfilerDriver.GetNextFrameIndex(num4);
				}
				num3 = num2;
				num4 = frameDataView.frameIndex;
				num5 = 0f;
				while (num5 < this.m_TimeArea.shownArea.x + this.m_TimeArea.shownArea.width && num3 >= 0)
				{
					if (frameDataView.frameIndex != num4)
					{
						this.DoProfilerFrame(num4, drawRect, true, threadCount, num5);
					}
					profilerFrameDataIterator.SetRoot(num4, 0);
					num4 = ProfilerDriver.GetNextFrameIndex(num4);
					if (num4 == -1)
					{
						break;
					}
					num5 += profilerFrameDataIterator.frameTimeMS;
					num3--;
				}
				GUI.enabled = enabled;
				threadCount = 0;
				this.DoProfilerFrame(frameDataView.frameIndex, drawRect, false, threadCount, 0f);
				GUI.EndClip();
				this.DoSelectionTooltip(frameDataView.frameIndex, this.m_TimeArea.drawRect);
			}
		}

		private void DoRangeSelection(Rect rect)
		{
			int controlID = GUIUtility.GetControlID(ProfilerTimelineGUI.RangeSelectionInfo.controlIDHint, FocusType.Passive);
			Event current = Event.current;
			switch (current.type)
			{
			case EventType.MouseDown:
				if (GUIUtility.hotControl == 0 && current.button == 0 && rect.Contains(current.mousePosition))
				{
					DragAndDropDelay dragAndDropDelay = (DragAndDropDelay)GUIUtility.GetStateObject(typeof(DragAndDropDelay), controlID);
					dragAndDropDelay.mouseDownPosition = current.mousePosition;
					this.m_RangeSelection.mouseDown = true;
					this.m_RangeSelection.active = false;
				}
				break;
			case EventType.MouseUp:
				if (GUIUtility.hotControl == controlID && current.button == 0)
				{
					GUIUtility.hotControl = 0;
					this.m_RangeSelection.mouseDown = false;
					current.Use();
				}
				break;
			case EventType.MouseDrag:
				if (GUIUtility.hotControl == 0 && this.m_RangeSelection.mouseDown)
				{
					DragAndDropDelay dragAndDropDelay2 = (DragAndDropDelay)GUIUtility.GetStateObject(typeof(DragAndDropDelay), controlID);
					if (dragAndDropDelay2.CanStartDrag())
					{
						GUIUtility.hotControl = controlID;
						this.m_RangeSelection.mouseDownTime = this.m_TimeArea.PixelToTime(dragAndDropDelay2.mouseDownPosition.x, rect);
						this.m_RangeSelection.startTime = (this.m_RangeSelection.endTime = this.m_RangeSelection.mouseDownTime);
						this.ClearSelection();
						this.m_RangeSelection.active = true;
						current.Use();
					}
				}
				else if (GUIUtility.hotControl == controlID)
				{
					float num = this.m_TimeArea.PixelToTime(current.mousePosition.x, rect);
					if (num < this.m_RangeSelection.mouseDownTime)
					{
						this.m_RangeSelection.startTime = num;
						this.m_RangeSelection.endTime = this.m_RangeSelection.mouseDownTime;
					}
					else
					{
						this.m_RangeSelection.startTime = this.m_RangeSelection.mouseDownTime;
						this.m_RangeSelection.endTime = num;
					}
					current.Use();
				}
				break;
			case EventType.Repaint:
				if (this.m_RangeSelection.active)
				{
					float num2 = this.m_TimeArea.TimeToPixel(this.m_RangeSelection.startTime, rect);
					float num3 = this.m_TimeArea.TimeToPixel(this.m_RangeSelection.endTime, rect);
					if (num2 <= rect.xMax && num3 >= rect.xMin)
					{
						Rect position = Rect.MinMaxRect(Mathf.Max(rect.xMin, num2), rect.yMin, Mathf.Min(rect.xMax, num3), rect.yMax);
						ProfilerTimelineGUI.styles.rectangleToolSelection.Draw(position, false, false, false, false);
					}
				}
				break;
			}
		}

		internal void DrawToolbar(FrameDataView frameDataView)
		{
			EditorGUILayout.BeginHorizontal(EditorStyles.toolbar, new GUILayoutOption[]
			{
				GUILayout.Width(179f)
			});
			base.DrawViewTypePopup(ProfilerViewType.Timeline);
			EditorGUILayout.EndHorizontal();
			float height = EditorStyles.toolbar.CalcHeight(GUIContent.none, 0f);
			Rect controlRect = EditorGUILayout.GetControlRect(false, height, GUIStyle.none, new GUILayoutOption[]
			{
				GUILayout.ExpandWidth(true)
			});
			ProfilerFrameDataIterator profilerFrameDataIterator = new ProfilerFrameDataIterator();
			profilerFrameDataIterator.SetRoot(frameDataView.frameIndex, 0);
			float frameTimeMS = profilerFrameDataIterator.frameTimeMS;
			this.DoTimeRulerGUI(controlRect, frameTimeMS);
		}
	}
}
