using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityEditor.Experimental.U2D
{
	public static class SpriteEditorExtension
	{
		public static GUID GetSpriteID(this Sprite sprite)
		{
			return new GUID(SpriteEditorExtension.GetSpriteIDScripting(sprite));
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string GetSpriteIDScripting(Sprite sprite);
	}
}
