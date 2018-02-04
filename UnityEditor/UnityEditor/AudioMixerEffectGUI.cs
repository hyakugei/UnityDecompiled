using System;
using System.Runtime.CompilerServices;
using UnityEditor.Audio;
using UnityEngine;

namespace UnityEditor
{
	internal static class AudioMixerEffectGUI
	{
		private class ExposedParamContext
		{
			public AudioMixerController controller;

			public AudioParameterPath path;

			public ExposedParamContext(AudioMixerController controller, AudioParameterPath path)
			{
				this.controller = controller;
				this.path = path;
			}
		}

		private class ParameterTransitionOverrideContext
		{
			public AudioMixerController controller;

			public GUID parameter;

			public ParameterTransitionType type;

			public ParameterTransitionOverrideContext(AudioMixerController controller, GUID parameter, ParameterTransitionType type)
			{
				this.controller = controller;
				this.parameter = parameter;
				this.type = type;
			}
		}

		private class ParameterTransitionOverrideRemoveContext
		{
			public AudioMixerController controller;

			public GUID parameter;

			public ParameterTransitionOverrideRemoveContext(AudioMixerController controller, GUID parameter)
			{
				this.controller = controller;
				this.parameter = parameter;
			}
		}

		private const string kAudioSliderFloatFormat = "F2";

		private const string kExposedParameterUnicodeChar = " ➔";

		/*
		[CompilerGenerated]
		private static GenericMenu.MenuFunction2 <>f__mg$cache0;

		[CompilerGenerated]
		private static GenericMenu.MenuFunction2 <>f__mg$cache1;

		[CompilerGenerated]
		private static GenericMenu.MenuFunction2 <>f__mg$cache2;

		[CompilerGenerated]
		private static GenericMenu.MenuFunction2 <>f__mg$cache3;

		[CompilerGenerated]
		private static GenericMenu.MenuFunction2 <>f__mg$cache4;

		[CompilerGenerated]
		private static GenericMenu.MenuFunction2 <>f__mg$cache5;

		[CompilerGenerated]
		private static GenericMenu.MenuFunction2 <>f__mg$cache6;

		[CompilerGenerated]
		private static GenericMenu.MenuFunction2 <>f__mg$cache7;
		*/

		private static AudioMixerDrawUtils.Styles styles
		{
			get
			{
				return AudioMixerDrawUtils.styles;
			}
		}

		public static void EffectHeader(string text)
		{
			GUILayout.Label(text, AudioMixerEffectGUI.styles.headerStyle, new GUILayoutOption[0]);
		}

		public static bool Slider(GUIContent label, ref float value, float displayScale, float displayExponent, string unit, float leftValue, float rightValue, AudioMixerController controller, AudioParameterPath path, params GUILayoutOption[] options)
		{
			EditorGUI.BeginChangeCheck();
			float fieldWidth = EditorGUIUtility.fieldWidth;
			string kFloatFieldFormatString = EditorGUI.kFloatFieldFormatString;
			bool flag = controller.ContainsExposedParameter(path.parameter);
			EditorGUIUtility.fieldWidth = 70f;
			EditorGUI.kFloatFieldFormatString = "F2";
			EditorGUI.s_UnitString = unit;
			GUIContent label2 = label;
			if (flag)
			{
				label2 = GUIContent.Temp(label.text + " ➔", label.tooltip);
			}
			float num = value * displayScale;
			num = EditorGUILayout.PowerSlider(label2, num, leftValue * displayScale, rightValue * displayScale, displayExponent, options);
			EditorGUI.s_UnitString = null;
			EditorGUI.kFloatFieldFormatString = kFloatFieldFormatString;
			EditorGUIUtility.fieldWidth = fieldWidth;
			if (Event.current.type == EventType.ContextClick)
			{
				if (GUILayoutUtility.topLevel.GetLast().Contains(Event.current.mousePosition))
				{
					Event.current.Use();
					GenericMenu genericMenu = new GenericMenu();
					if (!flag)
					{
						GenericMenu arg_120_0 = genericMenu;
						GUIContent arg_120_1 = EditorGUIUtility.TrTextContent("Expose '" + path.ResolveStringPath(false) + "' to script", null, null);
						bool arg_120_2 = false;
						/*
						if (AudioMixerEffectGUI.<>f__mg$cache0 == null)
						{
							AudioMixerEffectGUI.<>f__mg$cache0 = new GenericMenu.MenuFunction2(AudioMixerEffectGUI.ExposePopupCallback);
						}
						arg_120_0.AddItem(arg_120_1, arg_120_2, AudioMixerEffectGUI.<>f__mg$cache0, new AudioMixerEffectGUI.ExposedParamContext(controller, path));
						*/
					}
					else
					{
						GenericMenu arg_15F_0 = genericMenu;
						GUIContent arg_15F_1 = EditorGUIUtility.TrTextContent("Unexpose", null, null);
						bool arg_15F_2 = false;
						/*
						if (AudioMixerEffectGUI.<>f__mg$cache1 == null)
						{
							AudioMixerEffectGUI.<>f__mg$cache1 = new GenericMenu.MenuFunction2(AudioMixerEffectGUI.UnexposePopupCallback);
						}
						arg_15F_0.AddItem(arg_15F_1, arg_15F_2, AudioMixerEffectGUI.<>f__mg$cache1, new AudioMixerEffectGUI.ExposedParamContext(controller, path));
						*/
					}
					ParameterTransitionType parameterTransitionType;
					bool transitionTypeOverride = controller.TargetSnapshot.GetTransitionTypeOverride(path.parameter, out parameterTransitionType);
					genericMenu.AddSeparator(string.Empty);
					GenericMenu arg_1C6_0 = genericMenu;
					GUIContent arg_1C6_1 = EditorGUIUtility.TrTextContent("Linear Snapshot Transition", null, null);
					bool arg_1C6_2 = parameterTransitionType == ParameterTransitionType.Lerp;
					/*
					if (AudioMixerEffectGUI.<>f__mg$cache2 == null)
					{
						AudioMixerEffectGUI.<>f__mg$cache2 = new GenericMenu.MenuFunction2(AudioMixerEffectGUI.ParameterTransitionOverrideCallback);
					}
					arg_1C6_0.AddItem(arg_1C6_1, arg_1C6_2, AudioMixerEffectGUI.<>f__mg$cache2, new AudioMixerEffectGUI.ParameterTransitionOverrideContext(controller, path.parameter, ParameterTransitionType.Lerp));
					*/
					GenericMenu arg_20A_0 = genericMenu;
					GUIContent arg_20A_1 = EditorGUIUtility.TrTextContent("Smoothstep Snapshot Transition", null, null);
					bool arg_20A_2 = parameterTransitionType == ParameterTransitionType.Smoothstep;
					/*
					if (AudioMixerEffectGUI.<>f__mg$cache3 == null)
					{
						AudioMixerEffectGUI.<>f__mg$cache3 = new GenericMenu.MenuFunction2(AudioMixerEffectGUI.ParameterTransitionOverrideCallback);
					}
					arg_20A_0.AddItem(arg_20A_1, arg_20A_2, AudioMixerEffectGUI.<>f__mg$cache3, new AudioMixerEffectGUI.ParameterTransitionOverrideContext(controller, path.parameter, ParameterTransitionType.Smoothstep));
					*/
					GenericMenu arg_24E_0 = genericMenu;
					GUIContent arg_24E_1 = EditorGUIUtility.TrTextContent("Squared Snapshot Transition", null, null);
					bool arg_24E_2 = parameterTransitionType == ParameterTransitionType.Squared;
					/*
					if (AudioMixerEffectGUI.<>f__mg$cache4 == null)
					{
						AudioMixerEffectGUI.<>f__mg$cache4 = new GenericMenu.MenuFunction2(AudioMixerEffectGUI.ParameterTransitionOverrideCallback);
					}
					arg_24E_0.AddItem(arg_24E_1, arg_24E_2, AudioMixerEffectGUI.<>f__mg$cache4, new AudioMixerEffectGUI.ParameterTransitionOverrideContext(controller, path.parameter, ParameterTransitionType.Squared));
					*/
					GenericMenu arg_292_0 = genericMenu;
					GUIContent arg_292_1 = EditorGUIUtility.TrTextContent("SquareRoot Snapshot Transition", null, null);
					bool arg_292_2 = parameterTransitionType == ParameterTransitionType.SquareRoot;
					/*
					if (AudioMixerEffectGUI.<>f__mg$cache5 == null)
					{
						AudioMixerEffectGUI.<>f__mg$cache5 = new GenericMenu.MenuFunction2(AudioMixerEffectGUI.ParameterTransitionOverrideCallback);
					}
					arg_292_0.AddItem(arg_292_1, arg_292_2, AudioMixerEffectGUI.<>f__mg$cache5, new AudioMixerEffectGUI.ParameterTransitionOverrideContext(controller, path.parameter, ParameterTransitionType.SquareRoot));
					*/
					GenericMenu arg_2D6_0 = genericMenu;
					GUIContent arg_2D6_1 = EditorGUIUtility.TrTextContent("BrickwallStart Snapshot Transition", null, null);
					bool arg_2D6_2 = parameterTransitionType == ParameterTransitionType.BrickwallStart;
					/*
					if (AudioMixerEffectGUI.<>f__mg$cache6 == null)
					{
						AudioMixerEffectGUI.<>f__mg$cache6 = new GenericMenu.MenuFunction2(AudioMixerEffectGUI.ParameterTransitionOverrideCallback);
					}
					arg_2D6_0.AddItem(arg_2D6_1, arg_2D6_2, AudioMixerEffectGUI.<>f__mg$cache6, new AudioMixerEffectGUI.ParameterTransitionOverrideContext(controller, path.parameter, ParameterTransitionType.BrickwallStart));
					*/
					GenericMenu arg_31A_0 = genericMenu;
					GUIContent arg_31A_1 = EditorGUIUtility.TrTextContent("BrickwallEnd Snapshot Transition", null, null);
					bool arg_31A_2 = parameterTransitionType == ParameterTransitionType.BrickwallEnd;
					/*
					if (AudioMixerEffectGUI.<>f__mg$cache7 == null)
					{
						AudioMixerEffectGUI.<>f__mg$cache7 = new GenericMenu.MenuFunction2(AudioMixerEffectGUI.ParameterTransitionOverrideCallback);
					}
					arg_31A_0.AddItem(arg_31A_1, arg_31A_2, AudioMixerEffectGUI.<>f__mg$cache7, new AudioMixerEffectGUI.ParameterTransitionOverrideContext(controller, path.parameter, ParameterTransitionType.BrickwallEnd));
					*/
					genericMenu.AddSeparator(string.Empty);
					genericMenu.ShowAsContext();
				}
			}
			bool result;
			if (EditorGUI.EndChangeCheck())
			{
				value = num / displayScale;
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		public static void ExposePopupCallback(object obj)
		{
			AudioMixerEffectGUI.ExposedParamContext exposedParamContext = (AudioMixerEffectGUI.ExposedParamContext)obj;
			Undo.RecordObject(exposedParamContext.controller, "Expose Mixer Parameter");
			exposedParamContext.controller.AddExposedParameter(exposedParamContext.path);
			AudioMixerUtility.RepaintAudioMixerAndInspectors();
		}

		public static void UnexposePopupCallback(object obj)
		{
			AudioMixerEffectGUI.ExposedParamContext exposedParamContext = (AudioMixerEffectGUI.ExposedParamContext)obj;
			Undo.RecordObject(exposedParamContext.controller, "Unexpose Mixer Parameter");
			exposedParamContext.controller.RemoveExposedParameter(exposedParamContext.path.parameter);
			AudioMixerUtility.RepaintAudioMixerAndInspectors();
		}

		public static void ParameterTransitionOverrideCallback(object obj)
		{
			AudioMixerEffectGUI.ParameterTransitionOverrideContext parameterTransitionOverrideContext = (AudioMixerEffectGUI.ParameterTransitionOverrideContext)obj;
			Undo.RecordObject(parameterTransitionOverrideContext.controller, "Change Parameter Transition Type");
			if (parameterTransitionOverrideContext.type == ParameterTransitionType.Lerp)
			{
				parameterTransitionOverrideContext.controller.TargetSnapshot.ClearTransitionTypeOverride(parameterTransitionOverrideContext.parameter);
			}
			else
			{
				parameterTransitionOverrideContext.controller.TargetSnapshot.SetTransitionTypeOverride(parameterTransitionOverrideContext.parameter, parameterTransitionOverrideContext.type);
			}
		}

		public static bool PopupButton(GUIContent label, GUIContent buttonContent, GUIStyle style, out Rect buttonRect, params GUILayoutOption[] options)
		{
			if (label != null)
			{
				Rect rect = EditorGUILayout.s_LastRect = EditorGUILayout.GetControlRect(true, 16f, style, options);
				int controlID = GUIUtility.GetControlID("EditorPopup".GetHashCode(), FocusType.Keyboard, rect);
				buttonRect = EditorGUI.PrefixLabel(rect, controlID, label);
			}
			else
			{
				Rect rect2 = GUILayoutUtility.GetRect(buttonContent, style, options);
				buttonRect = rect2;
			}
			return EditorGUI.DropdownButton(buttonRect, buttonContent, FocusType.Passive, style);
		}
	}
}
