using System;
using UnityEngine;

namespace UnityEditor
{
	internal class ComponentDropdownElement : DropdownElement
	{
		private string m_MenuPath;

		private string m_Command;

		private bool m_IsLegacy;

		protected override bool isSearchable
		{
			get
			{
				return true;
			}
		}

		public ComponentDropdownElement(string name, string menuPath, string command) : base(name)
		{
			this.m_MenuPath = menuPath;
			this.m_Command = command;
			this.m_IsLegacy = menuPath.Contains("Legacy");
			if (command.StartsWith("SCRIPT"))
			{
				int instanceID = int.Parse(command.Substring(6));
				UnityEngine.Object obj = EditorUtility.InstanceIDToObject(instanceID);
				Texture2D miniThumbnail = AssetPreview.GetMiniThumbnail(obj);
				this.m_Content = new GUIContent(name, miniThumbnail);
			}
			else
			{
				int classID = int.Parse(command);
				this.m_Content = new GUIContent(name, AssetPreview.GetMiniTypeThumbnailFromClassID(classID));
			}
			this.m_ContentWhenSearching = new GUIContent(this.m_Content);
			if (this.m_IsLegacy)
			{
				GUIContent expr_A4 = this.m_ContentWhenSearching;
				expr_A4.text += " (Legacy)";
			}
		}

		public override int CompareTo(object o)
		{
			int result;
			if (o is ComponentDropdownElement)
			{
				ComponentDropdownElement componentDropdownElement = (ComponentDropdownElement)o;
				if (this.m_IsLegacy && !componentDropdownElement.m_IsLegacy)
				{
					result = 1;
					return result;
				}
				if (!this.m_IsLegacy && componentDropdownElement.m_IsLegacy)
				{
					result = -1;
					return result;
				}
			}
			result = base.CompareTo(o);
			return result;
		}

		public override bool OnAction()
		{
			AddComponentWindow.SendUsabilityAnalyticsEvent(new AddComponentWindow.AnalyticsEventData
			{
				name = base.name,
				filter = AddComponentWindow.s_AddComponentWindow.searchString,
				isNewScript = false
			});
			GameObject[] gameObjects = AddComponentWindow.s_AddComponentWindow.m_GameObjects;
			EditorApplication.ExecuteMenuItemOnGameObjects(this.m_MenuPath, gameObjects);
			return true;
		}

		public override string ToString()
		{
			return this.m_MenuPath + this.m_Command;
		}
	}
}
