using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor.Accessibility;
using UnityEditorInternal;
using UnityEditorInternal.Profiling;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.Scripting;

namespace UnityEditor
{
	[EditorWindowTitle(title = "Profiler", useTypeNameAsIconName = true)]
	internal class ProfilerWindow : EditorWindow, IProfilerWindowController, IHasCustomMenu
	{
		internal static class Styles
		{
			public static readonly GUIContent addArea;

			public static readonly GUIContent deepProfile;

			public static readonly GUIContent profileEditor;

			public static readonly GUIContent noData;

			public static readonly GUIContent frameDebugger;

			public static readonly GUIContent noFrameDebugger;

			public static readonly GUIContent gatherObjectReferences;

			public static readonly GUIContent memRecord;

			public static readonly GUIContent profilerRecord;

			public static readonly GUIContent profilerInstrumentation;

			public static readonly GUIContent prevFrame;

			public static readonly GUIContent nextFrame;

			public static readonly GUIContent currentFrame;

			public static readonly GUIContent frame;

			public static readonly GUIContent clearOnPlay;

			public static readonly GUIContent clearData;

			public static readonly GUIContent saveProfilingData;

			public static readonly GUIContent loadProfilingData;

			public static readonly GUIContent[] reasons;

			public static readonly GUIContent accessibilityModeLabel;

			public static readonly GUIStyle background;

			public static readonly GUIStyle header;

			public static readonly GUIStyle label;

			public static readonly GUIStyle entryEven;

			public static readonly GUIStyle entryOdd;

			public static readonly GUIStyle profilerGraphBackground;

			static Styles()
			{
				ProfilerWindow.Styles.addArea = EditorGUIUtility.TrTextContent("Add Profiler", "Add a profiler area", null);
				ProfilerWindow.Styles.deepProfile = EditorGUIUtility.TrTextContent("Deep Profile", "Instrument all mono calls to investigate scripts", null);
				ProfilerWindow.Styles.profileEditor = EditorGUIUtility.TrTextContent("Profile Editor", "Enable profiling of the editor", null);
				ProfilerWindow.Styles.noData = EditorGUIUtility.TrTextContent("No frame data available", null, null);
				ProfilerWindow.Styles.frameDebugger = EditorGUIUtility.TrTextContent("Open Frame Debugger", "Frame Debugger for current game view", null);
				ProfilerWindow.Styles.noFrameDebugger = EditorGUIUtility.TrTextContent("Frame Debugger", "Open Frame Debugger (Current frame needs to be selected)", null);
				ProfilerWindow.Styles.gatherObjectReferences = EditorGUIUtility.TrTextContent("Gather object references", "Collect reference information to see where objects are referenced from. Disable this to save memory", null);
				ProfilerWindow.Styles.memRecord = EditorGUIUtility.TrTextContent("Mem Record", "Record activity in the native memory system", null);
				ProfilerWindow.Styles.profilerRecord = EditorGUIUtility.TrTextContentWithIcon("Record", "Record profiling information", "Profiler.Record");
				ProfilerWindow.Styles.profilerInstrumentation = EditorGUIUtility.TrTextContent("Instrumentation", "Add Profiler Instrumentation to selected functions", null);
				ProfilerWindow.Styles.prevFrame = EditorGUIUtility.TrIconContent("Profiler.PrevFrame", "Go back one frame");
				ProfilerWindow.Styles.nextFrame = EditorGUIUtility.TrIconContent("Profiler.NextFrame", "Go one frame forwards");
				ProfilerWindow.Styles.currentFrame = EditorGUIUtility.TrTextContent("Current", "Go to current frame", null);
				ProfilerWindow.Styles.frame = EditorGUIUtility.TrTextContent("Frame: ", null, null);
				ProfilerWindow.Styles.clearOnPlay = EditorGUIUtility.TrTextContent("Clear on Play", null, null);
				ProfilerWindow.Styles.clearData = EditorGUIUtility.TrTextContent("Clear", null, null);
				ProfilerWindow.Styles.saveProfilingData = EditorGUIUtility.TrTextContent("Save", "Save current profiling information to a binary file", null);
				ProfilerWindow.Styles.loadProfilingData = EditorGUIUtility.TrTextContent("Load", "Load binary profiling information from a file. Shift click to append to the existing data", null);
				ProfilerWindow.Styles.reasons = ProfilerWindow.Styles.GetLocalizedReasons();
				ProfilerWindow.Styles.accessibilityModeLabel = EditorGUIUtility.TrTextContent("Color Blind Mode", null, null);
				ProfilerWindow.Styles.background = "OL Box";
				ProfilerWindow.Styles.header = "OL title";
				ProfilerWindow.Styles.label = "OL label";
				ProfilerWindow.Styles.entryEven = "OL EntryBackEven";
				ProfilerWindow.Styles.entryOdd = "OL EntryBackOdd";
				ProfilerWindow.Styles.profilerGraphBackground = "ProfilerScrollviewBackground";
				ProfilerWindow.Styles.profilerGraphBackground.overflow.left = -180;
			}

			internal static GUIContent[] GetLocalizedReasons()
			{
				return new GUIContent[]
				{
					EditorGUIUtility.TrTextContent("Scene object (Unloaded by loading a new scene or destroying it)", null, null),
					EditorGUIUtility.TrTextContent("Builtin Resource (Never unloaded)", null, null),
					EditorGUIUtility.TrTextContent("Object is marked Don't Save. (Must be explicitly destroyed or it will leak)", null, null),
					EditorGUIUtility.TrTextContent("Asset is dirty and must be saved first (Editor only)", null, null),
					null,
					EditorGUIUtility.TrTextContent("Asset type created from code or stored in the scene, referenced from native code.", null, null),
					EditorGUIUtility.TrTextContent("Asset type created from code or stored in the scene, referenced from scripts and native code.", null, null),
					null,
					EditorGUIUtility.TrTextContent("Asset referenced from native code.", null, null),
					EditorGUIUtility.TrTextContent("Asset referenced from scripts and native code.", null, null),
					EditorGUIUtility.TrTextContent("Not Applicable", null, null)
				};
			}
		}

		private struct CachedProfilerPropertyConfig
		{
			public int frameIndex;

			public ProfilerArea area;

			public ProfilerViewType viewType;

			public ProfilerColumn sortType;
		}

		private static readonly ProfilerArea[] ms_StackedAreas = new ProfilerArea[]
		{
			ProfilerArea.CPU,
			ProfilerArea.GPU,
			ProfilerArea.UI,
			ProfilerArea.GlobalIllumination
		};

		[NonSerialized]
		private bool m_Initialized;

		[SerializeField]
		private SplitterState m_VertSplit;

		private SplitterState m_ViewSplit = new SplitterState(new float[]
		{
			70f,
			30f
		}, new int[]
		{
			450,
			50
		}, null);

		private SplitterState m_NetworkSplit = new SplitterState(new float[]
		{
			20f,
			80f
		}, new int[]
		{
			100,
			100
		}, null);

		[SerializeField]
		private bool m_Recording;

		private AttachProfilerUI m_AttachProfilerUI = new AttachProfilerUI();

		private Vector2 m_GraphPos = Vector2.zero;

		private Vector2[] m_PaneScroll = new Vector2[13];

		private Vector2 m_PaneScroll_AudioChannels = Vector2.zero;

		private Vector2 m_PaneScroll_AudioDSP = Vector2.zero;

		private Vector2 m_PaneScroll_AudioClips = Vector2.zero;

		[SerializeField]
		private string m_ActiveNativePlatformSupportModule;

		private static List<ProfilerWindow> m_ProfilerWindows = new List<ProfilerWindow>();

		[SerializeField]
		private ProfilerViewType m_ViewType = ProfilerViewType.Hierarchy;

		[SerializeField]
		private ProfilerArea m_CurrentArea = ProfilerArea.CPU;

		private ProfilerMemoryView m_ShowDetailedMemoryPane = ProfilerMemoryView.Simple;

		private ProfilerAudioView m_ShowDetailedAudioPane = ProfilerAudioView.Stats;

		[SerializeField]
		private bool m_ShowInactiveDSPChains = false;

		[SerializeField]
		private bool m_HighlightAudibleDSPChains = true;

		[SerializeField]
		private float m_DSPGraphZoomFactor = 1f;

		private int m_CurrentFrame = -1;

		private int m_LastFrameFromTick = -1;

		private int m_PrevLastFrame = -1;

		private int m_LastAudioProfilerFrame = -1;

		private ProfilerChart[] m_Charts;

		private float[] m_ChartOldMax = new float[]
		{
			-1f,
			-1f,
			0f,
			0f,
			0f,
			0f,
			0f,
			0f,
			0f,
			0f,
			-1f,
			0f,
			0f,
			0f
		};

		private float m_ChartMaxClamp = 70000f;

		private const float kRowHeight = 16f;

		private const float kIndentPx = 16f;

		private const float kBaseIndent = 8f;

		private const float kNameColumnSize = 350f;

		private FrameDataView m_FrameDataView;

		[SerializeField]
		private ProfilerFrameDataHierarchyView m_CPUFrameDataHierarchyView;

		[SerializeField]
		private ProfilerFrameDataHierarchyView m_GPUFrameDataHierarchyView;

		private ProfilerTimelineGUI m_CPUTimelineGUI;

		private ProfilerWindow.CachedProfilerPropertyConfig m_CPUOrGPUProfilerPropertyConfig;

		private ProfilerProperty m_CPUOrGPUProfilerProperty;

		[SerializeField]
		private UISystemProfiler m_UISystemProfiler;

		private MemoryTreeList m_ReferenceListView;

		private MemoryTreeListClickable m_MemoryListView;

		private bool m_GatherObjectReferences = true;

		[SerializeField]
		private AudioProfilerGroupTreeViewState m_AudioProfilerGroupTreeViewState;

		private AudioProfilerGroupView m_AudioProfilerGroupView = null;

		private AudioProfilerGroupViewBackend m_AudioProfilerGroupViewBackend;

		[SerializeField]
		private AudioProfilerClipTreeViewState m_AudioProfilerClipTreeViewState;

		private AudioProfilerClipView m_AudioProfilerClipView = null;

		private AudioProfilerClipViewBackend m_AudioProfilerClipViewBackend;

		private AudioProfilerDSPView m_AudioProfilerDSPView;

		private ProfilerMemoryRecordMode m_SelectedMemRecordMode = ProfilerMemoryRecordMode.None;

		private readonly char s_CheckMark = '✔';

		[SerializeField]
		private bool m_ClearOnPlay;

		private const string kProfilerRecentSaveLoadProfilePath = "ProfilerRecentSaveLoadProfilePath";

		private const string kProfilerEnabledSessionKey = "ProfilerEnabled";

		private string[] msgNames = new string[]
		{
			"UserMessage",
			"ObjectDestroy",
			"ClientRpc",
			"ObjectSpawn",
			"Owner",
			"Command",
			"LocalPlayerTransform",
			"SyncEvent",
			"SyncVars",
			"SyncList",
			"ObjectSpawnScene",
			"NetworkInfo",
			"SpawnFinished",
			"ObjectHide",
			"CRC",
			"ClientAuthority"
		};

		private bool[] msgFoldouts = new bool[]
		{
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true
		};

		private bool wantsMemoryRefresh
		{
			get
			{
				return this.m_MemoryListView.RequiresRefresh;
			}
		}

		private static string[] ProfilerColumnNames(ProfilerColumn[] columns)
		{
			string[] names = Enum.GetNames(typeof(ProfilerColumn));
			string[] array = new string[columns.Length];
			for (int i = 0; i < columns.Length; i++)
			{
				switch (columns[i])
				{
				case ProfilerColumn.FunctionName:
					array[i] = LocalizationDatabase.GetLocalizedString("Overview");
					break;
				case ProfilerColumn.TotalPercent:
					array[i] = LocalizationDatabase.GetLocalizedString("Total");
					break;
				case ProfilerColumn.SelfPercent:
					array[i] = LocalizationDatabase.GetLocalizedString("Self");
					break;
				case ProfilerColumn.Calls:
					array[i] = LocalizationDatabase.GetLocalizedString("Calls");
					break;
				case ProfilerColumn.GCMemory:
					array[i] = LocalizationDatabase.GetLocalizedString("GC Alloc");
					break;
				case ProfilerColumn.TotalTime:
					array[i] = LocalizationDatabase.GetLocalizedString("Time ms");
					break;
				case ProfilerColumn.SelfTime:
					array[i] = LocalizationDatabase.GetLocalizedString("Self ms");
					break;
				case ProfilerColumn.DrawCalls:
					array[i] = LocalizationDatabase.GetLocalizedString("DrawCalls");
					break;
				case ProfilerColumn.TotalGPUTime:
					array[i] = LocalizationDatabase.GetLocalizedString("GPU ms");
					break;
				case ProfilerColumn.SelfGPUTime:
					array[i] = LocalizationDatabase.GetLocalizedString("Self ms");
					break;
				case ProfilerColumn.TotalGPUPercent:
					array[i] = LocalizationDatabase.GetLocalizedString("Total");
					break;
				case ProfilerColumn.SelfGPUPercent:
					array[i] = LocalizationDatabase.GetLocalizedString("Self");
					break;
				case ProfilerColumn.WarningCount:
					array[i] = LocalizationDatabase.GetLocalizedString("|Warnings");
					break;
				case ProfilerColumn.ObjectName:
					array[i] = LocalizationDatabase.GetLocalizedString("Name");
					break;
				default:
					array[i] = "ProfilerColumn." + names[(int)columns[i]];
					break;
				}
			}
			return array;
		}

		public void SetSelectedPropertyPath(string path)
		{
			if (ProfilerDriver.selectedPropertyPath != path)
			{
				ProfilerDriver.selectedPropertyPath = path;
				this.UpdateCharts();
			}
		}

		public void ClearSelectedPropertyPath()
		{
			if (ProfilerDriver.selectedPropertyPath != string.Empty)
			{
				ProfilerDriver.selectedPropertyPath = string.Empty;
				this.UpdateCharts();
			}
		}

		public ProfilerProperty CreateProperty()
		{
			return this.CreateProperty(ProfilerColumn.DontSort);
		}

		public ProfilerProperty CreateProperty(ProfilerColumn sortType)
		{
			ProfilerProperty profilerProperty = new ProfilerProperty();
			profilerProperty.SetRoot(this.GetActiveVisibleFrameIndex(), sortType, this.m_ViewType);
			profilerProperty.onlyShowGPUSamples = (this.m_CurrentArea == ProfilerArea.GPU);
			return profilerProperty;
		}

		public int GetActiveVisibleFrameIndex()
		{
			return (this.m_CurrentFrame != -1) ? this.m_CurrentFrame : this.m_LastFrameFromTick;
		}

		public bool IsRecording()
		{
			return this.m_Recording && ((EditorApplication.isPlaying && !EditorApplication.isPaused) || !ProfilerDriver.IsConnectionEditor());
		}

		private void OnEnable()
		{
			this.InitializeIfNeeded();
			base.titleContent = base.GetLocalizedTitleContent();
			this.m_AttachProfilerUI.OnProfilerTargetChanged = new AttachProfilerUI.ProfilerTargetSelectionChangedDelegate(this.ClearFramesCallback);
			ProfilerWindow.m_ProfilerWindows.Add(this);
			EditorApplication.playModeStateChanged += new Action<PlayModeStateChange>(this.OnPlaymodeStateChanged);
			UserAccessiblitySettings.colorBlindConditionChanged = (Action)Delegate.Combine(UserAccessiblitySettings.colorBlindConditionChanged, new Action(this.Initialize));
		}

		private void InitializeIfNeeded()
		{
			if (!this.m_Initialized)
			{
				this.Initialize();
			}
		}

		private void Initialize()
		{
			int num = ProfilerDriver.maxHistoryLength - 1;
			this.m_Charts = new ProfilerChart[13];
			Color[] chartAreaColors = ProfilerColors.chartAreaColors;
			for (ProfilerArea profilerArea = ProfilerArea.CPU; profilerArea < ProfilerArea.AreaCount; profilerArea++)
			{
				float scale = 1f;
				Chart.ChartType chartType = Chart.ChartType.Line;
				string[] graphStatisticsPropertiesForArea = ProfilerDriver.GetGraphStatisticsPropertiesForArea(profilerArea);
				int num2 = graphStatisticsPropertiesForArea.Length;
				if (Array.IndexOf<ProfilerArea>(ProfilerWindow.ms_StackedAreas, profilerArea) != -1)
				{
					chartType = Chart.ChartType.StackedFill;
					scale = 0.001f;
				}
				ProfilerChart profilerChart = this.CreateProfilerChart(profilerArea, chartType, scale, num2);
				for (int i = 0; i < num2; i++)
				{
					profilerChart.m_Series[i] = new ChartSeriesViewData(graphStatisticsPropertiesForArea[i], num, chartAreaColors[i % chartAreaColors.Length]);
					for (int j = 0; j < num; j++)
					{
						profilerChart.m_Series[i].xValues[j] = (float)j;
					}
				}
				this.m_Charts[(int)profilerArea] = profilerChart;
			}
			if (this.m_VertSplit == null || this.m_VertSplit.relativeSizes == null || this.m_VertSplit.relativeSizes.Length == 0)
			{
				this.m_VertSplit = new SplitterState(new float[]
				{
					50f,
					50f
				}, new int[]
				{
					50,
					50
				}, null);
			}
			if (this.m_ReferenceListView == null)
			{
				this.m_ReferenceListView = new MemoryTreeList(this, null);
			}
			if (this.m_MemoryListView == null)
			{
				this.m_MemoryListView = new MemoryTreeListClickable(this, this.m_ReferenceListView);
			}
			if (this.m_CPUFrameDataHierarchyView == null)
			{
				this.m_CPUFrameDataHierarchyView = new ProfilerFrameDataHierarchyView();
			}
			this.m_CPUFrameDataHierarchyView.gpuView = false;
			this.m_CPUFrameDataHierarchyView.viewTypeChanged += new ProfilerFrameDataViewBase.ViewTypeChangedCallback(this.CPUOrGPUViewTypeChanged);
			this.m_CPUFrameDataHierarchyView.selectionChanged += new ProfilerFrameDataHierarchyView.SelectionChangedCallback(this.CPUOrGPUViewSelectionChanged);
			if (this.m_GPUFrameDataHierarchyView == null)
			{
				this.m_GPUFrameDataHierarchyView = new ProfilerFrameDataHierarchyView();
			}
			this.m_GPUFrameDataHierarchyView.gpuView = true;
			this.m_GPUFrameDataHierarchyView.viewTypeChanged += new ProfilerFrameDataViewBase.ViewTypeChangedCallback(this.CPUOrGPUViewTypeChanged);
			this.m_GPUFrameDataHierarchyView.selectionChanged += new ProfilerFrameDataHierarchyView.SelectionChangedCallback(this.CPUOrGPUViewSelectionChanged);
			this.m_CPUTimelineGUI = new ProfilerTimelineGUI(this);
			this.m_CPUTimelineGUI.viewTypeChanged += new ProfilerFrameDataViewBase.ViewTypeChangedCallback(this.CPUOrGPUViewTypeChanged);
			this.m_UISystemProfiler = new UISystemProfiler();
			this.UpdateCharts();
			ProfilerChart[] charts = this.m_Charts;
			for (int k = 0; k < charts.Length; k++)
			{
				ProfilerChart profilerChart2 = charts[k];
				profilerChart2.LoadAndBindSettings();
			}
			this.m_Initialized = true;
		}

		private void CPUOrGPUViewSelectionChanged(int id)
		{
			if (this.m_FrameDataView != null && this.m_FrameDataView.IsValid())
			{
				int[] itemAncestors = this.m_FrameDataView.GetItemAncestors(id);
				StringBuilder stringBuilder = new StringBuilder();
				int[] array = itemAncestors;
				for (int i = 0; i < array.Length; i++)
				{
					int id2 = array[i];
					stringBuilder.Append(this.m_FrameDataView.GetItemFunctionName(id2));
					stringBuilder.Append('/');
				}
				stringBuilder.Append(this.m_FrameDataView.GetItemFunctionName(id));
				this.SetSelectedPropertyPath(stringBuilder.ToString());
			}
		}

		private void CPUOrGPUViewTypeChanged(ProfilerViewType viewtype)
		{
			if (this.m_ViewType != viewtype)
			{
				if (this.m_ViewType == ProfilerViewType.Timeline)
				{
					this.m_CPUFrameDataHierarchyView.SetSelectionFromLegacyPropertyPath(ProfilerDriver.selectedPropertyPath);
				}
				this.m_ViewType = viewtype;
			}
		}

		private ProfilerChart CreateProfilerChart(ProfilerArea i, Chart.ChartType chartType, float scale, int length)
		{
			ProfilerChart profilerChart = (i != ProfilerArea.UIDetails) ? new ProfilerChart(i, chartType, scale, length) : new UISystemProfilerChart(chartType, scale, length);
			profilerChart.selected += new Chart.ChangedEventHandler(this.OnChartSelected);
			profilerChart.closed += new Chart.ChangedEventHandler(this.OnChartClosed);
			return profilerChart;
		}

		private void OnChartClosed(Chart sender)
		{
			ProfilerChart profilerChart = this.m_Charts[(int)this.m_CurrentArea];
			if (profilerChart == sender)
			{
				this.m_CurrentArea = ProfilerArea.AreaCount;
				this.m_UISystemProfiler.CurrentAreaChanged(this.m_CurrentArea);
			}
			ProfilerChart profilerChart2 = (ProfilerChart)sender;
			profilerChart2.active = false;
		}

		private void OnChartSelected(Chart sender)
		{
			ProfilerArea profilerArea = this.m_CurrentArea;
			int i = 0;
			int num = this.m_Charts.Length;
			while (i < num)
			{
				if (this.m_Charts[i] == sender)
				{
					profilerArea = (ProfilerArea)i;
					break;
				}
				i++;
			}
			if (this.m_CurrentArea != profilerArea)
			{
				this.m_CurrentArea = profilerArea;
				if (this.m_CurrentArea != ProfilerArea.CPU)
				{
					this.ClearSelectedPropertyPath();
				}
				this.m_UISystemProfiler.CurrentAreaChanged(this.m_CurrentArea);
				base.Repaint();
				GUIUtility.keyboardControl = 0;
				GUIUtility.ExitGUI();
			}
		}

		private void CheckForPlatformModuleChange()
		{
			if (this.m_ActiveNativePlatformSupportModule != EditorUtility.GetActiveNativePlatformSupportModuleName())
			{
				ProfilerDriver.ResetHistory();
				this.Initialize();
				this.m_ActiveNativePlatformSupportModule = EditorUtility.GetActiveNativePlatformSupportModuleName();
			}
		}

		private void OnDisable()
		{
			ProfilerWindow.m_ProfilerWindows.Remove(this);
			this.m_UISystemProfiler.CurrentAreaChanged(ProfilerArea.AreaCount);
			EditorApplication.playModeStateChanged -= new Action<PlayModeStateChange>(this.OnPlaymodeStateChanged);
			UserAccessiblitySettings.colorBlindConditionChanged = (Action)Delegate.Remove(UserAccessiblitySettings.colorBlindConditionChanged, new Action(this.Initialize));
		}

		private void Awake()
		{
			if (Profiler.supported)
			{
				this.m_Recording = SessionState.GetBool("ProfilerEnabled", true);
				ProfilerDriver.enabled = this.m_Recording;
				this.m_SelectedMemRecordMode = ProfilerDriver.memoryRecordMode;
			}
		}

		private void OnPlaymodeStateChanged(PlayModeStateChange stateChange)
		{
			if (stateChange == PlayModeStateChange.EnteredPlayMode)
			{
				this.ClearFramesCallback();
			}
		}

		private void ClearFramesCallback()
		{
			if (this.m_ClearOnPlay)
			{
				this.Clear();
			}
		}

		private void OnDestroy()
		{
			if (Profiler.supported && !EditorApplication.isPlayingOrWillChangePlaymode)
			{
				ProfilerDriver.enabled = false;
			}
		}

		private void OnFocus()
		{
			if (Profiler.supported)
			{
				ProfilerDriver.enabled = this.m_Recording;
			}
		}

		private void OnLostFocus()
		{
			if (GUIUtility.hotControl != 0)
			{
				for (int i = 0; i < this.m_Charts.Length; i++)
				{
					ProfilerChart profilerChart = this.m_Charts[i];
					profilerChart.OnLostFocus();
				}
			}
		}

		public virtual void AddItemsToMenu(GenericMenu menu)
		{
			menu.AddItem(ProfilerWindow.Styles.accessibilityModeLabel, UserAccessiblitySettings.colorBlindCondition != ColorBlindCondition.Default, new GenericMenu.MenuFunction(this.OnToggleColorBlindMode));
		}

		private void OnToggleColorBlindMode()
		{
			UserAccessiblitySettings.colorBlindCondition = ((UserAccessiblitySettings.colorBlindCondition != ColorBlindCondition.Default) ? ColorBlindCondition.Default : ColorBlindCondition.Deuteranopia);
		}

		private static void ShowProfilerWindow()
		{
			EditorWindow.GetWindow<ProfilerWindow>(false);
		}

		[RequiredByNativeCode]
		private static void RepaintAllProfilerWindows()
		{
			foreach (ProfilerWindow current in ProfilerWindow.m_ProfilerWindows)
			{
				if (ProfilerDriver.lastFrameIndex != current.m_LastFrameFromTick)
				{
					current.m_LastFrameFromTick = ProfilerDriver.lastFrameIndex;
					current.RepaintImmediately();
				}
			}
		}

		private static void SetMemoryProfilerInfo(ObjectMemoryInfo[] memoryInfo, int[] referencedIndices)
		{
			foreach (ProfilerWindow current in ProfilerWindow.m_ProfilerWindows)
			{
				if (current.wantsMemoryRefresh)
				{
					current.m_MemoryListView.SetRoot(MemoryElementDataManager.GetTreeRoot(memoryInfo, referencedIndices));
				}
			}
		}

		private static void SetProfileDeepScripts(bool deep)
		{
			bool deepProfiling = ProfilerDriver.deepProfiling;
			if (deepProfiling != deep)
			{
				bool flag = true;
				if (EditorApplication.isPlaying)
				{
					if (deep)
					{
						flag = EditorUtility.DisplayDialog("Enable deep script profiling", "Enabling deep profiling requires reloading scripts.", "Reload", "Cancel");
					}
					else
					{
						flag = EditorUtility.DisplayDialog("Disable deep script profiling", "Disabling deep profiling requires reloading all scripts", "Reload", "Cancel");
					}
				}
				if (flag)
				{
					ProfilerDriver.deepProfiling = deep;
					InternalEditorUtility.RequestScriptReload();
				}
			}
		}

		private string PickFrameLabel()
		{
			string result;
			if (this.m_CurrentFrame == -1)
			{
				result = "Current";
			}
			else
			{
				result = this.m_CurrentFrame + 1 + " / " + (ProfilerDriver.lastFrameIndex + 1);
			}
			return result;
		}

		private void PrevFrame()
		{
			int previousFrameIndex = ProfilerDriver.GetPreviousFrameIndex(this.m_CurrentFrame);
			if (previousFrameIndex != -1)
			{
				this.SetCurrentFrame(previousFrameIndex);
			}
		}

		private void NextFrame()
		{
			int nextFrameIndex = ProfilerDriver.GetNextFrameIndex(this.m_CurrentFrame);
			if (nextFrameIndex != -1)
			{
				this.SetCurrentFrame(nextFrameIndex);
			}
		}

		private void DrawCPUTimelineViewToolbar(ProfilerTimelineGUI timelineView, FrameDataView frameDataView)
		{
			EditorGUILayout.BeginHorizontal(EditorStyles.toolbar, new GUILayoutOption[0]);
			timelineView.DrawToolbar(frameDataView);
			EditorGUILayout.EndHorizontal();
			this.HandleCommandEvents();
		}

		private void HandleCommandEvents()
		{
		}

		private static bool CheckFrameData(ProfilerProperty property)
		{
			return property.frameDataReady;
		}

		private void DrawCPUOrGPUPane(ProfilerFrameDataHierarchyView hierarchyView, ProfilerTimelineGUI timelinePane)
		{
			FrameDataView frameDataView = this.GetFrameDataView(this.m_ViewType, hierarchyView.sortedProfilerColumn, hierarchyView.sortedProfilerColumnAscending);
			if (timelinePane != null && this.m_ViewType == ProfilerViewType.Timeline)
			{
				this.DrawCPUTimelineViewToolbar(timelinePane, frameDataView);
				float num = (float)this.m_VertSplit.realSizes[1];
				num -= EditorStyles.toolbar.CalcHeight(GUIContent.none, 10f) + 2f;
				timelinePane.DoGUI(frameDataView, base.position.width, base.position.height - num, num);
			}
			else
			{
				hierarchyView.DoGUI(frameDataView);
			}
		}

		public ProfilerProperty GetRootProfilerProperty(ProfilerColumn sortType)
		{
			ProfilerProperty cPUOrGPUProfilerProperty;
			if (this.m_CPUOrGPUProfilerProperty != null && this.m_CPUOrGPUProfilerPropertyConfig.frameIndex == this.GetActiveVisibleFrameIndex() && this.m_CPUOrGPUProfilerPropertyConfig.area == this.m_CurrentArea && this.m_CPUOrGPUProfilerPropertyConfig.viewType == this.m_ViewType && this.m_CPUOrGPUProfilerPropertyConfig.sortType == sortType)
			{
				this.m_CPUOrGPUProfilerProperty.ResetToRoot();
				cPUOrGPUProfilerProperty = this.m_CPUOrGPUProfilerProperty;
			}
			else
			{
				if (this.m_CPUOrGPUProfilerProperty != null)
				{
					this.m_CPUOrGPUProfilerProperty.Cleanup();
				}
				this.m_CPUOrGPUProfilerProperty = this.CreateProperty(sortType);
				this.m_CPUOrGPUProfilerPropertyConfig.frameIndex = this.GetActiveVisibleFrameIndex();
				this.m_CPUOrGPUProfilerPropertyConfig.area = this.m_CurrentArea;
				this.m_CPUOrGPUProfilerPropertyConfig.viewType = this.m_ViewType;
				this.m_CPUOrGPUProfilerPropertyConfig.sortType = sortType;
				cPUOrGPUProfilerProperty = this.m_CPUOrGPUProfilerProperty;
			}
			return cPUOrGPUProfilerProperty;
		}

		public FrameDataView GetFrameDataView(ProfilerViewType viewType, ProfilerColumn profilerSortColumn, bool sortAscending)
		{
			int activeVisibleFrameIndex = this.GetActiveVisibleFrameIndex();
			FrameDataView frameDataView;
			if (this.m_FrameDataView != null && this.m_FrameDataView.IsValid())
			{
				if (this.m_FrameDataView.frameIndex == activeVisibleFrameIndex && this.m_FrameDataView.viewType == viewType)
				{
					frameDataView = this.m_FrameDataView;
					return frameDataView;
				}
			}
			this.m_FrameDataView = new FrameDataView(viewType, activeVisibleFrameIndex, 0, profilerSortColumn, sortAscending);
			frameDataView = this.m_FrameDataView;
			return frameDataView;
		}

		private void DrawMemoryPane(SplitterState splitter)
		{
			this.DrawMemoryToolbar();
			if (this.m_ShowDetailedMemoryPane == ProfilerMemoryView.Simple)
			{
				this.DrawOverviewText(ProfilerArea.Memory);
			}
			else
			{
				this.DrawDetailedMemoryPane(splitter);
			}
		}

		private void DrawDetailedMemoryPane(SplitterState splitter)
		{
			SplitterGUILayout.BeginHorizontalSplit(splitter, new GUILayoutOption[0]);
			this.m_MemoryListView.OnGUI();
			this.m_ReferenceListView.OnGUI();
			SplitterGUILayout.EndHorizontalSplit();
		}

		private static Rect GenerateRect(ref int row, int indent)
		{
			Rect result = new Rect((float)indent * 16f + 8f, (float)row * 16f, 0f, 16f);
			result.xMax = 350f;
			row++;
			return result;
		}

		private void DrawNetworkOperationsPane()
		{
			SplitterGUILayout.BeginHorizontalSplit(this.m_NetworkSplit, new GUILayoutOption[0]);
			GUILayout.Label(ProfilerDriver.GetOverviewText(this.m_CurrentArea, this.GetActiveVisibleFrameIndex()), EditorStyles.wordWrappedLabel, new GUILayoutOption[0]);
			this.m_PaneScroll[(int)this.m_CurrentArea] = GUILayout.BeginScrollView(this.m_PaneScroll[(int)this.m_CurrentArea], ProfilerWindow.Styles.background);
			EditorGUILayout.BeginHorizontal(EditorStyles.toolbar, new GUILayoutOption[0]);
			EditorGUILayout.LabelField("Operation Detail", new GUILayoutOption[0]);
			EditorGUILayout.LabelField("Over 5 Ticks", new GUILayoutOption[0]);
			EditorGUILayout.LabelField("Over 10 Ticks", new GUILayoutOption[0]);
			EditorGUILayout.LabelField("Total", new GUILayoutOption[0]);
			EditorGUILayout.EndHorizontal();
			EditorGUI.indentLevel++;
			short num = 0;
			while ((int)num < this.msgNames.Length)
			{
				if (NetworkDetailStats.m_NetworkOperations.ContainsKey(num))
				{
					this.msgFoldouts[(int)num] = EditorGUILayout.Foldout(this.msgFoldouts[(int)num], this.msgNames[(int)num] + ":");
					if (this.msgFoldouts[(int)num])
					{
						EditorGUILayout.BeginVertical(new GUILayoutOption[0]);
						NetworkDetailStats.NetworkOperationDetails networkOperationDetails = NetworkDetailStats.m_NetworkOperations[num];
						EditorGUI.indentLevel++;
						foreach (string current in networkOperationDetails.m_Entries.Keys)
						{
							int tick = (int)Time.time;
							NetworkDetailStats.NetworkOperationEntryDetails networkOperationEntryDetails = networkOperationDetails.m_Entries[current];
							if (networkOperationEntryDetails.m_IncomingTotal > 0)
							{
								EditorGUILayout.BeginHorizontal(new GUILayoutOption[0]);
								EditorGUILayout.LabelField("IN:" + current, new GUILayoutOption[0]);
								EditorGUILayout.LabelField(networkOperationEntryDetails.m_IncomingSequence.GetFiveTick(tick).ToString(), new GUILayoutOption[0]);
								EditorGUILayout.LabelField(networkOperationEntryDetails.m_IncomingSequence.GetTenTick(tick).ToString(), new GUILayoutOption[0]);
								EditorGUILayout.LabelField(networkOperationEntryDetails.m_IncomingTotal.ToString(), new GUILayoutOption[0]);
								EditorGUILayout.EndHorizontal();
							}
							if (networkOperationEntryDetails.m_OutgoingTotal > 0)
							{
								EditorGUILayout.BeginHorizontal(new GUILayoutOption[0]);
								EditorGUILayout.LabelField("OUT:" + current, new GUILayoutOption[0]);
								EditorGUILayout.LabelField(networkOperationEntryDetails.m_OutgoingSequence.GetFiveTick(tick).ToString(), new GUILayoutOption[0]);
								EditorGUILayout.LabelField(networkOperationEntryDetails.m_OutgoingSequence.GetTenTick(tick).ToString(), new GUILayoutOption[0]);
								EditorGUILayout.LabelField(networkOperationEntryDetails.m_OutgoingTotal.ToString(), new GUILayoutOption[0]);
								EditorGUILayout.EndHorizontal();
							}
						}
						EditorGUI.indentLevel--;
						EditorGUILayout.EndVertical();
					}
				}
				num += 1;
			}
			EditorGUI.indentLevel--;
			GUILayout.EndScrollView();
			SplitterGUILayout.EndHorizontalSplit();
		}

		private void AudioProfilerToggle(ProfilerCaptureFlags toggleFlag)
		{
			bool flag = (AudioSettings.profilerCaptureFlags & (int)toggleFlag) != 0;
			bool flag2 = GUILayout.Toggle(flag, "Record", EditorStyles.toolbarButton, new GUILayoutOption[0]);
			if (flag != flag2)
			{
				ProfilerDriver.SetAudioCaptureFlags((AudioSettings.profilerCaptureFlags & (int)(~(int)toggleFlag)) | (int)((!flag2) ? ProfilerCaptureFlags.None : toggleFlag));
			}
		}

		private void DrawAudioPane()
		{
			EditorGUILayout.BeginHorizontal(EditorStyles.toolbar, new GUILayoutOption[0]);
			ProfilerAudioView profilerAudioView = this.m_ShowDetailedAudioPane;
			if (GUILayout.Toggle(profilerAudioView == ProfilerAudioView.Stats, "Stats", EditorStyles.toolbarButton, new GUILayoutOption[0]))
			{
				profilerAudioView = ProfilerAudioView.Stats;
			}
			if (GUILayout.Toggle(profilerAudioView == ProfilerAudioView.Channels, "Channels", EditorStyles.toolbarButton, new GUILayoutOption[0]))
			{
				profilerAudioView = ProfilerAudioView.Channels;
			}
			if (GUILayout.Toggle(profilerAudioView == ProfilerAudioView.Groups, "Groups", EditorStyles.toolbarButton, new GUILayoutOption[0]))
			{
				profilerAudioView = ProfilerAudioView.Groups;
			}
			if (GUILayout.Toggle(profilerAudioView == ProfilerAudioView.ChannelsAndGroups, "Channels and groups", EditorStyles.toolbarButton, new GUILayoutOption[0]))
			{
				profilerAudioView = ProfilerAudioView.ChannelsAndGroups;
			}
			if (Unsupported.IsDeveloperMode() && GUILayout.Toggle(profilerAudioView == ProfilerAudioView.DSPGraph, "DSP Graph", EditorStyles.toolbarButton, new GUILayoutOption[0]))
			{
				profilerAudioView = ProfilerAudioView.DSPGraph;
			}
			if (Unsupported.IsDeveloperMode() && GUILayout.Toggle(profilerAudioView == ProfilerAudioView.Clips, "Clips", EditorStyles.toolbarButton, new GUILayoutOption[0]))
			{
				profilerAudioView = ProfilerAudioView.Clips;
			}
			if (profilerAudioView != this.m_ShowDetailedAudioPane)
			{
				this.m_ShowDetailedAudioPane = profilerAudioView;
				this.m_LastAudioProfilerFrame = -1;
			}
			if (this.m_ShowDetailedAudioPane == ProfilerAudioView.Stats)
			{
				GUILayout.Space(5f);
				GUILayout.FlexibleSpace();
				EditorGUILayout.EndHorizontal();
				this.DrawOverviewText(this.m_CurrentArea);
			}
			else if (this.m_ShowDetailedAudioPane == ProfilerAudioView.DSPGraph)
			{
				GUILayout.Space(5f);
				this.AudioProfilerToggle(ProfilerCaptureFlags.DSPNodes);
				GUILayout.Space(5f);
				this.m_ShowInactiveDSPChains = GUILayout.Toggle(this.m_ShowInactiveDSPChains, "Show inactive", EditorStyles.toolbarButton, new GUILayoutOption[0]);
				if (this.m_ShowInactiveDSPChains)
				{
					this.m_HighlightAudibleDSPChains = GUILayout.Toggle(this.m_HighlightAudibleDSPChains, "Highlight audible", EditorStyles.toolbarButton, new GUILayoutOption[0]);
				}
				GUILayout.FlexibleSpace();
				EditorGUILayout.EndHorizontal();
				Rect rect = GUILayoutUtility.GetRect(20f, 10000f, 10f, 20000f);
				this.m_PaneScroll_AudioDSP = GUI.BeginScrollView(rect, this.m_PaneScroll_AudioDSP, new Rect(0f, 0f, 10000f, 20000f));
				Rect clippingRect = new Rect(this.m_PaneScroll_AudioDSP.x, this.m_PaneScroll_AudioDSP.y, rect.width, rect.height);
				if (this.m_AudioProfilerDSPView == null)
				{
					this.m_AudioProfilerDSPView = new AudioProfilerDSPView();
				}
				ProfilerProperty profilerProperty = this.CreateProperty();
				if (ProfilerWindow.CheckFrameData(profilerProperty))
				{
					this.m_AudioProfilerDSPView.OnGUI(clippingRect, profilerProperty, this.m_ShowInactiveDSPChains, this.m_HighlightAudibleDSPChains, ref this.m_DSPGraphZoomFactor, ref this.m_PaneScroll_AudioDSP);
				}
				profilerProperty.Cleanup();
				GUI.EndScrollView();
				base.Repaint();
			}
			else if (this.m_ShowDetailedAudioPane == ProfilerAudioView.Clips)
			{
				GUILayout.Space(5f);
				this.AudioProfilerToggle(ProfilerCaptureFlags.Clips);
				GUILayout.FlexibleSpace();
				EditorGUILayout.EndHorizontal();
				Rect rect2 = GUILayoutUtility.GetRect(20f, 20000f, 10f, 10000f);
				Rect position = new Rect(rect2.x, rect2.y, 230f, rect2.height);
				Rect rect3 = new Rect(position.xMax, rect2.y, rect2.width - position.width, rect2.height);
				string overviewText = ProfilerDriver.GetOverviewText(this.m_CurrentArea, this.GetActiveVisibleFrameIndex());
				Vector2 vector = EditorStyles.wordWrappedLabel.CalcSize(GUIContent.Temp(overviewText));
				this.m_PaneScroll_AudioClips = GUI.BeginScrollView(position, this.m_PaneScroll_AudioClips, new Rect(0f, 0f, vector.x, vector.y));
				GUI.Label(new Rect(3f, 3f, vector.x, vector.y), overviewText, EditorStyles.wordWrappedLabel);
				GUI.EndScrollView();
				EditorGUI.DrawRect(new Rect(position.xMax - 1f, position.y, 1f, position.height), Color.black);
				if (this.m_AudioProfilerClipTreeViewState == null)
				{
					this.m_AudioProfilerClipTreeViewState = new AudioProfilerClipTreeViewState();
				}
				if (this.m_AudioProfilerClipViewBackend == null)
				{
					this.m_AudioProfilerClipViewBackend = new AudioProfilerClipViewBackend(this.m_AudioProfilerClipTreeViewState);
				}
				ProfilerProperty profilerProperty2 = this.CreateProperty();
				if (ProfilerWindow.CheckFrameData(profilerProperty2))
				{
					if (this.m_CurrentFrame == -1 || this.m_LastAudioProfilerFrame != this.m_CurrentFrame)
					{
						this.m_LastAudioProfilerFrame = this.m_CurrentFrame;
						AudioProfilerClipInfo[] audioProfilerClipInfo = profilerProperty2.GetAudioProfilerClipInfo();
						if (audioProfilerClipInfo != null && audioProfilerClipInfo.Length > 0)
						{
							List<AudioProfilerClipInfoWrapper> list = new List<AudioProfilerClipInfoWrapper>();
							AudioProfilerClipInfo[] array = audioProfilerClipInfo;
							for (int i = 0; i < array.Length; i++)
							{
								AudioProfilerClipInfo info = array[i];
								list.Add(new AudioProfilerClipInfoWrapper(info, profilerProperty2.GetAudioProfilerNameByOffset(info.assetNameOffset)));
							}
							this.m_AudioProfilerClipViewBackend.SetData(list);
							if (this.m_AudioProfilerClipView == null)
							{
								this.m_AudioProfilerClipView = new AudioProfilerClipView(this, this.m_AudioProfilerClipTreeViewState);
								this.m_AudioProfilerClipView.Init(rect3, this.m_AudioProfilerClipViewBackend);
							}
						}
					}
					if (this.m_AudioProfilerClipView != null)
					{
						this.m_AudioProfilerClipView.OnGUI(rect3);
					}
				}
				profilerProperty2.Cleanup();
			}
			else
			{
				GUILayout.Space(5f);
				this.AudioProfilerToggle(ProfilerCaptureFlags.Channels);
				GUILayout.Space(5f);
				bool flag = GUILayout.Toggle(AudioUtil.resetAllAudioClipPlayCountsOnPlay, "Reset play count on play", EditorStyles.toolbarButton, new GUILayoutOption[0]);
				if (flag != AudioUtil.resetAllAudioClipPlayCountsOnPlay)
				{
					AudioUtil.resetAllAudioClipPlayCountsOnPlay = flag;
				}
				if (Unsupported.IsDeveloperMode())
				{
					GUILayout.Space(5f);
					bool @bool = EditorPrefs.GetBool("AudioProfilerShowAllGroups");
					bool flag2 = GUILayout.Toggle(@bool, "Show all groups (dev mode only)", EditorStyles.toolbarButton, new GUILayoutOption[0]);
					if (@bool != flag2)
					{
						EditorPrefs.SetBool("AudioProfilerShowAllGroups", flag2);
					}
				}
				GUILayout.FlexibleSpace();
				EditorGUILayout.EndHorizontal();
				Rect rect4 = GUILayoutUtility.GetRect(20f, 20000f, 10f, 10000f);
				Rect position2 = new Rect(rect4.x, rect4.y, 230f, rect4.height);
				Rect rect5 = new Rect(position2.xMax, rect4.y, rect4.width - position2.width, rect4.height);
				string overviewText2 = ProfilerDriver.GetOverviewText(this.m_CurrentArea, this.GetActiveVisibleFrameIndex());
				Vector2 vector2 = EditorStyles.wordWrappedLabel.CalcSize(GUIContent.Temp(overviewText2));
				this.m_PaneScroll_AudioChannels = GUI.BeginScrollView(position2, this.m_PaneScroll_AudioChannels, new Rect(0f, 0f, vector2.x, vector2.y));
				GUI.Label(new Rect(3f, 3f, vector2.x, vector2.y), overviewText2, EditorStyles.wordWrappedLabel);
				GUI.EndScrollView();
				EditorGUI.DrawRect(new Rect(position2.xMax - 1f, position2.y, 1f, position2.height), Color.black);
				if (this.m_AudioProfilerGroupTreeViewState == null)
				{
					this.m_AudioProfilerGroupTreeViewState = new AudioProfilerGroupTreeViewState();
				}
				if (this.m_AudioProfilerGroupViewBackend == null)
				{
					this.m_AudioProfilerGroupViewBackend = new AudioProfilerGroupViewBackend(this.m_AudioProfilerGroupTreeViewState);
				}
				ProfilerProperty profilerProperty3 = this.CreateProperty();
				if (ProfilerWindow.CheckFrameData(profilerProperty3))
				{
					if (this.m_CurrentFrame == -1 || this.m_LastAudioProfilerFrame != this.m_CurrentFrame)
					{
						this.m_LastAudioProfilerFrame = this.m_CurrentFrame;
						AudioProfilerGroupInfo[] audioProfilerGroupInfo = profilerProperty3.GetAudioProfilerGroupInfo();
						if (audioProfilerGroupInfo != null && audioProfilerGroupInfo.Length > 0)
						{
							List<AudioProfilerGroupInfoWrapper> list2 = new List<AudioProfilerGroupInfoWrapper>();
							AudioProfilerGroupInfo[] array2 = audioProfilerGroupInfo;
							for (int j = 0; j < array2.Length; j++)
							{
								AudioProfilerGroupInfo info2 = array2[j];
								bool flag3 = (info2.flags & 64) != 0;
								if (this.m_ShowDetailedAudioPane != ProfilerAudioView.Channels || !flag3)
								{
									if (this.m_ShowDetailedAudioPane != ProfilerAudioView.Groups || flag3)
									{
										list2.Add(new AudioProfilerGroupInfoWrapper(info2, profilerProperty3.GetAudioProfilerNameByOffset(info2.assetNameOffset), profilerProperty3.GetAudioProfilerNameByOffset(info2.objectNameOffset), this.m_ShowDetailedAudioPane == ProfilerAudioView.Channels));
									}
								}
							}
							this.m_AudioProfilerGroupViewBackend.SetData(list2);
							if (this.m_AudioProfilerGroupView == null)
							{
								this.m_AudioProfilerGroupView = new AudioProfilerGroupView(this, this.m_AudioProfilerGroupTreeViewState);
								this.m_AudioProfilerGroupView.Init(rect5, this.m_AudioProfilerGroupViewBackend);
							}
						}
					}
					if (this.m_AudioProfilerGroupView != null)
					{
						this.m_AudioProfilerGroupView.OnGUI(rect5, this.m_ShowDetailedAudioPane == ProfilerAudioView.Channels);
					}
				}
				profilerProperty3.Cleanup();
			}
		}

		private static void DrawBackground(int row, bool selected)
		{
			Rect position = new Rect(1f, 16f * (float)row, GUIClip.visibleRect.width, 16f);
			GUIStyle gUIStyle = (row % 2 != 0) ? ProfilerWindow.Styles.entryOdd : ProfilerWindow.Styles.entryEven;
			if (Event.current.type == EventType.Repaint)
			{
				gUIStyle.Draw(position, GUIContent.none, false, false, selected, false);
			}
		}

		private void DrawMemoryToolbar()
		{
			EditorGUILayout.BeginHorizontal(EditorStyles.toolbar, new GUILayoutOption[0]);
			this.m_ShowDetailedMemoryPane = (ProfilerMemoryView)EditorGUILayout.EnumPopup(this.m_ShowDetailedMemoryPane, EditorStyles.toolbarDropDown, new GUILayoutOption[]
			{
				GUILayout.Width(70f)
			});
			GUILayout.Space(5f);
			if (this.m_ShowDetailedMemoryPane == ProfilerMemoryView.Detailed)
			{
				if (GUILayout.Button("Take Sample: " + this.m_AttachProfilerUI.GetConnectedProfiler(), EditorStyles.toolbarButton, new GUILayoutOption[0]))
				{
					this.RefreshMemoryData();
				}
				this.m_GatherObjectReferences = GUILayout.Toggle(this.m_GatherObjectReferences, ProfilerWindow.Styles.gatherObjectReferences, EditorStyles.toolbarButton, new GUILayoutOption[0]);
				if (this.m_AttachProfilerUI.IsEditor())
				{
					GUILayout.Label("Memory usage in editor is not as it would be in a player", EditorStyles.toolbarButton, new GUILayoutOption[0]);
				}
			}
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
		}

		private void RefreshMemoryData()
		{
			this.m_MemoryListView.RequiresRefresh = true;
			ProfilerDriver.RequestObjectMemoryInfo(this.m_GatherObjectReferences);
		}

		private static void UpdateChartGrid(float timeMax, ChartViewData data)
		{
			if (timeMax < 1500f)
			{
				data.SetGrid(new float[]
				{
					1000f,
					250f,
					100f
				}, new string[]
				{
					"1ms (1000FPS)",
					"0.25ms (4000FPS)",
					"0.1ms (10000FPS)"
				});
			}
			else if (timeMax < 10000f)
			{
				data.SetGrid(new float[]
				{
					8333f,
					4000f,
					1000f
				}, new string[]
				{
					"8ms (120FPS)",
					"4ms (250FPS)",
					"1ms (1000FPS)"
				});
			}
			else if (timeMax < 30000f)
			{
				data.SetGrid(new float[]
				{
					16667f,
					10000f,
					5000f
				}, new string[]
				{
					"16ms (60FPS)",
					"10ms (100FPS)",
					"5ms (200FPS)"
				});
			}
			else if (timeMax < 100000f)
			{
				data.SetGrid(new float[]
				{
					66667f,
					33333f,
					16667f
				}, new string[]
				{
					"66ms (15FPS)",
					"33ms (30FPS)",
					"16ms (60FPS)"
				});
			}
			else
			{
				data.SetGrid(new float[]
				{
					500000f,
					200000f,
					66667f
				}, new string[]
				{
					"500ms (2FPS)",
					"200ms (5FPS)",
					"66ms (15FPS)"
				});
			}
		}

		private void UpdateCharts()
		{
			int num = ProfilerDriver.maxHistoryLength - 1;
			int num2 = ProfilerDriver.lastFrameIndex - num;
			int firstFrame = Mathf.Max(ProfilerDriver.firstFrameIndex, num2);
			ProfilerChart[] charts = this.m_Charts;
			for (int i = 0; i < charts.Length; i++)
			{
				ProfilerChart chart = charts[i];
				ProfilerWindow.UpdateSingleChart(chart, num2, firstFrame);
			}
			string selectedPropertyPath = ProfilerDriver.selectedPropertyPath;
			bool flag = selectedPropertyPath != string.Empty && this.m_CurrentArea == ProfilerArea.CPU;
			ProfilerChart profilerChart = this.m_Charts[0];
			if (flag)
			{
				profilerChart.m_Data.hasOverlay = true;
				int numSeries = profilerChart.m_Data.numSeries;
				for (int j = 0; j < numSeries; j++)
				{
					ChartSeriesViewData chartSeriesViewData = profilerChart.m_Data.series[j];
					profilerChart.m_Data.overlays[j] = new ChartSeriesViewData(chartSeriesViewData.name, chartSeriesViewData.yValues.Length, chartSeriesViewData.color);
					for (int k = 0; k < chartSeriesViewData.yValues.Length; k++)
					{
						profilerChart.m_Data.overlays[j].xValues[k] = (float)k;
					}
					int statisticsIdentifier = ProfilerDriver.GetStatisticsIdentifier(string.Format("Selected{0}", profilerChart.m_Data.series[j].name));
					float num3;
					ProfilerDriver.GetStatisticsValues(statisticsIdentifier, num2, profilerChart.m_DataScale, profilerChart.m_Data.overlays[j].yValues, out num3);
				}
			}
			else
			{
				profilerChart.m_Data.hasOverlay = false;
			}
			for (int l = 0; l < ProfilerWindow.ms_StackedAreas.Length; l++)
			{
				this.ComputeChartScaleValue(ProfilerWindow.ms_StackedAreas[l], num, num2, firstFrame);
			}
			string notSupportedWarning = null;
			if (!ProfilerDriver.isGPUProfilerSupported)
			{
				notSupportedWarning = "GPU profiling is not supported by the graphics card driver. Please update to a newer version if available.";
				if (Application.platform == RuntimePlatform.OSXEditor)
				{
					if (!ProfilerDriver.isGPUProfilerSupportedByOS)
					{
						notSupportedWarning = "GPU profiling requires Mac OS X 10.7 (Lion) and a capable video card. GPU profiling is currently not supported on mobile.";
					}
					else
					{
						notSupportedWarning = "GPU profiling is not supported by the graphics card driver (or it was disabled because of driver bugs).";
					}
				}
			}
			this.m_Charts[1].m_NotSupportedWarning = notSupportedWarning;
		}

		private void ComputeChartScaleValue(ProfilerArea i, int historyLength, int firstEmptyFrame, int firstFrame)
		{
			ProfilerChart profilerChart = this.m_Charts[(int)i];
			float num = 0f;
			float num2 = 0f;
			for (int j = 0; j < historyLength; j++)
			{
				float num3 = 0f;
				for (int k = 0; k < profilerChart.m_Series.Length; k++)
				{
					if (profilerChart.m_Series[k].enabled)
					{
						num3 += profilerChart.m_Series[k].yValues[j];
					}
				}
				if (num3 > num)
				{
					num = num3;
				}
				if (num3 > num2 && j + firstEmptyFrame >= firstFrame + 1)
				{
					num2 = num3;
				}
			}
			if (num2 != 0f)
			{
				num = num2;
			}
			num = Math.Min(num, this.m_ChartMaxClamp);
			if (this.m_ChartOldMax[(int)i] > 0f)
			{
				num = Mathf.Lerp(this.m_ChartOldMax[(int)i], num, 0.4f);
			}
			this.m_ChartOldMax[(int)i] = num;
			for (int l = 0; l < profilerChart.m_Data.numSeries; l++)
			{
				profilerChart.m_Data.series[l].rangeAxis = new Vector2(0f, num);
			}
			ProfilerWindow.UpdateChartGrid(num, profilerChart.m_Data);
		}

		internal static void UpdateSingleChart(ProfilerChart chart, int firstEmptyFrame, int firstFrame)
		{
			float num = 1f;
			float[] array = new float[chart.m_Series.Length];
			int i = 0;
			int num2 = chart.m_Series.Length;
			while (i < num2)
			{
				int statisticsIdentifier = ProfilerDriver.GetStatisticsIdentifier(chart.m_Series[i].name);
				float num3;
				ProfilerDriver.GetStatisticsValues(statisticsIdentifier, firstEmptyFrame, chart.m_DataScale, chart.m_Series[i].yValues, out num3);
				num3 = Mathf.Max(num3, 0.0001f);
				if (num3 > num)
				{
					num = num3;
				}
				if (chart.m_Type == Chart.ChartType.Line)
				{
					array[i] = num3 * (1.05f + (float)i * 0.05f);
					chart.m_Series[i].rangeAxis = new Vector2(0f, array[i]);
				}
				else
				{
					array[i] = num3;
				}
				i++;
			}
			if (chart.m_Area == ProfilerArea.NetworkMessages || chart.m_Area == ProfilerArea.NetworkOperations)
			{
				int j = 0;
				int num4 = chart.m_Series.Length;
				while (j < num4)
				{
					chart.m_Series[j].rangeAxis = new Vector2(0f, 0.9f * num);
					j++;
				}
				chart.m_Data.maxValue = num;
			}
			chart.m_Data.Assign(chart.m_Series, firstEmptyFrame, firstFrame);
			if (chart is UISystemProfilerChart)
			{
				((UISystemProfilerChart)chart).Update(firstFrame, ProfilerDriver.maxHistoryLength - 1);
			}
		}

		private void AddAreaClick(object userData, string[] options, int selected)
		{
			this.m_Charts[selected].active = true;
		}

		private void MemRecordModeClick(object userData, string[] options, int selected)
		{
			this.m_SelectedMemRecordMode = (ProfilerMemoryRecordMode)selected;
			ProfilerDriver.memoryRecordMode = this.m_SelectedMemRecordMode;
		}

		private void SaveProfilingData()
		{
			string text = EditorGUIUtility.TempContent("Save profile").text;
			string @string = EditorPrefs.GetString("ProfilerRecentSaveLoadProfilePath");
			string directory = (!string.IsNullOrEmpty(@string)) ? Path.GetDirectoryName(@string) : "";
			string defaultName = (!string.IsNullOrEmpty(@string)) ? Path.GetFileName(@string) : "";
			string text2 = EditorUtility.SaveFilePanel(text, directory, defaultName, "data");
			if (text2.Length != 0)
			{
				EditorPrefs.SetString("ProfilerRecentSaveLoadProfilePath", text2);
				ProfilerDriver.SaveProfile(text2);
			}
			GUIUtility.ExitGUI();
		}

		private void LoadProfilingData(bool keepExistingData)
		{
			string text = EditorGUIUtility.TempContent("Load profile").text;
			string @string = EditorPrefs.GetString("ProfilerRecentSaveLoadProfilePath");
			string text2 = EditorUtility.OpenFilePanel(text, @string, "data");
			if (text2.Length != 0)
			{
				EditorPrefs.SetString("ProfilerRecentSaveLoadProfilePath", text2);
				if (ProfilerDriver.LoadProfile(text2, keepExistingData))
				{
					ProfilerDriver.enabled = (this.m_Recording = false);
					SessionState.SetBool("ProfilerEnabled", this.m_Recording);
					NetworkDetailStats.m_NetworkOperations.Clear();
				}
			}
			GUIUtility.ExitGUI();
		}

		private void DrawMainToolbar()
		{
			GUILayout.BeginHorizontal(EditorStyles.toolbar, new GUILayoutOption[0]);
			Rect rect = GUILayoutUtility.GetRect(ProfilerWindow.Styles.addArea, EditorStyles.toolbarDropDown, new GUILayoutOption[]
			{
				GUILayout.Width(120f)
			});
			if (EditorGUI.DropdownButton(rect, ProfilerWindow.Styles.addArea, FocusType.Passive, EditorStyles.toolbarDropDown))
			{
				int num = this.m_Charts.Length;
				string[] array = new string[num];
				bool[] array2 = new bool[num];
				for (int i = 0; i < num; i++)
				{
					string[] arg_7F_0 = array;
					int arg_7F_1 = i;
					ProfilerArea profilerArea = (ProfilerArea)i;
					arg_7F_0[arg_7F_1] = profilerArea.ToString();
					array2[i] = !this.m_Charts[i].active;
				}
				EditorUtility.DisplayCustomMenu(rect, array, array2, null, new EditorUtility.SelectMenuItemFunction(this.AddAreaClick), null);
			}
			GUILayout.FlexibleSpace();
			bool flag = GUILayout.Toggle(this.m_Recording, ProfilerWindow.Styles.profilerRecord, EditorStyles.toolbarButton, new GUILayoutOption[0]);
			if (flag != this.m_Recording)
			{
				ProfilerDriver.enabled = flag;
				this.m_Recording = flag;
				SessionState.SetBool("ProfilerEnabled", flag);
			}
			EditorGUI.BeginDisabledGroup(!this.m_AttachProfilerUI.IsEditor());
			ProfilerWindow.SetProfileDeepScripts(GUILayout.Toggle(ProfilerDriver.deepProfiling, ProfilerWindow.Styles.deepProfile, EditorStyles.toolbarButton, new GUILayoutOption[0]));
			ProfilerDriver.profileEditor = GUILayout.Toggle(ProfilerDriver.profileEditor, ProfilerWindow.Styles.profileEditor, EditorStyles.toolbarButton, new GUILayoutOption[0]);
			EditorGUI.EndDisabledGroup();
			this.m_AttachProfilerUI.OnGUILayout(this);
			this.AllocationCallstacksToolbarItem();
			GUILayout.FlexibleSpace();
			this.SetClearOnPlay(GUILayout.Toggle(this.GetClearOnPlay(), ProfilerWindow.Styles.clearOnPlay, EditorStyles.toolbarButton, new GUILayoutOption[0]));
			if (GUILayout.Button(ProfilerWindow.Styles.clearData, EditorStyles.toolbarButton, new GUILayoutOption[0]))
			{
				this.Clear();
			}
			if (GUILayout.Button(ProfilerWindow.Styles.loadProfilingData, EditorStyles.toolbarButton, new GUILayoutOption[0]))
			{
				this.LoadProfilingData(Event.current.shift);
			}
			using (new EditorGUI.DisabledScope(ProfilerDriver.lastFrameIndex == -1))
			{
				if (GUILayout.Button(ProfilerWindow.Styles.saveProfilingData, EditorStyles.toolbarButton, new GUILayoutOption[0]))
				{
					this.SaveProfilingData();
				}
			}
			GUILayout.Space(5f);
			this.FrameNavigationControls();
			GUILayout.EndHorizontal();
		}

		private void AllocationCallstacksToolbarItem()
		{
			ProfilerWindow.Styles.memRecord.text = "Allocation Callstacks";
			if (this.m_SelectedMemRecordMode != ProfilerMemoryRecordMode.None)
			{
				GUIContent expr_20 = ProfilerWindow.Styles.memRecord;
				string text = expr_20.text;
				expr_20.text = string.Concat(new object[]
				{
					text,
					" [",
					this.s_CheckMark,
					"]"
				});
			}
			Rect rect = GUILayoutUtility.GetRect(ProfilerWindow.Styles.memRecord, EditorStyles.toolbarDropDown, new GUILayoutOption[]
			{
				GUILayout.Width(130f)
			});
			if (EditorGUI.DropdownButton(rect, ProfilerWindow.Styles.memRecord, FocusType.Passive, EditorStyles.toolbarDropDown))
			{
				string[] array = new string[]
				{
					"None",
					"Managed Allocations"
				};
				if (Unsupported.IsDeveloperMode())
				{
					array = new string[]
					{
						"None",
						"Managed Allocations",
						"All Allocations (fast)",
						"All Allocations (full)"
					};
				}
				bool[] array2 = new bool[array.Length];
				for (int i = 0; i < array.Length; i++)
				{
					array2[i] = true;
				}
				int[] selected = new int[]
				{
					(int)this.m_SelectedMemRecordMode
				};
				EditorUtility.DisplayCustomMenu(rect, array, array2, selected, new EditorUtility.SelectMenuItemFunction(this.MemRecordModeClick), null);
			}
		}

		private void Clear()
		{
			this.m_CPUFrameDataHierarchyView.Clear();
			this.m_GPUFrameDataHierarchyView.Clear();
			ProfilerDriver.ClearAllFrames();
			NetworkDetailStats.m_NetworkOperations.Clear();
		}

		private void FrameNavigationControls()
		{
			if (this.m_CurrentFrame > ProfilerDriver.lastFrameIndex)
			{
				this.SetCurrentFrameDontPause(ProfilerDriver.lastFrameIndex);
			}
			GUILayout.Label(ProfilerWindow.Styles.frame, EditorStyles.miniLabel, new GUILayoutOption[0]);
			GUILayout.Label("   " + this.PickFrameLabel(), EditorStyles.miniLabel, new GUILayoutOption[]
			{
				GUILayout.Width(100f)
			});
			GUI.enabled = (ProfilerDriver.GetPreviousFrameIndex(this.m_CurrentFrame) != -1);
			if (GUILayout.Button(ProfilerWindow.Styles.prevFrame, EditorStyles.toolbarButton, new GUILayoutOption[0]))
			{
				this.PrevFrame();
			}
			GUI.enabled = (ProfilerDriver.GetNextFrameIndex(this.m_CurrentFrame) != -1);
			if (GUILayout.Button(ProfilerWindow.Styles.nextFrame, EditorStyles.toolbarButton, new GUILayoutOption[0]))
			{
				this.NextFrame();
			}
			GUI.enabled = true;
			GUILayout.Space(10f);
			if (GUILayout.Button(ProfilerWindow.Styles.currentFrame, EditorStyles.toolbarButton, new GUILayoutOption[0]))
			{
				this.SetCurrentFrame(-1);
				this.m_LastFrameFromTick = ProfilerDriver.lastFrameIndex;
			}
		}

		private static void DrawOtherToolbar(ProfilerArea area)
		{
			EditorGUILayout.BeginHorizontal(EditorStyles.toolbar, new GUILayoutOption[0]);
			if (area == ProfilerArea.Rendering)
			{
				if (GUILayout.Button((!GUI.enabled) ? ProfilerWindow.Styles.noFrameDebugger : ProfilerWindow.Styles.frameDebugger, EditorStyles.toolbarButton, new GUILayoutOption[0]))
				{
					FrameDebuggerWindow frameDebuggerWindow = FrameDebuggerWindow.ShowFrameDebuggerWindow();
					frameDebuggerWindow.EnableIfNeeded();
				}
			}
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
		}

		private void DrawOverviewText(ProfilerArea area)
		{
			if (area != ProfilerArea.AreaCount)
			{
				string overviewText = ProfilerDriver.GetOverviewText(area, this.GetActiveVisibleFrameIndex());
				float minHeight = EditorStyles.wordWrappedLabel.CalcHeight(GUIContent.Temp(overviewText), base.position.width);
				this.m_PaneScroll[(int)area] = GUILayout.BeginScrollView(this.m_PaneScroll[(int)area], ProfilerWindow.Styles.background);
				EditorGUILayout.SelectableLabel(overviewText, EditorStyles.wordWrappedLabel, new GUILayoutOption[]
				{
					GUILayout.MinHeight(minHeight)
				});
				GUILayout.EndScrollView();
			}
		}

		private void DrawPane(ProfilerArea area)
		{
			ProfilerWindow.DrawOtherToolbar(area);
			this.DrawOverviewText(area);
		}

		private void SetCurrentFrameDontPause(int frame)
		{
			this.m_CurrentFrame = frame;
		}

		private void SetCurrentFrame(int frame)
		{
			if (frame != -1 && ProfilerDriver.enabled && !ProfilerDriver.profileEditor && this.m_CurrentFrame != frame && EditorApplication.isPlayingOrWillChangePlaymode)
			{
				EditorApplication.isPaused = true;
			}
			if (ProfilerInstrumentationPopup.InstrumentationEnabled)
			{
				ProfilerInstrumentationPopup.UpdateInstrumentableFunctions();
			}
			this.SetCurrentFrameDontPause(frame);
		}

		private void OnGUI()
		{
			this.CheckForPlatformModuleChange();
			this.InitializeIfNeeded();
			this.DrawMainToolbar();
			SplitterGUILayout.BeginVerticalSplit(this.m_VertSplit, new GUILayoutOption[0]);
			this.m_GraphPos = EditorGUILayout.BeginScrollView(this.m_GraphPos, ProfilerWindow.Styles.profilerGraphBackground, new GUILayoutOption[0]);
			if (this.m_PrevLastFrame != ProfilerDriver.lastFrameIndex)
			{
				this.UpdateCharts();
				this.m_PrevLastFrame = ProfilerDriver.lastFrameIndex;
			}
			int num = this.m_CurrentFrame;
			for (int i = 0; i < this.m_Charts.Length; i++)
			{
				ProfilerChart profilerChart = this.m_Charts[i];
				if (profilerChart.active)
				{
					num = profilerChart.DoChartGUI(num, this.m_CurrentArea);
				}
			}
			if (num != this.m_CurrentFrame)
			{
				this.SetCurrentFrame(num);
				base.Repaint();
				GUIUtility.ExitGUI();
			}
			EditorGUILayout.EndScrollView();
			GUILayout.BeginVertical(new GUILayoutOption[0]);
			switch (this.m_CurrentArea)
			{
			case ProfilerArea.CPU:
				this.DrawCPUOrGPUPane(this.m_CPUFrameDataHierarchyView, this.m_CPUTimelineGUI);
				goto IL_1B5;
			case ProfilerArea.GPU:
				this.DrawCPUOrGPUPane(this.m_GPUFrameDataHierarchyView, null);
				goto IL_1B5;
			case ProfilerArea.Memory:
				this.DrawMemoryPane(this.m_ViewSplit);
				goto IL_1B5;
			case ProfilerArea.Audio:
				this.DrawAudioPane();
				goto IL_1B5;
			case ProfilerArea.NetworkMessages:
				this.DrawPane(this.m_CurrentArea);
				goto IL_1B5;
			case ProfilerArea.NetworkOperations:
				this.DrawNetworkOperationsPane();
				goto IL_1B5;
			case ProfilerArea.UI:
			case ProfilerArea.UIDetails:
				this.m_UISystemProfiler.DrawUIPane(this, this.m_CurrentArea, (UISystemProfilerChart)this.m_Charts[11]);
				goto IL_1B5;
			}
			this.DrawPane(this.m_CurrentArea);
			IL_1B5:
			GUILayout.EndVertical();
			SplitterGUILayout.EndVerticalSplit();
		}

		public void SetClearOnPlay(bool enabled)
		{
			this.m_ClearOnPlay = enabled;
		}

		public bool GetClearOnPlay()
		{
			return this.m_ClearOnPlay;
		}
	}
}
