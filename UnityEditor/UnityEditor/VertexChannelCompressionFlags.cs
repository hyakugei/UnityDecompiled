using System;

namespace UnityEditor
{
	[Flags]
	public enum VertexChannelCompressionFlags
	{
		None = 0,
		Position = 1,
		Normal = 2,
		Tangent = 4,
		Color = 8,
		TexCoord0 = 16,
		TexCoord1 = 32,
		TexCoord2 = 64,
		TexCoord3 = 128,
		[Obsolete("Use Position instead (UnityUpgradable) -> Position")]
		kPosition = 1,
		[Obsolete("Use Normal instead (UnityUpgradable) -> Normal")]
		kNormal = 2,
		[Obsolete("Use Color instead (UnityUpgradable) -> Color")]
		kColor = 4,
		[Obsolete("Use TexCoord0 instead (UnityUpgradable) -> TexCoord0")]
		kUV0 = 8,
		[Obsolete("Use TexCoord1 instead (UnityUpgradable) -> TexCoord1")]
		kUV1 = 16,
		[Obsolete("Use TexCoord2 instead (UnityUpgradable) -> TexCoord2")]
		kUV2 = 32,
		[Obsolete("Use TexCoord3 instead (UnityUpgradable) -> TexCoord3")]
		kUV3 = 64,
		[Obsolete("Use Tangent instead (UnityUpgradable) -> Tangent")]
		kTangent = 128
	}
}
