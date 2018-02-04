using System;
using UnityEngine;

namespace UnityEditor
{
	internal class TrailModuleUI : ModuleUI
	{
		private class Texts
		{
			public GUIContent mode = EditorGUIUtility.TrTextContent("Mode", "Select how trails are generated on the particles.", null);

			public GUIContent ratio = EditorGUIUtility.TrTextContent("Ratio", "Choose what proportion of particles will receive a trail.", null);

			public GUIContent lifetime = EditorGUIUtility.TrTextContent("Lifetime", "How long each trail will last, relative to the life of the particle.", null);

			public GUIContent minVertexDistance = EditorGUIUtility.TrTextContent("Minimum Vertex Distance", "The minimum distance each trail can travel before adding a new vertex.", null);

			public GUIContent textureMode = EditorGUIUtility.TrTextContent("Texture Mode", "Should the U coordinate be stretched or tiled?", null);

			public GUIContent worldSpace = EditorGUIUtility.TrTextContent("World Space", "Trail points will be dropped in world space, even if the particle system is simulating in local space.", null);

			public GUIContent dieWithParticles = EditorGUIUtility.TrTextContent("Die with Particles", "The trails will disappear when their owning particles die.", null);

			public GUIContent sizeAffectsWidth = EditorGUIUtility.TrTextContent("Size affects Width", "The trails will use the particle size to control their width.", null);

			public GUIContent sizeAffectsLifetime = EditorGUIUtility.TrTextContent("Size affects Lifetime", "The trails will use the particle size to control their lifetime.", null);

			public GUIContent inheritParticleColor = EditorGUIUtility.TrTextContent("Inherit Particle Color", "The trails will use the particle color as their base color.", null);

			public GUIContent colorOverLifetime = EditorGUIUtility.TrTextContent("Color over Lifetime", "The color of the trails during the lifetime of the particle they are attached to.", null);

			public GUIContent widthOverTrail = EditorGUIUtility.TrTextContent("Width over Trail", "Select a width for the trail from its start to end vertex.", null);

			public GUIContent colorOverTrail = EditorGUIUtility.TrTextContent("Color over Trail", "Select a color for the trail from its start to end vertex.", null);

			public GUIContent generateLightingData = EditorGUIUtility.TrTextContent("Generate Lighting Data", "Toggle generation of normal and tangent data, for use in lit shaders.", null);

			public GUIContent ribbonCount = EditorGUIUtility.TrTextContent("Ribbon Count", "Select how many ribbons to render throughout the Particle System.", null);

			public GUIContent splitSubEmitterRibbons = EditorGUIUtility.TrTextContent("Split Sub Emitter Ribbons", "When used on a sub emitter, ribbons will connect particles from each parent particle independently.", null);

			public GUIContent[] trailModeOptions = new GUIContent[]
			{
				EditorGUIUtility.TrTextContent("Particles", null, null),
				EditorGUIUtility.TrTextContent("Ribbon", null, null)
			};

			public GUIContent[] textureModeOptions = new GUIContent[]
			{
				EditorGUIUtility.TrTextContent("Stretch", null, null),
				EditorGUIUtility.TrTextContent("Tile", null, null),
				EditorGUIUtility.TrTextContent("DistributePerSegment", null, null),
				EditorGUIUtility.TrTextContent("RepeatPerSegment", null, null)
			};
		}

		private static TrailModuleUI.Texts s_Texts;

		private SerializedProperty m_Mode;

		private SerializedProperty m_Ratio;

		private SerializedMinMaxCurve m_Lifetime;

		private SerializedProperty m_MinVertexDistance;

		private SerializedProperty m_TextureMode;

		private SerializedProperty m_WorldSpace;

		private SerializedProperty m_DieWithParticles;

		private SerializedProperty m_SizeAffectsWidth;

		private SerializedProperty m_SizeAffectsLifetime;

		private SerializedProperty m_InheritParticleColor;

		private SerializedMinMaxGradient m_ColorOverLifetime;

		private SerializedMinMaxCurve m_WidthOverTrail;

		private SerializedMinMaxGradient m_ColorOverTrail;

		private SerializedProperty m_GenerateLightingData;

		private SerializedProperty m_RibbonCount;

		private SerializedProperty m_SplitSubEmitterRibbons;

		public TrailModuleUI(ParticleSystemUI owner, SerializedObject o, string displayName) : base(owner, o, "TrailModule", displayName)
		{
			this.m_ToolTip = "Attach trails to the particles.";
		}

		protected override void Init()
		{
			if (this.m_Ratio == null)
			{
				if (TrailModuleUI.s_Texts == null)
				{
					TrailModuleUI.s_Texts = new TrailModuleUI.Texts();
				}
				this.m_Mode = base.GetProperty("mode");
				this.m_Ratio = base.GetProperty("ratio");
				this.m_Lifetime = new SerializedMinMaxCurve(this, TrailModuleUI.s_Texts.lifetime, "lifetime");
				this.m_MinVertexDistance = base.GetProperty("minVertexDistance");
				this.m_TextureMode = base.GetProperty("textureMode");
				this.m_WorldSpace = base.GetProperty("worldSpace");
				this.m_DieWithParticles = base.GetProperty("dieWithParticles");
				this.m_SizeAffectsWidth = base.GetProperty("sizeAffectsWidth");
				this.m_SizeAffectsLifetime = base.GetProperty("sizeAffectsLifetime");
				this.m_InheritParticleColor = base.GetProperty("inheritParticleColor");
				this.m_ColorOverLifetime = new SerializedMinMaxGradient(this, "colorOverLifetime");
				this.m_WidthOverTrail = new SerializedMinMaxCurve(this, TrailModuleUI.s_Texts.widthOverTrail, "widthOverTrail");
				this.m_ColorOverTrail = new SerializedMinMaxGradient(this, "colorOverTrail");
				this.m_GenerateLightingData = base.GetProperty("generateLightingData");
				this.m_RibbonCount = base.GetProperty("ribbonCount");
				this.m_SplitSubEmitterRibbons = base.GetProperty("splitSubEmitterRibbons");
			}
		}

		public override void OnInspectorGUI(InitialModuleUI initial)
		{
			ParticleSystemTrailMode particleSystemTrailMode = (ParticleSystemTrailMode)ModuleUI.GUIPopup(TrailModuleUI.s_Texts.mode, this.m_Mode, TrailModuleUI.s_Texts.trailModeOptions, new GUILayoutOption[0]);
			if (!this.m_Mode.hasMultipleDifferentValues)
			{
				if (particleSystemTrailMode == ParticleSystemTrailMode.PerParticle)
				{
					ModuleUI.GUIFloat(TrailModuleUI.s_Texts.ratio, this.m_Ratio, new GUILayoutOption[0]);
					ModuleUI.GUIMinMaxCurve(TrailModuleUI.s_Texts.lifetime, this.m_Lifetime, new GUILayoutOption[0]);
					ModuleUI.GUIFloat(TrailModuleUI.s_Texts.minVertexDistance, this.m_MinVertexDistance, new GUILayoutOption[0]);
					ModuleUI.GUIToggle(TrailModuleUI.s_Texts.worldSpace, this.m_WorldSpace, new GUILayoutOption[0]);
					ModuleUI.GUIToggle(TrailModuleUI.s_Texts.dieWithParticles, this.m_DieWithParticles, new GUILayoutOption[0]);
				}
				else
				{
					ModuleUI.GUIInt(TrailModuleUI.s_Texts.ribbonCount, this.m_RibbonCount, new GUILayoutOption[0]);
					ModuleUI.GUIToggle(TrailModuleUI.s_Texts.splitSubEmitterRibbons, this.m_SplitSubEmitterRibbons, new GUILayoutOption[0]);
				}
			}
			ModuleUI.GUIPopup(TrailModuleUI.s_Texts.textureMode, this.m_TextureMode, TrailModuleUI.s_Texts.textureModeOptions, new GUILayoutOption[0]);
			ModuleUI.GUIToggle(TrailModuleUI.s_Texts.sizeAffectsWidth, this.m_SizeAffectsWidth, new GUILayoutOption[0]);
			if (!this.m_Mode.hasMultipleDifferentValues)
			{
				if (particleSystemTrailMode == ParticleSystemTrailMode.PerParticle)
				{
					ModuleUI.GUIToggle(TrailModuleUI.s_Texts.sizeAffectsLifetime, this.m_SizeAffectsLifetime, new GUILayoutOption[0]);
				}
			}
			ModuleUI.GUIToggle(TrailModuleUI.s_Texts.inheritParticleColor, this.m_InheritParticleColor, new GUILayoutOption[0]);
			base.GUIMinMaxGradient(TrailModuleUI.s_Texts.colorOverLifetime, this.m_ColorOverLifetime, false, new GUILayoutOption[0]);
			ModuleUI.GUIMinMaxCurve(TrailModuleUI.s_Texts.widthOverTrail, this.m_WidthOverTrail, new GUILayoutOption[0]);
			base.GUIMinMaxGradient(TrailModuleUI.s_Texts.colorOverTrail, this.m_ColorOverTrail, false, new GUILayoutOption[0]);
			ModuleUI.GUIToggle(TrailModuleUI.s_Texts.generateLightingData, this.m_GenerateLightingData, new GUILayoutOption[0]);
			ParticleSystem[] particleSystems = this.m_ParticleSystemUI.m_ParticleSystems;
			for (int i = 0; i < particleSystems.Length; i++)
			{
				ParticleSystem particleSystem = particleSystems[i];
				if (particleSystem.trails.enabled)
				{
					ParticleSystemRenderer component = particleSystem.GetComponent<ParticleSystemRenderer>();
					if (component != null && component.trailMaterial == null)
					{
						EditorGUILayout.HelpBox("Assign a Trail Material in the Renderer Module", MessageType.Warning, true);
						break;
					}
				}
			}
		}

		public override void UpdateCullingSupportedString(ref string text)
		{
			this.Init();
			if (this.m_Mode.intValue == 0)
			{
				text += "\nTrails module is enabled.";
			}
		}
	}
}
