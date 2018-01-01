using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityEditor
{
	public class PlatformIcon
	{
		internal List<Texture2D> m_Textures;

		private PlatformIconKind m_Kind;

		private int m_MaxLayerCount;

		private int m_MinLayerCount;

		private string m_Description;

		private string m_iconSubKind;

		private int m_Width;

		private int m_Height;

		public int layerCount
		{
			get
			{
				return this.m_Textures.Count;
			}
			set
			{
				value = ((value <= this.m_MaxLayerCount) ? value : this.m_MaxLayerCount);
				value = ((value >= this.m_MinLayerCount) ? value : this.m_MinLayerCount);
				if (value < this.m_Textures.Count)
				{
					this.m_Textures.RemoveRange(value, this.m_Textures.Count - 1);
				}
				else if (value > this.m_Textures.Count)
				{
					this.m_Textures.AddRange(new Texture2D[value - this.m_Textures.Count]);
				}
			}
		}

		public int maxLayerCount
		{
			get
			{
				return this.m_MaxLayerCount;
			}
			private set
			{
				this.m_MaxLayerCount = value;
			}
		}

		public int minLayerCount
		{
			get
			{
				return this.m_MinLayerCount;
			}
			private set
			{
				this.m_MinLayerCount = value;
			}
		}

		internal string description
		{
			get
			{
				return this.m_Description;
			}
			private set
			{
				this.m_Description = value;
			}
		}

		internal string iconSubKind
		{
			get
			{
				return this.m_iconSubKind;
			}
			private set
			{
				this.m_iconSubKind = value;
			}
		}

		public int width
		{
			get
			{
				return this.m_Width;
			}
			private set
			{
				this.m_Width = value;
			}
		}

		public int height
		{
			get
			{
				return this.m_Height;
			}
			private set
			{
				this.m_Height = value;
			}
		}

		public PlatformIconKind kind
		{
			get
			{
				return this.m_Kind;
			}
			private set
			{
				this.m_Kind = value;
			}
		}

		internal PlatformIcon(int width, int height, int minLayerCount, int maxLayerCount, string iconSubKind, string description, PlatformIconKind kind)
		{
			this.width = width;
			this.height = height;
			this.iconSubKind = iconSubKind;
			this.description = description;
			this.minLayerCount = minLayerCount;
			this.maxLayerCount = maxLayerCount;
			this.kind = kind;
			this.m_Textures = new List<Texture2D>();
		}

		internal PlatformIconStruct GetPlatformIconStruct()
		{
			return new PlatformIconStruct
			{
				m_Textures = this.m_Textures.ToArray(),
				m_Width = this.m_Width,
				m_Height = this.m_Height,
				m_Kind = this.m_Kind.kind,
				m_SubKind = this.m_iconSubKind
			};
		}

		internal bool IsEmpty()
		{
			return this.m_Textures.Count((Texture2D t) => t != null) == 0;
		}

		internal static PlatformIcon[] GetRequiredPlatformIconsByType(IPlatformIconProvider platformIcons, PlatformIconKind kind)
		{
			Dictionary<PlatformIconKind, PlatformIcon[]> requiredPlatformIcons = platformIcons.GetRequiredPlatformIcons();
			PlatformIcon[] result;
			if (kind != PlatformIconKind.Any)
			{
				result = requiredPlatformIcons[kind];
			}
			else
			{
				result = requiredPlatformIcons.Values.SelectMany((PlatformIcon[] i) => i).ToArray<PlatformIcon>();
			}
			return result;
		}

		public Texture2D GetTexture(int layer = 0)
		{
			if (layer < 0 || layer >= this.m_MaxLayerCount)
			{
				throw new ArgumentOutOfRangeException(string.Format("Attempting to retrieve icon layer {0}, while the icon only contains {1} layers!", layer, this.layerCount));
			}
			Texture2D result;
			if (layer < this.layerCount)
			{
				result = this.m_Textures[layer];
			}
			else
			{
				result = null;
			}
			return result;
		}

		public Texture2D[] GetTextures()
		{
			return this.m_Textures.ToArray();
		}

		internal Texture2D[] GetPreviewTextures()
		{
			Texture2D[] array = new Texture2D[this.maxLayerCount];
			for (int i = 0; i < this.maxLayerCount; i++)
			{
				array[i] = PlayerSettings.GetPlatformIconAtSize(this.m_Kind.platform, this.m_Width, this.m_Height, this.m_Kind.kind, this.m_iconSubKind, i);
			}
			return array;
		}

		public void SetTexture(Texture2D texture, int layer = 0)
		{
			if (layer < 0 || layer >= this.maxLayerCount)
			{
				throw new ArgumentOutOfRangeException(string.Format("Attempting to set icon layer {0}, while icon only supports {1} layers!", layer, this.maxLayerCount));
			}
			if (layer > this.m_Textures.Count - 1)
			{
				for (int i = this.m_Textures.Count; i <= layer; i++)
				{
					this.m_Textures.Add(null);
				}
			}
			this.m_Textures[layer] = texture;
		}

		public void SetTextures(params Texture2D[] textures)
		{
			if (textures != null && textures.Length != 0)
			{
				if (textures.Count((Texture2D t) => t != null) != 0)
				{
					if (textures.Length > this.maxLayerCount || textures.Length < this.minLayerCount)
					{
						throw new InvalidOperationException(string.Format("Attempting to assign an incorrect amount of layers to an PlatformIcon, trying to assign {0} textures while the Icon requires atleast {1} but no more than {2} layers", textures.Length, this.minLayerCount, this.maxLayerCount));
					}
					this.m_Textures = textures.ToList<Texture2D>();
					return;
				}
			}
			this.m_Textures.Clear();
		}

		public override string ToString()
		{
			return string.Format("({0}x{1}) {2}", this.width, this.height, this.description);
		}
	}
}
