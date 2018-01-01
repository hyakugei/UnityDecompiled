using System;

namespace UnityEngine.Collections
{
	internal sealed class NativeSliceDebugView<T> where T : struct
	{
		private NativeSlice<T> array;

		public T[] Items
		{
			get
			{
				return this.array.ToArray();
			}
		}

		public NativeSliceDebugView(NativeSlice<T> array)
		{
			this.array = array;
		}
	}
}
