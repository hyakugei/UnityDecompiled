using System;
using System.Runtime.InteropServices;

namespace UnityEngine.XR.WSA.Input
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
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
