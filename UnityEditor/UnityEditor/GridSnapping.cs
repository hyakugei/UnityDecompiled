using System;
using UnityEngine;

namespace UnityEditor
{
	internal static class GridSnapping
	{
		public static bool active
		{
			get
			{
				return GridSnapping.activeGrid != null;
			}
		}

		public static Grid activeGrid
		{
			get
			{
				return (!(Selection.activeGameObject != null)) ? null : Selection.activeGameObject.GetComponentInParent<Grid>();
			}
		}

		public static Vector3 Snap(Vector3 position)
		{
			return GridSnapping.Snap(GridSnapping.activeGrid, position);
		}

		public static Vector3 Snap(Grid grid, Vector3 position)
		{
			Vector3 result = position;
			if (grid != null && !EditorGUI.actionKey)
			{
				Vector3 localPosition = grid.WorldToLocal(position);
				Vector3 vector = grid.LocalToCellInterpolated(localPosition);
				Vector3 cellPosition = new Vector3(Mathf.Round(2f * vector.x) / 2f, Mathf.Round(2f * vector.y) / 2f, Mathf.Round(2f * vector.z) / 2f);
				localPosition = grid.CellToLocalInterpolated(cellPosition);
				result = grid.LocalToWorld(localPosition);
			}
			return result;
		}
	}
}
