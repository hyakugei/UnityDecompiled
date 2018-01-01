using System;

namespace UnityEditorInternal.Profiling
{
	public enum ProfilerColumn
	{
		DontSort = -1,
		FunctionName,
		TotalPercent,
		SelfPercent,
		Calls,
		GCMemory,
		TotalTime,
		SelfTime,
		DrawCalls,
		TotalGPUTime,
		SelfGPUTime,
		TotalGPUPercent,
		SelfGPUPercent,
		WarningCount,
		ObjectName
	}
}
