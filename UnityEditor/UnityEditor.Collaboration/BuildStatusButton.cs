using System;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace UnityEditor.Collaboration
{
	internal class BuildStatusButton : Button
	{
		private readonly string iconPrefix = "Icons/Collab.Build";

		private readonly string iconSuffix = ".png";

		private Label labelElement = new Label();

		private Image iconElement = new Image
		{
			name = "BuildIcon"
		};

		public BuildStatusButton(Action clickEvent) : base(clickEvent)
		{
			this.iconElement.image = (EditorGUIUtility.Load(this.iconPrefix + this.iconSuffix) as Texture);
			this.labelElement.text = "Build Now";
			base.Add(this.iconElement);
			base.Add(this.labelElement);
		}

		public BuildStatusButton(Action clickEvent, BuildState state, int failures) : base(clickEvent)
		{
			if (state != BuildState.InProgress)
			{
				if (state != BuildState.Failed)
				{
					if (state == BuildState.Success)
					{
						this.iconElement.image = (EditorGUIUtility.Load(this.iconPrefix + "Succeeded" + this.iconSuffix) as Texture);
						this.labelElement.text = "success";
					}
				}
				else
				{
					this.iconElement.image = (EditorGUIUtility.Load(this.iconPrefix + "Failed" + this.iconSuffix) as Texture);
					this.labelElement.text = failures + ((failures != 1) ? " failures" : " failure");
				}
			}
			else
			{
				this.iconElement.image = (EditorGUIUtility.Load(this.iconPrefix + this.iconSuffix) as Texture);
				this.labelElement.text = "In progress";
			}
			base.Add(this.iconElement);
			base.Add(this.labelElement);
		}
	}
}
