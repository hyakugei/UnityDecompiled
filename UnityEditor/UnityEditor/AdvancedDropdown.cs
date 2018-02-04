using System;
using System.Linq;

namespace UnityEditor
{
	internal class AdvancedDropdown : AdvancedDropdownWindow
	{
		private string[] m_DisplayedOptions;

		private string m_Label;

		private static int m_SelectedIndex;

		public string[] DisplayedOptions
		{
			set
			{
				this.m_DisplayedOptions = value;
			}
		}

		public string Label
		{
			set
			{
				this.m_Label = value;
			}
		}

		public int SelectedIndex
		{
			set
			{
				AdvancedDropdown.m_SelectedIndex = value;
			}
		}

		protected override DropdownElement RebuildTree()
		{
			GroupDropdownElement groupDropdownElement = new GroupDropdownElement(this.m_Label);
			for (int i = 0; i < this.m_DisplayedOptions.Length; i++)
			{
				string text = this.m_DisplayedOptions[i];
				string[] array = text.Split(new char[]
				{
					'/'
				});
				DropdownElement dropdownElement = groupDropdownElement;
				for (int j = 0; j < array.Length; j++)
				{
					string name = array[j];
					if (j == array.Length - 1)
					{
						SearchableDropdownElement searchableDropdownElement = new SearchableDropdownElement(name, text, i);
						searchableDropdownElement.SetParent(dropdownElement);
						dropdownElement.AddChild(searchableDropdownElement);
						if (i == AdvancedDropdown.m_SelectedIndex)
						{
							DropdownElement dropdownElement2 = dropdownElement;
							DropdownElement item = searchableDropdownElement;
							while (dropdownElement2 != null)
							{
								dropdownElement2.selectedItem = dropdownElement2.children.IndexOf(item);
								item = dropdownElement2;
								dropdownElement2 = dropdownElement2.parent;
							}
						}
					}
					else
					{
						string groupPathId = "";
						for (int k = 0; k <= j; k++)
						{
							groupPathId = groupPathId + array[k] + ".";
						}
						DropdownElement dropdownElement3 = dropdownElement.children.SingleOrDefault((DropdownElement c) => c.id == groupPathId);
						if (dropdownElement3 == null)
						{
							dropdownElement3 = new GroupDropdownElement(name, groupPathId);
							dropdownElement3.SetParent(dropdownElement);
							dropdownElement.AddChild(dropdownElement3);
						}
						dropdownElement = dropdownElement3;
					}
				}
			}
			return groupDropdownElement;
		}
	}
}
