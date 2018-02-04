using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

namespace UnityEditor
{
	[CanEditMultipleObjects, CustomEditor(typeof(TrailRenderer))]
	internal class TrailRendererInspector : RendererEditorBase
	{
		private class Styles
		{
			public static GUIContent colorGradient = EditorGUIUtility.TrTextContent("Color", "The gradient describing the color along the trail.", null);

			public static GUIContent numCornerVertices = EditorGUIUtility.TrTextContent("Corner Vertices", "How many vertices to add for each corner.", null);

			public static GUIContent numCapVertices = EditorGUIUtility.TrTextContent("End Cap Vertices", "How many vertices to add at each end.", null);

			public static GUIContent alignment = EditorGUIUtility.TrTextContent("Alignment", "Trails can rotate to face their transform component or the camera. Note that when using Local mode, trails will face the XY plane of the Transform.", null);

			public static GUIContent textureMode = EditorGUIUtility.TrTextContent("Texture Mode", "Should the U coordinate be stretched or tiled?", null);

			public static GUIContent generateLightingData = EditorGUIUtility.TrTextContent("Generate Lighting Data", "Toggle generation of normal and tangent data, for use in lit shaders.", null);
		}

		private string[] m_ExcludedProperties;

		private LineRendererCurveEditor m_CurveEditor = new LineRendererCurveEditor();

		private SerializedProperty m_ColorGradient;

		private SerializedProperty m_NumCornerVertices;

		private SerializedProperty m_NumCapVertices;

		private SerializedProperty m_Alignment;

		private SerializedProperty m_TextureMode;

		private SerializedProperty m_GenerateLightingData;

		public override void OnEnable()
		{
			base.OnEnable();
			this.m_ExcludedProperties = new List<string>
			{
				"m_LightProbeUsage",
				"m_LightProbeVolumeOverride",
				"m_ReflectionProbeUsage",
				"m_ProbeAnchor",
				"m_Parameters"
			}.ToArray();
			this.m_CurveEditor.OnEnable(base.serializedObject);
			this.m_ColorGradient = base.serializedObject.FindProperty("m_Parameters.colorGradient");
			this.m_NumCornerVertices = base.serializedObject.FindProperty("m_Parameters.numCornerVertices");
			this.m_NumCapVertices = base.serializedObject.FindProperty("m_Parameters.numCapVertices");
			this.m_Alignment = base.serializedObject.FindProperty("m_Parameters.alignment");
			this.m_TextureMode = base.serializedObject.FindProperty("m_Parameters.textureMode");
			this.m_GenerateLightingData = base.serializedObject.FindProperty("m_Parameters.generateLightingData");
			base.InitializeProbeFields();
		}

		public void OnDisable()
		{
			this.m_CurveEditor.OnDisable();
		}

		public override void OnInspectorGUI()
		{
			base.serializedObject.Update();
			List<string> list = new List<string>();
			if (!SupportedRenderingFeatures.active.rendererSupportsMotionVectors)
			{
				list.Add("m_MotionVectors");
			}
			if (!SupportedRenderingFeatures.active.rendererSupportsReceiveShadows)
			{
				list.Add("m_ReceiveShadows");
			}
			list.AddRange(this.m_ExcludedProperties);
			Editor.DrawPropertiesExcluding(this.m_SerializedObject, list.ToArray());
			this.m_CurveEditor.CheckCurveChangedExternally();
			this.m_CurveEditor.OnInspectorGUI();
			EditorGUILayout.PropertyField(this.m_ColorGradient, TrailRendererInspector.Styles.colorGradient, new GUILayoutOption[0]);
			EditorGUILayout.PropertyField(this.m_NumCornerVertices, TrailRendererInspector.Styles.numCornerVertices, new GUILayoutOption[0]);
			EditorGUILayout.PropertyField(this.m_NumCapVertices, TrailRendererInspector.Styles.numCapVertices, new GUILayoutOption[0]);
			EditorGUILayout.PropertyField(this.m_Alignment, TrailRendererInspector.Styles.alignment, new GUILayoutOption[0]);
			EditorGUILayout.PropertyField(this.m_TextureMode, TrailRendererInspector.Styles.textureMode, new GUILayoutOption[0]);
			EditorGUILayout.PropertyField(this.m_GenerateLightingData, TrailRendererInspector.Styles.generateLightingData, new GUILayoutOption[0]);
			EditorGUILayout.Space();
			base.RenderSortingLayerFields();
			this.m_Probes.OnGUI(base.targets, (Renderer)base.target, false);
			base.RenderRenderingLayer();
			base.serializedObject.ApplyModifiedProperties();
		}
	}
}
