using System;

namespace UnityEditor.DeploymentTargets
{
	internal class CorruptBuildException : DeploymentOperationFailedException
	{
		public CorruptBuildException(string message = "Corrupt build.", Exception inner = null) : base("Corrupt build", message, inner)
		{
		}
	}
}
