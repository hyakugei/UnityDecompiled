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
	public class AssetLoadInfo
	{
		[NativeName("asset")]
		internal GUID m_Asset;

		[NativeName("address")]
		internal string m_Address;

		[NativeName("processedScene")]
		internal string m_ProcessedScene;

		[NativeName("includedObjects")]
		internal List<ObjectIdentifier> m_IncludedObjects;

		[NativeName("referencedObjects")]
		internal List<ObjectIdentifier> m_ReferencedObjects;

		public GUID asset
		{
			get
			{
				return this.m_Asset;
			}
			set
			{
				this.m_Asset = value;
			}
		}

		public string address
		{
			get
			{
				return this.m_Address;
			}
			set
			{
				this.m_Address = value;
			}
		}

		public string processedScene
		{
			get
			{
				return this.m_ProcessedScene;
			}
			set
			{
				this.m_ProcessedScene = value;
			}
		}

		public List<ObjectIdentifier> includedObjects
		{
			get
			{
				return this.m_IncludedObjects;
			}
			set
			{
				this.m_IncludedObjects = value;
			}
		}

		public List<ObjectIdentifier> referencedObjects
		{
			get
			{
				return this.m_ReferencedObjects;
			}
			set
			{
				this.m_ReferencedObjects = value;
			}
		}
	}
}
