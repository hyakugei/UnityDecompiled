using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEditor
{
	[NativeType(CodegenOptions.Custom, "MonoPackageInfo", Header = "Editor/Src/PackageUtility.h")]
	public struct PackageInfo
	{
		public string packagePath;

		public string jsonInfo;

		public string iconURL;

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern PackageInfo[] GetPackageList();
	}
}
