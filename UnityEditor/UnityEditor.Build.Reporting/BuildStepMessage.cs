using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Bindings;

namespace UnityEditor.Build.Reporting
{
	[NativeType(Header = "Modules/BuildReportingEditor/Public/BuildReport.h")]
	public struct BuildStepMessage
	{
		public LogType type
		{
			[CompilerGenerated]
			get
			{
				return this.<type>k__BackingField;
			}
		}

		public string content
		{
			[CompilerGenerated]
			get
			{
				return this.<content>k__BackingField;
			}
		}
	}
}
