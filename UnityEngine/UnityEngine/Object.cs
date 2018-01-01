using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using UnityEngine.Bindings;
using UnityEngine.Internal;
using UnityEngine.Scripting;
using UnityEngineInternal;

namespace UnityEngine
{
	[GenerateManagedProxy, RequiredByNativeCode]
	[StructLayout(LayoutKind.Sequential)]
	public class Object
	{
		private IntPtr m_CachedPtr;

		private int m_InstanceID;

		private string m_UnityRuntimeErrorString;

		internal static int OffsetOfInstanceIDInCPlusPlusObject = -1;

		public extern string name
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public extern HideFlags hideFlags
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		[SecuritySafeCritical]
		public int GetInstanceID()
		{
			this.EnsureRunningOnMainThread();
			return this.m_InstanceID;
		}

		public override int GetHashCode()
		{
			return this.m_InstanceID;
		}

		public override bool Equals(object other)
		{
			Object @object = other as Object;
			return (!(@object == null) || other == null || other is Object) && Object.CompareBaseObjects(this, @object);
		}

		public static implicit operator bool(Object exists)
		{
			return !Object.CompareBaseObjects(exists, null);
		}

		private static bool CompareBaseObjects(Object lhs, Object rhs)
		{
			bool flag = lhs == null;
			bool flag2 = rhs == null;
			bool result;
			if (flag2 && flag)
			{
				result = true;
			}
			else if (flag2)
			{
				result = !Object.IsNativeObjectAlive(lhs);
			}
			else if (flag)
			{
				result = !Object.IsNativeObjectAlive(rhs);
			}
			else
			{
				result = (lhs.m_InstanceID == rhs.m_InstanceID);
			}
			return result;
		}

		private void EnsureRunningOnMainThread()
		{
			if (!Object.CurrentThreadIsMainThread())
			{
				throw new InvalidOperationException("EnsureRunningOnMainThread can only be called from the main thread");
			}
		}

		private static bool IsNativeObjectAlive(Object o)
		{
			return o.GetCachedPtr() != IntPtr.Zero || (!(o is MonoBehaviour) && !(o is ScriptableObject) && Object.DoesObjectWithInstanceIDExist(o.GetInstanceID()));
		}

		private IntPtr GetCachedPtr()
		{
			return this.m_CachedPtr;
		}

		[TypeInferenceRule(TypeInferenceRules.TypeOfFirstArgument)]
		public static Object Instantiate(Object original, Vector3 position, Quaternion rotation)
		{
			Object.CheckNullArgument(original, "The Object you want to instantiate is null.");
			if (original is ScriptableObject)
			{
				throw new ArgumentException("Cannot instantiate a ScriptableObject with a position and rotation");
			}
			return Object.Internal_InstantiateSingle(original, position, rotation);
		}

		[TypeInferenceRule(TypeInferenceRules.TypeOfFirstArgument)]
		public static Object Instantiate(Object original, Vector3 position, Quaternion rotation, Transform parent)
		{
			Object result;
			if (parent == null)
			{
				result = Object.Internal_InstantiateSingle(original, position, rotation);
			}
			else
			{
				Object.CheckNullArgument(original, "The Object you want to instantiate is null.");
				result = Object.Internal_InstantiateSingleWithParent(original, parent, position, rotation);
			}
			return result;
		}

		[TypeInferenceRule(TypeInferenceRules.TypeOfFirstArgument)]
		public static Object Instantiate(Object original)
		{
			Object.CheckNullArgument(original, "The Object you want to instantiate is null.");
			return Object.Internal_CloneSingle(original);
		}

		[TypeInferenceRule(TypeInferenceRules.TypeOfFirstArgument)]
		public static Object Instantiate(Object original, Transform parent)
		{
			return Object.Instantiate(original, parent, false);
		}

		[TypeInferenceRule(TypeInferenceRules.TypeOfFirstArgument)]
		public static Object Instantiate(Object original, Transform parent, bool instantiateInWorldSpace)
		{
			Object result;
			if (parent == null)
			{
				result = Object.Internal_CloneSingle(original);
			}
			else
			{
				Object.CheckNullArgument(original, "The Object you want to instantiate is null.");
				result = Object.Internal_CloneSingleWithParent(original, parent, instantiateInWorldSpace);
			}
			return result;
		}

		public static T Instantiate<T>(T original) where T : Object
		{
			Object.CheckNullArgument(original, "The Object you want to instantiate is null.");
			return (T)((object)Object.Internal_CloneSingle(original));
		}

		public static T Instantiate<T>(T original, Vector3 position, Quaternion rotation) where T : Object
		{
			return (T)((object)Object.Instantiate(original, position, rotation));
		}

		public static T Instantiate<T>(T original, Vector3 position, Quaternion rotation, Transform parent) where T : Object
		{
			return (T)((object)Object.Instantiate(original, position, rotation, parent));
		}

		public static T Instantiate<T>(T original, Transform parent) where T : Object
		{
			return Object.Instantiate<T>(original, parent, false);
		}

		public static T Instantiate<T>(T original, Transform parent, bool worldPositionStays) where T : Object
		{
			return (T)((object)Object.Instantiate(original, parent, worldPositionStays));
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Destroy(Object obj, [DefaultValue("0.0F")] float t);

		[ExcludeFromDocs]
		public static void Destroy(Object obj)
		{
			float t = 0f;
			Object.Destroy(obj, t);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void DestroyImmediate(Object obj, [DefaultValue("false")] bool allowDestroyingAssets);

		[ExcludeFromDocs]
		public static void DestroyImmediate(Object obj)
		{
			bool allowDestroyingAssets = false;
			Object.DestroyImmediate(obj, allowDestroyingAssets);
		}

		[TypeInferenceRule(TypeInferenceRules.ArrayOfTypeReferencedByFirstArgument)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern Object[] FindObjectsOfType(Type type);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void DontDestroyOnLoad(Object target);

		[Obsolete("use Object.Destroy instead.")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void DestroyObject(Object obj, [DefaultValue("0.0F")] float t);

		[Obsolete("use Object.Destroy instead."), ExcludeFromDocs]
		public static void DestroyObject(Object obj)
		{
			float t = 0f;
			Object.DestroyObject(obj, t);
		}

		[Obsolete("warning use Object.FindObjectsOfType instead.")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern Object[] FindSceneObjectsOfType(Type type);

		[Obsolete("use Resources.FindObjectsOfTypeAll instead.")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern Object[] FindObjectsOfTypeIncludingAssets(Type type);

		public static T[] FindObjectsOfType<T>() where T : Object
		{
			return Resources.ConvertObjects<T>(Object.FindObjectsOfType(typeof(T)));
		}

		public static T FindObjectOfType<T>() where T : Object
		{
			return (T)((object)Object.FindObjectOfType(typeof(T)));
		}

		[Obsolete("Please use Resources.FindObjectsOfTypeAll instead")]
		public static Object[] FindObjectsOfTypeAll(Type type)
		{
			return Resources.FindObjectsOfTypeAll(type);
		}

		private static void CheckNullArgument(object arg, string message)
		{
			if (arg == null)
			{
				throw new ArgumentException(message);
			}
		}

		[TypeInferenceRule(TypeInferenceRules.TypeReferencedByFirstArgument)]
		public static Object FindObjectOfType(Type type)
		{
			Object[] array = Object.FindObjectsOfType(type);
			Object result;
			if (array.Length > 0)
			{
				result = array[0];
			}
			else
			{
				result = null;
			}
			return result;
		}

		public override string ToString()
		{
			return Object.ToString(this);
		}

		public static bool operator ==(Object x, Object y)
		{
			return Object.CompareBaseObjects(x, y);
		}

		public static bool operator !=(Object x, Object y)
		{
			return !Object.CompareBaseObjects(x, y);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetOffsetOfInstanceIDInCPlusPlusObject();

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool CurrentThreadIsMainThread();

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Object Internal_CloneSingle(Object data);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Object Internal_CloneSingleWithParent(Object data, Transform parent, bool worldPositionStays);

		private static Object Internal_InstantiateSingle(Object data, Vector3 pos, Quaternion rot)
		{
			return Object.Internal_InstantiateSingle_Injected(data, ref pos, ref rot);
		}

		private static Object Internal_InstantiateSingleWithParent(Object data, Transform parent, Vector3 pos, Quaternion rot)
		{
			return Object.Internal_InstantiateSingleWithParent_Injected(data, parent, ref pos, ref rot);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string ToString(Object obj);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool DoesObjectWithInstanceIDExist(int instanceID);

		[VisibleToOtherModules]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern Object FindObjectFromInstanceID(int instanceID);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Object Internal_InstantiateSingle_Injected(Object data, ref Vector3 pos, ref Quaternion rot);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Object Internal_InstantiateSingleWithParent_Injected(Object data, Transform parent, ref Vector3 pos, ref Quaternion rot);
	}
}
