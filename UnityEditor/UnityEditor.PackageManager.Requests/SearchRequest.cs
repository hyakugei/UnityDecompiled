using System;
using UnityEngine;

namespace UnityEditor.PackageManager.Requests
{
	[Serializable]
	public sealed class SearchRequest : Request<UpmPackageInfo[]>
	{
		[SerializeField]
		private string m_PackageIdOrName;

		public string PackageIdOrName
		{
			get
			{
				return this.m_PackageIdOrName;
			}
		}

		private SearchRequest()
		{
		}

		internal SearchRequest(long operationId, NativeClient.StatusCode initialStatus, string packageIdOrName) : base(operationId, initialStatus)
		{
			this.m_PackageIdOrName = packageIdOrName;
		}

		protected override UpmPackageInfo[] GetResult()
		{
			return NativeClient.GetSearchOperationData(base.Id);
		}
	}
}
