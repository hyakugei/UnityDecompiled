using System;
using UnityEngine.Bindings;

namespace UnityEditor
{
	[NativeType(Header = "Editor/Src/EditorUserBuildSettings.h")]
	public enum AndroidBuildType
	{
		Debug,
		Development,
		Release
	}
}
