using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEditor.Build.Reporting;
using UnityEditor.DeploymentTargets;
using UnityEditor.Modules;
using UnityEditor.Utils;
using UnityEngine;
using UnityEngine.Scripting;

namespace UnityEditor
{
	internal static class PostprocessBuildPlayer
	{
		private class NoTargetsFoundException : Exception
		{
			public NoTargetsFoundException()
			{
			}

			public NoTargetsFoundException(string message) : base(message)
			{
			}
		}

		internal const string StreamingAssets = "Assets/StreamingAssets";

		public static string subDir32Bit
		{
			get
			{
				return "x86";
			}
		}

		public static string subDir64Bit
		{
			get
			{
				return "x86_64";
			}
		}

		internal static bool InstallPluginsByExtension(string pluginSourceFolder, string extension, string debugExtension, string destPluginFolder, bool copyDirectories)
		{
			bool flag = false;
			bool result;
			if (!Directory.Exists(pluginSourceFolder))
			{
				result = flag;
			}
			else
			{
				string[] fileSystemEntries = Directory.GetFileSystemEntries(pluginSourceFolder);
				string[] array = fileSystemEntries;
				for (int i = 0; i < array.Length; i++)
				{
					string text = array[i];
					string fileName = Path.GetFileName(text);
					string extension2 = Path.GetExtension(text);
					bool flag2 = extension2.Equals(extension, StringComparison.OrdinalIgnoreCase) || fileName.Equals(extension, StringComparison.OrdinalIgnoreCase);
					bool flag3 = debugExtension != null && debugExtension.Length != 0 && (extension2.Equals(debugExtension, StringComparison.OrdinalIgnoreCase) || fileName.Equals(debugExtension, StringComparison.OrdinalIgnoreCase));
					if (flag2 || flag3)
					{
						if (!Directory.Exists(destPluginFolder))
						{
							Directory.CreateDirectory(destPluginFolder);
						}
						string text2 = Path.Combine(destPluginFolder, fileName);
						if (copyDirectories)
						{
							FileUtil.CopyDirectoryRecursive(text, text2);
						}
						else if (!Directory.Exists(text))
						{
							FileUtil.UnityFileCopy(text, text2);
						}
						flag = true;
					}
				}
				result = flag;
			}
			return result;
		}

		internal static void InstallStreamingAssets(string stagingAreaDataPath)
		{
			PostprocessBuildPlayer.InstallStreamingAssets(stagingAreaDataPath, null);
		}

		internal static void InstallStreamingAssets(string stagingAreaDataPath, BuildReport report)
		{
			if (Directory.Exists("Assets/StreamingAssets"))
			{
				string text = Path.Combine(stagingAreaDataPath, "StreamingAssets");
				FileUtil.CopyDirectoryRecursiveForPostprocess("Assets/StreamingAssets", text, true);
				if (report != null)
				{
					report.RecordFilesAddedRecursive(text, CommonRoles.streamingAsset);
				}
			}
		}

		public static string PrepareForBuild(BuildOptions options, BuildTargetGroup targetGroup, BuildTarget target)
		{
			IBuildPostprocessor buildPostProcessor = ModuleManager.GetBuildPostProcessor(targetGroup, target);
			string result;
			if (buildPostProcessor == null)
			{
				result = null;
			}
			else
			{
				result = buildPostProcessor.PrepareForBuild(options, target);
			}
			return result;
		}

		public static bool SupportsScriptsOnlyBuild(BuildTargetGroup targetGroup, BuildTarget target)
		{
			IBuildPostprocessor buildPostProcessor = ModuleManager.GetBuildPostProcessor(targetGroup, target);
			return buildPostProcessor != null && buildPostProcessor.SupportsScriptsOnlyBuild();
		}

		public static string GetExtensionForBuildTarget(BuildTargetGroup targetGroup, BuildTarget target, BuildOptions options)
		{
			IBuildPostprocessor buildPostProcessor = ModuleManager.GetBuildPostProcessor(targetGroup, target);
			string result;
			if (buildPostProcessor == null)
			{
				result = string.Empty;
			}
			else
			{
				result = buildPostProcessor.GetExtension(target, options);
			}
			return result;
		}

		public static bool SupportsInstallInBuildFolder(BuildTargetGroup targetGroup, BuildTarget target)
		{
			IBuildPostprocessor buildPostProcessor = ModuleManager.GetBuildPostProcessor(targetGroup, target);
			bool result;
			if (buildPostProcessor != null)
			{
				result = buildPostProcessor.SupportsInstallInBuildFolder();
			}
			else
			{
				result = (target == BuildTarget.Android || target == BuildTarget.WSAPlayer || target == BuildTarget.PSP2);
			}
			return result;
		}

		public static bool SupportsLz4Compression(BuildTargetGroup targetGroup, BuildTarget target)
		{
			IBuildPostprocessor buildPostProcessor = ModuleManager.GetBuildPostProcessor(targetGroup, target);
			return buildPostProcessor != null && buildPostProcessor.SupportsLz4Compression();
		}

		public static void Launch(BuildTargetGroup targetGroup, BuildTarget buildTarget, string path, string productName, BuildOptions options, BuildReport buildReport)
		{
			IBuildPostprocessor buildPostProcessor = ModuleManager.GetBuildPostProcessor(targetGroup, buildTarget);
			if (buildPostProcessor != null)
			{
				BuildLaunchPlayerArgs args;
				args.target = buildTarget;
				args.playerPackage = BuildPipeline.GetPlaybackEngineDirectory(buildTarget, options);
				args.installPath = path;
				args.productName = productName;
				args.options = options;
				args.report = buildReport;
				buildPostProcessor.LaunchPlayer(args);
				return;
			}
			throw new UnityException(string.Format("Launching for target group {0}, build target {1} is not supported: There is no build post-processor available.", targetGroup, buildTarget));
		}

		public static void LaunchOnTargets(BuildTargetGroup targetGroup, BuildTarget buildTarget, BuildReport buildReport, List<DeploymentTargetId> launchTargets)
		{
			try
			{
				if (buildReport == null || !DeploymentTargetManager.IsExtensionSupported(targetGroup, buildReport.summary.platform))
				{
					throw new NotSupportedException();
				}
				ProgressHandler handler = new ProgressHandler("Deploying Player", delegate(string title, string message, float globalProgress)
				{
					if (EditorUtility.DisplayCancelableProgressBar(title, message, globalProgress))
					{
						throw new DeploymentOperationAbortedException();
					}
				}, 0.1f, 1f);
				ProgressTaskManager taskManager = new ProgressTaskManager(handler);
				taskManager.AddTask(delegate
				{
					int num = 0;
					List<DeploymentOperationFailedException> list = new List<DeploymentOperationFailedException>();
					foreach (DeploymentTargetId current in launchTargets)
					{
						try
						{
							DeploymentTargetManager.LaunchBuildOnTarget(targetGroup, buildReport, current, taskManager.SpawnProgressHandlerFromCurrentTask());
							num++;
						}
						catch (DeploymentOperationFailedException item)
						{
							list.Add(item);
						}
					}
					foreach (DeploymentOperationFailedException current2 in list)
					{
						UnityEngine.Debug.LogException(current2);
					}
					if (num == 0)
					{
						throw new PostprocessBuildPlayer.NoTargetsFoundException("Could not launch build");
					}
				});
				taskManager.Run();
			}
			catch (DeploymentOperationFailedException ex)
			{
				UnityEngine.Debug.LogException(ex);
				EditorUtility.DisplayDialog(ex.title, ex.Message, "Ok");
			}
			catch (DeploymentOperationAbortedException)
			{
				Console.WriteLine("Deployment aborted");
			}
			catch (PostprocessBuildPlayer.NoTargetsFoundException)
			{
				throw new UnityException(string.Format("Could not find any valid targets to launch on for {0}", buildTarget));
			}
		}

		[RequiredByNativeCode]
		public static void UpdateBootConfig(BuildTargetGroup targetGroup, BuildTarget target, BootConfigData config, BuildOptions options)
		{
			IBuildPostprocessor buildPostProcessor = ModuleManager.GetBuildPostProcessor(targetGroup, target);
			if (buildPostProcessor != null)
			{
				buildPostProcessor.UpdateBootConfig(target, config, options);
			}
		}

		public static void Postprocess(BuildTargetGroup targetGroup, BuildTarget target, string installPath, string companyName, string productName, int width, int height, BuildOptions options, RuntimeClassRegistry usedClassRegistry, BuildReport report)
		{
			string stagingArea = "Temp/StagingArea";
			string stagingAreaData = "Temp/StagingArea/Data";
			string stagingAreaDataManaged = "Temp/StagingArea/Data/Managed";
			string playbackEngineDirectory = BuildPipeline.GetPlaybackEngineDirectory(target, options);
			bool flag = (options & BuildOptions.InstallInBuildFolder) != BuildOptions.None && PostprocessBuildPlayer.SupportsInstallInBuildFolder(targetGroup, target);
			if (installPath == string.Empty && !flag)
			{
				throw new Exception(installPath + " must not be an empty string");
			}
			IBuildPostprocessor buildPostProcessor = ModuleManager.GetBuildPostProcessor(targetGroup, target);
			if (buildPostProcessor != null)
			{
				BuildPostProcessArgs args;
				args.target = target;
				args.stagingAreaData = stagingAreaData;
				args.stagingArea = stagingArea;
				args.stagingAreaDataManaged = stagingAreaDataManaged;
				args.playerPackage = playbackEngineDirectory;
				args.installPath = installPath;
				args.companyName = companyName;
				args.productName = productName;
				args.productGUID = PlayerSettings.productGUID;
				args.options = options;
				args.usedClassRegistry = usedClassRegistry;
				args.report = report;
				BuildProperties obj;
				buildPostProcessor.PostProcess(args, out obj);
				report.AddAppendix(obj);
				return;
			}
			throw new UnityException(string.Format("Build target '{0}' not supported", target));
		}

		internal static string ExecuteSystemProcess(string command, string args, string workingdir)
		{
			ProcessStartInfo si = new ProcessStartInfo
			{
				FileName = command,
				Arguments = args,
				WorkingDirectory = workingdir,
				CreateNoWindow = true
			};
			Program program = new Program(si);
			program.Start();
			while (!program.WaitForExit(100))
			{
			}
			string standardOutputAsString = program.GetStandardOutputAsString();
			program.Dispose();
			return standardOutputAsString;
		}
	}
}
