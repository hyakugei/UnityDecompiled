using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace UnityEditor.Experimental.UIElements.GraphView
{
	public class SelectionDragger : Dragger
	{
		private IDropTarget m_PrevDropTarget;

		private GraphViewChange m_GraphViewChange;

		private List<GraphElement> m_MovedElements;

		private GraphView m_GraphView;

		private Dictionary<GraphElement, Rect> m_OriginalPos;

		private Vector2 m_originalMouse;

		internal const int k_PanAreaWidth = 100;

		internal const int k_PanSpeed = 4;

		internal const int k_PanInterval = 10;

		internal const float k_MinSpeedFactor = 0.5f;

		internal const float k_MaxSpeedFactor = 2.5f;

		internal const float k_MaxPanSpeed = 10f;

		private IVisualElementScheduledItem m_PanSchedule;

		private Vector3 m_PanDiff = Vector3.zero;

		private Vector3 m_ItemPanDiff = Vector3.zero;

		private Vector2 m_MouseDiff = Vector2.zero;

		private GraphElement selectedElement
		{
			get;
			set;
		}

		private GraphElement clickedElement
		{
			get;
			set;
		}

		public SelectionDragger()
		{
			base.activators.Add(new ManipulatorActivationFilter
			{
				button = MouseButton.LeftMouse
			});
			base.activators.Add(new ManipulatorActivationFilter
			{
				button = MouseButton.LeftMouse,
				modifiers = EventModifiers.Shift
			});
			base.panSpeed = new Vector2(1f, 1f);
			base.clampToParentEdges = false;
			this.m_MovedElements = new List<GraphElement>();
			this.m_GraphViewChange.movedElements = this.m_MovedElements;
		}

		private IDropTarget GetDropTargetAt(Vector2 mousePosition)
		{
			Vector2 point = base.target.LocalToWorld(mousePosition);
			List<VisualElement> list = new List<VisualElement>();
			base.target.panel.PickAll(point, list);
			IDropTarget dropTarget = null;
			for (int i = list.Count - 1; i >= 0; i--)
			{
				if (list[i] != base.target)
				{
					dropTarget = (list[i] as IDropTarget);
					if (dropTarget != null && dropTarget != base.target)
					{
						break;
					}
				}
			}
			return dropTarget;
		}

		protected override void RegisterCallbacksOnTarget()
		{
			if (!(base.target is ISelection))
			{
				throw new InvalidOperationException("Manipulator can only be added to a control that supports selection");
			}
			base.target.RegisterCallback<MouseDownEvent>(new EventCallback<MouseDownEvent>(this.OnMouseDown), Capture.NoCapture);
			base.target.RegisterCallback<MouseMoveEvent>(new EventCallback<MouseMoveEvent>(this.OnMouseMove), Capture.NoCapture);
			base.target.RegisterCallback<MouseUpEvent>(new EventCallback<MouseUpEvent>(this.OnMouseUp), Capture.NoCapture);
			base.target.RegisterCallback<KeyDownEvent>(new EventCallback<KeyDownEvent>(this.OnKeyDown), Capture.NoCapture);
		}

		protected override void UnregisterCallbacksFromTarget()
		{
			base.target.UnregisterCallback<MouseDownEvent>(new EventCallback<MouseDownEvent>(this.OnMouseDown), Capture.NoCapture);
			base.target.UnregisterCallback<MouseMoveEvent>(new EventCallback<MouseMoveEvent>(this.OnMouseMove), Capture.NoCapture);
			base.target.UnregisterCallback<MouseUpEvent>(new EventCallback<MouseUpEvent>(this.OnMouseUp), Capture.NoCapture);
			base.target.UnregisterCallback<KeyDownEvent>(new EventCallback<KeyDownEvent>(this.OnKeyDown), Capture.NoCapture);
		}

		private void SendDragAndDropEvent(IMGUIEvent evt, List<ISelectable> selection, IDropTarget dropTarget)
		{
			if (dropTarget != null && dropTarget.CanAcceptDrop(selection))
			{
				EventType type = evt.imguiEvent.type;
				if (type != EventType.DragUpdated)
				{
					if (type != EventType.DragPerform)
					{
						if (type == EventType.DragExited)
						{
							dropTarget.DragExited();
						}
					}
					else
					{
						dropTarget.DragPerform(evt, selection, dropTarget);
					}
				}
				else
				{
					dropTarget.DragUpdated(evt, selection, dropTarget);
				}
			}
		}

		protected new void OnMouseDown(MouseDownEvent e)
		{
			if (this.m_Active)
			{
				e.StopImmediatePropagation();
			}
			else if (base.CanStartManipulation(e))
			{
				this.m_GraphView = (base.target as GraphView);
				if (this.m_GraphView != null)
				{
					this.selectedElement = null;
					this.clickedElement = (e.target as GraphElement);
					if (this.clickedElement == null)
					{
						VisualElement visualElement = e.target as VisualElement;
						this.clickedElement = visualElement.GetFirstAncestorOfType<GraphElement>();
						if (this.clickedElement == null)
						{
							return;
						}
					}
					if (this.clickedElement.IsMovable() && this.m_GraphView.selection.Contains(this.clickedElement) && this.clickedElement.HitTest(this.clickedElement.WorldToLocal(e.mousePosition)))
					{
						this.selectedElement = this.clickedElement;
						this.m_OriginalPos = new Dictionary<GraphElement, Rect>();
						foreach (ISelectable current in this.m_GraphView.selection)
						{
							GraphElement graphElement = current as GraphElement;
							if (graphElement != null && graphElement.IsMovable())
							{
								this.m_OriginalPos[graphElement] = graphElement.GetPosition();
							}
						}
						this.m_originalMouse = e.mousePosition;
						this.m_ItemPanDiff = Vector3.zero;
						if (this.m_PanSchedule == null)
						{
							this.m_PanSchedule = this.m_GraphView.schedule.Execute(new Action<TimerState>(this.Pan)).Every(10L).StartingIn(10L);
							this.m_PanSchedule.Pause();
						}
						this.m_Active = true;
						base.target.TakeMouseCapture();
						e.StopPropagation();
					}
				}
			}
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

		protected new void OnMouseMove(MouseMoveEvent e)
		{
			if (!base.target.HasMouseCapture())
			{
				this.m_PrevDropTarget = null;
				this.m_Active = false;
			}
			if (this.m_Active)
			{
				if (this.m_GraphView != null)
				{
					VisualElement src = (VisualElement)e.target;
					Vector2 mousePos = src.ChangeCoordinatesTo(this.m_GraphView.contentContainer, e.localMousePosition);
					this.m_PanDiff = this.GetEffectivePanSpeed(mousePos);
					if (this.m_PanDiff != Vector3.zero)
					{
						this.m_PanSchedule.Resume();
					}
					else
					{
						this.m_PanSchedule.Pause();
					}
					this.m_MouseDiff = this.m_originalMouse - e.mousePosition;
					foreach (KeyValuePair<GraphElement, Rect> current in this.m_OriginalPos)
					{
						GraphElement key = current.Key;
						Matrix4x4 worldTransform = key.worldTransform;
						Vector3 vector = new Vector3(worldTransform.m00, worldTransform.m11, worldTransform.m22);
						Rect position = key.GetPosition();
						key.SetPosition(new Rect(current.Value.x - (this.m_MouseDiff.x - this.m_ItemPanDiff.x) * base.panSpeed.x / vector.x, current.Value.y - (this.m_MouseDiff.y - this.m_ItemPanDiff.y) * base.panSpeed.y / vector.y, position.width, position.height));
					}
					List<ISelectable> selection = this.m_GraphView.selection;
					IDropTarget dropTargetAt = this.GetDropTargetAt(e.localMousePosition);
					if (this.m_PrevDropTarget != dropTargetAt && this.m_PrevDropTarget != null)
					{
						using (IMGUIEvent pooled = IMGUIEvent.GetPooled(e.imguiEvent))
						{
							pooled.imguiEvent.type = EventType.DragExited;
							this.SendDragAndDropEvent(pooled, selection, this.m_PrevDropTarget);
						}
					}
					using (IMGUIEvent pooled2 = IMGUIEvent.GetPooled(e.imguiEvent))
					{
						pooled2.imguiEvent.type = EventType.DragUpdated;
						this.SendDragAndDropEvent(pooled2, selection, dropTargetAt);
					}
					this.m_PrevDropTarget = dropTargetAt;
					this.selectedElement = null;
					e.StopPropagation();
				}
			}
		}

		private void Pan(TimerState ts)
		{
			this.m_GraphView.viewTransform.position -= this.m_PanDiff;
			this.m_ItemPanDiff += this.m_PanDiff;
			foreach (KeyValuePair<GraphElement, Rect> current in this.m_OriginalPos)
			{
				GraphElement key = current.Key;
				Matrix4x4 worldTransform = key.worldTransform;
				Vector3 vector = new Vector3(worldTransform.m00, worldTransform.m11, worldTransform.m22);
				Rect position = key.GetPosition();
				key.SetPosition(new Rect(current.Value.x - (this.m_MouseDiff.x - this.m_ItemPanDiff.x) * base.panSpeed.x / vector.x, current.Value.y - (this.m_MouseDiff.y - this.m_ItemPanDiff.y) * base.panSpeed.y / vector.y, position.width, position.height));
			}
		}

		protected new void OnMouseUp(MouseUpEvent evt)
		{
			if (this.m_GraphView == null)
			{
				if (this.m_Active && base.target.HasMouseCapture())
				{
					base.target.ReleaseMouseCapture();
					this.selectedElement = null;
					this.m_Active = false;
					this.m_PrevDropTarget = null;
				}
			}
			else
			{
				List<ISelectable> selection = this.m_GraphView.selection;
				if (base.CanStopManipulation(evt))
				{
					if (this.m_Active && base.target.HasMouseCapture())
					{
						if (this.selectedElement == null)
						{
							this.m_MovedElements.Clear();
							foreach (GraphElement current in this.m_OriginalPos.Keys)
							{
								current.UpdatePresenterPosition();
								this.m_MovedElements.Add(current);
							}
							GraphView graphView = base.target as GraphView;
							if (graphView != null && graphView.graphViewChanged != null)
							{
								graphView.graphViewChanged(this.m_GraphViewChange);
							}
						}
						this.m_PanSchedule.Pause();
						if (this.m_ItemPanDiff != Vector3.zero)
						{
							Vector3 position = this.m_GraphView.contentViewContainer.transform.position;
							Vector3 scale = this.m_GraphView.contentViewContainer.transform.scale;
							this.m_GraphView.UpdateViewTransform(position, scale);
						}
						if (selection.Count > 0 && this.m_PrevDropTarget != null)
						{
							using (IMGUIEvent pooled = IMGUIEvent.GetPooled(evt.imguiEvent))
							{
								pooled.imguiEvent.type = EventType.DragPerform;
								this.SendDragAndDropEvent(pooled, selection, this.m_PrevDropTarget);
							}
						}
						base.target.ReleaseMouseCapture();
						evt.StopPropagation();
					}
					this.selectedElement = null;
					this.m_Active = false;
					this.m_PrevDropTarget = null;
				}
			}
		}

		private void OnKeyDown(KeyDownEvent e)
		{
			if (e.keyCode == KeyCode.Escape && this.m_GraphView != null && this.m_Active)
			{
				foreach (KeyValuePair<GraphElement, Rect> current in this.m_OriginalPos)
				{
					current.Key.SetPosition(current.Value);
				}
				this.m_PanSchedule.Pause();
				if (this.m_ItemPanDiff != Vector3.zero)
				{
					Vector3 position = this.m_GraphView.contentViewContainer.transform.position;
					Vector3 scale = this.m_GraphView.contentViewContainer.transform.scale;
					this.m_GraphView.UpdateViewTransform(position, scale);
				}
				base.target.ReleaseMouseCapture();
				e.StopPropagation();
			}
		}
	}
}
