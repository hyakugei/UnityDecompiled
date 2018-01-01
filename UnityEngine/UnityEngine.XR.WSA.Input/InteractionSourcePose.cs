using System;
using UnityEngine.Scripting;

namespace UnityEngine.XR.WSA.Input
{
	[RequiredByNativeCode]
	public struct InteractionSourcePose
	{
		internal Quaternion m_GripRotation;

		internal Quaternion m_PointerRotation;

		internal Vector3 m_GripPosition;

		internal Vector3 m_PointerPosition;

		internal Vector3 m_Velocity;

		internal Vector3 m_AngularVelocity;

		internal InteractionSourcePositionAccuracy m_PositionAccuracy;

		internal InteractionSourcePoseFlags m_Flags;

		public InteractionSourcePositionAccuracy positionAccuracy
		{
			get
			{
				return this.m_PositionAccuracy;
			}
		}

		public bool TryGetPosition(out Vector3 position)
		{
			return this.TryGetPosition(out position, InteractionSourceNode.Grip);
		}

		public bool TryGetPosition(out Vector3 position, InteractionSourceNode node)
		{
			bool result;
			if (node == InteractionSourceNode.Grip)
			{
				position = this.m_GripPosition;
				result = ((this.m_Flags & InteractionSourcePoseFlags.HasGripPosition) != InteractionSourcePoseFlags.None);
			}
			else
			{
				position = this.m_PointerPosition;
				result = ((this.m_Flags & InteractionSourcePoseFlags.HasPointerPosition) != InteractionSourcePoseFlags.None);
			}
			return result;
		}

		public bool TryGetRotation(out Quaternion rotation, InteractionSourceNode node = InteractionSourceNode.Grip)
		{
			bool result;
			if (node == InteractionSourceNode.Grip)
			{
				rotation = this.m_GripRotation;
				result = ((this.m_Flags & InteractionSourcePoseFlags.HasGripRotation) != InteractionSourcePoseFlags.None);
			}
			else
			{
				rotation = this.m_PointerRotation;
				result = ((this.m_Flags & InteractionSourcePoseFlags.HasPointerRotation) != InteractionSourcePoseFlags.None);
			}
			return result;
		}

		public bool TryGetForward(out Vector3 forward, InteractionSourceNode node = InteractionSourceNode.Grip)
		{
			Quaternion rotation;
			bool result = this.TryGetRotation(out rotation, node);
			forward = rotation * Vector3.forward;
			return result;
		}

		public bool TryGetRight(out Vector3 right, InteractionSourceNode node = InteractionSourceNode.Grip)
		{
			Quaternion rotation;
			bool result = this.TryGetRotation(out rotation, node);
			right = rotation * Vector3.right;
			return result;
		}

		public bool TryGetUp(out Vector3 up, InteractionSourceNode node = InteractionSourceNode.Grip)
		{
			Quaternion rotation;
			bool result = this.TryGetRotation(out rotation, node);
			up = rotation * Vector3.up;
			return result;
		}

		public bool TryGetVelocity(out Vector3 velocity)
		{
			velocity = this.m_Velocity;
			return (this.m_Flags & InteractionSourcePoseFlags.HasVelocity) != InteractionSourcePoseFlags.None;
		}

		public bool TryGetAngularVelocity(out Vector3 angularVelocity)
		{
			angularVelocity = this.m_AngularVelocity;
			return (this.m_Flags & InteractionSourcePoseFlags.HasAngularVelocity) != InteractionSourcePoseFlags.None;
		}
	}
}
