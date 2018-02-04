using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs.LowLevel.Unsafe;

namespace Unity.Jobs
{
	public static class IJobParallelForExtensions
	{
		[StructLayout(LayoutKind.Sequential, Size = 1)]
		private struct ParallelForJobStruct<T> where T : struct, IJobParallelFor
		{
			public delegate void ExecuteJobFunction(ref T data, IntPtr additionalPtr, IntPtr bufferRangePatchData, ref JobRanges ranges, int jobIndex);

			public static IntPtr jobReflectionData;

			[CompilerGenerated]
			//private static IJobParallelForExtensions.ParallelForJobStruct<T>.ExecuteJobFunction <>f__mg$cache0;

			public static IntPtr Initialize()
			{
				if (IJobParallelForExtensions.ParallelForJobStruct<T>.jobReflectionData == IntPtr.Zero)
				{
					/*
					Type arg_3E_0 = typeof(T);
					if (IJobParallelForExtensions.ParallelForJobStruct<T>.<>f__mg$cache0 == null)
					{
						IJobParallelForExtensions.ParallelForJobStruct<T>.<>f__mg$cache0 = new IJobParallelForExtensions.ParallelForJobStruct<T>.ExecuteJobFunction(IJobParallelForExtensions.ParallelForJobStruct<T>.Execute);
					}
					IJobParallelForExtensions.ParallelForJobStruct<T>.jobReflectionData = JobsUtility.CreateJobReflectionData(arg_3E_0, IJobParallelForExtensions.ParallelForJobStruct<T>.<>f__mg$cache0, null, null);
					*/
				}
				return IJobParallelForExtensions.ParallelForJobStruct<T>.jobReflectionData;
			}

			public static void Execute(ref T jobData, IntPtr additionalPtr, IntPtr bufferRangePatchData, ref JobRanges ranges, int jobIndex)
			{
				int num;
				int num2;
				while (JobsUtility.GetWorkStealingRange(ref ranges, jobIndex, out num, out num2))
				{
					JobsUtility.PatchBufferMinMaxRanges(bufferRangePatchData, UnsafeUtility.AddressOf<T>(ref jobData), num, num2 - num);
					for (int i = num; i < num2; i++)
					{
						jobData.Execute(i);
					}
				}
			}
		}

		public static JobHandle Schedule<T>(T jobData, int arrayLength, int innerloopBatchCount, JobHandle dependsOn = default(JobHandle)) where T : struct, IJobParallelFor
		{
			JobsUtility.JobScheduleParameters jobScheduleParameters = new JobsUtility.JobScheduleParameters(UnsafeUtility.AddressOf<T>(ref jobData), IJobParallelForExtensions.ParallelForJobStruct<T>.Initialize(), dependsOn, ScheduleMode.Batched);
			return JobsUtility.ScheduleParallelFor(ref jobScheduleParameters, arrayLength, innerloopBatchCount);
		}

		public static void Run<T>(T jobData, int arrayLength) where T : struct, IJobParallelFor
		{
			JobsUtility.JobScheduleParameters jobScheduleParameters = new JobsUtility.JobScheduleParameters(UnsafeUtility.AddressOf<T>(ref jobData), IJobParallelForExtensions.ParallelForJobStruct<T>.Initialize(), default(JobHandle), ScheduleMode.Run);
			JobsUtility.ScheduleParallelFor(ref jobScheduleParameters, arrayLength, arrayLength);
		}
	}
}
