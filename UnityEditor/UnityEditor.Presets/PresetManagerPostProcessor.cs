using System;

namespace UnityEditor.Presets
{
	internal class PresetManagerPostProcessor : AssetPostprocessor
	{
		public override int GetPostprocessOrder()
		{
			return -1000;
		}

		public void OnPreprocessAsset()
		{
			if (base.assetImporter.importSettingsMissing)
			{
				Preset defaultForObject = Preset.GetDefaultForObject(base.assetImporter);
				if (defaultForObject != null)
				{
					defaultForObject.ApplyTo(base.assetImporter);
				}
			}
		}
	}
}
