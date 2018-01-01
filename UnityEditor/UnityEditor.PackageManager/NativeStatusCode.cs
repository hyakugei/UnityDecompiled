using System;

namespace UnityEditor.PackageManager
{
	internal enum NativeStatusCode
	{
		InQueue,
		InProgress,
		Done,
		Error,
		NotFound
	}
}
