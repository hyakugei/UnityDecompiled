using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine.XR.Tango
{
	[UsedByNativeCode]
	public sealed class MeshReconstructionServer : IDisposable
	{
		public delegate void SegmentChangedDelegate(GridIndex gridIndex, SegmentChange changeType, double updateTime);

		public delegate void SegmentReadyDelegate(SegmentGenerationResult generatedSegmentData);

		public enum Status
		{
			UnsupportedPlatform,
			Ok,
			MissingMeshReconstructionLibrary,
			FailedToCreateMeshReconstructionContext,
			FailedToSetDepthCalibration
		}

		internal IntPtr m_ServerPtr = IntPtr.Zero;

		private MeshReconstructionServer.Status m_Status = MeshReconstructionServer.Status.UnsupportedPlatform;

		public MeshReconstructionServer.Status status
		{
			get
			{
				return this.m_Status;
			}
		}

		public int generationRequests
		{
			get
			{
				return MeshReconstructionServer.Internal_GetNumGenerationRequests(this.m_ServerPtr);
			}
		}

		public bool enabled
		{
			get
			{
				return MeshReconstructionServer.Internal_GetEnabled(this.m_ServerPtr);
			}
			set
			{
				MeshReconstructionServer.Internal_SetEnabled(this.m_ServerPtr, value);
			}
		}

		public MeshReconstructionServer(MeshReconstructionConfig config)
		{
			int status = 0;
			this.m_ServerPtr = MeshReconstructionServer.Internal_Create(this, config, out status);
			this.m_Status = (MeshReconstructionServer.Status)status;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_ClearMeshes(IntPtr server);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool Internal_GetEnabled(IntPtr server);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_SetEnabled(IntPtr server, bool enabled);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr Internal_GetNativeReconstructionContextPtr(IntPtr server);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int Internal_GetNumGenerationRequests(IntPtr server);

		public void Dispose()
		{
			if (this.m_ServerPtr != IntPtr.Zero)
			{
				MeshReconstructionServer.Destroy(this.m_ServerPtr);
				this.m_ServerPtr = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}

		private static IntPtr Internal_Create(MeshReconstructionServer self, MeshReconstructionConfig config, out int status)
		{
			return MeshReconstructionServer.Internal_Create_Injected(self, ref config, out status);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Destroy(IntPtr server);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void DestroyThreaded(IntPtr server);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_GetChangedSegments(IntPtr serverPtr, MeshReconstructionServer.SegmentChangedDelegate onSegmentChanged);

		private static void Internal_GenerateSegmentAsync(IntPtr serverPtr, GridIndex gridIndex, MeshFilter destinationMeshFilter, MeshCollider destinationMeshCollider, MeshReconstructionServer.SegmentReadyDelegate onSegmentReady, bool provideNormals, bool provideColors, bool providePhysics)
		{
			MeshReconstructionServer.Internal_GenerateSegmentAsync_Injected(serverPtr, ref gridIndex, destinationMeshFilter, destinationMeshCollider, onSegmentReady, provideNormals, provideColors, providePhysics);
		}

		~MeshReconstructionServer()
		{
			if (this.m_ServerPtr != IntPtr.Zero)
			{
				MeshReconstructionServer.DestroyThreaded(this.m_ServerPtr);
				this.m_ServerPtr = IntPtr.Zero;
				GC.SuppressFinalize(this);
			}
		}

		public void ClearMeshes()
		{
			MeshReconstructionServer.Internal_ClearMeshes(this.m_ServerPtr);
		}

		public IntPtr GetNativeReconstructionContext()
		{
			return MeshReconstructionServer.Internal_GetNativeReconstructionContextPtr(this.m_ServerPtr);
		}

		public void GetChangedSegments(MeshReconstructionServer.SegmentChangedDelegate onSegmentChanged)
		{
			if (onSegmentChanged == null)
			{
				throw new ArgumentNullException("onSegmentChanged");
			}
			MeshReconstructionServer.Internal_GetChangedSegments(this.m_ServerPtr, onSegmentChanged);
		}

		public void GenerateSegmentAsync(SegmentGenerationRequest request, MeshReconstructionServer.SegmentReadyDelegate onSegmentReady)
		{
			if (onSegmentReady == null)
			{
				throw new ArgumentNullException("onSegmentRead");
			}
			MeshReconstructionServer.Internal_GenerateSegmentAsync(this.m_ServerPtr, request.gridIndex, request.destinationMeshFilter, request.destinationMeshCollider, onSegmentReady, request.provideNormals, request.provideColors, request.providePhysics);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr Internal_Create_Injected(MeshReconstructionServer self, ref MeshReconstructionConfig config, out int status);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_GenerateSegmentAsync_Injected(IntPtr serverPtr, ref GridIndex gridIndex, MeshFilter destinationMeshFilter, MeshCollider destinationMeshCollider, MeshReconstructionServer.SegmentReadyDelegate onSegmentReady, bool provideNormals, bool provideColors, bool providePhysics);
	}
}
