using System;
using System.IO;
using System.Linq;
using UnityEngine.Experimental.UIElements;

namespace UnityEditor.Collaboration
{
	internal class CollabHistoryDropDownItem : VisualElement
	{
		public CollabHistoryDropDownItem(string path, string action)
		{
			string fileName = Path.GetFileName(path);
			bool isFolder = Path.GetFileNameWithoutExtension(path).Equals(fileName);
			Image iconElement = this.GetIconElement(action, fileName, isFolder);
			VisualElement visualElement = new VisualElement();
			Label child = new Label
			{
				name = "FileName",
				text = fileName
			};
			Label child2 = new Label
			{
				name = "FilePath",
				text = path
			};
			visualElement.Add(child);
			visualElement.Add(child2);
			base.Add(iconElement);
			base.Add(visualElement);
		}

		private Image GetIconElement(string action, string fileName, bool isFolder)
		{
			string str = (!isFolder) ? "File" : "Folder";
			string text = action.First<char>().ToString().ToUpper() + action.Substring(1);
			text = ((!text.Equals("Renamed")) ? text : "Moved");
			return new Image
			{
				name = "FileIcon",
				image = EditorGUIUtility.LoadIcon("Icons/Collab." + str + text + ".png")
			};
		}
	}
}
