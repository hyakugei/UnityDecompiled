using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace UnityEditor.Experimental.UIElements.GraphView
{
	public class MiniMap : GraphElement
	{
		private float m_PreviousContainerWidth = -1f;

		private float m_PreviousContainerHeight = -1f;

		private readonly Label m_Label;

		private Dragger m_Dragger;

		private readonly Color m_ViewportColor = new Color(1f, 1f, 0f, 0.35f);

		private readonly Color m_SelectedChildrenColor = new Color(1f, 1f, 1f, 0.5f);

		private Rect m_ViewportRect;

		private Rect m_ContentRect;

		private Rect m_ContentRectLocal;

		private bool m_Anchored;

		private GraphView m_GraphView;

		private static Vector3[] s_CachedRect = new Vector3[4];

		[CompilerGenerated]
		private static Func<EventBase, ContextualMenu.MenuAction.StatusFlags> <>f__mg$cache0;

		public float maxHeight
		{
			get;
			set;
		}

		public float maxWidth
		{
			get;
			set;
		}

		private int titleBarOffset
		{
			get
			{
				return (int)base.style.paddingTop;
			}
		}

		public bool anchored
		{
			get
			{
				return this.m_Anchored;
			}
			set
			{
				if (this.m_Anchored != value)
				{
					this.m_Anchored = value;
					if (this.m_Anchored)
					{
						base.capabilities &= ~Capabilities.Movable;
						base.ResetPositionProperties();
						base.AddToClassList("anchored");
					}
					else
					{
						base.capabilities |= Capabilities.Movable;
						base.RemoveFromClassList("anchored");
					}
					this.Resize();
				}
			}
		}

		private GraphView graphView
		{
			get
			{
				if (this.m_GraphView == null)
				{
					this.m_GraphView = base.GetFirstAncestorOfType<GraphView>();
				}
				return this.m_GraphView;
			}
		}

		public MiniMap()
		{
			base.clippingOptions = VisualElement.ClippingOptions.NoClipping;
			base.capabilities = Capabilities.Movable;
			this.m_Dragger = new Dragger
			{
				clampToParentEdges = true
			};
			this.AddManipulator(this.m_Dragger);
			this.anchored = false;
			this.maxWidth = 200f;
			this.maxHeight = 180f;
			this.m_Label = new Label("Floating Minimap");
			base.Add(this.m_Label);
			base.RegisterCallback<MouseDownEvent>(new EventCallback<MouseDownEvent>(this.OnMouseDown), Capture.NoCapture);
			this.AddManipulator(new ContextualMenuManipulator(new Action<ContextualMenuPopulateEvent>(this.BuildContextualMenu)));
		}

		private void ToggleAnchorState(EventBase e)
		{
			if (base.dependsOnPresenter)
			{
				MiniMapPresenter presenter = base.GetPresenter<MiniMapPresenter>();
				presenter.anchored = !presenter.anchored;
			}
			else
			{
				this.anchored = !this.anchored;
			}
		}

		public virtual void BuildContextualMenu(ContextualMenuPopulateEvent evt)
		{
			bool anchored;
			if (base.dependsOnPresenter)
			{
				MiniMapPresenter presenter = base.GetPresenter<MiniMapPresenter>();
				anchored = presenter.anchored;
			}
			else
			{
				anchored = this.anchored;
			}
			ContextualMenu arg_6E_0 = evt.menu;
			string arg_6E_1 = (!anchored) ? "Anchor" : "Make floating";
			Action<EventBase> arg_6E_2 = new Action<EventBase>(this.ToggleAnchorState);
			if (MiniMap.<>f__mg$cache0 == null)
			{
				MiniMap.<>f__mg$cache0 = new Func<EventBase, ContextualMenu.MenuAction.StatusFlags>(ContextualMenu.MenuAction.AlwaysEnabled);
			}
			arg_6E_0.AppendAction(arg_6E_1, arg_6E_2, MiniMap.<>f__mg$cache0);
		}

		public override void OnDataChanged()
		{
			base.OnDataChanged();
			this.AdjustAnchoring();
			MiniMapPresenter presenter = base.GetPresenter<MiniMapPresenter>();
			base.style.width = presenter.maxWidth;
			base.style.height = presenter.maxHeight;
			this.Resize();
			this.UpdatePresenterPosition();
		}

		private void AdjustAnchoring()
		{
			MiniMapPresenter presenter = base.GetPresenter<MiniMapPresenter>();
			if (!(presenter == null))
			{
				if (presenter.anchored)
				{
					presenter.capabilities &= ~Capabilities.Movable;
					base.ResetPositionProperties();
					base.AddToClassList("anchored");
				}
				else
				{
					base.presenter.capabilities |= Capabilities.Movable;
					base.RemoveFromClassList("anchored");
				}
			}
		}

		private void Resize()
		{
			if (base.parent != null)
			{
				base.style.width = this.maxWidth;
				base.style.height = this.maxHeight;
				if (base.style.positionLeft + base.style.width > base.parent.layout.x + base.parent.layout.width)
				{
					Rect layout = base.layout;
					layout.x -= base.style.positionLeft + base.style.width - (base.parent.layout.x + base.parent.layout.width);
					base.layout = layout;
				}
				if (base.style.positionTop + base.style.height > base.parent.layout.y + base.parent.layout.height)
				{
					Rect layout2 = base.layout;
					layout2.y -= base.style.positionTop + base.style.height - (base.parent.layout.y + base.parent.layout.height);
					base.layout = layout2;
				}
				Rect layout3 = base.layout;
				layout3.width = base.style.width;
				layout3.height = base.style.height;
				layout3.x = Mathf.Max(base.parent.layout.x, layout3.x);
				layout3.y = Mathf.Max(base.parent.layout.y, layout3.y);
				base.layout = layout3;
			}
		}

		private static void ChangeToMiniMapCoords(ref Rect rect, float factor, Vector3 translation)
		{
			rect.width *= factor;
			rect.height *= factor;
			rect.x *= factor;
			rect.y *= factor;
			rect.x += translation.x;
			rect.y += translation.y;
		}

		private void CalculateRects(VisualElement container)
		{
			this.m_ContentRect = this.graphView.CalculateRectToFitAll(container);
			this.m_ContentRectLocal = this.m_ContentRect;
			Matrix4x4 inverse = container.worldTransform.inverse;
			Vector4 column = inverse.GetColumn(3);
			Vector2 vector = new Vector2(inverse.m00, inverse.m11);
			this.m_ViewportRect = base.parent.rect;
			this.m_ViewportRect.x = this.m_ViewportRect.x + column.x;
			this.m_ViewportRect.y = this.m_ViewportRect.y + column.y;
			this.m_ViewportRect.x = this.m_ViewportRect.x + base.parent.worldBound.x * vector.x;
			this.m_ViewportRect.y = this.m_ViewportRect.y + base.parent.worldBound.y * vector.y;
			this.m_ViewportRect.width = this.m_ViewportRect.width * vector.x;
			this.m_ViewportRect.height = this.m_ViewportRect.height * vector.y;
			float m = container.worldTransform.m00;
			this.m_Label.text = "MiniMap  " + string.Format("{0:F2}", m) + "x";
			Rect rect = RectUtils.Encompass(this.m_ContentRect, this.m_ViewportRect);
			float factor = base.layout.width / rect.width;
			MiniMap.ChangeToMiniMapCoords(ref rect, factor, Vector3.zero);
			Vector3 translation = new Vector3(-rect.x, (float)this.titleBarOffset - rect.y);
			MiniMap.ChangeToMiniMapCoords(ref this.m_ViewportRect, factor, translation);
			MiniMap.ChangeToMiniMapCoords(ref this.m_ContentRect, factor, translation);
			if (rect.height > base.layout.height - (float)this.titleBarOffset)
			{
				float num = (base.layout.height - (float)this.titleBarOffset) / rect.height;
				float num2 = (base.layout.width - rect.width * num) / 2f;
				float num3 = (float)this.titleBarOffset - (rect.y + translation.y) * num;
				this.m_ContentRect.width = this.m_ContentRect.width * num;
				this.m_ContentRect.height = this.m_ContentRect.height * num;
				this.m_ContentRect.y = this.m_ContentRect.y * num;
				this.m_ContentRect.x = this.m_ContentRect.x + num2;
				this.m_ContentRect.y = this.m_ContentRect.y + num3;
				this.m_ViewportRect.width = this.m_ViewportRect.width * num;
				this.m_ViewportRect.height = this.m_ViewportRect.height * num;
				this.m_ViewportRect.y = this.m_ViewportRect.y * num;
				this.m_ViewportRect.x = this.m_ViewportRect.x + num2;
				this.m_ViewportRect.y = this.m_ViewportRect.y + num3;
			}
		}

		private Rect CalculateElementRect(GraphElement elem)
		{
			Rect result;
			if (elem is Edge)
			{
				result = new Rect(0f, 0f, 0f, 0f);
			}
			else
			{
				Rect localBound = elem.localBound;
				localBound.x = this.m_ContentRect.x + (localBound.x - this.m_ContentRectLocal.x) * this.m_ContentRect.width / this.m_ContentRectLocal.width;
				localBound.y = this.m_ContentRect.y + (localBound.y - this.m_ContentRectLocal.y) * this.m_ContentRect.height / this.m_ContentRectLocal.height;
				localBound.width *= this.m_ContentRect.width / this.m_ContentRectLocal.width;
				localBound.height *= this.m_ContentRect.height / this.m_ContentRectLocal.height;
				int num = 2;
				int num2 = 0;
				float num3 = base.layout.width - 2f;
				float num4 = base.layout.height - 2f;
				if (localBound.x < (float)num)
				{
					if (localBound.x < (float)num - localBound.width)
					{
						result = new Rect(0f, 0f, 0f, 0f);
						return result;
					}
					localBound.width -= (float)num - localBound.x;
					localBound.x = (float)num;
				}
				if (localBound.x + localBound.width >= num3)
				{
					if (localBound.x >= num3)
					{
						result = new Rect(0f, 0f, 0f, 0f);
						return result;
					}
					localBound.width -= localBound.x + localBound.width - num3;
				}
				if (localBound.y < (float)(num2 + this.titleBarOffset))
				{
					if (localBound.y < (float)(num2 + this.titleBarOffset) - localBound.height)
					{
						result = new Rect(0f, 0f, 0f, 0f);
						return result;
					}
					localBound.height -= (float)(num2 + this.titleBarOffset) - localBound.y;
					localBound.y = (float)(num2 + this.titleBarOffset);
				}
				if (localBound.y + localBound.height >= num4)
				{
					if (localBound.y >= num4)
					{
						result = new Rect(0f, 0f, 0f, 0f);
						return result;
					}
					localBound.height -= localBound.y + localBound.height - num4;
				}
				result = localBound;
			}
			return result;
		}

		public override void DoRepaint()
		{
			GraphView graphView = this.graphView;
			VisualElement contentViewContainer = graphView.contentViewContainer;
			Matrix4x4 matrix = graphView.viewTransform.matrix;
			Vector2 vector = new Vector2(matrix.m00, matrix.m11);
			float num = base.parent.layout.width / vector.x;
			float num2 = base.parent.layout.height / vector.y;
			if (Mathf.Abs(num - this.m_PreviousContainerWidth) > Mathf.Epsilon || Mathf.Abs(num2 - this.m_PreviousContainerHeight) > Mathf.Epsilon)
			{
				this.m_PreviousContainerWidth = num;
				this.m_PreviousContainerHeight = num2;
				this.Resize();
			}
			this.CalculateRects(contentViewContainer);
			base.DoRepaint();
			Color color = Handles.color;
			graphView.graphElements.ForEach(delegate(GraphElement elem)
			{
				if (!(elem is Edge))
				{
					Rect rect = this.CalculateElementRect(elem);
					Handles.color = elem.elementTypeColor;
					MiniMap.s_CachedRect[0].Set(rect.xMin, rect.yMin, 0f);
					MiniMap.s_CachedRect[1].Set(rect.xMax, rect.yMin, 0f);
					MiniMap.s_CachedRect[2].Set(rect.xMax, rect.yMax, 0f);
					MiniMap.s_CachedRect[3].Set(rect.xMin, rect.yMax, 0f);
					Handles.DrawSolidRectangleWithOutline(MiniMap.s_CachedRect, elem.elementTypeColor, elem.elementTypeColor);
					if (elem.dependsOnPresenter)
					{
						GraphElementPresenter presenter = elem.GetPresenter<GraphElementPresenter>();
						if (presenter != null && presenter.selected)
						{
							this.DrawRectangleOutline(rect, this.m_SelectedChildrenColor);
						}
					}
					else if (elem.selected)
					{
						this.DrawRectangleOutline(rect, this.m_SelectedChildrenColor);
					}
				}
			});
			this.DrawRectangleOutline(this.m_ViewportRect, this.m_ViewportColor);
			Handles.color = color;
		}

		private void DrawRectangleOutline(Rect rect, Color color)
		{
			Color color2 = Handles.color;
			Handles.color = color;
			Handles.DrawPolyLine(new Vector3[]
			{
				new Vector3(rect.x, rect.y, 0f),
				new Vector3(rect.x + rect.width, rect.y, 0f),
				new Vector3(rect.x + rect.width, rect.y + rect.height, 0f),
				new Vector3(rect.x, rect.y + rect.height, 0f),
				new Vector3(rect.x, rect.y, 0f)
			});
			Handles.color = color2;
		}

		private void OnMouseDown(MouseDownEvent e)
		{
			GraphView gView = this.graphView;
			this.CalculateRects(gView.contentViewContainer);
			Vector2 mousePosition = e.localMousePosition;
			gView.graphElements.ForEach(delegate(GraphElement child)
			{
				if (child != null)
				{
					ISelectable firstOfType = child.GetFirstOfType<ISelectable>();
					if (firstOfType != null && firstOfType.IsSelectable())
					{
						if (this.CalculateElementRect(child).Contains(mousePosition))
						{
							gView.ClearSelection();
							gView.AddToSelection(firstOfType);
							gView.FrameSelection();
							e.StopPropagation();
						}
					}
				}
			});
		}
	}
}
