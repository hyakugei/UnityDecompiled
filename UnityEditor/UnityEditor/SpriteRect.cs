using System;
using UnityEngine;

namespace UnityEditor
{
	[Serializable]
	public class SpriteRect
	{
		[SerializeField]
		private string m_Name;

		[SerializeField]
		private string m_OriginalName;

		[SerializeField]
		private Vector2 m_Pivot;

		[SerializeField]
		private SpriteAlignment m_Alignment;

		[SerializeField]
		private Vector4 m_Border;

		[SerializeField]
		private Rect m_Rect;

		[SerializeField]
		private string m_SpriteID;

		private GUID m_GUID;

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

		internal string originalName
		{
			get
			{
				if (this.m_OriginalName == null)
				{
					this.m_OriginalName = this.name;
				}
				return this.m_OriginalName;
			}
			set
			{
				this.m_OriginalName = value;
			}
		}

		public GUID spriteID
		{
			get
			{
				this.ValidateGUID();
				return this.m_GUID;
			}
			set
			{
				this.m_GUID = value;
				this.m_SpriteID = this.m_GUID.ToString();
				this.ValidateGUID();
			}
		}

		private void ValidateGUID()
		{
			if (this.m_GUID.Empty())
			{
				this.m_GUID = new GUID(this.m_SpriteID);
				if (this.m_GUID.Empty())
				{
					this.m_GUID = GUID.Generate();
					this.m_SpriteID = this.m_GUID.ToString();
				}
			}
		}

		public static GUID GetSpriteIDFromSerializedProperty(SerializedProperty sp)
		{
			return new GUID(sp.FindPropertyRelative("m_SpriteID").stringValue);
		}
	}
}
