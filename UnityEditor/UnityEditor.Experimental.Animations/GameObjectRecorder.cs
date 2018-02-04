using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Bindings;

namespace UnityEditor.Experimental.Animations
{
	[NativeType]
	public class GameObjectRecorder : UnityEngine.Object
	{
		public extern GameObject root
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public extern float currentTime
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public extern bool isRecording
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		[Obsolete("The GameObjectRecorder constructor now takes a root GameObject", true)]
		public GameObjectRecorder()
		{
		}

		public GameObjectRecorder(GameObject root)
		{
			GameObjectRecorder.Internal_Create(this, root);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_Create([Writable] GameObjectRecorder notSelf, [NotNull] GameObject root);

		public void Bind(EditorCurveBinding binding)
		{
			this.Bind_Injected(ref binding);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void BindAll(GameObject target, bool recursive);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void BindComponent(GameObject target, Type componentType, bool recursive);

		public void BindComponent<T>(GameObject target, bool recursive) where T : Component
		{
			this.BindComponent(target, typeof(T), recursive);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern EditorCurveBinding[] GetBindings();

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void TakeSnapshot(float dt);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SaveToClip(AnimationClip clip);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void ResetRecording();

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Bind_Injected(ref EditorCurveBinding binding);
	}
}
