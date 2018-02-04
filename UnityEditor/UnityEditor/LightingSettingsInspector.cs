using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

namespace UnityEditor
{
	internal class LightingSettingsInspector
	{
		private class Styles
		{
			public GUIContent OptimizeRealtimeUVs = EditorGUIUtility.TrTextContent("Optimize Realtime UVs", "Specifies whether the authored mesh UVs get optimized for Realtime Global Illumination or not. When enabled, the authored UVs can get merged, scaled, and packed for optimisation purposes. When disabled, the authored UVs will get scaled and packed, but not merged.", null);

			public GUIContent IgnoreNormalsForChartDetection = EditorGUIUtility.TrTextContent("Ignore Normals", "When enabled, prevents the UV charts from being split during the precompute process for Realtime Global Illumination lighting.", null);

			public int[] MinimumChartSizeValues = new int[]
			{
				2,
				4
			};

			public GUIContent[] MinimumChartSizeStrings = new GUIContent[]
			{
				EditorGUIUtility.TrTextContent("2 (Minimum)", null, null),
				EditorGUIUtility.TrTextContent("4 (Stitchable)", null, null)
			};

			public GUIContent Lighting = new GUIContent(EditorGUIUtility.TrTextContent("Lighting", null, null).text);

			public GUIContent MinimumChartSize = EditorGUIUtility.TrTextContent("Min Chart Size", "Specifies the minimum texel size used for a UV chart. If stitching is required, a value of 4 will create a chart of 4x4 texels to store lighting and directionality. If stitching is not required, a value of 2 will reduce the texel density and provide better lighting build times and run time performance.", null);

			public GUIContent ImportantGI = EditorGUIUtility.TrTextContent("Prioritize Illumination", "When enabled, the object will be marked as a priority object and always included in lighting calculations. Useful for objects that will be strongly emissive to make sure that other objects will be illuminated by this object.", null);

			public GUIContent StitchLightmapSeams = EditorGUIUtility.TrTextContent("Stitch Seams", "When enabled, seams in baked lightmaps will get smoothed.", null);

			public GUIContent AutoUVMaxDistance = EditorGUIUtility.TrTextContent("Max Distance", "Specifies the maximum worldspace distance to be used for UV chart simplification. If charts are within this distance they will be simplified for optimization purposes.", null);

			public GUIContent AutoUVMaxAngle = EditorGUIUtility.TrTextContent("Max Angle", "Specifies the maximum angle in degrees between faces sharing a UV edge. If the angle between the faces is below this value, the UV charts will be simplified.", null);

			public GUIContent LightmapParameters = EditorGUIUtility.TrTextContent("Lightmap Parameters", "Allows the adjustment of advanced parameters that affect the process of generating a lightmap for an object using global illumination.", null);

			public GUIContent AtlasTilingX = EditorGUIUtility.TrTextContent("Tiling X", null, null);

			public GUIContent AtlasTilingY = EditorGUIUtility.TrTextContent("Tiling Y", null, null);

			public GUIContent AtlasOffsetX = EditorGUIUtility.TrTextContent("Offset X", null, null);

			public GUIContent AtlasOffsetY = EditorGUIUtility.TrTextContent("Offset Y", null, null);

			public GUIContent ClampedSize = EditorGUIUtility.TrTextContent("Object's size in lightmap has reached the max atlas size.", "If you need higher resolution for this object, divide it into smaller meshes or set higher max atlas size via the LightmapEditorSettings class.", null);

			public GUIContent ClampedPackingResolution = EditorGUIUtility.TrTextContent("Object's size in the realtime lightmap has reached the maximum size. If you need higher resolution for this object, divide it into smaller meshes.", null, null);

			public GUIContent ZeroAreaPackingMesh = EditorGUIUtility.TrTextContent("Mesh used by the renderer has zero UV or surface area. Non zero area is required for lightmapping.", null, null);

			public GUIContent NoNormalsNoLightmapping = EditorGUIUtility.TrTextContent("Mesh used by the renderer doesn't have normals. Normals are needed for lightmapping.", null, null);

			public GUIContent UVOverlap = EditorGUIUtility.TrTextContent("This GameObject has overlapping UVs. Please enable 'Generate Lightmap UVs' on the Asset or fix in your modelling package.", null, null);

			public GUIContent Atlas = EditorGUIUtility.TrTextContent("Baked Lightmap", null, null);

			public GUIContent RealtimeLM = EditorGUIUtility.TrTextContent("Realtime Lightmap", null, null);

			public GUIContent ScaleInLightmap = EditorGUIUtility.TrTextContent("Scale In Lightmap", "Specifies the relative size of object's UVs within a lightmap. A value of 0 will result in the object not being lightmapped, but still contribute lighting to other objects in the Scene.", null);

			public GUIContent AtlasIndex = EditorGUIUtility.TrTextContent("Lightmap Index", null, null);

			public GUIContent PVRInstanceHash = EditorGUIUtility.TrTextContent("Instance Hash", "The hash of the baked GI instance.", null);

			public GUIContent PVRAtlasHash = EditorGUIUtility.TrTextContent("Atlas Hash", "The hash of the atlas this baked GI instance is a part of.", null);

			public GUIContent PVRAtlasInstanceOffset = EditorGUIUtility.TrTextContent("Atlas Instance Offset", "The offset into the transform array instances of this atlas start at.", null);

			public GUIContent RealtimeLMResolution = EditorGUIUtility.TrTextContent("System Resolution", "The resolution in texels of the realtime lightmap that this renderer belongs to.", null);

			public GUIContent RealtimeLMInstanceResolution = EditorGUIUtility.TrTextContent("Instance Resolution", "The resolution in texels of the realtime lightmap packed instance.", null);

			public GUIContent RealtimeLMInputSystemHash = EditorGUIUtility.TrTextContent("System Hash", "The hash of the realtime system that the renderer belongs to.", null);

			public GUIContent RealtimeLMInstanceHash = EditorGUIUtility.TrTextContent("Instance Hash", "The hash of the realtime GI instance.", null);

			public GUIContent RealtimeLMGeometryHash = EditorGUIUtility.TrTextContent("Geometry Hash", "The hash of the realtime GI geometry that the renderer is using.", null);

			public GUIContent UVCharting = EditorGUIUtility.TrTextContent("UV Charting Control", null, null);

			public GUIContent LightmapSettings = EditorGUIUtility.TrTextContent("Lightmap Settings", null, null);

			public GUIContent LightmapStatic = EditorGUIUtility.TrTextContent("Lightmap Static", "Controls whether the geometry will be marked as Static for lightmapping purposes. When enabled, this mesh will be present in lightmap calculations.", null);

			public GUIContent CastShadows = EditorGUIUtility.TrTextContent("Cast Shadows", "Specifies whether a geometry creates shadows or not when a shadow-casting Light shines on it.", null);

			public GUIContent ReceiveShadows = EditorGUIUtility.TrTextContent("Receive Shadows", "When enabled, any shadows cast from other objects are drawn on the geometry.", null);

			public GUIContent MotionVectors = EditorGUIUtility.TrTextContent("Motion Vectors", "Specifies whether the Mesh renders 'Per Object Motion', 'Camera Motion', or 'No Motion' vectors to the Camera Motion Vector Texture.", null);

			public GUIContent LightmapInfoBox = EditorGUIUtility.TrTextContent("To enable generation of lightmaps for this Mesh Renderer, please enable the 'Lightmap Static' property.", null, null);

			public GUIContent TerrainLightmapInfoBox = EditorGUIUtility.TrTextContent("To enable generation of lightmaps for this Mesh Renderer, please enable the 'Lightmap Static' property.", null, null);

			public GUIContent ResolutionTooHighWarning = EditorGUIUtility.TrTextContent("Precompute/indirect resolution for this terrain is probably too high. Use a lower realtime/indirect resolution setting in the Lighting window or assign LightmapParameters that use a lower resolution setting. Otherwise it may take a very long time to bake and memory consumption during and after the bake may be very high.", null, null);

			public GUIContent ResolutionTooLowWarning = EditorGUIUtility.TrTextContent("Precompute/indirect resolution for this terrain is probably too low. If the Clustering stage takes a long time, try using a higher realtime/indirect resolution setting in the Lighting window or assign LightmapParameters that use a higher resolution setting.", null, null);

			public GUIContent GINotEnabledInfo = EditorGUIUtility.TrTextContent("Lightmapping settings are currently disabled. Enable Baked Global Illumination or Realtime Global Illumination to display these settings.", null, null);
		}

		private static LightingSettingsInspector.Styles s_Styles;

		private bool m_ShowSettings = false;

		private bool m_ShowChartingSettings = true;

		private bool m_ShowLightmapSettings = true;

		private bool m_ShowBakedLM = false;

		private bool m_ShowRealtimeLM = false;

		private SerializedObject m_SerializedObject;

		private SerializedObject m_GameObjectsSerializedObject;

		private SerializedObject m_LightmapSettings;

		private SerializedProperty m_StaticEditorFlags;

		private SerializedProperty m_ImportantGI;

		private SerializedProperty m_StitchLightmapSeams;

		private SerializedProperty m_LightmapParameters;

		private SerializedProperty m_LightmapIndex;

		private SerializedProperty m_LightmapTilingOffsetX;

		private SerializedProperty m_LightmapTilingOffsetY;

		private SerializedProperty m_LightmapTilingOffsetZ;

		private SerializedProperty m_LightmapTilingOffsetW;

		private SerializedProperty m_PreserveUVs;

		private SerializedProperty m_AutoUVMaxDistance;

		private SerializedProperty m_IgnoreNormalsForChartDetection;

		private SerializedProperty m_AutoUVMaxAngle;

		private SerializedProperty m_MinimumChartSize;

		private SerializedProperty m_LightmapScale;

		private SerializedProperty m_CastShadows;

		private SerializedProperty m_ReceiveShadows;

		private SerializedProperty m_MotionVectors;

		private SerializedProperty m_EnabledBakedGI;

		private SerializedProperty m_EnabledRealtimeGI;

		private Renderer[] m_Renderers;

		private Terrain[] m_Terrains;

		public bool showSettings
		{
			get
			{
				return this.m_ShowSettings;
			}
			set
			{
				this.m_ShowSettings = value;
			}
		}

		public bool showChartingSettings
		{
			get
			{
				return this.m_ShowChartingSettings;
			}
			set
			{
				this.m_ShowChartingSettings = value;
			}
		}

		public bool showLightmapSettings
		{
			get
			{
				return this.m_ShowLightmapSettings;
			}
			set
			{
				this.m_ShowLightmapSettings = value;
			}
		}

		private bool isPrefabAsset
		{
			get
			{
				bool result;
				if (this.m_SerializedObject == null || this.m_SerializedObject.targetObject == null)
				{
					result = false;
				}
				else
				{
					PrefabType prefabType = PrefabUtility.GetPrefabType(this.m_SerializedObject.targetObject);
					result = (prefabType == PrefabType.Prefab || prefabType == PrefabType.ModelPrefab);
				}
				return result;
			}
		}

		public LightingSettingsInspector(SerializedObject serializedObject)
		{
			this.m_SerializedObject = serializedObject;
			this.m_GameObjectsSerializedObject = new SerializedObject((from t in serializedObject.targetObjects
			select ((Component)t).gameObject).ToArray<GameObject>());
			this.m_ImportantGI = this.m_SerializedObject.FindProperty("m_ImportantGI");
			this.m_StitchLightmapSeams = this.m_SerializedObject.FindProperty("m_StitchLightmapSeams");
			this.m_LightmapParameters = this.m_SerializedObject.FindProperty("m_LightmapParameters");
			this.m_LightmapIndex = this.m_SerializedObject.FindProperty("m_LightmapIndex");
			this.m_LightmapTilingOffsetX = this.m_SerializedObject.FindProperty("m_LightmapTilingOffset.x");
			this.m_LightmapTilingOffsetY = this.m_SerializedObject.FindProperty("m_LightmapTilingOffset.y");
			this.m_LightmapTilingOffsetZ = this.m_SerializedObject.FindProperty("m_LightmapTilingOffset.z");
			this.m_LightmapTilingOffsetW = this.m_SerializedObject.FindProperty("m_LightmapTilingOffset.w");
			this.m_PreserveUVs = this.m_SerializedObject.FindProperty("m_PreserveUVs");
			this.m_AutoUVMaxDistance = this.m_SerializedObject.FindProperty("m_AutoUVMaxDistance");
			this.m_IgnoreNormalsForChartDetection = this.m_SerializedObject.FindProperty("m_IgnoreNormalsForChartDetection");
			this.m_AutoUVMaxAngle = this.m_SerializedObject.FindProperty("m_AutoUVMaxAngle");
			this.m_MinimumChartSize = this.m_SerializedObject.FindProperty("m_MinimumChartSize");
			this.m_LightmapScale = this.m_SerializedObject.FindProperty("m_ScaleInLightmap");
			this.m_CastShadows = serializedObject.FindProperty("m_CastShadows");
			this.m_ReceiveShadows = serializedObject.FindProperty("m_ReceiveShadows");
			this.m_MotionVectors = serializedObject.FindProperty("m_MotionVectors");
			this.m_Renderers = this.m_SerializedObject.targetObjects.OfType<Renderer>().ToArray<Renderer>();
			this.m_Terrains = this.m_SerializedObject.targetObjects.OfType<Terrain>().ToArray<Terrain>();
			this.m_StaticEditorFlags = this.m_GameObjectsSerializedObject.FindProperty("m_StaticEditorFlags");
			this.m_LightmapSettings = new SerializedObject(LightmapEditorSettings.GetLightmapSettings());
			this.m_EnabledBakedGI = this.m_LightmapSettings.FindProperty("m_GISettings.m_EnableBakedLightmaps");
			this.m_EnabledRealtimeGI = this.m_LightmapSettings.FindProperty("m_GISettings.m_EnableRealtimeLightmaps");
		}

		public bool Begin()
		{
			if (LightingSettingsInspector.s_Styles == null)
			{
				LightingSettingsInspector.s_Styles = new LightingSettingsInspector.Styles();
			}
			this.m_ShowSettings = EditorGUILayout.Foldout(this.m_ShowSettings, LightingSettingsInspector.s_Styles.Lighting);
			bool result;
			if (!this.m_ShowSettings)
			{
				result = false;
			}
			else
			{
				EditorGUI.indentLevel++;
				result = true;
			}
			return result;
		}

		public void End()
		{
			if (this.m_ShowSettings)
			{
				EditorGUI.indentLevel--;
			}
		}

		public void RenderMeshSettings(bool showLightmapSettings)
		{
			if (LightingSettingsInspector.s_Styles == null)
			{
				LightingSettingsInspector.s_Styles = new LightingSettingsInspector.Styles();
			}
			if (this.m_SerializedObject != null && this.m_GameObjectsSerializedObject != null && this.m_GameObjectsSerializedObject.targetObjects.Length != 0)
			{
				this.m_GameObjectsSerializedObject.Update();
				this.m_LightmapSettings.Update();
				EditorGUILayout.PropertyField(this.m_CastShadows, LightingSettingsInspector.s_Styles.CastShadows, true, new GUILayoutOption[0]);
				bool disabled = SceneView.IsUsingDeferredRenderingPath();
				if (SupportedRenderingFeatures.active.rendererSupportsReceiveShadows)
				{
					using (new EditorGUI.DisabledScope(disabled))
					{
						EditorGUILayout.PropertyField(this.m_ReceiveShadows, LightingSettingsInspector.s_Styles.ReceiveShadows, true, new GUILayoutOption[0]);
					}
				}
				if (SupportedRenderingFeatures.active.rendererSupportsMotionVectors)
				{
					EditorGUILayout.PropertyField(this.m_MotionVectors, LightingSettingsInspector.s_Styles.MotionVectors, true, new GUILayoutOption[0]);
				}
				if (showLightmapSettings)
				{
					this.LightmapStaticSettings();
					if (!this.m_EnabledBakedGI.boolValue && !this.m_EnabledRealtimeGI.boolValue && !this.isPrefabAsset)
					{
						EditorGUILayout.HelpBox(LightingSettingsInspector.s_Styles.GINotEnabledInfo.text, MessageType.Info);
					}
					else
					{
						bool flag = (this.m_StaticEditorFlags.intValue & 1) != 0;
						if (flag)
						{
							bool flag2 = this.isPrefabAsset || this.m_EnabledRealtimeGI.boolValue || (this.m_EnabledBakedGI.boolValue && LightmapEditorSettings.lightmapper == LightmapEditorSettings.Lightmapper.Radiosity);
							bool flag3 = this.isPrefabAsset || (this.m_EnabledBakedGI.boolValue && LightmapEditorSettings.lightmapper != LightmapEditorSettings.Lightmapper.Radiosity);
							if (flag2)
							{
								this.m_ShowChartingSettings = EditorGUILayout.Foldout(this.m_ShowChartingSettings, LightingSettingsInspector.s_Styles.UVCharting);
								if (this.m_ShowChartingSettings)
								{
									this.RendererUVSettings();
								}
							}
							this.m_ShowLightmapSettings = EditorGUILayout.Foldout(this.m_ShowLightmapSettings, LightingSettingsInspector.s_Styles.LightmapSettings);
							if (this.m_ShowLightmapSettings)
							{
								EditorGUI.indentLevel++;
								float num = LightmapVisualization.GetLightmapLODLevelScale(this.m_Renderers[0]);
								for (int i = 1; i < this.m_Renderers.Length; i++)
								{
									if (!Mathf.Approximately(num, LightmapVisualization.GetLightmapLODLevelScale(this.m_Renderers[i])))
									{
										num = 1f;
									}
								}
								float lightmapScale = this.LightmapScaleGUI(num) * LightmapVisualization.GetLightmapLODLevelScale(this.m_Renderers[0]);
								float cachedMeshSurfaceArea = InternalMeshUtil.GetCachedMeshSurfaceArea((MeshRenderer)this.m_Renderers[0]);
								this.ShowClampedSizeInLightmapGUI(lightmapScale, cachedMeshSurfaceArea);
								if (flag2)
								{
									EditorGUILayout.PropertyField(this.m_ImportantGI, LightingSettingsInspector.s_Styles.ImportantGI, new GUILayoutOption[0]);
								}
								if (flag3)
								{
									EditorGUILayout.PropertyField(this.m_StitchLightmapSeams, LightingSettingsInspector.s_Styles.StitchLightmapSeams, new GUILayoutOption[0]);
								}
								LightingSettingsInspector.LightmapParametersGUI(this.m_LightmapParameters, LightingSettingsInspector.s_Styles.LightmapParameters);
								this.m_ShowBakedLM = EditorGUILayout.Foldout(this.m_ShowBakedLM, LightingSettingsInspector.s_Styles.Atlas);
								if (this.m_ShowBakedLM)
								{
									this.ShowAtlasGUI(this.m_Renderers[0].GetInstanceID());
								}
								this.m_ShowRealtimeLM = EditorGUILayout.Foldout(this.m_ShowRealtimeLM, LightingSettingsInspector.s_Styles.RealtimeLM);
								if (this.m_ShowRealtimeLM)
								{
									this.ShowRealtimeLMGUI(this.m_Renderers[0]);
								}
								EditorGUI.indentLevel--;
							}
							if (LightmapEditorSettings.HasZeroAreaMesh(this.m_Renderers[0]))
							{
								EditorGUILayout.HelpBox(LightingSettingsInspector.s_Styles.ZeroAreaPackingMesh.text, MessageType.Warning);
							}
							if (LightmapEditorSettings.HasClampedResolution(this.m_Renderers[0]))
							{
								EditorGUILayout.HelpBox(LightingSettingsInspector.s_Styles.ClampedPackingResolution.text, MessageType.Warning);
							}
							if (!LightingSettingsInspector.HasNormals(this.m_Renderers[0]))
							{
								EditorGUILayout.HelpBox(LightingSettingsInspector.s_Styles.NoNormalsNoLightmapping.text, MessageType.Warning);
							}
							if (LightmapEditorSettings.HasUVOverlaps(this.m_Renderers[0]))
							{
								EditorGUILayout.HelpBox(LightingSettingsInspector.s_Styles.UVOverlap.text, MessageType.Warning);
							}
							this.m_SerializedObject.ApplyModifiedProperties();
						}
						else
						{
							EditorGUILayout.HelpBox(LightingSettingsInspector.s_Styles.LightmapInfoBox.text, MessageType.Info);
						}
					}
				}
			}
		}

		public void RenderTerrainSettings()
		{
			if (LightingSettingsInspector.s_Styles == null)
			{
				LightingSettingsInspector.s_Styles = new LightingSettingsInspector.Styles();
			}
			if (this.m_SerializedObject != null && this.m_GameObjectsSerializedObject != null && this.m_GameObjectsSerializedObject.targetObjects.Length != 0)
			{
				this.m_GameObjectsSerializedObject.Update();
				this.m_LightmapSettings.Update();
				this.LightmapStaticSettings();
				if (!this.m_EnabledBakedGI.boolValue && !this.m_EnabledRealtimeGI.boolValue && !this.isPrefabAsset)
				{
					EditorGUILayout.HelpBox(LightingSettingsInspector.s_Styles.GINotEnabledInfo.text, MessageType.Info);
				}
				else
				{
					bool flag = (this.m_StaticEditorFlags.intValue & 1) != 0;
					if (flag)
					{
						this.m_ShowLightmapSettings = EditorGUILayout.Foldout(this.m_ShowLightmapSettings, LightingSettingsInspector.s_Styles.LightmapSettings);
						if (this.m_ShowLightmapSettings)
						{
							EditorGUI.indentLevel++;
							using (new EditorGUI.DisabledScope(!flag))
							{
								if (GUI.enabled)
								{
									this.ShowTerrainChunks(this.m_Terrains);
								}
								float lightmapScale = this.LightmapScaleGUI(1f);
								TerrainData terrainData = this.m_Terrains[0].terrainData;
								float cachedSurfaceArea = (!(terrainData != null)) ? 0f : (terrainData.size.x * terrainData.size.z);
								this.ShowClampedSizeInLightmapGUI(lightmapScale, cachedSurfaceArea);
								LightingSettingsInspector.LightmapParametersGUI(this.m_LightmapParameters, LightingSettingsInspector.s_Styles.LightmapParameters);
								if (GUI.enabled && this.m_Terrains.Length == 1 && this.m_Terrains[0].terrainData != null)
								{
									this.ShowBakePerformanceWarning(this.m_Terrains[0]);
								}
								this.m_ShowBakedLM = EditorGUILayout.Foldout(this.m_ShowBakedLM, LightingSettingsInspector.s_Styles.Atlas);
								if (this.m_ShowBakedLM)
								{
									this.ShowAtlasGUI(this.m_Terrains[0].GetInstanceID());
								}
								this.m_ShowRealtimeLM = EditorGUILayout.Foldout(this.m_ShowRealtimeLM, LightingSettingsInspector.s_Styles.RealtimeLM);
								if (this.m_ShowRealtimeLM)
								{
									this.ShowRealtimeLMGUI(this.m_Terrains[0]);
								}
								this.m_SerializedObject.ApplyModifiedProperties();
							}
							EditorGUI.indentLevel--;
						}
						GUILayout.Space(10f);
					}
					else
					{
						EditorGUILayout.HelpBox(LightingSettingsInspector.s_Styles.TerrainLightmapInfoBox.text, MessageType.Info);
					}
				}
			}
		}

		private void LightmapStaticSettings()
		{
			bool flag = (this.m_StaticEditorFlags.intValue & 1) != 0;
			EditorGUI.BeginChangeCheck();
			flag = EditorGUILayout.Toggle(LightingSettingsInspector.s_Styles.LightmapStatic, flag, new GUILayoutOption[0]);
			if (EditorGUI.EndChangeCheck())
			{
				SceneModeUtility.SetStaticFlags(this.m_GameObjectsSerializedObject.targetObjects, 1, flag);
				this.m_GameObjectsSerializedObject.Update();
			}
		}

		private void RendererUVSettings()
		{
			EditorGUI.indentLevel++;
			bool flag = !this.m_PreserveUVs.boolValue;
			EditorGUI.BeginChangeCheck();
			flag = EditorGUILayout.Toggle(LightingSettingsInspector.s_Styles.OptimizeRealtimeUVs, flag, new GUILayoutOption[0]);
			if (EditorGUI.EndChangeCheck())
			{
				this.m_PreserveUVs.boolValue = !flag;
			}
			EditorGUI.indentLevel++;
			bool boolValue = this.m_PreserveUVs.boolValue;
			using (new EditorGUI.DisabledScope(boolValue))
			{
				EditorGUILayout.PropertyField(this.m_AutoUVMaxDistance, LightingSettingsInspector.s_Styles.AutoUVMaxDistance, new GUILayoutOption[0]);
				if (this.m_AutoUVMaxDistance.floatValue < 0f)
				{
					this.m_AutoUVMaxDistance.floatValue = 0f;
				}
				EditorGUILayout.Slider(this.m_AutoUVMaxAngle, 0f, 180f, LightingSettingsInspector.s_Styles.AutoUVMaxAngle, new GUILayoutOption[0]);
			}
			EditorGUI.indentLevel--;
			EditorGUILayout.PropertyField(this.m_IgnoreNormalsForChartDetection, LightingSettingsInspector.s_Styles.IgnoreNormalsForChartDetection, new GUILayoutOption[0]);
			EditorGUILayout.IntPopup(this.m_MinimumChartSize, LightingSettingsInspector.s_Styles.MinimumChartSizeStrings, LightingSettingsInspector.s_Styles.MinimumChartSizeValues, LightingSettingsInspector.s_Styles.MinimumChartSize, new GUILayoutOption[0]);
			EditorGUI.indentLevel--;
		}

		private void ShowClampedSizeInLightmapGUI(float lightmapScale, float cachedSurfaceArea)
		{
			float num = Mathf.Sqrt(cachedSurfaceArea) * LightmapEditorSettings.bakeResolution * lightmapScale;
			if (num > (float)LightmapEditorSettings.maxAtlasSize)
			{
				EditorGUILayout.HelpBox(LightingSettingsInspector.s_Styles.ClampedSize.text, MessageType.Info);
			}
		}

		private float LightmapScaleGUI(float lodScale)
		{
			float num = lodScale * this.m_LightmapScale.floatValue;
			Rect controlRect = EditorGUILayout.GetControlRect(new GUILayoutOption[0]);
			EditorGUI.BeginProperty(controlRect, LightingSettingsInspector.s_Styles.ScaleInLightmap, this.m_LightmapScale);
			EditorGUI.BeginChangeCheck();
			num = EditorGUI.FloatField(controlRect, LightingSettingsInspector.s_Styles.ScaleInLightmap, num);
			if (EditorGUI.EndChangeCheck())
			{
				this.m_LightmapScale.floatValue = Mathf.Max(num / Mathf.Max(lodScale, 1.401298E-45f), 0f);
			}
			EditorGUI.EndProperty();
			return num;
		}

		private void ShowAtlasGUI(int instanceID)
		{
			EditorGUI.indentLevel++;
			Hash128 hash;
			LightmapEditorSettings.GetPVRInstanceHash(instanceID, out hash);
			EditorGUILayout.LabelField(LightingSettingsInspector.s_Styles.PVRInstanceHash, GUIContent.Temp(hash.ToString()), new GUILayoutOption[0]);
			Hash128 hash2;
			LightmapEditorSettings.GetPVRAtlasHash(instanceID, out hash2);
			EditorGUILayout.LabelField(LightingSettingsInspector.s_Styles.PVRAtlasHash, GUIContent.Temp(hash2.ToString()), new GUILayoutOption[0]);
			int num;
			LightmapEditorSettings.GetPVRAtlasInstanceOffset(instanceID, out num);
			EditorGUILayout.LabelField(LightingSettingsInspector.s_Styles.PVRAtlasInstanceOffset, GUIContent.Temp(num.ToString()), new GUILayoutOption[0]);
			EditorGUILayout.LabelField(LightingSettingsInspector.s_Styles.AtlasIndex, GUIContent.Temp(this.m_LightmapIndex.intValue.ToString()), new GUILayoutOption[0]);
			EditorGUILayout.LabelField(LightingSettingsInspector.s_Styles.AtlasTilingX, GUIContent.Temp(this.m_LightmapTilingOffsetX.floatValue.ToString()), new GUILayoutOption[0]);
			EditorGUILayout.LabelField(LightingSettingsInspector.s_Styles.AtlasTilingY, GUIContent.Temp(this.m_LightmapTilingOffsetY.floatValue.ToString()), new GUILayoutOption[0]);
			EditorGUILayout.LabelField(LightingSettingsInspector.s_Styles.AtlasOffsetX, GUIContent.Temp(this.m_LightmapTilingOffsetZ.floatValue.ToString()), new GUILayoutOption[0]);
			EditorGUILayout.LabelField(LightingSettingsInspector.s_Styles.AtlasOffsetY, GUIContent.Temp(this.m_LightmapTilingOffsetW.floatValue.ToString()), new GUILayoutOption[0]);
			EditorGUI.indentLevel--;
		}

		private void ShowRealtimeLMGUI(Terrain terrain)
		{
			EditorGUI.indentLevel++;
			int num;
			int num2;
			int num3;
			int num4;
			if (LightmapEditorSettings.GetTerrainSystemResolution(terrain, out num, out num2, out num3, out num4))
			{
				string text = num.ToString() + "x" + num2.ToString();
				if (num3 > 1 || num4 > 1)
				{
					text += string.Format(" ({0}x{1} chunks)", num3, num4);
				}
				EditorGUILayout.LabelField(LightingSettingsInspector.s_Styles.RealtimeLMResolution, GUIContent.Temp(text), new GUILayoutOption[0]);
			}
			EditorGUI.indentLevel--;
		}

		private void ShowRealtimeLMGUI(Renderer renderer)
		{
			EditorGUI.indentLevel++;
			Hash128 hash;
			if (LightmapEditorSettings.GetInstanceHash(renderer, out hash))
			{
				EditorGUILayout.LabelField(LightingSettingsInspector.s_Styles.RealtimeLMInstanceHash, GUIContent.Temp(hash.ToString()), new GUILayoutOption[0]);
			}
			Hash128 hash2;
			if (LightmapEditorSettings.GetGeometryHash(renderer, out hash2))
			{
				EditorGUILayout.LabelField(LightingSettingsInspector.s_Styles.RealtimeLMGeometryHash, GUIContent.Temp(hash2.ToString()), new GUILayoutOption[0]);
			}
			int num;
			int num2;
			if (LightmapEditorSettings.GetInstanceResolution(renderer, out num, out num2))
			{
				EditorGUILayout.LabelField(LightingSettingsInspector.s_Styles.RealtimeLMInstanceResolution, GUIContent.Temp(num.ToString() + "x" + num2.ToString()), new GUILayoutOption[0]);
			}
			Hash128 hash3;
			if (LightmapEditorSettings.GetInputSystemHash(renderer, out hash3))
			{
				EditorGUILayout.LabelField(LightingSettingsInspector.s_Styles.RealtimeLMInputSystemHash, GUIContent.Temp(hash3.ToString()), new GUILayoutOption[0]);
			}
			int num3;
			int num4;
			if (LightmapEditorSettings.GetSystemResolution(renderer, out num3, out num4))
			{
				EditorGUILayout.LabelField(LightingSettingsInspector.s_Styles.RealtimeLMResolution, GUIContent.Temp(num3.ToString() + "x" + num4.ToString()), new GUILayoutOption[0]);
			}
			EditorGUI.indentLevel--;
		}

		private static bool HasNormals(Renderer renderer)
		{
			Mesh mesh = null;
			if (renderer is MeshRenderer)
			{
				MeshFilter component = renderer.GetComponent<MeshFilter>();
				if (component != null)
				{
					mesh = component.sharedMesh;
				}
			}
			else if (renderer is SkinnedMeshRenderer)
			{
				mesh = (renderer as SkinnedMeshRenderer).sharedMesh;
			}
			return mesh != null && InternalMeshUtil.HasNormals(mesh);
		}

		private static bool isBuiltIn(SerializedProperty prop)
		{
			bool result;
			if (prop.objectReferenceValue != null)
			{
				LightmapParameters lightmapParameters = prop.objectReferenceValue as LightmapParameters;
				result = (lightmapParameters.hideFlags == HideFlags.NotEditable);
			}
			else
			{
				result = false;
			}
			return result;
		}

		public static bool LightmapParametersGUI(SerializedProperty prop, GUIContent content)
		{
			EditorGUILayout.BeginHorizontal(new GUILayoutOption[0]);
			EditorGUIInternal.AssetPopup<LightmapParameters>(prop, content, "giparams", "Scene Default Parameters");
			string text = "Edit...";
			if (LightingSettingsInspector.isBuiltIn(prop))
			{
				text = "View";
			}
			bool result = false;
			if (prop.objectReferenceValue == null)
			{
				SerializedObject serializedObject = new SerializedObject(LightmapEditorSettings.GetLightmapSettings());
				SerializedProperty serializedProperty = serializedObject.FindProperty("m_LightmapEditorSettings.m_LightmapParameters");
				using (new EditorGUI.DisabledScope(serializedProperty == null))
				{
					if (LightingSettingsInspector.isBuiltIn(serializedProperty))
					{
						text = "View";
					}
					else
					{
						text = "Edit...";
					}
					if (GUILayout.Button(text, EditorStyles.miniButton, new GUILayoutOption[]
					{
						GUILayout.ExpandWidth(false)
					}))
					{
						Selection.activeObject = serializedProperty.objectReferenceValue;
						result = true;
					}
				}
			}
			else if (GUILayout.Button(text, EditorStyles.miniButton, new GUILayoutOption[]
			{
				GUILayout.ExpandWidth(false)
			}))
			{
				Selection.activeObject = prop.objectReferenceValue;
				result = true;
			}
			EditorGUILayout.EndHorizontal();
			return result;
		}

		private void ShowTerrainChunks(Terrain[] terrains)
		{
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < terrains.Length; i++)
			{
				Terrain terrain = terrains[i];
				int num3 = 0;
				int num4 = 0;
				Lightmapping.GetTerrainGIChunks(terrain, ref num3, ref num4);
				if (num == 0 && num2 == 0)
				{
					num = num3;
					num2 = num4;
				}
				else if (num != num3 || num2 != num4)
				{
					num2 = (num = 0);
					break;
				}
			}
			if (num * num2 > 1)
			{
				EditorGUILayout.HelpBox(string.Format(L10n.Tr("Terrain is chunked up into {0} instances for baking."), num * num2), MessageType.None);
			}
		}

		private void ShowBakePerformanceWarning(Terrain terrain)
		{
			float x = terrain.terrainData.size.x;
			float z = terrain.terrainData.size.z;
			LightmapParameters lightmapParameters = ((LightmapParameters)this.m_LightmapParameters.objectReferenceValue) ?? new LightmapParameters();
			float num = x * lightmapParameters.resolution * LightmapEditorSettings.realtimeResolution;
			float num2 = z * lightmapParameters.resolution * LightmapEditorSettings.realtimeResolution;
			if (num > 512f || num2 > 512f)
			{
				EditorGUILayout.HelpBox(LightingSettingsInspector.s_Styles.ResolutionTooHighWarning.text, MessageType.Warning);
			}
			float num3 = num * lightmapParameters.clusterResolution;
			float num4 = num2 * lightmapParameters.clusterResolution;
			float num5 = (float)terrain.terrainData.heightmapResolution / num3;
			float num6 = (float)terrain.terrainData.heightmapResolution / num4;
			if (num5 > 51.2f || num6 > 51.2f)
			{
				EditorGUILayout.HelpBox(LightingSettingsInspector.s_Styles.ResolutionTooLowWarning.text, MessageType.Warning);
			}
		}
	}
}
