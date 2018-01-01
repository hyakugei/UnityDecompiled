using System;
using System.Linq;
using UnityEngine;

namespace UnityEditor.Presets
{
	public class DefaultPresetSelectorReceiver : PresetSelectorReceiver
	{
		private UnityEngine.Object[] m_Targets;

		private Preset[] m_InitialValues;

		internal void Init(UnityEngine.Object[] targets)
		{
			this.m_Targets = targets;
			this.m_InitialValues = (from a in targets
			select new Preset(a)).ToArray<Preset>();
		}

		public override void OnSelectionChanged(Preset selection)
		{
			if (selection != null)
			{
				Undo.RecordObjects(this.m_Targets, "Apply Preset " + selection.name);
				UnityEngine.Object[] targets = this.m_Targets;
				for (int i = 0; i < targets.Length; i++)
				{
					UnityEngine.Object target = targets[i];
					selection.ApplyTo(target);
				}
			}
			else
			{
				Undo.RecordObjects(this.m_Targets, "Cancel Preset");
				for (int j = 0; j < this.m_Targets.Length; j++)
				{
					this.m_InitialValues[j].ApplyTo(this.m_Targets[j]);
				}
			}
		}

		public override void OnSelectionClosed(Preset selection)
		{
			this.OnSelectionChanged(selection);
			UnityEngine.Object.DestroyImmediate(this);
		}
	}
}
