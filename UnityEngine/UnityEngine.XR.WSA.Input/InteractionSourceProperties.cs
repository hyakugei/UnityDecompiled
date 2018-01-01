using System;
using UnityEngine.Scripting;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.XR.WSA.Input
{
	[MovedFrom("UnityEngine.VR.WSA.Input"), RequiredByNativeCode]
	public struct InteractionSourceProperties
	{
		internal double m_SourceLossRisk;

		internal Vector3 m_SourceLossMitigationDirection;

		internal InteractionSourcePose m_SourcePose;

		public double sourceLossRisk
		{
			get
			{
				return this.m_SourceLossRisk;
			}
		}

		public Vector3 sourceLossMitigationDirection
		{
			get
			{
				return this.m_SourceLossMitigationDirection;
			}
		}

		[Obsolete("InteractionSourceProperties.location is deprecated, and will be removed in a future release. Use InteractionSourceState.sourcePose instead.", true)]
		public InteractionSourceLocation location
		{
			get
			{
				return default(InteractionSourceLocation);
			}
		}

		[Obsolete("InteractionSourceProperties.sourcePose is deprecated, and will be removed in a future release. Use InteractionSourceState.sourcePose instead.", false)]
		public InteractionSourcePose sourcePose
		{
			get
			{
				return this.m_SourcePose;
			}
		}
	}
}
