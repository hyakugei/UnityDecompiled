using System;
using System.Runtime.InteropServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEditor.PackageManager
{
	[RequiredByNativeCode]
	[StructLayout(LayoutKind.Sequential)]
	internal class OperationStatus
	{
		[NativeName("status")]
		private NativeStatusCode m_Status;

		[NativeName("id")]
		private string m_Id;

		[NativeName("type")]
		private string m_Type;

		[NativeName("packageList")]
		private PackageInfo[] m_PackageList;

		[NativeName("progress")]
		private float m_Progress;

		[NativeName("error")]
		private Error m_Error;

		public string id
		{
			get
			{
				return this.m_Id;
			}
		}

		public NativeStatusCode status
		{
			get
			{
				return this.m_Status;
			}
		}

		public string type
		{
			get
			{
				return this.m_Type;
			}
		}

		public PackageInfo[] packageList
		{
			get
			{
				return this.m_PackageList;
			}
		}

		public float progress
		{
			get
			{
				return this.m_Progress;
			}
		}

		public Error error
		{
			get
			{
				Error result;
				if (this.m_Error != null && this.m_Error.errorCode == ErrorCode.Unknown && this.m_Error.message == "")
				{
					result = null;
				}
				else
				{
					result = this.m_Error;
				}
				return result;
			}
		}

		private OperationStatus()
		{
		}
	}
}
