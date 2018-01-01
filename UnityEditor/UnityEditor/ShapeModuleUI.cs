using System;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace UnityEditor
{
	internal class ShapeModuleUI : ModuleUI
	{
		private struct MultiModeParameter
		{
			public enum ValueMode
			{
				Random,
				Loop,
				PingPong,
				BurstSpread
			}

			public SerializedProperty m_Value;

			public SerializedProperty m_Mode;

			public SerializedProperty m_Spread;

			public SerializedMinMaxCurve m_Speed;

			public static ShapeModuleUI.MultiModeParameter GetProperty(ModuleUI ui, string name, GUIContent speed)
			{
				return new ShapeModuleUI.MultiModeParameter
				{
					m_Value = ui.GetProperty(name + ".value"),
					m_Mode = ui.GetProperty(name + ".mode"),
					m_Spread = ui.GetProperty(name + ".spread"),
					m_Speed = new SerializedMinMaxCurve(ui, speed, name + ".speed", ModuleUI.kUseSignedRange),
					m_Speed = 
					{
						m_AllowRandom = false
					}
				};
			}

			public void OnInspectorGUI(ShapeModuleUI.MultiModeTexts text)
			{
				ModuleUI.GUIFloat(text.value, this.m_Value, new GUILayoutOption[0]);
				EditorGUI.indentLevel++;
				ModuleUI.GUIPopup(text.mode, this.m_Mode, ShapeModuleUI.s_Texts.emissionModes, new GUILayoutOption[0]);
				ModuleUI.GUIFloat(text.spread, this.m_Spread, new GUILayoutOption[0]);
				if (!this.m_Mode.hasMultipleDifferentValues)
				{
					ShapeModuleUI.MultiModeParameter.ValueMode intValue = (ShapeModuleUI.MultiModeParameter.ValueMode)this.m_Mode.intValue;
					if (intValue == ShapeModuleUI.MultiModeParameter.ValueMode.Loop || intValue == ShapeModuleUI.MultiModeParameter.ValueMode.PingPong)
					{
						ModuleUI.GUIMinMaxCurve(text.speed, this.m_Speed, new GUILayoutOption[0]);
					}
				}
				EditorGUI.indentLevel--;
			}
		}

		private class Texts
		{
			public GUIContent shape = EditorGUIUtility.TrTextContent("Shape", "Defines the shape of the volume from which particles can be emitted, and the direction of the start velocity.", null);

			public GUIContent radius = EditorGUIUtility.TrTextContent("Radius", "Radius of the shape.", null);

			public GUIContent radiusThickness = EditorGUIUtility.TrTextContent("Radius Thickness", "Control the thickness of the spawn volume, from 0 to 1.", null);

			public GUIContent coneAngle = EditorGUIUtility.TrTextContent("Angle", "Angle of the cone.", null);

			public GUIContent coneLength = EditorGUIUtility.TrTextContent("Length", "Length of the cone.", null);

			public GUIContent boxThickness = EditorGUIUtility.TrTextContent("Box Thickness", "When using shell/edge modes, control the thickness of the spawn volume, from 0 to 1.", null);

			public GUIContent meshType = EditorGUIUtility.TrTextContent("Type", "Generate particles from vertices, edges or triangles.", null);

			public GUIContent mesh = EditorGUIUtility.TrTextContent("Mesh", "Mesh that the particle system will emit from.", null);

			public GUIContent meshRenderer = EditorGUIUtility.TrTextContent("Mesh", "MeshRenderer that the particle system will emit from.", null);

			public GUIContent skinnedMeshRenderer = EditorGUIUtility.TrTextContent("Mesh", "SkinnedMeshRenderer that the particle system will emit from.", null);

			public GUIContent meshMaterialIndex = EditorGUIUtility.TrTextContent("Single Material", "Only emit from a specific material of the mesh.", null);

			public GUIContent useMeshColors = EditorGUIUtility.TrTextContent("Use Mesh Colors", "Modulate particle color with mesh vertex colors, or if they don't exist, use the shader color property \"_Color\" or \"_TintColor\" from the material. Does not read texture colors.", null);

			public GUIContent meshNormalOffset = EditorGUIUtility.TrTextContent("Normal Offset", "Offset particle spawn positions along the mesh normal.", null);

			public GUIContent texture = EditorGUIUtility.TrTextContent("Texture", "Texture that the particles will sample their color from.", null);

			public GUIContent textureClipChannel = EditorGUIUtility.TrTextContent("Clip Channel", "Select a channel to use for discarding particles.", null);

			public GUIContent textureClipThreshold = EditorGUIUtility.TrTextContent("Clip Threshold", "Only emit from parts of the texture where the Clip Channel is greater than or equal to this value.", null);

			public GUIContent textureColorAffectsParticles = EditorGUIUtility.TrTextContent("Color affects Particles", "Multiply the particle color by the texture RGB value.", null);

			public GUIContent textureAlphaAffectsParticles = EditorGUIUtility.TrTextContent("Alpha affects Particles", "Multiply the particle alpha by the texture alpha value.", null);

			public GUIContent textureBilinearFiltering = EditorGUIUtility.TrTextContent("Bilinear Filtering", "Blend between pixels on the texture.", null);

			public GUIContent textureUVChannel = EditorGUIUtility.TrTextContent("UV Channel", "Use the selected UV channel from the source mesh, for reading the texture.", null);

			public GUIContent alignToDirection = EditorGUIUtility.TrTextContent("Align To Direction", "Automatically align particles based on their initial direction of travel.", null);

			public GUIContent randomDirectionAmount = EditorGUIUtility.TrTextContent("Randomize Direction", "Randomize the emission direction.", null);

			public GUIContent sphericalDirectionAmount = EditorGUIUtility.TrTextContent("Spherize Direction", "Spherize the emission direction.", null);

			public GUIContent randomPositionAmount = EditorGUIUtility.TrTextContent("Randomize Position", "Randomize the starting positions.", null);

			public GUIContent emitFrom = EditorGUIUtility.TrTextContent("Emit from:", "Specifies from where particles are emitted.", null);

			public GUIContent donutRadius = EditorGUIUtility.TrTextContent("Donut Radius", "The radius of the donut. Used to control the thickness of the ring.", null);

			public GUIContent position = EditorGUIUtility.TrTextContent("Position", "Translate the emission shape.", null);

			public GUIContent rotation = EditorGUIUtility.TrTextContent("Rotation", "Rotate the emission shape.", null);

			public GUIContent scale = EditorGUIUtility.TrTextContent("Scale", "Scale the emission shape.", null);

			public GUIContent[] shapeTypes = new GUIContent[]
			{
				EditorGUIUtility.TrTextContent("Sphere", null, null),
				EditorGUIUtility.TrTextContent("Hemisphere", null, null),
				EditorGUIUtility.TrTextContent("Cone", null, null),
				EditorGUIUtility.TrTextContent("Donut", null, null),
				EditorGUIUtility.TrTextContent("Box", null, null),
				EditorGUIUtility.TrTextContent("Mesh", null, null),
				EditorGUIUtility.TrTextContent("Mesh Renderer", null, null),
				EditorGUIUtility.TrTextContent("Skinned Mesh Renderer", null, null),
				EditorGUIUtility.TrTextContent("Circle", null, null),
				EditorGUIUtility.TrTextContent("Edge", null, null),
				EditorGUIUtility.TextContent("Rectangle")
			};

			public GUIContent[] boxTypes = new GUIContent[]
			{
				EditorGUIUtility.TrTextContent("Volume", null, null),
				EditorGUIUtility.TrTextContent("Shell", null, null),
				EditorGUIUtility.TrTextContent("Edge", null, null)
			};

			public GUIContent[] coneTypes = new GUIContent[]
			{
				EditorGUIUtility.TrTextContent("Base", null, null),
				EditorGUIUtility.TrTextContent("Volume", null, null)
			};

			public GUIContent[] meshTypes = new GUIContent[]
			{
				EditorGUIUtility.TrTextContent("Vertex", null, null),
				EditorGUIUtility.TrTextContent("Edge", null, null),
				EditorGUIUtility.TrTextContent("Triangle", null, null)
			};

			public GUIContent[] emissionModes = new GUIContent[]
			{
				EditorGUIUtility.TrTextContent("Random", null, null),
				EditorGUIUtility.TrTextContent("Loop", null, null),
				EditorGUIUtility.TrTextContent("Ping-Pong", null, null),
				EditorGUIUtility.TrTextContent("Burst Spread", null, null)
			};

			public GUIContent[] textureClipChannels = new GUIContent[]
			{
				EditorGUIUtility.TextContent("Red"),
				EditorGUIUtility.TextContent("Green"),
				EditorGUIUtility.TextContent("Blue"),
				EditorGUIUtility.TextContent("Alpha")
			};
		}

		private class MultiModeTexts
		{
			public GUIContent value;

			public GUIContent mode;

			public GUIContent spread;

			public GUIContent speed;

			public MultiModeTexts(string _value, string _mode, string _spread, string _speed)
			{
				this.value = EditorGUIUtility.TextContent(_value);
				this.mode = EditorGUIUtility.TextContent(_mode);
				this.spread = EditorGUIUtility.TextContent(_spread);
				this.speed = EditorGUIUtility.TextContent(_speed);
			}
		}

		private SerializedProperty m_Type;

		private SerializedProperty m_RandomDirectionAmount;

		private SerializedProperty m_SphericalDirectionAmount;

		private SerializedProperty m_RandomPositionAmount;

		private SerializedProperty m_AlignToDirection;

		private SerializedProperty m_Position;

		private SerializedProperty m_Scale;

		private SerializedProperty m_Rotation;

		private ShapeModuleUI.MultiModeParameter m_Radius;

		private SerializedProperty m_RadiusThickness;

		private SerializedProperty m_Angle;

		private SerializedProperty m_Length;

		private SerializedProperty m_BoxThickness;

		private ShapeModuleUI.MultiModeParameter m_Arc;

		private SerializedProperty m_DonutRadius;

		private SerializedProperty m_PlacementMode;

		private SerializedProperty m_Mesh;

		private SerializedProperty m_MeshRenderer;

		private SerializedProperty m_SkinnedMeshRenderer;

		private SerializedProperty m_MeshMaterialIndex;

		private SerializedProperty m_UseMeshMaterialIndex;

		private SerializedProperty m_UseMeshColors;

		private SerializedProperty m_MeshNormalOffset;

		private SerializedProperty m_Texture;

		private SerializedProperty m_TextureClipChannel;

		private SerializedProperty m_TextureClipThreshold;

		private SerializedProperty m_TextureColorAffectsParticles;

		private SerializedProperty m_TextureAlphaAffectsParticles;

		private SerializedProperty m_TextureBilinearFiltering;

		private SerializedProperty m_TextureUVChannel;

		private static Material s_Material;

		private static Material s_TextureMaterial;

		private static Material s_SphereTextureMaterial;

		private static Mesh s_CircleMesh;

		private static Mesh s_QuadMesh;

		private static Mesh s_SphereMesh;

		private static Mesh s_HemisphereMesh;

		private static readonly Matrix4x4 s_ArcHandleOffsetMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.AngleAxis(90f, Vector3.right) * Quaternion.AngleAxis(90f, Vector3.up), Vector3.one);

		private ArcHandle m_ArcHandle = new ArcHandle();

		private BoxBoundsHandle m_BoxBoundsHandle = new BoxBoundsHandle();

		private SphereBoundsHandle m_SphereBoundsHandle = new SphereBoundsHandle();

		private static Color s_ShapeGizmoColor = new Color(0.5803922f, 0.8980392f, 1f, 0.9f);

		private readonly ParticleSystemShapeType[] m_GuiTypes = new ParticleSystemShapeType[]
		{
			ParticleSystemShapeType.Sphere,
			ParticleSystemShapeType.Hemisphere,
			ParticleSystemShapeType.Cone,
			ParticleSystemShapeType.Donut,
			ParticleSystemShapeType.Box,
			ParticleSystemShapeType.Mesh,
			ParticleSystemShapeType.MeshRenderer,
			ParticleSystemShapeType.SkinnedMeshRenderer,
			ParticleSystemShapeType.Circle,
			ParticleSystemShapeType.SingleSidedEdge,
			ParticleSystemShapeType.Rectangle
		};

		private readonly int[] m_TypeToGuiTypeIndex = new int[]
		{
			0,
			0,
			1,
			1,
			2,
			4,
			5,
			2,
			2,
			2,
			8,
			8,
			9,
			6,
			7,
			4,
			4,
			3,
			10
		};

		private readonly ParticleSystemShapeType[] boxShapes = new ParticleSystemShapeType[]
		{
			ParticleSystemShapeType.Box,
			ParticleSystemShapeType.BoxShell,
			ParticleSystemShapeType.BoxEdge
		};

		private readonly ParticleSystemShapeType[] coneShapes = new ParticleSystemShapeType[]
		{
			ParticleSystemShapeType.Cone,
			ParticleSystemShapeType.ConeVolume
		};

		private static ShapeModuleUI.Texts s_Texts;

		private static ShapeModuleUI.MultiModeTexts s_RadiusTexts = new ShapeModuleUI.MultiModeTexts("Radius|New particles are spawned along the radius.", "Mode|Control how particles are spawned along the radius.", "Spread|Spawn particles only at specific positions along the radius (0 to disable).", "Speed|Control the speed that the emission position moves along the radius.");

		private static ShapeModuleUI.MultiModeTexts s_ArcTexts = new ShapeModuleUI.MultiModeTexts("Arc|New particles are spawned around the arc.", "Mode|Control how particles are spawned around the arc.", "Spread|Spawn particles only at specific angles around the arc (0 to disable).", "Speed|Control the speed that the emission position moves around the arc.");

		public ShapeModuleUI(ParticleSystemUI owner, SerializedObject o, string displayName) : base(owner, o, "ShapeModule", displayName, ModuleUI.VisibilityState.VisibleAndFolded)
		{
			this.m_ToolTip = "Shape of the emitter volume, which controls where particles are emitted and their initial direction.";
		}

		protected override void Init()
		{
			if (this.m_Type == null)
			{
				if (ShapeModuleUI.s_Texts == null)
				{
					ShapeModuleUI.s_Texts = new ShapeModuleUI.Texts();
				}
				this.m_Type = base.GetProperty("type");
				this.m_Radius = ShapeModuleUI.MultiModeParameter.GetProperty(this, "radius", ShapeModuleUI.s_RadiusTexts.speed);
				this.m_RadiusThickness = base.GetProperty("radiusThickness");
				this.m_Angle = base.GetProperty("angle");
				this.m_Length = base.GetProperty("length");
				this.m_BoxThickness = base.GetProperty("boxThickness");
				this.m_Arc = ShapeModuleUI.MultiModeParameter.GetProperty(this, "arc", ShapeModuleUI.s_ArcTexts.speed);
				this.m_DonutRadius = base.GetProperty("donutRadius");
				this.m_PlacementMode = base.GetProperty("placementMode");
				this.m_Mesh = base.GetProperty("m_Mesh");
				this.m_MeshRenderer = base.GetProperty("m_MeshRenderer");
				this.m_SkinnedMeshRenderer = base.GetProperty("m_SkinnedMeshRenderer");
				this.m_MeshMaterialIndex = base.GetProperty("m_MeshMaterialIndex");
				this.m_UseMeshMaterialIndex = base.GetProperty("m_UseMeshMaterialIndex");
				this.m_UseMeshColors = base.GetProperty("m_UseMeshColors");
				this.m_MeshNormalOffset = base.GetProperty("m_MeshNormalOffset");
				this.m_Texture = base.GetProperty("m_Texture");
				this.m_TextureClipChannel = base.GetProperty("m_TextureClipChannel");
				this.m_TextureClipThreshold = base.GetProperty("m_TextureClipThreshold");
				this.m_TextureColorAffectsParticles = base.GetProperty("m_TextureColorAffectsParticles");
				this.m_TextureAlphaAffectsParticles = base.GetProperty("m_TextureAlphaAffectsParticles");
				this.m_TextureBilinearFiltering = base.GetProperty("m_TextureBilinearFiltering");
				this.m_TextureUVChannel = base.GetProperty("m_TextureUVChannel");
				this.m_RandomDirectionAmount = base.GetProperty("randomDirectionAmount");
				this.m_SphericalDirectionAmount = base.GetProperty("sphericalDirectionAmount");
				this.m_RandomPositionAmount = base.GetProperty("randomPositionAmount");
				this.m_AlignToDirection = base.GetProperty("alignToDirection");
				this.m_Position = base.GetProperty("m_Position");
				this.m_Scale = base.GetProperty("m_Scale");
				this.m_Rotation = base.GetProperty("m_Rotation");
				if (!ShapeModuleUI.s_Material)
				{
					ShapeModuleUI.s_Material = (EditorGUIUtility.GetBuiltinExtraResource(typeof(Material), "Default-Material.mat") as Material);
				}
				if (!ShapeModuleUI.s_TextureMaterial)
				{
					ShapeModuleUI.s_TextureMaterial = new Material((Shader)EditorGUIUtility.Load("SceneView/ParticleShapeGizmo.shader"));
				}
				if (!ShapeModuleUI.s_SphereTextureMaterial)
				{
					ShapeModuleUI.s_SphereTextureMaterial = new Material((Shader)EditorGUIUtility.Load("SceneView/ParticleShapeGizmoSphere.shader"));
				}
				if (!ShapeModuleUI.s_CircleMesh)
				{
					ShapeModuleUI.s_CircleMesh = ((GameObject)EditorGUIUtility.Load("SceneView/Circle.fbx")).transform.GetComponent<MeshFilter>().sharedMesh;
				}
				if (!ShapeModuleUI.s_QuadMesh)
				{
					ShapeModuleUI.s_QuadMesh = (Resources.GetBuiltinResource(typeof(Mesh), "Quad.fbx") as Mesh);
				}
				if (!ShapeModuleUI.s_SphereMesh)
				{
					ShapeModuleUI.s_SphereMesh = (Resources.GetBuiltinResource(typeof(Mesh), "New-Sphere.fbx") as Mesh);
				}
				if (!ShapeModuleUI.s_HemisphereMesh)
				{
					ShapeModuleUI.s_HemisphereMesh = ((GameObject)EditorGUIUtility.Load("SceneView/Hemisphere.fbx")).transform.GetComponent<MeshFilter>().sharedMesh;
				}
			}
		}

		public override float GetXAxisScalar()
		{
			return this.m_ParticleSystemUI.GetEmitterDuration();
		}

		private ParticleSystemShapeType ConvertConeEmitFromToConeType(int emitFrom)
		{
			return this.coneShapes[emitFrom];
		}

		private int ConvertConeTypeToConeEmitFrom(ParticleSystemShapeType shapeType)
		{
			return Array.IndexOf<ParticleSystemShapeType>(this.coneShapes, shapeType);
		}

		private ParticleSystemShapeType ConvertBoxEmitFromToBoxType(int emitFrom)
		{
			return this.boxShapes[emitFrom];
		}

		private int ConvertBoxTypeToBoxEmitFrom(ParticleSystemShapeType shapeType)
		{
			return Array.IndexOf<ParticleSystemShapeType>(this.boxShapes, shapeType);
		}

		public override void OnInspectorGUI(InitialModuleUI initial)
		{
			EditorGUI.showMixedValue = this.m_Type.hasMultipleDifferentValues;
			int num = this.m_Type.intValue;
			int num2 = this.m_TypeToGuiTypeIndex[num];
			EditorGUI.BeginChangeCheck();
			int num3 = ModuleUI.GUIPopup(ShapeModuleUI.s_Texts.shape, num2, ShapeModuleUI.s_Texts.shapeTypes, this.m_Type, new GUILayoutOption[0]);
			bool flag = EditorGUI.EndChangeCheck();
			EditorGUI.showMixedValue = false;
			ParticleSystemShapeType particleSystemShapeType = this.m_GuiTypes[num3];
			if (num3 != num2)
			{
				num = (int)particleSystemShapeType;
			}
			if (!this.m_Type.hasMultipleDifferentValues)
			{
				switch (particleSystemShapeType)
				{
				case ParticleSystemShapeType.Sphere:
				case ParticleSystemShapeType.Hemisphere:
					ModuleUI.GUIFloat(ShapeModuleUI.s_Texts.radius, this.m_Radius.m_Value, new GUILayoutOption[0]);
					ModuleUI.GUIFloat(ShapeModuleUI.s_Texts.radiusThickness, this.m_RadiusThickness, new GUILayoutOption[0]);
					break;
				case ParticleSystemShapeType.Cone:
				{
					ModuleUI.GUIFloat(ShapeModuleUI.s_Texts.coneAngle, this.m_Angle, new GUILayoutOption[0]);
					ModuleUI.GUIFloat(ShapeModuleUI.s_Texts.radius, this.m_Radius.m_Value, new GUILayoutOption[0]);
					ModuleUI.GUIFloat(ShapeModuleUI.s_Texts.radiusThickness, this.m_RadiusThickness, new GUILayoutOption[0]);
					this.m_Arc.OnInspectorGUI(ShapeModuleUI.s_ArcTexts);
					bool disabled = num != 8;
					using (new EditorGUI.DisabledScope(disabled))
					{
						ModuleUI.GUIFloat(ShapeModuleUI.s_Texts.coneLength, this.m_Length, new GUILayoutOption[0]);
					}
					int num4 = this.ConvertConeTypeToConeEmitFrom((ParticleSystemShapeType)num);
					num4 = ModuleUI.GUIPopup(ShapeModuleUI.s_Texts.emitFrom, num4, ShapeModuleUI.s_Texts.coneTypes, this.m_Type, new GUILayoutOption[0]);
					num = (int)this.ConvertConeEmitFromToConeType(num4);
					break;
				}
				case ParticleSystemShapeType.Box:
				{
					int num5 = this.ConvertBoxTypeToBoxEmitFrom((ParticleSystemShapeType)num);
					num5 = ModuleUI.GUIPopup(ShapeModuleUI.s_Texts.emitFrom, num5, ShapeModuleUI.s_Texts.boxTypes, this.m_Type, new GUILayoutOption[0]);
					num = (int)this.ConvertBoxEmitFromToBoxType(num5);
					if (num == 15 || num == 16)
					{
						ModuleUI.GUIVector3Field(ShapeModuleUI.s_Texts.boxThickness, this.m_BoxThickness, new GUILayoutOption[0]);
					}
					break;
				}
				case ParticleSystemShapeType.Mesh:
				case ParticleSystemShapeType.MeshRenderer:
				case ParticleSystemShapeType.SkinnedMeshRenderer:
				{
					ModuleUI.GUIPopup(ShapeModuleUI.s_Texts.meshType, this.m_PlacementMode, ShapeModuleUI.s_Texts.meshTypes, new GUILayoutOption[0]);
					Material material = null;
					Mesh mesh = null;
					if (particleSystemShapeType == ParticleSystemShapeType.Mesh)
					{
						ModuleUI.GUIObject(ShapeModuleUI.s_Texts.mesh, this.m_Mesh, new GUILayoutOption[0]);
						mesh = (Mesh)this.m_Mesh.objectReferenceValue;
					}
					else if (particleSystemShapeType == ParticleSystemShapeType.MeshRenderer)
					{
						ModuleUI.GUIObject(ShapeModuleUI.s_Texts.meshRenderer, this.m_MeshRenderer, new GUILayoutOption[0]);
						MeshRenderer meshRenderer = (MeshRenderer)this.m_MeshRenderer.objectReferenceValue;
						if (meshRenderer)
						{
							material = meshRenderer.sharedMaterial;
							if (meshRenderer.GetComponent<MeshFilter>())
							{
								mesh = meshRenderer.GetComponent<MeshFilter>().sharedMesh;
							}
						}
					}
					else
					{
						ModuleUI.GUIObject(ShapeModuleUI.s_Texts.skinnedMeshRenderer, this.m_SkinnedMeshRenderer, new GUILayoutOption[0]);
						SkinnedMeshRenderer skinnedMeshRenderer = (SkinnedMeshRenderer)this.m_SkinnedMeshRenderer.objectReferenceValue;
						if (skinnedMeshRenderer)
						{
							material = skinnedMeshRenderer.sharedMaterial;
							mesh = skinnedMeshRenderer.sharedMesh;
						}
					}
					ModuleUI.GUIToggleWithIntField(ShapeModuleUI.s_Texts.meshMaterialIndex, this.m_UseMeshMaterialIndex, this.m_MeshMaterialIndex, false, new GUILayoutOption[0]);
					bool flag2 = ModuleUI.GUIToggle(ShapeModuleUI.s_Texts.useMeshColors, this.m_UseMeshColors, new GUILayoutOption[0]);
					if (flag2)
					{
						if (material != null && mesh != null)
						{
							int name = Shader.PropertyToID("_Color");
							int name2 = Shader.PropertyToID("_TintColor");
							if (!material.HasProperty(name) && !material.HasProperty(name2) && !mesh.HasChannel(Mesh.InternalShaderChannel.Color))
							{
								GUIContent gUIContent = EditorGUIUtility.TrTextContent("To use mesh colors, your source mesh must either provide vertex colors, or its shader must contain a color property named \"_Color\" or \"_TintColor\".", null, null);
								EditorGUILayout.HelpBox(gUIContent.text, MessageType.Warning, true);
							}
						}
					}
					ModuleUI.GUIFloat(ShapeModuleUI.s_Texts.meshNormalOffset, this.m_MeshNormalOffset, new GUILayoutOption[0]);
					break;
				}
				case ParticleSystemShapeType.Circle:
					ModuleUI.GUIFloat(ShapeModuleUI.s_Texts.radius, this.m_Radius.m_Value, new GUILayoutOption[0]);
					ModuleUI.GUIFloat(ShapeModuleUI.s_Texts.radiusThickness, this.m_RadiusThickness, new GUILayoutOption[0]);
					this.m_Arc.OnInspectorGUI(ShapeModuleUI.s_ArcTexts);
					break;
				case ParticleSystemShapeType.SingleSidedEdge:
					this.m_Radius.OnInspectorGUI(ShapeModuleUI.s_RadiusTexts);
					break;
				case ParticleSystemShapeType.Donut:
					ModuleUI.GUIFloat(ShapeModuleUI.s_Texts.radius, this.m_Radius.m_Value, new GUILayoutOption[0]);
					ModuleUI.GUIFloat(ShapeModuleUI.s_Texts.donutRadius, this.m_DonutRadius, new GUILayoutOption[0]);
					ModuleUI.GUIFloat(ShapeModuleUI.s_Texts.radiusThickness, this.m_RadiusThickness, new GUILayoutOption[0]);
					this.m_Arc.OnInspectorGUI(ShapeModuleUI.s_ArcTexts);
					break;
				}
			}
			if (flag || !this.m_Type.hasMultipleDifferentValues)
			{
				this.m_Type.intValue = num;
			}
			this.OnTextureInspectorGUI();
			this.OnTransformInspectorGUI();
			this.OnMiscInspectorGUI();
		}

		private void OnTextureInspectorGUI()
		{
			EditorGUILayout.Space();
			ModuleUI.GUIObject(ShapeModuleUI.s_Texts.texture, this.m_Texture, new GUILayoutOption[0]);
			ModuleUI.GUIPopup(ShapeModuleUI.s_Texts.textureClipChannel, this.m_TextureClipChannel, ShapeModuleUI.s_Texts.textureClipChannels, new GUILayoutOption[0]);
			ModuleUI.GUIFloat(ShapeModuleUI.s_Texts.textureClipThreshold, this.m_TextureClipThreshold, new GUILayoutOption[0]);
			ModuleUI.GUIToggle(ShapeModuleUI.s_Texts.textureColorAffectsParticles, this.m_TextureColorAffectsParticles, new GUILayoutOption[0]);
			ModuleUI.GUIToggle(ShapeModuleUI.s_Texts.textureAlphaAffectsParticles, this.m_TextureAlphaAffectsParticles, new GUILayoutOption[0]);
			ModuleUI.GUIToggle(ShapeModuleUI.s_Texts.textureBilinearFiltering, this.m_TextureBilinearFiltering, new GUILayoutOption[0]);
			if (!this.m_Type.hasMultipleDifferentValues)
			{
				if (this.m_Type.intValue == 6 || this.m_Type.intValue == 13 || this.m_Type.intValue == 14)
				{
					ModuleUI.GUIInt(ShapeModuleUI.s_Texts.textureUVChannel, this.m_TextureUVChannel, new GUILayoutOption[0]);
				}
			}
		}

		private void OnTransformInspectorGUI()
		{
			EditorGUILayout.Space();
			ModuleUI.GUIVector3Field(ShapeModuleUI.s_Texts.position, this.m_Position, new GUILayoutOption[0]);
			ModuleUI.GUIVector3Field(ShapeModuleUI.s_Texts.rotation, this.m_Rotation, new GUILayoutOption[0]);
			ModuleUI.GUIVector3Field(ShapeModuleUI.s_Texts.scale, this.m_Scale, new GUILayoutOption[0]);
		}

		private void OnMiscInspectorGUI()
		{
			EditorGUILayout.Space();
			ModuleUI.GUIToggle(ShapeModuleUI.s_Texts.alignToDirection, this.m_AlignToDirection, new GUILayoutOption[0]);
			ModuleUI.GUIFloat(ShapeModuleUI.s_Texts.randomDirectionAmount, this.m_RandomDirectionAmount, new GUILayoutOption[0]);
			ModuleUI.GUIFloat(ShapeModuleUI.s_Texts.sphericalDirectionAmount, this.m_SphericalDirectionAmount, new GUILayoutOption[0]);
			ModuleUI.GUIFloat(ShapeModuleUI.s_Texts.randomPositionAmount, this.m_RandomPositionAmount, new GUILayoutOption[0]);
		}

		public override void OnSceneViewGUI()
		{
			Color color = Handles.color;
			Handles.color = ShapeModuleUI.s_ShapeGizmoColor;
			Matrix4x4 matrix = Handles.matrix;
			EditorGUI.BeginChangeCheck();
			ParticleSystem[] particleSystems = this.m_ParticleSystemUI.m_ParticleSystems;
			for (int i = 0; i < particleSystems.Length; i++)
			{
				ParticleSystem particleSystem = particleSystems[i];
				ParticleSystem.ShapeModule shape = particleSystem.shape;
				ParticleSystem.MainModule main = particleSystem.main;
				ParticleSystemShapeType shapeType = shape.shapeType;
				Matrix4x4 matrix4x = default(Matrix4x4);
				if (main.scalingMode == ParticleSystemScalingMode.Local)
				{
					matrix4x.SetTRS(particleSystem.transform.position, particleSystem.transform.rotation, particleSystem.transform.localScale);
				}
				else if (main.scalingMode == ParticleSystemScalingMode.Hierarchy)
				{
					matrix4x = particleSystem.transform.localToWorldMatrix;
				}
				else
				{
					matrix4x.SetTRS(particleSystem.transform.position, particleSystem.transform.rotation, particleSystem.transform.lossyScale);
				}
				bool flag = shapeType == ParticleSystemShapeType.Box || shapeType == ParticleSystemShapeType.BoxShell || shapeType == ParticleSystemShapeType.BoxEdge || shapeType == ParticleSystemShapeType.Rectangle;
				Vector3 s = (!flag) ? shape.scale : Vector3.one;
				Matrix4x4 rhs = Matrix4x4.TRS(shape.position, Quaternion.Euler(shape.rotation), s);
				matrix4x *= rhs;
				Handles.matrix = matrix4x;
				if (shapeType == ParticleSystemShapeType.Sphere)
				{
					EditorGUI.BeginChangeCheck();
					float radius = Handles.DoSimpleRadiusHandle(Quaternion.identity, Vector3.zero, shape.radius, false);
					if (EditorGUI.EndChangeCheck())
					{
						Undo.RecordObject(particleSystem, "Sphere Handle Change");
						shape.radius = radius;
					}
					Matrix4x4 transform = matrix4x * Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one * shape.radius * 2f);
					this.OnSceneViewTextureGUI(shape, ShapeModuleUI.s_SphereMesh, false, ShapeModuleUI.s_SphereTextureMaterial, transform);
				}
				else if (shapeType == ParticleSystemShapeType.Circle)
				{
					EditorGUI.BeginChangeCheck();
					this.m_ArcHandle.radius = shape.radius;
					this.m_ArcHandle.angle = shape.arc;
					this.m_ArcHandle.SetColorWithRadiusHandle(Color.white, 0f);
					using (new Handles.DrawingScope(Handles.matrix * ShapeModuleUI.s_ArcHandleOffsetMatrix))
					{
						this.m_ArcHandle.DrawHandle();
					}
					if (EditorGUI.EndChangeCheck())
					{
						Undo.RecordObject(particleSystem, "Circle Handle Change");
						shape.radius = this.m_ArcHandle.radius;
						shape.arc = this.m_ArcHandle.angle;
					}
					Matrix4x4 transform2 = matrix4x * Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(90f, 0f, 180f), Vector3.one * shape.radius * 2f);
					this.OnSceneViewTextureGUI(shape, ShapeModuleUI.s_CircleMesh, true, ShapeModuleUI.s_TextureMaterial, transform2);
				}
				else if (shapeType == ParticleSystemShapeType.Hemisphere)
				{
					EditorGUI.BeginChangeCheck();
					float radius2 = Handles.DoSimpleRadiusHandle(Quaternion.identity, Vector3.zero, shape.radius, true);
					if (EditorGUI.EndChangeCheck())
					{
						Undo.RecordObject(particleSystem, "Hemisphere Handle Change");
						shape.radius = radius2;
					}
					Matrix4x4 transform3 = matrix4x * Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one * shape.radius * 2f);
					this.OnSceneViewTextureGUI(shape, ShapeModuleUI.s_HemisphereMesh, false, ShapeModuleUI.s_SphereTextureMaterial, transform3);
				}
				else if (shapeType == ParticleSystemShapeType.Cone)
				{
					EditorGUI.BeginChangeCheck();
					Vector3 radiusAngleRange = new Vector3(shape.radius, shape.angle, main.startSpeedMultiplier);
					radiusAngleRange = Handles.ConeFrustrumHandle(Quaternion.identity, Vector3.zero, radiusAngleRange);
					if (EditorGUI.EndChangeCheck())
					{
						Undo.RecordObject(particleSystem, "Cone Handle Change");
						shape.radius = radiusAngleRange.x;
						shape.angle = radiusAngleRange.y;
						main.startSpeedMultiplier = radiusAngleRange.z;
					}
					Matrix4x4 transform4 = matrix4x * Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(90f, 0f, 180f), Vector3.one * shape.radius * 2f);
					this.OnSceneViewTextureGUI(shape, ShapeModuleUI.s_CircleMesh, true, ShapeModuleUI.s_TextureMaterial, transform4);
				}
				else if (shapeType == ParticleSystemShapeType.ConeVolume)
				{
					EditorGUI.BeginChangeCheck();
					Vector3 radiusAngleRange2 = new Vector3(shape.radius, shape.angle, shape.length);
					radiusAngleRange2 = Handles.ConeFrustrumHandle(Quaternion.identity, Vector3.zero, radiusAngleRange2);
					if (EditorGUI.EndChangeCheck())
					{
						Undo.RecordObject(particleSystem, "Cone Volume Handle Change");
						shape.radius = radiusAngleRange2.x;
						shape.angle = radiusAngleRange2.y;
						shape.length = radiusAngleRange2.z;
					}
					Matrix4x4 transform5 = matrix4x * Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(90f, 0f, 180f), Vector3.one * shape.radius * 2f);
					this.OnSceneViewTextureGUI(shape, ShapeModuleUI.s_CircleMesh, true, ShapeModuleUI.s_TextureMaterial, transform5);
				}
				else if (shapeType == ParticleSystemShapeType.Box || shapeType == ParticleSystemShapeType.BoxShell || shapeType == ParticleSystemShapeType.BoxEdge)
				{
					EditorGUI.BeginChangeCheck();
					this.m_BoxBoundsHandle.center = Vector3.zero;
					this.m_BoxBoundsHandle.size = shape.scale;
					this.m_BoxBoundsHandle.DrawHandle();
					if (EditorGUI.EndChangeCheck())
					{
						Undo.RecordObject(particleSystem, "Box Handle Change");
						shape.scale = this.m_BoxBoundsHandle.size;
					}
					Matrix4x4 transform6 = matrix4x * Matrix4x4.TRS(new Vector3(0f, 0f, -this.m_BoxBoundsHandle.size.z * 0.5f), Quaternion.identity, this.m_BoxBoundsHandle.size);
					this.OnSceneViewTextureGUI(shape, ShapeModuleUI.s_QuadMesh, true, ShapeModuleUI.s_TextureMaterial, transform6);
				}
				else if (shapeType == ParticleSystemShapeType.Donut)
				{
					EditorGUI.BeginChangeCheck();
					this.m_ArcHandle.radius = shape.radius;
					this.m_ArcHandle.angle = shape.arc;
					this.m_ArcHandle.SetColorWithRadiusHandle(Color.white, 0f);
					this.m_ArcHandle.wireframeColor = Color.clear;
					using (new Handles.DrawingScope(Handles.matrix * ShapeModuleUI.s_ArcHandleOffsetMatrix))
					{
						this.m_ArcHandle.DrawHandle();
					}
					if (EditorGUI.EndChangeCheck())
					{
						Undo.RecordObject(particleSystem, "Donut Handle Change");
						shape.radius = this.m_ArcHandle.radius;
						shape.arc = this.m_ArcHandle.angle;
					}
					using (new Handles.DrawingScope(Handles.matrix * ShapeModuleUI.s_ArcHandleOffsetMatrix))
					{
						float num = shape.arc % 360f;
						float angle = (Mathf.Abs(shape.arc) < 360f) ? num : 360f;
						Handles.DrawWireArc(new Vector3(0f, shape.donutRadius, 0f), Vector3.up, Vector3.forward, angle, shape.radius);
						Handles.DrawWireArc(new Vector3(0f, -shape.donutRadius, 0f), Vector3.up, Vector3.forward, angle, shape.radius);
						Handles.DrawWireArc(Vector3.zero, Vector3.up, Vector3.forward, angle, shape.radius + shape.donutRadius);
						Handles.DrawWireArc(Vector3.zero, Vector3.up, Vector3.forward, angle, shape.radius - shape.donutRadius);
						if (shape.arc != 360f)
						{
							Quaternion rotation = Quaternion.AngleAxis(shape.arc, Vector3.up);
							Vector3 center = rotation * Vector3.forward * shape.radius;
							Handles.DrawWireDisc(center, rotation * Vector3.right, shape.donutRadius);
						}
					}
					this.m_SphereBoundsHandle.axes = (PrimitiveBoundsHandle.Axes.X | PrimitiveBoundsHandle.Axes.Y);
					this.m_SphereBoundsHandle.radius = shape.donutRadius;
					this.m_SphereBoundsHandle.center = Vector3.zero;
					this.m_SphereBoundsHandle.SetColor(Color.white);
					float num2 = 90f;
					int num3 = Mathf.Max(1, (int)Mathf.Ceil(shape.arc / num2));
					Matrix4x4 rhs2 = Matrix4x4.TRS(new Vector3(shape.radius, 0f, 0f), Quaternion.Euler(90f, 0f, 0f), Vector3.one);
					for (int j = 0; j < num3; j++)
					{
						EditorGUI.BeginChangeCheck();
						using (new Handles.DrawingScope(Handles.matrix * (Matrix4x4.Rotate(Quaternion.Euler(0f, 0f, num2 * (float)j)) * rhs2)))
						{
							this.m_SphereBoundsHandle.DrawHandle();
						}
						if (EditorGUI.EndChangeCheck())
						{
							Undo.RecordObject(particleSystem, "Donut Radius Handle Change");
							shape.donutRadius = this.m_SphereBoundsHandle.radius;
						}
					}
					Matrix4x4 transform7 = matrix4x * Matrix4x4.TRS(new Vector3(shape.radius, 0f, 0f), Quaternion.Euler(180f, 0f, 180f), Vector3.one * shape.donutRadius * 2f);
					this.OnSceneViewTextureGUI(shape, ShapeModuleUI.s_CircleMesh, true, ShapeModuleUI.s_TextureMaterial, transform7);
				}
				else if (shapeType == ParticleSystemShapeType.SingleSidedEdge)
				{
					EditorGUI.BeginChangeCheck();
					float radius3 = Handles.DoSimpleEdgeHandle(Quaternion.identity, Vector3.zero, shape.radius);
					if (EditorGUI.EndChangeCheck())
					{
						Undo.RecordObject(particleSystem, "Edge Handle Change");
						shape.radius = radius3;
					}
				}
				else if (shapeType == ParticleSystemShapeType.Mesh)
				{
					Mesh mesh = shape.mesh;
					if (mesh)
					{
						bool wireframe = GL.wireframe;
						GL.wireframe = true;
						ShapeModuleUI.s_Material.SetPass(0);
						Graphics.DrawMeshNow(mesh, matrix4x);
						GL.wireframe = wireframe;
						this.OnSceneViewTextureGUI(shape, mesh, false, ShapeModuleUI.s_TextureMaterial, matrix4x);
					}
				}
				else if (shapeType == ParticleSystemShapeType.Rectangle)
				{
					EditorGUI.BeginChangeCheck();
					this.m_BoxBoundsHandle.center = Vector3.zero;
					this.m_BoxBoundsHandle.size = new Vector3(shape.scale.x, shape.scale.y, 0f);
					this.m_BoxBoundsHandle.DrawHandle();
					if (EditorGUI.EndChangeCheck())
					{
						Undo.RecordObject(particleSystem, "Rectangle Handle Change");
						shape.scale = new Vector3(this.m_BoxBoundsHandle.size.x, this.m_BoxBoundsHandle.size.y, 0f);
					}
					this.OnSceneViewTextureGUI(shape, ShapeModuleUI.s_QuadMesh, true, ShapeModuleUI.s_TextureMaterial, matrix4x * Matrix4x4.Scale(this.m_BoxBoundsHandle.size));
				}
			}
			if (EditorGUI.EndChangeCheck())
			{
				this.m_ParticleSystemUI.m_ParticleEffectUI.m_Owner.Repaint();
			}
			Handles.color = color;
			Handles.matrix = matrix;
		}

		private void OnSceneViewTextureGUI(ParticleSystem.ShapeModule shapeModule, Mesh mesh, bool twoSided, Material mat, Matrix4x4 transform)
		{
			Texture texture = shapeModule.texture;
			if (texture)
			{
				mat.SetPass(0);
				mat.SetTexture("_MainTex", texture);
				mat.SetColor("_Color", new Color(1f, 1f, 1f, 0.4f));
				mat.SetFloat("_ClipChannel", (float)shapeModule.textureClipChannel);
				mat.SetFloat("_ClipThreshold", shapeModule.textureClipThreshold);
				mat.SetFloat("_Cull", (!twoSided) ? 2f : 0f);
				mat.SetFloat("_UVChannel", (float)shapeModule.textureUVChannel);
				Graphics.DrawMeshNow(mesh, transform);
			}
		}

		public override void UpdateCullingSupportedString(ref string text)
		{
			this.Init();
			if (this.m_Arc.m_Mode.intValue != 0 || this.m_Radius.m_Mode.intValue != 0)
			{
				text += "\n\tAnimated shape emission is enabled.";
			}
		}
	}
}
