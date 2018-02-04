using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.Connect;
using UnityEditor.IMGUI.Controls;
using UnityEditor.Modules;
using UnityEditor.PackageManager;
using UnityEditor.SceneManagement;
using UnityEditor.VersionControl;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

namespace UnityEditor
{
	public class BuildPlayerWindow : EditorWindow
	{
		private class Styles
		{
			public GUIContent invalidColorSpaceMessage = EditorGUIUtility.TrTextContent("In order to build a player go to 'Player Settings...' to resolve the incompatibility between the Color Space and the current settings.", EditorGUIUtility.GetHelpIcon(MessageType.Warning));

			public GUIStyle selected = "OL SelectedRow";

			public GUIStyle box = "OL Box";

			public GUIStyle title = EditorStyles.boldLabel;

			public GUIStyle evenRow = "CN EntryBackEven";

			public GUIStyle oddRow = "CN EntryBackOdd";

			public GUIStyle platformSelector = "PlayerSettingsPlatform";

			public GUIStyle toggle = "Toggle";

			public GUIStyle levelString = "PlayerSettingsLevel";

			public GUIStyle levelStringCounter = new GUIStyle("Label");

			public Vector2 toggleSize;

			public GUIContent becauseYouAreNot = EditorGUIUtility.TrTextContent("Because you are not a member of this project this build will not access Unity services.", EditorGUIUtility.GetHelpIcon(MessageType.Warning));

			public GUIContent noSessionDialogText = EditorGUIUtility.TrTextContent("In order to publish your build to UDN, you need to sign in via the AssetStore and tick the 'Stay signed in' checkbox.", null, null);

			public GUIContent platformTitle = EditorGUIUtility.TrTextContent("Platform", "Which platform to build for", null);

			public GUIContent switchPlatform = EditorGUIUtility.TrTextContent("Switch Platform", null, null);

			public GUIContent build = EditorGUIUtility.TrTextContent("Build", null, null);

			public GUIContent buildAndRun = EditorGUIUtility.TrTextContent("Build And Run", null, null);

			public GUIContent scenesInBuild = EditorGUIUtility.TrTextContent("Scenes In Build", "Which scenes to include in the build", null);

			public GUIContent checkOut = EditorGUIUtility.TrTextContent("Check out", null, null);

			public GUIContent addOpenSource = EditorGUIUtility.TrTextContent("Add Open Scenes", null, null);

			public string noModuleLoaded = L10n.Tr("No {0} module loaded.");

			public GUIContent openDownloadPage = EditorGUIUtility.TrTextContent("Open Download Page", null, null);

			public string infoText = L10n.Tr("{0} is not included in your Unity Pro license. Your {0} build will include a Unity Personal Edition splash screen.\n\nYou must be eligible to use Unity Personal Edition to use this build option. Please refer to our EULA for further information.");

			public GUIContent eula = EditorGUIUtility.TrTextContent("Eula", null, null);

			public string addToYourPro = L10n.Tr("Add {0} to your Unity Pro license");

			public GUIContent useNativeCompilation = EditorGUIUtility.TrTextContent("Use native compilation (no Mono runtime)", null, null);

			public Texture2D activePlatformIcon = EditorGUIUtility.IconContent("BuildSettings.SelectedIcon").image as Texture2D;

			public const float kButtonWidth = 110f;

			private const string kShopURL = "https://store.unity3d.com/shop/";

			private const string kDownloadURL = "http://unity3d.com/unity/download/";

			private const string kMailURL = "http://unity3d.com/company/sales?type=sales";

			private const string kMultiPlatformURL = "https://unity3d.com/unity/features/multiplatform";

			public GUIContent[,] notLicensedMessages;

			private GUIContent[,] buildTargetNotInstalled;

			public GUIContent debugBuild;

			public GUIContent profileBuild;

			public GUIContent allowDebugging;

			public GUIContent waitForManagedDebugger;

			public GUIContent symlinkiOSLibraries;

			public GUIContent explicitNullChecks;

			public GUIContent explicitDivideByZeroChecks;

			public GUIContent explicitArrayBoundsChecks;

			public GUIContent enableHeadlessMode;

			public GUIContent buildScriptsOnly;

			public GUIContent learnAboutUnityCloudBuild;

			public GUIContent compressionMethod;

			public Compression[] compressionTypes;

			public GUIContent[] compressionStrings;

			public Styles()
			{
				GUIContent[,] expr_1DD = new GUIContent[13, 3];
				expr_1DD[0, 0] = EditorGUIUtility.TextContent("Your license does not cover Standalone Publishing.");
				expr_1DD[0, 1] = new GUIContent("");
				expr_1DD[0, 2] = new GUIContent("https://store.unity3d.com/shop/");
				expr_1DD[1, 0] = EditorGUIUtility.TextContent("Your license does not cover iOS Publishing.");
				expr_1DD[1, 1] = EditorGUIUtility.TrTextContent("Go to Our Online Store", null, null);
				expr_1DD[1, 2] = new GUIContent("https://store.unity3d.com/shop/");
				expr_1DD[2, 0] = EditorGUIUtility.TextContent("Your license does not cover Apple TV Publishing.");
				expr_1DD[2, 1] = EditorGUIUtility.TrTextContent("Go to Our Online Store", null, null);
				expr_1DD[2, 2] = new GUIContent("https://store.unity3d.com/shop/");
				expr_1DD[3, 0] = EditorGUIUtility.TextContent("Your license does not cover Android Publishing.");
				expr_1DD[3, 1] = EditorGUIUtility.TrTextContent("Go to Our Online Store", null, null);
				expr_1DD[3, 2] = new GUIContent("https://store.unity3d.com/shop/");
				expr_1DD[4, 0] = EditorGUIUtility.TextContent("Your license does not cover Tizen Publishing.");
				expr_1DD[4, 1] = EditorGUIUtility.TrTextContent("Go to Our Online Store", null, null);
				expr_1DD[4, 2] = new GUIContent("https://store.unity3d.com/shop/");
				expr_1DD[5, 0] = EditorGUIUtility.TextContent("Your license does not cover Xbox One Publishing. For more information please see 'Unity for Console' on the multiplatform site.");
				expr_1DD[5, 1] = EditorGUIUtility.TrTextContent("Unity Multiplatform", null, null);
				expr_1DD[5, 2] = new GUIContent("https://unity3d.com/unity/features/multiplatform");
				expr_1DD[6, 0] = EditorGUIUtility.TextContent("Your license does not cover PS Vita Publishing. For more information please see 'Unity for Console' on the multiplatform site.");
				expr_1DD[6, 1] = EditorGUIUtility.TrTextContent("Unity Multiplatform", null, null);
				expr_1DD[6, 2] = new GUIContent("https://unity3d.com/unity/features/multiplatform");
				expr_1DD[7, 0] = EditorGUIUtility.TextContent("Your license does not cover PS4 Publishing. For more information please see 'Unity for Console' on the multiplatform site.");
				expr_1DD[7, 1] = EditorGUIUtility.TrTextContent("Unity Multiplatform", null, null);
				expr_1DD[7, 2] = new GUIContent("https://unity3d.com/unity/features/multiplatform");
				expr_1DD[8, 0] = EditorGUIUtility.TextContent("Your license does not cover Universal Windows Platform Publishing.");
				expr_1DD[8, 1] = EditorGUIUtility.TrTextContent("Go to Our Online Store", null, null);
				expr_1DD[8, 2] = new GUIContent("https://store.unity3d.com/shop/");
				expr_1DD[9, 0] = EditorGUIUtility.TextContent("Your license does not cover Windows Phone 8 Publishing.");
				expr_1DD[9, 1] = EditorGUIUtility.TrTextContent("Go to Our Online Store", null, null);
				expr_1DD[9, 2] = new GUIContent("https://store.unity3d.com/shop/");
				expr_1DD[10, 0] = EditorGUIUtility.TextContent("Your license does not cover Nintendo 3DS Publishing");
				expr_1DD[10, 1] = EditorGUIUtility.TrTextContent("Contact sales", null, null);
				expr_1DD[10, 2] = new GUIContent("http://unity3d.com/company/sales?type=sales");
				expr_1DD[11, 0] = EditorGUIUtility.TextContent("Your license does not cover Facebook Publishing");
				expr_1DD[11, 1] = EditorGUIUtility.TrTextContent("Go to Our Online Store", null, null);
				expr_1DD[11, 2] = new GUIContent("https://store.unity3d.com/shop/");
				expr_1DD[12, 0] = EditorGUIUtility.TextContent("Your license does not cover Nintendo Switch Publishing");
				expr_1DD[12, 1] = EditorGUIUtility.TrTextContent("Contact sales", null, null);
				expr_1DD[12, 2] = new GUIContent("http://unity3d.com/company/sales?type=sales");
				this.notLicensedMessages = expr_1DD;
				GUIContent[,] expr_4CD = new GUIContent[13, 3];
				expr_4CD[0, 0] = EditorGUIUtility.TrTextContent("Standalone Player is not supported in this build.\nDownload a build that supports it.", null, null);
				expr_4CD[0, 2] = new GUIContent("http://unity3d.com/unity/download/");
				expr_4CD[1, 0] = EditorGUIUtility.TrTextContent("iOS Player is not supported in this build.\nDownload a build that supports it.", null, null);
				expr_4CD[1, 2] = new GUIContent("http://unity3d.com/unity/download/");
				expr_4CD[2, 0] = EditorGUIUtility.TrTextContent("Apple TV Player is not supported in this build.\nDownload a build that supports it.", null, null);
				expr_4CD[2, 2] = new GUIContent("http://unity3d.com/unity/download/");
				expr_4CD[3, 0] = EditorGUIUtility.TrTextContent("Android Player is not supported in this build.\nDownload a build that supports it.", null, null);
				expr_4CD[3, 2] = new GUIContent("http://unity3d.com/unity/download/");
				expr_4CD[4, 0] = EditorGUIUtility.TrTextContent("Tizen is not supported in this build.\nDownload a build that supports it.", null, null);
				expr_4CD[4, 2] = new GUIContent("http://unity3d.com/unity/download/");
				expr_4CD[5, 0] = EditorGUIUtility.TrTextContent("Xbox One Player is not supported in this build.\nDownload a build that supports it.", null, null);
				expr_4CD[5, 2] = new GUIContent("http://unity3d.com/unity/download/");
				expr_4CD[6, 0] = EditorGUIUtility.TrTextContent("PS Vita Player is not supported in this build.\nDownload a build that supports it.", null, null);
				expr_4CD[6, 2] = new GUIContent("http://unity3d.com/unity/download/");
				expr_4CD[7, 0] = EditorGUIUtility.TrTextContent("PS4 Player is not supported in this build.\nDownload a build that supports it.", null, null);
				expr_4CD[7, 2] = new GUIContent("http://unity3d.com/unity/download/");
				expr_4CD[8, 0] = EditorGUIUtility.TrTextContent("Universal Windows Platform Player is not supported in\nthis build.\n\nDownload a build that supports it.", null, null);
				expr_4CD[8, 2] = new GUIContent("http://unity3d.com/unity/download/");
				expr_4CD[9, 0] = EditorGUIUtility.TrTextContent("Windows Phone 8 Player is not supported\nin this build.\n\nDownload a build that supports it.", null, null);
				expr_4CD[9, 2] = new GUIContent("http://unity3d.com/unity/download/");
				expr_4CD[10, 0] = EditorGUIUtility.TrTextContent("Nintendo 3DS is not supported in this build.\nDownload a build that supports it.", null, null);
				expr_4CD[10, 2] = new GUIContent("http://unity3d.com/unity/download/");
				expr_4CD[11, 0] = EditorGUIUtility.TrTextContent("Facebook is not supported in this build.\nDownload a build that supports it.", null, null);
				expr_4CD[11, 2] = new GUIContent("http://unity3d.com/unity/download/");
				expr_4CD[12, 0] = EditorGUIUtility.TrTextContent("Nintendo Switch is not supported in this build.\nDownload a build that supports it.", null, null);
				expr_4CD[12, 2] = new GUIContent("http://unity3d.com/unity/download/");
				this.buildTargetNotInstalled = expr_4CD;
				this.debugBuild = EditorGUIUtility.TrTextContent("Development Build", null, null);
				this.profileBuild = EditorGUIUtility.TrTextContent("Autoconnect Profiler", null, null);
				this.allowDebugging = EditorGUIUtility.TrTextContent("Script Debugging", null, null);
				this.waitForManagedDebugger = EditorGUIUtility.TrTextContent("Wait For Managed Debugger", "Show a dialog where you can attach a managed debugger before any script execution.", null);
				this.symlinkiOSLibraries = EditorGUIUtility.TrTextContent("Symlink Unity libraries", null, null);
				this.explicitNullChecks = EditorGUIUtility.TrTextContent("Explicit Null Checks", null, null);
				this.explicitDivideByZeroChecks = EditorGUIUtility.TrTextContent("Divide By Zero Checks", null, null);
				this.explicitArrayBoundsChecks = EditorGUIUtility.TrTextContent("Array Bounds Checks", null, null);
				this.enableHeadlessMode = EditorGUIUtility.TrTextContent("Headless Mode", null, null);
				this.buildScriptsOnly = EditorGUIUtility.TrTextContent("Scripts Only Build", null, null);
				this.learnAboutUnityCloudBuild = EditorGUIUtility.TrTextContent("Learn about Unity Cloud Build", null, null);
				this.compressionMethod = EditorGUIUtility.TrTextContent("Compression Method", "Compression applied to Player data (scenes and resources).\nDefault - none or default platform compression.\nLZ4 - fast compression suitable for Development Builds.\nLZ4HC - higher compression rate variance of LZ4, causes longer build times. Works best for Release Builds.", null);
				this.compressionTypes = new Compression[]
				{
					Compression.None,
					Compression.Lz4,
					Compression.Lz4HC
				};
				this.compressionStrings = new GUIContent[]
				{
					EditorGUIUtility.TrTextContent("Default", null, null),
					EditorGUIUtility.TrTextContent("LZ4", null, null),
					EditorGUIUtility.TrTextContent("LZ4HC", null, null)
				};
				base..ctor();
				this.levelStringCounter.alignment = TextAnchor.MiddleRight;
				if (Unsupported.IsSourceBuild() && (this.buildTargetNotInstalled.GetLength(0) != this.notLicensedMessages.GetLength(0) || this.buildTargetNotInstalled.GetLength(0) != BuildPlatforms.instance.buildPlatforms.Length))
				{
					Debug.LogErrorFormat("Build platforms and messages are desynced in BuildPlayerWindow! ({0} vs. {1} vs. {2}) DON'T SHIP THIS!", new object[]
					{
						this.buildTargetNotInstalled.GetLength(0),
						this.notLicensedMessages.GetLength(0),
						BuildPlatforms.instance.buildPlatforms.Length
					});
				}
			}

			public GUIContent GetTargetNotInstalled(int index, int item)
			{
				if (index >= this.buildTargetNotInstalled.GetLength(0))
				{
					index = 0;
				}
				return this.buildTargetNotInstalled[index, item];
			}

			public GUIContent GetDownloadErrorForTarget(BuildTarget target)
			{
				return null;
			}
		}

		public class BuildMethodException : Exception
		{
			public BuildMethodException() : base("")
			{
			}

			public BuildMethodException(string message) : base(message)
			{
			}
		}

		public static class DefaultBuildMethods
		{
			public static void BuildPlayer(BuildPlayerOptions options)
			{
				if (!UnityConnect.instance.canBuildWithUPID)
				{
					if (!EditorUtility.DisplayDialog("Missing Project ID", "Because you are not a member of this project this build will not access Unity services.\nDo you want to continue?", "Yes", "No"))
					{
						throw new BuildPlayerWindow.BuildMethodException();
					}
				}
				if (!BuildPipeline.IsBuildTargetSupported(options.targetGroup, options.target))
				{
					throw new BuildPlayerWindow.BuildMethodException("Build target is not supported.");
				}
				string targetStringFrom = ModuleManager.GetTargetStringFrom(EditorUserBuildSettings.selectedBuildTargetGroup, options.target);
				IBuildWindowExtension buildWindowExtension = ModuleManager.GetBuildWindowExtension(targetStringFrom);
				if (buildWindowExtension != null && (options.options & BuildOptions.AutoRunPlayer) != BuildOptions.None && !buildWindowExtension.EnabledBuildAndRunButton())
				{
					throw new BuildPlayerWindow.BuildMethodException();
				}
				if (Unsupported.IsBleedingEdgeBuild())
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.AppendLine("This version of Unity is a BleedingEdge build that has not seen any manual testing.");
					stringBuilder.AppendLine("You should consider this build unstable.");
					stringBuilder.AppendLine("We strongly recommend that you use a normal version of Unity instead.");
					if (EditorUtility.DisplayDialog("BleedingEdge Build", stringBuilder.ToString(), "Cancel", "OK"))
					{
						throw new BuildPlayerWindow.BuildMethodException();
					}
				}
				bool delayToAfterScriptReload = false;
				if (EditorUserBuildSettings.activeBuildTarget != options.target || EditorUserBuildSettings.activeBuildTargetGroup != options.targetGroup)
				{
					if (!EditorUserBuildSettings.SwitchActiveBuildTargetAsync(options.targetGroup, options.target))
					{
						string message = string.Format("Could not switch to build target '{0}', '{1}'.", BuildPipeline.GetBuildTargetGroupDisplayName(options.targetGroup), BuildPlatforms.instance.GetBuildTargetDisplayName(options.targetGroup, options.target));
						throw new BuildPlayerWindow.BuildMethodException(message);
					}
					if (EditorApplication.isCompiling)
					{
						delayToAfterScriptReload = true;
					}
				}
				BuildReport buildReport = BuildPipeline.BuildPlayerInternalNoCheck(options.scenes, options.locationPathName, null, options.targetGroup, options.target, options.options, delayToAfterScriptReload);
				if (buildReport != null)
				{
					string message2 = string.Format("Build completed with a result of '{0}'", buildReport.summary.result.ToString("g"));
					BuildResult result = buildReport.summary.result;
					if (result != BuildResult.Unknown)
					{
						if (result == BuildResult.Failed)
						{
							Debug.LogError(message2);
							throw new BuildPlayerWindow.BuildMethodException(buildReport.SummarizeErrors());
						}
						Debug.Log(message2);
					}
					else
					{
						Debug.LogWarning(message2);
					}
				}
			}

			public static BuildPlayerOptions GetBuildPlayerOptions(BuildPlayerOptions defaultBuildPlayerOptions)
			{
				return BuildPlayerWindow.DefaultBuildMethods.GetBuildPlayerOptionsInternal(true, defaultBuildPlayerOptions);
			}

			internal static BuildPlayerOptions GetBuildPlayerOptionsInternal(bool askForBuildLocation, BuildPlayerOptions defaultBuildPlayerOptions)
			{
				BuildPlayerOptions result = defaultBuildPlayerOptions;
				bool flag = false;
				BuildTarget buildTarget = EditorUserBuildSettingsUtils.CalculateSelectedBuildTarget();
				BuildTargetGroup selectedBuildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
				bool flag2 = EditorUserBuildSettings.installInBuildFolder && PostprocessBuildPlayer.SupportsInstallInBuildFolder(selectedBuildTargetGroup, buildTarget) && (Unsupported.IsSourceBuild() || BuildPlayerWindow.DefaultBuildMethods.IsMetroPlayer(buildTarget));
				if (PostprocessBuildPlayer.SupportsLz4Compression(selectedBuildTargetGroup, buildTarget))
				{
					if (EditorUserBuildSettings.GetCompressionType(selectedBuildTargetGroup) == Compression.Lz4)
					{
						result.options |= BuildOptions.CompressWithLz4;
					}
					else if (EditorUserBuildSettings.GetCompressionType(selectedBuildTargetGroup) == Compression.Lz4HC)
					{
						result.options |= BuildOptions.CompressWithLz4HC;
					}
				}
				bool development = EditorUserBuildSettings.development;
				if (development)
				{
					result.options |= BuildOptions.Development;
				}
				if (EditorUserBuildSettings.allowDebugging && development)
				{
					result.options |= BuildOptions.AllowDebugging;
				}
				if (EditorUserBuildSettings.symlinkLibraries)
				{
					result.options |= BuildOptions.SymlinkLibraries;
				}
				if (EditorUserBuildSettings.enableHeadlessMode)
				{
					result.options |= BuildOptions.EnableHeadlessMode;
				}
				if (EditorUserBuildSettings.connectProfiler && (development || buildTarget == BuildTarget.WSAPlayer))
				{
					result.options |= BuildOptions.ConnectWithProfiler;
				}
				if (EditorUserBuildSettings.buildScriptsOnly)
				{
					result.options |= BuildOptions.BuildScriptsOnly;
				}
				if (flag2)
				{
					result.options |= BuildOptions.InstallInBuildFolder;
				}
				if (!flag2)
				{
					if (askForBuildLocation && !BuildPlayerWindow.DefaultBuildMethods.PickBuildLocation(selectedBuildTargetGroup, buildTarget, result.options, out flag))
					{
						throw new BuildPlayerWindow.BuildMethodException();
					}
					string buildLocation = EditorUserBuildSettings.GetBuildLocation(buildTarget);
					if (buildLocation.Length == 0)
					{
						throw new BuildPlayerWindow.BuildMethodException("Build location for buildTarget " + buildTarget.ToString() + "is not valid.");
					}
					if (!askForBuildLocation)
					{
						CanAppendBuild canAppendBuild = InternalEditorUtility.BuildCanBeAppended(buildTarget, buildLocation);
						if (canAppendBuild != CanAppendBuild.Unsupported)
						{
							if (canAppendBuild != CanAppendBuild.Yes)
							{
								if (canAppendBuild == CanAppendBuild.No)
								{
									if (!BuildPlayerWindow.DefaultBuildMethods.PickBuildLocation(selectedBuildTargetGroup, buildTarget, result.options, out flag))
									{
										throw new BuildPlayerWindow.BuildMethodException();
									}
									buildLocation = EditorUserBuildSettings.GetBuildLocation(buildTarget);
									if (!BuildPlayerWindow.BuildLocationIsValid(buildLocation))
									{
										throw new BuildPlayerWindow.BuildMethodException("Build location for buildTarget " + buildTarget.ToString() + "is not valid.");
									}
								}
							}
							else
							{
								flag = true;
							}
						}
					}
				}
				if (flag)
				{
					result.options |= BuildOptions.AcceptExternalModificationsToPlayer;
				}
				result.target = buildTarget;
				result.targetGroup = selectedBuildTargetGroup;
				result.locationPathName = EditorUserBuildSettings.GetBuildLocation(buildTarget);
				result.assetBundleManifestPath = null;
				ArrayList arrayList = new ArrayList();
				EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
				EditorBuildSettingsScene[] array = scenes;
				for (int i = 0; i < array.Length; i++)
				{
					EditorBuildSettingsScene editorBuildSettingsScene = array[i];
					if (editorBuildSettingsScene.enabled)
					{
						arrayList.Add(editorBuildSettingsScene.path);
					}
				}
				result.scenes = (arrayList.ToArray(typeof(string)) as string[]);
				return result;
			}

			private static bool PickBuildLocation(BuildTargetGroup targetGroup, BuildTarget target, BuildOptions options, out bool updateExistingBuild)
			{
				updateExistingBuild = false;
				string buildLocation = EditorUserBuildSettings.GetBuildLocation(target);
				string directory;
				string text;
				if (buildLocation == string.Empty)
				{
					directory = FileUtil.DeleteLastPathNameComponent(Application.dataPath);
					text = "";
				}
				else
				{
					directory = FileUtil.DeleteLastPathNameComponent(buildLocation);
					text = FileUtil.GetLastPathNameComponent(buildLocation);
				}
				string extensionForBuildTarget = PostprocessBuildPlayer.GetExtensionForBuildTarget(targetGroup, target, options);
				if (extensionForBuildTarget != Path.GetExtension(text).Replace(".", ""))
				{
					text = string.Empty;
				}
				string title = "Build " + BuildPlatforms.instance.GetBuildTargetDisplayName(targetGroup, target);
				string text2 = EditorUtility.SaveBuildPanel(target, title, directory, text, extensionForBuildTarget, out updateExistingBuild);
				bool result;
				if (text2 == string.Empty)
				{
					result = false;
				}
				else
				{
					if (extensionForBuildTarget != string.Empty && FileUtil.GetPathExtension(text2).ToLower() != extensionForBuildTarget)
					{
						text2 = text2 + '.' + extensionForBuildTarget;
					}
					string lastPathNameComponent = FileUtil.GetLastPathNameComponent(text2);
					if (lastPathNameComponent == string.Empty)
					{
						result = false;
					}
					else
					{
						string path = (!(extensionForBuildTarget != string.Empty)) ? text2 : FileUtil.DeleteLastPathNameComponent(text2);
						if (!Directory.Exists(path))
						{
							Directory.CreateDirectory(path);
						}
						if (target == BuildTarget.iOS && Application.platform != RuntimePlatform.OSXEditor && !BuildPlayerWindow.DefaultBuildMethods.FolderIsEmpty(text2) && !BuildPlayerWindow.DefaultBuildMethods.UserWantsToDeleteFiles(text2))
						{
							result = false;
						}
						else
						{
							EditorUserBuildSettings.SetBuildLocation(target, text2);
							result = true;
						}
					}
				}
				return result;
			}

			private static bool FolderIsEmpty(string path)
			{
				return !Directory.Exists(path) || (Directory.GetDirectories(path).Length == 0 && Directory.GetFiles(path).Length == 0);
			}

			private static bool UserWantsToDeleteFiles(string path)
			{
				string message = "WARNING: all files and folders located in target folder: '" + path + "' will be deleted by build process.";
				return EditorUtility.DisplayDialog("Deleting existing files", message, "OK", "Cancel");
			}

			private static bool IsMetroPlayer(BuildTarget target)
			{
				return target == BuildTarget.WSAPlayer;
			}
		}

		private enum PackmanOperationType : uint
		{
			None,
			List,
			Add,
			Remove,
			Search,
			Outdated
		}

		private class PublishStyles
		{
			public const int kIconSize = 32;

			public const int kRowHeight = 36;

			public GUIContent xiaomiIcon = EditorGUIUtility.IconContent("BuildSettings.Xiaomi");

			public GUIContent learnAboutXiaomiInstallation = EditorGUIUtility.TrTextContent("Installation and Setup", null, null);

			public GUIContent publishTitle = EditorGUIUtility.TrTextContent("SDKs for App Stores", "Integrations with 3rd party app stores", null);
		}

		private Vector2 scrollPosition = new Vector2(0f, 0f);

		private const string kEditorBuildSettingsPath = "ProjectSettings/EditorBuildSettings.asset";

		internal const string kSettingDebuggingWaitForManagedDebugger = "WaitForManagedDebugger";

		private static BuildPlayerWindow.Styles styles = null;

		private BuildPlayerSceneTreeView m_TreeView = null;

		[SerializeField]
		private TreeViewState m_TreeViewState;

		private static Regex s_VersionPattern = new Regex("(?<shortVersion>\\d+\\.\\d+\\.\\d+(?<suffix>((?<alphabeta>[abx])|[fp])[^\\s]*))( \\((?<revision>[a-fA-F\\d]+)\\))?", RegexOptions.Compiled);

		private static Dictionary<string, string> s_ModuleNames = new Dictionary<string, string>
		{
			{
				"tvOS",
				"AppleTV"
			},
			{
				"OSXStandalone",
				"Mac"
			},
			{
				"WindowsStandalone",
				"Windows"
			},
			{
				"LinuxStandalone",
				"Linux"
			},
			{
				"Facebook",
				"Facebook-Games"
			}
		};

		private static Func<BuildPlayerOptions, BuildPlayerOptions> getBuildPlayerOptionsHandler;

		private static Action<BuildPlayerOptions> buildPlayerHandler;

		private static bool m_Building = false;

		private long getCurrentVersionOperationId = -1L;

		private long getLatestVersionOperationId = -1L;

		private bool isVersionInitialized = false;

		private long packmanOperationId = -1L;

		private BuildPlayerWindow.PackmanOperationType packmanOperationType = BuildPlayerWindow.PackmanOperationType.None;

		private bool packmanOperationRunning = false;

		private string xiaomiPackageName = "com.unity.xiaomi";

		private string currentXiaomiPackageVersion = "";

		private string latestXiaomiPackageVersion = "";

		private bool xiaomiPackageInstalled = false;

		private BuildPlayerWindow.PublishStyles publishStyles = null;

		private string CurrentXiaomiPackageId
		{
			get
			{
				return this.xiaomiPackageName + "@" + this.currentXiaomiPackageVersion;
			}
		}

		private string LatestXiaomiPackageId
		{
			get
			{
				return this.xiaomiPackageName + "@" + this.latestXiaomiPackageVersion;
			}
		}

		public BuildPlayerWindow()
		{
			base.position = new Rect(50f, 50f, 540f, 530f);
			base.minSize = new Vector2(630f, 580f);
			base.titleContent = EditorGUIUtility.TrTextContent("Build Settings", null, null);
		}

		public static void ShowBuildPlayerWindow()
		{
			EditorUserBuildSettings.selectedBuildTargetGroup = EditorUserBuildSettings.activeBuildTargetGroup;
			EditorWindow.GetWindow<BuildPlayerWindow>(true, "Build Settings");
		}

		private static bool BuildLocationIsValid(string path)
		{
			return path.Length > 0 && Directory.Exists(FileUtil.DeleteLastPathNameComponent(path));
		}

		private static void BuildPlayerAndRun()
		{
			BuildTarget target = EditorUserBuildSettingsUtils.CalculateSelectedBuildTarget();
			string buildLocation = EditorUserBuildSettings.GetBuildLocation(target);
			BuildPlayerWindow.BuildPlayerAndRun(!BuildPlayerWindow.BuildLocationIsValid(buildLocation));
		}

		private static void BuildPlayerAndRun(bool askForBuildLocation)
		{
			BuildPlayerWindow.CallBuildMethods(askForBuildLocation, BuildOptions.AutoRunPlayer | BuildOptions.StrictMode);
		}

		private void ActiveScenesGUI()
		{
			if (this.m_TreeView == null)
			{
				if (this.m_TreeViewState == null)
				{
					this.m_TreeViewState = new TreeViewState();
				}
				this.m_TreeView = new BuildPlayerSceneTreeView(this.m_TreeViewState);
				this.m_TreeView.Reload();
			}
			Rect rect = GUILayoutUtility.GetRect(BuildPlayerWindow.styles.scenesInBuild, BuildPlayerWindow.styles.title);
			GUI.Label(rect, BuildPlayerWindow.styles.scenesInBuild, BuildPlayerWindow.styles.title);
			Rect rect2 = GUILayoutUtility.GetRect(0f, base.position.width, 0f, base.position.height);
			this.m_TreeView.OnGUI(rect2);
		}

		private void OnDisable()
		{
			if (this.m_TreeView != null)
			{
				this.m_TreeView.UnsubscribeListChange();
			}
		}

		private void AddOpenScenes()
		{
			List<EditorBuildSettingsScene> list = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
			bool flag = false;
			for (int i = 0; i < SceneManager.sceneCount; i++)
			{
				Scene scene = SceneManager.GetSceneAt(i);
				if (scene.path.Length != 0 || EditorSceneManager.SaveScene(scene, "", false))
				{
					if (!list.Any((EditorBuildSettingsScene s) => s.path == scene.path))
					{
						GUID gUID;
						GUID.TryParse(scene.guid, out gUID);
						EditorBuildSettingsScene item = (!(gUID == default(GUID))) ? new EditorBuildSettingsScene(gUID, true) : new EditorBuildSettingsScene(scene.path, true);
						list.Add(item);
						flag = true;
					}
				}
			}
			if (flag)
			{
				EditorBuildSettings.scenes = list.ToArray();
				this.m_TreeView.Reload();
				base.Repaint();
				GUIUtility.ExitGUI();
			}
		}

		private void ActiveBuildTargetsGUI()
		{
			GUILayout.BeginVertical(new GUILayoutOption[0]);
			GUILayout.BeginVertical(new GUILayoutOption[]
			{
				GUILayout.Width(255f)
			});
			GUILayout.Label(BuildPlayerWindow.styles.platformTitle, BuildPlayerWindow.styles.title, new GUILayoutOption[0]);
			this.scrollPosition = GUILayout.BeginScrollView(this.scrollPosition, "OL Box");
			for (int i = 0; i < 2; i++)
			{
				bool flag = i == 0;
				bool flag2 = false;
				BuildPlatform[] buildPlatforms = BuildPlatforms.instance.buildPlatforms;
				for (int j = 0; j < buildPlatforms.Length; j++)
				{
					BuildPlatform buildPlatform = buildPlatforms[j];
					if (BuildPlayerWindow.IsBuildTargetGroupSupported(buildPlatform.targetGroup, buildPlatform.defaultTarget) == flag)
					{
						if (BuildPlayerWindow.IsBuildTargetGroupSupported(buildPlatform.targetGroup, buildPlatform.defaultTarget) || buildPlatform.forceShowTarget)
						{
							if (BuildPipeline.IsBuildTargetCompatibleWithOS(buildPlatform.defaultTarget))
							{
								this.ShowOption(buildPlatform, buildPlatform.title, (!flag2) ? BuildPlayerWindow.styles.oddRow : BuildPlayerWindow.styles.evenRow);
								flag2 = !flag2;
							}
						}
					}
				}
				GUI.contentColor = Color.white;
			}
			GUILayout.EndScrollView();
			GUILayout.EndVertical();
			GUILayout.Space(10f);
			BuildTarget target = EditorUserBuildSettingsUtils.CalculateSelectedBuildTarget();
			BuildTargetGroup selectedBuildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUI.enabled = (BuildPipeline.IsBuildTargetSupported(selectedBuildTargetGroup, target) && EditorUserBuildSettings.activeBuildTargetGroup != selectedBuildTargetGroup);
			if (GUILayout.Button(BuildPlayerWindow.styles.switchPlatform, new GUILayoutOption[]
			{
				GUILayout.Width(110f)
			}))
			{
				EditorUserBuildSettings.SwitchActiveBuildTargetAsync(selectedBuildTargetGroup, target);
				GUIUtility.ExitGUI();
			}
			GUI.enabled = BuildPipeline.IsBuildTargetSupported(selectedBuildTargetGroup, target);
			if (GUILayout.Button(EditorGUIUtility.TrTextContent("Player Settings...", null, null), new GUILayoutOption[]
			{
				GUILayout.Width(110f)
			}))
			{
				Selection.activeObject = Unsupported.GetSerializedAssetInterfaceSingleton("PlayerSettings");
				EditorWindow.GetWindow<InspectorWindow>();
			}
			GUILayout.EndHorizontal();
			GUI.enabled = true;
			GUILayout.EndVertical();
		}

		private void ShowAlert()
		{
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.Space(10f);
			GUILayout.BeginVertical(new GUILayoutOption[0]);
			EditorGUILayout.HelpBox(EditorGUIUtility.TrTextContent("Unable to access Unity services. Please log in, or request membership to this project to use these services.", null, null).text, MessageType.Warning);
			GUILayout.EndVertical();
			GUILayout.Space(5f);
			GUILayout.EndHorizontal();
		}

		private void ShowOption(BuildPlatform bp, GUIContent title, GUIStyle background)
		{
			Rect rect = GUILayoutUtility.GetRect(50f, 36f);
			rect.x += 1f;
			rect.y += 1f;
			bool flag = BuildPipeline.LicenseCheck(bp.defaultTarget);
			GUI.contentColor = new Color(1f, 1f, 1f, (!flag) ? 0.7f : 1f);
			bool flag2 = EditorUserBuildSettings.selectedBuildTargetGroup == bp.targetGroup;
			if (Event.current.type == EventType.Repaint)
			{
				background.Draw(rect, GUIContent.none, false, false, flag2, false);
				GUI.Label(new Rect(rect.x + 3f, rect.y + 3f, 32f, 32f), title.image, GUIStyle.none);
				if (EditorUserBuildSettings.activeBuildTargetGroup == bp.targetGroup)
				{
					GUI.Label(new Rect(rect.xMax - (float)BuildPlayerWindow.styles.activePlatformIcon.width - 8f, rect.y + 3f + (float)((32 - BuildPlayerWindow.styles.activePlatformIcon.height) / 2), (float)BuildPlayerWindow.styles.activePlatformIcon.width, (float)BuildPlayerWindow.styles.activePlatformIcon.height), BuildPlayerWindow.styles.activePlatformIcon, GUIStyle.none);
				}
			}
			if (GUI.Toggle(rect, flag2, title.text, BuildPlayerWindow.styles.platformSelector))
			{
				if (EditorUserBuildSettings.selectedBuildTargetGroup != bp.targetGroup)
				{
					EditorUserBuildSettings.selectedBuildTargetGroup = bp.targetGroup;
					UnityEngine.Object[] array = Resources.FindObjectsOfTypeAll(typeof(InspectorWindow));
					for (int i = 0; i < array.Length; i++)
					{
						InspectorWindow inspectorWindow = array[i] as InspectorWindow;
						if (inspectorWindow != null)
						{
							inspectorWindow.Repaint();
						}
					}
				}
			}
		}

		private void OnGUI()
		{
			if (BuildPlayerWindow.styles == null)
			{
				BuildPlayerWindow.styles = new BuildPlayerWindow.Styles();
				BuildPlayerWindow.styles.toggleSize = BuildPlayerWindow.styles.toggle.CalcSize(new GUIContent("X"));
			}
			if (!UnityConnect.instance.canBuildWithUPID)
			{
				this.ShowAlert();
			}
			GUILayout.Space(5f);
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.Space(10f);
			GUILayout.BeginVertical(new GUILayoutOption[0]);
			string text = "";
			bool flag = !AssetDatabase.IsOpenForEdit("ProjectSettings/EditorBuildSettings.asset", out text, StatusQueryOptions.UseCachedIfPossible);
			using (new EditorGUI.DisabledScope(flag))
			{
				this.ActiveScenesGUI();
				GUILayout.BeginHorizontal(new GUILayoutOption[0]);
				if (flag)
				{
					GUI.enabled = true;
					if (Provider.enabled && GUILayout.Button(BuildPlayerWindow.styles.checkOut, new GUILayoutOption[0]))
					{
						Asset assetByPath = Provider.GetAssetByPath("ProjectSettings/EditorBuildSettings.asset");
						Provider.Checkout(new AssetList
						{
							assetByPath
						}, CheckoutMode.Asset);
					}
					GUILayout.Label(text, new GUILayoutOption[0]);
					GUI.enabled = false;
				}
				GUILayout.FlexibleSpace();
				if (GUILayout.Button(BuildPlayerWindow.styles.addOpenSource, new GUILayoutOption[0]))
				{
					this.AddOpenScenes();
				}
				GUILayout.EndHorizontal();
			}
			GUILayout.Space(10f);
			GUILayout.BeginHorizontal(new GUILayoutOption[]
			{
				GUILayout.Height(351f)
			});
			this.ActiveBuildTargetsGUI();
			GUILayout.Space(10f);
			GUILayout.BeginVertical(new GUILayoutOption[0]);
			this.ShowBuildTargetSettings();
			GUILayout.EndVertical();
			GUILayout.EndHorizontal();
			GUILayout.Space(10f);
			GUILayout.EndVertical();
			GUILayout.Space(10f);
			GUILayout.EndHorizontal();
		}

		internal static bool IsBuildTargetGroupSupported(BuildTargetGroup targetGroup, BuildTarget target)
		{
			return targetGroup == BuildTargetGroup.Standalone || BuildPipeline.IsBuildTargetSupported(targetGroup, target);
		}

		private static void RepairSelectedBuildTargetGroup()
		{
			BuildTargetGroup selectedBuildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
			if (selectedBuildTargetGroup == BuildTargetGroup.Unknown || BuildPlatforms.instance.BuildPlatformIndexFromTargetGroup(selectedBuildTargetGroup) < 0)
			{
				EditorUserBuildSettings.selectedBuildTargetGroup = BuildTargetGroup.Standalone;
			}
		}

		private static bool IsAnyStandaloneModuleLoaded()
		{
			return ModuleManager.IsPlatformSupportLoaded(ModuleManager.GetTargetStringFromBuildTarget(BuildTarget.StandaloneLinux)) || ModuleManager.IsPlatformSupportLoaded(ModuleManager.GetTargetStringFromBuildTarget(BuildTarget.StandaloneOSX)) || ModuleManager.IsPlatformSupportLoaded(ModuleManager.GetTargetStringFromBuildTarget(BuildTarget.StandaloneWindows));
		}

		private static bool IsColorSpaceValid(BuildPlatform platform)
		{
			bool result;
			if (PlayerSettings.colorSpace == ColorSpace.Linear)
			{
				bool flag = true;
				bool flag2 = true;
				if (platform.targetGroup == BuildTargetGroup.iPhone)
				{
					GraphicsDeviceType[] graphicsAPIs = PlayerSettings.GetGraphicsAPIs(BuildTarget.iOS);
					flag = (!graphicsAPIs.Contains(GraphicsDeviceType.OpenGLES3) && !graphicsAPIs.Contains(GraphicsDeviceType.OpenGLES2));
					Version requiredVersion = new Version(8, 0);
					flag2 = PlayerSettings.iOS.IsTargetVersionEqualOrHigher(requiredVersion);
				}
				else if (platform.targetGroup == BuildTargetGroup.tvOS)
				{
					GraphicsDeviceType[] graphicsAPIs2 = PlayerSettings.GetGraphicsAPIs(BuildTarget.tvOS);
					flag = (!graphicsAPIs2.Contains(GraphicsDeviceType.OpenGLES3) && !graphicsAPIs2.Contains(GraphicsDeviceType.OpenGLES2));
				}
				else if (platform.targetGroup == BuildTargetGroup.Android)
				{
					GraphicsDeviceType[] graphicsAPIs3 = PlayerSettings.GetGraphicsAPIs(BuildTarget.Android);
					flag = ((graphicsAPIs3.Contains(GraphicsDeviceType.Vulkan) || graphicsAPIs3.Contains(GraphicsDeviceType.OpenGLES3)) && !graphicsAPIs3.Contains(GraphicsDeviceType.OpenGLES2));
					flag2 = (PlayerSettings.Android.minSdkVersion >= AndroidSdkVersions.AndroidApiLevel18);
				}
				else if (platform.targetGroup == BuildTargetGroup.WebGL)
				{
					GraphicsDeviceType[] graphicsAPIs4 = PlayerSettings.GetGraphicsAPIs(BuildTarget.WebGL);
					flag = (graphicsAPIs4.Contains(GraphicsDeviceType.OpenGLES3) && !graphicsAPIs4.Contains(GraphicsDeviceType.OpenGLES2));
				}
				result = (flag && flag2);
			}
			else
			{
				result = true;
			}
			return result;
		}

		public static string GetPlaybackEngineDownloadURL(string moduleName)
		{
			string result;
			if (moduleName == "PS4" || moduleName == "PSP2" || moduleName == "XboxOne")
			{
				result = "https://unity3d.com/platform-installation";
			}
			else
			{
				string fullUnityVersion = InternalEditorUtility.GetFullUnityVersion();
				string text = "";
				string text2 = "";
				Match match = BuildPlayerWindow.s_VersionPattern.Match(fullUnityVersion);
				if (!match.Success || !match.Groups["shortVersion"].Success || !match.Groups["suffix"].Success)
				{
					Debug.LogWarningFormat("Error parsing version '{0}'", new object[]
					{
						fullUnityVersion
					});
				}
				if (match.Groups["shortVersion"].Success)
				{
					text2 = match.Groups["shortVersion"].Value;
				}
				if (match.Groups["revision"].Success)
				{
					text = match.Groups["revision"].Value;
				}
				if (BuildPlayerWindow.s_ModuleNames.ContainsKey(moduleName))
				{
					moduleName = BuildPlayerWindow.s_ModuleNames[moduleName];
				}
				string text3 = "download";
				string text4 = "download_unity";
				string text5 = "Unknown";
				string text6 = string.Empty;
				if (match.Groups["alphabeta"].Success)
				{
					text3 = "beta";
					text4 = "download";
				}
				if (Application.platform == RuntimePlatform.WindowsEditor)
				{
					text5 = "TargetSupportInstaller";
					text6 = ".exe";
				}
				else if (Application.platform == RuntimePlatform.OSXEditor)
				{
					text5 = "MacEditorTargetInstaller";
					text6 = ".pkg";
				}
				result = string.Format("http://{0}.unity3d.com/{1}/{2}/{3}/UnitySetup-{4}-Support-for-Editor-{5}{6}", new object[]
				{
					text3,
					text4,
					text,
					text5,
					moduleName,
					text2,
					text6
				});
			}
			return result;
		}

		private bool IsModuleInstalled(BuildTargetGroup buildTargetGroup, BuildTarget buildTarget)
		{
			bool flag = BuildPipeline.LicenseCheck(buildTarget);
			string targetStringFrom = ModuleManager.GetTargetStringFrom(buildTargetGroup, buildTarget);
			return flag && !string.IsNullOrEmpty(targetStringFrom) && ModuleManager.GetBuildPostProcessor(targetStringFrom) == null && (EditorUserBuildSettings.selectedBuildTargetGroup != BuildTargetGroup.Standalone || !BuildPlayerWindow.IsAnyStandaloneModuleLoaded());
		}

		private void ShowBuildTargetSettings()
		{
			EditorGUIUtility.labelWidth = Mathf.Min(180f, (base.position.width - 265f) * 0.47f);
			BuildTarget buildTarget = EditorUserBuildSettingsUtils.CalculateSelectedBuildTarget();
			BuildTargetGroup selectedBuildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
			BuildPlatform buildPlatform = BuildPlatforms.instance.BuildPlatformFromTargetGroup(selectedBuildTargetGroup);
			bool flag = BuildPipeline.LicenseCheck(buildTarget);
			GUILayout.Space(18f);
			Rect rect = GUILayoutUtility.GetRect(50f, 36f);
			rect.x += 1f;
			GUI.Label(new Rect(rect.x + 3f, rect.y + 3f, 32f, 32f), buildPlatform.title.image, GUIStyle.none);
			GUI.Toggle(rect, false, buildPlatform.title.text, BuildPlayerWindow.styles.platformSelector);
			GUILayout.Space(10f);
			if (buildPlatform.targetGroup == BuildTargetGroup.WebGL && !BuildPipeline.IsBuildTargetSupported(buildPlatform.targetGroup, buildTarget))
			{
				if (IntPtr.Size == 4)
				{
					GUILayout.Label("Building for WebGL requires a 64-bit Unity editor.", new GUILayoutOption[0]);
					BuildPlayerWindow.GUIBuildButtons(false, false, false, buildPlatform);
					return;
				}
			}
			string targetStringFrom = ModuleManager.GetTargetStringFrom(selectedBuildTargetGroup, buildTarget);
			if (this.IsModuleInstalled(selectedBuildTargetGroup, buildTarget))
			{
				GUILayout.Label(EditorGUIUtility.TextContent(string.Format(BuildPlayerWindow.styles.noModuleLoaded, BuildPlatforms.instance.GetModuleDisplayName(selectedBuildTargetGroup, buildTarget))), new GUILayoutOption[0]);
				if (GUILayout.Button(BuildPlayerWindow.styles.openDownloadPage, EditorStyles.miniButton, new GUILayoutOption[]
				{
					GUILayout.ExpandWidth(false)
				}))
				{
					string playbackEngineDownloadURL = BuildPlayerWindow.GetPlaybackEngineDownloadURL(targetStringFrom);
					Help.BrowseURL(playbackEngineDownloadURL);
				}
				BuildPlayerWindow.GUIBuildButtons(false, false, false, buildPlatform);
			}
			else
			{
				if (Application.HasProLicense() && !InternalEditorUtility.HasAdvancedLicenseOnBuildTarget(buildTarget))
				{
					string text = string.Format(BuildPlayerWindow.styles.infoText, BuildPlatforms.instance.GetBuildTargetDisplayName(selectedBuildTargetGroup, buildTarget));
					GUILayout.BeginVertical(EditorStyles.helpBox, new GUILayoutOption[0]);
					GUILayout.Label(text, EditorStyles.wordWrappedMiniLabel, new GUILayoutOption[0]);
					GUILayout.BeginHorizontal(new GUILayoutOption[0]);
					if (GUILayout.Button(BuildPlayerWindow.styles.eula, EditorStyles.miniButton, new GUILayoutOption[0]))
					{
						Application.OpenURL("http://unity3d.com/legal/eula");
					}
					if (GUILayout.Button(string.Format(BuildPlayerWindow.styles.addToYourPro, BuildPlatforms.instance.GetBuildTargetDisplayName(selectedBuildTargetGroup, buildTarget)), EditorStyles.miniButton, new GUILayoutOption[0]))
					{
						Application.OpenURL("http://unity3d.com/get-unity");
					}
					GUILayout.EndHorizontal();
					GUILayout.EndVertical();
				}
				GUIContent downloadErrorForTarget = BuildPlayerWindow.styles.GetDownloadErrorForTarget(buildTarget);
				if (downloadErrorForTarget != null)
				{
					GUILayout.Label(downloadErrorForTarget, EditorStyles.wordWrappedLabel, new GUILayoutOption[0]);
					BuildPlayerWindow.GUIBuildButtons(false, false, false, buildPlatform);
				}
				else if (!flag)
				{
					int num = BuildPlatforms.instance.BuildPlatformIndexFromTargetGroup(buildPlatform.targetGroup);
					GUILayout.Label(BuildPlayerWindow.styles.notLicensedMessages[num, 0], EditorStyles.wordWrappedLabel, new GUILayoutOption[0]);
					GUILayout.Space(5f);
					GUILayout.BeginHorizontal(new GUILayoutOption[0]);
					GUILayout.FlexibleSpace();
					if (BuildPlayerWindow.styles.notLicensedMessages[num, 1].text.Length != 0)
					{
						if (GUILayout.Button(BuildPlayerWindow.styles.notLicensedMessages[num, 1], new GUILayoutOption[0]))
						{
							Application.OpenURL(BuildPlayerWindow.styles.notLicensedMessages[num, 2].text);
						}
					}
					GUILayout.EndHorizontal();
					BuildPlayerWindow.GUIBuildButtons(false, false, false, buildPlatform);
				}
				else
				{
					string targetStringFrom2 = ModuleManager.GetTargetStringFrom(buildPlatform.targetGroup, buildTarget);
					IBuildWindowExtension buildWindowExtension = ModuleManager.GetBuildWindowExtension(targetStringFrom2);
					if (buildWindowExtension != null)
					{
						buildWindowExtension.ShowPlatformBuildOptions();
					}
					GUI.changed = false;
					BuildTargetGroup targetGroup = buildPlatform.targetGroup;
					if (targetGroup == BuildTargetGroup.iPhone || targetGroup == BuildTargetGroup.tvOS)
					{
						if (Application.platform == RuntimePlatform.OSXEditor)
						{
							EditorUserBuildSettings.symlinkLibraries = EditorGUILayout.Toggle(BuildPlayerWindow.styles.symlinkiOSLibraries, EditorUserBuildSettings.symlinkLibraries, new GUILayoutOption[0]);
						}
					}
					GUI.enabled = true;
					bool flag2 = buildWindowExtension == null || buildWindowExtension.EnabledBuildButton();
					bool enableBuildAndRunButton = false;
					bool flag3 = buildWindowExtension == null || buildWindowExtension.ShouldDrawScriptDebuggingCheckbox();
					bool flag4 = buildWindowExtension != null && buildWindowExtension.ShouldDrawExplicitNullCheckbox();
					bool flag5 = buildWindowExtension != null && buildWindowExtension.ShouldDrawExplicitDivideByZeroCheckbox();
					bool flag6 = buildWindowExtension != null && buildWindowExtension.ShouldDrawExplicitArrayBoundsCheckbox();
					bool flag7 = buildWindowExtension == null || buildWindowExtension.ShouldDrawDevelopmentPlayerCheckbox();
					bool flag8 = buildTarget == BuildTarget.StandaloneLinux || buildTarget == BuildTarget.StandaloneLinux64 || buildTarget == BuildTarget.StandaloneLinuxUniversal;
					IBuildPostprocessor buildPostProcessor = ModuleManager.GetBuildPostProcessor(selectedBuildTargetGroup, buildTarget);
					bool flag9 = buildPostProcessor != null && buildPostProcessor.SupportsScriptsOnlyBuild();
					bool canInstallInBuildFolder = false;
					if (BuildPipeline.IsBuildTargetSupported(selectedBuildTargetGroup, buildTarget))
					{
						bool flag10 = buildWindowExtension == null || buildWindowExtension.ShouldDrawProfilerCheckbox();
						GUI.enabled = flag7;
						if (flag7)
						{
							EditorUserBuildSettings.development = EditorGUILayout.Toggle(BuildPlayerWindow.styles.debugBuild, EditorUserBuildSettings.development, new GUILayoutOption[0]);
						}
						bool development = EditorUserBuildSettings.development;
						GUI.enabled = development;
						if (flag10)
						{
							if (!GUI.enabled)
							{
								if (!development)
								{
									BuildPlayerWindow.styles.profileBuild.tooltip = "Profiling only enabled in Development Player";
								}
							}
							else
							{
								BuildPlayerWindow.styles.profileBuild.tooltip = "";
							}
							EditorUserBuildSettings.connectProfiler = EditorGUILayout.Toggle(BuildPlayerWindow.styles.profileBuild, EditorUserBuildSettings.connectProfiler, new GUILayoutOption[0]);
						}
						GUI.enabled = development;
						if (flag3)
						{
							EditorUserBuildSettings.allowDebugging = EditorGUILayout.Toggle(BuildPlayerWindow.styles.allowDebugging, EditorUserBuildSettings.allowDebugging, new GUILayoutOption[0]);
							if (EditorUserBuildSettings.allowDebugging && Unsupported.IsSourceBuild())
							{
								string buildTargetName = BuildPipeline.GetBuildTargetName(buildTarget);
								EditorUserBuildSettings.SetPlatformSettings(buildTargetName, "WaitForManagedDebugger", EditorGUILayout.Toggle(BuildPlayerWindow.styles.waitForManagedDebugger, EditorUserBuildSettings.GetPlatformSettings(buildTargetName, "WaitForManagedDebugger") == "true", new GUILayoutOption[0]).ToString().ToLower());
							}
						}
						if (flag4)
						{
							GUI.enabled = !development;
							if (!GUI.enabled)
							{
								EditorUserBuildSettings.explicitNullChecks = true;
							}
							EditorUserBuildSettings.explicitNullChecks = EditorGUILayout.Toggle(BuildPlayerWindow.styles.explicitNullChecks, EditorUserBuildSettings.explicitNullChecks, new GUILayoutOption[0]);
							GUI.enabled = development;
						}
						if (flag5)
						{
							GUI.enabled = !development;
							if (!GUI.enabled)
							{
								EditorUserBuildSettings.explicitDivideByZeroChecks = true;
							}
							EditorUserBuildSettings.explicitDivideByZeroChecks = EditorGUILayout.Toggle(BuildPlayerWindow.styles.explicitDivideByZeroChecks, EditorUserBuildSettings.explicitDivideByZeroChecks, new GUILayoutOption[0]);
							GUI.enabled = development;
						}
						if (flag6)
						{
							GUI.enabled = !development;
							if (!GUI.enabled)
							{
								EditorUserBuildSettings.explicitArrayBoundsChecks = true;
							}
							EditorUserBuildSettings.explicitArrayBoundsChecks = EditorGUILayout.Toggle(BuildPlayerWindow.styles.explicitArrayBoundsChecks, EditorUserBuildSettings.explicitArrayBoundsChecks, new GUILayoutOption[0]);
							GUI.enabled = development;
						}
						if (flag9)
						{
							EditorUserBuildSettings.buildScriptsOnly = EditorGUILayout.Toggle(BuildPlayerWindow.styles.buildScriptsOnly, EditorUserBuildSettings.buildScriptsOnly, new GUILayoutOption[0]);
						}
						GUI.enabled = !development;
						if (flag8)
						{
							EditorUserBuildSettings.enableHeadlessMode = EditorGUILayout.Toggle(BuildPlayerWindow.styles.enableHeadlessMode, EditorUserBuildSettings.enableHeadlessMode && !development, new GUILayoutOption[0]);
						}
						GUI.enabled = true;
						GUILayout.FlexibleSpace();
						if (buildPostProcessor != null && buildPostProcessor.SupportsLz4Compression())
						{
							int num2 = Array.IndexOf<Compression>(BuildPlayerWindow.styles.compressionTypes, EditorUserBuildSettings.GetCompressionType(selectedBuildTargetGroup));
							if (num2 == -1)
							{
								num2 = 1;
							}
							num2 = EditorGUILayout.Popup(BuildPlayerWindow.styles.compressionMethod, num2, BuildPlayerWindow.styles.compressionStrings, new GUILayoutOption[0]);
							EditorUserBuildSettings.SetCompressionType(selectedBuildTargetGroup, BuildPlayerWindow.styles.compressionTypes[num2]);
						}
						canInstallInBuildFolder = (Unsupported.IsSourceBuild() && PostprocessBuildPlayer.SupportsInstallInBuildFolder(selectedBuildTargetGroup, buildTarget));
						if (flag2)
						{
							enableBuildAndRunButton = ((buildWindowExtension == null) ? (!EditorUserBuildSettings.installInBuildFolder) : (buildWindowExtension.EnabledBuildAndRunButton() && !EditorUserBuildSettings.installInBuildFolder));
						}
					}
					else
					{
						GUILayout.BeginHorizontal(new GUILayoutOption[]
						{
							GUILayout.ExpandWidth(true)
						});
						GUILayout.BeginVertical(new GUILayoutOption[]
						{
							GUILayout.ExpandWidth(true)
						});
						int index = BuildPlatforms.instance.BuildPlatformIndexFromTargetGroup(buildPlatform.targetGroup);
						GUILayout.Label(BuildPlayerWindow.styles.GetTargetNotInstalled(index, 0), new GUILayoutOption[0]);
						if (BuildPlayerWindow.styles.GetTargetNotInstalled(index, 1) != null && GUILayout.Button(BuildPlayerWindow.styles.GetTargetNotInstalled(index, 1), new GUILayoutOption[0]))
						{
							Application.OpenURL(BuildPlayerWindow.styles.GetTargetNotInstalled(index, 2).text);
						}
						GUILayout.EndVertical();
						GUILayout.FlexibleSpace();
						GUILayout.EndHorizontal();
					}
					if (buildTarget == BuildTarget.Android)
					{
						this.AndroidPublishGUI();
					}
					BuildPlayerWindow.GUIBuildButtons(buildWindowExtension, flag2, enableBuildAndRunButton, canInstallInBuildFolder, buildPlatform);
				}
			}
		}

		private static void GUIBuildButtons(bool enableBuildButton, bool enableBuildAndRunButton, bool canInstallInBuildFolder, BuildPlatform platform)
		{
			BuildPlayerWindow.GUIBuildButtons(null, enableBuildButton, enableBuildAndRunButton, canInstallInBuildFolder, platform);
		}

		private static void GUIBuildButtons(IBuildWindowExtension buildWindowExtension, bool enableBuildButton, bool enableBuildAndRunButton, bool canInstallInBuildFolder, BuildPlatform platform)
		{
			GUILayout.FlexibleSpace();
			if (canInstallInBuildFolder)
			{
				EditorUserBuildSettings.installInBuildFolder = GUILayout.Toggle(EditorUserBuildSettings.installInBuildFolder, "Install in Builds folder\n(for debugging with source code)", new GUILayoutOption[]
				{
					GUILayout.ExpandWidth(false)
				});
			}
			else
			{
				EditorUserBuildSettings.installInBuildFolder = false;
			}
			if (buildWindowExtension != null && Unsupported.IsSourceBuild())
			{
				buildWindowExtension.ShowInternalPlatformBuildOptions();
			}
			if (buildWindowExtension != null)
			{
				buildWindowExtension.ShowPlatformBuildWarnings();
			}
			if (!BuildPlayerWindow.IsColorSpaceValid(platform) && enableBuildButton && enableBuildAndRunButton)
			{
				enableBuildAndRunButton = false;
				enableBuildButton = false;
				EditorGUILayout.HelpBox(BuildPlayerWindow.styles.invalidColorSpaceMessage, true);
			}
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.FlexibleSpace();
			if (EditorGUILayout.LinkLabel(BuildPlayerWindow.styles.learnAboutUnityCloudBuild, new GUILayoutOption[0]))
			{
				Application.OpenURL(string.Format("{0}/from/editor/buildsettings?upid={1}&pid={2}&currentplatform={3}&selectedplatform={4}&unityversion={5}", new object[]
				{
					WebURLs.cloudBuildPage,
					CloudProjectSettings.projectId,
					PlayerSettings.productGUID,
					EditorUserBuildSettings.activeBuildTarget,
					EditorUserBuildSettingsUtils.CalculateSelectedBuildTarget(),
					Application.unityVersion
				}));
			}
			GUILayout.EndHorizontal();
			GUILayout.Space(6f);
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.FlexibleSpace();
			GUIContent gUIContent = null;
			GUIContent gUIContent2 = null;
			if (buildWindowExtension != null)
			{
				buildWindowExtension.GetBuildButtonTitles(out gUIContent, out gUIContent2);
			}
			gUIContent = (gUIContent ?? BuildPlayerWindow.styles.build);
			gUIContent2 = (gUIContent2 ?? BuildPlayerWindow.styles.buildAndRun);
			GUI.enabled = enableBuildButton;
			if (GUILayout.Button(gUIContent, new GUILayoutOption[]
			{
				GUILayout.Width(110f)
			}))
			{
				BuildPlayerWindow.CallBuildMethods(true, BuildOptions.ShowBuiltPlayer | BuildOptions.StrictMode);
				GUIUtility.ExitGUI();
			}
			GUI.enabled = enableBuildAndRunButton;
			if (GUILayout.Button(gUIContent2, new GUILayoutOption[]
			{
				GUILayout.Width(110f)
			}))
			{
				BuildPlayerWindow.BuildPlayerAndRun(true);
				GUIUtility.ExitGUI();
			}
			GUILayout.EndHorizontal();
		}

		public static void RegisterGetBuildPlayerOptionsHandler(Func<BuildPlayerOptions, BuildPlayerOptions> func)
		{
			if (func != null && BuildPlayerWindow.getBuildPlayerOptionsHandler != null)
			{
				Debug.LogWarning("The get build player options handler in BuildPlayerWindow is being reassigned!");
			}
			BuildPlayerWindow.getBuildPlayerOptionsHandler = func;
		}

		public static void RegisterBuildPlayerHandler(Action<BuildPlayerOptions> func)
		{
			if (func != null && BuildPlayerWindow.buildPlayerHandler != null)
			{
				Debug.LogWarning("The build player handler in BuildPlayerWindow is being reassigned!");
			}
			BuildPlayerWindow.buildPlayerHandler = func;
		}

		private static void CallBuildMethods(bool askForBuildLocation, BuildOptions defaultBuildOptions)
		{
			if (!BuildPlayerWindow.m_Building)
			{
				try
				{
					BuildPlayerWindow.m_Building = true;
					BuildPlayerOptions buildPlayerOptions = default(BuildPlayerOptions);
					buildPlayerOptions.options = defaultBuildOptions;
					if (BuildPlayerWindow.getBuildPlayerOptionsHandler != null)
					{
						buildPlayerOptions = BuildPlayerWindow.getBuildPlayerOptionsHandler(buildPlayerOptions);
					}
					else
					{
						buildPlayerOptions = BuildPlayerWindow.DefaultBuildMethods.GetBuildPlayerOptionsInternal(askForBuildLocation, buildPlayerOptions);
					}
					if (BuildPlayerWindow.buildPlayerHandler != null)
					{
						BuildPlayerWindow.buildPlayerHandler(buildPlayerOptions);
					}
					else
					{
						BuildPlayerWindow.DefaultBuildMethods.BuildPlayer(buildPlayerOptions);
					}
				}
				catch (BuildPlayerWindow.BuildMethodException ex)
				{
					if (!string.IsNullOrEmpty(ex.Message))
					{
						Debug.LogError(ex);
					}
				}
				finally
				{
					BuildPlayerWindow.m_Building = false;
				}
			}
		}

		private void AndroidPublishGUI()
		{
			if (this.publishStyles == null)
			{
				this.publishStyles = new BuildPlayerWindow.PublishStyles();
			}
			GUILayout.BeginVertical(new GUILayoutOption[0]);
			GUILayout.Label(this.publishStyles.publishTitle, BuildPlayerWindow.styles.title, new GUILayoutOption[0]);
			using (new EditorGUILayout.HorizontalScope(BuildPlayerWindow.styles.box, new GUILayoutOption[]
			{
				GUILayout.Height(36f)
			}))
			{
				GUILayout.BeginVertical(new GUILayoutOption[0]);
				GUILayout.Space(3f);
				GUILayout.BeginHorizontal(new GUILayoutOption[0]);
				GUILayout.Space(4f);
				GUILayout.BeginVertical(new GUILayoutOption[0]);
				GUILayout.Space(2f);
				GUILayout.Label(this.publishStyles.xiaomiIcon, new GUILayoutOption[]
				{
					GUILayout.Width(32f),
					GUILayout.Height(32f)
				});
				GUILayout.EndVertical();
				GUILayout.BeginVertical(new GUILayoutOption[0]);
				GUILayout.FlexibleSpace();
				GUILayout.Label("Xiaomi Mi Game Center", new GUILayoutOption[0]);
				GUILayout.FlexibleSpace();
				GUILayout.EndVertical();
				GUILayout.FlexibleSpace();
				GUILayout.BeginVertical(new GUILayoutOption[0]);
				GUILayout.FlexibleSpace();
				this.XiaomiPackageControlGUI();
				GUILayout.FlexibleSpace();
				GUILayout.EndVertical();
				GUILayout.Space(4f);
				GUILayout.EndHorizontal();
				GUILayout.EndVertical();
			}
			GUILayout.EndVertical();
		}

		private void XiaomiPackageControlGUI()
		{
			EditorGUI.BeginDisabledGroup(!this.isVersionInitialized || this.packmanOperationRunning);
			if (!this.xiaomiPackageInstalled)
			{
				if (GUILayout.Button("Add", new GUILayoutOption[]
				{
					GUILayout.Width(60f)
				}))
				{
					if (this.packmanOperationRunning)
					{
						return;
					}
					NativeStatusCode nativeStatusCode = NativeClient.Add(out this.packmanOperationId, this.LatestXiaomiPackageId);
					if (nativeStatusCode == NativeStatusCode.Error)
					{
						Debug.LogError("Add " + this.LatestXiaomiPackageId + " error, please add it again.");
						return;
					}
					this.packmanOperationType = BuildPlayerWindow.PackmanOperationType.Add;
					Console.WriteLine(string.Concat(new object[]
					{
						"Add: OperationID ",
						this.packmanOperationId,
						" for ",
						this.LatestXiaomiPackageId
					}));
					this.packmanOperationRunning = true;
				}
			}
			else
			{
				GUILayout.BeginHorizontal(new GUILayoutOption[0]);
				if (!string.IsNullOrEmpty(this.latestXiaomiPackageVersion) && this.currentXiaomiPackageVersion != this.latestXiaomiPackageVersion)
				{
					if (GUILayout.Button("Update", new GUILayoutOption[]
					{
						GUILayout.Width(60f)
					}))
					{
						if (this.packmanOperationRunning)
						{
							return;
						}
						if (EditorUtility.DisplayDialog("Update Xiaomi SDK", "Are you sure you want to update to " + this.latestXiaomiPackageVersion + " ?", "Yes", "No"))
						{
							NativeStatusCode nativeStatusCode2 = NativeClient.Add(out this.packmanOperationId, this.LatestXiaomiPackageId);
							if (nativeStatusCode2 == NativeStatusCode.Error)
							{
								Debug.LogError("Update " + this.LatestXiaomiPackageId + " error, please update it again.");
								return;
							}
							this.packmanOperationType = BuildPlayerWindow.PackmanOperationType.Add;
							Console.WriteLine(string.Concat(new object[]
							{
								"Update: OperationID ",
								this.packmanOperationId,
								" for ",
								this.LatestXiaomiPackageId
							}));
							this.packmanOperationRunning = true;
						}
					}
				}
				if (GUILayout.Button("Remove", new GUILayoutOption[]
				{
					GUILayout.Width(60f)
				}))
				{
					if (this.packmanOperationRunning)
					{
						return;
					}
					NativeStatusCode nativeStatusCode3 = NativeClient.Remove(out this.packmanOperationId, this.CurrentXiaomiPackageId);
					if (nativeStatusCode3 == NativeStatusCode.Error)
					{
						Debug.LogError("Remove " + this.CurrentXiaomiPackageId + " error, please remove it again.");
						return;
					}
					this.packmanOperationType = BuildPlayerWindow.PackmanOperationType.Remove;
					Console.WriteLine(string.Concat(new object[]
					{
						"Remove: OperationID ",
						this.packmanOperationId,
						" for ",
						this.CurrentXiaomiPackageId
					}));
					this.packmanOperationRunning = true;
				}
				GUILayout.EndHorizontal();
			}
			EditorGUI.EndDisabledGroup();
		}

		private bool CheckXiaomiPackageVersions()
		{
			bool result;
			if (this.isVersionInitialized)
			{
				result = true;
			}
			else
			{
				NativeStatusCode nativeStatusCode;
				if (this.getCurrentVersionOperationId < 0L)
				{
					nativeStatusCode = NativeClient.List(out this.getCurrentVersionOperationId);
				}
				else
				{
					nativeStatusCode = NativeClient.GetOperationStatus(this.getCurrentVersionOperationId);
				}
				if (nativeStatusCode > NativeStatusCode.Done)
				{
					this.getCurrentVersionOperationId = -1L;
					result = false;
				}
				else
				{
					NativeStatusCode nativeStatusCode2;
					if (this.getLatestVersionOperationId < 0L)
					{
						nativeStatusCode2 = NativeClient.Search(out this.getLatestVersionOperationId, this.xiaomiPackageName);
					}
					else
					{
						nativeStatusCode2 = NativeClient.GetOperationStatus(this.getLatestVersionOperationId);
					}
					if (nativeStatusCode2 > NativeStatusCode.Done)
					{
						this.getLatestVersionOperationId = -1L;
						result = false;
					}
					else if (nativeStatusCode == NativeStatusCode.Done && nativeStatusCode2 == NativeStatusCode.Done)
					{
						this.CheckPackmanOperation(this.getCurrentVersionOperationId, BuildPlayerWindow.PackmanOperationType.List);
						this.CheckPackmanOperation(this.getLatestVersionOperationId, BuildPlayerWindow.PackmanOperationType.Search);
						Console.WriteLine("Current xiaomi package version is " + ((!string.IsNullOrEmpty(this.currentXiaomiPackageVersion)) ? this.currentXiaomiPackageVersion : "empty"));
						Console.WriteLine("Latest xiaomi package version is " + ((!string.IsNullOrEmpty(this.latestXiaomiPackageVersion)) ? this.latestXiaomiPackageVersion : "empty"));
						this.isVersionInitialized = true;
						result = true;
					}
					else
					{
						result = false;
					}
				}
			}
			return result;
		}

		private void Update()
		{
			if (this.CheckXiaomiPackageVersions())
			{
				if (this.packmanOperationRunning)
				{
					this.packmanOperationRunning = !this.CheckPackmanOperation(this.packmanOperationId, this.packmanOperationType);
				}
			}
		}

		private bool CheckPackmanOperation(long operationId, BuildPlayerWindow.PackmanOperationType operationType)
		{
			NativeStatusCode operationStatus = NativeClient.GetOperationStatus(operationId);
			bool result;
			if (operationStatus == NativeStatusCode.NotFound)
			{
				Debug.LogError("OperationID " + operationId + " Not Found");
				result = true;
			}
			else if (operationStatus == NativeStatusCode.Error)
			{
				Error operationError = NativeClient.GetOperationError(operationId);
				Debug.LogError(string.Concat(new object[]
				{
					"OperationID ",
					operationId,
					" failed with Error: ",
					operationError
				}));
				result = true;
			}
			else if (operationStatus == NativeStatusCode.InProgress || operationStatus == NativeStatusCode.InQueue)
			{
				result = false;
			}
			else if (operationStatus == NativeStatusCode.Done)
			{
				Console.WriteLine("OperationID " + operationId + " Done");
				switch (operationType)
				{
				case BuildPlayerWindow.PackmanOperationType.List:
					this.ExtractCurrentXiaomiPackageInfo(operationId);
					break;
				case BuildPlayerWindow.PackmanOperationType.Add:
					this.currentXiaomiPackageVersion = this.latestXiaomiPackageVersion;
					this.xiaomiPackageInstalled = true;
					break;
				case BuildPlayerWindow.PackmanOperationType.Remove:
					this.currentXiaomiPackageVersion = "";
					this.xiaomiPackageInstalled = false;
					break;
				case BuildPlayerWindow.PackmanOperationType.Search:
					this.ExtractLatestXiaomiPackageInfo(operationId);
					break;
				default:
					Console.WriteLine("Type " + operationType + " Not Supported");
					break;
				}
				result = true;
			}
			else
			{
				result = true;
			}
			return result;
		}

		private void ExtractCurrentXiaomiPackageInfo(long operationId)
		{
			OperationStatus listOperationData = NativeClient.GetListOperationData(operationId);
			UnityEditor.PackageManager.PackageInfo[] packageList = listOperationData.packageList;
			for (int i = 0; i < packageList.Length; i++)
			{
				UnityEditor.PackageManager.PackageInfo packageInfo = packageList[i];
				if (packageInfo.packageId.StartsWith(this.xiaomiPackageName))
				{
					this.xiaomiPackageInstalled = true;
					this.currentXiaomiPackageVersion = packageInfo.version;
				}
			}
		}

		private void ExtractLatestXiaomiPackageInfo(long operationId)
		{
			UnityEditor.PackageManager.PackageInfo[] searchOperationData = NativeClient.GetSearchOperationData(operationId);
			UnityEditor.PackageManager.PackageInfo[] array = searchOperationData;
			for (int i = 0; i < array.Length; i++)
			{
				UnityEditor.PackageManager.PackageInfo packageInfo = array[i];
				this.latestXiaomiPackageVersion = packageInfo.version;
			}
		}
	}
}
