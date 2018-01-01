using System;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace UnityEditor.Experimental.UIElements.GraphView
{
	public class ContentZoomer : Manipulator
	{
		public static readonly float DefaultReferenceScale = 1f;

		public static readonly float DefaultMinScale = 0.25f;

		public static readonly float DefaultMaxScale = 1f;

		public static readonly float DefaultScaleStep = 0.15f;

		private IVisualElementScheduledItem m_OnTimerTicker;

		public float referenceScale
		{
			get;
			set;
		}

		public float minScale
		{
			get;
			set;
		}

		public float maxScale
		{
			get;
			set;
		}

		public float scaleStep
		{
			get;
			set;
		}

		public bool keepPixelCacheOnZoom
		{
			get;
			set;
		}

		private bool delayRepaintScheduled
		{
			get;
			set;
		}

		public ContentZoomer()
		{
			this.<referenceScale>k__BackingField = ContentZoomer.DefaultReferenceScale;
			this.<minScale>k__BackingField = ContentZoomer.DefaultMinScale;
			this.<maxScale>k__BackingField = ContentZoomer.DefaultMaxScale;
			this.<scaleStep>k__BackingField = ContentZoomer.DefaultScaleStep;
			base..ctor();
		}

		protected override void RegisterCallbacksOnTarget()
		{
			if (!(base.target is GraphView))
			{
				throw new InvalidOperationException("Manipulator can only be added to a GraphView");
			}
			base.target.RegisterCallback<WheelEvent>(new EventCallback<WheelEvent>(this.OnWheel), Capture.NoCapture);
		}

		protected override void UnregisterCallbacksFromTarget()
		{
			base.target.UnregisterCallback<WheelEvent>(new EventCallback<WheelEvent>(this.OnWheel), Capture.NoCapture);
		}

		private void OnTimer(TimerState timerState)
		{
			GraphView graphView = base.target as GraphView;
			if (graphView != null)
			{
				if (graphView.elementPanel != null)
				{
					graphView.elementPanel.keepPixelCacheOnWorldBoundChange = false;
				}
				this.delayRepaintScheduled = false;
			}
		}

		private static float CalculateNewZoom(float currentZoom, float wheelDelta, float zoomStep, float referenceZoom, float minZoom, float maxZoom)
		{
			float result;
			if (minZoom <= 0f)
			{
				Debug.LogError(string.Format("The minimum zoom ({0}) must be greater than zero.", minZoom));
				result = currentZoom;
			}
			else if (referenceZoom < minZoom)
			{
				Debug.LogError(string.Format("The reference zoom ({0}) must be greater than or equal to the minimum zoom ({1}).", referenceZoom, minZoom));
				result = currentZoom;
			}
			else if (referenceZoom > maxZoom)
			{
				Debug.LogError(string.Format("The reference zoom ({0}) must be less than or equal to the maximum zoom ({1}).", referenceZoom, maxZoom));
				result = currentZoom;
			}
			else if (zoomStep < 0f)
			{
				Debug.LogError(string.Format("The zoom step ({0}) must be greater than or equal to zero.", zoomStep));
				result = currentZoom;
			}
			else
			{
				currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);
				if (Mathf.Approximately(wheelDelta, 0f))
				{
					result = currentZoom;
				}
				else
				{
					double num = Math.Log((double)referenceZoom, (double)(1f + zoomStep));
					double num2 = (double)referenceZoom - Math.Pow((double)(1f + zoomStep), num);
					double num3 = Math.Log((double)minZoom - num2, (double)(1f + zoomStep)) - num;
					double num4 = Math.Log((double)maxZoom - num2, (double)(1f + zoomStep)) - num;
					double num5 = Math.Log((double)currentZoom - num2, (double)(1f + zoomStep)) - num;
					wheelDelta = (float)Math.Sign(wheelDelta);
					num5 += (double)wheelDelta;
					if (num5 > num4 - 0.5)
					{
						result = maxZoom;
					}
					else if (num5 < num3 + 0.5)
					{
						result = minZoom;
					}
					else
					{
						num5 = Math.Round(num5);
						result = (float)(Math.Pow((double)(1f + zoomStep), num5 + num) + num2);
					}
				}
			}
			return result;
		}

		private void OnWheel(WheelEvent evt)
		{
			GraphView graphView = base.target as GraphView;
			if (graphView != null)
			{
				if (!MouseCaptureController.IsMouseCaptureTaken())
				{
					Vector3 vector = graphView.viewTransform.position;
					Vector3 scale = graphView.viewTransform.scale;
					Vector2 vector2 = base.target.ChangeCoordinatesTo(graphView.contentViewContainer, evt.localMousePosition);
					float x = vector2.x + graphView.contentViewContainer.layout.x;
					float y = vector2.y + graphView.contentViewContainer.layout.y;
					vector += Vector3.Scale(new Vector3(x, y, 0f), scale);
					float num = ContentZoomer.CalculateNewZoom(scale.y, -evt.delta.y, this.scaleStep, this.referenceScale, this.minScale, this.maxScale);
					scale.x = num;
					scale.y = num;
					scale.z = 1f;
					vector -= Vector3.Scale(new Vector3(x, y, 0f), scale);
					if (graphView.elementPanel != null && this.keepPixelCacheOnZoom)
					{
						graphView.elementPanel.keepPixelCacheOnWorldBoundChange = true;
						if (this.m_OnTimerTicker == null)
						{
							this.m_OnTimerTicker = graphView.schedule.Execute(new Action<TimerState>(this.OnTimer));
						}
						this.m_OnTimerTicker.ExecuteLater(500L);
						this.delayRepaintScheduled = true;
					}
					graphView.UpdateViewTransform(vector, scale);
					evt.StopPropagation();
				}
			}
		}
	}
}
