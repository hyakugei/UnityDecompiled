using System;
using System.Collections.Generic;

namespace UnityEngine.Experimental.UIElements
{
	public class VisualElementFocusRing : IFocusRing
	{
		public enum DefaultFocusOrder
		{
			ChildOrder,
			PositionXY,
			PositionYX
		}

		private struct FocusRingRecord
		{
			public int m_AutoIndex;

			public Focusable m_Focusable;
		}

		private VisualElement root;

		private List<VisualElementFocusRing.FocusRingRecord> m_FocusRing;

		public VisualElementFocusRing.DefaultFocusOrder defaultFocusOrder
		{
			get;
			set;
		}

		public VisualElementFocusRing(VisualElement root, VisualElementFocusRing.DefaultFocusOrder dfo = VisualElementFocusRing.DefaultFocusOrder.ChildOrder)
		{
			this.defaultFocusOrder = dfo;
			this.root = root;
			this.m_FocusRing = new List<VisualElementFocusRing.FocusRingRecord>();
		}

		private int FocusRingSort(VisualElementFocusRing.FocusRingRecord a, VisualElementFocusRing.FocusRingRecord b)
		{
			int result;
			if (a.m_Focusable.focusIndex == 0 && b.m_Focusable.focusIndex == 0)
			{
				switch (this.defaultFocusOrder)
				{
				case VisualElementFocusRing.DefaultFocusOrder.ChildOrder:
					IL_42:
					result = Comparer<int>.Default.Compare(a.m_AutoIndex, b.m_AutoIndex);
					return result;
				case VisualElementFocusRing.DefaultFocusOrder.PositionXY:
				{
					VisualElement visualElement = a.m_Focusable as VisualElement;
					VisualElement visualElement2 = b.m_Focusable as VisualElement;
					if (visualElement != null && visualElement2 != null)
					{
						if (visualElement.layout.position.x < visualElement2.layout.position.x)
						{
							result = -1;
							return result;
						}
						if (visualElement.layout.position.x > visualElement2.layout.position.x)
						{
							result = 1;
							return result;
						}
						if (visualElement.layout.position.y < visualElement2.layout.position.y)
						{
							result = -1;
							return result;
						}
						if (visualElement.layout.position.y > visualElement2.layout.position.y)
						{
							result = 1;
							return result;
						}
					}
					result = Comparer<int>.Default.Compare(a.m_AutoIndex, b.m_AutoIndex);
					return result;
				}
				case VisualElementFocusRing.DefaultFocusOrder.PositionYX:
				{
					VisualElement visualElement3 = a.m_Focusable as VisualElement;
					VisualElement visualElement4 = b.m_Focusable as VisualElement;
					if (visualElement3 != null && visualElement4 != null)
					{
						if (visualElement3.layout.position.y < visualElement4.layout.position.y)
						{
							result = -1;
							return result;
						}
						if (visualElement3.layout.position.y > visualElement4.layout.position.y)
						{
							result = 1;
							return result;
						}
						if (visualElement3.layout.position.x < visualElement4.layout.position.x)
						{
							result = -1;
							return result;
						}
						if (visualElement3.layout.position.x > visualElement4.layout.position.x)
						{
							result = 1;
							return result;
						}
					}
					result = Comparer<int>.Default.Compare(a.m_AutoIndex, b.m_AutoIndex);
					return result;
				}
				}
				goto IL_42;
			}
			if (a.m_Focusable.focusIndex == 0)
			{
				result = 1;
			}
			else if (b.m_Focusable.focusIndex == 0)
			{
				result = -1;
			}
			else
			{
				result = Comparer<int>.Default.Compare(a.m_Focusable.focusIndex, b.m_Focusable.focusIndex);
			}
			return result;
		}

		private void DoUpdate()
		{
			this.m_FocusRing.Clear();
			if (this.root != null)
			{
				int num = 0;
				this.BuildRingRecursive(this.root, ref num);
				this.m_FocusRing.Sort(new Comparison<VisualElementFocusRing.FocusRingRecord>(this.FocusRingSort));
			}
		}

		private void BuildRingRecursive(VisualElement vc, ref int focusIndex)
		{
			for (int i = 0; i < vc.shadow.childCount; i++)
			{
				VisualElement visualElement = vc.shadow[i];
				if (visualElement.canGrabFocus)
				{
					this.m_FocusRing.Add(new VisualElementFocusRing.FocusRingRecord
					{
						m_AutoIndex = focusIndex++,
						m_Focusable = visualElement
					});
				}
				this.BuildRingRecursive(visualElement, ref focusIndex);
			}
		}

		private int GetFocusableInternalIndex(Focusable f)
		{
			int result;
			if (f != null)
			{
				for (int i = 0; i < this.m_FocusRing.Count; i++)
				{
					if (f == this.m_FocusRing[i].m_Focusable)
					{
						result = i;
						return result;
					}
				}
			}
			result = -1;
			return result;
		}

		public FocusChangeDirection GetFocusChangeDirection(Focusable currentFocusable, EventBase e)
		{
			FocusChangeDirection result;
			if (currentFocusable is IMGUIContainer && e.imguiEvent != null)
			{
				result = FocusChangeDirection.none;
			}
			else
			{
				if (e.GetEventTypeId() == EventBase<KeyDownEvent>.TypeId())
				{
					KeyDownEvent keyDownEvent = e as KeyDownEvent;
					EventModifiers modifiers = keyDownEvent.modifiers;
					if (keyDownEvent.keyCode == KeyCode.Tab)
					{
						if (currentFocusable == null)
						{
							result = FocusChangeDirection.none;
							return result;
						}
						if ((modifiers & EventModifiers.Shift) == EventModifiers.None)
						{
							result = VisualElementFocusChangeDirection.right;
							return result;
						}
						result = VisualElementFocusChangeDirection.left;
						return result;
					}
				}
				result = FocusChangeDirection.none;
			}
			return result;
		}

		public Focusable GetNextFocusable(Focusable currentFocusable, FocusChangeDirection direction)
		{
			Focusable result;
			if (direction == FocusChangeDirection.none || direction == FocusChangeDirection.unspecified)
			{
				result = currentFocusable;
			}
			else
			{
				this.DoUpdate();
				if (this.m_FocusRing.Count == 0)
				{
					result = null;
				}
				else
				{
					int num = 0;
					if (direction == VisualElementFocusChangeDirection.right)
					{
						num = this.GetFocusableInternalIndex(currentFocusable) + 1;
						if (num == this.m_FocusRing.Count)
						{
							num = 0;
						}
					}
					else if (direction == VisualElementFocusChangeDirection.left)
					{
						num = this.GetFocusableInternalIndex(currentFocusable) - 1;
						if (num == -1)
						{
							num = this.m_FocusRing.Count - 1;
						}
					}
					result = this.m_FocusRing[num].m_Focusable;
				}
			}
			return result;
		}
	}
}
