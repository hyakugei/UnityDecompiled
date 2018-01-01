using System;
using System.Linq;
using UnityEngine.Bindings;

namespace UnityEngine.StyleSheets
{
	[VisibleToOtherModules(new string[]
	{
		"UnityEngine.UIElementsModule"
	})]
	[Serializable]
	internal class StyleComplexSelector
	{
		[SerializeField]
		private int m_Specificity;

		[SerializeField]
		private StyleSelector[] m_Selectors;

		[VisibleToOtherModules(new string[]
		{
			"UnityEngine.UIElementsModule"
		}), SerializeField]
		internal int ruleIndex;

		public int specificity
		{
			get
			{
				return this.m_Specificity;
			}
			internal set
			{
				this.m_Specificity = value;
			}
		}

		public StyleRule rule
		{
			get;
			internal set;
		}

		public bool isSimple
		{
			get
			{
				return this.selectors.Length == 1;
			}
		}

		public StyleSelector[] selectors
		{
			get
			{
				return this.m_Selectors;
			}
			[VisibleToOtherModules(new string[]
			{
				"UnityEngine.UIElementsModule"
			})]
			internal set
			{
				this.m_Selectors = value;
			}
		}

		public override string ToString()
		{
			return string.Format("[{0}]", string.Join(", ", (from x in this.m_Selectors
			select x.ToString()).ToArray<string>()));
		}
	}
}
