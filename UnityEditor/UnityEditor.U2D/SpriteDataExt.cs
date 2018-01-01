using System;
using System.Collections.Generic;
using UnityEditor.Experimental.U2D;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Experimental.U2D;

namespace UnityEditor.U2D
{
	internal class SpriteDataExt : SpriteRect
	{
		public List<SpriteBone> spriteBone;

		public float tessellationDetail = 0f;

		public List<Vector2[]> spriteOutline;

		public List<Vertex2DMetaData> vertices;

		public List<int> indices;

		public List<Vector2Int> edges;

		public List<Vector2[]> spritePhysicsOutline;

		internal SpriteDataExt()
		{
		}

		internal SpriteDataExt(SpriteRect sr)
		{
			base.originalName = sr.originalName;
			base.name = sr.name;
			base.border = sr.border;
			this.tessellationDetail = 0f;
			base.rect = sr.rect;
			base.spriteID = sr.spriteID;
			base.alignment = sr.alignment;
			base.pivot = sr.pivot;
			this.spriteOutline = new List<Vector2[]>();
			this.spritePhysicsOutline = new List<Vector2[]>();
			this.spriteBone = new List<SpriteBone>();
		}

		public void Apply(SerializedObject so)
		{
			so.FindProperty("m_Alignment").intValue = (int)base.alignment;
			so.FindProperty("m_SpriteBorder").vector4Value = base.border;
			so.FindProperty("m_SpritePivot").vector2Value = base.pivot;
			so.FindProperty("m_SpriteTessellationDetail").floatValue = this.tessellationDetail;
			so.FindProperty("m_SpriteSheet.m_SpriteID").stringValue = base.spriteID.ToString();
			SerializedProperty rectSP = so.FindProperty("m_SpriteSheet");
			if (this.spriteBone != null)
			{
				SpriteBoneDataTransfer.Apply(rectSP, this.spriteBone);
			}
			if (this.spriteOutline != null)
			{
				SpriteOutlineDataTransfer.Apply(rectSP, this.spriteOutline);
			}
			if (this.spritePhysicsOutline != null)
			{
				SpritePhysicsOutlineDataTransfer.Apply(rectSP, this.spritePhysicsOutline);
			}
			if (this.vertices != null)
			{
				SpriteMeshDataTransfer.Apply(rectSP, this.vertices, this.indices, this.edges);
			}
		}

		public void Apply(SerializedProperty sp)
		{
			sp.FindPropertyRelative("m_Rect").rectValue = base.rect;
			sp.FindPropertyRelative("m_Name").stringValue = base.name;
			sp.FindPropertyRelative("m_Border").vector4Value = base.border;
			sp.FindPropertyRelative("m_Alignment").intValue = (int)base.alignment;
			sp.FindPropertyRelative("m_Pivot").vector2Value = base.pivot;
			sp.FindPropertyRelative("m_TessellationDetail").floatValue = this.tessellationDetail;
			sp.FindPropertyRelative("m_SpriteID").stringValue = base.spriteID.ToString();
			if (this.spriteBone != null)
			{
				SpriteBoneDataTransfer.Apply(sp, this.spriteBone);
			}
			if (this.spriteOutline != null)
			{
				SpriteOutlineDataTransfer.Apply(sp, this.spriteOutline);
			}
			if (this.spritePhysicsOutline != null)
			{
				SpritePhysicsOutlineDataTransfer.Apply(sp, this.spritePhysicsOutline);
			}
			if (this.vertices != null)
			{
				SpriteMeshDataTransfer.Apply(sp, this.vertices, this.indices, this.edges);
			}
		}

		public void Load(SerializedObject so)
		{
			TextureImporter textureImporter = so.targetObject as TextureImporter;
			Texture texture = AssetDatabase.LoadAssetAtPath<Texture>(textureImporter.assetPath);
			base.name = texture.name;
			base.alignment = (SpriteAlignment)so.FindProperty("m_Alignment").intValue;
			base.border = textureImporter.spriteBorder;
			base.pivot = SpriteEditorUtility.GetPivotValue(base.alignment, textureImporter.spritePivot);
			this.tessellationDetail = so.FindProperty("m_SpriteTessellationDetail").floatValue;
			int num = 0;
			int num2 = 0;
			textureImporter.GetWidthAndHeight(ref num, ref num2);
			base.rect = new Rect(0f, 0f, (float)num, (float)num2);
			SerializedProperty serializedProperty = so.FindProperty("m_SpriteSheet.m_SpriteID");
			base.spriteID = new GUID(serializedProperty.stringValue);
		}

		public void Load(SerializedProperty sp)
		{
			base.rect = sp.FindPropertyRelative("m_Rect").rectValue;
			base.border = sp.FindPropertyRelative("m_Border").vector4Value;
			base.name = sp.FindPropertyRelative("m_Name").stringValue;
			base.alignment = (SpriteAlignment)sp.FindPropertyRelative("m_Alignment").intValue;
			base.pivot = SpriteEditorUtility.GetPivotValue(base.alignment, sp.FindPropertyRelative("m_Pivot").vector2Value);
			this.tessellationDetail = sp.FindPropertyRelative("m_TessellationDetail").floatValue;
			base.spriteID = new GUID(sp.FindPropertyRelative("m_SpriteID").stringValue);
		}

		public void CopyFromSpriteRect(SpriteRect spriteRect)
		{
			base.alignment = spriteRect.alignment;
			base.border = spriteRect.border;
			base.name = spriteRect.name;
			base.pivot = spriteRect.pivot;
			base.rect = spriteRect.rect;
			base.spriteID = spriteRect.spriteID;
		}
	}
}
