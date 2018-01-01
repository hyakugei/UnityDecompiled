using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityEditor.Experimental.UIElements.GraphView
{
	[Serializable]
	public abstract class GraphViewPresenter : ScriptableObject
	{
		[SerializeField]
		protected List<GraphElementPresenter> m_Elements;

		[SerializeField]
		private List<GraphElementPresenter> m_TempElements;

		[SerializeField]
		private Vector3 m_Position;

		[SerializeField]
		private Vector3 m_Scale;

		public virtual Vector3 position
		{
			get
			{
				return this.m_Position;
			}
			set
			{
				this.m_Position = value;
			}
		}

		public virtual Vector3 scale
		{
			get
			{
				return this.m_Scale;
			}
			set
			{
				this.m_Scale = value;
			}
		}

		public IEnumerable<GraphElementPresenter> allChildren
		{
			get
			{
				return this.m_Elements.SelectMany((GraphElementPresenter e) => e.allElements);
			}
		}

		public IEnumerable<GraphElementPresenter> elements
		{
			get
			{
				return this.m_Elements.Union(this.m_TempElements);
			}
		}

		public virtual void AddElement(GraphElementPresenter element)
		{
			this.m_Elements.Add(element);
		}

		public virtual void AddElement(EdgePresenter edge)
		{
			this.AddElement(edge);
		}

		public virtual void RemoveElement(GraphElementPresenter element)
		{
			element.OnRemoveFromGraph();
			this.m_Elements.Remove(element);
		}

		protected void OnEnable()
		{
			if (this.m_Elements == null)
			{
				this.m_Elements = new List<GraphElementPresenter>();
			}
			if (this.m_TempElements == null)
			{
				this.m_TempElements = new List<GraphElementPresenter>();
			}
			this.m_Elements.Clear();
			this.m_TempElements.Clear();
		}

		public void AddTempElement(GraphElementPresenter element)
		{
			this.m_TempElements.Add(element);
		}

		public void RemoveTempElement(GraphElementPresenter element)
		{
			element.OnRemoveFromGraph();
			this.m_TempElements.Remove(element);
		}

		public void ClearTempElements()
		{
			this.m_TempElements.Clear();
		}

		public virtual List<PortPresenter> GetCompatiblePorts(PortPresenter startPort, NodeAdapter nodeAdapter)
		{
			return (from nap in this.allChildren.OfType<PortPresenter>()
			where nap.IsConnectable() && nap.orientation == startPort.orientation && nap.direction != startPort.direction && nodeAdapter.GetAdapter(nap.source, startPort.source) != null
			select nap).ToList<PortPresenter>();
		}
	}
}
