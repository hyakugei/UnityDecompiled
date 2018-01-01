using System;
using System.Runtime.InteropServices;

namespace UnityEngine.XR.WSA.Input
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct InteractionSourceDetectedEventArgs
	{
		public InteractionSourceState state
		{
			get;
			private set;
		}

		public InteractionSourceDetectedEventArgs(InteractionSourceState state)
		{
			this = default(InteractionSourceDetectedEventArgs);
			this.state = state;
		}
	}
}
