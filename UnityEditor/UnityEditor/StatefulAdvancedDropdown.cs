using System;
using UnityEngine;

namespace UnityEditor
{
	internal class StatefulAdvancedDropdown
	{
		[NonSerialized]
		private AdvancedDropdown s_Instance;

		public Action<string> onSelected
		{
			get;
			set;
		}

		public string Label
		{
			get;
			set;
		}

		public int SelectedIndex
		{
			get;
			set;
		}

		public string[] DisplayedOptions
		{
			get;
			set;
		}

		public void Show(Rect rect)
		{
			if (this.s_Instance != null)
			{
				this.s_Instance.Close();
				this.s_Instance = null;
			}
			this.s_Instance = ScriptableObject.CreateInstance<AdvancedDropdown>();
			this.s_Instance.DisplayedOptions = this.DisplayedOptions;
			this.s_Instance.SelectedIndex = this.SelectedIndex;
			this.s_Instance.Label = this.Label;
			this.s_Instance.onSelected += delegate(AdvancedDropdownWindow w)
			{
				this.onSelected(w.GetIdOfSelectedItem());
			};
			this.s_Instance.Init(rect);
		}
	}
}
