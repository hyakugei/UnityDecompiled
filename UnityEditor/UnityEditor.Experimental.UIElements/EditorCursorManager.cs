using System;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace UnityEditor.Experimental.UIElements
{
	internal class EditorCursorManager : ICursorManager
	{
		public void SetCursor(CursorStyle cursor)
		{
			if (cursor.texture != null)
			{
				EditorGUIUtility.SetCurrentViewCursor(cursor.texture, cursor.hotspot, MouseCursor.CustomCursor);
			}
			else
			{
				EditorGUIUtility.SetCurrentViewCursor(null, Vector2.zero, (MouseCursor)cursor.defaultCursorId);
			}
		}

		public void ResetCursor()
		{
			EditorGUIUtility.ClearCurrentViewCursor();
		}
	}
}
