using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Scripting;

namespace UnityEditor.Experimental.AssetImporters
{
	public abstract class ScriptedImporter : AssetImporter
	{
		[RequiredByNativeCode]
		private void GenerateAssetData(AssetImportContext ctx)
		{
			this.OnImportAsset(ctx);
		}

		public abstract void OnImportAsset(AssetImportContext ctx);

		[RequiredByNativeCode]
		internal static void RegisterScriptedImporters()
		{
			IEnumerable<Type> allTypesWithAttribute = EditorAssemblies.GetAllTypesWithAttribute<ScriptedImporterAttribute>();
			foreach (Type current in allTypesWithAttribute)
			{
				Type type = current;
				ScriptedImporterAttribute scriptedImporterAttribute = Attribute.GetCustomAttribute(type, typeof(ScriptedImporterAttribute)) as ScriptedImporterAttribute;
				SortedDictionary<string, bool> handledExtensionsByImporter = ScriptedImporter.GetHandledExtensionsByImporter(scriptedImporterAttribute);
				foreach (Type current2 in allTypesWithAttribute)
				{
					if (current2 != current)
					{
						ScriptedImporterAttribute attribute = Attribute.GetCustomAttribute(current2, typeof(ScriptedImporterAttribute)) as ScriptedImporterAttribute;
						SortedDictionary<string, bool> handledExtensionsByImporter2 = ScriptedImporter.GetHandledExtensionsByImporter(attribute);
						foreach (KeyValuePair<string, bool> current3 in handledExtensionsByImporter2)
						{
							if (handledExtensionsByImporter.ContainsKey(current3.Key))
							{
								Debug.LogError(string.Format("Scripted importers {0} and {1} are targeting the {2} extension, rejecting both.", type.FullName, current2.FullName, current3.Key));
								handledExtensionsByImporter.Remove(current3.Key);
							}
						}
					}
				}
				MethodInfo method = type.GetMethod("GetHashOfImportedAssetDependencyHintsForTesting", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				foreach (KeyValuePair<string, bool> current4 in handledExtensionsByImporter)
				{
					AssetImporter.RegisterImporter(type, scriptedImporterAttribute.version, scriptedImporterAttribute.importQueuePriority, current4.Key, method != null);
				}
			}
		}

		private static SortedDictionary<string, bool> GetHandledExtensionsByImporter(ScriptedImporterAttribute attribute)
		{
			SortedDictionary<string, bool> sortedDictionary = new SortedDictionary<string, bool>();
			if (attribute.fileExtensions != null)
			{
				string[] fileExtensions = attribute.fileExtensions;
				for (int i = 0; i < fileExtensions.Length; i++)
				{
					string text = fileExtensions[i];
					string text2 = text;
					if (text2.StartsWith("."))
					{
						text2 = text2.Substring(1);
					}
					sortedDictionary.Add(text2, true);
				}
			}
			return sortedDictionary;
		}
	}
}
