using System;
using System.IO;
using UnityEditor.Experimental.AssetImporters;
using UnityEngine;
using UnityEngine.Experimental.UIElements.StyleSheets;
using UnityEngine.StyleSheets;

namespace UnityEditor.StyleSheets
{
	[ScriptedImporter(3, "uss", -20)]
	internal class StyleSheetImporter : ScriptedImporter
	{
		public override void OnImportAsset(AssetImportContext ctx)
		{
			StyleSheetImporterImpl styleSheetImporterImpl = new StyleSheetImporterImpl();
			string contents = File.ReadAllText(ctx.assetPath);
			StyleSheet styleSheet = ScriptableObject.CreateInstance<StyleSheet>();
			styleSheet.hideFlags = HideFlags.NotEditable;
			if (styleSheetImporterImpl.Import(styleSheet, contents))
			{
				ctx.AddObjectToAsset("stylesheet", styleSheet);
				StyleSheetAssetPostprocessor.ClearReferencedAssets();
				StyleContext.ClearStyleCache();
			}
			else
			{
				foreach (string current in styleSheetImporterImpl.errors.FormatErrors())
				{
					Debug.LogError(current);
				}
			}
		}
	}
}
