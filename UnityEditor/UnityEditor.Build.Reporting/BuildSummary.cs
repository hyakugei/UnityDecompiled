using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEditor.Build.Reporting
{
	[NativeType(Header = "Modules/BuildReportingEditor/Managed/BuildSummary.bindings.h", CodegenOptions = CodegenOptions.Custom)]
	public struct BuildSummary
	{
		internal long buildStartTimeTicks;

		internal ulong totalTimeTicks;

		public DateTime buildStartedAt
		{
			get
			{
				return new DateTime(this.buildStartTimeTicks);
			}
		}

		[NativeName("buildGUID")]
		public GUID guid
		{
			[CompilerGenerated]
			get
			{
				return this.<guid>k__BackingField;
			}
		}

		public BuildTarget platform
		{
			[CompilerGenerated]
			get
			{
				return this.<platform>k__BackingField;
			}
		}

		public BuildTargetGroup platformGroup
		{
			[CompilerGenerated]
			get
			{
				return this.<platformGroup>k__BackingField;
			}
		}

		public BuildOptions options
		{
			[CompilerGenerated]
			get
			{
				return this.<options>k__BackingField;
			}
		}

		internal BuildAssetBundleOptions assetBundleOptions
		{
			[CompilerGenerated]
			get
			{
				return this.<assetBundleOptions>k__BackingField;
			}
		}

		public string outputPath
		{
			[CompilerGenerated]
			get
			{
				return this.<outputPath>k__BackingField;
			}
		}

		internal uint crc
		{
			[CompilerGenerated]
			get
			{
				return this.<crc>k__BackingField;
			}
		}

		public ulong totalSize
		{
			[CompilerGenerated]
			get
			{
				return this.<totalSize>k__BackingField;
			}
		}

		public TimeSpan totalTime
		{
			get
			{
				return new TimeSpan((long)this.totalTimeTicks);
			}
		}

		public DateTime buildEndedAt
		{
			get
			{
				return this.buildStartedAt + this.totalTime;
			}
		}

		public int totalErrors
		{
			[CompilerGenerated]
			get
			{
				return this.<totalErrors>k__BackingField;
			}
		}

		public int totalWarnings
		{
			[CompilerGenerated]
			get
			{
				return this.<totalWarnings>k__BackingField;
			}
		}

		[NativeName("buildResult")]
		public BuildResult result
		{
			[CompilerGenerated]
			get
			{
				return this.<result>k__BackingField;
			}
		}

		internal BuildType buildType
		{
			[CompilerGenerated]
			get
			{
				return this.<buildType>k__BackingField;
			}
		}
	}
}
