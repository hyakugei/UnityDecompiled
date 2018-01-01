using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace UnityEditor.Experimental.UIElements
{
	public abstract class TextValueField<T> : TextInputFieldBase, INotifyValueChanged<T>, IValueField<T>
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
				this.m_Value = value;
				this.text = this.ValueToString(this.m_Value);
			}
		}

		public string formatString
		{
			get;
			set;
		}

		protected TextValueField(int maxLength) : base(maxLength, '\0')
		{
		}

		public void OnValueChanged(EventCallback<ChangeEvent<T>> callback)
		{
			base.RegisterCallback<ChangeEvent<T>>(callback, Capture.NoCapture);
		}

		public void UpdateValueFromText()
		{
			T valueAndNotify = this.StringToValue(this.text);
			this.SetValueAndNotify(valueAndNotify);
		}

		public void SetValueAndNotify(T newValue)
		{
			if (!EqualityComparer<T>.Default.Equals(this.value, newValue))
			{
				using (ChangeEvent<T> pooled = ChangeEvent<T>.GetPooled(this.value, newValue))
				{
					pooled.target = this;
					this.value = newValue;
					UIElementsUtility.eventDispatcher.DispatchEvent(pooled, base.panel);
				}
			}
		}

		public abstract void ApplyInputDeviceDelta(Vector3 delta, DeltaSpeed speed, T startValue);

		protected abstract string ValueToString(T value);

		protected abstract T StringToValue(string str);

		protected internal override void ExecuteDefaultActionAtTarget(EventBase evt)
		{
			base.ExecuteDefaultActionAtTarget(evt);
			if (evt.GetEventTypeId() == EventBase<KeyDownEvent>.TypeId())
			{
				KeyDownEvent keyDownEvent = evt as KeyDownEvent;
				if (keyDownEvent.character == '\n')
				{
					this.UpdateValueFromText();
				}
			}
		}

		protected internal override void ExecuteDefaultAction(EventBase evt)
		{
			base.ExecuteDefaultAction(evt);
			if (evt.GetEventTypeId() == EventBase<BlurEvent>.TypeId())
			{
				this.UpdateValueFromText();
			}
		}
	}
}
