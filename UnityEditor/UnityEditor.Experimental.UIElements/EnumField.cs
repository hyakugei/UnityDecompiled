using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace UnityEditor.Experimental.UIElements
{
	public class EnumField : BaseTextElement, INotifyValueChanged<Enum>
	{
		private Type m_EnumType;

		private Enum m_Value;

		public Enum value
		{
			get
			{
				return this.m_Value;
			}
			set
			{
				if (this.m_Value != value)
				{
					this.m_Value = value;
					this.text = this.m_Value.ToString();
					base.Dirty(ChangeType.Repaint);
				}
			}
		}

		public EnumField(Enum defaultValue)
		{
			this.m_EnumType = defaultValue.GetType();
			this.value = defaultValue;
		}

		public void SetValueAndNotify(Enum newValue)
		{
			if (this.value != newValue)
			{
				using (ChangeEvent<Enum> pooled = ChangeEvent<Enum>.GetPooled(this.value, newValue))
				{
					pooled.target = this;
					this.value = newValue;
					UIElementsUtility.eventDispatcher.DispatchEvent(pooled, base.panel);
				}
			}
		}

		public void OnValueChanged(EventCallback<ChangeEvent<Enum>> callback)
		{
			base.RegisterCallback<ChangeEvent<Enum>>(callback, Capture.NoCapture);
		}

		protected internal override void ExecuteDefaultAction(EventBase evt)
		{
			base.ExecuteDefaultAction(evt);
			if (evt.GetEventTypeId() == EventBase<MouseDownEvent>.TypeId())
			{
				this.OnMouseDown();
			}
		}

		private void OnMouseDown()
		{
			GenericMenu genericMenu = new GenericMenu();
			IEnumerator enumerator = Enum.GetValues(this.m_EnumType).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					Enum @enum = (Enum)enumerator.Current;
					bool on = @enum.CompareTo(this.value) == 0;
					genericMenu.AddItem(new GUIContent(@enum.ToString()), on, delegate(object contentView)
					{
						this.ChangeValueFromMenu(contentView);
					}, @enum);
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
			Vector2 vector = new Vector2(0f, base.layout.height);
			vector = this.LocalToWorld(vector);
			Rect position = new Rect(vector, Vector2.zero);
			genericMenu.DropDown(position);
		}

		private void ChangeValueFromMenu(object menuItem)
		{
			this.SetValueAndNotify(menuItem as Enum);
		}
	}
}
