using System;

namespace UnityEditor.DeploymentTargets
{
	internal struct DeploymentTargetId
	{
		internal static readonly DeploymentTargetId kDefault = new DeploymentTargetId("__builtin__target_default");

		internal static readonly DeploymentTargetId kAll = new DeploymentTargetId("__builtin__target_all");

		public string id;

		public DeploymentTargetId(string id)
		{
			if (string.IsNullOrEmpty(id))
			{
				id = DeploymentTargetId.kDefault.id;
			}
			this.id = id;
		}

		public static implicit operator DeploymentTargetId(string id)
		{
			return new DeploymentTargetId(id);
		}

		public static implicit operator string(DeploymentTargetId id)
		{
			return id.id;
		}

		public bool IsSpecificTarget()
		{
			return this.id != DeploymentTargetId.kDefault.id && this.id != DeploymentTargetId.kAll.id;
		}
	}
}
