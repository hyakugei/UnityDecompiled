using System;

namespace UnityEngine.Experimental.UIElements
{
	internal class ContextualMenuManipulator : MouseManipulator
	{
		private Action<ContextualMenuPopulateEvent> m_MenuBuilder;

		public ContextualMenuManipulator(Action<ContextualMenuPopulateEvent> menuBuilder)
		{
			this.m_MenuBuilder = menuBuilder;
			base.activators.Add(new ManipulatorActivationFilter
			{
				button = MouseButton.RightMouse
			});
		}

		protected override void RegisterCallbacksOnTarget()
		{
			base.target.RegisterCallback<MouseUpEvent>(new EventCallback<MouseUpEvent>(this.OnMouseUpEvent), Capture.NoCapture);
			base.target.RegisterCallback<KeyUpEvent>(new EventCallback<KeyUpEvent>(this.OnKeyUpEvent), Capture.NoCapture);
			base.target.RegisterCallback<ContextualMenuPopulateEvent>(new EventCallback<ContextualMenuPopulateEvent>(this.OnContextualMenuEvent), Capture.NoCapture);
		}

		protected override void UnregisterCallbacksFromTarget()
		{
			base.target.UnregisterCallback<MouseUpEvent>(new EventCallback<MouseUpEvent>(this.OnMouseUpEvent), Capture.NoCapture);
			base.target.UnregisterCallback<KeyUpEvent>(new EventCallback<KeyUpEvent>(this.OnKeyUpEvent), Capture.NoCapture);
			base.target.UnregisterCallback<ContextualMenuPopulateEvent>(new EventCallback<ContextualMenuPopulateEvent>(this.OnContextualMenuEvent), Capture.NoCapture);
		}

		private void OnMouseUpEvent(MouseUpEvent evt)
		{
			if (base.CanStartManipulation(evt))
			{
				if (base.target.elementPanel != null && base.target.elementPanel.contextualMenuManager != null)
				{
					base.target.elementPanel.contextualMenuManager.DisplayMenu(evt, base.target);
					evt.StopPropagation();
					evt.PreventDefault();
				}
			}
		}

		private void OnKeyUpEvent(KeyUpEvent evt)
		{
			if (evt.keyCode == KeyCode.Menu)
			{
				if (base.target.elementPanel != null && base.target.elementPanel.contextualMenuManager != null)
				{
					base.target.elementPanel.contextualMenuManager.DisplayMenu(evt, base.target);
					evt.StopPropagation();
					evt.PreventDefault();
				}
			}
		}

		private void OnContextualMenuEvent(ContextualMenuPopulateEvent evt)
		{
			if (this.m_MenuBuilder != null)
			{
				this.m_MenuBuilder(evt);
			}
		}
	}
}
