using System;
using UnityEngine;
using UnityEngine.Bindings;

namespace UnityEditor.Experimental.AssetImporters
{
	public struct TextureGenerationOutput
	{
		[NativeName("texture")]
		private Texture2D m_Texture;

		[NativeName("importInspectorWarnings")]
		private string m_ImportInspectorWarnings;

		[NativeName("importWarnings")]
		private string[] m_ImportWarnings;

		[NativeName("thumbNail")]
		private Texture2D m_ThumbNail;

		[NativeName("sprites")]
		private Sprite[] m_Sprites;

		public Texture2D texture
		{
			get
			{
				return this.m_Texture;
			}
		}

		public string importInspectorWarnings
		{
			get
			{
				return this.m_ImportInspectorWarnings;
			}
		}

		public string[] importWarnings
		{
			get
			{
				return this.m_ImportWarnings;
			}
		}

		public Texture2D thumbNail
		{
			get
			{
				return this.m_ThumbNail;
			}
		}

		public Sprite[] sprites
		{
			get
			{
				return this.m_Sprites;
			}
		}
	}
}
