using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using UnityEditor.Compilation;
using UnityEditor.Modules;
using UnityEditor.Scripting;
using UnityEditor.Scripting.Compilers;
using UnityEditor.Scripting.ScriptCompilation;
using UnityEditor.Utils;
using UnityEditorInternal;

namespace UnityEditor.VisualStudioIntegration
{
	internal class SolutionSynchronizer
	{
		private enum Mode
		{
			UnityScriptAsUnityProj,
			UnityScriptAsPrecompiledAssembly
		}

		public static readonly ISolutionSynchronizationSettings DefaultSynchronizationSettings = new DefaultSolutionSynchronizationSettings();

		private static readonly string WindowsNewline = "\r\n";

		internal static readonly Dictionary<string, ScriptingLanguage> BuiltinSupportedExtensions = new Dictionary<string, ScriptingLanguage>
		{
			{
				"cs",
				ScriptingLanguage.CSharp
			},
			{
				"js",
				ScriptingLanguage.UnityScript
			},
			{
				"boo",
				ScriptingLanguage.Boo
			},
			{
				"uxml",
				ScriptingLanguage.None
			},
			{
				"uss",
				ScriptingLanguage.None
			},
			{
				"shader",
				ScriptingLanguage.None
			},
			{
				"compute",
				ScriptingLanguage.None
			},
			{
				"cginc",
				ScriptingLanguage.None
			},
			{
				"hlsl",
				ScriptingLanguage.None
			},
			{
				"glslinc",
				ScriptingLanguage.None
			}
		};

		private string[] ProjectSupportedExtensions = new string[0];

		private static readonly Dictionary<ScriptingLanguage, string> ProjectExtensions = new Dictionary<ScriptingLanguage, string>
		{
			{
				ScriptingLanguage.Boo,
				".booproj"
			},
			{
				ScriptingLanguage.CSharp,
				".csproj"
			},
			{
				ScriptingLanguage.UnityScript,
				".unityproj"
			},
			{
				ScriptingLanguage.None,
				".csproj"
			}
		};

		private static readonly Regex _MonoDevelopPropertyHeader = new Regex("^\\s*GlobalSection\\(MonoDevelopProperties.*\\)");

		public static readonly string MSBuildNamespaceUri = "http://schemas.microsoft.com/developer/msbuild/2003";

		private readonly string _projectDirectory;

		private readonly ISolutionSynchronizationSettings _settings;

		private readonly string _projectName;

		private static readonly string DefaultMonoDevelopSolutionProperties = string.Join("\r\n", new string[]
		{
			"    GlobalSection(MonoDevelopProperties) = preSolution",
			"        StartupItem = Assembly-CSharp.csproj",
			"    EndGlobalSection"
		}).Replace("    ", "\t");

		public static readonly Regex scriptReferenceExpression = new Regex("^Library.ScriptAssemblies.(?<dllname>(?<project>.*)\\.dll$)", RegexOptions.IgnoreCase | RegexOptions.Compiled);

		public SolutionSynchronizer(string projectDirectory, ISolutionSynchronizationSettings settings)
		{
			this._projectDirectory = projectDirectory;
			this._settings = settings;
			this._projectName = Path.GetFileName(this._projectDirectory);
		}

		public SolutionSynchronizer(string projectDirectory) : this(projectDirectory, SolutionSynchronizer.DefaultSynchronizationSettings)
		{
		}

		private void SetupProjectSupportedExtensions()
		{
			this.ProjectSupportedExtensions = EditorSettings.projectGenerationUserExtensions;
		}

		public bool ShouldFileBePartOfSolution(string file)
		{
			string extension = Path.GetExtension(file);
			bool result;
			if (AssetDatabase.IsPackagedAssetPath(file))
			{
				string text = Path.GetFullPath(file).ConvertSeparatorsToUnity();
				if (!text.StartsWith(this._projectDirectory))
				{
					result = false;
					return result;
				}
			}
			result = (extension == ".dll" || file.ToLower().EndsWith(".asmdef") || this.IsSupportedExtension(extension));
			return result;
		}

		private bool IsSupportedExtension(string extension)
		{
			extension = extension.TrimStart(new char[]
			{
				'.'
			});
			return SolutionSynchronizer.BuiltinSupportedExtensions.ContainsKey(extension) || this.ProjectSupportedExtensions.Contains(extension);
		}

		private static ScriptingLanguage ScriptingLanguageFor(MonoIsland island)
		{
			return SolutionSynchronizer.ScriptingLanguageFor(island.GetExtensionOfSourceFiles());
		}

		private static ScriptingLanguage ScriptingLanguageFor(string extension)
		{
			ScriptingLanguage scriptingLanguage;
			ScriptingLanguage result;
			if (SolutionSynchronizer.BuiltinSupportedExtensions.TryGetValue(extension.TrimStart(new char[]
			{
				'.'
			}), out scriptingLanguage))
			{
				result = scriptingLanguage;
			}
			else
			{
				result = ScriptingLanguage.None;
			}
			return result;
		}

		public bool ProjectExists(MonoIsland island)
		{
			return File.Exists(this.ProjectFile(island));
		}

		public bool SolutionExists()
		{
			return File.Exists(this.SolutionFile());
		}

		private static void DumpIsland(MonoIsland island)
		{
			Console.WriteLine("{0} ({1})", island._output, island._api_compatibility_level);
			Console.WriteLine("Files: ");
			Console.WriteLine(string.Join("\n", island._files));
			Console.WriteLine("References: ");
			Console.WriteLine(string.Join("\n", island._references));
			Console.WriteLine("");
		}

		public bool SyncIfNeeded(IEnumerable<string> affectedFiles)
		{
			this.SetupProjectSupportedExtensions();
			bool result;
			if (this.SolutionExists() && affectedFiles.Any(new Func<string, bool>(this.ShouldFileBePartOfSolution)))
			{
				this.Sync();
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		public void Sync()
		{
			this.SetupProjectSupportedExtensions();
			if (!AssetPostprocessingInternal.OnPreGeneratingCSProjectFiles())
			{
				ScriptEditorUtility.ScriptEditor scriptEditorFromPreferences = ScriptEditorUtility.GetScriptEditorFromPreferences();
				if (scriptEditorFromPreferences == ScriptEditorUtility.ScriptEditor.SystemDefault || scriptEditorFromPreferences == ScriptEditorUtility.ScriptEditor.Other)
				{
					return;
				}
				IEnumerable<MonoIsland> islands = from i in EditorCompilationInterface.GetAllMonoIslands()
				where 0 < i._files.Length && i._files.Any((string f) => this.ShouldFileBePartOfSolution(f))
				select i;
				Dictionary<string, string> allAssetsProjectParts = this.GenerateAllAssetProjectParts();
				string[] responseFileDefinesFromFile = ScriptCompilerBase.GetResponseFileDefinesFromFile(MonoCSharpCompiler.ReponseFilename);
				this.SyncSolution(islands);
				List<MonoIsland> list = SolutionSynchronizer.RelevantIslandsForMode(islands, SolutionSynchronizer.ModeForCurrentExternalEditor()).ToList<MonoIsland>();
				foreach (MonoIsland current in list)
				{
					this.SyncProject(current, allAssetsProjectParts, responseFileDefinesFromFile, list);
				}
				if (scriptEditorFromPreferences == ScriptEditorUtility.ScriptEditor.VisualStudioCode)
				{
					this.WriteVSCodeSettingsFiles();
				}
			}
			AssetPostprocessingInternal.CallOnGeneratedCSProjectFiles();
		}

		private Dictionary<string, string> GenerateAllAssetProjectParts()
		{
			Dictionary<string, StringBuilder> dictionary = new Dictionary<string, StringBuilder>();
			string[] allAssetPaths = AssetDatabase.GetAllAssetPaths();
			int i = 0;
			while (i < allAssetPaths.Length)
			{
				string text = allAssetPaths[i];
				if (!AssetDatabase.IsPackagedAssetPath(text))
				{
					goto IL_4B;
				}
				string text2 = Path.GetFullPath(text).ConvertSeparatorsToUnity();
				if (text2.StartsWith(this._projectDirectory))
				{
					goto IL_4B;
				}
				IL_FE:
				i++;
				continue;
				IL_4B:
				string extension = Path.GetExtension(text);
				if (this.IsSupportedExtension(extension) && SolutionSynchronizer.ScriptingLanguageFor(extension) == ScriptingLanguage.None)
				{
					string text3 = CompilationPipeline.GetAssemblyNameFromScriptPath(text + ".cs");
					text3 = (text3 ?? CompilationPipeline.GetAssemblyNameFromScriptPath(text + ".js"));
					text3 = (text3 ?? CompilationPipeline.GetAssemblyNameFromScriptPath(text + ".boo"));
					text3 = Path.GetFileNameWithoutExtension(text3);
					StringBuilder stringBuilder = null;
					if (!dictionary.TryGetValue(text3, out stringBuilder))
					{
						stringBuilder = new StringBuilder();
						dictionary[text3] = stringBuilder;
					}
					stringBuilder.AppendFormat("     <None Include=\"{0}\" />{1}", this.EscapedRelativePathFor(text), SolutionSynchronizer.WindowsNewline);
				}
				goto IL_FE;
			}
			Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
			foreach (KeyValuePair<string, StringBuilder> current in dictionary)
			{
				dictionary2[current.Key] = current.Value.ToString();
			}
			return dictionary2;
		}

		private void SyncProject(MonoIsland island, Dictionary<string, string> allAssetsProjectParts, string[] additionalDefines, List<MonoIsland> allProjectIslands)
		{
			SolutionSynchronizer.SyncFileIfNotChanged(this.ProjectFile(island), this.ProjectText(island, SolutionSynchronizer.ModeForCurrentExternalEditor(), allAssetsProjectParts, additionalDefines, allProjectIslands));
		}

		private static void SyncFileIfNotChanged(string filename, string newContents)
		{
			if (!File.Exists(filename) || !(newContents == File.ReadAllText(filename)))
			{
				File.WriteAllText(filename, newContents, Encoding.UTF8);
			}
		}

		private void WriteVSCodeSettingsFiles()
		{
			string text = Path.Combine(this._projectDirectory, ".vscode");
			if (!Directory.Exists(text))
			{
				Directory.CreateDirectory(text);
			}
			string path = Path.Combine(text, "settings.json");
			if (!File.Exists(path))
			{
				File.WriteAllText(path, VSCodeTemplates.SettingsJson);
			}
		}

		private static bool IsAdditionalInternalAssemblyReference(bool isBuildingEditorProject, string reference)
		{
			return isBuildingEditorProject && ModuleUtils.GetAdditionalReferencesForEditorCsharpProject().Contains(reference);
		}

		private string ProjectText(MonoIsland island, SolutionSynchronizer.Mode mode, Dictionary<string, string> allAssetsProjectParts, string[] additionalDefines, List<MonoIsland> allProjectIslands)
		{
			StringBuilder stringBuilder = new StringBuilder(this.ProjectHeader(island, additionalDefines));
			List<string> list = new List<string>();
			List<Match> list2 = new List<Match>();
			bool isBuildingEditorProject = island._output.EndsWith("-Editor.dll");
			string[] files = island._files;
			for (int j = 0; j < files.Length; j++)
			{
				string text = files[j];
				if (this.ShouldFileBePartOfSolution(text))
				{
					string b = Path.GetExtension(text).ToLower();
					string text2 = (!Path.IsPathRooted(text)) ? Path.Combine(this._projectDirectory, text) : text;
					if (".dll" != b)
					{
						string arg = "Compile";
						stringBuilder.AppendFormat("     <{0} Include=\"{1}\" />{2}", arg, this.EscapedRelativePathFor(text2), SolutionSynchronizer.WindowsNewline);
					}
					else
					{
						list.Add(text2);
					}
				}
			}
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(island._output);
			string value;
			if (allAssetsProjectParts.TryGetValue(fileNameWithoutExtension, out value))
			{
				stringBuilder.Append(value);
			}
			List<string> list3 = new List<string>();
			foreach (string current in list.Union(island._references))
			{
				if (!current.EndsWith("/UnityEditor.dll") && !current.EndsWith("/UnityEngine.dll") && !current.EndsWith("\\UnityEditor.dll") && !current.EndsWith("\\UnityEngine.dll"))
				{
					Match match = SolutionSynchronizer.scriptReferenceExpression.Match(current);
					if (match.Success)
					{
						SupportedLanguage languageFromExtension = ScriptCompilers.GetLanguageFromExtension(island.GetExtensionOfSourceFiles());
						ScriptingLanguage scriptingLanguage = (ScriptingLanguage)Enum.Parse(typeof(ScriptingLanguage), languageFromExtension.GetLanguageName(), true);
						if (mode == SolutionSynchronizer.Mode.UnityScriptAsUnityProj || scriptingLanguage == ScriptingLanguage.CSharp)
						{
							string dllName = match.Groups["dllname"].Value;
							if (allProjectIslands.Any((MonoIsland i) => Path.GetFileName(i._output) == dllName))
							{
								list2.Add(match);
								continue;
							}
						}
					}
					string text3 = (!Path.IsPathRooted(current)) ? Path.Combine(this._projectDirectory, current) : current;
					if (AssemblyHelper.IsManagedAssembly(text3))
					{
						if (AssemblyHelper.IsInternalAssembly(text3))
						{
							if (!SolutionSynchronizer.IsAdditionalInternalAssemblyReference(isBuildingEditorProject, text3))
							{
								continue;
							}
							string fileName = Path.GetFileName(text3);
							if (list3.Contains(fileName))
							{
								continue;
							}
							list3.Add(fileName);
						}
						text3 = text3.Replace("\\", "/");
						text3 = text3.Replace("\\\\", "/");
						stringBuilder.AppendFormat(" <Reference Include=\"{0}\">{1}", Path.GetFileNameWithoutExtension(text3), SolutionSynchronizer.WindowsNewline);
						stringBuilder.AppendFormat(" <HintPath>{0}</HintPath>{1}", text3, SolutionSynchronizer.WindowsNewline);
						stringBuilder.AppendFormat(" </Reference>{0}", SolutionSynchronizer.WindowsNewline);
					}
				}
			}
			if (0 < list2.Count)
			{
				stringBuilder.AppendLine("  </ItemGroup>");
				stringBuilder.AppendLine("  <ItemGroup>");
				foreach (Match current2 in list2)
				{
					EditorBuildRules.TargetAssembly targetAssemblyDetails = EditorCompilationInterface.Instance.GetTargetAssemblyDetails(current2.Groups["dllname"].Value);
					ScriptingLanguage language = ScriptingLanguage.None;
					if (targetAssemblyDetails != null)
					{
						language = (ScriptingLanguage)Enum.Parse(typeof(ScriptingLanguage), targetAssemblyDetails.Language.GetLanguageName(), true);
					}
					string value2 = current2.Groups["project"].Value;
					stringBuilder.AppendFormat("    <ProjectReference Include=\"{0}{1}\">{2}", value2, SolutionSynchronizer.GetProjectExtension(language), SolutionSynchronizer.WindowsNewline);
					stringBuilder.AppendFormat("      <Project>{{{0}}}</Project>", this.ProjectGuid(Path.Combine("Temp", current2.Groups["project"].Value + ".dll")), SolutionSynchronizer.WindowsNewline);
					stringBuilder.AppendFormat("      <Name>{0}</Name>", value2, SolutionSynchronizer.WindowsNewline);
					stringBuilder.AppendLine("    </ProjectReference>");
				}
			}
			stringBuilder.Append(this.ProjectFooter(island));
			return stringBuilder.ToString();
		}

		public string ProjectFile(MonoIsland island)
		{
			ScriptingLanguage key = SolutionSynchronizer.ScriptingLanguageFor(island);
			return Path.Combine(this._projectDirectory, string.Format("{0}{1}", Path.GetFileNameWithoutExtension(island._output), SolutionSynchronizer.ProjectExtensions[key]));
		}

		internal string SolutionFile()
		{
			return Path.Combine(this._projectDirectory, string.Format("{0}.sln", this._projectName));
		}

		private string ProjectHeader(MonoIsland island, string[] additionalDefines)
		{
			string text = "v3.5";
			string text2 = "4";
			string text3 = "4.0";
			string text4 = "10.0.20506";
			string text5 = ".";
			ScriptingLanguage language = SolutionSynchronizer.ScriptingLanguageFor(island);
			if (PlayerSettingsEditor.IsLatestApiCompatibility(island._api_compatibility_level))
			{
				text = "v4.6";
				text2 = "6";
			}
			else if (ScriptEditorUtility.GetScriptEditorFromPreferences() == ScriptEditorUtility.ScriptEditor.Rider)
			{
				text = "v4.5";
			}
			else if (this._settings.VisualStudioVersion == 9)
			{
				text3 = "3.5";
				text4 = "9.0.21022";
			}
			object[] array = new object[]
			{
				text3,
				text4,
				this.ProjectGuid(island._output),
				this._settings.EngineAssemblyPath,
				this._settings.EditorAssemblyPath,
				string.Join(";", new string[]
				{
					"DEBUG",
					"TRACE"
				}.Concat(this._settings.Defines).Concat(island._defines).Concat(additionalDefines).Distinct<string>().ToArray<string>()),
				SolutionSynchronizer.MSBuildNamespaceUri,
				Path.GetFileNameWithoutExtension(island._output),
				EditorSettings.projectGenerationRootNamespace,
				text,
				text2,
				text5
			};
			string result;
			try
			{
				result = string.Format(this._settings.GetProjectHeaderTemplate(language), array);
			}
			catch (Exception)
			{
				throw new NotSupportedException("Failed creating c# project because the c# project header did not have the correct amount of arguments, which is " + array.Length);
			}
			return result;
		}

		private void SyncSolution(IEnumerable<MonoIsland> islands)
		{
			SolutionSynchronizer.SyncFileIfNotChanged(this.SolutionFile(), this.SolutionText(islands, SolutionSynchronizer.ModeForCurrentExternalEditor()));
		}

		private static SolutionSynchronizer.Mode ModeForCurrentExternalEditor()
		{
			ScriptEditorUtility.ScriptEditor scriptEditorFromPreferences = ScriptEditorUtility.GetScriptEditorFromPreferences();
			SolutionSynchronizer.Mode result;
			if (scriptEditorFromPreferences == ScriptEditorUtility.ScriptEditor.VisualStudio || scriptEditorFromPreferences == ScriptEditorUtility.ScriptEditor.VisualStudioExpress || scriptEditorFromPreferences == ScriptEditorUtility.ScriptEditor.VisualStudioCode)
			{
				result = SolutionSynchronizer.Mode.UnityScriptAsPrecompiledAssembly;
			}
			else
			{
				result = ((!EditorPrefs.GetBool("kExternalEditorSupportsUnityProj", false)) ? SolutionSynchronizer.Mode.UnityScriptAsPrecompiledAssembly : SolutionSynchronizer.Mode.UnityScriptAsUnityProj);
			}
			return result;
		}

		private string SolutionText(IEnumerable<MonoIsland> islands, SolutionSynchronizer.Mode mode)
		{
			string text = "11.00";
			string text2 = "2010";
			if (this._settings.VisualStudioVersion == 9)
			{
				text = "10.00";
				text2 = "2008";
			}
			IEnumerable<MonoIsland> enumerable = SolutionSynchronizer.RelevantIslandsForMode(islands, mode);
			string projectEntries = this.GetProjectEntries(enumerable);
			string text3 = string.Join(SolutionSynchronizer.WindowsNewline, (from i in enumerable
			select this.GetProjectActiveConfigurations(this.ProjectGuid(i._output))).ToArray<string>());
			return string.Format(this._settings.SolutionTemplate, new object[]
			{
				text,
				text2,
				projectEntries,
				text3,
				this.ReadExistingMonoDevelopSolutionProperties()
			});
		}

		private static IEnumerable<MonoIsland> RelevantIslandsForMode(IEnumerable<MonoIsland> islands, SolutionSynchronizer.Mode mode)
		{
			return from i in islands
			where mode == SolutionSynchronizer.Mode.UnityScriptAsUnityProj || ScriptingLanguage.CSharp == SolutionSynchronizer.ScriptingLanguageFor(i)
			select i;
		}

		private string GetProjectEntries(IEnumerable<MonoIsland> islands)
		{
			IEnumerable<string> source = from i in islands
			select string.Format(SolutionSynchronizer.DefaultSynchronizationSettings.SolutionProjectEntryTemplate, new object[]
			{
				this.SolutionGuid(i),
				this._projectName,
				Path.GetFileName(this.ProjectFile(i)),
				this.ProjectGuid(i._output)
			});
			return string.Join(SolutionSynchronizer.WindowsNewline, source.ToArray<string>());
		}

		private string GetProjectActiveConfigurations(string projectGuid)
		{
			return string.Format(SolutionSynchronizer.DefaultSynchronizationSettings.SolutionProjectConfigurationTemplate, projectGuid);
		}

		private string EscapedRelativePathFor(string file)
		{
			string prefix = this._projectDirectory.ConvertSeparatorsToWindows();
			file = file.ConvertSeparatorsToWindows();
			string text = Paths.SkipPathPrefix(file, prefix);
			if (AssetDatabase.IsPackagedAssetPath(text.ConvertSeparatorsToUnity()))
			{
				string path = Path.GetFullPath(text).ConvertSeparatorsToWindows();
				text = Paths.SkipPathPrefix(path, prefix);
			}
			return SecurityElement.Escape(text);
		}

		private string ProjectGuid(string assembly)
		{
			return SolutionGuidGenerator.GuidForProject(this._projectName + Path.GetFileNameWithoutExtension(assembly));
		}

		private string SolutionGuid(MonoIsland island)
		{
			return SolutionGuidGenerator.GuidForSolution(this._projectName, island.GetExtensionOfSourceFiles());
		}

		private string ProjectFooter(MonoIsland island)
		{
			return string.Format(this._settings.GetProjectFooterTemplate(SolutionSynchronizer.ScriptingLanguageFor(island)), this.ReadExistingMonoDevelopProjectProperties(island));
		}

		private string ReadExistingMonoDevelopSolutionProperties()
		{
			string result;
			if (!this.SolutionExists())
			{
				result = SolutionSynchronizer.DefaultMonoDevelopSolutionProperties;
			}
			else
			{
				string[] array;
				try
				{
					array = File.ReadAllLines(this.SolutionFile());
				}
				catch (IOException)
				{
					result = SolutionSynchronizer.DefaultMonoDevelopSolutionProperties;
					return result;
				}
				StringBuilder stringBuilder = new StringBuilder();
				bool flag = false;
				string[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					string text = array2[i];
					if (SolutionSynchronizer._MonoDevelopPropertyHeader.IsMatch(text))
					{
						flag = true;
					}
					if (flag)
					{
						if (text.Contains("EndGlobalSection"))
						{
							stringBuilder.Append(text);
							flag = false;
						}
						else
						{
							stringBuilder.AppendFormat("{0}{1}", text, SolutionSynchronizer.WindowsNewline);
						}
					}
				}
				if (0 < stringBuilder.Length)
				{
					result = stringBuilder.ToString();
				}
				else
				{
					result = SolutionSynchronizer.DefaultMonoDevelopSolutionProperties;
				}
			}
			return result;
		}

		private string ReadExistingMonoDevelopProjectProperties(MonoIsland island)
		{
			string result;
			if (!this.ProjectExists(island))
			{
				result = string.Empty;
			}
			else
			{
				XmlDocument xmlDocument = new XmlDocument();
				XmlNamespaceManager xmlNamespaceManager;
				try
				{
					xmlDocument.Load(this.ProjectFile(island));
					xmlNamespaceManager = new XmlNamespaceManager(xmlDocument.NameTable);
					xmlNamespaceManager.AddNamespace("msb", SolutionSynchronizer.MSBuildNamespaceUri);
				}
				catch (Exception ex)
				{
					if (ex is IOException || ex is XmlException)
					{
						result = string.Empty;
						return result;
					}
					throw;
				}
				XmlNodeList xmlNodeList = xmlDocument.SelectNodes("/msb:Project/msb:ProjectExtensions", xmlNamespaceManager);
				if (xmlNodeList.Count == 0)
				{
					result = string.Empty;
				}
				else
				{
					StringBuilder stringBuilder = new StringBuilder();
					IEnumerator enumerator = xmlNodeList.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							XmlNode xmlNode = (XmlNode)enumerator.Current;
							stringBuilder.AppendLine(xmlNode.OuterXml);
						}
					}
					finally
					{
						IDisposable disposable;
						if ((disposable = (enumerator as IDisposable)) != null)
						{
							disposable.Dispose();
						}
					}
					result = stringBuilder.ToString();
				}
			}
			return result;
		}

		[Obsolete("Use AssemblyHelper.IsManagedAssembly")]
		public static bool IsManagedAssembly(string file)
		{
			return AssemblyHelper.IsManagedAssembly(file);
		}

		public static string GetProjectExtension(ScriptingLanguage language)
		{
			if (!SolutionSynchronizer.ProjectExtensions.ContainsKey(language))
			{
				throw new ArgumentException("Unsupported language", "language");
			}
			return SolutionSynchronizer.ProjectExtensions[language];
		}
	}
}
