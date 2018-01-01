using System;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace UnityEditor.Experimental.UIElements.GraphView
{
	internal class FallbackGraphElement : GraphElement
	{
		private Label m_Text;

		public FallbackGraphElement()
		{
			base.style.backgroundColor = Color.grey;
			this.m_Text = new Label();
			base.Add(this.m_Text);
		}

		public override void OnDataChanged()
		{
			GraphElementPresenter presenter = base.GetPresenter<GraphElementPresenter>();
			this.m_Text.text = "Fallback for " + presenter.GetType() + ". No GraphElement registered for this type in this view.";
		}
	}
}
