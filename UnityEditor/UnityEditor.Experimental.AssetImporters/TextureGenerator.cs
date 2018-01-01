using System;
using System.Runtime.CompilerServices;

namespace UnityEditor.Experimental.AssetImporters
{
	public static class TextureGenerator
	{
		public static TextureGenerationOutput GenerateTexture(TextureGenerationSettings settings)
		{
			TextureGenerationOutput result;
			TextureGenerator.GenerateTexture_Injected(ref settings, out result);
			return result;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GenerateTexture_Injected(ref TextureGenerationSettings settings, out TextureGenerationOutput ret);
	}
}
