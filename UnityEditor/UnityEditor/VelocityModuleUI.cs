using System;
using UnityEngine;

namespace UnityEditor
{
	internal class VelocityModuleUI : ModuleUI
	{
		private class Texts
		{
			public GUIContent linearX = EditorGUIUtility.TextContent("Linear  X|Apply linear velocity to particles.");

			public GUIContent orbitalX = EditorGUIUtility.TextContent("Orbital X|Apply orbital velocity to particles, which will rotate them around the center of the system.");

			public GUIContent orbitalOffsetX = EditorGUIUtility.TextContent("Offset  X|Apply an offset to the center of rotation.");

			public GUIContent y = EditorGUIUtility.TextContent("Y");

			public GUIContent z = EditorGUIUtility.TextContent("Z");

			public GUIContent space = EditorGUIUtility.TrTextContent("Space", "Specifies if the velocity values are in local space (rotated with the transform) or world space.", null);

			public GUIContent speedMultiplier = EditorGUIUtility.TrTextContent("Speed Modifier", "Multiply the particle speed by this value", null);

			public GUIContent radial = EditorGUIUtility.TrTextContent("Radial", "Apply radial velocity to particles, which will project them out from the center of the system.", null);

			public GUIContent linearSpace = EditorGUIUtility.TrTextContent("Space", "Specifies if the velocity values are in local space (rotated with the transform) or world space.", null);

			public string[] spaces = new string[]
			{
				"Local",
				"World"
			};
		}

		private SerializedMinMaxCurve m_X;

		private SerializedMinMaxCurve m_Y;

		private SerializedMinMaxCurve m_Z;

		private SerializedMinMaxCurve m_OrbitalX;

		private SerializedMinMaxCurve m_OrbitalY;

		private SerializedMinMaxCurve m_OrbitalZ;

		private SerializedMinMaxCurve m_OrbitalOffsetX;

		private SerializedMinMaxCurve m_OrbitalOffsetY;

		private SerializedMinMaxCurve m_OrbitalOffsetZ;

		private SerializedMinMaxCurve m_Radial;

		private SerializedProperty m_InWorldSpace;

		private SerializedMinMaxCurve m_SpeedModifier;

		private static VelocityModuleUI.Texts s_Texts;

		public VelocityModuleUI(ParticleSystemUI owner, SerializedObject o, string displayName) : base(owner, o, "VelocityModule", displayName)
		{
			this.m_ToolTip = "Controls the velocity of each particle during its lifetime.";
		}

		protected override void Init()
		{
			if (this.m_X == null)
			{
				if (VelocityModuleUI.s_Texts == null)
				{
					VelocityModuleUI.s_Texts = new VelocityModuleUI.Texts();
				}
				this.m_X = new SerializedMinMaxCurve(this, VelocityModuleUI.s_Texts.linearX, "x", ModuleUI.kUseSignedRange);
				this.m_Y = new SerializedMinMaxCurve(this, VelocityModuleUI.s_Texts.y, "y", ModuleUI.kUseSignedRange);
				this.m_Z = new SerializedMinMaxCurve(this, VelocityModuleUI.s_Texts.z, "z", ModuleUI.kUseSignedRange);
				this.m_OrbitalX = new SerializedMinMaxCurve(this, VelocityModuleUI.s_Texts.orbitalX, "orbitalX", ModuleUI.kUseSignedRange);
				this.m_OrbitalY = new SerializedMinMaxCurve(this, VelocityModuleUI.s_Texts.y, "orbitalY", ModuleUI.kUseSignedRange);
				this.m_OrbitalZ = new SerializedMinMaxCurve(this, VelocityModuleUI.s_Texts.z, "orbitalZ", ModuleUI.kUseSignedRange);
				this.m_OrbitalOffsetX = new SerializedMinMaxCurve(this, VelocityModuleUI.s_Texts.orbitalOffsetX, "orbitalOffsetX", ModuleUI.kUseSignedRange);
				this.m_OrbitalOffsetY = new SerializedMinMaxCurve(this, VelocityModuleUI.s_Texts.y, "orbitalOffsetY", ModuleUI.kUseSignedRange);
				this.m_OrbitalOffsetZ = new SerializedMinMaxCurve(this, VelocityModuleUI.s_Texts.z, "orbitalOffsetZ", ModuleUI.kUseSignedRange);
				this.m_Radial = new SerializedMinMaxCurve(this, VelocityModuleUI.s_Texts.radial, "radial", ModuleUI.kUseSignedRange);
				this.m_InWorldSpace = base.GetProperty("inWorldSpace");
				this.m_SpeedModifier = new SerializedMinMaxCurve(this, VelocityModuleUI.s_Texts.speedMultiplier, "speedModifier", ModuleUI.kUseSignedRange);
			}
		}

		public override void OnInspectorGUI(InitialModuleUI initial)
		{
			base.GUITripleMinMaxCurve(GUIContent.none, VelocityModuleUI.s_Texts.linearX, this.m_X, VelocityModuleUI.s_Texts.y, this.m_Y, VelocityModuleUI.s_Texts.z, this.m_Z, null, new GUILayoutOption[0]);
			EditorGUI.indentLevel++;
			ModuleUI.GUIBoolAsPopup(VelocityModuleUI.s_Texts.linearSpace, this.m_InWorldSpace, VelocityModuleUI.s_Texts.spaces, new GUILayoutOption[0]);
			EditorGUI.indentLevel--;
			base.GUITripleMinMaxCurve(GUIContent.none, VelocityModuleUI.s_Texts.orbitalX, this.m_OrbitalX, VelocityModuleUI.s_Texts.y, this.m_OrbitalY, VelocityModuleUI.s_Texts.z, this.m_OrbitalZ, null, new GUILayoutOption[0]);
			base.GUITripleMinMaxCurve(GUIContent.none, VelocityModuleUI.s_Texts.orbitalOffsetX, this.m_OrbitalOffsetX, VelocityModuleUI.s_Texts.y, this.m_OrbitalOffsetY, VelocityModuleUI.s_Texts.z, this.m_OrbitalOffsetZ, null, new GUILayoutOption[0]);
			EditorGUI.indentLevel++;
			ModuleUI.GUIMinMaxCurve(VelocityModuleUI.s_Texts.radial, this.m_Radial, new GUILayoutOption[0]);
			EditorGUI.indentLevel--;
			ModuleUI.GUIMinMaxCurve(VelocityModuleUI.s_Texts.speedMultiplier, this.m_SpeedModifier, new GUILayoutOption[0]);
		}

		public override void UpdateCullingSupportedString(ref string text)
		{
			this.Init();
			string empty = string.Empty;
			if (!this.m_X.SupportsProcedural(ref empty))
			{
				text = text + "\nVelocity over Lifetime module curve X: " + empty;
			}
			empty = string.Empty;
			if (!this.m_Y.SupportsProcedural(ref empty))
			{
				text = text + "\nVelocity over Lifetime module curve Y: " + empty;
			}
			empty = string.Empty;
			if (!this.m_Z.SupportsProcedural(ref empty))
			{
				text = text + "\nVelocity over Lifetime module curve Z: " + empty;
			}
			empty = string.Empty;
			if (this.m_SpeedModifier.state != MinMaxCurveState.k_Scalar || this.m_SpeedModifier.maxConstant != 1f)
			{
				text += "\nVelocity over Lifetime module curve Speed Multiplier is being used";
			}
			empty = string.Empty;
			if (this.m_OrbitalX.maxConstant != 0f || this.m_OrbitalY.maxConstant != 0f || this.m_OrbitalZ.maxConstant != 0f)
			{
				text += "\nVelocity over Lifetime module orbital velocity is being used";
			}
			empty = string.Empty;
			if (this.m_Radial.maxConstant != 0f)
			{
				text += "\nVelocity over Lifetime module radial velocity is being used";
			}
		}
	}
}
