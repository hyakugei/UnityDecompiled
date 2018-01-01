using System;
using UnityEngine.Bindings;

namespace UnityEditor
{
	[NativeType(Header = "Editor/Src/EditorUserBuildSettings.h")]
	public enum AndroidBuildSystem
	{
		Internal,
		Gradle,
		[Obsolete("ADT/eclipse project export for Android is no longer supported - please use Gradle export instead", true)]
		ADT,
		VisualStudio
	}
}
