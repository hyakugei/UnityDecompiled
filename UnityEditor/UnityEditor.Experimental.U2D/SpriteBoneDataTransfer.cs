using System;
using System.Collections.Generic;
using UnityEditor.U2D;
using UnityEngine.Experimental.U2D;

namespace UnityEditor.Experimental.U2D
{
	internal class SpriteBoneDataTransfer : SpriteDataProviderBase, ISpriteBoneDataProvider
	{
		public SpriteBoneDataTransfer(TextureImporter dp) : base(dp)
		{
		}

		public List<SpriteBone> GetBones(GUID guid)
		{
			int spriteDataIndex = base.dataProvider.GetSpriteDataIndex(guid);
			return SpriteBoneDataTransfer.Load(new SerializedObject(base.dataProvider), base.dataProvider.spriteImportMode, spriteDataIndex);
		}

		public void SetBones(GUID guid, List<SpriteBone> bones)
		{
			((SpriteDataExt)base.dataProvider.GetSpriteData(guid)).spriteBone = bones;
		}

		private static List<SpriteBone> Load(SerializedObject importer, SpriteImportMode mode, int index)
		{
			SerializedProperty serializedProperty = (mode != SpriteImportMode.Multiple) ? importer.FindProperty("m_SpriteSheet.m_Bones") : importer.FindProperty("m_SpriteSheet.m_Sprites").GetArrayElementAtIndex(index).FindPropertyRelative("m_Bones");
			List<SpriteBone> list = new List<SpriteBone>(serializedProperty.arraySize);
			for (int i = 0; i < serializedProperty.arraySize; i++)
			{
				SerializedProperty arrayElementAtIndex = serializedProperty.GetArrayElementAtIndex(i);
				list.Add(new SpriteBone
				{
					length = arrayElementAtIndex.FindPropertyRelative("length").floatValue,
					position = arrayElementAtIndex.FindPropertyRelative("position").vector3Value,
					rotation = arrayElementAtIndex.FindPropertyRelative("rotation").quaternionValue,
					parentId = arrayElementAtIndex.FindPropertyRelative("parentId").intValue,
					name = arrayElementAtIndex.FindPropertyRelative("name").stringValue
				});
			}
			return list;
		}

		public static void Apply(SerializedProperty rectSP, List<SpriteBone> spriteBone)
		{
			SerializedProperty serializedProperty = rectSP.FindPropertyRelative("m_Bones");
			serializedProperty.arraySize = spriteBone.Count;
			for (int i = 0; i < serializedProperty.arraySize; i++)
			{
				SerializedProperty arrayElementAtIndex = serializedProperty.GetArrayElementAtIndex(i);
				SpriteBone spriteBone2 = spriteBone[i];
				arrayElementAtIndex.FindPropertyRelative("length").floatValue = spriteBone2.length;
				arrayElementAtIndex.FindPropertyRelative("position").vector3Value = spriteBone2.position;
				arrayElementAtIndex.FindPropertyRelative("rotation").quaternionValue = spriteBone2.rotation;
				arrayElementAtIndex.FindPropertyRelative("parentId").intValue = spriteBone2.parentId;
				arrayElementAtIndex.FindPropertyRelative("name").stringValue = spriteBone2.name;
			}
		}
	}
}
