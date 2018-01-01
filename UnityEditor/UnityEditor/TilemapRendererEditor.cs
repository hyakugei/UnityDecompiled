using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace UnityEditor
{
	[CanEditMultipleObjects, CustomEditor(typeof(TilemapRenderer))]
	internal class TilemapRendererEditor : RendererEditorBase
	{
		private static class Styles
		{
			public static readonly GUIContent materialLabel = EditorGUIUtility.TrTextContent("Material", "Material to be used by TilemapRenderer", null);

			public static readonly GUIContent maskInteractionLabel = EditorGUIUtility.TrTextContent("Mask Interaction", "TilemapRenderer's interaction with a Sprite Mask", null);
		}

		private SerializedProperty m_Material;

		private SerializedProperty m_SortOrder;

		private SerializedProperty m_MaskInteraction;

		public override void OnEnable()
		{
			base.OnEnable();
			this.m_Material = base.serializedObject.FindProperty("m_Materials.Array");
			this.m_SortOrder = base.serializedObject.FindProperty("m_SortOrder");
			this.m_MaskInteraction = base.serializedObject.FindProperty("m_MaskInteraction");
		}

		public override void OnInspectorGUI()
		{
			base.serializedObject.Update();
			EditorGUILayout.PropertyField(this.m_Material.GetArrayElementAtIndex(0), TilemapRendererEditor.Styles.materialLabel, true, new GUILayoutOption[0]);
			EditorGUILayout.PropertyField(this.m_SortOrder, new GUILayoutOption[0]);
			base.RenderSortingLayerFields();
			EditorGUILayout.Space();
			EditorGUILayout.PropertyField(this.m_MaskInteraction, TilemapRendererEditor.Styles.maskInteractionLabel, new GUILayoutOption[0]);
			base.RenderRenderingLayer();
			base.serializedObject.ApplyModifiedProperties();
		}
	}
}
