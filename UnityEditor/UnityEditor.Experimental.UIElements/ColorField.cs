using System;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace UnityEditor.Experimental.UIElements
{
	public class ColorField : VisualElement, INotifyValueChanged<Color>
	{
		private Color m_Value;

		private bool m_SetKbControl;

		public Color value
		{
			get
			{
				return this.m_Value;
			}
			set
			{
				this.m_Value = value;
				base.Dirty(ChangeType.Repaint);
			}
		}

		public bool showEyeDropper
		{
			get;
			set;
		}

		public bool showAlpha
		{
			get;
			set;
		}

		public bool hdr
		{
			get;
			set;
		}

		public ColorField()
		{
			this.showEyeDropper = true;
			this.showAlpha = true;
			IMGUIContainer child = new IMGUIContainer(new Action(this.OnGUIHandler))
			{
				name = "InternalColorField"
			};
			base.Add(child);
		}

		public void SetValueAndNotify(Color newValue)
		{
			if (this.value != newValue)
			{
				using (ChangeEvent<Color> pooled = ChangeEvent<Color>.GetPooled(this.value, newValue))
				{
					pooled.target = this;
					this.value = newValue;
					UIElementsUtility.eventDispatcher.DispatchEvent(pooled, base.panel);
				}
			}
		}

		public void OnValueChanged(EventCallback<ChangeEvent<Color>> callback)
		{
			base.RegisterCallback<ChangeEvent<Color>>(callback, Capture.NoCapture);
		}

		protected internal override void ExecuteDefaultAction(EventBase evt)
		{
			base.ExecuteDefaultAction(evt);
			if (evt.GetEventTypeId() == EventBase<FocusEvent>.TypeId())
			{
				this.m_SetKbControl = true;
			}
		}

		private void OnGUIHandler()
		{
			if (Event.current.type == EventType.ExecuteCommand && Event.current.commandName == "EyeDropperUpdate")
			{
				base.Dirty(ChangeType.Repaint);
			}
			Color valueAndNotify = EditorGUILayout.ColorField(GUIContent.none, this.value, this.showEyeDropper, this.showAlpha, this.hdr, new GUILayoutOption[0]);
			this.SetValueAndNotify(valueAndNotify);
			if (this.m_SetKbControl)
			{
				GUIUtility.SetKeyboardControlToFirstControlId();
				this.m_SetKbControl = false;
			}
		}
	}
}
