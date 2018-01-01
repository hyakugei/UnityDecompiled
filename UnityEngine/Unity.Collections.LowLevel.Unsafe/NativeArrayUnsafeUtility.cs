using System;

namespace Unity.Collections.LowLevel.Unsafe
{
	public static class NativeArrayUnsafeUtility
	{
		public static AtomicSafetyHandle GetAtomicSafetyHandle<T>(NativeArray<T> array) where T : struct
		{
			return array.m_Safety;
		}

		public static void SetAtomicSafetyHandle<T>(ref NativeArray<T> array, AtomicSafetyHandle safety) where T : struct
		{
			array.m_Safety = safety;
		}

		public static NativeArray<T> ConvertExistingDataToNativeArray<T>(IntPtr dataPointer, int length, Allocator allocator) where T : struct
		{
			return new NativeArray<T>
			{
				m_Buffer = dataPointer,
				m_Length = length,
				m_AllocatorLabel = allocator,
				m_MinIndex = 0,
				m_MaxIndex = length - 1,
				m_DisposeSentinel = null
			};
		}

		public static IntPtr GetUnsafePtr<T>(this NativeArray<T> nativeArray) where T : struct
		{
			AtomicSafetyHandle.CheckWriteAndThrow(nativeArray.m_Safety);
			return nativeArray.m_Buffer;
		}

		public static IntPtr GetUnsafeReadOnlyPtr<T>(this NativeArray<T> nativeArray) where T : struct
		{
			AtomicSafetyHandle.CheckReadAndThrow(nativeArray.m_Safety);
			return nativeArray.m_Buffer;
		}

		public static IntPtr GetUnsafeBufferPointerWithoutChecks<T>(this NativeArray<T> nativeArray) where T : struct
		{
			return nativeArray.m_Buffer;
		}
	}
}
