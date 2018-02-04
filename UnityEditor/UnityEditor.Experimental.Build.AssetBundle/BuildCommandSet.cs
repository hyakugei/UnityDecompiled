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
	public class BuildCommandSet
	{
		[NativeName("commands")]
		internal List<WriteCommand> m_Commands;

		public List<WriteCommand> commands
		{
			get
			{
				return this.m_Commands;
			}
			set
			{
				this.m_Commands = value;
			}
		}
	}
}
