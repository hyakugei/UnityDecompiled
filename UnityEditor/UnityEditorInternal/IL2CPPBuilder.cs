using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Scripting;
using UnityEditor.Scripting.Compilers;
using UnityEngine;

namespace UnityEditorInternal
{
	internal class IL2CPPBuilder
	{
		private readonly string m_TempFolder;

		private readonly string m_StagingAreaData;

		private readonly IIl2CppPlatformProvider m_PlatformProvider;

		private readonly Action<string> m_ModifyOutputBeforeCompile;

		private readonly RuntimeClassRegistry m_RuntimeClassRegistry;

		private readonly LinkXmlReader m_linkXmlReader = new LinkXmlReader();

		private readonly bool m_BuildForMonoRuntime;

		public IL2CPPBuilder(string tempFolder, string stagingAreaData, IIl2CppPlatformProvider platformProvider, Action<string> modifyOutputBeforeCompile, RuntimeClassRegistry runtimeClassRegistry, bool buildForMonoRuntime)
		{
			this.m_TempFolder = tempFolder;
			this.m_StagingAreaData = stagingAreaData;
			this.m_PlatformProvider = platformProvider;
			this.m_ModifyOutputBeforeCompile = modifyOutputBeforeCompile;
			this.m_RuntimeClassRegistry = runtimeClassRegistry;
			this.m_BuildForMonoRuntime = buildForMonoRuntime;
		}

		public void Run()
		{
			string cppOutputDirectoryInStagingArea = this.GetCppOutputDirectoryInStagingArea();
			string fullPath = Path.GetFullPath(Path.Combine(this.m_StagingAreaData, "Managed"));
			string[] files = Directory.GetFiles(fullPath);
			for (int i = 0; i < files.Length; i++)
			{
				string fileName = files[i];
				FileInfo fileInfo = new FileInfo(fileName);
				fileInfo.IsReadOnly = false;
			}
			AssemblyStripper.StripAssemblies(fullPath, this.m_PlatformProvider, this.m_RuntimeClassRegistry);
			FileUtil.CreateOrCleanDirectory(cppOutputDirectoryInStagingArea);
			if (this.m_ModifyOutputBeforeCompile != null)
			{
				this.m_ModifyOutputBeforeCompile(cppOutputDirectoryInStagingArea);
			}
			this.ConvertPlayerDlltoCpp(this.GetUserAssembliesToConvert(fullPath), cppOutputDirectoryInStagingArea, fullPath);
			INativeCompiler nativeCompiler = this.m_PlatformProvider.CreateNativeCompiler();
			if (nativeCompiler != null && this.m_PlatformProvider.CreateIl2CppNativeCodeBuilder() == null)
			{
				string outFile = this.OutputFileRelativePath();
				List<string> list = new List<string>(this.m_PlatformProvider.includePaths);
				list.Add(cppOutputDirectoryInStagingArea);
				this.m_PlatformProvider.CreateNativeCompiler().CompileDynamicLibrary(outFile, NativeCompiler.AllSourceFilesIn(cppOutputDirectoryInStagingArea), list, this.m_PlatformProvider.libraryPaths, new string[0]);
			}
		}

		public void RunCompileAndLink()
		{
			Il2CppNativeCodeBuilder il2CppNativeCodeBuilder = this.m_PlatformProvider.CreateIl2CppNativeCodeBuilder();
			if (il2CppNativeCodeBuilder != null)
			{
				Il2CppNativeCodeBuilderUtils.ClearAndPrepareCacheDirectory(il2CppNativeCodeBuilder);
				BuildTargetGroup buildTargetGroup = BuildPipeline.GetBuildTargetGroup(this.m_PlatformProvider.target);
				bool debugBuild = PlayerSettings.GetIl2CppCompilerConfiguration(buildTargetGroup) == Il2CppCompilerConfiguration.Debug;
				List<string> list = Il2CppNativeCodeBuilderUtils.AddBuilderArguments(il2CppNativeCodeBuilder, this.OutputFileRelativePath(), this.m_PlatformProvider.includePaths, debugBuild).ToList<string>();
				list.Add(string.Format("--map-file-parser=\"{0}\"", IL2CPPBuilder.GetMapFileParserPath()));
				list.Add(string.Format("--generatedcppdir=\"{0}\"", Path.GetFullPath(this.GetCppOutputDirectoryInStagingArea())));
				if (PlayerSettings.GetApiCompatibilityLevel(buildTargetGroup) == ApiCompatibilityLevel.NET_4_6)
				{
					list.Add("--dotnetprofile=\"net45\"");
				}
				if (PlayerSettings.GetApiCompatibilityLevel(buildTargetGroup) == ApiCompatibilityLevel.NET_Standard_2_0)
				{
					list.Add("--dotnetprofile=\"unityaot\"");
				}
				Action<ProcessStartInfo> setupStartInfo = new Action<ProcessStartInfo>(il2CppNativeCodeBuilder.SetupStartInfo);
				string fullPath = Path.GetFullPath(Path.Combine(this.m_StagingAreaData, "Managed"));
				this.RunIl2CppWithArguments(list, setupStartInfo, fullPath);
			}
		}

		private string OutputFileRelativePath()
		{
			string text = Path.Combine(this.m_StagingAreaData, "Native");
			Directory.CreateDirectory(text);
			return Path.Combine(text, this.m_PlatformProvider.nativeLibraryFileName);
		}

		internal List<string> GetUserAssembliesToConvert(string managedDir)
		{
			HashSet<string> userAssemblies = this.GetUserAssemblies(managedDir);
			userAssemblies.Add(Directory.GetFiles(managedDir, "UnityEngine.dll", SearchOption.TopDirectoryOnly).Single<string>());
			userAssemblies.UnionWith(this.FilterUserAssemblies(Directory.GetFiles(managedDir, "*.dll", SearchOption.TopDirectoryOnly), new Predicate<string>(this.m_linkXmlReader.IsDLLUsed), managedDir));
			return userAssemblies.ToList<string>();
		}

		private HashSet<string> GetUserAssemblies(string managedDir)
		{
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.UnionWith(this.FilterUserAssemblies(this.m_RuntimeClassRegistry.GetUserAssemblies(), new Predicate<string>(this.m_RuntimeClassRegistry.IsDLLUsed), managedDir));
			hashSet.UnionWith(this.FilterUserAssemblies(Directory.GetFiles(managedDir, "I18N*.dll", SearchOption.TopDirectoryOnly), (string assembly) => true, managedDir));
			return hashSet;
		}

		private IEnumerable<string> FilterUserAssemblies(IEnumerable<string> assemblies, Predicate<string> isUsed, string managedDir)
		{
			return from assembly in assemblies
			where isUsed(assembly)
			select assembly into usedAssembly
			select Path.Combine(managedDir, usedAssembly);
		}

		public string GetCppOutputDirectoryInStagingArea()
		{
			return IL2CPPBuilder.GetCppOutputPath(this.m_TempFolder);
		}

		public static string GetCppOutputPath(string tempFolder)
		{
			return Path.Combine(tempFolder, "il2cppOutput");
		}

		public static string GetMapFileParserPath()
		{
			return Path.GetFullPath(Path.Combine(EditorApplication.applicationContentsPath, (Application.platform != RuntimePlatform.WindowsEditor) ? "Tools/MapFileParser/MapFileParser" : "Tools\\MapFileParser\\MapFileParser.exe"));
		}

		private void ConvertPlayerDlltoCpp(ICollection<string> userAssemblies, string outputDirectory, string workingDirectory)
		{
			if (userAssemblies.Count != 0)
			{
				List<string> list = new List<string>();
				list.Add("--convert-to-cpp");
				if (this.m_PlatformProvider.emitNullChecks)
				{
					list.Add("--emit-null-checks");
				}
				if (this.m_PlatformProvider.enableStackTraces)
				{
					list.Add("--enable-stacktrace");
				}
				if (this.m_PlatformProvider.enableArrayBoundsCheck)
				{
					list.Add("--enable-array-bounds-check");
				}
				if (this.m_PlatformProvider.enableDivideByZeroCheck)
				{
					list.Add("--enable-divide-by-zero-check");
				}
				if (this.m_PlatformProvider.developmentMode)
				{
					list.Add("--development-mode");
				}
				if (this.m_BuildForMonoRuntime)
				{
					list.Add("--mono-runtime");
				}
				BuildTargetGroup buildTargetGroup = BuildPipeline.GetBuildTargetGroup(this.m_PlatformProvider.target);
				if (PlayerSettings.GetApiCompatibilityLevel(buildTargetGroup) == ApiCompatibilityLevel.NET_4_6)
				{
					list.Add("--dotnetprofile=\"net45\"");
				}
				if (PlayerSettings.GetApiCompatibilityLevel(buildTargetGroup) == ApiCompatibilityLevel.NET_Standard_2_0)
				{
					list.Add("--dotnetprofile=\"unityaot\"");
				}
				Il2CppNativeCodeBuilder il2CppNativeCodeBuilder = this.m_PlatformProvider.CreateIl2CppNativeCodeBuilder();
				if (il2CppNativeCodeBuilder != null)
				{
					bool debugBuild = PlayerSettings.GetIl2CppCompilerConfiguration(buildTargetGroup) == Il2CppCompilerConfiguration.Debug;
					Il2CppNativeCodeBuilderUtils.ClearAndPrepareCacheDirectory(il2CppNativeCodeBuilder);
					list.AddRange(Il2CppNativeCodeBuilderUtils.AddBuilderArguments(il2CppNativeCodeBuilder, this.OutputFileRelativePath(), this.m_PlatformProvider.includePaths, debugBuild));
				}
				list.Add(string.Format("--map-file-parser=\"{0}\"", IL2CPPBuilder.GetMapFileParserPath()));
				string text = PlayerSettings.GetAdditionalIl2CppArgs();
				if (!string.IsNullOrEmpty(text))
				{
					list.Add(text);
				}
				text = Environment.GetEnvironmentVariable("IL2CPP_ADDITIONAL_ARGS");
				if (!string.IsNullOrEmpty(text))
				{
					list.Add(text);
				}
				List<string> source = new List<string>(userAssemblies);
				list.AddRange(from arg in source
				select "--assembly=\"" + Path.GetFullPath(arg) + "\"");
				list.Add(string.Format("--generatedcppdir=\"{0}\"", Path.GetFullPath(outputDirectory)));
				string info = "Converting managed assemblies to C++";
				if (il2CppNativeCodeBuilder != null)
				{
					info = "Building native binary with IL2CPP...";
				}
				if (EditorUtility.DisplayCancelableProgressBar("Building Player", info, 0.3f))
				{
					throw new OperationCanceledException();
				}
				Action<ProcessStartInfo> setupStartInfo = null;
				if (il2CppNativeCodeBuilder != null)
				{
					setupStartInfo = new Action<ProcessStartInfo>(il2CppNativeCodeBuilder.SetupStartInfo);
				}
				this.RunIl2CppWithArguments(list, setupStartInfo, workingDirectory);
			}
		}

		private void RunIl2CppWithArguments(List<string> arguments, Action<ProcessStartInfo> setupStartInfo, string workingDirectory)
		{
			string text = arguments.Aggregate(string.Empty, (string current, string arg) => current + arg + " ");
			bool flag = this.ShouldUseIl2CppCore();
			string exe = (!flag) ? this.GetIl2CppExe() : this.GetIl2CppCoreExe();
			Console.WriteLine("Invoking il2cpp with arguments: " + text);
			CompilerOutputParserBase compilerOutputParserBase = this.m_PlatformProvider.CreateIl2CppOutputParser();
			if (compilerOutputParserBase == null)
			{
				compilerOutputParserBase = new Il2CppOutputParser();
			}
			if (flag)
			{
				Runner.RunNetCoreProgram(exe, text, workingDirectory, compilerOutputParserBase, setupStartInfo);
			}
			else
			{
				Runner.RunManagedProgram(exe, text, workingDirectory, compilerOutputParserBase, setupStartInfo);
			}
		}

		private string GetIl2CppExe()
		{
			return this.m_PlatformProvider.il2CppFolder + "/build/il2cpp.exe";
		}

		private string GetIl2CppCoreExe()
		{
			return this.m_PlatformProvider.il2CppFolder + "/build/il2cppcore/il2cppcore.dll";
		}

		private bool ShouldUseIl2CppCore()
		{
			bool flag = false;
			if (Application.platform == RuntimePlatform.OSXEditor)
			{
				if (SystemInfo.operatingSystem.StartsWith("Mac OS X 10."))
				{
					string version = SystemInfo.operatingSystem.Substring(9);
					Version v = new Version(version);
					if (v >= new Version(10, 9))
					{
						flag = true;
					}
				}
				else
				{
					flag = true;
				}
			}
			return flag && NetCoreProgram.IsNetCoreAvailable();
		}
	}
}
