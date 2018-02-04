using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEditor.PackageManager
{
	[RequiredByNativeCode]
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class Error
	{
		[NativeName("errorCode"), SerializeField]
		private ErrorCode m_ErrorCode;

		[NativeName("message"), SerializeField]
		private string m_Message;

		public ErrorCode errorCode
		{
			get
			{
				return this.m_ErrorCode;
			}
		}

		public string message
		{
			get
			{
				return this.m_Message;
			}
		}

		private Error()
		{
		}

		internal Error(ErrorCode errorCode, string message)
		{
			this.m_ErrorCode = errorCode;
			this.m_Message = message;
		}
	}
}
