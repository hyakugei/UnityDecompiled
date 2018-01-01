using System;
using System.Runtime.CompilerServices;

namespace UnityEngine.Playables
{
	internal static class AudioPlayableGraphExtensions
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool InternalCreateAudioOutput(ref PlayableGraph graph, string name, out PlayableOutputHandle handle);
	}
}
