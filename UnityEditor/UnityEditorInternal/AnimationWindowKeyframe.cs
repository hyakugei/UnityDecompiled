using System;
using UnityEditor;
using UnityEngine;

namespace UnityEditorInternal
{
	internal class AnimationWindowKeyframe
	{
		public float m_InTangent;

		public float m_OutTangent;

		public float m_InWeight;

		public float m_OutWeight;

		public WeightedMode m_WeightedMode;

		public int m_TangentMode;

		public int m_TimeHash;

		private int m_Hash;

		private float m_time;

		private object m_value;

		private AnimationWindowCurve m_curve;

		public float time
		{
			get
			{
				return this.m_time;
			}
			set
			{
				this.m_time = value;
				this.m_Hash = 0;
				this.m_TimeHash = value.GetHashCode();
			}
		}

		public object value
		{
			get
			{
				return this.m_value;
			}
			set
			{
				this.m_value = value;
			}
		}

		public float inTangent
		{
			get
			{
				return this.m_InTangent;
			}
			set
			{
				this.m_InTangent = value;
			}
		}

		public float outTangent
		{
			get
			{
				return this.m_OutTangent;
			}
			set
			{
				this.m_OutTangent = value;
			}
		}

		public float inWeight
		{
			get
			{
				return this.m_InWeight;
			}
			set
			{
				this.m_InWeight = value;
			}
		}

		public float outWeight
		{
			get
			{
				return this.m_OutWeight;
			}
			set
			{
				this.m_OutWeight = value;
			}
		}

		public WeightedMode weightedMode
		{
			get
			{
				return this.m_WeightedMode;
			}
			set
			{
				this.m_WeightedMode = value;
			}
		}

		public AnimationWindowCurve curve
		{
			get
			{
				return this.m_curve;
			}
			set
			{
				this.m_curve = value;
				this.m_Hash = 0;
			}
		}

		public bool isPPtrCurve
		{
			get
			{
				return this.curve.isPPtrCurve;
			}
		}

		public bool isDiscreteCurve
		{
			get
			{
				return this.curve.isDiscreteCurve;
			}
		}

		public AnimationWindowKeyframe()
		{
		}

		public AnimationWindowKeyframe(AnimationWindowKeyframe key)
		{
			this.time = key.time;
			this.value = key.value;
			this.curve = key.curve;
			this.m_InTangent = key.m_InTangent;
			this.m_OutTangent = key.m_OutTangent;
			this.m_InWeight = key.inWeight;
			this.m_OutWeight = key.outWeight;
			this.m_WeightedMode = key.weightedMode;
			this.m_TangentMode = key.m_TangentMode;
			this.m_curve = key.m_curve;
		}

		public AnimationWindowKeyframe(AnimationWindowCurve curve, Keyframe key)
		{
			this.time = key.time;
			this.value = key.value;
			this.curve = curve;
			this.m_InTangent = key.inTangent;
			this.m_OutTangent = key.outTangent;
			this.m_InWeight = key.inWeight;
			this.m_OutWeight = key.outWeight;
			this.m_WeightedMode = key.weightedMode;
			this.m_TangentMode = key.tangentModeInternal;
			this.m_curve = curve;
		}

		public AnimationWindowKeyframe(AnimationWindowCurve curve, ObjectReferenceKeyframe key)
		{
			this.time = key.time;
			this.value = key.value;
			this.curve = curve;
		}

		public int GetHash()
		{
			if (this.m_Hash == 0)
			{
				this.m_Hash = this.curve.GetHashCode();
				this.m_Hash = 33 * this.m_Hash + this.time.GetHashCode();
			}
			return this.m_Hash;
		}

		public int GetIndex()
		{
			int result;
			for (int i = 0; i < this.curve.m_Keyframes.Count; i++)
			{
				if (this.curve.m_Keyframes[i] == this)
				{
					result = i;
					return result;
				}
			}
			result = -1;
			return result;
		}

		public Keyframe ToKeyframe()
		{
			return new Keyframe(this.time, (float)this.value, this.inTangent, this.outTangent)
			{
				tangentModeInternal = this.m_TangentMode,
				weightedMode = this.weightedMode,
				inWeight = this.inWeight,
				outWeight = this.outWeight
			};
		}

		public ObjectReferenceKeyframe ToObjectReferenceKeyframe()
		{
			return new ObjectReferenceKeyframe
			{
				time = this.time,
				value = (UnityEngine.Object)this.value
			};
		}
	}
}
