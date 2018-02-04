using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	internal static class AssetBundleUtility
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void PatchAssetBundles(AssetBundle[] bundles, string[] filenames);
	}
}
