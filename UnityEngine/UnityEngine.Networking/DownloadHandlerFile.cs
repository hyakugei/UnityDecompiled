using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace UnityEngine.Networking
{
	[StructLayout(LayoutKind.Sequential)]
	public sealed class DownloadHandlerFile : DownloadHandler
	{
		public extern bool removeFileOnAbort
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public DownloadHandlerFile(string path)
		{
			this.InternalCreateVFS(path);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr Create(DownloadHandlerFile obj, string path);

		private void InternalCreateVFS(string path)
		{
			this.m_Ptr = DownloadHandlerFile.Create(this, path);
		}

		protected override byte[] GetData()
		{
			throw new NotSupportedException("Raw data access is not supported");
		}

		protected override string GetText()
		{
			throw new NotSupportedException("String access is not supported");
		}
	}
}
