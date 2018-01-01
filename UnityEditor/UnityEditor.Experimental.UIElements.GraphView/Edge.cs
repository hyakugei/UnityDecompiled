using System;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.Experimental.UIElements.StyleEnums;
using UnityEngine.Experimental.UIElements.StyleSheets;

namespace UnityEditor.Experimental.UIElements.GraphView
{
	public class Edge : GraphElement
	{
		private const float k_EndPointRadius = 4f;

		private const float k_InterceptWidth = 6f;

		private const string k_EdgeWidthProperty = "edge-width";

		private const string k_SelectedEdgeColorProperty = "selected-edge-color";

		private const string k_GhostEdgeColorProperty = "ghost-edge-color";

		private const string k_EdgeColorProperty = "edge-color";

		private GraphView m_GraphView;

		private Port m_OutputPort;

		private Port m_InputPort;

		private Vector2 m_CandidatePosition;

		private EdgeControl m_EdgeControl;

		private StyleValue<int> m_EdgeWidth;

		private StyleValue<Color> m_SelectedColor;

		private StyleValue<Color> m_DefaultColor;

		private StyleValue<Color> m_GhostColor;

		public bool isGhostEdge
		{
			get;
			set;
		}

		public Port output
		{
			get
			{
				return this.m_OutputPort;
			}
			set
			{
				if (this.m_OutputPort != null && value != this.m_OutputPort)
				{
					this.m_OutputPort.UpdateCapColor();
				}
				this.m_OutputPort = value;
				this.OnPortChanged(false);
			}
		}

		public Port input
		{
			get
			{
				return this.m_InputPort;
			}
			set
			{
				if (this.m_InputPort != null && value != this.m_InputPort)
				{
					this.m_InputPort.UpdateCapColor();
				}
				this.m_InputPort = value;
				this.OnPortChanged(true);
			}
		}

		public EdgeControl edgeControl
		{
			get
			{
				if (this.m_EdgeControl == null)
				{
					this.m_EdgeControl = this.CreateEdgeControl();
				}
				return this.m_EdgeControl;
			}
		}

		public Vector2 candidatePosition
		{
			get
			{
				return this.m_CandidatePosition;
			}
			set
			{
				this.m_CandidatePosition = value;
				this.UpdateEdgeControl();
			}
		}

		public int edgeWidth
		{
			get
			{
				return this.m_EdgeWidth.GetSpecifiedValueOrDefault(2);
			}
		}

		public Color selectedColor
		{
			get
			{
				return this.m_SelectedColor.GetSpecifiedValueOrDefault(new Color(0.9411765f, 0.9411765f, 0.9411765f));
			}
		}

		public Color defaultColor
		{
			get
			{
				return this.m_DefaultColor.GetSpecifiedValueOrDefault(new Color(0.572549045f, 0.572549045f, 0.572549045f));
			}
		}

		public Color ghostColor
		{
			get
			{
				return this.m_GhostColor.GetSpecifiedValueOrDefault(new Color(0.333333343f, 0.333333343f, 0.333333343f));
			}
		}

		protected Vector2[] PointsAndTangents
		{
			get
			{
				return this.edgeControl.controlPoints;
			}
		}

		public Edge()
		{
			base.clippingOptions = VisualElement.ClippingOptions.NoClipping;
			base.ClearClassList();
			base.AddToClassList("edge");
			base.style.positionType = PositionType.Absolute;
			base.Add(this.edgeControl);
			this.AddManipulator(new EdgeManipulator());
			base.capabilities |= (Capabilities.Selectable | Capabilities.Deletable);
			this.AddManipulator(new ContextualMenuManipulator(null));
		}

		public override bool Overlaps(Rect rectangle)
		{
			return this.UpdateEdgeControl() && this.edgeControl.Overlaps(this.ChangeCoordinatesTo(this.edgeControl, rectangle));
		}

		public override bool ContainsPoint(Vector2 localPoint)
		{
			return this.UpdateEdgeControl() && this.edgeControl.ContainsPoint(this.ChangeCoordinatesTo(this.edgeControl, localPoint));
		}

		public virtual void OnPortChanged(bool isInput)
		{
		}

		public bool UpdateEdgeControl()
		{
			bool result;
			if (this.m_OutputPort == null && this.m_InputPort == null)
			{
				result = false;
			}
			else
			{
				if (this.m_GraphView == null)
				{
					this.m_GraphView = base.GetFirstOfType<GraphView>();
				}
				if (this.m_GraphView == null)
				{
					result = false;
				}
				else
				{
					Vector2 zero = Vector2.zero;
					Vector2 zero2 = Vector2.zero;
					this.GetFromToPoints(ref zero, ref zero2);
					this.edgeControl.from = zero;
					this.edgeControl.to = zero2;
					this.edgeControl.drawFromCap = (this.m_OutputPort == null);
					this.edgeControl.drawToCap = (this.m_InputPort == null);
					this.edgeControl.UpdateLayout();
					result = true;
				}
			}
			return result;
		}

		public override void DoRepaint()
		{
			this.DrawEdge();
		}

		protected void GetFromToPoints(ref Vector2 from, ref Vector2 to)
		{
			if (this.m_OutputPort != null || this.m_InputPort != null)
			{
				if (this.m_GraphView == null)
				{
					this.m_GraphView = base.GetFirstOfType<GraphView>();
				}
				if (this.m_OutputPort != null)
				{
					from = this.m_OutputPort.GetGlobalCenter();
					from = this.WorldToLocal(from);
				}
				else
				{
					from = this.WorldToLocal(new Vector2(this.m_CandidatePosition.x, this.m_CandidatePosition.y));
				}
				if (this.m_InputPort != null)
				{
					to = this.m_InputPort.GetGlobalCenter();
					to = this.WorldToLocal(to);
				}
				else
				{
					to = this.WorldToLocal(new Vector2(this.m_CandidatePosition.x, this.m_CandidatePosition.y));
				}
			}
		}

		protected override void OnStyleResolved(ICustomStyle styles)
		{
			base.OnStyleResolved(styles);
			styles.ApplyCustomProperty("edge-width", ref this.m_EdgeWidth);
			styles.ApplyCustomProperty("selected-edge-color", ref this.m_SelectedColor);
			styles.ApplyCustomProperty("ghost-edge-color", ref this.m_GhostColor);
			styles.ApplyCustomProperty("edge-color", ref this.m_DefaultColor);
		}

		public override void OnDataChanged()
		{
			base.OnDataChanged();
			EdgePresenter edgePresenter = base.GetPresenter<EdgePresenter>();
			if (edgePresenter != null)
			{
				if (this.output == null || this.output.presenter != edgePresenter.output)
				{
					GraphView firstAncestorOfType = base.GetFirstAncestorOfType<GraphView>();
					if (firstAncestorOfType != null)
					{
						this.output = from t in firstAncestorOfType.Query().OfType<Port>(null, null)
						where t.presenter == edgePresenter.output
						select t;
					}
				}
				if (this.input == null || this.input.presenter != edgePresenter.input)
				{
					GraphView firstAncestorOfType2 = base.GetFirstAncestorOfType<GraphView>();
					if (firstAncestorOfType2 != null)
					{
						this.input = from t in firstAncestorOfType2.Query().OfType<Port>(null, null)
						where t.presenter == edgePresenter.input
						select t;
					}
				}
				if (edgePresenter.output != null || edgePresenter.input != null)
				{
					this.edgeControl.orientation = ((!(edgePresenter.output != null)) ? edgePresenter.input.orientation : edgePresenter.output.orientation);
				}
			}
		}

		protected virtual void DrawEdge()
		{
			if (this.UpdateEdgeControl())
			{
				if (base.selected)
				{
					if (this.isGhostEdge)
					{
						Debug.Log("Selected Ghost Edge: this should never be");
					}
					this.edgeControl.inputColor = this.selectedColor;
					this.edgeControl.outputColor = this.selectedColor;
					this.edgeControl.edgeWidth = this.edgeWidth;
					if (this.m_InputPort != null)
					{
						this.m_InputPort.capColor = this.selectedColor;
					}
					if (this.m_OutputPort != null)
					{
						this.m_OutputPort.capColor = this.selectedColor;
					}
				}
				else
				{
					if (this.m_InputPort != null)
					{
						this.m_InputPort.UpdateCapColor();
					}
					if (this.m_OutputPort != null)
					{
						this.m_OutputPort.UpdateCapColor();
					}
					this.edgeControl.inputColor = ((this.m_InputPort != null) ? this.m_InputPort.portColor : this.m_OutputPort.portColor);
					this.edgeControl.outputColor = ((this.m_OutputPort != null) ? this.m_OutputPort.portColor : this.m_InputPort.portColor);
					this.edgeControl.edgeWidth = this.edgeWidth;
					EdgePresenter presenter = base.GetPresenter<EdgePresenter>();
					if (presenter == null)
					{
						this.edgeControl.toCapColor = ((this.m_InputPort != null) ? this.m_InputPort.portColor : this.m_OutputPort.portColor);
						this.edgeControl.fromCapColor = ((this.m_OutputPort != null) ? this.m_OutputPort.portColor : this.m_InputPort.portColor);
					}
					else
					{
						this.edgeControl.toCapColor = ((!(presenter.input == null)) ? this.m_InputPort.portColor : this.m_OutputPort.portColor);
						this.edgeControl.fromCapColor = ((!(presenter.output == null)) ? this.m_OutputPort.portColor : this.m_InputPort.portColor);
					}
					if (this.isGhostEdge)
					{
						this.edgeControl.inputColor = new Color(this.edgeControl.inputColor.r, this.edgeControl.inputColor.g, this.edgeControl.inputColor.b, 0.5f);
						this.edgeControl.outputColor = new Color(this.edgeControl.outputColor.r, this.edgeControl.outputColor.g, this.edgeControl.outputColor.b, 0.5f);
					}
				}
			}
		}

		protected virtual EdgeControl CreateEdgeControl()
		{
			return new EdgeControl
			{
				capRadius = 4f,
				interceptWidth = 6f
			};
		}
	}
}
