using System;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;
using UnityEngine.Scripting;

namespace UnityEngine
{
	public sealed class TouchScreenKeyboard
	{
		public enum Status
		{
			Visible,
			Done,
			Canceled,
			LostFocus
		}

		[NonSerialized]
		internal IntPtr m_Ptr;

		public static bool isSupported
		{
			get
			{
				RuntimePlatform platform = Application.platform;
				bool result;
				switch (platform)
				{
				case RuntimePlatform.MetroPlayerX86:
				case RuntimePlatform.MetroPlayerX64:
				case RuntimePlatform.MetroPlayerARM:
					result = false;
					return result;
				case RuntimePlatform.WP8Player:
				case RuntimePlatform.BB10Player:
					IL_28:
					switch (platform)
					{
					case RuntimePlatform.IPhonePlayer:
					case RuntimePlatform.Android:
						goto IL_55;
					case RuntimePlatform.PS3:
					case RuntimePlatform.XBOX360:
						IL_40:
						if (platform != RuntimePlatform.tvOS && platform != RuntimePlatform.Switch)
						{
							result = false;
							return result;
						}
						goto IL_55;
					}
					goto IL_40;
				case RuntimePlatform.TizenPlayer:
					goto IL_55;
				}
				goto IL_28;
				IL_55:
				result = true;
				return result;
			}
		}

		public extern string text
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern bool hideInput
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public extern bool active
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		[Obsolete("property done is deprecated, use status instead")]
		public extern bool done
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		[Obsolete("property wasCanceled is deprecated, use status instead.")]
		public extern bool wasCanceled
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public extern TouchScreenKeyboard.Status status
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public extern bool canGetSelection
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public extern bool canSetSelection
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public RangeInt selection
		{
			get
			{
				RangeInt result;
				this.GetSelectionInternal(out result.start, out result.length);
				return result;
			}
			set
			{
				this.SetSelectionInternal(value.start, value.length);
			}
		}

		public extern TouchScreenKeyboardType type
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public extern int targetDisplay
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static Rect area
		{
			get
			{
				Rect result;
				TouchScreenKeyboard.INTERNAL_get_area(out result);
				return result;
			}
		}

		public static extern bool visible
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public TouchScreenKeyboard(string text, TouchScreenKeyboardType keyboardType, bool autocorrection, bool multiline, bool secure, bool alert, string textPlaceholder, int characterLimit)
		{
			TouchScreenKeyboard_InternalConstructorHelperArguments touchScreenKeyboard_InternalConstructorHelperArguments = default(TouchScreenKeyboard_InternalConstructorHelperArguments);
			touchScreenKeyboard_InternalConstructorHelperArguments.keyboardType = Convert.ToUInt32(keyboardType);
			touchScreenKeyboard_InternalConstructorHelperArguments.autocorrection = Convert.ToUInt32(autocorrection);
			touchScreenKeyboard_InternalConstructorHelperArguments.multiline = Convert.ToUInt32(multiline);
			touchScreenKeyboard_InternalConstructorHelperArguments.secure = Convert.ToUInt32(secure);
			touchScreenKeyboard_InternalConstructorHelperArguments.alert = Convert.ToUInt32(alert);
			touchScreenKeyboard_InternalConstructorHelperArguments.characterLimit = characterLimit;
			this.TouchScreenKeyboard_InternalConstructorHelper(ref touchScreenKeyboard_InternalConstructorHelperArguments, text, textPlaceholder);
		}

		[GeneratedByOldBindingsGenerator, ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Destroy();

		~TouchScreenKeyboard()
		{
			this.Destroy();
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void TouchScreenKeyboard_InternalConstructorHelper(ref TouchScreenKeyboard_InternalConstructorHelperArguments arguments, string text, string textPlaceholder);

		[ExcludeFromDocs]
		public static TouchScreenKeyboard Open(string text, TouchScreenKeyboardType keyboardType, bool autocorrection, bool multiline, bool secure, bool alert, string textPlaceholder)
		{
			int characterLimit = 0;
			return TouchScreenKeyboard.Open(text, keyboardType, autocorrection, multiline, secure, alert, textPlaceholder, characterLimit);
		}

		[ExcludeFromDocs]
		public static TouchScreenKeyboard Open(string text, TouchScreenKeyboardType keyboardType, bool autocorrection, bool multiline, bool secure, bool alert)
		{
			int characterLimit = 0;
			string textPlaceholder = "";
			return TouchScreenKeyboard.Open(text, keyboardType, autocorrection, multiline, secure, alert, textPlaceholder, characterLimit);
		}

		[ExcludeFromDocs]
		public static TouchScreenKeyboard Open(string text, TouchScreenKeyboardType keyboardType, bool autocorrection, bool multiline, bool secure)
		{
			int characterLimit = 0;
			string textPlaceholder = "";
			bool alert = false;
			return TouchScreenKeyboard.Open(text, keyboardType, autocorrection, multiline, secure, alert, textPlaceholder, characterLimit);
		}

		[ExcludeFromDocs]
		public static TouchScreenKeyboard Open(string text, TouchScreenKeyboardType keyboardType, bool autocorrection, bool multiline)
		{
			int characterLimit = 0;
			string textPlaceholder = "";
			bool alert = false;
			bool secure = false;
			return TouchScreenKeyboard.Open(text, keyboardType, autocorrection, multiline, secure, alert, textPlaceholder, characterLimit);
		}

		[ExcludeFromDocs]
		public static TouchScreenKeyboard Open(string text, TouchScreenKeyboardType keyboardType, bool autocorrection)
		{
			int characterLimit = 0;
			string textPlaceholder = "";
			bool alert = false;
			bool secure = false;
			bool multiline = false;
			return TouchScreenKeyboard.Open(text, keyboardType, autocorrection, multiline, secure, alert, textPlaceholder, characterLimit);
		}

		[ExcludeFromDocs]
		public static TouchScreenKeyboard Open(string text, TouchScreenKeyboardType keyboardType)
		{
			int characterLimit = 0;
			string textPlaceholder = "";
			bool alert = false;
			bool secure = false;
			bool multiline = false;
			bool autocorrection = true;
			return TouchScreenKeyboard.Open(text, keyboardType, autocorrection, multiline, secure, alert, textPlaceholder, characterLimit);
		}

		[ExcludeFromDocs]
		public static TouchScreenKeyboard Open(string text)
		{
			int characterLimit = 0;
			string textPlaceholder = "";
			bool alert = false;
			bool secure = false;
			bool multiline = false;
			bool autocorrection = true;
			TouchScreenKeyboardType keyboardType = TouchScreenKeyboardType.Default;
			return TouchScreenKeyboard.Open(text, keyboardType, autocorrection, multiline, secure, alert, textPlaceholder, characterLimit);
		}

		public static TouchScreenKeyboard Open(string text, [DefaultValue("TouchScreenKeyboardType.Default")] TouchScreenKeyboardType keyboardType, [DefaultValue("true")] bool autocorrection, [DefaultValue("false")] bool multiline, [DefaultValue("false")] bool secure, [DefaultValue("false")] bool alert, [DefaultValue("\"\"")] string textPlaceholder, [DefaultValue("0")] int characterLimit)
		{
			return new TouchScreenKeyboard(text, keyboardType, autocorrection, multiline, secure, alert, textPlaceholder, characterLimit);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetSelectionInternal(out int start, out int length);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetSelectionInternal(int start, int length);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_get_area(out Rect value);
	}
}
