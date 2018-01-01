using System;
using UnityEngine;

namespace UnityEditor
{
	internal class ParticleSystemStyles
	{
		private GUIStyle m_Label;

		private GUIStyle m_LabelBold;

		private GUIStyle m_EditableLabel;

		private GUIStyle m_EditableLabelBold;

		private GUIStyle m_ObjectField;

		private GUIStyle m_ObjectFieldBold;

		private GUIStyle m_NumberField;

		private GUIStyle m_NumberFieldBold;

		private GUIStyle m_ModuleHeaderStyle;

		private GUIStyle m_ModuleHeaderStyleBold;

		private GUIStyle m_PopupStyle;

		private GUIStyle m_PopupStyleBold;

		private GUIStyle m_EmitterHeaderStyle;

		private GUIStyle m_EffectBgStyle;

		private GUIStyle m_ModuleBgStyle;

		private GUIStyle m_Plus;

		private GUIStyle m_Minus;

		private GUIStyle m_Checkmark;

		private GUIStyle m_CheckmarkMixed;

		private GUIStyle m_MinMaxCurveStateDropDown;

		private GUIStyle m_Toggle;

		private GUIStyle m_ToggleMixed;

		private GUIStyle m_SelectionMarker;

		private GUIStyle m_ToolbarButtonLeftAlignText;

		private GUIStyle m_ModulePadding;

		private Texture2D m_WarningIcon;

		private static ParticleSystemStyles s_ParticleSystemStyles;

		public GUIStyle label
		{
			get
			{
				return (!EditorGUIUtility.GetBoldDefaultFont()) ? this.m_Label : this.m_LabelBold;
			}
		}

		public GUIStyle editableLabel
		{
			get
			{
				return (!EditorGUIUtility.GetBoldDefaultFont()) ? this.m_EditableLabel : this.m_EditableLabelBold;
			}
		}

		public GUIStyle objectField
		{
			get
			{
				return (!EditorGUIUtility.GetBoldDefaultFont()) ? this.m_ObjectField : this.m_ObjectFieldBold;
			}
		}

		public GUIStyle numberField
		{
			get
			{
				return (!EditorGUIUtility.GetBoldDefaultFont()) ? this.m_NumberField : this.m_NumberFieldBold;
			}
		}

		public GUIStyle moduleHeaderStyle
		{
			get
			{
				return (!EditorGUIUtility.GetBoldDefaultFont()) ? this.m_ModuleHeaderStyle : this.m_ModuleHeaderStyleBold;
			}
		}

		public GUIStyle popup
		{
			get
			{
				return (!EditorGUIUtility.GetBoldDefaultFont()) ? this.m_PopupStyle : this.m_PopupStyleBold;
			}
		}

		public GUIStyle emitterHeaderStyle
		{
			get
			{
				return this.m_EmitterHeaderStyle;
			}
		}

		public GUIStyle effectBgStyle
		{
			get
			{
				return this.m_EffectBgStyle;
			}
		}

		public GUIStyle moduleBgStyle
		{
			get
			{
				return this.m_ModuleBgStyle;
			}
		}

		public GUIStyle plus
		{
			get
			{
				return this.m_Plus;
			}
		}

		public GUIStyle minus
		{
			get
			{
				return this.m_Minus;
			}
		}

		public GUIStyle checkmark
		{
			get
			{
				return this.m_Checkmark;
			}
		}

		public GUIStyle checkmarkMixed
		{
			get
			{
				return this.m_CheckmarkMixed;
			}
		}

		public GUIStyle minMaxCurveStateDropDown
		{
			get
			{
				return this.m_MinMaxCurveStateDropDown;
			}
		}

		public GUIStyle toggle
		{
			get
			{
				return this.m_Toggle;
			}
		}

		public GUIStyle toggleMixed
		{
			get
			{
				return this.m_ToggleMixed;
			}
		}

		public GUIStyle selectionMarker
		{
			get
			{
				return this.m_SelectionMarker;
			}
		}

		public GUIStyle toolbarButtonLeftAlignText
		{
			get
			{
				return this.m_ToolbarButtonLeftAlignText;
			}
		}

		public GUIStyle modulePadding
		{
			get
			{
				return this.m_ModulePadding;
			}
		}

		public Texture2D warningIcon
		{
			get
			{
				return this.m_WarningIcon;
			}
		}

		private ParticleSystemStyles()
		{
			ParticleSystemStyles.InitStyle(out this.m_Label, out this.m_LabelBold, "ShurikenLabel");
			ParticleSystemStyles.InitStyle(out this.m_EditableLabel, out this.m_EditableLabelBold, "ShurikenEditableLabel");
			ParticleSystemStyles.InitStyle(out this.m_ObjectField, out this.m_ObjectFieldBold, "ShurikenObjectField");
			ParticleSystemStyles.InitStyle(out this.m_NumberField, out this.m_NumberFieldBold, "ShurikenValue");
			ParticleSystemStyles.InitStyle(out this.m_ModuleHeaderStyle, out this.m_ModuleHeaderStyleBold, "ShurikenModuleTitle");
			ParticleSystemStyles.InitStyle(out this.m_PopupStyle, out this.m_PopupStyleBold, "ShurikenPopUp");
			ParticleSystemStyles.InitStyle(out this.m_EmitterHeaderStyle, "ShurikenEmitterTitle");
			ParticleSystemStyles.InitStyle(out this.m_EmitterHeaderStyle, "ShurikenEmitterTitle");
			ParticleSystemStyles.InitStyle(out this.m_EffectBgStyle, "ShurikenEffectBg");
			ParticleSystemStyles.InitStyle(out this.m_ModuleBgStyle, "ShurikenModuleBg");
			ParticleSystemStyles.InitStyle(out this.m_Plus, "ShurikenPlus");
			ParticleSystemStyles.InitStyle(out this.m_Minus, "ShurikenMinus");
			ParticleSystemStyles.InitStyle(out this.m_Checkmark, "ShurikenCheckMark");
			ParticleSystemStyles.InitStyle(out this.m_CheckmarkMixed, "ShurikenCheckMarkMixed");
			ParticleSystemStyles.InitStyle(out this.m_MinMaxCurveStateDropDown, "ShurikenDropdown");
			ParticleSystemStyles.InitStyle(out this.m_Toggle, "ShurikenToggle");
			ParticleSystemStyles.InitStyle(out this.m_ToggleMixed, "ShurikenToggleMixed");
			ParticleSystemStyles.InitStyle(out this.m_SelectionMarker, "IN ThumbnailShadow");
			ParticleSystemStyles.InitStyle(out this.m_ToolbarButtonLeftAlignText, "ToolbarButton");
			this.m_EmitterHeaderStyle.clipping = TextClipping.Clip;
			this.m_EmitterHeaderStyle.padding.right = 45;
			this.m_WarningIcon = EditorGUIUtility.LoadIcon("console.infoicon.sml");
			this.m_ToolbarButtonLeftAlignText = new GUIStyle(this.m_ToolbarButtonLeftAlignText);
			this.m_ToolbarButtonLeftAlignText.alignment = TextAnchor.MiddleLeft;
			this.m_ModulePadding = new GUIStyle();
			this.m_ModulePadding.padding = new RectOffset(3, 3, 4, 2);
		}

		public static ParticleSystemStyles Get()
		{
			if (ParticleSystemStyles.s_ParticleSystemStyles == null)
			{
				ParticleSystemStyles.s_ParticleSystemStyles = new ParticleSystemStyles();
			}
			return ParticleSystemStyles.s_ParticleSystemStyles;
		}

		private static void InitStyle(out GUIStyle normal, string name)
		{
			normal = ParticleSystemStyles.FindStyle(name);
		}

		private static void InitStyle(out GUIStyle normal, out GUIStyle bold, string name)
		{
			ParticleSystemStyles.InitStyle(out normal, name);
			bold = new GUIStyle(normal);
			bold.font = EditorStyles.miniBoldFont;
		}

		private static GUIStyle FindStyle(string styleName)
		{
			return styleName;
		}
	}
}
