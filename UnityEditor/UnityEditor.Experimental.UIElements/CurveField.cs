using System;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.Experimental.UIElements.StyleSheets;

namespace UnityEditor.Experimental.UIElements
{
	public class CurveField : VisualElement, INotifyValueChanged<AnimationCurve>
	{
		private const string k_CurveColorProperty = "curve-color";

		private StyleValue<Color> m_CurveColor;

		private bool m_ValueNull;

		private AnimationCurve m_Value;

		private bool m_TextureDirty;

		public Rect ranges
		{
			get;
			set;
		}

		private Color curveColor
		{
			get
			{
				return this.m_CurveColor.GetSpecifiedValueOrDefault(Color.green);
			}
		}

		public AnimationCurve value
		{
			get
			{
				AnimationCurve result;
				if (this.m_ValueNull)
				{
					result = null;
				}
				else
				{
					result = new AnimationCurve
					{
						keys = this.m_Value.keys,
						preWrapMode = this.m_Value.preWrapMode,
						postWrapMode = this.m_Value.postWrapMode
					};
				}
				return result;
			}
			set
			{
				if (value != null || !this.m_ValueNull)
				{
					this.m_ValueNull = (value == null);
					if (!this.m_ValueNull)
					{
						this.m_Value.keys = value.keys;
						this.m_Value.preWrapMode = value.preWrapMode;
						this.m_Value.postWrapMode = value.postWrapMode;
					}
					else
					{
						this.m_Value.keys = new Keyframe[0];
						this.m_Value.preWrapMode = WrapMode.Once;
						this.m_Value.postWrapMode = WrapMode.Once;
					}
				}
				this.m_TextureDirty = true;
				base.Dirty(ChangeType.Repaint);
			}
		}

		public CurveField()
		{
			this.ranges = Rect.zero;
			VisualElement child = new VisualElement
			{
				name = "border",
				pickingMode = PickingMode.Ignore
			};
			base.Add(child);
			this.m_Value = new AnimationCurve(new Keyframe[0]);
		}

		private void OnDetach()
		{
			if (base.style.backgroundImage.value != null)
			{
				UnityEngine.Object.DestroyImmediate(base.style.backgroundImage.value);
				base.style.backgroundImage = null;
				this.m_TextureDirty = true;
			}
		}

		public void SetValueAndNotify(AnimationCurve newValue)
		{
			using (ChangeEvent<AnimationCurve> pooled = ChangeEvent<AnimationCurve>.GetPooled(this.value, newValue))
			{
				pooled.target = this;
				this.value = newValue;
				UIElementsUtility.eventDispatcher.DispatchEvent(pooled, base.panel);
			}
		}

		public void OnValueChanged(EventCallback<ChangeEvent<AnimationCurve>> callback)
		{
			base.RegisterCallback<ChangeEvent<AnimationCurve>>(callback, Capture.NoCapture);
		}

		protected override void OnStyleResolved(ICustomStyle style)
		{
			base.OnStyleResolved(style);
			style.ApplyCustomProperty("curve-color", ref this.m_CurveColor);
		}

		private void OnCurveClick()
		{
			if (base.enabledInHierarchy)
			{
				CurveEditorSettings settings = new CurveEditorSettings();
				if (this.m_Value == null)
				{
					this.m_Value = new AnimationCurve();
				}
				CurveEditorWindow.curve = this.m_Value;
				CurveEditorWindow.color = this.curveColor;
				CurveEditorWindow.instance.Show(new Action<AnimationCurve>(this.OnCurveChanged), settings);
			}
		}

		protected internal override void ExecuteDefaultAction(EventBase evt)
		{
			base.ExecuteDefaultAction(evt);
			if (evt.GetEventTypeId() == EventBase<MouseDownEvent>.TypeId())
			{
				this.OnCurveClick();
			}
			else if (evt.GetEventTypeId() == EventBase<DetachFromPanelEvent>.TypeId())
			{
				this.OnDetach();
			}
		}

		private void OnCurveChanged(AnimationCurve curve)
		{
			CurveEditorWindow.curve = this.m_Value;
			this.SetValueAndNotify(this.m_Value);
		}

		private void SendChangeEvent(AnimationCurve newValue)
		{
			using (ChangeEvent<AnimationCurve> pooled = ChangeEvent<AnimationCurve>.GetPooled(this.value, newValue))
			{
				pooled.target = this;
				this.value = newValue;
				UIElementsUtility.eventDispatcher.DispatchEvent(pooled, base.panel);
			}
		}

		public override void OnPersistentDataReady()
		{
			base.OnPersistentDataReady();
			this.m_TextureDirty = true;
		}

		public override void DoRepaint()
		{
			if (this.m_TextureDirty)
			{
				this.m_TextureDirty = false;
				int num = (int)base.layout.width;
				int num2 = (int)base.layout.height;
				Rect curveRanges = new Rect(0f, 0f, 1f, 1f);
				if (this.ranges.width > 0f && this.ranges.height > 0f)
				{
					curveRanges = this.ranges;
				}
				else if (!this.m_ValueNull && this.m_Value.keys.Length > 1)
				{
					float num3 = float.PositiveInfinity;
					float num4 = float.PositiveInfinity;
					float num5 = float.NegativeInfinity;
					float num6 = float.NegativeInfinity;
					for (int i = 0; i < this.m_Value.keys.Length; i++)
					{
						float value = this.m_Value.keys[i].value;
						float time = this.m_Value.keys[i].time;
						if (num3 > time)
						{
							num3 = time;
						}
						if (num5 < time)
						{
							num5 = time;
						}
						if (num4 > value)
						{
							num4 = value;
						}
						if (num6 < value)
						{
							num6 = value;
						}
					}
					if (num4 == num6)
					{
						num6 = num4 + 1f;
					}
					if (num3 == num5)
					{
						num5 = num3 + 1f;
					}
					curveRanges = Rect.MinMaxRect(num3, num4, num5, num6);
				}
				if (num2 > 0 && num > 0)
				{
					if (!this.m_ValueNull)
					{
						base.style.backgroundImage = AnimationCurvePreviewCache.GenerateCurvePreview(num, num2, curveRanges, this.m_Value, this.curveColor, base.style.backgroundImage.value);
					}
					else
					{
						base.style.backgroundImage = null;
					}
				}
			}
			base.DoRepaint();
		}
	}
}
