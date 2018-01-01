using System;
using System.Runtime.CompilerServices;
using UnityEditor.Experimental.Build.Player;

namespace UnityEditor.Experimental.Build.AssetBundle
{
	public class BundleBuildInterface
	{
		public static BuildInput GenerateBuildInput()
		{
			BuildInput result;
			BundleBuildInterface.GenerateBuildInput_Injected(out result);
			return result;
		}

		public static SceneLoadInfo PrepareScene(string scenePath, BuildSettings settings, string outputFolder, BuildUsageTagSet usageSet)
		{
			SceneLoadInfo result;
			BundleBuildInterface.PrepareScene_Injected(scenePath, ref settings, outputFolder, usageSet, out result);
			return result;
		}

		public static ObjectIdentifier[] GetPlayerObjectIdentifiersInAsset(GUID asset, BuildTarget target)
		{
			return BundleBuildInterface.GetPlayerObjectIdentifiersInAsset_Injected(ref asset, target);
		}

		public static ObjectIdentifier[] GetPlayerDependenciesForObject(ObjectIdentifier objectID, BuildTarget target, TypeDB typeDB)
		{
			return BundleBuildInterface.GetPlayerDependenciesForObject_Injected(ref objectID, target, typeDB);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern ObjectIdentifier[] GetPlayerDependenciesForObjects(ObjectIdentifier[] objectIDs, BuildTarget target, TypeDB typeDB);

		public static void CalculateBuildUsageTags(ObjectIdentifier[] objectIDs, ObjectIdentifier[] dependentObjectIDs, BuildUsageTagGlobal globalUsage, BuildUsageTagSet usageSet)
		{
			BundleBuildInterface.CalculateBuildUsageTags_Injected(objectIDs, dependentObjectIDs, ref globalUsage, usageSet);
		}

		public static Type GetTypeForObject(ObjectIdentifier objectID)
		{
			return BundleBuildInterface.GetTypeForObject_Injected(ref objectID);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern Type[] GetTypeForObjects(ObjectIdentifier[] objectIDs);

		public static BuildOutput WriteResourceFilesForBundle(BuildCommandSet commands, string bundleName, BuildSettings settings, BuildUsageTagSet usageSet, string outputFolder)
		{
			BuildOutput result;
			BundleBuildInterface.WriteResourceFilesForBundle_Injected(commands, bundleName, ref settings, usageSet, outputFolder, out result);
			return result;
		}

		public static BuildOutput WriteResourceFilesForBundles(BuildCommandSet commands, string[] bundleNames, BuildSettings settings, BuildUsageTagSet usageSet, string outputFolder)
		{
			BuildOutput result;
			BundleBuildInterface.WriteResourceFilesForBundles_Injected(commands, bundleNames, ref settings, usageSet, outputFolder, out result);
			return result;
		}

		public static BuildOutput WriteAllResourceFiles(BuildCommandSet commands, BuildSettings settings, BuildUsageTagSet usageSet, string outputFolder)
		{
			BuildOutput result;
			BundleBuildInterface.WriteAllResourceFiles_Injected(commands, ref settings, usageSet, outputFolder, out result);
			return result;
		}

		public static uint ArchiveAndCompress(ResourceFile[] resourceFiles, string outputBundlePath, BuildCompression compression)
		{
			return BundleBuildInterface.ArchiveAndCompress_Injected(resourceFiles, outputBundlePath, ref compression);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GenerateBuildInput_Injected(out BuildInput ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void PrepareScene_Injected(string scenePath, ref BuildSettings settings, string outputFolder, BuildUsageTagSet usageSet, out SceneLoadInfo ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern ObjectIdentifier[] GetPlayerObjectIdentifiersInAsset_Injected(ref GUID asset, BuildTarget target);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern ObjectIdentifier[] GetPlayerDependenciesForObject_Injected(ref ObjectIdentifier objectID, BuildTarget target, TypeDB typeDB);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void CalculateBuildUsageTags_Injected(ObjectIdentifier[] objectIDs, ObjectIdentifier[] dependentObjectIDs, ref BuildUsageTagGlobal globalUsage, BuildUsageTagSet usageSet);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Type GetTypeForObject_Injected(ref ObjectIdentifier objectID);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void WriteResourceFilesForBundle_Injected(BuildCommandSet commands, string bundleName, ref BuildSettings settings, BuildUsageTagSet usageSet, string outputFolder, out BuildOutput ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void WriteResourceFilesForBundles_Injected(BuildCommandSet commands, string[] bundleNames, ref BuildSettings settings, BuildUsageTagSet usageSet, string outputFolder, out BuildOutput ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void WriteAllResourceFiles_Injected(BuildCommandSet commands, ref BuildSettings settings, BuildUsageTagSet usageSet, string outputFolder, out BuildOutput ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern uint ArchiveAndCompress_Injected(ResourceFile[] resourceFiles, string outputBundlePath, ref BuildCompression compression);
	}
}
