using System;
using UnityEngine.Scripting;

namespace UnityEngine
{
	[UsedByNativeCode]
	public struct ContactPoint2D
	{
		private Vector2 m_Point;

		private Vector2 m_Normal;

		private Vector2 m_RelativeVelocity;

		private float m_Separation;

		private float m_NormalImpulse;

		private float m_TangentImpulse;

		private int m_Collider;

		private int m_OtherCollider;

		private int m_Rigidbody;

		private int m_OtherRigidbody;

		private int m_Enabled;

		public Vector2 point
		{
			get
			{
				return this.m_Point;
			}
		}

		public Vector2 normal
		{
			get
			{
				return this.m_Normal;
			}
		}

		public float separation
		{
			get
			{
				return this.m_Separation;
			}
		}

		public float normalImpulse
		{
			get
			{
				return this.m_NormalImpulse;
			}
		}

		public float tangentImpulse
		{
			get
			{
				return this.m_TangentImpulse;
			}
		}

		public Vector2 relativeVelocity
		{
			get
			{
				return this.m_RelativeVelocity;
			}
		}

		public Collider2D collider
		{
			get
			{
				return Object.FindObjectFromInstanceID(this.m_Collider) as Collider2D;
			}
		}

		public Collider2D otherCollider
		{
			get
			{
				return Object.FindObjectFromInstanceID(this.m_OtherCollider) as Collider2D;
			}
		}

		public Rigidbody2D rigidbody
		{
			get
			{
				return Object.FindObjectFromInstanceID(this.m_Rigidbody) as Rigidbody2D;
			}
		}

		public Rigidbody2D otherRigidbody
		{
			get
			{
				return Object.FindObjectFromInstanceID(this.m_OtherRigidbody) as Rigidbody2D;
			}
		}

		public bool enabled
		{
			get
			{
				return this.m_Enabled == 1;
			}
		}
	}
}
