using System;
using System.Collections.ObjectModel;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEditor.Experimental.Build.AssetBundle
{
	[UsedByNativeCode]
	[Serializable]
	public struct BuildOutput
	{
		[NativeName("results")]
		internal WriteResult[] m_Results;

		public ReadOnlyCollection<WriteResult> results
		{
			get
			{
				return Array.AsReadOnly<WriteResult>(this.m_Results);
			}
		}
	}
}
