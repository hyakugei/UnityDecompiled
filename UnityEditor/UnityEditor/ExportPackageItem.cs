using System;
using System.Runtime.InteropServices;
using UnityEngine.Bindings;

namespace UnityEditor
{
	[NativeType(CodegenOptions.Custom, "MonoExportPackageItem", Header = "Editor/Mono/PackageUtility.bindings.h")]
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	internal class ExportPackageItem
	{
		public string assetPath;

		public string guid;

		public bool isFolder;

		public int enabledStatus;
	}
}
