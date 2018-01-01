using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace UnityEditorInternal
{
	internal class AnimationWindowHierarchyDataSource : TreeViewDataSource
	{
		private AnimationWindowState state
		{
			get;
			set;
		}

		public bool showAll
		{
			get;
			set;
		}

		public AnimationWindowHierarchyDataSource(TreeViewController treeView, AnimationWindowState animationWindowState) : base(treeView)
		{
			this.state = animationWindowState;
		}

		private void SetupRootNodeSettings()
		{
			base.showRootItem = false;
			base.rootIsCollapsable = false;
			this.SetExpanded(this.m_RootItem, true);
		}

		private AnimationWindowHierarchyNode GetEmptyRootNode()
		{
			return new AnimationWindowHierarchyNode(0, -1, null, null, "", "", "root");
		}

		public override void FetchData()
		{
			this.m_RootItem = this.GetEmptyRootNode();
			this.SetupRootNodeSettings();
			this.m_NeedRefreshRows = true;
			if (this.state.selection.disabled)
			{
				base.root.children = null;
			}
			else
			{
				List<AnimationWindowHierarchyNode> list = new List<AnimationWindowHierarchyNode>();
				if (this.state.allCurves.Count > 0)
				{
					list.Add(new AnimationWindowHierarchyMasterNode
					{
						curves = this.state.allCurves.ToArray()
					});
				}
				list.AddRange(this.CreateTreeFromCurves());
				list.Add(new AnimationWindowHierarchyAddButtonNode());
				TreeViewUtility.SetChildParentReferences(new List<TreeViewItem>(list.ToArray()), base.root);
			}
		}

		public override bool IsRenamingItemAllowed(TreeViewItem item)
		{
			return !(item is AnimationWindowHierarchyAddButtonNode) && !(item is AnimationWindowHierarchyMasterNode) && !(item is AnimationWindowHierarchyClipNode) && (item as AnimationWindowHierarchyNode).path.Length != 0;
		}

		public List<AnimationWindowHierarchyNode> CreateTreeFromCurves()
		{
			List<AnimationWindowHierarchyNode> list = new List<AnimationWindowHierarchyNode>();
			List<AnimationWindowCurve> list2 = new List<AnimationWindowCurve>();
			AnimationWindowCurve[] array = this.state.allCurves.ToArray();
			AnimationWindowHierarchyNode parentNode = (AnimationWindowHierarchyNode)this.m_RootItem;
			for (int i = 0; i < array.Length; i++)
			{
				AnimationWindowCurve animationWindowCurve = array[i];
				AnimationWindowCurve animationWindowCurve2 = (i >= array.Length - 1) ? null : array[i + 1];
				list2.Add(animationWindowCurve);
				bool flag = animationWindowCurve2 != null && AnimationWindowUtility.GetPropertyGroupName(animationWindowCurve2.propertyName) == AnimationWindowUtility.GetPropertyGroupName(animationWindowCurve.propertyName);
				bool flag2 = animationWindowCurve2 != null && animationWindowCurve.path.Equals(animationWindowCurve2.path) && animationWindowCurve.type == animationWindowCurve2.type;
				if (i == array.Length - 1 || !flag || !flag2)
				{
					if (list2.Count > 1)
					{
						list.Add(this.AddPropertyGroupToHierarchy(list2.ToArray(), parentNode));
					}
					else
					{
						list.Add(this.AddPropertyToHierarchy(list2[0], parentNode));
					}
					list2.Clear();
				}
			}
			return list;
		}

		private AnimationWindowHierarchyPropertyGroupNode AddPropertyGroupToHierarchy(AnimationWindowCurve[] curves, AnimationWindowHierarchyNode parentNode)
		{
			List<AnimationWindowHierarchyNode> list = new List<AnimationWindowHierarchyNode>();
			Type type = curves[0].type;
			AnimationWindowHierarchyPropertyGroupNode animationWindowHierarchyPropertyGroupNode = new AnimationWindowHierarchyPropertyGroupNode(type, 0, AnimationWindowUtility.GetPropertyGroupName(curves[0].propertyName), curves[0].path, parentNode);
			animationWindowHierarchyPropertyGroupNode.icon = this.GetIcon(curves[0].binding);
			animationWindowHierarchyPropertyGroupNode.indent = curves[0].depth;
			animationWindowHierarchyPropertyGroupNode.curves = curves;
			for (int i = 0; i < curves.Length; i++)
			{
				AnimationWindowCurve curve = curves[i];
				AnimationWindowHierarchyPropertyNode animationWindowHierarchyPropertyNode = this.AddPropertyToHierarchy(curve, animationWindowHierarchyPropertyGroupNode);
				animationWindowHierarchyPropertyNode.displayName = AnimationWindowUtility.GetPropertyDisplayName(animationWindowHierarchyPropertyNode.propertyName);
				list.Add(animationWindowHierarchyPropertyNode);
			}
			TreeViewUtility.SetChildParentReferences(new List<TreeViewItem>(list.ToArray()), animationWindowHierarchyPropertyGroupNode);
			return animationWindowHierarchyPropertyGroupNode;
		}

		private AnimationWindowHierarchyPropertyNode AddPropertyToHierarchy(AnimationWindowCurve curve, AnimationWindowHierarchyNode parentNode)
		{
			AnimationWindowHierarchyPropertyNode animationWindowHierarchyPropertyNode = new AnimationWindowHierarchyPropertyNode(curve.type, 0, curve.propertyName, curve.path, parentNode, curve.binding, curve.isPPtrCurve);
			if (parentNode.icon != null)
			{
				animationWindowHierarchyPropertyNode.icon = parentNode.icon;
			}
			else
			{
				animationWindowHierarchyPropertyNode.icon = this.GetIcon(curve.binding);
			}
			animationWindowHierarchyPropertyNode.indent = curve.depth;
			animationWindowHierarchyPropertyNode.curves = new AnimationWindowCurve[]
			{
				curve
			};
			return animationWindowHierarchyPropertyNode;
		}

		public Texture2D GetIcon(EditorCurveBinding curveBinding)
		{
			Texture2D result;
			if (this.state.activeRootGameObject != null)
			{
				UnityEngine.Object animatedObject = AnimationUtility.GetAnimatedObject(this.state.activeRootGameObject, curveBinding);
				if (animatedObject != null)
				{
					result = AssetPreview.GetMiniThumbnail(animatedObject);
					return result;
				}
			}
			result = AssetPreview.GetMiniTypeThumbnail(curveBinding.type);
			return result;
		}

		public void UpdateData()
		{
			this.m_TreeView.ReloadData();
		}
	}
}
