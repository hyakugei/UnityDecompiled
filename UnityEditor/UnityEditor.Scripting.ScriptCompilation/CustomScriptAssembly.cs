using System;
using System.Linq;

namespace UnityEditor.Scripting.ScriptCompilation
{
	internal class CustomScriptAssembly
	{
		public string FilePath
		{
			get;
			set;
		}

		public string PathPrefix
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public string[] References
		{
			get;
			set;
		}

		public OptionalUnityReferences OptionalUnityReferences
		{
			get;
			set;
		}

		public CustomScriptAssemblyPlatform[] IncludePlatforms
		{
			get;
			set;
		}

		public CustomScriptAssemblyPlatform[] ExcludePlatforms
		{
			get;
			set;
		}

		public EditorCompilation.PackageAssembly? PackageAssembly
		{
			get;
			set;
		}

		public AssemblyFlags AssemblyFlags
		{
			get
			{
				AssemblyFlags result;
				if (this.IncludePlatforms != null && this.IncludePlatforms.Length == 1 && this.IncludePlatforms[0].BuildTarget == BuildTarget.NoTarget)
				{
					result = AssemblyFlags.EditorOnly;
				}
				else
				{
					result = AssemblyFlags.None;
				}
				return result;
			}
		}

		public static CustomScriptAssemblyPlatform[] Platforms
		{
			get;
			private set;
		}

		public static CustomScriptOptinalUnityAssembly[] OptinalUnityAssemblies
		{
			get;
			private set;
		}

		static CustomScriptAssembly()
		{
			CustomScriptAssembly.Platforms = new CustomScriptAssemblyPlatform[]
			{
				new CustomScriptAssemblyPlatform("Editor", "Editor", BuildTarget.NoTarget),
				new CustomScriptAssemblyPlatform("macOSStandalone", "macOS", BuildTarget.StandaloneOSX),
				new CustomScriptAssemblyPlatform("WindowsStandalone32", "Windows 32-bit", BuildTarget.StandaloneWindows),
				new CustomScriptAssemblyPlatform("WindowsStandalone64", "Windows 64-bit", BuildTarget.StandaloneWindows64),
				new CustomScriptAssemblyPlatform("LinuxStandalone32", "Linux 32-bit", BuildTarget.StandaloneLinux),
				new CustomScriptAssemblyPlatform("LinuxStandalone64", "Linux 64-bit", BuildTarget.StandaloneLinux64),
				new CustomScriptAssemblyPlatform("LinuxStandaloneUniversal", "Linux Universal", BuildTarget.StandaloneLinuxUniversal),
				new CustomScriptAssemblyPlatform("iOS", BuildTarget.iOS),
				new CustomScriptAssemblyPlatform("Android", BuildTarget.Android),
				new CustomScriptAssemblyPlatform("WebGL", BuildTarget.WebGL),
				new CustomScriptAssemblyPlatform("WSA", "Windows Store App", BuildTarget.WSAPlayer),
				new CustomScriptAssemblyPlatform("Tizen", BuildTarget.Tizen),
				new CustomScriptAssemblyPlatform("PSVita", BuildTarget.PSP2),
				new CustomScriptAssemblyPlatform("PS4", BuildTarget.PS4),
				new CustomScriptAssemblyPlatform("XboxOne", BuildTarget.XboxOne),
				new CustomScriptAssemblyPlatform("Nintendo3DS", BuildTarget.N3DS),
				new CustomScriptAssemblyPlatform("tvOS", BuildTarget.tvOS),
				new CustomScriptAssemblyPlatform("Switch", BuildTarget.Switch)
			};
			CustomScriptAssembly.OptinalUnityAssemblies = new CustomScriptOptinalUnityAssembly[]
			{
				new CustomScriptOptinalUnityAssembly("Test Assemblies", OptionalUnityReferences.TestAssemblies, "Predefined Assemblies (Assembly-CSharp.dll etc) will not reference this assembly.\nThis assembly will only be used for tests and will not be included in player builds.")
			};
		}

		public bool IsCompatibleWithEditor()
		{
			bool result;
			if (this.ExcludePlatforms != null)
			{
				result = this.ExcludePlatforms.All((CustomScriptAssemblyPlatform p) => p.BuildTarget != BuildTarget.NoTarget);
			}
			else if (this.IncludePlatforms != null)
			{
				result = this.IncludePlatforms.Any((CustomScriptAssemblyPlatform p) => p.BuildTarget == BuildTarget.NoTarget);
			}
			else
			{
				result = true;
			}
			return result;
		}

		public bool IsCompatibleWith(BuildTarget buildTarget, EditorScriptCompilationOptions options)
		{
			bool flag = (options & EditorScriptCompilationOptions.BuildingForEditor) == EditorScriptCompilationOptions.BuildingForEditor;
			bool flag2 = (this.OptionalUnityReferences & OptionalUnityReferences.TestAssemblies) == OptionalUnityReferences.TestAssemblies;
			bool flag3 = (options & EditorScriptCompilationOptions.BuildingIncludingTestAssemblies) == EditorScriptCompilationOptions.BuildingIncludingTestAssemblies;
			bool result;
			if (!flag && flag2 && !flag3)
			{
				result = false;
			}
			else
			{
				bool hasValue = this.PackageAssembly.HasValue;
				if (flag2 && hasValue && !this.PackageAssembly.Value.IncludeTestAssemblies)
				{
					result = false;
				}
				else if (this.IncludePlatforms == null && this.ExcludePlatforms == null)
				{
					result = true;
				}
				else if (flag)
				{
					result = this.IsCompatibleWithEditor();
				}
				else
				{
					if (flag)
					{
						buildTarget = BuildTarget.NoTarget;
					}
					if (this.ExcludePlatforms != null)
					{
						result = this.ExcludePlatforms.All((CustomScriptAssemblyPlatform p) => p.BuildTarget != buildTarget);
					}
					else
					{
						result = this.IncludePlatforms.Any((CustomScriptAssemblyPlatform p) => p.BuildTarget == buildTarget);
					}
				}
			}
			return result;
		}

		public static CustomScriptAssembly Create(string name, string directory)
		{
			CustomScriptAssembly customScriptAssembly = new CustomScriptAssembly();
			string text = AssetPath.ReplaceSeparators(directory);
			if (text.Last<char>() != AssetPath.Separator)
			{
				text += AssetPath.Separator;
			}
			customScriptAssembly.Name = name;
			customScriptAssembly.FilePath = text;
			customScriptAssembly.PathPrefix = text;
			customScriptAssembly.References = new string[0];
			return customScriptAssembly;
		}

		public static CustomScriptAssembly FromCustomScriptAssemblyData(string path, CustomScriptAssemblyData customScriptAssemblyData)
		{
			CustomScriptAssembly result;
			if (customScriptAssemblyData == null)
			{
				result = null;
			}
			else
			{
				string pathPrefix = path.Substring(0, path.Length - AssetPath.GetFileName(path).Length);
				CustomScriptAssembly customScriptAssembly = new CustomScriptAssembly();
				customScriptAssembly.Name = customScriptAssemblyData.name;
				customScriptAssembly.References = customScriptAssemblyData.references;
				customScriptAssembly.FilePath = path;
				customScriptAssembly.PathPrefix = pathPrefix;
				customScriptAssemblyData.optionalUnityReferences = (customScriptAssemblyData.optionalUnityReferences ?? new string[0]);
				string[] optionalUnityReferences = customScriptAssemblyData.optionalUnityReferences;
				for (int i = 0; i < optionalUnityReferences.Length; i++)
				{
					string value = optionalUnityReferences[i];
					OptionalUnityReferences optionalUnityReferences2 = (OptionalUnityReferences)Enum.Parse(typeof(OptionalUnityReferences), value);
					customScriptAssembly.OptionalUnityReferences |= optionalUnityReferences2;
				}
				if (customScriptAssemblyData.includePlatforms != null && customScriptAssemblyData.includePlatforms.Length > 0)
				{
					customScriptAssembly.IncludePlatforms = (from name in customScriptAssemblyData.includePlatforms
					select CustomScriptAssembly.GetPlatformFromName(name)).ToArray<CustomScriptAssemblyPlatform>();
				}
				if (customScriptAssemblyData.excludePlatforms != null && customScriptAssemblyData.excludePlatforms.Length > 0)
				{
					customScriptAssembly.ExcludePlatforms = (from name in customScriptAssemblyData.excludePlatforms
					select CustomScriptAssembly.GetPlatformFromName(name)).ToArray<CustomScriptAssemblyPlatform>();
				}
				result = customScriptAssembly;
			}
			return result;
		}

		public static CustomScriptAssemblyPlatform GetPlatformFromName(string name)
		{
			CustomScriptAssemblyPlatform[] platforms = CustomScriptAssembly.Platforms;
			for (int i = 0; i < platforms.Length; i++)
			{
				CustomScriptAssemblyPlatform result = platforms[i];
				if (string.Equals(result.Name, name, StringComparison.OrdinalIgnoreCase))
				{
					return result;
				}
			}
			string[] array = (from p in CustomScriptAssembly.Platforms
			select string.Format("\"{0}\"", p.Name)).ToArray<string>();
			Array.Sort<string>(array);
			string arg = string.Join(",\n", array);
			throw new ArgumentException(string.Format("Platform name '{0}' not supported.\nSupported platform names:\n{1}\n", name, arg));
		}

		public static CustomScriptAssemblyPlatform GetPlatformFromBuildTarget(BuildTarget buildTarget)
		{
			CustomScriptAssemblyPlatform[] platforms = CustomScriptAssembly.Platforms;
			for (int i = 0; i < platforms.Length; i++)
			{
				CustomScriptAssemblyPlatform result = platforms[i];
				if (result.BuildTarget == buildTarget)
				{
					return result;
				}
			}
			throw new ArgumentException(string.Format("No CustomScriptAssemblyPlatform setup for BuildTarget '{0}'", buildTarget));
		}
	}
}
