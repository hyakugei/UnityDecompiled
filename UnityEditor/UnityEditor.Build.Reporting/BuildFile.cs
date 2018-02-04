using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEditor.Build.Reporting
{
	[NativeType(Header = "Modules/BuildReportingEditor/Public/BuildReport.h")]
	public struct BuildFile
	{
		internal uint id
		{
			[CompilerGenerated]
			get
			{
				return this.<id>k__BackingField;
			}
		}

		public string path
		{
			[CompilerGenerated]
			get
			{
				return this.<path>k__BackingField;
			}
		}

		public string role
		{
			[CompilerGenerated]
			get
			{
				return this.<role>k__BackingField;
			}
		}

		[NativeName("totalSize")]
		public ulong size
		{
			[CompilerGenerated]
			get
			{
				return this.<size>k__BackingField;
			}
		}

		public override string ToString()
		{
			return this.path;
		}
	}
}
