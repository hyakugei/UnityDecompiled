using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
	internal class CollisionModuleUI : ModuleUI
	{
		private enum CollisionTypes
		{
			Plane,
			World
		}

		private enum CollisionModes
		{
			Mode3D,
			Mode2D
		}

		private enum ForceModes
		{
			None,
			Constant,
			SizeBased
		}

		private enum PlaneVizType
		{
			Grid,
			Solid
		}

		private class Texts
		{
			public GUIContent lifetimeLoss = EditorGUIUtility.TrTextContent("Lifetime Loss", "When particle collides, it will lose this fraction of its Start Lifetime", null);

			public GUIContent planes = EditorGUIUtility.TrTextContent("Planes", "Planes are defined by assigning a reference to a transform. This transform can be any transform in the scene and can be animated. Multiple planes can be used. Note: the Y-axis is used as the plane normal.", null);

			public GUIContent createPlane = EditorGUIUtility.TrTextContent("", "Create an empty GameObject and assign it as a plane.", null);

			public GUIContent minKillSpeed = EditorGUIUtility.TrTextContent("Min Kill Speed", "When particles collide and their speed is lower than this value, they are killed.", null);

			public GUIContent maxKillSpeed = EditorGUIUtility.TrTextContent("Max Kill Speed", "When particles collide and their speed is higher than this value, they are killed.", null);

			public GUIContent dampen = EditorGUIUtility.TrTextContent("Dampen", "When particle collides, it will lose this fraction of its speed. Unless this is set to 0.0, particle will become slower after collision.", null);

			public GUIContent bounce = EditorGUIUtility.TrTextContent("Bounce", "When particle collides, the bounce is scaled with this value. The bounce is the upwards motion in the plane normal direction.", null);

			public GUIContent radiusScale = EditorGUIUtility.TrTextContent("Radius Scale", "Scale particle bounds by this amount to get more precise collisions.", null);

			public GUIContent visualization = EditorGUIUtility.TrTextContent("Visualization", "Only used for visualizing the planes: Wireframe or Solid.", null);

			public GUIContent scalePlane = EditorGUIUtility.TrTextContent("Scale Plane", "Resizes the visualization planes.", null);

			public GUIContent visualizeBounds = EditorGUIUtility.TrTextContent("Visualize Bounds", "Render the collision bounds of the particles.", null);

			public GUIContent collidesWith = EditorGUIUtility.TrTextContent("Collides With", "Collides the particles with colliders included in the layermask.", null);

			public GUIContent collidesWithDynamic = EditorGUIUtility.TrTextContent("Enable Dynamic Colliders", "Should particles collide with dynamic objects?", null);

			public GUIContent maxCollisionShapes = EditorGUIUtility.TrTextContent("Max Collision Shapes", "How many collision shapes can be considered for particle collisions. Excess shapes will be ignored. Terrains take priority.", null);

			public GUIContent quality = EditorGUIUtility.TrTextContent("Collision Quality", "Quality of world collisions. Medium and low quality are approximate and may leak particles.", null);

			public GUIContent voxelSize = EditorGUIUtility.TrTextContent("Voxel Size", "Size of voxels in the collision cache. Smaller values improve accuracy, but require higher memory usage and are less efficient.", null);

			public GUIContent collisionMessages = EditorGUIUtility.TrTextContent("Send Collision Messages", "Send collision callback messages.", null);

			public GUIContent collisionType = EditorGUIUtility.TrTextContent("Type", "Collide with a list of Planes, or the Physics World.", null);

			public GUIContent collisionMode = EditorGUIUtility.TrTextContent("Mode", "Use 3D Physics or 2D Physics.", null);

			public GUIContent colliderForce = EditorGUIUtility.TrTextContent("Collider Force", "Control the strength of particle forces on colliders.", null);

			public GUIContent multiplyColliderForceByCollisionAngle = EditorGUIUtility.TrTextContent("Multiply by Collision Angle", "Should the force be proportional to the angle of the particle collision?  A particle collision directly along the collision normal produces all the specified force whilst collisions away from the collision normal produce less force.", null);

			public GUIContent multiplyColliderForceByParticleSpeed = EditorGUIUtility.TrTextContent("Multiply by Particle Speed", "Should the force be proportional to the particle speed?", null);

			public GUIContent multiplyColliderForceByParticleSize = EditorGUIUtility.TrTextContent("Multiply by Particle Size", "Should the force be proportional to the particle size?", null);

			public GUIContent[] collisionTypes = new GUIContent[]
			{
				EditorGUIUtility.TrTextContent("Planes", null, null),
				EditorGUIUtility.TrTextContent("World", null, null)
			};

			public GUIContent[] collisionModes = new GUIContent[]
			{
				EditorGUIUtility.TrTextContent("3D", null, null),
				EditorGUIUtility.TrTextContent("2D", null, null)
			};

			public GUIContent[] qualitySettings = new GUIContent[]
			{
				EditorGUIUtility.TrTextContent("High", null, null),
				EditorGUIUtility.TrTextContent("Medium (Static Colliders)", null, null),
				EditorGUIUtility.TrTextContent("Low (Static Colliders)", null, null)
			};

			public GUIContent[] planeVizTypes = new GUIContent[]
			{
				EditorGUIUtility.TrTextContent("Grid", null, null),
				EditorGUIUtility.TrTextContent("Solid", null, null)
			};

			public GUIContent[] toolContents = new GUIContent[]
			{
				EditorGUIUtility.TrIconContent("MoveTool", "Move plane editing mode."),
				EditorGUIUtility.TrIconContent("RotateTool", "Rotate plane editing mode.")
			};

			public EditMode.SceneViewEditMode[] sceneViewEditModes = new EditMode.SceneViewEditMode[]
			{
				EditMode.SceneViewEditMode.ParticleSystemCollisionModulePlanesMove,
				EditMode.SceneViewEditMode.ParticleSystemCollisionModulePlanesRotate
			};
		}

		private const int k_MaxNumPlanes = 6;

		private SerializedProperty m_Type;

		private SerializedProperty[] m_Planes = new SerializedProperty[6];

		private SerializedMinMaxCurve m_Dampen;

		private SerializedMinMaxCurve m_Bounce;

		private SerializedMinMaxCurve m_LifetimeLossOnCollision;

		private SerializedProperty m_MinKillSpeed;

		private SerializedProperty m_MaxKillSpeed;

		private SerializedProperty m_RadiusScale;

		private SerializedProperty m_CollidesWith;

		private SerializedProperty m_CollidesWithDynamic;

		private SerializedProperty m_MaxCollisionShapes;

		private SerializedProperty m_Quality;

		private SerializedProperty m_VoxelSize;

		private SerializedProperty m_CollisionMessages;

		private SerializedProperty m_CollisionMode;

		private SerializedProperty m_ColliderForce;

		private SerializedProperty m_MultiplyColliderForceByCollisionAngle;

		private SerializedProperty m_MultiplyColliderForceByParticleSpeed;

		private SerializedProperty m_MultiplyColliderForceByParticleSize;

		private SerializedProperty[] m_ShownPlanes;

		private List<Transform> m_ScenePlanes = new List<Transform>();

		private static CollisionModuleUI.PlaneVizType m_PlaneVisualizationType = CollisionModuleUI.PlaneVizType.Solid;

		private static float m_ScaleGrid = 1f;

		private static bool s_VisualizeBounds = false;

		private static Transform s_SelectedTransform;

		private static CollisionModuleUI.Texts s_Texts;

		[CompilerGenerated]
		private static Handles.CapFunction <>f__mg$cache0;

		private bool editingPlanes
		{
			get
			{
				return (EditMode.editMode == EditMode.SceneViewEditMode.ParticleSystemCollisionModulePlanesMove || EditMode.editMode == EditMode.SceneViewEditMode.ParticleSystemCollisionModulePlanesRotate) && EditMode.IsOwner(this.m_ParticleSystemUI.m_ParticleEffectUI.m_Owner.customEditor);
			}
			set
			{
				if (!value && this.editingPlanes)
				{
					EditMode.QuitEditMode();
				}
				SceneView.RepaintAll();
			}
		}

		public CollisionModuleUI(ParticleSystemUI owner, SerializedObject o, string displayName) : base(owner, o, "CollisionModule", displayName)
		{
			this.m_ToolTip = "Allows you to specify multiple collision planes that the particle can collide with.";
		}

		protected override void Init()
		{
			if (this.m_Type == null)
			{
				if (CollisionModuleUI.s_Texts == null)
				{
					CollisionModuleUI.s_Texts = new CollisionModuleUI.Texts();
				}
				this.m_Type = base.GetProperty("type");
				List<SerializedProperty> list = new List<SerializedProperty>();
				for (int i = 0; i < this.m_Planes.Length; i++)
				{
					this.m_Planes[i] = base.GetProperty("plane" + i);
					if (i == 0 || this.m_Planes[i].objectReferenceValue != null)
					{
						list.Add(this.m_Planes[i]);
					}
				}
				this.m_ShownPlanes = list.ToArray();
				this.m_Dampen = new SerializedMinMaxCurve(this, CollisionModuleUI.s_Texts.dampen, "m_Dampen");
				this.m_Dampen.m_AllowCurves = false;
				this.m_Bounce = new SerializedMinMaxCurve(this, CollisionModuleUI.s_Texts.bounce, "m_Bounce");
				this.m_Bounce.m_AllowCurves = false;
				this.m_LifetimeLossOnCollision = new SerializedMinMaxCurve(this, CollisionModuleUI.s_Texts.lifetimeLoss, "m_EnergyLossOnCollision");
				this.m_LifetimeLossOnCollision.m_AllowCurves = false;
				this.m_MinKillSpeed = base.GetProperty("minKillSpeed");
				this.m_MaxKillSpeed = base.GetProperty("maxKillSpeed");
				this.m_RadiusScale = base.GetProperty("radiusScale");
				CollisionModuleUI.m_PlaneVisualizationType = (CollisionModuleUI.PlaneVizType)EditorPrefs.GetInt("PlaneColisionVizType", 1);
				CollisionModuleUI.m_ScaleGrid = EditorPrefs.GetFloat("ScalePlaneColision", 1f);
				CollisionModuleUI.s_VisualizeBounds = EditorPrefs.GetBool("VisualizeBounds", false);
				this.m_CollidesWith = base.GetProperty("collidesWith");
				this.m_CollidesWithDynamic = base.GetProperty("collidesWithDynamic");
				this.m_MaxCollisionShapes = base.GetProperty("maxCollisionShapes");
				this.m_Quality = base.GetProperty("quality");
				this.m_VoxelSize = base.GetProperty("voxelSize");
				this.m_CollisionMessages = base.GetProperty("collisionMessages");
				this.m_CollisionMode = base.GetProperty("collisionMode");
				this.m_ColliderForce = base.GetProperty("colliderForce");
				this.m_MultiplyColliderForceByCollisionAngle = base.GetProperty("multiplyColliderForceByCollisionAngle");
				this.m_MultiplyColliderForceByParticleSpeed = base.GetProperty("multiplyColliderForceByParticleSpeed");
				this.m_MultiplyColliderForceByParticleSize = base.GetProperty("multiplyColliderForceByParticleSize");
				this.SyncVisualization();
			}
		}

		public override void UndoRedoPerformed()
		{
			base.UndoRedoPerformed();
			this.SyncVisualization();
		}

		protected override void SetVisibilityState(ModuleUI.VisibilityState newState)
		{
			base.SetVisibilityState(newState);
			if (newState != ModuleUI.VisibilityState.VisibleAndFoldedOut)
			{
				CollisionModuleUI.s_SelectedTransform = null;
				this.editingPlanes = false;
			}
			else
			{
				this.SyncVisualization();
			}
		}

		private Bounds GetBounds()
		{
			Bounds result = default(Bounds);
			bool flag = false;
			ParticleSystem[] particleSystems = this.m_ParticleSystemUI.m_ParticleSystems;
			for (int i = 0; i < particleSystems.Length; i++)
			{
				ParticleSystem particleSystem = particleSystems[i];
				ParticleSystemRenderer component = particleSystem.GetComponent<ParticleSystemRenderer>();
				if (!flag)
				{
					result = component.bounds;
				}
				result.Encapsulate(component.bounds);
				flag = true;
			}
			return result;
		}

		public override void OnInspectorGUI(InitialModuleUI initial)
		{
			EditorGUI.BeginChangeCheck();
			CollisionModuleUI.CollisionTypes collisionTypes = (CollisionModuleUI.CollisionTypes)ModuleUI.GUIPopup(CollisionModuleUI.s_Texts.collisionType, this.m_Type, CollisionModuleUI.s_Texts.collisionTypes, new GUILayoutOption[0]);
			if (EditorGUI.EndChangeCheck())
			{
				this.SyncVisualization();
			}
			if (collisionTypes == CollisionModuleUI.CollisionTypes.Plane)
			{
				this.DoListOfPlanesGUI();
				EditorGUI.BeginChangeCheck();
				CollisionModuleUI.m_PlaneVisualizationType = (CollisionModuleUI.PlaneVizType)ModuleUI.GUIPopup(CollisionModuleUI.s_Texts.visualization, (int)CollisionModuleUI.m_PlaneVisualizationType, CollisionModuleUI.s_Texts.planeVizTypes, new GUILayoutOption[0]);
				if (EditorGUI.EndChangeCheck())
				{
					EditorPrefs.SetInt("PlaneColisionVizType", (int)CollisionModuleUI.m_PlaneVisualizationType);
				}
				EditorGUI.BeginChangeCheck();
				CollisionModuleUI.m_ScaleGrid = ModuleUI.GUIFloat(CollisionModuleUI.s_Texts.scalePlane, CollisionModuleUI.m_ScaleGrid, "f2", new GUILayoutOption[0]);
				if (EditorGUI.EndChangeCheck())
				{
					CollisionModuleUI.m_ScaleGrid = Mathf.Max(0f, CollisionModuleUI.m_ScaleGrid);
					EditorPrefs.SetFloat("ScalePlaneColision", CollisionModuleUI.m_ScaleGrid);
				}
				ModuleUI.GUIButtonGroup(CollisionModuleUI.s_Texts.sceneViewEditModes, CollisionModuleUI.s_Texts.toolContents, new Func<Bounds>(this.GetBounds), this.m_ParticleSystemUI.m_ParticleEffectUI.m_Owner.customEditor);
			}
			else
			{
				ModuleUI.GUIPopup(CollisionModuleUI.s_Texts.collisionMode, this.m_CollisionMode, CollisionModuleUI.s_Texts.collisionModes, new GUILayoutOption[0]);
			}
			ModuleUI.GUIMinMaxCurve(CollisionModuleUI.s_Texts.dampen, this.m_Dampen, new GUILayoutOption[0]);
			ModuleUI.GUIMinMaxCurve(CollisionModuleUI.s_Texts.bounce, this.m_Bounce, new GUILayoutOption[0]);
			ModuleUI.GUIMinMaxCurve(CollisionModuleUI.s_Texts.lifetimeLoss, this.m_LifetimeLossOnCollision, new GUILayoutOption[0]);
			ModuleUI.GUIFloat(CollisionModuleUI.s_Texts.minKillSpeed, this.m_MinKillSpeed, new GUILayoutOption[0]);
			ModuleUI.GUIFloat(CollisionModuleUI.s_Texts.maxKillSpeed, this.m_MaxKillSpeed, new GUILayoutOption[0]);
			ModuleUI.GUIFloat(CollisionModuleUI.s_Texts.radiusScale, this.m_RadiusScale, new GUILayoutOption[0]);
			if (collisionTypes == CollisionModuleUI.CollisionTypes.World)
			{
				ModuleUI.GUIPopup(CollisionModuleUI.s_Texts.quality, this.m_Quality, CollisionModuleUI.s_Texts.qualitySettings, new GUILayoutOption[0]);
				EditorGUI.indentLevel++;
				ModuleUI.GUILayerMask(CollisionModuleUI.s_Texts.collidesWith, this.m_CollidesWith, new GUILayoutOption[0]);
				ModuleUI.GUIInt(CollisionModuleUI.s_Texts.maxCollisionShapes, this.m_MaxCollisionShapes, new GUILayoutOption[0]);
				if (this.m_Quality.intValue == 0)
				{
					ModuleUI.GUIToggle(CollisionModuleUI.s_Texts.collidesWithDynamic, this.m_CollidesWithDynamic, new GUILayoutOption[0]);
				}
				else
				{
					ModuleUI.GUIFloat(CollisionModuleUI.s_Texts.voxelSize, this.m_VoxelSize, new GUILayoutOption[0]);
				}
				EditorGUI.indentLevel--;
				ModuleUI.GUIFloat(CollisionModuleUI.s_Texts.colliderForce, this.m_ColliderForce, new GUILayoutOption[0]);
				EditorGUI.indentLevel++;
				ModuleUI.GUIToggle(CollisionModuleUI.s_Texts.multiplyColliderForceByCollisionAngle, this.m_MultiplyColliderForceByCollisionAngle, new GUILayoutOption[0]);
				ModuleUI.GUIToggle(CollisionModuleUI.s_Texts.multiplyColliderForceByParticleSpeed, this.m_MultiplyColliderForceByParticleSpeed, new GUILayoutOption[0]);
				ModuleUI.GUIToggle(CollisionModuleUI.s_Texts.multiplyColliderForceByParticleSize, this.m_MultiplyColliderForceByParticleSize, new GUILayoutOption[0]);
				EditorGUI.indentLevel--;
			}
			ModuleUI.GUIToggle(CollisionModuleUI.s_Texts.collisionMessages, this.m_CollisionMessages, new GUILayoutOption[0]);
			EditorGUI.BeginChangeCheck();
			CollisionModuleUI.s_VisualizeBounds = ModuleUI.GUIToggle(CollisionModuleUI.s_Texts.visualizeBounds, CollisionModuleUI.s_VisualizeBounds, new GUILayoutOption[0]);
			if (EditorGUI.EndChangeCheck())
			{
				EditorPrefs.SetBool("VisualizeBounds", CollisionModuleUI.s_VisualizeBounds);
			}
		}

		protected override void OnModuleEnable()
		{
			base.OnModuleEnable();
			this.SyncVisualization();
		}

		protected override void OnModuleDisable()
		{
			base.OnModuleDisable();
			this.editingPlanes = false;
		}

		private void SyncVisualization()
		{
			this.m_ScenePlanes.Clear();
			if (this.m_ParticleSystemUI.multiEdit)
			{
				ParticleSystem[] particleSystems = this.m_ParticleSystemUI.m_ParticleSystems;
				for (int i = 0; i < particleSystems.Length; i++)
				{
					ParticleSystem particleSystem = particleSystems[i];
					if (particleSystem.collision.type == ParticleSystemCollisionType.Planes)
					{
						for (int j = 0; j < 6; j++)
						{
							Transform plane = particleSystem.collision.GetPlane(j);
							if (plane != null && !this.m_ScenePlanes.Contains(plane))
							{
								this.m_ScenePlanes.Add(plane);
							}
						}
					}
				}
			}
			else
			{
				CollisionModuleUI.CollisionTypes intValue = (CollisionModuleUI.CollisionTypes)this.m_Type.intValue;
				if (intValue != CollisionModuleUI.CollisionTypes.Plane)
				{
					this.editingPlanes = false;
				}
				else
				{
					for (int k = 0; k < this.m_ShownPlanes.Length; k++)
					{
						Transform transform = this.m_ShownPlanes[k].objectReferenceValue as Transform;
						if (transform != null && !this.m_ScenePlanes.Contains(transform))
						{
							this.m_ScenePlanes.Add(transform);
						}
					}
				}
			}
		}

		private static GameObject CreateEmptyGameObject(string name, ParticleSystem parentOfGameObject)
		{
			GameObject gameObject = new GameObject(name);
			GameObject result;
			if (gameObject)
			{
				if (parentOfGameObject)
				{
					gameObject.transform.parent = parentOfGameObject.transform;
				}
				Undo.RegisterCreatedObjectUndo(gameObject, "Created `" + name + "`");
				result = gameObject;
			}
			else
			{
				result = null;
			}
			return result;
		}

		private bool IsListOfPlanesValid()
		{
			bool result;
			if (this.m_ParticleSystemUI.multiEdit)
			{
				for (int i = 0; i < 6; i++)
				{
					int num = -1;
					ParticleSystem[] particleSystems = this.m_ParticleSystemUI.m_ParticleSystems;
					for (int j = 0; j < particleSystems.Length; j++)
					{
						ParticleSystem particleSystem = particleSystems[j];
						int num2 = (!(particleSystem.collision.GetPlane(i) != null)) ? 0 : 1;
						if (num == -1)
						{
							num = num2;
						}
						else if (num2 != num)
						{
							result = false;
							return result;
						}
					}
				}
			}
			result = true;
			return result;
		}

		private void DoListOfPlanesGUI()
		{
			if (!this.IsListOfPlanesValid())
			{
				EditorGUILayout.HelpBox("Plane list editing is only available when all selected systems contain the same number of planes", MessageType.Info, true);
			}
			else
			{
				EditorGUI.BeginChangeCheck();
				int num = base.GUIListOfFloatObjectToggleFields(CollisionModuleUI.s_Texts.planes, this.m_ShownPlanes, null, CollisionModuleUI.s_Texts.createPlane, !this.m_ParticleSystemUI.multiEdit, new GUILayoutOption[0]);
				bool flag = EditorGUI.EndChangeCheck();
				if (num >= 0 && !this.m_ParticleSystemUI.multiEdit)
				{
					GameObject gameObject = CollisionModuleUI.CreateEmptyGameObject("Plane Transform " + (num + 1), this.m_ParticleSystemUI.m_ParticleSystems[0]);
					gameObject.transform.localPosition = new Vector3(0f, 0f, (float)(10 + num));
					gameObject.transform.localEulerAngles = new Vector3(-90f, 0f, 0f);
					this.m_ShownPlanes[num].objectReferenceValue = gameObject;
					flag = true;
				}
				Rect rect = GUILayoutUtility.GetRect(0f, 16f);
				rect.x = rect.xMax - 24f - 5f;
				rect.width = 12f;
				if (this.m_ShownPlanes.Length > 1)
				{
					if (ModuleUI.MinusButton(rect))
					{
						this.m_ShownPlanes[this.m_ShownPlanes.Length - 1].objectReferenceValue = null;
						List<SerializedProperty> list = new List<SerializedProperty>(this.m_ShownPlanes);
						list.RemoveAt(list.Count - 1);
						this.m_ShownPlanes = list.ToArray();
						flag = true;
					}
				}
				if (this.m_ShownPlanes.Length < 6 && !this.m_ParticleSystemUI.multiEdit)
				{
					rect.x += 17f;
					if (ModuleUI.PlusButton(rect))
					{
						List<SerializedProperty> list2 = new List<SerializedProperty>(this.m_ShownPlanes);
						list2.Add(this.m_Planes[list2.Count]);
						this.m_ShownPlanes = list2.ToArray();
					}
				}
				if (flag)
				{
					this.SyncVisualization();
				}
			}
		}

		public override void OnSceneViewGUI()
		{
			this.RenderCollisionBounds();
			this.CollisionPlanesSceneGUI();
		}

		private void CollisionPlanesSceneGUI()
		{
			if (this.m_ScenePlanes.Count != 0)
			{
				Event current = Event.current;
				Color color = Handles.color;
				Color color2 = new Color(1f, 1f, 1f, 0.5f);
				for (int i = 0; i < this.m_ScenePlanes.Count; i++)
				{
					if (!(this.m_ScenePlanes[i] == null))
					{
						Transform transform = this.m_ScenePlanes[i];
						Vector3 position = transform.position;
						Quaternion rotation = transform.rotation;
						Vector3 axis = rotation * Vector3.right;
						Vector3 normal = rotation * Vector3.up;
						Vector3 axis2 = rotation * Vector3.forward;
						bool flag = EditorApplication.isPlaying && transform.gameObject.isStatic;
						if (this.editingPlanes)
						{
							if (object.ReferenceEquals(CollisionModuleUI.s_SelectedTransform, transform))
							{
								EditorGUI.BeginChangeCheck();
								Vector3 position2 = transform.position;
								Quaternion rotation2 = transform.rotation;
								using (new EditorGUI.DisabledScope(flag))
								{
									if (flag)
									{
										Handles.ShowStaticLabel(position);
									}
									if (EditMode.editMode == EditMode.SceneViewEditMode.ParticleSystemCollisionModulePlanesMove)
									{
										position2 = Handles.PositionHandle(position, rotation);
									}
									else if (EditMode.editMode == EditMode.SceneViewEditMode.ParticleSystemCollisionModulePlanesRotate)
									{
										rotation2 = Handles.RotationHandle(rotation, position);
									}
								}
								if (EditorGUI.EndChangeCheck())
								{
									Undo.RecordObject(transform, "Modified Collision Plane Transform");
									transform.position = position2;
									transform.rotation = rotation2;
									ParticleSystemEditorUtils.PerformCompleteResimulation();
								}
							}
							else
							{
								float num = HandleUtility.GetHandleSize(position) * 0.6f;
								EventType eventType = current.type;
								if (current.type == EventType.Ignore && current.rawType == EventType.MouseUp)
								{
									eventType = current.rawType;
								}
								Vector3 arg_1F3_0 = position;
								Quaternion arg_1F3_1 = Quaternion.identity;
								float arg_1F3_2 = num;
								Vector3 arg_1F3_3 = Vector3.zero;
								if (CollisionModuleUI.<>f__mg$cache0 == null)
								{
									CollisionModuleUI.<>f__mg$cache0 = new Handles.CapFunction(Handles.RectangleHandleCap);
								}
								Handles.FreeMoveHandle(arg_1F3_0, arg_1F3_1, arg_1F3_2, arg_1F3_3, CollisionModuleUI.<>f__mg$cache0);
								if (eventType == EventType.MouseDown && current.type == EventType.Used)
								{
									CollisionModuleUI.s_SelectedTransform = transform;
									GUIUtility.hotControl = 0;
								}
							}
						}
						Handles.color = color2;
						Color color3 = Handles.s_ColliderHandleColor * 0.9f;
						if (flag)
						{
							color3.a *= 0.2f;
						}
						if (CollisionModuleUI.m_PlaneVisualizationType == CollisionModuleUI.PlaneVizType.Grid)
						{
							CollisionModuleUI.DrawGrid(position, axis, axis2, normal, color3);
						}
						else
						{
							CollisionModuleUI.DrawSolidPlane(position, rotation, color3, Color.yellow);
						}
					}
				}
				Handles.color = color;
			}
		}

		private void RenderCollisionBounds()
		{
			if (CollisionModuleUI.s_VisualizeBounds)
			{
				Color color = Handles.color;
				Handles.color = Color.green;
				Matrix4x4 matrix = Handles.matrix;
				Vector3[] array = new Vector3[20];
				Vector3[] array2 = new Vector3[20];
				Vector3[] array3 = new Vector3[20];
				Handles.SetDiscSectionPoints(array, Vector3.zero, Vector3.forward, Vector3.right, 360f, 1f);
				Handles.SetDiscSectionPoints(array2, Vector3.zero, Vector3.up, -Vector3.right, 360f, 1f);
				Handles.SetDiscSectionPoints(array3, Vector3.zero, Vector3.right, Vector3.up, 360f, 1f);
				Vector3[] array4 = new Vector3[array.Length + array2.Length + array3.Length];
				array.CopyTo(array4, 0);
				array2.CopyTo(array4, 20);
				array3.CopyTo(array4, 40);
				ParticleSystem[] particleSystems = this.m_ParticleSystemUI.m_ParticleSystems;
				for (int i = 0; i < particleSystems.Length; i++)
				{
					ParticleSystem particleSystem = particleSystems[i];
					if (particleSystem.collision.enabled)
					{
						ParticleSystem.Particle[] array5 = new ParticleSystem.Particle[particleSystem.particleCount];
						int particles = particleSystem.GetParticles(array5);
						Matrix4x4 lhs = Matrix4x4.identity;
						if (particleSystem.main.simulationSpace == ParticleSystemSimulationSpace.Local)
						{
							lhs = particleSystem.GetLocalToWorldMatrix();
						}
						for (int j = 0; j < particles; j++)
						{
							ParticleSystem.Particle particle = array5[j];
							Vector3 currentSize3D = particle.GetCurrentSize3D(particleSystem);
							float num = Math.Max(currentSize3D.x, Math.Max(currentSize3D.y, currentSize3D.z)) * 0.5f * particleSystem.collision.radiusScale;
							Handles.matrix = lhs * Matrix4x4.TRS(particle.position, Quaternion.identity, new Vector3(num, num, num));
							Handles.DrawPolyLine(array4);
						}
					}
				}
				Handles.color = color;
				Handles.matrix = matrix;
			}
		}

		private static void DrawSolidPlane(Vector3 pos, Quaternion rot, Color faceColor, Color edgeColor)
		{
			if (Event.current.type == EventType.Repaint)
			{
				Matrix4x4 matrix = Handles.matrix;
				float num = 10f * CollisionModuleUI.m_ScaleGrid;
				Handles.matrix = Matrix4x4.TRS(pos, rot, new Vector3(num, num, num)) * Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(90f, 0f, 0f), Vector3.one);
				Handles.DrawSolidRectangleWithOutline(new Rect(-0.5f, -0.5f, 1f, 1f), faceColor, edgeColor);
				Handles.DrawLine(Vector3.zero, Vector3.back / num);
				Handles.matrix = matrix;
			}
		}

		private static void DrawGrid(Vector3 pos, Vector3 axis1, Vector3 axis2, Vector3 normal, Color color)
		{
			if (Event.current.type == EventType.Repaint)
			{
				HandleUtility.ApplyWireMaterial();
				if (color.a > 0f)
				{
					GL.Begin(1);
					float num = 10f;
					num *= CollisionModuleUI.m_ScaleGrid;
					int num2 = (int)num;
					num2 = Mathf.Clamp(num2, 10, 40);
					if (num2 % 2 == 0)
					{
						num2++;
					}
					float d = num * 0.5f;
					float d2 = num / (float)(num2 - 1);
					Vector3 b = axis1 * num;
					Vector3 b2 = axis2 * num;
					Vector3 a = axis1 * d2;
					Vector3 a2 = axis2 * d2;
					Vector3 a3 = pos - axis1 * d - axis2 * d;
					for (int i = 0; i < num2; i++)
					{
						if (i % 2 == 0)
						{
							GL.Color(color * 0.7f);
						}
						else
						{
							GL.Color(color);
						}
						GL.Vertex(a3 + (float)i * a);
						GL.Vertex(a3 + (float)i * a + b2);
						GL.Vertex(a3 + (float)i * a2);
						GL.Vertex(a3 + (float)i * a2 + b);
					}
					GL.Color(color);
					GL.Vertex(pos);
					GL.Vertex(pos + normal);
					GL.End();
				}
			}
		}

		public override void UpdateCullingSupportedString(ref string text)
		{
			text += "\nCollision module is enabled.";
		}
	}
}
