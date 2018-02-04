using System;
using UnityEngine;

namespace UnityEditor.PlatformSupport
{
	internal class PlatformIconFieldSingleLayer : PlatformIconField
	{
		private bool m_ShowSizeLabel = true;

		private string m_PlatformName;

		internal PlatformIconFieldSingleLayer(PlatformIcon platformIcon, BuildTargetGroup targetGroup) : base(platformIcon, targetGroup)
		{
			this.m_PlatformName = PlayerSettings.GetPlatformName(this.m_TargetGroup);
		}

		private void DrawHeader(Rect headerRect)
		{
			if (Event.current.type == EventType.Repaint)
			{
				this.s_Defaults.DrawHeaderBackground(headerRect);
			}
			headerRect.xMin += 6f;
			headerRect.xMax -= 6f;
			headerRect.height -= 2f;
			headerRect.y += 1f;
			this.m_ShowSizeLabel = (headerRect.width > EditorGUIUtility.labelWidth + 64f + 8f + 24f);
			string text = this.m_HeaderString;
			if (!this.m_ShowSizeLabel)
			{
				text += string.Format("({0})", this.m_SizeLabel);
			}
			GUI.Label(headerRect, LocalizationDatabase.GetLocalizedString(text), EditorStyles.label);
		}

		private void DrawElement(Rect elementRect)
		{
			int num = 86;
			int num2 = (int)((float)base.platformIcon.height / (float)base.platformIcon.height * (float)num);
			int num3 = Mathf.Min(num, base.platformIcon.width);
			int num4 = (int)((float)base.platformIcon.height * (float)num3 / (float)base.platformIcon.width);
			if (Event.current.type == EventType.Repaint)
			{
				this.s_Defaults.boxBackground.Draw(elementRect, false, false, false, false);
			}
			float num5 = Mathf.Min(elementRect.width, EditorGUIUtility.labelWidth + 4f + 64f + 8f + 86f);
			Rect contentRect = PlatformIconField.GetContentRect(elementRect, 6f, 12f);
			Rect position = new Rect(contentRect.x + contentRect.width - 86f - (float)num - 8f, contentRect.y, (float)num, (float)num2);
			Texture2D texture = (Texture2D)EditorGUI.ObjectField(position, base.platformIcon.GetTexture(0), typeof(Texture2D), false);
			base.platformIcon.SetTexture(texture, 0);
			Rect rect = new Rect(contentRect.x + contentRect.width - 86f, contentRect.y, (float)num3, (float)num4);
			GUI.Box(rect, "");
			Texture2D platformIconAtSize = PlayerSettings.GetPlatformIconAtSize(this.m_PlatformName, base.platformIcon.width, base.platformIcon.height, base.platformIcon.kind.kind, base.platformIcon.iconSubKind, 0);
			if (platformIconAtSize != null)
			{
				GUI.DrawTexture(PlatformIconField.GetContentRect(rect, 1f, 1f), platformIconAtSize);
			}
			if (this.m_ShowSizeLabel)
			{
				GUI.Label(new Rect(contentRect.x, contentRect.y, num5 - 64f - 8f, 20f), this.m_SizeLabel);
			}
		}

		internal override void DrawAt()
		{
			Rect rect = GUILayoutUtility.GetRect(0f, 124f, new GUILayoutOption[]
			{
				GUILayout.ExpandWidth(true)
			});
			Rect headerRect = new Rect(rect.x, rect.y, rect.width, 18f);
			Rect elementRect = new Rect(rect.x, rect.y + 18f, rect.width, rect.height - 18f - 8f);
			this.DrawHeader(headerRect);
			this.DrawElement(elementRect);
		}
	}
}
