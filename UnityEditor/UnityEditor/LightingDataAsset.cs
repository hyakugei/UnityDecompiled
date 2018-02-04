using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Scripting;

namespace UnityEditor
{
	[ExcludeFromPreset]
	public sealed class LightingDataAsset : UnityEngine.Object
	{
		internal extern bool isValid
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		internal extern string validityErrorMessage
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		private LightingDataAsset()
		{
		}
	}
}
