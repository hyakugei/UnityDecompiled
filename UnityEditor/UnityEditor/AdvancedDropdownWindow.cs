using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

namespace UnityEditor
{
	[InitializeOnLoad]
	[Serializable]
	internal abstract class AdvancedDropdownWindow : EditorWindow
	{
		internal class Styles
		{
			public GUIStyle header = new GUIStyle(EditorStyles.inspectorBig);

			public GUIStyle componentButton = new GUIStyle("PR Label");

			public GUIStyle groupButton;

			public GUIStyle background = "grey_border";

			public GUIStyle previewHeader = new GUIStyle(EditorStyles.label);

			public GUIStyle previewText = new GUIStyle(EditorStyles.wordWrappedLabel);

			public GUIStyle rightArrow = "AC RightArrow";

			public GUIStyle leftArrow = "AC LeftArrow";

			public Styles()
			{
				this.header.font = EditorStyles.boldLabel.font;
				this.componentButton.alignment = TextAnchor.MiddleLeft;
				this.componentButton.padding.left -= 15;
				this.componentButton.fixedHeight = 20f;
				this.groupButton = new GUIStyle(this.componentButton);
				this.groupButton.padding.left += 17;
				this.previewText.padding.left += 3;
				this.previewText.padding.right += 3;
				this.previewHeader.padding.left++;
				this.previewHeader.padding.right += 3;
				this.previewHeader.padding.top += 3;
				this.previewHeader.padding.bottom += 2;
			}
		}

		protected internal static AdvancedDropdownWindow.Styles s_Styles;

		private const int kHeaderHeight = 30;

		private const int kWindowHeight = 320;

		private const int kHelpHeight = 0;

		private const string kSearchHeader = "Search";

		private DropdownElement m_MainTree;

		private DropdownElement m_SearchTree;

		private DropdownElement m_AnimationTree;

		private float m_NewAnimTarget = 0f;

		private long m_LastTime = 0L;

		private bool m_ScrollToSelected = true;

		private bool m_DirtyList = true;

		protected string m_Search = "";

		public event Action<AdvancedDropdownWindow> onSelected
		{
			add
			{
				Action<AdvancedDropdownWindow> action = this.onSelected;
				Action<AdvancedDropdownWindow> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action<AdvancedDropdownWindow>>(ref this.onSelected, (Action<AdvancedDropdownWindow>)Delegate.Combine(action2, value), action);
				}
				while (action != action2);
			}
			remove
			{
				Action<AdvancedDropdownWindow> action = this.onSelected;
				Action<AdvancedDropdownWindow> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action<AdvancedDropdownWindow>>(ref this.onSelected, (Action<AdvancedDropdownWindow>)Delegate.Remove(action2, value), action);
				}
				while (action != action2);
			}
		}

		protected DropdownElement CurrentlyRenderedTree
		{
			get
			{
				return (!this.hasSearch) ? this.m_MainTree : this.m_SearchTree;
			}
			set
			{
				if (this.hasSearch)
				{
					this.m_SearchTree = value;
				}
				else
				{
					this.m_MainTree = value;
				}
			}
		}

		private bool hasSearch
		{
			get
			{
				return !string.IsNullOrEmpty(this.m_Search);
			}
		}

		protected virtual bool isSearchFieldDisabled
		{
			[CompilerGenerated]
			get
			{
				return this.<isSearchFieldDisabled>k__BackingField;
			}
		}

		protected abstract DropdownElement RebuildTree();

		protected virtual void OnEnable()
		{
			this.m_DirtyList = true;
		}

		protected virtual void OnDisable()
		{
		}

		public static T CreateAndInit<T>(Rect rect) where T : AdvancedDropdownWindow
		{
			T result = ScriptableObject.CreateInstance<T>();
			result.Init(rect);
			return result;
		}

		public void Init(Rect buttonRect)
		{
			buttonRect = GUIUtility.GUIToScreenRect(buttonRect);
			this.OnDirtyList();
			base.ShowAsDropDown(buttonRect, new Vector2(buttonRect.width, 320f), null, ShowMode.PopupMenuWithKeyboardFocus);
			base.Focus();
			this.m_Parent.AddToAuxWindowList();
			base.wantsMouseMove = true;
		}

		internal void OnGUI()
		{
			if (AdvancedDropdownWindow.s_Styles == null)
			{
				AdvancedDropdownWindow.s_Styles = new AdvancedDropdownWindow.Styles();
			}
			GUI.Label(new Rect(0f, 0f, base.position.width, base.position.height), GUIContent.none, AdvancedDropdownWindow.s_Styles.background);
			if (this.m_DirtyList)
			{
				this.OnDirtyList();
			}
			this.HandleKeyboard();
			GUILayout.Space(7f);
			this.OnGUISearch();
			if (this.m_NewAnimTarget != 0f && Event.current.type == EventType.Layout)
			{
				long ticks = DateTime.Now.Ticks;
				float num = (float)(ticks - this.m_LastTime) / 1E+07f;
				this.m_LastTime = ticks;
				this.m_NewAnimTarget = Mathf.MoveTowards(this.m_NewAnimTarget, 0f, num * 4f);
				if (this.m_NewAnimTarget == 0f)
				{
					this.m_AnimationTree = null;
				}
				base.Repaint();
			}
			float num2 = this.m_NewAnimTarget;
			num2 = Mathf.Floor(num2) + Mathf.SmoothStep(0f, 1f, Mathf.Repeat(num2, 1f));
			if (num2 == 0f)
			{
				this.DrawDropdown(0f, this.CurrentlyRenderedTree);
			}
			else if (num2 < 0f)
			{
				this.DrawDropdown(num2, this.CurrentlyRenderedTree);
				this.DrawDropdown(num2 + 1f, this.m_AnimationTree);
			}
			else
			{
				this.DrawDropdown(num2 - 1f, this.m_AnimationTree);
				this.DrawDropdown(num2, this.CurrentlyRenderedTree);
			}
		}

		private void OnDirtyList()
		{
			this.m_DirtyList = false;
			this.m_MainTree = this.RebuildTree();
			if (this.hasSearch)
			{
				this.m_SearchTree = this.RebuildSearch();
			}
		}

		private void OnGUISearch()
		{
			if (!this.isSearchFieldDisabled)
			{
				EditorGUI.FocusTextInControl("ComponentSearch");
			}
			Rect rect = GUILayoutUtility.GetRect(10f, 20f);
			rect.x += 8f;
			rect.width -= 16f;
			GUI.SetNextControlName("ComponentSearch");
			using (new EditorGUI.DisabledScope(this.isSearchFieldDisabled))
			{
				string text = EditorGUI.SearchField(rect, this.m_Search);
				if (text != this.m_Search)
				{
					this.m_Search = text;
					this.m_SearchTree = this.RebuildSearch();
				}
			}
		}

		private void HandleKeyboard()
		{
			Event current = Event.current;
			if (current.type == EventType.KeyDown)
			{
				if (!this.SpecialKeyboardHandling(current))
				{
					if (current.keyCode == KeyCode.DownArrow)
					{
						this.CurrentlyRenderedTree.selectedItem++;
						this.m_ScrollToSelected = true;
						current.Use();
					}
					if (current.keyCode == KeyCode.UpArrow)
					{
						this.CurrentlyRenderedTree.selectedItem--;
						this.m_ScrollToSelected = true;
						current.Use();
					}
					if (current.keyCode == KeyCode.Return || current.keyCode == KeyCode.KeypadEnter)
					{
						if (this.CurrentlyRenderedTree.GetSelectedChild().children.Any<DropdownElement>())
						{
							this.GoToChild(this.CurrentlyRenderedTree);
						}
						else if (this.CurrentlyRenderedTree.GetSelectedChild().OnAction())
						{
							this.CloseWindow();
						}
						current.Use();
					}
					if (!this.hasSearch)
					{
						if (current.keyCode == KeyCode.LeftArrow || current.keyCode == KeyCode.Backspace)
						{
							this.GoToParent();
							current.Use();
						}
						if (current.keyCode == KeyCode.RightArrow)
						{
							if (this.CurrentlyRenderedTree.GetSelectedChild().children.Any<DropdownElement>())
							{
								this.GoToChild(this.CurrentlyRenderedTree);
							}
							current.Use();
						}
						if (current.keyCode == KeyCode.Escape)
						{
							base.Close();
							current.Use();
						}
					}
				}
			}
		}

		private void CloseWindow()
		{
			if (this.onSelected != null)
			{
				this.onSelected(this);
			}
			base.Close();
		}

		internal string GetIdOfSelectedItem()
		{
			return this.CurrentlyRenderedTree.GetSelectedChild().id;
		}

		protected virtual bool SpecialKeyboardHandling(Event evt)
		{
			return false;
		}

		private void DrawDropdown(float anim, DropdownElement group)
		{
			Rect screenRect = new Rect(base.position);
			screenRect.x = 1f + base.position.width * anim;
			screenRect.y = 30f;
			screenRect.height -= 30f;
			screenRect.width -= 2f;
			GUILayout.BeginArea(screenRect);
			Rect rect = GUILayoutUtility.GetRect(10f, 25f);
			string name = group.name;
			GUI.Label(rect, name, AdvancedDropdownWindow.s_Styles.header);
			if (group.parent != null)
			{
				Rect position = new Rect(rect.x + 4f, rect.y + 7f, 13f, 13f);
				if (Event.current.type == EventType.Repaint)
				{
					AdvancedDropdownWindow.s_Styles.leftArrow.Draw(position, false, false, false, false);
				}
				if (Event.current.type == EventType.MouseDown && rect.Contains(Event.current.mousePosition))
				{
					this.GoToParent();
					Event.current.Use();
				}
			}
			this.DrawList(group);
			GUILayout.EndArea();
		}

		private void DrawList(DropdownElement element)
		{
			element.m_Scroll = GUILayout.BeginScrollView(element.m_Scroll, new GUILayoutOption[0]);
			EditorGUIUtility.SetIconSize(new Vector2(16f, 16f));
			Rect rect = default(Rect);
			for (int i = 0; i < element.children.Count; i++)
			{
				DropdownElement dropdownElement = element.children[i];
				bool flag = i == element.m_SelectedItem;
				dropdownElement.Draw(flag, this.hasSearch);
				Rect lastRect = GUILayoutUtility.GetLastRect();
				if (flag)
				{
					rect = lastRect;
				}
				if (Event.current.type == EventType.MouseMove || Event.current.type == EventType.MouseDown)
				{
					if (!flag && lastRect.Contains(Event.current.mousePosition))
					{
						element.m_SelectedItem = i;
						Event.current.Use();
					}
				}
				if (Event.current.type == EventType.MouseDown && lastRect.Contains(Event.current.mousePosition))
				{
					element.m_SelectedItem = i;
					if (this.CurrentlyRenderedTree.GetSelectedChild().children.Any<DropdownElement>())
					{
						this.GoToChild(this.CurrentlyRenderedTree);
					}
					else if (this.CurrentlyRenderedTree.GetSelectedChild().OnAction())
					{
						this.CloseWindow();
						GUIUtility.ExitGUI();
					}
					Event.current.Use();
				}
			}
			EditorGUIUtility.SetIconSize(Vector2.zero);
			GUILayout.EndScrollView();
			if (this.m_ScrollToSelected && Event.current.type == EventType.Repaint)
			{
				this.m_ScrollToSelected = false;
				Rect lastRect2 = GUILayoutUtility.GetLastRect();
				if (rect.yMax - lastRect2.height > element.m_Scroll.y)
				{
					element.m_Scroll.y = rect.yMax - lastRect2.height;
					base.Repaint();
				}
				if (rect.y < element.m_Scroll.y)
				{
					element.m_Scroll.y = rect.y;
					base.Repaint();
				}
			}
		}

		protected virtual DropdownElement RebuildSearch()
		{
			DropdownElement result;
			if (string.IsNullOrEmpty(this.m_Search))
			{
				result = null;
			}
			else
			{
				string[] array = this.m_Search.ToLower().Split(new char[]
				{
					' '
				});
				List<DropdownElement> list = new List<DropdownElement>();
				List<DropdownElement> list2 = new List<DropdownElement>();
				foreach (DropdownElement current in this.m_MainTree.GetSearchableElements())
				{
					string text = current.name.ToLower().Replace(" ", "");
					bool flag = true;
					bool flag2 = false;
					for (int i = 0; i < array.Length; i++)
					{
						string value = array[i];
						if (!text.Contains(value))
						{
							flag = false;
							break;
						}
						if (i == 0 && text.StartsWith(value))
						{
							flag2 = true;
						}
					}
					if (flag)
					{
						if (flag2)
						{
							list.Add(current);
						}
						else
						{
							list2.Add(current);
						}
					}
				}
				list.Sort();
				list2.Sort();
				GroupDropdownElement groupDropdownElement = new GroupDropdownElement("Search");
				foreach (DropdownElement current2 in list)
				{
					groupDropdownElement.AddChild(current2);
				}
				foreach (DropdownElement current3 in list2)
				{
					groupDropdownElement.AddChild(current3);
				}
				result = groupDropdownElement;
			}
			return result;
		}

		protected void GoToParent()
		{
			if (this.CurrentlyRenderedTree.parent != null)
			{
				this.m_LastTime = DateTime.Now.Ticks;
				if (this.m_NewAnimTarget > 0f)
				{
					this.m_NewAnimTarget = -1f + this.m_NewAnimTarget;
				}
				else
				{
					this.m_NewAnimTarget = -1f;
				}
				this.m_AnimationTree = this.CurrentlyRenderedTree;
				this.CurrentlyRenderedTree = this.CurrentlyRenderedTree.parent;
			}
		}

		private void GoToChild(DropdownElement parent)
		{
			this.m_LastTime = DateTime.Now.Ticks;
			if (this.m_NewAnimTarget < 0f)
			{
				this.m_NewAnimTarget = 1f + this.m_NewAnimTarget;
			}
			else
			{
				this.m_NewAnimTarget = 1f;
			}
			this.CurrentlyRenderedTree = parent.GetSelectedChild();
			this.m_AnimationTree = parent;
		}

		public int GetSelectedIndex()
		{
			return this.CurrentlyRenderedTree.GetSelectedChildIndex();
		}
	}
}
