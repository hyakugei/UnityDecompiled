using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEditor.AnimatedValues;
using UnityEditor.Modules;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Scripting;

namespace UnityEditor
{
	[CanEditMultipleObjects, CustomEditor(typeof(Camera))]
	public class CameraEditor : Editor
	{
		public sealed class Settings
		{
			private SerializedObject m_SerializedObject;

			private static readonly GUIContent[] kCameraRenderPaths = new GUIContent[]
			{
				EditorGUIUtility.TrTextContent("Use Graphics Settings", null, null),
				EditorGUIUtility.TrTextContent("Forward", null, null),
				EditorGUIUtility.TrTextContent("Deferred", null, null),
				EditorGUIUtility.TrTextContent("Legacy Vertex Lit", null, null),
				EditorGUIUtility.TrTextContent("Legacy Deferred (light prepass)", null, null)
			};

			private static readonly int[] kCameraRenderPathValues = new int[]
			{
				-1,
				1,
				3,
				0,
				2
			};

			private static readonly GUIContent[] kTargetEyes = new GUIContent[]
			{
				EditorGUIUtility.TrTextContent("Both", null, null),
				EditorGUIUtility.TrTextContent("Left", null, null),
				EditorGUIUtility.TrTextContent("Right", null, null),
				EditorGUIUtility.TrTextContent("None (Main Display)", null, null)
			};

			private static readonly int[] kTargetEyeValues = new int[]
			{
				3,
				1,
				2,
				0
			};

			public SerializedProperty clearFlags
			{
				get;
				private set;
			}

			public SerializedProperty backgroundColor
			{
				get;
				private set;
			}

			public SerializedProperty normalizedViewPortRect
			{
				get;
				private set;
			}

			public SerializedProperty fieldOfView
			{
				get;
				private set;
			}

			public SerializedProperty orthographic
			{
				get;
				private set;
			}

			public SerializedProperty orthographicSize
			{
				get;
				private set;
			}

			public SerializedProperty depth
			{
				get;
				private set;
			}

			public SerializedProperty cullingMask
			{
				get;
				private set;
			}

			public SerializedProperty renderingPath
			{
				get;
				private set;
			}

			public SerializedProperty occlusionCulling
			{
				get;
				private set;
			}

			public SerializedProperty targetTexture
			{
				get;
				private set;
			}

			public SerializedProperty HDR
			{
				get;
				private set;
			}

			public SerializedProperty allowMSAA
			{
				get;
				private set;
			}

			public SerializedProperty allowDynamicResolution
			{
				get;
				private set;
			}

			public SerializedProperty stereoConvergence
			{
				get;
				private set;
			}

			public SerializedProperty stereoSeparation
			{
				get;
				private set;
			}

			public SerializedProperty nearClippingPlane
			{
				get;
				private set;
			}

			public SerializedProperty farClippingPlane
			{
				get;
				private set;
			}

			public SerializedProperty targetDisplay
			{
				get;
				private set;
			}

			public SerializedProperty targetEye
			{
				get;
				private set;
			}

			public Settings(SerializedObject so)
			{
				this.m_SerializedObject = so;
			}

			public void OnEnable()
			{
				this.clearFlags = this.m_SerializedObject.FindProperty("m_ClearFlags");
				this.backgroundColor = this.m_SerializedObject.FindProperty("m_BackGroundColor");
				this.normalizedViewPortRect = this.m_SerializedObject.FindProperty("m_NormalizedViewPortRect");
				this.nearClippingPlane = this.m_SerializedObject.FindProperty("near clip plane");
				this.farClippingPlane = this.m_SerializedObject.FindProperty("far clip plane");
				this.fieldOfView = this.m_SerializedObject.FindProperty("field of view");
				this.orthographic = this.m_SerializedObject.FindProperty("orthographic");
				this.orthographicSize = this.m_SerializedObject.FindProperty("orthographic size");
				this.depth = this.m_SerializedObject.FindProperty("m_Depth");
				this.cullingMask = this.m_SerializedObject.FindProperty("m_CullingMask");
				this.renderingPath = this.m_SerializedObject.FindProperty("m_RenderingPath");
				this.occlusionCulling = this.m_SerializedObject.FindProperty("m_OcclusionCulling");
				this.targetTexture = this.m_SerializedObject.FindProperty("m_TargetTexture");
				this.HDR = this.m_SerializedObject.FindProperty("m_HDR");
				this.allowMSAA = this.m_SerializedObject.FindProperty("m_AllowMSAA");
				this.allowDynamicResolution = this.m_SerializedObject.FindProperty("m_AllowDynamicResolution");
				this.stereoConvergence = this.m_SerializedObject.FindProperty("m_StereoConvergence");
				this.stereoSeparation = this.m_SerializedObject.FindProperty("m_StereoSeparation");
				this.targetDisplay = this.m_SerializedObject.FindProperty("m_TargetDisplay");
				this.targetEye = this.m_SerializedObject.FindProperty("m_TargetEye");
			}

			public void Update()
			{
				this.m_SerializedObject.Update();
			}

			public void ApplyModifiedProperties()
			{
				this.m_SerializedObject.ApplyModifiedProperties();
			}

			public void DrawClearFlags()
			{
				EditorGUILayout.PropertyField(this.clearFlags, EditorGUIUtility.TextContent("Clear Flags|What to display in empty areas of this Camera's view.\n\nChoose Skybox to display a skybox in empty areas, defaulting to a background color if no skybox is found.\n\nChoose Solid Color to display a background color in empty areas.\n\nChoose Depth Only to display nothing in empty areas.\n\nChoose Don't Clear to display whatever was displayed in the previous frame in empty areas."), new GUILayoutOption[0]);
			}

			public void DrawBackgroundColor()
			{
				EditorGUILayout.PropertyField(this.backgroundColor, EditorGUIUtility.TextContent("Background|The Camera clears the screen to this color before rendering."), new GUILayoutOption[0]);
			}

			public void DrawCullingMask()
			{
				EditorGUILayout.PropertyField(this.cullingMask, new GUILayoutOption[0]);
			}

			public void DrawProjection()
			{
				CameraEditor.ProjectionType projectionType = (!this.orthographic.boolValue) ? CameraEditor.ProjectionType.Perspective : CameraEditor.ProjectionType.Orthographic;
				EditorGUI.BeginChangeCheck();
				EditorGUI.showMixedValue = this.orthographic.hasMultipleDifferentValues;
				projectionType = (CameraEditor.ProjectionType)EditorGUILayout.EnumPopup(EditorGUIUtility.TextContent("Projection|How the Camera renders perspective.\n\nChoose Perspective to render objects with perspective.\n\nChoose Orthographic to render objects uniformly, with no sense of perspective."), projectionType, new GUILayoutOption[0]);
				EditorGUI.showMixedValue = false;
				if (EditorGUI.EndChangeCheck())
				{
					this.orthographic.boolValue = (projectionType == CameraEditor.ProjectionType.Orthographic);
				}
				if (!this.orthographic.hasMultipleDifferentValues)
				{
					if (projectionType == CameraEditor.ProjectionType.Orthographic)
					{
						EditorGUILayout.PropertyField(this.orthographicSize, new GUIContent("Size"), new GUILayoutOption[0]);
					}
					else
					{
						EditorGUILayout.Slider(this.fieldOfView, 1f, 179f, EditorGUIUtility.TextContent("Field of View|The width of the Camera’s view angle, measured in degrees along the local Y axis."), new GUILayoutOption[0]);
					}
				}
			}

			public void DrawClippingPlanes()
			{
				EditorGUILayout.PropertiesField(EditorGUI.s_ClipingPlanesLabel, new SerializedProperty[]
				{
					this.nearClippingPlane,
					this.farClippingPlane
				}, EditorGUI.s_NearAndFarLabels, 35f, new GUILayoutOption[0]);
			}

			public void DrawNormalizedViewPort()
			{
				EditorGUILayout.PropertyField(this.normalizedViewPortRect, EditorGUIUtility.TextContent("Viewport Rect|Four values that indicate where on the screen this camera view will be drawn. Measured in Viewport Coordinates (values 0–1)."), new GUILayoutOption[0]);
			}

			public void DrawDepth()
			{
				EditorGUILayout.PropertyField(this.depth, new GUILayoutOption[0]);
			}

			public void DrawRenderingPath()
			{
				EditorGUILayout.IntPopup(this.renderingPath, CameraEditor.Settings.kCameraRenderPaths, CameraEditor.Settings.kCameraRenderPathValues, EditorGUIUtility.TempContent("Rendering Path"), new GUILayoutOption[0]);
			}

			public void DrawTargetTexture(bool deferred)
			{
				EditorGUILayout.PropertyField(this.targetTexture, new GUILayoutOption[0]);
				if (!this.targetTexture.hasMultipleDifferentValues)
				{
					RenderTexture renderTexture = this.targetTexture.objectReferenceValue as RenderTexture;
					if (renderTexture && renderTexture.antiAliasing > 1 && deferred)
					{
						EditorGUILayout.HelpBox("Manual MSAA target set with deferred rendering. This will lead to undefined behavior.", MessageType.Warning, true);
					}
				}
			}

			public void DrawOcclusionCulling()
			{
				EditorGUILayout.PropertyField(this.occlusionCulling, new GUILayoutOption[0]);
			}

			public void DrawHDR()
			{
				EditorGUILayout.PropertyField(this.HDR, EditorGUIUtility.TempContent("Allow HDR"), new GUILayoutOption[0]);
			}

			public void DrawMSAA()
			{
				EditorGUILayout.PropertyField(this.allowMSAA, new GUILayoutOption[0]);
			}

			public void DrawDynamicResolution()
			{
				EditorGUILayout.PropertyField(this.allowDynamicResolution, new GUILayoutOption[0]);
			}

			public void DrawVR()
			{
				if (PlayerSettings.virtualRealitySupported)
				{
					EditorGUILayout.PropertyField(this.stereoSeparation, new GUILayoutOption[0]);
					EditorGUILayout.PropertyField(this.stereoConvergence, new GUILayoutOption[0]);
				}
			}

			public void DrawMultiDisplay()
			{
				if (ModuleManager.ShouldShowMultiDisplayOption())
				{
					int intValue = this.targetDisplay.intValue;
					EditorGUILayout.Space();
					EditorGUILayout.IntPopup(this.targetDisplay, DisplayUtility.GetDisplayNames(), DisplayUtility.GetDisplayIndices(), EditorGUIUtility.TempContent("Target Display"), new GUILayoutOption[0]);
					if (intValue != this.targetDisplay.intValue)
					{
						GameView.RepaintAll();
					}
				}
			}

			public void DrawTargetEye()
			{
				EditorGUILayout.IntPopup(this.targetEye, CameraEditor.Settings.kTargetEyes, CameraEditor.Settings.kTargetEyeValues, EditorGUIUtility.TempContent("Target Eye"), new GUILayoutOption[0]);
			}
		}

		private class Styles
		{
			public static GUIContent iconRemove = EditorGUIUtility.IconContent("Toolbar Minus", "|Remove command buffer");

			public static GUIStyle invisibleButton = "InvisibleButton";
		}

		private enum ProjectionType
		{
			Perspective,
			Orthographic
		}

		private readonly AnimBool m_ShowBGColorOptions = new AnimBool();

		private readonly AnimBool m_ShowOrthoOptions = new AnimBool();

		private readonly AnimBool m_ShowTargetEyeOption = new AnimBool();

		private Camera m_PreviewCamera;

		private RenderTexture m_PreviewTexture;

		private static readonly Color kGizmoCamera = new Color(0.9137255f, 0.9137255f, 0.9137255f, 0.5019608f);

		private const float kPreviewNormalizedSize = 0.2f;

		private bool m_CommandBuffersShown = true;

		private CameraEditor.Settings m_Settings;

		[CompilerGenerated]
		private static Handles.CapFunction <>f__mg$cache0;

		private Camera camera
		{
			get
			{
				return base.target as Camera;
			}
		}

		private bool wantDeferredRendering
		{
			get
			{
				bool flag = CameraEditor.IsDeferredRenderingPath(this.camera.renderingPath);
				bool flag2 = CameraEditor.IsDeferredRenderingPath(EditorGraphicsSettings.GetCurrentTierSettings().renderingPath);
				return flag || (this.camera.renderingPath == RenderingPath.UsePlayerSettings && flag2);
			}
		}

		protected Camera previewCamera
		{
			get
			{
				if (this.m_PreviewCamera == null)
				{
					this.m_PreviewCamera = EditorUtility.CreateGameObjectWithHideFlags("Preview Camera", HideFlags.HideAndDontSave, new Type[]
					{
						typeof(Camera),
						typeof(Skybox)
					}).GetComponent<Camera>();
				}
				this.m_PreviewCamera.enabled = false;
				return this.m_PreviewCamera;
			}
		}

		protected CameraEditor.Settings settings
		{
			get
			{
				if (this.m_Settings == null)
				{
					this.m_Settings = new CameraEditor.Settings(base.serializedObject);
				}
				return this.m_Settings;
			}
		}

		private bool clearFlagsHasMultipleValues
		{
			get
			{
				return this.settings.clearFlags.hasMultipleDifferentValues;
			}
		}

		private bool orthographicHasMultipleValues
		{
			get
			{
				return this.settings.orthographic.hasMultipleDifferentValues;
			}
		}

		private int targetEyeValue
		{
			get
			{
				return this.settings.targetEye.intValue;
			}
		}

		private static bool IsDeferredRenderingPath(RenderingPath rp)
		{
			return rp == RenderingPath.DeferredLighting || rp == RenderingPath.DeferredShading;
		}

		public void OnEnable()
		{
			this.settings.OnEnable();
			Camera camera = (Camera)base.target;
			this.m_ShowBGColorOptions.value = (!this.clearFlagsHasMultipleValues && (camera.clearFlags == CameraClearFlags.Color || camera.clearFlags == CameraClearFlags.Skybox));
			this.m_ShowOrthoOptions.value = camera.orthographic;
			this.m_ShowTargetEyeOption.value = (this.targetEyeValue != 3 || PlayerSettings.virtualRealitySupported);
			this.m_ShowBGColorOptions.valueChanged.AddListener(new UnityAction(base.Repaint));
			this.m_ShowOrthoOptions.valueChanged.AddListener(new UnityAction(base.Repaint));
			this.m_ShowTargetEyeOption.valueChanged.AddListener(new UnityAction(base.Repaint));
		}

		internal void OnDisable()
		{
			this.m_ShowBGColorOptions.valueChanged.RemoveListener(new UnityAction(base.Repaint));
			this.m_ShowOrthoOptions.valueChanged.RemoveListener(new UnityAction(base.Repaint));
			this.m_ShowTargetEyeOption.valueChanged.RemoveListener(new UnityAction(base.Repaint));
		}

		public void OnDestroy()
		{
			if (this.m_PreviewCamera != null)
			{
				UnityEngine.Object.DestroyImmediate(this.m_PreviewCamera.gameObject, true);
			}
		}

		private void DepthTextureModeGUI()
		{
			if (base.targets.Length == 1)
			{
				Camera camera = base.target as Camera;
				if (!(camera == null) && camera.depthTextureMode != DepthTextureMode.None)
				{
					List<string> list = new List<string>();
					if ((camera.depthTextureMode & DepthTextureMode.Depth) != DepthTextureMode.None)
					{
						list.Add("Depth");
					}
					if ((camera.depthTextureMode & DepthTextureMode.DepthNormals) != DepthTextureMode.None)
					{
						list.Add("DepthNormals");
					}
					if ((camera.depthTextureMode & DepthTextureMode.MotionVectors) != DepthTextureMode.None)
					{
						list.Add("MotionVectors");
					}
					if (list.Count != 0)
					{
						StringBuilder stringBuilder = new StringBuilder("Info: renders ");
						for (int i = 0; i < list.Count; i++)
						{
							if (i != 0)
							{
								stringBuilder.Append(" & ");
							}
							stringBuilder.Append(list[i]);
						}
						stringBuilder.Append((list.Count <= 1) ? " texture" : " textures");
						EditorGUILayout.HelpBox(stringBuilder.ToString(), MessageType.None, true);
					}
				}
			}
		}

		private static Rect GetRemoveButtonRect(Rect r)
		{
			Vector2 vector = CameraEditor.Styles.invisibleButton.CalcSize(CameraEditor.Styles.iconRemove);
			return new Rect(r.xMax - vector.x, r.y + (float)((int)(r.height / 2f - vector.y / 2f)), vector.x, vector.y);
		}

		[DrawGizmo(GizmoType.NonSelected)]
		private static void DrawCameraBound(Camera camera, GizmoType gizmoType)
		{
			SceneView currentDrawingSceneView = SceneView.currentDrawingSceneView;
			if (currentDrawingSceneView != null && currentDrawingSceneView.in2DMode)
			{
				if (camera == Camera.main && camera.orthographic)
				{
					CameraEditor.RenderGizmo(camera);
				}
			}
		}

		private void CommandBufferGUI()
		{
			if (base.targets.Length == 1)
			{
				Camera camera = base.target as Camera;
				if (!(camera == null))
				{
					int commandBufferCount = camera.commandBufferCount;
					if (commandBufferCount != 0)
					{
						this.m_CommandBuffersShown = GUILayout.Toggle(this.m_CommandBuffersShown, GUIContent.Temp(commandBufferCount + " command buffers"), EditorStyles.foldout, new GUILayoutOption[0]);
						if (this.m_CommandBuffersShown)
						{
							EditorGUI.indentLevel++;
							CameraEvent[] array = (CameraEvent[])Enum.GetValues(typeof(CameraEvent));
							for (int i = 0; i < array.Length; i++)
							{
								CameraEvent cameraEvent = array[i];
								CommandBuffer[] commandBuffers = camera.GetCommandBuffers(cameraEvent);
								CommandBuffer[] array2 = commandBuffers;
								for (int j = 0; j < array2.Length; j++)
								{
									CommandBuffer commandBuffer = array2[j];
									using (new GUILayout.HorizontalScope(new GUILayoutOption[0]))
									{
										Rect rect = GUILayoutUtility.GetRect(GUIContent.none, EditorStyles.miniLabel);
										rect.xMin += EditorGUI.indent;
										Rect removeButtonRect = CameraEditor.GetRemoveButtonRect(rect);
										rect.xMax = removeButtonRect.x;
										GUI.Label(rect, string.Format("{0}: {1} ({2})", cameraEvent, commandBuffer.name, EditorUtility.FormatBytes(commandBuffer.sizeInBytes)), EditorStyles.miniLabel);
										if (GUI.Button(removeButtonRect, CameraEditor.Styles.iconRemove, CameraEditor.Styles.invisibleButton))
										{
											camera.RemoveCommandBuffer(cameraEvent, commandBuffer);
											SceneView.RepaintAll();
											GameView.RepaintAll();
											GUIUtility.ExitGUI();
										}
									}
								}
							}
							using (new GUILayout.HorizontalScope(new GUILayoutOption[0]))
							{
								GUILayout.FlexibleSpace();
								if (GUILayout.Button("Remove all", EditorStyles.miniButton, new GUILayoutOption[0]))
								{
									camera.RemoveAllCommandBuffers();
									SceneView.RepaintAll();
									GameView.RepaintAll();
								}
							}
							EditorGUI.indentLevel--;
						}
					}
				}
			}
		}

		public override void OnInspectorGUI()
		{
			this.settings.Update();
			Camera camera = (Camera)base.target;
			this.m_ShowBGColorOptions.target = (!this.clearFlagsHasMultipleValues && (camera.clearFlags == CameraClearFlags.Color || camera.clearFlags == CameraClearFlags.Skybox));
			this.m_ShowOrthoOptions.target = (!this.orthographicHasMultipleValues && camera.orthographic);
			this.m_ShowTargetEyeOption.target = (this.targetEyeValue != 3 || PlayerSettings.virtualRealitySupported);
			this.settings.DrawClearFlags();
			using (new EditorGUILayout.FadeGroupScope(this.m_ShowBGColorOptions.faded))
			{
				this.settings.DrawBackgroundColor();
			}
			this.settings.DrawCullingMask();
			EditorGUILayout.Space();
			this.settings.DrawProjection();
			this.settings.DrawClippingPlanes();
			this.settings.DrawNormalizedViewPort();
			EditorGUILayout.Space();
			this.settings.DrawDepth();
			this.settings.DrawRenderingPath();
			if (this.m_ShowOrthoOptions.target && this.wantDeferredRendering)
			{
				EditorGUILayout.HelpBox("Deferred rendering does not work with Orthographic camera, will use Forward.", MessageType.Warning, true);
			}
			this.settings.DrawTargetTexture(this.wantDeferredRendering);
			this.settings.DrawOcclusionCulling();
			this.settings.DrawHDR();
			this.settings.DrawMSAA();
			this.settings.DrawDynamicResolution();
			this.DisplayCameraWarnings();
			this.settings.DrawVR();
			this.settings.DrawMultiDisplay();
			using (new EditorGUILayout.FadeGroupScope(this.m_ShowTargetEyeOption.faded))
			{
				this.settings.DrawTargetEye();
			}
			this.DepthTextureModeGUI();
			this.CommandBufferGUI();
			base.serializedObject.ApplyModifiedProperties();
		}

		private void DisplayCameraWarnings()
		{
			Camera camera = base.target as Camera;
			if (camera != null)
			{
				string[] cameraBufferWarnings = camera.GetCameraBufferWarnings();
				if (cameraBufferWarnings.Length > 0)
				{
					EditorGUILayout.HelpBox(string.Join("\n\n", cameraBufferWarnings), MessageType.Warning, true);
				}
			}
		}

		public virtual void OnOverlayGUI(UnityEngine.Object target, SceneView sceneView)
		{
			if (!(target == null))
			{
				Camera camera = (Camera)target;
				Vector2 mainGameViewTargetSize = GameView.GetMainGameViewTargetSize();
				if (mainGameViewTargetSize.x < 0f)
				{
					mainGameViewTargetSize.x = sceneView.position.width;
					mainGameViewTargetSize.y = sceneView.position.height;
				}
				Rect rect = camera.rect;
				mainGameViewTargetSize.x *= Mathf.Max(rect.width, 0f);
				mainGameViewTargetSize.y *= Mathf.Max(rect.height, 0f);
				if (mainGameViewTargetSize.x > 0f && mainGameViewTargetSize.y > 0f)
				{
					float num = mainGameViewTargetSize.x / mainGameViewTargetSize.y;
					mainGameViewTargetSize.y = 0.2f * sceneView.position.height;
					mainGameViewTargetSize.x = mainGameViewTargetSize.y * num;
					if (mainGameViewTargetSize.y > sceneView.position.height * 0.5f)
					{
						mainGameViewTargetSize.y = sceneView.position.height * 0.5f;
						mainGameViewTargetSize.x = mainGameViewTargetSize.y * num;
					}
					if (mainGameViewTargetSize.x > sceneView.position.width * 0.5f)
					{
						mainGameViewTargetSize.x = sceneView.position.width * 0.5f;
						mainGameViewTargetSize.y = mainGameViewTargetSize.x / num;
					}
					Rect rect2 = GUILayoutUtility.GetRect(mainGameViewTargetSize.x, mainGameViewTargetSize.y);
					if (Event.current.type == EventType.Repaint)
					{
						this.previewCamera.CopyFrom(camera);
						Skybox component = this.previewCamera.GetComponent<Skybox>();
						if (component)
						{
							Skybox component2 = camera.GetComponent<Skybox>();
							if (component2 && component2.enabled)
							{
								component.enabled = true;
								component.material = component2.material;
							}
							else
							{
								component.enabled = false;
							}
						}
						RenderTexture previewTextureWithSize = this.GetPreviewTextureWithSize((int)rect2.width, (int)rect2.height);
						previewTextureWithSize.antiAliasing = QualitySettings.antiAliasing;
						this.previewCamera.targetTexture = previewTextureWithSize;
						this.previewCamera.pixelRect = new Rect(0f, 0f, rect2.width, rect2.height);
						Handles.EmitGUIGeometryForCamera(camera, this.previewCamera);
						GL.sRGBWrite = (QualitySettings.activeColorSpace == ColorSpace.Linear);
						this.previewCamera.Render();
						GL.sRGBWrite = false;
						Graphics.DrawTexture(rect2, previewTextureWithSize, new Rect(0f, 0f, 1f, 1f), 0, 0, 0, 0, GUI.color, EditorGUIUtility.GUITextureBlit2SRGBMaterial);
					}
				}
			}
		}

		private RenderTexture GetPreviewTextureWithSize(int width, int height)
		{
			if (this.m_PreviewTexture == null || this.m_PreviewTexture.width != width || this.m_PreviewTexture.height != height)
			{
				this.m_PreviewTexture = new RenderTexture(width, height, 24, RenderTextureFormat.Default, RenderTextureReadWrite.Linear);
			}
			return this.m_PreviewTexture;
		}

		[RequiredByNativeCode]
		private static float GetGameViewAspectRatio()
		{
			Vector2 mainGameViewTargetSize = GameView.GetMainGameViewTargetSize();
			if (mainGameViewTargetSize.x < 0f)
			{
				mainGameViewTargetSize.x = (float)Screen.width;
				mainGameViewTargetSize.y = (float)Screen.height;
			}
			return mainGameViewTargetSize.x / mainGameViewTargetSize.y;
		}

		private static float GetFrustumAspectRatio(Camera camera)
		{
			Rect rect = camera.rect;
			float result;
			if (rect.width <= 0f || rect.height <= 0f)
			{
				result = -1f;
			}
			else
			{
				float num = rect.width / rect.height;
				result = CameraEditor.GetGameViewAspectRatio() * num;
			}
			return result;
		}

		private static bool GetFrustum(Camera camera, Vector3[] near, Vector3[] far, out float frustumAspect)
		{
			frustumAspect = CameraEditor.GetFrustumAspectRatio(camera);
			bool result;
			if (frustumAspect < 0f)
			{
				result = false;
			}
			else
			{
				if (far != null)
				{
					far[0] = new Vector3(0f, 0f, camera.farClipPlane);
					far[1] = new Vector3(0f, 1f, camera.farClipPlane);
					far[2] = new Vector3(1f, 1f, camera.farClipPlane);
					far[3] = new Vector3(1f, 0f, camera.farClipPlane);
					for (int i = 0; i < 4; i++)
					{
						far[i] = camera.ViewportToWorldPoint(far[i]);
					}
				}
				if (near != null)
				{
					near[0] = new Vector3(0f, 0f, camera.nearClipPlane);
					near[1] = new Vector3(0f, 1f, camera.nearClipPlane);
					near[2] = new Vector3(1f, 1f, camera.nearClipPlane);
					near[3] = new Vector3(1f, 0f, camera.nearClipPlane);
					for (int j = 0; j < 4; j++)
					{
						near[j] = camera.ViewportToWorldPoint(near[j]);
					}
				}
				result = true;
			}
			return result;
		}

		internal static void RenderGizmo(Camera camera)
		{
			Vector3[] array = new Vector3[4];
			Vector3[] array2 = new Vector3[4];
			float num;
			if (CameraEditor.GetFrustum(camera, array, array2, out num))
			{
				Color color = Handles.color;
				Handles.color = CameraEditor.kGizmoCamera;
				for (int i = 0; i < 4; i++)
				{
					Handles.DrawLine(array[i], array[(i + 1) % 4]);
					Handles.DrawLine(array2[i], array2[(i + 1) % 4]);
					Handles.DrawLine(array[i], array2[i]);
				}
				Handles.color = color;
			}
		}

		private static bool IsViewPortRectValidToRender(Rect normalizedViewPortRect)
		{
			return normalizedViewPortRect.width > 0f && normalizedViewPortRect.height > 0f && normalizedViewPortRect.x < 1f && normalizedViewPortRect.xMax > 0f && normalizedViewPortRect.y < 1f && normalizedViewPortRect.yMax > 0f;
		}

		public virtual void OnSceneGUI()
		{
			Camera camera = (Camera)base.target;
			if (CameraEditor.IsViewPortRectValidToRender(camera.rect))
			{
				SceneViewOverlay.Window(EditorGUIUtility.TrTextContent("Camera Preview", null, null), new SceneViewOverlay.WindowFunction(this.OnOverlayGUI), -100, base.target, SceneViewOverlay.WindowDisplayOption.OneWindowPerTarget);
				Color color = Handles.color;
				Color color2 = CameraEditor.kGizmoCamera;
				color2.a *= 2f;
				Handles.color = color2;
				Vector3[] array = new Vector3[4];
				float num;
				if (CameraEditor.GetFrustum(camera, null, array, out num))
				{
					Vector3 vector = array[0];
					Vector3 vector2 = array[1];
					Vector3 vector3 = array[2];
					Vector3 vector4 = array[3];
					bool changed = GUI.changed;
					Vector3 vector5 = Vector3.Lerp(vector, vector3, 0.5f);
					float num2 = -1f;
					Vector3 a = CameraEditor.MidPointPositionSlider(vector2, vector3, camera.transform.up);
					if (!GUI.changed)
					{
						a = CameraEditor.MidPointPositionSlider(vector, vector4, -camera.transform.up);
					}
					if (GUI.changed)
					{
						num2 = (a - vector5).magnitude;
					}
					GUI.changed = false;
					a = CameraEditor.MidPointPositionSlider(vector4, vector3, camera.transform.right);
					if (!GUI.changed)
					{
						a = CameraEditor.MidPointPositionSlider(vector, vector2, -camera.transform.right);
					}
					if (GUI.changed)
					{
						num2 = (a - vector5).magnitude / num;
					}
					if (num2 >= 0f)
					{
						Undo.RecordObject(camera, "Adjust Camera");
						if (camera.orthographic)
						{
							camera.orthographicSize = num2;
						}
						else
						{
							Vector3 a2 = vector5 + camera.transform.up * num2;
							camera.fieldOfView = Vector3.Angle(camera.transform.forward, a2 - camera.transform.position) * 2f;
						}
						changed = true;
					}
					GUI.changed = changed;
					Handles.color = color;
				}
			}
		}

		private static Vector3 MidPointPositionSlider(Vector3 position1, Vector3 position2, Vector3 direction)
		{
			Vector3 vector = Vector3.Lerp(position1, position2, 0.5f);
			Vector3 arg_3E_0 = vector;
			float arg_3E_2 = HandleUtility.GetHandleSize(vector) * 0.03f;
			if (CameraEditor.<>f__mg$cache0 == null)
			{
				CameraEditor.<>f__mg$cache0 = new Handles.CapFunction(Handles.DotHandleCap);
			}
			return Handles.Slider(arg_3E_0, direction, arg_3E_2, CameraEditor.<>f__mg$cache0, 0f);
		}
	}
}
