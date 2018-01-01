using System;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using UnityEngine.Experimental.Rendering;

namespace UnityEngine.Experimental.U2D
{
	public static class SpriteRendererDataAccessExtensions
	{
		public static NativeArray<Vector3> GetDeformableVertices(this SpriteRenderer spriteRenderer)
		{
			SpriteChannelInfo deformableChannelInfo = spriteRenderer.GetDeformableChannelInfo(VertexAttribute.Position);
			NativeArray<Vector3> result = NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<Vector3>(deformableChannelInfo.buffer, deformableChannelInfo.count, Allocator.Invalid);
			NativeArrayUnsafeUtility.SetAtomicSafetyHandle<Vector3>(ref result, spriteRenderer.GetSafetyHandle());
			return result;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void DeactivateDeformableBuffer(this SpriteRenderer renderer);

		public static void UpdateDeformableBuffer(this SpriteRenderer spriteRenderer, JobHandle fence)
		{
			SpriteRendererDataAccessExtensions.UpdateDeformableBuffer_Injected(spriteRenderer, ref fence);
		}

		private static SpriteChannelInfo GetDeformableChannelInfo(this SpriteRenderer sprite, VertexAttribute channel)
		{
			SpriteChannelInfo result;
			SpriteRendererDataAccessExtensions.GetDeformableChannelInfo_Injected(sprite, channel, out result);
			return result;
		}

		private static AtomicSafetyHandle GetSafetyHandle(this SpriteRenderer spriteRenderer)
		{
			AtomicSafetyHandle result;
			SpriteRendererDataAccessExtensions.GetSafetyHandle_Injected(spriteRenderer, out result);
			return result;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void UpdateDeformableBuffer_Injected(SpriteRenderer spriteRenderer, ref JobHandle fence);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetDeformableChannelInfo_Injected(SpriteRenderer sprite, VertexAttribute channel, out SpriteChannelInfo ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetSafetyHandle_Injected(SpriteRenderer spriteRenderer, out AtomicSafetyHandle ret);
	}
}
