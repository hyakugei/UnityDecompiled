using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Unity.Collections.LowLevel.Unsafe
{
	public static class UnsafeUtility
	{
		public unsafe static void CopyPtrToStructure<T>(IntPtr ptr, out T output) where T : struct
		{
			output = *ptr;
		}

		public unsafe static void CopyStructureToPtr<T>(ref T input, IntPtr ptr) where T : struct
		{
			*ptr = input;
		}

		public unsafe static T ReadArrayElement<T>(IntPtr source, int index)
		{
			return *(source + (IntPtr)(index * sizeof(T)));
		}

		public unsafe static T ReadArrayElementWithStride<T>(IntPtr source, int index, int stride)
		{
			return *(source + (IntPtr)(index * stride));
		}

		public unsafe static void WriteArrayElement<T>(IntPtr destination, int index, T value)
		{
			*(destination + (IntPtr)(index * sizeof(T))) = value;
		}

		public unsafe static void WriteArrayElementWithStride<T>(IntPtr destination, int index, int stride, T value)
		{
			*(destination + (IntPtr)(index * stride)) = value;
		}

		public static IntPtr AddressOf<T>(ref T output) where T : struct
		{
			return ref output;
		}

		public static int SizeOf<T>() where T : struct
		{
			return sizeof(T);
		}

		public static int AlignOf<T>() where T : struct
		{
			return 4;
		}

		public static int OffsetOf<T>(string name) where T : struct
		{
			return (int)Marshal.OffsetOf(typeof(T), name);
		}

		internal static bool IsFieldOfType<T>(string fieldName, Type expectedFieldType) where T : struct
		{
			return UnsafeUtility.IsFieldOfType(typeof(T), fieldName, expectedFieldType);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool IsFieldOfType(Type type, string fieldName, Type expectedFieldType);

		public static bool IsBlittable<T>() where T : struct
		{
			return UnsafeUtility.IsBlittable(typeof(T));
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern IntPtr Malloc(ulong size, int alignment, Allocator allocator);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Free(IntPtr memory, Allocator allocator);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void MemCpy(IntPtr destination, IntPtr source, ulong size);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void MemCpyReplicate(IntPtr destination, IntPtr source, ulong size, int count);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void MemMove(IntPtr destination, IntPtr source, ulong size);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void MemClear(IntPtr destination, ulong size);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int MemCmp(IntPtr ptr1, IntPtr ptr2, ulong size);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int SizeOf(Type type);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool IsBlittable(Type type);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void LogError(string msg, string filename, int linenumber);

		public static void SetFieldStruct<T>(object target, FieldInfo field, ref T value) where T : struct
		{
			UnsafeUtility.SetFieldStructInternal(target, field, UnsafeUtility.AddressOf<T>(ref value), Marshal.SizeOf(typeof(T)));
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetFieldStructInternal(object target, FieldInfo field, IntPtr value, int size);
	}
}
