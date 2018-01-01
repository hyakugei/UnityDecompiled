using System;
using System.Runtime.CompilerServices;

namespace UnityEngine.Jobs
{
	public struct TransformAccess
	{
		private IntPtr hierarchy;

		private int index;

		public Vector3 position
		{
			get
			{
				Vector3 result;
				TransformAccess.GetPosition(ref this, out result);
				return result;
			}
			set
			{
				TransformAccess.SetPosition(ref this, ref value);
			}
		}

		public Quaternion rotation
		{
			get
			{
				Quaternion result;
				TransformAccess.GetRotation(ref this, out result);
				return result;
			}
			set
			{
				TransformAccess.SetRotation(ref this, ref value);
			}
		}

		public Vector3 localPosition
		{
			get
			{
				Vector3 result;
				TransformAccess.GetLocalPosition(ref this, out result);
				return result;
			}
			set
			{
				TransformAccess.SetLocalPosition(ref this, ref value);
			}
		}

		public Quaternion localRotation
		{
			get
			{
				Quaternion result;
				TransformAccess.GetLocalRotation(ref this, out result);
				return result;
			}
			set
			{
				TransformAccess.SetLocalRotation(ref this, ref value);
			}
		}

		public Vector3 localScale
		{
			get
			{
				Vector3 result;
				TransformAccess.GetLocalScale(ref this, out result);
				return result;
			}
			set
			{
				TransformAccess.SetLocalScale(ref this, ref value);
			}
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetPosition(ref TransformAccess access, out Vector3 p);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetPosition(ref TransformAccess access, ref Vector3 p);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetRotation(ref TransformAccess access, out Quaternion r);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetRotation(ref TransformAccess access, ref Quaternion r);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetLocalPosition(ref TransformAccess access, out Vector3 p);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetLocalPosition(ref TransformAccess access, ref Vector3 p);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetLocalRotation(ref TransformAccess access, out Quaternion r);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetLocalRotation(ref TransformAccess access, ref Quaternion r);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetLocalScale(ref TransformAccess access, out Vector3 r);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetLocalScale(ref TransformAccess access, ref Vector3 r);
	}
}
