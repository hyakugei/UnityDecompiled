using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace UnityEditor.Experimental.UIElements.GraphView
{
	public abstract class EdgeDragHelper
	{
		internal const int k_PanAreaWidth = 100;

		internal const int k_PanSpeed = 4;

		internal const int k_PanInterval = 10;

		internal const float k_MinSpeedFactor = 0.5f;

		internal const float k_MaxSpeedFactor = 2.5f;

		internal const float k_MaxPanSpeed = 10f;

		public abstract Edge edgeCandidate
		{
			get;
			set;
		}

		public abstract Port draggedPort
		{
			get;
			set;
		}

		public abstract bool HandleMouseDown(MouseDownEvent evt);

		public abstract void HandleMouseMove(MouseMoveEvent evt);

		public abstract void HandleMouseUp(MouseUpEvent evt);

		public abstract void Reset(bool didConnect = false);
	}
	public class EdgeDragHelper<TEdge> : EdgeDragHelper where TEdge : Edge, new()
	{
		protected List<Port> m_CompatiblePorts;

		private Edge m_GhostEdge;

		protected GraphView m_GraphView;

		protected static NodeAdapter s_nodeAdapter = new NodeAdapter();

		protected readonly IEdgeConnectorListener m_Listener;

		private IVisualElementScheduledItem m_PanSchedule;

		private Vector3 m_PanDiff = Vector3.zero;

		private bool m_WasPanned;

		public bool resetPositionOnPan
		{
			get;
			set;
		}

		public override Edge edgeCandidate
		{
			get;
			set;
		}

		public override Port draggedPort
		{
			get;
			set;
		}

		public EdgeDragHelper(IEdgeConnectorListener listener)
		{
			this.m_Listener = listener;
			this.resetPositionOnPan = true;
			this.Reset(false);
		}

		public override void Reset(bool didConnect = false)
		{
			if (this.m_CompatiblePorts != null)
			{
				foreach (Port current in this.m_CompatiblePorts)
				{
					current.highlight = true;
				}
			}
			if (this.m_GhostEdge != null && this.m_GraphView != null)
			{
				this.m_GraphView.RemoveElement(this.m_GhostEdge);
			}
			if (this.m_WasPanned)
			{
				if (!this.resetPositionOnPan || didConnect)
				{
					Vector3 position = this.m_GraphView.contentViewContainer.transform.position;
					Vector3 scale = this.m_GraphView.contentViewContainer.transform.scale;
					this.m_GraphView.UpdateViewTransform(position, scale);
				}
			}
			if (this.m_PanSchedule != null)
			{
				this.m_PanSchedule.Pause();
			}
			if (this.m_GhostEdge != null)
			{
				this.m_GhostEdge.input = null;
				this.m_GhostEdge.output = null;
			}
			this.m_GhostEdge = null;
			this.edgeCandidate = null;
			this.draggedPort = null;
			this.m_CompatiblePorts = null;
			this.m_GraphView = null;
		}

		public override bool HandleMouseDown(MouseDownEvent evt)
		{
			Vector2 mousePosition = evt.mousePosition;
			bool result;
			if (this.draggedPort == null || this.edgeCandidate == null)
			{
				result = false;
			}
			else
			{
				this.m_GraphView = this.draggedPort.GetFirstAncestorOfType<GraphView>();
				if (this.m_GraphView == null)
				{
					result = false;
				}
				else
				{
					bool flag = this.draggedPort.direction == Direction.Output;
					if (flag)
					{
						this.edgeCandidate.output = this.draggedPort;
						this.edgeCandidate.input = null;
					}
					else
					{
						this.edgeCandidate.output = null;
						this.edgeCandidate.input = this.draggedPort;
					}
					this.draggedPort.portCapLit = true;
					if (this.edgeCandidate.parent == null)
					{
						this.m_GraphView.AddElement(this.edgeCandidate);
					}
					this.edgeCandidate.candidatePosition = mousePosition;
					this.m_CompatiblePorts = this.m_GraphView.GetCompatiblePorts(this.draggedPort, EdgeDragHelper<TEdge>.s_nodeAdapter);
					foreach (Port current in this.m_GraphView.ports.ToList())
					{
						current.highlight = false;
					}
					foreach (Port current2 in this.m_CompatiblePorts)
					{
						current2.highlight = true;
					}
					this.edgeCandidate.UpdateEdgeControl();
					if (this.m_PanSchedule == null)
					{
						this.m_PanSchedule = this.m_GraphView.schedule.Execute(new Action<TimerState>(this.Pan)).Every(10L).StartingIn(10L);
						this.m_PanSchedule.Pause();
					}
					this.m_WasPanned = false;
					this.edgeCandidate.layer = 2147483647;
					result = true;
				}
			}
			return result;
		}

		internal Vector2 GetEffectivePanSpeed(Vector2 mousePos)
		{
			Vector2 zero = Vector2.zero;
			if (mousePos.x <= 100f)
			{
				zero.x = -((100f - mousePos.x) / 100f + 0.5f) * 4f;
			}
			else if (mousePos.x >= this.m_GraphView.contentContainer.layout.width - 100f)
			{
				zero.x = ((mousePos.x - (this.m_GraphView.contentContainer.layout.width - 100f)) / 100f + 0.5f) * 4f;
			}
			if (mousePos.y <= 100f)
			{
				zero.y = -((100f - mousePos.y) / 100f + 0.5f) * 4f;
			}
			else if (mousePos.y >= this.m_GraphView.contentContainer.layout.height - 100f)
			{
				zero.y = ((mousePos.y - (this.m_GraphView.contentContainer.layout.height - 100f)) / 100f + 0.5f) * 4f;
			}
			return Vector2.ClampMagnitude(zero, 10f);
		}

		public override void HandleMouseMove(MouseMoveEvent evt)
		{
			VisualElement src = (VisualElement)evt.target;
			Vector2 mousePos = src.ChangeCoordinatesTo(this.m_GraphView.contentContainer, evt.localMousePosition);
			this.m_PanDiff = this.GetEffectivePanSpeed(mousePos);
			if (this.m_PanDiff != Vector3.zero)
			{
				this.m_PanSchedule.Resume();
			}
			else
			{
				this.m_PanSchedule.Pause();
			}
			Vector2 mousePosition = evt.mousePosition;
			this.edgeCandidate.candidatePosition = mousePosition;
			Port endPort = this.GetEndPort(mousePosition);
			if (endPort != null)
			{
				if (this.m_GhostEdge == null)
				{
					this.m_GhostEdge = Activator.CreateInstance<TEdge>();
					this.m_GhostEdge.isGhostEdge = true;
					this.m_GraphView.AddElement(this.m_GhostEdge);
				}
				if (this.edgeCandidate.output == null)
				{
					this.m_GhostEdge.input = this.edgeCandidate.input;
					if (this.m_GhostEdge.output != null)
					{
						this.m_GhostEdge.output.portCapLit = false;
					}
					this.m_GhostEdge.output = endPort;
					this.m_GhostEdge.output.portCapLit = true;
				}
				else
				{
					if (this.m_GhostEdge.input != null)
					{
						this.m_GhostEdge.input.portCapLit = false;
					}
					this.m_GhostEdge.input = endPort;
					this.m_GhostEdge.input.portCapLit = true;
					this.m_GhostEdge.output = this.edgeCandidate.output;
				}
			}
			else if (this.m_GhostEdge != null)
			{
				if (this.edgeCandidate.input == null)
				{
					if (this.m_GhostEdge.input != null)
					{
						this.m_GhostEdge.input.portCapLit = false;
					}
				}
				else if (this.m_GhostEdge.output != null)
				{
					this.m_GhostEdge.output.portCapLit = false;
				}
				this.m_GraphView.RemoveElement(this.m_GhostEdge);
				this.m_GhostEdge.input = null;
				this.m_GhostEdge.output = null;
				this.m_GhostEdge = null;
			}
		}

		private void Pan(TimerState ts)
		{
			this.m_GraphView.viewTransform.position -= this.m_PanDiff;
			this.m_WasPanned = true;
		}

		public override void HandleMouseUp(MouseUpEvent evt)
		{
			bool didConnect = false;
			Vector2 mousePosition = evt.mousePosition;
			foreach (Port current in this.m_GraphView.ports.ToList())
			{
				current.highlight = true;
			}
			if (this.m_GhostEdge != null)
			{
				if (this.m_GhostEdge.input != null)
				{
					this.m_GhostEdge.input.portCapLit = false;
				}
				if (this.m_GhostEdge.output != null)
				{
					this.m_GhostEdge.output.portCapLit = false;
				}
				this.m_GraphView.RemoveElement(this.m_GhostEdge);
				this.m_GhostEdge.input = null;
				this.m_GhostEdge.output = null;
				this.m_GhostEdge = null;
			}
			Port endPort = this.GetEndPort(mousePosition);
			if (endPort == null && this.m_Listener != null)
			{
				this.m_Listener.OnDropOutsidePort(this.edgeCandidate, mousePosition);
			}
			if (this.edgeCandidate.input != null)
			{
				this.edgeCandidate.input.portCapLit = false;
			}
			if (this.edgeCandidate.output != null)
			{
				this.edgeCandidate.output.portCapLit = false;
			}
			if (this.edgeCandidate.input != null && this.edgeCandidate.output != null)
			{
				Port input = this.edgeCandidate.input;
				Port output = this.edgeCandidate.output;
				this.m_GraphView.DeleteElements(new Edge[]
				{
					this.edgeCandidate
				});
				this.edgeCandidate.input = input;
				this.edgeCandidate.output = output;
			}
			else
			{
				this.m_GraphView.RemoveElement(this.edgeCandidate);
			}
			if (endPort != null)
			{
				if (endPort.direction == Direction.Output)
				{
					this.edgeCandidate.output = endPort;
				}
				else
				{
					this.edgeCandidate.input = endPort;
				}
				this.m_Listener.OnDrop(this.m_GraphView, this.edgeCandidate);
				didConnect = true;
			}
			else
			{
				this.edgeCandidate.output = null;
				this.edgeCandidate.input = null;
			}
			this.edgeCandidate.ResetLayer();
			this.edgeCandidate = null;
			this.m_CompatiblePorts = null;
			this.Reset(didConnect);
		}

		private Port GetEndPort(Vector2 mousePosition)
		{
			Port result;
			if (this.m_GraphView == null)
			{
				result = null;
			}
			else
			{
				Port port = null;
				foreach (Port current in this.m_CompatiblePorts)
				{
					Rect worldBound = current.worldBound;
					float height = worldBound.height;
					if (current.direction == Direction.Input)
					{
						worldBound.x -= height;
						worldBound.width += height;
					}
					else if (current.direction == Direction.Output)
					{
						worldBound.width += height;
					}
					if (worldBound.Contains(mousePosition))
					{
						port = current;
						break;
					}
				}
				result = port;
			}
			return result;
		}
	}
}
