using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Rendering;
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

		public extern uint renderingLayerMask
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

		public Material[] materials
		{
			get
			{
				Material[] result;
				if (this.IsPersistent())
				{
					Debug.LogError("Not allowed to access Renderer.materials on prefab object. Use Renderer.sharedMaterials instead", this);
					result = null;
				}
				else
				{
					result = this.GetMaterialArray();
				}
				return result;
			}
			set
			{
				this.SetMaterialArray(value);
			}
		}

		public Material material
		{
			get
			{
				Material result;
				if (this.IsPersistent())
				{
					Debug.LogError("Not allowed to access Renderer.material on prefab object. Use Renderer.sharedMaterial instead", this);
					result = null;
				}
				else
				{
					result = this.GetMaterial();
				}
				return result;
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
		private extern void SetMaterialArray([NotNull] Material[] m);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void Internal_SetPropertyBlock(MaterialPropertyBlock properties);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void Internal_GetPropertyBlock([NotNull] MaterialPropertyBlock dest);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void Internal_SetPropertyBlockMaterialIndex(MaterialPropertyBlock properties, int materialIndex);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void Internal_GetPropertyBlockMaterialIndex([NotNull] MaterialPropertyBlock dest, int materialIndex);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool HasPropertyBlock();

		public void SetPropertyBlock(MaterialPropertyBlock properties)
		{
			this.Internal_SetPropertyBlock(properties);
		}

		public void SetPropertyBlock(MaterialPropertyBlock properties, int materialIndex)
		{
			this.Internal_SetPropertyBlockMaterialIndex(properties, materialIndex);
		}

		public void GetPropertyBlock(MaterialPropertyBlock properties)
		{
			this.Internal_GetPropertyBlock(properties);
		}

		public void GetPropertyBlock(MaterialPropertyBlock properties, int materialIndex)
		{
			this.Internal_GetPropertyBlockMaterialIndex(properties, materialIndex);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetClosestReflectionProbesInternal(object result);

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

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern bool IsPersistent();

		public void GetClosestReflectionProbes(List<ReflectionProbeBlendInfo> result)
		{
			this.GetClosestReflectionProbesInternal(result);
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
