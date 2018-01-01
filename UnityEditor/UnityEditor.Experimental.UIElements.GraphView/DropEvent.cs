using System;
using System.Collections.Generic;
using UnityEngine.Experimental.UIElements;

namespace UnityEditor.Experimental.UIElements.GraphView
{
	public delegate void DropEvent(IMGUIEvent evt, List<ISelectable> selection, IDropTarget dropTarget);
}
