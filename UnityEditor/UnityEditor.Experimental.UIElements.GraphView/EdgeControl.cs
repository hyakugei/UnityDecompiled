using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace UnityEditor.Experimental.UIElements.GraphView
{
	public class EdgeControl : VisualElement
	{
		private struct EdgeCornerSweepValues
		{
			public Vector2 circleCenter;

			public double sweepAngle;

			public double startAngle;

			public double endAngle;

			public Vector2 crossPoint1;

			public Vector2 crossPoint2;

			public float radius;
		}

		private VisualElement m_FromCap;

		private VisualElement m_ToCap;

		private GraphView m_GraphView;

		private bool m_ControlPointsDirty = true;

		private bool m_RenderPointsDirty = true;

		private bool m_MeshDirty = true;

		private Mesh m_Mesh;

		public const float k_MinEdgeWidth = 1.75f;

		private const float k_EdgeLengthFromPort = 12f;

		private const float k_EdgeTurnDiameter = 16f;

		private const float k_EdgeSweepResampleRatio = 4f;

		private const int k_EdgeStraightLineSegmentDivisor = 5;

		private Orientation m_Orientation;

		private Color m_InputColor = Color.grey;

		private Color m_OutputColor = Color.grey;

		private Color m_FromCapColor;

		private Color m_ToCapColor;

		private float m_CapRadius = 5f;

		private int m_EdgeWidth = 2;

		private float m_InterceptWidth = 5f;

		private Vector2 m_From;

		private Vector2 m_To;

		private Vector2[] m_ControlPoints;

		private bool m_DrawFromCap;

		private bool m_DrawToCap;

		private List<Vector2> m_RenderPoints = new List<Vector2>();

		private static Material s_LineMat;

		public Orientation orientation
		{
			get
			{
				return this.m_Orientation;
			}
			set
			{
				if (this.m_Orientation != value)
				{
					this.m_Orientation = value;
					base.Dirty(ChangeType.Repaint);
				}
			}
		}

		[Obsolete("Use inputEdgeColor and/or outputEdgeColor")]
		public Color edgeColor
		{
			get
			{
				return this.m_InputColor;
			}
			set
			{
				if (!(this.m_InputColor == value) || !(this.m_OutputColor == value))
				{
					this.m_InputColor = value;
					this.m_OutputColor = value;
					base.Dirty(ChangeType.Repaint);
				}
			}
		}

		public Color inputColor
		{
			get
			{
				return this.m_InputColor;
			}
			set
			{
				if (this.m_InputColor != value)
				{
					this.m_InputColor = value;
					base.Dirty(ChangeType.Repaint);
				}
			}
		}

		public Color outputColor
		{
			get
			{
				return this.m_OutputColor;
			}
			set
			{
				if (this.m_OutputColor != value)
				{
					this.m_OutputColor = value;
					base.Dirty(ChangeType.Repaint);
				}
			}
		}

		public Color fromCapColor
		{
			get
			{
				return this.m_FromCapColor;
			}
			set
			{
				if (!(this.m_FromCapColor == value))
				{
					this.m_FromCapColor = value;
					this.m_FromCap.style.backgroundColor = this.m_FromCapColor;
					base.Dirty(ChangeType.Repaint);
				}
			}
		}

		public Color toCapColor
		{
			get
			{
				return this.m_ToCapColor;
			}
			set
			{
				if (!(this.m_ToCapColor == value))
				{
					this.m_ToCapColor = value;
					this.m_ToCap.style.backgroundColor = this.m_ToCapColor;
					base.Dirty(ChangeType.Repaint);
				}
			}
		}

		public float capRadius
		{
			get
			{
				return this.m_CapRadius;
			}
			set
			{
				if (this.m_CapRadius != value)
				{
					this.m_CapRadius = value;
					base.Dirty(ChangeType.Repaint);
				}
			}
		}

		public int edgeWidth
		{
			get
			{
				return this.m_EdgeWidth;
			}
			set
			{
				if (this.m_EdgeWidth != value)
				{
					this.m_EdgeWidth = value;
					this.m_MeshDirty = true;
					this.UpdateLayout();
					base.Dirty(ChangeType.Repaint);
				}
			}
		}

		public float interceptWidth
		{
			get
			{
				return this.m_InterceptWidth;
			}
			set
			{
				this.m_InterceptWidth = value;
			}
		}

		public Vector2 from
		{
			get
			{
				return this.m_From;
			}
			set
			{
				if ((this.m_From - value).sqrMagnitude > 0.25f)
				{
					this.m_From = value;
					this.PointsChanged();
				}
			}
		}

		public Vector2 to
		{
			get
			{
				return this.m_To;
			}
			set
			{
				if ((this.m_To - value).sqrMagnitude > 0.25f)
				{
					this.m_To = value;
					this.PointsChanged();
				}
			}
		}

		public Vector2[] controlPoints
		{
			get
			{
				return this.m_ControlPoints;
			}
		}

		public Vector2[] renderPoints
		{
			get
			{
				this.UpdateRenderPoints();
				return this.m_RenderPoints.ToArray();
			}
		}

		public bool drawFromCap
		{
			get
			{
				return this.m_DrawFromCap;
			}
			set
			{
				if (value != this.m_DrawFromCap)
				{
					this.m_DrawFromCap = value;
					if (!this.m_DrawFromCap)
					{
						this.m_FromCap.pseudoStates |= PseudoStates.Invisible;
					}
					else
					{
						this.m_FromCap.pseudoStates &= (PseudoStates)2147483647;
					}
					base.Dirty(ChangeType.Layout);
				}
			}
		}

		public bool drawToCap
		{
			get
			{
				return this.m_DrawToCap;
			}
			set
			{
				if (value != this.m_DrawToCap)
				{
					this.m_DrawToCap = value;
					if (!this.m_DrawToCap)
					{
						this.m_ToCap.pseudoStates |= PseudoStates.Invisible;
					}
					else
					{
						this.m_ToCap.pseudoStates &= (PseudoStates)2147483647;
					}
					base.Dirty(ChangeType.Layout);
				}
			}
		}

		private static Material lineMat
		{
			get
			{
				if (EdgeControl.s_LineMat == null)
				{
					EdgeControl.s_LineMat = new Material(EditorGUIUtility.LoadRequired("GraphView/AAEdge.shader") as Shader);
				}
				return EdgeControl.s_LineMat;
			}
		}

		public EdgeControl()
		{
			base.RegisterCallback<DetachFromPanelEvent>(new EventCallback<DetachFromPanelEvent>(this.OnLeavePanel), Capture.NoCapture);
			this.m_FromCap = new VisualElement();
			this.m_FromCap.AddToClassList("edgeCap");
			this.m_FromCap.pseudoStates |= PseudoStates.Invisible;
			base.Add(this.m_FromCap);
			this.m_ToCap = new VisualElement();
			this.m_ToCap.AddToClassList("edgeCap");
			this.m_ToCap.pseudoStates |= PseudoStates.Invisible;
			base.Add(this.m_ToCap);
			this.m_DrawFromCap = false;
			this.m_DrawToCap = false;
		}

		public override void DoRepaint()
		{
			this.m_FromCap.layout = new Rect(base.parent.ChangeCoordinatesTo(this, this.m_From) - this.m_FromCap.layout.size / 2f, this.m_FromCap.layout.size);
			this.m_ToCap.layout = new Rect(base.parent.ChangeCoordinatesTo(this, this.m_To) - this.m_ToCap.layout.size / 2f, this.m_ToCap.layout.size);
			this.DrawEdge();
		}

		public override bool ContainsPoint(Vector2 localPoint)
		{
			bool result;
			if (Vector2.Distance(this.from, localPoint) <= 2f * this.capRadius || Vector2.Distance(this.to, localPoint) <= 2f * this.capRadius)
			{
				result = false;
			}
			else
			{
				Vector2[] renderPoints = this.renderPoints;
				for (int i = 0; i < renderPoints.Length - 1; i++)
				{
					Vector2 a = renderPoints[i];
					Vector2 vector = renderPoints[i + 1];
					float num = Vector2.Distance(a, localPoint);
					float num2 = Vector2.Distance(vector, localPoint);
					float num3 = Vector2.Distance(a, vector);
					if (num < num3 && num2 < num3)
					{
						if (Mathf.Abs((vector.y - a.y) * localPoint.x - (vector.x - a.x) * localPoint.y + vector.x * a.y - vector.y * a.x) / num3 < this.interceptWidth)
						{
							result = true;
							return result;
						}
					}
				}
				result = false;
			}
			return result;
		}

		public override bool Overlaps(Rect rect)
		{
			bool result;
			for (int i = 0; i < this.m_RenderPoints.Count - 1; i++)
			{
				Vector2 p = new Vector2(this.m_RenderPoints[i].x, this.m_RenderPoints[i].y);
				Vector2 p2 = new Vector2(this.m_RenderPoints[i + 1].x, this.m_RenderPoints[i + 1].y);
				if (RectUtils.IntersectsSegment(rect, p, p2))
				{
					result = true;
					return result;
				}
			}
			result = false;
			return result;
		}

		protected virtual void PointsChanged()
		{
			this.m_ControlPointsDirty = true;
			base.Dirty(ChangeType.Repaint);
		}

		public virtual void UpdateLayout()
		{
			if (base.parent != null)
			{
				if (this.m_ControlPointsDirty)
				{
					this.ComputeControlPoints();
					this.ComputeLayout();
					this.m_ControlPointsDirty = false;
				}
			}
		}

		protected virtual void UpdateRenderPoints()
		{
			this.ComputeControlPoints();
			if (this.m_RenderPointsDirty || this.m_ControlPoints == null)
			{
				this.m_RenderPointsDirty = false;
				this.m_MeshDirty = true;
				this.m_RenderPoints.Clear();
				float num = 16f;
				Vector2 vector = base.parent.ChangeCoordinatesTo(this, this.m_ControlPoints[0]);
				Vector2 vector2 = base.parent.ChangeCoordinatesTo(this, this.m_ControlPoints[1]);
				Vector2 vector3 = base.parent.ChangeCoordinatesTo(this, this.m_ControlPoints[2]);
				Vector2 vector4 = base.parent.ChangeCoordinatesTo(this, this.m_ControlPoints[3]);
				if ((this.orientation == Orientation.Horizontal && Mathf.Abs(vector.y - vector4.y) < 2f && vector.x + 12f < vector4.x - 12f) || (this.orientation == Orientation.Vertical && Mathf.Abs(vector.x - vector4.x) < 2f && vector.y + 12f < vector4.y - 12f))
				{
					float num2 = (this.orientation != Orientation.Horizontal) ? Mathf.Abs(vector.y + 12f - (vector4.y - 12f)) : Mathf.Abs(vector.x + 12f - (vector4.x - 12f));
					float a = num2 / 5f;
					float num3 = Mathf.Min(a, num);
					num3 = Mathf.Max(0f, num3);
					Vector2 b = (this.orientation != Orientation.Horizontal) ? new Vector2(0f, num - num3) : new Vector2(num - num3, 0f);
					this.m_RenderPoints.Add(vector);
					this.m_RenderPoints.Add(vector2 - b);
					this.m_RenderPoints.Add(vector3 + b);
					this.m_RenderPoints.Add(vector4);
				}
				else
				{
					this.m_RenderPoints.Add(vector);
					EdgeControl.EdgeCornerSweepValues cornerSweepValues = this.GetCornerSweepValues(vector, vector2, vector3, num, Direction.Output);
					EdgeControl.EdgeCornerSweepValues cornerSweepValues2 = this.GetCornerSweepValues(vector2, vector3, vector4, num, Direction.Input);
					this.ValidateCornerSweepValues(ref cornerSweepValues, ref cornerSweepValues2);
					this.GetRoundedCornerPoints(this.m_RenderPoints, cornerSweepValues, Direction.Output);
					this.GetRoundedCornerPoints(this.m_RenderPoints, cornerSweepValues2, Direction.Input);
					this.m_RenderPoints.Add(vector4);
				}
			}
		}

		private void ValidateCornerSweepValues(ref EdgeControl.EdgeCornerSweepValues corner1, ref EdgeControl.EdgeCornerSweepValues corner2)
		{
			Vector2 b = (corner1.circleCenter + corner2.circleCenter) / 2f;
			Vector2 vector = corner1.circleCenter - corner1.crossPoint1;
			Vector2 vector2 = corner1.circleCenter - b;
			double num = (this.orientation != Orientation.Horizontal) ? (Math.Atan2((double)vector.x, (double)vector.y) - Math.Atan2((double)vector2.x, (double)vector2.y)) : (Math.Atan2((double)vector.y, (double)vector.x) - Math.Atan2((double)vector2.y, (double)vector2.x));
			num = (double)((float)(Math.Sign(num) * 2) * 3.14159274f) - num;
			if ((double)Mathf.Abs((float)num) > 4.71238911151886)
			{
				num = (double)((float)(-1 * Math.Sign(num) * 2) * 3.14159274f) + num;
			}
			float magnitude = vector2.magnitude;
			float num2 = Mathf.Acos(corner1.radius / magnitude);
			float a = Mathf.Abs((float)corner1.sweepAngle) - num2 * 2f;
			if (Mathf.Abs((float)num) < Mathf.Abs((float)corner1.sweepAngle))
			{
				corner1.sweepAngle = (double)((float)Math.Sign(corner1.sweepAngle) * Mathf.Min(a, Mathf.Abs((float)corner1.sweepAngle)));
				corner2.sweepAngle = (double)((float)Math.Sign(corner2.sweepAngle) * Mathf.Min(a, Mathf.Abs((float)corner2.sweepAngle)));
			}
		}

		private EdgeControl.EdgeCornerSweepValues GetCornerSweepValues(Vector2 p1, Vector2 cornerPoint, Vector2 p2, float diameter, Direction closestPortDirection)
		{
			EdgeControl.EdgeCornerSweepValues result = default(EdgeControl.EdgeCornerSweepValues);
			result.radius = diameter / 2f;
			Vector2 normalized = (cornerPoint - p1).normalized;
			Vector2 vector = normalized * diameter;
			float x = vector.x;
			float y = vector.y;
			Vector2 normalized2 = (cornerPoint - p2).normalized;
			Vector2 vector2 = normalized2 * diameter;
			float x2 = vector2.x;
			float y2 = vector2.y;
			float num = (float)(Math.Atan2((double)y, (double)x) - Math.Atan2((double)y2, (double)x2)) / 2f;
			float num2 = (float)Math.Abs(Math.Tan((double)num));
			float num3 = result.radius / num2;
			if (num3 > diameter)
			{
				num3 = diameter;
				result.radius = diameter * num2;
			}
			result.crossPoint1 = cornerPoint - normalized * num3;
			result.crossPoint2 = cornerPoint - normalized2 * num3;
			result.circleCenter = this.GetCornerCircleCenter(cornerPoint, result.crossPoint1, result.crossPoint2, num3, result.radius);
			result.startAngle = Math.Atan2((double)(result.crossPoint1.y - result.circleCenter.y), (double)(result.crossPoint1.x - result.circleCenter.x));
			result.endAngle = Math.Atan2((double)(result.crossPoint2.y - result.circleCenter.y), (double)(result.crossPoint2.x - result.circleCenter.x));
			result.sweepAngle = result.endAngle - result.startAngle;
			if (closestPortDirection == Direction.Input)
			{
				double endAngle = result.endAngle;
				result.endAngle = result.startAngle;
				result.startAngle = endAngle;
			}
			if (result.sweepAngle > 3.1415926535897931)
			{
				result.sweepAngle = -6.2831853071795862 + result.sweepAngle;
			}
			else if (result.sweepAngle < -3.1415926535897931)
			{
				result.sweepAngle = 6.2831853071795862 + result.sweepAngle;
			}
			return result;
		}

		private Vector2 GetCornerCircleCenter(Vector2 cornerPoint, Vector2 crossPoint1, Vector2 crossPoint2, float segment, float radius)
		{
			float x = cornerPoint.x * 2f - crossPoint1.x - crossPoint2.x;
			float y = cornerPoint.y * 2f - crossPoint1.y - crossPoint2.y;
			Vector2 vector = new Vector2(x, y);
			float magnitude = vector.magnitude;
			Vector2 vector2 = new Vector2(segment, radius);
			float magnitude2 = vector2.magnitude;
			float num = magnitude2 / magnitude;
			return new Vector2(cornerPoint.x - vector.x * num, cornerPoint.y - vector.y * num);
		}

		private void GetRoundedCornerPoints(List<Vector2> points, EdgeControl.EdgeCornerSweepValues corner, Direction closestPortDirection)
		{
			int num = Mathf.CeilToInt((float)Math.Abs(corner.sweepAngle * 4.0));
			int num2 = Math.Sign(corner.sweepAngle);
			bool flag = closestPortDirection == Direction.Input;
			int i = 0;
			while (i < num)
			{
				float num3 = (float)((!flag) ? i : (i - num));
				double num4 = corner.startAngle + (double)((float)num2 * num3 / 4f);
				float num5 = (float)((double)corner.circleCenter.x + Math.Cos(num4) * (double)corner.radius);
				float num6 = (float)((double)corner.circleCenter.y + Math.Sin(num4) * (double)corner.radius);
				if (i == 0 && flag)
				{
					if (this.orientation == Orientation.Horizontal)
					{
						if (corner.sweepAngle < 0.0 && points[points.Count - 1].y > num6)
						{
							goto IL_1B4;
						}
						if (corner.sweepAngle >= 0.0 && points[points.Count - 1].y < num6)
						{
							goto IL_1B4;
						}
					}
					else
					{
						if (corner.sweepAngle < 0.0 && points[points.Count - 1].x < num5)
						{
							goto IL_1B4;
						}
						if (corner.sweepAngle >= 0.0 && points[points.Count - 1].x > num5)
						{
							goto IL_1B4;
						}
					}
					goto IL_1A4;
				}
				goto IL_1A4;
				IL_1B4:
				i++;
				continue;
				IL_1A4:
				points.Add(new Vector2(num5, num6));
				goto IL_1B4;
			}
		}

		protected virtual void ComputeControlPoints()
		{
			if (this.m_ControlPointsDirty)
			{
				float num = 28f;
				float magnitude = (this.to - this.from).magnitude;
				num = Mathf.Min(num, magnitude * 2f);
				num = Mathf.Max(num, 16f);
				if (this.m_ControlPoints == null || this.m_ControlPoints.Length != 4)
				{
					this.m_ControlPoints = new Vector2[4];
				}
				this.m_ControlPoints[0] = this.from;
				if (this.orientation == Orientation.Horizontal)
				{
					this.m_ControlPoints[1] = new Vector2(this.from.x + num, this.from.y);
					this.m_ControlPoints[2] = new Vector2(this.to.x - num, this.to.y);
				}
				else
				{
					this.m_ControlPoints[1] = new Vector2(this.from.x, this.from.y + num);
					this.m_ControlPoints[2] = new Vector2(this.to.x, this.to.y - num);
				}
				this.m_ControlPoints[3] = this.to;
			}
		}

		private void ComputeLayout()
		{
			Vector2 lhs = this.m_ControlPoints[this.m_ControlPoints.Length - 1];
			Vector2 rhs = this.m_ControlPoints[0];
			Rect rect = new Rect(Vector2.Min(lhs, rhs), new Vector2(Mathf.Abs(rhs.x - lhs.x), Mathf.Abs(rhs.y - lhs.y)));
			for (int i = 1; i < this.m_ControlPoints.Length - 1; i++)
			{
				if (!rect.Contains(this.m_ControlPoints[i]))
				{
					Vector2 vector = this.m_ControlPoints[i];
					rect.xMin = Math.Min(rect.xMin, vector.x);
					rect.yMin = Math.Min(rect.yMin, vector.y);
					rect.xMax = Math.Max(rect.xMax, vector.x);
					rect.yMax = Math.Max(rect.yMax, vector.y);
				}
			}
			if (this.m_GraphView == null)
			{
				this.m_GraphView = base.GetFirstAncestorOfType<GraphView>();
			}
			float num = Mathf.Max((float)this.edgeWidth * 0.5f + 1f, 1.75f / this.m_GraphView.minScale);
			rect.xMin -= num;
			rect.yMin -= num;
			rect.width += num * 2f;
			rect.height += num * 2f;
			if (base.layout != rect)
			{
				base.layout = rect;
				this.m_RenderPointsDirty = true;
			}
		}

		protected virtual void DrawEdge()
		{
			if (this.edgeWidth > 0)
			{
				this.UpdateRenderPoints();
				Vector2[] controlPoints = this.controlPoints;
				Color inputColor = this.inputColor;
				Color outputColor = this.outputColor;
				float num = (float)this.edgeWidth;
				if (num * this.m_GraphView.scale < 1.75f)
				{
					num = 1.75f / this.m_GraphView.scale;
					inputColor.a = (outputColor.a = (float)this.edgeWidth / num);
				}
				if (this.m_MeshDirty || this.m_Mesh == null)
				{
					this.m_MeshDirty = false;
					int count = this.m_RenderPoints.Count;
					float num2 = 0f;
					for (int i = 1; i < count; i++)
					{
						num2 += (this.m_RenderPoints[i - 1] - this.m_RenderPoints[i]).magnitude;
					}
					if (this.m_Mesh == null)
					{
						this.m_Mesh = new Mesh();
						this.m_Mesh.hideFlags = HideFlags.HideAndDontSave;
					}
					Vector3[] array = this.m_Mesh.vertices;
					Vector2[] array2 = this.m_Mesh.uv;
					Vector3[] array3 = this.m_Mesh.normals;
					bool flag = false;
					int num3 = count * 2;
					if (array == null || array.Length != num3)
					{
						array = new Vector3[num3];
						array2 = new Vector2[num3];
						array3 = new Vector3[num3];
						flag = true;
						this.m_Mesh.triangles = new int[0];
					}
					float num4 = (float)this.edgeWidth * 0.5f;
					float num5 = num4 + 2f;
					float num6 = 0f;
					for (int j = 0; j < count; j++)
					{
						Vector2 vector;
						if (j > 0 && j < count - 1)
						{
							vector = (this.m_RenderPoints[j] - this.m_RenderPoints[j - 1]).normalized + (this.m_RenderPoints[j + 1] - this.m_RenderPoints[j]).normalized;
							vector.Normalize();
						}
						else if (j > 0)
						{
							vector = (this.m_RenderPoints[j] - this.m_RenderPoints[j - 1]).normalized;
						}
						else
						{
							vector = (this.m_RenderPoints[j + 1] - this.m_RenderPoints[j]).normalized;
						}
						Vector2 a = new Vector3(vector.y, -vector.x, 0f);
						Vector2 vector2 = -a * num5;
						array2[j * 2] = new Vector2(-num5, num4);
						array[j * 2] = this.m_RenderPoints[j];
						array3[j * 2] = new Vector3(-vector2.x, -vector2.y, num6 / num2);
						array2[j * 2 + 1] = new Vector2(num5, num4);
						array[j * 2 + 1] = this.m_RenderPoints[j];
						array3[j * 2 + 1] = new Vector3(vector2.x, vector2.y, num6 / num2);
						if (j < count - 2)
						{
							num6 += (this.m_RenderPoints[j + 1] - this.m_RenderPoints[j]).magnitude;
						}
						else
						{
							num6 = num2;
						}
					}
					this.m_Mesh.vertices = array;
					this.m_Mesh.normals = array3;
					this.m_Mesh.uv = array2;
					if (flag)
					{
						int[] array4 = new int[(num3 - 2) * 3];
						for (int k = 0; k < num3 - 2; k++)
						{
							if (k % 2 == 0)
							{
								array4[k * 3] = k;
								array4[k * 3 + 1] = k + 1;
								array4[k * 3 + 2] = k + 2;
							}
							else
							{
								array4[k * 3] = k + 1;
								array4[k * 3 + 1] = k;
								array4[k * 3 + 2] = k + 2;
							}
						}
						this.m_Mesh.triangles = array4;
					}
					this.m_Mesh.RecalculateBounds();
				}
				EdgeControl.lineMat.SetFloat("_ZoomFactor", this.m_GraphView.scale * num / (float)this.edgeWidth * EditorGUIUtility.pixelsPerPoint);
				EdgeControl.lineMat.SetFloat("_ZoomCorrection", num / (float)this.edgeWidth);
				EdgeControl.lineMat.SetColor("_InputColor", (QualitySettings.activeColorSpace != ColorSpace.Linear) ? inputColor : inputColor.gamma);
				EdgeControl.lineMat.SetColor("_OutputColor", (QualitySettings.activeColorSpace != ColorSpace.Linear) ? outputColor : outputColor.gamma);
				EdgeControl.lineMat.SetPass(0);
				Graphics.DrawMeshNow(this.m_Mesh, Matrix4x4.identity);
			}
		}

		private void OnLeavePanel(DetachFromPanelEvent e)
		{
			if (this.m_Mesh != null)
			{
				UnityEngine.Object.DestroyImmediate(this.m_Mesh);
				this.m_Mesh = null;
			}
		}
	}
}
