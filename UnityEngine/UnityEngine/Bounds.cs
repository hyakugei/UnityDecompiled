using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine
{
	[NativeType(Header = "Runtime/Geometry/AABB.h"), UsedByNativeCode, ThreadAndSerializationSafe]
	public struct Bounds
	{
		private Vector3 m_Center;

		private Vector3 m_Extents;

		public Vector3 center
		{
			get
			{
				return this.m_Center;
			}
			set
			{
				this.m_Center = value;
			}
		}

		public Vector3 size
		{
			get
			{
				return this.m_Extents * 2f;
			}
			set
			{
				this.m_Extents = value * 0.5f;
			}
		}

		public Vector3 extents
		{
			get
			{
				return this.m_Extents;
			}
			set
			{
				this.m_Extents = value;
			}
		}

		public Vector3 min
		{
			get
			{
				return this.center - this.extents;
			}
			set
			{
				this.SetMinMax(value, this.max);
			}
		}

		public Vector3 max
		{
			get
			{
				return this.center + this.extents;
			}
			set
			{
				this.SetMinMax(this.min, value);
			}
		}

		public Bounds(Vector3 center, Vector3 size)
		{
			this.m_Center = center;
			this.m_Extents = size * 0.5f;
		}

		public override int GetHashCode()
		{
			return this.center.GetHashCode() ^ this.extents.GetHashCode() << 2;
		}

		public override bool Equals(object other)
		{
			bool result;
			if (!(other is Bounds))
			{
				result = false;
			}
			else
			{
				Bounds bounds = (Bounds)other;
				result = (this.center.Equals(bounds.center) && this.extents.Equals(bounds.extents));
			}
			return result;
		}

		public static bool operator ==(Bounds lhs, Bounds rhs)
		{
			return lhs.center == rhs.center && lhs.extents == rhs.extents;
		}

		public static bool operator !=(Bounds lhs, Bounds rhs)
		{
			return !(lhs == rhs);
		}

		public void SetMinMax(Vector3 min, Vector3 max)
		{
			this.extents = (max - min) * 0.5f;
			this.center = min + this.extents;
		}

		public void Encapsulate(Vector3 point)
		{
			this.SetMinMax(Vector3.Min(this.min, point), Vector3.Max(this.max, point));
		}

		public void Encapsulate(Bounds bounds)
		{
			this.Encapsulate(bounds.center - bounds.extents);
			this.Encapsulate(bounds.center + bounds.extents);
		}

		public void Expand(float amount)
		{
			amount *= 0.5f;
			this.extents += new Vector3(amount, amount, amount);
		}

		public void Expand(Vector3 amount)
		{
			this.extents += amount * 0.5f;
		}

		public bool Intersects(Bounds bounds)
		{
			return this.min.x <= bounds.max.x && this.max.x >= bounds.min.x && this.min.y <= bounds.max.y && this.max.y >= bounds.min.y && this.min.z <= bounds.max.z && this.max.z >= bounds.min.z;
		}

		public bool IntersectRay(Ray ray)
		{
			float num;
			return Bounds.IntersectRayAABB(ray, this, out num);
		}

		public bool IntersectRay(Ray ray, out float distance)
		{
			return Bounds.IntersectRayAABB(ray, this, out distance);
		}

		public override string ToString()
		{
			return UnityString.Format("Center: {0}, Extents: {1}", new object[]
			{
				this.m_Center,
				this.m_Extents
			});
		}

		public string ToString(string format)
		{
			return UnityString.Format("Center: {0}, Extents: {1}", new object[]
			{
				this.m_Center.ToString(format),
				this.m_Extents.ToString(format)
			});
		}

		public bool Contains(Vector3 point)
		{
			return Bounds.Contains_Injected(ref this, ref point);
		}

		public float SqrDistance(Vector3 point)
		{
			return Bounds.SqrDistance_Injected(ref this, ref point);
		}

		private static bool IntersectRayAABB(Ray ray, Bounds bounds, out float dist)
		{
			return Bounds.IntersectRayAABB_Injected(ref ray, ref bounds, out dist);
		}

		public Vector3 ClosestPoint(Vector3 point)
		{
			Vector3 result;
			Bounds.ClosestPoint_Injected(ref this, ref point, out result);
			return result;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool Contains_Injected(ref Bounds _unity_self, ref Vector3 point);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern float SqrDistance_Injected(ref Bounds _unity_self, ref Vector3 point);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool IntersectRayAABB_Injected(ref Ray ray, ref Bounds bounds, out float dist);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void ClosestPoint_Injected(ref Bounds _unity_self, ref Vector3 point, out Vector3 ret);
	}
}
