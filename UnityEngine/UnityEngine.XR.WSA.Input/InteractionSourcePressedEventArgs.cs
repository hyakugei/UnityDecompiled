using System;

namespace UnityEngine.XR.WSA.Input
{
	public struct InteractionSourcePressedEventArgs
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

		public InteractionSourcePressedEventArgs(InteractionSourceState state, InteractionSourcePressType pressType)
		{
			this = default(InteractionSourcePressedEventArgs);
			this.state = state;
			this.pressType = pressType;
		}
	}
}
