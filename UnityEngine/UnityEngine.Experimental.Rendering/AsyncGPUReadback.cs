using System;
using System.Runtime.CompilerServices;

namespace UnityEngine.Experimental.Rendering
{
	public static class AsyncGPUReadback
	{
		public static AsyncGPUReadbackRequest Request(ComputeBuffer src)
		{
			if (src == null)
			{
				throw new ArgumentNullException();
			}
			AsyncGPUReadbackRequest result = AsyncGPUReadback.Request_Internal_ComputeBuffer_1(src);
			result.CreateSafetyHandle();
			return result;
		}

		public static AsyncGPUReadbackRequest Request(ComputeBuffer src, int size, int offset)
		{
			if (src == null)
			{
				throw new ArgumentNullException();
			}
			AsyncGPUReadbackRequest result = AsyncGPUReadback.Request_Internal_ComputeBuffer_2(src, size, offset);
			result.CreateSafetyHandle();
			return result;
		}

		public static AsyncGPUReadbackRequest Request(Texture src, int mipIndex = 0)
		{
			if (src == null)
			{
				throw new ArgumentNullException();
			}
			AsyncGPUReadbackRequest result = AsyncGPUReadback.Request_Internal_Texture_1(src, mipIndex);
			result.CreateSafetyHandle();
			return result;
		}

		public static AsyncGPUReadbackRequest Request(Texture src, int mipIndex, TextureFormat dstFormat)
		{
			if (src == null)
			{
				throw new ArgumentNullException();
			}
			AsyncGPUReadbackRequest result = AsyncGPUReadback.Request_Internal_Texture_2(src, mipIndex, dstFormat);
			result.CreateSafetyHandle();
			return result;
		}

		public static AsyncGPUReadbackRequest Request(Texture src, int mipIndex, int x, int width, int y, int height, int z, int depth)
		{
			if (src == null)
			{
				throw new ArgumentNullException();
			}
			AsyncGPUReadbackRequest result = AsyncGPUReadback.Request_Internal_Texture_3(src, mipIndex, x, width, y, height, z, depth);
			result.CreateSafetyHandle();
			return result;
		}

		public static AsyncGPUReadbackRequest Request(Texture src, int mipIndex, int x, int width, int y, int height, int z, int depth, TextureFormat dstFormat)
		{
			if (src == null)
			{
				throw new ArgumentNullException();
			}
			AsyncGPUReadbackRequest result = AsyncGPUReadback.Request_Internal_Texture_4(src, mipIndex, x, width, y, height, z, depth, dstFormat);
			result.CreateSafetyHandle();
			return result;
		}

		private static AsyncGPUReadbackRequest Request_Internal_ComputeBuffer_1(ComputeBuffer buffer)
		{
			AsyncGPUReadbackRequest result;
			AsyncGPUReadback.Request_Internal_ComputeBuffer_1_Injected(buffer, out result);
			return result;
		}

		private static AsyncGPUReadbackRequest Request_Internal_ComputeBuffer_2(ComputeBuffer src, int size, int offset)
		{
			AsyncGPUReadbackRequest result;
			AsyncGPUReadback.Request_Internal_ComputeBuffer_2_Injected(src, size, offset, out result);
			return result;
		}

		private static AsyncGPUReadbackRequest Request_Internal_Texture_1(Texture src, int mipIndex)
		{
			AsyncGPUReadbackRequest result;
			AsyncGPUReadback.Request_Internal_Texture_1_Injected(src, mipIndex, out result);
			return result;
		}

		private static AsyncGPUReadbackRequest Request_Internal_Texture_2(Texture src, int mipIndex, TextureFormat dstFormat)
		{
			AsyncGPUReadbackRequest result;
			AsyncGPUReadback.Request_Internal_Texture_2_Injected(src, mipIndex, dstFormat, out result);
			return result;
		}

		private static AsyncGPUReadbackRequest Request_Internal_Texture_3(Texture src, int mipIndex, int x, int width, int y, int height, int z, int depth)
		{
			AsyncGPUReadbackRequest result;
			AsyncGPUReadback.Request_Internal_Texture_3_Injected(src, mipIndex, x, width, y, height, z, depth, out result);
			return result;
		}

		private static AsyncGPUReadbackRequest Request_Internal_Texture_4(Texture src, int mipIndex, int x, int width, int y, int height, int z, int depth, TextureFormat dstFormat)
		{
			AsyncGPUReadbackRequest result;
			AsyncGPUReadback.Request_Internal_Texture_4_Injected(src, mipIndex, x, width, y, height, z, depth, dstFormat, out result);
			return result;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Request_Internal_ComputeBuffer_1_Injected(ComputeBuffer buffer, out AsyncGPUReadbackRequest ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Request_Internal_ComputeBuffer_2_Injected(ComputeBuffer src, int size, int offset, out AsyncGPUReadbackRequest ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Request_Internal_Texture_1_Injected(Texture src, int mipIndex, out AsyncGPUReadbackRequest ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Request_Internal_Texture_2_Injected(Texture src, int mipIndex, TextureFormat dstFormat, out AsyncGPUReadbackRequest ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Request_Internal_Texture_3_Injected(Texture src, int mipIndex, int x, int width, int y, int height, int z, int depth, out AsyncGPUReadbackRequest ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Request_Internal_Texture_4_Injected(Texture src, int mipIndex, int x, int width, int y, int height, int z, int depth, TextureFormat dstFormat, out AsyncGPUReadbackRequest ret);
	}
}
