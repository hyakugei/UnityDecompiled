using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor.Analytics;
using UnityEditor.Build.Reporting;
using UnityEditor.Scripting.ScriptCompilation;
using UnityEditor.Utils;
using UnityEditorInternal;
using UnityEditorInternal.VR;

namespace UnityEditor
{
	internal class CodeStrippingUtils
	{
		private static UnityType s_GameManagerTypeInfo = null;

		private static string[] s_blackListNativeClassNames = new string[]
		{
			"Behaviour",
			"PreloadData",
			"Material",
			"Cubemap",
			"Texture3D",
			"Texture2DArray",
			"RenderTexture",
			"Mesh",
			"MeshFilter",
			"MeshRenderer",
			"Sprite",
			"LowerResBlitTexture",
			"Transform",
			"RectTransform"
		};

		private static UnityType[] s_blackListNativeClasses;

		private static readonly Dictionary<string, string> s_blackListNativeClassesDependencyNames = new Dictionary<string, string>
		{
			{
				"ParticleSystemRenderer",
				"ParticleSystem"
			}
		};

		private static Dictionary<UnityType, UnityType> s_blackListNativeClassesDependency;

		private static readonly string[] s_TreatedAsUserAssemblies = new string[]
		{
			"UnityEngine.Analytics.dll"
		};

		private static UnityType GameManagerTypeInfo
		{
			get
			{
				if (CodeStrippingUtils.s_GameManagerTypeInfo == null)
				{
					CodeStrippingUtils.s_GameManagerTypeInfo = CodeStrippingUtils.FindTypeByNameChecked("GameManager", "initializing code stripping utils");
				}
				return CodeStrippingUtils.s_GameManagerTypeInfo;
			}
		}

		public static UnityType[] BlackListNativeClasses
		{
			get
			{
				if (CodeStrippingUtils.s_blackListNativeClasses == null)
				{
					CodeStrippingUtils.s_blackListNativeClasses = (from typeName in CodeStrippingUtils.s_blackListNativeClassNames
					select CodeStrippingUtils.FindTypeByNameChecked(typeName, "code stripping blacklist native class")).ToArray<UnityType>();
				}
				return CodeStrippingUtils.s_blackListNativeClasses;
			}
		}

		public static Dictionary<UnityType, UnityType> BlackListNativeClassesDependency
		{
			get
			{
				if (CodeStrippingUtils.s_blackListNativeClassesDependency == null)
				{
					CodeStrippingUtils.s_blackListNativeClassesDependency = new Dictionary<UnityType, UnityType>();
					foreach (KeyValuePair<string, string> current in CodeStrippingUtils.s_blackListNativeClassesDependencyNames)
					{
						CodeStrippingUtils.BlackListNativeClassesDependency.Add(CodeStrippingUtils.FindTypeByNameChecked(current.Key, "code stripping blacklist native class dependency key"), CodeStrippingUtils.FindTypeByNameChecked(current.Value, "code stripping blacklist native class dependency value"));
					}
				}
				return CodeStrippingUtils.s_blackListNativeClassesDependency;
			}
		}

		public static string[] UserAssemblies
		{
			get
			{
				EditorCompilation.TargetAssemblyInfo[] targetAssemblies = EditorCompilationInterface.GetTargetAssemblies();
				string[] array = new string[targetAssemblies.Length + CodeStrippingUtils.s_TreatedAsUserAssemblies.Length];
				for (int i = 0; i < targetAssemblies.Length; i++)
				{
					array[i] = targetAssemblies[i].Name;
				}
				for (int j = 0; j < CodeStrippingUtils.s_TreatedAsUserAssemblies.Length; j++)
				{
					array[targetAssemblies.Length + j] = CodeStrippingUtils.s_TreatedAsUserAssemblies[j];
				}
				return array;
			}
		}

		private static UnityType FindTypeByNameChecked(string name, string msg)
		{
			UnityType unityType = UnityType.FindTypeByName(name);
			if (unityType == null)
			{
				throw new ArgumentException(string.Format("Could not map typename '{0}' to type info ({1})", name, msg ?? "no context"));
			}
			return unityType;
		}

		public static HashSet<string> GetModulesFromICalls(string icallsListFile)
		{
			string[] array = File.ReadAllLines(icallsListFile);
			HashSet<string> hashSet = new HashSet<string>();
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string icall = array2[i];
				string iCallModule = ModuleMetadata.GetICallModule(icall);
				if (!string.IsNullOrEmpty(iCallModule))
				{
					hashSet.Add(iCallModule);
				}
			}
			return hashSet;
		}

		public static void InjectCustomDependencies(BuildTarget target, StrippingInfo strippingInfo, HashSet<UnityType> nativeClasses, HashSet<string> nativeModules)
		{
			UnityType item = UnityType.FindTypeByName("UnityConnectSettings");
			UnityType item2 = UnityType.FindTypeByName("CloudWebServicesManager");
			if (nativeClasses.Contains(item) || nativeClasses.Contains(item2))
			{
				if (PlayerSettings.submitAnalytics)
				{
					strippingInfo.RegisterDependency("UnityConnectSettings", "Required by HW Statistics (See Player Settings)");
					strippingInfo.RegisterDependency("CloudWebServicesManager", "Required by HW Statistics (See Player Settings)");
					strippingInfo.SetIcon("Required by HW Statistics (See Player Settings)", "class/PlayerSettings");
				}
			}
			UnityType item3 = UnityType.FindTypeByName("UnityAnalyticsManager");
			if (nativeClasses.Contains(item3))
			{
				if (AnalyticsSettings.enabled)
				{
					strippingInfo.RegisterDependency("UnityAnalyticsManager", "Required by Unity Analytics (See Services Window)");
					strippingInfo.SetIcon("Required by Unity Analytics (See Services Window)", "class/PlayerSettings");
				}
			}
			if (VRModule.ShouldInjectVRDependenciesForBuildTarget(target))
			{
				nativeModules.Add("VR");
				strippingInfo.RegisterDependency(StrippingInfo.ModuleName("VR"), "Required because VR is enabled in PlayerSettings");
				strippingInfo.SetIcon("Required because VR is enabled in PlayerSettings", "class/PlayerSettings");
			}
			string[] moduleNames = ModuleMetadata.GetModuleNames();
			for (int i = 0; i < moduleNames.Length; i++)
			{
				string text = moduleNames[i];
				if (!ModuleMetadata.IsStrippableModule(text))
				{
					string text2 = text + " is always required";
					nativeModules.Add(text);
					strippingInfo.AddModule(text, true);
					strippingInfo.RegisterDependency(StrippingInfo.ModuleName(text), text2);
					strippingInfo.SetIcon(text2, "class/DefaultAsset");
				}
			}
		}

		public static void GenerateDependencies(string strippedAssemblyDir, string icallsListFile, RuntimeClassRegistry rcr, bool doStripping, out HashSet<UnityType> nativeClasses, out HashSet<string> nativeModules, IIl2CppPlatformProvider platformProvider)
		{
			StrippingInfo strippingInfo = (platformProvider != null) ? StrippingInfo.GetBuildReportData(platformProvider.buildReport) : null;
			string[] userAssemblies = CodeStrippingUtils.GetUserAssemblies(strippedAssemblyDir);
			nativeClasses = ((!doStripping) ? null : CodeStrippingUtils.GenerateNativeClassList(rcr, strippedAssemblyDir, userAssemblies, strippingInfo));
			if (nativeClasses != null)
			{
				CodeStrippingUtils.ExcludeModuleManagers(ref nativeClasses);
			}
			nativeModules = CodeStrippingUtils.GetNativeModulesToRegister(nativeClasses, strippingInfo);
			if (nativeClasses != null && icallsListFile != null)
			{
				HashSet<string> modulesFromICalls = CodeStrippingUtils.GetModulesFromICalls(icallsListFile);
				foreach (string current in modulesFromICalls)
				{
					if (!nativeModules.Contains(current))
					{
						if (strippingInfo != null)
						{
							strippingInfo.RegisterDependency(StrippingInfo.ModuleName(current), "Required by Scripts");
						}
					}
					UnityType[] moduleTypes = ModuleMetadata.GetModuleTypes(current);
					UnityType[] array = moduleTypes;
					for (int i = 0; i < array.Length; i++)
					{
						UnityType unityType = array[i];
						if (unityType.IsDerivedFrom(CodeStrippingUtils.GameManagerTypeInfo))
						{
							nativeClasses.Add(unityType);
						}
					}
				}
				nativeModules.UnionWith(modulesFromICalls);
			}
			CodeStrippingUtils.ApplyManualStrippingOverrides(nativeClasses, nativeModules, strippingInfo);
			bool flag = true;
			if (platformProvider != null)
			{
				while (flag)
				{
					flag = false;
					foreach (string current2 in nativeModules.ToList<string>())
					{
						string[] moduleDependencies = ModuleMetadata.GetModuleDependencies(current2);
						if (moduleDependencies != null)
						{
							string[] array2 = moduleDependencies;
							for (int j = 0; j < array2.Length; j++)
							{
								string text = array2[j];
								if (!nativeModules.Contains(text))
								{
									nativeModules.Add(text);
									flag = true;
								}
								if (strippingInfo != null)
								{
									string str = StrippingInfo.ModuleName(current2);
									strippingInfo.RegisterDependency(StrippingInfo.ModuleName(text), "Required by " + str);
									strippingInfo.SetIcon("Required by " + str, string.Format("package/com.unity.modules.{0}", current2.ToLower()));
								}
							}
						}
					}
				}
			}
			AssemblyReferenceChecker assemblyReferenceChecker = new AssemblyReferenceChecker();
			assemblyReferenceChecker.CollectReferencesFromRoots(strippedAssemblyDir, userAssemblies, true, 0f, true);
			if (strippingInfo != null)
			{
				foreach (string current3 in nativeModules)
				{
					strippingInfo.AddModule(current3, true);
				}
				strippingInfo.AddModule("Core", true);
			}
			if (nativeClasses != null && strippingInfo != null)
			{
				CodeStrippingUtils.InjectCustomDependencies(platformProvider.target, strippingInfo, nativeClasses, nativeModules);
			}
		}

		public static void ApplyManualStrippingOverrides(HashSet<UnityType> nativeClasses, HashSet<string> nativeModules, StrippingInfo strippingInfo)
		{
			string[] moduleNames = ModuleMetadata.GetModuleNames();
			for (int i = 0; i < moduleNames.Length; i++)
			{
				string text = moduleNames[i];
				ModuleIncludeSetting moduleIncludeSettingForModule = ModuleMetadata.GetModuleIncludeSettingForModule(text);
				if (moduleIncludeSettingForModule == ModuleIncludeSetting.ForceInclude)
				{
					nativeModules.Add(text);
					UnityType[] moduleTypes = ModuleMetadata.GetModuleTypes(text);
					UnityType[] array = moduleTypes;
					for (int j = 0; j < array.Length; j++)
					{
						UnityType unityType = array[j];
						nativeClasses.Add(unityType);
						if (strippingInfo != null)
						{
							strippingInfo.RegisterDependency(unityType.name, "Force included module");
							strippingInfo.RegisterDependency(StrippingInfo.ModuleName(text), unityType.name);
						}
					}
					if (strippingInfo != null)
					{
						strippingInfo.RegisterDependency(StrippingInfo.ModuleName(text), "Force included module");
					}
				}
				else if (moduleIncludeSettingForModule == ModuleIncludeSetting.ForceExclude)
				{
					if (nativeModules.Contains(text))
					{
						nativeModules.Remove(text);
						UnityType[] moduleTypes2 = ModuleMetadata.GetModuleTypes(text);
						UnityType[] array2 = moduleTypes2;
						for (int k = 0; k < array2.Length; k++)
						{
							UnityType item = array2[k];
							if (nativeClasses.Contains(item))
							{
								nativeClasses.Remove(item);
							}
						}
						if (strippingInfo != null)
						{
							strippingInfo.modules.Remove(StrippingInfo.ModuleName(text));
						}
					}
				}
			}
		}

		public static string GetModuleWhitelist(string module, string moduleStrippingInformationFolder)
		{
			return Paths.Combine(new string[]
			{
				moduleStrippingInformationFolder,
				module + ".xml"
			});
		}

		public static void WriteModuleAndClassRegistrationFile(string strippedAssemblyDir, string icallsListFile, string outputDir, RuntimeClassRegistry rcr, IEnumerable<UnityType> classesToSkip, IIl2CppPlatformProvider platformProvider)
		{
			bool stripEngineCode = PlayerSettings.stripEngineCode;
			HashSet<UnityType> nativeClasses;
			HashSet<string> nativeModules;
			CodeStrippingUtils.GenerateDependencies(strippedAssemblyDir, icallsListFile, rcr, stripEngineCode, out nativeClasses, out nativeModules, platformProvider);
			string file = Path.Combine(outputDir, "UnityClassRegistration.cpp");
			CodeStrippingUtils.WriteModuleAndClassRegistrationFile(file, nativeModules, nativeClasses, new HashSet<UnityType>(classesToSkip));
		}

		public static HashSet<string> GetNativeModulesToRegister(HashSet<UnityType> nativeClasses, StrippingInfo strippingInfo)
		{
			return (nativeClasses != null) ? CodeStrippingUtils.GetRequiredStrippableModules(nativeClasses, strippingInfo) : CodeStrippingUtils.GetAllStrippableModules();
		}

		private static HashSet<string> GetAllStrippableModules()
		{
			HashSet<string> hashSet = new HashSet<string>();
			string[] moduleNames = ModuleMetadata.GetModuleNames();
			for (int i = 0; i < moduleNames.Length; i++)
			{
				string text = moduleNames[i];
				if (ModuleMetadata.IsStrippableModule(text))
				{
					hashSet.Add(text);
				}
			}
			return hashSet;
		}

		private static HashSet<string> GetRequiredStrippableModules(HashSet<UnityType> nativeClasses, StrippingInfo strippingInfo)
		{
			HashSet<UnityType> hashSet = new HashSet<UnityType>();
			HashSet<string> hashSet2 = new HashSet<string>();
			string[] moduleNames = ModuleMetadata.GetModuleNames();
			for (int i = 0; i < moduleNames.Length; i++)
			{
				string text = moduleNames[i];
				if (ModuleMetadata.IsStrippableModule(text))
				{
					HashSet<UnityType> hashSet3 = new HashSet<UnityType>(ModuleMetadata.GetModuleTypes(text));
					if (nativeClasses.Overlaps(hashSet3))
					{
						hashSet2.Add(text);
						if (strippingInfo != null)
						{
							foreach (UnityType current in hashSet3)
							{
								if (nativeClasses.Contains(current))
								{
									strippingInfo.RegisterDependency(StrippingInfo.ModuleName(text), current.name);
									hashSet.Add(current);
								}
							}
						}
					}
				}
			}
			if (strippingInfo != null)
			{
				foreach (UnityType current2 in nativeClasses)
				{
					if (!hashSet.Contains(current2))
					{
						strippingInfo.RegisterDependency(StrippingInfo.ModuleName("Core"), current2.name);
					}
				}
			}
			return hashSet2;
		}

		private static void ExcludeModuleManagers(ref HashSet<UnityType> nativeClasses)
		{
			string[] moduleNames = ModuleMetadata.GetModuleNames();
			string[] array = moduleNames;
			for (int i = 0; i < array.Length; i++)
			{
				string moduleName = array[i];
				if (ModuleMetadata.IsStrippableModule(moduleName))
				{
					UnityType[] moduleTypes = ModuleMetadata.GetModuleTypes(moduleName);
					HashSet<UnityType> hashSet = new HashSet<UnityType>();
					HashSet<UnityType> hashSet2 = new HashSet<UnityType>();
					UnityType[] array2 = moduleTypes;
					for (int j = 0; j < array2.Length; j++)
					{
						UnityType unityType = array2[j];
						if (unityType.IsDerivedFrom(CodeStrippingUtils.GameManagerTypeInfo))
						{
							hashSet.Add(unityType);
						}
						else
						{
							hashSet2.Add(unityType);
						}
					}
					if (hashSet2.Count != 0)
					{
						if (!nativeClasses.Overlaps(hashSet2))
						{
							foreach (UnityType current in hashSet)
							{
								nativeClasses.Remove(current);
							}
						}
						else
						{
							foreach (UnityType current2 in hashSet)
							{
								nativeClasses.Add(current2);
							}
						}
					}
				}
			}
		}

		private static HashSet<UnityType> GenerateNativeClassList(RuntimeClassRegistry rcr, string directory, string[] rootAssemblies, StrippingInfo strippingInfo)
		{
			HashSet<UnityType> hashSet = CodeStrippingUtils.CollectNativeClassListFromRoots(directory, rootAssemblies, strippingInfo);
			UnityType[] blackListNativeClasses = CodeStrippingUtils.BlackListNativeClasses;
			for (int i = 0; i < blackListNativeClasses.Length; i++)
			{
				UnityType item = blackListNativeClasses[i];
				hashSet.Add(item);
			}
			foreach (UnityType current in CodeStrippingUtils.BlackListNativeClassesDependency.Keys)
			{
				if (hashSet.Contains(current))
				{
					UnityType item2 = CodeStrippingUtils.BlackListNativeClassesDependency[current];
					hashSet.Add(item2);
				}
			}
			foreach (string current2 in rcr.GetAllNativeClassesIncludingManagersAsString())
			{
				UnityType unityType = UnityType.FindTypeByName(current2);
				if (unityType != null && unityType.baseClass != null)
				{
					hashSet.Add(unityType);
					if (strippingInfo != null)
					{
						if (!unityType.IsDerivedFrom(CodeStrippingUtils.GameManagerTypeInfo))
						{
							List<string> scenesForClass = rcr.GetScenesForClass(unityType.persistentTypeID);
							if (scenesForClass != null)
							{
								foreach (string current3 in scenesForClass)
								{
									strippingInfo.RegisterDependency(current2, current3);
									if (current3.EndsWith(".unity"))
									{
										strippingInfo.SetIcon(current3, "class/SceneAsset");
									}
									else
									{
										strippingInfo.SetIcon(current3, "class/AssetBundle");
									}
								}
							}
						}
					}
				}
			}
			HashSet<UnityType> hashSet2 = new HashSet<UnityType>();
			foreach (UnityType current4 in hashSet)
			{
				UnityType unityType2 = current4;
				while (unityType2.baseClass != null)
				{
					hashSet2.Add(unityType2);
					unityType2 = unityType2.baseClass;
				}
			}
			return hashSet2;
		}

		private static HashSet<UnityType> CollectNativeClassListFromRoots(string directory, string[] rootAssemblies, StrippingInfo strippingInfo)
		{
			HashSet<string> source = CodeStrippingUtils.CollectManagedTypeReferencesFromRoots(directory, rootAssemblies, strippingInfo);
			IEnumerable<UnityType> collection = from name in source
			select UnityType.FindTypeByName(name) into klass
			where klass != null && klass.baseClass != null
			select klass;
			return new HashSet<UnityType>(collection);
		}

		private static HashSet<string> CollectManagedTypeReferencesFromRoots(string directory, string[] rootAssemblies, StrippingInfo strippingInfo)
		{
			HashSet<string> hashSet = new HashSet<string>();
			AssemblyReferenceChecker assemblyReferenceChecker = new AssemblyReferenceChecker();
			bool collectMethods = false;
			bool ignoreSystemDlls = false;
			assemblyReferenceChecker.CollectReferencesFromRoots(directory, rootAssemblies, collectMethods, 0f, ignoreSystemDlls);
			string[] assemblyFileNames = assemblyReferenceChecker.GetAssemblyFileNames();
			AssemblyDefinition[] assemblyDefinitions = assemblyReferenceChecker.GetAssemblyDefinitions();
			AssemblyDefinition[] array = assemblyDefinitions;
			for (int i = 0; i < array.Length; i++)
			{
				AssemblyDefinition assemblyDefinition = array[i];
				foreach (TypeDefinition current in assemblyDefinition.MainModule.Types)
				{
					if (current.Namespace.StartsWith("UnityEngine"))
					{
						if (current.Fields.Count > 0 || current.Methods.Count > 0 || current.Properties.Count > 0)
						{
							string name = current.Name;
							hashSet.Add(name);
							if (strippingInfo != null)
							{
								if (!AssemblyReferenceChecker.IsIgnoredSystemDll(assemblyDefinition))
								{
									strippingInfo.RegisterDependency(name, "Required by Scripts");
								}
							}
						}
					}
				}
			}
			AssemblyDefinition assemblyDefinition2 = null;
			AssemblyDefinition assemblyDefinition3 = null;
			for (int j = 0; j < assemblyFileNames.Length; j++)
			{
				if (assemblyFileNames[j] == "UnityEngine.dll")
				{
					assemblyDefinition2 = assemblyDefinitions[j];
				}
				if (assemblyFileNames[j] == "UnityEngine.UI.dll")
				{
					assemblyDefinition3 = assemblyDefinitions[j];
				}
			}
			AssemblyDefinition[] array2 = assemblyDefinitions;
			for (int k = 0; k < array2.Length; k++)
			{
				AssemblyDefinition assemblyDefinition4 = array2[k];
				if (assemblyDefinition4 != assemblyDefinition2 && assemblyDefinition4 != assemblyDefinition3)
				{
					foreach (TypeReference current2 in assemblyDefinition4.MainModule.GetTypeReferences())
					{
						if (current2.Namespace.StartsWith("UnityEngine"))
						{
							string name2 = current2.Name;
							hashSet.Add(name2);
							if (strippingInfo != null)
							{
								if (!AssemblyReferenceChecker.IsIgnoredSystemDll(assemblyDefinition4))
								{
									strippingInfo.RegisterDependency(name2, "Required by Scripts");
								}
							}
						}
					}
				}
			}
			return hashSet;
		}

		private static void WriteStaticallyLinkedModuleRegistration(TextWriter w, HashSet<string> nativeModules, HashSet<UnityType> nativeClasses)
		{
			w.WriteLine("void InvokeRegisterStaticallyLinkedModuleClasses()");
			w.WriteLine("{");
			if (nativeClasses == null)
			{
				w.WriteLine("\tvoid RegisterStaticallyLinkedModuleClasses();");
				w.WriteLine("\tRegisterStaticallyLinkedModuleClasses();");
			}
			else
			{
				w.WriteLine("\t// Do nothing (we're in stripping mode)");
			}
			w.WriteLine("}");
			w.WriteLine();
			w.WriteLine("void RegisterStaticallyLinkedModulesGranular()");
			w.WriteLine("{");
			foreach (string current in nativeModules)
			{
				w.WriteLine("\tvoid RegisterModule_" + current + "();");
				w.WriteLine("\tRegisterModule_" + current + "();");
				w.WriteLine();
			}
			w.WriteLine("}");
		}

		private static void WriteModuleAndClassRegistrationFile(string file, HashSet<string> nativeModules, HashSet<UnityType> nativeClasses, HashSet<UnityType> classesToSkip)
		{
			using (TextWriter textWriter = new StreamWriter(file))
			{
				textWriter.WriteLine("template <typename T> void RegisterClass(const char*);");
				textWriter.WriteLine("template <typename T> void RegisterStrippedType(int, const char*, const char*);");
				textWriter.WriteLine();
				CodeStrippingUtils.WriteStaticallyLinkedModuleRegistration(textWriter, nativeModules, nativeClasses);
				textWriter.WriteLine();
				if (nativeClasses != null)
				{
					foreach (UnityType current in UnityType.GetTypes())
					{
						if (current.baseClass != null && !current.isEditorOnly && !classesToSkip.Contains(current))
						{
							if (current.hasNativeNamespace)
							{
								textWriter.Write("namespace {0} {{ class {1}; }} ", current.nativeNamespace, current.name);
							}
							else
							{
								textWriter.Write("class {0}; ", current.name);
							}
							if (nativeClasses.Contains(current))
							{
								textWriter.WriteLine("template <> void RegisterClass<{0}>(const char*);", current.qualifiedName);
							}
							else
							{
								textWriter.WriteLine();
							}
						}
					}
					textWriter.WriteLine();
				}
				textWriter.WriteLine("void RegisterAllClasses()");
				textWriter.WriteLine("{");
				if (nativeClasses == null)
				{
					textWriter.WriteLine("\tvoid RegisterAllClassesGranular();");
					textWriter.WriteLine("\tRegisterAllClassesGranular();");
				}
				else
				{
					textWriter.WriteLine("void RegisterBuiltinTypes();");
					textWriter.WriteLine("RegisterBuiltinTypes();");
					textWriter.WriteLine("\t//Total: {0} non stripped classes", nativeClasses.Count);
					int num = 0;
					foreach (UnityType current2 in nativeClasses)
					{
						textWriter.WriteLine("\t//{0}. {1}", num, current2.qualifiedName);
						if (classesToSkip.Contains(current2))
						{
							textWriter.WriteLine("\t//Skipping {0}", current2.qualifiedName);
						}
						else
						{
							textWriter.WriteLine("\tRegisterClass<{0}>(\"{1}\");", current2.qualifiedName, current2.module);
						}
						num++;
					}
					textWriter.WriteLine();
				}
				textWriter.WriteLine("}");
				textWriter.Close();
			}
		}

		private static string[] GetUserAssemblies(string strippedAssemblyDir)
		{
			List<string> list = new List<string>();
			string[] userAssemblies = CodeStrippingUtils.UserAssemblies;
			for (int i = 0; i < userAssemblies.Length; i++)
			{
				string searchPattern = userAssemblies[i];
				string[] files = Directory.GetFiles(strippedAssemblyDir, searchPattern, SearchOption.TopDirectoryOnly);
				list.AddRange(from f in files
				select Path.GetFileName(f));
			}
			return list.ToArray();
		}
	}
}
