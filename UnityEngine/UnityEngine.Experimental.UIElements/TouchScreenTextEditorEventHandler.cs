using System;

namespace UnityEngine.Experimental.UIElements
{
	internal class TouchScreenTextEditorEventHandler : TextEditorEventHandler
	{
		private string m_SecureText;

		public string secureText
		{
			get
			{
				return this.m_SecureText;
			}
			set
			{
				string text = value ?? string.Empty;
				if (text != this.m_SecureText)
				{
					this.m_SecureText = text;
				}
			}
		}

		public TouchScreenTextEditorEventHandler(TextEditorEngine editorEngine, TextInputFieldBase textInputField) : base(editorEngine, textInputField)
		{
			this.secureText = string.Empty;
		}

		public override void ExecuteDefaultActionAtTarget(EventBase evt)
		{
			base.ExecuteDefaultActionAtTarget(evt);
			long num = EventBase<MouseDownEvent>.TypeId();
			if (evt.GetEventTypeId() == num)
			{
				base.textInputField.SyncTextEngine();
				base.textInputField.UpdateText(base.editorEngine.text);
				base.textInputField.TakeMouseCapture();
				base.editorEngine.keyboardOnScreen = TouchScreenKeyboard.Open(string.IsNullOrEmpty(this.secureText) ? base.textInputField.text : this.secureText, TouchScreenKeyboardType.Default, true, base.editorEngine.multiline, !string.IsNullOrEmpty(this.secureText));
				base.editorEngine.UpdateScrollOffset();
				evt.StopPropagation();
			}
		}
	}
}
