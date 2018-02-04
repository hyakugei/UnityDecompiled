using System;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using UnityEditor.SceneManagement;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Scripting;

namespace UnityEditor
{
	public sealed class EditorApplication
	{
		public delegate void ProjectWindowItemCallback(string guid, Rect selectionRect);

		public delegate void HierarchyWindowItemCallback(int instanceID, Rect selectionRect);

		public delegate void CallbackFunction();

		public delegate void SerializedPropertyCallbackFunction(GenericMenu menu, SerializedProperty property);

		internal static UnityAction projectWasLoaded;

		internal static UnityAction editorApplicationQuit;

		public static EditorApplication.ProjectWindowItemCallback projectWindowItemOnGUI;

		public static EditorApplication.HierarchyWindowItemCallback hierarchyWindowItemOnGUI;

		public static EditorApplication.CallbackFunction update;

		public static EditorApplication.CallbackFunction delayCall;

		[Obsolete("Use EditorApplication.hierarchyChanged")]
		public static EditorApplication.CallbackFunction hierarchyWindowChanged;

		[Obsolete("Use EditorApplication.projectChanged")]
		public static EditorApplication.CallbackFunction projectWindowChanged;

		public static EditorApplication.CallbackFunction searchChanged;

		internal static EditorApplication.CallbackFunction assetLabelsChanged;

		internal static EditorApplication.CallbackFunction assetBundleNameChanged;

		public static EditorApplication.CallbackFunction modifierKeysChanged;

		[Obsolete("Use EditorApplication.playModeStateChanged and/or EditorApplication.pauseStateChanged")]
		public static EditorApplication.CallbackFunction playmodeStateChanged;

		internal static EditorApplication.CallbackFunction globalEventHandler;

		internal static EditorApplication.CallbackFunction windowsReordered;

		public static EditorApplication.SerializedPropertyCallbackFunction contextualPropertyMenu;

		private static EditorApplication.CallbackFunction delayedCallback;

		private static float s_DelayedCallbackTime = 0f;

		[CompilerGenerated]
		private static EditorApplication.CallbackFunction <>f__mg$cache0;

		[CompilerGenerated]
		private static EditorApplication.CallbackFunction <>f__mg$cache1;

		public static event Func<bool> wantsToQuit
		{
			add
			{
				Func<bool> func = EditorApplication.wantsToQuit;
				Func<bool> func2;
				do
				{
					func2 = func;
					func = Interlocked.CompareExchange<Func<bool>>(ref EditorApplication.wantsToQuit, (Func<bool>)Delegate.Combine(func2, value), func);
				}
				while (func != func2);
			}
			remove
			{
				Func<bool> func = EditorApplication.wantsToQuit;
				Func<bool> func2;
				do
				{
					func2 = func;
					func = Interlocked.CompareExchange<Func<bool>>(ref EditorApplication.wantsToQuit, (Func<bool>)Delegate.Remove(func2, value), func);
				}
				while (func != func2);
			}
		}

		public static event Action quitting
		{
			add
			{
				Action action = EditorApplication.quitting;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action>(ref EditorApplication.quitting, (Action)Delegate.Combine(action2, value), action);
				}
				while (action != action2);
			}
			remove
			{
				Action action = EditorApplication.quitting;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action>(ref EditorApplication.quitting, (Action)Delegate.Remove(action2, value), action);
				}
				while (action != action2);
			}
		}

		public static event Action hierarchyChanged
		{
			add
			{
				Action action = EditorApplication.hierarchyChanged;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action>(ref EditorApplication.hierarchyChanged, (Action)Delegate.Combine(action2, value), action);
				}
				while (action != action2);
			}
			remove
			{
				Action action = EditorApplication.hierarchyChanged;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action>(ref EditorApplication.hierarchyChanged, (Action)Delegate.Remove(action2, value), action);
				}
				while (action != action2);
			}
		}

		public static event Action projectChanged
		{
			add
			{
				Action action = EditorApplication.projectChanged;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action>(ref EditorApplication.projectChanged, (Action)Delegate.Combine(action2, value), action);
				}
				while (action != action2);
			}
			remove
			{
				Action action = EditorApplication.projectChanged;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action>(ref EditorApplication.projectChanged, (Action)Delegate.Remove(action2, value), action);
				}
				while (action != action2);
			}
		}

		public static event Action<PauseState> pauseStateChanged
		{
			add
			{
				Action<PauseState> action = EditorApplication.pauseStateChanged;
				Action<PauseState> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action<PauseState>>(ref EditorApplication.pauseStateChanged, (Action<PauseState>)Delegate.Combine(action2, value), action);
				}
				while (action != action2);
			}
			remove
			{
				Action<PauseState> action = EditorApplication.pauseStateChanged;
				Action<PauseState> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action<PauseState>>(ref EditorApplication.pauseStateChanged, (Action<PauseState>)Delegate.Remove(action2, value), action);
				}
				while (action != action2);
			}
		}

		public static event Action<PlayModeStateChange> playModeStateChanged
		{
			add
			{
				Action<PlayModeStateChange> action = EditorApplication.playModeStateChanged;
				Action<PlayModeStateChange> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action<PlayModeStateChange>>(ref EditorApplication.playModeStateChanged, (Action<PlayModeStateChange>)Delegate.Combine(action2, value), action);
				}
				while (action != action2);
			}
			remove
			{
				Action<PlayModeStateChange> action = EditorApplication.playModeStateChanged;
				Action<PlayModeStateChange> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action<PlayModeStateChange>>(ref EditorApplication.playModeStateChanged, (Action<PlayModeStateChange>)Delegate.Remove(action2, value), action);
				}
				while (action != action2);
			}
		}

		public static extern bool isPlaying
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern bool isPlayingOrWillChangePlaymode
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public static extern bool isPaused
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern bool isCompiling
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public static extern bool isUpdating
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public static extern bool isRemoteConnected
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public static extern ScriptingRuntimeVersion scriptingRuntimeVersion
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		internal static extern bool useLibmonoBackendForIl2cpp
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		[ThreadAndSerializationSafe]
		public static extern string applicationContentsPath
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public static extern string applicationPath
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		internal static extern string userJavascriptPackagesPath
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public static extern bool isTemporaryProject
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		internal static extern UnityEngine.Object tagManager
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		internal static extern UnityEngine.Object renderSettings
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public static extern double timeSinceStartup
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		internal static bool supportsHiDPI
		{
			get
			{
				return Application.platform == RuntimePlatform.OSXEditor;
			}
		}

		[Obsolete("Use Scene.isDirty instead. Use EditorSceneManager.GetScene API to get each open scene")]
		public static bool isSceneDirty
		{
			get
			{
				return SceneManager.GetActiveScene().isDirty;
			}
		}

		[Obsolete("Use EditorSceneManager to see which scenes are currently loaded")]
		public static string currentScene
		{
			get
			{
				Scene activeScene = SceneManager.GetActiveScene();
				string result;
				if (activeScene.IsValid())
				{
					result = activeScene.path;
				}
				else
				{
					result = "";
				}
				return result;
			}
			set
			{
			}
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void LoadLevelInPlayMode(string path);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void LoadLevelAdditiveInPlayMode(string path);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern AsyncOperation LoadLevelAsyncInPlayMode(string path);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern AsyncOperation LoadLevelAdditiveAsyncInPlayMode(string path);

		public static void OpenProject(string projectPath, params string[] args)
		{
			EditorApplication.OpenProjectInternal(projectPath, args);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void OpenProjectInternal(string projectPath, string[] args);

		[Obsolete("Use AssetDatabase.SaveAssets instead (UnityUpgradable) -> AssetDatabase.SaveAssets()", true), GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SaveAssets();

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Step();

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void LockReloadAssemblies();

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool ExecuteMenuItem(string menuItemPath);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool ExecuteMenuItemOnGameObjects(string menuItemPath, GameObject[] objects);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool ExecuteMenuItemWithTemporaryContext(string menuItemPath, UnityEngine.Object[] objects);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void UnlockReloadAssemblies();

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetTemporaryProjectKeepPath(string path);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Exit(int returnValue);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void SetSceneRepaintDirty();

		public static void QueuePlayerLoopUpdate()
		{
			EditorApplication.SetSceneRepaintDirty();
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void UpdateSceneIfNeeded();

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Beep();

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void CloseAndRelaunch(string[] arguments);

		private static void Internal_ProjectWasLoaded()
		{
			if (EditorApplication.projectWasLoaded != null)
			{
				EditorApplication.projectWasLoaded();
			}
		}

		[RequiredByNativeCode]
		private static bool Internal_EditorApplicationWantsToQuit()
		{
			bool result;
			if (EditorApplication.wantsToQuit == null)
			{
				result = true;
			}
			else
			{
				Delegate[] invocationList = EditorApplication.wantsToQuit.GetInvocationList();
				for (int i = 0; i < invocationList.Length; i++)
				{
					Func<bool> func = (Func<bool>)invocationList[i];
					try
					{
						if (!func())
						{
							result = false;
							return result;
						}
					}
					catch (Exception ex)
					{
						Debug.LogWarningFormat("EditorApplication.wantsToQuit: Exception raised during quit event." + Environment.NewLine + "Check the exception error's callstack to find out which event handler threw the exception.", new object[0]);
						Debug.LogException(ex);
						if (InternalEditorUtility.isHumanControllingUs)
						{
							string stackTrace = ex.StackTrace;
							StringBuilder stringBuilder = new StringBuilder("An exception was thrown here:");
							stringBuilder.AppendLine(Environment.NewLine);
							stringBuilder.AppendLine(stackTrace.Substring(0, stackTrace.IndexOf(Environment.NewLine)));
							bool flag = !EditorUtility.DisplayDialog("Error while quitting", stringBuilder.ToString(), "Ignore", "Cancel Quit");
							if (flag)
							{
								result = false;
								return result;
							}
						}
					}
				}
				result = true;
			}
			return result;
		}

		private static void Internal_EditorApplicationQuit()
		{
			if (EditorApplication.quitting != null)
			{
				EditorApplication.quitting();
			}
			if (EditorApplication.editorApplicationQuit != null)
			{
				EditorApplication.editorApplicationQuit();
			}
		}

		public static void RepaintProjectWindow()
		{
			foreach (ProjectBrowser current in ProjectBrowser.GetAllProjectBrowsers())
			{
				current.Repaint();
			}
		}

		public static void RepaintAnimationWindow()
		{
			foreach (AnimEditor current in AnimEditor.GetAllAnimationWindows())
			{
				current.Repaint();
			}
		}

		public static void RepaintHierarchyWindow()
		{
			UnityEngine.Object[] array = Resources.FindObjectsOfTypeAll(typeof(SceneHierarchyWindow));
			for (int i = 0; i < array.Length; i++)
			{
				SceneHierarchyWindow sceneHierarchyWindow = (SceneHierarchyWindow)array[i];
				sceneHierarchyWindow.Repaint();
			}
		}

		public static void DirtyHierarchyWindowSorting()
		{
			UnityEngine.Object[] array = Resources.FindObjectsOfTypeAll(typeof(SceneHierarchyWindow));
			for (int i = 0; i < array.Length; i++)
			{
				SceneHierarchyWindow sceneHierarchyWindow = (SceneHierarchyWindow)array[i];
				sceneHierarchyWindow.DirtySortingMethods();
			}
		}

		private static void Internal_CallUpdateFunctions()
		{
			if (EditorApplication.update != null)
			{
				EditorApplication.update();
			}
		}

		private static void Internal_CallDelayFunctions()
		{
			EditorApplication.CallbackFunction callbackFunction = EditorApplication.delayCall;
			EditorApplication.delayCall = null;
			if (callbackFunction != null)
			{
				callbackFunction();
			}
		}

		private static void Internal_SwitchSkin()
		{
			EditorGUIUtility.Internal_SwitchSkin();
		}

		internal static void RequestRepaintAllViews()
		{
			UnityEngine.Object[] array = Resources.FindObjectsOfTypeAll(typeof(GUIView));
			for (int i = 0; i < array.Length; i++)
			{
				GUIView gUIView = (GUIView)array[i];
				gUIView.Repaint();
			}
		}

		private static void Internal_CallHierarchyHasChanged()
		{
			if (EditorApplication.hierarchyWindowChanged != null)
			{
				EditorApplication.hierarchyWindowChanged();
			}
			if (EditorApplication.hierarchyChanged != null)
			{
				EditorApplication.hierarchyChanged();
			}
		}

		private static void Internal_CallProjectHasChanged()
		{
			if (EditorApplication.projectWindowChanged != null)
			{
				EditorApplication.projectWindowChanged();
			}
			if (EditorApplication.projectChanged != null)
			{
				EditorApplication.projectChanged();
			}
		}

		internal static void Internal_CallSearchHasChanged()
		{
			if (EditorApplication.searchChanged != null)
			{
				EditorApplication.searchChanged();
			}
		}

		internal static void Internal_CallAssetLabelsHaveChanged()
		{
			if (EditorApplication.assetLabelsChanged != null)
			{
				EditorApplication.assetLabelsChanged();
			}
		}

		internal static void Internal_CallAssetBundleNameChanged()
		{
			if (EditorApplication.assetBundleNameChanged != null)
			{
				EditorApplication.assetBundleNameChanged();
			}
		}

		internal static void CallDelayed(EditorApplication.CallbackFunction function, float timeFromNow)
		{
			EditorApplication.delayedCallback = function;
			EditorApplication.s_DelayedCallbackTime = Time.realtimeSinceStartup + timeFromNow;
			Delegate arg_35_0 = EditorApplication.update;
			if (EditorApplication.<>f__mg$cache0 == null)
			{
				EditorApplication.<>f__mg$cache0 = new EditorApplication.CallbackFunction(EditorApplication.CheckCallDelayed);
			}
			EditorApplication.update = (EditorApplication.CallbackFunction)Delegate.Combine(arg_35_0, EditorApplication.<>f__mg$cache0);
		}

		private static void CheckCallDelayed()
		{
			if (Time.realtimeSinceStartup > EditorApplication.s_DelayedCallbackTime)
			{
				Delegate arg_33_0 = EditorApplication.update;
				if (EditorApplication.<>f__mg$cache1 == null)
				{
					EditorApplication.<>f__mg$cache1 = new EditorApplication.CallbackFunction(EditorApplication.CheckCallDelayed);
				}
				EditorApplication.update = (EditorApplication.CallbackFunction)Delegate.Remove(arg_33_0, EditorApplication.<>f__mg$cache1);
				EditorApplication.delayedCallback();
			}
		}

		private static void Internal_PauseStateChanged(PauseState state)
		{
			if (EditorApplication.playmodeStateChanged != null)
			{
				EditorApplication.playmodeStateChanged();
			}
			if (EditorApplication.pauseStateChanged != null)
			{
				EditorApplication.pauseStateChanged(state);
			}
		}

		private static void Internal_PlayModeStateChanged(PlayModeStateChange state)
		{
			if (EditorApplication.playmodeStateChanged != null)
			{
				EditorApplication.playmodeStateChanged();
			}
			if (EditorApplication.playModeStateChanged != null)
			{
				EditorApplication.playModeStateChanged(state);
			}
		}

		private static void Internal_CallKeyboardModifiersChanged()
		{
			if (EditorApplication.modifierKeysChanged != null)
			{
				EditorApplication.modifierKeysChanged();
			}
		}

		private static void Internal_CallWindowsReordered()
		{
			if (EditorApplication.windowsReordered != null)
			{
				EditorApplication.windowsReordered();
			}
		}

		[RequiredByNativeCode]
		private static void Internal_CallGlobalEventHandler()
		{
			if (EditorApplication.globalEventHandler != null)
			{
				EditorApplication.globalEventHandler();
			}
			WindowLayout.MaximizeKeyHandler();
			Event.current = null;
		}

		[Obsolete("Use EditorSceneManager.NewScene (NewSceneSetup.DefaultGameObjects)")]
		public static void NewScene()
		{
			EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects);
		}

		[Obsolete("Use EditorSceneManager.NewScene (NewSceneSetup.EmptyScene)")]
		public static void NewEmptyScene()
		{
			EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
		}

		[Obsolete("Use EditorSceneManager.OpenScene")]
		public static bool OpenScene(string path)
		{
			if (!EditorApplication.isPlaying)
			{
				return EditorSceneManager.OpenScene(path).IsValid();
			}
			throw new InvalidOperationException("EditorApplication.OpenScene() cannot be called when in the Unity Editor is in play mode.");
		}

		[Obsolete("Use EditorSceneManager.OpenScene")]
		public static void OpenSceneAdditive(string path)
		{
			if (Application.isPlaying)
			{
				Debug.LogWarning("Exiting playmode.\nOpenSceneAdditive was called at a point where there was no active scene.\nThis usually means it was called in a PostprocessScene function during scene loading or it was called during playmode.\nThis is no longer allowed. Use SceneManager.LoadScene to load scenes at runtime or in playmode.");
			}
			Scene sourceScene = EditorSceneManager.OpenScene(path, OpenSceneMode.Additive);
			Scene activeScene = SceneManager.GetActiveScene();
			SceneManager.MergeScenes(sourceScene, activeScene);
		}

		[Obsolete("Use EditorSceneManager.SaveScene")]
		public static bool SaveScene()
		{
			return EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), "", false);
		}

		[Obsolete("Use EditorSceneManager.SaveScene")]
		public static bool SaveScene(string path)
		{
			return EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), path, false);
		}

		[Obsolete("Use EditorSceneManager.SaveScene")]
		public static bool SaveScene(string path, bool saveAsCopy)
		{
			return EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), path, saveAsCopy);
		}

		[Obsolete("Use EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo")]
		public static bool SaveCurrentSceneIfUserWantsTo()
		{
			return EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
		}

		[Obsolete("This function is internal and no longer supported")]
		internal static bool SaveCurrentSceneIfUserWantsToForce()
		{
			return false;
		}

		[Obsolete("Use EditorSceneManager.MarkSceneDirty or EditorSceneManager.MarkAllScenesDirty")]
		public static void MarkSceneDirty()
		{
			EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
		}
	}
}
