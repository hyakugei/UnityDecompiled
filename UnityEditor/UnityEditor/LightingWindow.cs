using System;
using System.Collections.Generic;
using System.Text;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityEditor
{
	[EditorWindowTitle(title = "Lighting", icon = "Lighting")]
	internal class LightingWindow : EditorWindow
	{
		private enum Mode
		{
			LightingSettings,
			OutputMaps,
			ObjectSettings
		}

		private enum BakeMode
		{
			BakeReflectionProbes,
			Clear
		}

		private static class Styles
		{
			public static readonly GUIContent[] ModeToggles = new GUIContent[]
			{
				EditorGUIUtility.TrTextContent("Scene", null, null),
				EditorGUIUtility.TrTextContent("Global Maps", null, null),
				EditorGUIUtility.TrTextContent("Object Maps", null, null)
			};

			public static readonly GUIContent ContinuousBakeLabel = EditorGUIUtility.TrTextContent("Auto Generate", "Automatically generates lighting data in the Scene when any changes are made to the lighting systems.", null);

			public static readonly GUIContent BuildLabel = EditorGUIUtility.TrTextContent("Generate Lighting", "Generates the lightmap data for the current master scene.  This lightmap data (for realtime and baked global illumination) is stored in the GI Cache. For GI Cache settings see the Preferences panel.", null);

			public static readonly GUIStyle LabelStyle = EditorStyles.wordWrappedMiniLabel;

			public static readonly GUIStyle ToolbarStyle = "preToolbar";

			public static readonly GUIStyle ToolbarTitleStyle = "preToolbar";

			public static readonly GUIStyle ButtonStyle = "LargeButton";
		}

		public const float kButtonWidth = 150f;

		private static string[] s_BakeModeOptions = new string[]
		{
			"Bake Reflection Probes",
			"Clear Baked Data"
		};

		private const string kGlobalIlluminationUnityManualPage = "file:///unity/Manual/GlobalIllumination.html";

		private LightingWindow.Mode m_Mode = LightingWindow.Mode.LightingSettings;

		private Vector2 m_ScrollPositionLighting = Vector2.zero;

		private Vector2 m_ScrollPositionOutputMaps = Vector2.zero;

		private LightingWindowObjectTab m_ObjectTab;

		public LightingWindowLightingTab m_LightingTab;

		private LightingWindowLightmapPreviewTab m_LightmapPreviewTab;

		private Scene m_LastActiveScene;

		private SerializedObject m_LightmapSettings;

		private SerializedProperty m_WorkflowMode;

		private SerializedProperty m_EnabledBakedGI;

		private PreviewResizer m_PreviewResizer = new PreviewResizer();

		private static bool s_IsVisible = false;

		private float m_ToolbarPadding = -1f;

		private float toolbarPadding
		{
			get
			{
				if (this.m_ToolbarPadding == -1f)
				{
					this.m_ToolbarPadding = EditorStyles.iconButton.CalcSize(EditorGUI.GUIContents.helpIcon).x * 2f + 6f;
				}
				return this.m_ToolbarPadding;
			}
		}

		private void OnEnable()
		{
			base.titleContent = base.GetLocalizedTitleContent();
			this.m_LightingTab = new LightingWindowLightingTab();
			this.m_LightingTab.OnEnable();
			this.m_LightmapPreviewTab = new LightingWindowLightmapPreviewTab();
			this.m_ObjectTab = new LightingWindowObjectTab();
			this.m_ObjectTab.OnEnable(this);
			this.InitLightmapSettings();
			base.autoRepaintOnSceneChange = false;
			this.m_PreviewResizer.Init("LightmappingPreview");
			EditorApplication.searchChanged = (EditorApplication.CallbackFunction)Delegate.Combine(EditorApplication.searchChanged, new EditorApplication.CallbackFunction(base.Repaint));
			base.Repaint();
		}

		private void OnDisable()
		{
			this.m_LightingTab.OnDisable();
			this.m_ObjectTab.OnDisable();
			EditorApplication.searchChanged = (EditorApplication.CallbackFunction)Delegate.Remove(EditorApplication.searchChanged, new EditorApplication.CallbackFunction(base.Repaint));
		}

		private void OnBecameVisible()
		{
			if (!LightingWindow.s_IsVisible)
			{
				LightingWindow.s_IsVisible = true;
				LightingWindow.RepaintSceneAndGameViews();
			}
		}

		private void OnBecameInvisible()
		{
			LightingWindow.s_IsVisible = false;
			LightingWindow.RepaintSceneAndGameViews();
		}

		private void OnSelectionChange()
		{
			this.m_LightmapPreviewTab.UpdateLightmapSelection();
			base.Repaint();
		}

		internal static void RepaintSceneAndGameViews()
		{
			SceneView.RepaintAll();
			GameView.RepaintAll();
		}

		private void OnGUI()
		{
			this.InitLightmapSettings();
			this.m_LightmapSettings.Update();
			EditorGUILayout.Space();
			EditorGUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.Space(this.toolbarPadding);
			this.ModeToggle();
			this.DrawHelpGUI();
			if (this.m_Mode == LightingWindow.Mode.LightingSettings)
			{
				this.DrawSettingsGUI();
			}
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Space();
			LightingWindow.Mode mode = this.m_Mode;
			if (mode != LightingWindow.Mode.LightingSettings)
			{
				if (mode != LightingWindow.Mode.OutputMaps)
				{
					if (mode != LightingWindow.Mode.ObjectSettings)
					{
					}
				}
				else
				{
					this.m_ScrollPositionOutputMaps = EditorGUILayout.BeginScrollView(this.m_ScrollPositionOutputMaps, new GUILayoutOption[0]);
					this.m_LightmapPreviewTab.Maps();
					EditorGUILayout.EndScrollView();
					EditorGUILayout.Space();
				}
			}
			else
			{
				this.m_ScrollPositionLighting = EditorGUILayout.BeginScrollView(this.m_ScrollPositionLighting, new GUILayoutOption[0]);
				this.m_LightingTab.OnGUI();
				EditorGUILayout.EndScrollView();
				EditorGUILayout.Space();
			}
			this.Buttons();
			this.Summary();
			this.PreviewSection();
			this.m_LightmapSettings.ApplyModifiedProperties();
		}

		private void InitLightmapSettings()
		{
			Scene activeScene = SceneManager.GetActiveScene();
			if (this.m_LastActiveScene != activeScene)
			{
				if (this.m_LightmapSettings != null)
				{
					this.m_LightmapSettings.Dispose();
					this.m_EnabledBakedGI.Dispose();
					this.m_WorkflowMode.Dispose();
				}
				this.m_LastActiveScene = activeScene;
				this.m_LightmapSettings = new SerializedObject(LightmapEditorSettings.GetLightmapSettings());
				this.m_EnabledBakedGI = this.m_LightmapSettings.FindProperty("m_GISettings.m_EnableBakedLightmaps");
				this.m_WorkflowMode = this.m_LightmapSettings.FindProperty("m_GIWorkflowMode");
			}
		}

		private void DrawHelpGUI()
		{
			Vector2 vector = EditorStyles.iconButton.CalcSize(EditorGUI.GUIContents.helpIcon);
			Rect rect = GUILayoutUtility.GetRect(vector.x, vector.y);
			if (GUI.Button(rect, EditorGUI.GUIContents.helpIcon, EditorStyles.iconButton))
			{
				Help.ShowHelpPage("file:///unity/Manual/GlobalIllumination.html");
			}
		}

		private void DrawSettingsGUI()
		{
			Vector2 vector = EditorStyles.iconButton.CalcSize(EditorGUI.GUIContents.titleSettingsIcon);
			Rect rect = GUILayoutUtility.GetRect(vector.x, vector.y);
			if (EditorGUI.DropdownButton(rect, EditorGUI.GUIContents.titleSettingsIcon, FocusType.Passive, EditorStyles.iconButton))
			{
				EditorUtility.DisplayCustomMenu(rect, new GUIContent[]
				{
					EditorGUIUtility.TrTextContent("Reset", null, null)
				}, -1, new EditorUtility.SelectMenuItemFunction(this.ResetSettings), null);
			}
		}

		private void ResetSettings(object userData, string[] options, int selected)
		{
			RenderSettings.Reset();
			LightmapEditorSettings.Reset();
			LightmapSettings.Reset();
		}

		private void PreviewSection()
		{
			if (this.m_Mode == LightingWindow.Mode.OutputMaps)
			{
				EditorGUILayout.BeginHorizontal(GUIContent.none, LightingWindow.Styles.ToolbarStyle, new GUILayoutOption[]
				{
					GUILayout.Height(17f)
				});
				GUILayout.FlexibleSpace();
				GUI.Label(GUILayoutUtility.GetLastRect(), "Preview", LightingWindow.Styles.ToolbarTitleStyle);
				EditorGUILayout.EndHorizontal();
			}
			LightingWindow.Mode mode = this.m_Mode;
			if (mode != LightingWindow.Mode.OutputMaps)
			{
				if (mode == LightingWindow.Mode.ObjectSettings)
				{
					int num = (LightmapEditorSettings.lightmapper != LightmapEditorSettings.Lightmapper.PathTracer) ? 115 : 185;
					Rect r = new Rect(0f, (float)num, base.position.width, base.position.height - (float)num);
					if (Selection.activeGameObject)
					{
						this.m_ObjectTab.ObjectPreview(r);
					}
				}
			}
			else
			{
				float num2 = this.m_PreviewResizer.ResizeHandle(base.position, 100f, 250f, 17f);
				Rect r2 = new Rect(0f, base.position.height - num2, base.position.width, num2);
				if (num2 > 0f)
				{
					this.m_LightmapPreviewTab.LightmapPreview(r2);
				}
			}
		}

		private void ModeToggle()
		{
			EditorGUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.FlexibleSpace();
			this.m_Mode = (LightingWindow.Mode)GUILayout.Toolbar((int)this.m_Mode, LightingWindow.Styles.ModeToggles, LightingWindow.Styles.ButtonStyle, GUI.ToolbarButtonSize.FitToContents, new GUILayoutOption[0]);
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
		}

		private void BakeDropDownCallback(object data)
		{
			LightingWindow.BakeMode bakeMode = (LightingWindow.BakeMode)data;
			if (bakeMode != LightingWindow.BakeMode.Clear)
			{
				if (bakeMode == LightingWindow.BakeMode.BakeReflectionProbes)
				{
					this.DoBakeReflectionProbes();
				}
			}
			else
			{
				this.DoClear();
			}
		}

		private void Buttons()
		{
			bool enabled = GUI.enabled;
			GUI.enabled &= !EditorApplication.isPlayingOrWillChangePlaymode;
			if (Lightmapping.lightingDataAsset && !Lightmapping.lightingDataAsset.isValid)
			{
				EditorGUILayout.HelpBox(Lightmapping.lightingDataAsset.validityErrorMessage, MessageType.Warning);
			}
			EditorGUILayout.Space();
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.FlexibleSpace();
			Rect rect = GUILayoutUtility.GetRect(LightingWindow.Styles.ContinuousBakeLabel, GUIStyle.none);
			EditorGUI.BeginProperty(rect, LightingWindow.Styles.ContinuousBakeLabel, this.m_WorkflowMode);
			bool flag = this.m_WorkflowMode.intValue == 0;
			EditorGUI.BeginChangeCheck();
			flag = GUILayout.Toggle(flag, LightingWindow.Styles.ContinuousBakeLabel, new GUILayoutOption[0]);
			if (EditorGUI.EndChangeCheck())
			{
				this.m_WorkflowMode.intValue = ((!flag) ? 1 : 0);
			}
			EditorGUI.EndProperty();
			using (new EditorGUI.DisabledScope(flag))
			{
				bool flag2 = flag || !Lightmapping.isRunning;
				if (flag2)
				{
					if (EditorGUI.ButtonWithDropdownList(LightingWindow.Styles.BuildLabel, LightingWindow.s_BakeModeOptions, new GenericMenu.MenuFunction2(this.BakeDropDownCallback), new GUILayoutOption[]
					{
						GUILayout.Width(170f)
					}))
					{
						this.DoBake();
						GUIUtility.ExitGUI();
					}
				}
				else
				{
					if (LightmapEditorSettings.lightmapper == LightmapEditorSettings.Lightmapper.PathTracer && this.m_EnabledBakedGI.boolValue && GUILayout.Button("Force Stop", new GUILayoutOption[]
					{
						GUILayout.Width(150f)
					}))
					{
						Lightmapping.ForceStop();
					}
					if (GUILayout.Button("Cancel", new GUILayoutOption[]
					{
						GUILayout.Width(150f)
					}))
					{
						Lightmapping.Cancel();
						UsabilityAnalytics.Track("/LightMapper/Cancel");
					}
				}
			}
			GUILayout.EndHorizontal();
			EditorGUILayout.Space();
			GUI.enabled = enabled;
		}

		private void DoBake()
		{
			UsabilityAnalytics.Track("/LightMapper/Start");
			UsabilityAnalytics.Event("LightMapper", "Mode", LightmapSettings.lightmapsMode.ToString(), 1);
			UsabilityAnalytics.Event("LightMapper", "Button", "BakeScene", 1);
			Lightmapping.BakeAsync();
		}

		private void DoClear()
		{
			Lightmapping.ClearLightingDataAsset();
			Lightmapping.Clear();
			UsabilityAnalytics.Track("/LightMapper/Clear");
		}

		private void DoBakeReflectionProbes()
		{
			Lightmapping.BakeAllReflectionProbesSnapshots();
			UsabilityAnalytics.Track("/LightMapper/BakeAllReflectionProbesSnapshots");
		}

		private void Summary()
		{
			GUILayout.BeginVertical(EditorStyles.helpBox, new GUILayoutOption[0]);
			long num = 0L;
			int num2 = 0;
			Dictionary<Vector2, int> dictionary = new Dictionary<Vector2, int>();
			bool flag = false;
			bool flag2 = false;
			LightmapData[] lightmaps = LightmapSettings.lightmaps;
			for (int i = 0; i < lightmaps.Length; i++)
			{
				LightmapData lightmapData = lightmaps[i];
				if (!(lightmapData.lightmapColor == null))
				{
					num2++;
					Vector2 vector = new Vector2((float)lightmapData.lightmapColor.width, (float)lightmapData.lightmapColor.height);
					if (dictionary.ContainsKey(vector))
					{
						Dictionary<Vector2, int> dictionary2;
						Vector2 key;
						(dictionary2 = dictionary)[key = vector] = dictionary2[key] + 1;
					}
					else
					{
						dictionary.Add(vector, 1);
					}
					num += TextureUtil.GetStorageMemorySizeLong(lightmapData.lightmapColor);
					if (lightmapData.lightmapDir)
					{
						num += TextureUtil.GetStorageMemorySizeLong(lightmapData.lightmapDir);
						flag = true;
					}
					if (lightmapData.shadowMask)
					{
						num += TextureUtil.GetStorageMemorySizeLong(lightmapData.shadowMask);
						flag2 = true;
					}
				}
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(num2);
			stringBuilder.Append((!flag) ? " Non-Directional" : " Directional");
			stringBuilder.Append(" Lightmap");
			if (num2 != 1)
			{
				stringBuilder.Append("s");
			}
			if (flag2)
			{
				stringBuilder.Append(" with Shadowmask");
				if (num2 != 1)
				{
					stringBuilder.Append("s");
				}
			}
			bool flag3 = true;
			foreach (KeyValuePair<Vector2, int> current in dictionary)
			{
				stringBuilder.Append((!flag3) ? ", " : ": ");
				flag3 = false;
				if (current.Value > 1)
				{
					stringBuilder.Append(current.Value);
					stringBuilder.Append("x");
				}
				stringBuilder.Append(current.Key.x);
				stringBuilder.Append("x");
				stringBuilder.Append(current.Key.y);
				stringBuilder.Append("px");
			}
			stringBuilder.Append(" ");
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.BeginVertical(new GUILayoutOption[0]);
			GUILayout.Label(stringBuilder.ToString(), LightingWindow.Styles.LabelStyle, new GUILayoutOption[0]);
			GUILayout.EndVertical();
			GUILayout.BeginVertical(new GUILayoutOption[0]);
			GUILayout.Label(EditorUtility.FormatBytes(num), LightingWindow.Styles.LabelStyle, new GUILayoutOption[0]);
			GUILayout.Label((num2 != 0) ? "" : "No Lightmaps", LightingWindow.Styles.LabelStyle, new GUILayoutOption[0]);
			GUILayout.EndVertical();
			GUILayout.EndHorizontal();
			if (LightmapEditorSettings.lightmapper != LightmapEditorSettings.Lightmapper.Radiosity)
			{
				GUILayout.BeginVertical(new GUILayoutOption[0]);
				GUILayout.Label("Memory Usage: " + Lightmapping.ComputeTotalMemoryUsageInMB().ToString("0.0") + " MB", LightingWindow.Styles.LabelStyle, new GUILayoutOption[0]);
				GUILayout.Label("Occupied Texels: " + InternalEditorUtility.CountToString(Lightmapping.occupiedTexelCount), LightingWindow.Styles.LabelStyle, new GUILayoutOption[0]);
				if (Lightmapping.isRunning)
				{
					int num3 = 0;
					int num4 = 0;
					int num5 = 0;
					int num6 = 0;
					int num7 = 0;
					int num8 = 0;
					int num9 = 0;
					int num10 = LightmapSettings.lightmaps.Length;
					for (int j = 0; j < num10; j++)
					{
						LightmapConvergence lightmapConvergence = Lightmapping.GetLightmapConvergence(j);
						if (!lightmapConvergence.IsValid())
						{
							num9++;
						}
						else if (Lightmapping.GetVisibleTexelCount(j) > 0uL)
						{
							num3++;
							if (lightmapConvergence.IsConverged())
							{
								num4++;
							}
							else
							{
								num5++;
							}
						}
						else
						{
							num6++;
							if (lightmapConvergence.IsConverged())
							{
								num7++;
							}
							else
							{
								num8++;
							}
						}
					}
					EditorGUILayout.LabelField("Lightmaps in view: " + num3, LightingWindow.Styles.LabelStyle, new GUILayoutOption[0]);
					EditorGUI.indentLevel++;
					EditorGUILayout.LabelField("Converged: " + num4, LightingWindow.Styles.LabelStyle, new GUILayoutOption[0]);
					EditorGUILayout.LabelField("Not Converged: " + num5, LightingWindow.Styles.LabelStyle, new GUILayoutOption[0]);
					EditorGUI.indentLevel--;
					EditorGUILayout.LabelField("Lightmaps not in view: " + num6, LightingWindow.Styles.LabelStyle, new GUILayoutOption[0]);
					EditorGUI.indentLevel++;
					EditorGUILayout.LabelField("Converged: " + num7, LightingWindow.Styles.LabelStyle, new GUILayoutOption[0]);
					EditorGUILayout.LabelField("Not Converged: " + num8, LightingWindow.Styles.LabelStyle, new GUILayoutOption[0]);
					EditorGUI.indentLevel--;
				}
				float lightmapBakeTimeTotal = Lightmapping.GetLightmapBakeTimeTotal();
				float lightmapBakePerformanceTotal = Lightmapping.GetLightmapBakePerformanceTotal();
				if ((double)lightmapBakePerformanceTotal >= 0.0)
				{
					GUILayout.Label("Bake Performance: " + lightmapBakePerformanceTotal.ToString("0.00") + " mrays/sec", LightingWindow.Styles.LabelStyle, new GUILayoutOption[0]);
				}
				if (!Lightmapping.isRunning)
				{
					float lightmapBakeTimeRaw = Lightmapping.GetLightmapBakeTimeRaw();
					if ((double)lightmapBakeTimeTotal >= 0.0)
					{
						int num11 = (int)lightmapBakeTimeTotal;
						int num12 = num11 / 3600;
						num11 -= 3600 * num12;
						int num13 = num11 / 60;
						num11 -= 60 * num13;
						int num14 = num11;
						int num15 = (int)lightmapBakeTimeRaw;
						int num16 = num15 / 3600;
						num15 -= 3600 * num16;
						int num17 = num15 / 60;
						num15 -= 60 * num17;
						int num18 = num15;
						int num19 = Math.Max(0, (int)(lightmapBakeTimeTotal - lightmapBakeTimeRaw));
						int num20 = num19 / 3600;
						num19 -= 3600 * num20;
						int num21 = num19 / 60;
						num19 -= 60 * num21;
						int num22 = num19;
						GUILayout.Label(string.Concat(new string[]
						{
							"Total Bake Time: ",
							num12.ToString("0"),
							":",
							num13.ToString("00"),
							":",
							num14.ToString("00")
						}), LightingWindow.Styles.LabelStyle, new GUILayoutOption[0]);
						if (Unsupported.IsDeveloperMode())
						{
							GUILayout.Label(string.Concat(new string[]
							{
								"(Raw Bake Time: ",
								num16.ToString("0"),
								":",
								num17.ToString("00"),
								":",
								num18.ToString("00"),
								", Overhead: ",
								num20.ToString("0"),
								":",
								num21.ToString("00"),
								":",
								num22.ToString("00"),
								")"
							}), LightingWindow.Styles.LabelStyle, new GUILayoutOption[0]);
						}
					}
				}
				GUILayout.EndVertical();
			}
			GUILayout.EndVertical();
		}

		[MenuItem("Window/Lighting/Settings", false, 2098)]
		private static void CreateLightingWindow()
		{
			LightingWindow window = EditorWindow.GetWindow<LightingWindow>();
			window.minSize = new Vector2(360f, 390f);
			window.Show();
		}
	}
}
