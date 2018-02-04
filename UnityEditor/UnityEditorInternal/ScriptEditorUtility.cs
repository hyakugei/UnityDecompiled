using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Utils;
using UnityEngine;

namespace UnityEditorInternal
{
	public class ScriptEditorUtility
	{
		public enum ScriptEditor
		{
			SystemDefault,
			MonoDevelop,
			VisualStudio,
			VisualStudioExpress,
			VisualStudioCode,
			Rider,
			Other = 32
		}

		public static ScriptEditorUtility.ScriptEditor GetScriptEditorFromPath(string path)
		{
			string text = path.ToLower();
			ScriptEditorUtility.ScriptEditor result;
			if (text == "internal")
			{
				result = ScriptEditorUtility.ScriptEditor.SystemDefault;
			}
			else if (text.Contains("monodevelop") || text.Contains("xamarinstudio") || text.Contains("xamarin studio"))
			{
				result = ScriptEditorUtility.ScriptEditor.MonoDevelop;
			}
			else if (text.EndsWith("devenv.exe"))
			{
				result = ScriptEditorUtility.ScriptEditor.VisualStudio;
			}
			else if (text.EndsWith("vcsexpress.exe"))
			{
				result = ScriptEditorUtility.ScriptEditor.VisualStudioExpress;
			}
			else
			{
				string text2 = Path.GetFileName(Paths.UnifyDirectorySeparator(text)).Replace(" ", "");
				if (text2 == "visualstudio.app")
				{
					result = ScriptEditorUtility.ScriptEditor.MonoDevelop;
				}
				else if (text2 == "code.exe" || text2 == "visualstudiocode.app" || text2 == "vscode.app" || text2 == "code.app" || text2 == "code")
				{
					result = ScriptEditorUtility.ScriptEditor.VisualStudioCode;
				}
				else if (text2 == "rider.exe" || text2 == "rider64.exe" || text2 == "rider32.exe" || (text2.StartsWith("rider") && text2.EndsWith(".app")) || text2 == "rider.sh")
				{
					result = ScriptEditorUtility.ScriptEditor.Rider;
				}
				else
				{
					result = ScriptEditorUtility.ScriptEditor.Other;
				}
			}
			return result;
		}

		public static string GetExternalScriptEditor()
		{
			string @string = EditorPrefs.GetString("kScriptsDefaultApp");
			string result;
			if (!string.IsNullOrEmpty(@string))
			{
				result = @string;
			}
			else
			{
				string[] foundScriptEditorPaths = ScriptEditorUtility.GetFoundScriptEditorPaths(Application.platform);
				if (foundScriptEditorPaths.Length > 0)
				{
					result = foundScriptEditorPaths[0];
				}
				else
				{
					result = string.Empty;
				}
			}
			return result;
		}

		public static void SetExternalScriptEditor(string path)
		{
			EditorPrefs.SetString("kScriptsDefaultApp", path);
		}

		private static string GetScriptEditorArgsKey(string path)
		{
			string result;
			if (Application.platform == RuntimePlatform.OSXEditor)
			{
				result = "kScriptEditorArgs_" + path;
			}
			else
			{
				result = "kScriptEditorArgs" + path;
			}
			return result;
		}

		private static string GetDefaultStringEditorArgs()
		{
			string result;
			if (Application.platform == RuntimePlatform.OSXEditor)
			{
				result = "";
			}
			else
			{
				result = "\"$(File)\"";
			}
			return result;
		}

		public static string GetExternalScriptEditorArgs()
		{
			string externalScriptEditor = ScriptEditorUtility.GetExternalScriptEditor();
			ScriptEditorUtility.ScriptEditor scriptEditorFromPath = ScriptEditorUtility.GetScriptEditorFromPath(externalScriptEditor);
			string result;
			if (scriptEditorFromPath != ScriptEditorUtility.ScriptEditor.Other)
			{
				result = "";
			}
			else
			{
				result = EditorPrefs.GetString(ScriptEditorUtility.GetScriptEditorArgsKey(externalScriptEditor), ScriptEditorUtility.GetDefaultStringEditorArgs());
			}
			return result;
		}

		public static void SetExternalScriptEditorArgs(string args)
		{
			string externalScriptEditor = ScriptEditorUtility.GetExternalScriptEditor();
			EditorPrefs.SetString(ScriptEditorUtility.GetScriptEditorArgsKey(externalScriptEditor), args);
		}

		public static ScriptEditorUtility.ScriptEditor GetScriptEditorFromPreferences()
		{
			return ScriptEditorUtility.GetScriptEditorFromPath(ScriptEditorUtility.GetExternalScriptEditor());
		}

		public static string[] GetFoundScriptEditorPaths(RuntimePlatform platform)
		{
			List<string> list = new List<string>();
			if (platform == RuntimePlatform.OSXEditor)
			{
				ScriptEditorUtility.AddIfDirectoryExists("/Applications/Visual Studio.app", list);
			}
			return list.ToArray();
		}

		private static void AddIfDirectoryExists(string path, List<string> list)
		{
			if (Directory.Exists(path))
			{
				list.Add(path);
			}
		}
	}
}
