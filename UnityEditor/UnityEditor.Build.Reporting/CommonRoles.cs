using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEditor.Build.Reporting
{
	[NativeType(Header = "Modules/BuildReportingEditor/Public/CommonRoles.h")]
	public static class CommonRoles
	{
		public static extern string scene
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public static extern string sharedAssets
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public static extern string resourcesFile
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public static extern string assetBundle
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public static extern string manifestAssetBundle
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public static extern string assetBundleTextManifest
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public static extern string managedLibrary
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public static extern string dependentManagedLibrary
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public static extern string executable
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public static extern string streamingResourceFile
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public static extern string streamingAsset
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public static extern string bootConfig
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public static extern string builtInResources
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public static extern string builtInShaders
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public static extern string appInfo
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public static extern string managedEngineApi
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public static extern string monoRuntime
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public static extern string monoConfig
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public static extern string debugInfo
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public static extern string globalGameManagers
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public static extern string crashHandler
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public static extern string engineLibrary
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}
	}
}
