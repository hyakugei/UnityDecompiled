using System;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEditor.Experimental.Build.AssetBundle
{
	[UsedByNativeCode]
	[Serializable]
	public struct SerializedLocation
	{
		[NativeName("fileName")]
		internal string m_FileName;

		[NativeName("offset")]
		internal ulong m_Offset;

		[NativeName("size")]
		internal ulong m_Size;

		public string fileName
		{
			get
			{
				return this.m_FileName;
			}
		}

		public ulong offset
		{
			get
			{
				return this.m_Offset;
			}
		}

		public ulong size
		{
			get
			{
				return this.m_Size;
			}
		}
	}
}
