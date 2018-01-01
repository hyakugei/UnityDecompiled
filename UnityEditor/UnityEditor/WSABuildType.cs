using System;
using UnityEngine.Bindings;

namespace UnityEditor
{
	[NativeType(Header = "Editor/Src/EditorUserBuildSettings.h")]
	public enum WSABuildType
	{
		Debug,
		Release,
		Master
	}
}
