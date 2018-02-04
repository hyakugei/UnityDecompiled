using System;
using UnityEngine.Scripting;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.XR.WSA.Input
{
	[MovedFrom("UnityEngine.VR.WSA.Input"), RequiredByNativeCode]
	public struct InteractionSourceState
	{
		internal InteractionSourceProperties m_Properties;

		internal InteractionSource m_Source;

		internal Pose m_HeadPose;

		internal Vector2 m_ThumbstickPosition;

		internal Vector2 m_TouchpadPosition;

		internal float m_SelectPressedAmount;

		internal InteractionSourceStateFlags m_Flags;

		public bool anyPressed
		{
			get
			{
				return (this.m_Flags & InteractionSourceStateFlags.AnyPressed) != InteractionSourceStateFlags.None;
			}
		}

		public Pose headPose
		{
			get
			{
				return this.m_HeadPose;
			}
		}

		public InteractionSourceProperties properties
		{
			get
			{
				return this.m_Properties;
			}
		}

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
				return this.m_Properties.m_SourcePose;
			}
		}

		public float selectPressedAmount
		{
			get
			{
				return this.m_SelectPressedAmount;
			}
		}

		public bool selectPressed
		{
			get
			{
				return (this.m_Flags & InteractionSourceStateFlags.SelectPressed) != InteractionSourceStateFlags.None;
			}
		}

		public bool menuPressed
		{
			get
			{
				return (this.m_Flags & InteractionSourceStateFlags.MenuPressed) != InteractionSourceStateFlags.None;
			}
		}

		public bool grasped
		{
			get
			{
				return (this.m_Flags & InteractionSourceStateFlags.Grasped) != InteractionSourceStateFlags.None;
			}
		}

		public bool touchpadTouched
		{
			get
			{
				return (this.m_Flags & InteractionSourceStateFlags.TouchpadTouched) != InteractionSourceStateFlags.None;
			}
		}

		public bool touchpadPressed
		{
			get
			{
				return (this.m_Flags & InteractionSourceStateFlags.TouchpadPressed) != InteractionSourceStateFlags.None;
			}
		}

		public Vector2 touchpadPosition
		{
			get
			{
				return this.m_TouchpadPosition;
			}
		}

		public Vector2 thumbstickPosition
		{
			get
			{
				return this.m_ThumbstickPosition;
			}
		}

		public bool thumbstickPressed
		{
			get
			{
				return (this.m_Flags & InteractionSourceStateFlags.ThumbstickPressed) != InteractionSourceStateFlags.None;
			}
		}

		[Obsolete("InteractionSourceState.pressed is deprecated, and will be removed in a future release. Use InteractionSourceState.selectPressed instead. (UnityUpgradable) -> selectPressed", false)]
		public bool pressed
		{
			get
			{
				return this.selectPressed;
			}
		}

		[Obsolete("InteractionSourceState.headRay is obsolete - update your scripts to use InteractionSourceLocation.headPose instead.", false)]
		public Ray headRay
		{
			get
			{
				return new Ray(this.m_HeadPose.position, this.m_HeadPose.rotation * Vector3.forward);
			}
		}
	}
}
