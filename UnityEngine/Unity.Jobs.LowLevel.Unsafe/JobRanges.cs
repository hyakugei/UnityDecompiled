using System;

namespace Unity.Jobs.LowLevel.Unsafe
{
	public struct JobRanges
	{
		public int batchSize;

		public int numJobs;

		public int totalIterationCount;

		public int numPhases;

		public int indicesPerPhase;

		public IntPtr startEndIndex;

		public IntPtr phaseData;
	}
}
