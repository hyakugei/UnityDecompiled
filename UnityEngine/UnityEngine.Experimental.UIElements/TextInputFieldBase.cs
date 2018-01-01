using System;
using UnityEngine.Experimental.UIElements.StyleSheets;

namespace UnityEngine.Experimental.UIElements
{
	public abstract class TextInputFieldBase : BaseTextElement
	{
		private const string SelectionColorProperty = "selection-color";

		private const string CursorColorProperty = "cursor-color";

		private StyleValue<Color> m_SelectionColor;

		private StyleValue<Color> m_CursorColor;

		internal const int kMaxLengthNone = -1;

		public virtual bool isPasswordField
		{
			get;
			set;
		}

		public Color selectionColor
		{
			get
			{
				return this.m_SelectionColor.GetSpecifiedValueOrDefault(Color.clear);
			}
		}

		public Color cursorColor
		{
			get
			{
				return this.m_CursorColor.GetSpecifiedValueOrDefault(Color.clear);
			}
		}

		public int maxLength
		{
			get;
			set;
		}

		public bool doubleClickSelectsWord
		{
			get;
			set;
		}

		public bool tripleClickSelectsLine
		{
			get;
			set;
		}

		private bool touchScreenTextField
		{
			get
			{
				return TouchScreenKeyboard.isSupported;
			}
		}

		internal bool hasFocus
		{
			get
			{
				return base.elementPanel != null && base.elementPanel.focusController.focusedElement == this;
			}
		}

		internal TextEditorEventHandler editorEventHandler
		{
			get;
			private set;
		}

		internal TextEditorEngine editorEngine
		{
			get;
			private set;
		}

		public char maskChar
		{
			get;
			set;
		}

		public override string text
		{
			set
			{
				base.text = value;
				this.editorEngine.text = value;
			}
		}

		public TextInputFieldBase(int maxLength, char maskChar)
		{
			this.maxLength = maxLength;
			this.maskChar = maskChar;
			this.editorEngine = new TextEditorEngine(this);
			if (this.touchScreenTextField)
			{
				this.editorEventHandler = new TouchScreenTextEditorEventHandler(this.editorEngine, this);
			}
			else
			{
				this.doubleClickSelectsWord = true;
				this.tripleClickSelectsLine = true;
				this.editorEventHandler = new KeyboardTextEditorEventHandler(this.editorEngine, this);
			}
			this.editorEngine.style = new GUIStyle(this.editorEngine.style);
			base.focusIndex = 0;
		}

		public void SelectAll()
		{
			if (this.editorEngine != null)
			{
				this.editorEngine.SelectAll();
			}
		}

		public void UpdateText(string value)
		{
			if (this.text != value)
			{
				using (InputEvent pooled = InputEvent.GetPooled(this.text, value))
				{
					pooled.target = this;
					this.text = value;
					UIElementsUtility.eventDispatcher.DispatchEvent(pooled, base.panel);
				}
			}
		}

		private ContextualMenu.MenuAction.StatusFlags CutCopyActionStatus(EventBase e)
		{
			return (!this.editorEngine.hasSelection || this.isPasswordField) ? ContextualMenu.MenuAction.StatusFlags.Disabled : ContextualMenu.MenuAction.StatusFlags.Normal;
		}

		private ContextualMenu.MenuAction.StatusFlags PasteActionStatus(EventBase e)
		{
			return (!this.editorEngine.CanPaste()) ? ContextualMenu.MenuAction.StatusFlags.Disabled : ContextualMenu.MenuAction.StatusFlags.Normal;
		}

		private void Cut(EventBase e)
		{
			this.editorEngine.Cut();
			this.editorEngine.text = this.CullString(this.editorEngine.text);
			this.UpdateText(this.editorEngine.text);
		}

		private void Copy(EventBase e)
		{
			this.editorEngine.Copy();
		}

		private void Paste(EventBase e)
		{
			this.editorEngine.Paste();
			this.editorEngine.text = this.CullString(this.editorEngine.text);
			this.UpdateText(this.editorEngine.text);
		}

		protected override void OnStyleResolved(ICustomStyle style)
		{
			base.OnStyleResolved(style);
			base.effectiveStyle.ApplyCustomProperty("selection-color", ref this.m_SelectionColor);
			base.effectiveStyle.ApplyCustomProperty("cursor-color", ref this.m_CursorColor);
			base.effectiveStyle.WriteToGUIStyle(this.editorEngine.style);
		}

		internal virtual void SyncTextEngine()
		{
			this.editorEngine.text = this.CullString(this.text);
			this.editorEngine.SaveBackup();
			this.editorEngine.position = base.layout;
			this.editorEngine.DetectFocusChange();
		}

		internal string CullString(string s)
		{
			string result;
			if (this.maxLength >= 0 && s != null && s.Length > this.maxLength)
			{
				result = s.Substring(0, this.maxLength);
			}
			else
			{
				result = s;
			}
			return result;
		}

		internal override void DoRepaint(IStylePainter painter)
		{
			if (this.touchScreenTextField)
			{
				TouchScreenTextEditorEventHandler touchScreenTextEditorEventHandler = this.editorEventHandler as TouchScreenTextEditorEventHandler;
				if (touchScreenTextEditorEventHandler != null && this.editorEngine.keyboardOnScreen != null)
				{
					this.UpdateText(this.CullString(this.editorEngine.keyboardOnScreen.text));
					if (this.editorEngine.keyboardOnScreen.status != TouchScreenKeyboard.Status.Visible)
					{
						this.editorEngine.keyboardOnScreen = null;
						GUI.changed = true;
					}
				}
				string text = this.text;
				if (touchScreenTextEditorEventHandler != null && !string.IsNullOrEmpty(touchScreenTextEditorEventHandler.secureText))
				{
					text = "".PadRight(touchScreenTextEditorEventHandler.secureText.Length, this.maskChar);
				}
				base.DoRepaint(painter);
				this.text = text;
			}
			else if (!this.hasFocus)
			{
				base.DoRepaint(painter);
			}
			else
			{
				this.DrawWithTextSelectionAndCursor(painter, this.text);
			}
		}

		internal void DrawWithTextSelectionAndCursor(IStylePainter painter, string newText)
		{
			KeyboardTextEditorEventHandler keyboardTextEditorEventHandler = this.editorEventHandler as KeyboardTextEditorEventHandler;
			if (keyboardTextEditorEventHandler != null)
			{
				keyboardTextEditorEventHandler.PreDrawCursor(newText);
				int cursorIndex = this.editorEngine.cursorIndex;
				int selectIndex = this.editorEngine.selectIndex;
				Rect localPosition = this.editorEngine.localPosition;
				Vector2 scrollOffset = this.editorEngine.scrollOffset;
				IStyle style = base.style;
				TextStylePainterParameters defaultTextParameters = painter.GetDefaultTextParameters(this);
				defaultTextParameters.text = " ";
				defaultTextParameters.wordWrapWidth = 0f;
				defaultTextParameters.wordWrap = false;
				float num = painter.ComputeTextHeight(defaultTextParameters);
				float num2 = (!style.wordWrap) ? 0f : base.contentRect.width;
				Input.compositionCursorPos = this.editorEngine.graphicalCursorPos - scrollOffset + new Vector2(localPosition.x, localPosition.y + num);
				Color specifiedValueOrDefault = this.m_CursorColor.GetSpecifiedValueOrDefault(Color.grey);
				int num3 = (!string.IsNullOrEmpty(Input.compositionString)) ? (cursorIndex + Input.compositionString.Length) : selectIndex;
				painter.DrawBackground(this);
				if (cursorIndex != num3)
				{
					RectStylePainterParameters defaultRectParameters = painter.GetDefaultRectParameters(this);
					defaultRectParameters.color = this.selectionColor;
					defaultRectParameters.border.SetWidth(0f);
					defaultRectParameters.border.SetRadius(0f);
					int cursorIndex2 = (cursorIndex >= num3) ? num3 : cursorIndex;
					int cursorIndex3 = (cursorIndex <= num3) ? num3 : cursorIndex;
					CursorPositionStylePainterParameters defaultCursorPositionParameters = painter.GetDefaultCursorPositionParameters(this);
					defaultCursorPositionParameters.text = this.editorEngine.text;
					defaultCursorPositionParameters.wordWrapWidth = num2;
					defaultCursorPositionParameters.cursorIndex = cursorIndex2;
					Vector2 a = painter.GetCursorPosition(defaultCursorPositionParameters);
					defaultCursorPositionParameters.cursorIndex = cursorIndex3;
					Vector2 a2 = painter.GetCursorPosition(defaultCursorPositionParameters);
					a -= scrollOffset;
					a2 -= scrollOffset;
					if (Mathf.Approximately(a.y, a2.y))
					{
						defaultRectParameters.rect = new Rect(a.x, a.y, a2.x - a.x, num);
						painter.DrawRect(defaultRectParameters);
					}
					else
					{
						defaultRectParameters.rect = new Rect(a.x, a.y, num2 - a.x, num);
						painter.DrawRect(defaultRectParameters);
						float num4 = a2.y - a.y - num;
						if (num4 > 0f)
						{
							defaultRectParameters.rect = new Rect(0f, a.y + num, num2, num4);
							painter.DrawRect(defaultRectParameters);
						}
						defaultRectParameters.rect = new Rect(0f, a2.y, a2.x, num);
						painter.DrawRect(defaultRectParameters);
					}
				}
				painter.DrawBorder(this);
				if (!string.IsNullOrEmpty(this.editorEngine.text) && base.contentRect.width > 0f && base.contentRect.height > 0f)
				{
					defaultTextParameters = painter.GetDefaultTextParameters(this);
					defaultTextParameters.rect = new Rect(base.contentRect.x - scrollOffset.x, base.contentRect.y - scrollOffset.y, base.contentRect.width, base.contentRect.height);
					defaultTextParameters.text = this.editorEngine.text;
					painter.DrawText(defaultTextParameters);
				}
				if (cursorIndex == num3 && style.font != null)
				{
					CursorPositionStylePainterParameters defaultCursorPositionParameters = painter.GetDefaultCursorPositionParameters(this);
					defaultCursorPositionParameters.text = this.editorEngine.text;
					defaultCursorPositionParameters.wordWrapWidth = num2;
					defaultCursorPositionParameters.cursorIndex = cursorIndex;
					Vector2 a3 = painter.GetCursorPosition(defaultCursorPositionParameters);
					a3 -= scrollOffset;
					RectStylePainterParameters painterParams = new RectStylePainterParameters
					{
						rect = new Rect(a3.x, a3.y, 1f, num),
						color = specifiedValueOrDefault
					};
					painter.DrawRect(painterParams);
				}
				if (this.editorEngine.altCursorPosition != -1)
				{
					CursorPositionStylePainterParameters defaultCursorPositionParameters = painter.GetDefaultCursorPositionParameters(this);
					defaultCursorPositionParameters.text = this.editorEngine.text.Substring(0, this.editorEngine.altCursorPosition);
					defaultCursorPositionParameters.wordWrapWidth = num2;
					defaultCursorPositionParameters.cursorIndex = this.editorEngine.altCursorPosition;
					Vector2 a4 = painter.GetCursorPosition(defaultCursorPositionParameters);
					a4 -= scrollOffset;
					RectStylePainterParameters painterParams2 = new RectStylePainterParameters
					{
						rect = new Rect(a4.x, a4.y, 1f, num),
						color = specifiedValueOrDefault
					};
					painter.DrawRect(painterParams2);
				}
				keyboardTextEditorEventHandler.PostDrawCursor();
			}
		}

		internal virtual bool AcceptCharacter(char c)
		{
			return true;
		}

		protected virtual void BuildContextualMenu(ContextualMenuPopulateEvent evt)
		{
			if (evt.target is TextInputFieldBase)
			{
				evt.menu.AppendAction("Cut", new Action<EventBase>(this.Cut), new Func<EventBase, ContextualMenu.MenuAction.StatusFlags>(this.CutCopyActionStatus));
				evt.menu.AppendAction("Copy", new Action<EventBase>(this.Copy), new Func<EventBase, ContextualMenu.MenuAction.StatusFlags>(this.CutCopyActionStatus));
				evt.menu.AppendAction("Paste", new Action<EventBase>(this.Paste), new Func<EventBase, ContextualMenu.MenuAction.StatusFlags>(this.PasteActionStatus));
			}
		}

		protected internal override void ExecuteDefaultActionAtTarget(EventBase evt)
		{
			base.ExecuteDefaultActionAtTarget(evt);
			if (base.elementPanel != null && base.elementPanel.contextualMenuManager != null)
			{
				base.elementPanel.contextualMenuManager.DisplayMenuIfEventMatches(evt, this);
			}
			if (evt.GetEventTypeId() == EventBase<ContextualMenuPopulateEvent>.TypeId())
			{
				ContextualMenuPopulateEvent contextualMenuPopulateEvent = evt as ContextualMenuPopulateEvent;
				int count = contextualMenuPopulateEvent.menu.MenuItems().Count;
				this.BuildContextualMenu(contextualMenuPopulateEvent);
				if (count > 0 && contextualMenuPopulateEvent.menu.MenuItems().Count > count)
				{
					contextualMenuPopulateEvent.menu.InsertSeparator(count);
				}
			}
			this.editorEventHandler.ExecuteDefaultActionAtTarget(evt);
		}

		protected internal override void ExecuteDefaultAction(EventBase evt)
		{
			base.ExecuteDefaultAction(evt);
			this.editorEventHandler.ExecuteDefaultAction(evt);
		}
	}
}
