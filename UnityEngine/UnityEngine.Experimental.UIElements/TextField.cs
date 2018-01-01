using System;
using System.Collections.Generic;

namespace UnityEngine.Experimental.UIElements
{
	public class TextField : TextInputFieldBase, INotifyValueChanged<string>
	{
		private bool m_Multiline;

		protected string m_Value;

		public bool multiline
		{
			get
			{
				return this.m_Multiline;
			}
			set
			{
				this.m_Multiline = value;
				if (!value)
				{
					this.text = this.text.Replace("\n", "");
				}
			}
		}

		public override bool isPasswordField
		{
			set
			{
				base.isPasswordField = value;
				if (value)
				{
					this.multiline = false;
				}
			}
		}

		public string value
		{
			get
			{
				return this.m_Value;
			}
			set
			{
				this.m_Value = value;
				this.text = this.m_Value;
			}
		}

		public TextField() : this(-1, false, false, '\0')
		{
		}

		public TextField(int maxLength, bool multiline, bool isPasswordField, char maskChar) : base(maxLength, maskChar)
		{
			this.multiline = multiline;
			this.isPasswordField = isPasswordField;
		}

		public void SetValueAndNotify(string newValue)
		{
			if (!EqualityComparer<string>.Default.Equals(this.value, newValue))
			{
				using (ChangeEvent<string> pooled = ChangeEvent<string>.GetPooled(this.value, newValue))
				{
					pooled.target = this;
					this.value = newValue;
					UIElementsUtility.eventDispatcher.DispatchEvent(pooled, base.panel);
				}
			}
		}

		public void OnValueChanged(EventCallback<ChangeEvent<string>> callback)
		{
			base.RegisterCallback<ChangeEvent<string>>(callback, Capture.NoCapture);
		}

		public override void OnPersistentDataReady()
		{
			base.OnPersistentDataReady();
			string fullHierarchicalPersistenceKey = base.GetFullHierarchicalPersistenceKey();
			base.OverwriteFromPersistedData(this, fullHierarchicalPersistenceKey);
		}

		internal override void SyncTextEngine()
		{
			base.editorEngine.multiline = this.multiline;
			base.editorEngine.isPasswordField = this.isPasswordField;
			base.SyncTextEngine();
		}

		internal override void DoRepaint(IStylePainter painter)
		{
			if (this.isPasswordField)
			{
				string text = "".PadRight(this.text.Length, base.maskChar);
				if (!base.hasFocus)
				{
					painter.DrawBackground(this);
					painter.DrawBorder(this);
					if (!string.IsNullOrEmpty(text) && base.contentRect.width > 0f && base.contentRect.height > 0f)
					{
						TextStylePainterParameters defaultTextParameters = painter.GetDefaultTextParameters(this);
						defaultTextParameters.text = text;
						painter.DrawText(defaultTextParameters);
					}
				}
				else
				{
					base.DrawWithTextSelectionAndCursor(painter, text);
				}
			}
			else
			{
				base.DoRepaint(painter);
			}
		}

		protected internal override void ExecuteDefaultActionAtTarget(EventBase evt)
		{
			base.ExecuteDefaultActionAtTarget(evt);
			if (evt.GetEventTypeId() == EventBase<KeyDownEvent>.TypeId())
			{
				KeyDownEvent keyDownEvent = evt as KeyDownEvent;
				if (keyDownEvent.character == '\n')
				{
					this.SetValueAndNotify(this.text);
				}
			}
		}

		protected internal override void ExecuteDefaultAction(EventBase evt)
		{
			base.ExecuteDefaultAction(evt);
			if (evt.GetEventTypeId() == EventBase<BlurEvent>.TypeId())
			{
				this.SetValueAndNotify(this.text);
			}
		}
	}
}
