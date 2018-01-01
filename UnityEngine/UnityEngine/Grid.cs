using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	[NativeType(Header = "Modules/Grid/Public/Grid.h"), RequireComponent(typeof(Transform))]
	public sealed class Grid : GridLayout
	{
		public new Vector3 cellSize
		{
			get
			{
				return Grid.GetCellSize(this);
			}
			set
			{
				Grid.SetCellSize(this, value);
			}
		}

		public new Vector3 cellGap
		{
			get
			{
				return Grid.GetCellGap(this);
			}
			set
			{
				Grid.SetCellGap(this, value);
			}
		}

		public new extern GridLayout.CellLayout cellLayout
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public new extern GridLayout.CellSwizzle cellSwizzle
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public Vector3 GetCellCenterLocal(Vector3Int position)
		{
			return base.CellToLocalInterpolated(position + base.GetLayoutCellCenter());
		}

		public Vector3 GetCellCenterWorld(Vector3Int position)
		{
			return base.LocalToWorld(base.CellToLocalInterpolated(position + base.GetLayoutCellCenter()));
		}

		private static Vector3 GetCellSize(Grid self)
		{
			Vector3 result;
			Grid.GetCellSize_Injected(self, out result);
			return result;
		}

		private static void SetCellSize(Grid self, Vector3 value)
		{
			Grid.SetCellSize_Injected(self, ref value);
		}

		private static Vector3 GetCellGap(Grid self)
		{
			Vector3 result;
			Grid.GetCellGap_Injected(self, out result);
			return result;
		}

		private static void SetCellGap(Grid self, Vector3 value)
		{
			Grid.SetCellGap_Injected(self, ref value);
		}

		public static Vector3 Swizzle(GridLayout.CellSwizzle swizzle, Vector3 position)
		{
			Vector3 result;
			Grid.Swizzle_Injected(swizzle, ref position, out result);
			return result;
		}

		public static Vector3 InverseSwizzle(GridLayout.CellSwizzle swizzle, Vector3 position)
		{
			Vector3 result;
			Grid.InverseSwizzle_Injected(swizzle, ref position, out result);
			return result;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetCellSize_Injected(Grid self, out Vector3 ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetCellSize_Injected(Grid self, ref Vector3 value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetCellGap_Injected(Grid self, out Vector3 ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetCellGap_Injected(Grid self, ref Vector3 value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Swizzle_Injected(GridLayout.CellSwizzle swizzle, ref Vector3 position, out Vector3 ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void InverseSwizzle_Injected(GridLayout.CellSwizzle swizzle, ref Vector3 position, out Vector3 ret);
	}
}
