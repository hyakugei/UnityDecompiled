using System;
using UnityEngine;
using UnityEngine.Animations;

namespace UnityEditor
{
	[CanEditMultipleObjects, CustomEditor(typeof(RotationConstraint))]
	internal class RotationConstraintEditor : ConstraintEditorBase
	{
		private class Styles : ConstraintStyleBase
		{
			private GUIContent m_RotationAtRest = EditorGUIUtility.TrTextContent("Rotation At Rest", null, null);

			private GUIContent m_RotationOffset = EditorGUIUtility.TrTextContent("Rotation Offset", null, null);

			private GUIContent m_RotationAxes = EditorGUIUtility.TrTextContent("Freeze Rotation Axes", null, null);

			public override GUIContent AtRest
			{
				get
				{
					return this.m_RotationAtRest;
				}
			}

			public override GUIContent Offset
			{
				get
				{
					return this.m_RotationOffset;
				}
			}

			public GUIContent FreezeAxes
			{
				get
				{
					return this.m_RotationAxes;
				}
			}
		}

		private SerializedProperty m_RotationAtRest;

		private SerializedProperty m_RotationOffset;

		private SerializedProperty m_Weight;

		private SerializedProperty m_IsContraintActive;

		private SerializedProperty m_IsLocked;

		private SerializedProperty m_Sources;

		private static RotationConstraintEditor.Styles s_Style = null;

		internal override SerializedProperty atRest
		{
			get
			{
				return this.m_RotationAtRest;
			}
		}

		internal override SerializedProperty offset
		{
			get
			{
				return this.m_RotationOffset;
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
			if (RotationConstraintEditor.s_Style == null)
			{
				RotationConstraintEditor.s_Style = new RotationConstraintEditor.Styles();
			}
			this.m_RotationAtRest = base.serializedObject.FindProperty("m_RotationAtRest");
			this.m_RotationOffset = base.serializedObject.FindProperty("m_RotationOffset");
			this.m_Weight = base.serializedObject.FindProperty("m_Weight");
			this.m_IsContraintActive = base.serializedObject.FindProperty("m_IsContraintActive");
			this.m_IsLocked = base.serializedObject.FindProperty("m_IsLocked");
			this.m_Sources = base.serializedObject.FindProperty("m_Sources");
			base.OnEnable(RotationConstraintEditor.s_Style);
		}

		internal override void OnValueAtRestChanged()
		{
			UnityEngine.Object[] targets = base.targets;
			for (int i = 0; i < targets.Length; i++)
			{
				UnityEngine.Object @object = targets[i];
				(@object as RotationConstraint).transform.SetLocalEulerAngles(this.atRest.vector3Value, RotationOrder.OrderZXY);
			}
		}

		internal override void ShowFreezeAxesControl()
		{
			Rect controlRect = EditorGUILayout.GetControlRect(true, EditorGUI.GetPropertyHeight(SerializedPropertyType.Vector3, RotationConstraintEditor.s_Style.FreezeAxes), EditorStyles.toggle, new GUILayoutOption[0]);
			EditorGUI.MultiPropertyField(controlRect, RotationConstraintEditor.s_Style.Axes, base.serializedObject.FindProperty("m_AffectRotationX"), RotationConstraintEditor.s_Style.FreezeAxes);
		}

		public override void OnInspectorGUI()
		{
			if (RotationConstraintEditor.s_Style == null)
			{
				RotationConstraintEditor.s_Style = new RotationConstraintEditor.Styles();
			}
			base.serializedObject.Update();
			base.ShowConstraintEditor<RotationConstraint>(RotationConstraintEditor.s_Style);
			base.serializedObject.ApplyModifiedProperties();
		}
	}
}
