using System;
using UnityEngine;

namespace UnityEditor
{
	internal class CurveControlPointRenderer
	{
		private ControlPointRenderer m_UnselectedPointRenderer;

		private ControlPointRenderer m_SelectedPointRenderer;

		private ControlPointRenderer m_SelectedPointOverlayRenderer;

		private ControlPointRenderer m_SemiSelectedPointOverlayRenderer;

		private ControlPointRenderer m_WeightedPointRenderer;

		public CurveControlPointRenderer()
		{
			this.m_UnselectedPointRenderer = new ControlPointRenderer(CurveEditor.Styles.pointIcon);
			this.m_SelectedPointRenderer = new ControlPointRenderer(CurveEditor.Styles.pointIconSelected);
			this.m_SelectedPointOverlayRenderer = new ControlPointRenderer(CurveEditor.Styles.pointIconSelectedOverlay);
			this.m_SemiSelectedPointOverlayRenderer = new ControlPointRenderer(CurveEditor.Styles.pointIconSemiSelectedOverlay);
			this.m_WeightedPointRenderer = new ControlPointRenderer(CurveEditor.Styles.pointIconWeighted);
		}

		public void FlushCache()
		{
			this.m_UnselectedPointRenderer.FlushCache();
			this.m_SelectedPointRenderer.FlushCache();
			this.m_SelectedPointOverlayRenderer.FlushCache();
			this.m_SemiSelectedPointOverlayRenderer.FlushCache();
			this.m_WeightedPointRenderer.FlushCache();
		}

		public void Clear()
		{
			this.m_UnselectedPointRenderer.Clear();
			this.m_SelectedPointRenderer.Clear();
			this.m_SelectedPointOverlayRenderer.Clear();
			this.m_SemiSelectedPointOverlayRenderer.Clear();
			this.m_WeightedPointRenderer.Clear();
		}

		public void Render()
		{
			this.m_UnselectedPointRenderer.Render();
			this.m_SelectedPointRenderer.Render();
			this.m_SelectedPointOverlayRenderer.Render();
			this.m_SemiSelectedPointOverlayRenderer.Render();
			this.m_WeightedPointRenderer.Render();
		}

		public void AddPoint(Rect rect, Color color)
		{
			this.m_UnselectedPointRenderer.AddPoint(rect, color);
		}

		public void AddSelectedPoint(Rect rect, Color color)
		{
			this.m_SelectedPointRenderer.AddPoint(rect, color);
			this.m_SelectedPointOverlayRenderer.AddPoint(rect, Color.white);
		}

		public void AddSemiSelectedPoint(Rect rect, Color color)
		{
			this.m_SelectedPointRenderer.AddPoint(rect, color);
			this.m_SemiSelectedPointOverlayRenderer.AddPoint(rect, Color.white);
		}

		public void AddWeightedPoint(Rect rect, Color color)
		{
			this.m_WeightedPointRenderer.AddPoint(rect, color);
		}
	}
}
