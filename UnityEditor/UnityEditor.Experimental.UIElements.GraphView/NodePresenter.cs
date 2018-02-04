using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityEditor.Experimental.UIElements.GraphView
{
	[Serializable]
	public class NodePresenter : SimpleElementPresenter
	{
		[SerializeField]
		protected List<PortPresenter> m_InputPorts;

		[SerializeField]
		protected List<PortPresenter> m_OutputPorts;

		[SerializeField]
		private bool m_expanded;

		protected Orientation m_Orientation;

		public List<PortPresenter> inputPorts
		{
			get
			{
				List<PortPresenter> arg_1C_0;
				if ((arg_1C_0 = this.m_InputPorts) == null)
				{
					arg_1C_0 = (this.m_InputPorts = new List<PortPresenter>());
				}
				return arg_1C_0;
			}
		}

		public List<PortPresenter> outputPorts
		{
			get
			{
				List<PortPresenter> arg_1C_0;
				if ((arg_1C_0 = this.m_OutputPorts) == null)
				{
					arg_1C_0 = (this.m_OutputPorts = new List<PortPresenter>());
				}
				return arg_1C_0;
			}
		}

		public virtual bool expanded
		{
			get
			{
				return this.m_expanded;
			}
			set
			{
				this.m_expanded = value;
			}
		}

		public virtual Orientation orientation
		{
			get
			{
				return this.m_Orientation;
			}
		}

		public override IEnumerable<GraphElementPresenter> allChildren
		{
			get
			{
				return this.inputPorts.Concat(this.outputPorts).Cast<GraphElementPresenter>();
			}
		}

		public override IEnumerable<GraphElementPresenter> allElements
		{
			get
			{
				NodePresenter.<>c__Iterator0 <>c__Iterator = new NodePresenter.<>c__Iterator0();
				<>c__Iterator.$this = this;
				NodePresenter.<>c__Iterator0 expr_0E = <>c__Iterator;
				expr_0E.$PC = -2;
				return expr_0E;
			}
		}

		protected NodePresenter()
		{
			this.m_expanded = true;
			this.m_Orientation = Orientation.Horizontal;
		}

		protected new void OnEnable()
		{
			base.OnEnable();
			base.capabilities |= Capabilities.Deletable;
		}
	}
}
