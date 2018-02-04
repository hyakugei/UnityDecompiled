using System;
using UnityEngine;

namespace UnityEditor.Experimental.UIElements.GraphView
{
	[Serializable]
	public struct SearchWindowContext
	{
		public Vector2 screenMousePosition
		{
			get;
			private set;
		}

		public float requestedWidth
		{
			get;
			private set;
		}

		public float requestedHeight
		{
			get;
			private set;
		}

		public SearchWindowContext(Vector2 screenMousePosition, float requestedWidth = 0f, float requestedHeight = 0f)
		{
			this.screenMousePosition = screenMousePosition;
			this.requestedWidth = requestedWidth;
			this.requestedHeight = requestedHeight;
		}
	}
}
