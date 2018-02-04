using System;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace UnityEditor.Collaboration
{
	internal class StatusView : VisualContainer
	{
		private Image m_Image;

		private Label m_Message;

		private Button m_Button;

		private Action m_Callback;

		public Texture icon
		{
			get
			{
				return this.m_Image.image;
			}
			set
			{
				this.m_Image.image = value;
				this.m_Image.visible = (value != null);
				this.m_Image.style.height = (float)((!(value != null)) ? 0 : 150);
			}
		}

		public string message
		{
			get
			{
				return this.m_Message.text;
			}
			set
			{
				this.m_Message.text = value;
				this.m_Message.visible = (value != null);
			}
		}

		public string buttonText
		{
			get
			{
				return this.m_Button.text;
			}
			set
			{
				this.m_Button.text = value;
				this.UpdateButton();
			}
		}

		public Action callback
		{
			get
			{
				return this.m_Callback;
			}
			set
			{
				this.m_Callback = value;
				this.UpdateButton();
			}
		}

		public StatusView()
		{
			base.name = "StatusView";
			this.StretchToParentSize();
			this.m_Image = new Image
			{
				name = "StatusIcon",
				visible = false,
				style = 
				{
					height = 0f
				}
			};
			this.m_Message = new Label
			{
				name = "StatusMessage",
				visible = false
			};
			this.m_Button = new Button(new Action(this.InternalCallaback))
			{
				name = "StatusButton",
				visible = false
			};
			base.Add(this.m_Image);
			base.Add(this.m_Message);
			base.Add(this.m_Button);
		}

		private void UpdateButton()
		{
			this.m_Button.visible = (this.m_Button.text != null && this.m_Callback != null);
		}

		private void InternalCallaback()
		{
			this.m_Callback();
		}
	}
}
