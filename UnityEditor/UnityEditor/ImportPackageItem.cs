using System;
using System.Runtime.InteropServices;
using UnityEngine.Bindings;

namespace UnityEditor
{
	[NativeType(CodegenOptions.Custom, "MonoImportPackageItem", Header = "Editor/Mono/PackageUtility.bindings.h")]
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	internal class ImportPackageItem
	{
		public string exportedAssetPath;

		public string destinationAssetPath;

		public string sourceFolder;

		public string previewPath;

		public string guid;

		public int enabledStatus;

		public bool isFolder;

		public bool exists;

		public bool assetChanged;

		public bool pathConflict;

		public bool projectAsset;
	}
}
