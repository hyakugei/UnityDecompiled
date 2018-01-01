using System;

namespace UnityEditor.PackageManager.Requests
{
	[Serializable]
	public sealed class AddRequest : Request<UnityEditor.PackageManager.PackageInfo>
	{
		private AddRequest()
		{
		}

		internal AddRequest(long operationId, NativeStatusCode initialStatus) : base(operationId, initialStatus)
		{
		}

		protected override UnityEditor.PackageManager.PackageInfo GetResult()
		{
			return NativeClient.GetAddOperationData(base.Id);
		}
	}
}
