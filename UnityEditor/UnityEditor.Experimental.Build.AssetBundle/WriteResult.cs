using System;
using System.Collections.ObjectModel;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEditor.Experimental.Build.AssetBundle
{
	[UsedByNativeCode]
	[Serializable]
	public struct WriteResult
	{
		[NativeName("assetBundleName")]
		internal string m_AssetBundleName;

		[NativeName("assetBundleAssets")]
		internal GUID[] m_AssetBundleAssets;

		[NativeName("assetBundleObjects")]
		internal ObjectSerializedInfo[] m_AssetBundleObjects;

		[NativeName("resourceFiles")]
		internal ResourceFile[] m_ResourceFiles;

		[NativeName("includedTypes")]
		internal Type[] m_IncludedTypes;

		public string assetBundleName
		{
			get
			{
				return this.m_AssetBundleName;
			}
		}

		public ReadOnlyCollection<GUID> assetBundleAssets
		{
			get
			{
				return Array.AsReadOnly<GUID>(this.m_AssetBundleAssets);
			}
		}

		public ReadOnlyCollection<ObjectSerializedInfo> assetBundleObjects
		{
			get
			{
				return Array.AsReadOnly<ObjectSerializedInfo>(this.m_AssetBundleObjects);
			}
		}

		public ReadOnlyCollection<ResourceFile> resourceFiles
		{
			get
			{
				return Array.AsReadOnly<ResourceFile>(this.m_ResourceFiles);
			}
		}

		public ReadOnlyCollection<Type> includedTypes
		{
			get
			{
				return Array.AsReadOnly<Type>(this.m_IncludedTypes);
			}
		}
	}
}
