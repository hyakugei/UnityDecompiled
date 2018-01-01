using System;
using UnityEngine;
using UnityEngine.StyleSheets;

namespace UnityEditor.StyleSheets
{
	[CustomEditor(typeof(StyleSheet))]
	internal class StyleSheetEditor : Editor
	{
		private Texture2D m_FileTypeIcon;

		protected void OnEnable()
		{
			this.m_FileTypeIcon = EditorGUIUtility.FindTexture("UssScript Icon");
		}

		public override bool HasPreviewGUI()
		{
			return true;
		}

		private void RenderIcon(Rect iconRect)
		{
			GUI.DrawTexture(iconRect, this.m_FileTypeIcon, ScaleMode.ScaleToFit);
		}

		public override void OnPreviewGUI(Rect r, GUIStyle background)
		{
			base.OnPreviewGUI(r, background);
			if (r.width > 64f || r.height > 64f)
			{
				base.OnPreviewGUI(r, background);
			}
			else
			{
				this.RenderIcon(r);
			}
		}
	}
}
