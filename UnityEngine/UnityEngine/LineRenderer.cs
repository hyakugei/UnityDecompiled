using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	public sealed class LineRenderer : Renderer
	{
		[Obsolete("Use positionCount instead (UnityUpgradable) -> positionCount", false)]
		public int numPositions
		{
			get
			{
				return this.positionCount;
			}
			set
			{
				this.positionCount = value;
			}
		}

		public extern float startWidth
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public extern float endWidth
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public extern float widthMultiplier
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public extern int numCornerVertices
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public extern int numCapVertices
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public extern bool useWorldSpace
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public extern bool loop
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public Color startColor
		{
			get
			{
				Color result;
				this.get_startColor_Injected(out result);
				return result;
			}
			set
			{
				this.set_startColor_Injected(ref value);
			}
		}

		public Color endColor
		{
			get
			{
				Color result;
				this.get_endColor_Injected(out result);
				return result;
			}
			set
			{
				this.set_endColor_Injected(ref value);
			}
		}

		public extern int positionCount
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public extern bool generateLightingData
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public extern LineTextureMode textureMode
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public extern LineAlignment alignment
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public AnimationCurve widthCurve
		{
			get
			{
				return this.GetWidthCurveCopy();
			}
			set
			{
				this.SetWidthCurve(value);
			}
		}

		public Gradient colorGradient
		{
			get
			{
				return this.GetColorGradientCopy();
			}
			set
			{
				this.SetColorGradient(value);
			}
		}

		[Obsolete("Use startWidth, endWidth or widthCurve instead.", false)]
		public void SetWidth(float start, float end)
		{
			this.startWidth = start;
			this.endWidth = end;
		}

		[Obsolete("Use startColor, endColor or colorGradient instead.", false)]
		public void SetColors(Color start, Color end)
		{
			this.startColor = start;
			this.endColor = end;
		}

		[Obsolete("Use positionCount instead.", false)]
		public void SetVertexCount(int count)
		{
			this.positionCount = count;
		}

		public void SetPosition(int index, Vector3 position)
		{
			this.SetPosition_Injected(index, ref position);
		}

		public Vector3 GetPosition(int index)
		{
			Vector3 result;
			this.GetPosition_Injected(index, out result);
			return result;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void Simplify(float tolerance);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern AnimationCurve GetWidthCurveCopy();

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetWidthCurve([NotNull] AnimationCurve curve);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern Gradient GetColorGradientCopy();

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetColorGradient([NotNull] Gradient curve);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern int GetPositions([NotNull] [Out] Vector3[] positions);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetPositions([NotNull] Vector3[] positions);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_startColor_Injected(out Color ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_startColor_Injected(ref Color value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_endColor_Injected(out Color ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_endColor_Injected(ref Color value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetPosition_Injected(int index, ref Vector3 position);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetPosition_Injected(int index, out Vector3 ret);
	}
}
