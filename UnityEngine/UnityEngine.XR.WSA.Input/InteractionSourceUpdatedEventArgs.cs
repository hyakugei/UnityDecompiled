using System;

namespace UnityEngine.XR.WSA.Input
{
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
