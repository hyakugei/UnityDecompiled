using System;
using UnityEngine.Bindings;

namespace UnityEngine.Experimental.U2D
{
	internal struct SpriteChannelInfo
	{
		[NativeName("buffer")]
		private IntPtr m_Buffer;

		[NativeName("count")]
		private int m_Count;

		[NativeName("offset")]
		private int m_Offset;

		[NativeName("stride")]
		private int m_Stride;

		public IntPtr buffer
		{
			get
			{
				return this.m_Buffer;
			}
			set
			{
				this.m_Buffer = value;
			}
		}

		public int count
		{
			get
			{
				return this.m_Count;
			}
			set
			{
				this.m_Count = value;
			}
		}

		public int offset
		{
			get
			{
				return this.m_Offset;
			}
			set
			{
				this.m_Offset = value;
			}
		}

		public int stride
		{
			get
			{
				return this.m_Stride;
			}
			set
			{
				this.m_Stride = value;
			}
		}
	}
}
