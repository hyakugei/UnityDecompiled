using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEditor.Build.Reporting;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Scripting;

namespace UnityEditor.Build
{
	internal static class BuildPipelineInterfaces
	{
		[Flags]
		internal enum BuildCallbacks
		{
			None = 0,
			BuildProcessors = 1,
			SceneProcessors = 2,
			BuildTargetProcessors = 4
		}

		private class AttributeCallbackWrapper : IPostprocessBuild, IProcessScene, IActiveBuildTargetChanged, IOrderedCallback
		{
			private int m_callbackOrder;

			private MethodInfo m_method;

			public int callbackOrder
			{
				get
				{
					return this.m_callbackOrder;
				}
			}

			public AttributeCallbackWrapper(MethodInfo m)
			{
				this.m_callbackOrder = ((CallbackOrderAttribute)Attribute.GetCustomAttribute(m, typeof(CallbackOrderAttribute))).callbackOrder;
				this.m_method = m;
			}

			public void OnActiveBuildTargetChanged(BuildTarget previousTarget, BuildTarget newTarget)
			{
				this.m_method.Invoke(null, new object[]
				{
					previousTarget,
					newTarget
				});
			}

			public void OnPostprocessBuild(BuildReport report)
			{
				this.m_method.Invoke(null, new object[]
				{
					report.summary.platform,
					report.summary.outputPath
				});
			}

			public void OnProcessScene(Scene scene, BuildReport report)
			{
				this.m_method.Invoke(null, null);
			}
		}

		private static List<IPreprocessBuild> buildPreprocessors;

		private static List<IPostprocessBuild> buildPostprocessors;

		private static List<IProcessScene> sceneProcessors;

		private static List<IActiveBuildTargetChanged> buildTargetProcessors;

		private static BuildPipelineInterfaces.BuildCallbacks previousFlags = BuildPipelineInterfaces.BuildCallbacks.None;

		[CompilerGenerated]
		private static Comparison<IPreprocessBuild> <>f__mg$cache0;

		[CompilerGenerated]
		private static Comparison<IPostprocessBuild> <>f__mg$cache1;

		[CompilerGenerated]
		private static Comparison<IActiveBuildTargetChanged> <>f__mg$cache2;

		[CompilerGenerated]
		private static Comparison<IProcessScene> <>f__mg$cache3;

		internal static int CompareICallbackOrder(IOrderedCallback a, IOrderedCallback b)
		{
			return a.callbackOrder - b.callbackOrder;
		}

		private static void AddToList<T>(object o, ref List<T> list) where T : class
		{
			T t = o as T;
			if (t != null)
			{
				if (list == null)
				{
					list = new List<T>();
				}
				list.Add(t);
			}
		}

		[RequiredByNativeCode]
		internal static void InitializeBuildCallbacks(BuildPipelineInterfaces.BuildCallbacks findFlags)
		{
			if (findFlags != BuildPipelineInterfaces.previousFlags)
			{
				BuildPipelineInterfaces.previousFlags = findFlags;
				BuildPipelineInterfaces.CleanupBuildCallbacks();
				bool flag = (findFlags & BuildPipelineInterfaces.BuildCallbacks.BuildProcessors) == BuildPipelineInterfaces.BuildCallbacks.BuildProcessors;
				bool flag2 = (findFlags & BuildPipelineInterfaces.BuildCallbacks.SceneProcessors) == BuildPipelineInterfaces.BuildCallbacks.SceneProcessors;
				bool flag3 = (findFlags & BuildPipelineInterfaces.BuildCallbacks.BuildTargetProcessors) == BuildPipelineInterfaces.BuildCallbacks.BuildTargetProcessors;
				Type[] expectedArguments = new Type[]
				{
					typeof(BuildTarget),
					typeof(string)
				};
				foreach (Type current in EditorAssemblies.GetAllTypesWithInterface<IOrderedCallback>())
				{
					if (!current.IsAbstract && !current.IsInterface)
					{
						object obj = null;
						if (flag)
						{
							if (BuildPipelineInterfaces.ValidateType<IPreprocessBuild>(current))
							{
								BuildPipelineInterfaces.AddToList<IPreprocessBuild>(obj = Activator.CreateInstance(current), ref BuildPipelineInterfaces.buildPreprocessors);
							}
							if (BuildPipelineInterfaces.ValidateType<IPostprocessBuild>(current))
							{
								BuildPipelineInterfaces.AddToList<IPostprocessBuild>(obj = ((obj != null) ? obj : Activator.CreateInstance(current)), ref BuildPipelineInterfaces.buildPostprocessors);
							}
						}
						if (flag2 && BuildPipelineInterfaces.ValidateType<IProcessScene>(current))
						{
							BuildPipelineInterfaces.AddToList<IProcessScene>(obj = ((obj != null) ? obj : Activator.CreateInstance(current)), ref BuildPipelineInterfaces.sceneProcessors);
						}
						if (flag3 && BuildPipelineInterfaces.ValidateType<IActiveBuildTargetChanged>(current))
						{
							BuildPipelineInterfaces.AddToList<IActiveBuildTargetChanged>((obj != null) ? obj : Activator.CreateInstance(current), ref BuildPipelineInterfaces.buildTargetProcessors);
						}
					}
				}
				if (flag)
				{
					foreach (MethodInfo current2 in EditorAssemblies.GetAllMethodsWithAttribute<PostProcessBuildAttribute>(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
					{
						if (BuildPipelineInterfaces.ValidateMethod<PostProcessBuildAttribute>(current2, expectedArguments))
						{
							BuildPipelineInterfaces.AddToList<IPostprocessBuild>(new BuildPipelineInterfaces.AttributeCallbackWrapper(current2), ref BuildPipelineInterfaces.buildPostprocessors);
						}
					}
				}
				if (flag2)
				{
					foreach (MethodInfo current3 in EditorAssemblies.GetAllMethodsWithAttribute<PostProcessSceneAttribute>(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
					{
						if (BuildPipelineInterfaces.ValidateMethod<PostProcessSceneAttribute>(current3, Type.EmptyTypes))
						{
							BuildPipelineInterfaces.AddToList<IProcessScene>(new BuildPipelineInterfaces.AttributeCallbackWrapper(current3), ref BuildPipelineInterfaces.sceneProcessors);
						}
					}
				}
				if (BuildPipelineInterfaces.buildPreprocessors != null)
				{
					List<IPreprocessBuild> arg_263_0 = BuildPipelineInterfaces.buildPreprocessors;
					if (BuildPipelineInterfaces.<>f__mg$cache0 == null)
					{
						BuildPipelineInterfaces.<>f__mg$cache0 = new Comparison<IPreprocessBuild>(BuildPipelineInterfaces.CompareICallbackOrder);
					}
					arg_263_0.Sort(BuildPipelineInterfaces.<>f__mg$cache0);
				}
				if (BuildPipelineInterfaces.buildPostprocessors != null)
				{
					List<IPostprocessBuild> arg_294_0 = BuildPipelineInterfaces.buildPostprocessors;
					if (BuildPipelineInterfaces.<>f__mg$cache1 == null)
					{
						BuildPipelineInterfaces.<>f__mg$cache1 = new Comparison<IPostprocessBuild>(BuildPipelineInterfaces.CompareICallbackOrder);
					}
					arg_294_0.Sort(BuildPipelineInterfaces.<>f__mg$cache1);
				}
				if (BuildPipelineInterfaces.buildTargetProcessors != null)
				{
					List<IActiveBuildTargetChanged> arg_2C5_0 = BuildPipelineInterfaces.buildTargetProcessors;
					if (BuildPipelineInterfaces.<>f__mg$cache2 == null)
					{
						BuildPipelineInterfaces.<>f__mg$cache2 = new Comparison<IActiveBuildTargetChanged>(BuildPipelineInterfaces.CompareICallbackOrder);
					}
					arg_2C5_0.Sort(BuildPipelineInterfaces.<>f__mg$cache2);
				}
				if (BuildPipelineInterfaces.sceneProcessors != null)
				{
					List<IProcessScene> arg_2F6_0 = BuildPipelineInterfaces.sceneProcessors;
					if (BuildPipelineInterfaces.<>f__mg$cache3 == null)
					{
						BuildPipelineInterfaces.<>f__mg$cache3 = new Comparison<IProcessScene>(BuildPipelineInterfaces.CompareICallbackOrder);
					}
					arg_2F6_0.Sort(BuildPipelineInterfaces.<>f__mg$cache3);
				}
			}
		}

		internal static bool ValidateType<T>(Type t)
		{
			return typeof(T).IsAssignableFrom(t) && t != typeof(BuildPipelineInterfaces.AttributeCallbackWrapper);
		}

		private static bool ValidateMethod<T>(MethodInfo method, Type[] expectedArguments)
		{
			Type typeFromHandle = typeof(T);
			bool result;
			if (method.IsDefined(typeFromHandle, false))
			{
				if (!method.IsStatic)
				{
					string text = typeFromHandle.Name.Replace("Attribute", "");
					Debug.LogErrorFormat("Method {0} with {1} attribute must be static.", new object[]
					{
						method.Name,
						text
					});
					result = false;
				}
				else if (method.IsGenericMethod || method.IsGenericMethodDefinition)
				{
					string text2 = typeFromHandle.Name.Replace("Attribute", "");
					Debug.LogErrorFormat("Method {0} with {1} attribute cannot be generic.", new object[]
					{
						method.Name,
						text2
					});
					result = false;
				}
				else
				{
					ParameterInfo[] parameters = method.GetParameters();
					bool flag = parameters.Length == expectedArguments.Length;
					if (flag)
					{
						for (int i = 0; i < parameters.Length; i++)
						{
							if (parameters[i].ParameterType != expectedArguments[i])
							{
								flag = false;
								break;
							}
						}
					}
					if (!flag)
					{
						string text3 = typeFromHandle.Name.Replace("Attribute", "");
						string text4 = "static void " + method.Name + "(";
						for (int j = 0; j < expectedArguments.Length; j++)
						{
							text4 += expectedArguments[j].Name;
							if (j != expectedArguments.Length - 1)
							{
								text4 += ", ";
							}
						}
						text4 += ")";
						Debug.LogErrorFormat("Method {0} with {1} attribute does not have the correct signature, expected: {2}.", new object[]
						{
							method.Name,
							text3,
							text4
						});
						result = false;
					}
					else
					{
						result = true;
					}
				}
			}
			else
			{
				result = false;
			}
			return result;
		}

		[RequiredByNativeCode]
		internal static void OnBuildPreProcess(BuildReport report)
		{
			if (BuildPipelineInterfaces.buildPreprocessors != null)
			{
				foreach (IPreprocessBuild current in BuildPipelineInterfaces.buildPreprocessors)
				{
					try
					{
						current.OnPreprocessBuild(report);
					}
					catch (Exception exception)
					{
						Debug.LogException(exception);
						if ((report.summary.options & BuildOptions.StrictMode) != BuildOptions.None || (report.summary.assetBundleOptions & BuildAssetBundleOptions.StrictMode) != BuildAssetBundleOptions.None)
						{
							break;
						}
					}
				}
			}
		}

		[RequiredByNativeCode]
		internal static void OnSceneProcess(Scene scene, BuildReport report)
		{
			if (BuildPipelineInterfaces.sceneProcessors != null)
			{
				foreach (IProcessScene current in BuildPipelineInterfaces.sceneProcessors)
				{
					try
					{
						current.OnProcessScene(scene, report);
					}
					catch (Exception exception)
					{
						Debug.LogException(exception);
						if ((report.summary.options & BuildOptions.StrictMode) != BuildOptions.None || (report.summary.assetBundleOptions & BuildAssetBundleOptions.StrictMode) != BuildAssetBundleOptions.None)
						{
							break;
						}
					}
				}
			}
		}

		[RequiredByNativeCode]
		internal static void OnBuildPostProcess(BuildReport report)
		{
			if (BuildPipelineInterfaces.buildPostprocessors != null)
			{
				foreach (IPostprocessBuild current in BuildPipelineInterfaces.buildPostprocessors)
				{
					try
					{
						current.OnPostprocessBuild(report);
					}
					catch (Exception exception)
					{
						Debug.LogException(exception);
						if ((report.summary.options & BuildOptions.StrictMode) != BuildOptions.None || (report.summary.assetBundleOptions & BuildAssetBundleOptions.StrictMode) != BuildAssetBundleOptions.None)
						{
							break;
						}
					}
				}
			}
		}

		[RequiredByNativeCode]
		internal static void OnActiveBuildTargetChanged(BuildTarget previousPlatform, BuildTarget newPlatform)
		{
			if (BuildPipelineInterfaces.buildTargetProcessors != null)
			{
				foreach (IActiveBuildTargetChanged current in BuildPipelineInterfaces.buildTargetProcessors)
				{
					try
					{
						current.OnActiveBuildTargetChanged(previousPlatform, newPlatform);
					}
					catch (Exception exception)
					{
						Debug.LogException(exception);
					}
				}
			}
		}

		[RequiredByNativeCode]
		internal static void CleanupBuildCallbacks()
		{
			BuildPipelineInterfaces.buildTargetProcessors = null;
			BuildPipelineInterfaces.buildPreprocessors = null;
			BuildPipelineInterfaces.buildPostprocessors = null;
			BuildPipelineInterfaces.sceneProcessors = null;
			BuildPipelineInterfaces.previousFlags = BuildPipelineInterfaces.BuildCallbacks.None;
		}
	}
}
