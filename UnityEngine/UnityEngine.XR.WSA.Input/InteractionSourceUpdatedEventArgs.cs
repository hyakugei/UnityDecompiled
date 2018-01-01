using System;
using System.Runtime.InteropServices;

namespace UnityEngine.XR.WSA.Input
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct InteractionSourceUpdatedEventArgs
	{
		public InteractionSourceState state
		{
			get;
			private set;
		}

		public InteractionSourceUpdatedEventArgs(InteractionSourceState state)
		{
			this = default(InteractionSourceUpdatedEventArgs);
			this.state = state;
		}
	}
}
