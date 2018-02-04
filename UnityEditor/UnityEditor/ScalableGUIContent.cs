using System;
using System.Collections.Generic;
using UnityEditor.StyleSheets;
using UnityEngine;

namespace UnityEditor
{
	[Serializable]
	internal class ScalableGUIContent
	{
		[Serializable]
		private struct TextureResource
		{
			public float pixelsPerPoint;

			public string resourcePath;
		}

		[SerializeField]
		private List<ScalableGUIContent.TextureResource> m_TextureResources = new List<ScalableGUIContent.TextureResource>(2);

		[SerializeField]
		private string m_CurrentResourcePath;

		[SerializeField]
		private GUIContent m_GuiContent;

		public ScalableGUIContent(string resourceName) : this(string.Empty, string.Empty, resourceName)
		{
		}

		public ScalableGUIContent(string text, string tooltip, string resourceName)
		{
			this.m_GuiContent = ((string.IsNullOrEmpty(text) && string.IsNullOrEmpty(tooltip)) ? new GUIContent() : EditorGUIUtility.TextContent(string.Format("{0}|{1}", text, tooltip)));
			this.m_TextureResources.Add(new ScalableGUIContent.TextureResource
			{
				pixelsPerPoint = 1f,
				resourcePath = resourceName
			});
			this.m_TextureResources.Add(new ScalableGUIContent.TextureResource
			{
				pixelsPerPoint = 2f,
				resourcePath = string.Format("{0}@2x", resourceName)
			});
		}

		public static implicit operator GUIContent(ScalableGUIContent gc)
		{
			float pixelsPerPoint = EditorGUIUtility.pixelsPerPoint;
			string text = gc.m_CurrentResourcePath;
			int i = 0;
			int count = gc.m_TextureResources.Count;
			while (i < count)
			{
				if (gc.m_TextureResources[i].pixelsPerPoint > pixelsPerPoint)
				{
					break;
				}
				text = gc.m_TextureResources[i].resourcePath;
				i++;
			}
			if (text != gc.m_CurrentResourcePath)
			{
				gc.m_GuiContent.image = (StyleSheetResourceUtil.LoadResource(text, typeof(Texture2D), false) as Texture2D);
				gc.m_CurrentResourcePath = text;
			}
			return gc.m_GuiContent;
		}
	}
}
