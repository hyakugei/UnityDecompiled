using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine.Experimental.U2D
{
	[NativeType(Header = "Modules/SpriteShape/Public/SpriteShapeRenderer.h")]
	public sealed class SpriteShapeRenderer : Renderer
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern int GetVertexCount();

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern int GetIndexCount();
	}
}
