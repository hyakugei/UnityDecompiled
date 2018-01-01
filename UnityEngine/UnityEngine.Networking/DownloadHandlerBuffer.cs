using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace UnityEngine.Networking
{
	[StructLayout(LayoutKind.Sequential)]
	public sealed class DownloadHandlerBuffer : DownloadHandler
	{
		public DownloadHandlerBuffer()
		{
			this.InternalCreateBuffer();
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr Create(DownloadHandlerBuffer obj);

		private void InternalCreateBuffer()
		{
			this.m_Ptr = DownloadHandlerBuffer.Create(this);
		}

		protected override byte[] GetData()
		{
			return this.InternalGetData();
		}

		private byte[] InternalGetData()
		{
			return DownloadHandler.InternalGetByteArray(this);
		}

		public static string GetContent(UnityWebRequest www)
		{
			return DownloadHandler.GetCheckedDownloader<DownloadHandlerBuffer>(www).text;
		}
	}
}
