using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.Experimental.UIElements.StyleSheets;

namespace UnityEditor.Experimental.UIElements.GraphView
{
	public class Port : GraphElement
	{
		private class DefaultEdgeConnectorListener : IEdgeConnectorListener
		{
			private GraphViewChange m_GraphViewChange;

			private List<Edge> m_EdgesToCreate;

			public DefaultEdgeConnectorListener()
			{
				this.m_EdgesToCreate = new List<Edge>();
				this.m_GraphViewChange.edgesToCreate = this.m_EdgesToCreate;
			}

			public void OnDropOutsidePort(Edge edge, Vector2 position)
			{
			}

			public void OnDrop(GraphView graphView, Edge edge)
			{
				this.m_EdgesToCreate.Clear();
				this.m_EdgesToCreate.Add(edge);
				List<Edge> edgesToCreate = this.m_EdgesToCreate;
				if (graphView.graphViewChanged != null)
				{
					edgesToCreate = graphView.graphViewChanged(this.m_GraphViewChange).edgesToCreate;
				}
				foreach (Edge current in edgesToCreate)
				{
					graphView.AddElement(current);
					edge.input.Connect(current);
					edge.output.Connect(current);
				}
			}
		}

		protected class DefaultEdgePresenterConnectorListener<TEdgePresenter> : IEdgeConnectorListener where TEdgePresenter : EdgePresenter
		{
			public void OnDropOutsidePort(Edge edge, Vector2 position)
			{
			}

			public void OnDrop(GraphView graphView, Edge edge)
			{
				if (graphView != null && edge != null)
				{
					if (!(graphView.presenter == null))
					{
						EdgePresenter edgePresenter = edge.GetPresenter<EdgePresenter>();
						if (edgePresenter == null)
						{
							edgePresenter = ScriptableObject.CreateInstance<TEdgePresenter>();
						}
						edgePresenter.output = edge.output.GetPresenter<PortPresenter>();
						edgePresenter.input = edge.input.GetPresenter<PortPresenter>();
						edgePresenter.output.Connect(edgePresenter);
						edgePresenter.input.Connect(edgePresenter);
						graphView.presenter.AddElement(edgePresenter);
					}
				}
			}
		}

		private const string k_PortColorProperty = "port-color";

		private const string k_DisabledPortColorProperty = "disabled-port-color";

		protected EdgeConnector m_EdgeConnector;

		protected VisualElement m_ConnectorBox;

		protected Label m_ConnectorText;

		protected VisualElement m_ConnectorBoxCap;

		private bool m_portCapLit;

		private string m_VisualClass;

		private Type m_PortType;

		private bool m_Highlight = true;

		private HashSet<Edge> m_Connections;

		private Direction m_Direction;

		private StyleValue<Color> m_PortColor;

		private StyleValue<Color> m_DisabledPortColor;

		internal Action<Port> OnConnect;

		internal Action<Port> OnDisconnect;

		internal Color capColor
		{
			get
			{
				Color result;
				if (this.m_ConnectorBoxCap == null)
				{
					result = Color.black;
				}
				else
				{
					result = this.m_ConnectorBoxCap.style.backgroundColor;
				}
				return result;
			}
			set
			{
				if (this.m_ConnectorBoxCap != null)
				{
					this.m_ConnectorBoxCap.style.backgroundColor = value;
				}
			}
		}

		public string portName
		{
			get
			{
				return this.m_ConnectorText.text;
			}
			set
			{
				this.m_ConnectorText.text = value;
			}
		}

		public bool portCapLit
		{
			get
			{
				return this.m_portCapLit;
			}
			set
			{
				if (value != this.m_portCapLit)
				{
					this.m_portCapLit = value;
					this.UpdateCapColor();
				}
			}
		}

		public Direction direction
		{
			get
			{
				return this.m_Direction;
			}
			private set
			{
				if (this.m_Direction != value)
				{
					base.RemoveFromClassList(this.m_Direction.ToString().ToLower());
					this.m_Direction = value;
					base.AddToClassList(this.m_Direction.ToString().ToLower());
				}
			}
		}

		public Orientation orientation
		{
			get;
			private set;
		}

		public string visualClass
		{
			get
			{
				return this.m_VisualClass;
			}
			set
			{
				if (!(value == this.m_VisualClass))
				{
					if (!string.IsNullOrEmpty(this.m_VisualClass))
					{
						base.RemoveFromClassList(this.m_VisualClass);
					}
					else
					{
						this.ManageTypeClassList(this.m_PortType, new Action<string>(base.RemoveFromClassList));
					}
					this.m_VisualClass = value;
					if (!string.IsNullOrEmpty(this.m_VisualClass))
					{
						base.AddToClassList(this.m_VisualClass);
					}
					else
					{
						this.ManageTypeClassList(this.m_PortType, new Action<string>(base.AddToClassList));
					}
				}
			}
		}

		public Type portType
		{
			get
			{
				return this.m_PortType;
			}
			set
			{
				if (this.m_PortType != value)
				{
					this.ManageTypeClassList(this.m_PortType, new Action<string>(base.RemoveFromClassList));
					this.m_PortType = value;
					Type typeFromHandle = typeof(PortSource<>);
					Type type = typeFromHandle.MakeGenericType(new Type[]
					{
						this.m_PortType
					});
					this.source = Activator.CreateInstance(type);
					if (string.IsNullOrEmpty(this.m_ConnectorText.text))
					{
						this.m_ConnectorText.text = this.m_PortType.Name;
					}
					this.ManageTypeClassList(this.m_PortType, new Action<string>(base.AddToClassList));
				}
			}
		}

		public EdgeConnector edgeConnector
		{
			get
			{
				return this.m_EdgeConnector;
			}
		}

		public object source
		{
			get;
			set;
		}

		public bool highlight
		{
			get
			{
				PortPresenter presenter = base.GetPresenter<PortPresenter>();
				bool highlight;
				if (presenter == null)
				{
					highlight = this.m_Highlight;
				}
				else
				{
					highlight = presenter.highlight;
				}
				return highlight;
			}
			set
			{
				PortPresenter presenter = base.GetPresenter<PortPresenter>();
				if (presenter != null)
				{
					presenter.highlight = value;
				}
				if (this.m_Highlight != value)
				{
					this.m_Highlight = value;
					this.UpdateConnectorColor();
				}
			}
		}

		public virtual IEnumerable<Edge> connections
		{
			get
			{
				return this.m_Connections;
			}
		}

		public virtual bool connected
		{
			get
			{
				PortPresenter presenter = base.GetPresenter<PortPresenter>();
				bool result;
				if (presenter != null)
				{
					result = presenter.connected;
				}
				else
				{
					result = (this.m_Connections.Count > 0);
				}
				return result;
			}
		}

		public virtual bool collapsed
		{
			get
			{
				PortPresenter presenter = base.GetPresenter<PortPresenter>();
				return presenter != null && presenter.collapsed;
			}
		}

		public Color portColor
		{
			get
			{
				return this.m_PortColor.GetSpecifiedValueOrDefault(new Color(0.9411765f, 0.9411765f, 0.9411765f));
			}
		}

		public Color disabledPortColor
		{
			get
			{
				return this.m_PortColor.GetSpecifiedValueOrDefault(new Color(0.274509817f, 0.274509817f, 0.274509817f));
			}
		}

		public Node node
		{
			get
			{
				return base.GetFirstAncestorOfType<Node>();
			}
		}

		protected Port(Orientation portOrientation, Direction portDirection, Type type)
		{
			base.ClearClassList();
			VisualTreeAsset visualTreeAsset = EditorGUIUtility.Load("UXML/GraphView/Port.uxml") as VisualTreeAsset;
			visualTreeAsset.CloneTree(this, null);
			this.m_ConnectorBox = this.Q("connector", null);
			this.m_ConnectorText = this.Q("type", null);
			this.m_ConnectorText.clippingOptions = VisualElement.ClippingOptions.NoClipping;
			this.m_ConnectorBoxCap = this.Q("cap", null);
			this.m_Connections = new HashSet<Edge>();
			this.orientation = portOrientation;
			this.direction = portDirection;
			this.portType = type;
			base.AddToClassList(portDirection.ToString().ToLower());
		}

		private void ManageTypeClassList(Type type, Action<string> classListAction)
		{
			if (type != null && string.IsNullOrEmpty(this.m_VisualClass))
			{
				if (type.IsSubclassOf(typeof(Component)))
				{
					classListAction("typeComponent");
				}
				else if (type.IsSubclassOf(typeof(GameObject)))
				{
					classListAction("typeGameObject");
				}
				else
				{
					classListAction("type" + type.Name);
				}
			}
		}

		public virtual void Connect(Edge edge)
		{
			if (edge == null)
			{
				throw new ArgumentException("The value passed to Port.Connect is null");
			}
			PortPresenter presenter = base.GetPresenter<PortPresenter>();
			if (presenter != null)
			{
				EdgePresenter presenter2 = edge.GetPresenter<EdgePresenter>();
				presenter.Connect(presenter2);
			}
			else if (!this.m_Connections.Contains(edge))
			{
				this.m_Connections.Add(edge);
			}
			if (this.OnConnect != null)
			{
				this.OnConnect(this);
			}
		}

		public virtual void Disconnect(Edge edge)
		{
			if (edge == null)
			{
				throw new ArgumentException("The value passed to PortPresenter.Disconnect is null");
			}
			PortPresenter presenter = base.GetPresenter<PortPresenter>();
			if (presenter != null)
			{
				EdgePresenter presenter2 = edge.GetPresenter<EdgePresenter>();
				presenter.Disconnect(presenter2);
			}
			else
			{
				this.m_Connections.Remove(edge);
			}
			if (this.OnDisconnect != null)
			{
				this.OnDisconnect(this);
			}
		}

		public virtual void DisconnectAll()
		{
			PortPresenter presenter = base.GetPresenter<PortPresenter>();
			if (presenter != null)
			{
				foreach (Edge current in this.m_Connections)
				{
					EdgePresenter presenter2 = current.GetPresenter<EdgePresenter>();
					presenter.Disconnect(presenter2);
				}
			}
			this.m_Connections.Clear();
			if (this.OnDisconnect != null)
			{
				this.OnDisconnect(this);
			}
		}

		public static Port Create<TEdge>(Orientation orientation, Direction direction, Type type) where TEdge : Edge, new()
		{
			Port.DefaultEdgeConnectorListener listener = new Port.DefaultEdgeConnectorListener();
			Port port = new Port(orientation, direction, type)
			{
				m_EdgeConnector = new EdgeConnector<TEdge>(listener)
			};
			port.AddManipulator(port.m_EdgeConnector);
			return port;
		}

		public static Port Create<TEdgePresenter, TEdge>(PortPresenter presenter) where TEdgePresenter : EdgePresenter where TEdge : Edge, new()
		{
			Port.DefaultEdgePresenterConnectorListener<TEdgePresenter> listener = new Port.DefaultEdgePresenterConnectorListener<TEdgePresenter>();
			Port port = new Port(Orientation.Horizontal, Direction.Input, typeof(object))
			{
				m_EdgeConnector = new EdgeConnector<TEdge>(listener),
				presenter = presenter
			};
			port.AddManipulator(port.m_EdgeConnector);
			return port;
		}

		protected virtual VisualElement CreateConnector()
		{
			return new VisualElement();
		}

		private void UpdateConnector()
		{
			if (this.m_EdgeConnector != null)
			{
				PortPresenter presenter = base.GetPresenter<PortPresenter>();
				if (this.m_EdgeConnector.target == null || !this.m_EdgeConnector.target.HasMouseCapture())
				{
					if (!presenter.connected || presenter.direction != Direction.Input)
					{
						this.AddManipulator(this.m_EdgeConnector);
					}
					else
					{
						this.RemoveManipulator(this.m_EdgeConnector);
					}
				}
			}
		}

		public bool IsConnectable()
		{
			PortPresenter portPresenter = base.presenter as PortPresenter;
			return !(portPresenter != null) || portPresenter.IsConnectable();
		}

		public override void OnDataChanged()
		{
			this.UpdateConnector();
			PortPresenter presenter = base.GetPresenter<PortPresenter>();
			Type portType = presenter.portType;
			Type typeFromHandle = typeof(PortSource<>);
			try
			{
				Type type = typeFromHandle.MakeGenericType(new Type[]
				{
					portType
				});
				presenter.source = Activator.CreateInstance(type);
			}
			catch (Exception ex)
			{
				Debug.Log("Couldn't build PortSouce<" + ((portType != null) ? portType.Name : "null") + "> " + ex.Message);
			}
			string text = (!string.IsNullOrEmpty(presenter.name)) ? presenter.name : portType.Name;
			this.m_ConnectorText.text = text;
			presenter.capabilities &= ~Capabilities.Selectable;
			this.direction = presenter.direction;
			this.orientation = presenter.orientation;
			this.portType = presenter.portType;
			this.source = presenter.source;
		}

		public override Vector3 GetGlobalCenter()
		{
			return this.m_ConnectorBox.LocalToWorld(this.m_ConnectorBox.rect.center);
		}

		public override bool ContainsPoint(Vector2 localPoint)
		{
			Rect layout = this.m_ConnectorBox.layout;
			Rect rect;
			if (this.direction == Direction.Input)
			{
				rect = new Rect(-layout.xMin, -layout.yMin, layout.width + layout.xMin, base.rect.height);
				rect.width += this.m_ConnectorText.layout.xMin - layout.xMax;
			}
			else
			{
				rect = new Rect(0f, -layout.yMin, base.rect.width - layout.xMin, base.rect.height);
				float num = layout.xMin - this.m_ConnectorText.layout.xMax;
				rect.xMin -= num;
				rect.width += num;
			}
			return rect.Contains(this.ChangeCoordinatesTo(this.m_ConnectorBox, localPoint));
		}

		internal void UpdateCapColor()
		{
			if (this.portCapLit || this.connected)
			{
				this.m_ConnectorBoxCap.style.backgroundColor = this.portColor;
			}
			else
			{
				this.m_ConnectorBoxCap.style.backgroundColor = StyleValue<Color>.nil;
			}
		}

		private void UpdateConnectorColor()
		{
			if (this.m_ConnectorBox != null)
			{
				this.m_ConnectorBox.style.borderColor = ((!this.highlight) ? this.m_DisabledPortColor.value : this.m_PortColor.value);
			}
		}

		protected internal override void ExecuteDefaultAction(EventBase evt)
		{
			base.ExecuteDefaultAction(evt);
			if (this.m_ConnectorBox != null && this.m_ConnectorBoxCap != null)
			{
				if (evt.GetEventTypeId() == EventBase<MouseEnterEvent>.TypeId())
				{
					this.m_ConnectorBoxCap.style.backgroundColor = this.portColor;
				}
				else if (evt.GetEventTypeId() == EventBase<MouseLeaveEvent>.TypeId())
				{
					this.UpdateCapColor();
				}
				else if (evt.GetEventTypeId() == EventBase<MouseUpEvent>.TypeId())
				{
					MouseUpEvent mouseUpEvent = (MouseUpEvent)evt;
					if (!base.layout.Contains(mouseUpEvent.localMousePosition))
					{
						this.UpdateCapColor();
					}
				}
			}
		}

		protected override void OnStyleResolved(ICustomStyle styles)
		{
			base.OnStyleResolved(styles);
			styles.ApplyCustomProperty("port-color", ref this.m_PortColor);
			styles.ApplyCustomProperty("disabled-port-color", ref this.m_DisabledPortColor);
			this.UpdateConnectorColor();
		}
	}
}
