using System;
using System.Runtime.InteropServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEditor.Experimental.Build.AssetBundle
{
	[UsedByNativeCode]
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class SerializationInfo
	{
		[NativeName("serializationObject")]
		internal ObjectIdentifier m_SerializationObject;

		[NativeName("serializationIndex")]
		internal long m_SerializationIndex;

		public ObjectIdentifier serializationObject
		{
			get
			{
				return this.m_SerializationObject;
			}
			set
			{
				this.m_SerializationObject = value;
			}
		}

		public long serializationIndex
		{
			get
			{
				return this.m_SerializationIndex;
			}
			set
			{
				this.m_SerializationIndex = value;
			}
		}
	}
}
