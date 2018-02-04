using System;

namespace UnityEngine.Tizen
{
	public sealed class Window
	{
		public static IntPtr windowHandle
		{
			get
			{
				return (IntPtr)null;
			}
		}

		public static IntPtr evasGL
		{
			get
			{
				return IntPtr.Zero;
			}
		}
	}
}
