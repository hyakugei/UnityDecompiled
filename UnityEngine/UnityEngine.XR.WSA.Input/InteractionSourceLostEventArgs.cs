using System;

namespace UnityEngine.XR.WSA.Input
{
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
