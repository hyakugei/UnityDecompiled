using System;
using UnityEngine.Scripting;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.XR.WSA.Input
{
	[MovedFrom("UnityEngine.VR.WSA.Input"), RequiredByNativeCode]
	public struct NavigationUpdatedEventArgs
	{
		internal InteractionSource m_Source;

		internal InteractionSourcePose m_SourcePose;

		internal Pose m_HeadPose;

		internal Vector3 m_NormalizedOffset;

		public InteractionSource source
		{
			get
			{
				return this.m_Source;
			}
		}

		public InteractionSourcePose sourcePose
		{
			get
			{
				return this.m_SourcePose;
			}
		}

		public Pose headPose
		{
			get
			{
				return this.m_HeadPose;
			}
		}

		public Vector3 normalizedOffset
		{
			get
			{
				return this.m_NormalizedOffset;
			}
		}
	}
}
