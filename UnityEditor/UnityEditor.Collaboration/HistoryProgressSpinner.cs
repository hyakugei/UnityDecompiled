using System;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace UnityEditor.Collaboration
{
	internal class HistoryProgressSpinner : Image
	{
		private readonly Texture2D[] m_StatusWheelTextures;

		private bool m_ProgressEnabled;

		private IVisualElementScheduledItem m_Animation;

		public bool ProgressEnabled
		{
			set
			{
				if (this.m_ProgressEnabled != value)
				{
					this.m_ProgressEnabled = value;
					base.visible = value;
					if (value)
					{
						if (this.m_Animation == null)
						{
							this.m_Animation = base.schedule.Execute(new Action<TimerState>(this.AnimateProgress)).Every(33L);
						}
						else
						{
							this.m_Animation.Resume();
						}
					}
					else if (this.m_Animation != null)
					{
						this.m_Animation.Pause();
					}
				}
			}
		}

		public HistoryProgressSpinner()
		{
			this.m_StatusWheelTextures = new Texture2D[12];
			for (int i = 0; i < 12; i++)
			{
				this.m_StatusWheelTextures[i] = EditorGUIUtility.LoadIcon("WaitSpin" + i.ToString("00"));
			}
			base.image = this.m_StatusWheelTextures[0];
			base.style.width = (float)this.m_StatusWheelTextures[0].width;
			base.style.height = (float)this.m_StatusWheelTextures[0].height;
			base.visible = false;
		}

		private void AnimateProgress(TimerState obj)
		{
			int num = (int)Mathf.Repeat(Time.realtimeSinceStartup * 10f, 11.99f);
			base.image = this.m_StatusWheelTextures[num];
			base.Dirty(ChangeType.Repaint);
		}
	}
}
