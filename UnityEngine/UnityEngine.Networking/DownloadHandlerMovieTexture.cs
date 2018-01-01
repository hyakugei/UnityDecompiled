using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace UnityEngine.Networking
{
	[StructLayout(LayoutKind.Sequential)]
	public sealed class DownloadHandlerMovieTexture : DownloadHandler
	{
		public extern MovieTexture movieTexture
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public DownloadHandlerMovieTexture()
		{
			this.InternalCreateDHMovieTexture();
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr Create(DownloadHandlerMovieTexture obj);

		private void InternalCreateDHMovieTexture()
		{
			this.m_Ptr = DownloadHandlerMovieTexture.Create(this);
		}

		protected override byte[] GetData()
		{
			return DownloadHandler.InternalGetByteArray(this);
		}

		protected override string GetText()
		{
			throw new NotSupportedException("String access is not supported for movies");
		}

		public static MovieTexture GetContent(UnityWebRequest uwr)
		{
			return DownloadHandler.GetCheckedDownloader<DownloadHandlerMovieTexture>(uwr).movieTexture;
		}
	}
}
