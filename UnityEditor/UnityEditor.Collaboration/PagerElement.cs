using System;
using UnityEngine.Experimental.UIElements;
using UnityEngine.Experimental.UIElements.StyleEnums;

namespace UnityEditor.Collaboration
{
	internal class PagerElement : VisualElement
	{
		public PageChangeAction OnPageChange;

		private readonly Label m_PageText;

		private readonly Button m_DownButton;

		private readonly Button m_UpButton;

		private int m_CurPage;

		private int m_TotalPages;

		public int PageCount
		{
			get
			{
				return this.m_TotalPages;
			}
			set
			{
				if (this.m_TotalPages != value)
				{
					this.m_TotalPages = value;
					if (this.m_CurPage > this.m_TotalPages)
					{
						this.m_CurPage = this.m_TotalPages;
					}
					this.UpdateControls();
				}
			}
		}

		public PagerElement(int pageCount, int startPage = 0)
		{
			base.style.flexDirection = FlexDirection.Row;
			base.style.alignSelf = Align.Center;
			this.m_CurPage = startPage;
			this.m_TotalPages = pageCount;
			base.Add(this.m_DownButton = new Button(new Action(this.OnPageDownClicked))
			{
				text = "◅ Newer"
			});
			this.m_DownButton.AddToClassList("PagerDown");
			this.m_PageText = new Label();
			this.m_PageText.AddToClassList("PagerLabel");
			base.Add(this.m_PageText);
			base.Add(this.m_UpButton = new Button(new Action(this.OnPageUpClicked))
			{
				text = "Older ▻"
			});
			this.m_UpButton.AddToClassList("PagerUp");
			this.UpdateControls();
		}

		private void OnPageDownClicked()
		{
			if (this.m_CurPage > 0)
			{
				this.m_CurPage--;
				if (this.OnPageChange != null)
				{
					this.OnPageChange(this.m_CurPage);
				}
				this.UpdateControls();
			}
		}

		private void OnPageUpClicked()
		{
			if (this.m_CurPage < this.m_TotalPages)
			{
				this.m_CurPage++;
				if (this.OnPageChange != null)
				{
					this.OnPageChange(this.m_CurPage);
				}
				this.UpdateControls();
			}
		}

		private void UpdateControls()
		{
			this.m_PageText.text = this.m_CurPage + 1 + " / " + this.m_TotalPages;
			this.m_DownButton.SetEnabled(this.m_CurPage > 0);
			this.m_UpButton.SetEnabled(this.m_CurPage < this.m_TotalPages - 1);
		}
	}
}
