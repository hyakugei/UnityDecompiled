using System;
using UnityEditor.Experimental.AssetImporters;
using UnityEngine;

namespace UnityEditor
{
	internal class ModelImporterModelEditor : BaseAssetImporterTabUI
	{
		private class Styles
		{
			public GUIContent Meshes = EditorGUIUtility.TrTextContent("Meshes", "These options control how geometry is imported.", null);

			public GUIContent ScaleFactor = EditorGUIUtility.TrTextContent("Scale Factor", "How much to scale the models compared to what is in the source file.", null);

			public GUIContent UseFileUnits = EditorGUIUtility.TrTextContent("Use File Units", "Detect file units and import as 1FileUnit=1UnityUnit, otherwise it will import as 1cm=1UnityUnit. See ModelImporter.useFileUnits for more details.", null);

			public GUIContent UseFileScale = EditorGUIUtility.TrTextContent("Use File Scale", "Use File Scale when importing.", null);

			public GUIContent FileScaleFactor = EditorGUIUtility.TrTextContent("File Scale", "Scale defined by source file, or 1 if Use File Scale is disabled. Click Apply to update.", null);

			public GUIContent ImportBlendShapes = EditorGUIUtility.TrTextContent("Import BlendShapes", "Should Unity import BlendShapes.", null);

			public GUIContent GenerateColliders = EditorGUIUtility.TrTextContent("Generate Colliders", "Should Unity generate mesh colliders for all meshes.", null);

			public GUIContent SwapUVChannels = EditorGUIUtility.TrTextContent("Swap UVs", "Swaps the 2 UV channels in meshes. Use if your diffuse texture uses UVs from the lightmap.", null);

			public GUIContent GenerateSecondaryUV = EditorGUIUtility.TrTextContent("Generate Lightmap UVs", "Generate lightmap UVs into UV2.", null);

			public GUIContent GenerateSecondaryUVAdvanced = EditorGUIUtility.TrTextContent("Advanced", null, null);

			public GUIContent secondaryUVAngleDistortion = EditorGUIUtility.TrTextContent("Angle Error", "Measured in percents. Angle error measures deviation of UV angles from geometry angles. Area error measures deviation of UV triangles area from geometry triangles if they were uniformly scaled.", null);

			public GUIContent secondaryUVAreaDistortion = EditorGUIUtility.TrTextContent("Area Error", null, null);

			public GUIContent secondaryUVHardAngle = EditorGUIUtility.TrTextContent("Hard Angle", "Angle between neighbor triangles that will generate seam.", null);

			public GUIContent secondaryUVPackMargin = EditorGUIUtility.TrTextContent("Pack Margin", "Measured in pixels, assuming mesh will cover an entire 1024x1024 lightmap.", null);

			public GUIContent secondaryUVDefaults = EditorGUIUtility.TrTextContent("Set Defaults", null, null);

			public GUIContent TangentSpace = EditorGUIUtility.TrTextContent("Normals & Tangents", null, null);

			public GUIContent TangentSpaceNormalLabel = EditorGUIUtility.TrTextContent("Normals", null, null);

			public GUIContent TangentSpaceTangentLabel = EditorGUIUtility.TrTextContent("Tangents", null, null);

			public GUIContent TangentSpaceOptionImport = EditorGUIUtility.TrTextContent("Import", null, null);

			public GUIContent TangentSpaceOptionCalculateLegacy = EditorGUIUtility.TrTextContent("Calculate Legacy", null, null);

			public GUIContent TangentSpaceOptionCalculateLegacySplit = EditorGUIUtility.TrTextContent("Calculate Legacy - Split Tangents", null, null);

			public GUIContent TangentSpaceOptionCalculate = EditorGUIUtility.TrTextContent("Calculate Tangent Space", null, null);

			public GUIContent TangentSpaceOptionNone = EditorGUIUtility.TrTextContent("None", null, null);

			public GUIContent TangentSpaceOptionNoneNoNormals = EditorGUIUtility.TrTextContent("None - (Normals required)", null, null);

			public GUIContent NormalOptionImport = EditorGUIUtility.TrTextContent("Import", null, null);

			public GUIContent NormalOptionCalculate = EditorGUIUtility.TrTextContent("Calculate", null, null);

			public GUIContent NormalOptionNone = EditorGUIUtility.TrTextContent("None", null, null);

			public GUIContent RecalculateNormalsLabel = EditorGUIUtility.TrTextContent("Normals Mode", null, null);

			public GUIContent[] RecalculateNormalsOpt = new GUIContent[]
			{
				EditorGUIUtility.TrTextContent("Unweighted Legacy", null, null),
				EditorGUIUtility.TrTextContent("Unweighted", null, null),
				EditorGUIUtility.TrTextContent("Area Weighted", null, null),
				EditorGUIUtility.TrTextContent("Angle Weighted", null, null),
				EditorGUIUtility.TrTextContent("Area and Angle Weighted", null, null)
			};

			public GUIContent[] TangentSpaceModeOptLabelsAll;

			public GUIContent[] TangentSpaceModeOptLabelsCalculate;

			public GUIContent[] TangentSpaceModeOptLabelsNone;

			public GUIContent[] NormalModeLabelsAll;

			public ModelImporterTangents[] TangentSpaceModeOptEnumsAll;

			public ModelImporterTangents[] TangentSpaceModeOptEnumsCalculate;

			public ModelImporterTangents[] TangentSpaceModeOptEnumsNone;

			public GUIContent SmoothingAngle = EditorGUIUtility.TrTextContent("Smoothing Angle", "Normal Smoothing Angle", null);

			public GUIContent MeshCompressionLabel = EditorGUIUtility.TrTextContent("Mesh Compression", "Higher compression ratio means lower mesh precision. If enabled, the mesh bounds and a lower bit depth per component are used to compress the mesh data.", null);

			public GUIContent[] MeshCompressionOpt = new GUIContent[]
			{
				EditorGUIUtility.TrTextContent("Off", null, null),
				EditorGUIUtility.TrTextContent("Low", null, null),
				EditorGUIUtility.TrTextContent("Medium", null, null),
				EditorGUIUtility.TrTextContent("High", null, null)
			};

			public GUIContent IndexFormatLabel = EditorGUIUtility.TrTextContent("Index Format", "Format of mesh index buffer. Auto mode picks 16 or 32 bit depending on mesh vertex count.", null);

			public GUIContent[] IndexFormatOpt = new GUIContent[]
			{
				EditorGUIUtility.TrTextContent("Auto", null, null),
				EditorGUIUtility.TrTextContent("16 bit", null, null),
				EditorGUIUtility.TrTextContent("32 bit", null, null)
			};

			public GUIContent OptimizeMeshForGPU = EditorGUIUtility.TrTextContent("Optimize Mesh", "The vertices and indices will be reordered for better GPU performance.", null);

			public GUIContent KeepQuads = EditorGUIUtility.TrTextContent("Keep Quads", "If model contains quad faces, they are kept for DX11 tessellation.", null);

			public GUIContent WeldVertices = EditorGUIUtility.TrTextContent("Weld Vertices", "Combine vertices that share the same position in space.", null);

			public GUIContent ImportVisibility = EditorGUIUtility.TrTextContent("Import Visibility", "Use visibility properties to enable or disable MeshRenderer components.", null);

			public GUIContent ImportCameras = EditorGUIUtility.TrTextContent("Import Cameras", null, null);

			public GUIContent ImportLights = EditorGUIUtility.TrTextContent("Import Lights", null, null);

			public GUIContent PreserveHierarchy = EditorGUIUtility.TrTextContent("Preserve Hierarchy", "Always create an explicit prefab root, even if the model only has a single root.", null);

			public GUIContent IsReadable = EditorGUIUtility.TrTextContent("Read/Write Enabled", "Allow vertices and indices to be accessed from script.", null);

			public Styles()
			{
				this.NormalModeLabelsAll = new GUIContent[]
				{
					this.NormalOptionImport,
					this.NormalOptionCalculate,
					this.NormalOptionNone
				};
				this.TangentSpaceModeOptLabelsAll = new GUIContent[]
				{
					this.TangentSpaceOptionImport,
					this.TangentSpaceOptionCalculate,
					this.TangentSpaceOptionCalculateLegacy,
					this.TangentSpaceOptionCalculateLegacySplit,
					this.TangentSpaceOptionNone
				};
				this.TangentSpaceModeOptLabelsCalculate = new GUIContent[]
				{
					this.TangentSpaceOptionCalculate,
					this.TangentSpaceOptionCalculateLegacy,
					this.TangentSpaceOptionCalculateLegacySplit,
					this.TangentSpaceOptionNone
				};
				this.TangentSpaceModeOptLabelsNone = new GUIContent[]
				{
					this.TangentSpaceOptionNoneNoNormals
				};
				this.TangentSpaceModeOptEnumsAll = new ModelImporterTangents[]
				{
					ModelImporterTangents.Import,
					ModelImporterTangents.CalculateMikk,
					ModelImporterTangents.CalculateLegacy,
					ModelImporterTangents.CalculateLegacyWithSplitTangents,
					ModelImporterTangents.None
				};
				this.TangentSpaceModeOptEnumsCalculate = new ModelImporterTangents[]
				{
					ModelImporterTangents.CalculateMikk,
					ModelImporterTangents.CalculateLegacy,
					ModelImporterTangents.CalculateLegacyWithSplitTangents,
					ModelImporterTangents.None
				};
				this.TangentSpaceModeOptEnumsNone = new ModelImporterTangents[]
				{
					ModelImporterTangents.None
				};
			}
		}

		private bool m_SecondaryUVAdvancedOptions = false;

		private SerializedProperty m_GlobalScale;

		private SerializedProperty m_UseFileScale;

		private SerializedProperty m_FileScale;

		private SerializedProperty m_MeshCompression;

		private SerializedProperty m_ImportBlendShapes;

		private SerializedProperty m_AddColliders;

		private SerializedProperty m_SwapUVChannels;

		private SerializedProperty m_GenerateSecondaryUV;

		private SerializedProperty m_SecondaryUVAngleDistortion;

		private SerializedProperty m_SecondaryUVAreaDistortion;

		private SerializedProperty m_SecondaryUVHardAngle;

		private SerializedProperty m_SecondaryUVPackMargin;

		private SerializedProperty m_NormalSmoothAngle;

		private SerializedProperty m_NormalImportMode;

		private SerializedProperty m_NormalCalculationMode;

		private SerializedProperty m_TangentImportMode;

		private SerializedProperty m_OptimizeMeshForGPU;

		private SerializedProperty m_IsReadable;

		private SerializedProperty m_KeepQuads;

		private SerializedProperty m_IndexFormat;

		private SerializedProperty m_WeldVertices;

		private SerializedProperty m_ImportCameras;

		private SerializedProperty m_ImportLights;

		private SerializedProperty m_ImportVisibility;

		private SerializedProperty m_PreserveHierarchy;

		private static ModelImporterModelEditor.Styles styles;

		public ModelImporterModelEditor(AssetImporterEditor panelContainer) : base(panelContainer)
		{
		}

		internal override void OnEnable()
		{
			this.m_GlobalScale = base.serializedObject.FindProperty("m_GlobalScale");
			this.m_UseFileScale = base.serializedObject.FindProperty("m_UseFileScale");
			this.m_FileScale = base.serializedObject.FindProperty("m_FileScale");
			this.m_MeshCompression = base.serializedObject.FindProperty("m_MeshCompression");
			this.m_ImportBlendShapes = base.serializedObject.FindProperty("m_ImportBlendShapes");
			this.m_ImportCameras = base.serializedObject.FindProperty("m_ImportCameras");
			this.m_ImportLights = base.serializedObject.FindProperty("m_ImportLights");
			this.m_AddColliders = base.serializedObject.FindProperty("m_AddColliders");
			this.m_SwapUVChannels = base.serializedObject.FindProperty("swapUVChannels");
			this.m_GenerateSecondaryUV = base.serializedObject.FindProperty("generateSecondaryUV");
			this.m_SecondaryUVAngleDistortion = base.serializedObject.FindProperty("secondaryUVAngleDistortion");
			this.m_SecondaryUVAreaDistortion = base.serializedObject.FindProperty("secondaryUVAreaDistortion");
			this.m_SecondaryUVHardAngle = base.serializedObject.FindProperty("secondaryUVHardAngle");
			this.m_SecondaryUVPackMargin = base.serializedObject.FindProperty("secondaryUVPackMargin");
			this.m_NormalSmoothAngle = base.serializedObject.FindProperty("normalSmoothAngle");
			this.m_NormalImportMode = base.serializedObject.FindProperty("normalImportMode");
			this.m_NormalCalculationMode = base.serializedObject.FindProperty("normalCalculationMode");
			this.m_TangentImportMode = base.serializedObject.FindProperty("tangentImportMode");
			this.m_OptimizeMeshForGPU = base.serializedObject.FindProperty("optimizeMeshForGPU");
			this.m_IsReadable = base.serializedObject.FindProperty("m_IsReadable");
			this.m_KeepQuads = base.serializedObject.FindProperty("keepQuads");
			this.m_IndexFormat = base.serializedObject.FindProperty("indexFormat");
			this.m_WeldVertices = base.serializedObject.FindProperty("weldVertices");
			this.m_ImportVisibility = base.serializedObject.FindProperty("m_ImportVisibility");
			this.m_PreserveHierarchy = base.serializedObject.FindProperty("m_PreserveHierarchy");
		}

		internal override void PreApply()
		{
			this.ScaleAvatar();
		}

		public override void OnInspectorGUI()
		{
			if (ModelImporterModelEditor.styles == null)
			{
				ModelImporterModelEditor.styles = new ModelImporterModelEditor.Styles();
			}
			this.MeshesGUI();
			this.NormalsAndTangentsGUI();
		}

		private void MeshesGUI()
		{
			GUILayout.Label(ModelImporterModelEditor.styles.Meshes, EditorStyles.boldLabel, new GUILayoutOption[0]);
			using (new EditorGUI.DisabledScope(base.targets.Length > 1))
			{
				EditorGUILayout.PropertyField(this.m_GlobalScale, ModelImporterModelEditor.styles.ScaleFactor, new GUILayoutOption[0]);
			}
			EditorGUILayout.PropertyField(this.m_UseFileScale, ModelImporterModelEditor.styles.UseFileScale, new GUILayoutOption[0]);
			if (this.m_UseFileScale.boolValue)
			{
				EditorGUI.indentLevel++;
				EditorGUILayout.PropertyField(this.m_FileScale, ModelImporterModelEditor.styles.FileScaleFactor, new GUILayoutOption[0]);
				EditorGUI.indentLevel--;
			}
			EditorGUILayout.Popup(this.m_MeshCompression, ModelImporterModelEditor.styles.MeshCompressionOpt, ModelImporterModelEditor.styles.MeshCompressionLabel, new GUILayoutOption[0]);
			EditorGUILayout.PropertyField(this.m_IsReadable, ModelImporterModelEditor.styles.IsReadable, new GUILayoutOption[0]);
			EditorGUILayout.PropertyField(this.m_OptimizeMeshForGPU, ModelImporterModelEditor.styles.OptimizeMeshForGPU, new GUILayoutOption[0]);
			EditorGUILayout.PropertyField(this.m_ImportBlendShapes, ModelImporterModelEditor.styles.ImportBlendShapes, new GUILayoutOption[0]);
			EditorGUILayout.PropertyField(this.m_AddColliders, ModelImporterModelEditor.styles.GenerateColliders, new GUILayoutOption[0]);
			EditorGUILayout.PropertyField(this.m_KeepQuads, ModelImporterModelEditor.styles.KeepQuads, new GUILayoutOption[0]);
			EditorGUILayout.Popup(this.m_IndexFormat, ModelImporterModelEditor.styles.IndexFormatOpt, ModelImporterModelEditor.styles.IndexFormatLabel, new GUILayoutOption[0]);
			EditorGUILayout.PropertyField(this.m_WeldVertices, ModelImporterModelEditor.styles.WeldVertices, new GUILayoutOption[0]);
			EditorGUILayout.PropertyField(this.m_ImportVisibility, ModelImporterModelEditor.styles.ImportVisibility, new GUILayoutOption[0]);
			EditorGUILayout.PropertyField(this.m_ImportCameras, ModelImporterModelEditor.styles.ImportCameras, new GUILayoutOption[0]);
			EditorGUILayout.PropertyField(this.m_ImportLights, ModelImporterModelEditor.styles.ImportLights, new GUILayoutOption[0]);
			EditorGUILayout.PropertyField(this.m_PreserveHierarchy, ModelImporterModelEditor.styles.PreserveHierarchy, new GUILayoutOption[0]);
			EditorGUILayout.PropertyField(this.m_SwapUVChannels, ModelImporterModelEditor.styles.SwapUVChannels, new GUILayoutOption[0]);
			EditorGUILayout.PropertyField(this.m_GenerateSecondaryUV, ModelImporterModelEditor.styles.GenerateSecondaryUV, new GUILayoutOption[0]);
			if (this.m_GenerateSecondaryUV.boolValue)
			{
				EditorGUI.indentLevel++;
				this.m_SecondaryUVAdvancedOptions = EditorGUILayout.Foldout(this.m_SecondaryUVAdvancedOptions, ModelImporterModelEditor.styles.GenerateSecondaryUVAdvanced, true, EditorStyles.foldout);
				if (this.m_SecondaryUVAdvancedOptions)
				{
					EditorGUI.BeginChangeCheck();
					EditorGUILayout.Slider(this.m_SecondaryUVHardAngle, 0f, 180f, ModelImporterModelEditor.styles.secondaryUVHardAngle, new GUILayoutOption[0]);
					EditorGUILayout.Slider(this.m_SecondaryUVPackMargin, 1f, 64f, ModelImporterModelEditor.styles.secondaryUVPackMargin, new GUILayoutOption[0]);
					EditorGUILayout.Slider(this.m_SecondaryUVAngleDistortion, 1f, 75f, ModelImporterModelEditor.styles.secondaryUVAngleDistortion, new GUILayoutOption[0]);
					EditorGUILayout.Slider(this.m_SecondaryUVAreaDistortion, 1f, 75f, ModelImporterModelEditor.styles.secondaryUVAreaDistortion, new GUILayoutOption[0]);
					if (EditorGUI.EndChangeCheck())
					{
						this.m_SecondaryUVHardAngle.floatValue = Mathf.Round(this.m_SecondaryUVHardAngle.floatValue);
						this.m_SecondaryUVPackMargin.floatValue = Mathf.Round(this.m_SecondaryUVPackMargin.floatValue);
						this.m_SecondaryUVAngleDistortion.floatValue = Mathf.Round(this.m_SecondaryUVAngleDistortion.floatValue);
						this.m_SecondaryUVAreaDistortion.floatValue = Mathf.Round(this.m_SecondaryUVAreaDistortion.floatValue);
					}
				}
				EditorGUI.indentLevel--;
			}
		}

		private void NormalsAndTangentsGUI()
		{
			GUILayout.Label(ModelImporterModelEditor.styles.TangentSpace, EditorStyles.boldLabel, new GUILayoutOption[0]);
			bool flag = true;
			UnityEngine.Object[] targets = base.targets;
			for (int i = 0; i < targets.Length; i++)
			{
				ModelImporter modelImporter = (ModelImporter)targets[i];
				if (!modelImporter.isTangentImportSupported)
				{
					flag = false;
				}
			}
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.Popup(this.m_NormalImportMode, ModelImporterModelEditor.styles.NormalModeLabelsAll, ModelImporterModelEditor.styles.TangentSpaceNormalLabel, new GUILayoutOption[0]);
			if (EditorGUI.EndChangeCheck())
			{
				if (this.m_NormalImportMode.intValue == 2)
				{
					this.m_TangentImportMode.intValue = 2;
				}
				else if (this.m_NormalImportMode.intValue == 0 && flag)
				{
					this.m_TangentImportMode.intValue = 0;
				}
				else
				{
					this.m_TangentImportMode.intValue = 3;
				}
			}
			using (new EditorGUI.DisabledScope(this.m_NormalImportMode.intValue != 1))
			{
				EditorGUILayout.Popup(this.m_NormalCalculationMode, ModelImporterModelEditor.styles.RecalculateNormalsOpt, ModelImporterModelEditor.styles.RecalculateNormalsLabel, new GUILayoutOption[0]);
				EditorGUI.BeginChangeCheck();
				EditorGUILayout.Slider(this.m_NormalSmoothAngle, 0f, 180f, ModelImporterModelEditor.styles.SmoothingAngle, new GUILayoutOption[0]);
				if (EditorGUI.EndChangeCheck())
				{
					this.m_NormalSmoothAngle.floatValue = Mathf.Round(this.m_NormalSmoothAngle.floatValue);
				}
			}
			GUIContent[] displayedOptions = ModelImporterModelEditor.styles.TangentSpaceModeOptLabelsAll;
			ModelImporterTangents[] array = ModelImporterModelEditor.styles.TangentSpaceModeOptEnumsAll;
			if (this.m_NormalImportMode.intValue == 1 || !flag)
			{
				displayedOptions = ModelImporterModelEditor.styles.TangentSpaceModeOptLabelsCalculate;
				array = ModelImporterModelEditor.styles.TangentSpaceModeOptEnumsCalculate;
			}
			else if (this.m_NormalImportMode.intValue == 2)
			{
				displayedOptions = ModelImporterModelEditor.styles.TangentSpaceModeOptLabelsNone;
				array = ModelImporterModelEditor.styles.TangentSpaceModeOptEnumsNone;
			}
			using (new EditorGUI.DisabledScope(this.m_NormalImportMode.intValue == 2))
			{
				int num = Array.IndexOf<ModelImporterTangents>(array, (ModelImporterTangents)this.m_TangentImportMode.intValue);
				EditorGUI.BeginChangeCheck();
				num = EditorGUILayout.Popup(ModelImporterModelEditor.styles.TangentSpaceTangentLabel, num, displayedOptions, new GUILayoutOption[0]);
				if (EditorGUI.EndChangeCheck())
				{
					this.m_TangentImportMode.intValue = (int)array[num];
				}
			}
		}

		private void ScaleAvatar()
		{
			if (!this.m_GlobalScale.hasMultipleDifferentValues)
			{
				if (base.targets.Length == 1)
				{
					UnityEngine.Object[] targets = base.targets;
					for (int i = 0; i < targets.Length; i++)
					{
						object obj = targets[i];
						float globalScale = (obj as ModelImporter).globalScale;
						float floatValue = this.m_GlobalScale.floatValue;
						if (globalScale != floatValue && floatValue != 0f && globalScale != 0f)
						{
							float d = floatValue / globalScale;
							SerializedProperty serializedProperty = base.serializedObject.FindProperty(AvatarSetupTool.sSkeleton);
							for (int j = 0; j < serializedProperty.arraySize; j++)
							{
								SerializedProperty arrayElementAtIndex = serializedProperty.GetArrayElementAtIndex(j);
								arrayElementAtIndex.FindPropertyRelative(AvatarSetupTool.sPosition).vector3Value = arrayElementAtIndex.FindPropertyRelative(AvatarSetupTool.sPosition).vector3Value * d;
							}
						}
					}
				}
			}
		}
	}
}
