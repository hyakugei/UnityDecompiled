using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityEditor.U2D
{
	internal class SpriteOutlineModel : ScriptableObject
	{
		[SerializeField]
		private List<SpriteOutlineList> m_SpriteOutlineList = new List<SpriteOutlineList>();

		public SpriteOutlineList this[int index]
		{
			get
			{
				return (!this.IsValidIndex(index)) ? null : this.m_SpriteOutlineList[index];
			}
			set
			{
				if (this.IsValidIndex(index))
				{
					this.m_SpriteOutlineList[index] = value;
				}
			}
		}

		public SpriteOutlineList this[GUID guid]
		{
			get
			{
				return this.m_SpriteOutlineList.FirstOrDefault((SpriteOutlineList x) => x.spriteID == guid);
			}
			set
			{
				int num = this.m_SpriteOutlineList.FindIndex((SpriteOutlineList x) => x.spriteID == guid);
				if (num != -1)
				{
					this.m_SpriteOutlineList[num] = value;
				}
			}
		}

		public int Count
		{
			get
			{
				return this.m_SpriteOutlineList.Count;
			}
		}

		private SpriteOutlineModel()
		{
		}

		public void AddListVector2(GUID guid, List<Vector2[]> outline)
		{
			this.m_SpriteOutlineList.Add(new SpriteOutlineList(guid, outline));
		}

		private bool IsValidIndex(int index)
		{
			return index >= 0 && index < this.Count;
		}
	}
}
