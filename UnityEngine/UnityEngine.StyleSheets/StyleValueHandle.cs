using System;
using UnityEngine.Bindings;

namespace UnityEngine.StyleSheets
{
	[VisibleToOtherModules(new string[]
	{
		"UnityEngine.UIElementsModule"
	})]
	[Serializable]
	internal struct StyleValueHandle
	{
		[SerializeField]
		private StyleValueType m_ValueType;

		[VisibleToOtherModules(new string[]
		{
			"UnityEngine.UIElementsModule"
		}), SerializeField]
		internal int valueIndex;

		public StyleValueType valueType
		{
			get
			{
				return this.m_ValueType;
			}
			internal set
			{
				this.m_ValueType = value;
			}
		}

		internal StyleValueHandle(int valueIndex, StyleValueType valueType)
		{
			this.valueIndex = valueIndex;
			this.m_ValueType = valueType;
		}
	}
}
