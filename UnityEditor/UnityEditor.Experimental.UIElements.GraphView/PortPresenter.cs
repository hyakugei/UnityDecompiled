using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor.Experimental.UIElements.GraphView
{
	[Serializable]
	public abstract class PortPresenter : GraphElementPresenter
	{
		protected object m_Source;

		[SerializeField]
		private Orientation m_Orientation;

		[SerializeField]
		private Type m_PortType;

		[SerializeField]
		private bool m_Highlight = true;

		[SerializeField]
		private List<EdgePresenter> m_Connections;

		public object source
		{
			get
			{
				return this.m_Source;
			}
			set
			{
				if (this.m_Source != value)
				{
					this.m_Source = value;
				}
			}
		}

		public abstract Direction direction
		{
			get;
		}

		public virtual Orientation orientation
		{
			get
			{
				return this.m_Orientation;
			}
			set
			{
				this.m_Orientation = value;
			}
		}

		public virtual Type portType
		{
			get
			{
				return this.m_PortType;
			}
			set
			{
				this.m_PortType = value;
			}
		}

		public virtual bool highlight
		{
			get
			{
				return this.m_Highlight;
			}
			set
			{
				this.m_Highlight = value;
			}
		}

		public virtual bool connected
		{
			get
			{
				return this.m_Connections.Count != 0;
			}
		}

		public virtual bool collapsed
		{
			get
			{
				return false;
			}
		}

		public virtual IEnumerable<EdgePresenter> connections
		{
			get
			{
				return this.m_Connections;
			}
		}

		public virtual void Connect(EdgePresenter edgePresenter)
		{
			if (edgePresenter == null)
			{
				throw new ArgumentException("The value passed to PortPresenter.Connect is null");
			}
			if (!this.m_Connections.Contains(edgePresenter))
			{
				this.m_Connections.Add(edgePresenter);
			}
		}

		public virtual void Disconnect(EdgePresenter edgePresenter)
		{
			if (edgePresenter == null)
			{
				throw new ArgumentException("The value passed to PortPresenter.Disconnect is null");
			}
			this.m_Connections.Remove(edgePresenter);
		}

		public bool IsConnectable()
		{
			return true;
		}

		protected virtual void SetCapabilities()
		{
			base.capabilities = (Capabilities)0;
		}

		protected new void OnEnable()
		{
			base.OnEnable();
			this.m_PortType = typeof(object);
			this.m_Connections = new List<EdgePresenter>();
			this.SetCapabilities();
		}
	}
}
