using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.IMGUI.Controls;
using UnityEngine.Experimental.UIElements;

namespace UnityEditor.Experimental.UIElements.Debugger
{
	internal class VisualTreeTreeView : TreeView
	{
		public Panel panel;

		public bool includeShadowHierarchy;

		public VisualTreeTreeView(TreeViewState state) : base(state)
		{
		}

		protected override TreeViewItem BuildRoot()
		{
			TreeViewItem treeViewItem = new TreeViewItem(0, -1);
			this.Recurse(treeViewItem, this.panel.visualTree);
			return treeViewItem;
		}

		private void Recurse(TreeViewItem tree, VisualElement elt)
		{
			VisualTreeItem visualTreeItem = new VisualTreeItem(elt, tree.depth + 1);
			tree.AddChild(visualTreeItem);
			IEnumerable<VisualElement> enumerable = (!this.includeShadowHierarchy) ? ((elt.contentContainer != null) ? elt.Children() : Enumerable.Empty<VisualElement>()) : elt.shadow.Children();
			foreach (VisualElement current in enumerable)
			{
				this.Recurse(visualTreeItem, current);
			}
		}

		public VisualTreeItem GetNodeFor(int selectedId)
		{
			return base.FindRows(new List<int>
			{
				selectedId
			}).FirstOrDefault<TreeViewItem>() as VisualTreeItem;
		}
	}
}
