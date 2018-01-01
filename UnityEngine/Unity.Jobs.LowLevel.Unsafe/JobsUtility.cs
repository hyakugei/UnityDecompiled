using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace Unity.Jobs.LowLevel.Unsafe
{
	[NativeType(Header = "Runtime/Jobs/ScriptBindings/JobsBindings.h")]
	public static class JobsUtility
	{
		public struct JobScheduleParameters
		{
			public JobHandle dependency;

			public int scheduleMode;

			public IntPtr reflectionData;

			public IntPtr jobDataPtr;

			public JobScheduleParameters(IntPtr i_jobData, IntPtr i_reflectionData, JobHandle i_dependency, ScheduleMode i_scheduleMode)
			{
				this.dependency = i_dependency;
				this.jobDataPtr = i_jobData;
				this.reflectionData = i_reflectionData;
				this.scheduleMode = (int)i_scheduleMode;
			}
		}

		public const int MaxJobThreadCount = 128;

		public const int CacheLineSize = 64;

		public unsafe static void GetJobRange(ref JobRanges ranges, int jobIndex, out int beginIndex, out int endIndex)
		{
			int* ptr = (int*)((void*)ranges.startEndIndex);
			beginIndex = ptr[jobIndex * 2];
			endIndex = ptr[jobIndex * 2 + 1];
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool GetWorkStealingRange(ref JobRanges ranges, int jobIndex, out int beginIndex, out int endIndex);

		public static JobHandle Schedule(ref JobsUtility.JobScheduleParameters parameters)
		{
			JobHandle result;
			JobsUtility.Schedule_Injected(ref parameters, out result);
			return result;
		}

		public static JobHandle ScheduleParallelFor(ref JobsUtility.JobScheduleParameters parameters, int arrayLength, int innerloopBatchCount)
		{
			JobHandle result;
			JobsUtility.ScheduleParallelFor_Injected(ref parameters, arrayLength, innerloopBatchCount, out result);
			return result;
		}

		public static JobHandle ScheduleParallelForTransform(ref JobsUtility.JobScheduleParameters parameters, IntPtr transfromAccesssArray)
		{
			JobHandle result;
			JobsUtility.ScheduleParallelForTransform_Injected(ref parameters, transfromAccesssArray, out result);
			return result;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void PatchBufferMinMaxRanges(IntPtr bufferRangePatchData, IntPtr jobdata, int startIndex, int rangeSize);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr CreateJobReflectionData(Type wrapperJobType, Type userJobType, object managedJobFunction0, object managedJobFunction1, object managedJobFunction2);

		public static IntPtr CreateJobReflectionData(Type type, object managedJobFunction0, object managedJobFunction1 = null, object managedJobFunction2 = null)
		{
			return JobsUtility.CreateJobReflectionData(type, type, managedJobFunction0, managedJobFunction1, managedJobFunction2);
		}

		public static IntPtr CreateJobReflectionData(Type wrapperJobType, Type userJobType, object managedJobFunction0)
		{
			return JobsUtility.CreateJobReflectionData(wrapperJobType, userJobType, managedJobFunction0, null, null);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetJobDebuggerEnabled(bool value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool GetJobDebuggerEnabled();

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Schedule_Injected(ref JobsUtility.JobScheduleParameters parameters, out JobHandle ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void ScheduleParallelFor_Injected(ref JobsUtility.JobScheduleParameters parameters, int arrayLength, int innerloopBatchCount, out JobHandle ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void ScheduleParallelForTransform_Injected(ref JobsUtility.JobScheduleParameters parameters, IntPtr transfromAccesssArray, out JobHandle ret);
	}
}
