using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityEditor
{
	[Serializable]
	internal class DropdownElement : IComparable
	{
		protected GUIContent m_Content;

		protected GUIContent m_ContentWhenSearching;

		private string m_Name;

		private string m_Id;

		private DropdownElement m_Parent;

		private List<DropdownElement> m_Children = new List<DropdownElement>();

		internal int m_Index = -1;

		internal Vector2 m_Scroll;

		internal int m_SelectedItem = 0;

		protected virtual GUIStyle labelStyle
		{
			get
			{
				return AdvancedDropdownWindow.s_Styles.componentButton;
			}
		}

		public string name
		{
			get
			{
				return this.m_Name;
			}
		}

		public string id
		{
			get
			{
				return this.m_Id;
			}
		}

		public DropdownElement parent
		{
			get
			{
				return this.m_Parent;
			}
		}

		public List<DropdownElement> children
		{
			get
			{
				return this.m_Children;
			}
		}

		protected virtual bool drawArrow
		{
			[CompilerGenerated]
			get
			{
				return this.<drawArrow>k__BackingField;
			}
		}

		protected virtual bool isSearchable
		{
			[CompilerGenerated]
			get
			{
				return this.<isSearchable>k__BackingField;
			}
		}

		public int selectedItem
		{
			get
			{
				return this.m_SelectedItem;
			}
			set
			{
				if (value < 0)
				{
					this.m_SelectedItem = 0;
				}
				else if (value >= this.children.Count)
				{
					this.m_SelectedItem = this.children.Count - 1;
				}
				else
				{
					this.m_SelectedItem = value;
				}
			}
		}

		public DropdownElement(string name) : this(name, name, -1)
		{
		}

		public DropdownElement(string name, string id) : this(name, id, -1)
		{
		}

		public DropdownElement(string name, int index) : this(name, name, index)
		{
		}

		public DropdownElement(string name, string id, int index)
		{
			this.m_Content = new GUIContent(name);
			this.m_ContentWhenSearching = new GUIContent(id);
			this.m_Name = name;
			this.m_Id = id;
			this.m_Index = index;
		}

		internal void AddChild(DropdownElement element)
		{
			this.children.Add(element);
		}

		public void SetParent(DropdownElement element)
		{
			this.m_Parent = element;
		}

		public virtual bool OnAction()
		{
			return true;
		}

		public virtual void Draw(bool selected, bool isSearching)
		{
			GUIContent content = isSearching ? this.m_ContentWhenSearching : this.m_Content;
			Rect rect = GUILayoutUtility.GetRect(content, this.labelStyle, new GUILayoutOption[]
			{
				GUILayout.ExpandWidth(true)
			});
			if (Event.current.type == EventType.Repaint)
			{
				this.labelStyle.Draw(rect, content, false, false, selected, selected);
				if (this.drawArrow)
				{
					Rect position = new Rect(rect.x + rect.width - 13f, rect.y + 4f, 13f, 13f);
					AdvancedDropdownWindow.s_Styles.rightArrow.Draw(position, false, false, false, false);
				}
			}
		}

		public DropdownElement GetSelectedChild()
		{
			DropdownElement result;
			if (this.children.Count == 0)
			{
				result = null;
			}
			else
			{
				result = this.children[this.m_SelectedItem];
			}
			return result;
		}

		public virtual int GetSelectedChildIndex()
		{
			int index = this.children[this.m_SelectedItem].m_Index;
			int result;
			if (index >= 0)
			{
				result = index;
			}
			else
			{
				result = this.m_SelectedItem;
			}
			return result;
		}

		[DebuggerHidden]
		public IEnumerable<DropdownElement> GetSearchableElements()
		{
			DropdownElement.<GetSearchableElements>c__Iterator0 <GetSearchableElements>c__Iterator = new DropdownElement.<GetSearchableElements>c__Iterator0();
			<GetSearchableElements>c__Iterator.$this = this;
			DropdownElement.<GetSearchableElements>c__Iterator0 expr_0E = <GetSearchableElements>c__Iterator;
			expr_0E.$PC = -2;
			return expr_0E;
		}

		public virtual int CompareTo(object o)
		{
			return this.name.CompareTo((o as DropdownElement).name);
		}
	}
}
