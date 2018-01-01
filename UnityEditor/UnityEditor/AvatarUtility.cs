using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityEditor
{
	internal class AvatarUtility
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void SetHumanPose(Animator animator, float[] dof);
	}
}
