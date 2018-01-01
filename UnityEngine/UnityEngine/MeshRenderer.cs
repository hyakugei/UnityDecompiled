using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public class MeshRenderer : Renderer
	{
		public extern Mesh additionalVertexStreams
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}
	}
}
