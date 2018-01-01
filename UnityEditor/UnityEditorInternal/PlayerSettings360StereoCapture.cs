using System;
using System.Runtime.CompilerServices;

namespace UnityEditorInternal
{
	internal class PlayerSettings360StereoCapture
	{
		public static extern bool enable360StereoCapture
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}
	}
}
