using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.XR.WSA.WebCam
{
	[MovedFrom("UnityEngine.VR.WSA.WebCam")]
	public static class WebCam
	{
		public static WebCamMode Mode
		{
			get
			{
				return (WebCamMode)WebCam.GetWebCamModeState_Internal();
			}
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetWebCamModeState_Internal();
	}
}
