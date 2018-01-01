using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Jobs.LowLevel.Unsafe;

namespace UnityEngine.Jobs
{
	public static class IJobParallelForTransformExtensions
	{
		[StructLayout(LayoutKind.Sequential, Size = 1)]
		private struct TransformParallelForLoopStruct<T> where T : struct, IJobParallelForTransform
		{
			public delegate void ExecuteJobFunction(ref T jobData, IntPtr additionalPtr, IntPtr bufferRangePatchData, ref JobRanges ranges, int jobIndex);

			public static IntPtr jobReflectionData;

			[CompilerGenerated]
			private static IJobParallelForTransformExtensions.TransformParallelForLoopStruct<T>.ExecuteJobFunction <>f__mg$cache0;

			public static IntPtr Initialize()
			{
				if (IJobParallelForTransformExtensions.TransformParallelForLoopStruct<T>.jobReflectionData == IntPtr.Zero)
				{
					Type arg_3E_0 = typeof(T);
					if (IJobParallelForTransformExtensions.TransformParallelForLoopStruct<T>.<>f__mg$cache0 == null)
					{
						IJobParallelForTransformExtensions.TransformParallelForLoopStruct<T>.<>f__mg$cache0 = new IJobParallelForTransformExtensions.TransformParallelForLoopStruct<T>.ExecuteJobFunction(IJobParallelForTransformExtensions.TransformParallelForLoopStruct<T>.Execute);
					}
					IJobParallelForTransformExtensions.TransformParallelForLoopStruct<T>.jobReflectionData = JobsUtility.CreateJobReflectionData(arg_3E_0, IJobParallelForTransformExtensions.TransformParallelForLoopStruct<T>.<>f__mg$cache0, null, null);
				}
				return IJobParallelForTransformExtensions.TransformParallelForLoopStruct<T>.jobReflectionData;
			}

			public unsafe static void Execute(ref T jobData, IntPtr jobData2, IntPtr bufferRangePatchData, ref JobRanges ranges, int jobIndex)
			{
				IntPtr transformArrayIntPtr;
				UnsafeUtility.CopyPtrToStructure<IntPtr>(jobData2, out transformArrayIntPtr);
				int* ptr = (int*)((void*)TransformAccessArray.GetSortedToUserIndex(transformArrayIntPtr));
				TransformAccess* ptr2 = (TransformAccess*)((void*)TransformAccessArray.GetSortedTransformAccess(transformArrayIntPtr));
				int num;
				int num2;
				JobsUtility.GetJobRange(ref ranges, jobIndex, out num, out num2);
				for (int i = num; i < num2; i++)
				{
					int num3 = i;
					int num4 = ptr[num3];
					JobsUtility.PatchBufferMinMaxRanges(bufferRangePatchData, UnsafeUtility.AddressOf<T>(ref jobData), num4, 1);
					jobData.Execute(num4, ptr2[num3]);
				}
			}
		}

		public static JobHandle Schedule<T>(this T jobData, TransformAccessArray transforms, JobHandle dependsOn = default(JobHandle)) where T : struct, IJobParallelForTransform
		{
			JobsUtility.JobScheduleParameters jobScheduleParameters = new JobsUtility.JobScheduleParameters(UnsafeUtility.AddressOf<T>(ref jobData), IJobParallelForTransformExtensions.TransformParallelForLoopStruct<T>.Initialize(), dependsOn, ScheduleMode.Batched);
			return JobsUtility.ScheduleParallelForTransform(ref jobScheduleParameters, transforms.GetTransformAccessArrayForSchedule());
		}
	}
}
