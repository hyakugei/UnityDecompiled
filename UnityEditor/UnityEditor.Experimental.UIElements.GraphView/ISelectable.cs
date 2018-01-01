using System;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace UnityEditor.Experimental.UIElements.GraphView
{
	public interface ISelectable
	{
		bool IsSelectable();

		bool HitTest(Vector2 localPoint);

		bool Overlaps(Rect rectangle);

		void Select(VisualElement selectionContainer, bool additive);

		void Unselect(VisualElement selectionContainer);

		bool IsSelected(VisualElement selectionContainer);
	}
}
