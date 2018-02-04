using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.Experimental.UIElements.StyleSheets;

namespace UnityEditor.Experimental.UIElements.GraphView
{
	public class FreehandSelector : MouseManipulator
	{
		private class FreehandElement : VisualElement
		{
			private List<Vector2> m_Points = new List<Vector2>();

			private const string k_SegmentSizeProperty = "segment-size";

			private const string k_SegmentColorProperty = "segment-color";

			private const string k_DeleteSegmentColorProperty = "delete-segment-color";

			private StyleValue<float> m_SegmentSize;

			private StyleValue<Color> m_SegmentColor;

			private StyleValue<Color> m_DeleteSegmentColor;

			public List<Vector2> points
			{
				get
				{
					return this.m_Points;
				}
			}

			public bool deleteModifier
			{
				private get;
				set;
			}

			public float segmentSize
			{
				get
				{
					return this.m_SegmentSize.GetSpecifiedValueOrDefault(5f);
				}
			}

			public Color segmentColor
			{
				get
				{
					return this.m_SegmentColor.GetSpecifiedValueOrDefault(new Color(1f, 0.6f, 0f));
				}
			}

			public Color deleteSegmentColor
			{
				get
				{
					return this.m_DeleteSegmentColor.GetSpecifiedValueOrDefault(new Color(1f, 0f, 0f));
				}
			}

			protected override void OnStyleResolved(ICustomStyle styles)
			{
				base.OnStyleResolved(styles);
				styles.ApplyCustomProperty("segment-size", ref this.m_SegmentSize);
				styles.ApplyCustomProperty("segment-color", ref this.m_SegmentColor);
				styles.ApplyCustomProperty("delete-segment-color", ref this.m_DeleteSegmentColor);
			}

			public override void DoRepaint()
			{
				int count = this.points.Count;
				if (count >= 1)
				{
					Color c = (!this.deleteModifier) ? this.segmentColor : this.deleteSegmentColor;
					HandleUtility.ApplyWireMaterial();
					GL.Begin(1);
					GL.Color(c);
					for (int i = 1; i < count; i++)
					{
						Vector2 v = this.points[i - 1] + base.parent.layout.position;
						Vector2 v2 = this.points[i] + base.parent.layout.position;
						this.DrawDottedLine(v, v2, this.segmentSize);
					}
					GL.End();
				}
			}

			private void DrawDottedLine(Vector3 p1, Vector3 p2, float segmentsLength)
			{
				float num = Vector3.Distance(p1, p2);
				int num2 = Mathf.CeilToInt(num / segmentsLength);
				for (int i = 0; i < num2; i += 2)
				{
					GL.Vertex(Vector3.Lerp(p1, p2, (float)i * segmentsLength / num));
					GL.Vertex(Vector3.Lerp(p1, p2, (float)(i + 1) * segmentsLength / num));
				}
			}
		}

		private readonly FreehandSelector.FreehandElement m_FreehandElement;

		private bool m_Active;

		private GraphView m_GraphView;

		public FreehandSelector()
		{
			base.activators.Add(new ManipulatorActivationFilter
			{
				button = MouseButton.LeftMouse,
				modifiers = EventModifiers.Control
			});
			base.activators.Add(new ManipulatorActivationFilter
			{
				button = MouseButton.LeftMouse,
				modifiers = (EventModifiers.Shift | EventModifiers.Control)
			});
			this.m_FreehandElement = new FreehandSelector.FreehandElement();
			this.m_FreehandElement.StretchToParentSize();
		}

		protected override void RegisterCallbacksOnTarget()
		{
			this.m_GraphView = (base.target as GraphView);
			if (this.m_GraphView == null)
			{
				throw new InvalidOperationException("Manipulator can only be added to a GraphView");
			}
			base.target.RegisterCallback<MouseDownEvent>(new EventCallback<MouseDownEvent>(this.OnMouseDown), Capture.NoCapture);
			base.target.RegisterCallback<MouseUpEvent>(new EventCallback<MouseUpEvent>(this.OnMouseUp), Capture.NoCapture);
			base.target.RegisterCallback<MouseMoveEvent>(new EventCallback<MouseMoveEvent>(this.OnMouseMove), Capture.NoCapture);
		}

		protected override void UnregisterCallbacksFromTarget()
		{
			base.target.UnregisterCallback<MouseDownEvent>(new EventCallback<MouseDownEvent>(this.OnMouseDown), Capture.NoCapture);
			base.target.UnregisterCallback<MouseUpEvent>(new EventCallback<MouseUpEvent>(this.OnMouseUp), Capture.NoCapture);
			base.target.UnregisterCallback<MouseMoveEvent>(new EventCallback<MouseMoveEvent>(this.OnMouseMove), Capture.NoCapture);
			this.m_GraphView = null;
		}

		private void OnMouseDown(MouseDownEvent e)
		{
			if (this.m_Active)
			{
				e.StopImmediatePropagation();
			}
			else if (e.target == base.target)
			{
				if (base.CanStartManipulation(e))
				{
					this.m_GraphView.ClearSelection();
					this.m_GraphView.Add(this.m_FreehandElement);
					this.m_FreehandElement.points.Clear();
					this.m_FreehandElement.points.Add(e.localMousePosition);
					this.m_FreehandElement.deleteModifier = e.shiftKey;
					this.m_Active = true;
					base.target.TakeMouseCapture();
					e.StopPropagation();
				}
			}
		}

		private void OnMouseUp(MouseUpEvent e)
		{
			if (this.m_Active && base.CanStopManipulation(e))
			{
				this.m_GraphView.Remove(this.m_FreehandElement);
				this.m_FreehandElement.points.Add(e.localMousePosition);
				List<ISelectable> selection = this.m_GraphView.selection;
				List<ISelectable> newSelection = new List<ISelectable>();
				this.m_GraphView.graphElements.ForEach(delegate(GraphElement element)
				{
					if (element.IsSelectable())
					{
						for (int i = 1; i < this.m_FreehandElement.points.Count; i++)
						{
							Vector2 vector = this.m_GraphView.ChangeCoordinatesTo(element, this.m_FreehandElement.points[i - 1]);
							Vector2 vector2 = this.m_GraphView.ChangeCoordinatesTo(element, this.m_FreehandElement.points[i]);
							float num = Mathf.Min(vector.x, vector2.x);
							float num2 = Mathf.Max(vector.x, vector2.x);
							float num3 = Mathf.Min(vector.y, vector2.y);
							float num4 = Mathf.Max(vector.y, vector2.y);
							Rect rectangle = new Rect(num, num3, num2 - num + 1f, num4 - num3 + 1f);
							if (element.Overlaps(rectangle))
							{
								newSelection.Add(element);
								break;
							}
						}
					}
				});
				foreach (ISelectable current in newSelection)
				{
					if (!selection.Contains(current))
					{
						this.m_GraphView.AddToSelection(current);
					}
				}
				if (e.shiftKey)
				{
					this.m_GraphView.DeleteSelection();
				}
				this.m_Active = false;
				base.target.ReleaseMouseCapture();
				e.StopPropagation();
			}
		}

		private void OnMouseMove(MouseMoveEvent e)
		{
			if (this.m_Active)
			{
				this.m_FreehandElement.points.Add(e.localMousePosition);
				this.m_FreehandElement.deleteModifier = e.shiftKey;
				e.StopPropagation();
			}
		}
	}
}
