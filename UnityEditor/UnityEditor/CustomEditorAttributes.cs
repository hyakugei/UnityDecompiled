using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Rendering;

namespace UnityEditor
{
	internal class CustomEditorAttributes
	{
		private class MonoEditorType
		{
			public Type m_InspectedType;

			public Type m_InspectorType;

			public Type m_RenderPipelineType;

			public bool m_EditorForChildClasses;

			public bool m_IsFallback;
		}

		private static readonly Dictionary<Type, List<CustomEditorAttributes.MonoEditorType>> kSCustomEditors = new Dictionary<Type, List<CustomEditorAttributes.MonoEditorType>>();

		private static readonly Dictionary<Type, List<CustomEditorAttributes.MonoEditorType>> kSCustomMultiEditors = new Dictionary<Type, List<CustomEditorAttributes.MonoEditorType>>();

		private static bool s_Initialized;

		private static List<CustomEditorAttributes.MonoEditorType> s_SearchCache = new List<CustomEditorAttributes.MonoEditorType>();

		internal static Type FindCustomEditorType(UnityEngine.Object o, bool multiEdit)
		{
			return CustomEditorAttributes.FindCustomEditorTypeByType(o.GetType(), multiEdit);
		}

		internal static Type FindCustomEditorTypeByType(Type type, bool multiEdit)
		{
			if (!CustomEditorAttributes.s_Initialized)
			{
				Assembly[] loadedAssemblies = EditorAssemblies.loadedAssemblies;
				for (int i = loadedAssemblies.Length - 1; i >= 0; i--)
				{
					CustomEditorAttributes.Rebuild(loadedAssemblies[i]);
				}
				CustomEditorAttributes.s_Initialized = true;
			}
			Type result;
			if (type == null)
			{
				result = null;
			}
			else
			{
				Dictionary<Type, List<CustomEditorAttributes.MonoEditorType>> dictionary = (!multiEdit) ? CustomEditorAttributes.kSCustomEditors : CustomEditorAttributes.kSCustomMultiEditors;
				for (int j = 0; j < 2; j++)
				{
					Type type2 = type;
					while (type2 != null)
					{
						List<CustomEditorAttributes.MonoEditorType> list;
						if (dictionary.TryGetValue(type2, out list))
						{
							goto IL_AB;
						}
						if (type2.IsGenericType)
						{
							type2 = type2.GetGenericTypeDefinition();
							if (dictionary.TryGetValue(type2, out list))
							{
								goto IL_AB;
							}
						}
						IL_20E:
						type2 = type2.BaseType;
						continue;
						IL_AB:
						CustomEditorAttributes.s_SearchCache.Clear();
						foreach (CustomEditorAttributes.MonoEditorType current in list)
						{
							if (CustomEditorAttributes.IsAppropriateEditor(current, type2, type != type2, j == 1))
							{
								CustomEditorAttributes.s_SearchCache.Add(current);
							}
						}
						Type type3 = null;
						if (GraphicsSettings.renderPipelineAsset != null)
						{
							Type type4 = GraphicsSettings.renderPipelineAsset.GetType();
							foreach (CustomEditorAttributes.MonoEditorType current2 in CustomEditorAttributes.s_SearchCache)
							{
								if (current2.m_RenderPipelineType == type4)
								{
									type3 = current2.m_InspectorType;
									break;
								}
							}
						}
						if (type3 == null)
						{
							foreach (CustomEditorAttributes.MonoEditorType current3 in CustomEditorAttributes.s_SearchCache)
							{
								if (current3.m_RenderPipelineType == null)
								{
									type3 = current3.m_InspectorType;
									break;
								}
							}
						}
						CustomEditorAttributes.s_SearchCache.Clear();
						if (type3 != null)
						{
							result = type3;
							return result;
						}
						goto IL_20E;
					}
				}
				result = null;
			}
			return result;
		}

		private static bool IsAppropriateEditor(CustomEditorAttributes.MonoEditorType editor, Type parentClass, bool isChildClass, bool isFallback)
		{
			return (!isChildClass || editor.m_EditorForChildClasses) && isFallback == editor.m_IsFallback && (parentClass == editor.m_InspectedType || (parentClass.IsGenericType && parentClass.GetGenericTypeDefinition() == editor.m_InspectedType));
		}

		internal static void Rebuild(Assembly assembly)
		{
			Type[] typesFromAssembly = AssemblyHelper.GetTypesFromAssembly(assembly);
			Type[] array = typesFromAssembly;
			for (int i = 0; i < array.Length; i++)
			{
				Type type = array[i];
				object[] customAttributes = type.GetCustomAttributes(typeof(CustomEditor), false);
				object[] array2 = customAttributes;
				int j = 0;
				while (j < array2.Length)
				{
					CustomEditor customEditor = (CustomEditor)array2[j];
					CustomEditorAttributes.MonoEditorType monoEditorType = new CustomEditorAttributes.MonoEditorType();
					if (customEditor.m_InspectedType == null)
					{
						Debug.Log("Can't load custom inspector " + type.Name + " because the inspected type is null.");
					}
					else if (!type.IsSubclassOf(typeof(Editor)))
					{
						if (!(type.FullName == "TweakMode") || !type.IsEnum || !(customEditor.m_InspectedType.FullName == "BloomAndFlares"))
						{
							Debug.LogWarning(type.Name + " uses the CustomEditor attribute but does not inherit from Editor.\nYou must inherit from Editor. See the Editor class script documentation.");
						}
					}
					else
					{
						monoEditorType.m_InspectedType = customEditor.m_InspectedType;
						monoEditorType.m_InspectorType = type;
						monoEditorType.m_EditorForChildClasses = customEditor.m_EditorForChildClasses;
						monoEditorType.m_IsFallback = customEditor.isFallback;
						CustomEditorForRenderPipelineAttribute customEditorForRenderPipelineAttribute = customEditor as CustomEditorForRenderPipelineAttribute;
						if (customEditorForRenderPipelineAttribute != null)
						{
							monoEditorType.m_RenderPipelineType = customEditorForRenderPipelineAttribute.renderPipelineType;
						}
						List<CustomEditorAttributes.MonoEditorType> list;
						if (!CustomEditorAttributes.kSCustomEditors.TryGetValue(customEditor.m_InspectedType, out list))
						{
							list = new List<CustomEditorAttributes.MonoEditorType>();
							CustomEditorAttributes.kSCustomEditors[customEditor.m_InspectedType] = list;
						}
						list.Add(monoEditorType);
						if (type.GetCustomAttributes(typeof(CanEditMultipleObjects), false).Length > 0)
						{
							List<CustomEditorAttributes.MonoEditorType> list2;
							if (!CustomEditorAttributes.kSCustomMultiEditors.TryGetValue(customEditor.m_InspectedType, out list2))
							{
								list2 = new List<CustomEditorAttributes.MonoEditorType>();
								CustomEditorAttributes.kSCustomMultiEditors[customEditor.m_InspectedType] = list2;
							}
							list2.Add(monoEditorType);
						}
					}
					IL_1CF:
					j++;
					continue;
					goto IL_1CF;
				}
			}
		}
	}
}
