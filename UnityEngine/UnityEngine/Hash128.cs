using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
	[UsedByNativeCode]
	public struct Hash128
	{
		private uint m_u32_0;

		private uint m_u32_1;

		private uint m_u32_2;

		private uint m_u32_3;

		public bool isValid
		{
			get
			{
				return this.m_u32_0 != 0u || this.m_u32_1 != 0u || this.m_u32_2 != 0u || this.m_u32_3 != 0u;
			}
		}

		public Hash128(uint u32_0, uint u32_1, uint u32_2, uint u32_3)
		{
			this.m_u32_0 = u32_0;
			this.m_u32_1 = u32_1;
			this.m_u32_2 = u32_2;
			this.m_u32_3 = u32_3;
		}

		public override string ToString()
		{
			return Hash128.Internal_Hash128ToString(this);
		}

		public static Hash128 Parse(string hashString)
		{
			Hash128 result;
			Hash128.Parse_Injected(hashString, out result);
			return result;
		}

		internal static string Internal_Hash128ToString(Hash128 hash128)
		{
			return Hash128.Internal_Hash128ToString_Injected(ref hash128);
		}

		public override bool Equals(object obj)
		{
			return obj is Hash128 && this == (Hash128)obj;
		}

		public override int GetHashCode()
		{
			return this.m_u32_0.GetHashCode() ^ this.m_u32_1.GetHashCode() ^ this.m_u32_2.GetHashCode() ^ this.m_u32_3.GetHashCode();
		}

		public static bool operator ==(Hash128 hash1, Hash128 hash2)
		{
			return hash1.m_u32_0 == hash2.m_u32_0 && hash1.m_u32_1 == hash2.m_u32_1 && hash1.m_u32_2 == hash2.m_u32_2 && hash1.m_u32_3 == hash2.m_u32_3;
		}

		public static bool operator !=(Hash128 hash1, Hash128 hash2)
		{
			return !(hash1 == hash2);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Parse_Injected(string hashString, out Hash128 ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string Internal_Hash128ToString_Injected(ref Hash128 hash128);
	}
}
