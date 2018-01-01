using System;
using System.Runtime.CompilerServices;

namespace UnityEditor.PackageManager
{
	internal class NativeClient
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern NativeStatusCode List(out long operationId);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern NativeStatusCode Add(out long operationId, string packageId);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern NativeStatusCode Remove(out long operationId, string packageId);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern NativeStatusCode Search(out long operationId, string packageId);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern NativeStatusCode SearchAll(out long operationId);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern NativeStatusCode ResetToEditorDefaults(out long operationId);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern NativeStatusCode GetOperationStatus(long operationId);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern Error GetOperationError(long operationId);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern OperationStatus GetListOperationData(long operationId);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern PackageInfo GetAddOperationData(long operationId);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern string GetRemoveOperationData(long operationId);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern PackageInfo[] GetSearchOperationData(long operationId);
	}
}
