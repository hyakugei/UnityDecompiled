using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

namespace UnityEditor
{
	[CanEditMultipleObjects, CustomEditor(typeof(ParticleRenderer))]
	internal class ParticleRendererEditor : RendererEditorBase
	{
		private string[] m_ExcludedProperties;

		public override void OnEnable()
		{
			base.OnEnable();
			base.InitializeProbeFields();
			List<string> list = new List<string>();
			if (!SupportedRenderingFeatures.active.rendererSupportsMotionVectors)
			{
				list.Add("m_MotionVectors");
			}
			if (!SupportedRenderingFeatures.active.rendererSupportsReceiveShadows)
			{
				list.Add("m_ReceiveShadows");
			}
			list.AddRange(RendererEditorBase.Probes.GetFieldsStringArray());
			this.m_ExcludedProperties = list.ToArray();
		}

		public override void OnInspectorGUI()
		{
			base.serializedObject.Update();
			Editor.DrawPropertiesExcluding(base.serializedObject, this.m_ExcludedProperties);
			this.m_Probes.OnGUI(base.targets, (Renderer)base.target, false);
			base.RenderRenderingLayer();
			base.serializedObject.ApplyModifiedProperties();
		}
	}
}
