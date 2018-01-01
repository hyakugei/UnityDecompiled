using System;
using UnityEngine.Experimental.UIElements;

namespace UnityEditor.Experimental.UIElements.GraphView
{
	public class SimpleElement : GraphElement
	{
		private Label m_Text;

		public SimpleElement()
		{
			this.m_Text = new Label();
			base.Add(this.m_Text);
		}

		public override void OnDataChanged()
		{
			base.OnDataChanged();
			SimpleElementPresenter presenter = base.GetPresenter<SimpleElementPresenter>();
			this.m_Text.text = presenter.title;
		}
	}
}
