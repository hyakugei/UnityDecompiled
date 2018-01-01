using System;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine.Scripting;

namespace UnityEngine.Playables
{
	[RequiredByNativeCode]
	public class PlayableDirector : Behaviour, IExposedPropertyTable
	{
		public event Action<PlayableDirector> played
		{
			add
			{
				Action<PlayableDirector> action = this.played;
				Action<PlayableDirector> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action<PlayableDirector>>(ref this.played, (Action<PlayableDirector>)Delegate.Combine(action2, value), action);
				}
				while (action != action2);
			}
			remove
			{
				Action<PlayableDirector> action = this.played;
				Action<PlayableDirector> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action<PlayableDirector>>(ref this.played, (Action<PlayableDirector>)Delegate.Remove(action2, value), action);
				}
				while (action != action2);
			}
		}

		public event Action<PlayableDirector> paused
		{
			add
			{
				Action<PlayableDirector> action = this.paused;
				Action<PlayableDirector> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action<PlayableDirector>>(ref this.paused, (Action<PlayableDirector>)Delegate.Combine(action2, value), action);
				}
				while (action != action2);
			}
			remove
			{
				Action<PlayableDirector> action = this.paused;
				Action<PlayableDirector> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action<PlayableDirector>>(ref this.paused, (Action<PlayableDirector>)Delegate.Remove(action2, value), action);
				}
				while (action != action2);
			}
		}

		public event Action<PlayableDirector> stopped
		{
			add
			{
				Action<PlayableDirector> action = this.stopped;
				Action<PlayableDirector> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action<PlayableDirector>>(ref this.stopped, (Action<PlayableDirector>)Delegate.Combine(action2, value), action);
				}
				while (action != action2);
			}
			remove
			{
				Action<PlayableDirector> action = this.stopped;
				Action<PlayableDirector> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action<PlayableDirector>>(ref this.stopped, (Action<PlayableDirector>)Delegate.Remove(action2, value), action);
				}
				while (action != action2);
			}
		}

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
				return this.Internal_GetPlayableAsset() as PlayableAsset;
			}
			set
			{
				this.SetPlayableAsset(value);
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

		public void SetGenericBinding(UnityEngine.Object key, UnityEngine.Object value)
		{
			this.Internal_SetGenericBinding(key, value);
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

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void RebuildGraph();

		public void ClearReferenceValue(PropertyName id)
		{
			this.ClearReferenceValue_Injected(ref id);
		}

		public void SetReferenceValue(PropertyName id, UnityEngine.Object value)
		{
			this.SetReferenceValue_Injected(ref id, value);
		}

		public UnityEngine.Object GetReferenceValue(PropertyName id, out bool idValid)
		{
			return this.GetReferenceValue_Injected(ref id, out idValid);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern UnityEngine.Object GetGenericBinding(UnityEngine.Object key);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void ProcessPendingGraphChanges();

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern bool HasGenericBinding(UnityEngine.Object key);

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

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Internal_SetGenericBinding(UnityEngine.Object key, UnityEngine.Object value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetPlayableAsset(ScriptableObject asset);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern ScriptableObject Internal_GetPlayableAsset();

		[RequiredByNativeCode]
		private void SendOnPlayableDirectorPlay()
		{
			if (this.played != null)
			{
				this.played(this);
			}
		}

		[RequiredByNativeCode]
		private void SendOnPlayableDirectorPause()
		{
			if (this.paused != null)
			{
				this.paused(this);
			}
		}

		[RequiredByNativeCode]
		private void SendOnPlayableDirectorStop()
		{
			if (this.stopped != null)
			{
				this.stopped(this);
			}
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void ClearReferenceValue_Injected(ref PropertyName id);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetReferenceValue_Injected(ref PropertyName id, UnityEngine.Object value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern UnityEngine.Object GetReferenceValue_Injected(ref PropertyName id, out bool idValid);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetGraphHandle_Injected(out PlayableGraph ret);
	}
}
