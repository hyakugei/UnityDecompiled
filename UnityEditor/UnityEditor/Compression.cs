using System;
using UnityEngine.Bindings;

namespace UnityEditor
{
	[NativeType(Header = "Runtime/Serialize/BuildTarget.h")]
	internal enum Compression
	{
		None,
		Lz4 = 2,
		Lz4HC
	}
}
