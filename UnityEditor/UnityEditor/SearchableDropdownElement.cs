using System;

namespace UnityEditor
{
	internal class SearchableDropdownElement : DropdownElement
	{
		protected override bool isSearchable
		{
			get
			{
				return true;
			}
		}

		public SearchableDropdownElement(string name, int index) : this(name, name, index)
		{
		}

		public SearchableDropdownElement(string name, string id, int index) : base(name, id, index)
		{
		}
	}
}
