using System;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;

namespace UnityEngine
{
	public static class ScreenCapture
	{
		public static void CaptureScreenshot(string filename)
		{
			ScreenCapture.CaptureScreenshot(filename, 1);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void CaptureScreenshot(string filename, [DefaultValue("1")] int superSize);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern Texture2D CaptureScreenshotAsTexture(int superSize = 1);
	}
}
