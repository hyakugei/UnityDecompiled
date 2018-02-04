using System;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace UnityEditor.Experimental.UIElements
{
	public class FieldMouseDragger<T>
	{
		private IValueField<T> m_DrivenField;

		private VisualElement m_DragElement;

		private Rect m_DragHotZone;

		public bool dragging;

		public T startValue;

		public FieldMouseDragger(IValueField<T> drivenField)
		{
			this.m_DrivenField = drivenField;
			this.m_DragElement = null;
			this.m_DragHotZone = new Rect(0f, 0f, -1f, -1f);
			this.dragging = false;
		}

		public void SetDragZone(VisualElement dragElement)
		{
			this.SetDragZone(dragElement, new Rect(0f, 0f, -1f, -1f));
		}

		public void SetDragZone(VisualElement dragElement, Rect hotZone)
		{
			if (this.m_DragElement != null)
			{
				this.m_DragElement.UnregisterCallback<MouseDownEvent>(new EventCallback<MouseDownEvent>(this.UpdateValueOnMouseDown), Capture.NoCapture);
				this.m_DragElement.UnregisterCallback<MouseMoveEvent>(new EventCallback<MouseMoveEvent>(this.UpdateValueOnMouseMove), Capture.NoCapture);
				this.m_DragElement.UnregisterCallback<MouseUpEvent>(new EventCallback<MouseUpEvent>(this.UpdateValueOnMouseUp), Capture.NoCapture);
				this.m_DragElement.UnregisterCallback<KeyDownEvent>(new EventCallback<KeyDownEvent>(this.UpdateValueOnKeyDown), Capture.NoCapture);
			}
			this.m_DragElement = dragElement;
			this.m_DragHotZone = hotZone;
			if (this.m_DragElement != null)
			{
				this.dragging = false;
				this.m_DragElement.RegisterCallback<MouseDownEvent>(new EventCallback<MouseDownEvent>(this.UpdateValueOnMouseDown), Capture.NoCapture);
				this.m_DragElement.RegisterCallback<MouseMoveEvent>(new EventCallback<MouseMoveEvent>(this.UpdateValueOnMouseMove), Capture.NoCapture);
				this.m_DragElement.RegisterCallback<MouseUpEvent>(new EventCallback<MouseUpEvent>(this.UpdateValueOnMouseUp), Capture.NoCapture);
				this.m_DragElement.RegisterCallback<KeyDownEvent>(new EventCallback<KeyDownEvent>(this.UpdateValueOnKeyDown), Capture.NoCapture);
			}
		}

		private void UpdateValueOnMouseDown(MouseDownEvent evt)
		{
			Rect rect;
			if (this.m_DragHotZone.width < 0f || this.m_DragHotZone.height < 0f)
			{
				rect = this.m_DragElement.localBound;
			}
			else
			{
				rect = this.m_DragHotZone;
			}
			if (evt.button == 0 && rect.Contains(this.m_DragElement.WorldToLocal(evt.mousePosition)))
			{
				this.m_DragElement.TakeMouseCapture();
				this.dragging = true;
				this.startValue = this.m_DrivenField.value;
				EditorGUIUtility.SetWantsMouseJumping(1);
			}
		}

		private void UpdateValueOnMouseMove(MouseMoveEvent evt)
		{
			if (this.dragging)
			{
				DeltaSpeed speed = (!evt.shiftKey) ? ((!evt.altKey) ? DeltaSpeed.Normal : DeltaSpeed.Slow) : DeltaSpeed.Fast;
				this.m_DrivenField.ApplyInputDeviceDelta(evt.mouseDelta, speed, this.startValue);
			}
		}

		private void UpdateValueOnMouseUp(MouseUpEvent evt)
		{
			if (this.dragging)
			{
				this.dragging = false;
				MouseCaptureController.ReleaseMouseCapture();
				EditorGUIUtility.SetWantsMouseJumping(0);
			}
		}

		private void UpdateValueOnKeyDown(KeyDownEvent evt)
		{
			if (this.dragging && evt.keyCode == KeyCode.Escape)
			{
				this.dragging = false;
				this.m_DrivenField.value = this.startValue;
				MouseCaptureController.ReleaseMouseCapture();
				EditorGUIUtility.SetWantsMouseJumping(0);
			}
		}
	}
}
