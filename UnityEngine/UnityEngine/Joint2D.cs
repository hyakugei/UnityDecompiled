using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	[RequireComponent(typeof(Transform), typeof(Rigidbody2D))]
	public class Joint2D : Behaviour
	{
		public extern Rigidbody2D attachedRigidbody
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public extern Rigidbody2D connectedBody
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public extern bool enableCollision
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public extern float breakForce
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public extern float breakTorque
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public Vector2 reactionForce
		{
			get
			{
				Vector2 result;
				this.get_reactionForce_Injected(out result);
				return result;
			}
		}

		public extern float reactionTorque
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		[Obsolete("Joint2D.collideConnected has been deprecated. Use Joint2D.enableCollision instead (UnityUpgradable) -> enableCollision", true)]
		public bool collideConnected
		{
			get
			{
				return this.enableCollision;
			}
			set
			{
				this.enableCollision = value;
			}
		}

		public Vector2 GetReactionForce(float timeStep)
		{
			Vector2 result;
			this.GetReactionForce_Injected(timeStep, out result);
			return result;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern float GetReactionTorque(float timeStep);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_reactionForce_Injected(out Vector2 ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetReactionForce_Injected(float timeStep, out Vector2 ret);
	}
}
