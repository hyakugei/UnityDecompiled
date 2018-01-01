using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
	public class SkinnedMeshRenderer : Renderer
	{
		public extern Transform[] bones
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public extern SkinQuality quality
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public extern bool updateWhenOffscreen
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public extern Transform rootBone
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		internal extern Transform actualRootBone
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public extern Mesh sharedMesh
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public extern bool skinnedMotionVectors
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public Bounds localBounds
		{
			get
			{
				return this.GetLocalAABB();
			}
			set
			{
				this.SetLocalAABB(value);
			}
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern float GetBlendShapeWeight(int index);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetBlendShapeWeight(int index, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void BakeMesh(Mesh mesh);

		private Bounds GetLocalAABB()
		{
			Bounds result;
			this.GetLocalAABB_Injected(out result);
			return result;
		}

		private void SetLocalAABB(Bounds b)
		{
			this.SetLocalAABB_Injected(ref b);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetLocalAABB_Injected(out Bounds ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetLocalAABB_Injected(ref Bounds b);
	}
}
