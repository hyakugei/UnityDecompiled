using System;
using System.Collections.Generic;
using System.Reflection;

namespace UnityEngine.Experimental.UIElements
{
	internal static class Factories
	{
		private static Dictionary<string, Func<IUxmlAttributes, CreationContext, VisualElement>> s_Factories;

		internal static void RegisterFactory(string fullTypeName, Func<IUxmlAttributes, CreationContext, VisualElement> factory)
		{
			Factories.DiscoverFactories();
			Factories.s_Factories.Add(fullTypeName, factory);
		}

		internal static void RegisterFactory<T>(Func<IUxmlAttributes, CreationContext, VisualElement> factory) where T : VisualElement
		{
			Factories.RegisterFactory(typeof(T).FullName, factory);
		}

		private static void DiscoverFactories()
		{
			if (Factories.s_Factories == null)
			{
				Factories.s_Factories = new Dictionary<string, Func<IUxmlAttributes, CreationContext, VisualElement>>();
				CoreFactories.RegisterAll();
				AppDomain currentDomain = AppDomain.CurrentDomain;
				HashSet<string> hashSet = new HashSet<string>(ScriptingRuntime.GetAllUserAssemblies());
				Assembly[] assemblies = currentDomain.GetAssemblies();
				for (int i = 0; i < assemblies.Length; i++)
				{
					Assembly assembly = assemblies[i];
					if (hashSet.Contains(assembly.GetName().Name + ".dll"))
					{
						try
						{
							Type[] types = assembly.GetTypes();
							for (int j = 0; j < types.Length; j++)
							{
								Type type = types[j];
								if (typeof(IUxmlFactory).IsAssignableFrom(type))
								{
									IUxmlFactory uxmlFactory = (IUxmlFactory)Activator.CreateInstance(type);
									Factories.RegisterFactory(uxmlFactory.CreatesType.FullName, new Func<IUxmlAttributes, CreationContext, VisualElement>(uxmlFactory.Create));
								}
							}
						}
						catch (TypeLoadException ex)
						{
							Debug.LogWarningFormat("Error while loading types from assembly {0}: {1}", new object[]
							{
								assembly.FullName,
								ex
							});
						}
					}
				}
			}
		}

		internal static bool TryGetValue(string fullTypeName, out Func<IUxmlAttributes, CreationContext, VisualElement> factory)
		{
			Factories.DiscoverFactories();
			factory = null;
			return Factories.s_Factories != null && Factories.s_Factories.TryGetValue(fullTypeName, out factory);
		}
	}
}
