using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEditor;
using UnityEditor.Utils;

namespace UnityEditorInternal
{
	internal class AssemblyStripper
	{
		[CompilerGenerated]
		private static Func<string, bool> <>f__mg$cache0;

		private static bool debugUnstripped
		{
			get
			{
				return false;
			}
		}

		private static string[] Il2CppBlacklistPaths
		{
			get
			{
				return new string[]
				{
					Path.Combine("..", "platform_native_link.xml")
				};
			}
		}

		private static string MonoLinker2Path
		{
			get
			{
				return Path.Combine(MonoInstallationFinder.GetFrameWorksFolder(), "il2cpp/build/UnityLinker.exe");
			}
		}

		private static string GetModuleWhitelist(string module, string moduleStrippingInformationFolder)
		{
			return Paths.Combine(new string[]
			{
				moduleStrippingInformationFolder,
				module + ".xml"
			});
		}

		private static bool StripAssembliesTo(string[] assemblies, string[] searchDirs, string outputFolder, string workingDirectory, out string output, out string error, string linkerPath, IIl2CppPlatformProvider platformProvider, IEnumerable<string> additionalBlacklist)
		{
			if (!Directory.Exists(outputFolder))
			{
				Directory.CreateDirectory(outputFolder);
			}
			IEnumerable<string> arg_50_0 = from s in additionalBlacklist
			select (!Path.IsPathRooted(s)) ? Path.Combine(workingDirectory, s) : s;
			if (AssemblyStripper.<>f__mg$cache0 == null)
			{
				AssemblyStripper.<>f__mg$cache0 = new Func<string, bool>(File.Exists);
			}
			additionalBlacklist = arg_50_0.Where(AssemblyStripper.<>f__mg$cache0);
			IEnumerable<string> userBlacklistFiles = AssemblyStripper.GetUserBlacklistFiles();
			foreach (string current in userBlacklistFiles)
			{
				Console.WriteLine("UserBlackList: " + current);
			}
			additionalBlacklist = additionalBlacklist.Concat(userBlacklistFiles);
			List<string> list = new List<string>
			{
				"--api=" + PlayerSettings.GetApiCompatibilityLevel(EditorUserBuildSettings.activeBuildTargetGroup).ToString(),
				"-out=\"" + outputFolder + "\"",
				"-l=none",
				"-c=link",
				"--link-symbols",
				"-x=\"" + AssemblyStripper.GetModuleWhitelist("Core", platformProvider.moduleStrippingInformationFolder) + "\"",
				"-f=\"" + Path.Combine(platformProvider.il2CppFolder, "LinkerDescriptors") + "\""
			};
			list.AddRange(from path in additionalBlacklist
			select "-x \"" + path + "\"");
			list.AddRange(from d in searchDirs
			select "-d \"" + d + "\"");
			list.AddRange(from assembly in assemblies
			select "-a  \"" + Path.GetFullPath(assembly) + "\"");
			return AssemblyStripper.RunAssemblyLinker(list, out output, out error, linkerPath, workingDirectory);
		}

		private static bool RunAssemblyLinker(IEnumerable<string> args, out string @out, out string err, string linkerPath, string workingDirectory)
		{
			string text = args.Aggregate((string buff, string s) => buff + " " + s);
			Console.WriteLine("Invoking UnityLinker with arguments: " + text);
			Runner.RunManagedProgram(linkerPath, text, workingDirectory, null, null);
			@out = "";
			err = "";
			return true;
		}

		private static List<string> GetUserAssemblies(RuntimeClassRegistry rcr, string managedDir)
		{
			return (from s in rcr.GetUserAssemblies()
			where rcr.IsDLLUsed(s)
			select Path.Combine(managedDir, s)).ToList<string>();
		}

		internal static void StripAssemblies(string managedAssemblyFolderPath, IIl2CppPlatformProvider platformProvider, RuntimeClassRegistry rcr)
		{
			List<string> userAssemblies = AssemblyStripper.GetUserAssemblies(rcr, managedAssemblyFolderPath);
			userAssemblies.AddRange(Directory.GetFiles(managedAssemblyFolderPath, "I18N*.dll", SearchOption.TopDirectoryOnly));
			string[] assembliesToStrip = userAssemblies.ToArray();
			string[] searchDirs = new string[]
			{
				managedAssemblyFolderPath
			};
			AssemblyStripper.RunAssemblyStripper(userAssemblies, managedAssemblyFolderPath, assembliesToStrip, searchDirs, AssemblyStripper.MonoLinker2Path, platformProvider, rcr);
		}

		internal static void GenerateInternalCallSummaryFile(string icallSummaryPath, string managedAssemblyFolderPath, string strippedDLLPath)
		{
			string exe = Path.Combine(MonoInstallationFinder.GetFrameWorksFolder(), "Tools/InternalCallRegistrationWriter/InternalCallRegistrationWriter.exe");
			IEnumerable<string> source = Directory.GetFiles(strippedDLLPath, "UnityEngine.*Module.dll").Concat(new string[]
			{
				Path.Combine(strippedDLLPath, "UnityEngine.dll")
			});
			string args = string.Format("-output=\"{0}\" -summary=\"{1}\" -assembly=\"{2}\"", Path.Combine(managedAssemblyFolderPath, "UnityICallRegistration.cpp"), icallSummaryPath, source.Aggregate((string dllArg, string next) => dllArg + ";" + next));
			Runner.RunManagedProgram(exe, args);
		}

		internal static IEnumerable<string> GetUserBlacklistFiles()
		{
			return from s in Directory.GetFiles("Assets", "link.xml", SearchOption.AllDirectories)
			select Path.Combine(Directory.GetCurrentDirectory(), s);
		}

		private static bool AddWhiteListsForModules(IEnumerable<string> nativeModules, ref IEnumerable<string> blacklists, string moduleStrippingInformationFolder)
		{
			bool result = false;
			foreach (string current in nativeModules)
			{
				string moduleWhitelist = AssemblyStripper.GetModuleWhitelist(current, moduleStrippingInformationFolder);
				if (File.Exists(moduleWhitelist))
				{
					if (!blacklists.Contains(moduleWhitelist))
					{
						blacklists = blacklists.Concat(new string[]
						{
							moduleWhitelist
						});
						result = true;
					}
				}
			}
			return result;
		}

		private static void RunAssemblyStripper(IEnumerable assemblies, string managedAssemblyFolderPath, string[] assembliesToStrip, string[] searchDirs, string monoLinkerPath, IIl2CppPlatformProvider platformProvider, RuntimeClassRegistry rcr)
		{
			BuildTargetGroup buildTargetGroup = BuildPipeline.GetBuildTargetGroup(platformProvider.target);
			bool flag = PlayerSettings.GetScriptingBackend(buildTargetGroup) == ScriptingImplementation.Mono2x;
			bool flag2 = rcr != null && PlayerSettings.stripEngineCode && platformProvider.supportsEngineStripping;
			IEnumerable<string> enumerable = AssemblyStripper.Il2CppBlacklistPaths;
			if (rcr != null)
			{
				enumerable = enumerable.Concat(new string[]
				{
					AssemblyStripper.WriteMethodsToPreserveBlackList(rcr, platformProvider.target),
					AssemblyStripper.WriteUnityEngineBlackList(),
					MonoAssemblyStripping.GenerateLinkXmlToPreserveDerivedTypes(managedAssemblyFolderPath, rcr)
				});
			}
			if (PlayerSettingsEditor.IsLatestApiCompatibility(PlayerSettings.GetApiCompatibilityLevel(buildTargetGroup)))
			{
				string path = Path.Combine(platformProvider.il2CppFolder, "LinkerDescriptors");
				enumerable = enumerable.Concat(Directory.GetFiles(path, "*45.xml"));
			}
			if (flag)
			{
				string path2 = Path.Combine(platformProvider.il2CppFolder, "LinkerDescriptors");
				enumerable = enumerable.Concat(Directory.GetFiles(path2, "*_mono.xml"));
				string text = Path.Combine(BuildPipeline.GetBuildToolsDirectory(platformProvider.target), "link.xml");
				if (File.Exists(text))
				{
					enumerable = enumerable.Concat(new string[]
					{
						text
					});
				}
			}
			if (!flag2)
			{
				string[] files = Directory.GetFiles(platformProvider.moduleStrippingInformationFolder, "*.xml");
				for (int i = 0; i < files.Length; i++)
				{
					string text2 = files[i];
					enumerable = enumerable.Concat(new string[]
					{
						text2
					});
				}
			}
			string fullPath = Path.GetFullPath(Path.Combine(managedAssemblyFolderPath, "tempStrip"));
			string text4;
			while (true)
			{
				bool flag3 = false;
				if (EditorUtility.DisplayCancelableProgressBar("Building Player", "Stripping assemblies", 0f))
				{
					break;
				}
				string text3;
				if (!AssemblyStripper.StripAssembliesTo(assembliesToStrip, searchDirs, fullPath, managedAssemblyFolderPath, out text3, out text4, monoLinkerPath, platformProvider, enumerable))
				{
					goto Block_9;
				}
				if (platformProvider.supportsEngineStripping)
				{
					string text5 = Path.Combine(managedAssemblyFolderPath, "ICallSummary.txt");
					AssemblyStripper.GenerateInternalCallSummaryFile(text5, managedAssemblyFolderPath, fullPath);
					if (flag2)
					{
						HashSet<UnityType> hashSet;
						HashSet<string> nativeModules;
						CodeStrippingUtils.GenerateDependencies(fullPath, text5, rcr, flag2, out hashSet, out nativeModules, platformProvider);
						flag3 = AssemblyStripper.AddWhiteListsForModules(nativeModules, ref enumerable, platformProvider.moduleStrippingInformationFolder);
					}
				}
				if (!flag3)
				{
					goto Block_12;
				}
			}
			throw new OperationCanceledException();
			Block_9:
			throw new Exception(string.Concat(new object[]
			{
				"Error in stripping assemblies: ",
				assemblies,
				", ",
				text4
			}));
			Block_12:
			string fullPath2 = Path.GetFullPath(Path.Combine(managedAssemblyFolderPath, "tempUnstripped"));
			if (AssemblyStripper.debugUnstripped)
			{
				Directory.CreateDirectory(fullPath2);
			}
			string[] files2 = Directory.GetFiles(managedAssemblyFolderPath);
			for (int j = 0; j < files2.Length; j++)
			{
				string text6 = files2[j];
				string extension = Path.GetExtension(text6);
				if (string.Equals(extension, ".dll", StringComparison.InvariantCultureIgnoreCase) || string.Equals(extension, ".winmd", StringComparison.InvariantCultureIgnoreCase) || string.Equals(extension, ".mdb", StringComparison.InvariantCultureIgnoreCase) || string.Equals(extension, ".pdb", StringComparison.InvariantCultureIgnoreCase))
				{
					if (AssemblyStripper.debugUnstripped)
					{
						File.Move(text6, Path.Combine(fullPath2, Path.GetFileName(text6)));
					}
					else
					{
						File.Delete(text6);
					}
				}
			}
			string[] files3 = Directory.GetFiles(fullPath);
			for (int k = 0; k < files3.Length; k++)
			{
				string text7 = files3[k];
				File.Move(text7, Path.Combine(managedAssemblyFolderPath, Path.GetFileName(text7)));
			}
			Directory.Delete(fullPath);
		}

		private static string WriteMethodsToPreserveBlackList(RuntimeClassRegistry rcr, BuildTarget target)
		{
			string tempFileName = Path.GetTempFileName();
			File.WriteAllText(tempFileName, AssemblyStripper.GetMethodPreserveBlacklistContents(rcr, target));
			return tempFileName;
		}

		private static string WriteUnityEngineBlackList()
		{
			string tempFileName = Path.GetTempFileName();
			File.WriteAllText(tempFileName, "<linker><assembly fullname=\"UnityEngine\" preserve=\"nothing\"/></linker>");
			return tempFileName;
		}

		private static string GetMethodPreserveBlacklistContents(RuntimeClassRegistry rcr, BuildTarget target)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("<linker>");
			IEnumerable<IGrouping<string, RuntimeClassRegistry.MethodDescription>> enumerable = from m in rcr.GetMethodsToPreserve()
			group m by m.assembly;
			foreach (IGrouping<string, RuntimeClassRegistry.MethodDescription> current in enumerable)
			{
				string key = current.Key;
				stringBuilder.AppendLine(string.Format("\t<assembly fullname=\"{0}\">", key));
				IEnumerable<IGrouping<string, RuntimeClassRegistry.MethodDescription>> enumerable2 = from m in current
				group m by m.fullTypeName;
				foreach (IGrouping<string, RuntimeClassRegistry.MethodDescription> current2 in enumerable2)
				{
					stringBuilder.AppendLine(string.Format("\t\t<type fullname=\"{0}\">", current2.Key));
					foreach (RuntimeClassRegistry.MethodDescription current3 in current2)
					{
						stringBuilder.AppendLine(string.Format("\t\t\t<method name=\"{0}\"/>", current3.methodName));
					}
					stringBuilder.AppendLine("\t\t</type>");
				}
				stringBuilder.AppendLine("\t</assembly>");
			}
			stringBuilder.AppendLine("</linker>");
			return stringBuilder.ToString();
		}

		public static void InvokeFromBuildPlayer(BuildTarget buildTarget, RuntimeClassRegistry usedClasses)
		{
			string path = Paths.Combine(new string[]
			{
				"Temp",
				"StagingArea",
				"Data"
			});
			BaseIl2CppPlatformProvider platformProvider = new BaseIl2CppPlatformProvider(buildTarget, Path.Combine(path, "Libraries"));
			string fullPath = Path.GetFullPath(Path.Combine(path, "Managed"));
			AssemblyStripper.StripAssemblies(fullPath, platformProvider, usedClasses);
		}
	}
}
