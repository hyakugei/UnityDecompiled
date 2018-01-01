using System;

namespace UnityEngine.XR.WSA.Input
{
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
