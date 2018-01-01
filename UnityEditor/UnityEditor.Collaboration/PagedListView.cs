using System;
using System.Collections.Generic;
using UnityEngine.Experimental.UIElements;

namespace UnityEditor.Collaboration
{
	internal class PagedListView : VisualElement
	{
		public const int DefaultItemsPerPage = 10;

		private readonly VisualElement m_ItemContainer;

		private readonly PagerElement m_Pager;

		private int m_PageSize = 10;

		private IEnumerable<VisualElement> m_Items;

		private int m_TotalItems;

		private PagerLocation m_PagerLoc = PagerLocation.Bottom;

		public PagerLocation PagerLoc
		{
			get
			{
				return this.m_PagerLoc;
			}
			set
			{
				if (value != this.m_PagerLoc)
				{
					this.m_PagerLoc = value;
					this.UpdatePager();
				}
			}
		}

		public int pageSize
		{
			set
			{
				this.m_PageSize = value;
			}
		}

		public IEnumerable<VisualElement> items
		{
			set
			{
				this.m_Items = value;
				this.LayoutItems();
			}
		}

		public int totalItems
		{
			set
			{
				if (this.m_TotalItems != value)
				{
					this.m_TotalItems = value;
					this.UpdatePager();
				}
			}
		}

		public PageChangeAction OnPageChange
		{
			set
			{
				this.m_Pager.OnPageChange = value;
			}
		}

		private int pageCount
		{
			get
			{
				int num = this.m_TotalItems / this.m_PageSize;
				if (this.m_TotalItems % this.m_PageSize > 0)
				{
					num++;
				}
				return num;
			}
		}

		public PagedListView()
		{
			this.m_Pager = new PagerElement(0, 0);
			this.m_ItemContainer = new VisualElement
			{
				name = "PagerItems"
			};
			base.Add(this.m_ItemContainer);
			this.m_Items = new List<VisualElement>();
		}

		private void UpdatePager()
		{
			if (this.m_Pager.parent == this)
			{
				base.Remove(this.m_Pager);
			}
			PagerLocation pagerLoc = this.m_PagerLoc;
			if (pagerLoc != PagerLocation.Top)
			{
				if (pagerLoc == PagerLocation.Bottom)
				{
					base.Add(this.m_Pager);
				}
			}
			else
			{
				base.Insert(0, this.m_Pager);
			}
			this.m_Pager.PageCount = this.pageCount;
		}

		private void LayoutItems()
		{
			this.m_ItemContainer.Clear();
			foreach (VisualElement current in this.m_Items)
			{
				this.m_ItemContainer.Add(current);
			}
		}
	}
}
