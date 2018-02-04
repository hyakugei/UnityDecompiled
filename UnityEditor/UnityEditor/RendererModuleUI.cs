using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

namespace UnityEditor
{
	internal class RendererModuleUI : ModuleUI
	{
		private enum RenderMode
		{
			Billboard,
			Stretch3D,
			BillboardFixedHorizontal,
			BillboardFixedVertical,
			Mesh,
			None
		}

		private class Texts
		{
			public GUIContent renderMode = EditorGUIUtility.TrTextContent("Render Mode", "Defines the render mode of the particle renderer.", null);

			public GUIContent material = EditorGUIUtility.TrTextContent("Material", "Defines the material used to render particles.", null);

			public GUIContent trailMaterial = EditorGUIUtility.TrTextContent("Trail Material", "Defines the material used to render particle trails.", null);

			public GUIContent mesh = EditorGUIUtility.TrTextContent("Mesh", "Defines the mesh that will be rendered as particle.", null);

			public GUIContent minParticleSize = EditorGUIUtility.TrTextContent("Min Particle Size", "How small is a particle allowed to be on screen at least? 1 is entire viewport. 0.5 is half viewport.", null);

			public GUIContent maxParticleSize = EditorGUIUtility.TrTextContent("Max Particle Size", "How large is a particle allowed to be on screen at most? 1 is entire viewport. 0.5 is half viewport.", null);

			public GUIContent cameraSpeedScale = EditorGUIUtility.TrTextContent("Camera Scale", "How much the camera speed is factored in when determining particle stretching.", null);

			public GUIContent speedScale = EditorGUIUtility.TrTextContent("Speed Scale", "Defines the length of the particle compared to its speed.", null);

			public GUIContent lengthScale = EditorGUIUtility.TrTextContent("Length Scale", "Defines the length of the particle compared to its width.", null);

			public GUIContent sortingFudge = EditorGUIUtility.TrTextContent("Sorting Fudge", "Lower the number and most likely these particles will appear in front of other transparent objects, including other particles.", null);

			public GUIContent sortMode = EditorGUIUtility.TrTextContent("Sort Mode", "The draw order of particles can be sorted by distance, oldest in front, or youngest in front.", null);

			public GUIContent rotation = EditorGUIUtility.TrTextContent("Rotation", "Set whether the rotation of the particles is defined in Screen or World space.", null);

			public GUIContent castShadows = EditorGUIUtility.TrTextContent("Cast Shadows", "Only opaque materials cast shadows", null);

			public GUIContent receiveShadows = EditorGUIUtility.TrTextContent("Receive Shadows", "Only opaque materials receive shadows. When using deferred rendering, all opaque objects receive shadows.", null);

			public GUIContent motionVectors = EditorGUIUtility.TrTextContent("Motion Vectors", "Specifies whether the Particle System renders 'Per Object Motion', 'Camera Motion', or 'No Motion' vectors to the Camera Motion Vector Texture. Note that there is no built-in support for Per-Particle Motion.", null);

			public GUIContent normalDirection = EditorGUIUtility.TrTextContent("Normal Direction", "Value between 0.0 and 1.0. If 1.0 is used, normals will point towards camera. If 0.0 is used, normals will point out in the corner direction of the particle.", null);

			public GUIContent sortingLayer = EditorGUIUtility.TrTextContent("Sorting Layer", "Name of the Renderer's sorting layer.", null);

			public GUIContent sortingOrder = EditorGUIUtility.TrTextContent("Order in Layer", "Renderer's order within a sorting layer", null);

			public GUIContent space = EditorGUIUtility.TrTextContent("Render Alignment", "Specifies if the particles will face the camera, align to world axes, or stay local to the system's transform.", null);

			public GUIContent pivot = EditorGUIUtility.TrTextContent("Pivot", "Applies an offset to the pivot of particles, as a multiplier of its size.", null);

			public GUIContent visualizePivot = EditorGUIUtility.TrTextContent("Visualize Pivot", "Render the pivot positions of the particles.", null);

			public GUIContent useCustomVertexStreams = EditorGUIUtility.TrTextContent("Custom Vertex Streams", "Choose whether to send custom particle data to the shader.", null);

			public GUIContent enableGPUInstancing = EditorGUIUtility.TrTextContent("Enable GPU Instancing", "Use GPU Instancing on platforms where it is supported, and when using shaders that contain a Procedural Instancing pass (#pragma instancing_options procedural).", null);

			public GUIContent[] particleTypes = new GUIContent[]
			{
				EditorGUIUtility.TrTextContent("Billboard", null, null),
				EditorGUIUtility.TrTextContent("Stretched Billboard", null, null),
				EditorGUIUtility.TrTextContent("Horizontal Billboard", null, null),
				EditorGUIUtility.TrTextContent("Vertical Billboard", null, null),
				EditorGUIUtility.TrTextContent("Mesh", null, null),
				EditorGUIUtility.TrTextContent("None", null, null)
			};

			public GUIContent[] sortTypes = new GUIContent[]
			{
				EditorGUIUtility.TrTextContent("None", null, null),
				EditorGUIUtility.TrTextContent("By Distance", null, null),
				EditorGUIUtility.TrTextContent("Oldest in Front", null, null),
				EditorGUIUtility.TrTextContent("Youngest in Front", null, null)
			};

			public GUIContent[] spaces = new GUIContent[]
			{
				EditorGUIUtility.TrTextContent("View", null, null),
				EditorGUIUtility.TrTextContent("World", null, null),
				EditorGUIUtility.TrTextContent("Local", null, null),
				EditorGUIUtility.TrTextContent("Facing", null, null),
				EditorGUIUtility.TrTextContent("Velocity", null, null)
			};

			public GUIContent[] localSpace = new GUIContent[]
			{
				EditorGUIUtility.TrTextContent("Local", null, null)
			};

			public GUIContent[] motionVectorOptions = new GUIContent[]
			{
				EditorGUIUtility.TrTextContent("Camera Motion Only", null, null),
				EditorGUIUtility.TrTextContent("Per Object Motion", null, null),
				EditorGUIUtility.TrTextContent("Force No Motion", null, null)
			};

			public GUIContent maskingMode = EditorGUIUtility.TrTextContent("Masking", "Defines the masking behavior of the particles. See Sprite Masking documentation for more details.", null);

			public GUIContent[] maskInteractions = new GUIContent[]
			{
				EditorGUIUtility.TrTextContent("No Masking", null, null),
				EditorGUIUtility.TrTextContent("Visible Inside Mask", null, null),
				EditorGUIUtility.TrTextContent("Visible Outside Mask", null, null)
			};

			private string[] vertexStreamsMenu = new string[]
			{
				"Position",
				"Normal",
				"Tangent",
				"Color",
				"UV/UV1",
				"UV/UV2",
				"UV/UV3",
				"UV/UV4",
				"UV/AnimBlend",
				"UV/AnimFrame",
				"Center",
				"VertexID",
				"Size/Size.x",
				"Size/Size.xy",
				"Size/Size.xyz",
				"Rotation/Rotation",
				"Rotation/Rotation3D",
				"Rotation/RotationSpeed",
				"Rotation/RotationSpeed3D",
				"Velocity",
				"Speed",
				"Lifetime/AgePercent",
				"Lifetime/InverseStartLifetime",
				"Random/Stable.x",
				"Random/Stable.xy",
				"Random/Stable.xyz",
				"Random/Stable.xyzw",
				"Random/Varying.x",
				"Random/Varying.xy",
				"Random/Varying.xyz",
				"Random/Varying.xyzw",
				"Custom/Custom1.x",
				"Custom/Custom1.xy",
				"Custom/Custom1.xyz",
				"Custom/Custom1.xyzw",
				"Custom/Custom2.x",
				"Custom/Custom2.xy",
				"Custom/Custom2.xyz",
				"Custom/Custom2.xyzw",
				"Noise/Sum.x",
				"Noise/Sum.xy",
				"Noise/Sum.xyz",
				"Noise/Impulse.x",
				"Noise/Impulse.xy",
				"Noise/Impulse.xyz"
			};

			public string[] vertexStreamsPacked = new string[]
			{
				"Position",
				"Normal",
				"Tangent",
				"Color",
				"UV",
				"UV2",
				"UV3",
				"UV4",
				"AnimBlend",
				"AnimFrame",
				"Center",
				"VertexID",
				"Size",
				"Size.xy",
				"Size.xyz",
				"Rotation",
				"Rotation3D",
				"RotationSpeed",
				"RotationSpeed3D",
				"Velocity",
				"Speed",
				"AgePercent",
				"InverseStartLifetime",
				"StableRandom.x",
				"StableRandom.xy",
				"StableRandom.xyz",
				"StableRandom.xyzw",
				"VariableRandom.x",
				"VariableRandom.xy",
				"VariableRandom.xyz",
				"VariableRandom.xyzw",
				"Custom1.x",
				"Custom1.xy",
				"Custom1.xyz",
				"Custom1.xyzw",
				"Custom2.x",
				"Custom2.xy",
				"Custom2.xyz",
				"Custom2.xyzw",
				"NoiseSum.x",
				"NoiseSum.xy",
				"NoiseSum.xyz",
				"NoiseImpulse.x",
				"NoiseImpulse.xy",
				"NoiseImpulse.xyz"
			};

			public string[] vertexStreamPackedTypes = new string[]
			{
				"POSITION.xyz",
				"NORMAL.xyz",
				"TANGENT.xyzw",
				"COLOR.xyzw"
			};

			public int[] vertexStreamTexCoordChannels = new int[]
			{
				0,
				0,
				0,
				0,
				2,
				2,
				2,
				2,
				1,
				1,
				3,
				1,
				1,
				2,
				3,
				1,
				3,
				1,
				3,
				3,
				1,
				1,
				1,
				1,
				2,
				3,
				4,
				1,
				2,
				3,
				4,
				1,
				2,
				3,
				4,
				1,
				2,
				3,
				4,
				1,
				2,
				3,
				1,
				2,
				3
			};

			public string channels = "xyzw|xyz";

			public int vertexStreamsInstancedStart = 8;

			public GUIContent[] vertexStreamsMenuContent;

			public Texts()
			{
				this.vertexStreamsMenuContent = (from x in this.vertexStreamsMenu
				select new GUIContent(x)).ToArray<GUIContent>();
			}
		}

		private class StreamCallbackData
		{
			public ReorderableList list;

			public SerializedProperty streamProp;

			public int stream;

			public StreamCallbackData(ReorderableList l, SerializedProperty prop, int s)
			{
				this.list = l;
				this.streamProp = prop;
				this.stream = s;
			}
		}

		private const int k_MaxNumMeshes = 4;

		private SerializedProperty m_CastShadows;

		private SerializedProperty m_ReceiveShadows;

		private SerializedProperty m_MotionVectors;

		private SerializedProperty m_Material;

		private SerializedProperty m_TrailMaterial;

		private SerializedProperty m_SortingOrder;

		private SerializedProperty m_SortingLayerID;

		private SerializedProperty m_RenderingLayerMask;

		private SerializedProperty m_RenderMode;

		private SerializedProperty[] m_Meshes = new SerializedProperty[4];

		private SerializedProperty[] m_ShownMeshes;

		private SerializedProperty m_MinParticleSize;

		private SerializedProperty m_MaxParticleSize;

		private SerializedProperty m_CameraVelocityScale;

		private SerializedProperty m_VelocityScale;

		private SerializedProperty m_LengthScale;

		private SerializedProperty m_SortMode;

		private SerializedProperty m_SortingFudge;

		private SerializedProperty m_NormalDirection;

		private RendererEditorBase.Probes m_Probes;

		private SerializedProperty m_RenderAlignment;

		private SerializedProperty m_Pivot;

		private SerializedProperty m_UseCustomVertexStreams;

		private SerializedProperty m_VertexStreams;

		private SerializedProperty m_MaskInteraction;

		private SerializedProperty m_EnableGPUInstancing;

		private ReorderableList m_VertexStreamsList;

		private int m_NumTexCoords;

		private int m_TexCoordChannelIndex;

		private int m_NumInstancedStreams;

		private bool m_HasTangent;

		private bool m_HasColor;

		private bool m_HasGPUInstancing;

		private static bool s_VisualizePivot = false;

		private static RendererModuleUI.Texts s_Texts;

		public RendererModuleUI(ParticleSystemUI owner, SerializedObject o, string displayName) : base(owner, o, "ParticleSystemRenderer", displayName, ModuleUI.VisibilityState.VisibleAndFolded)
		{
			this.m_ToolTip = "Specifies how the particles are rendered.";
		}

		public override bool DrawHeader(Rect rect, GUIContent label)
		{
			bool boldDefaultFont = EditorGUIUtility.GetBoldDefaultFont();
			SerializedProperty iterator = this.m_Object.GetIterator();
			bool boldDefaultFont2 = false;
			if (this.m_Object.targetObjects.Length == 1)
			{
				bool flag = iterator.Next(true);
				while (flag)
				{
					if (iterator.isInstantiatedPrefab && iterator.prefabOverride)
					{
						boldDefaultFont2 = true;
						break;
					}
					flag = iterator.Next(false);
				}
			}
			EditorGUIUtility.SetBoldDefaultFont(boldDefaultFont2);
			bool result = GUI.Toggle(rect, base.foldout, label, ParticleSystemStyles.Get().moduleHeaderStyle);
			EditorGUIUtility.SetBoldDefaultFont(boldDefaultFont);
			return result;
		}

		protected override void Init()
		{
			if (this.m_CastShadows == null)
			{
				if (RendererModuleUI.s_Texts == null)
				{
					RendererModuleUI.s_Texts = new RendererModuleUI.Texts();
				}
				this.m_CastShadows = base.GetProperty0("m_CastShadows");
				this.m_ReceiveShadows = base.GetProperty0("m_ReceiveShadows");
				this.m_MotionVectors = base.GetProperty0("m_MotionVectors");
				this.m_Material = base.GetProperty0("m_Materials.Array.data[0]");
				this.m_TrailMaterial = base.GetProperty0("m_Materials.Array.data[1]");
				this.m_SortingOrder = base.GetProperty0("m_SortingOrder");
				this.m_RenderingLayerMask = base.GetProperty0("m_RenderingLayerMask");
				this.m_SortingLayerID = base.GetProperty0("m_SortingLayerID");
				this.m_RenderMode = base.GetProperty0("m_RenderMode");
				this.m_MinParticleSize = base.GetProperty0("m_MinParticleSize");
				this.m_MaxParticleSize = base.GetProperty0("m_MaxParticleSize");
				this.m_CameraVelocityScale = base.GetProperty0("m_CameraVelocityScale");
				this.m_VelocityScale = base.GetProperty0("m_VelocityScale");
				this.m_LengthScale = base.GetProperty0("m_LengthScale");
				this.m_SortingFudge = base.GetProperty0("m_SortingFudge");
				this.m_SortMode = base.GetProperty0("m_SortMode");
				this.m_NormalDirection = base.GetProperty0("m_NormalDirection");
				this.m_Probes = new RendererEditorBase.Probes();
				this.m_Probes.Initialize(base.serializedObject);
				this.m_RenderAlignment = base.GetProperty0("m_RenderAlignment");
				this.m_Pivot = base.GetProperty0("m_Pivot");
				this.m_Meshes[0] = base.GetProperty0("m_Mesh");
				this.m_Meshes[1] = base.GetProperty0("m_Mesh1");
				this.m_Meshes[2] = base.GetProperty0("m_Mesh2");
				this.m_Meshes[3] = base.GetProperty0("m_Mesh3");
				List<SerializedProperty> list = new List<SerializedProperty>();
				for (int i = 0; i < this.m_Meshes.Length; i++)
				{
					if (i == 0 || this.m_Meshes[i].objectReferenceValue != null)
					{
						list.Add(this.m_Meshes[i]);
					}
				}
				this.m_ShownMeshes = list.ToArray();
				this.m_MaskInteraction = base.GetProperty0("m_MaskInteraction");
				this.m_EnableGPUInstancing = base.GetProperty0("m_EnableGPUInstancing");
				this.m_UseCustomVertexStreams = base.GetProperty0("m_UseCustomVertexStreams");
				this.m_VertexStreams = base.GetProperty0("m_VertexStreams");
				this.m_VertexStreamsList = new ReorderableList(base.serializedObject, this.m_VertexStreams, true, true, true, true);
				this.m_VertexStreamsList.elementHeight = 16f;
				this.m_VertexStreamsList.headerHeight = 0f;
				this.m_VertexStreamsList.onAddDropdownCallback = new ReorderableList.AddDropdownCallbackDelegate(this.OnVertexStreamListAddDropdownCallback);
				this.m_VertexStreamsList.onCanRemoveCallback = new ReorderableList.CanRemoveCallbackDelegate(this.OnVertexStreamListCanRemoveCallback);
				this.m_VertexStreamsList.drawElementCallback = new ReorderableList.ElementCallbackDelegate(this.DrawVertexStreamListElementCallback);
				RendererModuleUI.s_VisualizePivot = EditorPrefs.GetBool("VisualizePivot", false);
			}
		}

		public override void OnInspectorGUI(InitialModuleUI initial)
		{
			EditorGUI.BeginChangeCheck();
			RendererModuleUI.RenderMode renderMode = (RendererModuleUI.RenderMode)ModuleUI.GUIPopup(RendererModuleUI.s_Texts.renderMode, this.m_RenderMode, RendererModuleUI.s_Texts.particleTypes, new GUILayoutOption[0]);
			bool flag = EditorGUI.EndChangeCheck();
			if (!this.m_RenderMode.hasMultipleDifferentValues)
			{
				if (renderMode == RendererModuleUI.RenderMode.Mesh)
				{
					EditorGUI.indentLevel++;
					this.DoListOfMeshesGUI();
					EditorGUI.indentLevel--;
					if (flag && this.m_Meshes[0].objectReferenceInstanceIDValue == 0 && !this.m_Meshes[0].hasMultipleDifferentValues)
					{
						this.m_Meshes[0].objectReferenceValue = Resources.GetBuiltinResource(typeof(Mesh), "Cube.fbx");
					}
				}
				else if (renderMode == RendererModuleUI.RenderMode.Stretch3D)
				{
					EditorGUI.indentLevel++;
					ModuleUI.GUIFloat(RendererModuleUI.s_Texts.cameraSpeedScale, this.m_CameraVelocityScale, new GUILayoutOption[0]);
					ModuleUI.GUIFloat(RendererModuleUI.s_Texts.speedScale, this.m_VelocityScale, new GUILayoutOption[0]);
					ModuleUI.GUIFloat(RendererModuleUI.s_Texts.lengthScale, this.m_LengthScale, new GUILayoutOption[0]);
					EditorGUI.indentLevel--;
				}
				if (renderMode != RendererModuleUI.RenderMode.None)
				{
					if (renderMode != RendererModuleUI.RenderMode.Mesh)
					{
						ModuleUI.GUIFloat(RendererModuleUI.s_Texts.normalDirection, this.m_NormalDirection, new GUILayoutOption[0]);
					}
					if (this.m_Material != null)
					{
						ModuleUI.GUIObject(RendererModuleUI.s_Texts.material, this.m_Material, new GUILayoutOption[0]);
					}
				}
			}
			if (this.m_TrailMaterial != null)
			{
				ModuleUI.GUIObject(RendererModuleUI.s_Texts.trailMaterial, this.m_TrailMaterial, new GUILayoutOption[0]);
			}
			if (renderMode != RendererModuleUI.RenderMode.None)
			{
				if (!this.m_RenderMode.hasMultipleDifferentValues)
				{
					ModuleUI.GUIPopup(RendererModuleUI.s_Texts.sortMode, this.m_SortMode, RendererModuleUI.s_Texts.sortTypes, new GUILayoutOption[0]);
					ModuleUI.GUIFloat(RendererModuleUI.s_Texts.sortingFudge, this.m_SortingFudge, new GUILayoutOption[0]);
					if (renderMode != RendererModuleUI.RenderMode.Mesh)
					{
						ModuleUI.GUIFloat(RendererModuleUI.s_Texts.minParticleSize, this.m_MinParticleSize, new GUILayoutOption[0]);
						ModuleUI.GUIFloat(RendererModuleUI.s_Texts.maxParticleSize, this.m_MaxParticleSize, new GUILayoutOption[0]);
					}
					if (renderMode == RendererModuleUI.RenderMode.Billboard || renderMode == RendererModuleUI.RenderMode.Mesh)
					{
						bool flag2 = this.m_ParticleSystemUI.m_ParticleSystems.FirstOrDefault((ParticleSystem o) => o.shape.alignToDirection) != null;
						if (flag2)
						{
							using (new EditorGUI.DisabledScope(true))
							{
								ModuleUI.GUIPopup(RendererModuleUI.s_Texts.space, 0, RendererModuleUI.s_Texts.localSpace, new GUILayoutOption[0]);
							}
							GUIContent gUIContent = EditorGUIUtility.TrTextContent("Using Align to Direction in the Shape Module forces the system to be rendered using Local Render Alignment.", null, null);
							EditorGUILayout.HelpBox(gUIContent.text, MessageType.Info, true);
						}
						else
						{
							ModuleUI.GUIPopup(RendererModuleUI.s_Texts.space, this.m_RenderAlignment, RendererModuleUI.s_Texts.spaces, new GUILayoutOption[0]);
						}
					}
					if (renderMode == RendererModuleUI.RenderMode.Mesh)
					{
						ModuleUI.GUIToggle(RendererModuleUI.s_Texts.enableGPUInstancing, this.m_EnableGPUInstancing, new GUILayoutOption[0]);
					}
				}
				ModuleUI.GUIVector3Field(RendererModuleUI.s_Texts.pivot, this.m_Pivot, new GUILayoutOption[0]);
				EditorGUI.BeginChangeCheck();
				RendererModuleUI.s_VisualizePivot = ModuleUI.GUIToggle(RendererModuleUI.s_Texts.visualizePivot, RendererModuleUI.s_VisualizePivot, new GUILayoutOption[0]);
				if (EditorGUI.EndChangeCheck())
				{
					EditorPrefs.SetBool("VisualizePivot", RendererModuleUI.s_VisualizePivot);
				}
				ModuleUI.GUIPopup(RendererModuleUI.s_Texts.maskingMode, this.m_MaskInteraction, RendererModuleUI.s_Texts.maskInteractions, new GUILayoutOption[0]);
				if (ModuleUI.GUIToggle(RendererModuleUI.s_Texts.useCustomVertexStreams, this.m_UseCustomVertexStreams, new GUILayoutOption[0]))
				{
					this.DoVertexStreamsGUI(renderMode);
				}
			}
			EditorGUILayout.Space();
			ModuleUI.GUIPopup(RendererModuleUI.s_Texts.castShadows, this.m_CastShadows, EditorGUIUtility.TempContent(this.m_CastShadows.enumDisplayNames), new GUILayoutOption[0]);
			if (SupportedRenderingFeatures.active.rendererSupportsReceiveShadows)
			{
				if (SceneView.IsUsingDeferredRenderingPath())
				{
					using (new EditorGUI.DisabledScope(true))
					{
						ModuleUI.GUIToggle(RendererModuleUI.s_Texts.receiveShadows, true, new GUILayoutOption[0]);
					}
				}
				else
				{
					ModuleUI.GUIToggle(RendererModuleUI.s_Texts.receiveShadows, this.m_ReceiveShadows, new GUILayoutOption[0]);
				}
			}
			if (SupportedRenderingFeatures.active.rendererSupportsMotionVectors)
			{
				ModuleUI.GUIPopup(RendererModuleUI.s_Texts.motionVectors, this.m_MotionVectors, RendererModuleUI.s_Texts.motionVectorOptions, new GUILayoutOption[0]);
			}
			ModuleUI.GUISortingLayerField(RendererModuleUI.s_Texts.sortingLayer, this.m_SortingLayerID, new GUILayoutOption[0]);
			ModuleUI.GUIInt(RendererModuleUI.s_Texts.sortingOrder, this.m_SortingOrder, new GUILayoutOption[0]);
			List<ParticleSystemRenderer> list = new List<ParticleSystemRenderer>();
			ParticleSystem[] particleSystems = this.m_ParticleSystemUI.m_ParticleSystems;
			for (int i = 0; i < particleSystems.Length; i++)
			{
				ParticleSystem particleSystem = particleSystems[i];
				list.Add(particleSystem.GetComponent<ParticleSystemRenderer>());
			}
			ParticleSystemRenderer[] array = list.ToArray();
			this.m_Probes.OnGUI(array, list.FirstOrDefault<ParticleSystemRenderer>(), true);
			RendererEditorBase.RenderRenderingLayer(this.m_RenderingLayerMask, base.serializedObject.targetObject as Renderer, array, true);
		}

		private void DoListOfMeshesGUI()
		{
			base.GUIListOfFloatObjectToggleFields(RendererModuleUI.s_Texts.mesh, this.m_ShownMeshes, null, null, false, new GUILayoutOption[0]);
			Rect rect = GUILayoutUtility.GetRect(0f, 13f);
			rect.x = rect.xMax - 24f - 5f;
			rect.width = 12f;
			if (this.m_ShownMeshes.Length > 1)
			{
				if (ModuleUI.MinusButton(rect))
				{
					this.m_ShownMeshes[this.m_ShownMeshes.Length - 1].objectReferenceValue = null;
					List<SerializedProperty> list = new List<SerializedProperty>(this.m_ShownMeshes);
					list.RemoveAt(list.Count - 1);
					this.m_ShownMeshes = list.ToArray();
				}
			}
			if (this.m_ShownMeshes.Length < 4 && !this.m_ParticleSystemUI.multiEdit)
			{
				rect.x += 17f;
				if (ModuleUI.PlusButton(rect))
				{
					List<SerializedProperty> list2 = new List<SerializedProperty>(this.m_ShownMeshes);
					list2.Add(this.m_Meshes[list2.Count]);
					this.m_ShownMeshes = list2.ToArray();
				}
			}
		}

		private void SelectVertexStreamCallback(object obj)
		{
			RendererModuleUI.StreamCallbackData streamCallbackData = (RendererModuleUI.StreamCallbackData)obj;
			ReorderableList.defaultBehaviours.DoAddButton(streamCallbackData.list);
			SerializedProperty arrayElementAtIndex = streamCallbackData.streamProp.GetArrayElementAtIndex(streamCallbackData.list.index);
			arrayElementAtIndex.intValue = streamCallbackData.stream;
			this.m_ParticleSystemUI.m_RendererSerializedObject.ApplyModifiedProperties();
		}

		private void DoVertexStreamsGUI(RendererModuleUI.RenderMode renderMode)
		{
			ParticleSystemRenderer component = this.m_ParticleSystemUI.m_ParticleSystems[0].GetComponent<ParticleSystemRenderer>();
			this.m_NumTexCoords = 0;
			this.m_TexCoordChannelIndex = 0;
			this.m_NumInstancedStreams = 0;
			this.m_HasTangent = false;
			this.m_HasColor = false;
			this.m_HasGPUInstancing = (renderMode == RendererModuleUI.RenderMode.Mesh && component.supportsMeshInstancing);
			this.m_VertexStreamsList.DoLayoutList();
			if (!this.m_ParticleSystemUI.multiEdit)
			{
				string text = "";
				if (this.m_Material != null)
				{
					Material material = this.m_Material.objectReferenceValue as Material;
					int texCoordChannelCount = this.m_NumTexCoords * 4 + this.m_TexCoordChannelIndex;
					bool flag = false;
					bool flag2 = false;
					bool flag3 = false;
					bool flag4 = this.m_ParticleSystemUI.m_ParticleSystems[0].CheckVertexStreamsMatchShader(this.m_HasTangent, this.m_HasColor, texCoordChannelCount, material, ref flag, ref flag2, ref flag3);
					if (flag4)
					{
						text += "Vertex streams do not match the shader inputs. Particle systems may not render correctly. Ensure your streams match and are used by the shader.";
						if (flag)
						{
							text += "\n- TANGENT stream does not match.";
						}
						if (flag2)
						{
							text += "\n- COLOR stream does not match.";
						}
						if (flag3)
						{
							text += "\n- TEXCOORD streams do not match.";
						}
					}
				}
				int maxTexCoordStreams = this.m_ParticleSystemUI.m_ParticleSystems[0].GetMaxTexCoordStreams();
				if (this.m_NumTexCoords > maxTexCoordStreams || (this.m_NumTexCoords == maxTexCoordStreams && this.m_TexCoordChannelIndex > 0))
				{
					if (text != "")
					{
						text += "\n\n";
					}
					string text2 = text;
					text = string.Concat(new object[]
					{
						text2,
						"Only ",
						maxTexCoordStreams,
						" TEXCOORD streams are supported."
					});
				}
				if (renderMode == RendererModuleUI.RenderMode.Mesh)
				{
					Mesh[] array = new Mesh[4];
					int meshes = component.GetMeshes(array);
					for (int i = 0; i < meshes; i++)
					{
						if (array[i].HasChannel(Mesh.InternalShaderChannel.TexCoord2))
						{
							if (text != "")
							{
								text += "\n\n";
							}
							text += "Meshes may only use a maximum of 2 input UV streams.";
						}
					}
				}
				if (text != "")
				{
					GUIContent gUIContent = EditorGUIUtility.TextContent(text);
					EditorGUILayout.HelpBox(gUIContent.text, MessageType.Error, true);
				}
			}
		}

		private void OnVertexStreamListAddDropdownCallback(Rect rect, ReorderableList list)
		{
			List<int> list2 = new List<int>();
			for (int i = 0; i < RendererModuleUI.s_Texts.vertexStreamsPacked.Length; i++)
			{
				bool flag = false;
				for (int j = 0; j < this.m_VertexStreams.arraySize; j++)
				{
					if (this.m_VertexStreams.GetArrayElementAtIndex(j).intValue == i)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					list2.Add(i);
				}
			}
			GenericMenu genericMenu = new GenericMenu();
			for (int k = 0; k < list2.Count; k++)
			{
				genericMenu.AddItem(RendererModuleUI.s_Texts.vertexStreamsMenuContent[list2[k]], false, new GenericMenu.MenuFunction2(this.SelectVertexStreamCallback), new RendererModuleUI.StreamCallbackData(list, this.m_VertexStreams, list2[k]));
			}
			genericMenu.ShowAsContext();
			Event.current.Use();
		}

		private bool OnVertexStreamListCanRemoveCallback(ReorderableList list)
		{
			SerializedProperty arrayElementAtIndex = this.m_VertexStreams.GetArrayElementAtIndex(list.index);
			return RendererModuleUI.s_Texts.vertexStreamsPacked[arrayElementAtIndex.intValue] != "Position";
		}

		private void DrawVertexStreamListElementCallback(Rect rect, int index, bool isActive, bool isFocused)
		{
			SerializedProperty arrayElementAtIndex = this.m_VertexStreams.GetArrayElementAtIndex(index);
			int intValue = arrayElementAtIndex.intValue;
			string text = (!base.isWindowView) ? "TEXCOORD" : "TEX";
			string text2 = (!base.isWindowView) ? "INSTANCED" : "INST";
			int num = RendererModuleUI.s_Texts.vertexStreamTexCoordChannels[intValue];
			if (this.m_HasGPUInstancing && intValue >= RendererModuleUI.s_Texts.vertexStreamsInstancedStart)
			{
				string text3 = RendererModuleUI.s_Texts.channels.Substring(0, num);
				GUI.Label(rect, string.Concat(new object[]
				{
					RendererModuleUI.s_Texts.vertexStreamsPacked[intValue],
					" (",
					text2,
					this.m_NumInstancedStreams,
					".",
					text3,
					")"
				}), ParticleSystemStyles.Get().label);
				this.m_NumInstancedStreams++;
			}
			else if (num != 0)
			{
				int length = (this.m_TexCoordChannelIndex + num <= 4) ? num : (num + 1);
				string text4 = RendererModuleUI.s_Texts.channels.Substring(this.m_TexCoordChannelIndex, length);
				GUI.Label(rect, string.Concat(new object[]
				{
					RendererModuleUI.s_Texts.vertexStreamsPacked[intValue],
					" (",
					text,
					this.m_NumTexCoords,
					".",
					text4,
					")"
				}), ParticleSystemStyles.Get().label);
				this.m_TexCoordChannelIndex += num;
				if (this.m_TexCoordChannelIndex >= 4)
				{
					this.m_TexCoordChannelIndex -= 4;
					this.m_NumTexCoords++;
				}
			}
			else
			{
				GUI.Label(rect, RendererModuleUI.s_Texts.vertexStreamsPacked[intValue] + " (" + RendererModuleUI.s_Texts.vertexStreamPackedTypes[intValue] + ")", ParticleSystemStyles.Get().label);
				if (RendererModuleUI.s_Texts.vertexStreamsPacked[intValue] == "Tangent")
				{
					this.m_HasTangent = true;
				}
				if (RendererModuleUI.s_Texts.vertexStreamsPacked[intValue] == "Color")
				{
					this.m_HasColor = true;
				}
			}
		}

		public override void OnSceneViewGUI()
		{
			if (RendererModuleUI.s_VisualizePivot)
			{
				Color color = Handles.color;
				Handles.color = Color.green;
				Matrix4x4 matrix = Handles.matrix;
				Vector3[] array = new Vector3[6];
				ParticleSystem[] particleSystems = this.m_ParticleSystemUI.m_ParticleSystems;
				for (int i = 0; i < particleSystems.Length; i++)
				{
					ParticleSystem particleSystem = particleSystems[i];
					ParticleSystem.Particle[] array2 = new ParticleSystem.Particle[particleSystem.particleCount];
					int particles = particleSystem.GetParticles(array2);
					Matrix4x4 matrix2 = Matrix4x4.identity;
					if (particleSystem.main.simulationSpace == ParticleSystemSimulationSpace.Local)
					{
						matrix2 = particleSystem.GetLocalToWorldMatrix();
					}
					Handles.matrix = matrix2;
					for (int j = 0; j < particles; j++)
					{
						ParticleSystem.Particle particle = array2[j];
						Vector3 vector = particle.GetCurrentSize3D(particleSystem) * 0.05f;
						array[0] = particle.position - Vector3.right * vector.x;
						array[1] = particle.position + Vector3.right * vector.x;
						array[2] = particle.position - Vector3.up * vector.y;
						array[3] = particle.position + Vector3.up * vector.y;
						array[4] = particle.position - Vector3.forward * vector.z;
						array[5] = particle.position + Vector3.forward * vector.z;
						Handles.DrawLines(array);
					}
				}
				Handles.color = color;
				Handles.matrix = matrix;
			}
		}
	}
}
