using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityEditor.Animations
{
	public class AnimatorTransition : AnimatorTransitionBase
	{
		public AnimatorTransition()
		{
			AnimatorTransition.Internal_CreateAnimatorTransition(this);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_CreateAnimatorTransition([Writable] AnimatorTransition mono);
	}
}
