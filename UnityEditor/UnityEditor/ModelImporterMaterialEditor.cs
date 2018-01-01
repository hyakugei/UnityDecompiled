using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Experimental.AssetImporters;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
	internal class ModelImporterMaterialEditor : BaseAssetImporterTabUI
	{
		private class Styles
		{
			public GUIContent ImportMaterials = EditorGUIUtility.TextContent("Import Materials");

			public GUIContent MaterialLocation = EditorGUIUtility.TextContent("Material Location");

			public GUIContent[] MaterialLocationOpt = new GUIContent[]
			{
				EditorGUIUtility.TextContent("Use External Materials (Legacy)|Use external materials if found in the project."),
				EditorGUIUtility.TextContent("Use Embedded Materials|Embed the material inside the imported asset.")
			};

			public GUIContent MaterialName = EditorGUIUtility.TextContent("Material Naming");

			public GUIContent[] MaterialNameOptMain = new GUIContent[]
			{
				EditorGUIUtility.TextContent("By Base Texture Name"),
				EditorGUIUtility.TextContent("From Model's Material"),
				EditorGUIUtility.TextContent("Model Name + Model's Material")
			};

			public GUIContent[] MaterialNameOptAll = new GUIContent[]
			{
				EditorGUIUtility.TextContent("By Base Texture Name"),
				EditorGUIUtility.TextContent("From Model's Material"),
				EditorGUIUtility.TextContent("Model Name + Model's Material"),
				EditorGUIUtility.TextContent("Texture Name or Model Name + Model's Material (Obsolete)")
			};

			public GUIContent MaterialSearch = EditorGUIUtility.TextContent("Material Search");

			public GUIContent[] MaterialSearchOpt = new GUIContent[]
			{
				EditorGUIUtility.TextContent("Local Materials Folder"),
				EditorGUIUtility.TextContent("Recursive-Up"),
				EditorGUIUtility.TextContent("Project-Wide")
			};

			public GUIContent AutoMapExternalMaterials = EditorGUIUtility.TextContent("Map External Materials|Map the external materials found in the project automatically every time the asset is reimported.");

			public GUIContent NoMaterialHelp = EditorGUIUtility.TextContent("Do not generate materials. Use Unity's default material instead.");

			public GUIContent ExternalMaterialHelpStart = EditorGUIUtility.TextContent("For each imported material, Unity first looks for an existing material named %MAT%.");

			public GUIContent[] ExternalMaterialNameHelp = new GUIContent[]
			{
				EditorGUIUtility.TextContent("[BaseTextureName]"),
				EditorGUIUtility.TextContent("[MaterialName]"),
				EditorGUIUtility.TextContent("[ModelFileName]-[MaterialName]"),
				EditorGUIUtility.TextContent("[BaseTextureName] or [ModelFileName]-[MaterialName] if no base texture can be found")
			};

			public GUIContent[] ExternalMaterialSearchHelp = new GUIContent[]
			{
				EditorGUIUtility.TextContent("Unity will look for it in the local Materials folder."),
				EditorGUIUtility.TextContent("Unity will do a recursive-up search for it in all Materials folders up to the Assets folder."),
				EditorGUIUtility.TextContent("Unity will search for it anywhere inside the Assets folder.")
			};

			public GUIContent ExternalMaterialHelpEnd = EditorGUIUtility.TextContent("If it doesn't exist, a new one is created in the local Materials folder.");

			public GUIContent InternalMaterialHelp = EditorGUIUtility.TextContent("Materials are embedded inside the imported asset.");

			public GUIContent MaterialAssignmentsHelp = EditorGUIUtility.TextContent("Material assignments can be remapped below.");

			public GUIContent ExternalMaterialMappings = EditorGUIUtility.TextContent("Remapped Materials|External materials to use for each embedded material.");

			public GUIContent NoMaterialMappingsHelp = EditorGUIUtility.TextContent("Re-import the asset to see the list of used materials.");

			public GUIContent Textures = EditorGUIUtility.TextContent("Textures");

			public GUIContent ExtractEmbeddedTextures = EditorGUIUtility.TextContent("Extract Textures...|Click on this button to extract the embedded textures.");

			public GUIContent Materials = EditorGUIUtility.TextContent("Materials");

			public GUIContent ExtractEmbeddedMaterials = EditorGUIUtility.TextContent("Extract Materials...|Click on this button to extract the embedded materials.");
		}

		private bool m_ShowAllMaterialNameOptions = true;

		private SerializedProperty m_ImportMaterials;

		private SerializedProperty m_MaterialName;

		private SerializedProperty m_MaterialSearch;

		private SerializedProperty m_MaterialLocation;

		private SerializedProperty m_Materials;

		private SerializedProperty m_ExternalObjects;

		private SerializedProperty m_HasEmbeddedTextures;

		private SerializedProperty m_SupportsEmbeddedMaterials;

		private bool m_HasEmbeddedMaterials = false;

		private static ModelImporterMaterialEditor.Styles styles;

		public ModelImporterMaterialEditor(AssetImporterEditor panelContainer) : base(panelContainer)
		{
		}

		private void UpdateShowAllMaterialNameOptions()
		{
			this.m_MaterialName = base.serializedObject.FindProperty("m_MaterialName");
			this.m_ShowAllMaterialNameOptions = (this.m_MaterialName.intValue == 3);
		}

		private bool HasEmbeddedMaterials()
		{
			bool result;
			if (this.m_Materials.arraySize == 0)
			{
				result = false;
			}
			else if (this.m_ExternalObjects.serializedObject.hasModifiedProperties)
			{
				result = this.m_HasEmbeddedMaterials;
			}
			else
			{
				this.m_HasEmbeddedMaterials = true;
				UnityEngine.Object[] targetObjects = this.m_ExternalObjects.serializedObject.targetObjects;
				for (int i = 0; i < targetObjects.Length; i++)
				{
					UnityEngine.Object @object = targetObjects[i];
					ModelImporter modelImporter = @object as ModelImporter;
					Dictionary<AssetImporter.SourceAssetIdentifier, UnityEngine.Object> externalObjectMap = modelImporter.GetExternalObjectMap();
					AssetImporter.SourceAssetIdentifier[] sourceMaterials = modelImporter.sourceMaterials;
					int num = 0;
					foreach (KeyValuePair<AssetImporter.SourceAssetIdentifier, UnityEngine.Object> current in externalObjectMap)
					{
						if (current.Key.type == typeof(Material))
						{
							num++;
						}
					}
					this.m_HasEmbeddedMaterials = (this.m_HasEmbeddedMaterials && num != sourceMaterials.Length);
				}
				result = this.m_HasEmbeddedMaterials;
			}
			return result;
		}

		internal override void OnEnable()
		{
			this.m_ImportMaterials = base.serializedObject.FindProperty("m_ImportMaterials");
			this.m_MaterialName = base.serializedObject.FindProperty("m_MaterialName");
			this.m_MaterialSearch = base.serializedObject.FindProperty("m_MaterialSearch");
			this.m_MaterialLocation = base.serializedObject.FindProperty("m_MaterialLocation");
			this.m_Materials = base.serializedObject.FindProperty("m_Materials");
			this.m_ExternalObjects = base.serializedObject.FindProperty("m_ExternalObjects");
			this.m_HasEmbeddedTextures = base.serializedObject.FindProperty("m_HasEmbeddedTextures");
			this.m_SupportsEmbeddedMaterials = base.serializedObject.FindProperty("m_SupportsEmbeddedMaterials");
			this.UpdateShowAllMaterialNameOptions();
		}

		internal override void ResetValues()
		{
			base.ResetValues();
			this.UpdateShowAllMaterialNameOptions();
		}

		internal override void PostApply()
		{
			this.UpdateShowAllMaterialNameOptions();
		}

		public override void OnInspectorGUI()
		{
			if (ModelImporterMaterialEditor.styles == null)
			{
				ModelImporterMaterialEditor.styles = new ModelImporterMaterialEditor.Styles();
			}
			this.MaterialsGUI();
		}

		private void TexturesGUI()
		{
			using (new EditorGUILayout.HorizontalScope(new GUILayoutOption[0]))
			{
				EditorGUILayout.PrefixLabel(ModelImporterMaterialEditor.styles.Textures);
				using (new EditorGUI.DisabledScope(!this.m_HasEmbeddedTextures.boolValue && !this.m_HasEmbeddedTextures.hasMultipleDifferentValues))
				{
					if (GUILayout.Button(ModelImporterMaterialEditor.styles.ExtractEmbeddedTextures, new GUILayoutOption[0]))
					{
						List<Tuple<UnityEngine.Object, string>> list = new List<Tuple<UnityEngine.Object, string>>();
						string text = (base.target as ModelImporter).assetPath;
						text = EditorUtility.SaveFolderPanel("Select Textures Folder", FileUtil.DeleteLastPathNameComponent(text), "");
						if (!string.IsNullOrEmpty(text))
						{
							text = FileUtil.GetProjectRelativePath(text);
							try
							{
								AssetDatabase.StartAssetEditing();
								UnityEngine.Object[] targets = base.targets;
								for (int i = 0; i < targets.Length; i++)
								{
									UnityEngine.Object @object = targets[i];
									string text2 = FileUtil.GetUniqueTempPathInProject();
									text2 = text2.Replace("Temp", InternalEditorUtility.GetAssetsFolder());
									list.Add(Tuple.Create<UnityEngine.Object, string>(@object, text2));
									ModelImporter modelImporter = @object as ModelImporter;
									modelImporter.ExtractTextures(text2);
								}
							}
							finally
							{
								AssetDatabase.StopAssetEditing();
							}
							try
							{
								AssetDatabase.Refresh();
								AssetDatabase.StartAssetEditing();
								foreach (Tuple<UnityEngine.Object, string> current in list)
								{
									ModelImporter modelImporter2 = current.Item1 as ModelImporter;
									string[] array = AssetDatabase.FindAssets("t:Texture", new string[]
									{
										current.Item2
									});
									string[] array2 = array;
									for (int j = 0; j < array2.Length; j++)
									{
										string guid = array2[j];
										string text3 = AssetDatabase.GUIDToAssetPath(guid);
										Texture texture = AssetDatabase.LoadAssetAtPath<Texture>(text3);
										if (!(texture == null))
										{
											modelImporter2.AddRemap(new AssetImporter.SourceAssetIdentifier(texture), texture);
											string text4 = Path.Combine(text, FileUtil.UnityGetFileName(text3));
											text4 = AssetDatabase.GenerateUniqueAssetPath(text4);
											AssetDatabase.MoveAsset(text3, text4);
										}
									}
									AssetDatabase.ImportAsset(modelImporter2.assetPath, ImportAssetOptions.ForceUpdate);
									AssetDatabase.DeleteAsset(current.Item2);
								}
							}
							finally
							{
								AssetDatabase.StopAssetEditing();
							}
						}
					}
				}
			}
		}

		private void MaterialsGUI()
		{
			base.serializedObject.UpdateIfRequiredOrScript();
			EditorGUILayout.PropertyField(this.m_ImportMaterials, ModelImporterMaterialEditor.styles.ImportMaterials, new GUILayoutOption[0]);
			string text = string.Empty;
			if (!this.m_ImportMaterials.hasMultipleDifferentValues)
			{
				if (this.m_ImportMaterials.boolValue)
				{
					EditorGUILayout.Popup(this.m_MaterialLocation, ModelImporterMaterialEditor.styles.MaterialLocationOpt, ModelImporterMaterialEditor.styles.MaterialLocation, new GUILayoutOption[0]);
					if (!this.m_MaterialLocation.hasMultipleDifferentValues)
					{
						if (this.m_MaterialLocation.intValue == 0)
						{
							EditorGUILayout.Popup(this.m_MaterialName, (!this.m_ShowAllMaterialNameOptions) ? ModelImporterMaterialEditor.styles.MaterialNameOptMain : ModelImporterMaterialEditor.styles.MaterialNameOptAll, ModelImporterMaterialEditor.styles.MaterialName, new GUILayoutOption[0]);
							EditorGUILayout.Popup(this.m_MaterialSearch, ModelImporterMaterialEditor.styles.MaterialSearchOpt, ModelImporterMaterialEditor.styles.MaterialSearch, new GUILayoutOption[0]);
							text = string.Concat(new string[]
							{
								ModelImporterMaterialEditor.styles.ExternalMaterialHelpStart.text.Replace("%MAT%", ModelImporterMaterialEditor.styles.ExternalMaterialNameHelp[this.m_MaterialName.intValue].text),
								"\n",
								ModelImporterMaterialEditor.styles.ExternalMaterialSearchHelp[this.m_MaterialSearch.intValue].text,
								"\n",
								ModelImporterMaterialEditor.styles.ExternalMaterialHelpEnd.text
							});
						}
						else if (this.m_Materials.arraySize > 0)
						{
							text = ModelImporterMaterialEditor.styles.InternalMaterialHelp.text;
						}
					}
					if (base.targets.Length == 1 && this.m_Materials.arraySize > 0 && this.m_MaterialLocation.intValue != 0)
					{
						text = text + " " + ModelImporterMaterialEditor.styles.MaterialAssignmentsHelp.text;
					}
					if (this.m_MaterialLocation.intValue != 0 && !this.m_MaterialLocation.hasMultipleDifferentValues)
					{
						this.TexturesGUI();
						using (new EditorGUILayout.HorizontalScope(new GUILayoutOption[0]))
						{
							EditorGUILayout.PrefixLabel(ModelImporterMaterialEditor.styles.Materials);
							using (new EditorGUI.DisabledScope(!this.HasEmbeddedMaterials()))
							{
								if (GUILayout.Button(ModelImporterMaterialEditor.styles.ExtractEmbeddedMaterials, new GUILayoutOption[0]))
								{
									string text2 = (base.target as ModelImporter).assetPath;
									text2 = EditorUtility.SaveFolderPanel("Select Materials Folder", FileUtil.DeleteLastPathNameComponent(text2), "");
									if (string.IsNullOrEmpty(text2))
									{
										return;
									}
									text2 = FileUtil.GetProjectRelativePath(text2);
									try
									{
										AssetDatabase.StartAssetEditing();
										PrefabUtility.ExtractMaterialsFromAsset(base.targets, text2);
									}
									finally
									{
										AssetDatabase.StopAssetEditing();
									}
									return;
								}
							}
						}
					}
				}
				else
				{
					text = ModelImporterMaterialEditor.styles.NoMaterialHelp.text;
				}
			}
			if (!string.IsNullOrEmpty(text))
			{
				EditorGUILayout.HelpBox(text, MessageType.Info);
			}
			if ((base.targets.Length == 1 || !this.m_SupportsEmbeddedMaterials.hasMultipleDifferentValues) && !this.m_SupportsEmbeddedMaterials.boolValue && this.m_MaterialLocation.intValue != 0 && !this.m_MaterialLocation.hasMultipleDifferentValues)
			{
				EditorGUILayout.Space();
				EditorGUILayout.HelpBox(ModelImporterMaterialEditor.styles.NoMaterialMappingsHelp.text, MessageType.Warning);
			}
			if (this.m_ImportMaterials.boolValue && base.targets.Length == 1 && this.m_Materials.arraySize > 0 && this.m_MaterialLocation.intValue != 0 && !this.m_MaterialLocation.hasMultipleDifferentValues)
			{
				GUILayout.Label(ModelImporterMaterialEditor.styles.ExternalMaterialMappings, EditorStyles.boldLabel, new GUILayoutOption[0]);
				for (int i = 0; i < this.m_Materials.arraySize; i++)
				{
					SerializedProperty arrayElementAtIndex = this.m_Materials.GetArrayElementAtIndex(i);
					string stringValue = arrayElementAtIndex.FindPropertyRelative("name").stringValue;
					string stringValue2 = arrayElementAtIndex.FindPropertyRelative("type").stringValue;
					string stringValue3 = arrayElementAtIndex.FindPropertyRelative("assembly").stringValue;
					SerializedProperty serializedProperty = null;
					Material material = null;
					int index = 0;
					int j = 0;
					int arraySize = this.m_ExternalObjects.arraySize;
					while (j < arraySize)
					{
						SerializedProperty arrayElementAtIndex2 = this.m_ExternalObjects.GetArrayElementAtIndex(j);
						string stringValue4 = arrayElementAtIndex2.FindPropertyRelative("first.name").stringValue;
						string stringValue5 = arrayElementAtIndex2.FindPropertyRelative("first.type").stringValue;
						if (stringValue4 == stringValue && stringValue5 == stringValue2)
						{
							serializedProperty = arrayElementAtIndex2.FindPropertyRelative("second");
							material = ((serializedProperty == null) ? null : (serializedProperty.objectReferenceValue as Material));
							index = j;
							break;
						}
						j++;
					}
					GUIContent gUIContent = EditorGUIUtility.TextContent(stringValue);
					gUIContent.tooltip = stringValue;
					if (serializedProperty != null)
					{
						EditorGUI.BeginChangeCheck();
						EditorGUILayout.ObjectField(serializedProperty, typeof(Material), gUIContent, new GUILayoutOption[0]);
						if (EditorGUI.EndChangeCheck())
						{
							if (serializedProperty.objectReferenceValue == null)
							{
								this.m_ExternalObjects.DeleteArrayElementAtIndex(index);
							}
						}
					}
					else
					{
						EditorGUI.BeginChangeCheck();
						material = (EditorGUILayout.ObjectField(gUIContent, material, typeof(Material), false, new GUILayoutOption[0]) as Material);
						if (EditorGUI.EndChangeCheck())
						{
							if (material != null)
							{
								int index2 = this.m_ExternalObjects.arraySize++;
								SerializedProperty arrayElementAtIndex3 = this.m_ExternalObjects.GetArrayElementAtIndex(index2);
								arrayElementAtIndex3.FindPropertyRelative("first.name").stringValue = stringValue;
								arrayElementAtIndex3.FindPropertyRelative("first.type").stringValue = stringValue2;
								arrayElementAtIndex3.FindPropertyRelative("first.assembly").stringValue = stringValue3;
								arrayElementAtIndex3.FindPropertyRelative("second").objectReferenceValue = material;
							}
						}
					}
				}
			}
		}
	}
}
