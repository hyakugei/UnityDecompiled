using System;
using System.Collections.Generic;
using UnityEditor.U2D;
using UnityEngine;

namespace UnityEditor.Experimental.U2D
{
	internal class SpriteMeshDataTransfer : SpriteDataProviderBase, ISpriteMeshDataProvider
	{
		public SpriteMeshDataTransfer(TextureImporter dp) : base(dp)
		{
		}

		public Vertex2DMetaData[] GetVertices(GUID guid)
		{
			int spriteDataIndex = base.dataProvider.GetSpriteDataIndex(guid);
			return this.LoadVertex2DMetaData(new SerializedObject(base.dataProvider), base.dataProvider.spriteImportMode, spriteDataIndex);
		}

		public void SetVertices(GUID guid, Vertex2DMetaData[] data)
		{
			((SpriteDataExt)base.dataProvider.GetSpriteData(guid)).vertices = new List<Vertex2DMetaData>(data);
		}

		public int[] GetIndices(GUID guid)
		{
			int spriteDataIndex = base.dataProvider.GetSpriteDataIndex(guid);
			return this.LoadIndices(new SerializedObject(base.dataProvider), base.dataProvider.spriteImportMode, spriteDataIndex);
		}

		public void SetIndices(GUID guid, int[] indices)
		{
			((SpriteDataExt)base.dataProvider.GetSpriteData(guid)).indices = new List<int>(indices);
		}

		public Vector2Int[] GetEdges(GUID guid)
		{
			int spriteDataIndex = base.dataProvider.GetSpriteDataIndex(guid);
			return this.LoadEdges(new SerializedObject(base.dataProvider), base.dataProvider.spriteImportMode, spriteDataIndex);
		}

		public void SetEdges(GUID guid, Vector2Int[] edges)
		{
			((SpriteDataExt)base.dataProvider.GetSpriteData(guid)).edges = new List<Vector2Int>(edges);
		}

		private Vertex2DMetaData[] LoadVertex2DMetaData(SerializedObject importer, SpriteImportMode mode, int index)
		{
			SerializedProperty serializedProperty = (mode != SpriteImportMode.Multiple) ? importer.FindProperty("m_SpriteSheet") : importer.FindProperty("m_SpriteSheet.m_Sprites").GetArrayElementAtIndex(index);
			SerializedProperty serializedProperty2 = serializedProperty.FindPropertyRelative("m_Vertices");
			SerializedProperty serializedProperty3 = serializedProperty.FindPropertyRelative("m_Weights");
			Vertex2DMetaData[] array = new Vertex2DMetaData[serializedProperty2.arraySize];
			for (int i = 0; i < serializedProperty2.arraySize; i++)
			{
				SerializedProperty arrayElementAtIndex = serializedProperty2.GetArrayElementAtIndex(i);
				SerializedProperty arrayElementAtIndex2 = serializedProperty3.GetArrayElementAtIndex(i);
				array[i] = new Vertex2DMetaData
				{
					position = arrayElementAtIndex.vector2Value,
					boneWeight = new BoneWeight
					{
						weight0 = arrayElementAtIndex2.FindPropertyRelative("weight[0]").floatValue,
						weight1 = arrayElementAtIndex2.FindPropertyRelative("weight[1]").floatValue,
						weight2 = arrayElementAtIndex2.FindPropertyRelative("weight[2]").floatValue,
						weight3 = arrayElementAtIndex2.FindPropertyRelative("weight[3]").floatValue,
						boneIndex0 = arrayElementAtIndex2.FindPropertyRelative("boneIndex[0]").intValue,
						boneIndex1 = arrayElementAtIndex2.FindPropertyRelative("boneIndex[1]").intValue,
						boneIndex2 = arrayElementAtIndex2.FindPropertyRelative("boneIndex[2]").intValue,
						boneIndex3 = arrayElementAtIndex2.FindPropertyRelative("boneIndex[3]").intValue
					}
				};
			}
			return array;
		}

		private int[] LoadIndices(SerializedObject importer, SpriteImportMode mode, int index)
		{
			SerializedProperty serializedProperty = (mode != SpriteImportMode.Multiple) ? importer.FindProperty("m_SpriteSheet") : importer.FindProperty("m_SpriteSheet.m_Sprites").GetArrayElementAtIndex(index);
			SerializedProperty serializedProperty2 = serializedProperty.FindPropertyRelative("m_Indices");
			int[] array = new int[serializedProperty2.arraySize];
			for (int i = 0; i < serializedProperty2.arraySize; i++)
			{
				array[i] = serializedProperty2.GetArrayElementAtIndex(i).intValue;
			}
			return array;
		}

		private Vector2Int[] LoadEdges(SerializedObject importer, SpriteImportMode mode, int index)
		{
			SerializedProperty serializedProperty = (mode != SpriteImportMode.Multiple) ? importer.FindProperty("m_SpriteSheet") : importer.FindProperty("m_SpriteSheet.m_Sprites").GetArrayElementAtIndex(index);
			SerializedProperty serializedProperty2 = serializedProperty.FindPropertyRelative("m_Edges");
			Vector2Int[] array = new Vector2Int[serializedProperty2.arraySize];
			for (int i = 0; i < serializedProperty2.arraySize; i++)
			{
				array[i] = serializedProperty2.GetArrayElementAtIndex(i).vector2IntValue;
			}
			return array;
		}

		public static void Apply(SerializedProperty rectSP, List<Vertex2DMetaData> vertices, List<int> indices, List<Vector2Int> edges)
		{
			SerializedProperty serializedProperty = rectSP.FindPropertyRelative("m_Vertices");
			SerializedProperty serializedProperty2 = rectSP.FindPropertyRelative("m_Weights");
			SerializedProperty serializedProperty3 = rectSP.FindPropertyRelative("m_Indices");
			SerializedProperty serializedProperty4 = rectSP.FindPropertyRelative("m_Edges");
			serializedProperty.arraySize = vertices.Count;
			serializedProperty2.arraySize = vertices.Count;
			for (int i = 0; i < vertices.Count; i++)
			{
				SerializedProperty arrayElementAtIndex = serializedProperty.GetArrayElementAtIndex(i);
				SerializedProperty arrayElementAtIndex2 = serializedProperty2.GetArrayElementAtIndex(i);
				arrayElementAtIndex.vector2Value = vertices[i].position;
				arrayElementAtIndex2.FindPropertyRelative("weight[0]").floatValue = vertices[i].boneWeight.weight0;
				arrayElementAtIndex2.FindPropertyRelative("weight[1]").floatValue = vertices[i].boneWeight.weight1;
				arrayElementAtIndex2.FindPropertyRelative("weight[2]").floatValue = vertices[i].boneWeight.weight2;
				arrayElementAtIndex2.FindPropertyRelative("weight[3]").floatValue = vertices[i].boneWeight.weight3;
				arrayElementAtIndex2.FindPropertyRelative("boneIndex[0]").intValue = vertices[i].boneWeight.boneIndex0;
				arrayElementAtIndex2.FindPropertyRelative("boneIndex[1]").intValue = vertices[i].boneWeight.boneIndex1;
				arrayElementAtIndex2.FindPropertyRelative("boneIndex[2]").intValue = vertices[i].boneWeight.boneIndex2;
				arrayElementAtIndex2.FindPropertyRelative("boneIndex[3]").intValue = vertices[i].boneWeight.boneIndex3;
			}
			serializedProperty3.arraySize = indices.Count;
			for (int j = 0; j < indices.Count; j++)
			{
				serializedProperty3.GetArrayElementAtIndex(j).intValue = indices[j];
			}
			serializedProperty4.arraySize = edges.Count;
			for (int k = 0; k < edges.Count; k++)
			{
				serializedProperty4.GetArrayElementAtIndex(k).vector2IntValue = edges[k];
			}
		}
	}
}
