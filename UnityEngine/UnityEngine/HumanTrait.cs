using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public class HumanTrait
	{
		public static extern int MuscleCount
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public static extern string[] MuscleName
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public static extern int BoneCount
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public static extern string[] BoneName
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public static extern int RequiredBoneCount
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int GetBoneIndexFromMono(int humanId);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int GetBoneIndexToMono(int boneIndex);

		public static int MuscleFromBone(int i, int dofIndex)
		{
			return HumanTrait.Internal_MuscleFromBone(HumanTrait.GetBoneIndexFromMono(i), dofIndex);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int Internal_MuscleFromBone(int i, int dofIndex);

		public static int BoneFromMuscle(int i)
		{
			return HumanTrait.GetBoneIndexToMono(HumanTrait.Internal_BoneFromMuscle(i));
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int Internal_BoneFromMuscle(int i);

		public static bool RequiredBone(int i)
		{
			return HumanTrait.Internal_RequiredBone(HumanTrait.GetBoneIndexFromMono(i));
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool Internal_RequiredBone(int i);

		internal static bool HasCollider(Avatar avatar, int i)
		{
			return HumanTrait.Internal_HasCollider(avatar, HumanTrait.GetBoneIndexFromMono(i));
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool Internal_HasCollider(Avatar avatar, int i);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern float GetMuscleDefaultMin(int i);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern float GetMuscleDefaultMax(int i);

		public static int GetParentBone(int i)
		{
			int num = HumanTrait.Internal_GetParent(HumanTrait.GetBoneIndexFromMono(i));
			return (num == -1) ? -1 : HumanTrait.GetBoneIndexToMono(num);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int Internal_GetParent(int i);
	}
}
