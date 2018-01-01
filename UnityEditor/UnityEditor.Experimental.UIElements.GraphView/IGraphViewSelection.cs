using System;
using System.Collections.Generic;

namespace UnityEditor.Experimental.UIElements.GraphView
{
	internal interface IGraphViewSelection
	{
		int version
		{
			get;
			set;
		}

		List<string> selectedElements
		{
			get;
			set;
		}
	}
}
