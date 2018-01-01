using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEditorInternal
{
	[NativeType(Header = "Editor/Platform/Windows/VisualStudioUtilities.h")]
	internal static class VisualStudioUtil
	{
		public class VisualStudio
		{
			public readonly string DevEnvPath;

			public readonly string Edition;

			public readonly Version Version;

			public readonly string[] Workloads;

			internal VisualStudio(string devEnvPath, string edition, Version version, string[] workloads)
			{
				this.DevEnvPath = devEnvPath;
				this.Edition = edition;
				this.Version = version;
				this.Workloads = workloads;
			}
		}

		[DebuggerHidden]
		public static IEnumerable<VisualStudioUtil.VisualStudio> ParseRawDevEnvPaths(string[] rawDevEnvPaths)
		{
			VisualStudioUtil.<ParseRawDevEnvPaths>c__Iterator0 <ParseRawDevEnvPaths>c__Iterator = new VisualStudioUtil.<ParseRawDevEnvPaths>c__Iterator0();
			<ParseRawDevEnvPaths>c__Iterator.rawDevEnvPaths = rawDevEnvPaths;
			VisualStudioUtil.<ParseRawDevEnvPaths>c__Iterator0 expr_0E = <ParseRawDevEnvPaths>c__Iterator;
			expr_0E.$PC = -2;
			return expr_0E;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern string[] FindVisualStudioDevEnvPaths(int visualStudioVersion, string[] requiredWorkloads);
	}
}
