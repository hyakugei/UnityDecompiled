using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityEditor
{
	internal class LocalizationDatabase
	{
		public static extern SystemLanguage currentEditorLanguage
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern bool enableEditorLocalization
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern SystemLanguage GetDefaultEditorLanguage();

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern SystemLanguage[] GetAvailableEditorLanguages();

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern string GetLocalizedString(string original);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern string GetLocalizationResourceFolder();

		public static string MarkForTranslation(string value)
		{
			return value;
		}
	}
}
