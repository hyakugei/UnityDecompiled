using System;
using System.Linq;
using UnityEditor.IMGUI.Controls;
using UnityEngine.Experimental.UIElements;

namespace UnityEditor.Experimental.UIElements.Debugger
{
	internal class VisualTreeItem : TreeViewItem
	{
		public readonly VisualElement elt;

		public uint controlId
		{
			get
			{
				return this.elt.controlid;
			}
		}

		public VisualTreeItem(VisualElement elt, int depth) : base((int)elt.controlid, depth, VisualTreeItem.GetDisplayName(elt))
		{
			this.elt = elt;
		}

		private static string GetDisplayName(VisualElement elt)
		{
			string str = (elt.GetType() != typeof(VisualElement)) ? (elt.GetType().Name + " ") : string.Empty;
			string str2 = (!string.IsNullOrEmpty(elt.name)) ? ("#" + elt.name + " ") : string.Empty;
			string text = str + str2 + ((!elt.GetClasses().Any<string>()) ? string.Empty : ("." + string.Join(",.", elt.GetClasses().ToArray<string>())));
			string result;
			if (text == string.Empty)
			{
				result = elt.GetType().Name;
			}
			else
			{
				result = text;
			}
			return result;
		}
	}
}
