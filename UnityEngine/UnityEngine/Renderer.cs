using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine.Rendering;
using UnityEngine.Scripting;
using UnityEngineInternal;

namespace UnityEngine
{
	[RequireComponent(typeof(Transform))]
	public class Renderer : Component
	{
		[EditorBrowsable(EditorBrowsableState.Never), Obsolete("Property lightmapTilingOffset has been deprecated. Use lightmapScaleOffset (UnityUpgradable) -> lightmapScaleOffset", true)]
		public Vector4 lightmapTilingOffset
		{
			get
			{
				return Vector4.zero;
			}
			set
			{
			}
		}

		[Obsolete("Use probeAnchor instead (UnityUpgradable) -> probeAnchor", true)]
		public Transform lightProbeAnchor
		{
			get
			{
				return this.probeAnchor;
			}
			set
			{
				this.probeAnchor = value;
			}
		}

		[EditorBrowsable(EditorBrowsableState.Never), Obsolete("Use shadowCastingMode instead.", false)]
		public bool castShadows
		{
			get
			{
				return this.shadowCastingMode != ShadowCastingMode.Off;
			}
			set
			{
				this.shadowCastingMode = ((!value) ? ShadowCastingMode.Off : ShadowCastingMode.On);
			}
		}

		[Obsolete("Use motionVectorGenerationMode instead.", false)]
		public bool motionVectors
		{
			get
			{
				return this.motionVectorGenerationMode == MotionVectorGenerationMode.Object;
			}
			set
			{
				this.motionVectorGenerationMode = ((!value) ? MotionVectorGenerationMode.Camera : MotionVectorGenerationMode.Object);
			}
		}

		[Obsolete("Use lightProbeUsage instead.", false)]
		public bool useLightProbes
		{
			get
			{
				return this.lightProbeUsage != LightProbeUsage.Off;
			}
			set
			{
				this.lightProbeUsage = ((!value) ? LightProbeUsage.Off : LightProbeUsage.BlendProbes);
			}
		}

		public Bounds bounds
		{
			get
			{
				Bounds result;
				this.get_bounds_Injected(out result);
				return result;
			}
		}

		public extern bool enabled
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public extern bool isVisible
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public extern ShadowCastingMode shadowCastingMode
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public extern bool receiveShadows
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public extern MotionVectorGenerationMode motionVectorGenerationMode
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public extern LightProbeUsage lightProbeUsage
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public extern ReflectionProbeUsage reflectionProbeUsage
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public extern string sortingLayerName
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public extern int sortingLayerID
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public extern int sortingOrder
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		internal extern int sortingGroupID
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		internal extern int sortingGroupOrder
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public extern bool allowOcclusionWhenDynamic
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		internal extern Transform staticBatchRootTransform
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		internal extern int staticBatchIndex
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public extern bool isPartOfStaticBatch
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public Matrix4x4 worldToLocalMatrix
		{
			get
			{
				Matrix4x4 result;
				this.get_worldToLocalMatrix_Injected(out result);
				return result;
			}
		}

		public Matrix4x4 localToWorldMatrix
		{
			get
			{
				Matrix4x4 result;
				this.get_localToWorldMatrix_Injected(out result);
				return result;
			}
		}

		public extern GameObject lightProbeProxyVolumeOverride
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public extern Transform probeAnchor
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public int lightmapIndex
		{
			get
			{
				return this.GetLightmapIndex(LightmapType.StaticLightmap);
			}
			set
			{
				this.SetLightmapIndex(value, LightmapType.StaticLightmap);
			}
		}

		public int realtimeLightmapIndex
		{
			get
			{
				return this.GetLightmapIndex(LightmapType.DynamicLightmap);
			}
			set
			{
				this.SetLightmapIndex(value, LightmapType.DynamicLightmap);
			}
		}

		public Vector4 lightmapScaleOffset
		{
			get
			{
				return this.GetLightmapST(LightmapType.StaticLightmap);
			}
			set
			{
				this.SetStaticLightmapST(value);
			}
		}

		public Vector4 realtimeLightmapScaleOffset
		{
			get
			{
				return this.GetLightmapST(LightmapType.DynamicLightmap);
			}
			set
			{
				this.SetLightmapST(value, LightmapType.DynamicLightmap);
			}
		}

		public Material material
		{
			get
			{
				return this.GetMaterial();
			}
			set
			{
				this.SetMaterial(value);
			}
		}

		public Material sharedMaterial
		{
			get
			{
				return this.GetSharedMaterial();
			}
			set
			{
				this.SetMaterial(value);
			}
		}

		public Material[] materials
		{
			get
			{
				return this.GetMaterialArray();
			}
			set
			{
				this.SetMaterialArray(value);
			}
		}

		public Material[] sharedMaterials
		{
			get
			{
				return this.GetSharedMaterialArray();
			}
			set
			{
				this.SetMaterialArray(value);
			}
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetPropertyBlock(MaterialPropertyBlock properties);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void GetPropertyBlock(MaterialPropertyBlock dest);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void RenderNow(int material);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetClosestReflectionProbesInternal(object result);

		public void GetClosestReflectionProbes(List<ReflectionProbeBlendInfo> result)
		{
			this.GetClosestReflectionProbesInternal(result);
		}

		private void SetStaticLightmapST(Vector4 st)
		{
			this.SetStaticLightmapST_Injected(ref st);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern Material GetMaterial();

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern Material GetSharedMaterial();

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetMaterial(Material m);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern Material[] GetMaterialArray();

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern Material[] GetSharedMaterialArray();

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetMaterialArrayImpl(Material[] m);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void SetStaticBatchInfo(int firstSubMesh, int subMeshCount);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int GetLightmapIndex(LightmapType lt);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetLightmapIndex(int index, LightmapType lt);

		private Vector4 GetLightmapST(LightmapType lt)
		{
			Vector4 result;
			this.GetLightmapST_Injected(lt, out result);
			return result;
		}

		private void SetLightmapST(Vector4 st, LightmapType lt)
		{
			this.SetLightmapST_Injected(ref st, lt);
		}

		private void SetMaterialArray(Material[] m)
		{
			if (m == null)
			{
				throw new NullReferenceException("material array is null");
			}
			this.SetMaterialArrayImpl(m);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_bounds_Injected(out Bounds ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetStaticLightmapST_Injected(ref Vector4 st);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_worldToLocalMatrix_Injected(out Matrix4x4 ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_localToWorldMatrix_Injected(out Matrix4x4 ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetLightmapST_Injected(LightmapType lt, out Vector4 ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetLightmapST_Injected(ref Vector4 st, LightmapType lt);
	}
}
