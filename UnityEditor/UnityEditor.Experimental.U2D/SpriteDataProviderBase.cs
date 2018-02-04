using System;

namespace UnityEditor.Experimental.U2D
{
	internal class SpriteDataProviderBase
	{
		protected TextureImporter dataProvider
		{
			get;
			private set;
		}

		public SpriteDataProviderBase(TextureImporter dp)
		{
			this.dataProvider = dp;
		}
	}
}
