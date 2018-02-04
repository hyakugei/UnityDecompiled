using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
	internal class SpriteRectCache : ScriptableObject
	{
		[SerializeField]
		public List<SpriteRect> m_Rects;

		public int Count
		{
			get
			{
				return (this.m_Rects == null) ? 0 : this.m_Rects.Count;
			}
		}

		public SpriteRect RectAt(int i)
		{
			return (i < this.Count && i >= 0) ? this.m_Rects[i] : null;
		}

		public void AddRect(SpriteRect r)
		{
			if (this.m_Rects != null)
			{
				this.m_Rects.Add(r);
			}
		}

		public void RemoveRect(SpriteRect r)
		{
			if (this.m_Rects != null)
			{
				this.m_Rects.RemoveAll((SpriteRect x) => x.spriteID == r.spriteID);
			}
		}

		public void ClearAll()
		{
			if (this.m_Rects != null)
			{
				this.m_Rects.Clear();
			}
		}

		public int GetIndex(SpriteRect spriteRect)
		{
			int result;
			if (this.m_Rects != null && spriteRect != null)
			{
				result = this.m_Rects.FindIndex((SpriteRect p) => p.spriteID == spriteRect.spriteID);
			}
			else
			{
				result = -1;
			}
			return result;
		}

		public bool Contains(SpriteRect spriteRect)
		{
			return this.m_Rects != null && spriteRect != null && this.m_Rects.Find((SpriteRect x) => x.spriteID == spriteRect.spriteID) != null;
		}

		private void OnEnable()
		{
			if (this.m_Rects == null)
			{
				this.m_Rects = new List<SpriteRect>();
			}
		}
	}
}
