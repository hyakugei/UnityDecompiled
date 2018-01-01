using System;

namespace UnityEditor.Experimental.UIElements.GraphView
{
	[Serializable]
	internal class OutputPortPresenter : PortPresenter
	{
		public override Direction direction
		{
			get
			{
				return Direction.Output;
			}
		}
	}
}
