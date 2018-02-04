using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using UnityEditor.Utils;
using UnityEngine;

namespace UnityEditor.Scripting.Compilers
{
	internal class MonoCSharpCompiler : MonoScriptCompilerBase
	{
		public static readonly string ReponseFilename = "mcs.rsp";

		public MonoCSharpCompiler(MonoIsland island, bool runUpdater) : base(island, runUpdater)
		{
		}

		protected override Program StartCompiler()
		{
			List<string> list = new List<string>
			{
				"-debug",
				"-target:library",
				"-nowarn:0169",
				"-langversion:" + ((EditorApplication.scriptingRuntimeVersion != ScriptingRuntimeVersion.Latest) ? "4" : "6"),
				"-out:" + ScriptCompilerBase.PrepareFileName(this._island._output),
				"-nostdlib",
				"-unsafe"
			};
			if (!this._island._development_player && !this._island._editor)
			{
				list.Add("-optimize");
			}
			string[] references = this._island._references;
			for (int i = 0; i < references.Length; i++)
			{
				string fileName = references[i];
				list.Add("-r:" + ScriptCompilerBase.PrepareFileName(fileName));
			}
			foreach (string current in this._island._defines.Distinct<string>())
			{
				list.Add("-define:" + current);
			}
			string[] files = this._island._files;
			for (int j = 0; j < files.Length; j++)
			{
				string fileName2 = files[j];
				list.Add(ScriptCompilerBase.PrepareFileName(fileName2));
			}
			if (!base.AddCustomResponseFileIfPresent(list, MonoCSharpCompiler.ReponseFilename))
			{
				if (this._island._api_compatibility_level == ApiCompatibilityLevel.NET_2_0_Subset && base.AddCustomResponseFileIfPresent(list, "smcs.rsp"))
				{
					Debug.LogWarning(string.Format("Using obsolete custom response file 'smcs.rsp'. Please use '{0}' instead.", MonoCSharpCompiler.ReponseFilename));
				}
				else if (this._island._api_compatibility_level == ApiCompatibilityLevel.NET_2_0 && base.AddCustomResponseFileIfPresent(list, "gmcs.rsp"))
				{
					Debug.LogWarning(string.Format("Using obsolete custom response file 'gmcs.rsp'. Please use '{0}' instead.", MonoCSharpCompiler.ReponseFilename));
				}
			}
			return base.StartCompiler(this._island._target, this.GetCompilerPath(list), list, false, MonoInstallationFinder.GetMonoInstallation("MonoBleedingEdge"));
		}

		private string GetCompilerPath(List<string> arguments)
		{
			string profileDirectory = MonoInstallationFinder.GetProfileDirectory("4.5", "MonoBleedingEdge");
			string text = Path.Combine(profileDirectory, "mcs.exe");
			if (File.Exists(text))
			{
				string profile = (this._island._api_compatibility_level != ApiCompatibilityLevel.NET_2_0) ? BuildPipeline.CompatibilityProfileToClassLibFolder(this._island._api_compatibility_level) : "2.0-api";
				if (this._island._api_compatibility_level != ApiCompatibilityLevel.NET_Standard_2_0)
				{
					arguments.Add("-lib:" + ScriptCompilerBase.PrepareFileName(MonoInstallationFinder.GetProfileDirectory(profile, "MonoBleedingEdge")));
				}
				return text;
			}
			throw new ApplicationException("Unable to find csharp compiler in " + profileDirectory);
		}

		protected override CompilerOutputParserBase CreateOutputParser()
		{
			return new MonoCSharpCompilerOutputParser();
		}

		public static string[] Compile(string[] sources, string[] references, string[] defines, string outputFile)
		{
			MonoIsland island = new MonoIsland(BuildTarget.StandaloneWindows, ApiCompatibilityLevel.NET_2_0_Subset, sources, references, defines, outputFile);
			string[] result;
			using (MonoCSharpCompiler monoCSharpCompiler = new MonoCSharpCompiler(island, false))
			{
				monoCSharpCompiler.BeginCompiling();
				while (!monoCSharpCompiler.Poll())
				{
					Thread.Sleep(50);
				}
				result = (from cm in monoCSharpCompiler.GetCompilerMessages()
				select cm.message).ToArray<string>();
			}
			return result;
		}
	}
}
