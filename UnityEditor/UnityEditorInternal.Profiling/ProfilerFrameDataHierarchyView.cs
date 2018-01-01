using System;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace UnityEditorInternal.Profiling
{
	[Serializable]
	internal class ProfilerFrameDataHierarchyView : ProfilerFrameDataViewBase
	{
		private enum DetailedViewType
		{
			None,
			Objects,
			CallersAndCallees
		}

		public delegate void SelectionChangedCallback(int id);

		public delegate void SearchChangedCallback(string newSearch);

		private static readonly GUIContent[] kDetailedViewTypeNames = new GUIContent[]
		{
			EditorGUIUtility.TextContent("No Details"),
			EditorGUIUtility.TextContent("Show Related Objects"),
			EditorGUIUtility.TextContent("Show Calls")
		};

		private static readonly int[] kDetailedViewTypes = new int[]
		{
			0,
			1,
			2
		};

		[NonSerialized]
		private bool m_Initialized;

		[SerializeField]
		private TreeViewState m_TreeViewState;

		[SerializeField]
		private MultiColumnHeaderState m_MultiColumnHeaderState;

		[SerializeField]
		private ProfilerFrameDataHierarchyView.DetailedViewType m_DetailedViewType = ProfilerFrameDataHierarchyView.DetailedViewType.None;

		[SerializeField]
		private SplitterState m_DetailedViewSpliterState;

		private SearchField m_SearchField;

		private ProfilerFrameDataTreeView m_TreeView;

		[SerializeField]
		private ProfilerDetailedObjectsView m_DetailedObjectsView;

		[SerializeField]
		private ProfilerDetailedCallsView m_DetailedCallsView;

		public event ProfilerFrameDataHierarchyView.SelectionChangedCallback selectionChanged
		{
			add
			{
				ProfilerFrameDataHierarchyView.SelectionChangedCallback selectionChangedCallback = this.selectionChanged;
				ProfilerFrameDataHierarchyView.SelectionChangedCallback selectionChangedCallback2;
				do
				{
					selectionChangedCallback2 = selectionChangedCallback;
					selectionChangedCallback = Interlocked.CompareExchange<ProfilerFrameDataHierarchyView.SelectionChangedCallback>(ref this.selectionChanged, (ProfilerFrameDataHierarchyView.SelectionChangedCallback)Delegate.Combine(selectionChangedCallback2, value), selectionChangedCallback);
				}
				while (selectionChangedCallback != selectionChangedCallback2);
			}
			remove
			{
				ProfilerFrameDataHierarchyView.SelectionChangedCallback selectionChangedCallback = this.selectionChanged;
				ProfilerFrameDataHierarchyView.SelectionChangedCallback selectionChangedCallback2;
				do
				{
					selectionChangedCallback2 = selectionChangedCallback;
					selectionChangedCallback = Interlocked.CompareExchange<ProfilerFrameDataHierarchyView.SelectionChangedCallback>(ref this.selectionChanged, (ProfilerFrameDataHierarchyView.SelectionChangedCallback)Delegate.Remove(selectionChangedCallback2, value), selectionChangedCallback);
				}
				while (selectionChangedCallback != selectionChangedCallback2);
			}
		}

		public event ProfilerFrameDataHierarchyView.SearchChangedCallback searchChanged
		{
			add
			{
				ProfilerFrameDataHierarchyView.SearchChangedCallback searchChangedCallback = this.searchChanged;
				ProfilerFrameDataHierarchyView.SearchChangedCallback searchChangedCallback2;
				do
				{
					searchChangedCallback2 = searchChangedCallback;
					searchChangedCallback = Interlocked.CompareExchange<ProfilerFrameDataHierarchyView.SearchChangedCallback>(ref this.searchChanged, (ProfilerFrameDataHierarchyView.SearchChangedCallback)Delegate.Combine(searchChangedCallback2, value), searchChangedCallback);
				}
				while (searchChangedCallback != searchChangedCallback2);
			}
			remove
			{
				ProfilerFrameDataHierarchyView.SearchChangedCallback searchChangedCallback = this.searchChanged;
				ProfilerFrameDataHierarchyView.SearchChangedCallback searchChangedCallback2;
				do
				{
					searchChangedCallback2 = searchChangedCallback;
					searchChangedCallback = Interlocked.CompareExchange<ProfilerFrameDataHierarchyView.SearchChangedCallback>(ref this.searchChanged, (ProfilerFrameDataHierarchyView.SearchChangedCallback)Delegate.Remove(searchChangedCallback2, value), searchChangedCallback);
				}
				while (searchChangedCallback != searchChangedCallback2);
			}
		}

		public ProfilerFrameDataTreeView treeView
		{
			get
			{
				return this.m_TreeView;
			}
		}

		public ProfilerDetailedObjectsView detailedObjectsView
		{
			get
			{
				return this.m_DetailedObjectsView;
			}
		}

		public ProfilerDetailedCallsView detailedCallsView
		{
			get
			{
				return this.m_DetailedCallsView;
			}
		}

		public ProfilerColumn sortedProfilerColumn
		{
			get
			{
				return (this.m_TreeView != null) ? this.m_TreeView.sortedProfilerColumn : ProfilerColumn.DontSort;
			}
		}

		public bool sortedProfilerColumnAscending
		{
			get
			{
				return this.m_TreeView != null && this.m_TreeView.sortedProfilerColumnAscending;
			}
		}

		public ProfilerFrameDataHierarchyView()
		{
			this.m_Initialized = false;
		}

		private void InitIfNeeded()
		{
			if (!this.m_Initialized)
			{
				ProfilerColumn[] array = new ProfilerColumn[]
				{
					ProfilerColumn.FunctionName,
					ProfilerColumn.TotalPercent,
					ProfilerColumn.SelfPercent,
					ProfilerColumn.Calls,
					ProfilerColumn.GCMemory,
					ProfilerColumn.TotalTime,
					ProfilerColumn.SelfTime,
					ProfilerColumn.WarningCount
				};
				ProfilerColumn[] array2 = new ProfilerColumn[]
				{
					ProfilerColumn.FunctionName,
					ProfilerColumn.TotalGPUPercent,
					ProfilerColumn.DrawCalls,
					ProfilerColumn.TotalGPUTime
				};
				ProfilerColumn[] profilerColumns = (!base.gpuView) ? array : array2;
				ProfilerColumn defaultSortColumn = (!base.gpuView) ? ProfilerColumn.TotalTime : ProfilerColumn.TotalGPUTime;
				ProfilerFrameDataMultiColumnHeader.Column[] columns = ProfilerFrameDataHierarchyView.CreateColumns(profilerColumns);
				MultiColumnHeaderState multiColumnHeaderState = ProfilerFrameDataHierarchyView.CreateDefaultMultiColumnHeaderState(columns, defaultSortColumn);
				if (MultiColumnHeaderState.CanOverwriteSerializedFields(this.m_MultiColumnHeaderState, multiColumnHeaderState))
				{
					MultiColumnHeaderState.OverwriteSerializedFields(this.m_MultiColumnHeaderState, multiColumnHeaderState);
				}
				bool flag = this.m_MultiColumnHeaderState == null;
				this.m_MultiColumnHeaderState = multiColumnHeaderState;
				ProfilerFrameDataMultiColumnHeader profilerFrameDataMultiColumnHeader = new ProfilerFrameDataMultiColumnHeader(this.m_MultiColumnHeaderState, columns)
				{
					height = 25f
				};
				if (flag)
				{
					profilerFrameDataMultiColumnHeader.ResizeToFit();
				}
				if (this.m_TreeViewState == null)
				{
					this.m_TreeViewState = new TreeViewState();
				}
				this.m_TreeView = new ProfilerFrameDataTreeView(this.m_TreeViewState, profilerFrameDataMultiColumnHeader);
				this.m_TreeView.selectionChanged += new ProfilerFrameDataTreeView.SelectionChangedCallback(this.OnMainTreeViewSelectionChanged);
				this.m_TreeView.searchChanged += new ProfilerFrameDataTreeView.SearchChangedCallback(this.OnMainTreeViewSearchChanged);
				this.m_SearchField = new SearchField();
				this.m_SearchField.downOrUpArrowKeyPressed += new SearchField.SearchFieldCallback(this.m_TreeView.SetFocusAndEnsureSelectedItem);
				if (this.m_DetailedObjectsView == null)
				{
					this.m_DetailedObjectsView = new ProfilerDetailedObjectsView();
				}
				this.m_DetailedObjectsView.gpuView = base.gpuView;
				this.m_DetailedObjectsView.frameItemEvent += new ProfilerDetailedObjectsView.FrameItemCallback(this.FrameItem);
				if (this.m_DetailedCallsView == null)
				{
					this.m_DetailedCallsView = new ProfilerDetailedCallsView();
				}
				this.m_DetailedCallsView.frameItemEvent += new ProfilerDetailedCallsView.FrameItemCallback(this.FrameItem);
				if (this.m_DetailedViewSpliterState == null || this.m_DetailedViewSpliterState.relativeSizes == null || this.m_DetailedViewSpliterState.relativeSizes.Length == 0)
				{
					this.m_DetailedViewSpliterState = new SplitterState(new float[]
					{
						70f,
						30f
					}, new int[]
					{
						450,
						50
					}, null);
				}
				this.m_Initialized = true;
			}
		}

		public static ProfilerFrameDataMultiColumnHeader.Column[] CreateColumns(ProfilerColumn[] profilerColumns)
		{
			ProfilerFrameDataMultiColumnHeader.Column[] array = new ProfilerFrameDataMultiColumnHeader.Column[profilerColumns.Length];
			for (int i = 0; i < profilerColumns.Length; i++)
			{
				string profilerColumnName = ProfilerFrameDataHierarchyView.GetProfilerColumnName(profilerColumns[i]);
				GUIContent headerLabel = (!profilerColumnName.StartsWith("|")) ? new GUIContent(profilerColumnName) : EditorGUIUtility.IconContent("ProfilerColumn." + profilerColumns[i], profilerColumnName);
				ProfilerFrameDataMultiColumnHeader.Column column = new ProfilerFrameDataMultiColumnHeader.Column
				{
					profilerColumn = profilerColumns[i],
					headerLabel = headerLabel
				};
				array[i] = column;
			}
			return array;
		}

		public static MultiColumnHeaderState CreateDefaultMultiColumnHeaderState(ProfilerFrameDataMultiColumnHeader.Column[] columns, ProfilerColumn defaultSortColumn)
		{
			MultiColumnHeaderState.Column[] array = new MultiColumnHeaderState.Column[columns.Length];
			for (int i = 0; i < columns.Length; i++)
			{
				int num = 80;
				int num2 = 50;
				float maxWidth = 1000000f;
				bool autoResize = false;
				bool allowToggleVisibility = true;
				ProfilerColumn profilerColumn = columns[i].profilerColumn;
				if (profilerColumn != ProfilerColumn.FunctionName)
				{
					if (profilerColumn == ProfilerColumn.WarningCount)
					{
						num = 25;
						num2 = 25;
						maxWidth = 25f;
					}
				}
				else
				{
					num = 200;
					num2 = 200;
					autoResize = true;
					allowToggleVisibility = false;
				}
				MultiColumnHeaderState.Column column = new MultiColumnHeaderState.Column
				{
					headerContent = columns[i].headerLabel,
					headerTextAlignment = TextAlignment.Left,
					sortingArrowAlignment = TextAlignment.Right,
					width = (float)num,
					minWidth = (float)num2,
					maxWidth = maxWidth,
					autoResize = autoResize,
					allowToggleVisibility = allowToggleVisibility,
					sortedAscending = (i == 0)
				};
				array[i] = column;
			}
			return new MultiColumnHeaderState(array)
			{
				sortedColumnIndex = ProfilerFrameDataMultiColumnHeader.GetMultiColumnHeaderIndex(columns, defaultSortColumn)
			};
		}

		private static string GetProfilerColumnName(ProfilerColumn column)
		{
			string result;
			switch (column)
			{
			case ProfilerColumn.FunctionName:
				result = LocalizationDatabase.GetLocalizedString("Overview");
				break;
			case ProfilerColumn.TotalPercent:
				result = LocalizationDatabase.GetLocalizedString("Total");
				break;
			case ProfilerColumn.SelfPercent:
				result = LocalizationDatabase.GetLocalizedString("Self");
				break;
			case ProfilerColumn.Calls:
				result = LocalizationDatabase.GetLocalizedString("Calls");
				break;
			case ProfilerColumn.GCMemory:
				result = LocalizationDatabase.GetLocalizedString("GC Alloc");
				break;
			case ProfilerColumn.TotalTime:
				result = LocalizationDatabase.GetLocalizedString("Time ms");
				break;
			case ProfilerColumn.SelfTime:
				result = LocalizationDatabase.GetLocalizedString("Self ms");
				break;
			case ProfilerColumn.DrawCalls:
				result = LocalizationDatabase.GetLocalizedString("DrawCalls");
				break;
			case ProfilerColumn.TotalGPUTime:
				result = LocalizationDatabase.GetLocalizedString("GPU ms");
				break;
			case ProfilerColumn.SelfGPUTime:
				result = LocalizationDatabase.GetLocalizedString("Self ms");
				break;
			case ProfilerColumn.TotalGPUPercent:
				result = LocalizationDatabase.GetLocalizedString("Total");
				break;
			case ProfilerColumn.SelfGPUPercent:
				result = LocalizationDatabase.GetLocalizedString("Self");
				break;
			case ProfilerColumn.WarningCount:
				result = LocalizationDatabase.GetLocalizedString("|Warnings");
				break;
			case ProfilerColumn.ObjectName:
				result = LocalizationDatabase.GetLocalizedString("Name");
				break;
			default:
				result = "ProfilerColumn." + column;
				break;
			}
			return result;
		}

		public void DoGUI(FrameDataView frameDataView)
		{
			this.InitIfNeeded();
			bool flag = frameDataView != null && frameDataView.IsValid();
			this.m_TreeView.SetFrameDataView(frameDataView);
			bool flag2 = flag && this.m_DetailedViewType != ProfilerFrameDataHierarchyView.DetailedViewType.None;
			if (flag2)
			{
				SplitterGUILayout.BeginHorizontalSplit(this.m_DetailedViewSpliterState, new GUILayoutOption[0]);
			}
			GUILayout.BeginVertical(new GUILayoutOption[0]);
			this.DrawToolbar(frameDataView, flag2);
			if (!flag)
			{
				GUILayout.Label(ProfilerFrameDataViewBase.BaseStyles.noData, ProfilerFrameDataViewBase.BaseStyles.label, new GUILayoutOption[0]);
			}
			else
			{
				Rect rect = GUILayoutUtility.GetRect(GUIContent.none, GUIStyle.none, new GUILayoutOption[]
				{
					GUILayout.ExpandHeight(true),
					GUILayout.ExpandHeight(true)
				});
				this.m_TreeView.OnGUI(rect);
			}
			GUILayout.EndVertical();
			if (flag2)
			{
				GUILayout.BeginVertical(new GUILayoutOption[0]);
				EditorGUILayout.BeginHorizontal(ProfilerFrameDataViewBase.BaseStyles.toolbar, new GUILayoutOption[0]);
				this.DrawDetailedViewPopup();
				GUILayout.FlexibleSpace();
				EditorGUILayout.EndHorizontal();
				ProfilerFrameDataHierarchyView.DetailedViewType detailedViewType = this.m_DetailedViewType;
				if (detailedViewType != ProfilerFrameDataHierarchyView.DetailedViewType.Objects)
				{
					if (detailedViewType == ProfilerFrameDataHierarchyView.DetailedViewType.CallersAndCallees)
					{
						this.detailedCallsView.DoGUI(ProfilerFrameDataViewBase.BaseStyles.header, frameDataView, this.m_TreeView.GetSelection());
					}
				}
				else
				{
					this.detailedObjectsView.DoGUI(ProfilerFrameDataViewBase.BaseStyles.header, frameDataView, this.m_TreeView.GetSelection());
				}
				GUILayout.EndVertical();
				SplitterGUILayout.EndHorizontalSplit();
			}
			this.HandleKeyboardEvents();
		}

		private void DrawSearchBar()
		{
			Rect rect = GUILayoutUtility.GetRect(50f, 300f, 16f, 16f, EditorStyles.toolbarSearchField);
			this.treeView.searchString = this.m_SearchField.OnToolbarGUI(rect, this.treeView.searchString);
		}

		private void DrawToolbar(FrameDataView frameDataView, bool showDetailedView)
		{
			EditorGUILayout.BeginHorizontal(ProfilerFrameDataViewBase.BaseStyles.toolbar, new GUILayoutOption[0]);
			base.DrawViewTypePopup(frameDataView.viewType);
			GUILayout.FlexibleSpace();
			if (frameDataView != null)
			{
				GUILayout.Label(string.Format("CPU:{0}ms   GPU:{1}ms", frameDataView.frameTime, frameDataView.frameGpuTime), EditorStyles.miniLabel, new GUILayoutOption[0]);
			}
			GUILayout.FlexibleSpace();
			this.DrawSearchBar();
			if (!showDetailedView)
			{
				this.DrawDetailedViewPopup();
			}
			EditorGUILayout.EndHorizontal();
		}

		private void DrawDetailedViewPopup()
		{
			this.m_DetailedViewType = (ProfilerFrameDataHierarchyView.DetailedViewType)EditorGUILayout.IntPopup((int)this.m_DetailedViewType, ProfilerFrameDataHierarchyView.kDetailedViewTypeNames, ProfilerFrameDataHierarchyView.kDetailedViewTypes, EditorStyles.toolbarDropDown, new GUILayoutOption[]
			{
				GUILayout.Width(120f)
			});
		}

		private void HandleKeyboardEvents()
		{
			if (this.m_TreeView.HasFocus() && this.m_TreeView.HasSelection())
			{
				Event current = Event.current;
				if (current.type == EventType.KeyDown && (current.keyCode == KeyCode.Return || current.keyCode == KeyCode.KeypadEnter))
				{
					this.SelectObjectsInHierarchyView();
				}
			}
		}

		private void SelectObjectsInHierarchyView()
		{
			IList<int> selectedInstanceIds = this.m_TreeView.GetSelectedInstanceIds();
			if (selectedInstanceIds != null && selectedInstanceIds.Count != 0)
			{
				List<UnityEngine.Object> list = new List<UnityEngine.Object>();
				foreach (int current in selectedInstanceIds)
				{
					UnityEngine.Object @object = EditorUtility.InstanceIDToObject(current);
					Component component = @object as Component;
					if (component != null)
					{
						list.Add(component.gameObject);
					}
					else if (@object != null)
					{
						list.Add(@object);
					}
				}
				if (list.Count != 0)
				{
					Selection.objects = list.ToArray();
				}
			}
		}

		private void OnMainTreeViewSelectionChanged(int id)
		{
			if (this.selectionChanged != null)
			{
				this.selectionChanged(id);
			}
		}

		private void OnMainTreeViewSearchChanged(string newSearch)
		{
			if (this.searchChanged != null)
			{
				this.searchChanged(newSearch);
			}
		}

		private void FrameItem(int id)
		{
			this.m_TreeView.SetFocus();
			this.m_TreeView.SetSelection(new List<int>(1)
			{
				id
			});
			this.m_TreeView.FrameItem(id);
		}

		public void SetSelectionFromLegacyPropertyPath(string selectedPropertyPath)
		{
			this.InitIfNeeded();
			this.m_TreeView.SetSelectionFromLegacyPropertyPath(selectedPropertyPath);
		}

		public override void Clear()
		{
			if (this.m_DetailedObjectsView != null)
			{
				this.m_DetailedObjectsView.Clear();
			}
			if (this.m_DetailedCallsView != null)
			{
				this.m_DetailedCallsView.Clear();
			}
			if (this.m_TreeView != null)
			{
				this.m_TreeView.Clear();
			}
		}
	}
}
