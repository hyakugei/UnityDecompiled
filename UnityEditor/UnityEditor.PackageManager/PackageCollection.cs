using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityEditor.PackageManager
{
	[Serializable]
	public class PackageCollection : IEnumerable<PackageInfo>, IEnumerable
	{
		[SerializeField]
		private PackageInfo[] m_PackageList;

		[SerializeField]
		private Error m_Error;

		[SerializeField]
		private bool m_HasError;

		public Error error
		{
			get
			{
				return (!this.m_HasError) ? null : this.m_Error;
			}
		}

		private PackageCollection()
		{
		}

		internal PackageCollection(IEnumerable<PackageInfo> packages, Error error)
		{
			this.m_PackageList = (packages ?? new PackageInfo[0]).ToArray<PackageInfo>();
			this.m_Error = error;
			this.m_HasError = (this.m_Error != null);
		}

		IEnumerator<PackageInfo> IEnumerable<PackageInfo>.GetEnumerator()
		{
			return ((IEnumerable<PackageInfo>)this.m_PackageList).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.m_PackageList.GetEnumerator();
		}
	}
}
