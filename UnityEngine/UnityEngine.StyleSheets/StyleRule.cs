using System;
using UnityEngine.Bindings;

namespace UnityEngine.StyleSheets
{
	[VisibleToOtherModules(new string[]
	{
		"UnityEngine.UIElementsModule"
	})]
	[Serializable]
	internal class StyleRule
	{
		[SerializeField]
		private StyleProperty[] m_Properties;

		[SerializeField]
		internal int line;

		public StyleProperty[] properties
		{
			get
			{
				return this.m_Properties;
			}
			internal set
			{
				this.m_Properties = value;
			}
		}
	}
}
