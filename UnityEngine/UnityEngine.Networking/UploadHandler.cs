using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace UnityEngine.Networking
{
	[StructLayout(LayoutKind.Sequential)]
	public class UploadHandler : IDisposable
	{
		[NonSerialized]
		internal IntPtr m_Ptr;

		public byte[] data
		{
			get
			{
				return this.GetData();
			}
		}

		public string contentType
		{
			get
			{
				return this.GetContentType();
			}
			set
			{
				this.SetContentType(value);
			}
		}

		public float progress
		{
			get
			{
				return this.GetProgress();
			}
		}

		internal UploadHandler()
		{
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Release();

		~UploadHandler()
		{
			this.Dispose();
		}

		public void Dispose()
		{
			if (this.m_Ptr != IntPtr.Zero)
			{
				this.Release();
				this.m_Ptr = IntPtr.Zero;
			}
		}

		internal virtual byte[] GetData()
		{
			return null;
		}

		internal virtual string GetContentType()
		{
			return "text/plain";
		}

		internal virtual void SetContentType(string newContentType)
		{
		}

		internal virtual float GetProgress()
		{
			return 0.5f;
		}
	}
}
