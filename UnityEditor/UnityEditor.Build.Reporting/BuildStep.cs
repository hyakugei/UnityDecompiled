using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEditor.Build.Reporting
{
	[NativeType(Header = "Modules/BuildReportingEditor/Public/BuildReport.h")]
	public struct BuildStep
	{
		internal ulong durationTicks;

		[NativeName("stepName")]
		public string name
		{
			[CompilerGenerated]
			get
			{
				return this.<name>k__BackingField;
			}
		}

		public TimeSpan duration
		{
			get
			{
				return new TimeSpan((long)this.durationTicks);
			}
		}

		public BuildStepMessage[] messages
		{
			[CompilerGenerated]
			get
			{
				return this.<messages>k__BackingField;
			}
		}

		public int depth
		{
			[CompilerGenerated]
			get
			{
				return this.<depth>k__BackingField;
			}
		}

		public override string ToString()
		{
			return string.Format("{0} ({1}ms)", this.name, this.duration.TotalMilliseconds);
		}
	}
}
