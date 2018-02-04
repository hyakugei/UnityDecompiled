using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace UnityEngine
{
	public class HumanPoseHandler : IDisposable
	{
		internal IntPtr m_Ptr;

		public HumanPoseHandler(Avatar avatar, Transform root)
		{
			this.m_Ptr = IntPtr.Zero;
			if (root == null)
			{
				throw new ArgumentNullException("HumanPoseHandler root Transform is null");
			}
			if (avatar == null)
			{
				throw new ArgumentNullException("HumanPoseHandler avatar is null");
			}
			if (!avatar.isValid)
			{
				throw new ArgumentException("HumanPoseHandler avatar is invalid");
			}
			if (!avatar.isHuman)
			{
				throw new ArgumentException("HumanPoseHandler avatar is not human");
			}
			this.m_Ptr = HumanPoseHandler.Internal_Create(avatar, root);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr Internal_Create(Avatar avatar, Transform root);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_Destroy(IntPtr ptr);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetHumanPose(out Vector3 bodyPosition, out Quaternion bodyRotation, [Out] float[] muscles);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetHumanPose(ref Vector3 bodyPosition, ref Quaternion bodyRotation, float[] muscles);

		public void Dispose()
		{
			if (this.m_Ptr != IntPtr.Zero)
			{
				HumanPoseHandler.Internal_Destroy(this.m_Ptr);
				this.m_Ptr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}

		public void GetHumanPose(ref HumanPose humanPose)
		{
			if (this.m_Ptr == IntPtr.Zero)
			{
				throw new NullReferenceException("HumanPoseHandler is not initialized properly");
			}
			humanPose.Init();
			this.GetHumanPose(out humanPose.bodyPosition, out humanPose.bodyRotation, humanPose.muscles);
		}

		public void SetHumanPose(ref HumanPose humanPose)
		{
			if (this.m_Ptr == IntPtr.Zero)
			{
				throw new NullReferenceException("HumanPoseHandler is not initialized properly");
			}
			humanPose.Init();
			this.SetHumanPose(ref humanPose.bodyPosition, ref humanPose.bodyRotation, humanPose.muscles);
		}
	}
}
