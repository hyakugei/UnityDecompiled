using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityEditor
{
	[InitializeOnLoad]
	internal static class NativeFormatImporterUtility
	{
		private const string k_DefaultExtension = "asset";

		private static readonly Dictionary<Type, string[]> s_RegisteredExtensionsByType;

		static NativeFormatImporterUtility()
		{
			NativeFormatImporterUtility.s_RegisteredExtensionsByType = new Dictionary<Type, string[]>();
			foreach (Type current in EditorAssemblies.GetAllTypesWithAttribute<AssetFileNameExtensionAttribute>())
			{
				AssetFileNameExtensionAttribute assetFileNameExtensionAttribute = current.GetCustomAttributes(typeof(AssetFileNameExtensionAttribute), false)[0] as AssetFileNameExtensionAttribute;
				try
				{
					NativeFormatImporterUtility.RegisterExtensionForType(current, assetFileNameExtensionAttribute.preferredExtension, assetFileNameExtensionAttribute.otherExtensions.ToArray<string>());
				}
				catch (ArgumentException exception)
				{
					Debug.LogException(exception);
				}
				catch (NotSupportedException exception2)
				{
					Debug.LogException(exception2);
				}
			}
		}

		internal static void RegisterExtensionForType(Type type, string preferredExtension, params string[] otherExtensions)
		{
			if (!type.IsSubclassOf(typeof(ScriptableObject)))
			{
				throw new NotSupportedException(string.Format("{0} may only be added to {1} types.", typeof(AssetFileNameExtensionAttribute), typeof(ScriptableObject)));
			}
			if (NativeFormatImporterUtility.s_RegisteredExtensionsByType.ContainsKey(type))
			{
				throw new ArgumentException(string.Format("Extension already registered for type {0}.", type), "type");
			}
			string[] array = new string[otherExtensions.Length + 1];
			array[0] = NativeFormatImporterUtility.ValidateExtension(preferredExtension, type);
			int i = 0;
			int num = otherExtensions.Length;
			while (i < num)
			{
				array[i + 1] = NativeFormatImporterUtility.ValidateExtension(otherExtensions[i], type);
				i++;
			}
			NativeFormatImporterUtility.s_RegisteredExtensionsByType[type] = array;
		}

		private static string ValidateExtension(string extension, Type requestingType)
		{
			string extension2;
			if (string.Equals(extension, "asset", StringComparison.OrdinalIgnoreCase))
			{
				extension2 = extension;
			}
			else
			{
				KeyValuePair<Type, string[]> keyValuePair = NativeFormatImporterUtility.s_RegisteredExtensionsByType.FirstOrDefault((KeyValuePair<Type, string[]> kv) => kv.Value.Count((string ext) => string.Equals(ext, extension, StringComparison.OrdinalIgnoreCase)) > 0);
				if (keyValuePair.Key != null)
				{
					throw new ArgumentException(string.Format("Extension \"{0}\" is already registered for type {1}. It cannot also be used for {2}.", extension, keyValuePair.Key, requestingType), "extension");
				}
				bool flag;
				Type type = NativeFormatImporterUtility.Internal_GetNativeTypeForExtension(extension, out flag);
				if (!flag)
				{
					throw new ArgumentException(string.Format("Extension \"{0}\" must also be registered in NativeFormatImporterExtensions.h with identical capitalization.", extension), "extension");
				}
				if (type != null && type != requestingType)
				{
					throw new ArgumentException(string.Format("Extension \"{0}\" is registered for type {1}. It cannot also be used for {2}.", extension, type, requestingType), "extension");
				}
				extension2 = extension;
			}
			return extension2;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Type Internal_GetNativeTypeForExtension(string extension, out bool registered);

		internal static string GetExtensionForAsset(UnityEngine.Object asset)
		{
			Type type = asset.GetType();
			string result;
			if (!typeof(ScriptableObject).IsAssignableFrom(type))
			{
				result = NativeFormatImporterUtility.Internal_GetExtensionForNativeAsset(asset);
			}
			else
			{
				while (type != typeof(UnityEngine.Object))
				{
					foreach (KeyValuePair<Type, string[]> current in NativeFormatImporterUtility.s_RegisteredExtensionsByType)
					{
						if (current.Key == type)
						{
							result = current.Value[0];
							return result;
						}
					}
					type = type.BaseType;
				}
				result = "asset";
			}
			return result;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string Internal_GetExtensionForNativeAsset(UnityEngine.Object asset);
	}
}
