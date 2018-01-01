using System;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEditorInternal.Profiling;
using UnityEngine;

namespace UnityEditorInternal
{
	internal class ProfilerFrameDataTreeView : TreeView
	{
		public delegate void SelectionChangedCallback(int id);

		public delegate void SearchChangedCallback(string newSearch);

		private class FrameDataTreeViewItem : TreeViewItem
		{
			private FrameDataView m_FrameDataView;

			private bool m_Initialized;

			private string[] m_StringProperties;

			private string m_ResolvedCallstack;

			public string[] columnStrings
			{
				get
				{
					return this.m_StringProperties;
				}
			}

			public string resolvedCallstack
			{
				get
				{
					if (this.m_ResolvedCallstack == null)
					{
						this.m_ResolvedCallstack = this.m_FrameDataView.ResolveItemCallstack(this.id);
					}
					return this.m_ResolvedCallstack;
				}
			}

			public int samplesCount
			{
				get
				{
					return this.m_FrameDataView.GetItemSamplesCount(this.id);
				}
			}

			public FrameDataTreeViewItem(FrameDataView frameDataView, int id, int depth, TreeViewItem parent) : base(id, depth, parent, null)
			{
				this.m_FrameDataView = frameDataView;
				this.m_Initialized = false;
			}

			public void Init(ProfilerFrameDataMultiColumnHeader.Column[] columns)
			{
				if (!this.m_Initialized)
				{
					this.m_StringProperties = new string[columns.Length];
					for (int i = 0; i < columns.Length; i++)
					{
						string itemColumnData = this.m_FrameDataView.GetItemColumnData(this.id, columns[i].profilerColumn);
						this.m_StringProperties[i] = itemColumnData;
						if (columns[i].profilerColumn == ProfilerColumn.FunctionName)
						{
							this.displayName = itemColumnData;
						}
					}
					this.m_Initialized = true;
				}
			}
		}

		public static readonly GUIContent kFrameTooltip = EditorGUIUtility.TextContent("|Press 'F' to frame selection");

		private readonly List<TreeViewItem> m_Rows = new List<TreeViewItem>(1000);

		private ProfilerFrameDataMultiColumnHeader m_MultiColumnHeader;

		private FrameDataView m_FrameDataView;

		private FrameDataView.MarkerPath? m_SelectedItemMarkerIdPath;

		private string m_LegacySelectedItemMarkerNamePath;

		private HashSet<FrameDataView.MarkerPath> m_ExpandedMarkerIdPaths;

		public event ProfilerFrameDataTreeView.SelectionChangedCallback selectionChanged
		{
			add
			{
				ProfilerFrameDataTreeView.SelectionChangedCallback selectionChangedCallback = this.selectionChanged;
				ProfilerFrameDataTreeView.SelectionChangedCallback selectionChangedCallback2;
				do
				{
					selectionChangedCallback2 = selectionChangedCallback;
					selectionChangedCallback = Interlocked.CompareExchange<ProfilerFrameDataTreeView.SelectionChangedCallback>(ref this.selectionChanged, (ProfilerFrameDataTreeView.SelectionChangedCallback)Delegate.Combine(selectionChangedCallback2, value), selectionChangedCallback);
				}
				while (selectionChangedCallback != selectionChangedCallback2);
			}
			remove
			{
				ProfilerFrameDataTreeView.SelectionChangedCallback selectionChangedCallback = this.selectionChanged;
				ProfilerFrameDataTreeView.SelectionChangedCallback selectionChangedCallback2;
				do
				{
					selectionChangedCallback2 = selectionChangedCallback;
					selectionChangedCallback = Interlocked.CompareExchange<ProfilerFrameDataTreeView.SelectionChangedCallback>(ref this.selectionChanged, (ProfilerFrameDataTreeView.SelectionChangedCallback)Delegate.Remove(selectionChangedCallback2, value), selectionChangedCallback);
				}
				while (selectionChangedCallback != selectionChangedCallback2);
			}
		}

		public event ProfilerFrameDataTreeView.SearchChangedCallback searchChanged
		{
			add
			{
				ProfilerFrameDataTreeView.SearchChangedCallback searchChangedCallback = this.searchChanged;
				ProfilerFrameDataTreeView.SearchChangedCallback searchChangedCallback2;
				do
				{
					searchChangedCallback2 = searchChangedCallback;
					searchChangedCallback = Interlocked.CompareExchange<ProfilerFrameDataTreeView.SearchChangedCallback>(ref this.searchChanged, (ProfilerFrameDataTreeView.SearchChangedCallback)Delegate.Combine(searchChangedCallback2, value), searchChangedCallback);
				}
				while (searchChangedCallback != searchChangedCallback2);
			}
			remove
			{
				ProfilerFrameDataTreeView.SearchChangedCallback searchChangedCallback = this.searchChanged;
				ProfilerFrameDataTreeView.SearchChangedCallback searchChangedCallback2;
				do
				{
					searchChangedCallback2 = searchChangedCallback;
					searchChangedCallback = Interlocked.CompareExchange<ProfilerFrameDataTreeView.SearchChangedCallback>(ref this.searchChanged, (ProfilerFrameDataTreeView.SearchChangedCallback)Delegate.Remove(searchChangedCallback2, value), searchChangedCallback);
				}
				while (searchChangedCallback != searchChangedCallback2);
			}
		}

		public ProfilerColumn sortedProfilerColumn
		{
			get
			{
				return this.m_MultiColumnHeader.sortedProfilerColumn;
			}
		}

		public bool sortedProfilerColumnAscending
		{
			get
			{
				return this.m_MultiColumnHeader.sortedProfilerColumnAscending;
			}
		}

		public ProfilerFrameDataTreeView(TreeViewState state, ProfilerFrameDataMultiColumnHeader multicolumnHeader) : base(state, multicolumnHeader)
		{
			this.m_MultiColumnHeader = multicolumnHeader;
			this.m_MultiColumnHeader.sortingChanged += new MultiColumnHeader.HeaderCallback(this.OnSortingChanged);
		}

		public void SetFrameDataView(FrameDataView frameDataView)
		{
			bool flag = !object.Equals(this.m_FrameDataView, frameDataView);
			bool flag2 = frameDataView != null && (frameDataView.sortColumn != this.m_MultiColumnHeader.sortedProfilerColumn || frameDataView.sortColumnAscending != this.m_MultiColumnHeader.sortedProfilerColumnAscending);
			if (flag)
			{
				this.StoreExpandedState();
				this.StoreSelectedState();
			}
			this.m_FrameDataView = frameDataView;
			if (flag2)
			{
				this.m_FrameDataView.Sort(this.m_MultiColumnHeader.sortedProfilerColumn, this.m_MultiColumnHeader.sortedProfilerColumnAscending);
			}
			if (flag || flag2)
			{
				base.Reload();
			}
		}

		private void StoreExpandedState()
		{
			if (this.m_ExpandedMarkerIdPaths == null)
			{
				if (this.m_FrameDataView != null && this.m_FrameDataView.IsValid())
				{
					IList<int> expanded = base.GetExpanded();
					if (expanded.Count != 0)
					{
						this.m_ExpandedMarkerIdPaths = new HashSet<FrameDataView.MarkerPath>();
						foreach (int current in expanded)
						{
							FrameDataView.MarkerPath itemMarkerIDPath = this.m_FrameDataView.GetItemMarkerIDPath(current);
							this.m_ExpandedMarkerIdPaths.Add(itemMarkerIDPath);
						}
					}
				}
			}
		}

		public void SetSelectionFromLegacyPropertyPath(string selectedPropertyPath)
		{
			if (!string.IsNullOrEmpty(selectedPropertyPath))
			{
				this.m_LegacySelectedItemMarkerNamePath = selectedPropertyPath;
				this.m_SelectedItemMarkerIdPath = null;
			}
		}

		private void StoreSelectedState()
		{
			FrameDataView.MarkerPath? selectedItemMarkerIdPath = this.m_SelectedItemMarkerIdPath;
			if (!selectedItemMarkerIdPath.HasValue && this.m_LegacySelectedItemMarkerNamePath == null)
			{
				if (this.m_FrameDataView != null && this.m_FrameDataView.IsValid())
				{
					IList<int> selection = base.GetSelection();
					if (selection.Count != 0)
					{
						this.m_SelectedItemMarkerIdPath = new FrameDataView.MarkerPath?(this.m_FrameDataView.GetItemMarkerIDPath(selection[0]));
					}
				}
			}
		}

		private bool IsMigratedExpanded(int id)
		{
			bool result;
			if (this.m_ExpandedMarkerIdPaths == null)
			{
				result = base.IsExpanded(id);
			}
			else
			{
				FrameDataView.MarkerPath itemMarkerIDPath = this.m_FrameDataView.GetItemMarkerIDPath(id);
				result = this.m_ExpandedMarkerIdPaths.Contains(itemMarkerIDPath);
			}
			return result;
		}

		private void MigrateExpandedState(List<int> newExpandedIds)
		{
			if (newExpandedIds != null)
			{
				base.state.expandedIDs = newExpandedIds;
			}
		}

		private void MigrateSelectedState()
		{
			FrameDataView.MarkerPath? selectedItemMarkerIdPath = this.m_SelectedItemMarkerIdPath;
			if (selectedItemMarkerIdPath.HasValue || this.m_LegacySelectedItemMarkerNamePath != null)
			{
				int num = this.m_FrameDataView.GetRootItemID();
				FrameDataView.MarkerPath? selectedItemMarkerIdPath2 = this.m_SelectedItemMarkerIdPath;
				if (selectedItemMarkerIdPath2.HasValue)
				{
					foreach (int current in this.m_SelectedItemMarkerIdPath.Value.markerIds)
					{
						int[] itemChildren = this.m_FrameDataView.GetItemChildren(num);
						int[] array = itemChildren;
						for (int i = 0; i < array.Length; i++)
						{
							int num2 = array[i];
							if (current == this.m_FrameDataView.GetItemMarkerID(num2))
							{
								num = num2;
								break;
							}
						}
						if (num == 0)
						{
							break;
						}
					}
				}
				else if (this.m_LegacySelectedItemMarkerNamePath != null)
				{
					List<int> list = new List<int>();
					string[] array2 = this.m_LegacySelectedItemMarkerNamePath.Split(new char[]
					{
						'/'
					});
					string[] array3 = array2;
					for (int j = 0; j < array3.Length; j++)
					{
						string a = array3[j];
						int[] itemChildren2 = this.m_FrameDataView.GetItemChildren(num);
						int[] array4 = itemChildren2;
						for (int k = 0; k < array4.Length; k++)
						{
							int num3 = array4[k];
							if (a == this.m_FrameDataView.GetItemFunctionName(num3))
							{
								num = num3;
								list.Add(this.m_FrameDataView.GetItemMarkerID(num3));
								break;
							}
						}
						if (num == 0)
						{
							break;
						}
					}
					this.m_SelectedItemMarkerIdPath = new FrameDataView.MarkerPath?(new FrameDataView.MarkerPath(list));
					this.m_LegacySelectedItemMarkerNamePath = null;
				}
				List<int> selectedIDs = (num != 0) ? new List<int>
				{
					num
				} : new List<int>();
				base.state.selectedIDs = selectedIDs;
				base.FrameItem(num);
			}
		}

		public IList<int> GetSelectedInstanceIds()
		{
			IList<int> result;
			if (this.m_FrameDataView == null || !this.m_FrameDataView.IsValid())
			{
				result = null;
			}
			else
			{
				IList<int> selection = base.GetSelection();
				if (selection == null || selection.Count == 0)
				{
					result = null;
				}
				else
				{
					List<int> list = new List<int>();
					foreach (int current in selection)
					{
						list.AddRange(this.m_FrameDataView.GetItemInstanceIDs(current));
					}
					result = list;
				}
			}
			return result;
		}

		public void Clear()
		{
			if (this.m_FrameDataView != null)
			{
				this.m_FrameDataView.Dispose();
				this.m_FrameDataView = null;
				base.Reload();
			}
		}

		protected override TreeViewItem BuildRoot()
		{
			int id = (this.m_FrameDataView == null) ? 0 : this.m_FrameDataView.GetRootItemID();
			return new ProfilerFrameDataTreeView.FrameDataTreeViewItem(this.m_FrameDataView, id, -1, null);
		}

		protected override IList<TreeViewItem> BuildRows(TreeViewItem root)
		{
			this.m_Rows.Clear();
			IList<TreeViewItem> rows;
			if (this.m_FrameDataView == null || !this.m_FrameDataView.IsValid())
			{
				rows = this.m_Rows;
			}
			else
			{
				List<int> newExpandedIds = (this.m_ExpandedMarkerIdPaths != null) ? new List<int>(this.m_ExpandedMarkerIdPaths.Count) : null;
				if (!string.IsNullOrEmpty(base.searchString))
				{
					this.Search(root, base.searchString, this.m_Rows);
				}
				else
				{
					this.AddAllChildren((ProfilerFrameDataTreeView.FrameDataTreeViewItem)root, this.m_Rows, newExpandedIds);
				}
				this.MigrateExpandedState(newExpandedIds);
				this.MigrateSelectedState();
				rows = this.m_Rows;
			}
			return rows;
		}

		private void Search(TreeViewItem searchFromThis, string search, List<TreeViewItem> result)
		{
			if (searchFromThis == null)
			{
				throw new ArgumentException("Invalid searchFromThis: cannot be null", "searchFromThis");
			}
			if (string.IsNullOrEmpty(search))
			{
				throw new ArgumentException("Invalid search: cannot be null or empty", "search");
			}
			Stack<int> stack = new Stack<int>();
			if (this.m_FrameDataView.HasItemChildren(searchFromThis.id))
			{
				int[] itemChildren = this.m_FrameDataView.GetItemChildren(searchFromThis.id);
				int[] array = itemChildren;
				for (int i = 0; i < array.Length; i++)
				{
					int item = array[i];
					stack.Push(item);
				}
			}
			while (stack.Count > 0)
			{
				int id = stack.Pop();
				string itemFunctionName = this.m_FrameDataView.GetItemFunctionName(id);
				if (itemFunctionName.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0)
				{
					ProfilerFrameDataTreeView.FrameDataTreeViewItem frameDataTreeViewItem = new ProfilerFrameDataTreeView.FrameDataTreeViewItem(this.m_FrameDataView, id, 0, searchFromThis);
					searchFromThis.AddChild(frameDataTreeViewItem);
					result.Add(frameDataTreeViewItem);
				}
				if (this.m_FrameDataView.HasItemChildren(id))
				{
					int[] itemChildren2 = this.m_FrameDataView.GetItemChildren(id);
					int[] array2 = itemChildren2;
					for (int j = 0; j < array2.Length; j++)
					{
						int item2 = array2[j];
						stack.Push(item2);
					}
				}
			}
		}

		private void AddAllChildren(ProfilerFrameDataTreeView.FrameDataTreeViewItem parent, IList<TreeViewItem> newRows, List<int> newExpandedIds)
		{
			LinkedList<ProfilerFrameDataTreeView.FrameDataTreeViewItem> linkedList = new LinkedList<ProfilerFrameDataTreeView.FrameDataTreeViewItem>();
			linkedList.AddFirst(parent);
			while (linkedList.First != null)
			{
				ProfilerFrameDataTreeView.FrameDataTreeViewItem value = linkedList.First.Value;
				linkedList.RemoveFirst();
				if (value.depth != -1)
				{
					newRows.Add(value);
				}
				if (this.m_FrameDataView.HasItemChildren(value.id))
				{
					if (value.depth != -1)
					{
						if (!this.IsMigratedExpanded(value.id))
						{
							if (value.children == null)
							{
								value.children = TreeView.CreateChildListForCollapsedParent();
							}
							continue;
						}
						if (newExpandedIds != null)
						{
							newExpandedIds.Add(value.id);
						}
					}
					int[] itemChildren = this.m_FrameDataView.GetItemChildren(value.id);
					if (value.children != null)
					{
						value.children.Clear();
						value.children.Capacity = itemChildren.Length;
					}
					else
					{
						value.children = new List<TreeViewItem>(itemChildren.Length);
					}
					int[] array = itemChildren;
					for (int i = 0; i < array.Length; i++)
					{
						int id = array[i];
						ProfilerFrameDataTreeView.FrameDataTreeViewItem item = new ProfilerFrameDataTreeView.FrameDataTreeViewItem(this.m_FrameDataView, id, value.depth + 1, value);
						value.children.Add(item);
					}
					LinkedListNode<ProfilerFrameDataTreeView.FrameDataTreeViewItem> linkedListNode = null;
					foreach (TreeViewItem current in value.children)
					{
						linkedListNode = ((linkedListNode != null) ? linkedList.AddAfter(linkedListNode, (ProfilerFrameDataTreeView.FrameDataTreeViewItem)current) : linkedList.AddFirst((ProfilerFrameDataTreeView.FrameDataTreeViewItem)current));
					}
				}
			}
			if (newExpandedIds != null)
			{
				newExpandedIds.Sort();
			}
		}

		protected override bool CanMultiSelect(TreeViewItem item)
		{
			return false;
		}

		protected override void SelectionChanged(IList<int> selectedIds)
		{
			if (selectedIds.Count > 0)
			{
				this.m_SelectedItemMarkerIdPath = null;
				this.m_LegacySelectedItemMarkerNamePath = null;
			}
			int id = (selectedIds.Count <= 0) ? -1 : selectedIds[0];
			if (this.selectionChanged != null)
			{
				this.selectionChanged(id);
			}
		}

		protected override void ExpandedStateChanged()
		{
			this.m_ExpandedMarkerIdPaths = null;
		}

		protected override void DoubleClickedItem(int id)
		{
		}

		protected override void ContextClickedItem(int id)
		{
		}

		protected override void ContextClicked()
		{
		}

		protected override void SearchChanged(string newSearch)
		{
			if (this.searchChanged != null)
			{
				this.searchChanged(newSearch);
			}
		}

		protected override IList<int> GetAncestors(int id)
		{
			IList<int> result;
			if (this.m_FrameDataView == null)
			{
				result = new List<int>();
			}
			else
			{
				result = this.m_FrameDataView.GetItemAncestors(id);
			}
			return result;
		}

		protected override IList<int> GetDescendantsThatHaveChildren(int id)
		{
			IList<int> result;
			if (this.m_FrameDataView == null)
			{
				result = new List<int>();
			}
			else
			{
				result = this.m_FrameDataView.GetItemDescendantsThatHaveChildren(id);
			}
			return result;
		}

		private void OnSortingChanged(MultiColumnHeader header)
		{
			if (this.m_FrameDataView != null && base.multiColumnHeader.sortedColumnIndex != -1)
			{
				this.m_FrameDataView.Sort(this.m_MultiColumnHeader.sortedProfilerColumn, this.m_MultiColumnHeader.sortedProfilerColumnAscending);
				base.Reload();
			}
		}

		public override void OnGUI(Rect rect)
		{
			if (this.m_LegacySelectedItemMarkerNamePath != null)
			{
				this.MigrateSelectedState();
			}
			base.OnGUI(rect);
		}

		protected override void RowGUI(TreeView.RowGUIArgs args)
		{
			if (Event.current.rawType == EventType.Repaint)
			{
				ProfilerFrameDataTreeView.FrameDataTreeViewItem frameDataTreeViewItem = (ProfilerFrameDataTreeView.FrameDataTreeViewItem)args.item;
				frameDataTreeViewItem.Init(this.m_MultiColumnHeader.columns);
				for (int i = 0; i < args.GetNumVisibleColumns(); i++)
				{
					Rect cellRect = args.GetCellRect(i);
					this.CellGUI(cellRect, frameDataTreeViewItem, i == 0, args.GetColumn(i), ref args);
				}
				if (args.selected)
				{
					if (args.rowRect.Contains(Event.current.mousePosition))
					{
						if (base.hasSearch)
						{
							GUIStyle.SetMouseTooltip(ProfilerFrameDataTreeView.kFrameTooltip.tooltip, args.rowRect);
						}
					}
				}
			}
		}

		private void CellGUI(Rect cellRect, ProfilerFrameDataTreeView.FrameDataTreeViewItem item, bool needsIndent, int column, ref TreeView.RowGUIArgs args)
		{
			if (needsIndent)
			{
				float num = base.GetContentIndent(item) + base.extraSpaceBeforeIconAndLabel;
				cellRect.xMin += num;
			}
			base.CenterRectUsingSingleLineHeight(ref cellRect);
			GUIContent content = GUIContent.Temp(item.columnStrings[column], string.Empty);
			TreeView.DefaultStyles.label.Draw(cellRect, content, false, false, args.selected, args.focused);
		}
	}
}
