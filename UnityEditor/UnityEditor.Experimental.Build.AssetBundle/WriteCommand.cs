using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEditor.Experimental.Build.AssetBundle
{
	[NativeType(CodegenOptions = CodegenOptions.Custom), UsedByNativeCode]
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class WriteCommand
	{
		[NativeName("assetBundleName")]
		internal string m_AssetBundleName;

		[NativeName("explicitAssets")]
		internal List<AssetLoadInfo> m_ExplicitAssets;

		[NativeName("assetBundleObjects")]
		internal List<SerializationInfo> m_AssetBundleObjects;

		[NativeName("assetBundleDependencies")]
		internal List<string> m_AssetBundleDependencies;

		[NativeName("sceneBundle")]
		internal bool m_SceneBundle;

		[NativeName("globalUsage")]
		internal BuildUsageTagGlobal m_GlobalUsage;

		public string assetBundleName
		{
			get
			{
				return this.m_AssetBundleName;
			}
			set
			{
				this.m_AssetBundleName = value;
			}
		}

		public List<AssetLoadInfo> explicitAssets
		{
			get
			{
				return this.m_ExplicitAssets;
			}
			set
			{
				this.m_ExplicitAssets = value;
			}
		}

		public List<SerializationInfo> assetBundleObjects
		{
			get
			{
				return this.m_AssetBundleObjects;
			}
			set
			{
				this.m_AssetBundleObjects = value;
			}
		}

		public List<string> assetBundleDependencies
		{
			get
			{
				return this.m_AssetBundleDependencies;
			}
			set
			{
				this.m_AssetBundleDependencies = value;
			}
		}

		public bool sceneBundle
		{
			get
			{
				return this.m_SceneBundle;
			}
			set
			{
				this.m_SceneBundle = value;
			}
		}

		public BuildUsageTagGlobal globalUsage
		{
			get
			{
				return this.m_GlobalUsage;
			}
			set
			{
				this.m_GlobalUsage = value;
			}
		}
	}
}
