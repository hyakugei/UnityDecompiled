using System;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine.Internal;
using UnityEngine.Scripting;

namespace UnityEditor
{
	public sealed class EditorPrefs
	{
		internal delegate void ValueWasUpdated(string key);

		internal static event EditorPrefs.ValueWasUpdated onValueWasUpdated
		{
			add
			{
				EditorPrefs.ValueWasUpdated valueWasUpdated = EditorPrefs.onValueWasUpdated;
				EditorPrefs.ValueWasUpdated valueWasUpdated2;
				do
				{
					valueWasUpdated2 = valueWasUpdated;
					valueWasUpdated = Interlocked.CompareExchange<EditorPrefs.ValueWasUpdated>(ref EditorPrefs.onValueWasUpdated, (EditorPrefs.ValueWasUpdated)Delegate.Combine(valueWasUpdated2, value), valueWasUpdated);
				}
				while (valueWasUpdated != valueWasUpdated2);
			}
			remove
			{
				EditorPrefs.ValueWasUpdated valueWasUpdated = EditorPrefs.onValueWasUpdated;
				EditorPrefs.ValueWasUpdated valueWasUpdated2;
				do
				{
					valueWasUpdated2 = valueWasUpdated;
					valueWasUpdated = Interlocked.CompareExchange<EditorPrefs.ValueWasUpdated>(ref EditorPrefs.onValueWasUpdated, (EditorPrefs.ValueWasUpdated)Delegate.Remove(valueWasUpdated2, value), valueWasUpdated);
				}
				while (valueWasUpdated != valueWasUpdated2);
			}
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetIntInternal(string key, int value);

		public static void SetInt(string key, int value)
		{
			EditorPrefs.SetIntInternal(key, value);
			if (EditorPrefs.onValueWasUpdated != null)
			{
				EditorPrefs.onValueWasUpdated(key);
			}
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int GetInt(string key, [DefaultValue("0")] int defaultValue);

		[ExcludeFromDocs]
		public static int GetInt(string key)
		{
			int defaultValue = 0;
			return EditorPrefs.GetInt(key, defaultValue);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetFloatInternal(string key, float value);

		public static void SetFloat(string key, float value)
		{
			EditorPrefs.SetFloatInternal(key, value);
			if (EditorPrefs.onValueWasUpdated != null)
			{
				EditorPrefs.onValueWasUpdated(key);
			}
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern float GetFloat(string key, [DefaultValue("0.0F")] float defaultValue);

		[ExcludeFromDocs]
		public static float GetFloat(string key)
		{
			float defaultValue = 0f;
			return EditorPrefs.GetFloat(key, defaultValue);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetStringInternal(string key, string value);

		public static void SetString(string key, string value)
		{
			EditorPrefs.SetStringInternal(key, value);
			if (EditorPrefs.onValueWasUpdated != null)
			{
				EditorPrefs.onValueWasUpdated(key);
			}
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern string GetString(string key, [DefaultValue("\"\"")] string defaultValue);

		[ExcludeFromDocs]
		public static string GetString(string key)
		{
			string defaultValue = "";
			return EditorPrefs.GetString(key, defaultValue);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetBoolInternal(string key, bool value);

		public static void SetBool(string key, bool value)
		{
			EditorPrefs.SetBoolInternal(key, value);
			if (EditorPrefs.onValueWasUpdated != null)
			{
				EditorPrefs.onValueWasUpdated(key);
			}
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool GetBool(string key, [DefaultValue("false")] bool defaultValue);

		[ExcludeFromDocs]
		public static bool GetBool(string key)
		{
			bool defaultValue = false;
			return EditorPrefs.GetBool(key, defaultValue);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool HasKey(string key);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void DeleteKey(string key);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void DeleteAll();

		static EditorPrefs()
		{
			// Note: this type is marked as 'beforefieldinit'.
			EditorPrefs.onValueWasUpdated = null;
		}
	}
}
