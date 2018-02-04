using System;
using UnityEditor.Advertisements;
using UnityEditor.Connect;
using UnityEngine;

namespace UnityEditor.Web
{
	[InitializeOnLoad]
	internal class AdsAccess : CloudServiceAccess
	{
		[Serializable]
		public struct AdsServiceState
		{
			public bool ads;
		}

		private const string kServiceName = "Unity Ads";

		private const string kServiceDisplayName = "Ads";

		private const string kServicePackageName = "com.unity.ads";

		private const string kServiceUrl = "https://public-cdn.cloud.unity3d.com/editor/production/cloud/ads";

		static AdsAccess()
		{
			UnityConnectServiceData cloudService = new UnityConnectServiceData("Unity Ads", "https://public-cdn.cloud.unity3d.com/editor/production/cloud/ads", new AdsAccess(), "unity/project/cloud/ads");
			UnityConnectServiceCollection.instance.AddService(cloudService);
		}

		public override string GetServiceName()
		{
			return "Unity Ads";
		}

		public override string GetServiceDisplayName()
		{
			return "Ads";
		}

		public override string GetPackageName()
		{
			return "com.unity.ads";
		}

		public override bool IsServiceEnabled()
		{
			return AdvertisementSettings.enabled;
		}

		public override void EnableService(bool enabled)
		{
			if (AdvertisementSettings.enabled != enabled)
			{
				AdvertisementSettings.SetEnabledServiceWindow(enabled);
				EditorAnalytics.SendEventServiceInfo(new AdsAccess.AdsServiceState
				{
					ads = enabled
				});
			}
		}

		public override void OnProjectUnbound()
		{
			AdvertisementSettings.SetEnabledServiceWindow(false);
			AdvertisementSettings.SetGameId(RuntimePlatform.IPhonePlayer, "");
			AdvertisementSettings.SetGameId(RuntimePlatform.Android, "");
			AdvertisementSettings.testMode = false;
		}

		public bool IsInitializedOnStartup()
		{
			return AdvertisementSettings.initializeOnStartup;
		}

		public void SetInitializedOnStartup(bool enabled)
		{
			AdvertisementSettings.initializeOnStartup = enabled;
		}

		public string GetIOSGameId()
		{
			return AdvertisementSettings.GetGameId(RuntimePlatform.IPhonePlayer);
		}

		public void SetIOSGameId(string value)
		{
			AdvertisementSettings.SetGameId(RuntimePlatform.IPhonePlayer, value);
		}

		public string GetAndroidGameId()
		{
			return AdvertisementSettings.GetGameId(RuntimePlatform.Android);
		}

		public void SetAndroidGameId(string value)
		{
			AdvertisementSettings.SetGameId(RuntimePlatform.Android, value);
		}

		public string GetGameId(string platformName)
		{
			return AdvertisementSettings.GetPlatformGameId(platformName);
		}

		public void SetGameId(string platformName, string value)
		{
			AdvertisementSettings.SetPlatformGameId(platformName, value);
		}

		public bool IsTestModeEnabled()
		{
			return AdvertisementSettings.testMode;
		}

		public void SetTestModeEnabled(bool enabled)
		{
			AdvertisementSettings.testMode = enabled;
		}
	}
}
