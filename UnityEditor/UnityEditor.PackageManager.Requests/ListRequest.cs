using System;

namespace UnityEditor.PackageManager.Requests
{
	[Serializable]
	public sealed class ListRequest : Request<PackageCollection>
	{
		private ListRequest()
		{
		}

		internal ListRequest(long operationId, NativeClient.StatusCode initialStatus) : base(operationId, initialStatus)
		{
		}

		protected override PackageCollection GetResult()
		{
			OperationStatus listOperationData = NativeClient.GetListOperationData(base.Id);
			return new PackageCollection(listOperationData.packageList);
		}
	}
}
