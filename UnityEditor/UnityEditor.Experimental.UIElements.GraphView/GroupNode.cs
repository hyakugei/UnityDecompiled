using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace UnityEditor.Experimental.UIElements.GraphView
{
	public class GroupNode : GraphElement
	{
		private const int k_TitleItemMinWidth = 10;

		private VisualElement m_MainContainer;

		private VisualElement m_HeaderItem;

		private Label m_TitleItem;

		private TextField m_TitleEditor;

		private VisualElement m_ContentItem;

		private static readonly List<GraphElement> s_EmptyList = new List<GraphElement>();

		private List<GraphElement> m_ContainedElements;

		private Rect m_ContainedElementsRect;

		private bool m_IsUpdatingGeometryFromContent = false;

		private bool m_IsMovingElements = false;

		private bool m_Initialized = false;

		private bool m_HeaderSizeIsValid = false;

		private bool m_EditTitleCancelled = false;

		private Vector2 mPreviousPosInCanvasSpace = default(Vector2);

		public string title
		{
			get
			{
				return this.m_TitleItem.text;
			}
			set
			{
				if (!this.m_TitleItem.Equals(value))
				{
					this.m_TitleItem.text = value;
					GraphView firstAncestorOfType = base.GetFirstAncestorOfType<GraphView>();
					if (firstAncestorOfType != null && firstAncestorOfType.groupNodeTitleChanged != null)
					{
						firstAncestorOfType.groupNodeTitleChanged(this, value);
					}
					this.UpdateGeometryFromContent();
				}
			}
		}

		public List<GraphElement> containedElements
		{
			get
			{
				return (this.m_ContainedElements == null) ? GroupNode.s_EmptyList : this.m_ContainedElements;
			}
		}

		public Rect containedElementsRect
		{
			get
			{
				return this.m_ContainedElementsRect;
			}
		}

		public GroupNode()
		{
			this.m_ContentItem = new GroupNodeDropArea();
			this.m_ContentItem.ClearClassList();
			this.m_ContentItem.AddToClassList("content");
			VisualTreeAsset visualTreeAsset = EditorGUIUtility.Load("UXML/GraphView/GroupNode.uxml") as VisualTreeAsset;
			this.m_MainContainer = visualTreeAsset.CloneTree(null);
			this.m_MainContainer.AddToClassList("mainContainer");
			this.m_HeaderItem = this.m_MainContainer.Q("header", null);
			this.m_HeaderItem.AddToClassList("header");
			this.m_TitleItem = this.m_MainContainer.Q("titleLabel", null);
			this.m_TitleItem.AddToClassList("label");
			this.m_TitleEditor = (this.m_MainContainer.Q("titleField", null) as TextField);
			this.m_TitleEditor.AddToClassList("textfield");
			this.m_TitleEditor.visible = false;
			this.m_TitleEditor.RegisterCallback<FocusOutEvent>(delegate(FocusOutEvent e)
			{
				this.OnEditTitleFinished();
			}, Capture.NoCapture);
			this.m_TitleEditor.RegisterCallback<KeyDownEvent>(new EventCallback<KeyDownEvent>(this.OnKeyPressed), Capture.NoCapture);
			VisualElement visualElement = this.m_MainContainer.Q("contentPlaceholder", null);
			visualElement.Add(this.m_ContentItem);
			base.Add(this.m_MainContainer);
			base.ClearClassList();
			base.AddToClassList("groupNode");
			base.clippingOptions = VisualElement.ClippingOptions.ClipAndCacheContents;
			base.capabilities |= (Capabilities.Selectable | Capabilities.Movable | Capabilities.Deletable);
			this.m_HeaderItem.RegisterCallback<PostLayoutEvent>(new EventCallback<PostLayoutEvent>(this.OnHeaderSizeChanged), Capture.NoCapture);
			base.RegisterCallback<PostLayoutEvent>(delegate(PostLayoutEvent e)
			{
				this.MoveElements();
			}, Capture.NoCapture);
			base.RegisterCallback<MouseDownEvent>(new EventCallback<MouseDownEvent>(this.OnMouseUpEvent), Capture.NoCapture);
		}

		private void OnSubElementPostLayout(PostLayoutEvent e)
		{
			if (!this.IsSelected(base.GetFirstAncestorOfType<GraphView>()))
			{
				this.UpdateGeometryFromContent();
			}
		}

		private void OnSubElementDetachedFromPanel(DetachFromPanelEvent evt)
		{
			if (base.panel != null)
			{
				GraphElement element = evt.target as GraphElement;
				this.RemoveElement(element);
			}
		}

		private static bool IsValidSize(Vector2 size)
		{
			return size.x > 0f && !float.IsNaN(size.x) && size.y > 0f && !float.IsNaN(size.y);
		}

		private static bool IsValidRect(Rect rect)
		{
			return !float.IsNaN(rect.x) && !float.IsNaN(rect.y) && rect.width > 0f && !float.IsNaN(rect.width) && rect.height > 0f && !float.IsNaN(rect.height);
		}

		private void OnHeaderSizeChanged(PostLayoutEvent e)
		{
			if (!this.m_HeaderSizeIsValid && GroupNode.IsValidRect(this.m_HeaderItem.layout))
			{
				this.UpdateGeometryFromContent();
				this.m_HeaderSizeIsValid = true;
				this.m_HeaderItem.UnregisterCallback<PostLayoutEvent>(new EventCallback<PostLayoutEvent>(this.OnHeaderSizeChanged), Capture.NoCapture);
			}
		}

		private void OnKeyPressed(KeyDownEvent e)
		{
			KeyCode keyCode = e.keyCode;
			if (keyCode != KeyCode.Escape)
			{
				if (keyCode == KeyCode.Return)
				{
					this.m_TitleEditor.Blur();
				}
			}
			else
			{
				this.m_EditTitleCancelled = true;
				this.m_TitleEditor.Blur();
			}
		}

		private void OnEditTitleFinished()
		{
			this.m_TitleItem.visible = true;
			this.m_TitleEditor.visible = false;
			if (!this.m_EditTitleCancelled)
			{
				if (this.title != this.m_TitleEditor.text)
				{
					this.title = this.m_TitleEditor.text;
					this.UpdateGeometryFromContent();
				}
			}
			this.m_EditTitleCancelled = false;
		}

		private void OnMouseUpEvent(MouseDownEvent e)
		{
			if (e.clickCount == 2)
			{
				if (this.HitTest(e.localMousePosition))
				{
					this.m_TitleEditor.text = this.title;
					this.m_TitleEditor.visible = true;
					this.m_TitleItem.visible = false;
					base.schedule.Execute(new Action(this.GiveFocusToTitleEditor)).StartingIn(300L);
				}
			}
		}

		private void GiveFocusToTitleEditor()
		{
			this.m_TitleEditor.SelectAll();
			this.m_TitleEditor.Focus();
		}

		public override bool HitTest(Vector2 localPoint)
		{
			Vector2 localPoint2 = this.ChangeCoordinatesTo(this.m_HeaderItem, localPoint);
			return this.m_HeaderItem.ContainsPoint(localPoint2);
		}

		public override bool Overlaps(Rect rectangle)
		{
			Rect rectangle2 = this.ChangeCoordinatesTo(this.m_HeaderItem, rectangle);
			return this.m_HeaderItem.Overlaps(rectangle2);
		}

		public bool ContainsElement(GraphElement element)
		{
			return this.m_ContainedElements != null && this.m_ContainedElements.Contains(element);
		}

		public void AddElement(GraphElement element)
		{
			if (element == null)
			{
				throw new ArgumentException("Cannot add null element");
			}
			if (element is GroupNode)
			{
				throw new ArgumentException("Nested group node is not supported yet.");
			}
			if (this.m_ContainedElements == null)
			{
				this.m_ContainedElements = new List<GraphElement>();
			}
			else if (this.m_ContainedElements.Contains(element))
			{
				throw new ArgumentException("The element is already contained in this group node.");
			}
			GroupNode containingGroupNode = element.GetContainingGroupNode();
			if (containingGroupNode != null)
			{
				containingGroupNode.RemoveElement(element);
			}
			this.m_ContainedElements.Add(element);
			element.RegisterCallback<PostLayoutEvent>(new EventCallback<PostLayoutEvent>(this.OnSubElementPostLayout), Capture.NoCapture);
			element.RegisterCallback<DetachFromPanelEvent>(new EventCallback<DetachFromPanelEvent>(this.OnSubElementDetachedFromPanel), Capture.NoCapture);
			this.UpdateGeometryFromContent();
			GraphView firstAncestorOfType = base.GetFirstAncestorOfType<GraphView>();
			if (firstAncestorOfType != null && firstAncestorOfType.elementAddedToGroupNode != null)
			{
				firstAncestorOfType.elementAddedToGroupNode(this, element);
			}
		}

		public void RemoveElement(GraphElement element)
		{
			if (element == null)
			{
				throw new ArgumentException("Cannot remove null element from this group");
			}
			if (this.m_ContainedElements != null)
			{
				if (!this.m_ContainedElements.Contains(element))
				{
					throw new ArgumentException("This element is not contained in this group");
				}
				this.m_ContainedElements.Remove(element);
				element.UnregisterCallback<PostLayoutEvent>(new EventCallback<PostLayoutEvent>(this.OnSubElementPostLayout), Capture.NoCapture);
				element.UnregisterCallback<DetachFromPanelEvent>(new EventCallback<DetachFromPanelEvent>(this.OnSubElementDetachedFromPanel), Capture.NoCapture);
				this.UpdateGeometryFromContent();
				GraphView firstAncestorOfType = base.GetFirstAncestorOfType<GraphView>();
				if (firstAncestorOfType != null && firstAncestorOfType.elementRemovedFromGroupNode != null)
				{
					firstAncestorOfType.elementRemovedFromGroupNode(this, element);
				}
			}
		}

		private void MoveElements()
		{
			if (base.panel != null && this.m_Initialized)
			{
				GraphView firstAncestorOfType = base.GetFirstAncestorOfType<GraphView>();
				VisualElement contentViewContainer = firstAncestorOfType.contentViewContainer;
				Vector2 rhs = this.ChangeCoordinatesTo(contentViewContainer, new Vector2(0f, 0f));
				if (!(this.mPreviousPosInCanvasSpace == rhs))
				{
					float deltaX = rhs.x - this.mPreviousPosInCanvasSpace.x;
					float deltaY = rhs.y - this.mPreviousPosInCanvasSpace.y;
					this.mPreviousPosInCanvasSpace = rhs;
					this.MoveElements(deltaX, deltaY);
				}
			}
		}

		private void MoveElements(float deltaX, float deltaY)
		{
			if (this.m_ContainedElements != null)
			{
				this.m_IsMovingElements = true;
				for (int i = 0; i < this.m_ContainedElements.Count; i++)
				{
					GraphElement graphElement = this.m_ContainedElements[i];
					if (!this.m_IsUpdatingGeometryFromContent)
					{
						Rect position = graphElement.GetPosition();
						graphElement.SetPosition(new Rect(position.x + deltaX, position.y + deltaY, position.width, position.height));
					}
				}
				this.m_IsMovingElements = false;
			}
		}

		public void OnPositionChanged(VisualElement ve)
		{
			if (ve == this)
			{
				this.MoveElements();
			}
			else
			{
				this.UpdateGeometryFromContent();
			}
		}

		public override void DoRepaint()
		{
			if (!this.m_Initialized)
			{
				this.m_Initialized = true;
				this.UpdateGeometryFromContent();
			}
			base.DoRepaint();
		}

		public void UpdateGeometryFromContent()
		{
			if (base.panel != null && this.m_Initialized && !this.m_IsUpdatingGeometryFromContent && !this.m_IsMovingElements)
			{
				GraphView firstAncestorOfType = base.GetFirstAncestorOfType<GraphView>();
				if (firstAncestorOfType != null)
				{
					this.m_IsUpdatingGeometryFromContent = true;
					VisualElement contentViewContainer = firstAncestorOfType.contentViewContainer;
					Rect rect = Rect.zero;
					if (this.m_ContainedElements != null)
					{
						for (int i = 0; i < this.m_ContainedElements.Count; i++)
						{
							GraphElement graphElement = this.m_ContainedElements[i];
							if (graphElement.panel == base.panel)
							{
								Rect rect2 = new Rect(0f, 0f, graphElement.GetPosition().width, graphElement.GetPosition().height);
								if (GroupNode.IsValidRect(rect2))
								{
									rect2 = graphElement.ChangeCoordinatesTo(contentViewContainer, rect2);
									if (!GroupNode.IsValidRect(rect))
									{
										rect = rect2;
									}
									else
									{
										rect = RectUtils.Encompass(rect, rect2);
									}
								}
							}
						}
					}
					if (this.m_ContainedElements == null || this.m_ContainedElements.Count == 0)
					{
						float x = this.m_ContentItem.style.borderLeftWidth.value + this.m_ContentItem.style.paddingLeft.value;
						float y = this.m_HeaderItem.layout.height + this.m_ContentItem.style.borderTopWidth.value + this.m_ContentItem.style.paddingTop.value;
						rect = this.ChangeCoordinatesTo(contentViewContainer, new Rect(x, y, 0f, 0f));
					}
					float num = 10f;
					if (this.m_HeaderItem != null)
					{
						Vector2 size = this.m_TitleItem.DoMeasure(100f, VisualElement.MeasureMode.Undefined, 100f, VisualElement.MeasureMode.Undefined);
						if (GroupNode.IsValidSize(size))
						{
							num = size.x + this.m_TitleItem.style.marginLeft.value + this.m_TitleItem.style.paddingLeft.value + this.m_TitleItem.style.paddingRight.value + this.m_TitleItem.style.marginRight.value;
						}
					}
					float val = num + this.m_HeaderItem.style.paddingLeft.value + this.m_HeaderItem.style.paddingRight.value;
					Vector2 size2 = rect.size;
					size2.x += this.m_ContentItem.style.borderLeftWidth.value + this.m_ContentItem.style.paddingLeft.value + this.m_ContentItem.style.paddingRight.value + this.m_ContentItem.style.borderRightWidth.value;
					size2.y += this.m_ContentItem.style.borderTopWidth.value + this.m_ContentItem.style.paddingTop.value + this.m_ContentItem.style.paddingBottom.value + this.m_ContentItem.style.borderBottomWidth.value;
					Rect position = default(Rect);
					position.position = contentViewContainer.ChangeCoordinatesTo(base.parent, rect.position);
					position.width = Math.Max(size2.x, val) + base.style.borderLeftWidth.value + base.style.borderRightWidth.value;
					position.height = size2.y + this.m_HeaderItem.layout.height + base.style.borderTopWidth.value + base.style.borderBottomWidth.value;
					position.x -= this.m_ContentItem.style.paddingLeft.value + base.style.borderLeftWidth.value;
					position.y -= this.m_ContentItem.style.paddingTop.value + this.m_HeaderItem.layout.height + base.style.borderTopWidth.value;
					this.SetPosition(position);
					Vector2 vector = this.ChangeCoordinatesTo(contentViewContainer, new Vector2(0f, 0f));
					this.mPreviousPosInCanvasSpace = vector;
					this.m_ContainedElementsRect = contentViewContainer.ChangeCoordinatesTo(this, rect);
					this.m_IsUpdatingGeometryFromContent = false;
				}
			}
		}
	}
}
