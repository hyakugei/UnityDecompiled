using System;
using UnityEngine;

namespace UnityEditor.U2D.Interface
{
	internal class AssetDatabaseSystem : IAssetDatabase
	{
		public string GetAssetPath(UnityEngine.Object o)
		{
			return AssetDatabase.GetAssetPath(o);
		}

		public AssetImporter GetAssetImporterFromPath(string path)
		{
			return AssetImporter.GetAtPath(path);
		}
	}
}
