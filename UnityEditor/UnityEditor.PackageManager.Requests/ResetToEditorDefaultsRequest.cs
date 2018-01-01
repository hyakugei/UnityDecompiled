using System;

namespace UnityEditor.PackageManager.Requests
{
	[Serializable]
	public sealed class ResetToEditorDefaultsRequest : Request
	{
		private ResetToEditorDefaultsRequest()
		{
		}

		internal ResetToEditorDefaultsRequest(long operationId, NativeStatusCode initialStatus) : base(operationId, initialStatus)
		{
		}
	}
}
