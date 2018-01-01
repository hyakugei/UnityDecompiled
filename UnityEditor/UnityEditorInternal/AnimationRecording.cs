using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace UnityEditorInternal
{
	internal class AnimationRecording
	{
		internal class RotationModification
		{
			public UndoPropertyModification x;

			public UndoPropertyModification y;

			public UndoPropertyModification z;

			public UndoPropertyModification w;

			public UndoPropertyModification lastQuatModification;

			public bool useEuler = false;

			public UndoPropertyModification eulerX;

			public UndoPropertyModification eulerY;

			public UndoPropertyModification eulerZ;
		}

		internal class Vector3Modification
		{
			public UndoPropertyModification x;

			public UndoPropertyModification y;

			public UndoPropertyModification z;

			public UndoPropertyModification last;
		}

		internal class RootMotionModification
		{
			public UndoPropertyModification px;

			public UndoPropertyModification py;

			public UndoPropertyModification pz;

			public UndoPropertyModification lastP;

			public UndoPropertyModification rx;

			public UndoPropertyModification ry;

			public UndoPropertyModification rz;

			public UndoPropertyModification rw;

			public UndoPropertyModification lastR;
		}

		private const string kLocalPosition = "m_LocalPosition";

		private const string kLocalRotation = "m_LocalRotation";

		private const string kLocalScale = "m_LocalScale";

		private const string kLocalEulerAnglesHint = "m_LocalEulerAnglesHint";

		private static bool HasAnyRecordableModifications(GameObject root, UndoPropertyModification[] modifications)
		{
			bool result;
			for (int i = 0; i < modifications.Length; i++)
			{
				EditorCurveBinding editorCurveBinding;
				if (AnimationUtility.PropertyModificationToEditorCurveBinding(modifications[i].previousValue, root, out editorCurveBinding) != null)
				{
					result = true;
					return result;
				}
			}
			result = false;
			return result;
		}

		private static UndoPropertyModification[] FilterModifications(IAnimationRecordingState state, ref UndoPropertyModification[] modifications)
		{
			AnimationClip activeAnimationClip = state.activeAnimationClip;
			GameObject activeRootGameObject = state.activeRootGameObject;
			List<UndoPropertyModification> list = new List<UndoPropertyModification>();
			List<UndoPropertyModification> list2 = new List<UndoPropertyModification>();
			for (int i = 0; i < modifications.Length; i++)
			{
				UndoPropertyModification item = modifications[i];
				PropertyModification previousValue = item.previousValue;
				if (state.DiscardModification(previousValue))
				{
					list.Add(item);
				}
				else
				{
					EditorCurveBinding editorCurveBinding = default(EditorCurveBinding);
					if (AnimationUtility.PropertyModificationToEditorCurveBinding(previousValue, activeRootGameObject, out editorCurveBinding) != null)
					{
						list2.Add(item);
					}
					else
					{
						list.Add(item);
					}
				}
			}
			if (list.Count > 0)
			{
				modifications = list2.ToArray();
			}
			return list.ToArray();
		}

		private static void CollectRotationModifications(IAnimationRecordingState state, ref UndoPropertyModification[] modifications, ref Dictionary<object, AnimationRecording.RotationModification> rotationModifications)
		{
			List<UndoPropertyModification> list = new List<UndoPropertyModification>();
			UndoPropertyModification[] array = modifications;
			for (int i = 0; i < array.Length; i++)
			{
				UndoPropertyModification undoPropertyModification = array[i];
				PropertyModification previousValue = undoPropertyModification.previousValue;
				if (!(previousValue.target is Transform))
				{
					list.Add(undoPropertyModification);
				}
				else
				{
					EditorCurveBinding editorCurveBinding = default(EditorCurveBinding);
					AnimationUtility.PropertyModificationToEditorCurveBinding(previousValue, state.activeRootGameObject, out editorCurveBinding);
					if (editorCurveBinding.propertyName.StartsWith("m_LocalRotation"))
					{
						AnimationRecording.RotationModification rotationModification;
						if (!rotationModifications.TryGetValue(previousValue.target, out rotationModification))
						{
							rotationModification = new AnimationRecording.RotationModification();
							rotationModifications[previousValue.target] = rotationModification;
						}
						if (editorCurveBinding.propertyName.EndsWith("x"))
						{
							rotationModification.x = undoPropertyModification;
						}
						else if (editorCurveBinding.propertyName.EndsWith("y"))
						{
							rotationModification.y = undoPropertyModification;
						}
						else if (editorCurveBinding.propertyName.EndsWith("z"))
						{
							rotationModification.z = undoPropertyModification;
						}
						else if (editorCurveBinding.propertyName.EndsWith("w"))
						{
							rotationModification.w = undoPropertyModification;
						}
						rotationModification.lastQuatModification = undoPropertyModification;
					}
					else if (previousValue.propertyPath.StartsWith("m_LocalEulerAnglesHint"))
					{
						AnimationRecording.RotationModification rotationModification2;
						if (!rotationModifications.TryGetValue(previousValue.target, out rotationModification2))
						{
							rotationModification2 = new AnimationRecording.RotationModification();
							rotationModifications[previousValue.target] = rotationModification2;
						}
						rotationModification2.useEuler = true;
						if (previousValue.propertyPath.EndsWith("x"))
						{
							rotationModification2.eulerX = undoPropertyModification;
						}
						else if (previousValue.propertyPath.EndsWith("y"))
						{
							rotationModification2.eulerY = undoPropertyModification;
						}
						else if (previousValue.propertyPath.EndsWith("z"))
						{
							rotationModification2.eulerZ = undoPropertyModification;
						}
					}
					else
					{
						list.Add(undoPropertyModification);
					}
				}
			}
			if (rotationModifications.Count > 0)
			{
				modifications = list.ToArray();
			}
		}

		private static void DiscardRotationModification(AnimationRecording.RotationModification rotationModification, ref List<UndoPropertyModification> discardedModifications)
		{
			if (rotationModification.x.currentValue != null)
			{
				discardedModifications.Add(rotationModification.x);
			}
			if (rotationModification.y.currentValue != null)
			{
				discardedModifications.Add(rotationModification.y);
			}
			if (rotationModification.z.currentValue != null)
			{
				discardedModifications.Add(rotationModification.z);
			}
			if (rotationModification.w.currentValue != null)
			{
				discardedModifications.Add(rotationModification.w);
			}
			if (rotationModification.eulerX.currentValue != null)
			{
				discardedModifications.Add(rotationModification.eulerX);
			}
			if (rotationModification.eulerY.currentValue != null)
			{
				discardedModifications.Add(rotationModification.eulerY);
			}
			if (rotationModification.eulerZ.currentValue != null)
			{
				discardedModifications.Add(rotationModification.eulerZ);
			}
		}

		private static UndoPropertyModification[] FilterRotationModifications(IAnimationRecordingState state, ref Dictionary<object, AnimationRecording.RotationModification> rotationModifications)
		{
			AnimationClip activeAnimationClip = state.activeAnimationClip;
			GameObject activeRootGameObject = state.activeRootGameObject;
			List<object> list = new List<object>();
			List<UndoPropertyModification> list2 = new List<UndoPropertyModification>();
			foreach (KeyValuePair<object, AnimationRecording.RotationModification> current in rotationModifications)
			{
				AnimationRecording.RotationModification value = current.Value;
				if (state.DiscardModification(value.lastQuatModification.currentValue))
				{
					AnimationRecording.DiscardRotationModification(value, ref list2);
					list.Add(current.Key);
				}
				else
				{
					EditorCurveBinding editorCurveBinding = default(EditorCurveBinding);
					if (AnimationUtility.PropertyModificationToEditorCurveBinding(value.lastQuatModification.currentValue, activeRootGameObject, out editorCurveBinding) == null)
					{
						AnimationRecording.DiscardRotationModification(value, ref list2);
						list.Add(current.Key);
					}
				}
			}
			foreach (object current2 in list)
			{
				rotationModifications.Remove(current2);
			}
			return list2.ToArray();
		}

		private static void AddRotationPropertyModification(IAnimationRecordingState state, EditorCurveBinding baseBinding, UndoPropertyModification modification)
		{
			if (modification.previousValue != null)
			{
				EditorCurveBinding binding = baseBinding;
				binding.propertyName = modification.previousValue.propertyPath;
				state.AddPropertyModification(binding, modification.previousValue, modification.keepPrefabOverride);
			}
		}

		private static void ProcessRotationModifications(IAnimationRecordingState state, ref Dictionary<object, AnimationRecording.RotationModification> rotationModifications)
		{
			AnimationClip activeAnimationClip = state.activeAnimationClip;
			GameObject activeRootGameObject = state.activeRootGameObject;
			foreach (KeyValuePair<object, AnimationRecording.RotationModification> current in rotationModifications)
			{
				AnimationRecording.RotationModification value = current.Value;
				Transform transform = current.Key as Transform;
				if (!(transform == null))
				{
					EditorCurveBinding editorCurveBinding = default(EditorCurveBinding);
					Type type = AnimationUtility.PropertyModificationToEditorCurveBinding(value.lastQuatModification.currentValue, state.activeRootGameObject, out editorCurveBinding);
					if (type != null)
					{
						AnimationRecording.AddRotationPropertyModification(state, editorCurveBinding, value.x);
						AnimationRecording.AddRotationPropertyModification(state, editorCurveBinding, value.y);
						AnimationRecording.AddRotationPropertyModification(state, editorCurveBinding, value.z);
						AnimationRecording.AddRotationPropertyModification(state, editorCurveBinding, value.w);
						Quaternion localRotation = transform.localRotation;
						Quaternion localRotation2 = transform.localRotation;
						object obj;
						if (AnimationRecording.ValueFromPropertyModification(value.x.previousValue, editorCurveBinding, out obj))
						{
							localRotation.x = (float)obj;
						}
						object obj2;
						if (AnimationRecording.ValueFromPropertyModification(value.y.previousValue, editorCurveBinding, out obj2))
						{
							localRotation.y = (float)obj2;
						}
						object obj3;
						if (AnimationRecording.ValueFromPropertyModification(value.z.previousValue, editorCurveBinding, out obj3))
						{
							localRotation.z = (float)obj3;
						}
						object obj4;
						if (AnimationRecording.ValueFromPropertyModification(value.w.previousValue, editorCurveBinding, out obj4))
						{
							localRotation.w = (float)obj4;
						}
						if (AnimationRecording.ValueFromPropertyModification(value.x.currentValue, editorCurveBinding, out obj))
						{
							localRotation2.x = (float)obj;
						}
						if (AnimationRecording.ValueFromPropertyModification(value.y.currentValue, editorCurveBinding, out obj2))
						{
							localRotation2.y = (float)obj2;
						}
						if (AnimationRecording.ValueFromPropertyModification(value.z.currentValue, editorCurveBinding, out obj3))
						{
							localRotation2.z = (float)obj3;
						}
						if (AnimationRecording.ValueFromPropertyModification(value.w.currentValue, editorCurveBinding, out obj4))
						{
							localRotation2.w = (float)obj4;
						}
						if (value.useEuler)
						{
							AnimationRecording.AddRotationPropertyModification(state, editorCurveBinding, value.eulerX);
							AnimationRecording.AddRotationPropertyModification(state, editorCurveBinding, value.eulerY);
							AnimationRecording.AddRotationPropertyModification(state, editorCurveBinding, value.eulerZ);
							Vector3 vector = transform.GetLocalEulerAngles(RotationOrder.OrderZXY);
							Vector3 vector2 = vector;
							object obj5;
							if (AnimationRecording.ValueFromPropertyModification(value.eulerX.previousValue, editorCurveBinding, out obj5))
							{
								vector.x = (float)obj5;
							}
							object obj6;
							if (AnimationRecording.ValueFromPropertyModification(value.eulerY.previousValue, editorCurveBinding, out obj6))
							{
								vector.y = (float)obj6;
							}
							object obj7;
							if (AnimationRecording.ValueFromPropertyModification(value.eulerZ.previousValue, editorCurveBinding, out obj7))
							{
								vector.z = (float)obj7;
							}
							if (AnimationRecording.ValueFromPropertyModification(value.eulerX.currentValue, editorCurveBinding, out obj5))
							{
								vector2.x = (float)obj5;
							}
							if (AnimationRecording.ValueFromPropertyModification(value.eulerY.currentValue, editorCurveBinding, out obj6))
							{
								vector2.y = (float)obj6;
							}
							if (AnimationRecording.ValueFromPropertyModification(value.eulerZ.currentValue, editorCurveBinding, out obj7))
							{
								vector2.z = (float)obj7;
							}
							vector = AnimationUtility.GetClosestEuler(localRotation, vector, RotationOrder.OrderZXY);
							vector2 = AnimationUtility.GetClosestEuler(localRotation2, vector2, RotationOrder.OrderZXY);
							AnimationRecording.AddRotationKey(state, editorCurveBinding, type, vector, vector2);
						}
						else
						{
							Vector3 localEulerAngles = transform.GetLocalEulerAngles(RotationOrder.OrderZXY);
							Vector3 closestEuler = AnimationUtility.GetClosestEuler(localRotation, localEulerAngles, RotationOrder.OrderZXY);
							Vector3 closestEuler2 = AnimationUtility.GetClosestEuler(localRotation2, localEulerAngles, RotationOrder.OrderZXY);
							AnimationRecording.AddRotationKey(state, editorCurveBinding, type, closestEuler, closestEuler2);
						}
					}
				}
			}
		}

		private static void CollectVector3Modifications(IAnimationRecordingState state, ref UndoPropertyModification[] modifications, ref Dictionary<object, AnimationRecording.Vector3Modification> vector3Modifications, string propertyName)
		{
			List<UndoPropertyModification> list = new List<UndoPropertyModification>();
			UndoPropertyModification[] array = modifications;
			for (int i = 0; i < array.Length; i++)
			{
				UndoPropertyModification undoPropertyModification = array[i];
				PropertyModification previousValue = undoPropertyModification.previousValue;
				if (!(previousValue.target is Transform))
				{
					list.Add(undoPropertyModification);
				}
				else
				{
					EditorCurveBinding editorCurveBinding = default(EditorCurveBinding);
					AnimationUtility.PropertyModificationToEditorCurveBinding(previousValue, state.activeRootGameObject, out editorCurveBinding);
					if (editorCurveBinding.propertyName.StartsWith(propertyName))
					{
						AnimationRecording.Vector3Modification vector3Modification;
						if (!vector3Modifications.TryGetValue(previousValue.target, out vector3Modification))
						{
							vector3Modification = new AnimationRecording.Vector3Modification();
							vector3Modifications[previousValue.target] = vector3Modification;
						}
						if (editorCurveBinding.propertyName.EndsWith("x"))
						{
							vector3Modification.x = undoPropertyModification;
						}
						else if (editorCurveBinding.propertyName.EndsWith("y"))
						{
							vector3Modification.y = undoPropertyModification;
						}
						else if (editorCurveBinding.propertyName.EndsWith("z"))
						{
							vector3Modification.z = undoPropertyModification;
						}
						vector3Modification.last = undoPropertyModification;
					}
					else
					{
						list.Add(undoPropertyModification);
					}
				}
			}
			if (vector3Modifications.Count > 0)
			{
				modifications = list.ToArray();
			}
		}

		private static void ProcessVector3Modification(IAnimationRecordingState state, EditorCurveBinding baseBinding, UndoPropertyModification modification, Transform target, string axis)
		{
			EditorCurveBinding editorCurveBinding = baseBinding;
			PropertyModification propertyModification = modification.previousValue;
			editorCurveBinding.propertyName = editorCurveBinding.propertyName.Remove(editorCurveBinding.propertyName.Length - 1, 1) + axis;
			if (propertyModification == null)
			{
				propertyModification = new PropertyModification();
				propertyModification.target = target;
				propertyModification.propertyPath = editorCurveBinding.propertyName;
				object currentValue = CurveBindingUtility.GetCurrentValue(state.activeRootGameObject, editorCurveBinding);
				propertyModification.value = ((float)currentValue).ToString();
			}
			state.AddPropertyModification(editorCurveBinding, propertyModification, modification.keepPrefabOverride);
			AnimationRecording.AddKey(state, editorCurveBinding, typeof(float), propertyModification);
		}

		public static void ProcessVector3Modifications(IAnimationRecordingState state, ref Dictionary<object, AnimationRecording.Vector3Modification> vector3Modifications)
		{
			AnimationClip activeAnimationClip = state.activeAnimationClip;
			GameObject activeRootGameObject = state.activeRootGameObject;
			foreach (KeyValuePair<object, AnimationRecording.Vector3Modification> current in vector3Modifications)
			{
				AnimationRecording.Vector3Modification value = current.Value;
				Transform transform = current.Key as Transform;
				if (!(transform == null))
				{
					EditorCurveBinding baseBinding = default(EditorCurveBinding);
					if (AnimationUtility.PropertyModificationToEditorCurveBinding(value.last.currentValue, state.activeRootGameObject, out baseBinding) != null)
					{
						AnimationRecording.ProcessVector3Modification(state, baseBinding, value.x, transform, "x");
						AnimationRecording.ProcessVector3Modification(state, baseBinding, value.y, transform, "y");
						AnimationRecording.ProcessVector3Modification(state, baseBinding, value.z, transform, "z");
					}
				}
			}
		}

		public static void ProcessModifications(IAnimationRecordingState state, UndoPropertyModification[] modifications)
		{
			AnimationClip activeAnimationClip = state.activeAnimationClip;
			GameObject activeRootGameObject = state.activeRootGameObject;
			for (int i = 0; i < modifications.Length; i++)
			{
				EditorCurveBinding binding = default(EditorCurveBinding);
				PropertyModification previousValue = modifications[i].previousValue;
				Type type = AnimationUtility.PropertyModificationToEditorCurveBinding(previousValue, activeRootGameObject, out binding);
				if (type != null)
				{
					state.AddPropertyModification(binding, previousValue, modifications[i].keepPrefabOverride);
					AnimationRecording.AddKey(state, binding, type, previousValue);
				}
			}
		}

		public static UndoPropertyModification[] Process(IAnimationRecordingState state, UndoPropertyModification[] modifications)
		{
			GameObject activeRootGameObject = state.activeRootGameObject;
			UndoPropertyModification[] result;
			if (activeRootGameObject == null)
			{
				result = modifications;
			}
			else if (!AnimationRecording.HasAnyRecordableModifications(activeRootGameObject, modifications))
			{
				result = modifications;
			}
			else
			{
				Dictionary<object, AnimationRecording.RotationModification> dictionary = new Dictionary<object, AnimationRecording.RotationModification>();
				Dictionary<object, AnimationRecording.Vector3Modification> dictionary2 = new Dictionary<object, AnimationRecording.Vector3Modification>();
				Dictionary<object, AnimationRecording.Vector3Modification> dictionary3 = new Dictionary<object, AnimationRecording.Vector3Modification>();
				AnimationRecording.CollectRotationModifications(state, ref modifications, ref dictionary);
				UndoPropertyModification[] second = AnimationRecording.FilterRotationModifications(state, ref dictionary);
				UndoPropertyModification[] array = AnimationRecording.FilterModifications(state, ref modifications);
				AnimationRecording.CollectVector3Modifications(state, ref modifications, ref dictionary3, "m_LocalPosition");
				AnimationRecording.CollectVector3Modifications(state, ref modifications, ref dictionary2, "m_LocalScale");
				AnimationRecording.ProcessAnimatorModifications(state, ref dictionary3, ref dictionary, ref dictionary2);
				AnimationRecording.ProcessVector3Modifications(state, ref dictionary3);
				AnimationRecording.ProcessRotationModifications(state, ref dictionary);
				AnimationRecording.ProcessVector3Modifications(state, ref dictionary2);
				AnimationRecording.ProcessModifications(state, modifications);
				array.Concat(second);
				result = array.ToArray<UndoPropertyModification>();
			}
			return result;
		}

		private static bool ValueFromPropertyModification(PropertyModification modification, EditorCurveBinding binding, out object outObject)
		{
			bool result;
			float num;
			if (modification == null)
			{
				outObject = null;
				result = false;
			}
			else if (binding.isPPtrCurve)
			{
				outObject = modification.objectReference;
				result = true;
			}
			else if (float.TryParse(modification.value, out num))
			{
				outObject = num;
				result = true;
			}
			else
			{
				outObject = null;
				result = false;
			}
			return result;
		}

		private static void AddKey(IAnimationRecordingState state, EditorCurveBinding binding, Type type, PropertyModification modification)
		{
			GameObject activeRootGameObject = state.activeRootGameObject;
			AnimationClip activeAnimationClip = state.activeAnimationClip;
			if ((activeAnimationClip.hideFlags & HideFlags.NotEditable) == HideFlags.None)
			{
				AnimationWindowCurve animationWindowCurve = new AnimationWindowCurve(activeAnimationClip, binding, type);
				object currentValue = CurveBindingUtility.GetCurrentValue(activeRootGameObject, binding);
				if (state.addZeroFrame)
				{
					if (animationWindowCurve.length == 0)
					{
						object value = null;
						if (!AnimationRecording.ValueFromPropertyModification(modification, binding, out value))
						{
							value = currentValue;
						}
						if (state.currentFrame != 0)
						{
							AnimationWindowUtility.AddKeyframeToCurve(animationWindowCurve, value, type, AnimationKeyTime.Frame(0, activeAnimationClip.frameRate));
						}
					}
				}
				AnimationWindowUtility.AddKeyframeToCurve(animationWindowCurve, currentValue, type, AnimationKeyTime.Frame(state.currentFrame, activeAnimationClip.frameRate));
				state.SaveCurve(animationWindowCurve);
			}
		}

		private static void AddRotationKey(IAnimationRecordingState state, EditorCurveBinding binding, Type type, Vector3 previousEulerAngles, Vector3 currentEulerAngles)
		{
			AnimationClip activeAnimationClip = state.activeAnimationClip;
			if ((activeAnimationClip.hideFlags & HideFlags.NotEditable) == HideFlags.None)
			{
				EditorCurveBinding[] array = RotationCurveInterpolation.RemapAnimationBindingForRotationAddKey(binding, activeAnimationClip);
				for (int i = 0; i < 3; i++)
				{
					AnimationWindowCurve animationWindowCurve = new AnimationWindowCurve(activeAnimationClip, array[i], type);
					if (state.addZeroFrame)
					{
						if (animationWindowCurve.length == 0)
						{
							if (state.currentFrame != 0)
							{
								AnimationWindowUtility.AddKeyframeToCurve(animationWindowCurve, previousEulerAngles[i], type, AnimationKeyTime.Frame(0, activeAnimationClip.frameRate));
							}
						}
					}
					AnimationWindowUtility.AddKeyframeToCurve(animationWindowCurve, currentEulerAngles[i], type, AnimationKeyTime.Frame(state.currentFrame, activeAnimationClip.frameRate));
					state.SaveCurve(animationWindowCurve);
				}
			}
		}

		private static void ProcessRootMotionModifications(IAnimationRecordingState state, ref Dictionary<object, AnimationRecording.RootMotionModification> rootMotionModifications)
		{
			AnimationClip activeAnimationClip = state.activeAnimationClip;
			GameObject activeRootGameObject = state.activeRootGameObject;
			Animator component = activeRootGameObject.GetComponent<Animator>();
			bool flag = component != null && component.isHuman;
			foreach (KeyValuePair<object, AnimationRecording.RootMotionModification> current in rootMotionModifications)
			{
				AnimationRecording.RootMotionModification value = current.Value;
				Transform transform = current.Key as Transform;
				Vector3 vector = transform.localScale * ((!flag) ? 1f : component.humanScale);
				Vector3 localPosition = transform.localPosition;
				Quaternion localRotation = transform.localRotation;
				if (value.lastP.previousValue != null)
				{
					AnimationRecording.ProcessAnimatorModification(state, component, value.px, "MotionT.x", localPosition.x, vector.x);
					AnimationRecording.ProcessAnimatorModification(state, component, value.py, "MotionT.y", localPosition.y, vector.y);
					AnimationRecording.ProcessAnimatorModification(state, component, value.pz, "MotionT.z", localPosition.z, vector.z);
				}
				if (value.lastR.previousValue != null)
				{
					AnimationRecording.ProcessAnimatorModification(state, component, value.rx, "MotionQ.x", localRotation.x, 1f);
					AnimationRecording.ProcessAnimatorModification(state, component, value.ry, "MotionQ.y", localRotation.y, 1f);
					AnimationRecording.ProcessAnimatorModification(state, component, value.rz, "MotionQ.z", localRotation.z, 1f);
					AnimationRecording.ProcessAnimatorModification(state, component, value.rw, "MotionQ.w", localRotation.w, 1f);
				}
			}
		}

		private static void ProcessAnimatorModifications(IAnimationRecordingState state, ref Dictionary<object, AnimationRecording.Vector3Modification> positionModifications, ref Dictionary<object, AnimationRecording.RotationModification> rotationModifications, ref Dictionary<object, AnimationRecording.Vector3Modification> scaleModifications)
		{
			Dictionary<object, AnimationRecording.RootMotionModification> dictionary = new Dictionary<object, AnimationRecording.RootMotionModification>();
			AnimationClip activeAnimationClip = state.activeAnimationClip;
			GameObject activeRootGameObject = state.activeRootGameObject;
			Animator component = activeRootGameObject.GetComponent<Animator>();
			bool flag = component != null && component.isHuman;
			bool flag2 = component != null && component.hasRootMotion;
			bool flag3 = component != null && component.applyRootMotion;
			List<object> list = new List<object>();
			foreach (KeyValuePair<object, AnimationRecording.Vector3Modification> current in positionModifications)
			{
				AnimationRecording.Vector3Modification value = current.Value;
				Transform transform = current.Key as Transform;
				if (!(transform == null))
				{
					EditorCurveBinding editorCurveBinding = default(EditorCurveBinding);
					if (AnimationUtility.PropertyModificationToEditorCurveBinding(value.last.currentValue, state.activeRootGameObject, out editorCurveBinding) != null)
					{
						bool flag4 = activeRootGameObject.transform == transform;
						bool flag5 = (flag || flag2 || flag3) && flag4;
						bool flag6 = flag && !flag4 && component.IsBoneTransform(transform);
						if (flag6)
						{
							Debug.LogWarning("Keyframing translation on humanoid rig is not supported!", transform);
							list.Add(current.Key);
						}
						else if (flag5)
						{
							AnimationRecording.RootMotionModification rootMotionModification;
							if (!dictionary.TryGetValue(transform, out rootMotionModification))
							{
								rootMotionModification = new AnimationRecording.RootMotionModification();
								dictionary[transform] = rootMotionModification;
							}
							rootMotionModification.lastP = value.last;
							rootMotionModification.px = value.x;
							rootMotionModification.py = value.y;
							rootMotionModification.pz = value.z;
							list.Add(current.Key);
						}
					}
				}
			}
			foreach (object current2 in list)
			{
				positionModifications.Remove(current2);
			}
			List<object> list2 = new List<object>();
			foreach (KeyValuePair<object, AnimationRecording.RotationModification> current3 in rotationModifications)
			{
				AnimationRecording.RotationModification value2 = current3.Value;
				Transform transform2 = current3.Key as Transform;
				if (!(transform2 == null))
				{
					EditorCurveBinding editorCurveBinding2 = default(EditorCurveBinding);
					if (AnimationUtility.PropertyModificationToEditorCurveBinding(value2.lastQuatModification.currentValue, state.activeRootGameObject, out editorCurveBinding2) != null)
					{
						bool flag7 = activeRootGameObject.transform == transform2;
						bool flag8 = (flag || flag2 || flag3) && flag7;
						bool flag9 = flag && !flag7 && component.IsBoneTransform(transform2);
						if (flag9)
						{
							Debug.LogWarning("Keyframing rotation on humanoid rig is not supported!", transform2);
							list2.Add(current3.Key);
						}
						else if (flag8)
						{
							AnimationRecording.RootMotionModification rootMotionModification2;
							if (!dictionary.TryGetValue(transform2, out rootMotionModification2))
							{
								rootMotionModification2 = new AnimationRecording.RootMotionModification();
								dictionary[transform2] = rootMotionModification2;
							}
							rootMotionModification2.lastR = value2.lastQuatModification;
							rootMotionModification2.rx = value2.x;
							rootMotionModification2.ry = value2.y;
							rootMotionModification2.rz = value2.z;
							rootMotionModification2.rw = value2.w;
							list2.Add(current3.Key);
						}
					}
				}
			}
			foreach (object current4 in list2)
			{
				rotationModifications.Remove(current4);
			}
			List<object> list3 = new List<object>();
			foreach (KeyValuePair<object, AnimationRecording.Vector3Modification> current5 in scaleModifications)
			{
				AnimationRecording.Vector3Modification value3 = current5.Value;
				Transform transform3 = current5.Key as Transform;
				if (!(transform3 == null))
				{
					EditorCurveBinding editorCurveBinding3 = default(EditorCurveBinding);
					if (AnimationUtility.PropertyModificationToEditorCurveBinding(value3.last.currentValue, state.activeRootGameObject, out editorCurveBinding3) != null)
					{
						bool flag10 = activeRootGameObject.transform == transform3;
						bool flag11 = flag && !flag10 && component.IsBoneTransform(transform3);
						if (flag11)
						{
							Debug.LogWarning("Keyframing scale on humanoid rig is not supported!", transform3);
							list3.Add(current5.Key);
						}
					}
				}
			}
			foreach (object current6 in list3)
			{
				scaleModifications.Remove(current6);
			}
			AnimationRecording.ProcessRootMotionModifications(state, ref dictionary);
		}

		private static void ProcessAnimatorModification(IAnimationRecordingState state, Animator animator, UndoPropertyModification modification, string name, float value, float scale)
		{
			AnimationClip activeAnimationClip = state.activeAnimationClip;
			if ((activeAnimationClip.hideFlags & HideFlags.NotEditable) == HideFlags.None)
			{
				float num = value;
				object obj;
				if (AnimationRecording.ValueFromPropertyModification(modification.currentValue, default(EditorCurveBinding), out obj))
				{
					value = (float)obj;
				}
				if (AnimationRecording.ValueFromPropertyModification(modification.previousValue, default(EditorCurveBinding), out obj))
				{
					num = (float)obj;
				}
				value = ((Mathf.Abs(scale) <= Mathf.Epsilon) ? value : (value / scale));
				num = ((Mathf.Abs(scale) <= Mathf.Epsilon) ? num : (num / scale));
				EditorCurveBinding binding = default(EditorCurveBinding);
				binding.propertyName = name;
				binding.path = "";
				binding.type = typeof(Animator);
				PropertyModification propertyModification = new PropertyModification();
				propertyModification.target = animator;
				propertyModification.propertyPath = binding.propertyName;
				propertyModification.value = value.ToString();
				state.AddPropertyModification(binding, propertyModification, modification.keepPrefabOverride);
				AnimationWindowCurve animationWindowCurve = new AnimationWindowCurve(activeAnimationClip, binding, typeof(float));
				if (state.addZeroFrame && state.currentFrame != 0 && animationWindowCurve.length == 0)
				{
					AnimationWindowUtility.AddKeyframeToCurve(animationWindowCurve, num, typeof(float), AnimationKeyTime.Frame(0, activeAnimationClip.frameRate));
				}
				AnimationWindowUtility.AddKeyframeToCurve(animationWindowCurve, value, typeof(float), AnimationKeyTime.Frame(state.currentFrame, activeAnimationClip.frameRate));
				state.SaveCurve(animationWindowCurve);
			}
		}

		public static void SaveModifiedCurve(AnimationWindowCurve curve, AnimationClip clip)
		{
			curve.m_Keyframes.Sort((AnimationWindowKeyframe a, AnimationWindowKeyframe b) => a.time.CompareTo(b.time));
			if (curve.isPPtrCurve)
			{
				ObjectReferenceKeyframe[] array = curve.ToObjectCurve();
				if (array.Length == 0)
				{
					array = null;
				}
				AnimationUtility.SetObjectReferenceCurve(clip, curve.binding, array);
			}
			else
			{
				AnimationCurve animationCurve = curve.ToAnimationCurve();
				if (animationCurve.keys.Length == 0)
				{
					animationCurve = null;
				}
				else
				{
					QuaternionCurveTangentCalculation.UpdateTangentsFromMode(animationCurve, clip, curve.binding);
				}
				AnimationUtility.SetEditorCurve(clip, curve.binding, animationCurve);
			}
		}
	}
}
