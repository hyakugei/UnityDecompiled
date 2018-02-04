using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Internal;
using UnityEngine.Scripting;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine
{
	[MovedFrom("UnityEditor.Animations", true), UsedByNativeCode]
	public sealed class AvatarMask : Object
	{
		[Obsolete("AvatarMask.humanoidBodyPartCount is deprecated, use AvatarMaskBodyPart.LastBodyPart instead.")]
		public int humanoidBodyPartCount
		{
			get
			{
				return 13;
			}
		}

		public extern int transformCount
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		internal extern bool hasFeetIK
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public AvatarMask()
		{
			AvatarMask.Internal_Create(this);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_Create([Writable] AvatarMask self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool GetHumanoidBodyPartActive(AvatarMaskBodyPart index);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetHumanoidBodyPartActive(AvatarMaskBodyPart index, bool value);

		public void AddTransformPath(Transform transform)
		{
			this.AddTransformPath(transform, true);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void AddTransformPath([NotNull] Transform transform, [DefaultValue("true")] bool recursive);

		public void RemoveTransformPath(Transform transform)
		{
			this.RemoveTransformPath(transform, true);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void RemoveTransformPath([NotNull] Transform transform, [DefaultValue("true")] bool recursive);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern string GetTransformPath(int index);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetTransformPath(int index, string path);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern float GetTransformWeight(int index);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetTransformWeight(int index, float weight);

		public bool GetTransformActive(int index)
		{
			return this.GetTransformWeight(index) > 0.5f;
		}

		public void SetTransformActive(int index, bool value)
		{
			this.SetTransformWeight(index, (!value) ? 0f : 1f);
		}

		internal void Copy(AvatarMask other)
		{
			for (AvatarMaskBodyPart avatarMaskBodyPart = AvatarMaskBodyPart.Root; avatarMaskBodyPart < AvatarMaskBodyPart.LastBodyPart; avatarMaskBodyPart++)
			{
				this.SetHumanoidBodyPartActive(avatarMaskBodyPart, other.GetHumanoidBodyPartActive(avatarMaskBodyPart));
			}
			this.transformCount = other.transformCount;
			for (int i = 0; i < other.transformCount; i++)
			{
				this.SetTransformPath(i, other.GetTransformPath(i));
				this.SetTransformActive(i, other.GetTransformActive(i));
			}
		}
	}
}
