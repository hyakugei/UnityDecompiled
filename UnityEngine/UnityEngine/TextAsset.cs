using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
	public class TextAsset : Object
	{
		public extern string text
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public extern byte[] bytes
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public TextAsset()
		{
			TextAsset.Internal_CreateInstance(this, null);
		}

		public TextAsset(string text)
		{
			TextAsset.Internal_CreateInstance(this, text);
		}

		public override string ToString()
		{
			return this.text;
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_CreateInstance([Writable] TextAsset self, string text);
	}
}
