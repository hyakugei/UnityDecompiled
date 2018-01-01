using System;
using UnityEngine;

namespace UnityEditor
{
	[CustomEditor(typeof(AssetImportInProgressProxy))]
	internal class AssetImportInProgressProxyEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			AssetImportInProgressProxy assetImportInProgressProxy = (AssetImportInProgressProxy)base.target;
			if (GUILayout.Button("Import", new GUILayoutOption[0]))
			{
				UnityEngine.Object activeObject = AssetDatabase.LoadMainAssetAtGUID(assetImportInProgressProxy.asset);
				Selection.activeObject = activeObject;
			}
		}
	}
}
