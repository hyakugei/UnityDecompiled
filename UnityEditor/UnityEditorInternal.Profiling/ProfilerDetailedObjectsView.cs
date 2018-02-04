using System;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace UnityEditorInternal.Profiling
{
	[Serializable]
	internal class ProfilerDetailedObjectsView : ProfilerDetailedView
	{
		public delegate void FrameItemCallback(int id);

		private class ObjectInformation
		{
			public int id;

			public int instanceId;

			public string[] columnStrings;
		}

		private class ObjectsTreeView : TreeView
		{
			private List<ProfilerDetailedObjectsView.ObjectInformation> m_ObjectsData;

			public event ProfilerDetailedObjectsView.FrameItemCallback frameItemEvent
			{
				add
				{
					ProfilerDetailedObjectsView.FrameItemCallback frameItemCallback = this.frameItemEvent;
					ProfilerDetailedObjectsView.FrameItemCallback frameItemCallback2;
					do
					{
						frameItemCallback2 = frameItemCallback;
						frameItemCallback = Interlocked.CompareExchange<ProfilerDetailedObjectsView.FrameItemCallback>(ref this.frameItemEvent, (ProfilerDetailedObjectsView.FrameItemCallback)Delegate.Combine(frameItemCallback2, value), frameItemCallback);
					}
					while (frameItemCallback != frameItemCallback2);
				}
				remove
				{
					ProfilerDetailedObjectsView.FrameItemCallback frameItemCallback = this.frameItemEvent;
					ProfilerDetailedObjectsView.FrameItemCallback frameItemCallback2;
					do
					{
						frameItemCallback2 = frameItemCallback;
						frameItemCallback = Interlocked.CompareExchange<ProfilerDetailedObjectsView.FrameItemCallback>(ref this.frameItemEvent, (ProfilerDetailedObjectsView.FrameItemCallback)Delegate.Remove(frameItemCallback2, value), frameItemCallback);
					}
					while (frameItemCallback != frameItemCallback2);
				}
			}

			public ObjectsTreeView(TreeViewState treeViewState, ProfilerFrameDataMultiColumnHeader multicolumnHeader) : base(treeViewState, multicolumnHeader)
			{
				base.showBorder = true;
				base.showAlternatingRowBackgrounds = true;
				multicolumnHeader.sortingChanged += new MultiColumnHeader.HeaderCallback(this.OnSortingChanged);
				base.Reload();
			}

			public int GetSelectedFrameDataViewId()
			{
				int result;
				if (this.m_ObjectsData == null || base.state.selectedIDs.Count == 0)
				{
					result = -1;
				}
				else
				{
					int num = base.state.selectedIDs[0];
					if (num == -1)
					{
						result = -1;
					}
					else
					{
						result = this.m_ObjectsData[num].id;
					}
				}
				return result;
			}

			public void SetData(List<ProfilerDetailedObjectsView.ObjectInformation> objectsData)
			{
				this.m_ObjectsData = objectsData;
				this.OnSortingChanged(base.multiColumnHeader);
			}

			protected override TreeViewItem BuildRoot()
			{
				TreeViewItem treeViewItem = new TreeViewItem
				{
					id = -1,
					depth = -1
				};
				List<TreeViewItem> list = new List<TreeViewItem>();
				if (this.m_ObjectsData != null && this.m_ObjectsData.Count != 0)
				{
					list.Capacity = this.m_ObjectsData.Count;
					for (int i = 0; i < this.m_ObjectsData.Count; i++)
					{
						list.Add(new TreeViewItem
						{
							id = i,
							depth = 0
						});
					}
				}
				else
				{
					list.Add(new TreeViewItem
					{
						id = 0,
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
					if (this.m_ObjectsData == null || this.m_ObjectsData.Count == 0)
					{
						base.RowGUI(args);
					}
					else
					{
						TreeViewItem item = args.item;
						for (int i = 0; i < args.GetNumVisibleColumns(); i++)
						{
							this.CellGUI(args.GetCellRect(i), item, args.GetColumn(i), ref args);
						}
					}
				}
			}

			private void CellGUI(Rect cellRect, TreeViewItem item, int column, ref TreeView.RowGUIArgs args)
			{
				ProfilerDetailedObjectsView.ObjectInformation objectInformation = this.m_ObjectsData[args.item.id];
				base.CenterRectUsingSingleLineHeight(ref cellRect);
				TreeView.DefaultGUI.Label(cellRect, objectInformation.columnStrings[column], args.selected, args.focused);
			}

			private void OnSortingChanged(MultiColumnHeader header)
			{
				if (header.sortedColumnIndex != -1)
				{
					if (this.m_ObjectsData != null)
					{
						int orderMultiplier = (!header.IsSortedAscending(header.sortedColumnIndex)) ? -1 : 1;
						Comparison<ProfilerDetailedObjectsView.ObjectInformation> comparison = (ProfilerDetailedObjectsView.ObjectInformation objData1, ProfilerDetailedObjectsView.ObjectInformation objData2) => objData1.columnStrings[header.sortedColumnIndex].CompareTo(objData2.columnStrings[header.sortedColumnIndex]) * orderMultiplier;
						this.m_ObjectsData.Sort(comparison);
					}
					base.Reload();
				}
			}

			protected override void SingleClickedItem(int id)
			{
				if (this.m_ObjectsData != null)
				{
					int instanceId = this.m_ObjectsData[id].instanceId;
					UnityEngine.Object @object = EditorUtility.InstanceIDToObject(instanceId);
					if (@object is Component)
					{
						@object = ((Component)@object).gameObject;
					}
					if (@object != null)
					{
						EditorGUIUtility.PingObject(@object.GetInstanceID());
					}
				}
			}

			protected override void DoubleClickedItem(int id)
			{
				if (this.m_ObjectsData != null)
				{
					if (this.frameItemEvent != null)
					{
						this.frameItemEvent(this.m_ObjectsData[id].id);
					}
				}
			}

			protected override bool CanMultiSelect(TreeViewItem item)
			{
				return false;
			}
		}

		protected static readonly string kCallstackText = LocalizationDatabase.GetLocalizedString("Callstack:");

		[NonSerialized]
		private bool m_Initialized;

		[SerializeField]
		private TreeViewState m_TreeViewState;

		[SerializeField]
		private MultiColumnHeaderState m_MultiColumnHeaderState;

		[SerializeField]
		private SplitterState m_VertSplit;

		private ProfilerFrameDataMultiColumnHeader m_MultiColumnHeader;

		private ProfilerDetailedObjectsView.ObjectsTreeView m_TreeView;

		private Vector2 m_CallstackScrollViewPos;

		public event ProfilerDetailedObjectsView.FrameItemCallback frameItemEvent
		{
			add
			{
				ProfilerDetailedObjectsView.FrameItemCallback frameItemCallback = this.frameItemEvent;
				ProfilerDetailedObjectsView.FrameItemCallback frameItemCallback2;
				do
				{
					frameItemCallback2 = frameItemCallback;
					frameItemCallback = Interlocked.CompareExchange<ProfilerDetailedObjectsView.FrameItemCallback>(ref this.frameItemEvent, (ProfilerDetailedObjectsView.FrameItemCallback)Delegate.Combine(frameItemCallback2, value), frameItemCallback);
				}
				while (frameItemCallback != frameItemCallback2);
			}
			remove
			{
				ProfilerDetailedObjectsView.FrameItemCallback frameItemCallback = this.frameItemEvent;
				ProfilerDetailedObjectsView.FrameItemCallback frameItemCallback2;
				do
				{
					frameItemCallback2 = frameItemCallback;
					frameItemCallback = Interlocked.CompareExchange<ProfilerDetailedObjectsView.FrameItemCallback>(ref this.frameItemEvent, (ProfilerDetailedObjectsView.FrameItemCallback)Delegate.Remove(frameItemCallback2, value), frameItemCallback);
				}
				while (frameItemCallback != frameItemCallback2);
			}
		}

		public bool gpuView
		{
			get;
			set;
		}

		private void InitIfNeeded()
		{
			if (!this.m_Initialized)
			{
				ProfilerColumn[] array = new ProfilerColumn[]
				{
					ProfilerColumn.ObjectName,
					ProfilerColumn.TotalPercent,
					ProfilerColumn.GCMemory,
					ProfilerColumn.TotalTime
				};
				ProfilerColumn[] array2 = new ProfilerColumn[]
				{
					ProfilerColumn.ObjectName,
					ProfilerColumn.TotalGPUPercent,
					ProfilerColumn.DrawCalls,
					ProfilerColumn.TotalGPUTime
				};
				ProfilerColumn[] profilerColumns = (!this.gpuView) ? array : array2;
				ProfilerColumn defaultSortColumn = (!this.gpuView) ? ProfilerColumn.TotalTime : ProfilerColumn.TotalGPUTime;
				ProfilerFrameDataMultiColumnHeader.Column[] columns = ProfilerFrameDataHierarchyView.CreateColumns(profilerColumns);
				MultiColumnHeaderState multiColumnHeaderState = ProfilerFrameDataHierarchyView.CreateDefaultMultiColumnHeaderState(columns, defaultSortColumn);
				multiColumnHeaderState.columns[0].minWidth = 60f;
				multiColumnHeaderState.columns[0].autoResize = true;
				multiColumnHeaderState.columns[0].allowToggleVisibility = false;
				if (MultiColumnHeaderState.CanOverwriteSerializedFields(this.m_MultiColumnHeaderState, multiColumnHeaderState))
				{
					MultiColumnHeaderState.OverwriteSerializedFields(this.m_MultiColumnHeaderState, multiColumnHeaderState);
				}
				bool flag = this.m_MultiColumnHeaderState == null;
				this.m_MultiColumnHeaderState = multiColumnHeaderState;
				this.m_MultiColumnHeader = new ProfilerFrameDataMultiColumnHeader(this.m_MultiColumnHeaderState, columns)
				{
					height = 25f
				};
				if (flag)
				{
					this.m_MultiColumnHeader.ResizeToFit();
				}
				if (this.m_TreeViewState == null)
				{
					this.m_TreeViewState = new TreeViewState();
				}
				this.m_TreeView = new ProfilerDetailedObjectsView.ObjectsTreeView(this.m_TreeViewState, this.m_MultiColumnHeader);
				this.m_TreeView.frameItemEvent += this.frameItemEvent;
				if (this.m_VertSplit == null || this.m_VertSplit.relativeSizes == null || this.m_VertSplit.relativeSizes.Length == 0)
				{
					this.m_VertSplit = new SplitterState(new float[]
					{
						60f,
						40f
					}, new int[]
					{
						50,
						50
					}, null);
				}
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
				this.InitIfNeeded();
				this.UpdateIfNeeded(frameDataView, selection[0]);
				string text = null;
				int selectedFrameDataViewId = this.m_TreeView.GetSelectedFrameDataViewId();
				if (selectedFrameDataViewId != -1)
				{
					text = frameDataView.ResolveItemCallstack(selectedFrameDataViewId, this.m_TreeView.state.selectedIDs[0]);
				}
				bool flag = !string.IsNullOrEmpty(text);
				if (flag)
				{
					SplitterGUILayout.BeginVerticalSplit(this.m_VertSplit, ProfilerDetailedView.Styles.expandedArea, new GUILayoutOption[0]);
				}
				Rect rect = EditorGUILayout.BeginVertical(ProfilerDetailedView.Styles.expandedArea, new GUILayoutOption[0]);
				this.m_TreeView.OnGUI(rect);
				EditorGUILayout.EndVertical();
				if (flag)
				{
					EditorGUILayout.BeginVertical(ProfilerDetailedView.Styles.expandedArea, new GUILayoutOption[0]);
					this.m_CallstackScrollViewPos = EditorGUILayout.BeginScrollView(this.m_CallstackScrollViewPos, ProfilerDetailedView.Styles.callstackScroll, new GUILayoutOption[0]);
					string text2 = ProfilerDetailedObjectsView.kCallstackText + '\n' + text;
					EditorGUILayout.TextArea(text2, ProfilerDetailedView.Styles.callstackTextArea, new GUILayoutOption[0]);
					EditorGUILayout.EndScrollView();
					EditorGUILayout.EndVertical();
					SplitterGUILayout.EndVerticalSplit();
				}
			}
		}

		private void UpdateIfNeeded(FrameDataView frameDataView, int selectedId)
		{
			if (this.m_SelectedID != selectedId || !object.Equals(this.m_FrameDataView, frameDataView))
			{
				this.m_FrameDataView = frameDataView;
				this.m_SelectedID = selectedId;
				this.m_TreeView.SetSelection(new List<int>());
				int itemSamplesCount = this.m_FrameDataView.GetItemSamplesCount(selectedId);
				int num = this.m_MultiColumnHeader.columns.Length;
				List<ProfilerDetailedObjectsView.ObjectInformation> list = new List<ProfilerDetailedObjectsView.ObjectInformation>();
				string[][] array = new string[num][];
				int[] itemInstanceIDs = this.m_FrameDataView.GetItemInstanceIDs(selectedId);
				for (int i = 0; i < num; i++)
				{
					array[i] = this.m_FrameDataView.GetItemColumnDatas(selectedId, this.m_MultiColumnHeader.columns[i].profilerColumn);
				}
				for (int j = 0; j < itemSamplesCount; j++)
				{
					ProfilerDetailedObjectsView.ObjectInformation objectInformation = new ProfilerDetailedObjectsView.ObjectInformation
					{
						columnStrings = new string[num]
					};
					objectInformation.id = selectedId;
					objectInformation.instanceId = ((j >= itemInstanceIDs.Length) ? 0 : itemInstanceIDs[j]);
					for (int k = 0; k < num; k++)
					{
						objectInformation.columnStrings[k] = ((j >= array[k].Length) ? string.Empty : array[k][j]);
					}
					list.Add(objectInformation);
				}
				this.m_TreeView.SetData(list);
			}
		}

		public void Clear()
		{
			if (this.m_TreeView != null)
			{
				this.m_TreeView.SetData(null);
			}
		}
	}
}
