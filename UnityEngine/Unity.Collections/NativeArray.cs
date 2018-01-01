using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Collections
{
	[DebuggerDisplay("Length = {Length}"), DebuggerTypeProxy(typeof(NativeArrayDebugView<>)), NativeContainer, NativeContainerSupportsDeallocateOnJobCompletion, NativeContainerSupportsMinMaxWriteRestriction]
	public struct NativeArray<T> : IDisposable, IEnumerable<T>, IEnumerable where T : struct
	{
		internal struct Enumerator : IEnumerator<T>, IEnumerator, IDisposable
		{
			private NativeArray<T> m_Array;

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

			public Enumerator(ref NativeArray<T> array)
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

		internal int m_Length;

		internal int m_MinIndex;

		internal int m_MaxIndex;

		internal AtomicSafetyHandle m_Safety;

		internal DisposeSentinel m_DisposeSentinel;

		internal Allocator m_AllocatorLabel;

		public int Length
		{
			get
			{
				return this.m_Length;
			}
		}

		public unsafe T this[int index]
		{
			get
			{
				AtomicSafetyHandleVersionMask* ptr = (AtomicSafetyHandleVersionMask*)((void*)this.m_Safety.versionNode);
				if ((this.m_Safety.version & AtomicSafetyHandleVersionMask.Read) == (AtomicSafetyHandleVersionMask)0 && this.m_Safety.version != (*ptr & AtomicSafetyHandleVersionMask.WriteInv))
				{
					AtomicSafetyHandle.CheckReadAndThrowNoEarlyOut(this.m_Safety);
				}
				if (index < this.m_MinIndex || index > this.m_MaxIndex)
				{
					this.FailOutOfRangeError(index);
				}
				return UnsafeUtility.ReadArrayElement<T>(this.m_Buffer, index);
			}
			set
			{
				AtomicSafetyHandleVersionMask* ptr = (AtomicSafetyHandleVersionMask*)((void*)this.m_Safety.versionNode);
				if ((this.m_Safety.version & AtomicSafetyHandleVersionMask.Write) == (AtomicSafetyHandleVersionMask)0 && this.m_Safety.version != (*ptr & AtomicSafetyHandleVersionMask.ReadInv))
				{
					AtomicSafetyHandle.CheckWriteAndThrowNoEarlyOut(this.m_Safety);
				}
				if (index < this.m_MinIndex || index > this.m_MaxIndex)
				{
					this.FailOutOfRangeError(index);
				}
				UnsafeUtility.WriteArrayElement<T>(this.m_Buffer, index, value);
			}
		}

		public bool IsCreated
		{
			get
			{
				return this.m_Buffer != IntPtr.Zero;
			}
		}

		public NativeArray(int length, Allocator allocator, NativeArrayOptions options = NativeArrayOptions.ClearMemory)
		{
			NativeArray<T>.Allocate(length, allocator, out this);
			if ((options & NativeArrayOptions.ClearMemory) == NativeArrayOptions.ClearMemory)
			{
				UnsafeUtility.MemClear(this.m_Buffer, (ulong)((long)this.Length * (long)UnsafeUtility.SizeOf<T>()));
			}
		}

		public NativeArray(T[] array, Allocator allocator)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			NativeArray<T>.Allocate(array.Length, allocator, out this);
			this.CopyFrom(array);
		}

		public NativeArray(NativeArray<T> array, Allocator allocator)
		{
			NativeArray<T>.Allocate(array.Length, allocator, out this);
			this.CopyFrom(array);
		}

		private static void Allocate(int length, Allocator allocator, out NativeArray<T> array)
		{
			long num = (long)UnsafeUtility.SizeOf<T>() * (long)length;
			if (allocator <= Allocator.None)
			{
				throw new ArgumentException("Allocator must be Temp, TempJob or Persistent", "allocator");
			}
			if (length < 0)
			{
				throw new ArgumentOutOfRangeException("length", "Length must be >= 0");
			}
			if (!UnsafeUtility.IsBlittable<T>())
			{
				throw new ArgumentException(string.Format("{0} used in NativeArray<{0}> must be blittable", typeof(T)));
			}
			if (num > 2147483647L)
			{
				throw new ArgumentOutOfRangeException("length", string.Format("Length * sizeof(T) cannot exceed {0} bytes", 2147483647));
			}
			array.m_Buffer = UnsafeUtility.Malloc((ulong)num, UnsafeUtility.AlignOf<T>(), allocator);
			array.m_Length = length;
			array.m_AllocatorLabel = allocator;
			array.m_MinIndex = 0;
			array.m_MaxIndex = length - 1;
			DisposeSentinel.Create(array.m_Buffer, allocator, out array.m_Safety, out array.m_DisposeSentinel, 1, null);
		}

		public void Dispose()
		{
			DisposeSentinel.Dispose(this.m_Safety, ref this.m_DisposeSentinel);
			UnsafeUtility.Free(this.m_Buffer, this.m_AllocatorLabel);
			this.m_Buffer = IntPtr.Zero;
			this.m_Length = 0;
		}

		[MethodImpl((MethodImplOptions)256)]
		public void CopyFrom(T[] array)
		{
			AtomicSafetyHandle.CheckWriteAndThrow(this.m_Safety);
			if (this.Length != array.Length)
			{
				throw new ArgumentException("array.Length does not match the length of this instance");
			}
			for (int i = 0; i < this.Length; i++)
			{
				UnsafeUtility.WriteArrayElement<T>(this.m_Buffer, i, array[i]);
			}
		}

		[MethodImpl((MethodImplOptions)256)]
		public void CopyFrom(NativeArray<T> array)
		{
			array.CopyTo(this);
		}

		[MethodImpl((MethodImplOptions)256)]
		public void CopyTo(T[] array)
		{
			AtomicSafetyHandle.CheckReadAndThrow(this.m_Safety);
			if (this.Length != array.Length)
			{
				throw new ArgumentException("array.Length does not match the length of this instance");
			}
			for (int i = 0; i < this.Length; i++)
			{
				array[i] = UnsafeUtility.ReadArrayElement<T>(this.m_Buffer, i);
			}
		}

		[MethodImpl((MethodImplOptions)256)]
		public void CopyTo(NativeArray<T> array)
		{
			AtomicSafetyHandle.CheckReadAndThrow(this.m_Safety);
			AtomicSafetyHandle.CheckWriteAndThrow(array.m_Safety);
			if (this.Length != array.Length)
			{
				throw new ArgumentException("array.Length does not match the length of this instance");
			}
			UnsafeUtility.MemCpy(array.m_Buffer, this.m_Buffer, (ulong)((long)this.Length * (long)UnsafeUtility.SizeOf<T>()));
		}

		[MethodImpl((MethodImplOptions)256)]
		public T[] ToArray()
		{
			T[] array = new T[this.Length];
			this.CopyTo(array);
			return array;
		}

		private void FailOutOfRangeError(int index)
		{
			if (index < this.Length && (this.m_MinIndex != 0 || this.m_MaxIndex != this.Length - 1))
			{
				throw new IndexOutOfRangeException(string.Format("Index {0} is out of restricted IJobParallelFor range [{1}...{2}] in ReadWriteBuffer.\nReadWriteBuffers are restricted to only read & write the element at the job index. You can use double buffering strategies to avoid race conditions due to reading & writing in parallel to the same elements from a job.", index, this.m_MinIndex, this.m_MaxIndex));
			}
			throw new IndexOutOfRangeException(string.Format("Index {0} is out of range of '{1}' Length.", index, this.Length));
		}

		public IEnumerator<T> GetEnumerator()
		{
			return new NativeArray<T>.Enumerator(ref this);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
	}
}
