using System;
using System.Runtime.InteropServices;
using UnityEngine.Bindings;

namespace UnityEditor.Experimental.AssetImporters
{
	[StructLayout(LayoutKind.Sequential)]
	public class SourceTextureInformation
	{
		[NativeName("width")]
		private int m_Width;

		[NativeName("height")]
		private int m_Height;

		[NativeName("sourceContainsAlpha")]
		private bool m_SourceContainsAlpha;

		[NativeName("sourceWasHDR")]
		private bool m_SourceWasHDR;

		public int width
		{
			get
			{
				return this.m_Width;
			}
			set
			{
				this.m_Width = value;
			}
		}

		public int height
		{
			get
			{
				return this.m_Height;
			}
			set
			{
				this.m_Height = value;
			}
		}

		public bool containsAlpha
		{
			get
			{
				return this.m_SourceContainsAlpha;
			}
			set
			{
				this.m_SourceContainsAlpha = value;
			}
		}

		public bool hdr
		{
			get
			{
				return this.m_SourceWasHDR;
			}
			set
			{
				this.m_SourceWasHDR = value;
			}
		}
	}
}
