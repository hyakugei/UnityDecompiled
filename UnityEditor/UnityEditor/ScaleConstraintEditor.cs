using System;
using UnityEngine;
using UnityEngine.Animations;

namespace UnityEditor
{
	[CanEditMultipleObjects, CustomEditor(typeof(ScaleConstraint))]
	internal class ScaleConstraintEditor : ConstraintEditorBase
	{
		private class Styles : ConstraintStyleBase
		{
			private GUIContent m_ScaleAtRest = EditorGUIUtility.TrTextContent("Scale At Rest", null, null);

			private GUIContent m_ScaleOffset = EditorGUIUtility.TrTextContent("Scale Offset", null, null);

			private GUIContent m_ScalingAxes = EditorGUIUtility.TrTextContent("Freeze Scaling Axes", null, null);

			public override GUIContent AtRest
			{
				get
				{
					return this.m_ScaleAtRest;
				}
			}

			public override GUIContent Offset
			{
				get
				{
					return this.m_ScaleOffset;
				}
			}

			public GUIContent FreezeAxes
			{
				get
				{
					return this.m_ScalingAxes;
				}
			}
		}

		private SerializedProperty m_ScaleAtRest;

		private SerializedProperty m_ScaleOffset;

		private SerializedProperty m_Weight;

		private SerializedProperty m_IsContraintActive;

		private SerializedProperty m_IsLocked;

		private SerializedProperty m_Sources;

		private static ScaleConstraintEditor.Styles s_Style;

		internal override SerializedProperty atRest
		{
			get
			{
				return this.m_ScaleAtRest;
			}
		}

		internal override SerializedProperty offset
		{
			get
			{
				return this.m_ScaleOffset;
			}
		}

		internal override SerializedProperty weight
		{
			get
			{
				return this.m_Weight;
			}
		}

		internal override SerializedProperty isContraintActive
		{
			get
			{
				return this.m_IsContraintActive;
			}
		}

		internal override SerializedProperty isLocked
		{
			get
			{
				return this.m_IsLocked;
			}
		}

		internal override SerializedProperty sources
		{
			get
			{
				return this.m_Sources;
			}
		}

		public void OnEnable()
		{
			if (ScaleConstraintEditor.s_Style == null)
			{
				ScaleConstraintEditor.s_Style = new ScaleConstraintEditor.Styles();
			}
			this.m_ScaleAtRest = base.serializedObject.FindProperty("m_ScaleAtRest");
			this.m_ScaleOffset = base.serializedObject.FindProperty("m_ScaleOffset");
			this.m_Weight = base.serializedObject.FindProperty("m_Weight");
			this.m_IsContraintActive = base.serializedObject.FindProperty("m_IsContraintActive");
			this.m_IsLocked = base.serializedObject.FindProperty("m_IsLocked");
			this.m_Sources = base.serializedObject.FindProperty("m_Sources");
			base.OnEnable(ScaleConstraintEditor.s_Style);
		}

		internal override void OnValueAtRestChanged()
		{
			UnityEngine.Object[] targets = base.targets;
			for (int i = 0; i < targets.Length; i++)
			{
				UnityEngine.Object @object = targets[i];
				(@object as ScaleConstraint).transform.localScale = this.atRest.vector3Value;
			}
		}

		internal override void ShowFreezeAxesControl()
		{
			Rect controlRect = EditorGUILayout.GetControlRect(true, EditorGUI.GetPropertyHeight(SerializedPropertyType.Vector3, ScaleConstraintEditor.s_Style.FreezeAxes), EditorStyles.toggle, new GUILayoutOption[0]);
			EditorGUI.MultiPropertyField(controlRect, ScaleConstraintEditor.s_Style.Axes, base.serializedObject.FindProperty("m_AffectScalingX"), ScaleConstraintEditor.s_Style.FreezeAxes);
		}

		public override void OnInspectorGUI()
		{
			if (ScaleConstraintEditor.s_Style == null)
			{
				ScaleConstraintEditor.s_Style = new ScaleConstraintEditor.Styles();
			}
			base.serializedObject.Update();
			base.ShowConstraintEditor<ScaleConstraint>(ScaleConstraintEditor.s_Style);
			base.serializedObject.ApplyModifiedProperties();
		}
	}
}
