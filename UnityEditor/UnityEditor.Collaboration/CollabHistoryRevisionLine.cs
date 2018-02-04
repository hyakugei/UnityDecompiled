using System;
using UnityEngine.Experimental.UIElements;

namespace UnityEditor.Collaboration
{
	internal class CollabHistoryRevisionLine : VisualElement
	{
		public CollabHistoryRevisionLine(int number)
		{
			this.AddNumber(number);
			this.AddLine("topLine");
			this.AddLine("bottomLine");
			this.AddIndicator();
		}

		public CollabHistoryRevisionLine(DateTime date, bool isFullDateObtained)
		{
			this.AddLine((!isFullDateObtained) ? "absentDateLine" : "obtainedDateLine");
			this.AddHeader(this.GetFormattedHeader(date));
			base.AddToClassList("revisionLineHeader");
		}

		private void AddHeader(string content)
		{
			base.Add(new Label
			{
				text = content
			});
		}

		private void AddIndicator()
		{
			base.Add(new VisualElement
			{
				name = "RevisionIndicator"
			});
		}

		private void AddLine(string className = null)
		{
			VisualElement visualElement = new VisualElement
			{
				name = "RevisionLine"
			};
			if (!string.IsNullOrEmpty(className))
			{
				visualElement.AddToClassList(className);
			}
			base.Add(visualElement);
		}

		private void AddNumber(int number)
		{
			base.Add(new Label
			{
				text = number.ToString(),
				name = "RevisionIndex"
			});
		}

		private string GetFormattedHeader(DateTime date)
		{
			string text = "Commits on " + date.ToString("MMM d");
			int day = date.Day;
			switch (day)
			{
			case 1:
				break;
			case 2:
				goto IL_67;
			case 3:
				goto IL_78;
			default:
				switch (day)
				{
				case 21:
					break;
				case 22:
					goto IL_67;
				case 23:
					goto IL_78;
				default:
					if (day != 31)
					{
						text += "th";
						return text;
					}
					break;
				}
				break;
			}
			text += "st";
			return text;
			IL_67:
			text += "nd";
			return text;
			IL_78:
			text += "rd";
			return text;
		}
	}
}
