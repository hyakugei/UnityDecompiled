using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public class Motion : Object
	{
		public extern float averageDuration
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public extern float averageAngularSpeed
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public Vector3 averageSpeed
		{
			get
			{
				Vector3 result;
				this.get_averageSpeed_Injected(out result);
				return result;
			}
		}

		public extern float apparentSpeed
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public extern bool isLooping
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public extern bool legacy
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public extern bool isHumanMotion
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		[Obsolete("isAnimatorMotion is not supported anymore, please use !legacy instead.", true)]
		public bool isAnimatorMotion
		{
			[CompilerGenerated]
			get
			{
				return this.<isAnimatorMotion>k__BackingField;
			}
		}

		protected Motion()
		{
		}

		[Obsolete("ValidateIfRetargetable is not supported anymore, please use isHumanMotion instead.", true)]
		public bool ValidateIfRetargetable(bool val)
		{
			return false;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_averageSpeed_Injected(out Vector3 ret);
	}
}
