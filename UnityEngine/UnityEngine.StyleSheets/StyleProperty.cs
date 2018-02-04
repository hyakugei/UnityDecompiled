using System;
using UnityEngine.Bindings;

namespace UnityEngine.StyleSheets
{
	[VisibleToOtherModules(new string[]
	{
		"UnityEngine.UIElementsModule"
	})]
	[Serializable]
	internal class StyleProperty
	{
		[SerializeField]
		private string m_Name;

		[SerializeField]
		private StyleValueHandle[] m_Values;

		public string name
		{
			get
			{
				return this.m_Name;
			}
			internal set
			{
				this.m_Name = value;
			}
		}

		public StyleValueHandle[] values
		{
			get
			{
				return this.m_Values;
			}
			internal set
			{
				this.m_Values = value;
			}
		}
	}
}
