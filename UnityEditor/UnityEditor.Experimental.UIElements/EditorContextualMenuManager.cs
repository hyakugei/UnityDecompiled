using System;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace UnityEditor.Experimental.UIElements
{
	internal class EditorContextualMenuManager : ContextualMenuManager
	{
		public override void DisplayMenuIfEventMatches(EventBase evt, IEventHandler eventHandler)
		{
			if (evt.GetEventTypeId() == EventBase<MouseUpEvent>.TypeId())
			{
				MouseUpEvent mouseUpEvent = evt as MouseUpEvent;
				if (mouseUpEvent.button == 1)
				{
					base.DisplayMenu(evt, eventHandler);
					evt.StopPropagation();
				}
			}
			else if (evt.GetEventTypeId() == EventBase<KeyUpEvent>.TypeId())
			{
				KeyUpEvent keyUpEvent = evt as KeyUpEvent;
				if (keyUpEvent.keyCode == KeyCode.Menu)
				{
					base.DisplayMenu(evt, eventHandler);
					evt.StopPropagation();
				}
			}
		}

		protected override void DoDisplayMenu(ContextualMenu menu, EventBase triggerEvent)
		{
			GenericMenu genericMenu = new GenericMenu();
			foreach (ContextualMenu.MenuItem current in menu.MenuItems())
			{
				ContextualMenu.MenuAction action = current as ContextualMenu.MenuAction;
				if (action != null)
				{
					if ((action.status & ContextualMenu.MenuAction.StatusFlags.Hidden) != ContextualMenu.MenuAction.StatusFlags.Hidden)
					{
						bool on = (action.status & ContextualMenu.MenuAction.StatusFlags.Checked) == ContextualMenu.MenuAction.StatusFlags.Checked;
						if ((action.status & ContextualMenu.MenuAction.StatusFlags.Disabled) == ContextualMenu.MenuAction.StatusFlags.Disabled)
						{
							genericMenu.AddDisabledItem(new GUIContent(action.name));
						}
						else
						{
							genericMenu.AddItem(new GUIContent(action.name), on, delegate
							{
								action.Execute(triggerEvent);
							});
						}
					}
				}
				else
				{
					genericMenu.AddSeparator(string.Empty);
				}
			}
			Vector2 position = Vector2.zero;
			if (triggerEvent is IMouseEvent)
			{
				position = ((IMouseEvent)triggerEvent).mousePosition;
			}
			else if (triggerEvent.target is VisualElement)
			{
				position = ((VisualElement)triggerEvent.target).layout.center;
			}
			genericMenu.DropDown(new Rect(position, Vector2.zero));
		}
	}
}
