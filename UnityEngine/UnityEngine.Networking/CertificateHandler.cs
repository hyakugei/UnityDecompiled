using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Scripting;

namespace UnityEngine.Networking
{
	[StructLayout(LayoutKind.Sequential)]
	public class CertificateHandler : IDisposable
	{
		[NonSerialized]
		internal IntPtr m_Ptr;

		protected CertificateHandler()
		{
			this.m_Ptr = CertificateHandler.Create(this);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr Create(CertificateHandler obj);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Release();

		~CertificateHandler()
		{
			this.Dispose();
		}

		protected virtual bool ValidateCertificate(byte[] certificateData)
		{
			return false;
		}

		[RequiredByNativeCode]
		internal bool ValidateCertificateNative(byte[] certificateData)
		{
			return this.ValidateCertificate(certificateData);
		}

		public void Dispose()
		{
			if (this.m_Ptr != IntPtr.Zero)
			{
				this.Release();
				this.m_Ptr = IntPtr.Zero;
			}
		}
	}
}
