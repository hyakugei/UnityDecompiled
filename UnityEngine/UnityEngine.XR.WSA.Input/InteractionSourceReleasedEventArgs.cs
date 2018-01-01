using System;
using System.Runtime.InteropServices;

namespace UnityEngine.XR.WSA.Input
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
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
