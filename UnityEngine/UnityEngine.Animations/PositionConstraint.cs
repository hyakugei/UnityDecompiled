using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.Animations
{
	[RequireComponent(typeof(Transform)), UsedByNativeCode]
	public sealed class PositionConstraint : Behaviour, IConstraint, IConstraintInternal
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

		public Vector3 translationOffset
		{
			get
			{
				Vector3 result;
				this.get_translationOffset_Injected(out result);
				return result;
			}
			set
			{
				this.set_translationOffset_Injected(ref value);
			}
		}

		public extern Axis translationAxis
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
				return PositionConstraint.GetSourceCountInternal(this);
			}
		}

		private PositionConstraint()
		{
			PositionConstraint.Internal_Create(this);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_Create([Writable] PositionConstraint self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetSourceCountInternal(PositionConstraint self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void GetSources([NotNull] List<ConstraintSource> sources);

		public void SetSources(List<ConstraintSource> sources)
		{
			if (sources == null)
			{
				throw new ArgumentNullException("sources");
			}
			PositionConstraint.SetSourcesInternal(this, sources);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetSourcesInternal(PositionConstraint self, List<ConstraintSource> sources);

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

		private void ValidateSourceIndex(int index)
		{
			if (this.sourceCount == 0)
			{
				throw new InvalidOperationException("The PositionConstraint component has no sources.");
			}
			if (index < 0 || index >= this.sourceCount)
			{
				throw new ArgumentOutOfRangeException("index", string.Format("Constraint source index {0} is out of bounds (0-{1}).", index, this.sourceCount));
			}
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
		private extern void get_translationOffset_Injected(out Vector3 ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_translationOffset_Injected(ref Vector3 value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int AddSource_Injected(ref ConstraintSource source);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetSourceInternal_Injected(int index, out ConstraintSource ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetSourceInternal_Injected(int index, ref ConstraintSource source);
	}
}
