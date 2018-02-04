using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityEditor
{
	public class AnimationMode
	{
		private static bool s_InAnimationPlaybackMode = false;

		private static bool s_InAnimationRecordMode = false;

		private static PrefColor s_AnimatedPropertyColor = new PrefColor("Animation/Property Animated", 0.82f, 0.97f, 1f, 1f, 0.54f, 0.85f, 1f, 1f);

		private static PrefColor s_RecordedPropertyColor = new PrefColor("Animation/Property Recorded", 1f, 0.6f, 0.6f, 1f, 1f, 0.5f, 0.5f, 1f);

		private static PrefColor s_CandidatePropertyColor = new PrefColor("Animation/Property Candidate", 1f, 0.7f, 0.6f, 1f, 1f, 0.67f, 0.43f, 1f);

		private static AnimationModeDriver s_DummyDriver;

		public static Color animatedPropertyColor
		{
			get
			{
				return AnimationMode.s_AnimatedPropertyColor;
			}
		}

		public static Color recordedPropertyColor
		{
			get
			{
				return AnimationMode.s_RecordedPropertyColor;
			}
		}

		public static Color candidatePropertyColor
		{
			get
			{
				return AnimationMode.s_CandidatePropertyColor;
			}
		}

		private static AnimationModeDriver DummyDriver()
		{
			if (AnimationMode.s_DummyDriver == null)
			{
				AnimationMode.s_DummyDriver = ScriptableObject.CreateInstance<AnimationModeDriver>();
				AnimationMode.s_DummyDriver.name = "DummyDriver";
			}
			return AnimationMode.s_DummyDriver;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool IsPropertyAnimated(UnityEngine.Object target, string propertyPath);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool IsPropertyCandidate(UnityEngine.Object target, string propertyPath);

		public static void StopAnimationMode()
		{
			AnimationMode.StopAnimationMode(AnimationMode.DummyDriver());
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void StopAnimationMode(UnityEngine.Object driver);

		public static bool InAnimationMode()
		{
			return AnimationMode.Internal_InAnimationModeNoDriver();
		}

		internal static bool InAnimationMode(UnityEngine.Object driver)
		{
			return AnimationMode.Internal_InAnimationMode(driver);
		}

		public static void StartAnimationMode()
		{
			AnimationMode.StartAnimationMode(AnimationMode.DummyDriver());
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void StartAnimationMode(UnityEngine.Object driver);

		internal static void StopAnimationPlaybackMode()
		{
			AnimationMode.s_InAnimationPlaybackMode = false;
		}

		internal static bool InAnimationPlaybackMode()
		{
			return AnimationMode.s_InAnimationPlaybackMode;
		}

		internal static void StartAnimationPlaybackMode()
		{
			AnimationMode.s_InAnimationPlaybackMode = true;
		}

		internal static void StopAnimationRecording()
		{
			AnimationMode.s_InAnimationRecordMode = false;
		}

		internal static bool InAnimationRecording()
		{
			return AnimationMode.s_InAnimationRecordMode;
		}

		internal static void StartAnimationRecording()
		{
			AnimationMode.s_InAnimationRecordMode = true;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void StartCandidateRecording(UnityEngine.Object driver);

		internal static void AddCandidate(EditorCurveBinding binding, PropertyModification modification, bool keepPrefabOverride)
		{
			AnimationMode.AddCandidate_Injected(ref binding, modification, keepPrefabOverride);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void StopCandidateRecording();

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool IsRecordingCandidates();

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void BeginSampling();

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void EndSampling();

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SampleAnimationClip(GameObject gameObject, AnimationClip clip, float time);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void SampleCandidateClip(GameObject gameObject, AnimationClip clip, float time);

		public static void AddPropertyModification(EditorCurveBinding binding, PropertyModification modification, bool keepPrefabOverride)
		{
			AnimationMode.AddPropertyModification_Injected(ref binding, modification, keepPrefabOverride);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void InitializePropertyModificationForGameObject(GameObject gameObject, AnimationClip clip);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void InitializePropertyModificationForObject(UnityEngine.Object target, AnimationClip clip);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void RevertPropertyModificationsForGameObject(GameObject gameObject);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void RevertPropertyModificationsForObject(UnityEngine.Object target);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool Internal_InAnimationMode(UnityEngine.Object driver);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool Internal_InAnimationModeNoDriver();

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void AddCandidate_Injected(ref EditorCurveBinding binding, PropertyModification modification, bool keepPrefabOverride);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void AddPropertyModification_Injected(ref EditorCurveBinding binding, PropertyModification modification, bool keepPrefabOverride);
	}
}
