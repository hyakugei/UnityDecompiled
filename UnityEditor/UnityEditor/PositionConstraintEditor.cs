using System;
using UnityEngine;
using UnityEngine.Animations;

namespace UnityEditor
{
	[CanEditMultipleObjects, CustomEditor(typeof(PositionConstraint))]
	internal class PositionConstraintEditor : ConstraintEditorBase
	{
		private class Styles : ConstraintStyleBase
		{
			private GUIContent m_RestTranslation = EditorGUIUtility.TrTextContent("Position At Rest", null, null);

			private GUIContent m_TranslationOffset = EditorGUIUtility.TrTextContent("Position Offset", null, null);

			private GUIContent m_TranslationAxes = EditorGUIUtility.TrTextContent("Freeze Position Axes", null, null);

			public override GUIContent AtRest
			{
				get
				{
					return this.m_RestTranslation;
				}
			}

			public override GUIContent Offset
			{
				get
				{
					return this.m_TranslationOffset;
				}
			}

			public GUIContent FreezeAxes
			{
				get
				{
					return this.m_TranslationAxes;
				}
			}
		}

		private SerializedProperty m_TranslationAtRest;

		private SerializedProperty m_TranslationOffset;

		private SerializedProperty m_Weight;

		private SerializedProperty m_IsContraintActive;

		private SerializedProperty m_IsLocked;

		private SerializedProperty m_Sources;

		private static PositionConstraintEditor.Styles s_Style;

		internal override SerializedProperty atRest
		{
			get
			{
				return this.m_TranslationAtRest;
			}
		}

		internal override SerializedProperty offset
		{
			get
			{
				return this.m_TranslationOffset;
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
			if (PositionConstraintEditor.s_Style == null)
			{
				PositionConstraintEditor.s_Style = new PositionConstraintEditor.Styles();
			}
			this.m_TranslationAtRest = base.serializedObject.FindProperty("m_TranslationAtRest");
			this.m_TranslationOffset = base.serializedObject.FindProperty("m_TranslationOffset");
			this.m_Weight = base.serializedObject.FindProperty("m_Weight");
			this.m_IsContraintActive = base.serializedObject.FindProperty("m_IsContraintActive");
			this.m_IsLocked = base.serializedObject.FindProperty("m_IsLocked");
			this.m_Sources = base.serializedObject.FindProperty("m_Sources");
			base.OnEnable(PositionConstraintEditor.s_Style);
		}

		internal override void OnValueAtRestChanged()
		{
			UnityEngine.Object[] targets = base.targets;
			for (int i = 0; i < targets.Length; i++)
			{
				UnityEngine.Object @object = targets[i];
				(@object as PositionConstraint).transform.localPosition = this.atRest.vector3Value;
			}
		}

		internal override void ShowFreezeAxesControl()
		{
			Rect controlRect = EditorGUILayout.GetControlRect(true, EditorGUI.GetPropertyHeight(SerializedPropertyType.Vector3, PositionConstraintEditor.s_Style.FreezeAxes), EditorStyles.toggle, new GUILayoutOption[0]);
			EditorGUI.MultiPropertyField(controlRect, PositionConstraintEditor.s_Style.Axes, base.serializedObject.FindProperty("m_AffectTranslationX"), PositionConstraintEditor.s_Style.FreezeAxes);
		}

		public override void OnInspectorGUI()
		{
			if (PositionConstraintEditor.s_Style == null)
			{
				PositionConstraintEditor.s_Style = new PositionConstraintEditor.Styles();
			}
			base.serializedObject.Update();
			base.ShowConstraintEditor<PositionConstraint>(PositionConstraintEditor.s_Style);
			base.serializedObject.ApplyModifiedProperties();
		}
	}
}
