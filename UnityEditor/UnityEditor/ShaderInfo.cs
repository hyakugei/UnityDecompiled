using System;
using UnityEngine;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEditor
{
	[RequiredByNativeCode]
	[Serializable]
	public struct ShaderInfo
	{
		[NativeName("name"), SerializeField]
		internal string m_Name;

		[NativeName("supported"), SerializeField]
		internal bool m_Supported;

		[NativeName("hasErrors"), SerializeField]
		internal bool m_HasErrors;

		public string name
		{
			get
			{
				return this.m_Name;
			}
		}

		public bool supported
		{
			get
			{
				return this.m_Supported;
			}
		}

		public bool hasErrors
		{
			get
			{
				return this.m_HasErrors;
			}
		}
	}
}
