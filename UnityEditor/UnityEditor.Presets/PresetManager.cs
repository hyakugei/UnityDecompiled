using System;
using System.Runtime.CompilerServices;
using UnityEditorInternal;
using UnityEngine.Bindings;

namespace UnityEditor.Presets
{
	[NativeType(Header = "Modules/PresetsEditor/PresetManager.h")]
	internal class PresetManager : ProjectSettingsBase
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern string GetPresetTypeNameAtIndex(int index);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern bool SetAsDefaultInternal(Preset index);
	}
}
