using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Scripting;

namespace UnityEditor
{
	internal class AttributeHelper
	{
		private struct MonoGizmoMethod
		{
			public MethodInfo drawGizmo;

			public Type drawnType;

			public int options;
		}

		private struct MonoCreateAssetItem
		{
			public string menuItem;

			public string fileName;

			public int order;

			public Type type;
		}

		internal struct MethodWithAttribute
		{
			public MethodInfo info;

			public Attribute attribute;
		}

		internal class MethodInfoSorter
		{
			public IEnumerable<AttributeHelper.MethodWithAttribute> methodsWithAttributes
			{
				[CompilerGenerated]
				get
				{
					return this.<methodsWithAttributes>k__BackingField;
				}
			}

			internal MethodInfoSorter(List<AttributeHelper.MethodWithAttribute> methodsWithAttributes)
			{
				this.<methodsWithAttributes>k__BackingField = methodsWithAttributes;
			}

			public IEnumerable<MethodInfo> FilterAndSortOnAttribute<T>(Func<T, bool> filter, Func<T, IComparable> sorter) where T : Attribute
			{
				return from a in this.methodsWithAttributes
				where filter((T)((object)a.attribute))
				select a into c
				orderby sorter((T)((object)c.attribute))
				select c into o
				select o.info;
			}
		}

		private static Dictionary<Type, AttributeHelper.MethodInfoSorter> s_DecoratedMethodsByAttrTypeCache = new Dictionary<Type, AttributeHelper.MethodInfoSorter>();

		private const BindingFlags kAllStatic = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

		[RequiredByNativeCode]
		private static AttributeHelper.MonoGizmoMethod[] ExtractGizmos(Assembly assembly)
		{
			List<AttributeHelper.MonoGizmoMethod> list = new List<AttributeHelper.MonoGizmoMethod>();
			foreach (MethodInfo current in from m in EditorAssemblies.GetAllMethodsWithAttribute<DrawGizmo>(BindingFlags.Static)
			where m.DeclaringType.Assembly == assembly
			select m)
			{
				IEnumerable<DrawGizmo> enumerable = current.GetCustomAttributes(typeof(DrawGizmo), false).Cast<DrawGizmo>();
				foreach (DrawGizmo current2 in enumerable)
				{
					ParameterInfo[] parameters = current.GetParameters();
					if (parameters.Length != 2)
					{
						UnityEngine.Debug.LogWarningFormat("Method {0}.{1} is marked with the DrawGizmo attribute but does not take parameters (ComponentType, GizmoType) so will be ignored.", new object[]
						{
							current.DeclaringType.FullName,
							current.Name
						});
					}
					else if (current.DeclaringType != null && current.DeclaringType.IsGenericTypeDefinition)
					{
						UnityEngine.Debug.LogWarningFormat("Method {0}.{1} is marked with the DrawGizmo attribute but is defined on a generic type definition, so will be ignored.", new object[]
						{
							current.DeclaringType.FullName,
							current.Name
						});
					}
					else
					{
						AttributeHelper.MonoGizmoMethod item = default(AttributeHelper.MonoGizmoMethod);
						if (current2.drawnType == null)
						{
							item.drawnType = parameters[0].ParameterType;
						}
						else
						{
							if (!parameters[0].ParameterType.IsAssignableFrom(current2.drawnType))
							{
								UnityEngine.Debug.LogWarningFormat("Method {0}.{1} is marked with the DrawGizmo attribute but the component type it applies to could not be determined.", new object[]
								{
									current.DeclaringType.FullName,
									current.Name
								});
								continue;
							}
							item.drawnType = current2.drawnType;
						}
						if (parameters[1].ParameterType != typeof(GizmoType) && parameters[1].ParameterType != typeof(int))
						{
							UnityEngine.Debug.LogWarningFormat("Method {0}.{1} is marked with the DrawGizmo attribute but does not take a second parameter of type GizmoType so will be ignored.", new object[]
							{
								current.DeclaringType.FullName,
								current.Name
							});
						}
						else
						{
							item.drawGizmo = current;
							item.options = (int)current2.drawOptions;
							list.Add(item);
						}
					}
				}
			}
			return list.ToArray();
		}

		[RequiredByNativeCode]
		private static string GetComponentMenuName(Type type)
		{
			object[] customAttributes = type.GetCustomAttributes(typeof(AddComponentMenu), false);
			string result;
			if (customAttributes.Length > 0)
			{
				AddComponentMenu addComponentMenu = (AddComponentMenu)customAttributes[0];
				result = addComponentMenu.componentMenu;
			}
			else
			{
				result = null;
			}
			return result;
		}

		[RequiredByNativeCode]
		private static int GetComponentMenuOrdering(Type type)
		{
			object[] customAttributes = type.GetCustomAttributes(typeof(AddComponentMenu), false);
			int result;
			if (customAttributes.Length > 0)
			{
				AddComponentMenu addComponentMenu = (AddComponentMenu)customAttributes[0];
				result = addComponentMenu.componentOrder;
			}
			else
			{
				result = 0;
			}
			return result;
		}

		[RequiredByNativeCode]
		private static AttributeHelper.MonoCreateAssetItem[] ExtractCreateAssetMenuItems(Assembly assembly)
		{
			List<AttributeHelper.MonoCreateAssetItem> list = new List<AttributeHelper.MonoCreateAssetItem>();
			foreach (Type current in EditorAssemblies.GetAllTypesWithAttribute<CreateAssetMenuAttribute>())
			{
				CreateAssetMenuAttribute createAssetMenuAttribute = current.GetCustomAttributes(typeof(CreateAssetMenuAttribute), false).FirstOrDefault<object>() as CreateAssetMenuAttribute;
				if (createAssetMenuAttribute != null)
				{
					if (!current.IsSubclassOf(typeof(ScriptableObject)))
					{
						UnityEngine.Debug.LogWarningFormat("CreateAssetMenu attribute on {0} will be ignored as {0} is not derived from ScriptableObject.", new object[]
						{
							current.FullName
						});
					}
					else
					{
						string menuItem = (!string.IsNullOrEmpty(createAssetMenuAttribute.menuName)) ? createAssetMenuAttribute.menuName : ObjectNames.NicifyVariableName(current.Name);
						string text = (!string.IsNullOrEmpty(createAssetMenuAttribute.fileName)) ? createAssetMenuAttribute.fileName : ("New " + ObjectNames.NicifyVariableName(current.Name) + ".asset");
						if (!Path.HasExtension(text))
						{
							text += ".asset";
						}
						AttributeHelper.MonoCreateAssetItem item = new AttributeHelper.MonoCreateAssetItem
						{
							menuItem = menuItem,
							fileName = text,
							order = createAssetMenuAttribute.order,
							type = current
						};
						list.Add(item);
					}
				}
			}
			return list.ToArray();
		}

		internal static object InvokeMemberIfAvailable(object target, string methodName, object[] args)
		{
			MethodInfo method = target.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			object result;
			if (method != null)
			{
				result = method.Invoke(target, args);
			}
			else
			{
				result = null;
			}
			return result;
		}

		internal static bool GameObjectContainsAttribute<T>(GameObject go) where T : Attribute
		{
			Component[] components = go.GetComponents(typeof(Component));
			bool result;
			for (int i = 0; i < components.Length; i++)
			{
				Component component = components[i];
				if (!(component == null))
				{
					Type type = component.GetType();
					if (type.GetCustomAttributes(typeof(T), true).Length > 0)
					{
						result = true;
						return result;
					}
				}
			}
			result = false;
			return result;
		}

		[DebuggerHidden]
		internal static IEnumerable<TReturnValue> CallMethodsWithAttribute<TReturnValue, TAttr>(params object[] arguments) where TAttr : Attribute
		{
			AttributeHelper.<CallMethodsWithAttribute>c__Iterator0<TReturnValue, TAttr> <CallMethodsWithAttribute>c__Iterator = new AttributeHelper.<CallMethodsWithAttribute>c__Iterator0<TReturnValue, TAttr>();
			<CallMethodsWithAttribute>c__Iterator.arguments = arguments;
			AttributeHelper.<CallMethodsWithAttribute>c__Iterator0<TReturnValue, TAttr> expr_0E = <CallMethodsWithAttribute>c__Iterator;
			expr_0E.$PC = -2;
			return expr_0E;
		}

		private static bool AreSignaturesMatching(MethodInfo left, MethodInfo right)
		{
			bool result;
			if (left.IsStatic != right.IsStatic)
			{
				result = false;
			}
			else if (left.ReturnType != right.ReturnType)
			{
				result = false;
			}
			else
			{
				ParameterInfo[] parameters = left.GetParameters();
				ParameterInfo[] parameters2 = right.GetParameters();
				if (parameters.Length != parameters2.Length)
				{
					result = false;
				}
				else
				{
					for (int i = 0; i < parameters.Length; i++)
					{
						if (parameters[i].ParameterType != parameters2[i].ParameterType)
						{
							result = false;
							return result;
						}
					}
					result = true;
				}
			}
			return result;
		}

		internal static string MethodToString(MethodInfo method)
		{
			return string.Format("{0}{1}", (!method.IsStatic) ? "" : "static ", method);
		}

		internal static bool MethodMatchesAnyRequiredSignatureOfAttribute(MethodInfo method, Type attributeType)
		{
			List<MethodInfo> list = new List<MethodInfo>();
			MethodInfo[] methods = attributeType.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic);
			bool result;
			for (int i = 0; i < methods.Length; i++)
			{
				MethodInfo methodInfo = methods[i];
				object[] customAttributes = methodInfo.GetCustomAttributes(typeof(RequiredSignatureAttribute), false);
				if (customAttributes.Length > 0)
				{
					if (AttributeHelper.AreSignaturesMatching(method, methodInfo))
					{
						result = true;
						return result;
					}
					list.Add(methodInfo);
				}
			}
			if (list.Count == 0)
			{
				UnityEngine.Debug.LogError(string.Concat(new object[]
				{
					AttributeHelper.MethodToString(method),
					" has an invalid attribute : ",
					attributeType,
					". ",
					attributeType,
					" must have at least one required signature declaration"
				}));
			}
			else if (list.Count == 1)
			{
				UnityEngine.Debug.LogError(string.Concat(new object[]
				{
					AttributeHelper.MethodToString(method),
					" does not match ",
					attributeType,
					" expected signature.\n Use ",
					AttributeHelper.MethodToString(list[0])
				}));
			}
			else
			{
				object[] expr_102 = new object[5];
				expr_102[0] = AttributeHelper.MethodToString(method);
				expr_102[1] = " does not match any of ";
				expr_102[2] = attributeType;
				expr_102[3] = " expected signatures.\n Valid signatures are: ";
				expr_102[4] = string.Join(" , ", (from a in list
				select AttributeHelper.MethodToString(a)).ToArray<string>());
				UnityEngine.Debug.LogError(string.Concat(expr_102));
			}
			result = false;
			return result;
		}

		internal static AttributeHelper.MethodInfoSorter GetMethodsWithAttribute<T>(BindingFlags bindingFlags = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic) where T : Attribute
		{
			AttributeHelper.MethodInfoSorter methodInfoSorter;
			if (!AttributeHelper.s_DecoratedMethodsByAttrTypeCache.TryGetValue(typeof(T), out methodInfoSorter))
			{
				List<AttributeHelper.MethodWithAttribute> list = new List<AttributeHelper.MethodWithAttribute>();
				foreach (MethodInfo current in EditorAssemblies.GetAllMethodsWithAttribute<T>(bindingFlags))
				{
					if (current.IsGenericMethod)
					{
						UnityEngine.Debug.LogErrorFormat("{0} is a generic method. {1} cannot be applied to it.", new object[]
						{
							AttributeHelper.MethodToString(current),
							typeof(T)
						});
					}
					else
					{
						object[] customAttributes = current.GetCustomAttributes(typeof(T), false);
						for (int i = 0; i < customAttributes.Length; i++)
						{
							object obj = customAttributes[i];
							if (AttributeHelper.MethodMatchesAnyRequiredSignatureOfAttribute(current, typeof(T)))
							{
								AttributeHelper.MethodWithAttribute item = new AttributeHelper.MethodWithAttribute
								{
									info = current,
									attribute = (T)((object)obj)
								};
								list.Add(item);
							}
						}
					}
				}
				methodInfoSorter = new AttributeHelper.MethodInfoSorter(list);
				AttributeHelper.s_DecoratedMethodsByAttrTypeCache[typeof(T)] = methodInfoSorter;
			}
			return methodInfoSorter;
		}
	}
}
