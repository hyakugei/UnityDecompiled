using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public sealed class PhysicsMaterial2D : Object
	{
		public extern float bounciness
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public extern float friction
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public PhysicsMaterial2D()
		{
			PhysicsMaterial2D.Create_Internal(this, null);
		}

		public PhysicsMaterial2D(string name)
		{
			PhysicsMaterial2D.Create_Internal(this, name);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Create_Internal([Writable] PhysicsMaterial2D scriptMaterial, string name);
	}
}
