using System;
using UnityEngine;
using UnityEngine.Experimental.U2D;

namespace UnityEditor.Experimental.U2D
{
	[CanEditMultipleObjects, CustomEditor(typeof(SpriteShapeRenderer))]
	internal class SpriteShapeRendererInspector : RendererEditorBase
	{
		private static class Contents
		{
			public static readonly GUIContent materialLabel = EditorGUIUtility.TextContent("Material|Material to be used by SpriteRenderer");

			public static readonly GUIContent colorLabel = EditorGUIUtility.TextContent("Color|Rendering color for the Sprite graphic");

			public static readonly Texture2D warningIcon = EditorGUIUtility.LoadIcon("console.warnicon");
		}

		private SerializedProperty m_Color;

		private SerializedProperty m_Material;

		private SerializedProperty m_MaskInteraction;

		private static Texture2D s_WarningIcon;

		public override void OnEnable()
		{
			base.OnEnable();
			this.m_Color = base.serializedObject.FindProperty("m_Color");
			this.m_Material = base.serializedObject.FindProperty("m_Materials.Array");
			this.m_MaskInteraction = base.serializedObject.FindProperty("m_MaskInteraction");
		}

		public override void OnInspectorGUI()
		{
			base.serializedObject.Update();
			GUI.enabled = true;
			EditorGUILayout.PropertyField(this.m_Color, SpriteShapeRendererInspector.Contents.colorLabel, true, new GUILayoutOption[0]);
			if (this.m_Material.arraySize == 0)
			{
				this.m_Material.InsertArrayElementAtIndex(0);
			}
			Rect rect = GUILayoutUtility.GetRect(EditorGUILayout.kLabelFloatMinW, EditorGUILayout.kLabelFloatMaxW, 16f, 16f);
			EditorGUI.showMixedValue = this.m_Material.hasMultipleDifferentValues;
			UnityEngine.Object objectReferenceValue = this.m_Material.GetArrayElementAtIndex(0).objectReferenceValue;
			UnityEngine.Object @object = EditorGUI.ObjectField(rect, SpriteShapeRendererInspector.Contents.materialLabel, objectReferenceValue, typeof(Material), false);
			if (@object != objectReferenceValue)
			{
				this.m_Material.GetArrayElementAtIndex(0).objectReferenceValue = @object;
			}
			EditorGUI.showMixedValue = false;
			bool flag;
			if (!this.DoesMaterialHaveSpriteTexture(out flag))
			{
				SpriteShapeRendererInspector.ShowError("Material does not have a _MainTex texture property. It is required for SpriteRenderer.");
			}
			else if (flag)
			{
				SpriteShapeRendererInspector.ShowError("Material texture property _MainTex has offset/scale set. It is incompatible with SpriteRenderer.");
			}
			EditorGUILayout.PropertyField(this.m_MaskInteraction, new GUILayoutOption[0]);
			base.RenderSortingLayerFields();
			base.serializedObject.ApplyModifiedProperties();
		}

		private bool DoesMaterialHaveSpriteTexture(out bool tiled)
		{
			tiled = false;
			Material sharedMaterial = (base.target as SpriteShapeRenderer).sharedMaterial;
			bool result;
			if (sharedMaterial == null)
			{
				result = true;
			}
			else
			{
				bool flag = sharedMaterial.HasProperty("_MainTex");
				if (flag)
				{
					Vector2 textureOffset = sharedMaterial.GetTextureOffset("_MainTex");
					Vector2 textureScale = sharedMaterial.GetTextureScale("_MainTex");
					if (textureOffset.x != 0f || textureOffset.y != 0f || textureScale.x != 1f || textureScale.y != 1f)
					{
						tiled = true;
					}
				}
				result = sharedMaterial.HasProperty("_MainTex");
			}
			return result;
		}

		private static void ShowError(string error)
		{
			if (SpriteShapeRendererInspector.s_WarningIcon == null)
			{
				SpriteShapeRendererInspector.s_WarningIcon = EditorGUIUtility.LoadIcon("console.warnicon");
			}
			GUIContent content = new GUIContent(error)
			{
				image = SpriteShapeRendererInspector.s_WarningIcon
			};
			GUILayout.Space(5f);
			GUILayout.BeginVertical(EditorStyles.helpBox, new GUILayoutOption[0]);
			GUILayout.Label(content, EditorStyles.wordWrappedMiniLabel, new GUILayoutOption[0]);
			GUILayout.EndVertical();
		}
	}
}
