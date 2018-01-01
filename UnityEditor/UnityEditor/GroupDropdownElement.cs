using System;
using UnityEngine;

namespace UnityEditor
{
	[Serializable]
	internal class GroupDropdownElement : DropdownElement
	{
		protected override GUIStyle labelStyle
		{
			get
			{
				return AdvancedDropdownWindow.s_Styles.groupButton;
			}
		}

		protected override bool drawArrow
		{
			get
			{
				return true;
			}
		}

		public GroupDropdownElement(string name) : this(name, name)
		{
		}

		public GroupDropdownElement(string name, string id) : base(name, id)
		{
		}

		public void SetSelectedIndex(int index)
		{
			base.selectedItem = index;
		}
	}
}
