using System;
using UnityEngine.Bindings;

namespace UnityEditor
{
	[NativeType(Header = "Editor/Src/EditorUserBuildSettings.h")]
	public enum WSAUWPBuildType
	{
		XAML,
		D3D
	}
}
