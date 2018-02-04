using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Utils;

namespace UnityEditorInternal
{
	internal class IL2CPPUtils
	{
		public const string BinaryMetadataSuffix = "-metadata.dat";

		internal static string editorIl2cppFolder
		{
			get
			{
				string path = "il2cpp";
				return Path.GetFullPath(Path.Combine(EditorApplication.applicationContentsPath, path));
			}
		}

		internal static IIl2CppPlatformProvider PlatformProviderForNotModularPlatform(BuildTarget target, bool developmentBuild)
		{
			throw new Exception("Platform unsupported, or already modular.");
		}

		internal static IL2CPPBuilder RunIl2Cpp(string tempFolder, string stagingAreaData, IIl2CppPlatformProvider platformProvider, Action<string> modifyOutputBeforeCompile, RuntimeClassRegistry runtimeClassRegistry)
		{
			IL2CPPBuilder iL2CPPBuilder = new IL2CPPBuilder(tempFolder, stagingAreaData, platformProvider, modifyOutputBeforeCompile, runtimeClassRegistry, IL2CPPUtils.UseIl2CppCodegenWithMonoBackend(BuildPipeline.GetBuildTargetGroup(platformProvider.target)));
			iL2CPPBuilder.Run();
			return iL2CPPBuilder;
		}

		internal static IL2CPPBuilder RunIl2Cpp(string stagingAreaData, IIl2CppPlatformProvider platformProvider, Action<string> modifyOutputBeforeCompile, RuntimeClassRegistry runtimeClassRegistry)
		{
			IL2CPPBuilder iL2CPPBuilder = new IL2CPPBuilder(stagingAreaData, stagingAreaData, platformProvider, modifyOutputBeforeCompile, runtimeClassRegistry, IL2CPPUtils.UseIl2CppCodegenWithMonoBackend(BuildPipeline.GetBuildTargetGroup(platformProvider.target)));
			iL2CPPBuilder.Run();
			return iL2CPPBuilder;
		}

		internal static IL2CPPBuilder RunCompileAndLink(string tempFolder, string stagingAreaData, IIl2CppPlatformProvider platformProvider, Action<string> modifyOutputBeforeCompile, RuntimeClassRegistry runtimeClassRegistry)
		{
			IL2CPPBuilder iL2CPPBuilder = new IL2CPPBuilder(tempFolder, stagingAreaData, platformProvider, modifyOutputBeforeCompile, runtimeClassRegistry, IL2CPPUtils.UseIl2CppCodegenWithMonoBackend(BuildPipeline.GetBuildTargetGroup(platformProvider.target)));
			iL2CPPBuilder.RunCompileAndLink();
			return iL2CPPBuilder;
		}

		internal static void CopyEmbeddedResourceFiles(string tempFolder, string destinationFolder)
		{
			foreach (string current in from f in Directory.GetFiles(Paths.Combine(new string[]
			{
				IL2CPPBuilder.GetCppOutputPath(tempFolder),
				"Data",
				"Resources"
			}))
			where f.EndsWith("-resources.dat")
			select f)
			{
				File.Copy(current, Paths.Combine(new string[]
				{
					destinationFolder,
					Path.GetFileName(current)
				}), true);
			}
		}

		internal static void CopySymmapFile(string tempFolder, string destinationFolder)
		{
			IL2CPPUtils.CopySymmapFile(tempFolder, destinationFolder, string.Empty);
		}

		internal static void CopySymmapFile(string tempFolder, string destinationFolder, string destinationFileNameSuffix)
		{
			string text = Path.Combine(tempFolder, "SymbolMap");
			if (File.Exists(text))
			{
				File.Copy(text, Path.Combine(destinationFolder, "SymbolMap" + destinationFileNameSuffix), true);
			}
		}

		internal static void CopyMetadataFiles(string tempFolder, string destinationFolder)
		{
			foreach (string current in from f in Directory.GetFiles(Paths.Combine(new string[]
			{
				IL2CPPBuilder.GetCppOutputPath(tempFolder),
				"Data",
				"Metadata"
			}))
			where f.EndsWith("-metadata.dat")
			select f)
			{
				File.Copy(current, Paths.Combine(new string[]
				{
					destinationFolder,
					Path.GetFileName(current)
				}), true);
			}
		}

		internal static void CopyConfigFiles(string tempFolder, string destinationFolder)
		{
			string source = Paths.Combine(new string[]
			{
				IL2CPPBuilder.GetCppOutputPath(tempFolder),
				"Data",
				"etc"
			});
			FileUtil.CopyDirectoryRecursive(source, destinationFolder);
		}

		internal static string ApiCompatibilityLevelToDotNetProfileArgument(ApiCompatibilityLevel compatibilityLevel)
		{
			switch (compatibilityLevel)
			{
			case ApiCompatibilityLevel.NET_2_0:
			{
				string result = "net20";
				return result;
			}
			case ApiCompatibilityLevel.NET_2_0_Subset:
			{
				string result = "legacyunity";
				return result;
			}
			case ApiCompatibilityLevel.NET_4_6:
			{
				string result = "net45";
				return result;
			}
			case ApiCompatibilityLevel.NET_Standard_2_0:
			{
				string result = "unityaot";
				return result;
			}
			}
			throw new NotSupportedException(string.Format("ApiCompatibilityLevel.{0} is not supported by IL2CPP!", compatibilityLevel));
		}

		internal static bool UseIl2CppCodegenWithMonoBackend(BuildTargetGroup targetGroup)
		{
			return EditorApplication.scriptingRuntimeVersion == ScriptingRuntimeVersion.Latest && EditorApplication.useLibmonoBackendForIl2cpp && PlayerSettings.GetScriptingBackend(targetGroup) == ScriptingImplementation.IL2CPP;
		}
	}
}
