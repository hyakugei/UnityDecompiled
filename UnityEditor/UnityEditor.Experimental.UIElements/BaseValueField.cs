using System;
using System.Collections.Generic;
using UnityEngine.Experimental.UIElements;

namespace UnityEditor.Experimental.UIElements
{
	public abstract class BaseValueField<T> : VisualElement, INotifyValueChanged<T>
	{
		protected T m_Value;

		public T value
		{
			get
			{
				return this.m_Value;
			}
			set
			{
				if (!EqualityComparer<T>.Default.Equals(this.m_Value, value))
				{
					this.m_Value = value;
					this.UpdateDisplay();
				}
			}
		}

		protected abstract void UpdateDisplay();

		public void SetValueAndNotify(T newValue)
		{
			if (!EqualityComparer<T>.Default.Equals(this.m_Value, newValue))
			{
				using (ChangeEvent<T> pooled = ChangeEvent<T>.GetPooled(this.value, newValue))
				{
					pooled.target = this;
					this.value = newValue;
					UIElementsUtility.eventDispatcher.DispatchEvent(pooled, base.panel);
				}
			}
		}

		public void OnValueChanged(EventCallback<ChangeEvent<T>> callback)
		{
			base.RegisterCallback<ChangeEvent<T>>(callback, Capture.NoCapture);
		}
	}
}
