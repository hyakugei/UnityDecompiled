using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityEditor
{
	public class EditorUserBuildSettings : UnityEngine.Object
	{
		internal static AppleBuildAndRunType appleBuildAndRunType = AppleBuildAndRunType.Xcode;

		[Obsolete("UnityEditor.activeBuildTargetChanged has been deprecated.Use UnityEditor.Build.IActiveBuildTargetChanged instead.")]
		public static Action activeBuildTargetChanged;

		public static extern BuildTargetGroup selectedBuildTargetGroup
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern BuildTarget selectedStandaloneTarget
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		internal static extern BuildTarget selectedFacebookTarget
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		internal static extern string facebookAccessToken
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern PSP2BuildSubtarget psp2BuildSubtarget
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern PS4BuildSubtarget ps4BuildSubtarget
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern PS4HardwareTarget ps4HardwareTarget
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern bool explicitNullChecks
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern bool explicitDivideByZeroChecks
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern bool explicitArrayBoundsChecks
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern bool needSubmissionMaterials
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern bool compressWithPsArc
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern bool forceInstallation
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern bool movePackageToDiscOuterEdge
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern bool compressFilesInPackage
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern bool enableHeadlessMode
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern bool buildScriptsOnly
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern XboxBuildSubtarget xboxBuildSubtarget
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern int streamingInstallLaunchRange
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern XboxOneDeployMethod xboxOneDeployMethod
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern string xboxOneUsername
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern string xboxOneNetworkSharePath
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static string xboxOneAdditionalDebugPorts
		{
			get;
			set;
		}

		public static bool xboxOneRebootIfDeployFailsAndRetry
		{
			get;
			set;
		}

		public static extern MobileTextureSubtarget androidBuildSubtarget
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern AndroidETC2Fallback androidETC2Fallback
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern AndroidBuildSystem androidBuildSystem
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern AndroidBuildType androidBuildType
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern AndroidMinification androidDebugMinification
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern AndroidMinification androidReleaseMinification
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		internal static extern string androidDeviceSocketAddress
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		internal static extern string androidCurrentDeploymentTargetId
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern WSASubtarget wsaSubtarget
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		[Obsolete("EditorUserBuildSettings.wsaSDK is obsolete and has no effect.It will be removed in a subsequent Unity release.")]
		public static extern WSASDK wsaSDK
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern WSAUWPBuildType wsaUWPBuildType
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern string wsaUWPSDK
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern string wsaUWPVisualStudioVersion
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern WSABuildAndRunDeployTarget wsaBuildAndRunDeployTarget
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern bool wsaGenerateReferenceProjects
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern MobileTextureSubtarget tizenBuildSubtarget
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern BuildTarget activeBuildTarget
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		internal static extern BuildTargetGroup activeBuildTargetGroup
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public static extern string[] activeScriptCompilationDefines
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public static extern bool development
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern bool webGLUsePreBuiltUnityEngine
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern bool connectProfiler
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern bool allowDebugging
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern bool exportAsGoogleAndroidProject
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern bool symlinkLibraries
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		internal static extern bool symlinkTrampoline
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern iOSBuildType iOSBuildConfigType
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern bool n3dsCreateCIAFile
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern bool switchCreateSolutionFile
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern bool switchCreateRomFile
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern bool switchNVNGraphicsDebugger
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern bool switchEnableDebugPad
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern bool switchRedirectWritesToHostMount
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern bool installInBuildFolder
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		[Obsolete("forceOptimizeScriptCompilation is obsolete - will always return false. Control script optimization using the 'IL2CPP optimization level' configuration in Player Settings / Other.")]
		public static bool forceOptimizeScriptCompilation
		{
			get
			{
				return false;
			}
		}

		private EditorUserBuildSettings()
		{
		}

		internal static Compression GetCompressionType(BuildTargetGroup targetGroup)
		{
			return (Compression)EditorUserBuildSettings.GetCompressionTypeInternal(targetGroup);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetCompressionTypeInternal(BuildTargetGroup targetGroup);

		internal static void SetCompressionType(BuildTargetGroup targetGroup, Compression type)
		{
			EditorUserBuildSettings.SetCompressionTypeInternal(targetGroup, (int)type);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetCompressionTypeInternal(BuildTargetGroup targetGroup, int type);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetWSADotNetNative(WSABuildType config, bool enabled);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool GetWSADotNetNative(WSABuildType config);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool SwitchActiveBuildTarget(BuildTargetGroup targetGroup, BuildTarget target);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool SwitchActiveBuildTargetAsync(BuildTargetGroup targetGroup, BuildTarget target);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern string GetBuildLocation(BuildTarget target);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetBuildLocation(BuildTarget target, string location);

		public static void SetPlatformSettings(string platformName, string name, string value)
		{
			string buildTargetGroupName = BuildPipeline.GetBuildTargetGroupName(BuildPipeline.GetBuildTargetByName(platformName));
			EditorUserBuildSettings.SetPlatformSettings(buildTargetGroupName, platformName, name, value);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetPlatformSettings(string buildTargetGroup, string buildTarget, string name, string value);

		public static string GetPlatformSettings(string platformName, string name)
		{
			string buildTargetGroupName = BuildPipeline.GetBuildTargetGroupName(BuildPipeline.GetBuildTargetByName(platformName));
			return EditorUserBuildSettings.GetPlatformSettings(buildTargetGroupName, platformName, name);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern string GetPlatformSettings(string buildTargetGroup, string platformName, string name);

		[Obsolete("Please use SwitchActiveBuildTarget(BuildTargetGroup targetGroup, BuildTarget target)")]
		public static bool SwitchActiveBuildTarget(BuildTarget target)
		{
			return EditorUserBuildSettings.SwitchActiveBuildTarget(BuildPipeline.GetBuildTargetGroup(target), target);
		}

		internal static void Internal_ActiveBuildTargetChanged()
		{
			if (EditorUserBuildSettings.activeBuildTargetChanged != null)
			{
				EditorUserBuildSettings.activeBuildTargetChanged();
			}
		}
	}
}
