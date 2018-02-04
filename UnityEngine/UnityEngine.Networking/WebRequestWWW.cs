using System;
using System.Runtime.CompilerServices;

namespace UnityEngine.Networking
{
	internal static class WebRequestWWW
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern AudioClip InternalCreateAudioClipUsingDH(DownloadHandler dh, string url, bool stream, bool compressed, AudioType audioType);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern MovieTexture InternalCreateMovieTextureUsingDH(DownloadHandler dh);
	}
}
