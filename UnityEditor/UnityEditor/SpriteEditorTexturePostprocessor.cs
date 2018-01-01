using System;

namespace UnityEditor
{
	internal class SpriteEditorTexturePostprocessor : AssetPostprocessor
	{
		public override int GetPostprocessOrder()
		{
			return 1;
		}

		private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
		{
			for (int i = 0; i < importedAssets.Length; i++)
			{
				string path = importedAssets[i];
				SpriteEditorWindow.OnTextureReimport(path);
			}
		}
	}
}
