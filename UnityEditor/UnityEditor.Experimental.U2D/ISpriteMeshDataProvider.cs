using System;
using UnityEngine;

namespace UnityEditor.Experimental.U2D
{
	public interface ISpriteMeshDataProvider
	{
		Vertex2DMetaData[] GetVertices(GUID guid);

		void SetVertices(GUID guid, Vertex2DMetaData[] vertices);

		int[] GetIndices(GUID guid);

		void SetIndices(GUID guid, int[] indices);

		Vector2Int[] GetEdges(GUID guid);

		void SetEdges(GUID guid, Vector2Int[] edges);
	}
}
