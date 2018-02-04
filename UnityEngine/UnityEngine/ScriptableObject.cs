using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
	[NativeClass(null), RequiredByNativeCode]
	[StructLayout(LayoutKind.Sequential)]
	public class ScriptableObject : Object
	{
		public ScriptableObject()
		{
			ScriptableObject.CreateScriptableObject(this);
		}

		[Obsolete("Use EditorUtility.SetDirty instead")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetDirty();

		public static ScriptableObject CreateInstance(string className)
		{
			return ScriptableObject.CreateScriptableObjectInstanceFromName(className);
		}

		public static ScriptableObject CreateInstance(Type type)
		{
			return ScriptableObject.CreateScriptableObjectInstanceFromType(type);
		}

		public static T CreateInstance<T>() where T : ScriptableObject
		{
			return (T)((object)ScriptableObject.CreateInstance(typeof(T)));
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void CreateScriptableObject([Writable] ScriptableObject self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern ScriptableObject CreateScriptableObjectInstanceFromName(string className);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern ScriptableObject CreateScriptableObjectInstanceFromType(Type type);
	}
}
