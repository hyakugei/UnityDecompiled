using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Bindings;

namespace UnityEditor.Animations
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class StateMachineBehaviourContext
	{
		[NativeName("m_AnimatorController")]
		public AnimatorController animatorController;

		[NativeName("m_AnimatorObject")]
		public UnityEngine.Object animatorObject;

		[NativeName("m_LayerIndex")]
		public int layerIndex;
	}
}
