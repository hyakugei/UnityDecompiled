using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public struct ExposedPropertyResolver
	{
		internal IntPtr table;

		internal static Object ResolveReferenceInternal(IntPtr ptr, PropertyName name, out bool isValid)
		{
			if (ptr == IntPtr.Zero)
			{
				throw new ArgumentNullException("Argument \"ptr\" can't be null.");
			}
			return ExposedPropertyResolver.ResolveReferenceBindingsInternal(ptr, name, out isValid);
		}

		private static Object ResolveReferenceBindingsInternal(IntPtr ptr, PropertyName name, out bool isValid)
		{
			return ExposedPropertyResolver.ResolveReferenceBindingsInternal_Injected(ptr, ref name, out isValid);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Object ResolveReferenceBindingsInternal_Injected(IntPtr ptr, ref PropertyName name, out bool isValid);
	}
}
