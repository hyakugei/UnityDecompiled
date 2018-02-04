using System;
using UnityEngine;

namespace UnityEditor
{
	internal class AnimationWindowStyles
	{
		public static Texture2D pointIcon = EditorGUIUtility.LoadIcon("animationkeyframe");

		public static GUIContent playContent = EditorGUIUtility.TrIconContent("Animation.Play", "Play the animation clip.");

		public static GUIContent recordContent = EditorGUIUtility.TrIconContent("Animation.Record", "Enable/disable keyframe recording mode.");

		public static GUIContent previewContent = EditorGUIUtility.TrTextContent("Preview", "Enable/disable scene preview mode.", null);

		public static GUIContent prevKeyContent = EditorGUIUtility.TrIconContent("Animation.PrevKey", "Go to previous keyframe.");

		public static GUIContent nextKeyContent = EditorGUIUtility.TrIconContent("Animation.NextKey", "Go to next keyframe.");

		public static GUIContent firstKeyContent = EditorGUIUtility.TrIconContent("Animation.FirstKey", "Go to the beginning of the animation clip.");

		public static GUIContent lastKeyContent = EditorGUIUtility.TrIconContent("Animation.LastKey", "Go to the end of the animation clip.");

		public static GUIContent addKeyframeContent = EditorGUIUtility.TrIconContent("Animation.AddKeyframe", "Add keyframe.");

		public static GUIContent addEventContent = EditorGUIUtility.TrIconContent("Animation.AddEvent", "Add event.");

		public static GUIContent sequencerLinkContent = EditorGUIUtility.TrIconContent("Animation.SequencerLink", "Animation Window is linked to Sequence Editor.  Press to Unlink.");

		public static GUIContent noAnimatableObjectSelectedText = EditorGUIUtility.TrTextContent("No animatable object selected.", null, null);

		public static GUIContent formatIsMissing = EditorGUIUtility.TrTextContent("To begin animating {0}, create {1}.", null, null);

		public static GUIContent animatorAndAnimationClip = EditorGUIUtility.TrTextContent("an Animator and an Animation Clip", null, null);

		public static GUIContent animationClip = EditorGUIUtility.TrTextContent("an Animation Clip", null, null);

		public static GUIContent create = EditorGUIUtility.TrTextContent("Create", null, null);

		public static GUIContent dopesheet = EditorGUIUtility.TrTextContent("Dopesheet", null, null);

		public static GUIContent curves = EditorGUIUtility.TrTextContent("Curves", null, null);

		public static GUIContent samples = EditorGUIUtility.TrTextContent("Samples", null, null);

		public static GUIContent createNewClip = EditorGUIUtility.TrTextContent("Create New Clip...", null, null);

		public static GUIContent animatorOptimizedText = EditorGUIUtility.TrTextContent("Editing and playback of animations on optimized game object hierarchy is not supported.\nPlease select a game object that does not have 'Optimize Game Objects' applied.", null, null);

		public static GUIStyle playHead = "AnimationPlayHead";

		public static GUIStyle curveEditorBackground = "CurveEditorBackground";

		public static GUIStyle curveEditorLabelTickmarks = "CurveEditorLabelTickmarks";

		public static GUIStyle eventBackground = "AnimationEventBackground";

		public static GUIStyle eventTooltip = "AnimationEventTooltip";

		public static GUIStyle eventTooltipArrow = "AnimationEventTooltipArrow";

		public static GUIStyle keyframeBackground = "AnimationKeyframeBackground";

		public static GUIStyle timelineTick = "AnimationTimelineTick";

		public static GUIStyle dopeSheetKeyframe = "Dopesheetkeyframe";

		public static GUIStyle dopeSheetBackground = "DopesheetBackground";

		public static GUIStyle popupCurveDropdown = "PopupCurveDropdown";

		public static GUIStyle popupCurveEditorBackground = "PopupCurveEditorBackground";

		public static GUIStyle popupCurveEditorSwatch = "PopupCurveEditorSwatch";

		public static GUIStyle popupCurveSwatchBackground = "PopupCurveSwatchBackground";

		public static GUIStyle miniToolbar = new GUIStyle(EditorStyles.toolbar);

		public static GUIStyle miniToolbarButton = new GUIStyle(EditorStyles.toolbarButton);

		public static GUIStyle toolbarLabel = new GUIStyle(EditorStyles.toolbarPopup);

		public static void Initialize()
		{
			AnimationWindowStyles.toolbarLabel.normal.background = null;
			AnimationWindowStyles.miniToolbarButton.padding.top = 0;
			AnimationWindowStyles.miniToolbarButton.padding.bottom = 3;
		}
	}
}
