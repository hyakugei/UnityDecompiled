using System;

namespace UnityEngine.Experimental.UIElements
{
	public class FocusChangeDirection
	{
		private static readonly FocusChangeDirection s_Unspecified = new FocusChangeDirection(-1);

		private static readonly FocusChangeDirection s_None = new FocusChangeDirection(0);

		private int m_Value;

		public static FocusChangeDirection unspecified
		{
			get
			{
				return FocusChangeDirection.s_Unspecified;
			}
		}

		public static FocusChangeDirection none
		{
			get
			{
				return FocusChangeDirection.s_None;
			}
		}

		protected static FocusChangeDirection lastValue
		{
			get
			{
				return FocusChangeDirection.s_None;
			}
		}

		protected FocusChangeDirection(int value)
		{
			this.m_Value = value;
		}

		public static implicit operator int(FocusChangeDirection fcd)
		{
			return fcd.m_Value;
		}
	}
}
