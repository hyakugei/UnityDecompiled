using System;
using System.Runtime.CompilerServices;
using UnityEngine.Experimental.Audio;
using UnityEngine.Video;

namespace UnityEngine.Experimental.Video
{
	public static class VideoPlayerExtensions
	{
		public static AudioSampleProvider GetAudioSampleProvider(this VideoPlayer vp, ushort trackIndex)
		{
			ushort controlledAudioTrackCount = vp.controlledAudioTrackCount;
			if (trackIndex >= controlledAudioTrackCount)
			{
				throw new ArgumentOutOfRangeException("trackIndex", trackIndex, "VideoPlayer is currently configured with " + controlledAudioTrackCount + " tracks.");
			}
			VideoAudioOutputMode audioOutputMode = vp.audioOutputMode;
			if (audioOutputMode != VideoAudioOutputMode.APIOnly)
			{
				throw new InvalidOperationException("VideoPlayer.GetAudioSampleProvider requires audioOutputMode to be APIOnly. Current: " + audioOutputMode);
			}
			AudioSampleProvider audioSampleProvider = AudioSampleProvider.Lookup(VideoPlayerExtensions.InternalGetAudioSampleProviderId(vp, trackIndex), vp, trackIndex);
			if (audioSampleProvider == null)
			{
				throw new InvalidOperationException("VideoPlayer.GetAudioSampleProvider got null provider.");
			}
			if (audioSampleProvider.owner != vp)
			{
				throw new InvalidOperationException("Internal error: VideoPlayer.GetAudioSampleProvider got provider used by another object.");
			}
			if (audioSampleProvider.trackIndex != trackIndex)
			{
				throw new InvalidOperationException(string.Concat(new object[]
				{
					"Internal error: VideoPlayer.GetAudioSampleProvider got provider for track ",
					audioSampleProvider.trackIndex,
					" instead of ",
					trackIndex
				}));
			}
			return audioSampleProvider;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern uint InternalGetAudioSampleProviderId(VideoPlayer vp, ushort trackIndex);
	}
}
