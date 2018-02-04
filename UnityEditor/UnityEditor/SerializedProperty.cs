using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Scripting;

namespace UnityEditor
{
	[StructLayout(LayoutKind.Sequential)]
	public sealed class SerializedProperty : IDisposable
	{
		private IntPtr m_Property;

		internal SerializedObject m_SerializedObject;

		internal string m_CachedLocalizedDisplayName = "";

		public extern bool hasMultipleDifferentValues
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		internal extern int hasMultipleDifferentValuesBitwise
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public extern string displayName
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public extern string name
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public extern string type
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public extern string arrayElementType
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public extern string tooltip
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public extern int depth
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public extern string propertyPath
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		internal extern int hashCodeForPropertyPathWithoutArrayIndex
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public extern bool editable
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public extern bool isAnimated
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		internal extern bool isCandidate
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		internal extern bool isKey
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public extern bool isExpanded
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public extern bool hasChildren
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public extern bool hasVisibleChildren
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public extern bool isInstantiatedPrefab
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public extern bool prefabOverride
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public extern SerializedPropertyType propertyType
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public extern int intValue
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public extern long longValue
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public extern bool boolValue
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public extern float floatValue
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public extern double doubleValue
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public extern string stringValue
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public Color colorValue
		{
			get
			{
				Color result;
				this.INTERNAL_get_colorValue(out result);
				return result;
			}
			set
			{
				this.INTERNAL_set_colorValue(ref value);
			}
		}

		public extern AnimationCurve animationCurveValue
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		internal extern Gradient gradientValue
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public extern UnityEngine.Object objectReferenceValue
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public extern int objectReferenceInstanceIDValue
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		internal extern string objectReferenceStringValue
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		internal extern string objectReferenceTypeString
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		internal extern uint layerMaskBits
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public extern int enumValueIndex
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public extern string[] enumNames
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public extern string[] enumDisplayNames
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public Vector2 vector2Value
		{
			get
			{
				Vector2 result;
				this.INTERNAL_get_vector2Value(out result);
				return result;
			}
			set
			{
				this.INTERNAL_set_vector2Value(ref value);
			}
		}

		public Vector3 vector3Value
		{
			get
			{
				Vector3 result;
				this.INTERNAL_get_vector3Value(out result);
				return result;
			}
			set
			{
				this.INTERNAL_set_vector3Value(ref value);
			}
		}

		public Vector4 vector4Value
		{
			get
			{
				Vector4 result;
				this.INTERNAL_get_vector4Value(out result);
				return result;
			}
			set
			{
				this.INTERNAL_set_vector4Value(ref value);
			}
		}

		public Vector2Int vector2IntValue
		{
			get
			{
				Vector2Int result;
				this.INTERNAL_get_vector2IntValue(out result);
				return result;
			}
			set
			{
				this.INTERNAL_set_vector2IntValue(ref value);
			}
		}

		public Vector3Int vector3IntValue
		{
			get
			{
				Vector3Int result;
				this.INTERNAL_get_vector3IntValue(out result);
				return result;
			}
			set
			{
				this.INTERNAL_set_vector3IntValue(ref value);
			}
		}

		public Quaternion quaternionValue
		{
			get
			{
				Quaternion result;
				this.INTERNAL_get_quaternionValue(out result);
				return result;
			}
			set
			{
				this.INTERNAL_set_quaternionValue(ref value);
			}
		}

		public Rect rectValue
		{
			get
			{
				Rect result;
				this.INTERNAL_get_rectValue(out result);
				return result;
			}
			set
			{
				this.INTERNAL_set_rectValue(ref value);
			}
		}

		public RectInt rectIntValue
		{
			get
			{
				RectInt result;
				this.INTERNAL_get_rectIntValue(out result);
				return result;
			}
			set
			{
				this.INTERNAL_set_rectIntValue(ref value);
			}
		}

		public Bounds boundsValue
		{
			get
			{
				Bounds result;
				this.INTERNAL_get_boundsValue(out result);
				return result;
			}
			set
			{
				this.INTERNAL_set_boundsValue(ref value);
			}
		}

		public BoundsInt boundsIntValue
		{
			get
			{
				BoundsInt result;
				this.INTERNAL_get_boundsIntValue(out result);
				return result;
			}
			set
			{
				this.INTERNAL_set_boundsIntValue(ref value);
			}
		}

		public extern bool isArray
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public extern int arraySize
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public extern bool isFixedBuffer
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public extern int fixedBufferSize
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		internal extern bool isValidDisplayNameCache
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public SerializedObject serializedObject
		{
			get
			{
				return this.m_SerializedObject;
			}
		}

		public UnityEngine.Object exposedReferenceValue
		{
			get
			{
				UnityEngine.Object result;
				if (this.propertyType != SerializedPropertyType.ExposedReference)
				{
					result = null;
				}
				else
				{
					SerializedProperty serializedProperty = this.FindPropertyRelative("defaultValue");
					if (serializedProperty == null)
					{
						result = null;
					}
					else
					{
						UnityEngine.Object @object = serializedProperty.objectReferenceValue;
						IExposedPropertyTable exposedPropertyTable = this.serializedObject.context as IExposedPropertyTable;
						if (exposedPropertyTable != null)
						{
							SerializedProperty serializedProperty2 = this.FindPropertyRelative("exposedName");
							PropertyName id = new PropertyName(serializedProperty2.stringValue);
							bool flag = false;
							UnityEngine.Object referenceValue = exposedPropertyTable.GetReferenceValue(id, out flag);
							if (flag)
							{
								@object = referenceValue;
							}
						}
						result = @object;
					}
				}
				return result;
			}
			set
			{
				if (this.propertyType != SerializedPropertyType.ExposedReference)
				{
					throw new InvalidOperationException("Attempting to set the reference value on a SerializedProperty that is not an ExposedReference");
				}
				SerializedProperty serializedProperty = this.FindPropertyRelative("defaultValue");
				IExposedPropertyTable exposedPropertyTable = this.serializedObject.context as IExposedPropertyTable;
				if (exposedPropertyTable == null)
				{
					serializedProperty.objectReferenceValue = value;
					serializedProperty.serializedObject.ApplyModifiedProperties();
				}
				else
				{
					SerializedProperty serializedProperty2 = this.FindPropertyRelative("exposedName");
					string text = serializedProperty2.stringValue;
					if (string.IsNullOrEmpty(text))
					{
						text = GUID.Generate().ToString();
						serializedProperty2.stringValue = text;
					}
					PropertyName id = new PropertyName(text);
					exposedPropertyTable.SetReferenceValue(id, value);
				}
			}
		}

		internal bool isScript
		{
			get
			{
				return this.type == "PPtr<MonoScript>";
			}
		}

		private bool isUnityAssembly
		{
			get
			{
				return EditorUtility.IsUnityAssembly(this.m_SerializedObject.targetObject);
			}
		}

		internal string localizedDisplayName
		{
			get
			{
				if (!this.isValidDisplayNameCache)
				{
					this.isValidDisplayNameCache = true;
					this.m_CachedLocalizedDisplayName = ((!this.isUnityAssembly) ? this.displayName : L10n.Tr(this.displayName));
				}
				return this.m_CachedLocalizedDisplayName;
			}
		}

		internal string[] enumLocalizedDisplayNames
		{
			get
			{
				string[] enumDisplayNames = this.enumDisplayNames;
				string[] result;
				if (!this.isUnityAssembly)
				{
					result = enumDisplayNames;
				}
				else
				{
					string[] array = new string[enumDisplayNames.Length];
					for (int i = 0; i < array.Length; i++)
					{
						array[i] = L10n.Tr(enumDisplayNames[i]);
					}
					result = array;
				}
				return result;
			}
		}

		internal SerializedProperty()
		{
		}

		[GeneratedByOldBindingsGenerator, ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void Dispose();

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool EqualContents(SerializedProperty x, SerializedProperty y);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void SetBitAtIndexForAllTargetsImmediate(int index, bool value);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_get_colorValue(out Color value);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_set_colorValue(ref Color value);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern bool ValidateObjectReferenceValue(UnityEngine.Object obj);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern bool ValidateObjectReferenceValueExact(UnityEngine.Object obj);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void AppendFoldoutPPtrValue(UnityEngine.Object obj);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern string GetLayerMaskStringValue(uint layers);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_get_vector2Value(out Vector2 value);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_set_vector2Value(ref Vector2 value);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_get_vector3Value(out Vector3 value);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_set_vector3Value(ref Vector3 value);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_get_vector4Value(out Vector4 value);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_set_vector4Value(ref Vector4 value);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_get_vector2IntValue(out Vector2Int value);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_set_vector2IntValue(ref Vector2Int value);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_get_vector3IntValue(out Vector3Int value);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_set_vector3IntValue(ref Vector3Int value);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_get_quaternionValue(out Quaternion value);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_set_quaternionValue(ref Quaternion value);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_get_rectValue(out Rect value);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_set_rectValue(ref Rect value);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_get_rectIntValue(out RectInt value);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_set_rectIntValue(ref RectInt value);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_get_boundsValue(out Bounds value);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_set_boundsValue(ref Bounds value);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_get_boundsIntValue(out BoundsInt value);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_set_boundsIntValue(ref BoundsInt value);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool Next(bool enterChildren);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool NextVisible(bool enterChildren);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void Reset();

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern int CountRemaining();

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern int CountInProperty();

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern SerializedProperty CopyInternal();

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool DuplicateCommand();

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool DeleteCommand();

		[ExcludeFromDocs]
		public SerializedProperty GetEndProperty()
		{
			bool includeInvisible = false;
			return this.GetEndProperty(includeInvisible);
		}

		public SerializedProperty GetEndProperty([DefaultValue("false")] bool includeInvisible)
		{
			SerializedProperty serializedProperty = this.Copy();
			if (includeInvisible)
			{
				serializedProperty.Next(false);
			}
			else
			{
				serializedProperty.NextVisible(false);
			}
			return serializedProperty;
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern bool FindPropertyInternal(string propertyPath);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern bool FindPropertyRelativeInternal(string propertyPath);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int[] GetLayerMaskSelectedIndex(uint layers);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern string[] GetLayerMaskNames(uint layers);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern uint ToggleLayerMask(uint layers, int index);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void ToggleLayerMaskAtIndex(int index);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern bool GetArrayElementAtIndexInternal(int index);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void InsertArrayElementAtIndex(int index);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void DeleteArrayElementAtIndex(int index);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void ClearArray();

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool MoveArrayElement(int srcIndex, int dstIndex);

		public SerializedProperty GetFixedBufferElementAtIndex(int index)
		{
			SerializedProperty serializedProperty = this.Copy();
			SerializedProperty result;
			if (serializedProperty.GetFixedBufferAtIndexInternal(index))
			{
				result = serializedProperty;
			}
			else
			{
				result = null;
			}
			return result;
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern bool GetFixedBufferAtIndexInternal(int index);

		~SerializedProperty()
		{
			this.Dispose();
		}

		public SerializedProperty Copy()
		{
			SerializedProperty serializedProperty = this.CopyInternal();
			serializedProperty.m_SerializedObject = this.m_SerializedObject;
			return serializedProperty;
		}

		public SerializedProperty FindPropertyRelative(string relativePropertyPath)
		{
			SerializedProperty serializedProperty = this.Copy();
			SerializedProperty result;
			if (serializedProperty.FindPropertyRelativeInternal(relativePropertyPath))
			{
				result = serializedProperty;
			}
			else
			{
				result = null;
			}
			return result;
		}

		[DebuggerHidden]
		public IEnumerator GetEnumerator()
		{
			SerializedProperty.<GetEnumerator>c__Iterator0 <GetEnumerator>c__Iterator = new SerializedProperty.<GetEnumerator>c__Iterator0();
			<GetEnumerator>c__Iterator.$this = this;
			return <GetEnumerator>c__Iterator;
		}

		public SerializedProperty GetArrayElementAtIndex(int index)
		{
			SerializedProperty serializedProperty = this.Copy();
			SerializedProperty result;
			if (serializedProperty.GetArrayElementAtIndexInternal(index))
			{
				result = serializedProperty;
			}
			else
			{
				result = null;
			}
			return result;
		}

		internal void SetToValueOfTarget(UnityEngine.Object target)
		{
			SerializedProperty serializedProperty = new SerializedObject(target).FindProperty(this.propertyPath);
			if (serializedProperty == null)
			{
				UnityEngine.Debug.LogError(target.name + " does not have the property " + this.propertyPath);
			}
			else
			{
				switch (this.propertyType)
				{
				case SerializedPropertyType.Integer:
					this.intValue = serializedProperty.intValue;
					break;
				case SerializedPropertyType.Boolean:
					this.boolValue = serializedProperty.boolValue;
					break;
				case SerializedPropertyType.Float:
					this.floatValue = serializedProperty.floatValue;
					break;
				case SerializedPropertyType.String:
					this.stringValue = serializedProperty.stringValue;
					break;
				case SerializedPropertyType.Color:
					this.colorValue = serializedProperty.colorValue;
					break;
				case SerializedPropertyType.ObjectReference:
					this.objectReferenceValue = serializedProperty.objectReferenceValue;
					break;
				case SerializedPropertyType.LayerMask:
					this.intValue = serializedProperty.intValue;
					break;
				case SerializedPropertyType.Enum:
					this.enumValueIndex = serializedProperty.enumValueIndex;
					break;
				case SerializedPropertyType.Vector2:
					this.vector2Value = serializedProperty.vector2Value;
					break;
				case SerializedPropertyType.Vector3:
					this.vector3Value = serializedProperty.vector3Value;
					break;
				case SerializedPropertyType.Vector4:
					this.vector4Value = serializedProperty.vector4Value;
					break;
				case SerializedPropertyType.Rect:
					this.rectValue = serializedProperty.rectValue;
					break;
				case SerializedPropertyType.ArraySize:
					this.intValue = serializedProperty.intValue;
					break;
				case SerializedPropertyType.Character:
					this.intValue = serializedProperty.intValue;
					break;
				case SerializedPropertyType.AnimationCurve:
					this.animationCurveValue = serializedProperty.animationCurveValue;
					break;
				case SerializedPropertyType.Bounds:
					this.boundsValue = serializedProperty.boundsValue;
					break;
				case SerializedPropertyType.Gradient:
					this.gradientValue = serializedProperty.gradientValue;
					break;
				case SerializedPropertyType.ExposedReference:
					this.exposedReferenceValue = serializedProperty.exposedReferenceValue;
					break;
				case SerializedPropertyType.Vector2Int:
					this.vector2IntValue = serializedProperty.vector2IntValue;
					break;
				case SerializedPropertyType.Vector3Int:
					this.vector3IntValue = serializedProperty.vector3IntValue;
					break;
				case SerializedPropertyType.RectInt:
					this.rectIntValue = serializedProperty.rectIntValue;
					break;
				case SerializedPropertyType.BoundsInt:
					this.boundsIntValue = serializedProperty.boundsIntValue;
					break;
				}
			}
		}
	}
}
