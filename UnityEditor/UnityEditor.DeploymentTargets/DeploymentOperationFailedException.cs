using System;

namespace UnityEditor.DeploymentTargets
{
	internal class DeploymentOperationFailedException : Exception
	{
		public readonly string title;

		public DeploymentOperationFailedException(string title, string message, Exception inner = null) : base(message, inner)
		{
			this.title = title;
		}
	}
}
