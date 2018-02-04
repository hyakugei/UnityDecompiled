using System;
using System.Linq;
using UnityEngine;

namespace UnityEditor
{
	internal class InitialModuleUI : ModuleUI
	{
		private class Texts
		{
			public GUIContent duration = EditorGUIUtility.TrTextContent("Duration", "The length of time the Particle System is emitting particles. If the system is looping, this indicates the length of one cycle.", null);

			public GUIContent looping = EditorGUIUtility.TrTextContent("Looping", "If true, the emission cycle will repeat after the duration.", null);

			public GUIContent prewarm = EditorGUIUtility.TrTextContent("Prewarm", "When played a prewarmed system will be in a state as if it had emitted one loop cycle. Can only be used if the system is looping.", null);

			public GUIContent startDelay = EditorGUIUtility.TrTextContent("Start Delay", "Delay in seconds that this Particle System will wait before emitting particles. Cannot be used together with a prewarmed looping system.", null);

			public GUIContent maxParticles = EditorGUIUtility.TrTextContent("Max Particles", "The number of particles in the system will be limited by this number. Emission will be temporarily halted if this is reached.", null);

			public GUIContent lifetime = EditorGUIUtility.TrTextContent("Start Lifetime", "Start lifetime in seconds, particle will die when its lifetime reaches 0.", null);

			public GUIContent speed = EditorGUIUtility.TrTextContent("Start Speed", "The start speed of particles, applied in the starting direction.", null);

			public GUIContent color = EditorGUIUtility.TrTextContent("Start Color", "The start color of particles.", null);

			public GUIContent size3D = EditorGUIUtility.TrTextContent("3D Start Size", "If enabled, you can control the size separately for each axis.", null);

			public GUIContent size = EditorGUIUtility.TrTextContent("Start Size", "The start size of particles.", null);

			public GUIContent rotation3D = EditorGUIUtility.TrTextContent("3D Start Rotation", "If enabled, you can control the rotation separately for each axis.", null);

			public GUIContent rotation = EditorGUIUtility.TrTextContent("Start Rotation", "The start rotation of particles in degrees.", null);

			public GUIContent randomizeRotationDirection = EditorGUIUtility.TrTextContent("Flip Rotation", "Cause some particles to spin in the opposite direction. (Set between 0 and 1, where a higher value causes more to flip)", null);

			public GUIContent autoplay = EditorGUIUtility.TrTextContent("Play On Awake*", "If enabled, the system will start playing automatically. Note that this setting is shared between all Particle Systems in the current particle effect.", null);

			public GUIContent gravity = EditorGUIUtility.TrTextContent("Gravity Modifier", "Scales the gravity defined in Physics Manager", null);

			public GUIContent scalingMode = EditorGUIUtility.TrTextContent("Scaling Mode", "Use the combined scale from our entire hierarchy, just this local particle node, or only apply scale to the shape module.", null);

			public GUIContent simulationSpace = EditorGUIUtility.TrTextContent("Simulation Space", "Makes particle positions simulate in world, local or custom space. In local space they stay relative to their own Transform, and in custom space they are relative to the custom Transform.", null);

			public GUIContent customSimulationSpace = EditorGUIUtility.TrTextContent("Custom Simulation Space", "Makes particle positions simulate relative to a custom Transform component.", null);

			public GUIContent simulationSpeed = EditorGUIUtility.TrTextContent("Simulation Speed", "Scale the playback speed of the Particle System.", null);

			public GUIContent deltaTime = EditorGUIUtility.TrTextContent("Delta Time", "Use either the Delta Time or the Unscaled Delta Time. Useful for playing effects whilst paused.", null);

			public GUIContent autoRandomSeed = EditorGUIUtility.TrTextContent("Auto Random Seed", "Simulate differently each time the effect is played.", null);

			public GUIContent randomSeed = EditorGUIUtility.TrTextContent("Random Seed", "Randomize the look of the Particle System. Using the same seed will make the Particle System play identically each time. After changing this value, restart the Particle System to see the changes, or check the Resimulate box.", null);

			public GUIContent emitterVelocity = EditorGUIUtility.TrTextContent("Emitter Velocity", "When the Particle System is moving, should we use its Transform, or Rigidbody Component, to calculate its velocity?", null);

			public GUIContent stopAction = EditorGUIUtility.TrTextContent("Stop Action", "When the Particle System is stopped and all particles have died, should the GameObject automatically disable/destroy itself?", null);

			public GUIContent x = EditorGUIUtility.TextContent("X");

			public GUIContent y = EditorGUIUtility.TextContent("Y");

			public GUIContent z = EditorGUIUtility.TextContent("Z");

			public GUIContent[] simulationSpaces = new GUIContent[]
			{
				EditorGUIUtility.TrTextContent("Local", null, null),
				EditorGUIUtility.TrTextContent("World", null, null),
				EditorGUIUtility.TrTextContent("Custom", null, null)
			};

			public GUIContent[] scalingModes = new GUIContent[]
			{
				EditorGUIUtility.TrTextContent("Hierarchy", null, null),
				EditorGUIUtility.TrTextContent("Local", null, null),
				EditorGUIUtility.TrTextContent("Shape", null, null)
			};

			public GUIContent[] stopActions = new GUIContent[]
			{
				EditorGUIUtility.TrTextContent("None", null, null),
				EditorGUIUtility.TrTextContent("Disable", null, null),
				EditorGUIUtility.TrTextContent("Destroy", null, null),
				EditorGUIUtility.TrTextContent("Callback", null, null)
			};
		}

		public SerializedProperty m_LengthInSec;

		public SerializedProperty m_Looping;

		public SerializedProperty m_Prewarm;

		public SerializedMinMaxCurve m_StartDelay;

		public SerializedProperty m_PlayOnAwake;

		public SerializedProperty m_SimulationSpace;

		public SerializedProperty m_CustomSimulationSpace;

		public SerializedProperty m_SimulationSpeed;

		public SerializedProperty m_UseUnscaledTime;

		public SerializedProperty m_ScalingMode;

		public SerializedMinMaxCurve m_LifeTime;

		public SerializedMinMaxCurve m_Speed;

		public SerializedMinMaxGradient m_Color;

		public SerializedProperty m_Size3D;

		public SerializedMinMaxCurve m_SizeX;

		public SerializedMinMaxCurve m_SizeY;

		public SerializedMinMaxCurve m_SizeZ;

		public SerializedProperty m_Rotation3D;

		public SerializedMinMaxCurve m_RotationX;

		public SerializedMinMaxCurve m_RotationY;

		public SerializedMinMaxCurve m_RotationZ;

		public SerializedProperty m_RandomizeRotationDirection;

		public SerializedMinMaxCurve m_GravityModifier;

		public SerializedProperty m_EmitterVelocity;

		public SerializedProperty m_MaxNumParticles;

		public SerializedProperty m_AutoRandomSeed;

		public SerializedProperty m_RandomSeed;

		public SerializedProperty m_StopAction;

		private static InitialModuleUI.Texts s_Texts;

		public InitialModuleUI(ParticleSystemUI owner, SerializedObject o, string displayName) : base(owner, o, "InitialModule", displayName, ModuleUI.VisibilityState.VisibleAndFoldedOut)
		{
			this.Init();
		}

		public override bool DrawHeader(Rect rect, GUIContent label)
		{
			label = EditorGUI.BeginProperty(rect, label, this.m_ModuleRootProperty);
			bool result = GUI.Toggle(rect, base.foldout, label, ParticleSystemStyles.Get().emitterHeaderStyle);
			EditorGUI.EndProperty();
			return result;
		}

		public override float GetXAxisScalar()
		{
			return this.m_ParticleSystemUI.GetEmitterDuration();
		}

		protected override void Init()
		{
			if (this.m_LengthInSec == null)
			{
				if (InitialModuleUI.s_Texts == null)
				{
					InitialModuleUI.s_Texts = new InitialModuleUI.Texts();
				}
				this.m_LengthInSec = base.GetProperty0("lengthInSec");
				this.m_Looping = base.GetProperty0("looping");
				this.m_Prewarm = base.GetProperty0("prewarm");
				this.m_StartDelay = new SerializedMinMaxCurve(this, InitialModuleUI.s_Texts.startDelay, "startDelay", false, true);
				this.m_StartDelay.m_AllowCurves = false;
				this.m_PlayOnAwake = base.GetProperty0("playOnAwake");
				this.m_SimulationSpace = base.GetProperty0("moveWithTransform");
				this.m_CustomSimulationSpace = base.GetProperty0("moveWithCustomTransform");
				this.m_SimulationSpeed = base.GetProperty0("simulationSpeed");
				this.m_UseUnscaledTime = base.GetProperty0("useUnscaledTime");
				this.m_ScalingMode = base.GetProperty0("scalingMode");
				this.m_EmitterVelocity = base.GetProperty0("useRigidbodyForVelocity");
				this.m_AutoRandomSeed = base.GetProperty0("autoRandomSeed");
				this.m_RandomSeed = base.GetProperty0("randomSeed");
				this.m_StopAction = base.GetProperty0("stopAction");
				this.m_LifeTime = new SerializedMinMaxCurve(this, InitialModuleUI.s_Texts.lifetime, "startLifetime");
				this.m_Speed = new SerializedMinMaxCurve(this, InitialModuleUI.s_Texts.speed, "startSpeed", ModuleUI.kUseSignedRange);
				this.m_Color = new SerializedMinMaxGradient(this, "startColor");
				this.m_Color.m_AllowRandomColor = true;
				this.m_Size3D = base.GetProperty("size3D");
				this.m_SizeX = new SerializedMinMaxCurve(this, InitialModuleUI.s_Texts.x, "startSize");
				this.m_SizeY = new SerializedMinMaxCurve(this, InitialModuleUI.s_Texts.y, "startSizeY", false, false, this.m_Size3D.boolValue);
				this.m_SizeZ = new SerializedMinMaxCurve(this, InitialModuleUI.s_Texts.z, "startSizeZ", false, false, this.m_Size3D.boolValue);
				this.m_Rotation3D = base.GetProperty("rotation3D");
				this.m_RotationX = new SerializedMinMaxCurve(this, InitialModuleUI.s_Texts.x, "startRotationX", ModuleUI.kUseSignedRange, false, this.m_Rotation3D.boolValue);
				this.m_RotationY = new SerializedMinMaxCurve(this, InitialModuleUI.s_Texts.y, "startRotationY", ModuleUI.kUseSignedRange, false, this.m_Rotation3D.boolValue);
				this.m_RotationZ = new SerializedMinMaxCurve(this, InitialModuleUI.s_Texts.z, "startRotation", ModuleUI.kUseSignedRange);
				this.m_RotationX.m_RemapValue = 57.29578f;
				this.m_RotationY.m_RemapValue = 57.29578f;
				this.m_RotationZ.m_RemapValue = 57.29578f;
				this.m_RotationX.m_DefaultCurveScalar = 3.14159274f;
				this.m_RotationY.m_DefaultCurveScalar = 3.14159274f;
				this.m_RotationZ.m_DefaultCurveScalar = 3.14159274f;
				this.m_RandomizeRotationDirection = base.GetProperty("randomizeRotationDirection");
				this.m_GravityModifier = new SerializedMinMaxCurve(this, InitialModuleUI.s_Texts.gravity, "gravityModifier", ModuleUI.kUseSignedRange);
				this.m_MaxNumParticles = base.GetProperty("maxNumParticles");
			}
		}

		public override void OnInspectorGUI(InitialModuleUI initial)
		{
			ModuleUI.GUIFloat(InitialModuleUI.s_Texts.duration, this.m_LengthInSec, "f2", new GUILayoutOption[0]);
			EditorGUI.BeginChangeCheck();
			bool flag = ModuleUI.GUIToggle(InitialModuleUI.s_Texts.looping, this.m_Looping, new GUILayoutOption[0]);
			if (EditorGUI.EndChangeCheck() && flag)
			{
				ParticleSystem[] particleSystems = this.m_ParticleSystemUI.m_ParticleSystems;
				for (int i = 0; i < particleSystems.Length; i++)
				{
					ParticleSystem particleSystem = particleSystems[i];
					if (particleSystem.time >= particleSystem.main.duration)
					{
						particleSystem.time = 0f;
					}
				}
			}
			using (new EditorGUI.DisabledScope(!this.m_Looping.boolValue))
			{
				ModuleUI.GUIToggle(InitialModuleUI.s_Texts.prewarm, this.m_Prewarm, new GUILayoutOption[0]);
			}
			using (new EditorGUI.DisabledScope(this.m_Prewarm.boolValue && this.m_Looping.boolValue))
			{
				ModuleUI.GUIMinMaxCurve(InitialModuleUI.s_Texts.startDelay, this.m_StartDelay, new GUILayoutOption[0]);
			}
			ModuleUI.GUIMinMaxCurve(InitialModuleUI.s_Texts.lifetime, this.m_LifeTime, new GUILayoutOption[0]);
			ModuleUI.GUIMinMaxCurve(InitialModuleUI.s_Texts.speed, this.m_Speed, new GUILayoutOption[0]);
			EditorGUI.BeginChangeCheck();
			bool flag2 = ModuleUI.GUIToggle(InitialModuleUI.s_Texts.size3D, this.m_Size3D, new GUILayoutOption[0]);
			if (EditorGUI.EndChangeCheck())
			{
				if (!flag2)
				{
					this.m_SizeY.RemoveCurveFromEditor();
					this.m_SizeZ.RemoveCurveFromEditor();
				}
			}
			if (!this.m_SizeX.stateHasMultipleDifferentValues)
			{
				this.m_SizeZ.SetMinMaxState(this.m_SizeX.state, flag2);
				this.m_SizeY.SetMinMaxState(this.m_SizeX.state, flag2);
			}
			if (flag2)
			{
				this.m_SizeX.m_DisplayName = InitialModuleUI.s_Texts.x;
				base.GUITripleMinMaxCurve(GUIContent.none, InitialModuleUI.s_Texts.x, this.m_SizeX, InitialModuleUI.s_Texts.y, this.m_SizeY, InitialModuleUI.s_Texts.z, this.m_SizeZ, null, new GUILayoutOption[0]);
			}
			else
			{
				this.m_SizeX.m_DisplayName = InitialModuleUI.s_Texts.size;
				ModuleUI.GUIMinMaxCurve(InitialModuleUI.s_Texts.size, this.m_SizeX, new GUILayoutOption[0]);
			}
			EditorGUI.BeginChangeCheck();
			bool flag3 = ModuleUI.GUIToggle(InitialModuleUI.s_Texts.rotation3D, this.m_Rotation3D, new GUILayoutOption[0]);
			if (EditorGUI.EndChangeCheck())
			{
				if (!flag3)
				{
					this.m_RotationX.RemoveCurveFromEditor();
					this.m_RotationY.RemoveCurveFromEditor();
				}
			}
			if (!this.m_RotationZ.stateHasMultipleDifferentValues)
			{
				this.m_RotationX.SetMinMaxState(this.m_RotationZ.state, flag3);
				this.m_RotationY.SetMinMaxState(this.m_RotationZ.state, flag3);
			}
			if (flag3)
			{
				this.m_RotationZ.m_DisplayName = InitialModuleUI.s_Texts.z;
				base.GUITripleMinMaxCurve(GUIContent.none, InitialModuleUI.s_Texts.x, this.m_RotationX, InitialModuleUI.s_Texts.y, this.m_RotationY, InitialModuleUI.s_Texts.z, this.m_RotationZ, null, new GUILayoutOption[0]);
			}
			else
			{
				this.m_RotationZ.m_DisplayName = InitialModuleUI.s_Texts.rotation;
				ModuleUI.GUIMinMaxCurve(InitialModuleUI.s_Texts.rotation, this.m_RotationZ, new GUILayoutOption[0]);
			}
			ModuleUI.GUIFloat(InitialModuleUI.s_Texts.randomizeRotationDirection, this.m_RandomizeRotationDirection, new GUILayoutOption[0]);
			base.GUIMinMaxGradient(InitialModuleUI.s_Texts.color, this.m_Color, false, new GUILayoutOption[0]);
			ModuleUI.GUIMinMaxCurve(InitialModuleUI.s_Texts.gravity, this.m_GravityModifier, new GUILayoutOption[0]);
			int num = ModuleUI.GUIPopup(InitialModuleUI.s_Texts.simulationSpace, this.m_SimulationSpace, InitialModuleUI.s_Texts.simulationSpaces, new GUILayoutOption[0]);
			if (num == 2 && this.m_CustomSimulationSpace != null)
			{
				ModuleUI.GUIObject(InitialModuleUI.s_Texts.customSimulationSpace, this.m_CustomSimulationSpace, new GUILayoutOption[0]);
			}
			ModuleUI.GUIFloat(InitialModuleUI.s_Texts.simulationSpeed, this.m_SimulationSpeed, new GUILayoutOption[0]);
			ModuleUI.GUIBoolAsPopup(InitialModuleUI.s_Texts.deltaTime, this.m_UseUnscaledTime, new string[]
			{
				"Scaled",
				"Unscaled"
			}, new GUILayoutOption[0]);
			bool flag4 = this.m_ParticleSystemUI.m_ParticleSystems.FirstOrDefault((ParticleSystem o) => !o.shape.enabled || (o.shape.shapeType != ParticleSystemShapeType.SkinnedMeshRenderer && o.shape.shapeType != ParticleSystemShapeType.MeshRenderer)) != null;
			if (flag4)
			{
				ModuleUI.GUIPopup(InitialModuleUI.s_Texts.scalingMode, this.m_ScalingMode, InitialModuleUI.s_Texts.scalingModes, new GUILayoutOption[0]);
			}
			bool boolValue = this.m_PlayOnAwake.boolValue;
			bool flag5 = ModuleUI.GUIToggle(InitialModuleUI.s_Texts.autoplay, this.m_PlayOnAwake, new GUILayoutOption[0]);
			if (boolValue != flag5)
			{
				this.m_ParticleSystemUI.m_ParticleEffectUI.PlayOnAwakeChanged(flag5);
			}
			ModuleUI.GUIBoolAsPopup(InitialModuleUI.s_Texts.emitterVelocity, this.m_EmitterVelocity, new string[]
			{
				"Transform",
				"Rigidbody"
			}, new GUILayoutOption[0]);
			ModuleUI.GUIInt(InitialModuleUI.s_Texts.maxParticles, this.m_MaxNumParticles, new GUILayoutOption[0]);
			if (!ModuleUI.GUIToggle(InitialModuleUI.s_Texts.autoRandomSeed, this.m_AutoRandomSeed, new GUILayoutOption[0]))
			{
				bool flag6 = this.m_ParticleSystemUI.m_ParticleEffectUI.m_Owner is ParticleSystemInspector;
				if (flag6)
				{
					GUILayout.BeginHorizontal(new GUILayoutOption[0]);
					ModuleUI.GUIInt(InitialModuleUI.s_Texts.randomSeed, this.m_RandomSeed, new GUILayoutOption[0]);
					if (!this.m_ParticleSystemUI.multiEdit && GUILayout.Button("Reseed", EditorStyles.miniButton, new GUILayoutOption[]
					{
						GUILayout.Width(60f)
					}))
					{
						this.m_RandomSeed.intValue = this.m_ParticleSystemUI.m_ParticleSystems[0].GenerateRandomSeed();
					}
					GUILayout.EndHorizontal();
				}
				else
				{
					ModuleUI.GUIInt(InitialModuleUI.s_Texts.randomSeed, this.m_RandomSeed, new GUILayoutOption[0]);
					if (!this.m_ParticleSystemUI.multiEdit && GUILayout.Button("Reseed", EditorStyles.miniButton, new GUILayoutOption[0]))
					{
						this.m_RandomSeed.intValue = this.m_ParticleSystemUI.m_ParticleSystems[0].GenerateRandomSeed();
					}
				}
			}
			ModuleUI.GUIPopup(InitialModuleUI.s_Texts.stopAction, this.m_StopAction, InitialModuleUI.s_Texts.stopActions, new GUILayoutOption[0]);
		}

		public override void UpdateCullingSupportedString(ref string text)
		{
			if (this.m_SimulationSpace.intValue != 0)
			{
				text += "\nLocal space simulation is not being used.";
			}
			if (this.m_GravityModifier.state != MinMaxCurveState.k_Scalar)
			{
				text += "\nGravity modifier is not constant.";
			}
		}
	}
}
