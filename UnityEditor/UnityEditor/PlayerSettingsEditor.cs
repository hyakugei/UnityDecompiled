using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor.AnimatedValues;
using UnityEditor.Build;
using UnityEditor.CrashReporting;
using UnityEditor.Modules;
using UnityEditor.PlatformSupport;
using UnityEditor.SceneManagement;
using UnityEditor.Scripting.ScriptCompilation;
using UnityEditorInternal;
using UnityEditorInternal.VR;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

namespace UnityEditor
{
	[CustomEditor(typeof(PlayerSettings))]
	internal class PlayerSettingsEditor : Editor
	{
		private static class Styles
		{
			public static readonly GUIStyle categoryBox;

			public static readonly GUIContent colorSpaceAndroidWarning;

			public static readonly GUIContent colorSpaceWebGLWarning;

			public static readonly GUIContent colorSpaceIOSWarning;

			public static readonly GUIContent colorSpaceTVOSWarning;

			public static readonly GUIContent recordingInfo;

			public static readonly GUIContent notApplicableInfo;

			public static readonly GUIContent sharedBetweenPlatformsInfo;

			public static readonly GUIContent VRSupportOverridenInfo;

			public static readonly GUIContent IL2CPPAndroidExperimentalInfo;

			public static readonly GUIContent cursorHotspot;

			public static readonly GUIContent defaultCursor;

			public static readonly GUIContent defaultIcon;

			public static readonly GUIContent vertexChannelCompressionMask;

			public static readonly GUIContent iconTitle;

			public static readonly GUIContent resolutionPresentationTitle;

			public static readonly GUIContent resolutionTitle;

			public static readonly GUIContent orientationTitle;

			public static readonly GUIContent allowedOrientationTitle;

			public static readonly GUIContent multitaskingSupportTitle;

			public static readonly GUIContent statusBarTitle;

			public static readonly GUIContent standalonePlayerOptionsTitle;

			public static readonly GUIContent debuggingCrashReportingTitle;

			public static readonly GUIContent debuggingTitle;

			public static readonly GUIContent crashReportingTitle;

			public static readonly GUIContent otherSettingsTitle;

			public static readonly GUIContent renderingTitle;

			public static readonly GUIContent identificationTitle;

			public static readonly GUIContent macAppStoreTitle;

			public static readonly GUIContent configurationTitle;

			public static readonly GUIContent optimizationTitle;

			public static readonly GUIContent loggingTitle;

			public static readonly GUIContent publishingSettingsTitle;

			public static readonly GUIContent bakeCollisionMeshes;

			public static readonly GUIContent keepLoadedShadersAlive;

			public static readonly GUIContent preloadedAssets;

			public static readonly GUIContent stripEngineCode;

			public static readonly GUIContent iPhoneStrippingLevel;

			public static readonly GUIContent iPhoneScriptCallOptimization;

			public static readonly GUIContent enableInternalProfiler;

			public static readonly GUIContent stripUnusedMeshComponents;

			public static readonly GUIContent videoMemoryForVertexBuffers;

			public static readonly GUIContent protectGraphicsMemory;

			public static readonly GUIContent useOSAutoRotation;

			public static readonly GUIContent UIPrerenderedIcon;

			public static readonly GUIContent defaultScreenWidth;

			public static readonly GUIContent defaultScreenHeight;

			public static readonly GUIContent macRetinaSupport;

			public static readonly GUIContent runInBackground;

			public static readonly GUIContent defaultScreenOrientation;

			public static readonly GUIContent allowedAutoRotateToPortrait;

			public static readonly GUIContent allowedAutoRotateToPortraitUpsideDown;

			public static readonly GUIContent allowedAutoRotateToLandscapeRight;

			public static readonly GUIContent allowedAutoRotateToLandscapeLeft;

			public static readonly GUIContent UIRequiresFullScreen;

			public static readonly GUIContent UIStatusBarHidden;

			public static readonly GUIContent UIStatusBarStyle;

			public static readonly GUIContent useMacAppStoreValidation;

			public static readonly GUIContent macAppStoreCategory;

			public static readonly GUIContent fullscreenMode;

			public static readonly GUIContent exclusiveFullscreen;

			public static readonly GUIContent fullscreenWindow;

			public static readonly GUIContent maximizedWindow;

			public static readonly GUIContent windowed;

			public static readonly GUIContent visibleInBackground;

			public static readonly GUIContent allowFullscreenSwitch;

			public static readonly GUIContent use32BitDisplayBuffer;

			public static readonly GUIContent disableDepthAndStencilBuffers;

			public static readonly GUIContent iosShowActivityIndicatorOnLoading;

			public static readonly GUIContent androidShowActivityIndicatorOnLoading;

			public static readonly GUIContent actionOnDotNetUnhandledException;

			public static readonly GUIContent logObjCUncaughtExceptions;

			public static readonly GUIContent enableCrashReportAPI;

			public static readonly GUIContent activeColorSpace;

			public static readonly GUIContent colorGamut;

			public static readonly GUIContent colorGamutForMac;

			public static readonly GUIContent metalForceHardShadows;

			public static readonly GUIContent metalEditorSupport;

			public static readonly GUIContent metalAPIValidation;

			public static readonly GUIContent metalFramebufferOnly;

			public static readonly GUIContent mTRendering;

			public static readonly GUIContent staticBatching;

			public static readonly GUIContent dynamicBatching;

			public static readonly GUIContent graphicsJobs;

			public static readonly GUIContent graphicsJobsMode;

			public static readonly GUIContent applicationBuildNumber;

			public static readonly GUIContent appleDeveloperTeamID;

			public static readonly GUIContent useOnDemandResources;

			public static readonly GUIContent accelerometerFrequency;

			public static readonly GUIContent cameraUsageDescription;

			public static readonly GUIContent locationUsageDescription;

			public static readonly GUIContent microphoneUsageDescription;

			public static readonly GUIContent muteOtherAudioSources;

			public static readonly GUIContent prepareIOSForRecording;

			public static readonly GUIContent forceIOSSpeakersWhenRecording;

			public static readonly GUIContent UIRequiresPersistentWiFi;

			public static readonly GUIContent iOSAllowHTTPDownload;

			public static readonly GUIContent iOSURLSchemes;

			public static readonly GUIContent aotOptions;

			public static readonly GUIContent require31;

			public static readonly GUIContent requireAEP;

			public static readonly GUIContent skinOnGPU;

			public static readonly GUIContent skinOnGPUPS4;

			public static readonly GUIContent skinOnGPUAndroidWarning;

			public static readonly GUIContent disableStatistics;

			public static readonly GUIContent scriptingDefineSymbols;

			public static readonly GUIContent scriptingRuntimeVersion;

			public static readonly GUIContent scriptingRuntimeVersionLegacy;

			public static readonly GUIContent scriptingRuntimeVersionLatest;

			public static readonly GUIContent scriptingBackend;

			public static readonly GUIContent il2cppCompilerConfiguration;

			public static readonly GUIContent scriptingMono2x;

			public static readonly GUIContent scriptingWinRTDotNET;

			public static readonly GUIContent scriptingIL2CPP;

			public static readonly GUIContent scriptingDefault;

			public static readonly GUIContent apiCompatibilityLevel;

			public static readonly GUIContent apiCompatibilityLevel_NET_2_0;

			public static readonly GUIContent apiCompatibilityLevel_NET_2_0_Subset;

			public static readonly GUIContent apiCompatibilityLevel_NET_4_6;

			public static readonly GUIContent apiCompatibilityLevel_NET_Standard_2_0;

			public static readonly GUIContent activeInputHandling;

			public static readonly GUIContent[] activeInputHandlingOptions;

			public static readonly GUIContent vrSettingsMoved;

			public static readonly GUIContent lightmapEncodingLabel;

			public static readonly GUIContent[] lightmapEncodingNames;

			public static readonly GUIContent monoNotSupportediOS11WarningGUIContent;

			public static string undoChangedBundleIdentifierString
			{
				get
				{
					return LocalizationDatabase.GetLocalizedString("Changed macOS bundleIdentifier");
				}
			}

			public static string undoChangedBuildNumberString
			{
				get
				{
					return LocalizationDatabase.GetLocalizedString("Changed macOS build number");
				}
			}

			public static string undoChangedBatchingString
			{
				get
				{
					return LocalizationDatabase.GetLocalizedString("Changed Batching Settings");
				}
			}

			public static string undoChangedIconString
			{
				get
				{
					return LocalizationDatabase.GetLocalizedString("Changed Icon");
				}
			}

			public static string undoChangedGraphicsAPIString
			{
				get
				{
					return LocalizationDatabase.GetLocalizedString("Changed Graphics API Settings");
				}
			}

			static Styles()
			{
				PlayerSettingsEditor.Styles.categoryBox = new GUIStyle(EditorStyles.helpBox);
				PlayerSettingsEditor.Styles.colorSpaceAndroidWarning = EditorGUIUtility.TrTextContent("Linear colorspace requires OpenGL ES 3.0 or Vulkan, uncheck 'Automatic Graphics API' to remove OpenGL ES 2 API, Blit Type must be Always Blit or Auto and 'Minimum API Level' must be at least Android 4.3", null, null);
				PlayerSettingsEditor.Styles.colorSpaceWebGLWarning = EditorGUIUtility.TrTextContent("Linear colorspace requires WebGL 2.0, uncheck 'Automatic Graphics API' to remove WebGL 1.0 API. WARNING: If DXT sRGB is not supported by the browser, texture will be decompressed", null, null);
				PlayerSettingsEditor.Styles.colorSpaceIOSWarning = EditorGUIUtility.TrTextContent("Linear colorspace requires Metal API only. Uncheck 'Automatic Graphics API' and remove OpenGL ES 2 API. Additionally, 'minimum iOS version' set to 8.0 at least", null, null);
				PlayerSettingsEditor.Styles.colorSpaceTVOSWarning = EditorGUIUtility.TrTextContent("Linear colorspace requires Metal API only. Uncheck 'Automatic Graphics API' and remove OpenGL ES 2 API.", null, null);
				PlayerSettingsEditor.Styles.recordingInfo = EditorGUIUtility.TrTextContent("Reordering the list will switch editor to the first available platform", null, null);
				PlayerSettingsEditor.Styles.notApplicableInfo = EditorGUIUtility.TrTextContent("Not applicable for this platform.", null, null);
				PlayerSettingsEditor.Styles.sharedBetweenPlatformsInfo = EditorGUIUtility.TrTextContent("* Shared setting between multiple platforms.", null, null);
				PlayerSettingsEditor.Styles.VRSupportOverridenInfo = EditorGUIUtility.TrTextContent("This setting is overridden by Virtual Reality Support.", null, null);
				PlayerSettingsEditor.Styles.IL2CPPAndroidExperimentalInfo = EditorGUIUtility.TrTextContent("IL2CPP on Android is experimental and unsupported", null, null);
				PlayerSettingsEditor.Styles.cursorHotspot = EditorGUIUtility.TrTextContent("Cursor Hotspot", null, null);
				PlayerSettingsEditor.Styles.defaultCursor = EditorGUIUtility.TrTextContent("Default Cursor", null, null);
				PlayerSettingsEditor.Styles.defaultIcon = EditorGUIUtility.TrTextContent("Default Icon", null, null);
				PlayerSettingsEditor.Styles.vertexChannelCompressionMask = EditorGUIUtility.TrTextContent("Vertex Compression*", "Select which vertex channels should be compressed. Compression can save memory and bandwidth but precision will be lower.", null);
				PlayerSettingsEditor.Styles.iconTitle = EditorGUIUtility.TrTextContent("Icon", null, null);
				PlayerSettingsEditor.Styles.resolutionPresentationTitle = EditorGUIUtility.TrTextContent("Resolution and Presentation", null, null);
				PlayerSettingsEditor.Styles.resolutionTitle = EditorGUIUtility.TrTextContent("Resolution", null, null);
				PlayerSettingsEditor.Styles.orientationTitle = EditorGUIUtility.TrTextContent("Orientation", null, null);
				PlayerSettingsEditor.Styles.allowedOrientationTitle = EditorGUIUtility.TrTextContent("Allowed Orientations for Auto Rotation", null, null);
				PlayerSettingsEditor.Styles.multitaskingSupportTitle = EditorGUIUtility.TrTextContent("Multitasking Support", null, null);
				PlayerSettingsEditor.Styles.statusBarTitle = EditorGUIUtility.TrTextContent("Status Bar", null, null);
				PlayerSettingsEditor.Styles.standalonePlayerOptionsTitle = EditorGUIUtility.TrTextContent("Standalone Player Options", null, null);
				PlayerSettingsEditor.Styles.debuggingCrashReportingTitle = EditorGUIUtility.TrTextContent("Debugging and crash reporting", null, null);
				PlayerSettingsEditor.Styles.debuggingTitle = EditorGUIUtility.TrTextContent("Debugging", null, null);
				PlayerSettingsEditor.Styles.crashReportingTitle = EditorGUIUtility.TrTextContent("Crash Reporting", null, null);
				PlayerSettingsEditor.Styles.otherSettingsTitle = EditorGUIUtility.TrTextContent("Other Settings", null, null);
				PlayerSettingsEditor.Styles.renderingTitle = EditorGUIUtility.TrTextContent("Rendering", null, null);
				PlayerSettingsEditor.Styles.identificationTitle = EditorGUIUtility.TrTextContent("Identification", null, null);
				PlayerSettingsEditor.Styles.macAppStoreTitle = EditorGUIUtility.TrTextContent("Mac App Store Options", null, null);
				PlayerSettingsEditor.Styles.configurationTitle = EditorGUIUtility.TrTextContent("Configuration", null, null);
				PlayerSettingsEditor.Styles.optimizationTitle = EditorGUIUtility.TrTextContent("Optimization", null, null);
				PlayerSettingsEditor.Styles.loggingTitle = EditorGUIUtility.TrTextContent("Logging*", null, null);
				PlayerSettingsEditor.Styles.publishingSettingsTitle = EditorGUIUtility.TrTextContent("Publishing Settings", null, null);
				PlayerSettingsEditor.Styles.bakeCollisionMeshes = EditorGUIUtility.TrTextContent("Prebake Collision Meshes*", "Bake collision data into the meshes on build time", null);
				PlayerSettingsEditor.Styles.keepLoadedShadersAlive = EditorGUIUtility.TrTextContent("Keep Loaded Shaders Alive*", "Prevents shaders from being unloaded", null);
				PlayerSettingsEditor.Styles.preloadedAssets = EditorGUIUtility.TrTextContent("Preloaded Assets*", "Assets to load at start up in the player and kept alive until the player terminates", null);
				PlayerSettingsEditor.Styles.stripEngineCode = EditorGUIUtility.TrTextContent("Strip Engine Code*", "Strip Unused Engine Code - Note that byte code stripping of managed assemblies is always enabled for the IL2CPP scripting backend.", null);
				PlayerSettingsEditor.Styles.iPhoneStrippingLevel = EditorGUIUtility.TrTextContent("Stripping Level*", null, null);
				PlayerSettingsEditor.Styles.iPhoneScriptCallOptimization = EditorGUIUtility.TrTextContent("Script Call Optimization*", null, null);
				PlayerSettingsEditor.Styles.enableInternalProfiler = EditorGUIUtility.TrTextContent("Enable Internal Profiler* (Deprecated)", "Internal profiler counters should be accessed by scripts using UnityEngine.Profiling::Profiler API.", null);
				PlayerSettingsEditor.Styles.stripUnusedMeshComponents = EditorGUIUtility.TrTextContent("Optimize Mesh Data*", "Remove unused mesh components", null);
				PlayerSettingsEditor.Styles.videoMemoryForVertexBuffers = EditorGUIUtility.TrTextContent("Mesh Video Mem*", "How many megabytes of video memory to use for mesh data before we use main memory", null);
				PlayerSettingsEditor.Styles.protectGraphicsMemory = EditorGUIUtility.TrTextContent("Protect Graphics Memory", "Protect GPU memory from being read (on supported devices). Will prevent user from taking screenshots", null);
				PlayerSettingsEditor.Styles.useOSAutoRotation = EditorGUIUtility.TrTextContent("Use Animated Autorotation", "If set OS native animated autorotation method will be used. Otherwise orientation will be changed immediately.", null);
				PlayerSettingsEditor.Styles.UIPrerenderedIcon = EditorGUIUtility.TrTextContent("Prerendered Icon", null, null);
				PlayerSettingsEditor.Styles.defaultScreenWidth = EditorGUIUtility.TrTextContent("Default Screen Width", null, null);
				PlayerSettingsEditor.Styles.defaultScreenHeight = EditorGUIUtility.TrTextContent("Default Screen Height", null, null);
				PlayerSettingsEditor.Styles.macRetinaSupport = EditorGUIUtility.TrTextContent("Mac Retina Support", null, null);
				PlayerSettingsEditor.Styles.runInBackground = EditorGUIUtility.TrTextContent("Run In Background*", null, null);
				PlayerSettingsEditor.Styles.defaultScreenOrientation = EditorGUIUtility.TrTextContent("Default Orientation*", null, null);
				PlayerSettingsEditor.Styles.allowedAutoRotateToPortrait = EditorGUIUtility.TrTextContent("Portrait", null, null);
				PlayerSettingsEditor.Styles.allowedAutoRotateToPortraitUpsideDown = EditorGUIUtility.TrTextContent("Portrait Upside Down", null, null);
				PlayerSettingsEditor.Styles.allowedAutoRotateToLandscapeRight = EditorGUIUtility.TrTextContent("Landscape Right", null, null);
				PlayerSettingsEditor.Styles.allowedAutoRotateToLandscapeLeft = EditorGUIUtility.TrTextContent("Landscape Left", null, null);
				PlayerSettingsEditor.Styles.UIRequiresFullScreen = EditorGUIUtility.TrTextContent("Requires Fullscreen", null, null);
				PlayerSettingsEditor.Styles.UIStatusBarHidden = EditorGUIUtility.TrTextContent("Status Bar Hidden", null, null);
				PlayerSettingsEditor.Styles.UIStatusBarStyle = EditorGUIUtility.TrTextContent("Status Bar Style", null, null);
				PlayerSettingsEditor.Styles.useMacAppStoreValidation = EditorGUIUtility.TrTextContent("Mac App Store Validation", null, null);
				PlayerSettingsEditor.Styles.macAppStoreCategory = EditorGUIUtility.TrTextContent("Category", "'LSApplicationCategoryType'", null);
				PlayerSettingsEditor.Styles.fullscreenMode = EditorGUIUtility.TrTextContent("Fullscreen Mode ", " Not all platforms support all modes", null);
				PlayerSettingsEditor.Styles.exclusiveFullscreen = EditorGUIUtility.TrTextContent("Exclusive Fullscreen", null, null);
				PlayerSettingsEditor.Styles.fullscreenWindow = EditorGUIUtility.TrTextContent("Fullscreen Window", null, null);
				PlayerSettingsEditor.Styles.maximizedWindow = EditorGUIUtility.TrTextContent("Maximized Window", null, null);
				PlayerSettingsEditor.Styles.windowed = EditorGUIUtility.TrTextContent("Windowed", null, null);
				PlayerSettingsEditor.Styles.visibleInBackground = EditorGUIUtility.TrTextContent("Visible In Background", null, null);
				PlayerSettingsEditor.Styles.allowFullscreenSwitch = EditorGUIUtility.TrTextContent("Allow Fullscreen Switch", null, null);
				PlayerSettingsEditor.Styles.use32BitDisplayBuffer = EditorGUIUtility.TrTextContent("Use 32-bit Display Buffer*", "If set Display Buffer will be created to hold 32-bit color values. Use it only if you see banding, as it has performance implications.", null);
				PlayerSettingsEditor.Styles.disableDepthAndStencilBuffers = EditorGUIUtility.TrTextContent("Disable Depth and Stencil*", null, null);
				PlayerSettingsEditor.Styles.iosShowActivityIndicatorOnLoading = EditorGUIUtility.TrTextContent("Show Loading Indicator", null, null);
				PlayerSettingsEditor.Styles.androidShowActivityIndicatorOnLoading = EditorGUIUtility.TrTextContent("Show Loading Indicator", null, null);
				PlayerSettingsEditor.Styles.actionOnDotNetUnhandledException = EditorGUIUtility.TrTextContent("On .Net UnhandledException*", null, null);
				PlayerSettingsEditor.Styles.logObjCUncaughtExceptions = EditorGUIUtility.TrTextContent("Log Obj-C Uncaught Exceptions*", null, null);
				PlayerSettingsEditor.Styles.enableCrashReportAPI = EditorGUIUtility.TrTextContent("Enable CrashReport API*", null, null);
				PlayerSettingsEditor.Styles.activeColorSpace = EditorGUIUtility.TrTextContent("Color Space*", null, null);
				PlayerSettingsEditor.Styles.colorGamut = EditorGUIUtility.TrTextContent("Color Gamut*", null, null);
				PlayerSettingsEditor.Styles.colorGamutForMac = EditorGUIUtility.TrTextContent("Color Gamut For Mac*", null, null);
				PlayerSettingsEditor.Styles.metalForceHardShadows = EditorGUIUtility.TrTextContent("Force hard shadows on Metal*", null, null);
				PlayerSettingsEditor.Styles.metalEditorSupport = EditorGUIUtility.TextContent("Metal Editor Support*");
				PlayerSettingsEditor.Styles.metalAPIValidation = EditorGUIUtility.TrTextContent("Metal API Validation*", null, null);
				PlayerSettingsEditor.Styles.metalFramebufferOnly = EditorGUIUtility.TrTextContent("Metal Restricted Backbuffer Use", "Set framebufferOnly flag on backbuffer. This prevents readback from backbuffer but enables some driver optimizations.", null);
				PlayerSettingsEditor.Styles.mTRendering = EditorGUIUtility.TrTextContent("Multithreaded Rendering*", null, null);
				PlayerSettingsEditor.Styles.staticBatching = EditorGUIUtility.TrTextContent("Static Batching", null, null);
				PlayerSettingsEditor.Styles.dynamicBatching = EditorGUIUtility.TrTextContent("Dynamic Batching", null, null);
				PlayerSettingsEditor.Styles.graphicsJobs = EditorGUIUtility.TrTextContent("Graphics Jobs (Experimental)*", null, null);
				PlayerSettingsEditor.Styles.graphicsJobsMode = EditorGUIUtility.TrTextContent("Graphics Jobs Mode*", null, null);
				PlayerSettingsEditor.Styles.applicationBuildNumber = EditorGUIUtility.TrTextContent("Build", null, null);
				PlayerSettingsEditor.Styles.appleDeveloperTeamID = EditorGUIUtility.TrTextContent("iOS Developer Team ID", "Developers can retrieve their Team ID by visiting the Apple Developer site under Account > Membership.", null);
				PlayerSettingsEditor.Styles.useOnDemandResources = EditorGUIUtility.TrTextContent("Use on demand resources*", null, null);
				PlayerSettingsEditor.Styles.accelerometerFrequency = EditorGUIUtility.TrTextContent("Accelerometer Frequency*", null, null);
				PlayerSettingsEditor.Styles.cameraUsageDescription = EditorGUIUtility.TrTextContent("Camera Usage Description*", null, null);
				PlayerSettingsEditor.Styles.locationUsageDescription = EditorGUIUtility.TrTextContent("Location Usage Description*", null, null);
				PlayerSettingsEditor.Styles.microphoneUsageDescription = EditorGUIUtility.TrTextContent("Microphone Usage Description*", null, null);
				PlayerSettingsEditor.Styles.muteOtherAudioSources = EditorGUIUtility.TrTextContent("Mute Other Audio Sources*", null, null);
				PlayerSettingsEditor.Styles.prepareIOSForRecording = EditorGUIUtility.TrTextContent("Prepare iOS for Recording", null, null);
				PlayerSettingsEditor.Styles.forceIOSSpeakersWhenRecording = EditorGUIUtility.TrTextContent("Force iOS Speakers when Recording", null, null);
				PlayerSettingsEditor.Styles.UIRequiresPersistentWiFi = EditorGUIUtility.TrTextContent("Requires Persistent WiFi*", null, null);
				PlayerSettingsEditor.Styles.iOSAllowHTTPDownload = EditorGUIUtility.TrTextContent("Allow downloads over HTTP (nonsecure)*", null, null);
				PlayerSettingsEditor.Styles.iOSURLSchemes = EditorGUIUtility.TrTextContent("Supported URL schemes*", null, null);
				PlayerSettingsEditor.Styles.aotOptions = EditorGUIUtility.TrTextContent("AOT Compilation Options*", null, null);
				PlayerSettingsEditor.Styles.require31 = EditorGUIUtility.TrTextContent("Require ES3.1", null, null);
				PlayerSettingsEditor.Styles.requireAEP = EditorGUIUtility.TrTextContent("Require ES3.1+AEP", null, null);
				PlayerSettingsEditor.Styles.skinOnGPU = EditorGUIUtility.TrTextContent("GPU Skinning*", "Use DX11/ES3 GPU Skinning", null);
				PlayerSettingsEditor.Styles.skinOnGPUPS4 = EditorGUIUtility.TrTextContent("Compute Skinning*", "Use Compute pipeline for Skinning", null);
				PlayerSettingsEditor.Styles.skinOnGPUAndroidWarning = EditorGUIUtility.TrTextContent("GPU skinning on Android devices is only enabled in VR builds, and is experimental. Be sure to validate behavior and performance on your target devices.", null, null);
				PlayerSettingsEditor.Styles.disableStatistics = EditorGUIUtility.TrTextContent("Disable HW Statistics*", "Disables HW Statistics (Pro Only)", null);
				PlayerSettingsEditor.Styles.scriptingDefineSymbols = EditorGUIUtility.TrTextContent("Scripting Define Symbols*", null, null);
				PlayerSettingsEditor.Styles.scriptingRuntimeVersion = EditorGUIUtility.TrTextContent("Scripting Runtime Version*", "The scripting runtime version to be used. Unity uses different scripting backends based on platform, so these options are listed as equivalent expected behavior.", null);
				PlayerSettingsEditor.Styles.scriptingRuntimeVersionLegacy = EditorGUIUtility.TrTextContent("Stable (.NET 3.5 Equivalent)", null, null);
				PlayerSettingsEditor.Styles.scriptingRuntimeVersionLatest = EditorGUIUtility.TrTextContent("Experimental (.NET 4.6 Equivalent)", null, null);
				PlayerSettingsEditor.Styles.scriptingBackend = EditorGUIUtility.TrTextContent("Scripting Backend", null, null);
				PlayerSettingsEditor.Styles.il2cppCompilerConfiguration = EditorGUIUtility.TrTextContent("C++ Compiler Configuration", null, null);
				PlayerSettingsEditor.Styles.scriptingMono2x = EditorGUIUtility.TrTextContent("Mono", null, null);
				PlayerSettingsEditor.Styles.scriptingWinRTDotNET = EditorGUIUtility.TrTextContent(".NET", null, null);
				PlayerSettingsEditor.Styles.scriptingIL2CPP = EditorGUIUtility.TrTextContent("IL2CPP", null, null);
				PlayerSettingsEditor.Styles.scriptingDefault = EditorGUIUtility.TrTextContent("Default", null, null);
				PlayerSettingsEditor.Styles.apiCompatibilityLevel = EditorGUIUtility.TrTextContent("Api Compatibility Level*", null, null);
				PlayerSettingsEditor.Styles.apiCompatibilityLevel_NET_2_0 = EditorGUIUtility.TrTextContent(".NET 2.0", null, null);
				PlayerSettingsEditor.Styles.apiCompatibilityLevel_NET_2_0_Subset = EditorGUIUtility.TrTextContent(".NET 2.0 Subset", null, null);
				PlayerSettingsEditor.Styles.apiCompatibilityLevel_NET_4_6 = EditorGUIUtility.TrTextContent(".NET 4.6", null, null);
				PlayerSettingsEditor.Styles.apiCompatibilityLevel_NET_Standard_2_0 = EditorGUIUtility.TrTextContent(".NET Standard 2.0", null, null);
				PlayerSettingsEditor.Styles.activeInputHandling = EditorGUIUtility.TrTextContent("Active Input Handling*", null, null);
				PlayerSettingsEditor.Styles.activeInputHandlingOptions = new GUIContent[]
				{
					EditorGUIUtility.TrTextContent("Input Manager", null, null),
					EditorGUIUtility.TrTextContent("Input System (Preview)", null, null),
					EditorGUIUtility.TrTextContent("Both", null, null)
				};
				PlayerSettingsEditor.Styles.vrSettingsMoved = EditorGUIUtility.TrTextContent("Virtual Reality moved to XR Settings", null, null);
				PlayerSettingsEditor.Styles.lightmapEncodingLabel = EditorGUIUtility.TrTextContent("Lightmap Encoding", "Affects the encoding scheme and compression format of the lightmaps.", null);
				PlayerSettingsEditor.Styles.lightmapEncodingNames = new GUIContent[]
				{
					EditorGUIUtility.TrTextContent("Normal Quality", null, null),
					EditorGUIUtility.TrTextContent("High Quality", null, null)
				};
				PlayerSettingsEditor.Styles.monoNotSupportediOS11WarningGUIContent = EditorGUIUtility.TrTextContent("Mono is not supported on iOS11 and above.", null, null);
				PlayerSettingsEditor.Styles.categoryBox.padding.left = 14;
			}
		}

		private struct ChangeGraphicsApiAction
		{
			public readonly bool changeList;

			public readonly bool reloadGfx;

			public ChangeGraphicsApiAction(bool doChange, bool doReload)
			{
				this.changeList = doChange;
				this.reloadGfx = doReload;
			}
		}

		[Serializable]
		private struct HwStatsServiceState
		{
			public bool hwstats;
		}

		private const int kSlotSize = 64;

		private const int kMaxPreviewSize = 96;

		private const int kIconSpacing = 6;

		private PlayerSettingsSplashScreenEditor m_SplashScreenEditor;

		private static GraphicsJobMode[] m_GfxJobModeValues = new GraphicsJobMode[]
		{
			GraphicsJobMode.Native,
			GraphicsJobMode.Legacy
		};

		private static GUIContent[] m_GfxJobModeNames = new GUIContent[]
		{
			EditorGUIUtility.TrTextContent("Native", null, null),
			EditorGUIUtility.TrTextContent("Legacy", null, null)
		};

		private SavedInt m_SelectedSection = new SavedInt("PlayerSettings.ShownSection", -1);

		private BuildPlatform[] validPlatforms;

		private SerializedProperty m_StripEngineCode;

		private SerializedProperty m_ApplicationBundleVersion;

		private SerializedProperty m_UseMacAppStoreValidation;

		private SerializedProperty m_MacAppStoreCategory;

		private SerializedProperty m_IPhoneApplicationDisplayName;

		private SerializedProperty m_CameraUsageDescription;

		private SerializedProperty m_LocationUsageDescription;

		private SerializedProperty m_MicrophoneUsageDescription;

		private SerializedProperty m_IPhoneStrippingLevel;

		private SerializedProperty m_IPhoneScriptCallOptimization;

		private SerializedProperty m_AotOptions;

		private SerializedProperty m_DefaultScreenOrientation;

		private SerializedProperty m_AllowedAutoRotateToPortrait;

		private SerializedProperty m_AllowedAutoRotateToPortraitUpsideDown;

		private SerializedProperty m_AllowedAutoRotateToLandscapeRight;

		private SerializedProperty m_AllowedAutoRotateToLandscapeLeft;

		private SerializedProperty m_UseOSAutoRotation;

		private SerializedProperty m_Use32BitDisplayBuffer;

		private SerializedProperty m_DisableDepthAndStencilBuffers;

		private SerializedProperty m_iosShowActivityIndicatorOnLoading;

		private SerializedProperty m_androidShowActivityIndicatorOnLoading;

		private SerializedProperty m_tizenShowActivityIndicatorOnLoading;

		private SerializedProperty m_AndroidProfiler;

		private SerializedProperty m_UIPrerenderedIcon;

		private SerializedProperty m_UIRequiresPersistentWiFi;

		private SerializedProperty m_UIStatusBarHidden;

		private SerializedProperty m_UIRequiresFullScreen;

		private SerializedProperty m_UIStatusBarStyle;

		private SerializedProperty m_IOSAllowHTTPDownload;

		private SerializedProperty m_SubmitAnalytics;

		private SerializedProperty m_IOSURLSchemes;

		private SerializedProperty m_AccelerometerFrequency;

		private SerializedProperty m_useOnDemandResources;

		private SerializedProperty m_MuteOtherAudioSources;

		private SerializedProperty m_PrepareIOSForRecording;

		private SerializedProperty m_ForceIOSSpeakersWhenRecording;

		private SerializedProperty m_EnableInternalProfiler;

		private SerializedProperty m_ActionOnDotNetUnhandledException;

		private SerializedProperty m_LogObjCUncaughtExceptions;

		private SerializedProperty m_EnableCrashReportAPI;

		private SerializedProperty m_EnableInputSystem;

		private SerializedProperty m_DisableInputManager;

		private SerializedProperty m_VideoMemoryForVertexBuffers;

		private SerializedProperty m_CompanyName;

		private SerializedProperty m_ProductName;

		private SerializedProperty m_DefaultCursor;

		private SerializedProperty m_CursorHotspot;

		private SerializedProperty m_DefaultScreenWidth;

		private SerializedProperty m_DefaultScreenHeight;

		private SerializedProperty m_ActiveColorSpace;

		private SerializedProperty m_StripUnusedMeshComponents;

		private SerializedProperty m_VertexChannelCompressionMask;

		private SerializedProperty m_MetalEditorSupport;

		private SerializedProperty m_MetalAPIValidation;

		private SerializedProperty m_MetalFramebufferOnly;

		private SerializedProperty m_MetalForceHardShadows;

		private SerializedProperty m_DisplayResolutionDialog;

		private SerializedProperty m_DefaultIsNativeResolution;

		private SerializedProperty m_MacRetinaSupport;

		private SerializedProperty m_UsePlayerLog;

		private SerializedProperty m_KeepLoadedShadersAlive;

		private SerializedProperty m_PreloadedAssets;

		private SerializedProperty m_BakeCollisionMeshes;

		private SerializedProperty m_ResizableWindow;

		private SerializedProperty m_FullscreenMode;

		private SerializedProperty m_VisibleInBackground;

		private SerializedProperty m_AllowFullscreenSwitch;

		private SerializedProperty m_ForceSingleInstance;

		private SerializedProperty m_RunInBackground;

		private SerializedProperty m_CaptureSingleScreen;

		private SerializedProperty m_SupportedAspectRatios;

		private SerializedProperty m_SkinOnGPU;

		private SerializedProperty m_GraphicsJobs;

		private SerializedProperty m_RequireES31;

		private SerializedProperty m_RequireES31AEP;

		private SerializedProperty m_LightmapEncodingQuality;

		private string m_LocalizedTargetName;

		private static Dictionary<BuildTarget, ReorderableList> s_GraphicsDeviceLists = new Dictionary<BuildTarget, ReorderableList>();

		private static ReorderableList s_ColorGamutList;

		public PlayerSettingsEditorVR m_VRSettings;

		private int selectedPlatform = 0;

		private int scriptingDefinesControlID = 0;

		private ISettingEditorExtension[] m_SettingsExtensions;

		private const int kNumberGUISections = 7;

		private AnimBool[] m_SectionAnimators = new AnimBool[7];

		private readonly AnimBool m_ShowDefaultIsNativeResolution = new AnimBool();

		private readonly AnimBool m_ShowResolution = new AnimBool();

		private static Texture2D s_WarningIcon;

		private static Dictionary<BuildTargetGroup, List<ColorGamut>> s_SupportedColorGamuts = new Dictionary<BuildTargetGroup, List<ColorGamut>>
		{
			{
				BuildTargetGroup.Standalone,
				new List<ColorGamut>
				{
					ColorGamut.sRGB,
					ColorGamut.DisplayP3
				}
			},
			{
				BuildTargetGroup.iPhone,
				new List<ColorGamut>
				{
					ColorGamut.sRGB,
					ColorGamut.DisplayP3
				}
			}
		};

		private static ApiCompatibilityLevel[] only_4_x_profiles = new ApiCompatibilityLevel[]
		{
			ApiCompatibilityLevel.NET_4_6,
			ApiCompatibilityLevel.NET_Standard_2_0
		};

		private static ApiCompatibilityLevel[] only_2_0_profiles = new ApiCompatibilityLevel[]
		{
			ApiCompatibilityLevel.NET_2_0,
			ApiCompatibilityLevel.NET_2_0_Subset
		};

		private static ApiCompatibilityLevel[] allProfiles = new ApiCompatibilityLevel[]
		{
			ApiCompatibilityLevel.NET_2_0,
			ApiCompatibilityLevel.NET_2_0_Subset,
			ApiCompatibilityLevel.NET_4_6,
			ApiCompatibilityLevel.NET_Standard_2_0
		};

		private static Il2CppCompilerConfiguration[] m_Il2cppCompilerConfigurations;

		private static GUIContent[] m_Il2cppCompilerConfigurationNames;

		private static Dictionary<ScriptingImplementation, GUIContent> m_NiceScriptingBackendNames;

		private static Dictionary<ApiCompatibilityLevel, GUIContent> m_NiceApiCompatibilityLevelNames;

		private PlayerSettingsSplashScreenEditor splashScreenEditor
		{
			get
			{
				if (this.m_SplashScreenEditor == null)
				{
					this.m_SplashScreenEditor = new PlayerSettingsSplashScreenEditor(this);
				}
				return this.m_SplashScreenEditor;
			}
		}

		internal override string targetTitle
		{
			get
			{
				if (this.m_LocalizedTargetName == null)
				{
					this.m_LocalizedTargetName = L10n.Tr(base.target.name);
				}
				return this.m_LocalizedTargetName;
			}
		}

		public static void SyncPlatformAPIsList(BuildTarget target)
		{
			if (PlayerSettingsEditor.s_GraphicsDeviceLists.ContainsKey(target))
			{
				PlayerSettingsEditor.s_GraphicsDeviceLists[target].list = PlayerSettings.GetGraphicsAPIs(target).ToList<GraphicsDeviceType>();
			}
		}

		public static void SyncColorGamuts()
		{
			PlayerSettingsEditor.s_ColorGamutList.list = PlayerSettings.GetColorGamuts().ToList<ColorGamut>();
		}

		public bool IsMobileTarget(BuildTargetGroup targetGroup)
		{
			return targetGroup == BuildTargetGroup.iPhone || targetGroup == BuildTargetGroup.tvOS || targetGroup == BuildTargetGroup.Android || targetGroup == BuildTargetGroup.Tizen;
		}

		public SerializedProperty FindPropertyAssert(string name)
		{
			SerializedProperty serializedProperty = base.serializedObject.FindProperty(name);
			if (serializedProperty == null)
			{
				Debug.LogError("Failed to find:" + name);
			}
			return serializedProperty;
		}

		private void OnEnable()
		{
			this.validPlatforms = BuildPlatforms.instance.GetValidPlatforms(true).ToArray();
			this.m_IPhoneStrippingLevel = this.FindPropertyAssert("iPhoneStrippingLevel");
			this.m_StripEngineCode = this.FindPropertyAssert("stripEngineCode");
			this.m_IPhoneScriptCallOptimization = this.FindPropertyAssert("iPhoneScriptCallOptimization");
			this.m_AndroidProfiler = this.FindPropertyAssert("AndroidProfiler");
			this.m_CompanyName = this.FindPropertyAssert("companyName");
			this.m_ProductName = this.FindPropertyAssert("productName");
			this.m_DefaultCursor = this.FindPropertyAssert("defaultCursor");
			this.m_CursorHotspot = this.FindPropertyAssert("cursorHotspot");
			this.m_UIPrerenderedIcon = this.FindPropertyAssert("uIPrerenderedIcon");
			this.m_UIRequiresFullScreen = this.FindPropertyAssert("uIRequiresFullScreen");
			this.m_UIStatusBarHidden = this.FindPropertyAssert("uIStatusBarHidden");
			this.m_UIStatusBarStyle = this.FindPropertyAssert("uIStatusBarStyle");
			this.m_ActiveColorSpace = this.FindPropertyAssert("m_ActiveColorSpace");
			this.m_StripUnusedMeshComponents = this.FindPropertyAssert("StripUnusedMeshComponents");
			this.m_VertexChannelCompressionMask = this.FindPropertyAssert("VertexChannelCompressionMask");
			this.m_MetalEditorSupport = this.FindPropertyAssert("metalEditorSupport");
			this.m_MetalAPIValidation = this.FindPropertyAssert("metalAPIValidation");
			this.m_MetalFramebufferOnly = this.FindPropertyAssert("metalFramebufferOnly");
			this.m_MetalForceHardShadows = this.FindPropertyAssert("iOSMetalForceHardShadows");
			this.m_ApplicationBundleVersion = base.serializedObject.FindProperty("bundleVersion");
			if (this.m_ApplicationBundleVersion == null)
			{
				this.m_ApplicationBundleVersion = this.FindPropertyAssert("iPhoneBundleVersion");
			}
			this.m_useOnDemandResources = this.FindPropertyAssert("useOnDemandResources");
			this.m_AccelerometerFrequency = this.FindPropertyAssert("accelerometerFrequency");
			this.m_MuteOtherAudioSources = this.FindPropertyAssert("muteOtherAudioSources");
			this.m_PrepareIOSForRecording = this.FindPropertyAssert("Prepare IOS For Recording");
			this.m_ForceIOSSpeakersWhenRecording = this.FindPropertyAssert("Force IOS Speakers When Recording");
			this.m_UIRequiresPersistentWiFi = this.FindPropertyAssert("uIRequiresPersistentWiFi");
			this.m_IOSAllowHTTPDownload = this.FindPropertyAssert("iosAllowHTTPDownload");
			this.m_SubmitAnalytics = this.FindPropertyAssert("submitAnalytics");
			this.m_IOSURLSchemes = this.FindPropertyAssert("iOSURLSchemes");
			this.m_AotOptions = this.FindPropertyAssert("aotOptions");
			this.m_CameraUsageDescription = this.FindPropertyAssert("cameraUsageDescription");
			this.m_LocationUsageDescription = this.FindPropertyAssert("locationUsageDescription");
			this.m_MicrophoneUsageDescription = this.FindPropertyAssert("microphoneUsageDescription");
			this.m_EnableInternalProfiler = this.FindPropertyAssert("enableInternalProfiler");
			this.m_ActionOnDotNetUnhandledException = this.FindPropertyAssert("actionOnDotNetUnhandledException");
			this.m_LogObjCUncaughtExceptions = this.FindPropertyAssert("logObjCUncaughtExceptions");
			this.m_EnableCrashReportAPI = this.FindPropertyAssert("enableCrashReportAPI");
			this.m_EnableInputSystem = this.FindPropertyAssert("enableNativePlatformBackendsForNewInputSystem");
			this.m_DisableInputManager = this.FindPropertyAssert("disableOldInputManagerSupport");
			this.m_DefaultScreenWidth = this.FindPropertyAssert("defaultScreenWidth");
			this.m_DefaultScreenHeight = this.FindPropertyAssert("defaultScreenHeight");
			this.m_RunInBackground = this.FindPropertyAssert("runInBackground");
			this.m_DefaultScreenOrientation = this.FindPropertyAssert("defaultScreenOrientation");
			this.m_AllowedAutoRotateToPortrait = this.FindPropertyAssert("allowedAutorotateToPortrait");
			this.m_AllowedAutoRotateToPortraitUpsideDown = this.FindPropertyAssert("allowedAutorotateToPortraitUpsideDown");
			this.m_AllowedAutoRotateToLandscapeRight = this.FindPropertyAssert("allowedAutorotateToLandscapeRight");
			this.m_AllowedAutoRotateToLandscapeLeft = this.FindPropertyAssert("allowedAutorotateToLandscapeLeft");
			this.m_UseOSAutoRotation = this.FindPropertyAssert("useOSAutorotation");
			this.m_Use32BitDisplayBuffer = this.FindPropertyAssert("use32BitDisplayBuffer");
			this.m_DisableDepthAndStencilBuffers = this.FindPropertyAssert("disableDepthAndStencilBuffers");
			this.m_iosShowActivityIndicatorOnLoading = this.FindPropertyAssert("iosShowActivityIndicatorOnLoading");
			this.m_androidShowActivityIndicatorOnLoading = this.FindPropertyAssert("androidShowActivityIndicatorOnLoading");
			this.m_tizenShowActivityIndicatorOnLoading = this.FindPropertyAssert("tizenShowActivityIndicatorOnLoading");
			this.m_DefaultIsNativeResolution = this.FindPropertyAssert("defaultIsNativeResolution");
			this.m_MacRetinaSupport = this.FindPropertyAssert("macRetinaSupport");
			this.m_CaptureSingleScreen = this.FindPropertyAssert("captureSingleScreen");
			this.m_DisplayResolutionDialog = this.FindPropertyAssert("displayResolutionDialog");
			this.m_SupportedAspectRatios = this.FindPropertyAssert("m_SupportedAspectRatios");
			this.m_UsePlayerLog = this.FindPropertyAssert("usePlayerLog");
			this.m_KeepLoadedShadersAlive = this.FindPropertyAssert("keepLoadedShadersAlive");
			this.m_PreloadedAssets = this.FindPropertyAssert("preloadedAssets");
			this.m_BakeCollisionMeshes = this.FindPropertyAssert("bakeCollisionMeshes");
			this.m_ResizableWindow = this.FindPropertyAssert("resizableWindow");
			this.m_UseMacAppStoreValidation = this.FindPropertyAssert("useMacAppStoreValidation");
			this.m_MacAppStoreCategory = this.FindPropertyAssert("macAppStoreCategory");
			this.m_FullscreenMode = this.FindPropertyAssert("fullscreenMode");
			this.m_VisibleInBackground = this.FindPropertyAssert("visibleInBackground");
			this.m_AllowFullscreenSwitch = this.FindPropertyAssert("allowFullscreenSwitch");
			this.m_SkinOnGPU = this.FindPropertyAssert("gpuSkinning");
			this.m_GraphicsJobs = this.FindPropertyAssert("graphicsJobs");
			this.m_ForceSingleInstance = this.FindPropertyAssert("forceSingleInstance");
			this.m_RequireES31 = this.FindPropertyAssert("openGLRequireES31");
			this.m_RequireES31AEP = this.FindPropertyAssert("openGLRequireES31AEP");
			this.m_VideoMemoryForVertexBuffers = this.FindPropertyAssert("videoMemoryForVertexBuffers");
			this.m_SettingsExtensions = new ISettingEditorExtension[this.validPlatforms.Length];
			for (int i = 0; i < this.validPlatforms.Length; i++)
			{
				string targetStringFromBuildTargetGroup = ModuleManager.GetTargetStringFromBuildTargetGroup(this.validPlatforms[i].targetGroup);
				this.m_SettingsExtensions[i] = ModuleManager.GetEditorSettingsExtension(targetStringFromBuildTargetGroup);
				if (this.m_SettingsExtensions[i] != null)
				{
					this.m_SettingsExtensions[i].OnEnable(this);
				}
			}
			for (int j = 0; j < this.m_SectionAnimators.Length; j++)
			{
				this.m_SectionAnimators[j] = new AnimBool(this.m_SelectedSection.value == j, new UnityAction(base.Repaint));
			}
			this.m_ShowDefaultIsNativeResolution.valueChanged.AddListener(new UnityAction(base.Repaint));
			this.m_ShowResolution.valueChanged.AddListener(new UnityAction(base.Repaint));
			this.m_VRSettings = new PlayerSettingsEditorVR(this);
			this.splashScreenEditor.OnEnable();
			PlayerSettingsEditor.s_GraphicsDeviceLists.Clear();
		}

		public override bool UseDefaultMargins()
		{
			return false;
		}

		public override void OnInspectorGUI()
		{
			base.serializedObject.Update();
			EditorGUILayout.BeginVertical(EditorStyles.inspectorDefaultMargins, new GUILayoutOption[0]);
			this.CommonSettings();
			EditorGUILayout.EndVertical();
			EditorGUILayout.Space();
			EditorGUI.BeginChangeCheck();
			int num = this.selectedPlatform;
			this.selectedPlatform = EditorGUILayout.BeginPlatformGrouping(this.validPlatforms, null);
			if (EditorGUI.EndChangeCheck())
			{
				if (EditorGUI.s_DelayedTextEditor.IsEditingControl(this.scriptingDefinesControlID))
				{
					EditorGUI.EndEditingActiveTextField();
					GUIUtility.keyboardControl = 0;
					PlayerSettings.SetScriptingDefineSymbolsForGroup(this.validPlatforms[num].targetGroup, EditorGUI.s_DelayedTextEditor.text);
				}
				GUI.FocusControl("");
			}
			GUILayout.Label(string.Format(L10n.Tr("Settings for {0}"), this.validPlatforms[this.selectedPlatform].title.text), new GUILayoutOption[0]);
			EditorGUIUtility.labelWidth = Mathf.Max(150f, EditorGUIUtility.labelWidth - 8f);
			BuildPlatform buildPlatform = this.validPlatforms[this.selectedPlatform];
			BuildTargetGroup targetGroup = buildPlatform.targetGroup;
			int num2 = 0;
			this.IconSectionGUI(targetGroup, this.m_SettingsExtensions[this.selectedPlatform], num2++);
			this.ResolutionSectionGUI(targetGroup, this.m_SettingsExtensions[this.selectedPlatform], num2++);
			this.m_SplashScreenEditor.SplashSectionGUI(buildPlatform, targetGroup, this.m_SettingsExtensions[this.selectedPlatform], num2++);
			this.DebugAndCrashReportingGUI(buildPlatform, targetGroup, this.m_SettingsExtensions[this.selectedPlatform], num2++);
			this.OtherSectionGUI(buildPlatform, targetGroup, this.m_SettingsExtensions[this.selectedPlatform], num2++);
			this.PublishSectionGUI(targetGroup, this.m_SettingsExtensions[this.selectedPlatform], num2++);
			this.m_VRSettings.XRSectionGUI(targetGroup, num2++);
			if (num2 != 7)
			{
				Debug.LogError("Mismatched number of GUI sections.");
			}
			EditorGUILayout.EndPlatformGrouping();
			base.serializedObject.ApplyModifiedProperties();
		}

		private void CommonSettings()
		{
			EditorGUILayout.PropertyField(this.m_CompanyName, new GUILayoutOption[0]);
			EditorGUILayout.PropertyField(this.m_ProductName, new GUILayoutOption[0]);
			EditorGUILayout.Space();
			GUI.changed = false;
			string platform = "";
			Texture2D[] array = PlayerSettings.GetAllIconsForPlatform(platform);
			int[] iconWidthsOfAllKindsForPlatform = PlayerSettings.GetIconWidthsOfAllKindsForPlatform(platform);
			if (array.Length != iconWidthsOfAllKindsForPlatform.Length)
			{
				array = new Texture2D[iconWidthsOfAllKindsForPlatform.Length];
			}
			array[0] = (Texture2D)EditorGUILayout.ObjectField(PlayerSettingsEditor.Styles.defaultIcon, array[0], typeof(Texture2D), false, new GUILayoutOption[0]);
			if (GUI.changed)
			{
				Undo.RecordObject(base.target, PlayerSettingsEditor.Styles.undoChangedIconString);
				PlayerSettings.SetIconsForPlatform(platform, array);
			}
			GUILayout.Space(3f);
			Rect controlRect = EditorGUILayout.GetControlRect(true, 64f, new GUILayoutOption[0]);
			EditorGUI.BeginProperty(controlRect, PlayerSettingsEditor.Styles.defaultCursor, this.m_DefaultCursor);
			this.m_DefaultCursor.objectReferenceValue = EditorGUI.ObjectField(controlRect, PlayerSettingsEditor.Styles.defaultCursor, this.m_DefaultCursor.objectReferenceValue, typeof(Texture2D), false);
			EditorGUI.EndProperty();
			Rect rect = EditorGUILayout.GetControlRect(new GUILayoutOption[0]);
			rect = EditorGUI.PrefixLabel(rect, 0, PlayerSettingsEditor.Styles.cursorHotspot);
			EditorGUI.PropertyField(rect, this.m_CursorHotspot, GUIContent.none);
		}

		public bool BeginSettingsBox(int nr, GUIContent header)
		{
			bool enabled = GUI.enabled;
			GUI.enabled = true;
			EditorGUILayout.BeginVertical(PlayerSettingsEditor.Styles.categoryBox, new GUILayoutOption[0]);
			Rect rect = GUILayoutUtility.GetRect(20f, 18f);
			rect.x += 3f;
			rect.width += 6f;
			EditorGUI.BeginChangeCheck();
			bool flag = GUI.Toggle(rect, this.m_SelectedSection.value == nr, header, EditorStyles.inspectorTitlebarText);
			if (EditorGUI.EndChangeCheck())
			{
				this.m_SelectedSection.value = ((!flag) ? -1 : nr);
				GUIUtility.keyboardControl = 0;
			}
			this.m_SectionAnimators[nr].target = flag;
			GUI.enabled = enabled;
			return EditorGUILayout.BeginFadeGroup(this.m_SectionAnimators[nr].faded);
		}

		public void EndSettingsBox()
		{
			EditorGUILayout.EndFadeGroup();
			EditorGUILayout.EndVertical();
		}

		private void ShowNoSettings()
		{
			GUILayout.Label(PlayerSettingsEditor.Styles.notApplicableInfo, EditorStyles.miniLabel, new GUILayoutOption[0]);
		}

		public void ShowSharedNote()
		{
			GUILayout.Label(PlayerSettingsEditor.Styles.sharedBetweenPlatformsInfo, EditorStyles.miniLabel, new GUILayoutOption[0]);
		}

		private void IconSectionGUI(BuildTargetGroup targetGroup, ISettingEditorExtension settingsExtension, int sectionIndex)
		{
			if (this.BeginSettingsBox(sectionIndex, PlayerSettingsEditor.Styles.iconTitle))
			{
				bool flag = true;
				if (settingsExtension != null)
				{
					flag = settingsExtension.UsesStandardIcons();
				}
				if (flag)
				{
					bool flag2 = this.selectedPlatform < 0;
					BuildPlatform buildPlatform = null;
					targetGroup = BuildTargetGroup.Standalone;
					string platform = "";
					if (!flag2)
					{
						buildPlatform = this.validPlatforms[this.selectedPlatform];
						targetGroup = buildPlatform.targetGroup;
						platform = buildPlatform.name;
					}
					bool enabled = GUI.enabled;
					if (targetGroup == BuildTargetGroup.WebGL)
					{
						this.ShowNoSettings();
						EditorGUILayout.Space();
					}
					else if (targetGroup != BuildTargetGroup.WSA)
					{
						Texture2D[] array = PlayerSettings.GetAllIconsForPlatform(platform);
						int[] iconWidthsOfAllKindsForPlatform = PlayerSettings.GetIconWidthsOfAllKindsForPlatform(platform);
						int[] iconHeightsOfAllKindsForPlatform = PlayerSettings.GetIconHeightsOfAllKindsForPlatform(platform);
						IconKind[] iconKindsForPlatform = PlayerSettings.GetIconKindsForPlatform(platform);
						bool flag3 = true;
						if (!flag2)
						{
							GUI.changed = false;
							flag3 = (array.Length == iconWidthsOfAllKindsForPlatform.Length);
							flag3 = GUILayout.Toggle(flag3, string.Format(L10n.Tr("Override for {0}"), buildPlatform.title.text), new GUILayoutOption[0]);
							GUI.enabled = (enabled && flag3);
							if (GUI.changed || (!flag3 && array.Length > 0))
							{
								if (flag3)
								{
									array = new Texture2D[iconWidthsOfAllKindsForPlatform.Length];
								}
								else
								{
									array = new Texture2D[0];
								}
								if (GUI.changed)
								{
									PlayerSettings.SetIconsForPlatform(platform, array);
								}
							}
						}
						GUI.changed = false;
						for (int i = 0; i < iconWidthsOfAllKindsForPlatform.Length; i++)
						{
							int num = Mathf.Min(96, iconWidthsOfAllKindsForPlatform[i]);
							int num2 = (int)((float)iconHeightsOfAllKindsForPlatform[i] * (float)num / (float)iconWidthsOfAllKindsForPlatform[i]);
							if (targetGroup == BuildTargetGroup.iPhone)
							{
								if (iconKindsForPlatform[i] == IconKind.Spotlight && iconKindsForPlatform[i - 1] != IconKind.Spotlight)
								{
									Rect rect = GUILayoutUtility.GetRect(EditorGUIUtility.labelWidth, 20f);
									GUI.Label(new Rect(rect.x, rect.y, EditorGUIUtility.labelWidth, 20f), "Spotlight icons", EditorStyles.boldLabel);
								}
								if (iconKindsForPlatform[i] == IconKind.Settings && iconKindsForPlatform[i - 1] != IconKind.Settings)
								{
									Rect rect2 = GUILayoutUtility.GetRect(EditorGUIUtility.labelWidth, 20f);
									GUI.Label(new Rect(rect2.x, rect2.y, EditorGUIUtility.labelWidth, 20f), "Settings icons", EditorStyles.boldLabel);
								}
								if (iconKindsForPlatform[i] == IconKind.Notification && iconKindsForPlatform[i - 1] != IconKind.Notification)
								{
									Rect rect3 = GUILayoutUtility.GetRect(EditorGUIUtility.labelWidth, 20f);
									GUI.Label(new Rect(rect3.x, rect3.y, EditorGUIUtility.labelWidth, 20f), "Notification icons", EditorStyles.boldLabel);
								}
								if (iconKindsForPlatform[i] == IconKind.Store && iconKindsForPlatform[i - 1] != IconKind.Store)
								{
									Rect rect4 = GUILayoutUtility.GetRect(EditorGUIUtility.labelWidth, 20f);
									GUI.Label(new Rect(rect4.x, rect4.y, EditorGUIUtility.labelWidth, 20f), "App Store icons", EditorStyles.boldLabel);
								}
							}
							Rect rect5 = GUILayoutUtility.GetRect(64f, (float)(Mathf.Max(64, num2) + 6));
							float num3 = Mathf.Min(rect5.width, EditorGUIUtility.labelWidth + 4f + 64f + 6f + 96f);
							string text = iconWidthsOfAllKindsForPlatform[i] + "x" + iconHeightsOfAllKindsForPlatform[i];
							GUI.Label(new Rect(rect5.x, rect5.y, num3 - 96f - 64f - 12f, 20f), text);
							if (flag3)
							{
								int num4 = 64;
								int num5 = (int)((float)iconHeightsOfAllKindsForPlatform[i] / (float)iconWidthsOfAllKindsForPlatform[i] * 64f);
								array[i] = (Texture2D)EditorGUI.ObjectField(new Rect(rect5.x + num3 - 96f - 64f - 6f, rect5.y, (float)num4, (float)num5), array[i], typeof(Texture2D), false);
							}
							Rect position = new Rect(rect5.x + num3 - 96f, rect5.y, (float)num, (float)num2);
							Texture2D iconForPlatformAtSize = PlayerSettings.GetIconForPlatformAtSize(platform, iconWidthsOfAllKindsForPlatform[i], iconHeightsOfAllKindsForPlatform[i], iconKindsForPlatform[i]);
							if (iconForPlatformAtSize != null)
							{
								GUI.DrawTexture(position, iconForPlatformAtSize);
							}
							else
							{
								GUI.Box(position, "");
							}
						}
						if (GUI.changed)
						{
							Undo.RecordObject(base.target, PlayerSettingsEditor.Styles.undoChangedIconString);
							PlayerSettings.SetIconsForPlatform(platform, array);
						}
						GUI.enabled = enabled;
						if (targetGroup == BuildTargetGroup.iPhone || targetGroup == BuildTargetGroup.tvOS)
						{
							EditorGUILayout.PropertyField(this.m_UIPrerenderedIcon, PlayerSettingsEditor.Styles.UIPrerenderedIcon, new GUILayoutOption[0]);
							EditorGUILayout.Space();
						}
					}
				}
				if (settingsExtension != null)
				{
					settingsExtension.IconSectionGUI();
				}
			}
			this.EndSettingsBox();
		}

		private static bool TargetSupportsOptionalBuiltinSplashScreen(BuildTargetGroup targetGroup, ISettingEditorExtension settingsExtension)
		{
			bool result;
			if (settingsExtension != null)
			{
				result = settingsExtension.CanShowUnitySplashScreen();
			}
			else
			{
				result = (targetGroup == BuildTargetGroup.Standalone);
			}
			return result;
		}

		private static bool TargetSupportsProtectedGraphicsMem(BuildTargetGroup targetGroup)
		{
			return targetGroup == BuildTargetGroup.Android;
		}

		public void ResolutionSectionGUI(BuildTargetGroup targetGroup, ISettingEditorExtension settingsExtension, int sectionIndex = 0)
		{
			if (this.BeginSettingsBox(sectionIndex, PlayerSettingsEditor.Styles.resolutionPresentationTitle))
			{
				if (settingsExtension != null)
				{
					float h = 16f;
					float midWidth = 80f + EditorGUIUtility.fieldWidth + 5f;
					float maxWidth = 80f + EditorGUIUtility.fieldWidth + 5f;
					settingsExtension.ResolutionSectionGUI(h, midWidth, maxWidth);
				}
				if (targetGroup == BuildTargetGroup.Standalone)
				{
					GUILayout.Label(PlayerSettingsEditor.Styles.resolutionTitle, EditorStyles.boldLabel, new GUILayoutOption[0]);
					FullScreenMode[] options = new FullScreenMode[]
					{
						FullScreenMode.FullScreenWindow,
						FullScreenMode.ExclusiveFullScreen,
						FullScreenMode.MaximizedWindow,
						FullScreenMode.Windowed
					};
					GUIContent[] optionNames = new GUIContent[]
					{
						PlayerSettingsEditor.Styles.fullscreenWindow,
						PlayerSettingsEditor.Styles.exclusiveFullscreen,
						PlayerSettingsEditor.Styles.maximizedWindow,
						PlayerSettingsEditor.Styles.windowed
					};
					FullScreenMode fullScreenMode = PlayerSettingsEditor.BuildEnumPopup<FullScreenMode>(this.m_FullscreenMode, PlayerSettingsEditor.Styles.fullscreenMode, options, optionNames);
					bool flag = fullScreenMode != FullScreenMode.Windowed;
					this.m_ShowDefaultIsNativeResolution.target = flag;
					if (EditorGUILayout.BeginFadeGroup(this.m_ShowDefaultIsNativeResolution.faded))
					{
						EditorGUILayout.PropertyField(this.m_DefaultIsNativeResolution, new GUILayoutOption[0]);
					}
					if (this.m_ShowDefaultIsNativeResolution.faded != 0f && this.m_ShowDefaultIsNativeResolution.faded != 1f)
					{
						EditorGUILayout.EndFadeGroup();
					}
					this.m_ShowResolution.target = (!flag || !this.m_DefaultIsNativeResolution.boolValue);
					if (EditorGUILayout.BeginFadeGroup(this.m_ShowResolution.faded))
					{
						EditorGUI.BeginChangeCheck();
						EditorGUILayout.PropertyField(this.m_DefaultScreenWidth, PlayerSettingsEditor.Styles.defaultScreenWidth, new GUILayoutOption[0]);
						if (EditorGUI.EndChangeCheck() && this.m_DefaultScreenWidth.intValue < 1)
						{
							this.m_DefaultScreenWidth.intValue = 1;
						}
						EditorGUI.BeginChangeCheck();
						EditorGUILayout.PropertyField(this.m_DefaultScreenHeight, PlayerSettingsEditor.Styles.defaultScreenHeight, new GUILayoutOption[0]);
						if (EditorGUI.EndChangeCheck() && this.m_DefaultScreenHeight.intValue < 1)
						{
							this.m_DefaultScreenHeight.intValue = 1;
						}
					}
					if (this.m_ShowResolution.faded != 0f && this.m_ShowResolution.faded != 1f)
					{
						EditorGUILayout.EndFadeGroup();
					}
				}
				if (targetGroup == BuildTargetGroup.Standalone)
				{
					EditorGUILayout.PropertyField(this.m_MacRetinaSupport, PlayerSettingsEditor.Styles.macRetinaSupport, new GUILayoutOption[0]);
					EditorGUILayout.PropertyField(this.m_RunInBackground, PlayerSettingsEditor.Styles.runInBackground, new GUILayoutOption[0]);
				}
				if (settingsExtension != null && settingsExtension.SupportsOrientation())
				{
					GUILayout.Label(PlayerSettingsEditor.Styles.orientationTitle, EditorStyles.boldLabel, new GUILayoutOption[0]);
					using (new EditorGUI.DisabledScope(PlayerSettings.virtualRealitySupported))
					{
						EditorGUILayout.PropertyField(this.m_DefaultScreenOrientation, PlayerSettingsEditor.Styles.defaultScreenOrientation, new GUILayoutOption[0]);
						if (PlayerSettings.virtualRealitySupported)
						{
							EditorGUILayout.HelpBox(PlayerSettingsEditor.Styles.VRSupportOverridenInfo.text, MessageType.Info);
						}
						if (this.m_DefaultScreenOrientation.enumValueIndex == 4)
						{
							if (targetGroup == BuildTargetGroup.iPhone || targetGroup == BuildTargetGroup.Tizen)
							{
								EditorGUILayout.PropertyField(this.m_UseOSAutoRotation, PlayerSettingsEditor.Styles.useOSAutoRotation, new GUILayoutOption[0]);
							}
							EditorGUI.indentLevel++;
							GUILayout.Label(PlayerSettingsEditor.Styles.allowedOrientationTitle, EditorStyles.boldLabel, new GUILayoutOption[0]);
							if (!this.m_AllowedAutoRotateToPortrait.boolValue && !this.m_AllowedAutoRotateToPortraitUpsideDown.boolValue && !this.m_AllowedAutoRotateToLandscapeRight.boolValue && !this.m_AllowedAutoRotateToLandscapeLeft.boolValue)
							{
								this.m_AllowedAutoRotateToPortrait.boolValue = true;
								Debug.LogError("All orientations are disabled. Allowing portrait");
							}
							EditorGUILayout.PropertyField(this.m_AllowedAutoRotateToPortrait, PlayerSettingsEditor.Styles.allowedAutoRotateToPortrait, new GUILayoutOption[0]);
							EditorGUILayout.PropertyField(this.m_AllowedAutoRotateToPortraitUpsideDown, PlayerSettingsEditor.Styles.allowedAutoRotateToPortraitUpsideDown, new GUILayoutOption[0]);
							EditorGUILayout.PropertyField(this.m_AllowedAutoRotateToLandscapeRight, PlayerSettingsEditor.Styles.allowedAutoRotateToLandscapeRight, new GUILayoutOption[0]);
							EditorGUILayout.PropertyField(this.m_AllowedAutoRotateToLandscapeLeft, PlayerSettingsEditor.Styles.allowedAutoRotateToLandscapeLeft, new GUILayoutOption[0]);
							EditorGUI.indentLevel--;
						}
					}
				}
				if (targetGroup == BuildTargetGroup.iPhone)
				{
					GUILayout.Label(PlayerSettingsEditor.Styles.multitaskingSupportTitle, EditorStyles.boldLabel, new GUILayoutOption[0]);
					EditorGUILayout.PropertyField(this.m_UIRequiresFullScreen, PlayerSettingsEditor.Styles.UIRequiresFullScreen, new GUILayoutOption[0]);
					EditorGUILayout.Space();
					GUILayout.Label(PlayerSettingsEditor.Styles.statusBarTitle, EditorStyles.boldLabel, new GUILayoutOption[0]);
					EditorGUILayout.PropertyField(this.m_UIStatusBarHidden, PlayerSettingsEditor.Styles.UIStatusBarHidden, new GUILayoutOption[0]);
					EditorGUILayout.PropertyField(this.m_UIStatusBarStyle, PlayerSettingsEditor.Styles.UIStatusBarStyle, new GUILayoutOption[0]);
					EditorGUILayout.Space();
				}
				EditorGUILayout.Space();
				if (targetGroup == BuildTargetGroup.Standalone)
				{
					GUILayout.Label(PlayerSettingsEditor.Styles.standalonePlayerOptionsTitle, EditorStyles.boldLabel, new GUILayoutOption[0]);
					EditorGUILayout.PropertyField(this.m_CaptureSingleScreen, new GUILayoutOption[0]);
					EditorGUILayout.PropertyField(this.m_DisplayResolutionDialog, new GUILayoutOption[0]);
					EditorGUILayout.PropertyField(this.m_UsePlayerLog, new GUILayoutOption[0]);
					EditorGUILayout.PropertyField(this.m_ResizableWindow, new GUILayoutOption[0]);
					EditorGUILayout.PropertyField(this.m_VisibleInBackground, PlayerSettingsEditor.Styles.visibleInBackground, new GUILayoutOption[0]);
					EditorGUILayout.PropertyField(this.m_AllowFullscreenSwitch, PlayerSettingsEditor.Styles.allowFullscreenSwitch, new GUILayoutOption[0]);
					EditorGUILayout.PropertyField(this.m_ForceSingleInstance, new GUILayoutOption[0]);
					EditorGUILayout.PropertyField(this.m_SupportedAspectRatios, true, new GUILayoutOption[0]);
					EditorGUILayout.Space();
				}
				if (this.IsMobileTarget(targetGroup))
				{
					if (targetGroup != BuildTargetGroup.Tizen && targetGroup != BuildTargetGroup.iPhone && targetGroup != BuildTargetGroup.tvOS)
					{
						EditorGUILayout.PropertyField(this.m_Use32BitDisplayBuffer, PlayerSettingsEditor.Styles.use32BitDisplayBuffer, new GUILayoutOption[0]);
					}
					EditorGUILayout.PropertyField(this.m_DisableDepthAndStencilBuffers, PlayerSettingsEditor.Styles.disableDepthAndStencilBuffers, new GUILayoutOption[0]);
				}
				if (targetGroup == BuildTargetGroup.iPhone)
				{
					EditorGUILayout.PropertyField(this.m_iosShowActivityIndicatorOnLoading, PlayerSettingsEditor.Styles.iosShowActivityIndicatorOnLoading, new GUILayoutOption[0]);
				}
				if (targetGroup == BuildTargetGroup.Android)
				{
					EditorGUILayout.PropertyField(this.m_androidShowActivityIndicatorOnLoading, PlayerSettingsEditor.Styles.androidShowActivityIndicatorOnLoading, new GUILayoutOption[0]);
				}
				if (targetGroup == BuildTargetGroup.Tizen)
				{
					EditorGUILayout.PropertyField(this.m_tizenShowActivityIndicatorOnLoading, EditorGUIUtility.TrTextContent("Show Loading Indicator", null, null), new GUILayoutOption[0]);
				}
				if (targetGroup == BuildTargetGroup.iPhone || targetGroup == BuildTargetGroup.Android || targetGroup == BuildTargetGroup.Tizen)
				{
					EditorGUILayout.Space();
				}
				this.ShowSharedNote();
			}
			this.EndSettingsBox();
		}

		private void AddGraphicsDeviceMenuSelected(object userData, string[] options, int selected)
		{
			BuildTarget platform = (BuildTarget)userData;
			GraphicsDeviceType[] array = PlayerSettings.GetGraphicsAPIs(platform);
			if (array != null)
			{
				GraphicsDeviceType item = (GraphicsDeviceType)Enum.Parse(typeof(GraphicsDeviceType), options[selected], true);
				List<GraphicsDeviceType> list = array.ToList<GraphicsDeviceType>();
				list.Add(item);
				array = list.ToArray();
				PlayerSettings.SetGraphicsAPIs(platform, array);
			}
		}

		private void AddGraphicsDeviceElement(BuildTarget target, Rect rect, ReorderableList list)
		{
			GraphicsDeviceType[] supportedGraphicsAPIs = PlayerSettings.GetSupportedGraphicsAPIs(target);
			if (supportedGraphicsAPIs != null && supportedGraphicsAPIs.Length != 0)
			{
				string[] array = new string[supportedGraphicsAPIs.Length];
				bool[] array2 = new bool[supportedGraphicsAPIs.Length];
				for (int i = 0; i < supportedGraphicsAPIs.Length; i++)
				{
					array[i] = supportedGraphicsAPIs[i].ToString();
					array2[i] = !list.list.Contains(supportedGraphicsAPIs[i]);
				}
				EditorUtility.DisplayCustomMenu(rect, array, array2, null, new EditorUtility.SelectMenuItemFunction(this.AddGraphicsDeviceMenuSelected), target);
			}
		}

		private bool CanRemoveGraphicsDeviceElement(ReorderableList list)
		{
			return list.list.Count >= 2;
		}

		private void RemoveGraphicsDeviceElement(BuildTarget target, ReorderableList list)
		{
			GraphicsDeviceType[] array = PlayerSettings.GetGraphicsAPIs(target);
			if (array != null)
			{
				if (array.Length < 2)
				{
					EditorApplication.Beep();
				}
				else
				{
					List<GraphicsDeviceType> list2 = array.ToList<GraphicsDeviceType>();
					list2.RemoveAt(list.index);
					array = list2.ToArray();
					this.ApplyChangedGraphicsAPIList(target, array, list.index == 0);
				}
			}
		}

		private void ReorderGraphicsDeviceElement(BuildTarget target, ReorderableList list)
		{
			GraphicsDeviceType[] graphicsAPIs = PlayerSettings.GetGraphicsAPIs(target);
			List<GraphicsDeviceType> list2 = (List<GraphicsDeviceType>)list.list;
			GraphicsDeviceType[] array = list2.ToArray();
			bool firstEntryChanged = graphicsAPIs[0] != array[0];
			this.ApplyChangedGraphicsAPIList(target, array, firstEntryChanged);
		}

		private PlayerSettingsEditor.ChangeGraphicsApiAction CheckApplyGraphicsAPIList(BuildTarget target, bool firstEntryChanged)
		{
			bool doChange = true;
			bool doReload = false;
			if (firstEntryChanged && PlayerSettingsEditor.WillEditorUseFirstGraphicsAPI(target))
			{
				doChange = false;
				if (EditorUtility.DisplayDialog("Changing editor graphics device", "Changing active graphics API requires reloading all graphics objects, it might take a while", "Apply", "Cancel"))
				{
					if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
					{
						doReload = (doChange = true);
					}
				}
			}
			return new PlayerSettingsEditor.ChangeGraphicsApiAction(doChange, doReload);
		}

		private void ApplyChangeGraphicsApiAction(BuildTarget target, GraphicsDeviceType[] apis, PlayerSettingsEditor.ChangeGraphicsApiAction action)
		{
			if (action.changeList)
			{
				PlayerSettings.SetGraphicsAPIs(target, apis);
			}
			else
			{
				PlayerSettingsEditor.s_GraphicsDeviceLists.Remove(target);
			}
			if (action.reloadGfx)
			{
				ShaderUtil.RecreateGfxDevice();
				GUIUtility.ExitGUI();
			}
		}

		private void ApplyChangedGraphicsAPIList(BuildTarget target, GraphicsDeviceType[] apis, bool firstEntryChanged)
		{
			PlayerSettingsEditor.ChangeGraphicsApiAction action = this.CheckApplyGraphicsAPIList(target, firstEntryChanged);
			this.ApplyChangeGraphicsApiAction(target, apis, action);
		}

		private void DrawGraphicsDeviceElement(BuildTarget target, Rect rect, int index, bool selected, bool focused)
		{
			object obj = PlayerSettingsEditor.s_GraphicsDeviceLists[target].list[index];
			string text = obj.ToString();
			if (text == "Direct3D12")
			{
				text = "Direct3D12 (Experimental)";
			}
			if (text == "Vulkan" && target != BuildTarget.Android)
			{
				text = "Vulkan (Experimental)";
			}
			if (text == "XboxOneD3D12")
			{
				text = "XboxOneD3D12 (Experimental)";
			}
			if (target == BuildTarget.WebGL)
			{
				if (text == "OpenGLES3")
				{
					text = "WebGL 2.0";
				}
				else if (text == "OpenGLES2")
				{
					text = "WebGL 1.0";
				}
			}
			GUI.Label(rect, text, EditorStyles.label);
		}

		private static bool WillEditorUseFirstGraphicsAPI(BuildTarget targetPlatform)
		{
			return (Application.platform == RuntimePlatform.WindowsEditor && targetPlatform == BuildTarget.StandaloneWindows) || (Application.platform == RuntimePlatform.OSXEditor && targetPlatform == BuildTarget.StandaloneOSX);
		}

		private void OpenGLES31OptionsGUI(BuildTargetGroup targetGroup, BuildTarget targetPlatform)
		{
			if (targetGroup == BuildTargetGroup.Android)
			{
				GraphicsDeviceType[] graphicsAPIs = PlayerSettings.GetGraphicsAPIs(targetPlatform);
				if (graphicsAPIs.Contains(GraphicsDeviceType.OpenGLES3) && !graphicsAPIs.Contains(GraphicsDeviceType.OpenGLES2))
				{
					EditorGUILayout.PropertyField(this.m_RequireES31, PlayerSettingsEditor.Styles.require31, new GUILayoutOption[0]);
					EditorGUILayout.PropertyField(this.m_RequireES31AEP, PlayerSettingsEditor.Styles.requireAEP, new GUILayoutOption[0]);
				}
			}
		}

		private void GraphicsAPIsGUIOnePlatform(BuildTargetGroup targetGroup, BuildTarget targetPlatform, string platformTitle)
		{
			GraphicsDeviceType[] supportedGraphicsAPIs = PlayerSettings.GetSupportedGraphicsAPIs(targetPlatform);
			if (supportedGraphicsAPIs != null && supportedGraphicsAPIs.Length >= 2)
			{
				EditorGUI.BeginChangeCheck();
				bool flag = PlayerSettings.GetUseDefaultGraphicsAPIs(targetPlatform);
				flag = EditorGUILayout.Toggle(string.Format(L10n.Tr("Auto Graphics API {0}"), platformTitle ?? string.Empty), flag, new GUILayoutOption[0]);
				if (EditorGUI.EndChangeCheck())
				{
					Undo.RecordObject(base.target, PlayerSettingsEditor.Styles.undoChangedGraphicsAPIString);
					PlayerSettings.SetUseDefaultGraphicsAPIs(targetPlatform, flag);
				}
				if (!flag)
				{
					if (PlayerSettingsEditor.WillEditorUseFirstGraphicsAPI(targetPlatform))
					{
						EditorGUILayout.HelpBox(PlayerSettingsEditor.Styles.recordingInfo.text, MessageType.Info, true);
					}
					string displayTitle = "Graphics APIs";
					if (platformTitle != null)
					{
						displayTitle += platformTitle;
					}
					if (!PlayerSettingsEditor.s_GraphicsDeviceLists.ContainsKey(targetPlatform))
					{
						GraphicsDeviceType[] graphicsAPIs = PlayerSettings.GetGraphicsAPIs(targetPlatform);
						List<GraphicsDeviceType> elements = (graphicsAPIs == null) ? new List<GraphicsDeviceType>() : graphicsAPIs.ToList<GraphicsDeviceType>();
						ReorderableList reorderableList = new ReorderableList(elements, typeof(GraphicsDeviceType), true, true, true, true);
						reorderableList.onAddDropdownCallback = delegate(Rect rect, ReorderableList list)
						{
							this.AddGraphicsDeviceElement(targetPlatform, rect, list);
						};
						reorderableList.onCanRemoveCallback = new ReorderableList.CanRemoveCallbackDelegate(this.CanRemoveGraphicsDeviceElement);
						reorderableList.onRemoveCallback = delegate(ReorderableList list)
						{
							this.RemoveGraphicsDeviceElement(targetPlatform, list);
						};
						reorderableList.onReorderCallback = delegate(ReorderableList list)
						{
							this.ReorderGraphicsDeviceElement(targetPlatform, list);
						};
						reorderableList.drawElementCallback = delegate(Rect rect, int index, bool isActive, bool isFocused)
						{
							this.DrawGraphicsDeviceElement(targetPlatform, rect, index, isActive, isFocused);
						};
						reorderableList.drawHeaderCallback = delegate(Rect rect)
						{
							GUI.Label(rect, displayTitle, EditorStyles.label);
						};
						reorderableList.elementHeight = 16f;
						PlayerSettingsEditor.s_GraphicsDeviceLists.Add(targetPlatform, reorderableList);
					}
					PlayerSettingsEditor.s_GraphicsDeviceLists[targetPlatform].DoLayoutList();
					this.OpenGLES31OptionsGUI(targetGroup, targetPlatform);
				}
			}
		}

		private void GraphicsAPIsGUI(BuildTargetGroup targetGroup, BuildTarget target)
		{
			if (targetGroup == BuildTargetGroup.Standalone)
			{
				this.GraphicsAPIsGUIOnePlatform(targetGroup, BuildTarget.StandaloneWindows, " for Windows");
				this.GraphicsAPIsGUIOnePlatform(targetGroup, BuildTarget.StandaloneOSX, " for Mac");
				this.GraphicsAPIsGUIOnePlatform(targetGroup, BuildTarget.StandaloneLinuxUniversal, " for Linux");
			}
			else
			{
				this.GraphicsAPIsGUIOnePlatform(targetGroup, target, null);
			}
		}

		private static bool IsColorGamutSupportedOnTargetGroup(BuildTargetGroup targetGroup, ColorGamut gamut)
		{
			return gamut == ColorGamut.sRGB || (PlayerSettingsEditor.s_SupportedColorGamuts.ContainsKey(targetGroup) && PlayerSettingsEditor.s_SupportedColorGamuts[targetGroup].Contains(gamut));
		}

		private static string GetColorGamutDisplayString(BuildTargetGroup targetGroup, ColorGamut gamut)
		{
			string text = gamut.ToString();
			if (!PlayerSettingsEditor.IsColorGamutSupportedOnTargetGroup(targetGroup, gamut))
			{
				text += " (not supported on this platform)";
			}
			return text;
		}

		private void AddColorGamutElement(BuildTargetGroup targetGroup, Rect rect, ReorderableList list)
		{
			ColorGamut[] array = new ColorGamut[]
			{
				ColorGamut.sRGB,
				ColorGamut.DisplayP3
			};
			string[] array2 = new string[array.Length];
			bool[] array3 = new bool[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array2[i] = PlayerSettingsEditor.GetColorGamutDisplayString(targetGroup, array[i]);
				array3[i] = !list.list.Contains(array[i]);
			}
			EditorUtility.DisplayCustomMenu(rect, array2, array3, null, new EditorUtility.SelectMenuItemFunction(this.AddColorGamutMenuSelected), array);
		}

		private void AddColorGamutMenuSelected(object userData, string[] options, int selected)
		{
			ColorGamut[] array = (ColorGamut[])userData;
			List<ColorGamut> list = PlayerSettings.GetColorGamuts().ToList<ColorGamut>();
			list.Add(array[selected]);
			PlayerSettings.SetColorGamuts(list.ToArray());
		}

		private bool CanRemoveColorGamutElement(ReorderableList list)
		{
			List<ColorGamut> list2 = (List<ColorGamut>)list.list;
			return list2[list.index] != ColorGamut.sRGB;
		}

		private void RemoveColorGamutElement(ReorderableList list)
		{
			List<ColorGamut> list2 = PlayerSettings.GetColorGamuts().ToList<ColorGamut>();
			if (list2.Count < 2)
			{
				EditorApplication.Beep();
			}
			else
			{
				list2.RemoveAt(list.index);
				PlayerSettings.SetColorGamuts(list2.ToArray());
			}
		}

		private void ReorderColorGamutElement(ReorderableList list)
		{
			List<ColorGamut> list2 = (List<ColorGamut>)list.list;
			PlayerSettings.SetColorGamuts(list2.ToArray());
		}

		private void DrawColorGamutElement(BuildTargetGroup targetGroup, Rect rect, int index, bool selected, bool focused)
		{
			object obj = PlayerSettingsEditor.s_ColorGamutList.list[index];
			GUI.Label(rect, PlayerSettingsEditor.GetColorGamutDisplayString(targetGroup, (ColorGamut)obj), EditorStyles.label);
		}

		private void ColorGamutGUI(BuildTargetGroup targetGroup)
		{
			if (PlayerSettingsEditor.s_SupportedColorGamuts.ContainsKey(targetGroup))
			{
				if (PlayerSettingsEditor.s_ColorGamutList == null)
				{
					ColorGamut[] colorGamuts = PlayerSettings.GetColorGamuts();
					List<ColorGamut> elements = (colorGamuts == null) ? new List<ColorGamut>() : colorGamuts.ToList<ColorGamut>();
					PlayerSettingsEditor.s_ColorGamutList = new ReorderableList(elements, typeof(ColorGamut), true, true, true, true)
					{
						onCanRemoveCallback = new ReorderableList.CanRemoveCallbackDelegate(this.CanRemoveColorGamutElement),
						onRemoveCallback = new ReorderableList.RemoveCallbackDelegate(this.RemoveColorGamutElement),
						onReorderCallback = new ReorderableList.ReorderCallbackDelegate(this.ReorderColorGamutElement),
						elementHeight = 16f
					};
				}
				GUIContent header = (targetGroup != BuildTargetGroup.Standalone) ? PlayerSettingsEditor.Styles.colorGamut : PlayerSettingsEditor.Styles.colorGamutForMac;
				PlayerSettingsEditor.s_ColorGamutList.drawHeaderCallback = delegate(Rect rect)
				{
					GUI.Label(rect, header, EditorStyles.label);
				};
				PlayerSettingsEditor.s_ColorGamutList.onAddDropdownCallback = delegate(Rect rect, ReorderableList list)
				{
					this.AddColorGamutElement(targetGroup, rect, list);
				};
				PlayerSettingsEditor.s_ColorGamutList.drawElementCallback = delegate(Rect rect, int index, bool selected, bool focused)
				{
					this.DrawColorGamutElement(targetGroup, rect, index, selected, focused);
				};
				PlayerSettingsEditor.s_ColorGamutList.DoLayoutList();
			}
		}

		public void DebugAndCrashReportingGUI(BuildPlatform platform, BuildTargetGroup targetGroup, ISettingEditorExtension settingsExtension, int sectionIndex = 3)
		{
			if (targetGroup == BuildTargetGroup.iPhone || targetGroup == BuildTargetGroup.tvOS)
			{
				if (this.BeginSettingsBox(sectionIndex, PlayerSettingsEditor.Styles.debuggingCrashReportingTitle))
				{
					GUILayout.Label(PlayerSettingsEditor.Styles.debuggingTitle, EditorStyles.boldLabel, new GUILayoutOption[0]);
					EditorGUILayout.PropertyField(this.m_EnableInternalProfiler, PlayerSettingsEditor.Styles.enableInternalProfiler, new GUILayoutOption[0]);
					EditorGUILayout.Space();
					GUILayout.Label(PlayerSettingsEditor.Styles.crashReportingTitle, EditorStyles.boldLabel, new GUILayoutOption[0]);
					EditorGUILayout.PropertyField(this.m_ActionOnDotNetUnhandledException, PlayerSettingsEditor.Styles.actionOnDotNetUnhandledException, new GUILayoutOption[0]);
					EditorGUILayout.PropertyField(this.m_LogObjCUncaughtExceptions, PlayerSettingsEditor.Styles.logObjCUncaughtExceptions, new GUILayoutOption[0]);
					GUIContent gUIContent = PlayerSettingsEditor.Styles.enableCrashReportAPI;
					bool disabled = false;
					if (CrashReportingSettings.enabled)
					{
						gUIContent = new GUIContent(gUIContent);
						disabled = true;
						gUIContent.tooltip = "CrashReport API must be enabled for Performance Reporting service.";
						this.m_EnableCrashReportAPI.boolValue = true;
					}
					EditorGUI.BeginDisabledGroup(disabled);
					EditorGUILayout.PropertyField(this.m_EnableCrashReportAPI, gUIContent, new GUILayoutOption[0]);
					EditorGUI.EndDisabledGroup();
					EditorGUILayout.Space();
				}
				this.EndSettingsBox();
			}
		}

		public static void BuildDisabledEnumPopup(GUIContent selected, GUIContent uiString)
		{
			using (new EditorGUI.DisabledScope(true))
			{
				EditorGUI.Popup(EditorGUILayout.GetControlRect(true, new GUILayoutOption[0]), uiString, 0, new GUIContent[]
				{
					selected
				});
			}
		}

		public static T BuildEnumPopup<T>(SerializedProperty prop, GUIContent uiString, T[] options, GUIContent[] optionNames)
		{
			T t = (T)((object)prop.intValue);
			T t2 = PlayerSettingsEditor.BuildEnumPopup<T>(uiString, t, options, optionNames);
			if (!t2.Equals(t))
			{
				prop.intValue = (int)((object)t2);
				prop.serializedObject.ApplyModifiedProperties();
			}
			return t2;
		}

		public static T BuildEnumPopup<T>(GUIContent uiString, T selected, T[] options, GUIContent[] optionNames)
		{
			int selectedIndex = 0;
			for (int i = 1; i < options.Length; i++)
			{
				if (selected.Equals(options[i]))
				{
					selectedIndex = i;
					break;
				}
			}
			int num = EditorGUILayout.Popup(uiString, selectedIndex, optionNames, new GUILayoutOption[0]);
			return options[num];
		}

		public void OtherSectionGUI(BuildPlatform platform, BuildTargetGroup targetGroup, ISettingEditorExtension settingsExtension, int sectionIndex = 4)
		{
			if (this.BeginSettingsBox(sectionIndex, PlayerSettingsEditor.Styles.otherSettingsTitle))
			{
				this.OtherSectionRenderingGUI(platform, targetGroup, settingsExtension);
				this.OtherSectionIdentificationGUI(targetGroup, settingsExtension);
				this.OtherSectionConfigurationGUI(targetGroup, settingsExtension);
				this.OtherSectionOptimizationGUI(targetGroup);
				this.OtherSectionLoggingGUI();
				this.ShowSharedNote();
			}
			this.EndSettingsBox();
		}

		private void OtherSectionRenderingGUI(BuildPlatform platform, BuildTargetGroup targetGroup, ISettingEditorExtension settingsExtension)
		{
			GUILayout.Label(PlayerSettingsEditor.Styles.renderingTitle, EditorStyles.boldLabel, new GUILayoutOption[0]);
			if (targetGroup == BuildTargetGroup.Standalone || targetGroup == BuildTargetGroup.iPhone || targetGroup == BuildTargetGroup.tvOS || targetGroup == BuildTargetGroup.Android || targetGroup == BuildTargetGroup.PS4 || targetGroup == BuildTargetGroup.XboxOne || targetGroup == BuildTargetGroup.WSA || targetGroup == BuildTargetGroup.WebGL || targetGroup == BuildTargetGroup.Switch)
			{
				using (new EditorGUI.DisabledScope(EditorApplication.isPlaying))
				{
					EditorGUI.BeginChangeCheck();
					EditorGUILayout.PropertyField(this.m_ActiveColorSpace, PlayerSettingsEditor.Styles.activeColorSpace, new GUILayoutOption[0]);
					if (EditorGUI.EndChangeCheck())
					{
						base.serializedObject.ApplyModifiedProperties();
						GUIUtility.ExitGUI();
					}
				}
				if (PlayerSettings.colorSpace == ColorSpace.Linear)
				{
					if (targetGroup == BuildTargetGroup.iPhone)
					{
						GraphicsDeviceType[] graphicsAPIs = PlayerSettings.GetGraphicsAPIs(BuildTarget.iOS);
						bool flag = !graphicsAPIs.Contains(GraphicsDeviceType.OpenGLES3) && !graphicsAPIs.Contains(GraphicsDeviceType.OpenGLES2);
						Version requiredVersion = new Version(8, 0);
						bool flag2 = PlayerSettings.iOS.IsTargetVersionEqualOrHigher(requiredVersion);
						if (!flag || !flag2)
						{
							EditorGUILayout.HelpBox(PlayerSettingsEditor.Styles.colorSpaceIOSWarning.text, MessageType.Warning);
						}
					}
					if (targetGroup == BuildTargetGroup.tvOS)
					{
						GraphicsDeviceType[] graphicsAPIs2 = PlayerSettings.GetGraphicsAPIs(BuildTarget.tvOS);
						if (graphicsAPIs2.Contains(GraphicsDeviceType.OpenGLES3) || graphicsAPIs2.Contains(GraphicsDeviceType.OpenGLES2))
						{
							EditorGUILayout.HelpBox(PlayerSettingsEditor.Styles.colorSpaceTVOSWarning.text, MessageType.Warning);
						}
					}
					if (targetGroup == BuildTargetGroup.Android)
					{
						GraphicsDeviceType[] graphicsAPIs3 = PlayerSettings.GetGraphicsAPIs(BuildTarget.Android);
						bool flag3 = (graphicsAPIs3.Contains(GraphicsDeviceType.Vulkan) || graphicsAPIs3.Contains(GraphicsDeviceType.OpenGLES3)) && !graphicsAPIs3.Contains(GraphicsDeviceType.OpenGLES2);
						if (PlayerSettings.Android.blitType == AndroidBlitType.Never || !flag3 || PlayerSettings.Android.minSdkVersion < AndroidSdkVersions.AndroidApiLevel18)
						{
							EditorGUILayout.HelpBox(PlayerSettingsEditor.Styles.colorSpaceAndroidWarning.text, MessageType.Warning);
						}
					}
					if (targetGroup == BuildTargetGroup.WebGL)
					{
						GraphicsDeviceType[] graphicsAPIs4 = PlayerSettings.GetGraphicsAPIs(BuildTarget.WebGL);
						if (!graphicsAPIs4.Contains(GraphicsDeviceType.OpenGLES3) || graphicsAPIs4.Contains(GraphicsDeviceType.OpenGLES2))
						{
							EditorGUILayout.HelpBox(PlayerSettingsEditor.Styles.colorSpaceWebGLWarning.text, MessageType.Error);
						}
					}
				}
			}
			this.GraphicsAPIsGUI(targetGroup, platform.defaultTarget);
			this.ColorGamutGUI(targetGroup);
			if (Application.platform == RuntimePlatform.OSXEditor && (targetGroup == BuildTargetGroup.Standalone || targetGroup == BuildTargetGroup.iPhone || targetGroup == BuildTargetGroup.tvOS))
			{
				bool flag4 = this.m_MetalEditorSupport.boolValue || SystemInfo.graphicsDeviceType == GraphicsDeviceType.Metal;
				bool flag5 = EditorGUILayout.Toggle(PlayerSettingsEditor.Styles.metalEditorSupport, flag4, new GUILayoutOption[0]);
				if (flag5 != flag4)
				{
					if (Application.platform == RuntimePlatform.OSXEditor)
					{
						GraphicsDeviceType[] graphicsAPIs5 = PlayerSettings.GetGraphicsAPIs(BuildTarget.StandaloneOSX);
						bool firstEntryChanged = graphicsAPIs5[0] != SystemInfo.graphicsDeviceType;
						if (!flag5 && SystemInfo.graphicsDeviceType == GraphicsDeviceType.Metal)
						{
							firstEntryChanged = true;
						}
						if (flag5 && graphicsAPIs5[0] == GraphicsDeviceType.Metal)
						{
							firstEntryChanged = true;
						}
						PlayerSettingsEditor.ChangeGraphicsApiAction action = this.CheckApplyGraphicsAPIList(BuildTarget.StandaloneOSX, firstEntryChanged);
						if (action.changeList)
						{
							this.m_MetalEditorSupport.boolValue = flag5;
							base.serializedObject.ApplyModifiedProperties();
							action = new PlayerSettingsEditor.ChangeGraphicsApiAction(false, action.reloadGfx);
						}
						this.ApplyChangeGraphicsApiAction(BuildTarget.StandaloneOSX, graphicsAPIs5, action);
					}
					else
					{
						this.m_MetalEditorSupport.boolValue = flag5;
						base.serializedObject.ApplyModifiedProperties();
					}
				}
				if (this.m_MetalEditorSupport.boolValue)
				{
					using (new EditorGUI.IndentLevelScope())
					{
						this.m_MetalAPIValidation.boolValue = EditorGUILayout.Toggle(PlayerSettingsEditor.Styles.metalAPIValidation, this.m_MetalAPIValidation.boolValue, new GUILayoutOption[0]);
					}
				}
				EditorGUILayout.PropertyField(this.m_MetalFramebufferOnly, PlayerSettingsEditor.Styles.metalFramebufferOnly, new GUILayoutOption[0]);
				if (targetGroup == BuildTargetGroup.iPhone || targetGroup == BuildTargetGroup.tvOS)
				{
					EditorGUILayout.PropertyField(this.m_MetalForceHardShadows, PlayerSettingsEditor.Styles.metalForceHardShadows, new GUILayoutOption[0]);
				}
			}
			if (settingsExtension != null && settingsExtension.SupportsMultithreadedRendering())
			{
				settingsExtension.MultithreadedRenderingGUI(targetGroup);
			}
			bool flag6 = true;
			bool flag7 = true;
			if (settingsExtension != null)
			{
				flag6 = settingsExtension.SupportsStaticBatching();
				flag7 = settingsExtension.SupportsDynamicBatching();
			}
			int num;
			int num2;
			PlayerSettings.GetBatchingForPlatform(platform.defaultTarget, out num, out num2);
			bool flag8 = false;
			if (!flag6 && num == 1)
			{
				num = 0;
				flag8 = true;
			}
			if (!flag7 && num2 == 1)
			{
				num2 = 0;
				flag8 = true;
			}
			if (flag8)
			{
				PlayerSettings.SetBatchingForPlatform(platform.defaultTarget, num, num2);
			}
			EditorGUI.BeginChangeCheck();
			using (new EditorGUI.DisabledScope(!flag6))
			{
				if (GUI.enabled)
				{
					num = ((!EditorGUILayout.Toggle(PlayerSettingsEditor.Styles.staticBatching, num != 0, new GUILayoutOption[0])) ? 0 : 1);
				}
				else
				{
					EditorGUILayout.Toggle(PlayerSettingsEditor.Styles.staticBatching, false, new GUILayoutOption[0]);
				}
			}
			using (new EditorGUI.DisabledScope(!flag7))
			{
				num2 = ((!EditorGUILayout.Toggle(PlayerSettingsEditor.Styles.dynamicBatching, num2 != 0, new GUILayoutOption[0])) ? 0 : 1);
			}
			if (EditorGUI.EndChangeCheck())
			{
				Undo.RecordObject(base.target, PlayerSettingsEditor.Styles.undoChangedBatchingString);
				PlayerSettings.SetBatchingForPlatform(platform.defaultTarget, num, num2);
			}
			bool flag9 = false;
			bool flag10 = false;
			bool flag11 = targetGroup == BuildTargetGroup.Standalone;
			if (settingsExtension != null)
			{
				flag9 = settingsExtension.SupportsHighDynamicRangeDisplays();
				flag10 = settingsExtension.SupportsGfxJobModes();
				flag11 = (flag11 || settingsExtension.SupportsCustomLightmapEncoding());
			}
			if (targetGroup == BuildTargetGroup.Standalone || targetGroup == BuildTargetGroup.iPhone || targetGroup == BuildTargetGroup.tvOS || targetGroup == BuildTargetGroup.Android || targetGroup == BuildTargetGroup.PSP2 || targetGroup == BuildTargetGroup.PS4 || targetGroup == BuildTargetGroup.XboxOne || targetGroup == BuildTargetGroup.WSA || targetGroup == BuildTargetGroup.Switch)
			{
				EditorGUI.BeginChangeCheck();
				EditorGUILayout.PropertyField(this.m_SkinOnGPU, (targetGroup == BuildTargetGroup.PS4 || targetGroup == BuildTargetGroup.Switch) ? PlayerSettingsEditor.Styles.skinOnGPUPS4 : PlayerSettingsEditor.Styles.skinOnGPU, new GUILayoutOption[0]);
				if (EditorGUI.EndChangeCheck())
				{
					ShaderUtil.RecreateSkinnedMeshResources();
				}
			}
			if (targetGroup == BuildTargetGroup.Android && PlayerSettings.gpuSkinning)
			{
				EditorGUILayout.HelpBox(PlayerSettingsEditor.Styles.skinOnGPUAndroidWarning.text, MessageType.Warning);
			}
			if (targetGroup == BuildTargetGroup.XboxOne)
			{
				GraphicsDeviceType[] graphicsAPIs6 = PlayerSettings.GetGraphicsAPIs(platform.defaultTarget);
				PlayerSettings.graphicsJobMode = ((graphicsAPIs6[0] != GraphicsDeviceType.XboxOneD3D12) ? GraphicsJobMode.Legacy : GraphicsJobMode.Native);
				if (graphicsAPIs6[0] == GraphicsDeviceType.XboxOneD3D12)
				{
					PlayerSettings.graphicsJobs = true;
				}
				using (new EditorGUI.DisabledScope(graphicsAPIs6[0] == GraphicsDeviceType.XboxOneD3D12))
				{
					EditorGUILayout.PropertyField(this.m_GraphicsJobs, PlayerSettingsEditor.Styles.graphicsJobs, new GUILayoutOption[0]);
				}
			}
			else
			{
				EditorGUILayout.PropertyField(this.m_GraphicsJobs, PlayerSettingsEditor.Styles.graphicsJobs, new GUILayoutOption[0]);
				if (flag10)
				{
					using (new EditorGUI.DisabledScope(!this.m_GraphicsJobs.boolValue))
					{
						GraphicsJobMode graphicsJobMode = PlayerSettings.graphicsJobMode;
						GraphicsJobMode graphicsJobMode2 = PlayerSettingsEditor.BuildEnumPopup<GraphicsJobMode>(PlayerSettingsEditor.Styles.graphicsJobsMode, graphicsJobMode, PlayerSettingsEditor.m_GfxJobModeValues, PlayerSettingsEditor.m_GfxJobModeNames);
						if (graphicsJobMode2 != graphicsJobMode)
						{
							PlayerSettings.graphicsJobMode = graphicsJobMode2;
						}
					}
				}
			}
			if (flag11)
			{
				using (new EditorGUI.DisabledScope(EditorApplication.isPlaying || Lightmapping.isRunning))
				{
					EditorGUI.BeginChangeCheck();
					LightmapEncodingQuality lightmapEncodingQuality = PlayerSettings.GetLightmapEncodingQualityForPlatformGroup(targetGroup);
					LightmapEncodingQuality[] options = new LightmapEncodingQuality[]
					{
						LightmapEncodingQuality.Normal,
						LightmapEncodingQuality.High
					};
					lightmapEncodingQuality = PlayerSettingsEditor.BuildEnumPopup<LightmapEncodingQuality>(PlayerSettingsEditor.Styles.lightmapEncodingLabel, lightmapEncodingQuality, options, PlayerSettingsEditor.Styles.lightmapEncodingNames);
					if (EditorGUI.EndChangeCheck())
					{
						PlayerSettings.SetLightmapEncodingQualityForPlatformGroup(targetGroup, lightmapEncodingQuality);
						Lightmapping.OnUpdateLightmapEncoding(targetGroup);
						base.serializedObject.ApplyModifiedProperties();
						GUIUtility.ExitGUI();
					}
				}
			}
			if (this.m_VRSettings.TargetGroupSupportsVirtualReality(targetGroup))
			{
				if (EditorGUILayout.LinkLabel(PlayerSettingsEditor.Styles.vrSettingsMoved, new GUILayoutOption[0]))
				{
					this.m_SelectedSection.value = this.m_VRSettings.GUISectionIndex;
				}
			}
			if (PlayerSettingsEditor.TargetSupportsProtectedGraphicsMem(targetGroup))
			{
				PlayerSettings.protectGraphicsMemory = EditorGUILayout.Toggle(PlayerSettingsEditor.Styles.protectGraphicsMemory, PlayerSettings.protectGraphicsMemory, new GUILayoutOption[0]);
			}
			if (flag9)
			{
				PlayerSettings.useHDRDisplay = EditorGUILayout.Toggle(EditorGUIUtility.TrTextContent("Use display in HDR mode", "Automatically switch the display to HDR output (on supported displays) at start of application.", null), PlayerSettings.useHDRDisplay, new GUILayoutOption[0]);
			}
			EditorGUILayout.Space();
		}

		private void OtherSectionIdentificationGUI(BuildTargetGroup targetGroup, ISettingEditorExtension settingsExtension)
		{
			if (settingsExtension != null && settingsExtension.HasIdentificationGUI())
			{
				GUILayout.Label(PlayerSettingsEditor.Styles.identificationTitle, EditorStyles.boldLabel, new GUILayoutOption[0]);
				settingsExtension.IdentificationSectionGUI();
				EditorGUILayout.Space();
			}
			else if (targetGroup == BuildTargetGroup.Standalone)
			{
				GUILayout.Label(PlayerSettingsEditor.Styles.macAppStoreTitle, EditorStyles.boldLabel, new GUILayoutOption[0]);
				PlayerSettingsEditor.ShowApplicationIdentifierUI(base.serializedObject, BuildTargetGroup.Standalone, "Bundle Identifier", "'CFBundleIdentifier'", PlayerSettingsEditor.Styles.undoChangedBundleIdentifierString);
				EditorGUILayout.PropertyField(this.m_ApplicationBundleVersion, EditorGUIUtility.TrTextContent("Version*", "'CFBundleShortVersionString'", null), new GUILayoutOption[0]);
				PlayerSettingsEditor.ShowBuildNumberUI(base.serializedObject, BuildTargetGroup.Standalone, "Build", "'CFBundleVersion'", PlayerSettingsEditor.Styles.undoChangedBuildNumberString);
				EditorGUILayout.PropertyField(this.m_MacAppStoreCategory, PlayerSettingsEditor.Styles.macAppStoreCategory, new GUILayoutOption[0]);
				EditorGUILayout.PropertyField(this.m_UseMacAppStoreValidation, PlayerSettingsEditor.Styles.useMacAppStoreValidation, new GUILayoutOption[0]);
				EditorGUILayout.Space();
			}
		}

		internal static void ShowPlatformIconsByKind(PlatformIconFieldGroup iconFieldGroup, bool foldByKind = true, bool foldBySubkind = true)
		{
			int num = 20;
			if (iconFieldGroup.m_IconsFields.Count == 0)
			{
				PlatformIconKind[] supportedIconKindsForPlatform = PlayerSettings.GetSupportedIconKindsForPlatform(iconFieldGroup.targetGroup);
				for (int i = 0; i < supportedIconKindsForPlatform.Length; i++)
				{
					PlatformIconKind kind = supportedIconKindsForPlatform[i];
					iconFieldGroup.AddPlatformIcons(PlayerSettings.GetPlatformIcons(iconFieldGroup.targetGroup, kind), kind);
				}
			}
			foreach (KeyValuePair<PlatformIconFieldGroup.IconFieldGroupInfo, Dictionary<PlatformIconFieldGroup.IconFieldGroupInfo, PlatformIconField[]>> current in iconFieldGroup.m_IconsFields)
			{
				EditorGUI.BeginChangeCheck();
				PlatformIconFieldGroup.IconFieldGroupInfo key = current.Key;
				if (foldByKind)
				{
					string content = string.Format("{0} icons ({1}/{2})", key.m_Label, current.Key.m_SetIconSlots, current.Key.m_IconSlotCount);
					Rect rect = GUILayoutUtility.GetRect(64f, (float)num);
					rect.x += 2f;
					key.m_State = EditorGUI.Foldout(rect, key.m_State, content, EditorStyles.foldout);
				}
				else
				{
					key.m_State = true;
				}
				if (key.m_State)
				{
					current.Key.m_SetIconSlots = 0;
					foreach (KeyValuePair<PlatformIconFieldGroup.IconFieldGroupInfo, PlatformIconField[]> current2 in current.Value)
					{
						current2.Key.m_SetIconSlots = PlayerSettings.GetNonEmptyPlatformIconCount((from x in current2.Value
						select x.platformIcon).ToArray<PlatformIcon>());
						current.Key.m_SetIconSlots += current2.Key.m_SetIconSlots;
						if (foldBySubkind)
						{
							string content2 = string.Format("{0} icons ({1}/{2})", current2.Key.m_Label, current2.Key.m_SetIconSlots, current2.Value.Length);
							Rect rect2 = GUILayoutUtility.GetRect(64f, (float)num);
							rect2.x += 8f;
							current2.Key.m_State = EditorGUI.Foldout(rect2, current2.Key.m_State, content2, EditorStyles.foldout);
						}
						else
						{
							current2.Key.m_State = true;
						}
						if (current2.Key.m_State || !foldBySubkind)
						{
							PlatformIconField[] value = current2.Value;
							for (int j = 0; j < value.Length; j++)
							{
								PlatformIconField platformIconField = value[j];
								platformIconField.DrawAt();
							}
						}
					}
				}
				if (EditorGUI.EndChangeCheck())
				{
					PlayerSettings.SetPlatformIcons(iconFieldGroup.targetGroup, key.m_Kind, iconFieldGroup.m_PlatformIconsByKind[key.m_Kind]);
				}
			}
		}

		internal static void ShowApplicationIdentifierUI(SerializedObject serializedObject, BuildTargetGroup targetGroup, string label, string tooltip, string undoText)
		{
			EditorGUI.BeginChangeCheck();
			string identifier = EditorGUILayout.TextField(EditorGUIUtility.TrTextContent(label, tooltip, null), PlayerSettings.GetApplicationIdentifier(targetGroup), new GUILayoutOption[0]);
			if (EditorGUI.EndChangeCheck())
			{
				Undo.RecordObject(serializedObject.targetObject, undoText);
				PlayerSettings.SetApplicationIdentifier(targetGroup, identifier);
			}
		}

		internal static void ShowBuildNumberUI(SerializedObject serializedObject, BuildTargetGroup targetGroup, string label, string tooltip, string undoText)
		{
			EditorGUI.BeginChangeCheck();
			string buildNumber = EditorGUILayout.TextField(EditorGUIUtility.TrTextContent(label, tooltip, null), PlayerSettings.GetBuildNumber(targetGroup), new GUILayoutOption[0]);
			if (EditorGUI.EndChangeCheck())
			{
				Undo.RecordObject(serializedObject.targetObject, undoText);
				PlayerSettings.SetBuildNumber(targetGroup, buildNumber);
			}
		}

		private void OtherSectionConfigurationGUI(BuildTargetGroup targetGroup, ISettingEditorExtension settingsExtension)
		{
			GUILayout.Label(PlayerSettingsEditor.Styles.configurationTitle, EditorStyles.boldLabel, new GUILayoutOption[0]);
			ScriptingRuntimeVersion[] options = new ScriptingRuntimeVersion[]
			{
				ScriptingRuntimeVersion.Legacy,
				ScriptingRuntimeVersion.Latest
			};
			GUIContent[] optionNames = new GUIContent[]
			{
				PlayerSettingsEditor.Styles.scriptingRuntimeVersionLegacy,
				PlayerSettingsEditor.Styles.scriptingRuntimeVersionLatest
			};
			ScriptingRuntimeVersion scriptingRuntimeVersion = PlayerSettings.scriptingRuntimeVersion;
			if (EditorApplication.isPlaying)
			{
				GUIContent selected = (PlayerSettings.scriptingRuntimeVersion != ScriptingRuntimeVersion.Legacy) ? PlayerSettingsEditor.Styles.scriptingRuntimeVersionLatest : PlayerSettingsEditor.Styles.scriptingRuntimeVersionLegacy;
				PlayerSettingsEditor.BuildDisabledEnumPopup(selected, PlayerSettingsEditor.Styles.scriptingRuntimeVersion);
			}
			else
			{
				scriptingRuntimeVersion = PlayerSettingsEditor.BuildEnumPopup<ScriptingRuntimeVersion>(PlayerSettingsEditor.Styles.scriptingRuntimeVersion, PlayerSettings.scriptingRuntimeVersion, options, optionNames);
			}
			if (scriptingRuntimeVersion != PlayerSettings.scriptingRuntimeVersion)
			{
				if (EditorUtility.DisplayDialog(LocalizationDatabase.GetLocalizedString("Restart required"), LocalizationDatabase.GetLocalizedString("Changing scripting runtime version requires a restart of the Editor to take effect. Do you wish to proceed?"), LocalizationDatabase.GetLocalizedString("Restart"), LocalizationDatabase.GetLocalizedString("Cancel")))
				{
					PlayerSettings.scriptingRuntimeVersion = scriptingRuntimeVersion;
					EditorCompilationInterface.Instance.CleanScriptAssemblies();
					if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
					{
						EditorApplication.OpenProject(Environment.CurrentDirectory, new string[0]);
					}
				}
			}
			IScriptingImplementations scriptingImplementations = ModuleManager.GetScriptingImplementations(targetGroup);
			bool flag = false;
			bool flag2 = false;
			if (scriptingImplementations == null)
			{
				PlayerSettingsEditor.BuildDisabledEnumPopup(PlayerSettingsEditor.Styles.scriptingDefault, PlayerSettingsEditor.Styles.scriptingBackend);
			}
			else
			{
				ScriptingImplementation[] array = scriptingImplementations.Enabled();
				ScriptingImplementation[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					ScriptingImplementation scriptingImplementation = array2[i];
					if (scriptingImplementation == ScriptingImplementation.IL2CPP)
					{
						flag = true;
						break;
					}
				}
				ScriptingImplementation scriptingBackend = PlayerSettings.GetScriptingBackend(targetGroup);
				flag2 = (scriptingBackend == ScriptingImplementation.IL2CPP);
				ScriptingImplementation scriptingImplementation2;
				if (targetGroup == BuildTargetGroup.tvOS)
				{
					scriptingImplementation2 = ScriptingImplementation.IL2CPP;
					PlayerSettingsEditor.BuildDisabledEnumPopup(PlayerSettingsEditor.Styles.scriptingIL2CPP, PlayerSettingsEditor.Styles.scriptingBackend);
				}
				else
				{
					scriptingImplementation2 = PlayerSettingsEditor.BuildEnumPopup<ScriptingImplementation>(PlayerSettingsEditor.Styles.scriptingBackend, scriptingBackend, array, PlayerSettingsEditor.GetNiceScriptingBackendNames(array));
				}
				if (targetGroup == BuildTargetGroup.iPhone && scriptingImplementation2 == ScriptingImplementation.Mono2x)
				{
					EditorGUILayout.HelpBox(PlayerSettingsEditor.Styles.monoNotSupportediOS11WarningGUIContent.text, MessageType.Warning);
				}
				if (scriptingImplementation2 != scriptingBackend)
				{
					PlayerSettings.SetScriptingBackend(targetGroup, scriptingImplementation2);
				}
			}
			ApiCompatibilityLevel apiCompatibilityLevel = PlayerSettings.GetApiCompatibilityLevel(targetGroup);
			ApiCompatibilityLevel[] availableApiCompatibilityLevels = this.GetAvailableApiCompatibilityLevels(targetGroup);
			ApiCompatibilityLevel apiCompatibilityLevel2 = PlayerSettingsEditor.BuildEnumPopup<ApiCompatibilityLevel>(PlayerSettingsEditor.Styles.apiCompatibilityLevel, apiCompatibilityLevel, availableApiCompatibilityLevels, PlayerSettingsEditor.GetNiceApiCompatibilityLevelNames(availableApiCompatibilityLevels));
			if (apiCompatibilityLevel != apiCompatibilityLevel2)
			{
				PlayerSettings.SetApiCompatibilityLevel(targetGroup, apiCompatibilityLevel2);
			}
			if (flag)
			{
				using (new EditorGUI.DisabledScope(!flag2))
				{
					Il2CppCompilerConfiguration il2CppCompilerConfiguration = PlayerSettings.GetIl2CppCompilerConfiguration(targetGroup);
					Il2CppCompilerConfiguration[] il2CppCompilerConfigurations = this.GetIl2CppCompilerConfigurations();
					GUIContent[] il2CppCompilerConfigurationNames = this.GetIl2CppCompilerConfigurationNames();
					Il2CppCompilerConfiguration il2CppCompilerConfiguration2 = PlayerSettingsEditor.BuildEnumPopup<Il2CppCompilerConfiguration>(PlayerSettingsEditor.Styles.il2cppCompilerConfiguration, il2CppCompilerConfiguration, il2CppCompilerConfigurations, il2CppCompilerConfigurationNames);
					if (il2CppCompilerConfiguration != il2CppCompilerConfiguration2)
					{
						PlayerSettings.SetIl2CppCompilerConfiguration(targetGroup, il2CppCompilerConfiguration2);
					}
				}
			}
			bool flag3 = targetGroup == BuildTargetGroup.iPhone || targetGroup == BuildTargetGroup.tvOS || targetGroup == BuildTargetGroup.Android || targetGroup == BuildTargetGroup.WSA;
			if (flag3)
			{
				if (targetGroup == BuildTargetGroup.iPhone || targetGroup == BuildTargetGroup.tvOS)
				{
					EditorGUILayout.PropertyField(this.m_useOnDemandResources, PlayerSettingsEditor.Styles.useOnDemandResources, new GUILayoutOption[0]);
				}
				bool flag4 = targetGroup == BuildTargetGroup.iPhone || targetGroup == BuildTargetGroup.tvOS || targetGroup == BuildTargetGroup.WSA;
				if (flag4)
				{
					EditorGUILayout.PropertyField(this.m_AccelerometerFrequency, PlayerSettingsEditor.Styles.accelerometerFrequency, new GUILayoutOption[0]);
				}
				if (targetGroup == BuildTargetGroup.iPhone || targetGroup == BuildTargetGroup.tvOS)
				{
					EditorGUILayout.PropertyField(this.m_CameraUsageDescription, PlayerSettingsEditor.Styles.cameraUsageDescription, new GUILayoutOption[0]);
					EditorGUILayout.PropertyField(this.m_LocationUsageDescription, PlayerSettingsEditor.Styles.locationUsageDescription, new GUILayoutOption[0]);
					EditorGUILayout.PropertyField(this.m_MicrophoneUsageDescription, PlayerSettingsEditor.Styles.microphoneUsageDescription, new GUILayoutOption[0]);
				}
				if (targetGroup == BuildTargetGroup.iPhone || targetGroup == BuildTargetGroup.tvOS || targetGroup == BuildTargetGroup.Android)
				{
					EditorGUILayout.PropertyField(this.m_MuteOtherAudioSources, PlayerSettingsEditor.Styles.muteOtherAudioSources, new GUILayoutOption[0]);
				}
				if (targetGroup == BuildTargetGroup.iPhone || targetGroup == BuildTargetGroup.tvOS)
				{
					if (targetGroup == BuildTargetGroup.iPhone)
					{
						EditorGUILayout.PropertyField(this.m_PrepareIOSForRecording, PlayerSettingsEditor.Styles.prepareIOSForRecording, new GUILayoutOption[0]);
						EditorGUILayout.PropertyField(this.m_ForceIOSSpeakersWhenRecording, PlayerSettingsEditor.Styles.forceIOSSpeakersWhenRecording, new GUILayoutOption[0]);
					}
					EditorGUILayout.PropertyField(this.m_UIRequiresPersistentWiFi, PlayerSettingsEditor.Styles.UIRequiresPersistentWiFi, new GUILayoutOption[0]);
					EditorGUILayout.PropertyField(this.m_IOSAllowHTTPDownload, PlayerSettingsEditor.Styles.iOSAllowHTTPDownload, new GUILayoutOption[0]);
					EditorGUILayout.PropertyField(this.m_IOSURLSchemes, PlayerSettingsEditor.Styles.iOSURLSchemes, true, new GUILayoutOption[0]);
				}
			}
			using (new EditorGUI.DisabledScope(!Application.HasProLicense()))
			{
				bool flag5 = !this.m_SubmitAnalytics.boolValue;
				bool flag6 = EditorGUILayout.Toggle(PlayerSettingsEditor.Styles.disableStatistics, flag5, new GUILayoutOption[0]);
				if (flag5 != flag6)
				{
					this.m_SubmitAnalytics.boolValue = !flag6;
					EditorAnalytics.SendEventServiceInfo(new PlayerSettingsEditor.HwStatsServiceState
					{
						hwstats = !flag6
					});
				}
				if (!Application.HasProLicense())
				{
					this.m_SubmitAnalytics.boolValue = true;
				}
			}
			if (settingsExtension != null)
			{
				settingsExtension.ConfigurationSectionGUI();
			}
			EditorGUILayout.LabelField(PlayerSettingsEditor.Styles.scriptingDefineSymbols, new GUILayoutOption[0]);
			EditorGUI.BeginChangeCheck();
			string defines = EditorGUILayout.DelayedTextField(PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup), EditorStyles.textField, new GUILayoutOption[0]);
			this.scriptingDefinesControlID = EditorGUIUtility.s_LastControlID;
			if (EditorGUI.EndChangeCheck())
			{
				PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, defines);
			}
			int num = this.m_EnableInputSystem.boolValue ? ((!this.m_DisableInputManager.boolValue) ? 2 : 1) : 0;
			int num2 = num;
			EditorGUI.BeginChangeCheck();
			num = EditorGUILayout.Popup(PlayerSettingsEditor.Styles.activeInputHandling, num, PlayerSettingsEditor.Styles.activeInputHandlingOptions, new GUILayoutOption[0]);
			if (EditorGUI.EndChangeCheck())
			{
				if (num != num2)
				{
					EditorUtility.DisplayDialog("Unity editor restart required", "The Unity editor must be restarted for this change to take effect.", "OK");
					this.m_EnableInputSystem.boolValue = (num == 1 || num == 2);
					this.m_DisableInputManager.boolValue = (num != 0 && num != 2);
					this.m_EnableInputSystem.serializedObject.ApplyModifiedProperties();
				}
				GUIUtility.ExitGUI();
			}
			EditorGUILayout.Space();
		}

		private void OtherSectionOptimizationGUI(BuildTargetGroup targetGroup)
		{
			GUILayout.Label(PlayerSettingsEditor.Styles.optimizationTitle, EditorStyles.boldLabel, new GUILayoutOption[0]);
			EditorGUILayout.PropertyField(this.m_BakeCollisionMeshes, PlayerSettingsEditor.Styles.bakeCollisionMeshes, new GUILayoutOption[0]);
			EditorGUILayout.PropertyField(this.m_KeepLoadedShadersAlive, PlayerSettingsEditor.Styles.keepLoadedShadersAlive, new GUILayoutOption[0]);
			EditorGUILayout.PropertyField(this.m_PreloadedAssets, PlayerSettingsEditor.Styles.preloadedAssets, true, new GUILayoutOption[0]);
			bool flag = targetGroup == BuildTargetGroup.iPhone || targetGroup == BuildTargetGroup.tvOS || targetGroup == BuildTargetGroup.XboxOne || targetGroup == BuildTargetGroup.PS4 || targetGroup == BuildTargetGroup.PSP2;
			if (flag)
			{
				EditorGUILayout.PropertyField(this.m_AotOptions, PlayerSettingsEditor.Styles.aotOptions, new GUILayoutOption[0]);
			}
			bool flag2 = targetGroup == BuildTargetGroup.iPhone || targetGroup == BuildTargetGroup.tvOS || targetGroup == BuildTargetGroup.Android || targetGroup == BuildTargetGroup.Tizen || targetGroup == BuildTargetGroup.WebGL || targetGroup == BuildTargetGroup.PSP2 || targetGroup == BuildTargetGroup.PS4 || targetGroup == BuildTargetGroup.XboxOne || targetGroup == BuildTargetGroup.WSA;
			if (flag2)
			{
				ScriptingImplementation scriptingBackend = PlayerSettings.GetScriptingBackend(targetGroup);
				if (targetGroup == BuildTargetGroup.WebGL || scriptingBackend == ScriptingImplementation.IL2CPP)
				{
					EditorGUILayout.PropertyField(this.m_StripEngineCode, PlayerSettingsEditor.Styles.stripEngineCode, new GUILayoutOption[0]);
				}
				else if (scriptingBackend != ScriptingImplementation.WinRTDotNET)
				{
					EditorGUILayout.PropertyField(this.m_IPhoneStrippingLevel, PlayerSettingsEditor.Styles.iPhoneStrippingLevel, new GUILayoutOption[0]);
				}
			}
			if (targetGroup == BuildTargetGroup.iPhone || targetGroup == BuildTargetGroup.tvOS)
			{
				EditorGUILayout.PropertyField(this.m_IPhoneScriptCallOptimization, PlayerSettingsEditor.Styles.iPhoneScriptCallOptimization, new GUILayoutOption[0]);
			}
			if (targetGroup == BuildTargetGroup.Android)
			{
				EditorGUILayout.PropertyField(this.m_AndroidProfiler, PlayerSettingsEditor.Styles.enableInternalProfiler, new GUILayoutOption[0]);
			}
			EditorGUILayout.Space();
			VertexChannelCompressionFlags vertexChannelCompressionFlags = (VertexChannelCompressionFlags)this.m_VertexChannelCompressionMask.intValue;
			vertexChannelCompressionFlags = (VertexChannelCompressionFlags)EditorGUILayout.EnumFlagsField(PlayerSettingsEditor.Styles.vertexChannelCompressionMask, vertexChannelCompressionFlags, new GUILayoutOption[0]);
			this.m_VertexChannelCompressionMask.intValue = (int)vertexChannelCompressionFlags;
			EditorGUILayout.PropertyField(this.m_StripUnusedMeshComponents, PlayerSettingsEditor.Styles.stripUnusedMeshComponents, new GUILayoutOption[0]);
			if (targetGroup == BuildTargetGroup.PSP2)
			{
				EditorGUI.BeginChangeCheck();
				EditorGUILayout.PropertyField(this.m_VideoMemoryForVertexBuffers, PlayerSettingsEditor.Styles.videoMemoryForVertexBuffers, new GUILayoutOption[0]);
				if (EditorGUI.EndChangeCheck())
				{
					if (this.m_VideoMemoryForVertexBuffers.intValue < 0)
					{
						this.m_VideoMemoryForVertexBuffers.intValue = 0;
					}
					else if (this.m_VideoMemoryForVertexBuffers.intValue > 192)
					{
						this.m_VideoMemoryForVertexBuffers.intValue = 192;
					}
				}
			}
			EditorGUILayout.Space();
		}

		private ApiCompatibilityLevel[] GetAvailableApiCompatibilityLevels(BuildTargetGroup activeBuildTargetGroup)
		{
			ApiCompatibilityLevel[] result;
			if (EditorApplication.scriptingRuntimeVersion == ScriptingRuntimeVersion.Latest)
			{
				result = PlayerSettingsEditor.only_4_x_profiles;
			}
			else if (activeBuildTargetGroup == BuildTargetGroup.WSA || activeBuildTargetGroup == BuildTargetGroup.XboxOne)
			{
				result = PlayerSettingsEditor.allProfiles;
			}
			else
			{
				result = PlayerSettingsEditor.only_2_0_profiles;
			}
			return result;
		}

		private Il2CppCompilerConfiguration[] GetIl2CppCompilerConfigurations()
		{
			if (PlayerSettingsEditor.m_Il2cppCompilerConfigurations == null)
			{
				PlayerSettingsEditor.m_Il2cppCompilerConfigurations = new Il2CppCompilerConfiguration[]
				{
					Il2CppCompilerConfiguration.Debug,
					Il2CppCompilerConfiguration.Release
				};
			}
			return PlayerSettingsEditor.m_Il2cppCompilerConfigurations;
		}

		private GUIContent[] GetIl2CppCompilerConfigurationNames()
		{
			if (PlayerSettingsEditor.m_Il2cppCompilerConfigurationNames == null)
			{
				Il2CppCompilerConfiguration[] il2CppCompilerConfigurations = this.GetIl2CppCompilerConfigurations();
				PlayerSettingsEditor.m_Il2cppCompilerConfigurationNames = new GUIContent[il2CppCompilerConfigurations.Length];
				for (int i = 0; i < il2CppCompilerConfigurations.Length; i++)
				{
					PlayerSettingsEditor.m_Il2cppCompilerConfigurationNames[i] = EditorGUIUtility.TextContent(il2CppCompilerConfigurations[i].ToString());
				}
			}
			return PlayerSettingsEditor.m_Il2cppCompilerConfigurationNames;
		}

		public static bool IsLatestApiCompatibility(ApiCompatibilityLevel level)
		{
			return level == ApiCompatibilityLevel.NET_4_6 || level == ApiCompatibilityLevel.NET_Standard_2_0;
		}

		private void OtherSectionLoggingGUI()
		{
			GUILayout.Label(PlayerSettingsEditor.Styles.loggingTitle, EditorStyles.boldLabel, new GUILayoutOption[0]);
			GUILayout.BeginVertical(new GUILayoutOption[0]);
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.Label("Log Type", new GUILayoutOption[0]);
			IEnumerator enumerator = Enum.GetValues(typeof(StackTraceLogType)).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					StackTraceLogType stackTraceLogType = (StackTraceLogType)enumerator.Current;
					GUILayout.Label(stackTraceLogType.ToString(), new GUILayoutOption[]
					{
						GUILayout.Width(70f)
					});
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
			GUILayout.EndHorizontal();
			IEnumerator enumerator2 = Enum.GetValues(typeof(LogType)).GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					LogType logType = (LogType)enumerator2.Current;
					GUILayout.BeginHorizontal(new GUILayoutOption[0]);
					GUILayout.Label(logType.ToString(), new GUILayoutOption[0]);
					IEnumerator enumerator3 = Enum.GetValues(typeof(StackTraceLogType)).GetEnumerator();
					try
					{
						while (enumerator3.MoveNext())
						{
							StackTraceLogType stackTraceLogType2 = (StackTraceLogType)enumerator3.Current;
							StackTraceLogType stackTraceLogType3 = PlayerSettings.GetStackTraceLogType(logType);
							EditorGUI.BeginChangeCheck();
							bool flag = EditorGUILayout.ToggleLeft(" ", stackTraceLogType3 == stackTraceLogType2, new GUILayoutOption[]
							{
								GUILayout.Width(70f)
							});
							if (EditorGUI.EndChangeCheck() && flag)
							{
								PlayerSettings.SetStackTraceLogType(logType, stackTraceLogType2);
							}
						}
					}
					finally
					{
						IDisposable disposable2;
						if ((disposable2 = (enumerator3 as IDisposable)) != null)
						{
							disposable2.Dispose();
						}
					}
					GUILayout.EndHorizontal();
				}
			}
			finally
			{
				IDisposable disposable3;
				if ((disposable3 = (enumerator2 as IDisposable)) != null)
				{
					disposable3.Dispose();
				}
			}
			GUILayout.EndVertical();
		}

		private static GUIContent[] GetGUIContentsForValues<T>(Dictionary<T, GUIContent> contents, T[] values)
		{
			GUIContent[] array = new GUIContent[values.Length];
			for (int i = 0; i < values.Length; i++)
			{
				if (!contents.ContainsKey(values[i]))
				{
					throw new NotImplementedException(string.Format("Missing name for {0}", values[i]));
				}
				array[i] = contents[values[i]];
			}
			return array;
		}

		private static GUIContent[] GetNiceScriptingBackendNames(ScriptingImplementation[] scriptingBackends)
		{
			if (PlayerSettingsEditor.m_NiceScriptingBackendNames == null)
			{
				PlayerSettingsEditor.m_NiceScriptingBackendNames = new Dictionary<ScriptingImplementation, GUIContent>
				{
					{
						ScriptingImplementation.Mono2x,
						PlayerSettingsEditor.Styles.scriptingMono2x
					},
					{
						ScriptingImplementation.WinRTDotNET,
						PlayerSettingsEditor.Styles.scriptingWinRTDotNET
					},
					{
						ScriptingImplementation.IL2CPP,
						PlayerSettingsEditor.Styles.scriptingIL2CPP
					}
				};
			}
			return PlayerSettingsEditor.GetGUIContentsForValues<ScriptingImplementation>(PlayerSettingsEditor.m_NiceScriptingBackendNames, scriptingBackends);
		}

		private static GUIContent[] GetNiceApiCompatibilityLevelNames(ApiCompatibilityLevel[] apiCompatibilityLevels)
		{
			if (PlayerSettingsEditor.m_NiceApiCompatibilityLevelNames == null)
			{
				PlayerSettingsEditor.m_NiceApiCompatibilityLevelNames = new Dictionary<ApiCompatibilityLevel, GUIContent>
				{
					{
						ApiCompatibilityLevel.NET_2_0,
						PlayerSettingsEditor.Styles.apiCompatibilityLevel_NET_2_0
					},
					{
						ApiCompatibilityLevel.NET_2_0_Subset,
						PlayerSettingsEditor.Styles.apiCompatibilityLevel_NET_2_0_Subset
					},
					{
						ApiCompatibilityLevel.NET_4_6,
						PlayerSettingsEditor.Styles.apiCompatibilityLevel_NET_4_6
					},
					{
						ApiCompatibilityLevel.NET_Standard_2_0,
						PlayerSettingsEditor.Styles.apiCompatibilityLevel_NET_Standard_2_0
					}
				};
			}
			return PlayerSettingsEditor.GetGUIContentsForValues<ApiCompatibilityLevel>(PlayerSettingsEditor.m_NiceApiCompatibilityLevelNames, apiCompatibilityLevels);
		}

		private void AutoAssignProperty(SerializedProperty property, string packageDir, string fileName)
		{
			if (property.stringValue.Length == 0 || !File.Exists(Path.Combine(packageDir, property.stringValue)))
			{
				string path = Path.Combine(packageDir, fileName);
				if (File.Exists(path))
				{
					property.stringValue = fileName;
				}
			}
		}

		public void BrowseablePathProperty(string propertyLabel, SerializedProperty property, string browsePanelTitle, string extension, string dir)
		{
			EditorGUILayout.BeginHorizontal(new GUILayoutOption[0]);
			EditorGUILayout.PrefixLabel(EditorGUIUtility.TextContent(propertyLabel));
			GUIContent content = EditorGUIUtility.TrTextContent("...", null, null);
			Vector2 vector = GUI.skin.GetStyle("Button").CalcSize(content);
			if (GUILayout.Button(content, EditorStyles.miniButton, new GUILayoutOption[]
			{
				GUILayout.MaxWidth(vector.x)
			}))
			{
				GUI.FocusControl("");
				string text = EditorGUIUtility.TempContent(browsePanelTitle).text;
				string text2 = (!string.IsNullOrEmpty(dir)) ? (dir.Replace('\\', '/') + "/") : (Directory.GetCurrentDirectory().Replace('\\', '/') + "/");
				string text3;
				if (string.IsNullOrEmpty(extension))
				{
					text3 = EditorUtility.OpenFolderPanel(text, text2, "");
				}
				else
				{
					text3 = EditorUtility.OpenFilePanel(text, text2, extension);
				}
				if (text3.StartsWith(text2))
				{
					text3 = text3.Substring(text2.Length);
				}
				if (!string.IsNullOrEmpty(text3))
				{
					property.stringValue = text3;
					base.serializedObject.ApplyModifiedProperties();
				}
			}
			bool flag = string.IsNullOrEmpty(property.stringValue);
			using (new EditorGUI.DisabledScope(flag))
			{
				GUIContent gUIContent;
				if (flag)
				{
					gUIContent = EditorGUIUtility.TrTextContent("Not selected.", null, null);
				}
				else
				{
					gUIContent = EditorGUIUtility.TempContent(property.stringValue);
				}
				EditorGUI.BeginChangeCheck();
				GUILayoutOption[] options = new GUILayoutOption[]
				{
					GUILayout.Width(32f),
					GUILayout.ExpandWidth(true)
				};
				string value = EditorGUILayout.TextArea(gUIContent.text, options);
				if (EditorGUI.EndChangeCheck())
				{
					if (string.IsNullOrEmpty(value))
					{
						property.stringValue = "";
						base.serializedObject.ApplyModifiedProperties();
						GUI.FocusControl("");
					}
				}
			}
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Space();
		}

		internal static bool BuildPathBoxButton(SerializedProperty prop, string uiString, string directory)
		{
			return PlayerSettingsEditor.BuildPathBoxButton(prop, uiString, directory, null);
		}

		internal static bool BuildPathBoxButton(SerializedProperty prop, string uiString, string directory, Action onSelect)
		{
			float num = 16f;
			float minWidth = 80f + EditorGUIUtility.fieldWidth + 5f;
			float maxWidth = 80f + EditorGUIUtility.fieldWidth + 5f;
			Rect rect = GUILayoutUtility.GetRect(minWidth, maxWidth, num, num, EditorStyles.layerMaskField, null);
			float labelWidth = EditorGUIUtility.labelWidth;
			Rect position = new Rect(rect.x + EditorGUI.indent, rect.y, labelWidth - EditorGUI.indent, rect.height);
			Rect position2 = new Rect(rect.x + labelWidth, rect.y, rect.width - labelWidth, rect.height);
			string text = (prop.stringValue.Length != 0) ? prop.stringValue : "Not selected.";
			EditorGUI.TextArea(position2, text, EditorStyles.label);
			bool result = false;
			if (GUI.Button(position, EditorGUIUtility.TextContent(uiString)))
			{
				string stringValue = prop.stringValue;
				string text2 = EditorUtility.OpenFolderPanel(EditorGUIUtility.TextContent(uiString).text, directory, "");
				string projectRelativePath = FileUtil.GetProjectRelativePath(text2);
				prop.stringValue = ((!(projectRelativePath != string.Empty)) ? text2 : projectRelativePath);
				result = (prop.stringValue != stringValue);
				if (onSelect != null)
				{
					onSelect();
				}
				prop.serializedObject.ApplyModifiedProperties();
			}
			return result;
		}

		internal static bool BuildFileBoxButton(SerializedProperty prop, string uiString, string directory, string ext)
		{
			return PlayerSettingsEditor.BuildFileBoxButton(prop, uiString, directory, ext, null);
		}

		internal static bool BuildFileBoxButton(SerializedProperty prop, string uiString, string directory, string ext, Action onSelect)
		{
			float num = 16f;
			float minWidth = 80f + EditorGUIUtility.fieldWidth + 5f;
			float maxWidth = 80f + EditorGUIUtility.fieldWidth + 5f;
			Rect rect = GUILayoutUtility.GetRect(minWidth, maxWidth, num, num, EditorStyles.layerMaskField, null);
			float labelWidth = EditorGUIUtility.labelWidth;
			Rect position = new Rect(rect.x + EditorGUI.indent, rect.y, labelWidth - EditorGUI.indent, rect.height);
			Rect position2 = new Rect(rect.x + labelWidth, rect.y, rect.width - labelWidth, rect.height);
			string text = (prop.stringValue.Length != 0) ? prop.stringValue : "Not selected.";
			EditorGUI.TextArea(position2, text, EditorStyles.label);
			bool result = false;
			if (GUI.Button(position, EditorGUIUtility.TextContent(uiString)))
			{
				string stringValue = prop.stringValue;
				string text2 = EditorUtility.OpenFilePanel(EditorGUIUtility.TextContent(uiString).text, directory, ext);
				string projectRelativePath = FileUtil.GetProjectRelativePath(text2);
				prop.stringValue = ((!(projectRelativePath != string.Empty)) ? text2 : projectRelativePath);
				result = (prop.stringValue != stringValue);
				if (onSelect != null)
				{
					onSelect();
				}
				prop.serializedObject.ApplyModifiedProperties();
			}
			return result;
		}

		public void PublishSectionGUI(BuildTargetGroup targetGroup, ISettingEditorExtension settingsExtension, int sectionIndex = 5)
		{
			if (targetGroup == BuildTargetGroup.WSA || targetGroup == BuildTargetGroup.PSP2 || (settingsExtension != null && settingsExtension.HasPublishSection()))
			{
				if (this.BeginSettingsBox(sectionIndex, PlayerSettingsEditor.Styles.publishingSettingsTitle))
				{
					float h = 16f;
					float midWidth = 80f + EditorGUIUtility.fieldWidth + 5f;
					float maxWidth = 80f + EditorGUIUtility.fieldWidth + 5f;
					if (settingsExtension != null)
					{
						settingsExtension.PublishSectionGUI(h, midWidth, maxWidth);
					}
				}
				this.EndSettingsBox();
			}
		}

		private static void ShowWarning(GUIContent warningMessage)
		{
			if (PlayerSettingsEditor.s_WarningIcon == null)
			{
				PlayerSettingsEditor.s_WarningIcon = EditorGUIUtility.LoadIcon("console.warnicon");
			}
			warningMessage.image = PlayerSettingsEditor.s_WarningIcon;
			GUILayout.Space(5f);
			GUILayout.BeginVertical(EditorStyles.helpBox, new GUILayoutOption[0]);
			GUILayout.Label(warningMessage, EditorStyles.wordWrappedMiniLabel, new GUILayoutOption[0]);
			GUILayout.EndVertical();
		}

		protected override bool ShouldHideOpenButton()
		{
			return true;
		}
	}
}
