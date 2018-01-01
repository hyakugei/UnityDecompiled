using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

namespace UnityEditorInternal
{
	public class BlendTreePreviewUtility
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void GetRootBlendTreeChildWeights(Animator animator, int layerIndex, int stateHash, [Out] float[] weightArray);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void CalculateRootBlendTreeChildWeights(Animator animator, int layerIndex, int stateHash, [Out] float[] weightArray, float blendX, float blendY);

		public static void CalculateBlendTexture(Animator animator, int layerIndex, int stateHash, Texture2D blendTexture, Texture2D[] weightTextures, Rect rect)
		{
			BlendTreePreviewUtility.CalculateBlendTexture(animator, layerIndex, stateHash, blendTexture, weightTextures, rect.x, rect.y, rect.x + rect.width, rect.y + rect.height);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		protected static extern void CalculateBlendTexture(Animator animator, int layerIndex, int stateHash, Texture2D blendTexture, Texture2D[] weightTextures, float minX, float minY, float maxX, float maxY);
	}
}
