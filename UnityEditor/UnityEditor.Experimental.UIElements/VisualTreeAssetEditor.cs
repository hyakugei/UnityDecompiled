using System;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace UnityEditor.Experimental.UIElements
{
	[CustomEditor(typeof(VisualTreeAsset))]
	internal class VisualTreeAssetEditor : Editor
	{
		private Panel m_Panel;

		private VisualElement m_Tree;

		private VisualTreeAsset m_LastTree;

		private Texture2D m_FileTypeIcon;

		internal override string targetTitle
		{
			get
			{
				if (!base.target)
				{
					base.serializedObject.Update();
					base.InternalSetTargets(base.serializedObject.targetObjects);
				}
				return base.targetTitle;
			}
		}

		protected void OnEnable()
		{
			this.m_FileTypeIcon = EditorGUIUtility.FindTexture("UxmlScript Icon");
		}

		public override bool HasPreviewGUI()
		{
			return true;
		}

		public override GUIContent GetPreviewTitle()
		{
			return GUIContent.Temp(this.targetTitle);
		}

		private void RenderIcon(Rect iconRect)
		{
			GUI.DrawTexture(iconRect, this.m_FileTypeIcon, ScaleMode.ScaleToFit);
		}

		public void Render(VisualTreeAsset vta, Rect r, GUIStyle background)
		{
			if (Event.current.type == EventType.Repaint && (r.width >= 100f || r.height >= 100f))
			{
				bool flag = false;
				if (vta != this.m_LastTree || !this.m_LastTree)
				{
					this.m_LastTree = vta;
					this.m_Tree = vta.CloneTree(null);
					this.m_Tree.StretchToParentSize();
					flag = true;
				}
				if (this.m_Panel == null)
				{
					UXMLEditorFactories.RegisterAll();
					this.m_Panel = UIElementsUtility.FindOrCreatePanel(this.m_LastTree, ContextType.Editor, new DataWatchService());
					if (this.m_Panel.visualTree.styleSheets == null)
					{
						UIElementsEditorUtility.AddDefaultEditorStyleSheets(this.m_Panel.visualTree);
						this.m_Panel.visualTree.LoadStyleSheetsFromPaths();
					}
					this.m_Panel.allowPixelCaching = false;
					flag = true;
				}
				if (flag)
				{
					this.m_Panel.visualTree.Clear();
					this.m_Panel.visualTree.Add(this.m_Tree);
				}
				EditorGUI.DrawRect(r, (!EditorGUIUtility.isProSkin) ? HostView.kViewColor : EditorGUIUtility.kDarkViewBackground);
				this.m_Panel.visualTree.layout = GUIClip.UnclipToWindow(r);
				this.m_Panel.visualTree.Dirty(ChangeType.Layout);
				this.m_Panel.visualTree.Dirty(ChangeType.Repaint);
				SavedGUIState savedGUIState = SavedGUIState.Create();
				for (int i = GUIClip.Internal_GetCount(); i > 0; i--)
				{
					GUIClip.Pop();
				}
				this.m_Panel.Repaint(Event.current);
				savedGUIState.ApplyAndForget();
			}
		}

		public override void OnPreviewGUI(Rect r, GUIStyle background)
		{
			base.OnPreviewGUI(r, background);
			if (r.width > 64f || r.height > 64f)
			{
				this.Render(base.target as VisualTreeAsset, r, background);
			}
			else
			{
				this.RenderIcon(r);
			}
		}
	}
}
