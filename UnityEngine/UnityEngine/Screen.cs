using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;
using UnityEngine.Scripting;

namespace UnityEngine
{
	public sealed class Screen
	{
		public static extern Resolution[] resolutions
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public static Resolution currentResolution
		{
			get
			{
				Resolution result;
				Screen.INTERNAL_get_currentResolution(out result);
				return result;
			}
		}

		public static extern bool fullScreen
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern int width
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public static extern int height
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public static extern float dpi
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public static extern ScreenOrientation orientation
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern int sleepTimeout
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static bool autorotateToPortrait
		{
			get
			{
				return Screen.IsOrientationEnabled(EnabledOrientation.kAutorotateToPortrait);
			}
			set
			{
				Screen.SetOrientationEnabled(EnabledOrientation.kAutorotateToPortrait, value);
			}
		}

		public static bool autorotateToPortraitUpsideDown
		{
			get
			{
				return Screen.IsOrientationEnabled(EnabledOrientation.kAutorotateToPortraitUpsideDown);
			}
			set
			{
				Screen.SetOrientationEnabled(EnabledOrientation.kAutorotateToPortraitUpsideDown, value);
			}
		}

		public static bool autorotateToLandscapeLeft
		{
			get
			{
				return Screen.IsOrientationEnabled(EnabledOrientation.kAutorotateToLandscapeLeft);
			}
			set
			{
				Screen.SetOrientationEnabled(EnabledOrientation.kAutorotateToLandscapeLeft, value);
			}
		}

		public static bool autorotateToLandscapeRight
		{
			get
			{
				return Screen.IsOrientationEnabled(EnabledOrientation.kAutorotateToLandscapeRight);
			}
			set
			{
				Screen.SetOrientationEnabled(EnabledOrientation.kAutorotateToLandscapeRight, value);
			}
		}

		[EditorBrowsable(EditorBrowsableState.Never), Obsolete("Property GetResolution has been deprecated. Use resolutions instead (UnityUpgradable) -> resolutions", true)]
		public static Resolution[] GetResolution
		{
			get
			{
				return null;
			}
		}

		[EditorBrowsable(EditorBrowsableState.Never), Obsolete("Property showCursor has been deprecated. Use Cursor.visible instead (UnityUpgradable) -> UnityEngine.Cursor.visible", true)]
		public static bool showCursor
		{
			get;
			set;
		}

		[EditorBrowsable(EditorBrowsableState.Never), Obsolete("Use Cursor.lockState and Cursor.visible instead.", false)]
		public static bool lockCursor
		{
			get
			{
				return CursorLockMode.Locked == Cursor.lockState;
			}
			set
			{
				if (value)
				{
					Cursor.visible = false;
					Cursor.lockState = CursorLockMode.Locked;
				}
				else
				{
					Cursor.lockState = CursorLockMode.None;
					Cursor.visible = true;
				}
			}
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_get_currentResolution(out Resolution value);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetResolution(int width, int height, bool fullscreen, [UnityEngine.Internal.DefaultValue("0")] int preferredRefreshRate);

		[ExcludeFromDocs]
		public static void SetResolution(int width, int height, bool fullscreen)
		{
			int preferredRefreshRate = 0;
			Screen.SetResolution(width, height, fullscreen, preferredRefreshRate);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool IsOrientationEnabled(EnabledOrientation orient);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetOrientationEnabled(EnabledOrientation orient, bool enabled);
	}
}
