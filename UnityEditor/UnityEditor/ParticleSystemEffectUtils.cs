using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityEditor
{
	internal static class ParticleSystemEffectUtils
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern string CheckCircularReferences(ParticleSystem subEmitter);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void StopEffect();
	}
}
