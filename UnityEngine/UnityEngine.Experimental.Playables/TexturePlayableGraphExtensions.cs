using System;
using System.Runtime.CompilerServices;
using UnityEngine.Playables;

namespace UnityEngine.Experimental.Playables
{
	internal static class TexturePlayableGraphExtensions
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool InternalCreateTextureOutput(ref PlayableGraph graph, string name, out PlayableOutputHandle handle);
	}
}
