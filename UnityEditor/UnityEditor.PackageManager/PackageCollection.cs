using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityEditor.PackageManager
{
	[Serializable]
	public class PackageCollection : IEnumerable<UpmPackageInfo>, IEnumerable
	{
		[SerializeField]
		private UpmPackageInfo[] m_PackageList;

		private PackageCollection()
		{
		}

		internal PackageCollection(IEnumerable<UpmPackageInfo> packages)
		{
			this.m_PackageList = (packages ?? new UpmPackageInfo[0]).ToArray<UpmPackageInfo>();
		}

		IEnumerator<UpmPackageInfo> IEnumerable<UpmPackageInfo>.GetEnumerator()
		{
			return ((IEnumerable<UpmPackageInfo>)this.m_PackageList).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.m_PackageList.GetEnumerator();
		}
	}
}
