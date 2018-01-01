using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Scripting;

namespace UnityEditor
{
	internal static class EditorAssemblies
	{
		private const BindingFlags k_DefaultMethodBindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

		internal static List<RuntimeInitializeClassInfo> m_RuntimeInitializeClassInfoList;

		internal static int m_TotalNumRuntimeInitializeMethods;

		internal static Assembly[] loadedAssemblies
		{
			get;
			private set;
		}

		internal static IEnumerable<Type> loadedTypes
		{
			get
			{
				return EditorAssemblies.loadedAssemblies.SelectMany((Assembly assembly) => AssemblyHelper.GetTypesFromAssembly(assembly));
			}
		}

		internal static IEnumerable<MethodInfo> GetAllMethodsWithAttribute<T>(BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic) where T : Attribute
		{
			return EditorAssemblies.Internal_GetAllMethodsWithAttribute(typeof(T), bindingFlags).Cast<MethodInfo>();
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern object[] Internal_GetAllMethodsWithAttribute(Type attrType, BindingFlags staticness);

		internal static IEnumerable<Type> GetAllTypesWithAttribute<T>() where T : Attribute
		{
			return EditorAssemblies.Internal_GetAllTypesWithAttribute(typeof(T));
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Type[] Internal_GetAllTypesWithAttribute(Type attrType);

		internal static IEnumerable<Type> GetAllTypesWithInterface<T>() where T : class
		{
			return EditorAssemblies.GetAllTypesWithInterface(typeof(T));
		}

		private static IEnumerable<Type> GetAllTypesWithInterface(Type interfaceType)
		{
			if (!interfaceType.IsInterface)
			{
				throw new ArgumentException(string.Format("Specified type {0} is not an interface.", interfaceType), "interfaceType");
			}
			return EditorAssemblies.Internal_GetAllTypesWithInterface(interfaceType);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Type[] Internal_GetAllTypesWithInterface(Type interfaceType);

		internal static IEnumerable<Type> SubclassesOf(Type parent)
		{
			return (!parent.IsInterface) ? (from klass in EditorAssemblies.loadedTypes
			where klass.IsSubclassOf(parent)
			select klass) : EditorAssemblies.GetAllTypesWithInterface(parent);
		}

		[RequiredByNativeCode]
		private static void SetLoadedEditorAssemblies(Assembly[] assemblies)
		{
			EditorAssemblies.loadedAssemblies = assemblies;
		}

		[RequiredByNativeCode]
		private static RuntimeInitializeClassInfo[] GetRuntimeInitializeClassInfos()
		{
			RuntimeInitializeClassInfo[] result;
			if (EditorAssemblies.m_RuntimeInitializeClassInfoList == null)
			{
				result = null;
			}
			else
			{
				result = EditorAssemblies.m_RuntimeInitializeClassInfoList.ToArray();
			}
			return result;
		}

		[RequiredByNativeCode]
		private static int GetTotalNumRuntimeInitializeMethods()
		{
			return EditorAssemblies.m_TotalNumRuntimeInitializeMethods;
		}

		private static void StoreRuntimeInitializeClassInfo(Type type, List<string> methodNames, List<RuntimeInitializeLoadType> loadTypes)
		{
			RuntimeInitializeClassInfo runtimeInitializeClassInfo = new RuntimeInitializeClassInfo();
			runtimeInitializeClassInfo.assemblyName = type.Assembly.GetName().Name.ToString();
			runtimeInitializeClassInfo.className = type.ToString();
			runtimeInitializeClassInfo.methodNames = methodNames.ToArray();
			runtimeInitializeClassInfo.loadTypes = loadTypes.ToArray();
			EditorAssemblies.m_RuntimeInitializeClassInfoList.Add(runtimeInitializeClassInfo);
			EditorAssemblies.m_TotalNumRuntimeInitializeMethods += methodNames.Count;
		}

		private static void ProcessEditorInitializeOnLoad(Type type)
		{
			try
			{
				RuntimeHelpers.RunClassConstructor(type.TypeHandle);
			}
			catch (TypeInitializationException ex)
			{
				Debug.LogError(ex.InnerException);
			}
		}

		private static void ProcessRuntimeInitializeOnLoad(MethodInfo method)
		{
			RuntimeInitializeLoadType item = RuntimeInitializeLoadType.AfterSceneLoad;
			object[] customAttributes = method.GetCustomAttributes(typeof(RuntimeInitializeOnLoadMethodAttribute), false);
			if (customAttributes != null && customAttributes.Length > 0)
			{
				item = ((RuntimeInitializeOnLoadMethodAttribute)customAttributes[0]).loadType;
			}
			EditorAssemblies.StoreRuntimeInitializeClassInfo(method.DeclaringType, new List<string>
			{
				method.Name
			}, new List<RuntimeInitializeLoadType>
			{
				item
			});
		}

		private static void ProcessInitializeOnLoadMethod(MethodInfo method)
		{
			try
			{
				method.Invoke(null, null);
			}
			catch (TargetInvocationException ex)
			{
				Debug.LogError(ex.InnerException);
			}
		}

		[RequiredByNativeCode]
		private static int[] ProcessInitializeOnLoadAttributes()
		{
			EditorAssemblies.m_TotalNumRuntimeInitializeMethods = 0;
			EditorAssemblies.m_RuntimeInitializeClassInfoList = new List<RuntimeInitializeClassInfo>();
			foreach (Type current in EditorAssemblies.GetAllTypesWithAttribute<InitializeOnLoadAttribute>())
			{
				EditorAssemblies.ProcessEditorInitializeOnLoad(current);
			}
			foreach (MethodInfo current2 in EditorAssemblies.GetAllMethodsWithAttribute<RuntimeInitializeOnLoadMethodAttribute>(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
			{
				EditorAssemblies.ProcessRuntimeInitializeOnLoad(current2);
			}
			foreach (MethodInfo current3 in EditorAssemblies.GetAllMethodsWithAttribute<InitializeOnLoadMethodAttribute>(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
			{
				EditorAssemblies.ProcessInitializeOnLoadMethod(current3);
			}
			return null;
		}
	}
}
