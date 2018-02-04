using System;
using UnityEngine.Bindings;

namespace UnityEditor
{
	[NativeType(Header = "Editor/Src/EditorUserBuildSettings.h")]
	public enum WSASDK
	{
		SDK80,
		SDK81,
		PhoneSDK81,
		UniversalSDK81,
		UWP
	}
}
