using System;
using System.Linq;
using UnityEditor.Experimental.AssetImporters;
using UnityEngine;

namespace UnityEditor.Presets
{
	internal class PresetContextMenu : PresetSelectorReceiver
	{
		private UnityEngine.Object[] m_Targets;

		private Preset[] m_Presets;

		private AssetImporterEditor[] m_ImporterEditors;

		private SerializedObject m_ImporterSerialized;

		internal static void CreateAndShow(UnityEngine.Object[] targets)
		{
			PresetContextMenu presetContextMenu = ScriptableObject.CreateInstance<PresetContextMenu>();
			if (targets[0] is AssetImporter)
			{
				presetContextMenu.m_ImporterEditors = (from e in Resources.FindObjectsOfTypeAll<AssetImporterEditor>()
				where e.targets == targets
				select e).ToArray<AssetImporterEditor>();
				presetContextMenu.m_Targets = new UnityEngine.Object[]
				{
					UnityEngine.Object.Instantiate(targets[0])
				};
				presetContextMenu.m_ImporterSerialized = new SerializedObject(targets);
				SerializedProperty iterator = presetContextMenu.m_ImporterEditors[0].m_SerializedObject.GetIterator();
				while (iterator.Next(true))
				{
					presetContextMenu.m_ImporterSerialized.CopyFromSerializedProperty(iterator);
				}
			}
			else
			{
				presetContextMenu.m_Targets = targets;
				presetContextMenu.m_Presets = (from t in targets
				select new Preset(t)).ToArray<Preset>();
			}
			PresetSelector.ShowSelector(targets[0], null, true, presetContextMenu);
		}

		private void RevertValues()
		{
			if (this.m_ImporterEditors != null)
			{
				AssetImporterEditor[] importerEditors = this.m_ImporterEditors;
				for (int i = 0; i < importerEditors.Length; i++)
				{
					AssetImporterEditor assetImporterEditor = importerEditors[i];
					assetImporterEditor.m_SerializedObject.SetIsDifferentCacheDirty();
					assetImporterEditor.m_SerializedObject.Update();
					SerializedProperty iterator = this.m_ImporterSerialized.GetIterator();
					while (iterator.Next(true))
					{
						assetImporterEditor.m_SerializedObject.CopyFromSerializedPropertyIfDifferent(iterator);
					}
				}
			}
			else
			{
				Undo.RecordObjects(this.m_Targets, "Cancel Preset");
				for (int j = 0; j < this.m_Targets.Length; j++)
				{
					this.m_Presets[j].ApplyTo(this.m_Targets[j]);
				}
			}
		}

		public override void OnSelectionChanged(Preset selection)
		{
			if (selection == null)
			{
				this.RevertValues();
			}
			else
			{
				Undo.RecordObjects(this.m_Targets, "Apply Preset " + selection.name);
				UnityEngine.Object[] targets = this.m_Targets;
				for (int i = 0; i < targets.Length; i++)
				{
					UnityEngine.Object target = targets[i];
					selection.ApplyTo(target);
				}
				if (this.m_ImporterEditors != null)
				{
					AssetImporterEditor[] importerEditors = this.m_ImporterEditors;
					for (int j = 0; j < importerEditors.Length; j++)
					{
						AssetImporterEditor assetImporterEditor = importerEditors[j];
						assetImporterEditor.m_SerializedObject.SetIsDifferentCacheDirty();
						assetImporterEditor.m_SerializedObject.Update();
						SerializedProperty iterator = new SerializedObject(this.m_Targets).GetIterator();
						while (iterator.Next(true))
						{
							assetImporterEditor.m_SerializedObject.CopyFromSerializedPropertyIfDifferent(iterator);
						}
					}
				}
			}
			InspectorWindow.RepaintAllInspectors();
		}

		public override void OnSelectionClosed(Preset selection)
		{
			this.OnSelectionChanged(selection);
			UnityEngine.Object.DestroyImmediate(this);
		}
	}
}
