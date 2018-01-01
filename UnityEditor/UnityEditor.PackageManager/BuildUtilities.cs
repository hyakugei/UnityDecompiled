using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace UnityEditor.PackageManager
{
	public static class BuildUtilities
	{
		private static Dictionary<string, IShouldIncludeInBuildCallback> m_PackageNameToCallback = new Dictionary<string, IShouldIncludeInBuildCallback>();

		public static void RegisterShouldIncludeInBuildCallback(IShouldIncludeInBuildCallback cb)
		{
			if (string.IsNullOrEmpty(cb.PackageName))
			{
				throw new ArgumentException("PackageName is empty.", "cb");
			}
			if (BuildUtilities.m_PackageNameToCallback.ContainsKey(cb.PackageName))
			{
				throw new NotSupportedException("Only one callback per package is supported.");
			}
			BuildUtilities.m_PackageNameToCallback[cb.PackageName] = cb;
		}

		internal static int ShouldIncludeInBuild(string packagePath, string packageFullPath)
		{
			string value = Regex.Match(packagePath, "\\/(.*)\\/").Groups[1].Value;
			IShouldIncludeInBuildCallback shouldIncludeInBuildCallback;
			int result;
			if (value != string.Empty && BuildUtilities.m_PackageNameToCallback.TryGetValue(value, out shouldIncludeInBuildCallback))
			{
				result = ((!shouldIncludeInBuildCallback.ShouldIncludeInBuild(packageFullPath)) ? 0 : 1);
			}
			else
			{
				result = -1;
			}
			return result;
		}
	}
}
