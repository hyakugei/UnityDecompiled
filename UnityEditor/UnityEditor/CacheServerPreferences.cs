using System;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
	internal class CacheServerPreferences
	{
		internal class Styles
		{
			public static readonly GUIContent browse = EditorGUIUtility.TrTextContent("Browse...", null, null);

			public static readonly GUIContent maxCacheSize = EditorGUIUtility.TrTextContent("Maximum Cache Size (GB)", "The size of the local asset cache server folder will be kept below this maximum value.", null);

			public static readonly GUIContent customCacheLocation = EditorGUIUtility.TrTextContent("Custom cache location", "Specify the local asset cache server folder location.", null);

			public static readonly GUIContent cacheFolderLocation = EditorGUIUtility.TrTextContent("Cache Folder Location", "The local asset cache server folder is shared between all projects.", null);

			public static readonly GUIContent cleanCache = EditorGUIUtility.TrTextContent("Clean Cache", null, null);

			public static readonly GUIContent enumerateCache = EditorGUIUtility.TrTextContent("Check Cache Size", "Check the size of the local asset cache server - can take a while", null);

			public static readonly GUIContent browseCacheLocation = EditorGUIUtility.TrTextContent("Browse for local asset cache server location", null, null);
		}

		internal class Constants
		{
			public GUIStyle cacheFolderLocation = new GUIStyle(GUI.skin.label);

			public Constants()
			{
				this.cacheFolderLocation.wordWrap = true;
			}
		}

		private enum ConnectionState
		{
			Unknown,
			Success,
			Failure
		}

		public enum CacheServerMode
		{
			Local,
			Remote,
			Disabled
		}

		private const string kIPAddressKey = "CacheServerIPAddress";

		private const string kIpAddressKeyArgs = "-CacheServerIPAddress";

		private const string kModeKey = "CacheServerMode";

		private const string kDeprecatedEnabledKey = "CacheServerEnabled";

		private static bool s_PrefsLoaded;

		private static bool s_HasPendingChanges = false;

		private static CacheServerPreferences.ConnectionState s_ConnectionState;

		private static CacheServerPreferences.CacheServerMode s_CacheServerMode;

		private static string s_CacheServerIPAddress;

		private static int s_LocalCacheServerSize;

		private static long s_LocalCacheServerUsedSize = -1L;

		private static bool s_EnableCustomPath;

		private static string s_CachePath;

		private static CacheServerPreferences.Constants s_Constants = null;

		public static void ReadPreferences()
		{
			CacheServerPreferences.s_CacheServerIPAddress = EditorPrefs.GetString("CacheServerIPAddress", CacheServerPreferences.s_CacheServerIPAddress);
			CacheServerPreferences.s_CacheServerMode = (CacheServerPreferences.CacheServerMode)EditorPrefs.GetInt("CacheServerMode", (!EditorPrefs.GetBool("CacheServerEnabled")) ? 2 : 1);
			CacheServerPreferences.s_LocalCacheServerSize = EditorPrefs.GetInt("LocalCacheServerSize", 10);
			CacheServerPreferences.s_CachePath = EditorPrefs.GetString("LocalCacheServerPath");
			CacheServerPreferences.s_EnableCustomPath = EditorPrefs.GetBool("LocalCacheServerCustomPath");
		}

		public static void WritePreferences()
		{
			if (CacheServerPreferences.GetCommandLineRemoteAddressOverride() == null)
			{
				CacheServerPreferences.CacheServerMode @int = (CacheServerPreferences.CacheServerMode)EditorPrefs.GetInt("CacheServerMode");
				string @string = EditorPrefs.GetString("LocalCacheServerPath");
				bool @bool = EditorPrefs.GetBool("LocalCacheServerCustomPath");
				bool flag = false;
				if (@int != CacheServerPreferences.s_CacheServerMode && @int == CacheServerPreferences.CacheServerMode.Local)
				{
					flag = true;
				}
				if (CacheServerPreferences.s_EnableCustomPath && @string != CacheServerPreferences.s_CachePath)
				{
					flag = true;
				}
				if (CacheServerPreferences.s_EnableCustomPath != @bool && CacheServerPreferences.s_CachePath != LocalCacheServer.GetCacheLocation() && CacheServerPreferences.s_CachePath != "")
				{
					flag = true;
				}
				if (flag)
				{
					CacheServerPreferences.s_LocalCacheServerUsedSize = -1L;
					string text = (CacheServerPreferences.s_CacheServerMode != CacheServerPreferences.CacheServerMode.Local) ? "You have disabled the local cache." : "You have changed the location of the local cache storage.";
					text = text + " Do you want to delete the old locally cached data at " + LocalCacheServer.GetCacheLocation() + "?";
					if (EditorUtility.DisplayDialog("Delete old Cache", text, "Delete", "Don't Delete"))
					{
						LocalCacheServer.Clear();
						CacheServerPreferences.s_LocalCacheServerUsedSize = -1L;
					}
				}
				EditorPrefs.SetString("CacheServerIPAddress", CacheServerPreferences.s_CacheServerIPAddress);
				EditorPrefs.SetInt("CacheServerMode", (int)CacheServerPreferences.s_CacheServerMode);
				EditorPrefs.SetInt("LocalCacheServerSize", CacheServerPreferences.s_LocalCacheServerSize);
				EditorPrefs.SetString("LocalCacheServerPath", CacheServerPreferences.s_CachePath);
				EditorPrefs.SetBool("LocalCacheServerCustomPath", CacheServerPreferences.s_EnableCustomPath);
				LocalCacheServer.Setup();
				if (flag)
				{
					GUIUtility.ExitGUI();
				}
			}
		}

		private static string GetCommandLineRemoteAddressOverride()
		{
			string result = null;
			string[] commandLineArgs = Environment.GetCommandLineArgs();
			int num = Array.IndexOf<string>(commandLineArgs, "-CacheServerIPAddress");
			if (num >= 0 && commandLineArgs.Length > num + 1)
			{
				result = commandLineArgs[num + 1];
			}
			return result;
		}

		[PreferenceItem("Cache Server")]
		private static void OnGUI()
		{
			EventType type = Event.current.type;
			if (CacheServerPreferences.s_Constants == null)
			{
				CacheServerPreferences.s_Constants = new CacheServerPreferences.Constants();
			}
			if (!InternalEditorUtility.HasTeamLicense())
			{
				GUILayout.Label(EditorGUIUtility.TempContent("You need to have a Pro or Team license to use the cache server.", EditorGUIUtility.GetHelpIcon(MessageType.Warning)), EditorStyles.helpBox, new GUILayoutOption[0]);
			}
			using (new EditorGUI.DisabledScope(!InternalEditorUtility.HasTeamLicense()))
			{
				if (!CacheServerPreferences.s_PrefsLoaded)
				{
					CacheServerPreferences.ReadPreferences();
					if (CacheServerPreferences.s_CacheServerMode != CacheServerPreferences.CacheServerMode.Disabled && CacheServerPreferences.s_ConnectionState == CacheServerPreferences.ConnectionState.Unknown)
					{
						if (InternalEditorUtility.CanConnectToCacheServer())
						{
							CacheServerPreferences.s_ConnectionState = CacheServerPreferences.ConnectionState.Success;
						}
						else
						{
							CacheServerPreferences.s_ConnectionState = CacheServerPreferences.ConnectionState.Failure;
						}
					}
					CacheServerPreferences.s_PrefsLoaded = true;
				}
				EditorGUI.BeginChangeCheck();
				string commandLineRemoteAddressOverride = CacheServerPreferences.GetCommandLineRemoteAddressOverride();
				if (commandLineRemoteAddressOverride != null)
				{
					EditorGUILayout.HelpBox("Cache Server preferences cannot be modified because a remote address was specified via command line argument. To modify Cache Server preferences, restart Unity without the -CacheServerIPAddress command line argument.", MessageType.Info, true);
				}
				using (new EditorGUI.DisabledScope(commandLineRemoteAddressOverride != null))
				{
					CacheServerPreferences.CacheServerMode cacheServerMode = (commandLineRemoteAddressOverride == null) ? CacheServerPreferences.s_CacheServerMode : CacheServerPreferences.CacheServerMode.Remote;
					CacheServerPreferences.s_CacheServerMode = (CacheServerPreferences.CacheServerMode)EditorGUILayout.EnumPopup("Cache Server Mode", cacheServerMode, new GUILayoutOption[0]);
				}
				if (CacheServerPreferences.s_CacheServerMode == CacheServerPreferences.CacheServerMode.Remote)
				{
					using (new EditorGUI.DisabledScope(commandLineRemoteAddressOverride != null))
					{
						string text = (commandLineRemoteAddressOverride == null) ? CacheServerPreferences.s_CacheServerIPAddress : commandLineRemoteAddressOverride;
						CacheServerPreferences.s_CacheServerIPAddress = EditorGUILayout.DelayedTextField("IP Address", text, new GUILayoutOption[0]);
						if (GUI.changed)
						{
							CacheServerPreferences.s_ConnectionState = CacheServerPreferences.ConnectionState.Unknown;
						}
					}
					GUILayout.Space(5f);
					if (GUILayout.Button("Check Connection", new GUILayoutOption[]
					{
						GUILayout.Width(150f)
					}))
					{
						if (InternalEditorUtility.CanConnectToCacheServer())
						{
							CacheServerPreferences.s_ConnectionState = CacheServerPreferences.ConnectionState.Success;
						}
						else
						{
							CacheServerPreferences.s_ConnectionState = CacheServerPreferences.ConnectionState.Failure;
						}
					}
					GUILayout.Space(-25f);
					CacheServerPreferences.ConnectionState connectionState = CacheServerPreferences.s_ConnectionState;
					if (connectionState != CacheServerPreferences.ConnectionState.Success)
					{
						if (connectionState != CacheServerPreferences.ConnectionState.Failure)
						{
							if (connectionState == CacheServerPreferences.ConnectionState.Unknown)
							{
								GUILayout.Space(44f);
							}
						}
						else
						{
							EditorGUILayout.HelpBox("Connection failed.", MessageType.Warning, false);
						}
					}
					else
					{
						EditorGUILayout.HelpBox("Connection successful.", MessageType.Info, false);
					}
				}
				else if (CacheServerPreferences.s_CacheServerMode == CacheServerPreferences.CacheServerMode.Local)
				{
					CacheServerPreferences.s_LocalCacheServerSize = EditorGUILayout.IntSlider(CacheServerPreferences.Styles.maxCacheSize, CacheServerPreferences.s_LocalCacheServerSize, 1, 200, new GUILayoutOption[0]);
					CacheServerPreferences.s_EnableCustomPath = EditorGUILayout.Toggle(CacheServerPreferences.Styles.customCacheLocation, CacheServerPreferences.s_EnableCustomPath, new GUILayoutOption[0]);
					if (CacheServerPreferences.s_EnableCustomPath)
					{
						GUIStyle miniButton = EditorStyles.miniButton;
						GUILayout.BeginHorizontal(new GUILayoutOption[0]);
						EditorGUILayout.PrefixLabel(CacheServerPreferences.Styles.cacheFolderLocation, miniButton);
						Rect rect = GUILayoutUtility.GetRect(GUIContent.none, miniButton);
						GUIContent content = (!string.IsNullOrEmpty(CacheServerPreferences.s_CachePath)) ? new GUIContent(CacheServerPreferences.s_CachePath) : CacheServerPreferences.Styles.browse;
						if (EditorGUI.DropdownButton(rect, content, FocusType.Passive, miniButton))
						{
							string folder = CacheServerPreferences.s_CachePath;
							string text2 = EditorUtility.OpenFolderPanel(CacheServerPreferences.Styles.browseCacheLocation.text, folder, "");
							if (!string.IsNullOrEmpty(text2))
							{
								if (LocalCacheServer.CheckValidCacheLocation(text2))
								{
									CacheServerPreferences.s_CachePath = text2;
									CacheServerPreferences.WritePreferences();
								}
								else
								{
									EditorUtility.DisplayDialog("Invalid Cache Location", string.Format("The directory {0} contains some files which don't look like Unity Cache server files. Please delete the directory contents or choose another directory.", text2), "OK");
								}
								GUIUtility.ExitGUI();
							}
						}
						GUILayout.EndHorizontal();
					}
					else
					{
						CacheServerPreferences.s_CachePath = "";
					}
					bool flag = LocalCacheServer.CheckCacheLocationExists();
					if (flag)
					{
						GUIContent label = EditorGUIUtility.TrTextContent("Cache size is unknown", null, null);
						if (CacheServerPreferences.s_LocalCacheServerUsedSize != -1L)
						{
							label = EditorGUIUtility.TextContent("Cache size is " + EditorUtility.FormatBytes(CacheServerPreferences.s_LocalCacheServerUsedSize));
						}
						GUILayout.BeginHorizontal(new GUILayoutOption[0]);
						GUIStyle miniButton2 = EditorStyles.miniButton;
						EditorGUILayout.PrefixLabel(label, miniButton2);
						Rect rect2 = GUILayoutUtility.GetRect(GUIContent.none, miniButton2);
						if (EditorGUI.Button(rect2, CacheServerPreferences.Styles.enumerateCache, miniButton2))
						{
							CacheServerPreferences.s_LocalCacheServerUsedSize = ((!LocalCacheServer.CheckCacheLocationExists()) ? 0L : FileUtil.GetDirectorySize(LocalCacheServer.GetCacheLocation()));
						}
						GUILayout.EndHorizontal();
						GUILayout.BeginHorizontal(new GUILayoutOption[0]);
						GUIContent blankContent = EditorGUIUtility.blankContent;
						EditorGUILayout.PrefixLabel(blankContent, miniButton2);
						Rect rect3 = GUILayoutUtility.GetRect(GUIContent.none, miniButton2);
						if (EditorGUI.Button(rect3, CacheServerPreferences.Styles.cleanCache, miniButton2))
						{
							LocalCacheServer.Clear();
							CacheServerPreferences.s_LocalCacheServerUsedSize = 0L;
						}
						GUILayout.EndHorizontal();
					}
					else
					{
						EditorGUILayout.HelpBox("Local cache directory does not exist - please check that you can access the cache folder and are able to write to it", MessageType.Warning, false);
						CacheServerPreferences.s_LocalCacheServerUsedSize = -1L;
					}
					GUILayout.Label(CacheServerPreferences.Styles.cacheFolderLocation.text + ":", new GUILayoutOption[0]);
					GUILayout.Label(LocalCacheServer.GetCacheLocation(), CacheServerPreferences.s_Constants.cacheFolderLocation, new GUILayoutOption[0]);
				}
				if (EditorGUI.EndChangeCheck())
				{
					CacheServerPreferences.s_HasPendingChanges = true;
				}
				if (CacheServerPreferences.s_HasPendingChanges && GUIUtility.hotControl == 0)
				{
					CacheServerPreferences.s_HasPendingChanges = false;
					CacheServerPreferences.WritePreferences();
					CacheServerPreferences.ReadPreferences();
				}
			}
		}
	}
}
