using System;
using UnityEngine;
using UnityEngine.Animations;

namespace UnityEditor
{
	[CanEditMultipleObjects, CustomEditor(typeof(AimConstraint))]
	internal class AimConstraintEditor : ConstraintEditorBase
	{
		private class Styles : ConstraintStyleBase
		{
			private GUIContent m_RotationAtRest = EditorGUIUtility.TrTextContent("Rotation At Rest", "The orientation of the constrained object when the weights of the sources add up to zero or when all the rotation axes are disabled.", null);

			private GUIContent m_RotationOffset = EditorGUIUtility.TrTextContent("Rotation Offset", "The offset from the constrained orientation.", null);

			private GUIContent m_RotationAxes = EditorGUIUtility.TrTextContent("Freeze Rotation Axes", "The axes along which the constraint is applied.", null);

			private GUIContent m_AimVector = EditorGUIUtility.TrTextContent("Aim Vector", "Specifies which axis of the constrained object should aim at the target.", null);

			private GUIContent m_UpVector = EditorGUIUtility.TrTextContent("Up Vector", "Specifies the direction of the up vector in local space.", null);

			private GUIContent m_WorldUpVector = EditorGUIUtility.TrTextContent("World Up Vector", "Specifies the direction of the global up vector.", null);

			private GUIContent m_WorldUpObject = EditorGUIUtility.TrTextContent("World Up Object", "The reference object when the World Up Type is either Object Up or Object Rotation Up.", null);

			private GUIContent m_WorldUpType = EditorGUIUtility.TrTextContent("World Up Type", "Specifies how the world up vector should be computed.", null);

			private GUIContent[] m_WorldUpTypes = new GUIContent[]
			{
				EditorGUIUtility.TrTextContent("Scene Up", "Use the Y axis as the world up vector.", null),
				EditorGUIUtility.TrTextContent("Object Up", "Use a vector that points to the reference object as the world up vector.", null),
				EditorGUIUtility.TrTextContent("Object Rotation Up", "Use a vector defined in the reference object's local space as the world up vector.", null),
				EditorGUIUtility.TrTextContent("Vector", "The world up vector is user defined.", null),
				EditorGUIUtility.TrTextContent("None", "The world up vector is ignored.", null)
			};

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

			public GUIContent AimVector
			{
				get
				{
					return this.m_AimVector;
				}
			}

			public GUIContent UpVector
			{
				get
				{
					return this.m_UpVector;
				}
			}

			public GUIContent WorldUpVector
			{
				get
				{
					return this.m_WorldUpVector;
				}
			}

			public GUIContent WorldUpObject
			{
				get
				{
					return this.m_WorldUpObject;
				}
			}

			public GUIContent WorldUpType
			{
				get
				{
					return this.m_WorldUpType;
				}
			}

			public GUIContent[] WorldUpTypes
			{
				get
				{
					return this.m_WorldUpTypes;
				}
			}
		}

		private SerializedProperty m_RotationAtRest;

		private SerializedProperty m_RotationOffset;

		private SerializedProperty m_AimVector;

		private SerializedProperty m_UpVector;

		private SerializedProperty m_WorldUpVector;

		private SerializedProperty m_WorldUpObject;

		private SerializedProperty m_WorldUpType;

		private SerializedProperty m_Weight;

		private SerializedProperty m_IsContraintActive;

		private SerializedProperty m_IsLocked;

		private SerializedProperty m_Sources;

		private static AimConstraintEditor.Styles s_Style = null;

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
			if (AimConstraintEditor.s_Style == null)
			{
				AimConstraintEditor.s_Style = new AimConstraintEditor.Styles();
			}
			this.m_RotationAtRest = base.serializedObject.FindProperty("m_RotationAtRest");
			this.m_RotationOffset = base.serializedObject.FindProperty("m_RotationOffset");
			this.m_AimVector = base.serializedObject.FindProperty("m_AimVector");
			this.m_UpVector = base.serializedObject.FindProperty("m_UpVector");
			this.m_WorldUpVector = base.serializedObject.FindProperty("m_WorldUpVector");
			this.m_WorldUpObject = base.serializedObject.FindProperty("m_WorldUpObject");
			this.m_WorldUpType = base.serializedObject.FindProperty("m_UpType");
			this.m_Weight = base.serializedObject.FindProperty("m_Weight");
			this.m_IsContraintActive = base.serializedObject.FindProperty("m_IsContraintActive");
			this.m_IsLocked = base.serializedObject.FindProperty("m_IsLocked");
			this.m_Sources = base.serializedObject.FindProperty("m_Sources");
			base.OnEnable(AimConstraintEditor.s_Style);
		}

		internal override void OnValueAtRestChanged()
		{
			UnityEngine.Object[] targets = base.targets;
			for (int i = 0; i < targets.Length; i++)
			{
				UnityEngine.Object @object = targets[i];
				(@object as AimConstraint).transform.SetLocalEulerAngles(this.atRest.vector3Value, RotationOrder.OrderZXY);
			}
		}

		internal override void ShowOffset<T>(ConstraintStyleBase style)
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

		internal override void ShowCustomProperties()
		{
			EditorGUILayout.PropertyField(this.m_AimVector, AimConstraintEditor.s_Style.AimVector, new GUILayoutOption[0]);
			EditorGUILayout.PropertyField(this.m_UpVector, AimConstraintEditor.s_Style.UpVector, new GUILayoutOption[0]);
			EditorGUILayout.Popup(this.m_WorldUpType, AimConstraintEditor.s_Style.WorldUpTypes, AimConstraintEditor.s_Style.WorldUpType, new GUILayoutOption[0]);
			AimConstraint.WorldUpType intValue = (AimConstraint.WorldUpType)this.m_WorldUpType.intValue;
			using (new EditorGUI.DisabledGroupScope(intValue != AimConstraint.WorldUpType.ObjectRotationUp && intValue != AimConstraint.WorldUpType.Vector))
			{
				EditorGUILayout.PropertyField(this.m_WorldUpVector, AimConstraintEditor.s_Style.WorldUpVector, new GUILayoutOption[0]);
			}
			using (new EditorGUI.DisabledGroupScope(intValue != AimConstraint.WorldUpType.ObjectUp && intValue != AimConstraint.WorldUpType.ObjectRotationUp))
			{
				EditorGUILayout.PropertyField(this.m_WorldUpObject, AimConstraintEditor.s_Style.WorldUpObject, new GUILayoutOption[0]);
			}
		}

		internal override void ShowFreezeAxesControl()
		{
			Rect controlRect = EditorGUILayout.GetControlRect(true, EditorGUI.GetPropertyHeight(SerializedPropertyType.Vector3, AimConstraintEditor.s_Style.FreezeAxes), EditorStyles.toggle, new GUILayoutOption[0]);
			EditorGUI.MultiPropertyField(controlRect, AimConstraintEditor.s_Style.Axes, base.serializedObject.FindProperty("m_AffectRotationX"), AimConstraintEditor.s_Style.FreezeAxes);
		}

		public override void OnInspectorGUI()
		{
			if (AimConstraintEditor.s_Style == null)
			{
				AimConstraintEditor.s_Style = new AimConstraintEditor.Styles();
			}
			base.serializedObject.Update();
			base.ShowConstraintEditor<AimConstraint>(AimConstraintEditor.s_Style);
			base.serializedObject.ApplyModifiedProperties();
		}
	}
}
