using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityEditor.Experimental.UIElements.GraphView
{
	[InitializeOnLoad]
	public class SearchWindow : EditorWindow
	{
		private class Styles
		{
			public GUIStyle header = EditorStyles.inspectorBig;

			public GUIStyle componentButton = new GUIStyle("PR Label");

			public GUIStyle groupButton;

			public GUIStyle background = "grey_border";

			public GUIStyle previewBackground = "PopupCurveSwatchBackground";

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

		private const float k_DefaultWidth = 240f;

		private const float k_DefaultHeight = 320f;

		private const int k_HeaderHeight = 30;

		private const int k_WindowYOffset = 16;

		private const string kSearchHeader = "Search";

		private static SearchWindow.Styles s_Styles;

		private static SearchWindow s_FilterWindow;

		private static long s_LastClosedTime;

		private static bool s_DirtyList;

		private ScriptableObject m_Owner;

		private SearchWindowContext m_Context;

		private SearchTreeEntry[] m_Tree;

		private SearchTreeEntry[] m_SearchResultTree;

		private List<SearchTreeGroupEntry> m_SelectionStack = new List<SearchTreeGroupEntry>();

		private float m_Anim = 1f;

		private int m_AnimTarget = 1;

		private long m_LastTime = 0L;

		private bool m_ScrollToSelected = false;

		private string m_DelayedSearch = null;

		private string m_Search = "";

		private ISearchWindowProvider provider
		{
			get
			{
				return this.m_Owner as ISearchWindowProvider;
			}
		}

		private bool hasSearch
		{
			get
			{
				return !string.IsNullOrEmpty(this.m_Search);
			}
		}

		private SearchTreeGroupEntry activeParent
		{
			get
			{
				int num = this.m_SelectionStack.Count - 2 + this.m_AnimTarget;
				SearchTreeGroupEntry result;
				if (num < 0 || num >= this.m_SelectionStack.Count)
				{
					result = null;
				}
				else
				{
					result = this.m_SelectionStack[num];
				}
				return result;
			}
		}

		private SearchTreeEntry[] activeTree
		{
			get
			{
				return (!this.hasSearch) ? this.m_Tree : this.m_SearchResultTree;
			}
		}

		private SearchTreeEntry activeSearchTreeEntry
		{
			get
			{
				SearchTreeEntry result;
				if (this.activeTree == null)
				{
					result = null;
				}
				else
				{
					List<SearchTreeEntry> children = this.GetChildren(this.activeTree, this.activeParent);
					if (this.activeParent == null || this.activeParent.selectedIndex < 0 || this.activeParent.selectedIndex >= children.Count)
					{
						result = null;
					}
					else
					{
						result = children[this.activeParent.selectedIndex];
					}
				}
				return result;
			}
		}

		private bool isAnimating
		{
			get
			{
				return this.m_Anim != (float)this.m_AnimTarget;
			}
		}

		static SearchWindow()
		{
			SearchWindow.s_FilterWindow = null;
			SearchWindow.s_DirtyList = false;
			SearchWindow.s_DirtyList = true;
		}

		private void OnEnable()
		{
			SearchWindow.s_FilterWindow = this;
		}

		private void OnDisable()
		{
			SearchWindow.s_LastClosedTime = DateTime.Now.Ticks / 10000L;
			SearchWindow.s_FilterWindow = null;
		}

		public static bool Open<T>(SearchWindowContext context, T provider) where T : ScriptableObject, ISearchWindowProvider
		{
			UnityEngine.Object[] array = Resources.FindObjectsOfTypeAll(typeof(SearchWindow));
			bool result;
			if (array.Length > 0)
			{
				try
				{
					((EditorWindow)array[0]).Close();
					result = false;
					return result;
				}
				catch (Exception)
				{
					SearchWindow.s_FilterWindow = null;
				}
			}
			long num = DateTime.Now.Ticks / 10000L;
			if (num >= SearchWindow.s_LastClosedTime + 50L)
			{
				if (SearchWindow.s_FilterWindow == null)
				{
					SearchWindow.s_FilterWindow = ScriptableObject.CreateInstance<SearchWindow>();
					SearchWindow.s_FilterWindow.hideFlags = HideFlags.HideAndDontSave;
				}
				SearchWindow.s_FilterWindow.Init(context, provider);
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		private void Init(SearchWindowContext context, ScriptableObject provider)
		{
			this.m_Owner = provider;
			this.m_Context = context;
			float num = Math.Max(context.requestedWidth, 240f);
			float y = Math.Max(context.requestedHeight, 320f);
			Rect buttonRect = new Rect(context.screenMousePosition.x - num / 2f, context.screenMousePosition.y - 16f, num, 1f);
			this.CreateSearchTree();
			base.ShowAsDropDown(buttonRect, new Vector2(buttonRect.width, y));
			base.Focus();
			base.wantsMouseMove = true;
		}

		private void CreateSearchTree()
		{
			List<SearchTreeEntry> list = this.provider.CreateSearchTree(this.m_Context);
			if (list != null)
			{
				this.m_Tree = list.ToArray();
			}
			else
			{
				this.m_Tree = new SearchTreeEntry[0];
			}
			if (this.m_SelectionStack.Count == 0)
			{
				this.m_SelectionStack.Add(this.m_Tree[0] as SearchTreeGroupEntry);
			}
			else
			{
				SearchTreeGroupEntry searchTreeGroupEntry = this.m_Tree[0] as SearchTreeGroupEntry;
				int level = 0;
				while (true)
				{
					SearchTreeGroupEntry searchTreeGroupEntry2 = this.m_SelectionStack[level];
					this.m_SelectionStack[level] = searchTreeGroupEntry;
					this.m_SelectionStack[level].selectedIndex = searchTreeGroupEntry2.selectedIndex;
					this.m_SelectionStack[level].scroll = searchTreeGroupEntry2.scroll;
					level++;
					if (level == this.m_SelectionStack.Count)
					{
						break;
					}
					List<SearchTreeEntry> children = this.GetChildren(this.activeTree, searchTreeGroupEntry);
					SearchTreeEntry searchTreeEntry = children.FirstOrDefault((SearchTreeEntry c) => c.name == this.m_SelectionStack[level].name);
					if (searchTreeEntry != null && searchTreeEntry is SearchTreeGroupEntry)
					{
						searchTreeGroupEntry = (searchTreeEntry as SearchTreeGroupEntry);
					}
					else
					{
						this.m_SelectionStack.RemoveRange(level, this.m_SelectionStack.Count - level);
					}
				}
			}
			SearchWindow.s_DirtyList = false;
			this.RebuildSearch();
		}

		internal void OnGUI()
		{
			if (SearchWindow.s_Styles == null)
			{
				SearchWindow.s_Styles = new SearchWindow.Styles();
			}
			GUI.Label(new Rect(0f, 0f, base.position.width, base.position.height), GUIContent.none, SearchWindow.s_Styles.background);
			if (SearchWindow.s_DirtyList)
			{
				this.CreateSearchTree();
			}
			this.HandleKeyboard();
			GUILayout.Space(7f);
			EditorGUI.FocusTextInControl("ComponentSearch");
			Rect rect = GUILayoutUtility.GetRect(10f, 20f);
			rect.x += 8f;
			rect.width -= 16f;
			GUI.SetNextControlName("ComponentSearch");
			EditorGUI.BeginChangeCheck();
			string text = EditorGUI.SearchField(rect, this.m_DelayedSearch ?? this.m_Search);
			if (EditorGUI.EndChangeCheck() && (text != this.m_Search || this.m_DelayedSearch != null))
			{
				if (!this.isAnimating)
				{
					this.m_Search = (this.m_DelayedSearch ?? text);
					this.RebuildSearch();
					this.m_DelayedSearch = null;
				}
				else
				{
					this.m_DelayedSearch = text;
				}
			}
			this.ListGUI(this.activeTree, this.m_Anim, this.GetSearchTreeEntryRelative(0), this.GetSearchTreeEntryRelative(-1));
			if (this.m_Anim < 1f)
			{
				this.ListGUI(this.activeTree, this.m_Anim + 1f, this.GetSearchTreeEntryRelative(-1), this.GetSearchTreeEntryRelative(-2));
			}
			if (this.isAnimating && Event.current.type == EventType.Repaint)
			{
				long ticks = DateTime.Now.Ticks;
				float num = (float)(ticks - this.m_LastTime) / 1E+07f;
				this.m_LastTime = ticks;
				this.m_Anim = Mathf.MoveTowards(this.m_Anim, (float)this.m_AnimTarget, num * 4f);
				if (this.m_AnimTarget == 0 && this.m_Anim == 0f)
				{
					this.m_Anim = 1f;
					this.m_AnimTarget = 1;
					this.m_SelectionStack.RemoveAt(this.m_SelectionStack.Count - 1);
				}
				base.Repaint();
			}
		}

		private void HandleKeyboard()
		{
			Event current = Event.current;
			if (current.type == EventType.KeyDown)
			{
				if (current.keyCode == KeyCode.DownArrow)
				{
					this.activeParent.selectedIndex++;
					this.activeParent.selectedIndex = Mathf.Min(this.activeParent.selectedIndex, this.GetChildren(this.activeTree, this.activeParent).Count - 1);
					this.m_ScrollToSelected = true;
					current.Use();
				}
				if (current.keyCode == KeyCode.UpArrow)
				{
					this.activeParent.selectedIndex--;
					this.activeParent.selectedIndex = Mathf.Max(this.activeParent.selectedIndex, 0);
					this.m_ScrollToSelected = true;
					current.Use();
				}
				if (current.keyCode == KeyCode.Return || current.keyCode == KeyCode.KeypadEnter)
				{
					if (this.activeSearchTreeEntry != null)
					{
						this.SelectEntry(this.activeSearchTreeEntry, true);
						current.Use();
					}
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
						if (this.activeSearchTreeEntry != null)
						{
							this.SelectEntry(this.activeSearchTreeEntry, false);
							current.Use();
						}
					}
					if (current.keyCode == KeyCode.Escape)
					{
						base.Close();
						current.Use();
					}
				}
			}
		}

		private void RebuildSearch()
		{
			if (!this.hasSearch)
			{
				this.m_SearchResultTree = null;
				if (this.m_SelectionStack[this.m_SelectionStack.Count - 1].name == "Search")
				{
					this.m_SelectionStack.Clear();
					this.m_SelectionStack.Add(this.m_Tree[0] as SearchTreeGroupEntry);
				}
				this.m_AnimTarget = 1;
				this.m_LastTime = DateTime.Now.Ticks;
			}
			else
			{
				string[] array = this.m_Search.ToLower().Split(new char[]
				{
					' '
				});
				List<SearchTreeEntry> list = new List<SearchTreeEntry>();
				List<SearchTreeEntry> list2 = new List<SearchTreeEntry>();
				SearchTreeEntry[] tree = this.m_Tree;
				for (int i = 0; i < tree.Length; i++)
				{
					SearchTreeEntry searchTreeEntry = tree[i];
					if (!(searchTreeEntry is SearchTreeGroupEntry))
					{
						string text = searchTreeEntry.name.ToLower().Replace(" ", "");
						bool flag = true;
						bool flag2 = false;
						for (int j = 0; j < array.Length; j++)
						{
							string value = array[j];
							if (!text.Contains(value))
							{
								flag = false;
								break;
							}
							if (j == 0 && text.StartsWith(value))
							{
								flag2 = true;
							}
						}
						if (flag)
						{
							if (flag2)
							{
								list.Add(searchTreeEntry);
							}
							else
							{
								list2.Add(searchTreeEntry);
							}
						}
					}
				}
				list.Sort();
				list2.Sort();
				List<SearchTreeEntry> list3 = new List<SearchTreeEntry>();
				list3.Add(new SearchTreeGroupEntry(new GUIContent("Search"), 0));
				list3.AddRange(list);
				list3.AddRange(list2);
				this.m_SearchResultTree = list3.ToArray();
				this.m_SelectionStack.Clear();
				this.m_SelectionStack.Add(this.m_SearchResultTree[0] as SearchTreeGroupEntry);
				if (this.GetChildren(this.activeTree, this.activeParent).Count >= 1)
				{
					this.activeParent.selectedIndex = 0;
				}
				else
				{
					this.activeParent.selectedIndex = -1;
				}
			}
		}

		private SearchTreeGroupEntry GetSearchTreeEntryRelative(int rel)
		{
			int num = this.m_SelectionStack.Count + rel - 1;
			SearchTreeGroupEntry result;
			if (num < 0 || num >= this.m_SelectionStack.Count)
			{
				result = null;
			}
			else
			{
				result = this.m_SelectionStack[num];
			}
			return result;
		}

		private void GoToParent()
		{
			if (this.m_SelectionStack.Count > 1)
			{
				this.m_AnimTarget = 0;
				this.m_LastTime = DateTime.Now.Ticks;
			}
		}

		private void ListGUI(SearchTreeEntry[] tree, float anim, SearchTreeGroupEntry parent, SearchTreeGroupEntry grandParent)
		{
			anim = Mathf.Floor(anim) + Mathf.SmoothStep(0f, 1f, Mathf.Repeat(anim, 1f));
			Rect position = base.position;
			position.x = base.position.width * (1f - anim) + 1f;
			position.y = 30f;
			position.height -= 30f;
			position.width -= 2f;
			GUILayout.BeginArea(position);
			Rect rect = GUILayoutUtility.GetRect(10f, 25f);
			string name = parent.name;
			GUI.Label(rect, name, SearchWindow.s_Styles.header);
			if (grandParent != null)
			{
				Rect position2 = new Rect(rect.x + 4f, rect.y + 7f, 13f, 13f);
				if (Event.current.type == EventType.Repaint)
				{
					SearchWindow.s_Styles.leftArrow.Draw(position2, false, false, false, false);
				}
				if (Event.current.type == EventType.MouseDown && rect.Contains(Event.current.mousePosition))
				{
					this.GoToParent();
					Event.current.Use();
				}
			}
			this.ListGUI(tree, parent);
			GUILayout.EndArea();
		}

		private void SelectEntry(SearchTreeEntry e, bool shouldInvokeCallback)
		{
			if (e is SearchTreeGroupEntry)
			{
				if (!this.hasSearch)
				{
					this.m_LastTime = DateTime.Now.Ticks;
					if (this.m_AnimTarget == 0)
					{
						this.m_AnimTarget = 1;
					}
					else if (this.m_Anim == 1f)
					{
						this.m_Anim = 0f;
						this.m_SelectionStack.Add(e as SearchTreeGroupEntry);
					}
				}
			}
			else if (shouldInvokeCallback && this.provider.OnSelectEntry(e, this.m_Context))
			{
				base.Close();
			}
		}

		private void ListGUI(SearchTreeEntry[] tree, SearchTreeGroupEntry parent)
		{
			parent.scroll = GUILayout.BeginScrollView(parent.scroll, new GUILayoutOption[0]);
			EditorGUIUtility.SetIconSize(new Vector2(16f, 16f));
			List<SearchTreeEntry> children = this.GetChildren(tree, parent);
			Rect rect = default(Rect);
			for (int i = 0; i < children.Count; i++)
			{
				SearchTreeEntry searchTreeEntry = children[i];
				Rect rect2 = GUILayoutUtility.GetRect(16f, 20f, new GUILayoutOption[]
				{
					GUILayout.ExpandWidth(true)
				});
				if (Event.current.type == EventType.MouseMove || Event.current.type == EventType.MouseDown)
				{
					if (parent.selectedIndex != i && rect2.Contains(Event.current.mousePosition))
					{
						parent.selectedIndex = i;
						base.Repaint();
					}
				}
				bool flag = false;
				if (i == parent.selectedIndex)
				{
					flag = true;
					rect = rect2;
				}
				if (Event.current.type == EventType.Repaint)
				{
					GUIStyle gUIStyle = (!(searchTreeEntry is SearchTreeGroupEntry)) ? SearchWindow.s_Styles.componentButton : SearchWindow.s_Styles.groupButton;
					gUIStyle.Draw(rect2, searchTreeEntry.content, false, false, flag, flag);
					if (searchTreeEntry is SearchTreeGroupEntry)
					{
						Rect position = new Rect(rect2.x + rect2.width - 13f, rect2.y + 4f, 13f, 13f);
						SearchWindow.s_Styles.rightArrow.Draw(position, false, false, false, false);
					}
				}
				if (Event.current.type == EventType.MouseDown && rect2.Contains(Event.current.mousePosition))
				{
					Event.current.Use();
					parent.selectedIndex = i;
					this.SelectEntry(searchTreeEntry, true);
				}
			}
			EditorGUIUtility.SetIconSize(Vector2.zero);
			GUILayout.EndScrollView();
			if (this.m_ScrollToSelected && Event.current.type == EventType.Repaint)
			{
				this.m_ScrollToSelected = false;
				Rect lastRect = GUILayoutUtility.GetLastRect();
				if (rect.yMax - lastRect.height > parent.scroll.y)
				{
					parent.scroll.y = rect.yMax - lastRect.height;
					base.Repaint();
				}
				if (rect.y < parent.scroll.y)
				{
					parent.scroll.y = rect.y;
					base.Repaint();
				}
			}
		}

		private List<SearchTreeEntry> GetChildren(SearchTreeEntry[] tree, SearchTreeEntry parent)
		{
			List<SearchTreeEntry> list = new List<SearchTreeEntry>();
			int num = -1;
			int i;
			for (i = 0; i < tree.Length; i++)
			{
				if (tree[i] == parent)
				{
					num = parent.level + 1;
					i++;
					break;
				}
			}
			List<SearchTreeEntry> result;
			if (num == -1)
			{
				result = list;
			}
			else
			{
				while (i < tree.Length)
				{
					SearchTreeEntry searchTreeEntry = tree[i];
					if (searchTreeEntry.level < num)
					{
						break;
					}
					if (searchTreeEntry.level <= num || this.hasSearch)
					{
						list.Add(searchTreeEntry);
					}
					i++;
				}
				result = list;
			}
			return result;
		}
	}
}
