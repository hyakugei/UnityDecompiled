using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
	[RequiredByNativeCode]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class Coroutine : YieldInstruction
	{
		internal IntPtr m_Ptr;

		private Coroutine()
		{
		}

		~Coroutine()
		{
			Coroutine.ReleaseCoroutine(this.m_Ptr);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void ReleaseCoroutine(IntPtr ptr);
	}
}
