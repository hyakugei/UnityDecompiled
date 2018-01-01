using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.Experimental.UIElements.StyleEnums;

namespace UnityEditor.Experimental.UIElements.GraphView
{
	public class Node : GraphElement
	{
		private VisualElement m_CollapsibleArea;

		private bool m_Expanded;

		private readonly Label m_TitleLabel;

		protected readonly VisualElement m_CollapseButton;

		private const string k_ExpandedStyleClass = "expanded";

		private const string k_CollapsedStyleClass = "collapsed";

		public VisualElement mainContainer
		{
			get;
			private set;
		}

		public VisualElement titleContainer
		{
			get;
			private set;
		}

		public VisualElement inputContainer
		{
			get;
			private set;
		}

		public VisualElement outputContainer
		{
			get;
			private set;
		}

		public VisualElement topContainer
		{
			get;
			private set;
		}

		public VisualElement extensionContainer
		{
			get;
			private set;
		}

		public virtual bool expanded
		{
			get
			{
				return this.m_Expanded;
			}
			set
			{
				if (this.m_Expanded != value)
				{
					this.m_Expanded = value;
					this.RefreshExpandedState();
				}
			}
		}

		public string title
		{
			get
			{
				return this.m_TitleLabel.text;
			}
			set
			{
				this.m_TitleLabel.text = value;
			}
		}

		public Node()
		{
			base.clippingOptions = VisualElement.ClippingOptions.NoClipping;
			VisualTreeAsset visualTreeAsset = EditorGUIUtility.Load("UXML/GraphView/Node.uxml") as VisualTreeAsset;
			visualTreeAsset.CloneTree(this, new Dictionary<string, VisualElement>());
			VisualElement visualElement = this.Q("node-border", null);
			if (visualElement != null)
			{
				visualElement.clippingOptions = VisualElement.ClippingOptions.ClipAndCacheContents;
				this.mainContainer = visualElement;
			}
			else
			{
				this.mainContainer = this;
			}
			this.titleContainer = this.Q("title", null);
			this.inputContainer = this.Q("input", null);
			this.m_CollapsibleArea = this.Q("collapsible-area", null);
			this.extensionContainer = this.Q("extension", null);
			VisualElement visualElement2 = this.Q("output", null);
			this.outputContainer = visualElement2;
			this.topContainer = visualElement2.parent;
			this.m_TitleLabel = this.Q("title-label", null);
			this.m_CollapseButton = this.Q("collapse-button", null);
			this.m_CollapseButton.AddManipulator(new Clickable(new Action(this.ToggleCollapse)));
			base.elementTypeColor = new Color(0.9f, 0.9f, 0.9f, 0.5f);
			if (this != this)
			{
				base.Add(this);
			}
			base.AddToClassList("node");
			base.capabilities |= (Capabilities.Selectable | Capabilities.Movable | Capabilities.Deletable | Capabilities.Ascendable);
			this.m_Expanded = true;
			this.UpdateExpandedButtonState();
			this.UpdateCollapsibleAreaVisibility();
			this.AddManipulator(new ContextualMenuManipulator(new Action<ContextualMenuPopulateEvent>(this.BuildContextualMenu)));
		}

		public void RefreshExpandedState()
		{
			this.UpdateExpandedButtonState();
			bool isVisible = this.RefreshPorts();
			VisualElement visualElement = this.mainContainer.Q("contents", null);
			if (visualElement != null)
			{
				VisualElement visualElement2 = visualElement.Q("divider", null);
				if (visualElement2 != null)
				{
					this.SetElementVisible(visualElement2, isVisible);
				}
				this.UpdateCollapsibleAreaVisibility();
			}
		}

		private void UpdateCollapsibleAreaVisibility()
		{
			if (this.m_CollapsibleArea != null)
			{
				bool flag = this.expanded && this.extensionContainer.childCount > 0;
				if (flag)
				{
					if (this.m_CollapsibleArea.parent == null)
					{
						VisualElement visualElement = this.mainContainer.Q("contents", null);
						if (visualElement == null)
						{
							return;
						}
						visualElement.Add(this.m_CollapsibleArea);
					}
					this.m_CollapsibleArea.BringToFront();
				}
				else if (this.m_CollapsibleArea.parent != null)
				{
					this.m_CollapsibleArea.RemoveFromHierarchy();
				}
			}
		}

		private void UpdateExpandedButtonState()
		{
			base.RemoveFromClassList((!this.m_Expanded) ? "expanded" : "collapsed");
			base.AddToClassList((!this.m_Expanded) ? "collapsed" : "expanded");
		}

		public override Rect GetPosition()
		{
			return new Rect(base.style.positionLeft, base.style.positionTop, base.layout.width, base.layout.height);
		}

		public override void SetPosition(Rect newPos)
		{
			base.style.positionType = PositionType.Absolute;
			base.style.positionLeft = newPos.x;
			base.style.positionTop = newPos.y;
		}

		protected virtual void SetLayoutClassLists(NodePresenter nodePresenter)
		{
		}

		protected virtual void OnPortRemoved(Port port)
		{
		}

		private void ProcessRemovedPorts(IList<Port> currentPorts, VisualElement portContainer, IList<PortPresenter> currentPresenters)
		{
			foreach (Port current in currentPorts)
			{
				bool flag = false;
				PortPresenter presenter = current.GetPresenter<PortPresenter>();
				foreach (PortPresenter current2 in currentPresenters)
				{
					if (current2 == presenter)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					Port expr_75 = current;
					expr_75.OnConnect = (Action<Port>)Delegate.Remove(expr_75.OnConnect, new Action<Port>(this.OnPortConnectAction));
					Port expr_97 = current;
					expr_97.OnDisconnect = (Action<Port>)Delegate.Remove(expr_97.OnDisconnect, new Action<Port>(this.OnPortConnectAction));
					this.OnPortRemoved(current);
					portContainer.Remove(current);
				}
			}
		}

		private void ProcessAddedPorts(IList<Port> currentPorts, VisualElement portContainer, IList<PortPresenter> currentPresenters)
		{
			int num = 0;
			foreach (PortPresenter current in currentPresenters)
			{
				bool flag = false;
				foreach (Port current2 in currentPorts)
				{
					if (current == current2.GetPresenter<PortPresenter>())
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					Port port = this.InstantiatePort(current);
					Port expr_7F = port;
					expr_7F.OnConnect = (Action<Port>)Delegate.Combine(expr_7F.OnConnect, new Action<Port>(this.OnPortConnectAction));
					Port expr_A2 = port;
					expr_A2.OnDisconnect = (Action<Port>)Delegate.Combine(expr_A2.OnDisconnect, new Action<Port>(this.OnPortConnectAction));
					portContainer.Insert(num, port);
				}
				num++;
			}
		}

		public virtual Port InstantiatePort(Orientation orientation, Direction direction, Type type)
		{
			return Port.Create<Edge>(orientation, direction, type);
		}

		public virtual Port InstantiatePort(PortPresenter newPres)
		{
			return Port.Create<EdgePresenter, Edge>(newPres);
		}

		private void SetElementVisible(VisualElement element, bool isVisible)
		{
			element.visible = isVisible;
			if (isVisible)
			{
				element.RemoveFromClassList("hidden");
			}
			else
			{
				element.AddToClassList("hidden");
			}
		}

		private bool AllElementsHidden(VisualElement element)
		{
			bool result;
			for (int i = 0; i < element.childCount; i++)
			{
				if (element[i].visible)
				{
					result = false;
					return result;
				}
			}
			result = true;
			return result;
		}

		private int ShowPorts(bool show, IList<Port> currentPorts)
		{
			int num = 0;
			foreach (Port current in currentPorts)
			{
				if ((show || current.connected) && !current.collapsed)
				{
					this.SetElementVisible(current, true);
					num++;
				}
				else
				{
					this.SetElementVisible(current, false);
				}
			}
			return num;
		}

		public bool RefreshPorts()
		{
			NodePresenter presenter = base.GetPresenter<NodePresenter>();
			bool expanded = this.expanded;
			if (presenter != null)
			{
				List<Port> currentPorts = this.inputContainer.Query(null, null).ToList();
				List<Port> currentPorts2 = this.outputContainer.Query(null, null).ToList();
				this.ProcessRemovedPorts(currentPorts, this.inputContainer, presenter.inputPorts);
				this.ProcessRemovedPorts(currentPorts2, this.outputContainer, presenter.outputPorts);
				this.ProcessAddedPorts(currentPorts, this.inputContainer, presenter.inputPorts);
				this.ProcessAddedPorts(currentPorts2, this.outputContainer, presenter.outputPorts);
				expanded = presenter.expanded;
			}
			List<Port> list = this.inputContainer.Query(null, null).ToList();
			List<Port> list2 = this.outputContainer.Query(null, null).ToList();
			foreach (Port current in list)
			{
				Port expr_EB = current;
				expr_EB.OnConnect = (Action<Port>)Delegate.Combine(expr_EB.OnConnect, new Action<Port>(this.OnPortConnectAction));
				Port expr_10E = current;
				expr_10E.OnDisconnect = (Action<Port>)Delegate.Combine(expr_10E.OnDisconnect, new Action<Port>(this.OnPortConnectAction));
			}
			foreach (Port current2 in list2)
			{
				Port expr_16A = current2;
				expr_16A.OnConnect = (Action<Port>)Delegate.Combine(expr_16A.OnConnect, new Action<Port>(this.OnPortConnectAction));
				Port expr_18D = current2;
				expr_18D.OnDisconnect = (Action<Port>)Delegate.Combine(expr_18D.OnDisconnect, new Action<Port>(this.OnPortConnectAction));
			}
			int num = this.ShowPorts(expanded, list);
			int num2 = this.ShowPorts(expanded, list2);
			VisualElement element = this.topContainer.Q("divider", null);
			bool flag = num2 > 0 || !this.AllElementsHidden(this.outputContainer);
			bool flag2 = num > 0 || !this.AllElementsHidden(this.inputContainer);
			if (flag)
			{
				if (this.outputContainer.shadow.parent != this.topContainer)
				{
					this.topContainer.Add(this.outputContainer);
					this.outputContainer.BringToFront();
				}
			}
			else if (this.outputContainer.shadow.parent == this.topContainer)
			{
				this.outputContainer.RemoveFromHierarchy();
			}
			if (flag2)
			{
				if (this.inputContainer.shadow.parent != this.topContainer)
				{
					this.topContainer.Add(this.inputContainer);
					this.inputContainer.SendToBack();
				}
			}
			else if (this.inputContainer.shadow.parent == this.topContainer)
			{
				this.inputContainer.RemoveFromHierarchy();
			}
			this.SetElementVisible(element, flag2 && flag);
			return flag2 || flag;
		}

		private void OnPortConnectAction(Port port)
		{
			bool flag = false;
			List<Port> list = this.inputContainer.Query(null, null).ToList();
			List<Port> list2 = this.outputContainer.Query(null, null).ToList();
			foreach (Port current in list)
			{
				if (!current.connected)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				foreach (Port current2 in list2)
				{
					if (!current2.connected)
					{
						flag = true;
						break;
					}
				}
			}
			if (flag)
			{
				this.m_CollapseButton.pseudoStates &= ~PseudoStates.Disabled;
			}
			else
			{
				this.m_CollapseButton.pseudoStates |= PseudoStates.Disabled;
			}
		}

		public override void OnDataChanged()
		{
			base.OnDataChanged();
			NodePresenter presenter = base.GetPresenter<NodePresenter>();
			this.m_Expanded = presenter.expanded;
			this.RefreshExpandedState();
			this.m_TitleLabel.text = presenter.title;
			this.SetLayoutClassLists(presenter);
		}

		private void ToggleCollapsePresenter()
		{
			NodePresenter presenter = base.GetPresenter<NodePresenter>();
			presenter.expanded = !presenter.expanded;
		}

		protected virtual void ToggleCollapse()
		{
			if (base.GetPresenter<NodePresenter>() != null)
			{
				this.ToggleCollapsePresenter();
			}
			else
			{
				this.expanded = !this.expanded;
			}
		}

		private void AddConnectionsToDeleteSet(VisualElement container, ref HashSet<GraphElement> toDelete)
		{
			List<GraphElement> toDeleteList = new List<GraphElement>();
			container.Query(null, null).ForEach(delegate(Port elem)
			{
				if (elem.connected)
				{
					foreach (Edge current in elem.connections)
					{
						if ((current.capabilities & Capabilities.Deletable) != (Capabilities)0)
						{
							toDeleteList.Add(current);
						}
					}
				}
			});
			toDelete.UnionWith(toDeleteList);
		}

		private void DisconnectAll(EventBase evt)
		{
			HashSet<GraphElement> hashSet = new HashSet<GraphElement>();
			this.AddConnectionsToDeleteSet(this.inputContainer, ref hashSet);
			this.AddConnectionsToDeleteSet(this.outputContainer, ref hashSet);
			hashSet.Remove(null);
			GraphView firstAncestorOfType = base.GetFirstAncestorOfType<GraphView>();
			if (firstAncestorOfType != null)
			{
				firstAncestorOfType.DeleteElements(hashSet);
			}
			else
			{
				Debug.Log("Disconnecting nodes that are not in a GraphView will not work.");
			}
		}

		private ContextualMenu.MenuAction.StatusFlags DisconnectAllStatus(EventBase evt)
		{
			VisualElement[] array = new VisualElement[]
			{
				this.inputContainer,
				this.outputContainer
			};
			VisualElement[] array2 = array;
			ContextualMenu.MenuAction.StatusFlags result;
			for (int i = 0; i < array2.Length; i++)
			{
				VisualElement e = array2[i];
				List<Port> list = e.Query(null, null).ToList();
				foreach (Port current in list)
				{
					if (current.connected)
					{
						result = ContextualMenu.MenuAction.StatusFlags.Normal;
						return result;
					}
				}
			}
			result = ContextualMenu.MenuAction.StatusFlags.Disabled;
			return result;
		}

		public virtual void BuildContextualMenu(ContextualMenuPopulateEvent evt)
		{
			if (evt.target is Node)
			{
				evt.menu.AppendAction("Disconnect all", new Action<EventBase>(this.DisconnectAll), new Func<EventBase, ContextualMenu.MenuAction.StatusFlags>(this.DisconnectAllStatus));
				evt.menu.AppendSeparator();
			}
		}
	}
}
