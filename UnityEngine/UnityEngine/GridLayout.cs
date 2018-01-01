using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	[NativeType(Header = "Modules/Grid/Public/Grid.h"), RequireComponent(typeof(Transform))]
	public class GridLayout : Behaviour
	{
		public enum CellLayout
		{
			Rectangle
		}

		public enum CellSwizzle
		{
			XYZ,
			XZY,
			YXZ,
			YZX,
			ZXY,
			ZYX
		}

		public Vector3 cellSize
		{
			get
			{
				return GridLayout.GetCellSize(this);
			}
		}

		public Vector3 cellGap
		{
			get
			{
				return GridLayout.GetCellGap(this);
			}
		}

		public extern GridLayout.CellLayout cellLayout
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public extern GridLayout.CellSwizzle cellSwizzle
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		private static Vector3 GetCellSize(GridLayout self)
		{
			Vector3 result;
			GridLayout.GetCellSize_Injected(self, out result);
			return result;
		}

		private static Vector3 GetCellGap(GridLayout self)
		{
			Vector3 result;
			GridLayout.GetCellGap_Injected(self, out result);
			return result;
		}

		public Bounds GetBoundsLocal(Vector3Int cellPosition)
		{
			return GridLayout.GetBoundsLocal(this, cellPosition);
		}

		private static Bounds GetBoundsLocal(GridLayout self, Vector3Int cellPosition)
		{
			Bounds result;
			GridLayout.GetBoundsLocal_Injected(self, ref cellPosition, out result);
			return result;
		}

		public Vector3 CellToLocal(Vector3Int cellPosition)
		{
			return GridLayout.CellToLocal(this, cellPosition);
		}

		private static Vector3 CellToLocal(GridLayout self, Vector3Int cellPosition)
		{
			Vector3 result;
			GridLayout.CellToLocal_Injected(self, ref cellPosition, out result);
			return result;
		}

		public Vector3Int LocalToCell(Vector3 localPosition)
		{
			return GridLayout.LocalToCell(this, localPosition);
		}

		private static Vector3Int LocalToCell(GridLayout self, Vector3 localPosition)
		{
			Vector3Int result;
			GridLayout.LocalToCell_Injected(self, ref localPosition, out result);
			return result;
		}

		public Vector3 CellToLocalInterpolated(Vector3 cellPosition)
		{
			return GridLayout.CellToLocalInterpolated(this, cellPosition);
		}

		private static Vector3 CellToLocalInterpolated(GridLayout self, Vector3 cellPosition)
		{
			Vector3 result;
			GridLayout.CellToLocalInterpolated_Injected(self, ref cellPosition, out result);
			return result;
		}

		public Vector3 LocalToCellInterpolated(Vector3 localPosition)
		{
			return GridLayout.LocalToCellInterpolated(this, localPosition);
		}

		private static Vector3 LocalToCellInterpolated(GridLayout self, Vector3 localPosition)
		{
			Vector3 result;
			GridLayout.LocalToCellInterpolated_Injected(self, ref localPosition, out result);
			return result;
		}

		public Vector3 CellToWorld(Vector3Int cellPosition)
		{
			return GridLayout.CellToWorld(this, cellPosition);
		}

		private static Vector3 CellToWorld(GridLayout self, Vector3Int cellPosition)
		{
			Vector3 result;
			GridLayout.CellToWorld_Injected(self, ref cellPosition, out result);
			return result;
		}

		public Vector3Int WorldToCell(Vector3 worldPosition)
		{
			return GridLayout.WorldToCell(this, worldPosition);
		}

		private static Vector3Int WorldToCell(GridLayout self, Vector3 worldPosition)
		{
			Vector3Int result;
			GridLayout.WorldToCell_Injected(self, ref worldPosition, out result);
			return result;
		}

		public Vector3 LocalToWorld(Vector3 localPosition)
		{
			return GridLayout.LocalToWorld(this, localPosition);
		}

		private static Vector3 LocalToWorld(GridLayout self, Vector3 localPosition)
		{
			Vector3 result;
			GridLayout.LocalToWorld_Injected(self, ref localPosition, out result);
			return result;
		}

		public Vector3 WorldToLocal(Vector3 worldPosition)
		{
			return GridLayout.WorldToLocal(this, worldPosition);
		}

		private static Vector3 WorldToLocal(GridLayout self, Vector3 worldPosition)
		{
			Vector3 result;
			GridLayout.WorldToLocal_Injected(self, ref worldPosition, out result);
			return result;
		}

		public Vector3 GetLayoutCellCenter()
		{
			return GridLayout.GetLayoutCellCenter(this);
		}

		private static Vector3 GetLayoutCellCenter(GridLayout self)
		{
			Vector3 result;
			GridLayout.GetLayoutCellCenter_Injected(self, out result);
			return result;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetCellSize_Injected(GridLayout self, out Vector3 ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetCellGap_Injected(GridLayout self, out Vector3 ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetBoundsLocal_Injected(GridLayout self, ref Vector3Int cellPosition, out Bounds ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void CellToLocal_Injected(GridLayout self, ref Vector3Int cellPosition, out Vector3 ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void LocalToCell_Injected(GridLayout self, ref Vector3 localPosition, out Vector3Int ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void CellToLocalInterpolated_Injected(GridLayout self, ref Vector3 cellPosition, out Vector3 ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void LocalToCellInterpolated_Injected(GridLayout self, ref Vector3 localPosition, out Vector3 ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void CellToWorld_Injected(GridLayout self, ref Vector3Int cellPosition, out Vector3 ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void WorldToCell_Injected(GridLayout self, ref Vector3 worldPosition, out Vector3Int ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void LocalToWorld_Injected(GridLayout self, ref Vector3 localPosition, out Vector3 ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void WorldToLocal_Injected(GridLayout self, ref Vector3 worldPosition, out Vector3 ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetLayoutCellCenter_Injected(GridLayout self, out Vector3 ret);
	}
}
