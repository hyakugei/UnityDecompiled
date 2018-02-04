using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

namespace UnityEditor
{
	internal class SceneRenderModeWindow : PopupWindowContent
	{
		private class Styles
		{
			public static GUIStyle s_MenuItem;

			public static GUIStyle s_Separator;

			public static readonly string kShadingMode = "Shading Mode";

			public static readonly string kMiscellaneous = "Miscellaneous";

			public static readonly string kDeferred = "Deferred";

			public static readonly string kGlobalIllumination = "Global Illumination";

			public static readonly string kRealtimeGI = "Realtime Global Illumination";

			public static readonly string kBakedGI = "Baked Global Illumination";

			public static readonly string kMaterialValidation = "Material Validation";

			public static readonly GUIContent sResolutionToggle = EditorGUIUtility.TextContent("Show Lightmap Resolution");

			public static readonly SceneView.CameraMode[] sBuiltinCameraModes = new SceneView.CameraMode[]
			{
				new SceneView.CameraMode(DrawCameraMode.Textured, "Shaded", SceneRenderModeWindow.Styles.kShadingMode),
				new SceneView.CameraMode(DrawCameraMode.Wireframe, "Wireframe", SceneRenderModeWindow.Styles.kShadingMode),
				new SceneView.CameraMode(DrawCameraMode.TexturedWire, "Shaded Wireframe", SceneRenderModeWindow.Styles.kShadingMode),
				new SceneView.CameraMode(DrawCameraMode.ShadowCascades, "Shadow Cascades", SceneRenderModeWindow.Styles.kMiscellaneous),
				new SceneView.CameraMode(DrawCameraMode.RenderPaths, "Render Paths", SceneRenderModeWindow.Styles.kMiscellaneous),
				new SceneView.CameraMode(DrawCameraMode.AlphaChannel, "Alpha Channel", SceneRenderModeWindow.Styles.kMiscellaneous),
				new SceneView.CameraMode(DrawCameraMode.Overdraw, "Overdraw", SceneRenderModeWindow.Styles.kMiscellaneous),
				new SceneView.CameraMode(DrawCameraMode.Mipmaps, "Mipmaps", SceneRenderModeWindow.Styles.kMiscellaneous),
				new SceneView.CameraMode(DrawCameraMode.SpriteMask, "Sprite Mask", SceneRenderModeWindow.Styles.kMiscellaneous),
				new SceneView.CameraMode(DrawCameraMode.DeferredDiffuse, "Albedo", SceneRenderModeWindow.Styles.kDeferred),
				new SceneView.CameraMode(DrawCameraMode.DeferredSpecular, "Specular", SceneRenderModeWindow.Styles.kDeferred),
				new SceneView.CameraMode(DrawCameraMode.DeferredSmoothness, "Smoothness", SceneRenderModeWindow.Styles.kDeferred),
				new SceneView.CameraMode(DrawCameraMode.DeferredNormal, "Normal", SceneRenderModeWindow.Styles.kDeferred),
				new SceneView.CameraMode(DrawCameraMode.Systems, "Systems", SceneRenderModeWindow.Styles.kGlobalIllumination),
				new SceneView.CameraMode(DrawCameraMode.Clustering, "Clustering", SceneRenderModeWindow.Styles.kGlobalIllumination),
				new SceneView.CameraMode(DrawCameraMode.LitClustering, "Lit Clustering", SceneRenderModeWindow.Styles.kGlobalIllumination),
				new SceneView.CameraMode(DrawCameraMode.RealtimeCharting, "UV Charts", SceneRenderModeWindow.Styles.kGlobalIllumination),
				new SceneView.CameraMode(DrawCameraMode.RealtimeAlbedo, "Albedo", SceneRenderModeWindow.Styles.kRealtimeGI),
				new SceneView.CameraMode(DrawCameraMode.RealtimeEmissive, "Emissive", SceneRenderModeWindow.Styles.kRealtimeGI),
				new SceneView.CameraMode(DrawCameraMode.RealtimeIndirect, "Indirect", SceneRenderModeWindow.Styles.kRealtimeGI),
				new SceneView.CameraMode(DrawCameraMode.RealtimeDirectionality, "Directionality", SceneRenderModeWindow.Styles.kRealtimeGI),
				new SceneView.CameraMode(DrawCameraMode.BakedLightmap, "Baked Lightmap", SceneRenderModeWindow.Styles.kBakedGI),
				new SceneView.CameraMode(DrawCameraMode.BakedDirectionality, "Directionality", SceneRenderModeWindow.Styles.kBakedGI),
				new SceneView.CameraMode(DrawCameraMode.ShadowMasks, "Shadowmask", SceneRenderModeWindow.Styles.kBakedGI),
				new SceneView.CameraMode(DrawCameraMode.BakedAlbedo, "Albedo", SceneRenderModeWindow.Styles.kBakedGI),
				new SceneView.CameraMode(DrawCameraMode.BakedEmissive, "Emissive", SceneRenderModeWindow.Styles.kBakedGI),
				new SceneView.CameraMode(DrawCameraMode.BakedCharting, "UV Charts", SceneRenderModeWindow.Styles.kBakedGI),
				new SceneView.CameraMode(DrawCameraMode.BakedTexelValidity, "Texel Validity", SceneRenderModeWindow.Styles.kBakedGI),
				new SceneView.CameraMode(DrawCameraMode.BakedUVOverlap, "UV Overlap", SceneRenderModeWindow.Styles.kBakedGI),
				new SceneView.CameraMode(DrawCameraMode.BakedIndices, "Lightmap Indices", SceneRenderModeWindow.Styles.kBakedGI),
				new SceneView.CameraMode(DrawCameraMode.LightOverlap, "Light Overlap", SceneRenderModeWindow.Styles.kBakedGI),
				new SceneView.CameraMode(DrawCameraMode.ValidateAlbedo, "Validate Albedo", SceneRenderModeWindow.Styles.kMaterialValidation),
				new SceneView.CameraMode(DrawCameraMode.ValidateMetalSpecular, "Validate Metal Specular", SceneRenderModeWindow.Styles.kMaterialValidation)
			};

			public static GUIStyle sMenuItem
			{
				get
				{
					if (SceneRenderModeWindow.Styles.s_MenuItem == null)
					{
						SceneRenderModeWindow.Styles.s_MenuItem = "MenuItem";
					}
					return SceneRenderModeWindow.Styles.s_MenuItem;
				}
			}

			public static GUIStyle sSeparator
			{
				get
				{
					if (SceneRenderModeWindow.Styles.s_Separator == null)
					{
						SceneRenderModeWindow.Styles.s_Separator = "sv_iconselector_sep";
					}
					return SceneRenderModeWindow.Styles.s_Separator;
				}
			}
		}

		private const float kSeparatorHeight = 3f;

		private const float kFrameWidth = 1f;

		private const float kHeaderHorizontalPadding = 5f;

		private const float kHeaderVerticalPadding = 1f;

		private const float kShowLightmapResolutionHeight = 22f;

		private const float kTogglePadding = 7f;

		private readonly SceneView m_SceneView;

		private float windowHeight
		{
			get
			{
				int num = (from mode in SceneRenderModeWindow.Styles.sBuiltinCameraModes
				where this.m_SceneView.IsCameraDrawModeEnabled(mode)
				select mode.section).Distinct<string>().Count<string>() + (from mode in SceneView.userDefinedModes
				where this.m_SceneView.IsCameraDrawModeEnabled(mode)
				select mode.section).Distinct<string>().Count<string>();
				int num2 = SceneRenderModeWindow.Styles.sBuiltinCameraModes.Count((SceneView.CameraMode mode) => this.m_SceneView.IsCameraDrawModeEnabled(mode)) + SceneView.userDefinedModes.Count((SceneView.CameraMode mode) => this.m_SceneView.IsCameraDrawModeEnabled(mode));
				int num3 = num - 2;
				return (float)(num + num2) * 16f + 3f * (float)num3 + 22f;
			}
		}

		private float windowWidth
		{
			get
			{
				return 205f;
			}
		}

		public SceneRenderModeWindow(SceneView sceneView)
		{
			this.m_SceneView = sceneView;
		}

		public override Vector2 GetWindowSize()
		{
			return new Vector2(this.windowWidth, this.windowHeight);
		}

		public override void OnGUI(Rect rect)
		{
			if (!(this.m_SceneView == null) && this.m_SceneView.sceneViewState != null)
			{
				if (Event.current.type != EventType.Layout)
				{
					this.Draw(base.editorWindow, rect.width);
					if (Event.current.type == EventType.MouseMove)
					{
						Event.current.Use();
					}
					if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Escape)
					{
						base.editorWindow.Close();
						GUIUtility.ExitGUI();
					}
				}
			}
		}

		private void DrawSeparator(ref Rect rect)
		{
			Rect position = rect;
			position.x += 5f;
			position.y += 3f;
			position.width -= 10f;
			position.height = 3f;
			GUI.Label(position, GUIContent.none, SceneRenderModeWindow.Styles.sSeparator);
			rect.y += 3f;
		}

		private void DrawHeader(ref Rect rect, GUIContent label)
		{
			Rect position = rect;
			position.y += 1f;
			position.x += 5f;
			position.width = EditorStyles.miniLabel.CalcSize(label).x;
			position.height = EditorStyles.miniLabel.CalcSize(label).y;
			GUI.Label(position, label, EditorStyles.miniLabel);
			rect.y += 16f;
		}

		private void Draw(EditorWindow caller, float listElementWidth)
		{
			Rect rect = new Rect(0f, 0f, listElementWidth, 16f);
			bool flag = GraphicsSettings.renderPipelineAsset != null;
			string text = null;
			foreach (SceneView.CameraMode current in (from mode in SceneView.userDefinedModes
			orderby mode.section
			select mode).Concat(SceneRenderModeWindow.Styles.sBuiltinCameraModes))
			{
				if (!flag || current.drawMode == DrawCameraMode.UserDefined || this.m_SceneView.IsCameraDrawModeEnabled(current))
				{
					if (text != current.section)
					{
						if (text != null)
						{
							this.DrawSeparator(ref rect);
						}
						this.DrawHeader(ref rect, EditorGUIUtility.TextContent(current.section));
						text = current.section;
					}
					using (new EditorGUI.DisabledScope(!this.m_SceneView.IsCameraDrawModeEnabled(current)))
					{
						this.DoBuiltinMode(caller, ref rect, current);
					}
				}
			}
			bool disabled = this.m_SceneView.cameraMode.drawMode < DrawCameraMode.RealtimeCharting || !this.IsModeEnabled(this.m_SceneView.cameraMode.drawMode);
			this.DoResolutionToggle(rect, disabled);
		}

		private bool IsModeEnabled(DrawCameraMode mode)
		{
			return this.m_SceneView.IsCameraDrawModeEnabled(SceneRenderModeWindow.GetBuiltinCameraMode(mode));
		}

		private void DoResolutionToggle(Rect rect, bool disabled)
		{
			GUI.Label(new Rect(1f, rect.y, this.windowWidth - 2f, 22f), "", EditorStyles.inspectorBig);
			rect.y += 3f;
			rect.x += 7f;
			using (new EditorGUI.DisabledScope(disabled))
			{
				EditorGUI.BeginChangeCheck();
				bool showResolution = GUI.Toggle(rect, LightmapVisualization.showResolution, SceneRenderModeWindow.Styles.sResolutionToggle);
				if (EditorGUI.EndChangeCheck())
				{
					LightmapVisualization.showResolution = showResolution;
					SceneView.RepaintAll();
				}
			}
		}

		private void DoBuiltinMode(EditorWindow caller, ref Rect rect, SceneView.CameraMode mode)
		{
			using (new EditorGUI.DisabledScope(!this.m_SceneView.CheckDrawModeForRenderingPath(mode.drawMode)))
			{
				EditorGUI.BeginChangeCheck();
				GUI.Toggle(rect, this.m_SceneView.cameraMode == mode, EditorGUIUtility.TextContent(mode.name), SceneRenderModeWindow.Styles.sMenuItem);
				if (EditorGUI.EndChangeCheck())
				{
					this.m_SceneView.cameraMode = mode;
					this.m_SceneView.Repaint();
					GUIUtility.ExitGUI();
				}
				rect.y += 16f;
			}
		}

		public static GUIContent GetGUIContent(DrawCameraMode drawCameraMode)
		{
			GUIContent result;
			if (drawCameraMode == DrawCameraMode.UserDefined)
			{
				result = GUIContent.none;
			}
			else
			{
				result = EditorGUIUtility.TextContent(SceneRenderModeWindow.Styles.sBuiltinCameraModes.Single((SceneView.CameraMode mode) => mode.drawMode == drawCameraMode).name);
			}
			return result;
		}

		internal static SceneView.CameraMode GetBuiltinCameraMode(DrawCameraMode drawMode)
		{
			return SceneRenderModeWindow.Styles.sBuiltinCameraModes.Single((SceneView.CameraMode mode) => mode.drawMode == drawMode);
		}
	}
}
