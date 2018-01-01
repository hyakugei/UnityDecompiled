using System;
using System.Runtime.InteropServices;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.XR.WSA.Input
{
	[MovedFrom("UnityEngine.VR.WSA.Input")]
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct GestureErrorEventArgs
	{
		public string error
		{
			get;
			private set;
		}

		public int hresult
		{
			get;
			private set;
		}

		public GestureErrorEventArgs(string error, int hresult)
		{
			this = default(GestureErrorEventArgs);
			this.error = error;
			this.hresult = hresult;
		}
	}
}
