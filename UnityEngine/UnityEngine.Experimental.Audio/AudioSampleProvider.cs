using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.Experimental.Audio
{
	[NativeType(Header = "Modules/Audio/Public/ScriptBindings/AudioSampleProvider.bindings.h")]
	public class AudioSampleProvider : IDisposable
	{
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate uint ConsumeSampleFramesNativeFunction(uint providerId, IntPtr interleavedSampleFrames, uint sampleFrameCount);

		public delegate void SampleFramesHandler(AudioSampleProvider provider, uint sampleFrameCount);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void SampleFramesEventNativeFunction(IntPtr userData, uint providerId, uint sampleFrameCount);

		private AudioSampleProvider.ConsumeSampleFramesNativeFunction m_ConsumeSampleFramesNativeFunction;

		public event AudioSampleProvider.SampleFramesHandler sampleFramesAvailable
		{
			add
			{
				AudioSampleProvider.SampleFramesHandler sampleFramesHandler = this.sampleFramesAvailable;
				AudioSampleProvider.SampleFramesHandler sampleFramesHandler2;
				do
				{
					sampleFramesHandler2 = sampleFramesHandler;
					sampleFramesHandler = Interlocked.CompareExchange<AudioSampleProvider.SampleFramesHandler>(ref this.sampleFramesAvailable, (AudioSampleProvider.SampleFramesHandler)Delegate.Combine(sampleFramesHandler2, value), sampleFramesHandler);
				}
				while (sampleFramesHandler != sampleFramesHandler2);
			}
			remove
			{
				AudioSampleProvider.SampleFramesHandler sampleFramesHandler = this.sampleFramesAvailable;
				AudioSampleProvider.SampleFramesHandler sampleFramesHandler2;
				do
				{
					sampleFramesHandler2 = sampleFramesHandler;
					sampleFramesHandler = Interlocked.CompareExchange<AudioSampleProvider.SampleFramesHandler>(ref this.sampleFramesAvailable, (AudioSampleProvider.SampleFramesHandler)Delegate.Remove(sampleFramesHandler2, value), sampleFramesHandler);
				}
				while (sampleFramesHandler != sampleFramesHandler2);
			}
		}

		public event AudioSampleProvider.SampleFramesHandler sampleFramesOverflow
		{
			add
			{
				AudioSampleProvider.SampleFramesHandler sampleFramesHandler = this.sampleFramesOverflow;
				AudioSampleProvider.SampleFramesHandler sampleFramesHandler2;
				do
				{
					sampleFramesHandler2 = sampleFramesHandler;
					sampleFramesHandler = Interlocked.CompareExchange<AudioSampleProvider.SampleFramesHandler>(ref this.sampleFramesOverflow, (AudioSampleProvider.SampleFramesHandler)Delegate.Combine(sampleFramesHandler2, value), sampleFramesHandler);
				}
				while (sampleFramesHandler != sampleFramesHandler2);
			}
			remove
			{
				AudioSampleProvider.SampleFramesHandler sampleFramesHandler = this.sampleFramesOverflow;
				AudioSampleProvider.SampleFramesHandler sampleFramesHandler2;
				do
				{
					sampleFramesHandler2 = sampleFramesHandler;
					sampleFramesHandler = Interlocked.CompareExchange<AudioSampleProvider.SampleFramesHandler>(ref this.sampleFramesOverflow, (AudioSampleProvider.SampleFramesHandler)Delegate.Remove(sampleFramesHandler2, value), sampleFramesHandler);
				}
				while (sampleFramesHandler != sampleFramesHandler2);
			}
		}

		public uint id
		{
			get;
			private set;
		}

		public ushort trackIndex
		{
			get;
			private set;
		}

		public UnityEngine.Object owner
		{
			get;
			private set;
		}

		public bool valid
		{
			get
			{
				return AudioSampleProvider.InternalIsValid(this.id);
			}
		}

		public ushort channelCount
		{
			get;
			private set;
		}

		public uint sampleRate
		{
			get;
			private set;
		}

		public uint maxSampleFrameCount
		{
			get
			{
				return AudioSampleProvider.InternalGetMaxSampleFrameCount(this.id);
			}
		}

		public uint availableSampleFrameCount
		{
			get
			{
				return AudioSampleProvider.InternalGetAvailableSampleFrameCount(this.id);
			}
		}

		public uint freeSampleFrameCount
		{
			get
			{
				return AudioSampleProvider.InternalGetFreeSampleFrameCount(this.id);
			}
		}

		public uint freeSampleFrameCountLowThreshold
		{
			get
			{
				return AudioSampleProvider.InternalGetFreeSampleFrameCountLowThreshold(this.id);
			}
			set
			{
				AudioSampleProvider.InternalSetFreeSampleFrameCountLowThreshold(this.id, value);
			}
		}

		public bool enableSampleFramesAvailableEvents
		{
			get
			{
				return AudioSampleProvider.InternalGetEnableSampleFramesAvailableEvents(this.id);
			}
			set
			{
				AudioSampleProvider.InternalSetEnableSampleFramesAvailableEvents(this.id, value);
			}
		}

		public bool enableSilencePadding
		{
			get
			{
				return AudioSampleProvider.InternalGetEnableSilencePadding(this.id);
			}
			set
			{
				AudioSampleProvider.InternalSetEnableSilencePadding(this.id, value);
			}
		}

		public static AudioSampleProvider.ConsumeSampleFramesNativeFunction consumeSampleFramesNativeFunction
		{
			get
			{
				return (AudioSampleProvider.ConsumeSampleFramesNativeFunction)Marshal.GetDelegateForFunctionPointer(AudioSampleProvider.InternalGetConsumeSampleFramesNativeFunctionPtr(), typeof(AudioSampleProvider.ConsumeSampleFramesNativeFunction));
			}
		}

		private AudioSampleProvider(uint providerId, UnityEngine.Object ownerObj, ushort trackIdx)
		{
			this.owner = ownerObj;
			this.id = providerId;
			this.trackIndex = trackIdx;
			this.m_ConsumeSampleFramesNativeFunction = (AudioSampleProvider.ConsumeSampleFramesNativeFunction)Marshal.GetDelegateForFunctionPointer(AudioSampleProvider.InternalGetConsumeSampleFramesNativeFunctionPtr(), typeof(AudioSampleProvider.ConsumeSampleFramesNativeFunction));
			ushort channelCount = 0;
			uint sampleRate = 0u;
			AudioSampleProvider.InternalGetFormatInfo(providerId, out channelCount, out sampleRate);
			this.channelCount = channelCount;
			this.sampleRate = sampleRate;
			AudioSampleProvider.InternalSetScriptingPtr(providerId, this);
		}

		[VisibleToOtherModules]
		internal static AudioSampleProvider Lookup(uint providerId, UnityEngine.Object ownerObj, ushort trackIndex)
		{
			AudioSampleProvider audioSampleProvider = AudioSampleProvider.InternalGetScriptingPtr(providerId);
			AudioSampleProvider result;
			if (audioSampleProvider != null)
			{
				result = audioSampleProvider;
			}
			else
			{
				result = new AudioSampleProvider(providerId, ownerObj, trackIndex);
			}
			return result;
		}

		~AudioSampleProvider()
		{
			this.Dispose();
		}

		public void Dispose()
		{
			if (this.id != 0u)
			{
				AudioSampleProvider.InternalSetScriptingPtr(this.id, null);
				this.id = 0u;
			}
			GC.SuppressFinalize(this);
		}

		public uint ConsumeSampleFrames(NativeArray<float> sampleFrames)
		{
			uint result;
			if (this.channelCount == 0)
			{
				result = 0u;
			}
			else
			{
				result = this.m_ConsumeSampleFramesNativeFunction(this.id, sampleFrames.GetUnsafePtr<float>(), (uint)(sampleFrames.Length / (int)this.channelCount));
			}
			return result;
		}

		public void SetSampleFramesAvailableNativeHandler(AudioSampleProvider.SampleFramesEventNativeFunction handler, IntPtr userData)
		{
			AudioSampleProvider.InternalSetSampleFramesAvailableNativeHandler(this.id, Marshal.GetFunctionPointerForDelegate(handler), userData);
		}

		public void ClearSampleFramesAvailableNativeHandler()
		{
			AudioSampleProvider.InternalClearSampleFramesAvailableNativeHandler(this.id);
		}

		public void SetSampleFramesOverflowNativeHandler(AudioSampleProvider.SampleFramesEventNativeFunction handler, IntPtr userData)
		{
			AudioSampleProvider.InternalSetSampleFramesOverflowNativeHandler(this.id, Marshal.GetFunctionPointerForDelegate(handler), userData);
		}

		public void ClearSampleFramesOverflowNativeHandler()
		{
			AudioSampleProvider.InternalClearSampleFramesOverflowNativeHandler(this.id);
		}

		[RequiredByNativeCode]
		private void InvokeSampleFramesAvailable(int sampleFrameCount)
		{
			if (this.sampleFramesAvailable != null)
			{
				this.sampleFramesAvailable(this, (uint)sampleFrameCount);
			}
		}

		[RequiredByNativeCode]
		private void InvokeSampleFramesOverflow(int droppedSampleFrameCount)
		{
			if (this.sampleFramesOverflow != null)
			{
				this.sampleFramesOverflow(this, (uint)droppedSampleFrameCount);
			}
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void InternalGetFormatInfo(uint providerId, out ushort chCount, out uint sRate);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern AudioSampleProvider InternalGetScriptingPtr(uint providerId);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void InternalSetScriptingPtr(uint providerId, AudioSampleProvider provider);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool InternalIsValid(uint providerId);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern uint InternalGetMaxSampleFrameCount(uint providerId);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern uint InternalGetAvailableSampleFrameCount(uint providerId);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern uint InternalGetFreeSampleFrameCount(uint providerId);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern uint InternalGetFreeSampleFrameCountLowThreshold(uint providerId);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void InternalSetFreeSampleFrameCountLowThreshold(uint providerId, uint sampleFrameCount);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool InternalGetEnableSampleFramesAvailableEvents(uint providerId);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void InternalSetEnableSampleFramesAvailableEvents(uint providerId, bool enable);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void InternalSetSampleFramesAvailableNativeHandler(uint providerId, IntPtr handler, IntPtr userData);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void InternalClearSampleFramesAvailableNativeHandler(uint providerId);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void InternalSetSampleFramesOverflowNativeHandler(uint providerId, IntPtr handler, IntPtr userData);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void InternalClearSampleFramesOverflowNativeHandler(uint providerId);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool InternalGetEnableSilencePadding(uint id);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void InternalSetEnableSilencePadding(uint id, bool enabled);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr InternalGetConsumeSampleFramesNativeFunctionPtr();
	}
}
