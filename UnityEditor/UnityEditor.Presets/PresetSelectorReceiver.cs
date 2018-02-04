using System;
using UnityEngine;

namespace UnityEditor.Presets
{
	public abstract class PresetSelectorReceiver : ScriptableObject
	{
		public virtual void OnSelectionChanged(Preset selection)
		{
		}

		public virtual void OnSelectionClosed(Preset selection)
		{
		}
	}
}
