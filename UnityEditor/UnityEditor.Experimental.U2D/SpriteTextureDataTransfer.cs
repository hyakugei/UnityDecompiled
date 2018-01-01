using System;
using UnityEngine;

namespace UnityEditor.Experimental.U2D
{
	internal class SpriteTextureDataTransfer : SpriteDataProviderBase, ITextureDataProvider
	{
		private Texture2D m_ReadableTexture;

		private Texture2D m_OriginalTexture;

		public Texture2D texture
		{
			get
			{
				if (this.m_OriginalTexture == null)
				{
					this.m_OriginalTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(base.dataProvider.assetPath);
				}
				return this.m_OriginalTexture;
			}
		}

		public SpriteTextureDataTransfer(TextureImporter dp) : base(dp)
		{
		}

		public void GetTextureActualWidthAndHeight(out int width, out int height)
		{
			width = (height = 0);
			base.dataProvider.GetWidthAndHeight(ref width, ref height);
		}

		public Texture2D GetReadableTexture2D()
		{
			if (this.m_ReadableTexture == null)
			{
				int width = 0;
				int height = 0;
				this.GetTextureActualWidthAndHeight(out width, out height);
				this.m_ReadableTexture = UnityEditor.SpriteUtility.CreateTemporaryDuplicate(this.texture, width, height);
				if (this.m_ReadableTexture != null)
				{
					this.m_ReadableTexture.filterMode = this.texture.filterMode;
				}
			}
			return this.m_ReadableTexture;
		}
	}
}
