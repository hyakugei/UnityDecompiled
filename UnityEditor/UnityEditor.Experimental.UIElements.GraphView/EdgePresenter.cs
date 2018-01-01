using System;
using UnityEngine;

namespace UnityEditor.Experimental.UIElements.GraphView
{
	[Serializable]
	public class EdgePresenter : GraphElementPresenter
	{
		[SerializeField]
		protected PortPresenter m_OutputPresenter;

		[SerializeField]
		protected PortPresenter m_InputPresenter;

		public virtual PortPresenter output
		{
			get
			{
				return this.m_OutputPresenter;
			}
			set
			{
				if (this.m_OutputPresenter != null)
				{
					this.m_OutputPresenter.Disconnect(this);
				}
				this.m_OutputPresenter = value;
				if (this.m_OutputPresenter != null)
				{
					this.m_OutputPresenter.Connect(this);
				}
			}
		}

		public virtual PortPresenter input
		{
			get
			{
				return this.m_InputPresenter;
			}
			set
			{
				if (this.m_InputPresenter != null)
				{
					this.m_InputPresenter.Disconnect(this);
				}
				this.m_InputPresenter = value;
				if (this.m_InputPresenter != null)
				{
					this.m_InputPresenter.Connect(this);
				}
			}
		}

		public Vector2 candidatePosition
		{
			get;
			set;
		}

		public bool candidate
		{
			get;
			set;
		}

		protected EdgePresenter()
		{
		}

		protected new void OnEnable()
		{
			base.OnEnable();
			base.capabilities = (Capabilities.Selectable | Capabilities.Deletable);
		}
	}
}
