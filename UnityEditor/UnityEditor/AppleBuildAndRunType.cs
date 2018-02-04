using System;
using UnityEngine.Bindings;

namespace UnityEditor
{
	[NativeType(Header = "Editor/Src/EditorUserBuildSettings.h")]
	internal enum AppleBuildAndRunType
	{
		Xcode,
		Xcodebuild,
		iOSDeploy
	}
}
