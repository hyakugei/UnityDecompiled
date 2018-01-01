using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Scripting;

namespace UnityEditor.PackageManager
{
	[RequiredByNativeCode]
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class UpmPackageInfo
	{
		[SerializeField]
		private string m_PackageId;

		[SerializeField]
		private string m_Tag;

		[SerializeField]
		private string m_Version;

		[SerializeField]
		private OriginType m_OriginType;

		[SerializeField]
		private string m_OriginLocation;

		[SerializeField]
		private RelationType m_RelationType;

		[SerializeField]
		private string m_ResolvedPath;

		[SerializeField]
		private string m_Name;

		[SerializeField]
		private string m_DisplayName;

		[SerializeField]
		private string m_Category;

		[SerializeField]
		private string m_Description;

		public string packageId
		{
			get
			{
				return this.m_PackageId;
			}
		}

		public string tag
		{
			get
			{
				return this.m_Tag;
			}
		}

		public string version
		{
			get
			{
				return this.m_Version;
			}
		}

		public OriginType originType
		{
			get
			{
				return this.m_OriginType;
			}
		}

		public string originLocation
		{
			get
			{
				return this.m_OriginLocation;
			}
		}

		public RelationType relationType
		{
			get
			{
				return this.m_RelationType;
			}
		}

		public string resolvedPath
		{
			get
			{
				return this.m_ResolvedPath;
			}
		}

		public string name
		{
			get
			{
				return this.m_Name;
			}
		}

		public string displayName
		{
			get
			{
				return this.m_DisplayName;
			}
		}

		public string category
		{
			get
			{
				return this.m_Category;
			}
		}

		public string description
		{
			get
			{
				return this.m_Description;
			}
		}

		private UpmPackageInfo()
		{
		}

		internal UpmPackageInfo(string packageId, string displayName = "", string category = "", string description = "", string resolvedPath = "", string tag = "")
		{
			this.m_OriginType = OriginType.Unknown;
			this.m_RelationType = RelationType.Unknown;
			this.m_Tag = tag;
			this.m_OriginLocation = "not implemented";
			this.m_PackageId = packageId;
			this.m_DisplayName = displayName;
			this.m_Category = category;
			this.m_Description = description;
			this.m_ResolvedPath = resolvedPath;
			string[] array = packageId.Split(new char[]
			{
				'@'
			});
			this.m_Name = array[0];
			this.m_Version = array[1];
		}
	}
}
