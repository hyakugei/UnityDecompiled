using System;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace UnityEditor.Experimental.UIElements
{
	public class GradientField : VisualElement, INotifyValueChanged<Gradient>
	{
		private bool m_ValueNull;

		private Gradient m_Value;

		public Gradient value
		{
			get
			{
				Gradient result;
				if (this.m_ValueNull)
				{
					result = null;
				}
				else
				{
					Gradient gradient = new Gradient();
					gradient.colorKeys = this.m_Value.colorKeys;
					gradient.alphaKeys = this.m_Value.alphaKeys;
					gradient.mode = this.m_Value.mode;
					result = this.m_Value;
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
						this.m_Value.colorKeys = value.colorKeys;
						this.m_Value.alphaKeys = value.alphaKeys;
						this.m_Value.mode = value.mode;
					}
					else
					{
						this.m_Value.colorKeys = new GradientColorKey[]
						{
							new GradientColorKey(Color.white, 0f),
							new GradientColorKey(Color.white, 1f)
						};
						this.m_Value.alphaKeys = new GradientAlphaKey[]
						{
							new GradientAlphaKey(1f, 0f),
							new GradientAlphaKey(1f, 1f)
						};
						this.m_Value.mode = GradientMode.Blend;
					}
				}
				this.UpdateGradientTexture();
			}
		}

		public GradientField()
		{
			VisualElement child = new VisualElement
			{
				name = "border",
				pickingMode = PickingMode.Ignore
			};
			base.Add(child);
			this.m_Value = new Gradient();
		}

		protected internal override void ExecuteDefaultAction(EventBase evt)
		{
			base.ExecuteDefaultAction(evt);
			if (evt.GetEventTypeId() == EventBase<MouseDownEvent>.TypeId())
			{
				this.OnClick();
			}
			else if (evt.GetEventTypeId() == EventBase<DetachFromPanelEvent>.TypeId())
			{
				this.OnDetach();
			}
			else if (evt.GetEventTypeId() == EventBase<AttachToPanelEvent>.TypeId())
			{
				this.OnAttach();
			}
		}

		private void OnDetach()
		{
			if (base.style.backgroundImage.value != null)
			{
				UnityEngine.Object.DestroyImmediate(base.style.backgroundImage.value);
				base.style.backgroundImage = null;
			}
		}

		private void OnAttach()
		{
			this.UpdateGradientTexture();
		}

		private void OnClick()
		{
			GradientPicker.Show(this.m_Value, true, new Action<Gradient>(this.OnGradientChanged));
		}

		public override void OnPersistentDataReady()
		{
			base.OnPersistentDataReady();
			this.UpdateGradientTexture();
		}

		private void UpdateGradientTexture()
		{
			if (this.m_ValueNull)
			{
				base.style.backgroundImage = null;
			}
			else
			{
				Texture2D value = GradientPreviewCache.GenerateGradientPreview(this.value, base.style.backgroundImage.value);
				base.style.backgroundImage = value;
			}
		}

		private void OnGradientChanged(Gradient newValue)
		{
			this.SetValueAndNotify(newValue);
			GradientPreviewCache.ClearCache();
			base.Dirty(ChangeType.Repaint);
		}

		public void SetValueAndNotify(Gradient newValue)
		{
			using (ChangeEvent<Gradient> pooled = ChangeEvent<Gradient>.GetPooled(this.value, newValue))
			{
				pooled.target = this;
				this.value = newValue;
				UIElementsUtility.eventDispatcher.DispatchEvent(pooled, base.panel);
			}
		}

		public void OnValueChanged(EventCallback<ChangeEvent<Gradient>> callback)
		{
			base.RegisterCallback<ChangeEvent<Gradient>>(callback, Capture.NoCapture);
		}

		public override void DoRepaint()
		{
			Texture2D backgroundTexture = GradientEditor.GetBackgroundTexture();
			IStylePainter stylePainter = base.elementPanel.stylePainter;
			TextureStylePainterParameters defaultTextureParameters = stylePainter.GetDefaultTextureParameters(this);
			defaultTextureParameters.texture = backgroundTexture;
			stylePainter.DrawTexture(defaultTextureParameters);
			base.DoRepaint();
		}
	}
}
