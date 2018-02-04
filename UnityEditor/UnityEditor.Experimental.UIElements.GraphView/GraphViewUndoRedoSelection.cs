using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor.Experimental.UIElements.GraphView
{
	internal class GraphViewUndoRedoSelection : ScriptableObject, IGraphViewSelection
	{
		[SerializeField]
		private int m_Version;

		[SerializeField]
		private List<string> m_SelectedElements;

		public int version
		{
			get
			{
				return this.m_Version;
			}
			set
			{
				this.m_Version = value;
			}
		}

		public List<string> selectedElements
		{
			get
			{
				return this.m_SelectedElements;
			}
			set
			{
				this.m_SelectedElements = value;
			}
		}
	}
}
