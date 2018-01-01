using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor.U2D
{
	[Serializable]
	internal class SpriteOutlineList
	{
		[SerializeField]
		private List<SpriteOutline> m_SpriteOutlines = new List<SpriteOutline>();

		[SerializeField]
		private float m_TessellationDetail = 0f;

		public List<SpriteOutline> spriteOutlines
		{
			get
			{
				return this.m_SpriteOutlines;
			}
			set
			{
				this.m_SpriteOutlines = value;
			}
		}

		public GUID spriteID
		{
			get;
			private set;
		}

		public float tessellationDetail
		{
			get
			{
				return this.m_TessellationDetail;
			}
			set
			{
				this.m_TessellationDetail = value;
				this.m_TessellationDetail = Mathf.Min(1f, this.m_TessellationDetail);
				this.m_TessellationDetail = Mathf.Max(0f, this.m_TessellationDetail);
			}
		}

		public SpriteOutline this[int index]
		{
			get
			{
				return (!this.IsValidIndex(index)) ? null : this.m_SpriteOutlines[index];
			}
			set
			{
				if (this.IsValidIndex(index))
				{
					this.m_SpriteOutlines[index] = value;
				}
			}
		}

		public int Count
		{
			get
			{
				return this.m_SpriteOutlines.Count;
			}
		}

		public SpriteOutlineList(GUID guid)
		{
			this.spriteID = guid;
			this.m_SpriteOutlines = new List<SpriteOutline>();
		}

		public SpriteOutlineList(GUID guid, List<Vector2[]> list)
		{
			this.spriteID = guid;
			this.m_SpriteOutlines = new List<SpriteOutline>(list.Count);
			for (int i = 0; i < list.Count; i++)
			{
				SpriteOutline spriteOutline = new SpriteOutline();
				spriteOutline.m_Path.AddRange(list[i]);
				this.m_SpriteOutlines.Add(spriteOutline);
			}
		}

		public SpriteOutlineList(GUID guid, List<SpriteOutline> list)
		{
			this.spriteID = guid;
			this.m_SpriteOutlines = list;
		}

		public List<Vector2[]> ToListVector()
		{
			List<Vector2[]> list = new List<Vector2[]>(this.m_SpriteOutlines.Count);
			foreach (SpriteOutline current in this.m_SpriteOutlines)
			{
				list.Add(current.m_Path.ToArray());
			}
			return list;
		}

		public static implicit operator List<SpriteOutline>(SpriteOutlineList list)
		{
			return (list == null) ? null : list.m_SpriteOutlines;
		}

		private bool IsValidIndex(int index)
		{
			return index >= 0 && index < this.Count;
		}
	}
}
