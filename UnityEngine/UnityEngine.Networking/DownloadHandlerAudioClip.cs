using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace UnityEngine.Networking
{
	[StructLayout(LayoutKind.Sequential)]
	public sealed class DownloadHandlerAudioClip : DownloadHandler
	{
		public extern AudioClip audioClip
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public DownloadHandlerAudioClip(string url, AudioType audioType)
		{
			this.InternalCreateAudioClip(url, audioType);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr Create(DownloadHandlerAudioClip obj, string url, AudioType audioType);

		private void InternalCreateAudioClip(string url, AudioType audioType)
		{
			this.m_Ptr = DownloadHandlerAudioClip.Create(this, url, audioType);
		}

		protected override byte[] GetData()
		{
			return DownloadHandler.InternalGetByteArray(this);
		}

		protected override string GetText()
		{
			throw new NotSupportedException("String access is not supported for audio clips");
		}

		public static AudioClip GetContent(UnityWebRequest www)
		{
			return DownloadHandler.GetCheckedDownloader<DownloadHandlerAudioClip>(www).audioClip;
		}
	}
}
