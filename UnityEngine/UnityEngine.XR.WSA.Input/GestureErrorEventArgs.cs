using System;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.XR.WSA.Input
{
	[MovedFrom("UnityEngine.VR.WSA.Input")]
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
