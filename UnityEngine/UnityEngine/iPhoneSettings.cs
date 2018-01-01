using System;

namespace UnityEngine
{
	public class iPhoneSettings
	{
		[Obsolete("screenOrientation property is deprecated. Please use Screen.orientation instead (UnityUpgradable) -> Screen.orientation", true)]
		public static iPhoneScreenOrientation screenOrientation
		{
			get
			{
				return iPhoneScreenOrientation.Unknown;
			}
		}

		[Obsolete("uniqueIdentifier property is deprecated. Please use SystemInfo.deviceUniqueIdentifier instead (UnityUpgradable) -> SystemInfo.deviceUniqueIdentifier", true)]
		public static string uniqueIdentifier
		{
			get
			{
				return string.Empty;
			}
		}

		[Obsolete("name property is deprecated (UnityUpgradable). Please use SystemInfo.deviceName instead (UnityUpgradable) -> SystemInfo.deviceName", true)]
		public static string name
		{
			get
			{
				return string.Empty;
			}
		}

		[Obsolete("model property is deprecated. Please use SystemInfo.deviceModel instead (UnityUpgradable) -> SystemInfo.deviceModel", true)]
		public static string model
		{
			get
			{
				return string.Empty;
			}
		}

		[Obsolete("systemName property is deprecated. Please use SystemInfo.operatingSystem instead (UnityUpgradable) -> SystemInfo.operatingSystem", true)]
		public static string systemName
		{
			get
			{
				return string.Empty;
			}
		}

		[Obsolete("internetReachability property is deprecated. Please use Application.internetReachability instead (UnityUpgradable) -> Application.internetReachability", true)]
		public static iPhoneNetworkReachability internetReachability
		{
			get
			{
				return iPhoneNetworkReachability.NotReachable;
			}
		}

		[Obsolete("systemVersion property is deprecated. Please use iOS.Device.systemVersion instead (UnityUpgradable) -> UnityEngine.iOS.Device.systemVersion", true)]
		public static string systemVersion
		{
			get
			{
				return string.Empty;
			}
		}

		[Obsolete("generation property is deprecated. Please use iOS.Device.generation instead (UnityUpgradable) -> UnityEngine.iOS.Device.generation", true)]
		public static iPhoneGeneration generation
		{
			get
			{
				return iPhoneGeneration.Unknown;
			}
		}

		[Obsolete("verticalOrientation property is deprecated. Please use Screen.orientation == ScreenOrientation.Portrait instead.", false)]
		public static bool verticalOrientation
		{
			get
			{
				return false;
			}
		}

		[Obsolete("screenCanDarken property is deprecated. Please use (Screen.sleepTimeout != SleepTimeout.NeverSleep) instead.", false)]
		public static bool screenCanDarken
		{
			get
			{
				return false;
			}
		}

		[Obsolete("locationServiceEnabledByUser property is deprecated. Please use Input.location.isEnabledByUser instead.", false)]
		public static bool locationServiceEnabledByUser
		{
			get
			{
				return Input.location.isEnabledByUser;
			}
		}

		[Obsolete("locationServiceStatus property is deprecated. Please use Input.location.status instead.", false)]
		public static LocationServiceStatus locationServiceStatus
		{
			get
			{
				return Input.location.status;
			}
		}

		[Obsolete("StartLocationServiceUpdates method is deprecated. Please use Input.location.Start instead.", false)]
		public static void StartLocationServiceUpdates(float desiredAccuracyInMeters, float updateDistanceInMeters)
		{
			Input.location.Start(desiredAccuracyInMeters, updateDistanceInMeters);
		}

		[Obsolete("StartLocationServiceUpdates method is deprecated. Please use Input.location.Start instead.", false)]
		public static void StartLocationServiceUpdates(float desiredAccuracyInMeters)
		{
			Input.location.Start(desiredAccuracyInMeters);
		}

		[Obsolete("StartLocationServiceUpdates method is deprecated. Please use Input.location.Start instead.", false)]
		public static void StartLocationServiceUpdates()
		{
			Input.location.Start();
		}

		[Obsolete("StopLocationServiceUpdates method is deprecated. Please use Input.location.Stop instead.", false)]
		public static void StopLocationServiceUpdates()
		{
			Input.location.Stop();
		}
	}
}
