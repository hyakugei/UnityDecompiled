using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEditor.PackageManager
{
	[RequiredByNativeCode]
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class VersionsInfo
	{
		[NativeName("all"), SerializeField]
		private string[] m_All;

		[NativeName("compatible"), SerializeField]
		private string[] m_Compatible;

		[NativeName("recommended"), SerializeField]
		private string m_Recommended;

		public string[] all
		{
			get
			{
				return this.m_All;
			}
		}

		public string[] compatible
		{
			get
			{
				return this.m_Compatible;
			}
		}

		public string recommended
		{
			get
			{
				return this.m_Recommended;
			}
		}

		public string latest
		{
			get
			{
				return this.all.LastOrDefault<string>() ?? string.Empty;
			}
		}

		public string latestCompatible
		{
			get
			{
				return this.compatible.LastOrDefault<string>() ?? string.Empty;
			}
		}

		private VersionsInfo()
		{
		}

		internal VersionsInfo(IEnumerable<string> all, IEnumerable<string> compatible, string recommended)
		{
			this.m_All = (all ?? new string[0]).ToArray<string>();
			this.m_Compatible = (compatible ?? new string[0]).ToArray<string>();
			this.m_Recommended = (recommended ?? string.Empty);
		}
	}
}
