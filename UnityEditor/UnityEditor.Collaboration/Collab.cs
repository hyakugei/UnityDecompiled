using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEditor.Connect;
using UnityEditor.SceneManagement;
using UnityEditor.Web;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Scripting;

namespace UnityEditor.Collaboration
{
	[InitializeOnLoad]
	internal sealed class Collab : AssetPostprocessor
	{
		[Flags]
		public enum CollabStates : ulong
		{
			kCollabNone = 0uL,
			kCollabLocal = 1uL,
			kCollabSynced = 2uL,
			kCollabOutOfSync = 4uL,
			kCollabIgnored = 8uL,
			kCollabCheckedOutLocal = 16uL,
			kCollabCheckedOutRemote = 32uL,
			kCollabDeletedLocal = 64uL,
			kCollabDeletedRemote = 128uL,
			kCollabAddedLocal = 256uL,
			kCollabAddedRemote = 512uL,
			kCollabConflicted = 1024uL,
			kCollabMovedLocal = 2048uL,
			kCollabMovedRemote = 4096uL,
			kCollabUpdating = 8192uL,
			kCollabReadOnly = 16384uL,
			kCollabMetaFile = 32768uL,
			kCollabUseMine = 65536uL,
			kCollabUseTheir = 131072uL,
			kCollabMerged = 262144uL,
			kCollabPendingMerge = 524288uL,
			kCollabFolderMetaFile = 1048576uL,
			KCollabContentChanged = 2097152uL,
			KCollabContentConflicted = 4194304uL,
			KCollabContentDeleted = 8388608uL,
			kCollabInvalidState = 1073741824uL,
			kAnyLocalChanged = 2384uL,
			kAnyLocalEdited = 2320uL
		}

		internal enum CollabStateID
		{
			None,
			Uninitialized,
			Initialized
		}

		private static Collab s_Instance;

		private static bool s_IsFirstStateChange;

		[SerializeField]
		public CollabFilters collabFilters = new CollabFilters();

		public string[] currentProjectBrowserSelection;

		public static string[] clientType;

		internal static string editorPrefCollabClientType;

		[CompilerGenerated]
		private static ObjectListArea.OnAssetIconDrawDelegate <>f__mg$cache0;

		[CompilerGenerated]
		private static AssetsTreeViewGUI.OnAssetIconDrawDelegate <>f__mg$cache1;

		[CompilerGenerated]
		private static CollabSettingsManager.SettingStatusChanged <>f__mg$cache2;

		[CompilerGenerated]
		private static UnityEditor.Connect.StateChangedDelegate <>f__mg$cache3;

		[CompilerGenerated]
		private static CollabSettingsManager.SettingStatusChanged <>f__mg$cache4;

		[CompilerGenerated]
		private static CollabSettingsManager.SettingStatusChanged <>f__mg$cache5;

		[CompilerGenerated]
		private static CollabSettingsManager.SettingStatusChanged <>f__mg$cache6;

		[CompilerGenerated]
		private static CollabSettingsManager.SettingStatusChanged <>f__mg$cache7;

		public event StateChangedDelegate StateChanged
		{
			add
			{
				StateChangedDelegate stateChangedDelegate = this.StateChanged;
				StateChangedDelegate stateChangedDelegate2;
				do
				{
					stateChangedDelegate2 = stateChangedDelegate;
					stateChangedDelegate = Interlocked.CompareExchange<StateChangedDelegate>(ref this.StateChanged, (StateChangedDelegate)Delegate.Combine(stateChangedDelegate2, value), stateChangedDelegate);
				}
				while (stateChangedDelegate != stateChangedDelegate2);
			}
			remove
			{
				StateChangedDelegate stateChangedDelegate = this.StateChanged;
				StateChangedDelegate stateChangedDelegate2;
				do
				{
					stateChangedDelegate2 = stateChangedDelegate;
					stateChangedDelegate = Interlocked.CompareExchange<StateChangedDelegate>(ref this.StateChanged, (StateChangedDelegate)Delegate.Remove(stateChangedDelegate2, value), stateChangedDelegate);
				}
				while (stateChangedDelegate != stateChangedDelegate2);
			}
		}

		public event StateChangedDelegate RevisionUpdated
		{
			add
			{
				StateChangedDelegate stateChangedDelegate = this.RevisionUpdated;
				StateChangedDelegate stateChangedDelegate2;
				do
				{
					stateChangedDelegate2 = stateChangedDelegate;
					stateChangedDelegate = Interlocked.CompareExchange<StateChangedDelegate>(ref this.RevisionUpdated, (StateChangedDelegate)Delegate.Combine(stateChangedDelegate2, value), stateChangedDelegate);
				}
				while (stateChangedDelegate != stateChangedDelegate2);
			}
			remove
			{
				StateChangedDelegate stateChangedDelegate = this.RevisionUpdated;
				StateChangedDelegate stateChangedDelegate2;
				do
				{
					stateChangedDelegate2 = stateChangedDelegate;
					stateChangedDelegate = Interlocked.CompareExchange<StateChangedDelegate>(ref this.RevisionUpdated, (StateChangedDelegate)Delegate.Remove(stateChangedDelegate2, value), stateChangedDelegate);
				}
				while (stateChangedDelegate != stateChangedDelegate2);
			}
		}

		public event StateChangedDelegate JobsCompleted
		{
			add
			{
				StateChangedDelegate stateChangedDelegate = this.JobsCompleted;
				StateChangedDelegate stateChangedDelegate2;
				do
				{
					stateChangedDelegate2 = stateChangedDelegate;
					stateChangedDelegate = Interlocked.CompareExchange<StateChangedDelegate>(ref this.JobsCompleted, (StateChangedDelegate)Delegate.Combine(stateChangedDelegate2, value), stateChangedDelegate);
				}
				while (stateChangedDelegate != stateChangedDelegate2);
			}
			remove
			{
				StateChangedDelegate stateChangedDelegate = this.JobsCompleted;
				StateChangedDelegate stateChangedDelegate2;
				do
				{
					stateChangedDelegate2 = stateChangedDelegate;
					stateChangedDelegate = Interlocked.CompareExchange<StateChangedDelegate>(ref this.JobsCompleted, (StateChangedDelegate)Delegate.Remove(stateChangedDelegate2, value), stateChangedDelegate);
				}
				while (stateChangedDelegate != stateChangedDelegate2);
			}
		}

		public extern CollabInfo collabInfo
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public string projectBrowserSingleSelectionPath
		{
			get;
			set;
		}

		public string projectBrowserSingleMetaSelectionPath
		{
			get;
			set;
		}

		public static Collab instance
		{
			get
			{
				return Collab.s_Instance;
			}
		}

		static Collab()
		{
			Collab.s_IsFirstStateChange = true;
			Collab.clientType = new string[]
			{
				"Cloud Server",
				"Mock Server"
			};
			Collab.editorPrefCollabClientType = "CollabConfig_Client";
			Collab.s_Instance = new Collab();
			Collab.s_Instance.projectBrowserSingleSelectionPath = string.Empty;
			Collab.s_Instance.projectBrowserSingleMetaSelectionPath = string.Empty;
			JSProxyMgr.GetInstance().AddGlobalObject("unity/collab", Collab.s_Instance);
			if (Collab.<>f__mg$cache0 == null)
			{
				Collab.<>f__mg$cache0 = new ObjectListArea.OnAssetIconDrawDelegate(CollabProjectHook.OnProjectWindowIconOverlay);
			}
			ObjectListArea.postAssetIconDrawCallback += Collab.<>f__mg$cache0;
			if (Collab.<>f__mg$cache1 == null)
			{
				Collab.<>f__mg$cache1 = new AssetsTreeViewGUI.OnAssetIconDrawDelegate(CollabProjectHook.OnProjectBrowserNavPanelIconOverlay);
			}
			AssetsTreeViewGUI.postAssetIconDrawCallback += Collab.<>f__mg$cache1;
			Collab.InitializeSoftlocksViewController();
			Dictionary<CollabSettingType, CollabSettingsManager.SettingStatusChanged> statusNotifier;
			Dictionary<CollabSettingType, CollabSettingsManager.SettingStatusChanged> expr_B7 = statusNotifier = CollabSettingsManager.statusNotifier;
			CollabSettingType arg_E8_1 = CollabSettingType.InProgressEnabled;
			Delegate arg_DE_0 = statusNotifier[CollabSettingType.InProgressEnabled];
			if (Collab.<>f__mg$cache2 == null)
			{
				Collab.<>f__mg$cache2 = new CollabSettingsManager.SettingStatusChanged(Collab.OnSettingStatusChanged);
			}
			expr_B7[arg_E8_1] = (CollabSettingsManager.SettingStatusChanged)Delegate.Combine(arg_DE_0, Collab.<>f__mg$cache2);
			(statusNotifier = CollabSettingsManager.statusNotifier)[CollabSettingType.InProgressEnabled] = (CollabSettingsManager.SettingStatusChanged)Delegate.Combine(statusNotifier[CollabSettingType.InProgressEnabled], new CollabSettingsManager.SettingStatusChanged(SoftlockViewController.Instance.softLockFilters.OnSettingStatusChanged));
		}

		[GeneratedByOldBindingsGenerator, ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern string GetProjectPath();

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern string GetProjectGUID();

		[GeneratedByOldBindingsGenerator, ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool IsConnected();

		[GeneratedByOldBindingsGenerator, ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool AnyJobRunning();

		[GeneratedByOldBindingsGenerator, ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool JobRunning(int a_jobID);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void Disconnect();

		public ProgressInfo GetJobProgress(int jobId)
		{
			return this.GetJobProgressByType(jobId);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern ProgressInfo GetJobProgressByType(int jobType);

		[ExcludeFromDocs]
		public void CancelJob(int jobId)
		{
			bool forceCancel = false;
			this.CancelJob(jobId, forceCancel);
		}

		public void CancelJob(int jobId, [DefaultValue("false")] bool forceCancel)
		{
			this.CancelJobByType(jobId, forceCancel);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void CancelJobByType(int jobType, [DefaultValue("false")] bool forceCancel);

		[ExcludeFromDocs]
		public void CancelJobByType(int jobType)
		{
			bool forceCancel = false;
			this.CancelJobByType(jobType, forceCancel);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern long GetAssetStateInternal(string guid);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern long GetSelectedAssetStateInternal();

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void DoInitialCommit();

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetSeat(bool value);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool ShouldDoInitialCommit();

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void Publish(string comment, [DefaultValue("false")] bool useSelectedAssets, [DefaultValue("false")] bool confirmMatchesPrevious);

		[ExcludeFromDocs]
		public void Publish(string comment, bool useSelectedAssets)
		{
			bool confirmMatchesPrevious = false;
			this.Publish(comment, useSelectedAssets, confirmMatchesPrevious);
		}

		[ExcludeFromDocs]
		public void Publish(string comment)
		{
			bool confirmMatchesPrevious = false;
			bool useSelectedAssets = false;
			this.Publish(comment, useSelectedAssets, confirmMatchesPrevious);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool ValidateSelectiveCommit();

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void Update(string revisionID, bool updateToRevision);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void RevertFile(string path, bool forceOverwrite);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern Change[] GetCollabConflicts();

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool SetConflictResolvedMine(string path);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool SetConflictsResolvedMine(string[] paths);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool SetConflictResolvedTheirs(string path);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool SetConflictsResolvedTheirs(string[] paths);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool ClearConflictResolved(string path);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool ClearConflictsResolved(string[] paths);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void LaunchConflictExternalMerge(string path);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void CheckConflictsResolvedExternal();

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void ShowConflictDifferences(string path);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void ShowDifferences(string path);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern Collab.CollabStateID GetCollabState();

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern Change[] GetChangesToPublishInternal();

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void ResyncSnapshot();

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void GoBackToRevision(string revisionID, bool updateToRevision);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SendNotification();

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void ResyncToRevision(string revisionID);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool GetError(int filter, out int code, out int priority, out int behaviour, out string errorMsg, out string errorShortMsg);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetError(int errorCode);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void ClearError(int errorCode);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void ClearErrors();

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetCollabEnabledForCurrentProject(bool enabled);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool IsCollabEnabledForCurrentProject();

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void OnPostprocessAssetbundleNameChanged(string assetPath, string previousAssetBundleName, string newAssetBundleName);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern SoftLock[] GetSoftLocks(string assetGuid);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool AreTestsRunning();

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetTestsRunning(bool running);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void ClearAllFailures();

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void DeleteTempFile(string path, CollabTempFolder folderMask);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void FailNextOperation(int operation, int code);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void TimeOutNextOperation(int operation, int timeOutSec);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void ClearNextOperationFailure();

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void FailNextOperationForFile(string path, int operation, int code);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void TimeOutNextOperationForFile(string path, int operation, int timeOutSec);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void ClearNextOperationFailureForFile(string path);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern string GetGUIDForTests();

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void NewGUIDForTests();

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void TestPostSoftLockAsCollaborator(string projectGuid, string projectPath, string machineGuid, string assetGuid);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void TestClearSoftLockAsCollaborator(string projectGuid, string projectPath, string machineGuid, string softLockHash);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Revision[] InternalGetRevisions(bool withChanges = false, int startIndex = 0, int numRevisions = -1);

		public Revision[] GetRevisions(bool withChanges = false, int startIndex = 0, int numRevisions = -1)
		{
			return Collab.InternalGetRevisions(withChanges, startIndex, numRevisions);
		}

		private static RevisionsData InternalGetRevisionsData(bool withChanges, int startIndex, int numRevisions)
		{
			RevisionsData result;
			Collab.InternalGetRevisionsData_Injected(withChanges, startIndex, numRevisions, out result);
			return result;
		}

		public RevisionsData GetRevisionsData(bool withChanges, int startIndex, int numRevisions)
		{
			return Collab.InternalGetRevisionsData(withChanges, startIndex, numRevisions);
		}

		public static string GetProjectClientType()
		{
			string configValue = EditorUserSettings.GetConfigValue(Collab.editorPrefCollabClientType);
			return (!string.IsNullOrEmpty(configValue)) ? configValue : Collab.clientType[0];
		}

		[MenuItem("Window/Collab/Get Revisions", false, 1000, true)]
		public static void TestGetRevisions()
		{
			Revision[] revisions = Collab.instance.GetRevisions(false, 0, -1);
			if (revisions.Length == 0)
			{
				Debug.Log("No revisions");
			}
			else
			{
				int num = revisions.Length;
				Revision[] array = revisions;
				for (int i = 0; i < array.Length; i++)
				{
					Revision revision = array[i];
					Debug.Log(string.Concat(new object[]
					{
						"Revision #",
						num,
						": ",
						revision.revisionID
					}));
					num--;
				}
			}
		}

		public static void OnSettingStatusChanged(CollabSettingType type, CollabSettingStatus status)
		{
			Collab.InitializeSoftlocksViewController();
		}

		public static bool InitializeSoftlocksViewController()
		{
			bool result;
			if (!CollabSettingsManager.IsAvailable(CollabSettingType.InProgressEnabled))
			{
				result = false;
			}
			else
			{
				if (CollabSettingsManager.inProgressEnabled)
				{
					SoftlockViewController.Instance.TurnOn();
				}
				else
				{
					SoftlockViewController.Instance.TurnOff();
				}
				result = true;
			}
			return result;
		}

		public void CancelJobWithoutException(int jobType)
		{
			try
			{
				this.CancelJobByType(jobType);
			}
			catch (Exception ex)
			{
				Debug.Log("Cannot cancel job, reason:" + ex.Message);
			}
		}

		public Collab.CollabStates GetAssetState(string guid)
		{
			return (Collab.CollabStates)this.GetAssetStateInternal(guid);
		}

		public Collab.CollabStates GetSelectedAssetState()
		{
			return (Collab.CollabStates)this.GetSelectedAssetStateInternal();
		}

		public void UpdateEditorSelectionCache()
		{
			List<string> list = new List<string>();
			string[] assetGUIDsDeepSelection = Selection.assetGUIDsDeepSelection;
			for (int i = 0; i < assetGUIDsDeepSelection.Length; i++)
			{
				string guid = assetGUIDsDeepSelection[i];
				string text = AssetDatabase.GUIDToAssetPath(guid);
				list.Add(text);
				string text2 = text + ".meta";
				if (File.Exists(text2))
				{
					list.Add(text2);
				}
			}
			this.currentProjectBrowserSelection = list.ToArray();
		}

		public CollabInfo GetCollabInfo()
		{
			return this.collabInfo;
		}

		public static bool IsDiffToolsAvailable()
		{
			return InternalEditorUtility.GetAvailableDiffTools().Length > 0;
		}

		public void SaveAssets()
		{
			AssetDatabase.SaveAssets();
		}

		public static void SwitchToDefaultMode()
		{
			bool flag = EditorSettings.defaultBehaviorMode == EditorBehaviorMode.Mode2D;
			SceneView lastActiveSceneView = SceneView.lastActiveSceneView;
			if (lastActiveSceneView != null && lastActiveSceneView.in2DMode != flag)
			{
				lastActiveSceneView.in2DMode = flag;
			}
		}

		public void ShowInProjectBrowser(string filterString)
		{
			this.collabFilters.ShowInProjectBrowser(filterString);
		}

		public PublishInfo GetChangesToPublish()
		{
			Change[] changesToPublishInternal = this.GetChangesToPublishInternal();
			return new PublishInfo
			{
				changes = changesToPublishInternal,
				filter = false
			};
		}

		private static void OnStateChanged()
		{
			if (Collab.s_IsFirstStateChange)
			{
				Collab.s_IsFirstStateChange = false;
				UnityConnect arg_34_0 = UnityConnect.instance;
				if (Collab.<>f__mg$cache3 == null)
				{
					Collab.<>f__mg$cache3 = new UnityEditor.Connect.StateChangedDelegate(Collab.OnUnityConnectStateChanged);
				}
				arg_34_0.StateChanged += Collab.<>f__mg$cache3;
			}
			StateChangedDelegate stateChanged = Collab.instance.StateChanged;
			if (stateChanged != null)
			{
				stateChanged(Collab.instance.collabInfo);
			}
		}

		private static void OnRevisionUpdated()
		{
			StateChangedDelegate revisionUpdated = Collab.instance.RevisionUpdated;
			if (revisionUpdated != null)
			{
				revisionUpdated(Collab.instance.collabInfo);
			}
		}

		private static void OnJobsCompleted()
		{
			StateChangedDelegate jobsCompleted = Collab.instance.JobsCompleted;
			if (jobsCompleted != null)
			{
				jobsCompleted(Collab.instance.collabInfo);
			}
			CollabTesting.OnJobsCompleted();
		}

		private static void PublishDialog(string changelist)
		{
			if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
			{
				CollabPublishDialog collabPublishDialog = CollabPublishDialog.ShowCollabWindow(changelist);
				if (collabPublishDialog.Options.DoPublish)
				{
					Collab.instance.Publish(collabPublishDialog.Options.Comments, true);
				}
			}
		}

		private static void CannotPublishDialog(string infoMessage)
		{
			CollabCannotPublishDialog.ShowCollabWindow(infoMessage);
		}

		private static void OnUnityConnectStateChanged(ConnectInfo state)
		{
			Collab.instance.SendNotification();
		}

		public static void OnProgressEnabledSettingStatusChanged(CollabSettingType type, CollabSettingStatus status)
		{
			if (type == CollabSettingType.InProgressEnabled && status == CollabSettingStatus.Available)
			{
				if (CollabSettingsManager.inProgressEnabled)
				{
					SoftlockViewController.Instance.softLockFilters.ShowInFavoriteSearchFilters();
				}
				Dictionary<CollabSettingType, CollabSettingsManager.SettingStatusChanged> statusNotifier;
				Dictionary<CollabSettingType, CollabSettingsManager.SettingStatusChanged> expr_2F = statusNotifier = CollabSettingsManager.statusNotifier;
				CollabSettingType arg_60_1 = CollabSettingType.InProgressEnabled;
				Delegate arg_56_0 = statusNotifier[CollabSettingType.InProgressEnabled];
				if (Collab.<>f__mg$cache4 == null)
				{
					Collab.<>f__mg$cache4 = new CollabSettingsManager.SettingStatusChanged(Collab.OnProgressEnabledSettingStatusChanged);
				}
				expr_2F[arg_60_1] = (CollabSettingsManager.SettingStatusChanged)Delegate.Remove(arg_56_0, Collab.<>f__mg$cache4);
			}
		}

		[RequiredByNativeCode]
		private static void OnCollabEnabledForCurrentProject(bool enabled)
		{
			if (enabled)
			{
				Collab.instance.StateChanged += new StateChangedDelegate(Collab.instance.collabFilters.OnCollabStateChanged);
				Collab.instance.collabFilters.ShowInFavoriteSearchFilters();
				if (CollabSettingsManager.IsAvailable(CollabSettingType.InProgressEnabled))
				{
					if (CollabSettingsManager.inProgressEnabled)
					{
						SoftlockViewController.Instance.softLockFilters.ShowInFavoriteSearchFilters();
					}
				}
				else
				{
					Dictionary<CollabSettingType, CollabSettingsManager.SettingStatusChanged> statusNotifier;
					Dictionary<CollabSettingType, CollabSettingsManager.SettingStatusChanged> expr_69 = statusNotifier = CollabSettingsManager.statusNotifier;
					CollabSettingType arg_9A_1 = CollabSettingType.InProgressEnabled;
					Delegate arg_90_0 = statusNotifier[CollabSettingType.InProgressEnabled];
					if (Collab.<>f__mg$cache5 == null)
					{
						Collab.<>f__mg$cache5 = new CollabSettingsManager.SettingStatusChanged(Collab.OnProgressEnabledSettingStatusChanged);
					}
					expr_69[arg_9A_1] = (CollabSettingsManager.SettingStatusChanged)Delegate.Remove(arg_90_0, Collab.<>f__mg$cache5);
					Dictionary<CollabSettingType, CollabSettingsManager.SettingStatusChanged> expr_A4 = statusNotifier = CollabSettingsManager.statusNotifier;
					CollabSettingType arg_D5_1 = CollabSettingType.InProgressEnabled;
					Delegate arg_CB_0 = statusNotifier[CollabSettingType.InProgressEnabled];
					if (Collab.<>f__mg$cache6 == null)
					{
						Collab.<>f__mg$cache6 = new CollabSettingsManager.SettingStatusChanged(Collab.OnProgressEnabledSettingStatusChanged);
					}
					expr_A4[arg_D5_1] = (CollabSettingsManager.SettingStatusChanged)Delegate.Combine(arg_CB_0, Collab.<>f__mg$cache6);
				}
			}
			else
			{
				Collab.instance.StateChanged -= new StateChangedDelegate(Collab.instance.collabFilters.OnCollabStateChanged);
				Collab.instance.collabFilters.HideFromFavoriteSearchFilters();
				SoftlockViewController.Instance.softLockFilters.HideFromFavoriteSearchFilters();
				Dictionary<CollabSettingType, CollabSettingsManager.SettingStatusChanged> statusNotifier;
				Dictionary<CollabSettingType, CollabSettingsManager.SettingStatusChanged> expr_124 = statusNotifier = CollabSettingsManager.statusNotifier;
				CollabSettingType arg_155_1 = CollabSettingType.InProgressEnabled;
				Delegate arg_14B_0 = statusNotifier[CollabSettingType.InProgressEnabled];
				if (Collab.<>f__mg$cache7 == null)
				{
					Collab.<>f__mg$cache7 = new CollabSettingsManager.SettingStatusChanged(Collab.OnProgressEnabledSettingStatusChanged);
				}
				expr_124[arg_155_1] = (CollabSettingsManager.SettingStatusChanged)Delegate.Remove(arg_14B_0, Collab.<>f__mg$cache7);
				if (ProjectBrowser.s_LastInteractedProjectBrowser != null)
				{
					if (ProjectBrowser.s_LastInteractedProjectBrowser.Initialized() && ProjectBrowser.s_LastInteractedProjectBrowser.IsTwoColumns())
					{
						int mainAssetInstanceID = AssetDatabase.GetMainAssetInstanceID("assets");
						ProjectBrowser.s_LastInteractedProjectBrowser.SetFolderSelection(new int[]
						{
							mainAssetInstanceID
						}, true);
					}
					ProjectBrowser.s_LastInteractedProjectBrowser.SetSearch("");
					ProjectBrowser.s_LastInteractedProjectBrowser.Repaint();
				}
			}
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void InternalGetRevisionsData_Injected(bool withChanges, int startIndex, int numRevisions, out RevisionsData ret);
	}
}
