using System;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.Experimental.Rendering;

namespace UnityEngine.Experimental.U2D
{
	public static class SpriteDataAccessExtensions
	{
		private static void CheckAttributeTypeMatchesAndThrow<T>(VertexAttribute channel)
		{
			bool flag;
			switch (channel)
			{
			case VertexAttribute.Position:
			case VertexAttribute.Normal:
				flag = (typeof(T) == typeof(Vector3));
				break;
			case VertexAttribute.Tangent:
				flag = (typeof(T) == typeof(Vector4));
				break;
			case VertexAttribute.Color:
				flag = (typeof(T) == typeof(Color32));
				break;
			case VertexAttribute.TexCoord0:
			case VertexAttribute.TexCoord1:
			case VertexAttribute.TexCoord2:
			case VertexAttribute.TexCoord3:
			case VertexAttribute.TexCoord4:
			case VertexAttribute.TexCoord5:
			case VertexAttribute.TexCoord6:
				flag = (typeof(T) == typeof(Vector2));
				break;
			default:
				throw new InvalidOperationException(string.Format("The requested channel '{0}' is unknown.", channel));
			}
			if (!flag)
			{
				throw new InvalidOperationException(string.Format("The requested channel '{0}' does not match the return type {1}.", channel, typeof(T).Name));
			}
		}

		public static NativeSlice<T> GetVertexAttribute<T>(this Sprite sprite, VertexAttribute channel) where T : struct
		{
			SpriteDataAccessExtensions.CheckAttributeTypeMatchesAndThrow<T>(channel);
			SpriteChannelInfo channelInfo = SpriteDataAccessExtensions.GetChannelInfo(sprite, channel);
			NativeSlice<T> result = NativeSliceUnsafeUtility.ConvertExistingDataToNativeSlice<T>(channelInfo.buffer, channelInfo.offset, channelInfo.count, channelInfo.stride);
			NativeSliceUnsafeUtility.SetAtomicSafetyHandle<T>(ref result, sprite.GetSafetyHandle());
			return result;
		}

		public static void SetVertexAttribute<T>(this Sprite sprite, VertexAttribute channel, NativeArray<T> src) where T : struct
		{
			SpriteDataAccessExtensions.CheckAttributeTypeMatchesAndThrow<T>(channel);
			SpriteDataAccessExtensions.SetChannelData(sprite, channel, src.GetUnsafeReadOnlyPtr<T>());
		}

		public static NativeArray<Matrix4x4> GetBindPoses(this Sprite sprite)
		{
			SpriteChannelInfo bindPoseInfo = SpriteDataAccessExtensions.GetBindPoseInfo(sprite);
			NativeArray<Matrix4x4> result = NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<Matrix4x4>(bindPoseInfo.buffer, bindPoseInfo.count, Allocator.Invalid);
			NativeArrayUnsafeUtility.SetAtomicSafetyHandle<Matrix4x4>(ref result, sprite.GetSafetyHandle());
			return result;
		}

		public static void SetBindPoses(this Sprite sprite, NativeArray<Matrix4x4> src)
		{
			SpriteDataAccessExtensions.SetBindPoseData(sprite, src.GetUnsafeReadOnlyPtr<Matrix4x4>(), src.Length);
		}

		public static NativeArray<ushort> GetIndices(this Sprite sprite)
		{
			SpriteChannelInfo indicesInfo = SpriteDataAccessExtensions.GetIndicesInfo(sprite);
			NativeArray<ushort> result = NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<ushort>(indicesInfo.buffer, indicesInfo.count, Allocator.Invalid);
			NativeArrayUnsafeUtility.SetAtomicSafetyHandle<ushort>(ref result, sprite.GetSafetyHandle());
			return result;
		}

		public static void SetIndices(this Sprite sprite, NativeArray<ushort> src)
		{
			SpriteDataAccessExtensions.SetIndicesData(sprite, src.GetUnsafeReadOnlyPtr<ushort>(), src.Length);
		}

		public static NativeArray<BoneWeight> GetBoneWeights(this Sprite sprite)
		{
			SpriteChannelInfo boneWeightsInfo = SpriteDataAccessExtensions.GetBoneWeightsInfo(sprite);
			NativeArray<BoneWeight> result = NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<BoneWeight>(boneWeightsInfo.buffer, boneWeightsInfo.count, Allocator.Invalid);
			NativeArrayUnsafeUtility.SetAtomicSafetyHandle<BoneWeight>(ref result, sprite.GetSafetyHandle());
			return result;
		}

		public static void SetBoneWeights(this Sprite sprite, NativeArray<BoneWeight> src)
		{
			SpriteDataAccessExtensions.SetBoneWeightsData(sprite, src.GetUnsafeReadOnlyPtr<BoneWeight>(), src.Length);
		}

		public static SpriteBone[] GetBones(this Sprite sprite)
		{
			return SpriteDataAccessExtensions.GetBoneInfo(sprite);
		}

		public static void SetBones(this Sprite sprite, SpriteBone[] src)
		{
			SpriteDataAccessExtensions.SetBoneData(sprite, src);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool HasVertexAttribute(this Sprite sprite, VertexAttribute channel);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetVertexCount(this Sprite sprite, int count);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int GetVertexCount(this Sprite sprite);

		private static SpriteChannelInfo GetBindPoseInfo(Sprite sprite)
		{
			SpriteChannelInfo result;
			SpriteDataAccessExtensions.GetBindPoseInfo_Injected(sprite, out result);
			return result;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetBindPoseData(Sprite sprite, IntPtr src, int count);

		private static SpriteChannelInfo GetIndicesInfo(Sprite sprite)
		{
			SpriteChannelInfo result;
			SpriteDataAccessExtensions.GetIndicesInfo_Injected(sprite, out result);
			return result;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetIndicesData(Sprite sprite, IntPtr src, int count);

		private static SpriteChannelInfo GetChannelInfo(Sprite sprite, VertexAttribute channel)
		{
			SpriteChannelInfo result;
			SpriteDataAccessExtensions.GetChannelInfo_Injected(sprite, channel, out result);
			return result;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetChannelData(Sprite sprite, VertexAttribute channel, IntPtr src);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern SpriteBone[] GetBoneInfo(Sprite sprite);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetBoneData(Sprite sprite, SpriteBone[] src);

		private static SpriteChannelInfo GetBoneWeightsInfo(Sprite sprite)
		{
			SpriteChannelInfo result;
			SpriteDataAccessExtensions.GetBoneWeightsInfo_Injected(sprite, out result);
			return result;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetBoneWeightsData(Sprite sprite, IntPtr src, int count);

		private static AtomicSafetyHandle GetSafetyHandle(this Sprite sprite)
		{
			AtomicSafetyHandle result;
			SpriteDataAccessExtensions.GetSafetyHandle_Injected(sprite, out result);
			return result;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetBindPoseInfo_Injected(Sprite sprite, out SpriteChannelInfo ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetIndicesInfo_Injected(Sprite sprite, out SpriteChannelInfo ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetChannelInfo_Injected(Sprite sprite, VertexAttribute channel, out SpriteChannelInfo ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetBoneWeightsInfo_Injected(Sprite sprite, out SpriteChannelInfo ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetSafetyHandle_Injected(Sprite sprite, out AtomicSafetyHandle ret);
	}
}
