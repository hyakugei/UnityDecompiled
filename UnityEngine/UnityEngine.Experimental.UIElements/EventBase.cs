using System;

namespace UnityEngine.Experimental.UIElements
{
	public abstract class EventBase
	{
		[Flags]
		public enum EventFlags
		{
			None = 0,
			Bubbles = 1,
			Capturable = 2,
			Cancellable = 4
		}

		private static long s_LastTypeId = 0L;

		protected EventBase.EventFlags flags;

		protected IEventHandler m_CurrentTarget;

		public long timestamp
		{
			get;
			private set;
		}

		public bool bubbles
		{
			get
			{
				return (this.flags & EventBase.EventFlags.Bubbles) != EventBase.EventFlags.None;
			}
		}

		public bool capturable
		{
			get
			{
				return (this.flags & EventBase.EventFlags.Capturable) != EventBase.EventFlags.None;
			}
		}

		public IEventHandler target
		{
			get;
			internal set;
		}

		public bool isPropagationStopped
		{
			get;
			private set;
		}

		public bool isImmediatePropagationStopped
		{
			get;
			private set;
		}

		public bool isDefaultPrevented
		{
			get;
			private set;
		}

		public PropagationPhase propagationPhase
		{
			get;
			internal set;
		}

		public virtual IEventHandler currentTarget
		{
			get
			{
				return this.m_CurrentTarget;
			}
			internal set
			{
				this.m_CurrentTarget = value;
				if (this.imguiEvent != null)
				{
					VisualElement visualElement = this.currentTarget as VisualElement;
					if (visualElement != null)
					{
						this.imguiEvent.mousePosition = visualElement.WorldToLocal(this.imguiEvent.mousePosition);
					}
				}
			}
		}

		public bool dispatch
		{
			get;
			internal set;
		}

		public Event imguiEvent
		{
			get;
			protected set;
		}

		protected EventBase()
		{
			this.Init();
		}

		protected static long RegisterEventType()
		{
			return EventBase.s_LastTypeId += 1L;
		}

		public abstract long GetEventTypeId();

		public void StopPropagation()
		{
			this.isPropagationStopped = true;
		}

		public void StopImmediatePropagation()
		{
			this.isPropagationStopped = true;
			this.isImmediatePropagationStopped = true;
		}

		public void PreventDefault()
		{
			if ((this.flags & EventBase.EventFlags.Cancellable) == EventBase.EventFlags.Cancellable)
			{
				this.isDefaultPrevented = true;
			}
		}

		protected virtual void Init()
		{
			this.timestamp = DateTime.Now.Ticks;
			this.flags = EventBase.EventFlags.None;
			this.target = null;
			this.isPropagationStopped = false;
			this.isImmediatePropagationStopped = false;
			this.isDefaultPrevented = false;
			this.propagationPhase = PropagationPhase.None;
			this.m_CurrentTarget = null;
			this.dispatch = false;
			this.imguiEvent = null;
		}
	}
	public abstract class EventBase<T> : EventBase where T : EventBase<T>, new()
	{
		private static readonly long s_TypeId = EventBase.RegisterEventType();

		private static readonly EventPool<T> s_Pool = new EventPool<T>();

		public static long TypeId()
		{
			return EventBase<T>.s_TypeId;
		}

		public static T GetPooled()
		{
			T result = EventBase<T>.s_Pool.Get();
			result.Init();
			return result;
		}

		public static void ReleasePooled(T evt)
		{
			EventBase<T>.s_Pool.Release(evt);
		}

		public override long GetEventTypeId()
		{
			return EventBase<T>.s_TypeId;
		}
	}
}
