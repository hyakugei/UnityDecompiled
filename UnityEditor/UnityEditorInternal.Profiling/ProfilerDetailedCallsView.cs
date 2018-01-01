using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace UnityEditorInternal.Profiling
{
	[Serializable]
	internal class ProfilerDetailedCallsView : ProfilerDetailedView
	{
		public delegate void FrameItemCallback(int id);

		private struct CallsData
		{
			public List<ProfilerDetailedCallsView.CallInformation> calls;

			public float totalSelectedPropertyTime;
		}

		private class CallInformation
		{
			public int id;

			public string name;

			public int callsCount;

			public int gcAllocBytes;

			public double totalCallTimeMs;

			public double totalSelfTimeMs;

			public double timePercent;
		}

		private class CallsTreeView : TreeView
		{
			public enum Type
			{
				Callers,
				Callees
			}

			public enum Column
			{
				Name,
				Calls,
				GcAlloc,
				TimeMs,
				TimePercent,
				Count
			}

			internal ProfilerDetailedCallsView.CallsData m_CallsData;

			private ProfilerDetailedCallsView.CallsTreeView.Type m_Type;

			public event ProfilerDetailedCallsView.FrameItemCallback frameItemEvent
			{
				add
				{
					ProfilerDetailedCallsView.FrameItemCallback frameItemCallback = this.frameItemEvent;
					ProfilerDetailedCallsView.FrameItemCallback frameItemCallback2;
					do
					{
						frameItemCallback2 = frameItemCallback;
						frameItemCallback = Interlocked.CompareExchange<ProfilerDetailedCallsView.FrameItemCallback>(ref this.frameItemEvent, (ProfilerDetailedCallsView.FrameItemCallback)Delegate.Combine(frameItemCallback2, value), frameItemCallback);
					}
					while (frameItemCallback != frameItemCallback2);
				}
				remove
				{
					ProfilerDetailedCallsView.FrameItemCallback frameItemCallback = this.frameItemEvent;
					ProfilerDetailedCallsView.FrameItemCallback frameItemCallback2;
					do
					{
						frameItemCallback2 = frameItemCallback;
						frameItemCallback = Interlocked.CompareExchange<ProfilerDetailedCallsView.FrameItemCallback>(ref this.frameItemEvent, (ProfilerDetailedCallsView.FrameItemCallback)Delegate.Remove(frameItemCallback2, value), frameItemCallback);
					}
					while (frameItemCallback != frameItemCallback2);
				}
			}

			public CallsTreeView(ProfilerDetailedCallsView.CallsTreeView.Type type, TreeViewState treeViewState, MultiColumnHeader multicolumnHeader) : base(treeViewState, multicolumnHeader)
			{
				this.m_Type = type;
				base.showBorder = true;
				base.showAlternatingRowBackgrounds = true;
				multicolumnHeader.sortingChanged += new MultiColumnHeader.HeaderCallback(this.OnSortingChanged);
				base.Reload();
			}

			public void SetCallsData(ProfilerDetailedCallsView.CallsData callsData)
			{
				this.m_CallsData = callsData;
				if (this.m_CallsData.calls != null)
				{
					foreach (ProfilerDetailedCallsView.CallInformation current in this.m_CallsData.calls)
					{
						current.timePercent = ((this.m_Type != ProfilerDetailedCallsView.CallsTreeView.Type.Callees) ? (current.totalSelfTimeMs / current.totalCallTimeMs) : (current.totalCallTimeMs / (double)this.m_CallsData.totalSelectedPropertyTime));
					}
				}
				this.OnSortingChanged(base.multiColumnHeader);
			}

			protected override TreeViewItem BuildRoot()
			{
				TreeViewItem treeViewItem = new TreeViewItem
				{
					id = 0,
					depth = -1
				};
				List<TreeViewItem> list = new List<TreeViewItem>();
				if (this.m_CallsData.calls != null && this.m_CallsData.calls.Count != 0)
				{
					list.Capacity = this.m_CallsData.calls.Count;
					for (int i = 0; i < this.m_CallsData.calls.Count; i++)
					{
						list.Add(new TreeViewItem
						{
							id = i + 1,
							depth = 0,
							displayName = this.m_CallsData.calls[i].name
						});
					}
				}
				else
				{
					list.Add(new TreeViewItem
					{
						id = 1,
						depth = 0,
						displayName = ProfilerDetailedView.kNoneText
					});
				}
				TreeView.SetupParentsAndChildrenFromDepths(treeViewItem, list);
				return treeViewItem;
			}

			protected override void RowGUI(TreeView.RowGUIArgs args)
			{
				if (Event.current.rawType == EventType.Repaint)
				{
					for (int i = 0; i < args.GetNumVisibleColumns(); i++)
					{
						this.CellGUI(args.GetCellRect(i), args.item, (ProfilerDetailedCallsView.CallsTreeView.Column)args.GetColumn(i), ref args);
					}
				}
			}

			private void CellGUI(Rect cellRect, TreeViewItem item, ProfilerDetailedCallsView.CallsTreeView.Column column, ref TreeView.RowGUIArgs args)
			{
				if (this.m_CallsData.calls.Count == 0)
				{
					base.RowGUI(args);
				}
				else
				{
					ProfilerDetailedCallsView.CallInformation callInformation = this.m_CallsData.calls[args.item.id - 1];
					base.CenterRectUsingSingleLineHeight(ref cellRect);
					switch (column)
					{
					case ProfilerDetailedCallsView.CallsTreeView.Column.Name:
						TreeView.DefaultGUI.Label(cellRect, callInformation.name, args.selected, args.focused);
						break;
					case ProfilerDetailedCallsView.CallsTreeView.Column.Calls:
					{
						string label = callInformation.callsCount.ToString();
						TreeView.DefaultGUI.Label(cellRect, label, args.selected, args.focused);
						break;
					}
					case ProfilerDetailedCallsView.CallsTreeView.Column.GcAlloc:
					{
						int gcAllocBytes = callInformation.gcAllocBytes;
						TreeView.DefaultGUI.Label(cellRect, gcAllocBytes.ToString(), args.selected, args.focused);
						break;
					}
					case ProfilerDetailedCallsView.CallsTreeView.Column.TimeMs:
					{
						double num = (this.m_Type != ProfilerDetailedCallsView.CallsTreeView.Type.Callees) ? callInformation.totalSelfTimeMs : callInformation.totalCallTimeMs;
						TreeView.DefaultGUI.Label(cellRect, num.ToString("f2"), args.selected, args.focused);
						break;
					}
					case ProfilerDetailedCallsView.CallsTreeView.Column.TimePercent:
						TreeView.DefaultGUI.Label(cellRect, (callInformation.timePercent * 100.0).ToString("f2"), args.selected, args.focused);
						break;
					}
				}
			}

			private void OnSortingChanged(MultiColumnHeader header)
			{
				if (header.sortedColumnIndex != -1)
				{
					if (this.m_CallsData.calls != null)
					{
						int orderMultiplier = (!header.IsSortedAscending(header.sortedColumnIndex)) ? -1 : 1;
						Comparison<ProfilerDetailedCallsView.CallInformation> comparison;
						switch (header.sortedColumnIndex)
						{
						case 0:
							comparison = ((ProfilerDetailedCallsView.CallInformation callInfo1, ProfilerDetailedCallsView.CallInformation callInfo2) => callInfo1.name.CompareTo(callInfo2.name) * orderMultiplier);
							break;
						case 1:
							comparison = ((ProfilerDetailedCallsView.CallInformation callInfo1, ProfilerDetailedCallsView.CallInformation callInfo2) => callInfo1.callsCount.CompareTo(callInfo2.callsCount) * orderMultiplier);
							break;
						case 2:
							comparison = ((ProfilerDetailedCallsView.CallInformation callInfo1, ProfilerDetailedCallsView.CallInformation callInfo2) => callInfo1.gcAllocBytes.CompareTo(callInfo2.gcAllocBytes) * orderMultiplier);
							break;
						case 3:
							comparison = ((ProfilerDetailedCallsView.CallInformation callInfo1, ProfilerDetailedCallsView.CallInformation callInfo2) => callInfo1.totalCallTimeMs.CompareTo(callInfo2.totalCallTimeMs) * orderMultiplier);
							break;
						case 4:
							comparison = ((ProfilerDetailedCallsView.CallInformation callInfo1, ProfilerDetailedCallsView.CallInformation callInfo2) => callInfo1.timePercent.CompareTo(callInfo2.timePercent) * orderMultiplier);
							break;
						case 5:
							comparison = ((ProfilerDetailedCallsView.CallInformation callInfo1, ProfilerDetailedCallsView.CallInformation callInfo2) => callInfo1.callsCount.CompareTo(callInfo2.callsCount) * orderMultiplier);
							break;
						default:
							return;
						}
						this.m_CallsData.calls.Sort(comparison);
					}
					base.Reload();
				}
			}

			protected override void DoubleClickedItem(int id)
			{
				if (this.m_CallsData.calls != null && this.m_CallsData.calls.Count != 0)
				{
					if (this.frameItemEvent != null)
					{
						this.frameItemEvent(this.m_CallsData.calls[id - 1].id);
					}
				}
			}
		}

		[Serializable]
		private class CallsTreeViewController
		{
			private static class Styles
			{
				public static GUIContent callersLabel = EditorGUIUtility.TrTextContent("Called From", "Parents the selected function is called from\n\n(Press 'F' for frame selection)", null);

				public static GUIContent calleesLabel = EditorGUIUtility.TrTextContent("Calls To", "Functions which are called from the selected function\n\n(Press 'F' for frame selection)", null);

				public static GUIContent callsLabel = EditorGUIUtility.TrTextContent("Calls", "Total number of calls in a selected frame", null);

				public static GUIContent gcAllocLabel = EditorGUIUtility.TrTextContent("GC Alloc", null, null);

				public static GUIContent timeMsCallersLabel = EditorGUIUtility.TrTextContent("Time ms", "Total time the selected function spend within a parent", null);

				public static GUIContent timeMsCalleesLabel = EditorGUIUtility.TrTextContent("Time ms", "Total time the child call spend within selected function", null);

				public static GUIContent timePctCallersLabel = EditorGUIUtility.TrTextContent("Time %", "Shows how often the selected function was called from the parent call", null);

				public static GUIContent timePctCalleesLabel = EditorGUIUtility.TrTextContent("Time %", "Shows how often child call was called from the selected function", null);
			}

			[NonSerialized]
			private bool m_Initialized;

			[NonSerialized]
			private ProfilerDetailedCallsView.CallsTreeView.Type m_Type;

			[SerializeField]
			private TreeViewState m_ViewState;

			[SerializeField]
			private MultiColumnHeaderState m_ViewHeaderState;

			private ProfilerDetailedCallsView.CallsTreeView m_View;

			public event ProfilerDetailedCallsView.FrameItemCallback frameItemEvent
			{
				add
				{
					ProfilerDetailedCallsView.FrameItemCallback frameItemCallback = this.frameItemEvent;
					ProfilerDetailedCallsView.FrameItemCallback frameItemCallback2;
					do
					{
						frameItemCallback2 = frameItemCallback;
						frameItemCallback = Interlocked.CompareExchange<ProfilerDetailedCallsView.FrameItemCallback>(ref this.frameItemEvent, (ProfilerDetailedCallsView.FrameItemCallback)Delegate.Combine(frameItemCallback2, value), frameItemCallback);
					}
					while (frameItemCallback != frameItemCallback2);
				}
				remove
				{
					ProfilerDetailedCallsView.FrameItemCallback frameItemCallback = this.frameItemEvent;
					ProfilerDetailedCallsView.FrameItemCallback frameItemCallback2;
					do
					{
						frameItemCallback2 = frameItemCallback;
						frameItemCallback = Interlocked.CompareExchange<ProfilerDetailedCallsView.FrameItemCallback>(ref this.frameItemEvent, (ProfilerDetailedCallsView.FrameItemCallback)Delegate.Remove(frameItemCallback2, value), frameItemCallback);
					}
					while (frameItemCallback != frameItemCallback2);
				}
			}

			private void InitIfNeeded()
			{
				if (!this.m_Initialized)
				{
					if (this.m_ViewState == null)
					{
						this.m_ViewState = new TreeViewState();
					}
					bool flag = this.m_ViewHeaderState == null;
					MultiColumnHeaderState multiColumnHeaderState = this.CreateDefaultMultiColumnHeaderState();
					if (MultiColumnHeaderState.CanOverwriteSerializedFields(this.m_ViewHeaderState, multiColumnHeaderState))
					{
						MultiColumnHeaderState.OverwriteSerializedFields(this.m_ViewHeaderState, multiColumnHeaderState);
					}
					this.m_ViewHeaderState = multiColumnHeaderState;
					MultiColumnHeader multiColumnHeader = new MultiColumnHeader(this.m_ViewHeaderState)
					{
						height = 25f
					};
					if (flag)
					{
						multiColumnHeader.state.visibleColumns = new int[]
						{
							0,
							1,
							3,
							4
						};
						multiColumnHeader.ResizeToFit();
					}
					this.m_View = new ProfilerDetailedCallsView.CallsTreeView(this.m_Type, this.m_ViewState, multiColumnHeader);
					this.m_View.frameItemEvent += this.frameItemEvent;
					this.m_Initialized = true;
				}
			}

			private MultiColumnHeaderState CreateDefaultMultiColumnHeaderState()
			{
				MultiColumnHeaderState.Column[] columns = new MultiColumnHeaderState.Column[]
				{
					new MultiColumnHeaderState.Column
					{
						headerContent = ((this.m_Type != ProfilerDetailedCallsView.CallsTreeView.Type.Callers) ? ProfilerDetailedCallsView.CallsTreeViewController.Styles.calleesLabel : ProfilerDetailedCallsView.CallsTreeViewController.Styles.callersLabel),
						headerTextAlignment = TextAlignment.Left,
						sortedAscending = true,
						sortingArrowAlignment = TextAlignment.Center,
						width = 150f,
						minWidth = 150f,
						autoResize = true,
						allowToggleVisibility = false
					},
					new MultiColumnHeaderState.Column
					{
						headerContent = ProfilerDetailedCallsView.CallsTreeViewController.Styles.callsLabel,
						headerTextAlignment = TextAlignment.Left,
						sortedAscending = false,
						sortingArrowAlignment = TextAlignment.Right,
						width = 60f,
						minWidth = 60f,
						autoResize = false,
						allowToggleVisibility = true
					},
					new MultiColumnHeaderState.Column
					{
						headerContent = ProfilerDetailedCallsView.CallsTreeViewController.Styles.gcAllocLabel,
						headerTextAlignment = TextAlignment.Left,
						sortedAscending = false,
						sortingArrowAlignment = TextAlignment.Right,
						width = 60f,
						minWidth = 60f,
						autoResize = false,
						allowToggleVisibility = true
					},
					new MultiColumnHeaderState.Column
					{
						headerContent = ((this.m_Type != ProfilerDetailedCallsView.CallsTreeView.Type.Callers) ? ProfilerDetailedCallsView.CallsTreeViewController.Styles.timeMsCalleesLabel : ProfilerDetailedCallsView.CallsTreeViewController.Styles.timeMsCallersLabel),
						headerTextAlignment = TextAlignment.Left,
						sortedAscending = false,
						sortingArrowAlignment = TextAlignment.Right,
						width = 60f,
						minWidth = 60f,
						autoResize = false,
						allowToggleVisibility = true
					},
					new MultiColumnHeaderState.Column
					{
						headerContent = ((this.m_Type != ProfilerDetailedCallsView.CallsTreeView.Type.Callers) ? ProfilerDetailedCallsView.CallsTreeViewController.Styles.timePctCalleesLabel : ProfilerDetailedCallsView.CallsTreeViewController.Styles.timePctCallersLabel),
						headerTextAlignment = TextAlignment.Left,
						sortedAscending = false,
						sortingArrowAlignment = TextAlignment.Right,
						width = 60f,
						minWidth = 60f,
						autoResize = false,
						allowToggleVisibility = true
					}
				};
				return new MultiColumnHeaderState(columns)
				{
					sortedColumnIndex = 3
				};
			}

			public void SetType(ProfilerDetailedCallsView.CallsTreeView.Type type)
			{
				this.m_Type = type;
			}

			public void SetCallsData(ProfilerDetailedCallsView.CallsData callsData)
			{
				this.InitIfNeeded();
				this.m_View.SetCallsData(callsData);
			}

			public void OnGUI(Rect r)
			{
				this.InitIfNeeded();
				this.m_View.OnGUI(r);
			}
		}

		[NonSerialized]
		private bool m_Initialized = false;

		[NonSerialized]
		private float m_TotalSelectedPropertyTime;

		[NonSerialized]
		private GUIContent m_TotalSelectedPropertyTimeLabel = new GUIContent("", "Total time of all calls of the selected function in the frame.");

		[SerializeField]
		private SplitterState m_VertSplit;

		[SerializeField]
		private ProfilerDetailedCallsView.CallsTreeViewController m_CalleesTreeView;

		[SerializeField]
		private ProfilerDetailedCallsView.CallsTreeViewController m_CallersTreeView;

		public event ProfilerDetailedCallsView.FrameItemCallback frameItemEvent
		{
			add
			{
				ProfilerDetailedCallsView.FrameItemCallback frameItemCallback = this.frameItemEvent;
				ProfilerDetailedCallsView.FrameItemCallback frameItemCallback2;
				do
				{
					frameItemCallback2 = frameItemCallback;
					frameItemCallback = Interlocked.CompareExchange<ProfilerDetailedCallsView.FrameItemCallback>(ref this.frameItemEvent, (ProfilerDetailedCallsView.FrameItemCallback)Delegate.Combine(frameItemCallback2, value), frameItemCallback);
				}
				while (frameItemCallback != frameItemCallback2);
			}
			remove
			{
				ProfilerDetailedCallsView.FrameItemCallback frameItemCallback = this.frameItemEvent;
				ProfilerDetailedCallsView.FrameItemCallback frameItemCallback2;
				do
				{
					frameItemCallback2 = frameItemCallback;
					frameItemCallback = Interlocked.CompareExchange<ProfilerDetailedCallsView.FrameItemCallback>(ref this.frameItemEvent, (ProfilerDetailedCallsView.FrameItemCallback)Delegate.Remove(frameItemCallback2, value), frameItemCallback);
				}
				while (frameItemCallback != frameItemCallback2);
			}
		}

		private void InitIfNeeded()
		{
			if (!this.m_Initialized)
			{
				if (this.m_VertSplit == null || this.m_VertSplit.relativeSizes == null || this.m_VertSplit.relativeSizes.Length == 0)
				{
					this.m_VertSplit = new SplitterState(new float[]
					{
						40f,
						60f
					}, new int[]
					{
						50,
						50
					}, null);
				}
				if (this.m_CalleesTreeView == null)
				{
					this.m_CalleesTreeView = new ProfilerDetailedCallsView.CallsTreeViewController();
				}
				this.m_CalleesTreeView.SetType(ProfilerDetailedCallsView.CallsTreeView.Type.Callees);
				this.m_CalleesTreeView.frameItemEvent += this.frameItemEvent;
				if (this.m_CallersTreeView == null)
				{
					this.m_CallersTreeView = new ProfilerDetailedCallsView.CallsTreeViewController();
				}
				this.m_CallersTreeView.SetType(ProfilerDetailedCallsView.CallsTreeView.Type.Callers);
				this.m_CallersTreeView.frameItemEvent += this.frameItemEvent;
				this.m_Initialized = true;
			}
		}

		public void DoGUI(GUIStyle headerStyle, FrameDataView frameDataView, IList<int> selection)
		{
			if (frameDataView == null || !frameDataView.IsValid() || selection.Count == 0)
			{
				base.DrawEmptyPane(headerStyle);
			}
			else
			{
				int selectedId = selection[0];
				this.InitIfNeeded();
				this.UpdateIfNeeded(frameDataView, selectedId);
				GUILayout.Label(this.m_TotalSelectedPropertyTimeLabel, EditorStyles.label, new GUILayoutOption[0]);
				SplitterGUILayout.BeginVerticalSplit(this.m_VertSplit, new GUILayoutOption[]
				{
					GUILayout.ExpandWidth(true),
					GUILayout.ExpandHeight(true)
				});
				Rect r = EditorGUILayout.BeginHorizontal(new GUILayoutOption[0]);
				this.m_CalleesTreeView.OnGUI(r);
				EditorGUILayout.EndHorizontal();
				r = EditorGUILayout.BeginHorizontal(new GUILayoutOption[0]);
				this.m_CallersTreeView.OnGUI(r);
				EditorGUILayout.EndHorizontal();
				SplitterGUILayout.EndVerticalSplit();
			}
		}

		private void UpdateIfNeeded(FrameDataView frameDataView, int selectedId)
		{
			if (this.m_SelectedID != selectedId || !object.Equals(this.m_FrameDataView, frameDataView))
			{
				this.m_FrameDataView = frameDataView;
				this.m_SelectedID = selectedId;
				this.m_TotalSelectedPropertyTime = 0f;
				int itemMarkerID = this.m_FrameDataView.GetItemMarkerID(this.m_SelectedID);
				Dictionary<int, ProfilerDetailedCallsView.CallInformation> dictionary = new Dictionary<int, ProfilerDetailedCallsView.CallInformation>();
				Dictionary<int, ProfilerDetailedCallsView.CallInformation> dictionary2 = new Dictionary<int, ProfilerDetailedCallsView.CallInformation>();
				Stack<int> stack = new Stack<int>();
				stack.Push(this.m_FrameDataView.GetRootItemID());
				while (stack.Count > 0)
				{
					int num = stack.Pop();
					if (this.m_FrameDataView.HasItemChildren(num))
					{
						int itemMarkerID2 = this.m_FrameDataView.GetItemMarkerID(num);
						int[] itemChildren = this.m_FrameDataView.GetItemChildren(num);
						int[] array = itemChildren;
						for (int i = 0; i < array.Length; i++)
						{
							int num2 = array[i];
							int itemMarkerID3 = this.m_FrameDataView.GetItemMarkerID(num2);
							if (itemMarkerID3 == itemMarkerID)
							{
								float itemColumnDataAsSingle = this.m_FrameDataView.GetItemColumnDataAsSingle(num2, ProfilerColumn.TotalTime);
								this.m_TotalSelectedPropertyTime += itemColumnDataAsSingle;
								if (num != 0)
								{
									float itemColumnDataAsSingle2 = this.m_FrameDataView.GetItemColumnDataAsSingle(num, ProfilerColumn.TotalTime);
									int num3 = (int)this.m_FrameDataView.GetItemColumnDataAsSingle(num, ProfilerColumn.Calls);
									int num4 = (int)this.m_FrameDataView.GetItemColumnDataAsSingle(num, ProfilerColumn.GCMemory);
									ProfilerDetailedCallsView.CallInformation callInformation;
									if (!dictionary.TryGetValue(itemMarkerID2, out callInformation))
									{
										dictionary.Add(itemMarkerID2, new ProfilerDetailedCallsView.CallInformation
										{
											id = num,
											name = this.m_FrameDataView.GetItemFunctionName(num),
											callsCount = num3,
											gcAllocBytes = num4,
											totalCallTimeMs = (double)itemColumnDataAsSingle2,
											totalSelfTimeMs = (double)itemColumnDataAsSingle
										});
									}
									else
									{
										callInformation.callsCount += num3;
										callInformation.gcAllocBytes += num4;
										callInformation.totalCallTimeMs += (double)itemColumnDataAsSingle2;
										callInformation.totalSelfTimeMs += (double)itemColumnDataAsSingle;
									}
								}
							}
							if (itemMarkerID2 == itemMarkerID)
							{
								float itemColumnDataAsSingle3 = this.m_FrameDataView.GetItemColumnDataAsSingle(num2, ProfilerColumn.TotalTime);
								int num5 = (int)this.m_FrameDataView.GetItemColumnDataAsSingle(num2, ProfilerColumn.Calls);
								int num6 = (int)this.m_FrameDataView.GetItemColumnDataAsSingle(num2, ProfilerColumn.GCMemory);
								ProfilerDetailedCallsView.CallInformation callInformation2;
								if (!dictionary2.TryGetValue(itemMarkerID3, out callInformation2))
								{
									dictionary2.Add(itemMarkerID3, new ProfilerDetailedCallsView.CallInformation
									{
										id = num2,
										name = this.m_FrameDataView.GetItemFunctionName(num2),
										callsCount = num5,
										gcAllocBytes = num6,
										totalCallTimeMs = (double)itemColumnDataAsSingle3,
										totalSelfTimeMs = 0.0
									});
								}
								else
								{
									callInformation2.callsCount += num5;
									callInformation2.gcAllocBytes += num6;
									callInformation2.totalCallTimeMs += (double)itemColumnDataAsSingle3;
								}
							}
							stack.Push(num2);
						}
					}
				}
				this.m_CallersTreeView.SetCallsData(new ProfilerDetailedCallsView.CallsData
				{
					calls = dictionary.Values.ToList<ProfilerDetailedCallsView.CallInformation>(),
					totalSelectedPropertyTime = this.m_TotalSelectedPropertyTime
				});
				this.m_CalleesTreeView.SetCallsData(new ProfilerDetailedCallsView.CallsData
				{
					calls = dictionary2.Values.ToList<ProfilerDetailedCallsView.CallInformation>(),
					totalSelectedPropertyTime = this.m_TotalSelectedPropertyTime
				});
				this.m_TotalSelectedPropertyTimeLabel.text = this.m_FrameDataView.GetItemFunctionName(selectedId) + string.Format(" - Total time: {0:f2} ms", this.m_TotalSelectedPropertyTime);
			}
		}

		public void Clear()
		{
			if (this.m_CalleesTreeView != null)
			{
				this.m_CallersTreeView.SetCallsData(new ProfilerDetailedCallsView.CallsData
				{
					calls = null,
					totalSelectedPropertyTime = 0f
				});
			}
			if (this.m_CalleesTreeView != null)
			{
				this.m_CalleesTreeView.SetCallsData(new ProfilerDetailedCallsView.CallsData
				{
					calls = null,
					totalSelectedPropertyTime = 0f
				});
			}
		}
	}
}
