using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEditor.PackageManager
{
	[NativeType(IntermediateScriptingStructName = "PackageManager_PackageInfo"), RequiredByNativeCode]
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class PackageInfo
	{
		[NativeName("packageId"), SerializeField]
		private string m_PackageId;

		[NativeName("version"), SerializeField]
		private string m_Version;

		[NativeName("originType"), SerializeField]
		private OriginType m_OriginType;

		[NativeName("resolvedPath"), SerializeField]
		private string m_ResolvedPath;

		[NativeName("name"), SerializeField]
		private string m_Name;

		[NativeName("displayName"), SerializeField]
		private string m_DisplayName;

		[NativeName("category"), SerializeField]
		private string m_Category;

		[NativeName("description"), SerializeField]
		private string m_Description;

		[NativeName("status"), SerializeField]
		private PackageStatus m_Status;

		[NativeName("errors"), SerializeField]
		private Error[] m_Errors;

		[NativeName("versions"), SerializeField]
		private VersionsInfo m_Versions;

		public string packageId
		{
			get
			{
				return this.m_PackageId;
			}
		}

		public string version
		{
			get
			{
				return this.m_Version;
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

		public PackageStatus status
		{
			get
			{
				return this.m_Status;
			}
		}

		public Error[] errors
		{
			get
			{
				return this.m_Errors;
			}
		}

		public VersionsInfo versions
		{
			get
			{
				return this.m_Versions;
			}
		}

		public OriginType originType
		{
			get
			{
				OriginType result;
				if (this.m_Name.StartsWith("com.unity.modules."))
				{
					result = OriginType.Builtin;
				}
				else
				{
					result = OriginType.Registry;
				}
				return result;
			}
		}

		private PackageInfo()
		{
		}

		internal PackageInfo(string packageId, string displayName = "", string category = "", string description = "", string resolvedPath = "", string tag = "", PackageStatus status = PackageStatus.Unavailable, IEnumerable<Error> errors = null, VersionsInfo versions = null)
		{
			this.m_OriginType = OriginType.Unknown;
			this.m_PackageId = packageId;
			this.m_DisplayName = displayName;
			this.m_Category = category;
			this.m_Description = description;
			this.m_ResolvedPath = resolvedPath;
			this.m_Status = status;
			this.m_Errors = (errors ?? new Error[0]).ToArray<Error>();
			this.m_Versions = (versions ?? new VersionsInfo(null, null, null));
			string[] array = packageId.Split(new char[]
			{
				'@'
			});
			this.m_Name = array[0];
			this.m_Version = array[1];
		}
	}
}
