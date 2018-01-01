using System;
using UnityEngine;
using UnityEngine.Bindings;

namespace UnityEditor.Animations
{
	[NativeType(CodegenOptions.Custom, "MonoStateBehavioursPair")]
	internal struct StateBehavioursPair
	{
		public AnimatorState m_State;

		public ScriptableObject[] m_Behaviours;
	}
}
