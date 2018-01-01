using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	internal class ObjectGUIState : IDisposable
	{
		internal IntPtr m_Ptr;

		public ObjectGUIState()
		{
			this.m_Ptr = ObjectGUIState.Internal_Create();
		}

		public void Dispose()
		{
			this.Destroy();
			GC.SuppressFinalize(this);
		}

		~ObjectGUIState()
		{
			this.Destroy();
		}

		private void Destroy()
		{
			if (this.m_Ptr != IntPtr.Zero)
			{
				ObjectGUIState.Internal_Destroy(this.m_Ptr);
				this.m_Ptr = IntPtr.Zero;
			}
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr Internal_Create();

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_Destroy(IntPtr ptr);
	}
}
