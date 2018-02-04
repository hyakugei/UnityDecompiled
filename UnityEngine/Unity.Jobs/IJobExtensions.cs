using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs.LowLevel.Unsafe;

namespace Unity.Jobs
{
	public static class IJobExtensions
	{
		[StructLayout(LayoutKind.Sequential, Size = 1)]
		private struct JobStruct<T> where T : struct, IJob
		{
			public delegate void ExecuteJobFunction(ref T data, IntPtr additionalPtr, IntPtr bufferRangePatchData, ref JobRanges ranges, int jobIndex);

			public static IntPtr jobReflectionData;

			[CompilerGenerated]
			private static IJobExtensions.JobStruct<T>.ExecuteJobFunction <>f__mg$cache0;

			public static IntPtr Initialize()
			{
				if (IJobExtensions.JobStruct<T>.jobReflectionData == IntPtr.Zero)
				{
					Type arg_3E_0 = typeof(T);
					if (IJobExtensions.JobStruct<T>.<>f__mg$cache0 == null)
					{
						IJobExtensions.JobStruct<T>.<>f__mg$cache0 = new IJobExtensions.JobStruct<T>.ExecuteJobFunction(IJobExtensions.JobStruct<T>.Execute);
					}
					IJobExtensions.JobStruct<T>.jobReflectionData = JobsUtility.CreateJobReflectionData(arg_3E_0, IJobExtensions.JobStruct<T>.<>f__mg$cache0, null, null);
				}
				return IJobExtensions.JobStruct<T>.jobReflectionData;
			}

			public static void Execute(ref T data, IntPtr additionalPtr, IntPtr bufferRangePatchData, ref JobRanges ranges, int jobIndex)
			{
				data.Execute();
			}
		}

		public static JobHandle Schedule<T>(this T jobData, JobHandle dependsOn = default(JobHandle)) where T : struct, IJob
		{
			JobsUtility.JobScheduleParameters jobScheduleParameters = new JobsUtility.JobScheduleParameters(UnsafeUtility.AddressOf<T>(ref jobData), IJobExtensions.JobStruct<T>.Initialize(), dependsOn, ScheduleMode.Batched);
			return JobsUtility.Schedule(ref jobScheduleParameters);
		}

		public static void Run<T>(this T jobData) where T : struct, IJob
		{
			JobsUtility.JobScheduleParameters jobScheduleParameters = new JobsUtility.JobScheduleParameters(UnsafeUtility.AddressOf<T>(ref jobData), IJobExtensions.JobStruct<T>.Initialize(), default(JobHandle), ScheduleMode.Run);
			JobsUtility.Schedule(ref jobScheduleParameters);
		}
	}
}
