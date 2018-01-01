using System;
using System.ComponentModel;

namespace Unity.Collections.LowLevel.Unsafe
{
	public static class NativeSliceUnsafeUtility
	{
		public static AtomicSafetyHandle GetAtomicSafetyHandle<T>(NativeSlice<T> slice) where T : struct
		{
			return slice.m_Safety;
		}

		public static void SetAtomicSafetyHandle<T>(ref NativeSlice<T> slice, AtomicSafetyHandle safety) where T : struct
		{
			slice.m_Safety = safety;
		}

		public static NativeSlice<T> ConvertExistingDataToNativeSlice<T>(IntPtr dataPointer, int length) where T : struct
		{
			return new NativeSlice<T>
			{
				m_Stride = UnsafeUtility.SizeOf<T>(),
				m_Buffer = dataPointer,
				m_Length = length,
				m_MinIndex = 0,
				m_MaxIndex = length - 1,
				m_Safety = AtomicSafetyHandle.GetTempUnsafePtrSliceHandle()
			};
		}

		public unsafe static NativeSlice<T> ConvertExistingDataToNativeSlice<T>(IntPtr dataPointer, int start, int length, int stride) where T : struct
		{
			if (length < 0)
			{
				throw new ArgumentException(string.Format("Invalid length of '{0}'. It must be greater than 0.", length));
			}
			if (start < 0)
			{
				throw new ArgumentException(string.Format("Invalid start index of '{0}'. It must be greater than 0.", start));
			}
			if (stride < 0)
			{
				throw new ArgumentException(string.Format("Invalid stride '{0}'. It must be greater than 0.", stride));
			}
			byte* value = (byte*)((void*)dataPointer) + start;
			return new NativeSlice<T>
			{
				m_Stride = stride,
				m_Buffer = (IntPtr)((void*)value),
				m_Length = length,
				m_MinIndex = 0,
				m_MaxIndex = length - 1,
				m_Safety = AtomicSafetyHandle.GetTempUnsafePtrSliceHandle()
			};
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		public static IntPtr GetUnsafePtr<T>(this NativeSlice<T> nativeSlice) where T : struct
		{
			AtomicSafetyHandle.CheckWriteAndThrow(nativeSlice.m_Safety);
			return nativeSlice.m_Buffer;
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		public static IntPtr GetUnsafeReadOnlyPtr<T>(this NativeSlice<T> nativeSlice) where T : struct
		{
			AtomicSafetyHandle.CheckReadAndThrow(nativeSlice.m_Safety);
			return nativeSlice.m_Buffer;
		}
	}
}
