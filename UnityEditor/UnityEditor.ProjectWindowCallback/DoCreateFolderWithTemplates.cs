using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace UnityEditor.ProjectWindowCallback
{
	internal class DoCreateFolderWithTemplates : EndNameEditAction
	{
		private const string kResourcesTemplatePath = "Resources/ScriptTemplates";

		public IList<string> templates
		{
			get;
			set;
		}

		public override void Action(int instanceId, string pathName, string resourceFile)
		{
			string fileName = Path.GetFileName(pathName);
			string guid = AssetDatabase.CreateFolder(Path.GetDirectoryName(pathName), fileName);
			string path = Path.Combine(EditorApplication.applicationContentsPath, "Resources/ScriptTemplates");
			foreach (string current in (this.templates ?? Enumerable.Empty<string>()))
			{
				string path2 = current.Replace(".txt", string.Empty);
				string extension = Path.GetExtension(path2);
				ProjectWindowUtil.CreateScriptAssetFromTemplate(Path.Combine(pathName, fileName + extension), Path.Combine(path, current));
			}
			UnityEngine.Object o = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(guid), typeof(UnityEngine.Object));
			ProjectWindowUtil.ShowCreatedAsset(o);
		}
	}
}
