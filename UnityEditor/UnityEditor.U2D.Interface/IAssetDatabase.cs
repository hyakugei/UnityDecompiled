using System;
using UnityEngine;

namespace UnityEditor.U2D.Interface
{
	internal interface IAssetDatabase
	{
		string GetAssetPath(UnityEngine.Object o);

		AssetImporter GetAssetImporterFromPath(string path);
	}
}
