using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor.Experimental.AssetImporters
{
	public struct SpriteImportData
	{
		private string m_Name;

		private Rect m_Rect;

		private SpriteAlignment m_Alignment;

		private Vector2 m_Pivot;

		private Vector4 m_Border;

		private float m_TessellationDetail;

		private string m_SpriteID;

		private List<Vector2[]> m_Outline;

		public string name
		{
			get
			{
				return this.m_Name;
			}
			set
			{
				this.m_Name = value;
			}
		}

		public Rect rect
		{
			get
			{
				return this.m_Rect;
			}
			set
			{
				this.m_Rect = value;
			}
		}

		public SpriteAlignment alignment
		{
			get
			{
				return this.m_Alignment;
			}
			set
			{
				this.m_Alignment = value;
			}
		}

		public Vector2 pivot
		{
			get
			{
				return this.m_Pivot;
			}
			set
			{
				this.m_Pivot = value;
			}
		}

		public Vector4 border
		{
			get
			{
				return this.m_Border;
			}
			set
			{
				this.m_Border = value;
			}
		}

		public List<Vector2[]> outline
		{
			get
			{
				return this.m_Outline;
			}
			set
			{
				this.m_Outline = value;
			}
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
			}
		}

		public string spriteID
		{
			get
			{
				return this.m_SpriteID;
			}
			set
			{
				this.m_SpriteID = value;
			}
		}
	}
}
