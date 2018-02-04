using System;
using UnityEngine;

namespace UnityEditor.Experimental.U2D
{
	public interface ITextureDataProvider
	{
		Texture2D texture
		{
			get;
		}

		void GetTextureActualWidthAndHeight(out int width, out int height);

		Texture2D GetReadableTexture2D();
	}
}
