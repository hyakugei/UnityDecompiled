using System;
using UnityEngine;

namespace UnityEditor
{
	internal class MaximizedHostView : HostView
	{
		protected override void OldOnGUI()
		{
			base.ClearBackground();
			EditorGUIUtility.ResetGUIState();
			Rect rect = new Rect(-2f, 0f, base.position.width + 4f, base.position.height);
			this.background = "dockarea";
			rect = this.background.margin.Remove(rect);
			Rect position = new Rect(rect.x + 1f, rect.y, rect.width - 2f, 17f);
			if (Event.current.type == EventType.Repaint)
			{
				this.background.Draw(rect, GUIContent.none, false, false, false, false);
				GUIStyle gUIStyle = "dragTab";
				gUIStyle.Draw(position, base.actualView.titleContent, false, false, true, base.hasFocus);
			}
			if (Event.current.type == EventType.ContextClick && position.Contains(Event.current.mousePosition))
			{
				base.PopupGenericMenu(base.actualView, new Rect(Event.current.mousePosition.x, Event.current.mousePosition.y, 0f, 0f));
			}
			base.ShowGenericMenu();
			if (base.actualView)
			{
				base.actualView.m_Pos = base.borderSize.Remove(base.screenPosition);
			}
			base.InvokeOnGUI(rect);
		}

		protected override RectOffset GetBorderSize()
		{
			this.m_BorderSize.left = 0;
			this.m_BorderSize.right = 0;
			this.m_BorderSize.top = 17;
			this.m_BorderSize.bottom = 4;
			return this.m_BorderSize;
		}

		private void Unmaximize(object userData)
		{
			EditorWindow win = (EditorWindow)userData;
			WindowLayout.Unmaximize(win);
		}

		protected override void AddDefaultItemsToMenu(GenericMenu menu, EditorWindow window)
		{
			base.AddDefaultItemsToMenu(menu, window);
			menu.AddItem(EditorGUIUtility.TrTextContent("Maximize", null, null), !(base.parent is SplitView), new GenericMenu.MenuFunction2(this.Unmaximize), window);
			menu.AddDisabledItem(EditorGUIUtility.TrTextContent("Close Tab", null, null));
			menu.AddSeparator("");
			Type[] paneTypes = base.GetPaneTypes();
			GUIContent gUIContent = EditorGUIUtility.TrTextContent("Add Tab", null, null);
			Type[] array = paneTypes;
			for (int i = 0; i < array.Length; i++)
			{
				Type type = array[i];
				if (type != null)
				{
					GUIContent gUIContent2 = new GUIContent(EditorWindow.GetLocalizedTitleContentFromType(type));
					gUIContent2.text = gUIContent.text + "/" + gUIContent2.text;
					menu.AddDisabledItem(gUIContent2);
				}
			}
		}
	}
}
