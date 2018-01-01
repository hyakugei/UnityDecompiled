using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Bindings;
using UnityEngine.Internal;
using UnityEngine.Scripting;

namespace UnityEditor
{
	public sealed class AnimationUtility
	{
		public enum CurveModifiedType
		{
			CurveDeleted,
			CurveModified,
			ClipModified
		}

		public enum TangentMode
		{
			Free,
			Auto,
			Linear,
			Constant,
			ClampedAuto
		}

		internal enum PolynomialValid
		{
			Valid,
			InvalidPreWrapMode,
			InvalidPostWrapMode,
			TooManySegments
		}

		public delegate void OnCurveWasModified(AnimationClip clip, EditorCurveBinding binding, AnimationUtility.CurveModifiedType type);

		public static AnimationUtility.OnCurveWasModified onCurveWasModified;

		public static bool GetObjectReferenceValue(GameObject root, EditorCurveBinding binding, out UnityEngine.Object targetObject)
		{
			return AnimationUtility.INTERNAL_CALL_GetObjectReferenceValue(root, ref binding, out targetObject);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool INTERNAL_CALL_GetObjectReferenceValue(GameObject root, ref EditorCurveBinding binding, out UnityEngine.Object targetObject);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern Type PropertyModificationToEditorCurveBinding(PropertyModification modification, GameObject gameObject, out EditorCurveBinding binding);

		[RequiredByNativeCode]
		private static void Internal_CallOnCurveWasModified(AnimationClip clip, EditorCurveBinding binding, AnimationUtility.CurveModifiedType type)
		{
			if (AnimationUtility.onCurveWasModified != null)
			{
				AnimationUtility.onCurveWasModified(clip, binding, type);
			}
		}

		[RequiredByNativeCode]
		private static void Internal_CallAnimationClipAwake(AnimationClip clip)
		{
			if (AnimationUtility.onCurveWasModified != null)
			{
				AnimationUtility.onCurveWasModified(clip, default(EditorCurveBinding), AnimationUtility.CurveModifiedType.ClipModified);
			}
		}

		[Obsolete("GetAnimationClips(Animation) is deprecated. Use GetAnimationClips(GameObject) instead.")]
		public static AnimationClip[] GetAnimationClips(Animation component)
		{
			return AnimationUtility.GetAnimationClips(component.gameObject);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern AnimationClip[] GetAnimationClips([NotNull] GameObject gameObject);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetAnimationClips([NotNull] Animation animation, AnimationClip[] clips);

		public static EditorCurveBinding[] GetAnimatableBindings(GameObject targetObject, GameObject root)
		{
			return AnimationUtility.Internal_GetGameObjectAnimatableBindings(targetObject, root);
		}

		internal static EditorCurveBinding[] GetAnimatableBindings(ScriptableObject scriptableObject)
		{
			return AnimationUtility.Internal_GetScriptableObjectAnimatableBindings(scriptableObject);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern EditorCurveBinding[] Internal_GetGameObjectAnimatableBindings([NotNull] GameObject targetObject, [NotNull] GameObject root);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern EditorCurveBinding[] Internal_GetScriptableObjectAnimatableBindings([NotNull] ScriptableObject scriptableObject);

		public static Type GetEditorCurveValueType(GameObject root, EditorCurveBinding binding)
		{
			return AnimationUtility.Internal_GetGameObjectEditorCurveValueType(root, binding);
		}

		internal static Type GetEditorCurveValueType(ScriptableObject scriptableObject, EditorCurveBinding binding)
		{
			return AnimationUtility.Internal_GetScriptableObjectEditorCurveValueType(scriptableObject, binding);
		}

		private static Type Internal_GetGameObjectEditorCurveValueType([NotNull] GameObject root, EditorCurveBinding binding)
		{
			return AnimationUtility.Internal_GetGameObjectEditorCurveValueType_Injected(root, ref binding);
		}

		private static Type Internal_GetScriptableObjectEditorCurveValueType([NotNull] ScriptableObject scriptableObject, EditorCurveBinding binding)
		{
			return AnimationUtility.Internal_GetScriptableObjectEditorCurveValueType_Injected(scriptableObject, ref binding);
		}

		public static bool GetFloatValue([NotNull] GameObject root, EditorCurveBinding binding, out float data)
		{
			return AnimationUtility.GetFloatValue_Injected(root, ref binding, out data);
		}

		public static UnityEngine.Object GetAnimatedObject([NotNull] GameObject root, EditorCurveBinding binding)
		{
			return AnimationUtility.GetAnimatedObject_Injected(root, ref binding);
		}

		internal static PropertyModification EditorCurveBindingToPropertyModification(EditorCurveBinding binding, [NotNull] GameObject gameObject)
		{
			return AnimationUtility.EditorCurveBindingToPropertyModification_Injected(ref binding, gameObject);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern EditorCurveBinding[] GetCurveBindings([NotNull] AnimationClip clip);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern EditorCurveBinding[] GetObjectReferenceCurveBindings([NotNull] AnimationClip clip);

		public static ObjectReferenceKeyframe[] GetObjectReferenceCurve([NotNull] AnimationClip clip, EditorCurveBinding binding)
		{
			return AnimationUtility.GetObjectReferenceCurve_Injected(clip, ref binding);
		}

		public static void SetObjectReferenceCurve([NotNull] AnimationClip clip, EditorCurveBinding binding, ObjectReferenceKeyframe[] keyframes)
		{
			AnimationUtility.SetObjectReferenceCurve_Injected(clip, ref binding, keyframes);
		}

		public static AnimationCurve GetEditorCurve([NotNull] AnimationClip clip, EditorCurveBinding binding)
		{
			return AnimationUtility.GetEditorCurve_Injected(clip, ref binding);
		}

		public static void SetEditorCurve([NotNull] AnimationClip clip, EditorCurveBinding binding, AnimationCurve curve)
		{
			AnimationUtility.SetEditorCurve_Injected(clip, ref binding, curve);
		}

		internal static void SetEditorCurves(AnimationClip clip, EditorCurveBinding[] bindings, AnimationCurve[] curves)
		{
			if (clip == null)
			{
				throw new ArgumentNullException("clip");
			}
			if (curves == null)
			{
				throw new ArgumentNullException("curves");
			}
			if (bindings == null)
			{
				throw new ArgumentNullException("bindings");
			}
			if (bindings.Length != curves.Length)
			{
				throw new ArgumentException("bindings and curves array sizes do not match");
			}
			for (int i = 0; i < bindings.Length; i++)
			{
				AnimationUtility.Internal_SetEditorCurve(clip, bindings[i], curves[i], false);
				if (AnimationUtility.onCurveWasModified != null)
				{
					AnimationUtility.onCurveWasModified(clip, bindings[i], (curves[i] == null) ? AnimationUtility.CurveModifiedType.CurveDeleted : AnimationUtility.CurveModifiedType.CurveModified);
				}
			}
			AnimationUtility.Internal_SyncEditorCurves(clip);
		}

		private static void Internal_SetEditorCurve(AnimationClip clip, EditorCurveBinding binding, AnimationCurve curve, bool syncEditorCurve)
		{
			AnimationUtility.Internal_SetEditorCurve_Injected(clip, ref binding, curve, syncEditorCurve);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_SyncEditorCurves(AnimationClip clip);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void UpdateTangentsFromModeSurrounding([NotNull] AnimationCurve curve, int index);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void UpdateTangentsFromMode([NotNull] AnimationCurve curve);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern AnimationUtility.TangentMode GetKeyLeftTangentMode([NotNull] AnimationCurve curve, int index);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern AnimationUtility.TangentMode GetKeyRightTangentMode([NotNull] AnimationCurve curve, int index);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool GetKeyBroken([NotNull] AnimationCurve curve, int index);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetKeyLeftTangentMode([NotNull] AnimationCurve curve, int index, AnimationUtility.TangentMode tangentMode);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetKeyRightTangentMode([NotNull] AnimationCurve curve, int index, AnimationUtility.TangentMode tangentMode);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetKeyBroken([NotNull] AnimationCurve curve, int index, bool broken);

		internal static AnimationUtility.TangentMode GetKeyLeftTangentMode(Keyframe key)
		{
			return AnimationUtility.Internal_GetKeyLeftTangentMode(key);
		}

		internal static AnimationUtility.TangentMode GetKeyRightTangentMode(Keyframe key)
		{
			return AnimationUtility.Internal_GetKeyRightTangentMode(key);
		}

		internal static bool GetKeyBroken(Keyframe key)
		{
			return AnimationUtility.Internal_GetKeyBroken(key);
		}

		private static AnimationUtility.TangentMode Internal_GetKeyLeftTangentMode(Keyframe key)
		{
			return AnimationUtility.Internal_GetKeyLeftTangentMode_Injected(ref key);
		}

		private static AnimationUtility.TangentMode Internal_GetKeyRightTangentMode(Keyframe key)
		{
			return AnimationUtility.Internal_GetKeyRightTangentMode_Injected(ref key);
		}

		private static bool Internal_GetKeyBroken(Keyframe key)
		{
			return AnimationUtility.Internal_GetKeyBroken_Injected(ref key);
		}

		internal static void SetKeyLeftTangentMode(ref Keyframe key, AnimationUtility.TangentMode tangentMode)
		{
			AnimationUtility.Internal_SetKeyLeftTangentMode(ref key, tangentMode);
		}

		internal static void SetKeyRightTangentMode(ref Keyframe key, AnimationUtility.TangentMode tangentMode)
		{
			AnimationUtility.Internal_SetKeyRightTangentMode(ref key, tangentMode);
		}

		internal static void SetKeyBroken(ref Keyframe key, bool broken)
		{
			AnimationUtility.Internal_SetKeyBroken(ref key, broken);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_SetKeyLeftTangentMode(ref Keyframe key, AnimationUtility.TangentMode tangentMode);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_SetKeyRightTangentMode(ref Keyframe key, AnimationUtility.TangentMode tangentMode);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_SetKeyBroken(ref Keyframe key, bool broken);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int AddInbetweenKey(AnimationCurve curve, float time);

		[Obsolete("GetAllCurves is deprecated. Use GetCurveBindings and GetObjectReferenceCurveBindings instead.")]
		public static AnimationClipCurveData[] GetAllCurves(AnimationClip clip)
		{
			bool includeCurveData = true;
			return AnimationUtility.GetAllCurves(clip, includeCurveData);
		}

		[Obsolete("GetAllCurves is deprecated. Use GetCurveBindings and GetObjectReferenceCurveBindings instead.")]
		public static AnimationClipCurveData[] GetAllCurves(AnimationClip clip, [DefaultValue("true")] bool includeCurveData)
		{
			EditorCurveBinding[] curveBindings = AnimationUtility.GetCurveBindings(clip);
			AnimationClipCurveData[] array = new AnimationClipCurveData[curveBindings.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = new AnimationClipCurveData(curveBindings[i]);
				if (includeCurveData)
				{
					array[i].curve = AnimationUtility.GetEditorCurve(clip, curveBindings[i]);
				}
			}
			return array;
		}

		[Obsolete("This overload is deprecated. Use the one with EditorCurveBinding instead.")]
		public static bool GetFloatValue(GameObject root, string relativePath, Type type, string propertyName, out float data)
		{
			return AnimationUtility.GetFloatValue(root, EditorCurveBinding.FloatCurve(relativePath, type, propertyName), out data);
		}

		[Obsolete("This overload is deprecated. Use the one with EditorCurveBinding instead.")]
		public static void SetEditorCurve(AnimationClip clip, string relativePath, Type type, string propertyName, AnimationCurve curve)
		{
			AnimationUtility.SetEditorCurve(clip, EditorCurveBinding.FloatCurve(relativePath, type, propertyName), curve);
		}

		[Obsolete("This overload is deprecated. Use the one with EditorCurveBinding instead.")]
		public static AnimationCurve GetEditorCurve(AnimationClip clip, string relativePath, Type type, string propertyName)
		{
			return AnimationUtility.GetEditorCurve(clip, EditorCurveBinding.FloatCurve(relativePath, type, propertyName));
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern AnimationEvent[] GetAnimationEvents([NotNull] AnimationClip clip);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetAnimationEvents([NotNull] AnimationClip clip, [NotNull] AnimationEvent[] events);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern string CalculateTransformPath([NotNull] Transform targetTransform, Transform root);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern AnimationClipSettings GetAnimationClipSettings([NotNull] AnimationClip clip);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetAnimationClipSettings([NotNull] AnimationClip clip, AnimationClipSettings srcClipInfo);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void SetAnimationClipSettingsNoDirty([NotNull] AnimationClip clip, AnimationClipSettings srcClipInfo);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetAdditiveReferencePose(AnimationClip clip, AnimationClip referenceClip, float time);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool IsValidOptimizedPolynomialCurve(AnimationCurve curve);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void ConstrainToPolynomialCurve(AnimationCurve curve);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int GetMaxNumPolynomialSegmentsSupported();

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern AnimationUtility.PolynomialValid IsValidPolynomialCurve(AnimationCurve curve);

		internal static AnimationClipStats GetAnimationClipStats(AnimationClip clip)
		{
			AnimationClipStats result;
			AnimationUtility.GetAnimationClipStats_Injected(clip, out result);
			return result;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool GetGenerateMotionCurves(AnimationClip clip);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetGenerateMotionCurves(AnimationClip clip, bool value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool HasGenericRootTransform(AnimationClip clip);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool HasMotionFloatCurves(AnimationClip clip);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool HasMotionCurves(AnimationClip clip);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool HasRootCurves(AnimationClip clip);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool AmbiguousBinding(string path, int classID, Transform root);

		internal static Vector3 GetClosestEuler(Quaternion q, Vector3 eulerHint, RotationOrder rotationOrder)
		{
			Vector3 result;
			AnimationUtility.GetClosestEuler_Injected(ref q, ref eulerHint, rotationOrder, out result);
			return result;
		}

		[Obsolete("Use AnimationMode.InAnimationMode instead.")]
		public static bool InAnimationMode()
		{
			return AnimationMode.InAnimationMode();
		}

		[Obsolete("Use AnimationMode.StartAnimationmode instead.")]
		public static void StartAnimationMode(UnityEngine.Object[] objects)
		{
			Debug.LogWarning("AnimationUtility.StartAnimationMode is deprecated. Use AnimationMode.StartAnimationMode with the new APIs. The objects passed to this function will no longer be reverted automatically. See AnimationMode.AddPropertyModification");
			AnimationMode.StartAnimationMode();
		}

		[Obsolete("Use AnimationMode.StopAnimationMode instead.")]
		public static void StopAnimationMode()
		{
			AnimationMode.StopAnimationMode();
		}

		[Obsolete("SetAnimationType is no longer supported.")]
		public static void SetAnimationType(AnimationClip clip, ModelImporterAnimationType type)
		{
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Type Internal_GetGameObjectEditorCurveValueType_Injected(GameObject root, ref EditorCurveBinding binding);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Type Internal_GetScriptableObjectEditorCurveValueType_Injected(ScriptableObject scriptableObject, ref EditorCurveBinding binding);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetFloatValue_Injected(GameObject root, ref EditorCurveBinding binding, out float data);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern UnityEngine.Object GetAnimatedObject_Injected(GameObject root, ref EditorCurveBinding binding);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern PropertyModification EditorCurveBindingToPropertyModification_Injected(ref EditorCurveBinding binding, GameObject gameObject);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern ObjectReferenceKeyframe[] GetObjectReferenceCurve_Injected(AnimationClip clip, ref EditorCurveBinding binding);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetObjectReferenceCurve_Injected(AnimationClip clip, ref EditorCurveBinding binding, ObjectReferenceKeyframe[] keyframes);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern AnimationCurve GetEditorCurve_Injected(AnimationClip clip, ref EditorCurveBinding binding);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetEditorCurve_Injected(AnimationClip clip, ref EditorCurveBinding binding, AnimationCurve curve);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_SetEditorCurve_Injected(AnimationClip clip, ref EditorCurveBinding binding, AnimationCurve curve, bool syncEditorCurve);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern AnimationUtility.TangentMode Internal_GetKeyLeftTangentMode_Injected(ref Keyframe key);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern AnimationUtility.TangentMode Internal_GetKeyRightTangentMode_Injected(ref Keyframe key);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool Internal_GetKeyBroken_Injected(ref Keyframe key);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetAnimationClipStats_Injected(AnimationClip clip, out AnimationClipStats ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetClosestEuler_Injected(ref Quaternion q, ref Vector3 eulerHint, RotationOrder rotationOrder, out Vector3 ret);
	}
}
