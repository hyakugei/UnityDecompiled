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
		private static class Styles
		{
			public static GUIContent ImportMaterials = EditorGUIUtility.TrTextContent("Import Materials", null, null);

			public static GUIContent MaterialLocation = EditorGUIUtility.TrTextContent("Location", null, null);

			public static GUIContent[] MaterialLocationOpt = new GUIContent[]
			{
				EditorGUIUtility.TrTextContent("Use External Materials (Legacy)", "Use external materials if found in the project.", null),
				EditorGUIUtility.TrTextContent("Use Embedded Materials", "Embed the material inside the imported asset.", null)
			};

			public static GUIContent MaterialName = EditorGUIUtility.TrTextContent("Naming", null, null);

			public static GUIContent[] MaterialNameOptMain = new GUIContent[]
			{
				EditorGUIUtility.TrTextContent("By Base Texture Name", null, null),
				EditorGUIUtility.TrTextContent("From Model's Material", null, null),
				EditorGUIUtility.TrTextContent("Model Name + Model's Material", null, null)
			};

			public static GUIContent[] MaterialNameOptAll = new GUIContent[]
			{
				EditorGUIUtility.TrTextContent("By Base Texture Name", null, null),
				EditorGUIUtility.TrTextContent("From Model's Material", null, null),
				EditorGUIUtility.TrTextContent("Model Name + Model's Material", null, null),
				EditorGUIUtility.TrTextContent("Texture Name or Model Name + Model's Material (Obsolete)", null, null)
			};

			public static GUIContent MaterialSearch = EditorGUIUtility.TrTextContent("Search", null, null);

			public static GUIContent[] MaterialSearchOpt = new GUIContent[]
			{
				EditorGUIUtility.TrTextContent("Local Materials Folder", null, null),
				EditorGUIUtility.TrTextContent("Recursive-Up", null, null),
				EditorGUIUtility.TrTextContent("Project-Wide", null, null)
			};

			public static GUIContent NoMaterialHelp = EditorGUIUtility.TrTextContent("Do not generate materials. Use Unity's default material instead.", null, null);

			public static GUIContent ExternalMaterialHelpStart = EditorGUIUtility.TrTextContent("For each imported material, Unity first looks for an existing material named %MAT%.", null, null);

			public static GUIContent[] ExternalMaterialNameHelp = new GUIContent[]
			{
				EditorGUIUtility.TrTextContent("[BaseTextureName]", null, null),
				EditorGUIUtility.TrTextContent("[MaterialName]", null, null),
				EditorGUIUtility.TrTextContent("[ModelFileName]-[MaterialName]", null, null),
				EditorGUIUtility.TrTextContent("[BaseTextureName] or [ModelFileName]-[MaterialName] if no base texture can be found", null, null)
			};

			public static GUIContent[] ExternalMaterialSearchHelp = new GUIContent[]
			{
				EditorGUIUtility.TrTextContent("Unity will look for it in the local Materials folder.", null, null),
				EditorGUIUtility.TrTextContent("Unity will do a recursive-up search for it in all Materials folders up to the Assets folder.", null, null),
				EditorGUIUtility.TrTextContent("Unity will search for it anywhere inside the Assets folder.", null, null)
			};

			public static GUIContent ExternalMaterialHelpEnd = EditorGUIUtility.TrTextContent("If it doesn't exist, a new one is created in the local Materials folder.", null, null);

			public static GUIContent InternalMaterialHelp = EditorGUIUtility.TrTextContent("Materials are embedded inside the imported asset.", null, null);

			public static GUIContent MaterialAssignmentsHelp = EditorGUIUtility.TrTextContent("Material assignments can be remapped below.", null, null);

			public static GUIContent ExternalMaterialMappings = EditorGUIUtility.TrTextContent("Remapped Materials", "External materials to use for each embedded material.", null);

			public static GUIContent NoMaterialMappingsHelp = EditorGUIUtility.TrTextContent("Re-import the asset to see the list of used materials.", null, null);

			public static GUIContent Textures = EditorGUIUtility.TrTextContent("Textures", null, null);

			public static GUIContent ExtractEmbeddedTextures = EditorGUIUtility.TrTextContent("Extract Textures...", "Click on this button to extract the embedded textures.", null);

			public static GUIContent Materials = EditorGUIUtility.TrTextContent("Materials", null, null);

			public static GUIContent ExtractEmbeddedMaterials = EditorGUIUtility.TrTextContent("Extract Materials...", "Click on this button to extract the embedded materials.", null);

			public static GUIContent RemapOptions = EditorGUIUtility.TrTextContent("On Demand Remap", null, null);

			public static GUIContent RemapMaterialsInProject = EditorGUIUtility.TrTextContent("Search and Remap", "Click on this button to search and remap the materials from the project.", null);
		}

		private bool m_ShowAllMaterialNameOptions = true;

		private bool m_ShowMaterialRemapOptions = false;

		private SerializedProperty m_ImportMaterials;

		private SerializedProperty m_MaterialName;

		private SerializedProperty m_MaterialSearch;

		private SerializedProperty m_MaterialLocation;

		private SerializedProperty m_Materials;

		private SerializedProperty m_ExternalObjects;

		private SerializedProperty m_HasEmbeddedTextures;

		private SerializedProperty m_SupportsEmbeddedMaterials;

		private bool m_HasEmbeddedMaterials = false;

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
					using (Dictionary<AssetImporter.SourceAssetIdentifier, UnityEngine.Object>.Enumerator enumerator = externalObjectMap.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							KeyValuePair<AssetImporter.SourceAssetIdentifier, UnityEngine.Object> entry = enumerator.Current;
							if (entry.Key.type == typeof(Material) && Array.Exists<AssetImporter.SourceAssetIdentifier>(sourceMaterials, (AssetImporter.SourceAssetIdentifier x) => x.name == entry.Key.name))
							{
								num++;
							}
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
			this.DoMaterialsGUI();
		}

		private void ExtractTexturesGUI()
		{
			using (new EditorGUILayout.HorizontalScope(new GUILayoutOption[0]))
			{
				EditorGUILayout.PrefixLabel(ModelImporterMaterialEditor.Styles.Textures);
				using (new EditorGUI.DisabledScope(!this.m_HasEmbeddedTextures.boolValue && !this.m_HasEmbeddedTextures.hasMultipleDifferentValues))
				{
					if (GUILayout.Button(ModelImporterMaterialEditor.Styles.ExtractEmbeddedTextures, new GUILayoutOption[0]))
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

		private bool ExtractMaterialsGUI()
		{
			bool result;
			using (new EditorGUILayout.HorizontalScope(new GUILayoutOption[0]))
			{
				EditorGUILayout.PrefixLabel(ModelImporterMaterialEditor.Styles.Materials);
				using (new EditorGUI.DisabledScope(!this.HasEmbeddedMaterials()))
				{
					if (GUILayout.Button(ModelImporterMaterialEditor.Styles.ExtractEmbeddedMaterials, new GUILayoutOption[0]))
					{
						string text = (base.target as ModelImporter).assetPath;
						text = EditorUtility.SaveFolderPanel("Select Materials Folder", FileUtil.DeleteLastPathNameComponent(text), "");
						if (string.IsNullOrEmpty(text))
						{
							result = false;
							return result;
						}
						text = FileUtil.GetProjectRelativePath(text);
						try
						{
							AssetDatabase.StartAssetEditing();
							PrefabUtility.ExtractMaterialsFromAsset(base.targets, text);
						}
						finally
						{
							AssetDatabase.StopAssetEditing();
						}
						result = true;
						return result;
					}
				}
			}
			result = false;
			return result;
		}

		private bool MaterialRemapOptons()
		{
			this.m_ShowMaterialRemapOptions = EditorGUILayout.Foldout(this.m_ShowMaterialRemapOptions, ModelImporterMaterialEditor.Styles.RemapOptions);
			bool result;
			if (this.m_ShowMaterialRemapOptions)
			{
				EditorGUI.indentLevel++;
				EditorGUILayout.Popup(this.m_MaterialName, (!this.m_ShowAllMaterialNameOptions) ? ModelImporterMaterialEditor.Styles.MaterialNameOptMain : ModelImporterMaterialEditor.Styles.MaterialNameOptAll, ModelImporterMaterialEditor.Styles.MaterialName, new GUILayoutOption[0]);
				EditorGUILayout.Popup(this.m_MaterialSearch, ModelImporterMaterialEditor.Styles.MaterialSearchOpt, ModelImporterMaterialEditor.Styles.MaterialSearch, new GUILayoutOption[0]);
				string message = ModelImporterMaterialEditor.Styles.ExternalMaterialHelpStart.text.Replace("%MAT%", ModelImporterMaterialEditor.Styles.ExternalMaterialNameHelp[this.m_MaterialName.intValue].text) + "\n" + ModelImporterMaterialEditor.Styles.ExternalMaterialSearchHelp[this.m_MaterialSearch.intValue].text;
				EditorGUILayout.HelpBox(message, MessageType.Info);
				EditorGUI.indentLevel--;
				using (new EditorGUILayout.HorizontalScope(new GUILayoutOption[0]))
				{
					GUILayout.FlexibleSpace();
					using (new EditorGUI.DisabledScope(base.assetTarget == null))
					{
						if (GUILayout.Button(ModelImporterMaterialEditor.Styles.RemapMaterialsInProject, new GUILayoutOption[0]))
						{
							try
							{
								AssetDatabase.StartAssetEditing();
								UnityEngine.Object[] targets = base.targets;
								for (int i = 0; i < targets.Length; i++)
								{
									UnityEngine.Object @object = targets[i];
									ModelImporter modelImporter = @object as ModelImporter;
									modelImporter.SearchAndRemapMaterials((ModelImporterMaterialName)this.m_MaterialName.intValue, (ModelImporterMaterialSearch)this.m_MaterialSearch.intValue);
									AssetDatabase.WriteImportSettingsIfDirty(modelImporter.assetPath);
									AssetDatabase.ImportAsset(modelImporter.assetPath, ImportAssetOptions.ForceUpdate);
								}
							}
							finally
							{
								AssetDatabase.StopAssetEditing();
							}
							result = true;
							return result;
						}
					}
				}
				EditorGUILayout.Space();
			}
			result = false;
			return result;
		}

		private void DoMaterialsGUI()
		{
			base.serializedObject.UpdateIfRequiredOrScript();
			EditorGUILayout.PropertyField(this.m_ImportMaterials, ModelImporterMaterialEditor.Styles.ImportMaterials, new GUILayoutOption[0]);
			string text = string.Empty;
			if (!this.m_ImportMaterials.hasMultipleDifferentValues)
			{
				if (this.m_ImportMaterials.boolValue)
				{
					EditorGUILayout.Popup(this.m_MaterialLocation, ModelImporterMaterialEditor.Styles.MaterialLocationOpt, ModelImporterMaterialEditor.Styles.MaterialLocation, new GUILayoutOption[0]);
					if (!this.m_MaterialLocation.hasMultipleDifferentValues)
					{
						if (this.m_MaterialLocation.intValue == 0)
						{
							EditorGUILayout.Popup(this.m_MaterialName, (!this.m_ShowAllMaterialNameOptions) ? ModelImporterMaterialEditor.Styles.MaterialNameOptMain : ModelImporterMaterialEditor.Styles.MaterialNameOptAll, ModelImporterMaterialEditor.Styles.MaterialName, new GUILayoutOption[0]);
							EditorGUILayout.Popup(this.m_MaterialSearch, ModelImporterMaterialEditor.Styles.MaterialSearchOpt, ModelImporterMaterialEditor.Styles.MaterialSearch, new GUILayoutOption[0]);
							text = string.Concat(new string[]
							{
								ModelImporterMaterialEditor.Styles.ExternalMaterialHelpStart.text.Replace("%MAT%", ModelImporterMaterialEditor.Styles.ExternalMaterialNameHelp[this.m_MaterialName.intValue].text),
								"\n",
								ModelImporterMaterialEditor.Styles.ExternalMaterialSearchHelp[this.m_MaterialSearch.intValue].text,
								"\n",
								ModelImporterMaterialEditor.Styles.ExternalMaterialHelpEnd.text
							});
						}
						else if (this.m_Materials.arraySize > 0 && this.HasEmbeddedMaterials())
						{
							text = ModelImporterMaterialEditor.Styles.InternalMaterialHelp.text;
						}
					}
					if (base.targets.Length == 1 && this.m_Materials.arraySize > 0 && this.m_MaterialLocation.intValue != 0)
					{
						text = text + " " + ModelImporterMaterialEditor.Styles.MaterialAssignmentsHelp.text;
					}
					if (this.m_MaterialLocation.intValue != 0 && !this.m_MaterialLocation.hasMultipleDifferentValues)
					{
						this.ExtractTexturesGUI();
						if (this.ExtractMaterialsGUI())
						{
							return;
						}
					}
				}
				else
				{
					text = ModelImporterMaterialEditor.Styles.NoMaterialHelp.text;
				}
			}
			if (!string.IsNullOrEmpty(text))
			{
				EditorGUILayout.HelpBox(text, MessageType.Info);
			}
			if ((base.targets.Length == 1 || !this.m_SupportsEmbeddedMaterials.hasMultipleDifferentValues) && !this.m_SupportsEmbeddedMaterials.boolValue && this.m_MaterialLocation.intValue != 0 && !this.m_MaterialLocation.hasMultipleDifferentValues)
			{
				EditorGUILayout.Space();
				EditorGUILayout.HelpBox(ModelImporterMaterialEditor.Styles.NoMaterialMappingsHelp.text, MessageType.Warning);
			}
			if (this.m_ImportMaterials.boolValue && base.targets.Length == 1 && this.m_Materials.arraySize > 0 && this.m_MaterialLocation.intValue != 0 && !this.m_MaterialLocation.hasMultipleDifferentValues)
			{
				GUILayout.Label(ModelImporterMaterialEditor.Styles.ExternalMaterialMappings, EditorStyles.boldLabel, new GUILayoutOption[0]);
				if (!this.MaterialRemapOptons())
				{
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
}
