using System;
using System.Runtime.InteropServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEditor
{
	[RequiredByNativeCode]
	[StructLayout(LayoutKind.Sequential)]
	internal class MuscleClipQualityInfo
	{
		[NativeName("m_Loop")]
		public float loop = 0f;

		[NativeName("m_LoopOrientation")]
		public float loopOrientation = 0f;

		[NativeName("m_LoopPositionY")]
		public float loopPositionY = 0f;

		[NativeName("m_LoopPositionXZ")]
		public float loopPositionXZ = 0f;
	}
}
