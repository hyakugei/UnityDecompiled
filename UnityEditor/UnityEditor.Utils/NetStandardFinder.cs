using System;
using System.IO;

namespace UnityEditor.Utils
{
	internal class NetStandardFinder
	{
		public const string NetStandardInstallation = "NetStandard";

		public static string GetReferenceDirectory()
		{
			string netStandardInstallation = NetStandardFinder.GetNetStandardInstallation();
			return Path.Combine(netStandardInstallation, Path.Combine("ref", "2.0.0"));
		}

		public static string GetCompatShimsDirectory()
		{
			return Path.Combine("compat", Path.Combine("2.0.0", "shims"));
		}

		public static string GetNetStandardCompatShimsDirectory()
		{
			string netStandardInstallation = NetStandardFinder.GetNetStandardInstallation();
			return Path.Combine(netStandardInstallation, Path.Combine(NetStandardFinder.GetCompatShimsDirectory(), "netstandard"));
		}

		public static string GetDotNetFrameworkCompatShimsDirectory()
		{
			string netStandardInstallation = NetStandardFinder.GetNetStandardInstallation();
			return Path.Combine(netStandardInstallation, Path.Combine(NetStandardFinder.GetCompatShimsDirectory(), "netfx"));
		}

		public static string GetNetStandardInstallation()
		{
			return Path.Combine(MonoInstallationFinder.GetFrameWorksFolder(), "NetStandard");
		}
	}
}
