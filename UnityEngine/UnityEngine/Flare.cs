using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public sealed class Flare : Object
	{
		public Flare()
		{
			Flare.Internal_Create(this);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_Create([Writable] Flare self);
	}
}
