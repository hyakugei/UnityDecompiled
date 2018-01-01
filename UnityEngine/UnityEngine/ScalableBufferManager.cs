using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public static class ScalableBufferManager
	{
		public static float widthScaleFactor
		{
			get
			{
				return ScalableBufferManager.GetWidthScaleFactor();
			}
		}

		public static float heightScaleFactor
		{
			get
			{
				return ScalableBufferManager.GetHeightScaleFactor();
			}
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void ResizeBuffers(float widthScale, float heightScale);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern float GetWidthScaleFactor();

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern float GetHeightScaleFactor();
	}
}
