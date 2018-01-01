using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine.Playables
{
	[RequiredByNativeCode]
	public class PlayableDirector : Behaviour, IExposedPropertyTable
	{
		public PlayState state
		{
			get
			{
				return this.GetPlayState();
			}
		}

		public DirectorWrapMode extrapolationMode
		{
			get
			{
				return this.GetWrapMode();
			}
			set
			{
				this.SetWrapMode(value);
			}
		}

		public PlayableAsset playableAsset
		{
			get
			{
				return this.GetPlayableAssetInternal() as PlayableAsset;
			}
			set
			{
				this.SetPlayableAssetInternal(value);
			}
		}

		public PlayableGraph playableGraph
		{
			get
			{
				return this.GetGraphHandle();
			}
		}

		public bool playOnAwake
		{
			get
			{
				return this.GetPlayOnAwake();
			}
			set
			{
				this.SetPlayOnAwake(value);
			}
		}

		public extern DirectorUpdateMode timeUpdateMode
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public extern double time
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public extern double initialTime
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public extern double duration
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public void DeferredEvaluate()
		{
			this.EvaluateNextFrame();
		}

		public void Play(PlayableAsset asset)
		{
			if (asset == null)
			{
				throw new ArgumentNullException("asset");
			}
			this.Play(asset, this.extrapolationMode);
		}

		public void Play(PlayableAsset asset, DirectorWrapMode mode)
		{
			if (asset == null)
			{
				throw new ArgumentNullException("asset");
			}
			this.playableAsset = asset;
			this.extrapolationMode = mode;
			this.Play();
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void Evaluate();

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void Play();

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void Stop();

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void Pause();

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void Resume();

		public void ClearReferenceValue(PropertyName id)
		{
			this.ClearReferenceValue_Injected(ref id);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void StopImmediately();

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern PlayState GetPlayState();

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetWrapMode(DirectorWrapMode mode);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern DirectorWrapMode GetWrapMode();

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void EvaluateNextFrame();

		private PlayableGraph GetGraphHandle()
		{
			PlayableGraph result;
			this.GetGraphHandle_Injected(out result);
			return result;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetPlayOnAwake(bool on);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern bool GetPlayOnAwake();

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetPlayableAssetInternal(ScriptableObject asset);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern ScriptableObject GetPlayableAssetInternal();

		public void SetReferenceValue(PropertyName id, UnityEngine.Object value)
		{
			PlayableDirector.INTERNAL_CALL_SetReferenceValue(this, ref id, value);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_SetReferenceValue(PlayableDirector self, ref PropertyName id, UnityEngine.Object value);

		public UnityEngine.Object GetReferenceValue(PropertyName id, out bool idValid)
		{
			return PlayableDirector.INTERNAL_CALL_GetReferenceValue(this, ref id, out idValid);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern UnityEngine.Object INTERNAL_CALL_GetReferenceValue(PlayableDirector self, ref PropertyName id, out bool idValid);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetGenericBinding(UnityEngine.Object key, UnityEngine.Object value);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern UnityEngine.Object GetGenericBinding(UnityEngine.Object key);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern bool HasGenericBinding(UnityEngine.Object key);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void ClearReferenceValue_Injected(ref PropertyName id);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetGraphHandle_Injected(out PlayableGraph ret);
	}
}
