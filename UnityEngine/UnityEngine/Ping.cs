using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public sealed class Ping
	{
		internal IntPtr m_Ptr;

		public bool isDone
		{
			get
			{
				return !(this.m_Ptr == IntPtr.Zero) && this.Internal_IsDone();
			}
		}

		public extern int time
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public extern string ip
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public Ping(string address)
		{
			this.m_Ptr = Ping.Internal_Create(address);
		}

		~Ping()
		{
			this.DestroyPing();
		}

		[ThreadAndSerializationSafe]
		public void DestroyPing()
		{
			this.m_Ptr = IntPtr.Zero;
			Ping.Internal_Destroy(this.m_Ptr);
		}

		[ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_Destroy(IntPtr ptr);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr Internal_Create(string address);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern bool Internal_IsDone();
	}
}
