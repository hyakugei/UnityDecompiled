using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	public class AvatarBuilder
	{
		public static Avatar BuildHumanAvatar(GameObject go, HumanDescription humanDescription)
		{
			if (go == null)
			{
				throw new NullReferenceException();
			}
			return AvatarBuilder.BuildHumanAvatarInternal(go, humanDescription);
		}

		private static Avatar BuildHumanAvatarInternal(GameObject go, HumanDescription humanDescription)
		{
			return AvatarBuilder.BuildHumanAvatarInternal_Injected(go, ref humanDescription);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern Avatar BuildGenericAvatar([NotNull] GameObject go, [NotNull] string rootMotionTransformName);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Avatar BuildHumanAvatarInternal_Injected(GameObject go, ref HumanDescription humanDescription);
	}
}
