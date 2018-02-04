using System;

namespace UnityEngine.Experimental.UIElements
{
	internal class TextEditorEngine : TextEditor
	{
		private TextInputFieldBase textInputField
		{
			get;
			set;
		}

		internal override Rect localPosition
		{
			get
			{
				return new Rect(0f, 0f, base.position.width, base.position.height);
			}
		}

		public TextEditorEngine(TextInputFieldBase field)
		{
			this.textInputField = field;
		}

		internal override void OnDetectFocusChange()
		{
			if (this.m_HasFocus && !this.textInputField.hasFocus)
			{
				base.OnFocus();
			}
			if (!this.m_HasFocus && this.textInputField.hasFocus)
			{
				base.OnLostFocus();
			}
		}

		internal override void OnCursorIndexChange()
		{
			this.textInputField.Dirty(ChangeType.Repaint);
		}

		internal override void OnSelectIndexChange()
		{
			this.textInputField.Dirty(ChangeType.Repaint);
		}
	}
}
