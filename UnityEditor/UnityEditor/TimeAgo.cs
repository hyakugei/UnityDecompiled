using System;

namespace UnityEditor
{
	internal static class TimeAgo
	{
		private const int k_Second = 1;

		private const int k_Minute = 60;

		private const int k_Hour = 3600;

		private const int k_Day = 86400;

		private const int k_Month = 2592000;

		public static string GetString(DateTime dateTime)
		{
			TimeSpan timeSpan = new TimeSpan(DateTime.UtcNow.Ticks - dateTime.ToUniversalTime().Ticks);
			double num = Math.Abs(timeSpan.TotalSeconds);
			string result;
			if (num < 60.0)
			{
				result = "less than a minute ago";
			}
			else if (num < 120.0)
			{
				result = "a minute ago";
			}
			else if (num < 2700.0)
			{
				result = timeSpan.Minutes + " minutes ago";
			}
			else if (num < 5400.0)
			{
				result = "an hour ago";
			}
			else if (num < 86400.0)
			{
				result = timeSpan.Hours + " hours ago";
			}
			else if (num < 172800.0)
			{
				result = "yesterday";
			}
			else if (num < 2592000.0)
			{
				result = timeSpan.Days + " days ago";
			}
			else if (num < 31104000.0)
			{
				int num2 = Convert.ToInt32(Math.Floor((double)timeSpan.Days / 30.0));
				result = ((num2 > 1) ? (num2 + " months ago") : "a month ago");
			}
			else
			{
				int num3 = Convert.ToInt32(Math.Floor((double)timeSpan.Days / 365.0));
				result = ((num3 > 1) ? (num3 + " years ago") : "one year ago");
			}
			return result;
		}
	}
}
