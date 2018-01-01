using System;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using UnityEngine.Scripting;

namespace UnityEditor
{
	public class EditorBuildSettings : UnityEngine.Object
	{
		private enum ConfigObjectResult
		{
			Succeeded,
			FailedEntryNotFound,
			FailedNullObj,
			FailedNonPersistedObj,
			FailedEntryExists,
			FailedTypeMismatch
		}

		public static event Action sceneListChanged
		{
			add
			{
				Action action = EditorBuildSettings.sceneListChanged;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action>(ref EditorBuildSettings.sceneListChanged, (Action)Delegate.Combine(action2, value), action);
				}
				while (action != action2);
			}
			remove
			{
				Action action = EditorBuildSettings.sceneListChanged;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action>(ref EditorBuildSettings.sceneListChanged, (Action)Delegate.Remove(action2, value), action);
				}
				while (action != action2);
			}
		}

		public static EditorBuildSettingsScene[] scenes
		{
			get
			{
				return EditorBuildSettings.GetEditorBuildSettingsScenes();
			}
			set
			{
				EditorBuildSettings.SetEditorBuildSettingsScenes(value);
			}
		}

		private EditorBuildSettings()
		{
		}

		[RequiredByNativeCode]
		private static void SceneListChanged()
		{
			if (EditorBuildSettings.sceneListChanged != null)
			{
				EditorBuildSettings.sceneListChanged();
			}
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern EditorBuildSettingsScene[] GetEditorBuildSettingsScenes();

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetEditorBuildSettingsScenes(EditorBuildSettingsScene[] scenes);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern EditorBuildSettings.ConfigObjectResult AddConfigObjectInternal(string name, UnityEngine.Object obj, bool overwrite);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool RemoveConfigObject(string name);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern string[] GetConfigObjectNames();

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern UnityEngine.Object GetConfigObject(string name);

		public static void AddConfigObject(string name, UnityEngine.Object obj, bool overwrite)
		{
			EditorBuildSettings.ConfigObjectResult configObjectResult = EditorBuildSettings.AddConfigObjectInternal(name, obj, overwrite);
			if (configObjectResult != EditorBuildSettings.ConfigObjectResult.Succeeded)
			{
				if (configObjectResult == EditorBuildSettings.ConfigObjectResult.FailedEntryExists)
				{
					throw new Exception("Config object with name '" + name + "' already exists.");
				}
				if (configObjectResult == EditorBuildSettings.ConfigObjectResult.FailedNonPersistedObj)
				{
					throw new Exception("Cannot add non-persisted config object with name '" + name + "'.");
				}
				if (configObjectResult == EditorBuildSettings.ConfigObjectResult.FailedNullObj)
				{
					throw new Exception("Cannot add null config object with name '" + name + "'.");
				}
			}
		}

		public static bool TryGetConfigObject<T>(string name, out T result) where T : UnityEngine.Object
		{
			result = (EditorBuildSettings.GetConfigObject(name) as T);
			return result != null;
		}
	}
}
