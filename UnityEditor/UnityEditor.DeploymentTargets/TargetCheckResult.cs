using System;

namespace UnityEditor.DeploymentTargets
{
	internal struct TargetCheckResult
	{
		public CategoryCheckResult hardware;

		public CategoryCheckResult sdk;

		public bool Passed()
		{
			return this.hardware.status == CheckStatus.Ok && this.sdk.status == CheckStatus.Ok;
		}
	}
}
