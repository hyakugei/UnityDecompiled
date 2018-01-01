using System;
using UnityEngine;
using UnityEngine.Bindings;

namespace UnityEditor.Animations
{
	[NativeType(CodegenOptions.Custom, "MonoStateMotionPair")]
	internal struct StateMotionPair
	{
		public AnimatorState m_State;

		public Motion m_Motion;
	}
}
