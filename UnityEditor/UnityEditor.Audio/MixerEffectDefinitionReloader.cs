using System;
using System.Runtime.CompilerServices;

namespace UnityEditor.Audio
{
	[InitializeOnLoad]
	internal static class MixerEffectDefinitionReloader
	{
		[CompilerGenerated]
		private static Action <>f__mg$cache0;

		static MixerEffectDefinitionReloader()
		{
			MixerEffectDefinitions.Refresh();
			if (MixerEffectDefinitionReloader.<>f__mg$cache0 == null)
			{
				MixerEffectDefinitionReloader.<>f__mg$cache0 = new Action(MixerEffectDefinitionReloader.OnProjectChanged);
			}
			EditorApplication.projectChanged += MixerEffectDefinitionReloader.<>f__mg$cache0;
		}

		private static void OnProjectChanged()
		{
			MixerEffectDefinitions.Refresh();
		}
	}
}
