using System;
using System.Collections.Generic;

namespace UnityEngine.Experimental.UIElements
{
	public class ContextualMenu
	{
		public abstract class MenuItem
		{
		}

		public class Separator : ContextualMenu.MenuItem
		{
		}

		public class MenuAction : ContextualMenu.MenuItem
		{
			[Flags]
			public enum StatusFlags
			{
				Normal = 0,
				Disabled = 1,
				Checked = 2,
				Hidden = 4
			}

			public string name;

			private Action<EventBase> actionCallback;

			private Func<EventBase, ContextualMenu.MenuAction.StatusFlags> actionStatusCallback;

			public ContextualMenu.MenuAction.StatusFlags status
			{
				get;
				private set;
			}

			public MenuAction(string actionName, Action<EventBase> actionCallback, Func<EventBase, ContextualMenu.MenuAction.StatusFlags> actionStatusCallback)
			{
				this.name = actionName;
				this.actionCallback = actionCallback;
				this.actionStatusCallback = actionStatusCallback;
			}

			public static ContextualMenu.MenuAction.StatusFlags AlwaysEnabled(EventBase e)
			{
				return ContextualMenu.MenuAction.StatusFlags.Normal;
			}

			public static ContextualMenu.MenuAction.StatusFlags AlwaysDisabled(EventBase e)
			{
				return ContextualMenu.MenuAction.StatusFlags.Disabled;
			}

			public void UpdateActionStatus(EventBase e)
			{
				this.status = ((this.actionStatusCallback == null) ? ContextualMenu.MenuAction.StatusFlags.Hidden : this.actionStatusCallback(e));
			}

			public void Execute(EventBase e)
			{
				if (this.actionCallback != null)
				{
					this.actionCallback(e);
				}
			}
		}

		private List<ContextualMenu.MenuItem> menuItems = new List<ContextualMenu.MenuItem>();

		public List<ContextualMenu.MenuItem> MenuItems()
		{
			return this.menuItems;
		}

		public void AppendAction(string actionName, Action<EventBase> action, Func<EventBase, ContextualMenu.MenuAction.StatusFlags> actionStatusCallback)
		{
			ContextualMenu.MenuAction item = new ContextualMenu.MenuAction(actionName, action, actionStatusCallback);
			this.menuItems.Add(item);
		}

		public void InsertAction(string actionName, Action<EventBase> action, Func<EventBase, ContextualMenu.MenuAction.StatusFlags> actionStatusCallback, int atIndex)
		{
			ContextualMenu.MenuAction item = new ContextualMenu.MenuAction(actionName, action, actionStatusCallback);
			this.menuItems.Insert(atIndex, item);
		}

		public void AppendSeparator()
		{
			if (this.menuItems.Count > 0 && !(this.menuItems[this.menuItems.Count - 1] is ContextualMenu.Separator))
			{
				ContextualMenu.Separator item = new ContextualMenu.Separator();
				this.menuItems.Add(item);
			}
		}

		public void InsertSeparator(int atIndex)
		{
			if (atIndex > 0 && atIndex <= this.menuItems.Count && !(this.menuItems[atIndex - 1] is ContextualMenu.Separator))
			{
				ContextualMenu.Separator item = new ContextualMenu.Separator();
				this.menuItems.Insert(atIndex, item);
			}
		}

		public void PrepareForDisplay(EventBase e)
		{
			foreach (ContextualMenu.MenuItem current in this.menuItems)
			{
				ContextualMenu.MenuAction menuAction = current as ContextualMenu.MenuAction;
				if (menuAction != null)
				{
					menuAction.UpdateActionStatus(e);
				}
			}
			if (this.menuItems[this.menuItems.Count - 1] is ContextualMenu.Separator)
			{
				this.menuItems.RemoveAt(this.menuItems.Count - 1);
			}
		}
	}
}
