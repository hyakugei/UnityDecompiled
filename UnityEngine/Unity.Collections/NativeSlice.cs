using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Collections
{
	[DebuggerDisplay("Length = {Length}"), DebuggerTypeProxy(typeof(NativeSliceDebugView<>)), NativeContainer, NativeContainerSupportsMinMaxWriteRestriction]
	public struct NativeSlice<T> : IEnumerable<T>, IEnumerable where T : struct
	{
		internal struct Enumerator : IEnumerator<T>, IEnumerator, IDisposable
		{
			private NativeSlice<T> m_Array;

			private int m_Index;

			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			public T Current
			{
				get
				{
					return this.m_Array[this.m_Index];
				}
			}

			public Enumerator(ref NativeSlice<T> array)
			{
				this.m_Array = array;
				this.m_Index = -1;
			}

			public void Dispose()
			{
			}

			public bool MoveNext()
			{
				this.m_Index++;
				return this.m_Index < this.m_Array.Length;
			}

			public void Reset()
			{
				this.m_Index = -1;
			}
		}

		internal IntPtr m_Buffer;

		internal int m_Stride;

		internal int m_Length;

		internal int m_MinIndex;

		internal int m_MaxIndex;

		internal AtomicSafetyHandle m_Safety;

		public unsafe T this[int index]
		{
			get
			{
				if (this.m_Buffer == IntPtr.Zero)
				{
					throw new InvalidOperationException("NativeSlice not properly initialized");
				}
				AtomicSafetyHandleVersionMask* ptr = (AtomicSafetyHandleVersionMask*)((void*)this.m_Safety.versionNode);
				if ((this.m_Safety.version & AtomicSafetyHandleVersionMask.Read) == (AtomicSafetyHandleVersionMask)0 && this.m_Safety.version != (*ptr & AtomicSafetyHandleVersionMask.WriteInv))
				{
					AtomicSafetyHandle.CheckReadAndThrowNoEarlyOut(this.m_Safety);
				}
				if (index < this.m_MinIndex || index > this.m_MaxIndex)
				{
					this.FailOutOfRangeError(index);
				}
				return UnsafeUtility.ReadArrayElementWithStride<T>(this.m_Buffer, index, this.m_Stride);
			}
			set
			{
				if (this.m_Buffer == IntPtr.Zero)
				{
					throw new InvalidOperationException("NativeSlice not properly initialized");
				}
				AtomicSafetyHandleVersionMask* ptr = (AtomicSafetyHandleVersionMask*)((void*)this.m_Safety.versionNode);
				if ((this.m_Safety.version & AtomicSafetyHandleVersionMask.Write) == (AtomicSafetyHandleVersionMask)0 && this.m_Safety.version != (*ptr & AtomicSafetyHandleVersionMask.ReadInv))
				{
					AtomicSafetyHandle.CheckWriteAndThrowNoEarlyOut(this.m_Safety);
				}
				if (index < this.m_MinIndex || index > this.m_MaxIndex)
				{
					this.FailOutOfRangeError(index);
				}
				UnsafeUtility.WriteArrayElementWithStride<T>(this.m_Buffer, index, this.m_Stride, value);
			}
		}

		public int Stride
		{
			get
			{
				return this.m_Stride;
			}
		}

		public int Length
		{
			get
			{
				return this.m_Length;
			}
		}

		public NativeSlice(NativeArray<T> array)
		{
			this = new NativeSlice<T>(array, 0, array.Length);
		}

		public NativeSlice(NativeArray<T> array, int start)
		{
			this = new NativeSlice<T>(array, start, array.Length - start);
		}

		public unsafe NativeSlice(NativeArray<T> array, int start, int length)
		{
			if (start < 0)
			{
				throw new ArgumentOutOfRangeException("start", string.Format("Slice start {0} < 0.", start));
			}
			if (length < 0)
			{
				throw new ArgumentOutOfRangeException("length", string.Format("Slice length {0} < 0.", length));
			}
			if (start + length > array.Length)
			{
				throw new ArgumentException(string.Format("Slice start + length ({0}) range must be <= array.Length ({1})", start + length, array.Length));
			}
			if ((array.m_MinIndex != 0 || array.m_MaxIndex != array.m_Length - 1) && (start < array.m_MinIndex || array.m_MaxIndex < start || array.m_MaxIndex < start + length - 1))
			{
				throw new ArgumentException("Slice may not be used on a restricted range array", "array");
			}
			this.m_Stride = UnsafeUtility.SizeOf<T>();
			byte* value = (byte*)((void*)array.m_Buffer) + this.m_Stride * start;
			this.m_Buffer = (IntPtr)((void*)value);
			this.m_Length = length;
			this.m_MinIndex = 0;
			this.m_MaxIndex = length - 1;
			this.m_Safety = array.m_Safety;
		}

		public static implicit operator NativeSlice<T>(NativeArray<T> array)
		{
			return new NativeSlice<T>(array);
		}

		[MethodImpl((MethodImplOptions)256)]
		public NativeSlice<U> SliceConvert<U>() where U : struct
		{
			int num = UnsafeUtility.SizeOf<U>();
			if (this.m_Stride != UnsafeUtility.SizeOf<T>())
			{
				throw new ArgumentException("SliceConvert requires that stride matches the size of the source type");
			}
			if (this.m_MinIndex != 0 || this.m_MaxIndex != this.m_Length - 1)
			{
				throw new ArgumentException("SliceConvert may not be used on a restricted range array");
			}
			if (this.m_Stride * this.m_Length % num != 0)
			{
				throw new ArgumentException("SliceConvert requires that Length * sizeof(T) is a multiple of sizeof(U).");
			}
			NativeSlice<U> result;
			result.m_Buffer = this.m_Buffer;
			result.m_Stride = num;
			result.m_Length = this.m_Length * this.m_Stride / num;
			result.m_MinIndex = 0;
			result.m_MaxIndex = result.m_Length - 1;
			result.m_Safety = this.m_Safety;
			return result;
		}

		[MethodImpl((MethodImplOptions)256)]
		public unsafe NativeSlice<U> SliceWithStride<U>(int offset) where U : struct
		{
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", "SliceWithStride offset must be >= 0");
			}
			if (offset + UnsafeUtility.SizeOf<U>() > UnsafeUtility.SizeOf<T>())
			{
				throw new ArgumentException("SliceWithStride sizeof(U) + offset must be <= sizeof(T)");
			}
			byte* value = (byte*)((void*)this.m_Buffer) + offset;
			NativeSlice<U> result;
			result.m_Buffer = (IntPtr)((void*)value);
			result.m_Stride = this.m_Stride;
			result.m_Length = this.m_Length;
			result.m_MinIndex = this.m_MinIndex;
			result.m_MaxIndex = this.m_MaxIndex;
			result.m_Safety = this.m_Safety;
			return result;
		}

		[MethodImpl((MethodImplOptions)256)]
		public NativeSlice<U> SliceWithStride<U>() where U : struct
		{
			return this.SliceWithStride<U>(0);
		}

		[MethodImpl((MethodImplOptions)256)]
		public NativeSlice<U> SliceWithStride<U>(string offsetFieldName) where U : struct
		{
			if (!UnsafeUtility.IsFieldOfType<T>(offsetFieldName, typeof(U)))
			{
				throw new ArgumentException(string.Format("Field {0}.{1} does not exist or is not of type {2}", typeof(T).FullName, offsetFieldName, typeof(U).Name));
			}
			int offset = UnsafeUtility.OffsetOf<T>(offsetFieldName);
			return this.SliceWithStride<U>(offset);
		}

		private void FailOutOfRangeError(int index)
		{
			if (index < this.Length && (this.m_MinIndex != 0 || this.m_MaxIndex != this.Length - 1))
			{
				throw new IndexOutOfRangeException(string.Format("Index {0} is out of restricted IJobParallelFor range [{1}...{2}] in ReadWriteBuffer.\nReadWriteBuffers are restricted to only read & write the element at the job index. You can use double buffering strategies to avoid race conditions due to reading & writing in parallel to the same elements from a job.", index, this.m_MinIndex, this.m_MaxIndex));
			}
			throw new IndexOutOfRangeException(string.Format("Index {0} is out of range of '{1}' Length.", index, this.Length));
		}

		[MethodImpl((MethodImplOptions)256)]
		public void CopyFrom(NativeSlice<T> slice)
		{
			if (this.Length != slice.Length)
			{
				throw new ArgumentException(string.Format("slice.Length ({0}) does not match the Length of this instance ({1}).", slice.Length, this.Length));
			}
			for (int num = 0; num != this.m_Length; num++)
			{
				this[num] = slice[num];
			}
		}

		[MethodImpl((MethodImplOptions)256)]
		public void CopyFrom(T[] array)
		{
			if (this.Length != array.Length)
			{
				throw new ArgumentException(string.Format("array.Length ({0}) does not match the Length of this instance ({1}).", array.Length, this.Length));
			}
			for (int num = 0; num != this.m_Length; num++)
			{
				this[num] = array[num];
			}
		}

		[MethodImpl((MethodImplOptions)256)]
		public void CopyTo(NativeArray<T> array)
		{
			if (this.Length != array.Length)
			{
				throw new ArgumentException(string.Format("array.Length ({0}) does not match the Length of this instance ({1}).", array.Length, this.Length));
			}
			for (int num = 0; num != this.m_Length; num++)
			{
				array[num] = this[num];
			}
		}

		[MethodImpl((MethodImplOptions)256)]
		public void CopyTo(T[] array)
		{
			if (this.Length != array.Length)
			{
				throw new ArgumentException(string.Format("array.Length ({0}) does not match the Length of this instance ({1}).", array.Length, this.Length));
			}
			for (int num = 0; num != this.m_Length; num++)
			{
				array[num] = this[num];
			}
		}

		[MethodImpl((MethodImplOptions)256)]
		public T[] ToArray()
		{
			T[] array = new T[this.Length];
			this.CopyTo(array);
			return array;
		}

		public IEnumerator<T> GetEnumerator()
		{
			return new NativeSlice<T>.Enumerator(ref this);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
	}
}
