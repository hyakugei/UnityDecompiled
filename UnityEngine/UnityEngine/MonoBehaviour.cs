using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;
using UnityEngine.Scripting;

namespace UnityEngine
{
	[RequiredByNativeCode]
	public class MonoBehaviour : Behaviour
	{
		public extern bool useGUILayout
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public extern bool runInEditMode
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public MonoBehaviour()
		{
			MonoBehaviour.ConstructorCheck(this);
		}

		public bool IsInvoking()
		{
			return MonoBehaviour.Internal_IsInvokingAll(this);
		}

		public void CancelInvoke()
		{
			MonoBehaviour.Internal_CancelInvokeAll(this);
		}

		public void Invoke(string methodName, float time)
		{
			MonoBehaviour.InvokeDelayed(this, methodName, time, 0f);
		}

		public void InvokeRepeating(string methodName, float time, float repeatRate)
		{
			if (repeatRate <= 1E-05f && repeatRate != 0f)
			{
				throw new UnityException("Invoke repeat rate has to be larger than 0.00001F)");
			}
			MonoBehaviour.InvokeDelayed(this, methodName, time, repeatRate);
		}

		public void CancelInvoke(string methodName)
		{
			MonoBehaviour.CancelInvoke(this, methodName);
		}

		public bool IsInvoking(string methodName)
		{
			return MonoBehaviour.IsInvoking(this, methodName);
		}

		[ExcludeFromDocs]
		public Coroutine StartCoroutine(string methodName)
		{
			object value = null;
			return this.StartCoroutine(methodName, value);
		}

		public Coroutine StartCoroutine(string methodName, [DefaultValue("null")] object value)
		{
			if (string.IsNullOrEmpty(methodName))
			{
				throw new NullReferenceException("methodName is null or empty");
			}
			if (!MonoBehaviour.IsObjectMonoBehaviour(this))
			{
				throw new ArgumentException("Coroutines can only be stopped on a MonoBehaviour");
			}
			return this.StartCoroutineManaged(methodName, value);
		}

		public Coroutine StartCoroutine(IEnumerator routine)
		{
			if (routine == null)
			{
				throw new NullReferenceException("routine is null");
			}
			if (!MonoBehaviour.IsObjectMonoBehaviour(this))
			{
				throw new ArgumentException("Coroutines can only be stopped on a MonoBehaviour");
			}
			return this.StartCoroutineManaged2(routine);
		}

		[Obsolete("StartCoroutine_Auto has been deprecated. Use StartCoroutine instead (UnityUpgradable) -> StartCoroutine([mscorlib] System.Collections.IEnumerator)", false)]
		public Coroutine StartCoroutine_Auto(IEnumerator routine)
		{
			return this.StartCoroutine(routine);
		}

		public void StopCoroutine(IEnumerator routine)
		{
			if (routine == null)
			{
				throw new NullReferenceException("routine is null");
			}
			if (!MonoBehaviour.IsObjectMonoBehaviour(this))
			{
				throw new ArgumentException("Coroutines can only be stopped on a MonoBehaviour");
			}
			this.StopCoroutineFromEnumeratorManaged(routine);
		}

		public void StopCoroutine(Coroutine routine)
		{
			if (routine == null)
			{
				throw new NullReferenceException("routine is null");
			}
			if (!MonoBehaviour.IsObjectMonoBehaviour(this))
			{
				throw new ArgumentException("Coroutines can only be stopped on a MonoBehaviour");
			}
			this.StopCoroutineManaged(routine);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void StopCoroutine(string methodName);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void StopAllCoroutines();

		public static void print(object message)
		{
			Debug.Log(message);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void ConstructorCheck([Writable] Object self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_CancelInvokeAll(MonoBehaviour self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool Internal_IsInvokingAll(MonoBehaviour self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void InvokeDelayed(MonoBehaviour self, string methodName, float time, float repeatRate);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void CancelInvoke(MonoBehaviour self, string methodName);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool IsInvoking(MonoBehaviour self, string methodName);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool IsObjectMonoBehaviour(Object obj);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern Coroutine StartCoroutineManaged(string methodName, object value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern Coroutine StartCoroutineManaged2(IEnumerator enumerator);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void StopCoroutineManaged(Coroutine routine);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void StopCoroutineFromEnumeratorManaged(IEnumerator routine);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern string GetScriptClassName();
	}
}
