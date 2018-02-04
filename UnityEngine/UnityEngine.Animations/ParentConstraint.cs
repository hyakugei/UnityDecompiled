using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.Animations
{
	[RequireComponent(typeof(Transform)), UsedByNativeCode]
	public sealed class ParentConstraint : Behaviour, IConstraint, IConstraintInternal
	{
		Transform IConstraintInternal.transform
		{
			get
			{
				return base.transform;
			}
		}

		public extern float weight
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public extern bool constraintActive
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public extern bool locked
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public int sourceCount
		{
			get
			{
				return ParentConstraint.GetSourceCountInternal(this);
			}
		}

		public Vector3 translationAtRest
		{
			get
			{
				Vector3 result;
				this.get_translationAtRest_Injected(out result);
				return result;
			}
			set
			{
				this.set_translationAtRest_Injected(ref value);
			}
		}

		public Vector3 rotationAtRest
		{
			get
			{
				Vector3 result;
				this.get_rotationAtRest_Injected(out result);
				return result;
			}
			set
			{
				this.set_rotationAtRest_Injected(ref value);
			}
		}

		public extern Vector3[] translationOffsets
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public extern Vector3[] rotationOffsets
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public extern Axis translationAxis
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public extern Axis rotationAxis
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		private ParentConstraint()
		{
			ParentConstraint.Internal_Create(this);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_Create([Writable] ParentConstraint self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetSourceCountInternal(ParentConstraint self);

		public Vector3 GetTranslationOffset(int index)
		{
			this.ValidateSourceIndex(index);
			return this.GetTranslationOffsetInternal(index);
		}

		public void SetTranslationOffset(int index, Vector3 value)
		{
			this.ValidateSourceIndex(index);
			this.SetTranslationOffsetInternal(index, value);
		}

		private Vector3 GetTranslationOffsetInternal(int index)
		{
			Vector3 result;
			this.GetTranslationOffsetInternal_Injected(index, out result);
			return result;
		}

		private void SetTranslationOffsetInternal(int index, Vector3 value)
		{
			this.SetTranslationOffsetInternal_Injected(index, ref value);
		}

		public Vector3 GetRotationOffset(int index)
		{
			this.ValidateSourceIndex(index);
			return this.GetRotationOffsetInternal(index);
		}

		public void SetRotationOffset(int index, Vector3 value)
		{
			this.ValidateSourceIndex(index);
			this.SetRotationOffsetInternal(index, value);
		}

		private Vector3 GetRotationOffsetInternal(int index)
		{
			Vector3 result;
			this.GetRotationOffsetInternal_Injected(index, out result);
			return result;
		}

		private void SetRotationOffsetInternal(int index, Vector3 value)
		{
			this.SetRotationOffsetInternal_Injected(index, ref value);
		}

		private void ValidateSourceIndex(int index)
		{
			if (this.sourceCount == 0)
			{
				throw new InvalidOperationException("The ParentConstraint component has no sources.");
			}
			if (index < 0 || index >= this.sourceCount)
			{
				throw new ArgumentOutOfRangeException("index", string.Format("Constraint source index {0} is out of bounds (0-{1}).", index, this.sourceCount));
			}
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void GetSources([NotNull] List<ConstraintSource> sources);

		public void SetSources(List<ConstraintSource> sources)
		{
			if (sources == null)
			{
				throw new ArgumentNullException("sources");
			}
			ParentConstraint.SetSourcesInternal(this, sources);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetSourcesInternal(ParentConstraint self, List<ConstraintSource> sources);

		public int AddSource(ConstraintSource source)
		{
			return this.AddSource_Injected(ref source);
		}

		public void RemoveSource(int index)
		{
			this.ValidateSourceIndex(index);
			this.RemoveSourceInternal(index);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void RemoveSourceInternal(int index);

		public ConstraintSource GetSource(int index)
		{
			this.ValidateSourceIndex(index);
			return this.GetSourceInternal(index);
		}

		private ConstraintSource GetSourceInternal(int index)
		{
			ConstraintSource result;
			this.GetSourceInternal_Injected(index, out result);
			return result;
		}

		public void SetSource(int index, ConstraintSource source)
		{
			this.ValidateSourceIndex(index);
			this.SetSourceInternal(index, source);
		}

		private void SetSourceInternal(int index, ConstraintSource source)
		{
			this.SetSourceInternal_Injected(index, ref source);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void ActivateAndPreserveOffset();

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void ActivateWithZeroOffset();

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void UserUpdateOffset();

		void IConstraintInternal.ActivateAndPreserveOffset()
		{
			this.ActivateAndPreserveOffset();
		}

		void IConstraintInternal.ActivateWithZeroOffset()
		{
			this.ActivateWithZeroOffset();
		}

		void IConstraintInternal.UserUpdateOffset()
		{
			this.UserUpdateOffset();
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_translationAtRest_Injected(out Vector3 ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_translationAtRest_Injected(ref Vector3 value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_rotationAtRest_Injected(out Vector3 ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_rotationAtRest_Injected(ref Vector3 value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetTranslationOffsetInternal_Injected(int index, out Vector3 ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetTranslationOffsetInternal_Injected(int index, ref Vector3 value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetRotationOffsetInternal_Injected(int index, out Vector3 ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetRotationOffsetInternal_Injected(int index, ref Vector3 value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int AddSource_Injected(ref ConstraintSource source);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetSourceInternal_Injected(int index, out ConstraintSource ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetSourceInternal_Injected(int index, ref ConstraintSource source);
	}
}
