using System;
using System.Runtime.InteropServices;

namespace UnityEngine.XR.WSA.Input
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct InteractionSourceLostEventArgs
	{
		public InteractionSourceState state
		{
			get;
			private set;
		}

		public InteractionSourceLostEventArgs(InteractionSourceState state)
		{
			this = default(InteractionSourceLostEventArgs);
			this.state = state;
		}
	}
}
