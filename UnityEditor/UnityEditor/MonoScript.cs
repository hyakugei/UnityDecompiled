using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Bindings;

namespace UnityEditor
{
	[NativeType("Editor/Mono/MonoScript.bindings.h"), NativeClass(null)]
	public class MonoScript : TextAsset
	{
		public MonoScript()
		{
			MonoScript.Init_Internal(this);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern Type GetClass();

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern MonoScript FromMonoBehaviour(MonoBehaviour behaviour);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern MonoScript FromScriptableObject(ScriptableObject scriptableObject);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern bool GetScriptTypeWasJustCreatedFromComponentMenu();

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void SetScriptTypeWasJustCreatedFromComponentMenu();

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Init_Internal([Writable] MonoScript script);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void Init(string scriptContents, string className, string nameSpace, string assemblyName, bool isEditorScript);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern string GetNamespace();
	}
}
