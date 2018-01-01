using System;
using System.Collections.Generic;
using UnityEditor.U2D;
using UnityEngine;

namespace UnityEditor.Experimental.U2D
{
	internal class SpriteOutlineDataTransfer : SpriteDataProviderBase, ISpriteOutlineDataProvider
	{
		public SpriteOutlineDataTransfer(TextureImporter dp) : base(dp)
		{
		}

		public List<Vector2[]> GetOutlines(GUID guid)
		{
			int spriteDataIndex = base.dataProvider.GetSpriteDataIndex(guid);
			return SpriteOutlineDataTransfer.Load(new SerializedObject(base.dataProvider), base.dataProvider.spriteImportMode, spriteDataIndex);
		}

		public void SetOutlines(GUID guid, List<Vector2[]> data)
		{
			((SpriteDataExt)base.dataProvider.GetSpriteData(guid)).spriteOutline = data;
		}

		public float GetTessellationDetail(GUID guid)
		{
			return ((SpriteDataExt)base.dataProvider.GetSpriteData(guid)).tessellationDetail;
		}

		public void SetTessellationDetail(GUID guid, float value)
		{
			((SpriteDataExt)base.dataProvider.GetSpriteData(guid)).tessellationDetail = value;
		}

		private static List<Vector2[]> Load(SerializedObject importer, SpriteImportMode mode, int index)
		{
			SerializedProperty serializedProperty = (mode != SpriteImportMode.Multiple) ? importer.FindProperty("m_SpriteSheet.m_Outline") : importer.FindProperty("m_SpriteSheet.m_Sprites").GetArrayElementAtIndex(index).FindPropertyRelative("m_Outline");
			List<Vector2[]> list = new List<Vector2[]>();
			for (int i = 0; i < serializedProperty.arraySize; i++)
			{
				SerializedProperty arrayElementAtIndex = serializedProperty.GetArrayElementAtIndex(i);
				Vector2[] array = new Vector2[arrayElementAtIndex.arraySize];
				for (int j = 0; j < arrayElementAtIndex.arraySize; j++)
				{
					array[j] = arrayElementAtIndex.GetArrayElementAtIndex(j).vector2Value;
				}
				list.Add(array);
			}
			return list;
		}

		public static void Apply(SerializedProperty rectSP, List<Vector2[]> outline)
		{
			SerializedProperty serializedProperty = rectSP.FindPropertyRelative("m_Outline");
			serializedProperty.ClearArray();
			for (int i = 0; i < outline.Count; i++)
			{
				serializedProperty.InsertArrayElementAtIndex(i);
				Vector2[] array = outline[i];
				SerializedProperty arrayElementAtIndex = serializedProperty.GetArrayElementAtIndex(i);
				arrayElementAtIndex.ClearArray();
				for (int j = 0; j < array.Length; j++)
				{
					arrayElementAtIndex.InsertArrayElementAtIndex(j);
					arrayElementAtIndex.GetArrayElementAtIndex(j).vector2Value = array[j];
				}
			}
		}
	}
}
