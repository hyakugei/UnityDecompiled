using System;
using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.Modules;
using UnityEditor.Utils;
using UnityEditorInternal;
using UnityEngine;

internal abstract class DesktopStandalonePostProcessor : DefaultBuildPostprocessor
{
	internal class ScriptingImplementations : IScriptingImplementations
	{
		public ScriptingImplementation[] Supported()
		{
			return new ScriptingImplementation[]
			{
				ScriptingImplementation.Mono2x,
				ScriptingImplementation.IL2CPP
			};
		}

		public ScriptingImplementation[] Enabled()
		{
			return new ScriptingImplementation[]
			{
				ScriptingImplementation.Mono2x,
				ScriptingImplementation.IL2CPP
			};
		}
	}

	protected bool UseIl2Cpp
	{
		get
		{
			return PlayerSettings.GetScriptingBackend(BuildTargetGroup.Standalone) == ScriptingImplementation.IL2CPP;
		}
	}

	public override void PostProcess(BuildPostProcessArgs args)
	{
		this.SetupStagingArea(args);
		this.CopyStagingAreaIntoDestination(args);
	}

	public override bool SupportsLz4Compression()
	{
		return true;
	}

	public override void UpdateBootConfig(BuildTarget target, BootConfigData config, BuildOptions options)
	{
		base.UpdateBootConfig(target, config, options);
		if (PlayerSettings.forceSingleInstance)
		{
			config.AddKey("single-instance");
		}
		if (EditorApplication.scriptingRuntimeVersion == ScriptingRuntimeVersion.Latest)
		{
			config.Set("scripting-runtime-version", "latest");
		}
		if (IL2CPPUtils.UseIl2CppCodegenWithMonoBackend(BuildPipeline.GetBuildTargetGroup(target)))
		{
			config.Set("mono-codegen", "il2cpp");
		}
	}

	private void CopyNativePlugins(BuildPostProcessArgs args)
	{
		string buildTargetName = BuildPipeline.GetBuildTargetName(args.target);
		IPluginImporterExtension pluginImporterExtension = new DesktopPluginImporterExtension();
		string stagingAreaPluginsFolder = this.GetStagingAreaPluginsFolder(args);
		string path = Path.Combine(stagingAreaPluginsFolder, "x86");
		string path2 = Path.Combine(stagingAreaPluginsFolder, "x86_64");
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		PluginImporter[] importers = PluginImporter.GetImporters(args.target);
		for (int i = 0; i < importers.Length; i++)
		{
			PluginImporter pluginImporter = importers[i];
			BuildTarget target = args.target;
			if (pluginImporter.isNativePlugin)
			{
				if (string.IsNullOrEmpty(pluginImporter.assetPath))
				{
					Debug.LogWarning("Got empty plugin importer path for " + args.target.ToString());
				}
				else
				{
					if (!flag)
					{
						Directory.CreateDirectory(stagingAreaPluginsFolder);
						flag = true;
					}
					bool flag4 = Directory.Exists(pluginImporter.assetPath);
					string platformData = pluginImporter.GetPlatformData(target, "CPU");
					if (platformData != null)
					{
						if (!(platformData == "x86"))
						{
							if (!(platformData == "x86_64"))
							{
								if (platformData == "None")
								{
									goto IL_1E9;
								}
							}
							else
							{
								if (target != BuildTarget.StandaloneOSX && target != BuildTarget.StandaloneWindows64 && target != BuildTarget.StandaloneLinux64 && target != BuildTarget.StandaloneLinuxUniversal)
								{
									goto IL_1E9;
								}
								if (!flag3)
								{
									Directory.CreateDirectory(path2);
									flag3 = true;
								}
							}
						}
						else
						{
							if (target == BuildTarget.StandaloneWindows64 || target == BuildTarget.StandaloneLinux64)
							{
								goto IL_1E9;
							}
							if (!flag2)
							{
								Directory.CreateDirectory(path);
								flag2 = true;
							}
						}
					}
					string text = pluginImporterExtension.CalculateFinalPluginPath(buildTargetName, pluginImporter);
					if (!string.IsNullOrEmpty(text))
					{
						string text2 = Path.Combine(stagingAreaPluginsFolder, text);
						if (flag4)
						{
							FileUtil.CopyDirectoryRecursive(pluginImporter.assetPath, text2);
						}
						else
						{
							FileUtil.UnityFileCopy(pluginImporter.assetPath, text2);
						}
					}
				}
			}
			IL_1E9:;
		}
		foreach (PluginDesc current in PluginImporter.GetExtensionPlugins(args.target))
		{
			if (!flag)
			{
				Directory.CreateDirectory(stagingAreaPluginsFolder);
				flag = true;
			}
			string text3 = Path.Combine(stagingAreaPluginsFolder, Path.GetFileName(current.pluginPath));
			if (!Directory.Exists(text3) && !File.Exists(text3))
			{
				if (Directory.Exists(current.pluginPath))
				{
					FileUtil.CopyDirectoryRecursive(current.pluginPath, text3);
				}
				else
				{
					FileUtil.CopyFileIfExists(current.pluginPath, text3, false);
				}
			}
		}
	}

	private void SetupStagingArea(BuildPostProcessArgs args)
	{
		if (this.UseIl2Cpp && this.GetCreateSolution())
		{
			throw new Exception("CreateSolution is not supported with IL2CPP build");
		}
		Directory.CreateDirectory(args.stagingAreaData);
		this.CopyNativePlugins(args);
		if (args.target == BuildTarget.StandaloneWindows || args.target == BuildTarget.StandaloneWindows64)
		{
			this.CreateApplicationData(args);
		}
		PostprocessBuildPlayer.InstallStreamingAssets(args.stagingAreaData, args.report);
		if (this.UseIl2Cpp)
		{
			this.CopyVariationFolderIntoStagingArea(args);
			IL2CPPUtils.RunIl2Cpp(args.stagingAreaData, this.GetPlatformProvider(args), null, args.usedClassRegistry);
			string text = Path.Combine(args.stagingAreaData, "Native");
			string directoryForGameAssembly = this.GetDirectoryForGameAssembly(args);
			string[] files = Directory.GetFiles(text);
			for (int i = 0; i < files.Length; i++)
			{
				string text2 = files[i];
				string fileName = Path.GetFileName(text2);
				if (!fileName.StartsWith("."))
				{
					FileUtil.MoveFileOrDirectory(text2, Path.Combine(directoryForGameAssembly, fileName));
				}
			}
			if (this.PlaceIL2CPPSymbolMapNextToExecutable())
			{
				FileUtil.MoveFileOrDirectory(Paths.Combine(new string[]
				{
					text,
					"Data",
					"SymbolMap"
				}), Path.Combine(args.stagingArea, "SymbolMap"));
			}
			FileUtil.MoveFileOrDirectory(Path.Combine(text, "Data"), Path.Combine(args.stagingAreaData, "il2cpp_data"));
			FileUtil.DeleteFileOrDirectory(text);
			string text3 = Path.Combine(args.stagingArea, this.GetIl2CppDataBackupFolderName(args));
			FileUtil.CreateOrCleanDirectory(text3);
			FileUtil.MoveFileOrDirectory(Path.Combine(args.stagingAreaData, "il2cppOutput"), Path.Combine(text3, "il2cppOutput"));
			if (IL2CPPUtils.UseIl2CppCodegenWithMonoBackend(BuildPipeline.GetBuildTargetGroup(args.target)))
			{
				DesktopStandalonePostProcessor.StripAssembliesToLeaveOnlyMetadata(args.target, args.stagingAreaDataManaged);
			}
			else
			{
				FileUtil.MoveFileOrDirectory(args.stagingAreaDataManaged, Path.Combine(text3, "Managed"));
			}
			this.ProcessPlatformSpecificIL2CPPOutput(args);
		}
		if (this.GetInstallingIntoBuildsFolder(args))
		{
			this.CopyDataForBuildsFolder(args);
		}
		else
		{
			if (!this.UseIl2Cpp)
			{
				this.CopyVariationFolderIntoStagingArea(args);
			}
			if (this.GetCreateSolution())
			{
				this.CopyPlayerSolutionIntoStagingArea(args);
			}
			this.RenameFilesInStagingArea(args);
		}
	}

	private static void StripAssembliesToLeaveOnlyMetadata(BuildTarget target, string stagingAreaDataManaged)
	{
		AssemblyReferenceChecker assemblyReferenceChecker = new AssemblyReferenceChecker();
		assemblyReferenceChecker.CollectReferences(stagingAreaDataManaged, true, 0f, false);
		EditorUtility.DisplayProgressBar("Removing bytecode from assemblies", "Stripping assemblies so that only metadata remains", 0.95f);
		MonoAssemblyStripping.MonoCilStrip(target, stagingAreaDataManaged, assemblyReferenceChecker.GetAssemblyFileNames());
	}

	protected void CreateApplicationData(BuildPostProcessArgs args)
	{
		File.WriteAllText(Path.Combine(args.stagingAreaData, "app.info"), string.Join("\n", new string[]
		{
			args.companyName,
			args.productName
		}));
		args.report.RecordFileAdded(Path.Combine(args.stagingAreaData, "app.info"), CommonRoles.appInfo);
	}

	protected bool CopyPlayerFilter(string path, BuildPostProcessArgs args)
	{
		return Path.GetExtension(path) != ".mdb" || !Path.GetFileName(path).StartsWith("UnityEngine.");
	}

	protected virtual void CopyPlayerSolutionIntoStagingArea(BuildPostProcessArgs args)
	{
		throw new Exception("CreateSolution is not supported on " + BuildPipeline.GetBuildTargetName(args.target));
	}

	protected virtual void CopyVariationFolderIntoStagingArea(BuildPostProcessArgs args)
	{
		string text = args.playerPackage + "/Variations/" + this.GetVariationName(args);
		FileUtil.CopyDirectoryFiltered(text, args.stagingArea, true, (string f) => this.CopyPlayerFilter(f, args), true);
		string[] files = Directory.GetFiles(Path.Combine(text, "Data/Managed"), "*.dll");
		for (int i = 0; i < files.Length; i++)
		{
			string path = files[i];
			string fileName = Path.GetFileName(path);
			if (fileName.StartsWith("UnityEngine"))
			{
				string path2 = Path.Combine(args.stagingArea, "Data/Managed/" + fileName);
				args.report.RecordFileAdded(path2, CommonRoles.managedEngineApi);
			}
		}
		args.report.RecordFileAdded(Path.Combine(args.stagingArea, "Data/Resources/unity default resources"), CommonRoles.builtInResources);
		string[] array = new string[]
		{
			"Mono",
			"MonoBleedingEdge"
		};
		for (int j = 0; j < array.Length; j++)
		{
			string str = array[j];
			args.report.RecordFilesAddedRecursive(Path.Combine(args.stagingArea, "Data/" + str + "/EmbedRuntime"), CommonRoles.monoRuntime);
			args.report.RecordFilesAddedRecursive(Path.Combine(args.stagingArea, "Data/" + str + "/etc"), CommonRoles.monoConfig);
		}
	}

	private void CopyStagingAreaIntoDestination(BuildPostProcessArgs args)
	{
		if (this.GetInstallingIntoBuildsFolder(args))
		{
			string text = Unsupported.GetBaseUnityDeveloperFolder() + "/" + this.GetDestinationFolderForInstallingIntoBuildsFolder(args);
			if (!Directory.Exists(Path.GetDirectoryName(text)))
			{
				throw new Exception("Installing in builds folder failed because the player has not been built (You most likely want to enable 'Development build').");
			}
			FileUtil.CopyDirectoryFiltered(args.stagingAreaData, text, true, (string f) => true, true);
		}
		else
		{
			if (!this.GetCreateSolution())
			{
				this.DeleteDestination(args);
			}
			FileUtil.CopyDirectoryFiltered(args.stagingArea, this.GetDestinationFolder(args), true, (string f) => true, true);
			args.report.RecordFilesMoved(args.stagingArea, this.GetDestinationFolder(args));
		}
	}

	protected abstract string GetStagingAreaPluginsFolder(BuildPostProcessArgs args);

	protected abstract void DeleteDestination(BuildPostProcessArgs args);

	protected static string GetMonoFolderName(ScriptingRuntimeVersion scriptingRuntimeVersion)
	{
		string result;
		if (scriptingRuntimeVersion != ScriptingRuntimeVersion.Legacy)
		{
			if (scriptingRuntimeVersion != ScriptingRuntimeVersion.Latest)
			{
				throw new ArgumentOutOfRangeException("scriptingRuntimeVersion", "Unknown scripting runtime version");
			}
			result = "MonoBleedingEdge";
		}
		else
		{
			result = "Mono";
		}
		return result;
	}

	protected void DeleteUnusedMono(string dataFolder, BuildReport report)
	{
		bool flag = IL2CPPUtils.UseIl2CppCodegenWithMonoBackend(BuildTargetGroup.Standalone);
		if (flag || EditorApplication.scriptingRuntimeVersion == ScriptingRuntimeVersion.Latest)
		{
			string text = Path.Combine(dataFolder, DesktopStandalonePostProcessor.GetMonoFolderName(ScriptingRuntimeVersion.Legacy));
			FileUtil.DeleteFileOrDirectory(text);
			report.RecordFilesDeletedRecursive(text);
		}
		if (flag || EditorApplication.scriptingRuntimeVersion == ScriptingRuntimeVersion.Legacy)
		{
			string text2 = Path.Combine(dataFolder, DesktopStandalonePostProcessor.GetMonoFolderName(ScriptingRuntimeVersion.Latest));
			FileUtil.DeleteFileOrDirectory(text2);
			report.RecordFilesDeletedRecursive(text2);
		}
	}

	protected abstract string GetDestinationFolderForInstallingIntoBuildsFolder(BuildPostProcessArgs args);

	protected abstract void CopyDataForBuildsFolder(BuildPostProcessArgs args);

	protected bool GetInstallingIntoBuildsFolder(BuildPostProcessArgs args)
	{
		return (args.options & BuildOptions.InstallInBuildFolder) != BuildOptions.None;
	}

	protected virtual bool GetCreateSolution()
	{
		return false;
	}

	protected virtual string GetDestinationFolder(BuildPostProcessArgs args)
	{
		return FileUtil.UnityGetDirectoryName(args.installPath);
	}

	protected bool GetDevelopment(BuildPostProcessArgs args)
	{
		return (args.options & BuildOptions.Development) != BuildOptions.None;
	}

	protected virtual string GetVariationName(BuildPostProcessArgs args)
	{
		return string.Format("{0}_{1}", this.PlatformStringFor(args.target), (!this.GetDevelopment(args)) ? "nondevelopment" : "development");
	}

	protected abstract string PlatformStringFor(BuildTarget target);

	protected abstract void RenameFilesInStagingArea(BuildPostProcessArgs args);

	protected abstract IIl2CppPlatformProvider GetPlatformProvider(BuildPostProcessArgs args);

	protected virtual void ProcessPlatformSpecificIL2CPPOutput(BuildPostProcessArgs args)
	{
	}

	protected string GetIl2CppDataBackupFolderName(BuildPostProcessArgs args)
	{
		return Path.GetFileNameWithoutExtension(args.installPath) + "_BackUpThisFolder_ButDontShipItWithYourGame";
	}

	protected virtual string GetDirectoryForGameAssembly(BuildPostProcessArgs args)
	{
		return args.stagingArea;
	}

	protected virtual bool PlaceIL2CPPSymbolMapNextToExecutable()
	{
		return true;
	}
}
