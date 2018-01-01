using System;
using System.Collections;
using System.IO;
using System.Xml;
using UnityEditor.Utils;
using UnityEngine;

namespace UnityEditor.VisualStudioIntegration
{
	internal class SolutionPostProcessor : AssetPostprocessor
	{
		public override int GetPostprocessOrder()
		{
			return 2147483647;
		}

		private static bool ReplacePathInNode(XmlNode node, string projectDir)
		{
			string value = node.Attributes["Include"].Value;
			bool result;
			if (AssetDatabase.IsPackagedAssetPath(value.ConvertSeparatorsToUnity()))
			{
				string path = Path.GetFullPath(value).ConvertSeparatorsToWindows();
				string text = Paths.SkipPathPrefix(path, projectDir);
				if (text != value)
				{
					node.Attributes["Include"].Value = text;
					result = true;
					return result;
				}
			}
			result = false;
			return result;
		}

		internal static void ReplacePathsInProjectFile(string projectFile, string projectDir)
		{
			XmlDocument xmlDocument = new XmlDocument();
			try
			{
				bool flag = false;
				xmlDocument.Load(projectFile);
				XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager(xmlDocument.NameTable);
				xmlNamespaceManager.AddNamespace("msb", SolutionSynchronizer.MSBuildNamespaceUri);
				XmlNodeList xmlNodeList = xmlDocument.SelectNodes("//msb:Compile[@Include]", xmlNamespaceManager);
				IEnumerator enumerator = xmlNodeList.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						XmlNode node = (XmlNode)enumerator.Current;
						flag |= SolutionPostProcessor.ReplacePathInNode(node, projectDir);
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
				XmlNodeList xmlNodeList2 = xmlDocument.SelectNodes("//msb:None[@Include]", xmlNamespaceManager);
				IEnumerator enumerator2 = xmlNodeList2.GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						XmlNode node2 = (XmlNode)enumerator2.Current;
						flag |= SolutionPostProcessor.ReplacePathInNode(node2, projectDir);
					}
				}
				finally
				{
					IDisposable disposable2;
					if ((disposable2 = (enumerator2 as IDisposable)) != null)
					{
						disposable2.Dispose();
					}
				}
				if (flag)
				{
					xmlDocument.Save(projectFile);
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning("Post processing of CS project file " + projectFile + " failed, reason: " + ex.Message);
			}
		}

		public static void OnGeneratedCSProjectFiles()
		{
			if (UnityVSSupport.IsUnityVSEnabled())
			{
				try
				{
					string currentDirectory = Directory.GetCurrentDirectory();
					string[] files = Directory.GetFiles(currentDirectory, string.Format("*{0}", SolutionSynchronizer.GetProjectExtension(ScriptingLanguage.CSharp)), SearchOption.TopDirectoryOnly);
					string[] array = files;
					for (int i = 0; i < array.Length; i++)
					{
						string text = array[i];
						string projectFile = text.Substring(currentDirectory.Length + 1);
						SolutionPostProcessor.ReplacePathsInProjectFile(projectFile, currentDirectory);
					}
				}
				catch (Exception ex)
				{
					Debug.LogWarning("Post processing of CS project files failed, reason: " + ex.Message);
				}
			}
		}
	}
}
