using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace UnityEngine
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	internal struct StaticBatchingHelper
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern Mesh InternalCombineVertices(MeshSubsetCombineUtility.MeshInstance[] meshes, string meshName);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void InternalCombineIndices(MeshSubsetCombineUtility.SubMeshInstance[] submeshes, Mesh combinedMesh);
	}
}
