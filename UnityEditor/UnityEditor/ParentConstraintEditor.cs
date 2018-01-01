using System;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Animations;

namespace UnityEditor
{
	[CanEditMultipleObjects, CustomEditor(typeof(ParentConstraint))]
	internal class ParentConstraintEditor : ConstraintEditorBase
	{
		private class Styles : ConstraintStyleBase
		{
			private GUIContent m_RestTranslation = EditorGUIUtility.TextContent("Position At Rest");

			private GUIContent m_TranslationOffset = EditorGUIUtility.TextContent("Position Offset");

			private GUIContent m_RestRotation = EditorGUIUtility.TextContent("Rotation At Rest");

			private GUIContent m_RotationOffset = EditorGUIUtility.TextContent("Rotation Offset");

			private GUIContent m_TranslationAxes = EditorGUIUtility.TextContent("Freeze Position Axes");

			private GUIContent m_RotationAxes = EditorGUIUtility.TextContent("Freeze Rotation Axes");

			private GUIContent m_DefaultSourceName = EditorGUIUtility.TextContent("None");

			private GUIContent m_SourceOffsets = EditorGUIUtility.TextContent("Source Offsets");

			public override GUIContent AtRest
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			public override GUIContent Offset
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			public GUIContent TranslationAtRest
			{
				get
				{
					return this.m_RestTranslation;
				}
			}

			public GUIContent RotationAtRest
			{
				get
				{
					return this.m_RestRotation;
				}
			}

			public GUIContent TranslationOffset
			{
				get
				{
					return this.m_TranslationOffset;
				}
			}

			public GUIContent RotationOffset
			{
				get
				{
					return this.m_RotationOffset;
				}
			}

			public GUIContent FreezeTranslationAxes
			{
				get
				{
					return this.m_TranslationAxes;
				}
			}

			public GUIContent FreezeRotationAxes
			{
				get
				{
					return this.m_RotationAxes;
				}
			}

			public GUIContent SourceOffsets
			{
				get
				{
					return this.m_SourceOffsets;
				}
			}

			public GUIContent DefaultSourceName
			{
				get
				{
					return this.m_DefaultSourceName;
				}
			}
		}

		private SerializedProperty m_TranslationAtRest;

		private SerializedProperty m_TranslationOffsets;

		private SerializedProperty m_RotationAtRest;

		private SerializedProperty m_RotationOffsets;

		private SerializedProperty m_Weight;

		private SerializedProperty m_IsContraintActive;

		private SerializedProperty m_IsLocked;

		private SerializedProperty m_Sources;

		private ReorderableList m_OffsetList;

		private static ParentConstraintEditor.Styles s_Style;

		private static readonly float kListItemHeight = 3f * (EditorGUIUtility.singleLineHeight + 2f * EditorGUIUtility.standardVerticalSpacing);

		private static readonly float kListItemNarrowHeight = 5f * (EditorGUIUtility.singleLineHeight + 2f * EditorGUIUtility.standardVerticalSpacing);

		internal override SerializedProperty atRest
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		internal override SerializedProperty offset
		{
			get
			{
				throw new NotImplementedException();
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
			if (ParentConstraintEditor.s_Style == null)
			{
				ParentConstraintEditor.s_Style = new ParentConstraintEditor.Styles();
			}
			this.m_TranslationAtRest = base.serializedObject.FindProperty("m_TranslationAtRest");
			this.m_TranslationOffsets = base.serializedObject.FindProperty("m_TranslationOffsets");
			this.m_RotationAtRest = base.serializedObject.FindProperty("m_RotationAtRest");
			this.m_RotationOffsets = base.serializedObject.FindProperty("m_RotationOffsets");
			this.m_Weight = base.serializedObject.FindProperty("m_Weight");
			this.m_IsContraintActive = base.serializedObject.FindProperty("m_IsContraintActive");
			this.m_IsLocked = base.serializedObject.FindProperty("m_IsLocked");
			this.m_Sources = base.serializedObject.FindProperty("m_Sources");
			this.m_OffsetList = new ReorderableList(base.serializedObject, this.sources, false, false, false, false);
			ReorderableList expr_E6 = this.m_OffsetList;
			expr_E6.drawHeaderCallback = (ReorderableList.HeaderCallbackDelegate)Delegate.Combine(expr_E6.drawHeaderCallback, new ReorderableList.HeaderCallbackDelegate(delegate(Rect rect)
			{
				EditorGUI.LabelField(rect, ParentConstraintEditor.s_Style.SourceOffsets);
			}));
			ReorderableList expr_11E = this.m_OffsetList;
			expr_11E.drawElementCallback = (ReorderableList.ElementCallbackDelegate)Delegate.Combine(expr_11E.drawElementCallback, new ReorderableList.ElementCallbackDelegate(this.DrawOffsetElementCallback));
			ReorderableList expr_145 = this.m_OffsetList;
			expr_145.elementHeightCallback = (ReorderableList.ElementHeightCallbackDelegate)Delegate.Combine(expr_145.elementHeightCallback, new ReorderableList.ElementHeightCallbackDelegate(this.OnElementHeightCallback));
			base.OnEnable(ParentConstraintEditor.s_Style);
			this.m_OffsetList.index = base.selectedSourceIndex;
		}

		private void DrawOffsetElementCallback(Rect rect, int index, bool isActive, bool isFocused)
		{
			SerializedProperty arrayElementAtIndex = this.m_TranslationOffsets.GetArrayElementAtIndex(index);
			SerializedProperty arrayElementAtIndex2 = this.m_RotationOffsets.GetArrayElementAtIndex(index);
			SerializedProperty arrayElementAtIndex3 = this.m_Sources.GetArrayElementAtIndex(index);
			SerializedProperty serializedProperty = arrayElementAtIndex3.FindPropertyRelative("sourceTransform");
			GUIContent label = ParentConstraintEditor.s_Style.DefaultSourceName;
			if (serializedProperty.objectReferenceValue != null)
			{
				label = EditorGUIUtility.TextContent(serializedProperty.objectReferenceValue.name);
			}
			Rect position = rect;
			position.height = EditorGUIUtility.singleLineHeight;
			position.y += EditorGUIUtility.standardVerticalSpacing;
			EditorGUI.LabelField(position, label);
			position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
			EditorGUI.PropertyField(position, arrayElementAtIndex, ParentConstraintEditor.s_Style.TranslationOffset);
			position.y += ((!EditorGUIUtility.wideMode) ? (2f * (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing)) : (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing));
			EditorGUI.PropertyField(position, arrayElementAtIndex2, ParentConstraintEditor.s_Style.RotationOffset);
		}

		protected override void OnRemoveCallback(ReorderableList list)
		{
			int index = list.index;
			ReorderableList.defaultBehaviours.DoRemoveButton(list);
			this.m_TranslationOffsets.DeleteArrayElementAtIndex(index);
			this.m_RotationOffsets.DeleteArrayElementAtIndex(index);
			if (base.selectedSourceIndex >= list.serializedProperty.arraySize)
			{
				base.SelectSource(list.serializedProperty.arraySize - 1);
			}
		}

		protected override void OnAddCallback(ReorderableList list)
		{
			int arraySize = list.serializedProperty.arraySize;
			ReorderableList.defaultBehaviours.DoAddButton(list);
			this.m_TranslationOffsets.arraySize++;
			this.m_RotationOffsets.arraySize++;
			SerializedProperty arrayElementAtIndex = list.serializedProperty.GetArrayElementAtIndex(arraySize);
			arrayElementAtIndex.FindPropertyRelative("sourceTransform").objectReferenceValue = null;
			arrayElementAtIndex.FindPropertyRelative("weight").floatValue = 1f;
			base.SelectSource(arraySize);
		}

		protected override void OnReorderCallback(ReorderableList list, int oldActiveElement, int newActiveElement)
		{
			this.m_TranslationOffsets.MoveArrayElement(oldActiveElement, newActiveElement);
			this.m_RotationOffsets.MoveArrayElement(oldActiveElement, newActiveElement);
			this.m_SerializedObject.ApplyModifiedProperties();
			this.m_SerializedObject.Update();
		}

		protected override void OnSelectedCallback(ReorderableList list)
		{
			base.OnSelectedCallback(list);
			this.m_OffsetList.index = list.index;
		}

		private float OnElementHeightCallback(int index)
		{
			float result;
			if (EditorGUIUtility.wideMode)
			{
				result = ParentConstraintEditor.kListItemHeight;
			}
			else
			{
				result = ParentConstraintEditor.kListItemNarrowHeight;
			}
			return result;
		}

		internal override void OnValueAtRestChanged()
		{
			UnityEngine.Object[] targets = base.targets;
			for (int i = 0; i < targets.Length; i++)
			{
				UnityEngine.Object @object = targets[i];
				(@object as IConstraintInternal).transform.localPosition = this.m_TranslationAtRest.vector3Value;
				(@object as IConstraintInternal).transform.SetLocalEulerAngles(this.m_RotationAtRest.vector3Value, RotationOrder.OrderZXY);
			}
		}

		internal override void ShowFreezeAxesControl()
		{
			Rect controlRect = EditorGUILayout.GetControlRect(true, EditorGUI.GetPropertyHeight(SerializedPropertyType.Vector3, ParentConstraintEditor.s_Style.FreezeTranslationAxes), EditorStyles.toggle, new GUILayoutOption[0]);
			EditorGUI.MultiPropertyField(controlRect, ParentConstraintEditor.s_Style.Axes, base.serializedObject.FindProperty("m_AffectTranslationX"), ParentConstraintEditor.s_Style.FreezeTranslationAxes);
			Rect controlRect2 = EditorGUILayout.GetControlRect(true, EditorGUI.GetPropertyHeight(SerializedPropertyType.Vector3, ParentConstraintEditor.s_Style.FreezeRotationAxes), EditorStyles.toggle, new GUILayoutOption[0]);
			EditorGUI.MultiPropertyField(controlRect2, ParentConstraintEditor.s_Style.Axes, base.serializedObject.FindProperty("m_AffectRotationX"), ParentConstraintEditor.s_Style.FreezeRotationAxes);
		}

		internal override void ShowValueAtRest(ConstraintStyleBase style)
		{
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(this.m_TranslationAtRest, (style as ParentConstraintEditor.Styles).TranslationAtRest, new GUILayoutOption[0]);
			EditorGUILayout.PropertyField(this.m_RotationAtRest, (style as ParentConstraintEditor.Styles).RotationAtRest, new GUILayoutOption[0]);
			if (EditorGUI.EndChangeCheck())
			{
				this.OnValueAtRestChanged();
			}
		}

		internal override void ShowOffset<T>(ConstraintStyleBase style)
		{
			using (new EditorGUI.DisabledGroupScope(this.isLocked.boolValue))
			{
				EditorGUI.BeginChangeCheck();
				this.m_OffsetList.DoLayoutList();
				if (EditorGUI.EndChangeCheck())
				{
					UnityEngine.Object[] targets = base.targets;
					for (int i = 0; i < targets.Length; i++)
					{
						UnityEngine.Object @object = targets[i];
						(@object as IConstraintInternal).UserUpdateOffset();
					}
				}
			}
		}

		public override void OnInspectorGUI()
		{
			if (ParentConstraintEditor.s_Style == null)
			{
				ParentConstraintEditor.s_Style = new ParentConstraintEditor.Styles();
			}
			base.serializedObject.Update();
			base.ShowConstraintEditor<ParentConstraint>(ParentConstraintEditor.s_Style);
			base.serializedObject.ApplyModifiedProperties();
		}
	}
}
