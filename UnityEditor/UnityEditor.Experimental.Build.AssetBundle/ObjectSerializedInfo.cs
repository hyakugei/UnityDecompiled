using System;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEditor.Experimental.Build.AssetBundle
{
	[UsedByNativeCode]
	[Serializable]
	public struct ObjectSerializedInfo
	{
		[NativeName("serializedObject")]
		internal ObjectIdentifier m_SerializedObject;

		[NativeName("header")]
		internal SerializedLocation m_Header;

		[NativeName("rawData")]
		internal SerializedLocation m_RawData;

		public ObjectIdentifier serializedObject
		{
			get
			{
				return this.m_SerializedObject;
			}
		}

		public SerializedLocation header
		{
			get
			{
				return this.m_Header;
			}
		}

		public SerializedLocation rawData
		{
			get
			{
				return this.m_RawData;
			}
		}
	}
}
