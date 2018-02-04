using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
	[ExcludeFromObjectFactory, UsedByNativeCode]
	public class RuntimeAnimatorController : Object
	{
		public extern AnimationClip[] animationClips
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		protected RuntimeAnimatorController()
		{
		}
	}
}
