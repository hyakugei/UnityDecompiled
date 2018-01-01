using System;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Scripting;

namespace UnityEditor
{
	[InitializeOnLoad]
	internal class AddComponentWindow : AdvancedDropdownWindow
	{
		[Serializable]
		internal class AnalyticsEventData
		{
			public string name;

			public string filter;

			public bool isNewScript;
		}

		internal static AddComponentWindow s_AddComponentWindow = null;

		internal GameObject[] m_GameObjects;

		private DateTime m_ComponentOpenTime;

		private const string kComponentSearch = "ComponentSearchString";

		public const string OpenAddComponentDropdown = "OpenAddComponentDropdown";

		internal string searchString
		{
			[CompilerGenerated]
			get
			{
				return this.m_Search;
			}
		}

		protected override bool isSearchFieldDisabled
		{
			get
			{
				DropdownElement selectedChild = base.CurrentlyRenderedTree.GetSelectedChild();
				return selectedChild != null && selectedChild is NewScriptDropdownElement;
			}
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			AddComponentWindow.s_AddComponentWindow = this;
			this.m_Search = EditorPrefs.GetString("ComponentSearchString", "");
		}

		protected override void OnDisable()
		{
			AddComponentWindow.s_AddComponentWindow = null;
			EditorPrefs.SetString("ComponentSearchString", this.m_Search);
		}

		internal static bool Show(Rect rect, GameObject[] gos)
		{
			AddComponentWindow.CloseAllOpenWindows<AddComponentWindow>();
			Event.current.Use();
			AddComponentWindow.s_AddComponentWindow = AdvancedDropdownWindow.CreateAndInit<AddComponentWindow>(rect);
			AddComponentWindow.s_AddComponentWindow.m_GameObjects = gos;
			AddComponentWindow.s_AddComponentWindow.m_ComponentOpenTime = DateTime.UtcNow;
			return true;
		}

		protected override bool SpecialKeyboardHandling(Event evt)
		{
			DropdownElement selectedChild = base.CurrentlyRenderedTree.GetSelectedChild();
			bool result;
			if (selectedChild is NewScriptDropdownElement)
			{
				if (evt.keyCode == KeyCode.Return || evt.keyCode == KeyCode.KeypadEnter)
				{
					selectedChild.OnAction();
					evt.Use();
					GUIUtility.ExitGUI();
				}
				if (evt.keyCode == KeyCode.Escape)
				{
					base.GoToParent();
					evt.Use();
				}
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		protected override DropdownElement RebuildTree()
		{
			DropdownElement dropdownElement = new DropdownElement("ROOT");
			string[] submenus = Unsupported.GetSubmenus("Component");
			string[] submenusCommands = Unsupported.GetSubmenusCommands("Component");
			for (int i = 0; i < submenus.Length; i++)
			{
				if (!(submenusCommands[i] == "ADD"))
				{
					string text = submenus[i];
					string[] array = text.Split(new char[]
					{
						'/'
					});
					DropdownElement dropdownElement2 = dropdownElement;
					for (int j = 0; j < array.Length; j++)
					{
						string path = array[j];
						if (j == array.Length - 1)
						{
							ComponentDropdownElement componentDropdownElement = new ComponentDropdownElement(LocalizationDatabase.GetLocalizedString(path), text, submenusCommands[i]);
							componentDropdownElement.SetParent(dropdownElement2);
							dropdownElement2.AddChild(componentDropdownElement);
						}
						else
						{
							DropdownElement dropdownElement3 = dropdownElement2.children.SingleOrDefault((DropdownElement c) => c.name == path);
							if (dropdownElement3 == null)
							{
								dropdownElement3 = new GroupDropdownElement(path);
								dropdownElement3.SetParent(dropdownElement2);
								dropdownElement2.AddChild(dropdownElement3);
							}
							dropdownElement2 = dropdownElement3;
						}
					}
				}
			}
			dropdownElement = dropdownElement.children.Single<DropdownElement>();
			dropdownElement.SetParent(null);
			GroupDropdownElement groupDropdownElement = new GroupDropdownElement("New script");
			groupDropdownElement.AddChild(new NewScriptDropdownElement());
			groupDropdownElement.SetParent(dropdownElement);
			dropdownElement.AddChild(groupDropdownElement);
			return dropdownElement;
		}

		protected override DropdownElement RebuildSearch()
		{
			DropdownElement dropdownElement = base.RebuildSearch();
			if (dropdownElement != null)
			{
				GroupDropdownElement groupDropdownElement = new GroupDropdownElement("New script");
				NewScriptDropdownElement newScriptDropdownElement = new NewScriptDropdownElement();
				newScriptDropdownElement.className = this.m_Search;
				groupDropdownElement.AddChild(newScriptDropdownElement);
				newScriptDropdownElement.SetParent(groupDropdownElement);
				groupDropdownElement.SetParent(dropdownElement);
				dropdownElement.AddChild(groupDropdownElement);
			}
			return dropdownElement;
		}

		internal static void SendUsabilityAnalyticsEvent(AddComponentWindow.AnalyticsEventData eventData)
		{
			DateTime componentOpenTime = AddComponentWindow.s_AddComponentWindow.m_ComponentOpenTime;
			UsabilityAnalytics.SendEvent("executeAddComponentWindow", componentOpenTime, DateTime.UtcNow - componentOpenTime, false, eventData);
		}

		private static void CloseAllOpenWindows<T>()
		{
			UnityEngine.Object[] array = Resources.FindObjectsOfTypeAll(typeof(T));
			UnityEngine.Object[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				UnityEngine.Object @object = array2[i];
				try
				{
					((EditorWindow)@object).Close();
				}
				catch
				{
					UnityEngine.Object.DestroyImmediate(@object);
				}
			}
		}

		[UsedByNativeCode]
		internal static bool ValidateAddComponentMenuItem()
		{
			return AddComponentWindow.FirstInspectorWithGameObject() != null;
		}

		[UsedByNativeCode]
		internal static void ExecuteAddComponentMenuItem()
		{
			InspectorWindow inspectorWindow = AddComponentWindow.FirstInspectorWithGameObject();
			if (inspectorWindow != null)
			{
				inspectorWindow.SendEvent(EditorGUIUtility.CommandEvent("OpenAddComponentDropdown"));
			}
		}

		private static InspectorWindow FirstInspectorWithGameObject()
		{
			InspectorWindow result;
			foreach (InspectorWindow current in InspectorWindow.GetInspectors())
			{
				if (current.GetInspectedObject() is GameObject)
				{
					result = current;
					return result;
				}
			}
			result = null;
			return result;
		}
	}
}
