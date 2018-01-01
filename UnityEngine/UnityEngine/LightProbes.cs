using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Rendering;
using UnityEngine.Scripting;

namespace UnityEngine
{
	public sealed class LightProbes : Object
	{
		public extern Vector3[] positions
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public extern SphericalHarmonicsL2[] bakedProbes
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public extern int count
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public extern int cellCount
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		[Obsolete("Use bakedProbes instead.", true)]
		public float[] coefficients
		{
			get
			{
				return new float[0];
			}
			set
			{
			}
		}

		private LightProbes()
		{
		}

		public static void GetInterpolatedProbe(Vector3 position, Renderer renderer, out SphericalHarmonicsL2 probe)
		{
			LightProbes.INTERNAL_CALL_GetInterpolatedProbe(ref position, renderer, out probe);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_GetInterpolatedProbe(ref Vector3 position, Renderer renderer, out SphericalHarmonicsL2 probe);

		public static void CalculateInterpolatedLightAndOcclusionProbes(Vector3[] positions, SphericalHarmonicsL2[] lightProbes, Vector4[] occlusionProbes)
		{
			if (positions == null)
			{
				throw new ArgumentNullException("positions");
			}
			if (lightProbes == null && occlusionProbes == null)
			{
				throw new ArgumentException("Argument lightProbes and occlusionProbes cannot both be null.");
			}
			if (lightProbes != null && lightProbes.Length < positions.Length)
			{
				throw new ArgumentException("lightProbes", "Argument lightProbes has less elements than positions");
			}
			if (occlusionProbes != null && occlusionProbes.Length < positions.Length)
			{
				throw new ArgumentException("occlusionProbes", "Argument occlusionProbes has less elements than positions");
			}
			LightProbes.Internal_CalculateInterpolatedLightAndOcclusionProbes(positions, positions.Length, lightProbes, occlusionProbes);
		}

		public static void CalculateInterpolatedLightAndOcclusionProbes(List<Vector3> positions, List<SphericalHarmonicsL2> lightProbes, List<Vector4> occlusionProbes)
		{
			if (positions == null)
			{
				throw new ArgumentNullException("positions");
			}
			if (lightProbes == null && occlusionProbes == null)
			{
				throw new ArgumentException("Argument lightProbes and occlusionProbes cannot both be null.");
			}
			if (lightProbes != null)
			{
				if (lightProbes.Capacity < positions.Count)
				{
					lightProbes.Capacity = positions.Count;
				}
				if (lightProbes.Count < positions.Count)
				{
					NoAllocHelpers.ResizeList<SphericalHarmonicsL2>(lightProbes, positions.Count);
				}
			}
			if (occlusionProbes != null)
			{
				if (occlusionProbes.Capacity < positions.Count)
				{
					occlusionProbes.Capacity = positions.Count;
				}
				if (occlusionProbes.Count < positions.Count)
				{
					NoAllocHelpers.ResizeList<Vector4>(occlusionProbes, positions.Count);
				}
			}
			LightProbes.Internal_CalculateInterpolatedLightAndOcclusionProbes(NoAllocHelpers.ExtractArrayFromListT<Vector3>(positions), positions.Count, NoAllocHelpers.ExtractArrayFromListT<SphericalHarmonicsL2>(lightProbes), NoAllocHelpers.ExtractArrayFromListT<Vector4>(occlusionProbes));
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void Internal_CalculateInterpolatedLightAndOcclusionProbes(Vector3[] positions, int count, SphericalHarmonicsL2[] lightProbes, Vector4[] occlusionProbes);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool AreLightProbesAllowed(Renderer renderer);

		[Obsolete("Use GetInterpolatedProbe instead.", true)]
		public void GetInterpolatedLightProbe(Vector3 position, Renderer renderer, float[] coefficients)
		{
		}
	}
}
