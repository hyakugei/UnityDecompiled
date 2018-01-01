using System;
using System.Collections.Generic;

namespace UnityEngine.Experimental.UIElements
{
	public abstract class MouseManipulator : Manipulator
	{
		private ManipulatorActivationFilter m_currentActivator;

		public List<ManipulatorActivationFilter> activators
		{
			get;
			private set;
		}

		public MouseManipulator()
		{
			this.activators = new List<ManipulatorActivationFilter>();
		}

		protected bool CanStartManipulation(IMouseEvent e)
		{
			bool result;
			foreach (ManipulatorActivationFilter current in this.activators)
			{
				if (current.Matches(e))
				{
					this.m_currentActivator = current;
					result = true;
					return result;
				}
			}
			result = false;
			return result;
		}

		protected bool CanStopManipulation(IMouseEvent e)
		{
			return e.button == (int)this.m_currentActivator.button && base.target.HasCapture();
		}
	}
}
