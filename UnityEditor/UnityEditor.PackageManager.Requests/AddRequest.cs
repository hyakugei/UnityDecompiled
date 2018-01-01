using System;

namespace UnityEditor.PackageManager.Requests
{
	[Serializable]
	public sealed class AddRequest : Request<UpmPackageInfo>
	{
		private AddRequest()
		{
		}

		internal AddRequest(long operationId, NativeClient.StatusCode initialStatus) : base(operationId, initialStatus)
		{
		}

		protected override UpmPackageInfo GetResult()
		{
			return NativeClient.GetAddOperationData(base.Id);
		}
	}
}
