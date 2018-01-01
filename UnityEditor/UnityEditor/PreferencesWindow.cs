using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEditor.Collaboration;
using UnityEditor.Connect;
using UnityEditor.Modules;
using UnityEditor.VisualStudioIntegration;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
	internal class PreferencesWindow : EditorWindow
	{
		internal class Constants
		{
			public GUIStyle sectionScrollView = "PreferencesSectionBox";

			public GUIStyle settingsBoxTitle = "OL Title";

			public GUIStyle settingsBox = "OL Box";

			public GUIStyle errorLabel = "WordWrappedLabel";

			public GUIStyle sectionElement = "PreferencesSection";

			public GUIStyle evenRow = "CN EntryBackEven";

			public GUIStyle oddRow = "CN EntryBackOdd";

			public GUIStyle selected = "OL SelectedRow";

			public GUIStyle keysElement = "PreferencesKeysElement";

			public GUIStyle warningIcon = "CN EntryWarn";

			public GUIStyle sectionHeader = new GUIStyle(EditorStyles.largeLabel);

			public GUIStyle cacheFolderLocation = new GUIStyle(GUI.skin.label);

			public Constants()
			{
				this.sectionScrollView = new GUIStyle(this.sectionScrollView);
				this.sectionScrollView.overflow.bottom++;
				this.sectionHeader.fontStyle = FontStyle.Bold;
				this.sectionHeader.fontSize = 18;
				this.sectionHeader.margin.top = 10;
				this.sectionHeader.margin.left++;
				if (!EditorGUIUtility.isProSkin)
				{
					this.sectionHeader.normal.textColor = new Color(0.4f, 0.4f, 0.4f, 1f);
				}
				else
				{
					this.sectionHeader.normal.textColor = new Color(0.7f, 0.7f, 0.7f, 1f);
				}
				this.cacheFolderLocation.wordWrap = true;
			}
		}

		internal class Styles
		{
			public static readonly GUIContent browse = EditorGUIUtility.TrTextContent("Browse...", null, null);

			public static readonly GUIContent autoRefresh = EditorGUIUtility.TrTextContent("Auto Refresh", null, null);

			public static readonly GUIContent autoRefreshHelpBox = EditorGUIUtility.TrTextContent("Auto Refresh must be set when using Collaboration feature.", EditorGUIUtility.GetHelpIcon(MessageType.Warning));

			public static readonly GUIContent loadPreviousProjectOnStartup = EditorGUIUtility.TrTextContent("Load Previous Project on Startup", null, null);

			public static readonly GUIContent compressAssetsOnImport = EditorGUIUtility.TrTextContent("Compress Assets on Import", null, null);

			public static readonly GUIContent osxColorPicker = EditorGUIUtility.TrTextContent("OS X Color Picker", null, null);

			public static readonly GUIContent disableEditorAnalytics = EditorGUIUtility.TrTextContent("Disable Editor Analytics (Pro Only)", null, null);

			public static readonly GUIContent showAssetStoreSearchHits = EditorGUIUtility.TrTextContent("Show Asset Store search hits", null, null);

			public static readonly GUIContent verifySavingAssets = EditorGUIUtility.TrTextContent("Verify Saving Assets", null, null);

			public static readonly GUIContent editorSkin = EditorGUIUtility.TrTextContent("Editor Skin", null, null);

			public static readonly GUIContent[] editorSkinOptions = new GUIContent[]
			{
				EditorGUIUtility.TrTextContent("Personal", null, null),
				EditorGUIUtility.TrTextContent("Professional", null, null)
			};

			public static readonly GUIContent enableAlphaNumericSorting = EditorGUIUtility.TrTextContent("Enable Alpha Numeric Sorting", null, null);

			public static readonly GUIContent downloadMonoDevelopInstaller = EditorGUIUtility.TrTextContent("Download MonoDevelop Installer", null, null);

			public static readonly GUIContent addUnityProjeToSln = EditorGUIUtility.TrTextContent("Add .unityproj's to .sln", null, null);

			public static readonly GUIContent editorAttaching = EditorGUIUtility.TrTextContent("Editor Attaching", null, null);

			public static readonly GUIContent changingThisSettingRequiresRestart = EditorGUIUtility.TrTextContent("Changing this setting requires a restart to take effect.", null, null);

			public static readonly GUIContent revisionControlDiffMerge = EditorGUIUtility.TrTextContent("Revision Control Diff/Merge", null, null);

			public static readonly GUIContent externalScriptEditor = EditorGUIUtility.TrTextContent("External Script Editor", null, null);

			public static readonly GUIContent imageApplication = EditorGUIUtility.TrTextContent("Image application", null, null);

			public static readonly GUIContent userDefaults = EditorGUIUtility.TrTextContent("Use Defaults", null, null);

			public static readonly GUIContent maxCacheSize = EditorGUIUtility.TrTextContent("Maximum Cache Size (GB)", "The size of the GI Cache folder will be kept below this maximum value when possible. A background job will periodically clean up the oldest unused files.", null);

			public static readonly GUIContent customCacheLocation = EditorGUIUtility.TrTextContent("Custom cache location", "Specify the GI Cache folder location.", null);

			public static readonly GUIContent cacheFolderLocation = EditorGUIUtility.TrTextContent("Cache Folder Location", "The GI Cache folder is shared between all projects.", null);

			public static readonly GUIContent cacheCompression = EditorGUIUtility.TrTextContent("Cache compression", "Use fast realtime compression for the GI cache files to reduce the size of generated data. Disable it and clean the cache if you need access to the raw data generated by Enlighten.", null);

			public static readonly GUIContent cantChangeCacheSettings = EditorGUIUtility.TrTextContent("Cache settings can't be changed while lightmapping is being computed.", null, null);

			public static readonly GUIContent cleanCache = EditorGUIUtility.TrTextContent("Clean Cache", null, null);

			public static readonly GUIContent browseGICacheLocation = EditorGUIUtility.TrTextContent("Browse for GI Cache location", null, null);

			public static readonly GUIContent cacheSizeIs = EditorGUIUtility.TrTextContent("Cache size is", null, null);

			public static readonly GUIContent pleaseWait = EditorGUIUtility.TrTextContent("Please wait...", null, null);

			public static readonly GUIContent spriteMaxCacheSize = EditorGUIUtility.TrTextContent("Max Sprite Atlas Cache Size (GB)", "The size of the Sprite Atlas Cache folder will be kept below this maximum value when possible. Change requires Editor restart", null);

			public static readonly GUIContent editorLanguageExperimental = EditorGUIUtility.TrTextContent("Editor Language(Experimental)", null, null);

			public static readonly GUIContent editorLanguage = EditorGUIUtility.TrTextContent("Editor language", null, null);
		}

		private delegate void OnGUIDelegate();

		private class Section
		{
			public GUIContent content;

			public PreferencesWindow.OnGUIDelegate guiFunc;

			public Section(string name, PreferencesWindow.OnGUIDelegate guiFunc)
			{
				this.content = EditorGUIUtility.TrTextContent(name, null, null);
				this.guiFunc = guiFunc;
			}

			public Section(string name, Texture2D icon, PreferencesWindow.OnGUIDelegate guiFunc)
			{
				this.content = EditorGUIUtility.TrTextContent(name, icon);
				this.guiFunc = guiFunc;
			}

			public Section(GUIContent content, PreferencesWindow.OnGUIDelegate guiFunc)
			{
				this.content = content;
				this.guiFunc = guiFunc;
			}
		}

		private struct GICacheSettings
		{
			public bool m_EnableCustomPath;

			public int m_MaximumSize;

			public string m_CachePath;

			public int m_CompressionLevel;
		}

		private class RefString
		{
			public string str;

			public RefString(string s)
			{
				this.str = s;
			}

			public static implicit operator string(PreferencesWindow.RefString s)
			{
				return s.str;
			}

			public override string ToString()
			{
				return this.str;
			}
		}

		private class AppsListUserData
		{
			public string[] paths;

			public PreferencesWindow.RefString str;

			public Action onChanged;

			public AppsListUserData(string[] paths, PreferencesWindow.RefString str, Action onChanged)
			{
				this.paths = paths;
				this.str = str;
				this.onChanged = onChanged;
			}
		}

		private List<PreferencesWindow.Section> m_Sections;

		private int m_SelectedSectionIndex;

		private static PreferencesWindow.Constants constants = null;

		private List<IPreferenceWindowExtension> prefWinExtensions;

		private bool m_AutoRefresh;

		private bool m_ReopenLastUsedProjectOnStartup;

		private bool m_CompressAssetsOnImport;

		private bool m_UseOSColorPicker;

		private bool m_EnableEditorAnalytics;

		private bool m_ShowAssetStoreSearchHits;

		private bool m_VerifySavingAssets;

		private bool m_DeveloperMode;

		private bool m_DeveloperModeDirty;

		private bool m_AllowAttachedDebuggingOfEditor;

		private bool m_AllowAttachedDebuggingOfEditorStateChangedThisSession;

		private string m_GpuDevice;

		private string[] m_CachedGpuDevices;

		private PreferencesWindow.GICacheSettings m_GICacheSettings;

		private PreferencesWindow.RefString m_ScriptEditorPath = new PreferencesWindow.RefString("");

		private string m_ScriptEditorArgs = "";

		private bool m_ExternalEditorSupportsUnityProj;

		private PreferencesWindow.RefString m_ImageAppPath = new PreferencesWindow.RefString("");

		private int m_DiffToolIndex;

		private const int k_LangListMenuOffset = 2;

		private string m_SelectedLanguage;

		private GUIContent[] m_EditorLanguageNames;

		private bool m_EnableEditorLocalization;

		private SystemLanguage[] m_stableLanguages = new SystemLanguage[]
		{
			SystemLanguage.English
		};

		private bool m_AllowAlphaNumericHierarchy = false;

		private string[] m_ScriptApps;

		private string[] m_ScriptAppsEditions;

		private string[] m_ImageApps;

		private string[] m_DiffTools;

		private string m_noDiffToolsMessage = string.Empty;

		private bool m_RefreshCustomPreferences;

		private string[] m_ScriptAppDisplayNames;

		private string[] m_ImageAppDisplayNames;

		private Vector2 m_KeyScrollPos;

		private Vector2 m_SectionScrollPos;

		private PrefKey m_SelectedKey = null;

		private const string kRecentScriptAppsKey = "RecentlyUsedScriptApp";

		private const string kRecentImageAppsKey = "RecentlyUsedImageApp";

		private const string m_ExpressNotSupportedMessage = "Unfortunately Visual Studio Express does not allow itself to be controlled by external applications. You can still use it by manually opening the Visual Studio project file, but Unity cannot automatically open files for you when you doubleclick them. \n(This does work with Visual Studio Pro)";

		private const int kRecentAppsCount = 10;

		private SortedDictionary<string, List<KeyValuePair<string, PrefColor>>> s_CachedColors = null;

		private static Vector2 s_ScrollPosition = Vector2.zero;

		private int m_SpriteAtlasCacheSize;

		private static int kMinSpriteCacheSizeInGigabytes = 1;

		private static int kMaxSpriteCacheSizeInGigabytes = 200;

		private bool m_ValidKeyChange = true;

		private string m_InvalidKeyMessage = string.Empty;

		private static int s_KeysControlHash = "KeysControlHash".GetHashCode();

		private int selectedSectionIndex
		{
			get
			{
				return this.m_SelectedSectionIndex;
			}
			set
			{
				if (this.m_SelectedSectionIndex != value)
				{
					this.m_ValidKeyChange = true;
				}
				this.m_SelectedSectionIndex = value;
				if (this.m_SelectedSectionIndex >= this.m_Sections.Count)
				{
					this.m_SelectedSectionIndex = 0;
				}
				else if (this.m_SelectedSectionIndex < 0)
				{
					this.m_SelectedSectionIndex = this.m_Sections.Count - 1;
				}
			}
		}

		private PreferencesWindow.Section selectedSection
		{
			get
			{
				return this.m_Sections[this.m_SelectedSectionIndex];
			}
		}

		private static void ShowPreferencesWindow()
		{
			EditorWindow window = EditorWindow.GetWindow<PreferencesWindow>(true, L10n.Tr("Unity Preferences"));
			window.minSize = new Vector2(540f, 400f);
			window.maxSize = new Vector2(window.minSize.x, window.maxSize.y);
			window.position = new Rect(new Vector2(100f, 100f), window.minSize);
			window.m_Parent.window.m_DontSaveToLayout = true;
		}

		private void OnEnable()
		{
			this.prefWinExtensions = ModuleManager.GetPreferenceWindowExtensions();
			this.ReadPreferences();
			this.m_Sections = new List<PreferencesWindow.Section>();
			this.m_Sections.Add(new PreferencesWindow.Section("General", new PreferencesWindow.OnGUIDelegate(this.ShowGeneral)));
			this.m_Sections.Add(new PreferencesWindow.Section("External Tools", new PreferencesWindow.OnGUIDelegate(this.ShowExternalApplications)));
			this.m_Sections.Add(new PreferencesWindow.Section("Colors", new PreferencesWindow.OnGUIDelegate(this.ShowColors)));
			this.m_Sections.Add(new PreferencesWindow.Section("Keys", new PreferencesWindow.OnGUIDelegate(this.ShowKeys)));
			this.m_Sections.Add(new PreferencesWindow.Section("GI Cache", new PreferencesWindow.OnGUIDelegate(this.ShowGICache)));
			this.m_Sections.Add(new PreferencesWindow.Section("2D", new PreferencesWindow.OnGUIDelegate(this.Show2D)));
			SystemLanguage[] editorLanguages = LocalizationDatabase.GetAvailableEditorLanguages();
			if (this.m_EditorLanguageNames == null || this.m_EditorLanguageNames.Length != editorLanguages.Length)
			{
				this.m_EditorLanguageNames = new GUIContent[editorLanguages.Length];
				int i;
				for (i = 0; i < editorLanguages.Length; i++)
				{
					if (ArrayUtility.FindIndex<SystemLanguage>(this.m_stableLanguages, (SystemLanguage v) => v == editorLanguages[i]) < 0)
					{
						this.m_EditorLanguageNames[i] = EditorGUIUtility.TextContent(string.Format("{0} (Experimental)", editorLanguages[i].ToString()));
					}
					else
					{
						this.m_EditorLanguageNames[i] = EditorGUIUtility.TextContent(editorLanguages[i].ToString());
					}
				}
				ArrayUtility.Insert<GUIContent>(ref this.m_EditorLanguageNames, 0, EditorGUIUtility.TextContent(""));
				GUIContent item = EditorGUIUtility.TextContent(string.Format("Default ( {0} )", LocalizationDatabase.GetDefaultEditorLanguage().ToString()));
				ArrayUtility.Insert<GUIContent>(ref this.m_EditorLanguageNames, 0, item);
			}
			if (editorLanguages.Length > 1)
			{
				this.m_Sections.Add(new PreferencesWindow.Section("Language", new PreferencesWindow.OnGUIDelegate(this.ShowLanguage)));
			}
			if (Unsupported.IsDeveloperMode() || UnityConnect.preferencesEnabled)
			{
				this.m_Sections.Add(new PreferencesWindow.Section("Unity Services", new PreferencesWindow.OnGUIDelegate(this.ShowUnityConnectPrefs)));
			}
			this.m_RefreshCustomPreferences = true;
		}

		private void AddCustomSections()
		{
			AttributeHelper.MethodInfoSorter methodsWithAttribute = AttributeHelper.GetMethodsWithAttribute<PreferenceItem>(BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			foreach (AttributeHelper.MethodWithAttribute current in methodsWithAttribute.methodsWithAttributes)
			{
				PreferencesWindow.OnGUIDelegate onGUIDelegate = Delegate.CreateDelegate(typeof(PreferencesWindow.OnGUIDelegate), current.info) as PreferencesWindow.OnGUIDelegate;
				if (onGUIDelegate != null)
				{
					string attributeName = (current.attribute as PreferenceItem).name;
					int num = this.m_Sections.FindIndex((PreferencesWindow.Section section) => section.content.text.Equals(attributeName));
					if (num >= 0)
					{
						PreferencesWindow.Section expr_95 = this.m_Sections[num];
						expr_95.guiFunc = (PreferencesWindow.OnGUIDelegate)Delegate.Combine(expr_95.guiFunc, onGUIDelegate);
					}
					else
					{
						this.m_Sections.Add(new PreferencesWindow.Section(attributeName, onGUIDelegate));
					}
				}
			}
		}

		private void OnGUI()
		{
			if (this.m_RefreshCustomPreferences)
			{
				this.AddCustomSections();
				this.m_RefreshCustomPreferences = false;
			}
			EditorGUIUtility.labelWidth = 200f;
			if (PreferencesWindow.constants == null)
			{
				PreferencesWindow.constants = new PreferencesWindow.Constants();
			}
			this.HandleKeys();
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			this.m_SectionScrollPos = GUILayout.BeginScrollView(this.m_SectionScrollPos, PreferencesWindow.constants.sectionScrollView, new GUILayoutOption[]
			{
				GUILayout.Width(140f)
			});
			GUILayout.Space(40f);
			for (int i = 0; i < this.m_Sections.Count; i++)
			{
				PreferencesWindow.Section section = this.m_Sections[i];
				Rect rect = GUILayoutUtility.GetRect(section.content, PreferencesWindow.constants.sectionElement, new GUILayoutOption[]
				{
					GUILayout.ExpandWidth(true)
				});
				if (section == this.selectedSection && Event.current.type == EventType.Repaint)
				{
					PreferencesWindow.constants.selected.Draw(rect, false, false, false, false);
				}
				EditorGUI.BeginChangeCheck();
				if (GUI.Toggle(rect, this.selectedSectionIndex == i, section.content, PreferencesWindow.constants.sectionElement))
				{
					this.selectedSectionIndex = i;
				}
				if (EditorGUI.EndChangeCheck())
				{
					GUIUtility.keyboardControl = 0;
				}
			}
			GUILayout.EndScrollView();
			GUILayout.Space(10f);
			GUILayout.BeginVertical(new GUILayoutOption[0]);
			GUILayout.Label(this.selectedSection.content, PreferencesWindow.constants.sectionHeader, new GUILayoutOption[0]);
			GUILayout.Space(10f);
			PreferencesWindow.s_ScrollPosition = EditorGUILayout.BeginScrollView(PreferencesWindow.s_ScrollPosition, new GUILayoutOption[0]);
			this.selectedSection.guiFunc();
			EditorGUILayout.EndScrollView();
			GUILayout.EndVertical();
			GUILayout.EndHorizontal();
		}

		private void HandleKeys()
		{
			if (Event.current.type == EventType.KeyDown && GUIUtility.keyboardControl == 0)
			{
				KeyCode keyCode = Event.current.keyCode;
				if (keyCode != KeyCode.UpArrow)
				{
					if (keyCode == KeyCode.DownArrow)
					{
						this.selectedSectionIndex++;
						Event.current.Use();
					}
				}
				else
				{
					this.selectedSectionIndex--;
					Event.current.Use();
				}
			}
		}

		private void ShowExternalApplications()
		{
			this.FilePopup(PreferencesWindow.Styles.externalScriptEditor, this.m_ScriptEditorPath, ref this.m_ScriptAppDisplayNames, ref this.m_ScriptApps, this.m_ScriptEditorPath, "internal", new Action(this.OnScriptEditorChanged));
			ScriptEditorUtility.ScriptEditor selectedScriptEditor = this.GetSelectedScriptEditor();
			if (selectedScriptEditor == ScriptEditorUtility.ScriptEditor.Other)
			{
				string scriptEditorArgs = this.m_ScriptEditorArgs;
				this.m_ScriptEditorArgs = EditorGUILayout.TextField("External Script Editor Args", this.m_ScriptEditorArgs, new GUILayoutOption[0]);
				if (scriptEditorArgs != this.m_ScriptEditorArgs)
				{
					this.OnScriptEditorArgsChanged();
				}
			}
			this.DoUnityProjCheckbox();
			bool allowAttachedDebuggingOfEditor = this.m_AllowAttachedDebuggingOfEditor;
			this.m_AllowAttachedDebuggingOfEditor = EditorGUILayout.Toggle(PreferencesWindow.Styles.editorAttaching, this.m_AllowAttachedDebuggingOfEditor, new GUILayoutOption[0]);
			if (allowAttachedDebuggingOfEditor != this.m_AllowAttachedDebuggingOfEditor)
			{
				this.m_AllowAttachedDebuggingOfEditorStateChangedThisSession = true;
			}
			if (this.m_AllowAttachedDebuggingOfEditorStateChangedThisSession)
			{
				GUILayout.Label(PreferencesWindow.Styles.changingThisSettingRequiresRestart, EditorStyles.helpBox, new GUILayoutOption[0]);
			}
			if (this.GetSelectedScriptEditor() == ScriptEditorUtility.ScriptEditor.VisualStudioExpress)
			{
				GUILayout.BeginHorizontal(EditorStyles.helpBox, new GUILayoutOption[0]);
				GUILayout.Label("", PreferencesWindow.constants.warningIcon, new GUILayoutOption[0]);
				GUILayout.Label("Unfortunately Visual Studio Express does not allow itself to be controlled by external applications. You can still use it by manually opening the Visual Studio project file, but Unity cannot automatically open files for you when you doubleclick them. \n(This does work with Visual Studio Pro)", PreferencesWindow.constants.errorLabel, new GUILayoutOption[0]);
				GUILayout.EndHorizontal();
			}
			GUILayout.Space(10f);
			this.FilePopup(PreferencesWindow.Styles.imageApplication, this.m_ImageAppPath, ref this.m_ImageAppDisplayNames, ref this.m_ImageApps, this.m_ImageAppPath, "internal", null);
			GUILayout.Space(10f);
			using (new EditorGUI.DisabledScope(!InternalEditorUtility.HasTeamLicense()))
			{
				this.m_DiffToolIndex = EditorGUILayout.Popup(PreferencesWindow.Styles.revisionControlDiffMerge, this.m_DiffToolIndex, this.m_DiffTools, new GUILayoutOption[0]);
			}
			if (this.m_noDiffToolsMessage != string.Empty)
			{
				GUILayout.BeginHorizontal(EditorStyles.helpBox, new GUILayoutOption[0]);
				GUILayout.Label("", PreferencesWindow.constants.warningIcon, new GUILayoutOption[0]);
				GUILayout.Label(this.m_noDiffToolsMessage, PreferencesWindow.constants.errorLabel, new GUILayoutOption[0]);
				GUILayout.EndHorizontal();
			}
			GUILayout.Space(10f);
			foreach (IPreferenceWindowExtension current in this.prefWinExtensions)
			{
				if (current.HasExternalApplications())
				{
					GUILayout.Space(10f);
					current.ShowExternalApplications();
				}
			}
			this.ApplyChangesToPrefs(false);
		}

		private void DoUnityProjCheckbox()
		{
			bool flag = false;
			bool flag2 = false;
			ScriptEditorUtility.ScriptEditor selectedScriptEditor = this.GetSelectedScriptEditor();
			if (selectedScriptEditor == ScriptEditorUtility.ScriptEditor.MonoDevelop)
			{
				flag = true;
				flag2 = this.m_ExternalEditorSupportsUnityProj;
			}
			using (new EditorGUI.DisabledScope(!flag))
			{
				flag2 = EditorGUILayout.Toggle(PreferencesWindow.Styles.addUnityProjeToSln, flag2, new GUILayoutOption[0]);
			}
			if (flag)
			{
				this.m_ExternalEditorSupportsUnityProj = flag2;
			}
		}

		private ScriptEditorUtility.ScriptEditor GetSelectedScriptEditor()
		{
			return ScriptEditorUtility.GetScriptEditorFromPath(this.m_ScriptEditorPath.str);
		}

		private void OnScriptEditorChanged()
		{
			ScriptEditorUtility.SetExternalScriptEditor(this.m_ScriptEditorPath);
			this.m_ScriptEditorArgs = ScriptEditorUtility.GetExternalScriptEditorArgs();
			UnityVSSupport.ScriptEditorChanged(this.m_ScriptEditorPath.str);
		}

		private void OnScriptEditorArgsChanged()
		{
			ScriptEditorUtility.SetExternalScriptEditorArgs(this.m_ScriptEditorArgs);
		}

		private void ShowUnityConnectPrefs()
		{
			UnityConnectPrefs.ShowPanelPrefUI();
			this.ApplyChangesToPrefs(false);
		}

		private void ShowGeneral()
		{
			bool flag = Collab.instance.IsCollabEnabledForCurrentProject();
			using (new EditorGUI.DisabledScope(flag))
			{
				if (flag)
				{
					EditorGUILayout.Toggle(PreferencesWindow.Styles.autoRefresh, true, new GUILayoutOption[0]);
					EditorGUILayout.HelpBox(PreferencesWindow.Styles.autoRefreshHelpBox, true);
				}
				else
				{
					this.m_AutoRefresh = EditorGUILayout.Toggle(PreferencesWindow.Styles.autoRefresh, this.m_AutoRefresh, new GUILayoutOption[0]);
				}
			}
			this.m_ReopenLastUsedProjectOnStartup = EditorGUILayout.Toggle(PreferencesWindow.Styles.loadPreviousProjectOnStartup, this.m_ReopenLastUsedProjectOnStartup, new GUILayoutOption[0]);
			bool compressAssetsOnImport = this.m_CompressAssetsOnImport;
			this.m_CompressAssetsOnImport = EditorGUILayout.Toggle(PreferencesWindow.Styles.compressAssetsOnImport, compressAssetsOnImport, new GUILayoutOption[0]);
			if (GUI.changed && this.m_CompressAssetsOnImport != compressAssetsOnImport)
			{
				Unsupported.SetApplicationSettingCompressAssetsOnImport(this.m_CompressAssetsOnImport);
			}
			if (Application.platform == RuntimePlatform.OSXEditor)
			{
				this.m_UseOSColorPicker = EditorGUILayout.Toggle(PreferencesWindow.Styles.osxColorPicker, this.m_UseOSColorPicker, new GUILayoutOption[0]);
			}
			bool flag2 = Application.HasProLicense();
			using (new EditorGUI.DisabledScope(!flag2))
			{
				this.m_EnableEditorAnalytics = !EditorGUILayout.Toggle(PreferencesWindow.Styles.disableEditorAnalytics, !this.m_EnableEditorAnalytics, new GUILayoutOption[0]);
				if (!flag2 && !this.m_EnableEditorAnalytics)
				{
					this.m_EnableEditorAnalytics = true;
				}
			}
			bool flag3 = false;
			EditorGUI.BeginChangeCheck();
			this.m_ShowAssetStoreSearchHits = EditorGUILayout.Toggle(PreferencesWindow.Styles.showAssetStoreSearchHits, this.m_ShowAssetStoreSearchHits, new GUILayoutOption[0]);
			if (EditorGUI.EndChangeCheck())
			{
				flag3 = true;
			}
			this.m_VerifySavingAssets = EditorGUILayout.Toggle(PreferencesWindow.Styles.verifySavingAssets, this.m_VerifySavingAssets, new GUILayoutOption[0]);
			if (Unsupported.IsSourceBuild() || this.m_DeveloperMode)
			{
				EditorGUI.BeginChangeCheck();
				this.m_DeveloperMode = EditorGUILayout.Toggle("Developer Mode", this.m_DeveloperMode, new GUILayoutOption[0]);
				if (EditorGUI.EndChangeCheck())
				{
					this.m_DeveloperModeDirty = true;
				}
			}
			using (new EditorGUI.DisabledScope(!flag2))
			{
				int num = EditorGUILayout.Popup(PreferencesWindow.Styles.editorSkin, EditorGUIUtility.isProSkin ? 1 : 0, PreferencesWindow.Styles.editorSkinOptions, new GUILayoutOption[0]);
				if ((EditorGUIUtility.isProSkin ? 1 : 0) != num)
				{
					InternalEditorUtility.SwitchSkinAndRepaintAllViews();
				}
			}
			bool allowAlphaNumericHierarchy = this.m_AllowAlphaNumericHierarchy;
			this.m_AllowAlphaNumericHierarchy = EditorGUILayout.Toggle(PreferencesWindow.Styles.enableAlphaNumericSorting, this.m_AllowAlphaNumericHierarchy, new GUILayoutOption[0]);
			if (InternalEditorUtility.IsGpuDeviceSelectionSupported())
			{
				if (this.m_CachedGpuDevices == null)
				{
					string[] gpuDevices = InternalEditorUtility.GetGpuDevices();
					this.m_CachedGpuDevices = new string[gpuDevices.Length + 1];
					this.m_CachedGpuDevices[0] = "Automatic";
					Array.Copy(gpuDevices, 0, this.m_CachedGpuDevices, 1, gpuDevices.Length);
				}
				int num2 = Array.FindIndex<string>(this.m_CachedGpuDevices, (string gpuDevice) => this.m_GpuDevice == gpuDevice);
				if (num2 == -1)
				{
					num2 = 0;
				}
				int num3 = EditorGUILayout.Popup("Device To Use", num2, this.m_CachedGpuDevices, new GUILayoutOption[0]);
				if (num2 != num3)
				{
					this.m_GpuDevice = this.m_CachedGpuDevices[num3];
					InternalEditorUtility.SetGpuDeviceAndRecreateGraphics(num3 - 1, this.m_GpuDevice);
				}
			}
			this.ApplyChangesToPrefs(false);
			if (allowAlphaNumericHierarchy != this.m_AllowAlphaNumericHierarchy)
			{
				EditorApplication.DirtyHierarchyWindowSorting();
			}
			if (flag3)
			{
				ProjectBrowser.ShowAssetStoreHitsWhileSearchingLocalAssetsChanged();
			}
		}

		public void ApplyChangesToPrefs(bool force = false)
		{
			if (GUI.changed || force)
			{
				this.WritePreferences();
				this.ReadPreferences();
				base.Repaint();
			}
		}

		private void RevertKeys()
		{
			foreach (KeyValuePair<string, PrefKey> current in Settings.Prefs<PrefKey>())
			{
				current.Value.ResetToDefault();
				EditorPrefs.SetString(current.Value.Name, current.Value.ToUniqueString());
			}
		}

		private SortedDictionary<string, List<KeyValuePair<string, T>>> OrderPrefs<T>(IEnumerable<KeyValuePair<string, T>> input) where T : IPrefType
		{
			SortedDictionary<string, List<KeyValuePair<string, T>>> sortedDictionary = new SortedDictionary<string, List<KeyValuePair<string, T>>>();
			foreach (KeyValuePair<string, T> current in input)
			{
				int num = current.Key.IndexOf('/');
				string key;
				string key2;
				if (num == -1)
				{
					key = "General";
					key2 = current.Key;
				}
				else
				{
					key = current.Key.Substring(0, num);
					key2 = current.Key.Substring(num + 1);
				}
				if (!sortedDictionary.ContainsKey(key))
				{
					sortedDictionary.Add(key, new List<KeyValuePair<string, T>>(new List<KeyValuePair<string, T>>
					{
						new KeyValuePair<string, T>(key2, current.Value)
					}));
				}
				else
				{
					sortedDictionary[key].Add(new KeyValuePair<string, T>(key2, current.Value));
				}
			}
			return sortedDictionary;
		}

		private void ShowKeys()
		{
			int controlID = GUIUtility.GetControlID(PreferencesWindow.s_KeysControlHash, FocusType.Keyboard);
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.BeginVertical(new GUILayoutOption[]
			{
				GUILayout.Width(185f)
			});
			GUILayout.Label("Actions", PreferencesWindow.constants.settingsBoxTitle, new GUILayoutOption[]
			{
				GUILayout.ExpandWidth(true)
			});
			this.m_KeyScrollPos = GUILayout.BeginScrollView(this.m_KeyScrollPos, PreferencesWindow.constants.settingsBox);
			PrefKey prefKey = null;
			PrefKey prefKey2 = null;
			bool flag = false;
			foreach (KeyValuePair<string, PrefKey> current in Settings.Prefs<PrefKey>())
			{
				if (!flag)
				{
					if (current.Value == this.m_SelectedKey)
					{
						flag = true;
					}
					else
					{
						prefKey = current.Value;
					}
				}
				else if (prefKey2 == null)
				{
					prefKey2 = current.Value;
				}
				EditorGUI.BeginChangeCheck();
				if (GUILayout.Toggle(current.Value == this.m_SelectedKey, current.Key, PreferencesWindow.constants.keysElement, new GUILayoutOption[0]))
				{
					if (this.m_SelectedKey != current.Value)
					{
						this.m_ValidKeyChange = true;
					}
					this.m_SelectedKey = current.Value;
				}
				if (EditorGUI.EndChangeCheck())
				{
					GUIUtility.keyboardControl = controlID;
				}
			}
			GUILayout.EndScrollView();
			GUILayout.EndVertical();
			GUILayout.Space(10f);
			GUILayout.BeginVertical(new GUILayoutOption[0]);
			if (this.m_SelectedKey != null)
			{
				Event @event = this.m_SelectedKey.KeyboardEvent;
				GUI.changed = false;
				string[] array = this.m_SelectedKey.Name.Split(new char[]
				{
					'/'
				});
				GUILayout.Label(array[0], "boldLabel", new GUILayoutOption[0]);
				GUILayout.Label(array[1], "boldLabel", new GUILayoutOption[0]);
				GUILayout.BeginHorizontal(new GUILayoutOption[0]);
				GUILayout.Label("Key:", new GUILayoutOption[0]);
				@event = EditorGUILayout.KeyEventField(@event, new GUILayoutOption[0]);
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(new GUILayoutOption[0]);
				GUILayout.Label("Modifiers:", new GUILayoutOption[0]);
				GUILayout.BeginVertical(new GUILayoutOption[0]);
				if (Application.platform == RuntimePlatform.OSXEditor)
				{
					@event.command = GUILayout.Toggle(@event.command, "Command", new GUILayoutOption[0]);
				}
				@event.control = GUILayout.Toggle(@event.control, "Control", new GUILayoutOption[0]);
				@event.shift = GUILayout.Toggle(@event.shift, "Shift", new GUILayoutOption[0]);
				@event.alt = GUILayout.Toggle(@event.alt, "Alt", new GUILayoutOption[0]);
				if (GUI.changed)
				{
					this.m_ValidKeyChange = true;
					string b = this.m_SelectedKey.Name.Split(new char[]
					{
						'/'
					})[0];
					foreach (KeyValuePair<string, PrefKey> current2 in Settings.Prefs<PrefKey>())
					{
						string a = current2.Key.Split(new char[]
						{
							'/'
						})[0];
						if (current2.Value.KeyboardEvent.Equals(@event) && a == b && current2.Key != this.m_SelectedKey.Name)
						{
							this.m_ValidKeyChange = false;
							StringBuilder stringBuilder = new StringBuilder();
							if (Application.platform == RuntimePlatform.OSXEditor && @event.command)
							{
								stringBuilder.Append("Command+");
							}
							if (@event.control)
							{
								stringBuilder.Append("Ctrl+");
							}
							if (@event.shift)
							{
								stringBuilder.Append("Shift+");
							}
							if (@event.alt)
							{
								stringBuilder.Append("Alt+");
							}
							stringBuilder.Append(@event.keyCode);
							this.m_InvalidKeyMessage = string.Format("Key {0} can't be used for action \"{1}\" because it's already used for action \"{2}\"", stringBuilder, this.m_SelectedKey.Name, current2.Key);
							break;
						}
					}
					if (this.m_ValidKeyChange)
					{
						this.m_SelectedKey.KeyboardEvent = @event;
						Settings.Set<PrefKey>(this.m_SelectedKey.Name, this.m_SelectedKey);
					}
				}
				else if (GUIUtility.keyboardControl == controlID && Event.current.type == EventType.KeyDown)
				{
					KeyCode keyCode = Event.current.keyCode;
					if (keyCode != KeyCode.UpArrow)
					{
						if (keyCode == KeyCode.DownArrow)
						{
							if (prefKey2 != null && prefKey2 != this.m_SelectedKey)
							{
								this.m_SelectedKey = prefKey2;
								this.m_ValidKeyChange = true;
							}
							Event.current.Use();
						}
					}
					else
					{
						if (prefKey != null && prefKey != this.m_SelectedKey)
						{
							this.m_SelectedKey = prefKey;
							this.m_ValidKeyChange = true;
						}
						Event.current.Use();
					}
				}
				GUILayout.EndVertical();
				GUILayout.EndHorizontal();
				if (!this.m_ValidKeyChange)
				{
					GUILayout.BeginHorizontal(new GUILayoutOption[0]);
					GUILayout.Label("", PreferencesWindow.constants.warningIcon, new GUILayoutOption[0]);
					GUILayout.Label(this.m_InvalidKeyMessage, PreferencesWindow.constants.errorLabel, new GUILayoutOption[0]);
					GUILayout.EndHorizontal();
				}
			}
			GUILayout.EndVertical();
			GUILayout.Space(10f);
			GUILayout.EndHorizontal();
			GUILayout.Space(5f);
			if (GUILayout.Button(PreferencesWindow.Styles.userDefaults, new GUILayoutOption[]
			{
				GUILayout.Width(120f)
			}))
			{
				this.m_ValidKeyChange = true;
				this.RevertKeys();
			}
		}

		private void RevertColors()
		{
			foreach (KeyValuePair<string, PrefColor> current in Settings.Prefs<PrefColor>())
			{
				current.Value.ResetToDefault();
				EditorPrefs.SetString(current.Value.Name, current.Value.ToUniqueString());
			}
		}

		private void ShowColors()
		{
			if (this.s_CachedColors == null)
			{
				this.s_CachedColors = this.OrderPrefs<PrefColor>(Settings.Prefs<PrefColor>());
			}
			bool flag = false;
			PrefColor prefColor = null;
			foreach (KeyValuePair<string, List<KeyValuePair<string, PrefColor>>> current in this.s_CachedColors)
			{
				GUILayout.Label(current.Key, EditorStyles.boldLabel, new GUILayoutOption[0]);
				foreach (KeyValuePair<string, PrefColor> current2 in current.Value)
				{
					EditorGUI.BeginChangeCheck();
					Color color = EditorGUILayout.ColorField(current2.Key, current2.Value.Color, new GUILayoutOption[0]);
					if (EditorGUI.EndChangeCheck())
					{
						prefColor = current2.Value;
						prefColor.Color = color;
						flag = true;
					}
				}
				if (prefColor != null)
				{
					Settings.Set<PrefColor>(prefColor.Name, prefColor);
				}
			}
			GUILayout.Space(5f);
			if (GUILayout.Button(PreferencesWindow.Styles.userDefaults, new GUILayoutOption[]
			{
				GUILayout.Width(120f)
			}))
			{
				this.RevertColors();
				flag = true;
			}
			if (flag)
			{
				EditorApplication.RequestRepaintAllViews();
			}
		}

		private void Show2D()
		{
			EditorGUI.BeginChangeCheck();
			this.m_SpriteAtlasCacheSize = EditorGUILayout.IntSlider(PreferencesWindow.Styles.spriteMaxCacheSize, this.m_SpriteAtlasCacheSize, PreferencesWindow.kMinSpriteCacheSizeInGigabytes, PreferencesWindow.kMaxSpriteCacheSizeInGigabytes, new GUILayoutOption[0]);
			if (EditorGUI.EndChangeCheck())
			{
				this.WritePreferences();
			}
		}

		private void ShowGICache()
		{
			this.m_GICacheSettings.m_MaximumSize = EditorGUILayout.IntSlider(PreferencesWindow.Styles.maxCacheSize, this.m_GICacheSettings.m_MaximumSize, 5, 200, new GUILayoutOption[0]);
			this.WritePreferences();
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			if (Lightmapping.isRunning)
			{
				GUIContent gUIContent = EditorGUIUtility.TextContent(PreferencesWindow.Styles.cantChangeCacheSettings.text);
				EditorGUILayout.HelpBox(gUIContent.text, MessageType.Warning, true);
			}
			GUILayout.EndHorizontal();
			using (new EditorGUI.DisabledScope(Lightmapping.isRunning))
			{
				this.m_GICacheSettings.m_EnableCustomPath = EditorGUILayout.Toggle(PreferencesWindow.Styles.customCacheLocation, this.m_GICacheSettings.m_EnableCustomPath, new GUILayoutOption[0]);
				if (this.m_GICacheSettings.m_EnableCustomPath)
				{
					GUIStyle miniButton = EditorStyles.miniButton;
					GUILayout.BeginHorizontal(new GUILayoutOption[0]);
					EditorGUILayout.PrefixLabel(PreferencesWindow.Styles.cacheFolderLocation, miniButton);
					Rect rect = GUILayoutUtility.GetRect(GUIContent.none, miniButton);
					GUIContent content = (!string.IsNullOrEmpty(this.m_GICacheSettings.m_CachePath)) ? new GUIContent(this.m_GICacheSettings.m_CachePath) : PreferencesWindow.Styles.browse;
					if (EditorGUI.DropdownButton(rect, content, FocusType.Passive, miniButton))
					{
						string cachePath = this.m_GICacheSettings.m_CachePath;
						string text = EditorUtility.OpenFolderPanel(PreferencesWindow.Styles.browseGICacheLocation.text, cachePath, "");
						if (!string.IsNullOrEmpty(text))
						{
							this.m_GICacheSettings.m_CachePath = text;
							this.WritePreferences();
						}
					}
					GUILayout.EndHorizontal();
				}
				else
				{
					this.m_GICacheSettings.m_CachePath = "";
				}
				this.m_GICacheSettings.m_CompressionLevel = ((!EditorGUILayout.Toggle(PreferencesWindow.Styles.cacheCompression, this.m_GICacheSettings.m_CompressionLevel == 1, new GUILayoutOption[0])) ? 0 : 1);
				if (GUILayout.Button(PreferencesWindow.Styles.cleanCache, new GUILayoutOption[]
				{
					GUILayout.Width(120f)
				}))
				{
					EditorUtility.DisplayProgressBar(PreferencesWindow.Styles.cleanCache.text, PreferencesWindow.Styles.pleaseWait.text, 0f);
					Lightmapping.Clear();
					EditorUtility.DisplayProgressBar(PreferencesWindow.Styles.cleanCache.text, PreferencesWindow.Styles.pleaseWait.text, 0.5f);
					Lightmapping.ClearDiskCache();
					EditorUtility.ClearProgressBar();
				}
				if (Lightmapping.diskCacheSize >= 0L)
				{
					GUILayout.Label(PreferencesWindow.Styles.cacheSizeIs.text + " " + EditorUtility.FormatBytes(Lightmapping.diskCacheSize), new GUILayoutOption[0]);
				}
				else
				{
					GUILayout.Label(PreferencesWindow.Styles.cacheSizeIs.text + " is being calculated...", new GUILayoutOption[0]);
				}
				GUILayout.Label(PreferencesWindow.Styles.cacheFolderLocation.text + ":", new GUILayoutOption[0]);
				GUILayout.Label(Lightmapping.diskCachePath, PreferencesWindow.constants.cacheFolderLocation, new GUILayoutOption[0]);
			}
		}

		private void ShowLanguage()
		{
			bool flag = EditorGUILayout.Toggle(PreferencesWindow.Styles.editorLanguageExperimental, this.m_EnableEditorLocalization, new GUILayoutOption[0]);
			if (flag != this.m_EnableEditorLocalization)
			{
				this.m_EnableEditorLocalization = flag;
				this.m_SelectedLanguage = LocalizationDatabase.GetDefaultEditorLanguage().ToString();
			}
			EditorGUI.BeginDisabledGroup(!this.m_EnableEditorLocalization);
			SystemLanguage[] availableEditorLanguages = LocalizationDatabase.GetAvailableEditorLanguages();
			int selectedIndex = 0;
			for (int i = 0; i < availableEditorLanguages.Length; i++)
			{
				if (availableEditorLanguages[i].ToString().Equals(this.m_SelectedLanguage))
				{
					selectedIndex = 2 + i;
					break;
				}
			}
			int num = EditorGUILayout.Popup(PreferencesWindow.Styles.editorLanguage, selectedIndex, this.m_EditorLanguageNames, new GUILayoutOption[0]);
			this.m_SelectedLanguage = ((num != 0) ? availableEditorLanguages[num - 2].ToString() : LocalizationDatabase.GetDefaultEditorLanguage().ToString());
			EditorGUI.EndDisabledGroup();
			if (!this.m_SelectedLanguage.Equals(LocalizationDatabase.currentEditorLanguage.ToString()))
			{
				SystemLanguage newLanguage = (SystemLanguage)Enum.Parse(typeof(SystemLanguage), this.m_SelectedLanguage);
				EditorGUIUtility.NotifyLanguageChanged(newLanguage);
				InternalEditorUtility.RequestScriptReload();
			}
			this.ApplyChangesToPrefs(false);
		}

		private void WriteRecentAppsList(string[] paths, string path, string prefsKey)
		{
			int num = 0;
			if (path.Length != 0)
			{
				EditorPrefs.SetString(prefsKey + num, path);
				num++;
			}
			for (int i = 0; i < paths.Length; i++)
			{
				if (num >= 10)
				{
					break;
				}
				if (paths[i].Length != 0)
				{
					if (!(paths[i] == path))
					{
						EditorPrefs.SetString(prefsKey + num, paths[i]);
						num++;
					}
				}
			}
		}

		private void WritePreferences()
		{
			ScriptEditorUtility.SetExternalScriptEditor(this.m_ScriptEditorPath);
			ScriptEditorUtility.SetExternalScriptEditorArgs(this.m_ScriptEditorArgs);
			EditorPrefs.SetBool("kExternalEditorSupportsUnityProj", this.m_ExternalEditorSupportsUnityProj);
			EditorPrefs.SetString("kImagesDefaultApp", this.m_ImageAppPath);
			EditorPrefs.SetString("kDiffsDefaultApp", (this.m_DiffTools.Length != 0) ? this.m_DiffTools[this.m_DiffToolIndex] : "");
			this.WriteRecentAppsList(this.m_ScriptApps, this.m_ScriptEditorPath, "RecentlyUsedScriptApp");
			this.WriteRecentAppsList(this.m_ImageApps, this.m_ImageAppPath, "RecentlyUsedImageApp");
			EditorPrefs.SetBool("kAutoRefresh", this.m_AutoRefresh);
			if (Unsupported.IsDeveloperMode() || UnityConnect.preferencesEnabled)
			{
				UnityConnectPrefs.StorePanelPrefs();
			}
			EditorPrefs.SetBool("ReopenLastUsedProjectOnStartup", this.m_ReopenLastUsedProjectOnStartup);
			EditorPrefs.SetBool("UseOSColorPicker", this.m_UseOSColorPicker);
			EditorPrefs.SetBool("EnableEditorAnalytics", this.m_EnableEditorAnalytics);
			EditorPrefs.SetBool("ShowAssetStoreSearchHits", this.m_ShowAssetStoreSearchHits);
			EditorPrefs.SetBool("VerifySavingAssets", this.m_VerifySavingAssets);
			if (this.m_DeveloperModeDirty)
			{
				EditorPrefs.SetBool("DeveloperMode", this.m_DeveloperMode);
				InternalEditorUtility.RepaintAllViews();
			}
			EditorPrefs.SetBool("AllowAttachedDebuggingOfEditor", this.m_AllowAttachedDebuggingOfEditor);
			EditorPrefs.SetBool("Editor.kEnableEditorLocalization", this.m_EnableEditorLocalization);
			EditorPrefs.SetString("Editor.kEditorLocale", this.m_SelectedLanguage.ToString());
			EditorPrefs.SetBool("AllowAlphaNumericHierarchy", this.m_AllowAlphaNumericHierarchy);
			EditorPrefs.SetString("GpuDevice", this.m_GpuDevice);
			EditorPrefs.SetBool("GICacheEnableCustomPath", this.m_GICacheSettings.m_EnableCustomPath);
			EditorPrefs.SetInt("GICacheMaximumSizeGB", this.m_GICacheSettings.m_MaximumSize);
			EditorPrefs.SetString("GICacheFolder", this.m_GICacheSettings.m_CachePath);
			EditorPrefs.SetInt("GICacheCompressionLevel", this.m_GICacheSettings.m_CompressionLevel);
			EditorPrefs.SetInt("SpritePackerCacheMaximumSizeGB", this.m_SpriteAtlasCacheSize);
			foreach (IPreferenceWindowExtension current in this.prefWinExtensions)
			{
				current.WritePreferences();
			}
			Lightmapping.UpdateCachePath();
		}

		private static void SetupDefaultPreferences()
		{
		}

		private static string GetProgramFilesFolder()
		{
			string environmentVariable = Environment.GetEnvironmentVariable("ProgramFiles(x86)");
			string result;
			if (environmentVariable != null)
			{
				result = environmentVariable;
			}
			else
			{
				result = Environment.GetEnvironmentVariable("ProgramFiles");
			}
			return result;
		}

		private void ReadPreferences()
		{
			this.m_ScriptEditorPath.str = ScriptEditorUtility.GetExternalScriptEditor();
			this.m_ScriptEditorArgs = ScriptEditorUtility.GetExternalScriptEditorArgs();
			this.m_ExternalEditorSupportsUnityProj = EditorPrefs.GetBool("kExternalEditorSupportsUnityProj", false);
			this.m_ImageAppPath.str = EditorPrefs.GetString("kImagesDefaultApp");
			this.m_ScriptApps = this.BuildAppPathList(this.m_ScriptEditorPath, "RecentlyUsedScriptApp", "internal");
			this.m_ScriptAppsEditions = new string[this.m_ScriptApps.Length];
			if (Application.platform == RuntimePlatform.WindowsEditor)
			{
				foreach (VisualStudioPath[] current in SyncVS.InstalledVisualStudios.Values)
				{
					VisualStudioPath[] array = current;
					for (int i = 0; i < array.Length; i++)
					{
						VisualStudioPath visualStudioPath = array[i];
						int num = Array.IndexOf<string>(this.m_ScriptApps, visualStudioPath.Path);
						if (num == -1)
						{
							ArrayUtility.Add<string>(ref this.m_ScriptApps, visualStudioPath.Path);
							ArrayUtility.Add<string>(ref this.m_ScriptAppsEditions, visualStudioPath.Edition);
						}
						else
						{
							this.m_ScriptAppsEditions[num] = visualStudioPath.Edition;
						}
					}
				}
			}
			string[] foundScriptEditorPaths = ScriptEditorUtility.GetFoundScriptEditorPaths(Application.platform);
			string[] array2 = foundScriptEditorPaths;
			for (int j = 0; j < array2.Length; j++)
			{
				string item = array2[j];
				ArrayUtility.Add<string>(ref this.m_ScriptApps, item);
				ArrayUtility.Add<string>(ref this.m_ScriptAppsEditions, null);
			}
			this.m_ImageApps = this.BuildAppPathList(this.m_ImageAppPath, "RecentlyUsedImageApp", "");
			this.m_ScriptAppDisplayNames = this.BuildFriendlyAppNameList(this.m_ScriptApps, this.m_ScriptAppsEditions, "Open by file extension");
			this.m_ImageAppDisplayNames = this.BuildFriendlyAppNameList(this.m_ImageApps, null, L10n.Tr("Open by file extension"));
			this.m_DiffTools = InternalEditorUtility.GetAvailableDiffTools();
			if ((this.m_DiffTools == null || this.m_DiffTools.Length == 0) && InternalEditorUtility.HasTeamLicense())
			{
				this.m_noDiffToolsMessage = InternalEditorUtility.GetNoDiffToolsDetectedMessage();
			}
			string @string = EditorPrefs.GetString("kDiffsDefaultApp");
			this.m_DiffToolIndex = ArrayUtility.IndexOf<string>(this.m_DiffTools, @string);
			if (this.m_DiffToolIndex == -1)
			{
				this.m_DiffToolIndex = 0;
			}
			this.m_AutoRefresh = EditorPrefs.GetBool("kAutoRefresh");
			this.m_ReopenLastUsedProjectOnStartup = EditorPrefs.GetBool("ReopenLastUsedProjectOnStartup");
			this.m_UseOSColorPicker = EditorPrefs.GetBool("UseOSColorPicker");
			this.m_EnableEditorAnalytics = EditorPrefs.GetBool("EnableEditorAnalytics", true);
			this.m_ShowAssetStoreSearchHits = EditorPrefs.GetBool("ShowAssetStoreSearchHits", true);
			this.m_VerifySavingAssets = EditorPrefs.GetBool("VerifySavingAssets", false);
			this.m_DeveloperMode = Unsupported.IsDeveloperMode();
			this.m_GICacheSettings.m_EnableCustomPath = EditorPrefs.GetBool("GICacheEnableCustomPath");
			this.m_GICacheSettings.m_CachePath = EditorPrefs.GetString("GICacheFolder");
			this.m_GICacheSettings.m_MaximumSize = EditorPrefs.GetInt("GICacheMaximumSizeGB", 10);
			this.m_GICacheSettings.m_CompressionLevel = EditorPrefs.GetInt("GICacheCompressionLevel");
			this.m_SpriteAtlasCacheSize = EditorPrefs.GetInt("SpritePackerCacheMaximumSizeGB");
			this.m_AllowAttachedDebuggingOfEditor = EditorPrefs.GetBool("AllowAttachedDebuggingOfEditor", true);
			this.m_EnableEditorLocalization = EditorPrefs.GetBool("Editor.kEnableEditorLocalization", true);
			this.m_SelectedLanguage = EditorPrefs.GetString("Editor.kEditorLocale", LocalizationDatabase.GetDefaultEditorLanguage().ToString());
			this.m_AllowAlphaNumericHierarchy = EditorPrefs.GetBool("AllowAlphaNumericHierarchy", false);
			this.m_CompressAssetsOnImport = Unsupported.GetApplicationSettingCompressAssetsOnImport();
			this.m_GpuDevice = EditorPrefs.GetString("GpuDevice");
			foreach (IPreferenceWindowExtension current2 in this.prefWinExtensions)
			{
				current2.ReadPreferences();
			}
		}

		private string StripMicrosoftFromVisualStudioName(string arg)
		{
			string result;
			if (!arg.Contains("Visual Studio"))
			{
				result = arg;
			}
			else if (!arg.StartsWith("Microsoft"))
			{
				result = arg;
			}
			else
			{
				result = arg.Substring("Microsoft ".Length);
			}
			return result;
		}

		private void AppsListClick(object userData, string[] options, int selected)
		{
			PreferencesWindow.AppsListUserData appsListUserData = (PreferencesWindow.AppsListUserData)userData;
			if (options[selected] == "Browse...")
			{
				string text = EditorUtility.OpenFilePanel("Browse for application", "", InternalEditorUtility.GetApplicationExtensionForRuntimePlatform(Application.platform));
				if (text.Length != 0)
				{
					appsListUserData.str.str = text;
					if (appsListUserData.onChanged != null)
					{
						appsListUserData.onChanged();
					}
				}
			}
			else
			{
				appsListUserData.str.str = appsListUserData.paths[selected];
				if (appsListUserData.onChanged != null)
				{
					appsListUserData.onChanged();
				}
			}
			this.WritePreferences();
			this.ReadPreferences();
		}

		private void FilePopup(GUIContent label, string selectedString, ref string[] names, ref string[] paths, PreferencesWindow.RefString outString, string defaultString, Action onChanged)
		{
			GUIStyle popup = EditorStyles.popup;
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			EditorGUILayout.PrefixLabel(label, popup);
			int[] array = new int[0];
			if (paths.Contains(selectedString))
			{
				array = new int[]
				{
					Array.IndexOf<string>(paths, selectedString)
				};
			}
			GUIContent content = new GUIContent((array.Length != 0) ? names[array[0]] : defaultString);
			Rect rect = GUILayoutUtility.GetRect(GUIContent.none, popup);
			PreferencesWindow.AppsListUserData userData = new PreferencesWindow.AppsListUserData(paths, outString, onChanged);
			if (EditorGUI.DropdownButton(rect, content, FocusType.Passive, popup))
			{
				ArrayUtility.Add<string>(ref names, PreferencesWindow.Styles.browse.text);
				EditorUtility.DisplayCustomMenu(rect, names, array, new EditorUtility.SelectMenuItemFunction(this.AppsListClick), userData, false);
			}
			GUILayout.EndHorizontal();
		}

		private string[] BuildAppPathList(string userAppPath, string recentAppsKey, string stringForInternalEditor)
		{
			string[] array = new string[]
			{
				stringForInternalEditor
			};
			if (userAppPath != null && userAppPath.Length != 0 && Array.IndexOf<string>(array, userAppPath) == -1)
			{
				ArrayUtility.Add<string>(ref array, userAppPath);
			}
			for (int i = 0; i < 10; i++)
			{
				string text = EditorPrefs.GetString(recentAppsKey + i);
				if (!File.Exists(text))
				{
					text = "";
					EditorPrefs.SetString(recentAppsKey + i, text);
				}
				if (text.Length != 0 && Array.IndexOf<string>(array, text) == -1)
				{
					ArrayUtility.Add<string>(ref array, text);
				}
			}
			return array;
		}

		private string[] BuildFriendlyAppNameList(string[] appPathList, string[] appEditionList, string defaultBuiltIn)
		{
			List<string> list = new List<string>();
			for (int i = 0; i < appPathList.Length; i++)
			{
				string text = appPathList[i];
				if (text == "internal" || text == "")
				{
					list.Add(defaultBuiltIn);
				}
				else
				{
					string text2 = this.StripMicrosoftFromVisualStudioName(OSUtil.GetAppFriendlyName(text));
					if (appEditionList != null && !string.IsNullOrEmpty(appEditionList[i]))
					{
						text2 = string.Format("{0} ({1})", text2, appEditionList[i]);
					}
					list.Add(text2);
				}
			}
			return list.ToArray();
		}
	}
}
