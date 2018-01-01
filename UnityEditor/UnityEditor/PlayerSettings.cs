using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using UnityEditor.Build;
using UnityEditor.Rendering;
using UnityEditor.XR.Daydream;
using UnityEditorInternal;
using UnityEditorInternal.VR;
using UnityEngine;
using UnityEngine.Bindings;
using UnityEngine.Internal;
using UnityEngine.Rendering;
using UnityEngine.Scripting;

namespace UnityEditor
{
	[NativeClass(null)]
	public sealed class PlayerSettings : UnityEngine.Object
	{
		public sealed class Android
		{
			public static extern bool disableDepthAndStencilBuffers
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			[Obsolete("This has been replaced by disableDepthAndStencilBuffers")]
			public static bool use24BitDepthBuffer
			{
				get
				{
					return !PlayerSettings.Android.disableDepthAndStencilBuffers;
				}
				set
				{
				}
			}

			public static extern int bundleVersionCode
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern AndroidSdkVersions minSdkVersion
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern AndroidSdkVersions targetSdkVersion
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern AndroidPreferredInstallLocation preferredInstallLocation
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern bool forceInternetPermission
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern bool forceSDCardPermission
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern bool androidTVCompatibility
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern bool androidIsGame
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			internal static extern bool androidTangoEnabled
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			internal static extern bool androidBannerEnabled
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			internal static extern AndroidGamepadSupportLevel androidGamepadSupportLevel
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			internal static extern bool createWallpaper
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern AndroidArchitecture targetArchitectures
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern AndroidSplashScreenScale splashScreenScale
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern string keystoreName
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern string keystorePass
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern string keyaliasName
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern string keyaliasPass
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern bool licenseVerification
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
			}

			public static extern bool useAPKExpansionFiles
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern AndroidShowActivityIndicatorOnLoading showActivityIndicatorOnLoading
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern AndroidBlitType blitType
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			internal static extern int supportedAspectRatioMode
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern float maxAspectRatio
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			internal static extern bool useLowAccuracyLocation
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
			}

			[Obsolete("Use targetArchitectures instead. (UnityUpgradable) -> targetArchitectures", false)]
			public static AndroidTargetDevice targetDevice
			{
				get
				{
					AndroidArchitecture targetArchitectures = PlayerSettings.Android.targetArchitectures;
					AndroidTargetDevice result;
					if (targetArchitectures != AndroidArchitecture.ARMv7)
					{
						if (targetArchitectures != AndroidArchitecture.X86)
						{
							result = AndroidTargetDevice.FAT;
						}
						else
						{
							result = AndroidTargetDevice.x86;
						}
					}
					else
					{
						result = AndroidTargetDevice.ARMv7;
					}
					return result;
				}
				set
				{
					if (value != AndroidTargetDevice.ARMv7)
					{
						if (value != AndroidTargetDevice.x86)
						{
							PlayerSettings.Android.targetArchitectures = (AndroidArchitecture.ARMv7 | AndroidArchitecture.X86);
						}
						else
						{
							PlayerSettings.Android.targetArchitectures = AndroidArchitecture.X86;
						}
					}
					else
					{
						PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARMv7;
					}
				}
			}

			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			internal static extern AndroidBanner[] GetAndroidBanners();

			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			internal static extern Texture2D GetAndroidBannerForHeight(int height);

			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			internal static extern void SetAndroidBanners(Texture2D[] banners);
		}

		public static class VRCardboard
		{
			public static extern int depthFormat
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}
		}

		public static class VRDaydream
		{
			public static extern Texture2D daydreamIcon
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern Texture2D daydreamIconBackground
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern int depthFormat
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern SupportedHeadTracking minimumSupportedHeadTracking
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern SupportedHeadTracking maximumSupportedHeadTracking
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern bool enableVideoSurface
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern bool enableVideoSurfaceProtectedMemory
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}
		}

		public sealed class Facebook
		{
			public static extern string sdkVersion
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}
		}

		public sealed class N3DS
		{
			public enum TargetPlatform
			{
				Nintendo3DS = 1,
				NewNintendo3DS
			}

			public enum Region
			{
				Japan = 1,
				America,
				Europe,
				China,
				Korea,
				Taiwan,
				All
			}

			public enum MediaSize
			{
				_128MB,
				_256MB,
				_512MB,
				_1GB,
				_2GB
			}

			public enum LogoStyle
			{
				Nintendo,
				Distributed,
				iQue,
				Licensed
			}

			public static extern bool disableDepthAndStencilBuffers
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern bool disableStereoscopicView
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern bool enableSharedListOpt
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern bool enableVSync
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern bool useExtSaveData
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern bool compressStaticMem
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern string extSaveDataNumber
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern int stackSize
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern PlayerSettings.N3DS.TargetPlatform targetPlatform
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern PlayerSettings.N3DS.Region region
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern PlayerSettings.N3DS.MediaSize mediaSize
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern PlayerSettings.N3DS.LogoStyle logoStyle
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern string title
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern string productCode
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern string applicationId
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}
		}

		public struct SplashScreenLogo
		{
			private const float k_MinLogoTime = 2f;

			private static Sprite s_UnityLogo;

			private Sprite m_Logo;

			private float m_Duration;

			public Sprite logo
			{
				get
				{
					return this.m_Logo;
				}
				set
				{
					this.m_Logo = value;
				}
			}

			public static Sprite unityLogo
			{
				get
				{
					return PlayerSettings.SplashScreenLogo.s_UnityLogo;
				}
			}

			public float duration
			{
				get
				{
					return Mathf.Max(this.m_Duration, 2f);
				}
				set
				{
					this.m_Duration = Mathf.Max(value, 2f);
				}
			}

			static SplashScreenLogo()
			{
				PlayerSettings.SplashScreenLogo.s_UnityLogo = Resources.GetBuiltinResource<Sprite>("UnitySplash-cube.png");
			}

			[ExcludeFromDocs]
			public static PlayerSettings.SplashScreenLogo Create(float duration)
			{
				Sprite logo = null;
				return PlayerSettings.SplashScreenLogo.Create(duration, logo);
			}

			[ExcludeFromDocs]
			public static PlayerSettings.SplashScreenLogo Create()
			{
				Sprite logo = null;
				float duration = 2f;
				return PlayerSettings.SplashScreenLogo.Create(duration, logo);
			}

			public static PlayerSettings.SplashScreenLogo Create([UnityEngine.Internal.DefaultValue("k_MinLogoTime")] float duration, [UnityEngine.Internal.DefaultValue("null")] Sprite logo)
			{
				return new PlayerSettings.SplashScreenLogo
				{
					m_Duration = duration,
					m_Logo = logo
				};
			}

			[ExcludeFromDocs]
			public static PlayerSettings.SplashScreenLogo CreateWithUnityLogo()
			{
				float duration = 2f;
				return PlayerSettings.SplashScreenLogo.CreateWithUnityLogo(duration);
			}

			public static PlayerSettings.SplashScreenLogo CreateWithUnityLogo([UnityEngine.Internal.DefaultValue("k_MinLogoTime")] float duration)
			{
				return new PlayerSettings.SplashScreenLogo
				{
					m_Duration = duration,
					m_Logo = PlayerSettings.SplashScreenLogo.s_UnityLogo
				};
			}
		}

		public sealed class SplashScreen
		{
			public enum AnimationMode
			{
				Static,
				Dolly,
				Custom
			}

			public enum DrawMode
			{
				UnityLogoBelow,
				AllSequential
			}

			public enum UnityLogoStyle
			{
				DarkOnLight,
				LightOnDark
			}

			public static extern PlayerSettings.SplashScreen.AnimationMode animationMode
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern float animationBackgroundZoom
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern float animationLogoZoom
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern Sprite background
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern Sprite backgroundPortrait
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static Color backgroundColor
			{
				get
				{
					Color result;
					PlayerSettings.SplashScreen.INTERNAL_get_backgroundColor(out result);
					return result;
				}
				set
				{
					PlayerSettings.SplashScreen.INTERNAL_set_backgroundColor(ref value);
				}
			}

			public static extern PlayerSettings.SplashScreen.DrawMode drawMode
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern PlayerSettings.SplashScreenLogo[] logos
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern float overlayOpacity
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern bool show
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern bool showUnityLogo
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern PlayerSettings.SplashScreen.UnityLogoStyle unityLogoStyle
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			private static extern void INTERNAL_get_backgroundColor(out Color value);

			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			private static extern void INTERNAL_set_backgroundColor(ref Color value);
		}

		public sealed class WebGL
		{
			public static extern int memorySize
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern WebGLExceptionSupport exceptionSupport
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern bool dataCaching
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern string emscriptenArgs
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern string modulesDirectory
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern string template
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern bool analyzeBuildSize
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern bool useEmbeddedResources
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			[Obsolete("useWasm Property deprecated. Use linkerTarget instead")]
			public static extern bool useWasm
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern WebGLLinkerTarget linkerTarget
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern WebGLCompressionFormat compressionFormat
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern bool nameFilesAsHashes
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern bool debugSymbols
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}
		}

		public sealed class Switch
		{
			public enum ScreenResolutionBehavior
			{
				Manual,
				OperationMode,
				PerformanceMode,
				Both
			}

			public enum Languages
			{
				AmericanEnglish,
				BritishEnglish,
				Japanese,
				French,
				German,
				LatinAmericanSpanish,
				Spanish,
				Italian,
				Dutch,
				CanadianFrench,
				Portuguese,
				Russian,
				SimplifiedChinese,
				TraditionalChinese,
				Korean
			}

			public enum StartupUserAccount
			{
				None,
				Required,
				RequiredWithNetworkServiceAccountAvailable
			}

			public enum TouchScreenUsage
			{
				Supported,
				Required,
				None
			}

			public enum LogoHandling
			{
				Auto,
				Manual
			}

			public enum LogoType
			{
				LicensedByNintendo,
				[Obsolete("This attribute is no longer available as of NintendoSDK 4.3.", true)]
				DistributedByNintendo,
				Nintendo
			}

			public enum ApplicationAttribute
			{
				None,
				Demo
			}

			public enum RatingCategories
			{
				CERO,
				GRACGCRB,
				GSRMR,
				ESRB,
				ClassInd,
				USK,
				PEGI,
				PEGIPortugal,
				PEGIBBFC,
				Russian,
				ACB,
				OFLC
			}

			private enum SupportedNpadStyleBits
			{
				FullKey = 1,
				Handheld,
				JoyDual = 4,
				JoyLeft = 8,
				JoyRight = 16
			}

			[Flags]
			public enum SupportedNpadStyle
			{
				FullKey = 2,
				Handheld = 4,
				JoyDual = 16,
				JoyLeft = 256,
				JoyRight = 65536
			}

			public static extern int socketMemoryPoolSize
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern int socketAllocatorPoolSize
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern int socketConcurrencyLimit
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern bool useSwitchCPUProfiler
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern PlayerSettings.Switch.ScreenResolutionBehavior screenResolutionBehavior
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern string applicationID
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern string nsoDependencies
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern string[] titleNames
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern string[] publisherNames
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern Texture2D[] icons
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern Texture2D[] smallIcons
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static string manualHTMLPath
			{
				get
				{
					string manualHTMLPath = PlayerSettings.Switch.GetManualHTMLPath();
					string result;
					if (string.IsNullOrEmpty(manualHTMLPath))
					{
						result = "";
					}
					else
					{
						string text = manualHTMLPath;
						if (!Path.IsPathRooted(text))
						{
							text = Path.GetFullPath(text);
						}
						if (!Directory.Exists(text))
						{
							result = "";
						}
						else
						{
							result = text;
						}
					}
					return result;
				}
				set
				{
					PlayerSettings.Switch.SetManualHTMLPath((!string.IsNullOrEmpty(value)) ? value : "");
				}
			}

			public static string accessibleURLPath
			{
				get
				{
					string accessibleURLPath = PlayerSettings.Switch.GetAccessibleURLPath();
					string result;
					if (string.IsNullOrEmpty(accessibleURLPath))
					{
						result = "";
					}
					else
					{
						string text = accessibleURLPath;
						if (!Path.IsPathRooted(text))
						{
							text = Path.GetFullPath(text);
						}
						result = text;
					}
					return result;
				}
				set
				{
					PlayerSettings.Switch.SetAccessibleURLPath((!string.IsNullOrEmpty(value)) ? value : "");
				}
			}

			public static string legalInformationPath
			{
				get
				{
					string text = PlayerSettings.Switch.GetLegalInformationPath();
					string result;
					if (string.IsNullOrEmpty(text))
					{
						result = "";
					}
					else
					{
						if (!Path.IsPathRooted(text))
						{
							text = Path.GetFullPath(text);
						}
						result = text;
					}
					return result;
				}
				set
				{
					PlayerSettings.Switch.SetLegalInformationPath((!string.IsNullOrEmpty(value)) ? value : "");
				}
			}

			public static extern int mainThreadStackSize
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern string presenceGroupId
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern PlayerSettings.Switch.LogoHandling logoHandling
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern string releaseVersion
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern string displayVersion
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern PlayerSettings.Switch.StartupUserAccount startupUserAccount
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern PlayerSettings.Switch.TouchScreenUsage touchScreenUsage
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern int supportedLanguages
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern PlayerSettings.Switch.LogoType logoType
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern string applicationErrorCodeCategory
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern int userAccountSaveDataSize
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern int userAccountSaveDataJournalSize
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern PlayerSettings.Switch.ApplicationAttribute applicationAttribute
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern int cardSpecSize
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern int cardSpecClock
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern int ratingsMask
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
			}

			public static extern string[] localCommunicationIds
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern bool isUnderParentalControl
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
			}

			public static extern bool isScreenshotEnabled
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			[Obsolete("isAllowsScreenshot was renamed to isScreenshotEnabled.")]
			public static extern bool isAllowsScreenshot
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern bool isVideoCapturingEnabled
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern bool isRuntimeAddOnContentInstallEnabled
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern bool isDataLossConfirmationEnabled
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			[Obsolete("isDataLossConfirmation was renamed to isDataLossConfirmationEnabled.")]
			public static extern bool isDataLossConfirmation
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern PlayerSettings.Switch.SupportedNpadStyle supportedNpadStyles
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern bool socketConfigEnabled
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern int tcpInitialSendBufferSize
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern int tcpInitialReceiveBufferSize
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern int tcpAutoSendBufferSizeMax
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern int tcpAutoReceiveBufferSizeMax
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern int udpSendBufferSize
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern int udpReceiveBufferSize
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern int socketBufferEfficiency
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern bool socketInitializeEnabled
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern bool networkInterfaceManagerInitializeEnabled
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern bool playerConnectionEnabled
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			private static extern string GetManualHTMLPath();

			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			private static extern void SetManualHTMLPath(string path);

			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			private static extern string GetAccessibleURLPath();

			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			private static extern void SetAccessibleURLPath(string path);

			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			private static extern string GetLegalInformationPath();

			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			private static extern void SetLegalInformationPath(string path);

			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			public static extern int GetRatingAge(PlayerSettings.Switch.RatingCategories category);
		}

		public enum WSAApplicationShowName
		{
			NotSet,
			AllLogos,
			NoLogos,
			StandardLogoOnly,
			WideLogoOnly
		}

		public enum WSADefaultTileSize
		{
			NotSet,
			Medium,
			Wide
		}

		public enum WSAApplicationForegroundText
		{
			Light = 1,
			Dark
		}

		public enum WSACompilationOverrides
		{
			None,
			UseNetCore,
			UseNetCorePartially
		}

		public enum WSACapability
		{
			EnterpriseAuthentication,
			InternetClient,
			InternetClientServer,
			MusicLibrary,
			PicturesLibrary,
			PrivateNetworkClientServer,
			RemovableStorage,
			SharedUserCertificates,
			VideosLibrary,
			WebCam,
			Proximity,
			Microphone,
			Location,
			HumanInterfaceDevice,
			AllJoyn,
			BlockedChatMessages,
			Chat,
			CodeGeneration,
			Objects3D,
			PhoneCall,
			UserAccountInformation,
			VoipCall,
			Bluetooth,
			SpatialPerception,
			InputInjectionBrokered
		}

		public enum WSAImageScale
		{
			_80 = 80,
			_100 = 100,
			_125 = 125,
			_140 = 140,
			_150 = 150,
			_180 = 180,
			_200 = 200,
			_240 = 240,
			_400 = 400,
			Target16 = 16,
			Target24 = 24,
			Target32 = 32,
			Target48 = 48,
			Target256 = 256
		}

		public enum WSAImageType
		{
			PackageLogo = 1,
			SplashScreenImage,
			UWPSquare44x44Logo = 31,
			UWPSquare71x71Logo,
			UWPSquare150x150Logo,
			UWPSquare310x310Logo,
			UWPWide310x150Logo
		}

		public enum WSAInputSource
		{
			CoreWindow,
			IndependentInputSource,
			SwapChainPanel
		}

		[RequiredByNativeCode]
		public struct WSASupportedFileType
		{
			public string contentType;

			public string fileType;
		}

		[RequiredByNativeCode]
		public struct WSAFileTypeAssociations
		{
			public string name;

			public PlayerSettings.WSASupportedFileType[] supportedFileTypes;
		}

		public sealed class WSA
		{
			public static class Declarations
			{
				public static string protocolName
				{
					get
					{
						return PlayerSettings.WSA.internalProtocolName;
					}
					set
					{
						PlayerSettings.WSA.internalProtocolName = value;
					}
				}

				public static PlayerSettings.WSAFileTypeAssociations fileTypeAssociations
				{
					get
					{
						return PlayerSettings.WSA.internalFileTypeAssociations;
					}
					set
					{
						PlayerSettings.WSA.internalFileTypeAssociations = value;
					}
				}
			}

			public static extern bool transparentSwapchain
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern string packageName
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern string packageLogo
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			private static extern string packageVersionRaw
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern string commandLineArgsFile
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern string certificatePath
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
			}

			internal static extern string certificatePassword
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
			}

			public static extern string certificateSubject
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
			}

			public static extern string certificateIssuer
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
			}

			private static extern long certificateNotAfterRaw
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
			}

			public static extern string applicationDescription
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern string tileShortName
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern PlayerSettings.WSAApplicationShowName tileShowName
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern bool mediumTileShowName
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern bool largeTileShowName
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern bool wideTileShowName
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern PlayerSettings.WSADefaultTileSize defaultTileSize
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern PlayerSettings.WSACompilationOverrides compilationOverrides
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern PlayerSettings.WSAApplicationForegroundText tileForegroundText
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static Color tileBackgroundColor
			{
				get
				{
					Color result;
					PlayerSettings.WSA.INTERNAL_get_tileBackgroundColor(out result);
					return result;
				}
				set
				{
					PlayerSettings.WSA.INTERNAL_set_tileBackgroundColor(ref value);
				}
			}

			[Obsolete("PlayerSettings.WSA.enableIndependentInputSource is deprecated. Use PlayerSettings.WSA.inputSource.", false)]
			public static bool enableIndependentInputSource
			{
				get
				{
					return PlayerSettings.WSA.inputSource == PlayerSettings.WSAInputSource.IndependentInputSource;
				}
				set
				{
					PlayerSettings.WSA.inputSource = ((!value) ? PlayerSettings.WSAInputSource.CoreWindow : PlayerSettings.WSAInputSource.IndependentInputSource);
				}
			}

			public static extern PlayerSettings.WSAInputSource inputSource
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			private static extern bool splashScreenUseBackgroundColor
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			private static Color splashScreenBackgroundColorRaw
			{
				get
				{
					Color result;
					PlayerSettings.WSA.INTERNAL_get_splashScreenBackgroundColorRaw(out result);
					return result;
				}
				set
				{
					PlayerSettings.WSA.INTERNAL_set_splashScreenBackgroundColorRaw(ref value);
				}
			}

			internal static extern string internalProtocolName
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			internal static PlayerSettings.WSAFileTypeAssociations internalFileTypeAssociations
			{
				get
				{
					PlayerSettings.WSAFileTypeAssociations result;
					PlayerSettings.WSA.INTERNAL_get_internalFileTypeAssociations(out result);
					return result;
				}
				set
				{
					PlayerSettings.WSA.INTERNAL_set_internalFileTypeAssociations(ref value);
				}
			}

			public static Version packageVersion
			{
				get
				{
					Version result;
					try
					{
						result = new Version(PlayerSettings.WSA.ValidatePackageVersion(PlayerSettings.WSA.packageVersionRaw));
					}
					catch (Exception ex)
					{
						throw new Exception(string.Format("{0}, the raw string was {1}", ex.Message, PlayerSettings.WSA.packageVersionRaw));
					}
					return result;
				}
				set
				{
					PlayerSettings.WSA.packageVersionRaw = value.ToString();
				}
			}

			public static DateTime? certificateNotAfter
			{
				get
				{
					long certificateNotAfterRaw = PlayerSettings.WSA.certificateNotAfterRaw;
					DateTime? result;
					if (certificateNotAfterRaw != 0L)
					{
						result = new DateTime?(DateTime.FromFileTime(certificateNotAfterRaw));
					}
					else
					{
						result = null;
					}
					return result;
				}
			}

			public static Color? splashScreenBackgroundColor
			{
				get
				{
					Color? result;
					if (PlayerSettings.WSA.splashScreenUseBackgroundColor)
					{
						result = new Color?(PlayerSettings.WSA.splashScreenBackgroundColorRaw);
					}
					else
					{
						result = null;
					}
					return result;
				}
				set
				{
					PlayerSettings.WSA.splashScreenUseBackgroundColor = value.HasValue;
					if (value.HasValue)
					{
						PlayerSettings.WSA.splashScreenBackgroundColorRaw = value.Value;
					}
				}
			}

			[Obsolete("Use GetVisualAssetsImage()/SetVisualAssetsImage()", true)]
			public static string storeTileLogo80
			{
				get
				{
					throw new NotSupportedException("PlayerSettings.storeTileLogo80 is deprecated. Use GetVisualAssetsImage() instead.");
				}
				set
				{
					throw new NotSupportedException("PlayerSettings.storeTileLogo80 is deprecated. Use SetVisualAssetsImage() instead.");
				}
			}

			[Obsolete("Use GetVisualAssetsImage()/SetVisualAssetsImage()", true)]
			public static string storeTileLogo
			{
				get
				{
					throw new NotSupportedException("PlayerSettings.storeTileLogo is deprecated. Use GetVisualAssetsImage() instead.");
				}
				set
				{
					throw new NotSupportedException("PlayerSettings.storeTileLogo is deprecated. Use SetVisualAssetsImage() instead.");
				}
			}

			[Obsolete("Use GetVisualAssetsImage()/SetVisualAssetsImage()", true)]
			public static string storeTileLogo140
			{
				get
				{
					throw new NotSupportedException("PlayerSettings.storeTileLogo140 is deprecated. Use GetVisualAssetsImage() instead.");
				}
				set
				{
					throw new NotSupportedException("PlayerSettings.storeTileLogo140 is deprecated. Use SetVisualAssetsImage() instead.");
				}
			}

			[Obsolete("Use GetVisualAssetsImage()/SetVisualAssetsImage()", true)]
			public static string storeTileLogo180
			{
				get
				{
					throw new NotSupportedException("PlayerSettings.storeTileLogo180 is deprecated. Use GetVisualAssetsImage() instead.");
				}
				set
				{
					throw new NotSupportedException("PlayerSettings.storeTileLogo180 is deprecated. Use SetVisualAssetsImage() instead.");
				}
			}

			[Obsolete("Use GetVisualAssetsImage()/SetVisualAssetsImage()", true)]
			public static string storeTileWideLogo80
			{
				get
				{
					throw new NotSupportedException("PlayerSettings.storeTileWideLogo80 is deprecated. Use GetVisualAssetsImage() instead.");
				}
				set
				{
					throw new NotSupportedException("PlayerSettings.storeTileWideLogo80 is deprecated. Use SetVisualAssetsImage() instead.");
				}
			}

			[Obsolete("Use GetVisualAssetsImage()/SetVisualAssetsImage()", true)]
			public static string storeTileWideLogo
			{
				get
				{
					throw new NotSupportedException("PlayerSettings.storeTileWideLogo is deprecated. Use GetVisualAssetsImage() instead.");
				}
				set
				{
					throw new NotSupportedException("PlayerSettings.storeTileWideLogo is deprecated. Use SetVisualAssetsImage() instead.");
				}
			}

			[Obsolete("Use GetVisualAssetsImage()/SetVisualAssetsImage()", true)]
			public static string storeTileWideLogo140
			{
				get
				{
					throw new NotSupportedException("PlayerSettings.storeTileWideLogo140 is deprecated. Use GetVisualAssetsImage() instead.");
				}
				set
				{
					throw new NotSupportedException("PlayerSettings.storeTileWideLogo140 is deprecated. Use SetVisualAssetsImage() instead.");
				}
			}

			[Obsolete("Use GetVisualAssetsImage()/SetVisualAssetsImage()", true)]
			public static string storeTileWideLogo180
			{
				get
				{
					throw new NotSupportedException("PlayerSettings.storeTileWideLogo180 is deprecated. Use GetVisualAssetsImage() instead.");
				}
				set
				{
					throw new NotSupportedException("PlayerSettings.storeTileWideLogo180 is deprecated. Use SetVisualAssetsImage() instead.");
				}
			}

			[Obsolete("Use GetVisualAssetsImage()/SetVisualAssetsImage()", true)]
			public static string storeTileSmallLogo80
			{
				get
				{
					throw new NotSupportedException("PlayerSettings.storeTileSmallLogo80 is deprecated. Use GetVisualAssetsImage() instead.");
				}
				set
				{
					throw new NotSupportedException("PlayerSettings.storeTileSmallLogo80 is deprecated. Use SetVisualAssetsImage() instead.");
				}
			}

			[Obsolete("Use GetVisualAssetsImage()/SetVisualAssetsImage()", true)]
			public static string storeTileSmallLogo
			{
				get
				{
					throw new NotSupportedException("PlayerSettings.storeTileSmallLogo is deprecated. Use GetVisualAssetsImage() instead.");
				}
				set
				{
					throw new NotSupportedException("PlayerSettings.storeTileSmallLogo is deprecated. Use SetVisualAssetsImage() instead.");
				}
			}

			[Obsolete("Use GetVisualAssetsImage()/SetVisualAssetsImage()", true)]
			public static string storeTileSmallLogo140
			{
				get
				{
					throw new NotSupportedException("PlayerSettings.storeTileSmallLogo140 is deprecated. Use GetVisualAssetsImage() instead.");
				}
				set
				{
					throw new NotSupportedException("PlayerSettings.storeTileSmallLogo140 is deprecated. Use SetVisualAssetsImage() instead.");
				}
			}

			[Obsolete("Use GetVisualAssetsImage()/SetVisualAssetsImage()", true)]
			public static string storeTileSmallLogo180
			{
				get
				{
					throw new NotSupportedException("PlayerSettings.storeTileSmallLogo180 is deprecated. Use GetVisualAssetsImage() instead.");
				}
				set
				{
					throw new NotSupportedException("PlayerSettings.storeTileSmallLogo180 is deprecated. Use SetVisualAssetsImage() instead.");
				}
			}

			[Obsolete("Use GetVisualAssetsImage()/SetVisualAssetsImage()", true)]
			public static string storeSmallTile80
			{
				get
				{
					throw new NotSupportedException("PlayerSettings.storeSmallTile80 is deprecated. Use GetVisualAssetsImage() instead.");
				}
				set
				{
					throw new NotSupportedException("PlayerSettings.storeSmallTile80 is deprecated. Use SetVisualAssetsImage() instead.");
				}
			}

			[Obsolete("Use GetVisualAssetsImage()/SetVisualAssetsImage()", true)]
			public static string storeSmallTile
			{
				get
				{
					throw new NotSupportedException("PlayerSettings.storeSmallTile is deprecated. Use GetVisualAssetsImage() instead.");
				}
				set
				{
					throw new NotSupportedException("PlayerSettings.storeSmallTile is deprecated. Use SetVisualAssetsImage() instead.");
				}
			}

			[Obsolete("Use GetVisualAssetsImage()/SetVisualAssetsImage()", true)]
			public static string storeSmallTile140
			{
				get
				{
					throw new NotSupportedException("PlayerSettings.storeSmallTile140 is deprecated. Use GetVisualAssetsImage() instead.");
				}
				set
				{
					throw new NotSupportedException("PlayerSettings.storeSmallTile140 is deprecated. Use SetVisualAssetsImage() instead.");
				}
			}

			[Obsolete("Use GetVisualAssetsImage()/SetVisualAssetsImage()", true)]
			public static string storeSmallTile180
			{
				get
				{
					throw new NotSupportedException("PlayerSettings.storeSmallTile180 is deprecated. Use GetVisualAssetsImage() instead.");
				}
				set
				{
					throw new NotSupportedException("PlayerSettings.storeSmallTile180 is deprecated. Use SetVisualAssetsImage() instead.");
				}
			}

			[Obsolete("Use GetVisualAssetsImage()/SetVisualAssetsImage()", true)]
			public static string storeLargeTile80
			{
				get
				{
					throw new NotSupportedException("PlayerSettings.storeLargeTile80 is deprecated. Use GetVisualAssetsImage() instead.");
				}
				set
				{
					throw new NotSupportedException("PlayerSettings.storeLargeTile80 is deprecated. Use SetVisualAssetsImage() instead.");
				}
			}

			[Obsolete("Use GetVisualAssetsImage()/SetVisualAssetsImage()", true)]
			public static string storeLargeTile
			{
				get
				{
					throw new NotSupportedException("PlayerSettings.storeLargeTile is deprecated. Use GetVisualAssetsImage() instead.");
				}
				set
				{
					throw new NotSupportedException("PlayerSettings.storeLargeTile is deprecated. Use SetVisualAssetsImage() instead.");
				}
			}

			[Obsolete("Use GetVisualAssetsImage()/SetVisualAssetsImage()", true)]
			public static string storeLargeTile140
			{
				get
				{
					throw new NotSupportedException("PlayerSettings.storeLargeTile140 is deprecated. Use GetVisualAssetsImage() instead.");
				}
				set
				{
					throw new NotSupportedException("PlayerSettings.storeLargeTile140 is deprecated. Use SetVisualAssetsImage() instead.");
				}
			}

			[Obsolete("Use GetVisualAssetsImage()/SetVisualAssetsImage()", true)]
			public static string storeLargeTile180
			{
				get
				{
					throw new NotSupportedException("PlayerSettings.storeLargeTile180 is deprecated. Use GetVisualAssetsImage() instead.");
				}
				set
				{
					throw new NotSupportedException("PlayerSettings.storeLargeTile180 is deprecated. Use SetVisualAssetsImage() instead.");
				}
			}

			[Obsolete("Use GetVisualAssetsImage()/SetVisualAssetsImage()", true)]
			public static string storeSplashScreenImage
			{
				get
				{
					throw new NotSupportedException("PlayerSettings.storeSplashScreenImage is deprecated. Use GetVisualAssetsImage() instead.");
				}
				set
				{
					throw new NotSupportedException("PlayerSettings.storeSplashScreenImage is deprecated. Use SetVisualAssetsImage() instead.");
				}
			}

			[Obsolete("Use GetVisualAssetsImage()/SetVisualAssetsImage()", true)]
			public static string storeSplashScreenImageScale140
			{
				get
				{
					throw new NotSupportedException("PlayerSettings.storeSplashScreenImageScale140 is deprecated. Use GetVisualAssetsImage() instead.");
				}
				set
				{
					throw new NotSupportedException("PlayerSettings.storeSplashScreenImageScale140 is deprecated. Use SetVisualAssetsImage() instead.");
				}
			}

			[Obsolete("Use GetVisualAssetsImage()/SetVisualAssetsImage()", true)]
			public static string storeSplashScreenImageScale180
			{
				get
				{
					throw new NotSupportedException("PlayerSettings.storeSplashScreenImageScale180 is deprecated. Use GetVisualAssetsImage() instead.");
				}
				set
				{
					throw new NotSupportedException("PlayerSettings.storeSplashScreenImageScale180 is deprecated. Use SetVisualAssetsImage() instead.");
				}
			}

			[Obsolete("Use GetVisualAssetsImage()/SetVisualAssetsImage()", true)]
			public static string phoneAppIcon
			{
				get
				{
					throw new NotSupportedException("PlayerSettings.phoneAppIcon is deprecated. Use GetVisualAssetsImage() instead.");
				}
				set
				{
					throw new NotSupportedException("PlayerSettings.phoneAppIcon is deprecated. Use SetVisualAssetsImage() instead.");
				}
			}

			[Obsolete("Use GetVisualAssetsImage()/SetVisualAssetsImage()", true)]
			public static string phoneAppIcon140
			{
				get
				{
					throw new NotSupportedException("PlayerSettings.phoneAppIcon140 is deprecated. Use GetVisualAssetsImage() instead.");
				}
				set
				{
					throw new NotSupportedException("PlayerSettings.phoneAppIcon140 is deprecated. Use SetVisualAssetsImage() instead.");
				}
			}

			[Obsolete("Use GetVisualAssetsImage()/SetVisualAssetsImage()", true)]
			public static string phoneAppIcon240
			{
				get
				{
					throw new NotSupportedException("PlayerSettings.phoneAppIcon240 is deprecated. Use GetVisualAssetsImage() instead.");
				}
				set
				{
					throw new NotSupportedException("PlayerSettings.phoneAppIcon240 is deprecated. Use SetVisualAssetsImage() instead.");
				}
			}

			[Obsolete("Use GetVisualAssetsImage()/SetVisualAssetsImage()", true)]
			public static string phoneSmallTile
			{
				get
				{
					throw new NotSupportedException("PlayerSettings.phoneSmallTile is deprecated. Use GetVisualAssetsImage() instead.");
				}
				set
				{
					throw new NotSupportedException("PlayerSettings.phoneSmallTile is deprecated. Use SetVisualAssetsImage() instead.");
				}
			}

			[Obsolete("Use GetVisualAssetsImage()/SetVisualAssetsImage()", true)]
			public static string phoneSmallTile140
			{
				get
				{
					throw new NotSupportedException("PlayerSettings.phoneSmallTile140 is deprecated. Use GetVisualAssetsImage() instead.");
				}
				set
				{
					throw new NotSupportedException("PlayerSettings.phoneSmallTile140 is deprecated. Use SetVisualAssetsImage() instead.");
				}
			}

			[Obsolete("Use GetVisualAssetsImage()/SetVisualAssetsImage()", true)]
			public static string phoneSmallTile240
			{
				get
				{
					throw new NotSupportedException("PlayerSettings.phoneSmallTile240 is deprecated. Use GetVisualAssetsImage() instead.");
				}
				set
				{
					throw new NotSupportedException("PlayerSettings.phoneSmallTile240 is deprecated. Use SetVisualAssetsImage() instead.");
				}
			}

			[Obsolete("Use GetVisualAssetsImage()/SetVisualAssetsImage()", true)]
			public static string phoneMediumTile
			{
				get
				{
					throw new NotSupportedException("PlayerSettings.phoneMediumTile is deprecated. Use GetVisualAssetsImage() instead.");
				}
				set
				{
					throw new NotSupportedException("PlayerSettings.phoneMediumTile is deprecated. Use SetVisualAssetsImage() instead.");
				}
			}

			[Obsolete("Use GetVisualAssetsImage()/SetVisualAssetsImage()", true)]
			public static string phoneMediumTile140
			{
				get
				{
					throw new NotSupportedException("PlayerSettings.phoneMediumTile140 is deprecated. Use GetVisualAssetsImage() instead.");
				}
				set
				{
					throw new NotSupportedException("PlayerSettings.phoneMediumTile140 is deprecated. Use SetVisualAssetsImage() instead.");
				}
			}

			[Obsolete("Use GetVisualAssetsImage()/SetVisualAssetsImage()", true)]
			public static string phoneMediumTile240
			{
				get
				{
					throw new NotSupportedException("PlayerSettings.phoneMediumTile240 is deprecated. Use GetVisualAssetsImage() instead.");
				}
				set
				{
					throw new NotSupportedException("PlayerSettings.phoneMediumTile240 is deprecated. Use SetVisualAssetsImage() instead.");
				}
			}

			[Obsolete("Use GetVisualAssetsImage()/SetVisualAssetsImage()", true)]
			public static string phoneWideTile
			{
				get
				{
					throw new NotSupportedException("PlayerSettings.phoneWideTile is deprecated. Use GetVisualAssetsImage() instead.");
				}
				set
				{
					throw new NotSupportedException("PlayerSettings.phoneWideTile is deprecated. Use SetVisualAssetsImage() instead.");
				}
			}

			[Obsolete("Use GetVisualAssetsImage()/SetVisualAssetsImage()", true)]
			public static string phoneWideTile140
			{
				get
				{
					throw new NotSupportedException("PlayerSettings.phoneWideTile140 is deprecated. Use GetVisualAssetsImage() instead.");
				}
				set
				{
					throw new NotSupportedException("PlayerSettings.phoneWideTile140 is deprecated. Use SetVisualAssetsImage() instead.");
				}
			}

			[Obsolete("Use GetVisualAssetsImage()/SetVisualAssetsImage()", true)]
			public static string phoneWideTile240
			{
				get
				{
					throw new NotSupportedException("PlayerSettings.phoneWideTile240 is deprecated. Use GetVisualAssetsImage() instead.");
				}
				set
				{
					throw new NotSupportedException("PlayerSettings.phoneWideTile240 is deprecated. Use SetVisualAssetsImage() instead.");
				}
			}

			[Obsolete("Use GetVisualAssetsImage()/SetVisualAssetsImage()", true)]
			public static string phoneSplashScreenImage
			{
				get
				{
					throw new NotSupportedException("PlayerSettings.phoneSplashScreenImage is deprecated. Use GetVisualAssetsImage() instead.");
				}
				set
				{
					throw new NotSupportedException("PlayerSettings.phoneSplashScreenImage is deprecated. Use SetVisualAssetsImage() instead.");
				}
			}

			[Obsolete("Use GetVisualAssetsImage()/SetVisualAssetsImage()", true)]
			public static string phoneSplashScreenImageScale140
			{
				get
				{
					throw new NotSupportedException("PlayerSettings.phoneSplashScreenImageScale140 is deprecated. Use GetVisualAssetsImage() instead.");
				}
				set
				{
					throw new NotSupportedException("PlayerSettings.phoneSplashScreenImageScale140 is deprecated. Use SetVisualAssetsImage() instead.");
				}
			}

			[Obsolete("Use GetVisualAssetsImage()/SetVisualAssetsImage()", true)]
			public static string phoneSplashScreenImageScale240
			{
				get
				{
					throw new NotSupportedException("PlayerSettings.phoneSplashScreenImageScale240 is deprecated. Use GetVisualAssetsImage() instead.");
				}
				set
				{
					throw new NotSupportedException("PlayerSettings.phoneSplashScreenImageScale240 is deprecated. Use SetVisualAssetsImage() instead.");
				}
			}

			[Obsolete("Use GetVisualAssetsImage()/SetVisualAssetsImage()", true)]
			public static string packageLogo140
			{
				get
				{
					throw new NotSupportedException("PlayerSettings.packageLogo140 is deprecated. Use GetVisualAssetsImage() instead.");
				}
				set
				{
					throw new NotSupportedException("PlayerSettings.packageLogo140 is deprecated. Use SetVisualAssetsImage() instead.");
				}
			}

			[Obsolete("Use GetVisualAssetsImage()/SetVisualAssetsImage()", true)]
			public static string packageLogo180
			{
				get
				{
					throw new NotSupportedException("PlayerSettings.packageLogo180 is deprecated. Use GetVisualAssetsImage() instead.");
				}
				set
				{
					throw new NotSupportedException("PlayerSettings.packageLogo180 is deprecated. Use SetVisualAssetsImage() instead.");
				}
			}

			[Obsolete("Use GetVisualAssetsImage()/SetVisualAssetsImage()", true)]
			public static string packageLogo240
			{
				get
				{
					throw new NotSupportedException("PlayerSettings.packageLogo240 is deprecated. Use GetVisualAssetsImage() instead.");
				}
				set
				{
					throw new NotSupportedException("PlayerSettings.packageLogo240 is deprecated. Use SetVisualAssetsImage() instead.");
				}
			}

			[Obsolete("PlayerSettings.enableLowLatencyPresentationAPI is deprecated. It is now always enabled.", true)]
			public static bool enableLowLatencyPresentationAPI
			{
				get
				{
					return true;
				}
				set
				{
				}
			}

			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			private static extern string GetWSAImage(PlayerSettings.WSAImageType type, PlayerSettings.WSAImageScale scale);

			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			private static extern void SetWSAImage(string image, PlayerSettings.WSAImageType type, PlayerSettings.WSAImageScale scale);

			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			public static extern bool SetCertificate(string path, string password);

			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			private static extern void INTERNAL_get_tileBackgroundColor(out Color value);

			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			private static extern void INTERNAL_set_tileBackgroundColor(ref Color value);

			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			private static extern void INTERNAL_get_splashScreenBackgroundColorRaw(out Color value);

			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			private static extern void INTERNAL_set_splashScreenBackgroundColorRaw(ref Color value);

			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			private static extern void InternalSetCapability(string name, string value);

			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			private static extern string InternalGetCapability(string name);

			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			private static extern void INTERNAL_get_internalFileTypeAssociations(out PlayerSettings.WSAFileTypeAssociations value);

			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			private static extern void INTERNAL_set_internalFileTypeAssociations(ref PlayerSettings.WSAFileTypeAssociations value);

			internal static string ValidatePackageVersion(string value)
			{
				Regex regex = new Regex("^(\\d+)\\.(\\d+)\\.(\\d+)\\.(\\d+)$", RegexOptions.Compiled | RegexOptions.CultureInvariant);
				string result;
				if (regex.IsMatch(value))
				{
					result = value;
				}
				else
				{
					result = "1.0.0.0";
				}
				return result;
			}

			private static void ValidateWSAImageType(PlayerSettings.WSAImageType type)
			{
				switch (type)
				{
				case PlayerSettings.WSAImageType.UWPSquare44x44Logo:
				case PlayerSettings.WSAImageType.UWPSquare71x71Logo:
				case PlayerSettings.WSAImageType.UWPSquare150x150Logo:
				case PlayerSettings.WSAImageType.UWPSquare310x310Logo:
				case PlayerSettings.WSAImageType.UWPWide310x150Logo:
					break;
				default:
					if (type != PlayerSettings.WSAImageType.PackageLogo && type != PlayerSettings.WSAImageType.SplashScreenImage)
					{
						throw new Exception("Unknown WSA image type: " + type);
					}
					break;
				}
			}

			private static void ValidateWSAImageScale(PlayerSettings.WSAImageScale scale)
			{
				if (scale != PlayerSettings.WSAImageScale.Target16 && scale != PlayerSettings.WSAImageScale.Target24 && scale != PlayerSettings.WSAImageScale.Target32 && scale != PlayerSettings.WSAImageScale.Target48 && scale != PlayerSettings.WSAImageScale._80 && scale != PlayerSettings.WSAImageScale._100 && scale != PlayerSettings.WSAImageScale._125 && scale != PlayerSettings.WSAImageScale._140 && scale != PlayerSettings.WSAImageScale._150 && scale != PlayerSettings.WSAImageScale._180 && scale != PlayerSettings.WSAImageScale._200 && scale != PlayerSettings.WSAImageScale._240 && scale != PlayerSettings.WSAImageScale.Target256 && scale != PlayerSettings.WSAImageScale._400)
				{
					throw new Exception("Unknown image scale: " + scale);
				}
			}

			public static string GetVisualAssetsImage(PlayerSettings.WSAImageType type, PlayerSettings.WSAImageScale scale)
			{
				PlayerSettings.WSA.ValidateWSAImageType(type);
				PlayerSettings.WSA.ValidateWSAImageScale(scale);
				return PlayerSettings.WSA.GetWSAImage(type, scale);
			}

			public static void SetVisualAssetsImage(string image, PlayerSettings.WSAImageType type, PlayerSettings.WSAImageScale scale)
			{
				PlayerSettings.WSA.ValidateWSAImageType(type);
				PlayerSettings.WSA.ValidateWSAImageScale(scale);
				PlayerSettings.WSA.SetWSAImage(image, type, scale);
			}

			public static void SetCapability(PlayerSettings.WSACapability capability, bool value)
			{
				PlayerSettings.WSA.InternalSetCapability(capability.ToString(), value.ToString());
			}

			public static bool GetCapability(PlayerSettings.WSACapability capability)
			{
				string text = PlayerSettings.WSA.InternalGetCapability(capability.ToString());
				bool result;
				if (string.IsNullOrEmpty(text))
				{
					result = false;
				}
				else
				{
					try
					{
						result = (bool)TypeDescriptor.GetConverter(typeof(bool)).ConvertFromString(text);
					}
					catch
					{
						Debug.LogError(string.Concat(new string[]
						{
							"Failed to parse value  ('",
							capability.ToString(),
							",",
							text,
							"') to bool type."
						}));
						result = false;
					}
				}
				return result;
			}
		}

		public sealed class XboxOne
		{
			public static extern XboxOneLoggingLevel defaultLoggingLevel
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern string ProductId
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern string UpdateKey
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			[Obsolete("SandboxId is obsolete please remove", true)]
			public static extern string SandboxId
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern string ContentId
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern string TitleId
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern string SCID
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern bool EnableVariableGPU
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern uint PresentImmediateThreshold
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern bool Enable7thCore
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern bool DisableKinectGpuReservation
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern bool EnablePIXSampling
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern string GameOsOverridePath
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern string PackagingOverridePath
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern XboxOneEncryptionLevel PackagingEncryption
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern XboxOnePackageUpdateGranularity PackageUpdateGranularity
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern string AppManifestOverridePath
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern bool IsContentPackage
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern string Version
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern string Description
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern string[] SocketNames
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
			}

			public static extern string[] AllowedProductIds
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
			}

			public static extern uint PersistentLocalStorageSize
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern int monoLoggingLevel
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern ScriptCompiler scriptCompiler
			{
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[GeneratedByOldBindingsGenerator]
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			public static extern void SetCapability(string capability, bool value);

			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			public static extern bool GetCapability(string capability);

			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			public static extern void SetSupportedLanguage(string language, bool enabled);

			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			public static extern bool GetSupportedLanguage(string language);

			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			public static extern void RemoveSocketDefinition(string name);

			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			public static extern void SetSocketDefinition(string name, string port, int protocol, int[] usages, string templateName, int sessionRequirment, int[] deviceUsages);

			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			public static extern void GetSocketDefinition(string name, out string port, out int protocol, out int[] usages, out string templateName, out int sessionRequirment, out int[] deviceUsages);

			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			public static extern void RemoveAllowedProductId(string id);

			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			public static extern bool AddAllowedProductId(string id);

			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			public static extern void UpdateAllowedProductId(int idx, string id);

			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			public static extern void SetGameRating(string name, int value);

			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			public static extern int GetGameRating(string name);
		}

		public class iOS
		{
			[Obsolete("exitOnSuspend is deprecated, use appInBackgroundBehavior", false)]
			public static bool exitOnSuspend
			{
				get
				{
					return PlayerSettings.iOS.appInBackgroundBehavior == iOSAppInBackgroundBehavior.Exit;
				}
				set
				{
					PlayerSettings.iOS.appInBackgroundBehavior = iOSAppInBackgroundBehavior.Exit;
				}
			}

			[Obsolete("Use Screen.SetResolution at runtime", true)]
			public static iOSTargetResolution targetResolution
			{
				get
				{
					return iOSTargetResolution.Native;
				}
				set
				{
				}
			}

			[Obsolete("Use PlayerSettings.muteOtherAudioSources instead (UnityUpgradable) -> UnityEditor.PlayerSettings.muteOtherAudioSources", false)]
			public static bool overrideIPodMusic
			{
				get
				{
					return PlayerSettings.muteOtherAudioSources;
				}
				set
				{
					PlayerSettings.muteOtherAudioSources = value;
				}
			}

			public static extern string applicationDisplayName
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static string buildNumber
			{
				get
				{
					return PlayerSettings.GetBuildNumber(BuildTargetGroup.iPhone);
				}
				set
				{
					PlayerSettings.SetBuildNumber(BuildTargetGroup.iPhone, value);
				}
			}

			public static extern bool disableDepthAndStencilBuffers
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			private static extern int scriptCallOptimizationInternal
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static ScriptCallOptimizationLevel scriptCallOptimization
			{
				get
				{
					return (ScriptCallOptimizationLevel)PlayerSettings.iOS.scriptCallOptimizationInternal;
				}
				set
				{
					PlayerSettings.iOS.scriptCallOptimizationInternal = (int)value;
				}
			}

			private static extern int sdkVersionInternal
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static iOSSdkVersion sdkVersion
			{
				get
				{
					return (iOSSdkVersion)PlayerSettings.iOS.sdkVersionInternal;
				}
				set
				{
					PlayerSettings.iOS.sdkVersionInternal = (int)value;
				}
			}

			[Obsolete("OBSOLETE warning targetOSVersion is obsolete, use targetOSVersionString")]
			public static iOSTargetOSVersion targetOSVersion
			{
				get
				{
					return (iOSTargetOSVersion)PlayerSettings.iOS.iOSTargetOSVersionStringToObsoleteEnum(PlayerSettings.iOS.targetOSVersionString);
				}
				set
				{
					PlayerSettings.iOS.targetOSVersionString = PlayerSettings.iOS.iOSTargetOSVersionObsoleteEnumToString((int)value);
				}
			}

			public static extern string targetOSVersionString
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			private static extern int targetDeviceInternal
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static iOSTargetDevice targetDevice
			{
				get
				{
					return (iOSTargetDevice)PlayerSettings.iOS.targetDeviceInternal;
				}
				set
				{
					PlayerSettings.iOS.targetDeviceInternal = (int)value;
				}
			}

			public static extern bool prerenderedIcon
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern bool requiresPersistentWiFi
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern bool requiresFullScreen
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			private static extern int statusBarStyleInternal
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			[NativeProperty("UIStatusBarStyle")]
			public static iOSStatusBarStyle statusBarStyle
			{
				get
				{
					return (iOSStatusBarStyle)PlayerSettings.iOS.statusBarStyleInternal;
				}
				set
				{
					PlayerSettings.iOS.statusBarStyleInternal = (int)value;
				}
			}

			private static extern int deferSystemGesturesModeInternal
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static iOSSystemGestureDeferMode deferSystemGesturesMode
			{
				get
				{
					return (iOSSystemGestureDeferMode)PlayerSettings.iOS.deferSystemGesturesModeInternal;
				}
				set
				{
					PlayerSettings.iOS.deferSystemGesturesModeInternal = (int)value;
				}
			}

			[NativeProperty("HideHomeButton")]
			public static bool hideHomeButton
			{
				get;
				set;
			}

			private static extern int appInBackgroundBehaviorInternal
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			[NativeProperty("IOSAppInBackgroundBehavior")]
			public static iOSAppInBackgroundBehavior appInBackgroundBehavior
			{
				get
				{
					return (iOSAppInBackgroundBehavior)PlayerSettings.iOS.appInBackgroundBehaviorInternal;
				}
				set
				{
					PlayerSettings.iOS.appInBackgroundBehaviorInternal = (int)value;
				}
			}

			private static extern int backgroundModesInternal
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			[NativeProperty("IOSAppInBackgroundBehavior")]
			public static iOSBackgroundMode backgroundModes
			{
				get
				{
					return (iOSBackgroundMode)PlayerSettings.iOS.backgroundModesInternal;
				}
				set
				{
					PlayerSettings.iOS.backgroundModesInternal = (int)value;
				}
			}

			public static extern bool forceHardShadowsOnMetal
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern bool allowHTTPDownload
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern string appleDeveloperTeamID
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern string iOSManualProvisioningProfileID
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern string tvOSManualProvisioningProfileID
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			private static extern int appleEnableAutomaticSigningInternal
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static bool appleEnableAutomaticSigning
			{
				get
				{
					return PlayerSettings.iOS.appleEnableAutomaticSigningInternal == 1;
				}
				set
				{
					PlayerSettings.iOS.appleEnableAutomaticSigningInternal = ((!value) ? 2 : 1);
				}
			}

			public static extern string cameraUsageDescription
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern string locationUsageDescription
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern string microphoneUsageDescription
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			private static extern int showActivityIndicatorOnLoadingInternal
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			[NativeProperty("IOSAppInBackgroundBehavior")]
			public static iOSShowActivityIndicatorOnLoading showActivityIndicatorOnLoading
			{
				get
				{
					return (iOSShowActivityIndicatorOnLoading)PlayerSettings.iOS.showActivityIndicatorOnLoadingInternal;
				}
				set
				{
					PlayerSettings.iOS.showActivityIndicatorOnLoadingInternal = (int)value;
				}
			}

			public static extern bool useOnDemandResources
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			private static extern string[] iOSURLSchemes
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
			}

			internal static extern bool requiresARKitSupport
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			internal static extern bool appleEnableProMotion
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			[MethodImpl(MethodImplOptions.InternalCall)]
			private static extern string iOSTargetOSVersionObsoleteEnumToString(int val);

			[MethodImpl(MethodImplOptions.InternalCall)]
			private static extern int iOSTargetOSVersionStringToObsoleteEnum(string val);

			[MethodImpl(MethodImplOptions.InternalCall)]
			internal static extern string[] GetAssetBundleVariantsWithDeviceRequirements();

			[MethodImpl(MethodImplOptions.InternalCall)]
			private static extern int GetIOSDeviceRequirementCountForVariantName(string name);

			private static bool CheckAssetBundleVariantHasDeviceRequirements(string name)
			{
				return PlayerSettings.iOS.GetIOSDeviceRequirementCountForVariantName(name) > 0;
			}

			[MethodImpl(MethodImplOptions.InternalCall)]
			private static extern void SetLaunchScreenImageInternal(Texture2D image, int type);

			public static void SetLaunchScreenImage(Texture2D image, iOSLaunchScreenImageType type)
			{
				PlayerSettings.iOS.SetLaunchScreenImageInternal(image, (int)type);
			}

			[MethodImpl(MethodImplOptions.InternalCall)]
			private static extern void SetiOSLaunchScreenType(int type, int device);

			public static void SetiPhoneLaunchScreenType(iOSLaunchScreenType type)
			{
				PlayerSettings.iOS.SetiOSLaunchScreenType((int)type, 0);
			}

			public static void SetiPadLaunchScreenType(iOSLaunchScreenType type)
			{
				PlayerSettings.iOS.SetiOSLaunchScreenType((int)type, 1);
			}

			internal static iOSDeviceRequirementGroup GetDeviceRequirementsForAssetBundleVariant(string name)
			{
				iOSDeviceRequirementGroup result;
				if (!PlayerSettings.iOS.CheckAssetBundleVariantHasDeviceRequirements(name))
				{
					result = null;
				}
				else
				{
					result = new iOSDeviceRequirementGroup(name);
				}
				return result;
			}

			internal static void RemoveDeviceRequirementsForAssetBundleVariant(string name)
			{
				iOSDeviceRequirementGroup deviceRequirementsForAssetBundleVariant = PlayerSettings.iOS.GetDeviceRequirementsForAssetBundleVariant(name);
				for (int i = 0; i < deviceRequirementsForAssetBundleVariant.count; i++)
				{
					deviceRequirementsForAssetBundleVariant.RemoveAt(0);
				}
			}

			internal static iOSDeviceRequirementGroup AddDeviceRequirementsForAssetBundleVariant(string name)
			{
				return new iOSDeviceRequirementGroup(name);
			}

			internal static bool IsTargetVersionEqualOrHigher(Version requiredVersion)
			{
				Version version = new Version(8, 0);
				Version v = (!string.IsNullOrEmpty(PlayerSettings.iOS.targetOSVersionString)) ? new Version(PlayerSettings.iOS.targetOSVersionString) : version;
				return v >= requiredVersion;
			}

			internal static string[] GetURLSchemes()
			{
				return PlayerSettings.iOS.iOSURLSchemes;
			}
		}

		public class macOS
		{
			public static string buildNumber
			{
				get
				{
					return PlayerSettings.GetBuildNumber(BuildTargetGroup.Standalone);
				}
				set
				{
					PlayerSettings.SetBuildNumber(BuildTargetGroup.Standalone, value);
				}
			}

			internal static extern string applicationCategoryType
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}
		}

		public sealed class PS4
		{
			public enum PS4AppCategory
			{
				Application,
				Patch,
				Remaster
			}

			public enum PS4RemotePlayKeyAssignment
			{
				None = -1,
				PatternA,
				PatternB,
				PatternC,
				PatternD,
				PatternE,
				PatternF,
				PatternG,
				PatternH
			}

			public enum PS4EnterButtonAssignment
			{
				CircleButton,
				CrossButton
			}

			public enum PlayStationVREyeToEyeDistanceSettings
			{
				PerUser,
				ForceDefault,
				DynamicModeAtRuntime
			}

			public static extern string npTrophyPackPath
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern int npAgeRating
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern string npTitleSecret
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern int parentalLevel
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern int applicationParameter1
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern int applicationParameter2
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern int applicationParameter3
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern int applicationParameter4
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern string passcode
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern string monoEnv
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern bool playerPrefsSupport
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern bool restrictedAudioUsageRights
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern bool useResolutionFallback
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern string contentID
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern PlayerSettings.PS4.PS4AppCategory category
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern int appType
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern string masterVersion
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern string appVersion
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern PlayerSettings.PS4.PS4RemotePlayKeyAssignment remotePlayKeyAssignment
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern string remotePlayKeyMappingDir
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern int playTogetherPlayerCount
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern PlayerSettings.PS4.PS4EnterButtonAssignment enterButtonAssignment
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern string paramSfxPath
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern int videoOutPixelFormat
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern int videoOutInitialWidth
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			[Obsolete("videoOutResolution is deprecated. Use PlayerSettings.PS4.videoOutInitialWidth and PlayerSettings.PS4.videoOutReprojectionRate to control initial display resolution and reprojection rate.")]
			public static int videoOutResolution
			{
				get
				{
					int videoOutReprojectionRate = PlayerSettings.PS4.videoOutReprojectionRate;
					int videoOutInitialWidth = PlayerSettings.PS4.videoOutInitialWidth;
					int result;
					if (videoOutReprojectionRate != 60)
					{
						if (videoOutReprojectionRate != 90)
						{
							if (videoOutReprojectionRate != 120)
							{
								if (videoOutInitialWidth != 1280)
								{
									if (videoOutInitialWidth != 1440)
									{
										if (videoOutInitialWidth != 1600)
										{
											if (videoOutInitialWidth != 1760)
											{
												if (videoOutInitialWidth != 1920)
												{
												}
												result = 4;
											}
											else
											{
												result = 3;
											}
										}
										else
										{
											result = 2;
										}
									}
									else
									{
										result = 1;
									}
								}
								else
								{
									result = 0;
								}
							}
							else
							{
								result = 7;
							}
						}
						else
						{
							result = 6;
						}
					}
					else
					{
						result = 5;
					}
					return result;
				}
				set
				{
					int videoOutReprojectionRate = 0;
					int videoOutInitialWidth = 1920;
					switch (value)
					{
					case 0:
						videoOutInitialWidth = 1280;
						break;
					case 1:
						videoOutInitialWidth = 1440;
						break;
					case 2:
						videoOutInitialWidth = 1600;
						break;
					case 3:
						videoOutInitialWidth = 1760;
						break;
					case 4:
						videoOutInitialWidth = 1920;
						break;
					case 5:
						videoOutReprojectionRate = 60;
						break;
					case 6:
						videoOutReprojectionRate = 90;
						break;
					case 7:
						videoOutReprojectionRate = 120;
						break;
					}
					PlayerSettings.PS4.videoOutInitialWidth = videoOutInitialWidth;
					PlayerSettings.PS4.videoOutReprojectionRate = videoOutReprojectionRate;
				}
			}

			public static extern int videoOutBaseModeInitialWidth
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern int videoOutReprojectionRate
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern string PronunciationXMLPath
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern string PronunciationSIGPath
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern string BackgroundImagePath
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern string StartupImagePath
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern string startupImagesFolder
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern string iconImagesFolder
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern string SaveDataImagePath
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern string SdkOverride
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern string BGMPath
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern string ShareFilePath
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern string ShareOverlayImagePath
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern string PrivacyGuardImagePath
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern bool patchDayOne
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern string PatchPkgPath
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern string PatchLatestPkgPath
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern string PatchChangeinfoPath
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern string NPtitleDatPath
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern bool pnSessions
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern bool pnPresence
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern bool pnFriends
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern bool pnGameCustomData
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern int downloadDataSize
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern int garlicHeapSize
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern int proGarlicHeapSize
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern bool reprojectionSupport
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern bool useAudio3dBackend
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern int audio3dVirtualSpeakerCount
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern int scriptOptimizationLevel
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern int socialScreenEnabled
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern bool attribUserManagement
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern bool attribMoveSupport
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern bool attrib3DSupport
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern bool attribShareSupport
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern bool attribExclusiveVR
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern bool disableAutoHideSplash
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern int attribCpuUsage
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern bool videoRecordingFeaturesUsed
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern bool contentSearchFeaturesUsed
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern PlayerSettings.PS4.PlayStationVREyeToEyeDistanceSettings attribEyeToEyeDistanceSettingVR
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern string[] includedModules
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern bool enableApplicationExit
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}
		}

		public sealed class PSVita
		{
			public enum PSVitaPowerMode
			{
				ModeA,
				ModeB,
				ModeC
			}

			public enum PSVitaTvBootMode
			{
				Default,
				PSVitaBootablePSVitaTvBootable,
				PSVitaBootablePSVitaTvNotBootable
			}

			public enum PSVitaEnterButtonAssignment
			{
				Default,
				CircleButton,
				CrossButton
			}

			public enum PSVitaAppCategory
			{
				Application,
				ApplicationPatch
			}

			public enum PSVitaMemoryExpansionMode
			{
				None,
				ExpandBy29MB,
				ExpandBy77MB,
				ExpandBy109MB
			}

			public enum PSVitaDRMType
			{
				PaidFor,
				Free
			}

			public static extern string npTrophyPackPath
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern PlayerSettings.PSVita.PSVitaPowerMode powerMode
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern bool acquireBGM
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern bool npSupportGBMorGJP
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern PlayerSettings.PSVita.PSVitaTvBootMode tvBootMode
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern bool tvDisableEmu
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern bool upgradable
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern bool healthWarning
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern bool useLibLocation
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern bool infoBarOnStartup
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern bool infoBarColor
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern int scriptOptimizationLevel
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern PlayerSettings.PSVita.PSVitaEnterButtonAssignment enterButtonAssignment
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern int saveDataQuota
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern int parentalLevel
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern string shortTitle
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern string contentID
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern PlayerSettings.PSVita.PSVitaAppCategory category
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern string masterVersion
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern string appVersion
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			[Obsolete("AllowTwitterDialog has no effect as of SDK 3.570")]
			public static extern bool AllowTwitterDialog
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern int npAgeRating
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern string npTitleDatPath
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern string npCommunicationsID
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern string npCommsPassphrase
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern string npCommsSig
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern string paramSfxPath
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern string manualPath
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern string liveAreaGatePath
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern string liveAreaBackroundPath
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern string liveAreaPath
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern string liveAreaTrialPath
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern string patchChangeInfoPath
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern string patchOriginalPackage
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern string packagePassword
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern string keystoneFile
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern PlayerSettings.PSVita.PSVitaMemoryExpansionMode memoryExpansionMode
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern PlayerSettings.PSVita.PSVitaDRMType drmType
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern int storageType
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern int mediaCapacity
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}
		}

		public enum TizenCapability
		{
			Location,
			DataSharing,
			NetworkGet,
			WifiDirect,
			CallHistoryRead,
			Power,
			ContactWrite,
			MessageWrite,
			ContentWrite,
			Push,
			AccountRead,
			ExternalStorage,
			Recorder,
			PackageManagerInfo,
			NFCCardEmulation,
			CalendarWrite,
			WindowPrioritySet,
			VolumeSet,
			CallHistoryWrite,
			AlarmSet,
			Call,
			Email,
			ContactRead,
			Shortcut,
			KeyManager,
			LED,
			NetworkProfile,
			AlarmGet,
			Display,
			CalendarRead,
			NFC,
			AccountWrite,
			Bluetooth,
			Notification,
			NetworkSet,
			ExternalStorageAppData,
			Download,
			Telephony,
			MessageRead,
			MediaStorage,
			Internet,
			Camera,
			Haptic,
			AppManagerLaunch,
			SystemSettings
		}

		public sealed class Tizen
		{
			public static extern string productDescription
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern string productURL
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern string signingProfileName
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern string deploymentTarget
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern int deploymentTargetType
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern TizenOSVersion minOSVersion
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern TizenShowActivityIndicatorOnLoading showActivityIndicatorOnLoading
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			[MethodImpl(MethodImplOptions.InternalCall)]
			private static extern void SetCapability(string name, string value);

			[MethodImpl(MethodImplOptions.InternalCall)]
			private static extern string GetCapability(string name);

			public static void SetCapability(PlayerSettings.TizenCapability capability, bool value)
			{
				PlayerSettings.Tizen.SetCapability(capability.ToString(), value.ToString());
			}

			public static bool GetCapability(PlayerSettings.TizenCapability capability)
			{
				string capability2 = PlayerSettings.Tizen.GetCapability(capability.ToString());
				bool result;
				if (string.IsNullOrEmpty(capability2))
				{
					result = false;
				}
				else
				{
					try
					{
						result = (bool)TypeDescriptor.GetConverter(typeof(bool)).ConvertFromString(capability2);
					}
					catch
					{
						Debug.LogError(string.Concat(new string[]
						{
							"Failed to parse value  ('",
							capability.ToString(),
							",",
							capability2,
							"') to bool type."
						}));
						result = false;
					}
				}
				return result;
			}
		}

		public class tvOS
		{
			private static extern int sdkVersionInt
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static tvOSSdkVersion sdkVersion
			{
				get
				{
					return (tvOSSdkVersion)PlayerSettings.tvOS.sdkVersionInt;
				}
				set
				{
					PlayerSettings.tvOS.sdkVersionInt = (int)value;
				}
			}

			public static string buildNumber
			{
				get
				{
					return PlayerSettings.GetBuildNumber(BuildTargetGroup.tvOS);
				}
				set
				{
					PlayerSettings.SetBuildNumber(BuildTargetGroup.tvOS, value);
				}
			}

			[Obsolete("targetOSVersion is obsolete. Use targetOSVersionString instead.", false)]
			public static tvOSTargetOSVersion targetOSVersion
			{
				get
				{
					string targetOSVersionString = PlayerSettings.tvOS.targetOSVersionString;
					tvOSTargetOSVersion result;
					if (targetOSVersionString == "9.0")
					{
						result = tvOSTargetOSVersion.tvOS_9_0;
					}
					else if (targetOSVersionString == "9.1")
					{
						result = tvOSTargetOSVersion.tvOS_9_1;
					}
					else
					{
						result = tvOSTargetOSVersion.Unknown;
					}
					return result;
				}
				set
				{
					string targetOSVersionString = "";
					if (value == tvOSTargetOSVersion.tvOS_9_0)
					{
						targetOSVersionString = "9.0";
					}
					else if (value == tvOSTargetOSVersion.tvOS_9_1)
					{
						targetOSVersionString = "9.1";
					}
					PlayerSettings.tvOS.targetOSVersionString = targetOSVersionString;
				}
			}

			public static extern string targetOSVersionString
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			private static extern Texture2D[] smallIconLayers
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			private static extern Texture2D[] smallIconLayers2x
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			private static extern Texture2D[] largeIconLayers
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			private static extern Texture2D[] largeIconLayers2x
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			private static extern Texture2D[] topShelfImageLayers
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			private static extern Texture2D[] topShelfImageLayers2x
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			private static extern Texture2D[] topShelfImageWideLayers
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			private static extern Texture2D[] topShelfImageWideLayers2x
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			public static extern bool requireExtendedGameController
			{
				[MethodImpl(MethodImplOptions.InternalCall)]
				get;
				[MethodImpl(MethodImplOptions.InternalCall)]
				set;
			}

			internal static Texture2D[] GetSmallIconLayers()
			{
				return PlayerSettings.tvOS.smallIconLayers;
			}

			internal static void SetSmallIconLayers(Texture2D[] layers)
			{
				PlayerSettings.tvOS.smallIconLayers = layers;
			}

			internal static Texture2D[] GetSmallIconLayers2x()
			{
				return PlayerSettings.tvOS.smallIconLayers2x;
			}

			internal static void SetSmallIconLayers2x(Texture2D[] layers)
			{
				PlayerSettings.tvOS.smallIconLayers2x = layers;
			}

			internal static Texture2D[] GetLargeIconLayers()
			{
				return PlayerSettings.tvOS.largeIconLayers;
			}

			internal static void SetLargeIconLayers(Texture2D[] layers)
			{
				PlayerSettings.tvOS.largeIconLayers = layers;
			}

			internal static Texture2D[] GetLargeIconLayers2x()
			{
				return PlayerSettings.tvOS.largeIconLayers2x;
			}

			internal static void SetLargeIconLayers2x(Texture2D[] layers)
			{
				PlayerSettings.tvOS.largeIconLayers2x = layers;
			}

			internal static Texture2D[] GetTopShelfImageLayers()
			{
				return PlayerSettings.tvOS.topShelfImageLayers;
			}

			internal static void SetTopShelfImageLayers(Texture2D[] layers)
			{
				PlayerSettings.tvOS.topShelfImageLayers = layers;
			}

			internal static Texture2D[] GetTopShelfImageLayers2x()
			{
				return PlayerSettings.tvOS.topShelfImageLayers2x;
			}

			internal static void SetTopShelfImageLayers2x(Texture2D[] layers)
			{
				PlayerSettings.tvOS.topShelfImageLayers2x = layers;
			}

			internal static Texture2D[] GetTopShelfImageWideLayers()
			{
				return PlayerSettings.tvOS.topShelfImageWideLayers;
			}

			internal static void SetTopShelfImageWideLayers(Texture2D[] layers)
			{
				PlayerSettings.tvOS.topShelfImageWideLayers = layers;
			}

			internal static Texture2D[] GetTopShelfImageWideLayers2x()
			{
				return PlayerSettings.tvOS.topShelfImageWideLayers2x;
			}

			internal static void SetTopShelfImageWideLayers2x(Texture2D[] layers)
			{
				PlayerSettings.tvOS.topShelfImageWideLayers2x = layers;
			}
		}

		private static SerializedObject _serializedObject;

		internal static readonly char[] defineSplits = new char[]
		{
			';',
			',',
			' '
		};

		internal static Dictionary<BuildTargetGroup, IPlatformIconProvider> platformIconProviders = new Dictionary<BuildTargetGroup, IPlatformIconProvider>();

		public static extern string companyName
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern string productName
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		[Obsolete("Use PlayerSettings.SplashScreen.show instead")]
		public static extern bool showUnitySplashScreen
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		[Obsolete("Use PlayerSettings.SplashScreen.unityLogoStyle instead")]
		public static extern SplashScreenStyle splashScreenStyle
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		[Obsolete("cloudProjectId is deprecated, use CloudProjectSettings.projectId instead")]
		public static string cloudProjectId
		{
			get
			{
				return PlayerSettings.cloudProjectIdRaw;
			}
		}

		private static extern string cloudProjectIdRaw
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static Guid productGUID
		{
			get
			{
				return new Guid(PlayerSettings.productGUIDRaw);
			}
		}

		private static extern byte[] productGUIDRaw
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public static extern ColorSpace colorSpace
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern int defaultScreenWidth
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern int defaultScreenHeight
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern int defaultWebScreenWidth
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern int defaultWebScreenHeight
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern ResolutionDialogSetting displayResolutionDialog
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		[Obsolete("defaultIsFullScreen is deprecated, use fullScreenMode instead")]
		public static extern bool defaultIsFullScreen
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern bool defaultIsNativeResolution
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern bool macRetinaSupport
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern bool runInBackground
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern bool captureSingleScreen
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern bool usePlayerLog
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern bool resizableWindow
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern bool bakeCollisionMeshes
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern bool useMacAppStoreValidation
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		[Obsolete("macFullscreenMode is deprecated, use fullScreenMode instead")]
		public static extern MacFullscreenMode macFullscreenMode
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		[Obsolete("d3d9FullscreenMode is deprecated, use fullScreenMode instead")]
		public static extern D3D9FullscreenMode d3d9FullscreenMode
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		[Obsolete("d3d11FullscreenMode is deprecated, use fullScreenMode instead")]
		public static extern D3D11FullscreenMode d3d11FullscreenMode
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static FullScreenMode fullScreenMode
		{
			get
			{
				return (FullScreenMode)PlayerSettings.GetFullscreenMode();
			}
			set
			{
				PlayerSettings.SetFullscreenMode((int)value);
			}
		}

		public static extern bool virtualRealitySupported
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		[Obsolete("singlePassStereoRendering will be deprecated. Use stereoRenderingPath instead.")]
		public static extern bool singlePassStereoRendering
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern StereoRenderingPath stereoRenderingPath
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern bool protectGraphicsMemory
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern bool useHDRDisplay
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern bool visibleInBackground
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern bool allowFullscreenSwitch
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern bool forceSingleInstance
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern bool openGLRequireES31
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern bool openGLRequireES31AEP
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern Texture2D resolutionDialogBanner
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern Texture2D virtualRealitySplashScreen
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		[Obsolete("iPhoneBundleIdentifier is deprecated. Use PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.iOS) instead.")]
		public static string iPhoneBundleIdentifier
		{
			get
			{
				return PlayerSettings.GetApplicationIdentifier(BuildTargetGroup.iPhone);
			}
			set
			{
				PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.iPhone, value);
			}
		}

		internal static extern string[] templateCustomKeys
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		internal static extern string spritePackerPolicy
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern ScriptingRuntimeVersion scriptingRuntimeVersion
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern string keystorePass
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern string keyaliasPass
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		[Obsolete("Xbox 360 has been removed in >=5.5")]
		public static extern string xboxTitleId
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		[Obsolete("Xbox 360 has been removed in >=5.5")]
		public static extern string xboxImageXexFilePath
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		[Obsolete("Xbox 360 has been removed in >=5.5")]
		public static extern string xboxSpaFilePath
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		[Obsolete("Xbox 360 has been removed in >=5.5")]
		public static extern bool xboxGenerateSpa
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		[Obsolete("Xbox 360 has been removed in >=5.5")]
		public static extern bool xboxEnableGuest
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		[Obsolete("Xbox 360 has been removed in >=5.5")]
		public static extern bool xboxDeployKinectResources
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		[Obsolete("Xbox 360 has been removed in >=5.5")]
		public static extern bool xboxDeployKinectHeadOrientation
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		[Obsolete("Xbox 360 has been removed in >=5.5")]
		public static extern bool xboxDeployKinectHeadPosition
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		[Obsolete("Xbox 360 has been removed in >=5.5")]
		public static extern Texture2D xboxSplashScreen
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		[Obsolete("Xbox 360 has been removed in >=5.5")]
		public static extern int xboxAdditionalTitleMemorySize
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		[Obsolete("Xbox 360 has been removed in >=5.5")]
		public static extern bool xboxEnableKinect
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		[Obsolete("Xbox 360 has been removed in >=5.5")]
		public static extern bool xboxEnableKinectAutoTracking
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		[Obsolete("Xbox 360 has been removed in >=5.5")]
		public static extern bool xboxEnableSpeech
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		[Obsolete("Xbox 360 has been removed in >=5.5")]
		public static extern uint xboxSpeechDB
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public static extern bool gpuSkinning
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern bool graphicsJobs
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern GraphicsJobMode graphicsJobMode
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern bool xboxPIXTextureCapture
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public static extern bool xboxEnableAvatar
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public static extern int xboxOneResolution
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public static extern bool enableInternalProfiler
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern ActionOnDotNetUnhandledException actionOnDotNetUnhandledException
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern bool logObjCUncaughtExceptions
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern bool enableCrashReportAPI
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static string applicationIdentifier
		{
			get
			{
				return PlayerSettings.GetApplicationIdentifier(EditorUserBuildSettings.selectedBuildTargetGroup);
			}
			set
			{
				Debug.LogWarning("PlayerSettings.applicationIdentifier only changes the identifier for the currently active platform. Please use SetApplicationIdentifier to set it for any platform");
				PlayerSettings.SetApplicationIdentifier(EditorUserBuildSettings.selectedBuildTargetGroup, value);
			}
		}

		public static extern string bundleVersion
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern bool statusBarHidden
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern StrippingLevel strippingLevel
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern bool stripEngineCode
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern UIOrientation defaultInterfaceOrientation
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern bool allowedAutorotateToPortrait
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern bool allowedAutorotateToPortraitUpsideDown
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern bool allowedAutorotateToLandscapeRight
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern bool allowedAutorotateToLandscapeLeft
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern bool useAnimatedAutorotation
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern bool use32BitDisplayBuffer
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern bool preserveFramebufferAlpha
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		[Obsolete("apiCompatibilityLevel is deprecated. Use PlayerSettings.GetApiCompatibilityLevel()/PlayerSettings.SetApiCompatibilityLevel() instead.")]
		public static extern ApiCompatibilityLevel apiCompatibilityLevel
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern bool stripUnusedMeshComponents
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern bool advancedLicense
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public static extern string aotOptions
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern Texture2D defaultCursor
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static Vector2 cursorHotspot
		{
			get
			{
				Vector2 result;
				PlayerSettings.INTERNAL_get_cursorHotspot(out result);
				return result;
			}
			set
			{
				PlayerSettings.INTERNAL_set_cursorHotspot(ref value);
			}
		}

		public static extern int accelerometerFrequency
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern bool MTRendering
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		[Obsolete("Use UnityEditor.PlayerSettings.SetGraphicsAPIs/GetGraphicsAPIs instead")]
		public static extern bool useDirect3D11
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		internal static extern bool submitAnalytics
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		[Obsolete("Use VREditor.GetStereoDeviceEnabled instead")]
		public static extern bool stereoscopic3D
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern bool muteOtherAudioSources
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		internal static extern bool playModeTestRunnerEnabled
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		internal static extern bool runPlayModeTestAsEditModeTest
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static bool enable360StereoCapture
		{
			get
			{
				return PlayerSettings360StereoCapture.enable360StereoCapture;
			}
			set
			{
				PlayerSettings360StereoCapture.enable360StereoCapture = value;
			}
		}

		[Obsolete("The option alwaysDisplayWatermark is deprecated and is always false", true)]
		public static bool alwaysDisplayWatermark
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		[Obsolete("Use AssetBundles instead for streaming data", true)]
		public static int firstStreamedLevelWithResources
		{
			get
			{
				return 0;
			}
			set
			{
			}
		}

		[Obsolete("targetGlesGraphics is ignored, use SetGraphicsAPIs/GetGraphicsAPIs APIs", false)]
		public static TargetGlesGraphics targetGlesGraphics
		{
			get
			{
				return TargetGlesGraphics.Automatic;
			}
			set
			{
			}
		}

		[Obsolete("targetIOSGraphics is ignored, use SetGraphicsAPIs/GetGraphicsAPIs APIs", false)]
		public static TargetIOSGraphics targetIOSGraphics
		{
			get
			{
				return TargetIOSGraphics.Automatic;
			}
			set
			{
			}
		}

		[Obsolete("Use PlayerSettings.iOS.locationUsageDescription instead (UnityUpgradable) -> UnityEditor.PlayerSettings/iOS.locationUsageDescription", false)]
		public static string locationUsageDescription
		{
			get
			{
				return PlayerSettings.iOS.locationUsageDescription;
			}
			set
			{
				PlayerSettings.iOS.locationUsageDescription = value;
			}
		}

		[Obsolete("renderingPath is ignored, use UnityEditor.Rendering.TierSettings with UnityEditor.Rendering.SetTierSettings/GetTierSettings instead", false)]
		public static RenderingPath renderingPath
		{
			get
			{
				return EditorGraphicsSettings.GetCurrentTierSettings().renderingPath;
			}
			set
			{
			}
		}

		[Obsolete("mobileRenderingPath is ignored, use UnityEditor.Rendering.TierSettings with UnityEditor.Rendering.SetTierSettings/GetTierSettings instead", false)]
		public static RenderingPath mobileRenderingPath
		{
			get
			{
				return EditorGraphicsSettings.GetCurrentTierSettings().renderingPath;
			}
			set
			{
			}
		}

		[Obsolete("Use PlayerSettings.applicationIdentifier instead (UnityUpgradable) -> UnityEditor.PlayerSettings.applicationIdentifier", true)]
		public static string bundleIdentifier
		{
			get
			{
				return PlayerSettings.applicationIdentifier;
			}
			set
			{
				PlayerSettings.applicationIdentifier = value;
			}
		}

		private PlayerSettings()
		{
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern UnityEngine.Object InternalGetPlayerSettingsObject();

		internal static SerializedObject GetSerializedObject()
		{
			if (PlayerSettings._serializedObject == null)
			{
				PlayerSettings._serializedObject = new SerializedObject(PlayerSettings.InternalGetPlayerSettingsObject());
			}
			return PlayerSettings._serializedObject;
		}

		internal static SerializedProperty FindProperty(string name)
		{
			SerializedProperty serializedProperty = PlayerSettings.GetSerializedObject().FindProperty(name);
			if (serializedProperty == null)
			{
				Debug.LogError("Failed to find:" + name);
			}
			return serializedProperty;
		}

		[Obsolete("Use explicit API instead."), GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetPropertyInt(string name, int value, [UnityEngine.Internal.DefaultValue("BuildTargetGroup.Unknown")] BuildTargetGroup target);

		[Obsolete("Use explicit API instead."), ExcludeFromDocs]
		public static void SetPropertyInt(string name, int value)
		{
			BuildTargetGroup target = BuildTargetGroup.Unknown;
			PlayerSettings.SetPropertyInt(name, value, target);
		}

		[Obsolete("Use explicit API instead.")]
		public static void SetPropertyInt(string name, int value, BuildTarget target)
		{
			PlayerSettings.SetPropertyInt(name, value, BuildPipeline.GetBuildTargetGroup(target));
		}

		[Obsolete("Use explicit API instead."), GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int GetPropertyInt(string name, [UnityEngine.Internal.DefaultValue("BuildTargetGroup.Unknown")] BuildTargetGroup target);

		[Obsolete("Use explicit API instead."), ExcludeFromDocs]
		public static int GetPropertyInt(string name)
		{
			BuildTargetGroup target = BuildTargetGroup.Unknown;
			return PlayerSettings.GetPropertyInt(name, target);
		}

		[Obsolete("Use explicit API instead."), ExcludeFromDocs]
		public static bool GetPropertyOptionalInt(string name, ref int value)
		{
			BuildTargetGroup target = BuildTargetGroup.Unknown;
			return PlayerSettings.GetPropertyOptionalInt(name, ref value, target);
		}

		[Obsolete("Use explicit API instead.")]
		public static bool GetPropertyOptionalInt(string name, ref int value, [UnityEngine.Internal.DefaultValue("BuildTargetGroup.Unknown")] BuildTargetGroup target)
		{
			value = PlayerSettings.GetPropertyInt(name, target);
			return true;
		}

		[Obsolete("Use explicit API instead."), GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetPropertyBool(string name, bool value, [UnityEngine.Internal.DefaultValue("BuildTargetGroup.Unknown")] BuildTargetGroup target);

		[Obsolete("Use explicit API instead."), ExcludeFromDocs]
		public static void SetPropertyBool(string name, bool value)
		{
			BuildTargetGroup target = BuildTargetGroup.Unknown;
			PlayerSettings.SetPropertyBool(name, value, target);
		}

		[Obsolete("Use explicit API instead.")]
		public static void SetPropertyBool(string name, bool value, BuildTarget target)
		{
			PlayerSettings.SetPropertyBool(name, value, BuildPipeline.GetBuildTargetGroup(target));
		}

		[Obsolete("Use explicit API instead."), GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool GetPropertyBool(string name, [UnityEngine.Internal.DefaultValue("BuildTargetGroup.Unknown")] BuildTargetGroup target);

		[Obsolete("Use explicit API instead."), ExcludeFromDocs]
		public static bool GetPropertyBool(string name)
		{
			BuildTargetGroup target = BuildTargetGroup.Unknown;
			return PlayerSettings.GetPropertyBool(name, target);
		}

		[Obsolete("Use explicit API instead."), ExcludeFromDocs]
		public static bool GetPropertyOptionalBool(string name, ref bool value)
		{
			BuildTargetGroup target = BuildTargetGroup.Unknown;
			return PlayerSettings.GetPropertyOptionalBool(name, ref value, target);
		}

		[Obsolete("Use explicit API instead.")]
		public static bool GetPropertyOptionalBool(string name, ref bool value, [UnityEngine.Internal.DefaultValue("BuildTargetGroup.Unknown")] BuildTargetGroup target)
		{
			value = PlayerSettings.GetPropertyBool(name, target);
			return true;
		}

		[Obsolete("Use explicit API instead."), GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetPropertyString(string name, string value, [UnityEngine.Internal.DefaultValue("BuildTargetGroup.Unknown")] BuildTargetGroup target);

		[Obsolete("Use explicit API instead."), ExcludeFromDocs]
		public static void SetPropertyString(string name, string value)
		{
			BuildTargetGroup target = BuildTargetGroup.Unknown;
			PlayerSettings.SetPropertyString(name, value, target);
		}

		[Obsolete("Use explicit API instead.")]
		public static void SetPropertyString(string name, string value, BuildTarget target)
		{
			PlayerSettings.SetPropertyString(name, value, BuildPipeline.GetBuildTargetGroup(target));
		}

		[Obsolete("Use explicit API instead."), GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern string GetPropertyString(string name, [UnityEngine.Internal.DefaultValue("BuildTargetGroup.Unknown")] BuildTargetGroup target);

		[Obsolete("Use explicit API instead."), ExcludeFromDocs]
		public static string GetPropertyString(string name)
		{
			BuildTargetGroup target = BuildTargetGroup.Unknown;
			return PlayerSettings.GetPropertyString(name, target);
		}

		[Obsolete("Use explicit API instead."), ExcludeFromDocs]
		public static bool GetPropertyOptionalString(string name, ref string value)
		{
			BuildTargetGroup target = BuildTargetGroup.Unknown;
			return PlayerSettings.GetPropertyOptionalString(name, ref value, target);
		}

		[Obsolete("Use explicit API instead.")]
		public static bool GetPropertyOptionalString(string name, ref string value, [UnityEngine.Internal.DefaultValue("BuildTargetGroup.Unknown")] BuildTargetGroup target)
		{
			value = PlayerSettings.GetPropertyString(name, target);
			return true;
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void SetDirty();

		internal static void SetCloudProjectId(string projectId)
		{
			PlayerSettings.cloudProjectIdRaw = projectId;
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void SetCloudServiceEnabled(string serviceKey, bool enabled);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool GetCloudServiceEnabled(string serviceKey);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool HasAspectRatio(AspectRatio aspectRatio);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetAspectRatio(AspectRatio aspectRatio, bool enable);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetFullscreenMode();

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetFullscreenMode(int fullscreenMode);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern Texture2D[] GetIconsForPlatform(string platform, IconKind kind);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern Texture2D[] GetAllIconsForPlatform(string platform);

		internal static void SetIconsForPlatform(string platform, Texture2D[] icons)
		{
			PlayerSettings.SetIconsForPlatform(platform, icons, IconKind.Any);
		}

		internal static void SetIconsForPlatform(string platform, Texture2D[] icons, IconKind[] kinds)
		{
			IconKind[] supportedIconKindsForPlatform = PlayerSettings.GetSupportedIconKindsForPlatform(platform);
			for (int i = 0; i < supportedIconKindsForPlatform.Length; i++)
			{
				IconKind iconKind = supportedIconKindsForPlatform[i];
				List<Texture2D> list = new List<Texture2D>();
				for (int j = 0; j < icons.Length; j++)
				{
					if (kinds[j] == iconKind)
					{
						list.Add(icons[j]);
					}
				}
				PlayerSettings.SetIconsForPlatform(platform, list.ToArray(), iconKind);
			}
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void SetIconsForPlatform(string platform, Texture2D[] icons, IconKind kind);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int[] GetIconWidthsForPlatform(string platform, IconKind kind);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int[] GetIconHeightsForPlatform(string platform, IconKind kind);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int[] GetIconWidthsOfAllKindsForPlatform(string platform);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int[] GetIconHeightsOfAllKindsForPlatform(string platform);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern IconKind[] GetIconKindsForPlatform(string platform);

		internal static IconKind[] GetSupportedIconKindsForPlatform(string platform)
		{
			List<IconKind> list = new List<IconKind>();
			IconKind[] iconKindsForPlatform = PlayerSettings.GetIconKindsForPlatform(platform);
			IconKind[] array = iconKindsForPlatform;
			for (int i = 0; i < array.Length; i++)
			{
				IconKind item = array[i];
				if (!list.Contains(item))
				{
					list.Add(item);
				}
			}
			return list.ToArray();
		}

		internal static string GetPlatformName(BuildTargetGroup targetGroup)
		{
			BuildPlatform buildPlatform = BuildPlatforms.instance.GetValidPlatforms().Find((BuildPlatform p) => p.targetGroup == targetGroup);
			return (buildPlatform != null) ? buildPlatform.name : string.Empty;
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern Texture2D GetIconForPlatformAtSize(string platform, int width, int height, IconKind kind);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void GetBatchingForPlatform(BuildTarget platform, out int staticBatching, out int dynamicBatching);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void SetBatchingForPlatform(BuildTarget platform, int staticBatching, int dynamicBatching);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern LightmapEncodingQuality GetLightmapEncodingQualityForPlatformGroup(BuildTargetGroup platformGroup);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void SetLightmapEncodingQualityForPlatformGroup(BuildTargetGroup platformGroup, LightmapEncodingQuality encodingQuality);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern GraphicsDeviceType[] GetSupportedGraphicsAPIs(BuildTarget platform);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern GraphicsDeviceType[] GetGraphicsAPIs(BuildTarget platform);

		public static void SetGraphicsAPIs(BuildTarget platform, GraphicsDeviceType[] apis)
		{
			PlayerSettings.SetGraphicsAPIsImpl(platform, apis);
			PlayerSettingsEditor.SyncPlatformAPIsList(platform);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetGraphicsAPIsImpl(BuildTarget platform, GraphicsDeviceType[] apis);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool GetUseDefaultGraphicsAPIs(BuildTarget platform);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetUseDefaultGraphicsAPIs(BuildTarget platform, bool automatic);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern ColorGamut[] GetColorGamuts();

		internal static void SetColorGamuts(ColorGamut[] colorSpaces)
		{
			PlayerSettings.SetColorGamutsImpl(colorSpaces);
			PlayerSettingsEditor.SyncColorGamuts();
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetColorGamutsImpl(ColorGamut[] colorSpaces);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void SetTemplateCustomValue(string key, string value);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern string GetTemplateCustomValue(string key);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern string GetScriptingDefineSymbolsForGroup(BuildTargetGroup targetGroup);

		public static void SetScriptingDefineSymbolsForGroup(BuildTargetGroup targetGroup, string defines)
		{
			if (!string.IsNullOrEmpty(defines))
			{
				defines = string.Join(";", defines.Split(PlayerSettings.defineSplits, StringSplitOptions.RemoveEmptyEntries));
			}
			PlayerSettings.SetScriptingDefineSymbolsForGroupInternal(targetGroup, defines);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetScriptingDefineSymbolsForGroupInternal(BuildTargetGroup targetGroup, string defines);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int GetArchitecture(BuildTargetGroup targetGroup);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetArchitecture(BuildTargetGroup targetGroup, int architecture);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern ScriptingImplementation GetScriptingBackend(BuildTargetGroup targetGroup);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetApplicationIdentifier(BuildTargetGroup targetGroup, string identifier);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern string GetApplicationIdentifier(BuildTargetGroup targetGroup);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void SetBuildNumber(BuildTargetGroup targetGroup, string buildNumber);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern string GetBuildNumber(BuildTargetGroup targetGroup);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetScriptingBackend(BuildTargetGroup targetGroup, ScriptingImplementation backend);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern ScriptingImplementation GetDefaultScriptingBackend(BuildTargetGroup targetGroup);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetIl2CppCompilerConfiguration(BuildTargetGroup targetGroup, Il2CppCompilerConfiguration configuration);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern Il2CppCompilerConfiguration GetIl2CppCompilerConfiguration(BuildTargetGroup targetGroup);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool GetIncrementalIl2CppBuild(BuildTargetGroup targetGroup);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetIncrementalIl2CppBuild(BuildTargetGroup targetGroup, bool enabled);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern string GetAdditionalIl2CppArgs();

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetAdditionalIl2CppArgs(string additionalArgs);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool GetPlatformVuforiaEnabled(BuildTargetGroup targetGroup);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetPlatformVuforiaEnabled(BuildTargetGroup targetGroup, bool enabled);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern ApiCompatibilityLevel GetApiCompatibilityLevel(BuildTargetGroup buildTargetGroup);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetApiCompatibilityLevel(BuildTargetGroup buildTargetGroup, ApiCompatibilityLevel value);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_get_cursorHotspot(out Vector2 value);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_set_cursorHotspot(ref Vector2 value);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetMobileMTRendering(BuildTargetGroup targetGroup, bool enable);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool GetMobileMTRendering(BuildTargetGroup targetGroup);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern StackTraceLogType GetStackTraceLogType(LogType logType);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetStackTraceLogType(LogType logType, StackTraceLogType stackTraceType);

		public static string[] GetAvailableVirtualRealitySDKs(BuildTargetGroup targetGroup)
		{
			return VREditor.GetAvailableVirtualRealitySDKs(targetGroup);
		}

		public static bool GetVirtualRealitySupported(BuildTargetGroup targetGroup)
		{
			return VREditor.GetVREnabledOnTargetGroup(targetGroup);
		}

		public static void SetVirtualRealitySupported(BuildTargetGroup targetGroup, bool value)
		{
			VREditor.SetVREnabledOnTargetGroup(targetGroup, value);
		}

		public static string[] GetVirtualRealitySDKs(BuildTargetGroup targetGroup)
		{
			return VREditor.GetVirtualRealitySDKs(targetGroup);
		}

		public static void SetVirtualRealitySDKs(BuildTargetGroup targetGroup, string[] sdks)
		{
			VREditor.SetVirtualRealitySDKs(targetGroup, sdks);
		}

		internal static IPlatformIconProvider GetPlatformIconProvider(BuildTargetGroup platform)
		{
			IPlatformIconProvider result;
			if (!PlayerSettings.platformIconProviders.ContainsKey(platform))
			{
				result = null;
			}
			else
			{
				result = PlayerSettings.platformIconProviders[platform];
			}
			return result;
		}

		internal static bool HasPlatformIconProvider(BuildTargetGroup platform)
		{
			return PlayerSettings.platformIconProviders.ContainsKey(platform);
		}

		internal static void RegisterPlatformIconProvider(BuildTargetGroup platform, IPlatformIconProvider platformIconProvider)
		{
			if (!PlayerSettings.platformIconProviders.ContainsKey(platform))
			{
				PlayerSettings.platformIconProviders[platform] = platformIconProvider;
			}
		}

		public static PlatformIcon[] GetPlatformIcons(BuildTargetGroup platform, PlatformIconKind kind)
		{
			IPlatformIconProvider platformIconProvider = PlayerSettings.GetPlatformIconProvider(platform);
			PlatformIcon[] result;
			if (platformIconProvider == null)
			{
				result = new PlatformIcon[0];
			}
			else
			{
				string platformName = PlayerSettings.GetPlatformName(platform);
				PlatformIconStruct[] platformIconsInternal = PlayerSettings.GetPlatformIconsInternal(platformName, kind.kind);
				PlatformIcon[] requiredPlatformIconsByType = PlatformIcon.GetRequiredPlatformIconsByType(platformIconProvider, kind);
				if (platformIconsInternal.Length <= 0)
				{
					PlatformIcon[] array = requiredPlatformIconsByType;
					for (int i = 0; i < array.Length; i++)
					{
						PlatformIcon platformIcon = array[i];
						platformIcon.SetTextures(null);
					}
				}
				else
				{
					PlatformIcon[] array2 = requiredPlatformIconsByType;
					for (int j = 0; j < array2.Length; j++)
					{
						PlatformIcon platformIcon2 = array2[j];
						PlatformIconStruct[] array3 = platformIconsInternal;
						for (int k = 0; k < array3.Length; k++)
						{
							PlatformIconStruct platformIconStruct = array3[k];
							int num = (!kind.Equals(PlatformIconKind.Any)) ? kind.kind : platformIconStruct.m_Kind;
							if (platformIcon2.kind.kind == num && platformIcon2.iconSubKind == platformIconStruct.m_SubKind)
							{
								if (platformIcon2.width == platformIconStruct.m_Width && platformIcon2.height == platformIconStruct.m_Height)
								{
									Texture2D[] array4 = platformIconStruct.m_Textures.Take(platformIcon2.maxLayerCount).ToArray<Texture2D>();
									Texture2D[] array5 = new Texture2D[(array4.Length <= platformIcon2.minLayerCount) ? platformIcon2.minLayerCount : array4.Length];
									for (int l = 0; l < array4.Length; l++)
									{
										array5[l] = array4[l];
									}
									platformIcon2.SetTextures(array5);
									break;
								}
							}
						}
					}
				}
				result = requiredPlatformIconsByType;
			}
			return result;
		}

		public static void SetPlatformIcons(BuildTargetGroup platform, PlatformIconKind kind, PlatformIcon[] icons)
		{
			string platformName = PlayerSettings.GetPlatformName(platform);
			IPlatformIconProvider platformIconProvider = PlayerSettings.GetPlatformIconProvider(platform);
			if (platformIconProvider != null)
			{
				int num = PlatformIcon.GetRequiredPlatformIconsByType(platformIconProvider, kind).Length;
				PlatformIconStruct[] icons2;
				if (icons == null)
				{
					icons2 = new PlatformIconStruct[0];
				}
				else
				{
					if (num != icons.Length)
					{
						throw new InvalidOperationException(string.Format("Attempting to set an incorrect number of icons for {0} {1} kind, it requires {2} icons but trying to assign {3}.", new object[]
						{
							platform.ToString(),
							kind.ToString(),
							num,
							icons.Length
						}));
					}
					icons2 = (from i in icons
					select i.GetPlatformIconStruct()).ToArray<PlatformIconStruct>();
				}
				PlayerSettings.SetPlatformIconsInternal(platformName, icons2, kind.kind);
			}
		}

		public static PlatformIconKind[] GetSupportedIconKindsForPlatform(BuildTargetGroup platform)
		{
			IPlatformIconProvider platformIconProvider = PlayerSettings.GetPlatformIconProvider(platform);
			PlatformIconKind[] result;
			if (platformIconProvider == null)
			{
				result = new PlatformIconKind[0];
			}
			else
			{
				result = platformIconProvider.GetRequiredPlatformIcons().Keys.ToArray<PlatformIconKind>();
			}
			return result;
		}

		internal static int GetNonEmptyPlatformIconCount(PlatformIcon[] icons)
		{
			return icons.Count((PlatformIcon i) => !i.IsEmpty());
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void SetPlatformIconsInternal(string platform, PlatformIconStruct[] icons, int kind);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern PlatformIconStruct[] GetPlatformIconsInternal(string platform, int kind);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern Texture2D GetPlatformIconAtSize(string platform, int width, int height, int kind, string subKind = "", int layer = 0);

		internal static void ClearSetIconsForPlatform(BuildTargetGroup target)
		{
			PlayerSettings.SetPlatformIcons(target, PlatformIconKind.Any, null);
		}

		public static void SetIconsForTargetGroup(BuildTargetGroup platform, Texture2D[] icons, IconKind kind)
		{
			if (platform == BuildTargetGroup.iPhone || platform == BuildTargetGroup.tvOS || platform == BuildTargetGroup.Android)
			{
				IPlatformIconProvider platformIconProvider = PlayerSettings.GetPlatformIconProvider(platform);
				if (platformIconProvider != null)
				{
					PlatformIconKind platformIconKindFromEnumValue = platformIconProvider.GetPlatformIconKindFromEnumValue(kind);
					PlatformIcon[] platformIcons = PlayerSettings.GetPlatformIcons(platform, platformIconKindFromEnumValue);
					for (int i = 0; i < icons.Length; i++)
					{
						platformIcons[i].SetTexture(icons[i], 0);
					}
					PlayerSettings.SetPlatformIcons(platform, platformIconKindFromEnumValue, platformIcons);
				}
			}
			else
			{
				PlayerSettings.SetIconsForPlatform(PlayerSettings.GetPlatformName(platform), icons, kind);
			}
		}

		public static void SetIconsForTargetGroup(BuildTargetGroup platform, Texture2D[] icons)
		{
			PlayerSettings.SetIconsForTargetGroup(platform, icons, IconKind.Any);
		}

		public static Texture2D[] GetIconsForTargetGroup(BuildTargetGroup platform, IconKind kind)
		{
			Texture2D[] result;
			if (platform == BuildTargetGroup.iPhone || platform == BuildTargetGroup.tvOS || platform == BuildTargetGroup.Android)
			{
				IPlatformIconProvider platformIconProvider = PlayerSettings.GetPlatformIconProvider(platform);
				if (platformIconProvider == null)
				{
					result = new Texture2D[0];
				}
				else
				{
					PlatformIconKind platformIconKindFromEnumValue = platformIconProvider.GetPlatformIconKindFromEnumValue(kind);
					result = (from t in PlayerSettings.GetPlatformIcons(platform, platformIconKindFromEnumValue)
					select t.GetTexture(0)).ToArray<Texture2D>();
				}
			}
			else
			{
				Texture2D[] iconsForPlatform = PlayerSettings.GetIconsForPlatform(PlayerSettings.GetPlatformName(platform), kind);
				result = iconsForPlatform;
			}
			return result;
		}

		public static Texture2D[] GetIconsForTargetGroup(BuildTargetGroup platform)
		{
			return PlayerSettings.GetIconsForTargetGroup(platform, IconKind.Any);
		}

		public static int[] GetIconSizesForTargetGroup(BuildTargetGroup platform, IconKind kind)
		{
			int[] result;
			if (platform == BuildTargetGroup.iPhone || platform == BuildTargetGroup.tvOS || platform == BuildTargetGroup.Android)
			{
				IPlatformIconProvider platformIconProvider = PlayerSettings.GetPlatformIconProvider(platform);
				if (platformIconProvider == null)
				{
					result = new int[0];
				}
				else
				{
					PlatformIconKind platformIconKindFromEnumValue = platformIconProvider.GetPlatformIconKindFromEnumValue(kind);
					result = (from s in PlayerSettings.GetPlatformIcons(platform, platformIconKindFromEnumValue)
					select s.width).ToArray<int>();
				}
			}
			else
			{
				result = PlayerSettings.GetIconWidthsForPlatform(PlayerSettings.GetPlatformName(platform), kind);
			}
			return result;
		}

		public static int[] GetIconSizesForTargetGroup(BuildTargetGroup platform)
		{
			return PlayerSettings.GetIconSizesForTargetGroup(platform, IconKind.Any);
		}
	}
}
