using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
	internal sealed class AudioExtensionManager
	{
		private static List<AudioSpatializerExtensionDefinition> m_ListenerSpatializerExtensionDefinitions = new List<AudioSpatializerExtensionDefinition>();

		private static List<AudioSpatializerExtensionDefinition> m_SourceSpatializerExtensionDefinitions = new List<AudioSpatializerExtensionDefinition>();

		private static List<AudioAmbisonicExtensionDefinition> m_SourceAmbisonicDecoderExtensionDefinitions = new List<AudioAmbisonicExtensionDefinition>();

		private static List<AudioSourceExtension> m_SourceExtensionsToUpdate = new List<AudioSourceExtension>();

		private static int m_NextStopIndex = 0;

		private static bool m_BuiltinDefinitionsRegistered = false;

		private static PropertyName m_SpatializerName = 0;

		private static PropertyName m_SpatializerExtensionName = 0;

		private static PropertyName m_ListenerSpatializerExtensionName = 0;

		internal static bool IsListenerSpatializerExtensionRegistered()
		{
			bool result;
			foreach (AudioSpatializerExtensionDefinition current in AudioExtensionManager.m_ListenerSpatializerExtensionDefinitions)
			{
				if (AudioSettings.GetSpatializerPluginName() == current.spatializerName)
				{
					result = true;
					return result;
				}
			}
			result = false;
			return result;
		}

		internal static bool IsSourceSpatializerExtensionRegistered()
		{
			bool result;
			foreach (AudioSpatializerExtensionDefinition current in AudioExtensionManager.m_SourceSpatializerExtensionDefinitions)
			{
				if (AudioSettings.GetSpatializerPluginName() == current.spatializerName)
				{
					result = true;
					return result;
				}
			}
			result = false;
			return result;
		}

		internal static bool IsSourceAmbisonicDecoderExtensionRegistered()
		{
			bool result;
			foreach (AudioAmbisonicExtensionDefinition current in AudioExtensionManager.m_SourceAmbisonicDecoderExtensionDefinitions)
			{
				if (AudioSettings.GetAmbisonicDecoderPluginName() == current.ambisonicPluginName)
				{
					result = true;
					return result;
				}
			}
			result = false;
			return result;
		}

		internal static AudioSourceExtension AddSpatializerExtension(AudioSource source)
		{
			AudioSourceExtension result;
			if (!source.spatialize)
			{
				result = null;
			}
			else if (source.spatializerExtension != null)
			{
				result = source.spatializerExtension;
			}
			else
			{
				AudioExtensionManager.RegisterBuiltinDefinitions();
				foreach (AudioSpatializerExtensionDefinition current in AudioExtensionManager.m_SourceSpatializerExtensionDefinitions)
				{
					if (AudioSettings.GetSpatializerPluginName() == current.spatializerName)
					{
						AudioSourceExtension audioSourceExtension = source.AddSpatializerExtension(current.definition.GetExtensionType());
						if (audioSourceExtension != null)
						{
							audioSourceExtension.audioSource = source;
							source.spatializerExtension = audioSourceExtension;
							AudioExtensionManager.WriteExtensionProperties(audioSourceExtension, current.definition.GetExtensionType().Name);
							result = audioSourceExtension;
							return result;
						}
					}
				}
				result = null;
			}
			return result;
		}

		internal static AudioSourceExtension AddAmbisonicDecoderExtension(AudioSource source)
		{
			AudioSourceExtension result;
			if (source.ambisonicExtension != null)
			{
				result = source.ambisonicExtension;
			}
			else
			{
				AudioExtensionManager.RegisterBuiltinDefinitions();
				foreach (AudioAmbisonicExtensionDefinition current in AudioExtensionManager.m_SourceAmbisonicDecoderExtensionDefinitions)
				{
					if (AudioSettings.GetAmbisonicDecoderPluginName() == current.ambisonicPluginName)
					{
						AudioSourceExtension audioSourceExtension = source.AddAmbisonicExtension(current.definition.GetExtensionType());
						if (audioSourceExtension != null)
						{
							audioSourceExtension.audioSource = source;
							source.ambisonicExtension = audioSourceExtension;
							result = audioSourceExtension;
							return result;
						}
					}
				}
				result = null;
			}
			return result;
		}

		internal static void WriteExtensionProperties(AudioSourceExtension extension, string extensionName)
		{
			if (AudioExtensionManager.m_SpatializerExtensionName == 0)
			{
				AudioExtensionManager.m_SpatializerExtensionName = extensionName;
			}
			for (int i = 0; i < extension.audioSource.GetNumExtensionProperties(); i++)
			{
				if (extension.audioSource.ReadExtensionName(i) == AudioExtensionManager.m_SpatializerExtensionName)
				{
					PropertyName propertyName = extension.audioSource.ReadExtensionPropertyName(i);
					float propertyValue = extension.audioSource.ReadExtensionPropertyValue(i);
					extension.WriteExtensionProperty(propertyName, propertyValue);
				}
			}
		}

		internal static AudioListenerExtension AddSpatializerExtension(AudioListener listener)
		{
			AudioListenerExtension result;
			if (listener.spatializerExtension != null)
			{
				result = listener.spatializerExtension;
			}
			else
			{
				AudioExtensionManager.RegisterBuiltinDefinitions();
				foreach (AudioSpatializerExtensionDefinition current in AudioExtensionManager.m_ListenerSpatializerExtensionDefinitions)
				{
					if (AudioSettings.GetSpatializerPluginName() == current.spatializerName || AudioSettings.GetAmbisonicDecoderPluginName() == current.spatializerName)
					{
						AudioListenerExtension audioListenerExtension = listener.AddExtension(current.definition.GetExtensionType());
						if (audioListenerExtension != null)
						{
							audioListenerExtension.audioListener = listener;
							listener.spatializerExtension = audioListenerExtension;
							AudioExtensionManager.WriteExtensionProperties(audioListenerExtension, current.definition.GetExtensionType().Name);
							result = audioListenerExtension;
							return result;
						}
					}
				}
				result = null;
			}
			return result;
		}

		internal static void WriteExtensionProperties(AudioListenerExtension extension, string extensionName)
		{
			if (AudioExtensionManager.m_ListenerSpatializerExtensionName == 0)
			{
				AudioExtensionManager.m_ListenerSpatializerExtensionName = extensionName;
			}
			for (int i = 0; i < extension.audioListener.GetNumExtensionProperties(); i++)
			{
				if (extension.audioListener.ReadExtensionName(i) == AudioExtensionManager.m_ListenerSpatializerExtensionName)
				{
					PropertyName propertyName = extension.audioListener.ReadExtensionPropertyName(i);
					float propertyValue = extension.audioListener.ReadExtensionPropertyValue(i);
					extension.WriteExtensionProperty(propertyName, propertyValue);
				}
			}
		}

		internal static AudioListenerExtension GetSpatializerExtension(AudioListener listener)
		{
			AudioListenerExtension result;
			if (listener.spatializerExtension != null)
			{
				result = listener.spatializerExtension;
			}
			else
			{
				result = null;
			}
			return result;
		}

		internal static AudioSourceExtension GetSpatializerExtension(AudioSource source)
		{
			return (!source.spatialize) ? null : source.spatializerExtension;
		}

		internal static AudioSourceExtension GetAmbisonicExtension(AudioSource source)
		{
			return source.ambisonicExtension;
		}

		internal static Type GetListenerSpatializerExtensionType()
		{
			Type result;
			foreach (AudioSpatializerExtensionDefinition current in AudioExtensionManager.m_ListenerSpatializerExtensionDefinitions)
			{
				if (AudioSettings.GetSpatializerPluginName() == current.spatializerName)
				{
					result = current.definition.GetExtensionType();
					return result;
				}
			}
			result = null;
			return result;
		}

		internal static Type GetListenerSpatializerExtensionEditorType()
		{
			Type result;
			foreach (AudioSpatializerExtensionDefinition current in AudioExtensionManager.m_ListenerSpatializerExtensionDefinitions)
			{
				if (AudioSettings.GetSpatializerPluginName() == current.spatializerName)
				{
					result = current.editorDefinition.GetExtensionType();
					return result;
				}
			}
			result = null;
			return result;
		}

		internal static Type GetSourceSpatializerExtensionType()
		{
			Type result;
			foreach (AudioSpatializerExtensionDefinition current in AudioExtensionManager.m_SourceSpatializerExtensionDefinitions)
			{
				if (AudioSettings.GetSpatializerPluginName() == current.spatializerName)
				{
					result = current.definition.GetExtensionType();
					return result;
				}
			}
			result = null;
			return result;
		}

		internal static Type GetSourceSpatializerExtensionEditorType()
		{
			Type result;
			foreach (AudioSpatializerExtensionDefinition current in AudioExtensionManager.m_SourceSpatializerExtensionDefinitions)
			{
				if (AudioSettings.GetSpatializerPluginName() == current.spatializerName)
				{
					result = current.editorDefinition.GetExtensionType();
					return result;
				}
			}
			result = null;
			return result;
		}

		internal static Type GetSourceAmbisonicExtensionType()
		{
			Type result;
			foreach (AudioAmbisonicExtensionDefinition current in AudioExtensionManager.m_SourceAmbisonicDecoderExtensionDefinitions)
			{
				if (AudioSettings.GetAmbisonicDecoderPluginName() == current.ambisonicPluginName)
				{
					result = current.definition.GetExtensionType();
					return result;
				}
			}
			result = null;
			return result;
		}

		internal static PropertyName GetSpatializerName()
		{
			return AudioExtensionManager.m_SpatializerName;
		}

		internal static PropertyName GetSourceSpatializerExtensionName()
		{
			return AudioExtensionManager.m_SpatializerExtensionName;
		}

		internal static PropertyName GetListenerSpatializerExtensionName()
		{
			return AudioExtensionManager.m_ListenerSpatializerExtensionName;
		}

		internal static void AddExtensionToManager(AudioSourceExtension extension)
		{
			AudioExtensionManager.RegisterBuiltinDefinitions();
			if (extension.m_ExtensionManagerUpdateIndex == -1)
			{
				AudioExtensionManager.m_SourceExtensionsToUpdate.Add(extension);
				extension.m_ExtensionManagerUpdateIndex = AudioExtensionManager.m_SourceExtensionsToUpdate.Count - 1;
			}
		}

		internal static void RemoveExtensionFromManager(AudioSourceExtension extension)
		{
			int extensionManagerUpdateIndex = extension.m_ExtensionManagerUpdateIndex;
			if (extensionManagerUpdateIndex >= 0 && extensionManagerUpdateIndex < AudioExtensionManager.m_SourceExtensionsToUpdate.Count)
			{
				int index = AudioExtensionManager.m_SourceExtensionsToUpdate.Count - 1;
				AudioExtensionManager.m_SourceExtensionsToUpdate[extensionManagerUpdateIndex] = AudioExtensionManager.m_SourceExtensionsToUpdate[index];
				AudioExtensionManager.m_SourceExtensionsToUpdate[extensionManagerUpdateIndex].m_ExtensionManagerUpdateIndex = extensionManagerUpdateIndex;
				AudioExtensionManager.m_SourceExtensionsToUpdate.RemoveAt(index);
			}
			extension.m_ExtensionManagerUpdateIndex = -1;
		}

		internal static void Update()
		{
			AudioExtensionManager.RegisterBuiltinDefinitions();
			if (AudioExtensionManager.m_SpatializerName != AudioSettings.GetSpatializerPluginName())
			{
				AudioExtensionManager.m_SpatializerName = AudioSettings.GetSpatializerPluginName();
				if (AudioExtensionManager.GetSourceSpatializerExtensionType() != null)
				{
					AudioExtensionManager.m_SpatializerExtensionName = AudioExtensionManager.GetSourceSpatializerExtensionType().Name;
				}
				if (AudioExtensionManager.GetListenerSpatializerExtensionEditorType() != null)
				{
					AudioExtensionManager.m_ListenerSpatializerExtensionName = AudioExtensionManager.GetListenerSpatializerExtensionType().Name;
				}
			}
			AudioListener audioListener = AudioExtensionManager.GetAudioListener() as AudioListener;
			if (audioListener != null)
			{
				AudioListenerExtension audioListenerExtension = AudioExtensionManager.AddSpatializerExtension(audioListener);
				if (audioListenerExtension != null)
				{
					audioListenerExtension.ExtensionUpdate();
				}
			}
			for (int i = 0; i < AudioExtensionManager.m_SourceExtensionsToUpdate.Count; i++)
			{
				AudioExtensionManager.m_SourceExtensionsToUpdate[i].ExtensionUpdate();
			}
			AudioExtensionManager.m_NextStopIndex = ((AudioExtensionManager.m_NextStopIndex < AudioExtensionManager.m_SourceExtensionsToUpdate.Count) ? AudioExtensionManager.m_NextStopIndex : 0);
			int num = (AudioExtensionManager.m_SourceExtensionsToUpdate.Count <= 0) ? 0 : (1 + AudioExtensionManager.m_SourceExtensionsToUpdate.Count / 8);
			for (int j = 0; j < num; j++)
			{
				AudioSourceExtension audioSourceExtension = AudioExtensionManager.m_SourceExtensionsToUpdate[AudioExtensionManager.m_NextStopIndex];
				if (audioSourceExtension.audioSource == null || !audioSourceExtension.audioSource.enabled || !audioSourceExtension.audioSource.isPlaying)
				{
					audioSourceExtension.Stop();
					AudioExtensionManager.RemoveExtensionFromManager(audioSourceExtension);
				}
				else
				{
					AudioExtensionManager.m_NextStopIndex++;
					AudioExtensionManager.m_NextStopIndex = ((AudioExtensionManager.m_NextStopIndex < AudioExtensionManager.m_SourceExtensionsToUpdate.Count) ? AudioExtensionManager.m_NextStopIndex : 0);
				}
			}
		}

		internal static void GetReadyToPlay(AudioSourceExtension extension)
		{
			if (extension != null)
			{
				extension.Play();
				AudioExtensionManager.AddExtensionToManager(extension);
			}
		}

		private static void RegisterBuiltinDefinitions()
		{
			bool flag = true;
			if (!AudioExtensionManager.m_BuiltinDefinitionsRegistered)
			{
				if (flag || AudioSettings.GetSpatializerPluginName() == "GVR Audio Spatializer")
				{
				}
				if (flag || AudioSettings.GetAmbisonicDecoderPluginName() == "GVR Audio Spatializer")
				{
				}
				AudioExtensionManager.m_BuiltinDefinitionsRegistered = true;
			}
		}

		private static bool RegisterListenerSpatializerDefinition(string spatializerName, AudioExtensionDefinition extensionDefinition, AudioExtensionDefinition editorDefinition)
		{
			bool result;
			foreach (AudioSpatializerExtensionDefinition current in AudioExtensionManager.m_ListenerSpatializerExtensionDefinitions)
			{
				if (spatializerName == current.spatializerName)
				{
					Debug.Log("RegisterListenerSpatializerDefinition failed for " + extensionDefinition.GetExtensionType() + ". We only allow one audio listener extension to be registered for each spatializer.");
					result = false;
					return result;
				}
			}
			AudioSpatializerExtensionDefinition item = new AudioSpatializerExtensionDefinition(spatializerName, extensionDefinition, editorDefinition);
			AudioExtensionManager.m_ListenerSpatializerExtensionDefinitions.Add(item);
			result = true;
			return result;
		}

		private static bool RegisterSourceSpatializerDefinition(string spatializerName, AudioExtensionDefinition extensionDefinition, AudioExtensionDefinition editorDefinition)
		{
			bool result;
			foreach (AudioSpatializerExtensionDefinition current in AudioExtensionManager.m_SourceSpatializerExtensionDefinitions)
			{
				if (spatializerName == current.spatializerName)
				{
					Debug.Log("RegisterSourceSpatializerDefinition failed for " + extensionDefinition.GetExtensionType() + ". We only allow one audio source extension to be registered for each spatializer.");
					result = false;
					return result;
				}
			}
			AudioSpatializerExtensionDefinition item = new AudioSpatializerExtensionDefinition(spatializerName, extensionDefinition, editorDefinition);
			AudioExtensionManager.m_SourceSpatializerExtensionDefinitions.Add(item);
			result = true;
			return result;
		}

		private static bool RegisterSourceAmbisonicDefinition(string ambisonicDecoderName, AudioExtensionDefinition extensionDefinition)
		{
			bool result;
			foreach (AudioAmbisonicExtensionDefinition current in AudioExtensionManager.m_SourceAmbisonicDecoderExtensionDefinitions)
			{
				if (ambisonicDecoderName == current.ambisonicPluginName)
				{
					Debug.Log("RegisterSourceAmbisonicDefinition failed for " + extensionDefinition.GetExtensionType() + ". We only allow one audio source extension to be registered for each ambisonic decoder.");
					result = false;
					return result;
				}
			}
			AudioAmbisonicExtensionDefinition item = new AudioAmbisonicExtensionDefinition(ambisonicDecoderName, extensionDefinition);
			AudioExtensionManager.m_SourceAmbisonicDecoderExtensionDefinitions.Add(item);
			result = true;
			return result;
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern Object GetAudioListener();
	}
}
