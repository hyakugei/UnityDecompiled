using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
	[UsedByNativeCode]
	[StructLayout(LayoutKind.Sequential)]
	public class Collision2D
	{
		internal int m_Collider;

		internal int m_OtherCollider;

		internal int m_Rigidbody;

		internal int m_OtherRigidbody;

		internal ContactPoint2D[] m_Contacts;

		internal Vector2 m_RelativeVelocity;

		internal int m_Enabled;

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

		public Transform transform
		{
			get
			{
				return (!(this.rigidbody != null)) ? this.collider.transform : this.rigidbody.transform;
			}
		}

		public GameObject gameObject
		{
			get
			{
				return (!(this.rigidbody != null)) ? this.collider.gameObject : this.rigidbody.gameObject;
			}
		}

		public ContactPoint2D[] contacts
		{
			get
			{
				if (this.m_Contacts == null)
				{
					this.m_Contacts = Collision2D.CreateCollisionContacts_Internal(this.collider, this.otherCollider, this.rigidbody, this.otherRigidbody, this.enabled);
				}
				return this.m_Contacts;
			}
		}

		public Vector2 relativeVelocity
		{
			get
			{
				return this.m_RelativeVelocity;
			}
		}

		public bool enabled
		{
			get
			{
				return this.m_Enabled == 1;
			}
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern ContactPoint2D[] CreateCollisionContacts_Internal(Collider2D collider, Collider2D otherCollider, Rigidbody2D rigidbody, Rigidbody2D otherRigidbody, bool enabled);

		public int GetContacts(ContactPoint2D[] contacts)
		{
			return Physics2D.GetContacts(this.collider, this.otherCollider, default(ContactFilter2D).NoFilter(), contacts);
		}
	}
}
