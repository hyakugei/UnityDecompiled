using System;

namespace UnityEngine.Experimental.UIElements
{
	public struct CursorStyle
	{
		public Texture2D texture
		{
			get;
			set;
		}

		public Vector2 hotspot
		{
			get;
			set;
		}

		internal int defaultCursorId
		{
			get;
			set;
		}

		public override int GetHashCode()
		{
			return this.texture.GetHashCode() ^ this.hotspot.GetHashCode() ^ this.defaultCursorId.GetHashCode();
		}

		public override bool Equals(object other)
		{
			bool result;
			if (!(other is CursorStyle))
			{
				result = false;
			}
			else
			{
				CursorStyle cursorStyle = (CursorStyle)other;
				result = (this.texture.Equals(cursorStyle.texture) && this.hotspot.Equals(cursorStyle.hotspot) && this.defaultCursorId == cursorStyle.defaultCursorId);
			}
			return result;
		}

		public static bool operator ==(CursorStyle lhs, CursorStyle rhs)
		{
			return lhs.texture == rhs.texture && lhs.hotspot == rhs.hotspot;
		}

		public static bool operator !=(CursorStyle lhs, CursorStyle rhs)
		{
			return !(lhs == rhs);
		}
	}
}
