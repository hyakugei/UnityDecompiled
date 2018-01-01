using System;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEditor.Experimental.Build.AssetBundle
{
	[UsedByNativeCode]
	[Serializable]
	public struct BuildInput
	{
		[UsedByNativeCode]
		[Serializable]
		public struct Definition
		{
			[NativeName("assetBundleName")]
			internal string m_AssetBundleName;

			[NativeName("explicitAssets")]
			internal AssetIdentifier[] m_ExplicitAssets;

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

			public AssetIdentifier[] explicitAssets
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
		}

		[NativeName("definitions")]
		internal BuildInput.Definition[] m_Definitions;

		public BuildInput.Definition[] definitions
		{
			get
			{
				return this.m_Definitions;
			}
			set
			{
				this.m_Definitions = value;
			}
		}
	}
}
