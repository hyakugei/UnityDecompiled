using System;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Animations;

namespace UnityEditor
{
	internal abstract class ConstraintEditorBase : Editor
	{
		private bool m_ShowConstraintSettings = false;

		private ReorderableList m_SourceList;

		private int m_SelectedSourceIdx = -1;

		protected const int kSourceWeightWidth = 60;

		internal abstract SerializedProperty atRest
		{
			get;
		}

		internal abstract SerializedProperty offset
		{
			get;
		}

		internal abstract SerializedProperty weight
		{
			get;
		}

		internal abstract SerializedProperty isContraintActive
		{
			get;
		}

		internal abstract SerializedProperty isLocked
		{
			get;
		}

		internal abstract SerializedProperty sources
		{
			get;
		}

		protected int selectedSourceIndex
		{
			get
			{
				return this.m_SelectedSourceIdx;
			}
			set
			{
				this.m_SelectedSourceIdx = value;
			}
		}

		public void OnEnable(ConstraintStyleBase style)
		{
			Undo.undoRedoPerformed = (Undo.UndoRedoCallback)Delegate.Combine(Undo.undoRedoPerformed, new Undo.UndoRedoCallback(this.OnUndoRedoPerformed));
			this.m_SourceList = new ReorderableList(base.serializedObject, this.sources, this.sources.editable, true, this.sources.editable, this.sources.editable);
			ReorderableList expr_6D = this.m_SourceList;
			expr_6D.drawElementCallback = (ReorderableList.ElementCallbackDelegate)Delegate.Combine(expr_6D.drawElementCallback, new ReorderableList.ElementCallbackDelegate(this.DrawElementCallback));
			ReorderableList expr_94 = this.m_SourceList;
			expr_94.onAddCallback = (ReorderableList.AddCallbackDelegate)Delegate.Combine(expr_94.onAddCallback, new ReorderableList.AddCallbackDelegate(this.OnAddCallback));
			ReorderableList expr_BC = this.m_SourceList;
			expr_BC.drawHeaderCallback = (ReorderableList.HeaderCallbackDelegate)Delegate.Combine(expr_BC.drawHeaderCallback, new ReorderableList.HeaderCallbackDelegate(delegate(Rect rect)
			{
				EditorGUI.LabelField(rect, style.Sources);
			}));
			ReorderableList expr_E3 = this.m_SourceList;
			expr_E3.onRemoveCallback = (ReorderableList.RemoveCallbackDelegate)Delegate.Combine(expr_E3.onRemoveCallback, new ReorderableList.RemoveCallbackDelegate(this.OnRemoveCallback));
			ReorderableList expr_10B = this.m_SourceList;
			expr_10B.onSelectCallback = (ReorderableList.SelectCallbackDelegate)Delegate.Combine(expr_10B.onSelectCallback, new ReorderableList.SelectCallbackDelegate(this.OnSelectedCallback));
			ReorderableList expr_133 = this.m_SourceList;
			expr_133.onReorderCallbackWithDetails = (ReorderableList.ReorderCallbackDelegateWithDetails)Delegate.Combine(expr_133.onReorderCallbackWithDetails, new ReorderableList.ReorderCallbackDelegateWithDetails(this.OnReorderCallback));
			if (this.sources.arraySize > 0 && this.m_SelectedSourceIdx == -1)
			{
				this.SelectSource(0);
			}
		}

		public void OnDisable()
		{
			Undo.undoRedoPerformed = (Undo.UndoRedoCallback)Delegate.Remove(Undo.undoRedoPerformed, new Undo.UndoRedoCallback(this.OnUndoRedoPerformed));
		}

		internal void OnUndoRedoPerformed()
		{
			UnityEngine.Object[] targets = base.targets;
			for (int i = 0; i < targets.Length; i++)
			{
				UnityEngine.Object @object = targets[i];
				(@object as IConstraintInternal).UserUpdateOffset();
			}
		}

		protected void SelectSource(int index)
		{
			this.m_SelectedSourceIdx = index;
			if (this.m_SourceList.index != index)
			{
				this.m_SourceList.index = index;
			}
		}

		protected virtual void OnSelectedCallback(ReorderableList list)
		{
			this.SelectSource(list.index);
		}

		protected virtual void OnReorderCallback(ReorderableList list, int oldActiveElement, int newActiveElement)
		{
		}

		protected virtual void OnRemoveCallback(ReorderableList list)
		{
			ReorderableList.defaultBehaviours.DoRemoveButton(list);
			if (this.m_SelectedSourceIdx >= list.serializedProperty.arraySize)
			{
				this.SelectSource(list.serializedProperty.arraySize - 1);
			}
		}

		protected virtual void OnAddCallback(ReorderableList list)
		{
			int arraySize = list.serializedProperty.arraySize;
			ReorderableList.defaultBehaviours.DoAddButton(list);
			SerializedProperty arrayElementAtIndex = list.serializedProperty.GetArrayElementAtIndex(arraySize);
			arrayElementAtIndex.FindPropertyRelative("sourceTransform").objectReferenceValue = null;
			arrayElementAtIndex.FindPropertyRelative("weight").floatValue = 1f;
			this.SelectSource(arraySize);
		}

		private void DrawElementCallback(Rect rect, int index, bool isActive, bool isFocused)
		{
			rect.height = EditorGUIUtility.singleLineHeight;
			rect.y += 1f;
			SerializedProperty arrayElementAtIndex = this.sources.GetArrayElementAtIndex(index);
			SerializedProperty property = arrayElementAtIndex.FindPropertyRelative("sourceTransform");
			SerializedProperty property2 = arrayElementAtIndex.FindPropertyRelative("weight");
			EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width - 60f, EditorGUIUtility.singleLineHeight), property, GUIContent.none);
			EditorGUI.PropertyField(new Rect(rect.x + rect.width - 60f, rect.y, 60f, EditorGUIUtility.singleLineHeight), property2, GUIContent.none);
		}

		internal abstract void OnValueAtRestChanged();

		internal abstract void ShowFreezeAxesControl();

		internal virtual void ShowCustomProperties()
		{
		}

		internal void ShowConstraintEditor<T>(ConstraintStyleBase style) where T : class, IConstraintInternal
		{
			if (this.m_SelectedSourceIdx == -1 || this.m_SelectedSourceIdx >= this.m_SourceList.serializedProperty.arraySize)
			{
				this.SelectSource(0);
			}
			using (new EditorGUI.DisabledScope(Application.isPlaying))
			{
				using (new EditorGUILayout.HorizontalScope(new GUILayoutOption[0]))
				{
					if (GUILayout.Button(style.Activate, new GUILayoutOption[0]))
					{
						List<UnityEngine.Object> list = new List<UnityEngine.Object>();
						list.AddRange(base.targets);
						UnityEngine.Object[] targets = base.targets;
						for (int i = 0; i < targets.Length; i++)
						{
							UnityEngine.Object @object = targets[i];
							List<UnityEngine.Object> arg_AB_0 = list;
							T t = @object as T;
							arg_AB_0.Add(t.transform);
						}
						Undo.RegisterCompleteObjectUndo(list.ToArray(), "Activate the Constraint");
						UnityEngine.Object[] targets2 = base.targets;
						for (int j = 0; j < targets2.Length; j++)
						{
							UnityEngine.Object object2 = targets2[j];
							T t2 = object2 as T;
							t2.ActivateAndPreserveOffset();
						}
					}
					if (GUILayout.Button(style.Zero, new GUILayoutOption[0]))
					{
						List<UnityEngine.Object> list2 = new List<UnityEngine.Object>();
						list2.AddRange(base.targets);
						UnityEngine.Object[] targets3 = base.targets;
						for (int k = 0; k < targets3.Length; k++)
						{
							UnityEngine.Object object3 = targets3[k];
							List<UnityEngine.Object> arg_180_0 = list2;
							T t3 = object3 as T;
							arg_180_0.Add(t3.transform);
						}
						Undo.RegisterCompleteObjectUndo(list2.ToArray(), "Zero the Constraint");
						UnityEngine.Object[] targets4 = base.targets;
						for (int l = 0; l < targets4.Length; l++)
						{
							UnityEngine.Object object4 = targets4[l];
							T t4 = object4 as T;
							t4.ActivateWithZeroOffset();
						}
					}
				}
			}
			EditorGUILayout.PropertyField(this.isContraintActive, style.IsActive, new GUILayoutOption[0]);
			EditorGUILayout.Slider(this.weight, 0f, 1f, style.Weight, new GUILayoutOption[0]);
			this.ShowCustomProperties();
			this.m_ShowConstraintSettings = EditorGUILayout.Foldout(this.m_ShowConstraintSettings, style.ConstraintSettings, true);
			if (this.m_ShowConstraintSettings)
			{
				EditorGUI.indentLevel++;
				using (new EditorGUI.DisabledScope(Application.isPlaying))
				{
					EditorGUILayout.PropertyField(this.isLocked, style.IsLocked, new GUILayoutOption[0]);
				}
				using (new EditorGUI.DisabledGroupScope(this.isLocked.boolValue))
				{
					this.ShowValueAtRest(style);
					this.ShowOffset<T>(style);
				}
				this.ShowFreezeAxesControl();
				EditorGUI.indentLevel--;
			}
			this.m_SourceList.DoLayoutList();
		}

		internal virtual void ShowValueAtRest(ConstraintStyleBase style)
		{
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(this.atRest, style.AtRest, new GUILayoutOption[0]);
			if (EditorGUI.EndChangeCheck())
			{
				this.OnValueAtRestChanged();
			}
		}

		internal virtual void ShowOffset<T>(ConstraintStyleBase style) where T : class, IConstraintInternal
		{
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(this.offset, style.Offset, new GUILayoutOption[0]);
			if (EditorGUI.EndChangeCheck())
			{
				UnityEngine.Object[] targets = base.targets;
				for (int i = 0; i < targets.Length; i++)
				{
					UnityEngine.Object @object = targets[i];
					T t = @object as T;
					t.UserUpdateOffset();
				}
			}
		}
	}
}
