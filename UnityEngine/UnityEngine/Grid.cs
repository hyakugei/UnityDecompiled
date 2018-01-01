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
				Vector3 result;
				this.get_cellSize_Injected(out result);
				return result;
			}
			set
			{
				this.set_cellSize_Injected(ref value);
			}
		}

		public new Vector3 cellGap
		{
			get
			{
				Vector3 result;
				this.get_cellGap_Injected(out result);
				return result;
			}
			set
			{
				this.set_cellGap_Injected(ref value);
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
		private extern void get_cellSize_Injected(out Vector3 ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_cellSize_Injected(ref Vector3 value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_cellGap_Injected(out Vector3 ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_cellGap_Injected(ref Vector3 value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Swizzle_Injected(GridLayout.CellSwizzle swizzle, ref Vector3 position, out Vector3 ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void InverseSwizzle_Injected(GridLayout.CellSwizzle swizzle, ref Vector3 position, out Vector3 ret);
	}
}
