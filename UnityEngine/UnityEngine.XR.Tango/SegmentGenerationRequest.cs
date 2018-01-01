using System;

namespace UnityEngine.XR.Tango
{
	public struct SegmentGenerationRequest
	{
		public GridIndex gridIndex;

		public MeshFilter destinationMeshFilter;

		public MeshCollider destinationMeshCollider;

		public bool provideNormals;

		public bool provideColors;

		public bool providePhysics;
	}
}
