using System;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.Experimental.UIElements.StyleSheets;

namespace UnityEditor.Experimental.UIElements.GraphView
{
	public abstract class GraphElement : DataWatchContainer, ISelectable
	{
		private GraphElementPresenter m_Presenter;

		private StyleValue<int> m_Layer;

		private const string k_LayerProperty = "layer";

		private Capabilities m_Capabilities;

		private bool m_Selected;

		private ClickSelector m_ClickSelector;

		public bool dependsOnPresenter
		{
			get;
			private set;
		}

		public Color elementTypeColor
		{
			get;
			set;
		}

		public int layer
		{
			get
			{
				return this.m_Layer.value;
			}
			set
			{
				if (this.m_Layer.value != value)
				{
					this.m_Layer = value;
				}
			}
		}

		public Capabilities capabilities
		{
			get
			{
				return this.m_Capabilities;
			}
			set
			{
				if (this.m_Capabilities != value)
				{
					this.m_Capabilities = value;
					if (this.IsSelectable() && this.m_ClickSelector == null)
					{
						this.m_ClickSelector = new ClickSelector();
						this.AddManipulator(this.m_ClickSelector);
					}
					else if (!this.IsSelectable() && this.m_ClickSelector != null)
					{
						this.RemoveManipulator(this.m_ClickSelector);
						this.m_ClickSelector = null;
					}
				}
			}
		}

		public bool selected
		{
			get
			{
				return this.m_Selected;
			}
			set
			{
				if ((this.capabilities & Capabilities.Selectable) == Capabilities.Selectable)
				{
					if (this.m_Selected != value)
					{
						this.m_Selected = value;
						if (this.m_Selected)
						{
							base.AddToClassList("selected");
						}
						else
						{
							base.RemoveFromClassList("selected");
						}
					}
				}
			}
		}

		public GraphElementPresenter presenter
		{
			get
			{
				return this.m_Presenter;
			}
			set
			{
				if (!(this.m_Presenter == value))
				{
					base.RemoveWatch();
					this.m_Presenter = value;
					this.dependsOnPresenter = (this.m_Presenter != null);
					this.OnDataChanged();
					base.AddWatch();
				}
			}
		}

		protected override UnityEngine.Object[] toWatch
		{
			get
			{
				return (!(this.presenter == null)) ? this.presenter.GetObjectsToWatch() : null;
			}
		}

		protected GraphElement()
		{
			this.dependsOnPresenter = false;
			base.ClearClassList();
			base.AddToClassList("graphElement");
			this.elementTypeColor = new Color(0.9f, 0.9f, 0.9f, 0.5f);
			base.persistenceKey = Guid.NewGuid().ToString();
		}

		public void ResetLayer()
		{
			int value = this.m_Layer.value;
			this.m_Layer = StyleValue<int>.nil;
			base.effectiveStyle.ApplyCustomProperty("layer", ref this.m_Layer);
			this.UpdateLayer(value);
		}

		protected override void OnStyleResolved(ICustomStyle style)
		{
			base.OnStyleResolved(style);
			int value = this.m_Layer.value;
			style.ApplyCustomProperty("layer", ref this.m_Layer);
			this.UpdateLayer(value);
		}

		private void UpdateLayer(int prevLayer)
		{
			if (prevLayer != this.m_Layer.value)
			{
				GraphView firstAncestorOfType = base.GetFirstAncestorOfType<GraphView>();
				if (firstAncestorOfType != null)
				{
					firstAncestorOfType.ChangeLayer(this);
				}
			}
		}

		public T GetPresenter<T>() where T : GraphElementPresenter
		{
			return this.presenter as T;
		}

		public override void OnDataChanged()
		{
			if (!(this.presenter == null))
			{
				foreach (VisualElement current in this)
				{
					GraphElement graphElement = current as GraphElement;
					if (graphElement != null)
					{
						GraphElementPresenter presenter = graphElement.presenter;
						if (presenter != null)
						{
							presenter.selected = this.presenter.selected;
						}
					}
				}
				this.capabilities = this.m_Presenter.capabilities;
				this.selected = this.presenter.selected;
				this.SetPosition(this.presenter.position);
			}
		}

		public virtual bool IsSelectable()
		{
			return (this.capabilities & Capabilities.Selectable) == Capabilities.Selectable;
		}

		public virtual bool IsMovable()
		{
			return (this.capabilities & Capabilities.Movable) == Capabilities.Movable;
		}

		public virtual bool IsResizable()
		{
			return (this.capabilities & Capabilities.Resizable) == Capabilities.Resizable;
		}

		public virtual bool IsDroppable()
		{
			return (this.capabilities & Capabilities.Droppable) == Capabilities.Droppable;
		}

		public virtual bool IsAscendable()
		{
			return (this.capabilities & Capabilities.Ascendable) == Capabilities.Ascendable;
		}

		public virtual Vector3 GetGlobalCenter()
		{
			Vector2 center = base.layout.center;
			Vector3 point = new Vector3(center.x + base.parent.layout.x, center.y + base.parent.layout.y);
			return base.parent.worldTransform.MultiplyPoint3x4(point);
		}

		public virtual void UpdatePresenterPosition()
		{
			if (!(this.presenter == null))
			{
				base.RemoveWatch();
				this.presenter.position = this.GetPosition();
				this.presenter.CommitChanges();
				base.AddWatch();
			}
		}

		public virtual Rect GetPosition()
		{
			return base.layout;
		}

		public virtual void SetPosition(Rect newPos)
		{
			base.layout = newPos;
		}

		public virtual void OnSelected()
		{
			if (this.IsAscendable())
			{
				base.BringToFront();
			}
		}

		public virtual void OnUnselected()
		{
		}

		public virtual bool HitTest(Vector2 localPoint)
		{
			return this.ContainsPoint(localPoint);
		}

		public virtual void Select(VisualElement selectionContainer, bool additive)
		{
			GraphView graphView = selectionContainer as GraphView;
			if (graphView != null && (base.parent == graphView.contentViewContainer || (base.parent != null && base.parent.parent == graphView.contentViewContainer)))
			{
				if (!graphView.selection.Contains(this))
				{
					if (!additive)
					{
						graphView.ClearSelection();
					}
					graphView.AddToSelection(this);
				}
			}
		}

		public virtual void Unselect(VisualElement selectionContainer)
		{
			GraphView graphView = selectionContainer as GraphView;
			if (graphView != null && (base.parent == graphView.contentViewContainer || (base.parent != null && base.parent.parent == graphView.contentViewContainer)))
			{
				if (graphView.selection.Contains(this))
				{
					graphView.RemoveFromSelection(this);
				}
			}
		}

		public virtual bool IsSelected(VisualElement selectionContainer)
		{
			GraphView graphView = selectionContainer as GraphView;
			bool result;
			if (graphView != null && (base.parent == graphView.contentViewContainer || (base.parent != null && base.parent.parent == graphView.contentViewContainer)))
			{
				if (graphView.selection.Contains(this))
				{
					result = true;
					return result;
				}
			}
			result = false;
			return result;
		}
	}
}
