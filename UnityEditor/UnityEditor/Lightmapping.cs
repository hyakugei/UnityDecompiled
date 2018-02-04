using System;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Scripting;

namespace UnityEditor
{
	public sealed class Lightmapping
	{
		internal enum ConcurrentJobsType
		{
			Min,
			Low,
			High
		}

		public enum GIWorkflowMode
		{
			Iterative,
			OnDemand,
			Legacy
		}

		public delegate void OnStartedFunction();

		public delegate void OnCompletedFunction();

		public static Lightmapping.OnCompletedFunction completed;

		public static event Lightmapping.OnStartedFunction started
		{
			add
			{
				Lightmapping.OnStartedFunction onStartedFunction = Lightmapping.started;
				Lightmapping.OnStartedFunction onStartedFunction2;
				do
				{
					onStartedFunction2 = onStartedFunction;
					onStartedFunction = Interlocked.CompareExchange<Lightmapping.OnStartedFunction>(ref Lightmapping.started, (Lightmapping.OnStartedFunction)Delegate.Combine(onStartedFunction2, value), onStartedFunction);
				}
				while (onStartedFunction != onStartedFunction2);
			}
			remove
			{
				Lightmapping.OnStartedFunction onStartedFunction = Lightmapping.started;
				Lightmapping.OnStartedFunction onStartedFunction2;
				do
				{
					onStartedFunction2 = onStartedFunction;
					onStartedFunction = Interlocked.CompareExchange<Lightmapping.OnStartedFunction>(ref Lightmapping.started, (Lightmapping.OnStartedFunction)Delegate.Remove(onStartedFunction2, value), onStartedFunction);
				}
				while (onStartedFunction != onStartedFunction2);
			}
		}

		public static extern Lightmapping.GIWorkflowMode giWorkflowMode
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern bool realtimeGI
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern bool bakedGI
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern float indirectOutputScale
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern float bounceBoost
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		internal static extern Lightmapping.ConcurrentJobsType concurrentJobsType
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		internal static extern long diskCacheSize
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		internal static extern string diskCachePath
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		internal static extern bool enlightenForceWhiteAlbedo
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		internal static extern bool enlightenForceUpdates
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		internal static extern FilterMode filterMode
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		internal static extern bool isProgressiveLightmapperDone
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		internal static extern ulong occupiedTexelCount
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public static extern bool isRunning
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public static extern float buildProgress
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public static extern LightingDataAsset lightingDataAsset
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		[Obsolete("lightmapSnapshot has been deprecated. Use lightingDataAsset instead (UnityUpgradable) -> lightingDataAsset", true)]
		public static LightmapSnapshot lightmapSnapshot
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void ClearDiskCache();

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void UpdateCachePath();

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern ulong GetVisibleTexelCount(int lightmapIndex);

		internal static LightmapConvergence GetLightmapConvergence(int lightmapIndex)
		{
			LightmapConvergence result;
			Lightmapping.INTERNAL_CALL_GetLightmapConvergence(lightmapIndex, out result);
			return result;
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_GetLightmapConvergence(int lightmapIndex, out LightmapConvergence value);

		internal static LightmapMemory GetLightmapMemory(int lightmapIndex)
		{
			LightmapMemory result;
			Lightmapping.INTERNAL_CALL_GetLightmapMemory(lightmapIndex, out result);
			return result;
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_GetLightmapMemory(int lightmapIndex, out LightmapMemory value);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool GetGBufferHash(int lightmapIndex, out Hash128 gbufferHash);

		internal static float GetGBufferMemory(ref Hash128 gbufferHash)
		{
			return Lightmapping.INTERNAL_CALL_GetGBufferMemory(ref gbufferHash);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern float INTERNAL_CALL_GetGBufferMemory(ref Hash128 gbufferHash);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void GetTransmissionTexturesMemLabels(out string[] labels, out float[] sizes);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void ResetExplicitlyShownMemLabels();

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void GetNotShownMemLabels(out string[] labels, out float[] sizes);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void GetGeometryMemory(out string[] labels, out float[] sizes, out ulong[] triCounts);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern float ComputeTotalMemoryUsageInMB();

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern float GetLightmapBakeTimeRaw();

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern float GetLightmapBakeTimeTotal();

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern float GetLightmapBakePerformanceTotal();

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern float GetLightmapBakePerformance(int lightmapIndex);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void PrintStateToConsole();

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool BakeAsync();

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool Bake();

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Cancel();

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void ForceStop();

		private static void Internal_CallStartedFunctions()
		{
			if (Lightmapping.started != null)
			{
				Lightmapping.started();
			}
		}

		private static void Internal_CallCompletedFunctions()
		{
			if (Lightmapping.completed != null)
			{
				Lightmapping.completed();
			}
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Clear();

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void ClearLightingDataAsset();

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Tetrahedralize(Vector3[] positions, out int[] outIndices, out Vector3[] outPositions);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool BakeReflectionProbe(ReflectionProbe probe, string path);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool BakeReflectionProbeSnapshot(ReflectionProbe probe);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool BakeAllReflectionProbesSnapshots();

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void OnUpdateLightmapEncoding(BuildTargetGroup target);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void GetTerrainGIChunks(Terrain terrain, ref int numChunksX, ref int numChunksY);

		public static void BakeMultipleScenes(string[] paths)
		{
			if (paths.Length != 0)
			{
				for (int i = 0; i < paths.Length; i++)
				{
					for (int j = i + 1; j < paths.Length; j++)
					{
						if (paths[i] == paths[j])
						{
							throw new Exception("no duplication of scenes is allowed");
						}
					}
				}
				if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
				{
					SceneSetup[] sceneSetup = EditorSceneManager.GetSceneManagerSetup();
					Lightmapping.OnCompletedFunction OnBakeFinish = null;
					OnBakeFinish = delegate
					{
						EditorSceneManager.SaveOpenScenes();
						EditorSceneManager.RestoreSceneManagerSetup(sceneSetup);
						Lightmapping.completed = (Lightmapping.OnCompletedFunction)Delegate.Remove(Lightmapping.completed, OnBakeFinish);
					};
					EditorSceneManager.SceneOpenedCallback BakeOnAllOpen = null;
					BakeOnAllOpen = delegate(Scene scene, OpenSceneMode loadSceneMode)
					{
						if (EditorSceneManager.loadedSceneCount == paths.Length)
						{
							Lightmapping.BakeAsync();
							Lightmapping.completed = (Lightmapping.OnCompletedFunction)Delegate.Combine(Lightmapping.completed, OnBakeFinish);
							EditorSceneManager.sceneOpened -= BakeOnAllOpen;
						}
					};
					EditorSceneManager.sceneOpened += BakeOnAllOpen;
					EditorSceneManager.OpenScene(paths[0]);
					for (int k = 1; k < paths.Length; k++)
					{
						EditorSceneManager.OpenScene(paths[k], OpenSceneMode.Additive);
					}
				}
			}
		}

		[Obsolete("BakeSelectedAsync has been deprecated. Use BakeAsync instead (UnityUpgradable) -> BakeAsync()", true)]
		public static bool BakeSelectedAsync()
		{
			return false;
		}

		[Obsolete("BakeSelected has been deprecated. Use Bake instead (UnityUpgradable) -> Bake()", true)]
		public static bool BakeSelected()
		{
			return false;
		}

		[Obsolete("BakeLightProbesOnlyAsync has been deprecated. Use BakeAsync instead (UnityUpgradable) -> BakeAsync()", true)]
		public static bool BakeLightProbesOnlyAsync()
		{
			return false;
		}

		[Obsolete("BakeLightProbesOnly has been deprecated. Use Bake instead (UnityUpgradable) -> Bake()", true)]
		public static bool BakeLightProbesOnly()
		{
			return false;
		}
	}
}
