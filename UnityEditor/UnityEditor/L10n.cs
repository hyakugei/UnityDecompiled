using System;
using System.Text;

namespace UnityEditor
{
	internal class L10n
	{
		private L10n()
		{
		}

		public static string Tr(string str)
		{
			return LocalizationDatabase.GetLocalizedString(str);
		}

		public static string[] Tr(string[] str_list)
		{
			string[] array = new string[str_list.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = L10n.Tr(str_list[i]);
			}
			return array;
		}

		public static string TrPath(string path)
		{
			string[] separator = new string[]
			{
				"/"
			};
			StringBuilder stringBuilder = new StringBuilder(256);
			string[] array = path.Split(separator, StringSplitOptions.RemoveEmptyEntries);
			for (int i = 0; i < array.Length; i++)
			{
				stringBuilder.Append(L10n.Tr(array[i]));
				if (i < array.Length - 1)
				{
					stringBuilder.Append("/");
				}
			}
			return stringBuilder.ToString();
		}
	}
}
