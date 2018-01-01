using System;

namespace UnityEngine.XR.WSA.Input
{
	public struct InteractionSourceReleasedEventArgs
	{
		public InteractionSourceState state
		{
			get;
			private set;
		}

		public InteractionSourcePressType pressType
		{
			get;
			private set;
		}

		public InteractionSourceReleasedEventArgs(InteractionSourceState state, InteractionSourcePressType pressType)
		{
			this = default(InteractionSourceReleasedEventArgs);
			this.state = state;
			this.pressType = pressType;
		}
	}
}
