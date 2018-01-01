using System;

namespace UnityEditor.Experimental.UIElements.GraphView
{
	[Serializable]
	internal class InputPortPresenter : PortPresenter
	{
		public override Direction direction
		{
			get
			{
				return Direction.Input;
			}
		}
	}
}
