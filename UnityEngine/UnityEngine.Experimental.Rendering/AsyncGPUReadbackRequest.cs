using System;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.Scripting;

namespace UnityEngine.Experimental.Rendering
{
	[UsedByNativeCode]
	public struct AsyncGPUReadbackRequest
	{
		internal IntPtr m_Ptr;

		internal int m_Version;

		public bool done
		{
			get
			{
				return this.IsDone();
			}
		}

		public bool hasError
		{
			get
			{
				return this.HasError();
			}
		}

		public int layerCount
		{
			get
			{
				return this.GetLayerCount();
			}
		}

		public void Update()
		{
			AsyncGPUReadbackRequest.Update_Injected(ref this);
		}

		public void WaitForCompletion()
		{
			AsyncGPUReadbackRequest.WaitForCompletion_Injected(ref this);
		}

		public NativeArray<T> GetData<T>(int layer = 0) where T : struct
		{
			if (!this.done || this.hasError)
			{
				throw new InvalidOperationException("Cannot access the data as it is not available");
			}
			if (layer < 0 || layer >= this.layerCount)
			{
				throw new ArgumentException(string.Format("Layer index is out of range {0} / {1}", layer, this.layerCount));
			}
			int num = UnsafeUtility.SizeOf<T>();
			NativeArray<T> result = NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<T>(this.GetDataRaw(layer), this.GetLayerDataSize() / num, Allocator.None);
			NativeArrayUnsafeUtility.SetAtomicSafetyHandle<T>(ref result, this.GetSafetyHandle());
			return result;
		}

		private bool IsDone()
		{
			return AsyncGPUReadbackRequest.IsDone_Injected(ref this);
		}

		private bool HasError()
		{
			return AsyncGPUReadbackRequest.HasError_Injected(ref this);
		}

		private int GetLayerCount()
		{
			return AsyncGPUReadbackRequest.GetLayerCount_Injected(ref this);
		}

		internal void CreateSafetyHandle()
		{
			AsyncGPUReadbackRequest.CreateSafetyHandle_Injected(ref this);
		}

		private AtomicSafetyHandle GetSafetyHandle()
		{
			AtomicSafetyHandle result;
			AsyncGPUReadbackRequest.GetSafetyHandle_Injected(ref this, out result);
			return result;
		}

		private IntPtr GetDataRaw(int layer)
		{
			return AsyncGPUReadbackRequest.GetDataRaw_Injected(ref this, layer);
		}

		private int GetLayerDataSize()
		{
			return AsyncGPUReadbackRequest.GetLayerDataSize_Injected(ref this);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Update_Injected(ref AsyncGPUReadbackRequest _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void WaitForCompletion_Injected(ref AsyncGPUReadbackRequest _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool IsDone_Injected(ref AsyncGPUReadbackRequest _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool HasError_Injected(ref AsyncGPUReadbackRequest _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetLayerCount_Injected(ref AsyncGPUReadbackRequest _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void CreateSafetyHandle_Injected(ref AsyncGPUReadbackRequest _unity_self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetSafetyHandle_Injected(ref AsyncGPUReadbackRequest _unity_self, out AtomicSafetyHandle ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr GetDataRaw_Injected(ref AsyncGPUReadbackRequest _unity_self, int layer);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetLayerDataSize_Injected(ref AsyncGPUReadbackRequest _unity_self);
	}
}
