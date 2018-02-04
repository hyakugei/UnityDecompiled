using AOT;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEngine.Scripting;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.XR.WSA.Input
{
	[MovedFrom("UnityEngine.VR.WSA.Input")]
	public sealed class InteractionManager
	{
		private enum EventType
		{
			SourceDetected,
			SourceLost,
			SourceUpdated,
			SourcePressed,
			SourceReleased
		}

		private delegate void InternalSourceEventHandler(InteractionManager.EventType eventType, InteractionSourceState state, InteractionSourcePressType pressType);

		public delegate void SourceEventHandler(InteractionSourceState state);

		private static InteractionManager.InternalSourceEventHandler m_OnSourceEventHandler;

		/*
		[CompilerGenerated]
		private static InteractionManager.InternalSourceEventHandler <>f__mg$cache0;
		*/

		public static event Action<InteractionSourceDetectedEventArgs> InteractionSourceDetected
		{
			add
			{
				/*
				Action<InteractionSourceDetectedEventArgs> action = InteractionManager.InteractionSourceDetected;
				Action<InteractionSourceDetectedEventArgs> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action<InteractionSourceDetectedEventArgs>>(ref InteractionManager.InteractionSourceDetected, (Action<InteractionSourceDetectedEventArgs>)Delegate.Combine(action2, value), action);
				}
				while (action != action2);
				*/
			}
			remove
			{
				/*
				Action<InteractionSourceDetectedEventArgs> action = InteractionManager.InteractionSourceDetected;
				Action<InteractionSourceDetectedEventArgs> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action<InteractionSourceDetectedEventArgs>>(ref InteractionManager.InteractionSourceDetected, (Action<InteractionSourceDetectedEventArgs>)Delegate.Remove(action2, value), action);
				}
				while (action != action2);
				*/
			}
		}

		public static event Action<InteractionSourceLostEventArgs> InteractionSourceLost
		{
			add
			{
				/*
				Action<InteractionSourceLostEventArgs> action = InteractionManager.InteractionSourceLost;
				Action<InteractionSourceLostEventArgs> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action<InteractionSourceLostEventArgs>>(ref InteractionManager.InteractionSourceLost, (Action<InteractionSourceLostEventArgs>)Delegate.Combine(action2, value), action);
				}
				while (action != action2);
				*/
			}
			remove
			{
				/*
				Action<InteractionSourceLostEventArgs> action = InteractionManager.InteractionSourceLost;
				Action<InteractionSourceLostEventArgs> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action<InteractionSourceLostEventArgs>>(ref InteractionManager.InteractionSourceLost, (Action<InteractionSourceLostEventArgs>)Delegate.Remove(action2, value), action);
				}
				while (action != action2);
				*/
			}
		}

		public static event Action<InteractionSourcePressedEventArgs> InteractionSourcePressed
		{
			add
			{
				/*
				Action<InteractionSourcePressedEventArgs> action = InteractionManager.InteractionSourcePressed;
				Action<InteractionSourcePressedEventArgs> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action<InteractionSourcePressedEventArgs>>(ref InteractionManager.InteractionSourcePressed, (Action<InteractionSourcePressedEventArgs>)Delegate.Combine(action2, value), action);
				}
				while (action != action2);
				*/
			}
			remove
			{
				/*
				Action<InteractionSourcePressedEventArgs> action = InteractionManager.InteractionSourcePressed;
				Action<InteractionSourcePressedEventArgs> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action<InteractionSourcePressedEventArgs>>(ref InteractionManager.InteractionSourcePressed, (Action<InteractionSourcePressedEventArgs>)Delegate.Remove(action2, value), action);
				}
				while (action != action2);
				*/
			}
		}

		public static event Action<InteractionSourceReleasedEventArgs> InteractionSourceReleased
		{
			add
			{
				/*
				Action<InteractionSourceReleasedEventArgs> action = InteractionManager.InteractionSourceReleased;
				Action<InteractionSourceReleasedEventArgs> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action<InteractionSourceReleasedEventArgs>>(ref InteractionManager.InteractionSourceReleased, (Action<InteractionSourceReleasedEventArgs>)Delegate.Combine(action2, value), action);
				}
				while (action != action2);
				*/
			}
			remove
			{
				/*
				Action<InteractionSourceReleasedEventArgs> action = InteractionManager.InteractionSourceReleased;
				Action<InteractionSourceReleasedEventArgs> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action<InteractionSourceReleasedEventArgs>>(ref InteractionManager.InteractionSourceReleased, (Action<InteractionSourceReleasedEventArgs>)Delegate.Remove(action2, value), action);
				}
				while (action != action2);
				*/
			}
		}

		public static event Action<InteractionSourceUpdatedEventArgs> InteractionSourceUpdated
		{
			add
			{
				/*
				Action<InteractionSourceUpdatedEventArgs> action = InteractionManager.InteractionSourceUpdated;
				Action<InteractionSourceUpdatedEventArgs> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action<InteractionSourceUpdatedEventArgs>>(ref InteractionManager.InteractionSourceUpdated, (Action<InteractionSourceUpdatedEventArgs>)Delegate.Combine(action2, value), action);
				}
				while (action != action2);
				*/
			}
			remove
			{
				/*
				Action<InteractionSourceUpdatedEventArgs> action = InteractionManager.InteractionSourceUpdated;
				Action<InteractionSourceUpdatedEventArgs> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action<InteractionSourceUpdatedEventArgs>>(ref InteractionManager.InteractionSourceUpdated, (Action<InteractionSourceUpdatedEventArgs>)Delegate.Remove(action2, value), action);
				}
				while (action != action2);
				*/
			}
		}

		[Obsolete("SourceDetected is deprecated, and will be removed in a future release. Use InteractionSourceDetected instead. (UnityUpgradable) -> InteractionSourceDetectedLegacy", true)]
		public static event InteractionManager.SourceEventHandler SourceDetected
		{
			add
			{
				/*
				InteractionManager.SourceEventHandler sourceEventHandler = InteractionManager.SourceDetected;
				InteractionManager.SourceEventHandler sourceEventHandler2;
				do
				{
					sourceEventHandler2 = sourceEventHandler;
					sourceEventHandler = Interlocked.CompareExchange<InteractionManager.SourceEventHandler>(ref InteractionManager.SourceDetected, (InteractionManager.SourceEventHandler)Delegate.Combine(sourceEventHandler2, value), sourceEventHandler);
				}
				while (sourceEventHandler != sourceEventHandler2);
				*/
			}
			remove
			{
				/*
				InteractionManager.SourceEventHandler sourceEventHandler = InteractionManager.SourceDetected;
				InteractionManager.SourceEventHandler sourceEventHandler2;
				do
				{
					sourceEventHandler2 = sourceEventHandler;
					sourceEventHandler = Interlocked.CompareExchange<InteractionManager.SourceEventHandler>(ref InteractionManager.SourceDetected, (InteractionManager.SourceEventHandler)Delegate.Remove(sourceEventHandler2, value), sourceEventHandler);
				}
				while (sourceEventHandler != sourceEventHandler2);
				*/
			}
		}

		[Obsolete("SourceLost is deprecated, and will be removed in a future release. Use InteractionSourceLost instead. (UnityUpgradable) -> InteractionSourceLostLegacy", true)]
		public static event InteractionManager.SourceEventHandler SourceLost
		{
			add
			{
				/*
				InteractionManager.SourceEventHandler sourceEventHandler = InteractionManager.SourceLost;
				InteractionManager.SourceEventHandler sourceEventHandler2;
				do
				{
					sourceEventHandler2 = sourceEventHandler;
					sourceEventHandler = Interlocked.CompareExchange<InteractionManager.SourceEventHandler>(ref InteractionManager.SourceLost, (InteractionManager.SourceEventHandler)Delegate.Combine(sourceEventHandler2, value), sourceEventHandler);
				}
				while (sourceEventHandler != sourceEventHandler2);
				*/
			}
			remove
			{
				/*
				InteractionManager.SourceEventHandler sourceEventHandler = InteractionManager.SourceLost;
				InteractionManager.SourceEventHandler sourceEventHandler2;
				do
				{
					sourceEventHandler2 = sourceEventHandler;
					sourceEventHandler = Interlocked.CompareExchange<InteractionManager.SourceEventHandler>(ref InteractionManager.SourceLost, (InteractionManager.SourceEventHandler)Delegate.Remove(sourceEventHandler2, value), sourceEventHandler);
				}
				while (sourceEventHandler != sourceEventHandler2);
				*/
			}
		}

		[Obsolete("SourcePressed is deprecated, and will be removed in a future release. Use InteractionSourcePressed instead. (UnityUpgradable) -> InteractionSourcePressedLegacy", true)]
		public static event InteractionManager.SourceEventHandler SourcePressed
		{
			add
			{
				/*
				InteractionManager.SourceEventHandler sourceEventHandler = InteractionManager.SourcePressed;
				InteractionManager.SourceEventHandler sourceEventHandler2;
				do
				{
					sourceEventHandler2 = sourceEventHandler;
					sourceEventHandler = Interlocked.CompareExchange<InteractionManager.SourceEventHandler>(ref InteractionManager.SourcePressed, (InteractionManager.SourceEventHandler)Delegate.Combine(sourceEventHandler2, value), sourceEventHandler);
				}
				while (sourceEventHandler != sourceEventHandler2);
				*/
			}
			remove
			{
				/*
				InteractionManager.SourceEventHandler sourceEventHandler = InteractionManager.SourcePressed;
				InteractionManager.SourceEventHandler sourceEventHandler2;
				do
				{
					sourceEventHandler2 = sourceEventHandler;
					sourceEventHandler = Interlocked.CompareExchange<InteractionManager.SourceEventHandler>(ref InteractionManager.SourcePressed, (InteractionManager.SourceEventHandler)Delegate.Remove(sourceEventHandler2, value), sourceEventHandler);
				}
				while (sourceEventHandler != sourceEventHandler2);
				*/
			}
		}

		[Obsolete("SourceReleased is deprecated, and will be removed in a future release. Use InteractionSourceReleased instead. (UnityUpgradable) -> InteractionSourceReleasedLegacy", true)]
		public static event InteractionManager.SourceEventHandler SourceReleased
		{
			add
			{
				/*
				InteractionManager.SourceEventHandler sourceEventHandler = InteractionManager.SourceReleased;
				InteractionManager.SourceEventHandler sourceEventHandler2;
				do
				{
					sourceEventHandler2 = sourceEventHandler;
					sourceEventHandler = Interlocked.CompareExchange<InteractionManager.SourceEventHandler>(ref InteractionManager.SourceReleased, (InteractionManager.SourceEventHandler)Delegate.Combine(sourceEventHandler2, value), sourceEventHandler);
				}
				while (sourceEventHandler != sourceEventHandler2);
				*/
			}
			remove
			{
				/*
				InteractionManager.SourceEventHandler sourceEventHandler = InteractionManager.SourceReleased;
				InteractionManager.SourceEventHandler sourceEventHandler2;
				do
				{
					sourceEventHandler2 = sourceEventHandler;
					sourceEventHandler = Interlocked.CompareExchange<InteractionManager.SourceEventHandler>(ref InteractionManager.SourceReleased, (InteractionManager.SourceEventHandler)Delegate.Remove(sourceEventHandler2, value), sourceEventHandler);
				}
				while (sourceEventHandler != sourceEventHandler2);
				*/
			}
		}

		[Obsolete("SourceUpdated is deprecated, and will be removed in a future release. Use InteractionSourceUpdated instead. (UnityUpgradable) -> InteractionSourceUpdatedLegacy", true)]
		public static event InteractionManager.SourceEventHandler SourceUpdated
		{
			add
			{
				/*
				InteractionManager.SourceEventHandler sourceEventHandler = InteractionManager.SourceUpdated;
				InteractionManager.SourceEventHandler sourceEventHandler2;
				do
				{
					sourceEventHandler2 = sourceEventHandler;
					sourceEventHandler = Interlocked.CompareExchange<InteractionManager.SourceEventHandler>(ref InteractionManager.SourceUpdated, (InteractionManager.SourceEventHandler)Delegate.Combine(sourceEventHandler2, value), sourceEventHandler);
				}
				while (sourceEventHandler != sourceEventHandler2);
				*/
			}
			remove
			{
				/*
				InteractionManager.SourceEventHandler sourceEventHandler = InteractionManager.SourceUpdated;
				InteractionManager.SourceEventHandler sourceEventHandler2;
				do
				{
					sourceEventHandler2 = sourceEventHandler;
					sourceEventHandler = Interlocked.CompareExchange<InteractionManager.SourceEventHandler>(ref InteractionManager.SourceUpdated, (InteractionManager.SourceEventHandler)Delegate.Remove(sourceEventHandler2, value), sourceEventHandler);
				}
				while (sourceEventHandler != sourceEventHandler2);
				*/
			}
		}

		[Obsolete("InteractionSourceDetectedLegacy is deprecated, and will be removed in a future release. Use InteractionSourceDetected instead.", false)]
		public static event InteractionManager.SourceEventHandler InteractionSourceDetectedLegacy
		{
			add
			{
				/*
				InteractionManager.SourceEventHandler sourceEventHandler = InteractionManager.InteractionSourceDetectedLegacy;
				InteractionManager.SourceEventHandler sourceEventHandler2;
				do
				{
					sourceEventHandler2 = sourceEventHandler;
					sourceEventHandler = Interlocked.CompareExchange<InteractionManager.SourceEventHandler>(ref InteractionManager.InteractionSourceDetectedLegacy, (InteractionManager.SourceEventHandler)Delegate.Combine(sourceEventHandler2, value), sourceEventHandler);
				}
				while (sourceEventHandler != sourceEventHandler2);
				*/
			}
			remove
			{
				/*
				InteractionManager.SourceEventHandler sourceEventHandler = InteractionManager.InteractionSourceDetectedLegacy;
				InteractionManager.SourceEventHandler sourceEventHandler2;
				do
				{
					sourceEventHandler2 = sourceEventHandler;
					sourceEventHandler = Interlocked.CompareExchange<InteractionManager.SourceEventHandler>(ref InteractionManager.InteractionSourceDetectedLegacy, (InteractionManager.SourceEventHandler)Delegate.Remove(sourceEventHandler2, value), sourceEventHandler);
				}
				while (sourceEventHandler != sourceEventHandler2);
				*/
			}
		}

		[Obsolete("InteractionSourceLostLegacy is deprecated, and will be removed in a future release. Use InteractionSourceLost instead.", false)]
		public static event InteractionManager.SourceEventHandler InteractionSourceLostLegacy
		{
			add
			{
				/*
				InteractionManager.SourceEventHandler sourceEventHandler = InteractionManager.InteractionSourceLostLegacy;
				InteractionManager.SourceEventHandler sourceEventHandler2;
				do
				{
					sourceEventHandler2 = sourceEventHandler;
					sourceEventHandler = Interlocked.CompareExchange<InteractionManager.SourceEventHandler>(ref InteractionManager.InteractionSourceLostLegacy, (InteractionManager.SourceEventHandler)Delegate.Combine(sourceEventHandler2, value), sourceEventHandler);
				}
				while (sourceEventHandler != sourceEventHandler2);
				*/
			}
			remove
			{
				/*
				InteractionManager.SourceEventHandler sourceEventHandler = InteractionManager.InteractionSourceLostLegacy;
				InteractionManager.SourceEventHandler sourceEventHandler2;
				do
				{
					sourceEventHandler2 = sourceEventHandler;
					sourceEventHandler = Interlocked.CompareExchange<InteractionManager.SourceEventHandler>(ref InteractionManager.InteractionSourceLostLegacy, (InteractionManager.SourceEventHandler)Delegate.Remove(sourceEventHandler2, value), sourceEventHandler);
				}
				while (sourceEventHandler != sourceEventHandler2);
				*/
			}
		}

		[Obsolete("InteractionSourcePressedLegacy has been deprecated, and will be removed in a future release. Use InteractionSourcePressed instead.", false)]
		public static event InteractionManager.SourceEventHandler InteractionSourcePressedLegacy
		{
			add
			{
				/*
				InteractionManager.SourceEventHandler sourceEventHandler = InteractionManager.InteractionSourcePressedLegacy;
				InteractionManager.SourceEventHandler sourceEventHandler2;
				do
				{
					sourceEventHandler2 = sourceEventHandler;
					sourceEventHandler = Interlocked.CompareExchange<InteractionManager.SourceEventHandler>(ref InteractionManager.InteractionSourcePressedLegacy, (InteractionManager.SourceEventHandler)Delegate.Combine(sourceEventHandler2, value), sourceEventHandler);
				}
				while (sourceEventHandler != sourceEventHandler2);
				*/
			}
			remove
			{
				/*
				InteractionManager.SourceEventHandler sourceEventHandler = InteractionManager.InteractionSourcePressedLegacy;
				InteractionManager.SourceEventHandler sourceEventHandler2;
				do
				{
					sourceEventHandler2 = sourceEventHandler;
					sourceEventHandler = Interlocked.CompareExchange<InteractionManager.SourceEventHandler>(ref InteractionManager.InteractionSourcePressedLegacy, (InteractionManager.SourceEventHandler)Delegate.Remove(sourceEventHandler2, value), sourceEventHandler);
				}
				while (sourceEventHandler != sourceEventHandler2);
				*/
			}
		}

		[Obsolete("InteractionSourceReleasedLegacy has been deprecated, and will be removed in a future release. Use InteractionSourceReleased instead.", false)]
		public static event InteractionManager.SourceEventHandler InteractionSourceReleasedLegacy
		{
			add
			{
				/*
				InteractionManager.SourceEventHandler sourceEventHandler = InteractionManager.InteractionSourceReleasedLegacy;
				InteractionManager.SourceEventHandler sourceEventHandler2;
				do
				{
					sourceEventHandler2 = sourceEventHandler;
					sourceEventHandler = Interlocked.CompareExchange<InteractionManager.SourceEventHandler>(ref InteractionManager.InteractionSourceReleasedLegacy, (InteractionManager.SourceEventHandler)Delegate.Combine(sourceEventHandler2, value), sourceEventHandler);
				}
				while (sourceEventHandler != sourceEventHandler2);
				*/
			}
			remove
			{
				/*
				InteractionManager.SourceEventHandler sourceEventHandler = InteractionManager.InteractionSourceReleasedLegacy;
				InteractionManager.SourceEventHandler sourceEventHandler2;
				do
				{
					sourceEventHandler2 = sourceEventHandler;
					sourceEventHandler = Interlocked.CompareExchange<InteractionManager.SourceEventHandler>(ref InteractionManager.InteractionSourceReleasedLegacy, (InteractionManager.SourceEventHandler)Delegate.Remove(sourceEventHandler2, value), sourceEventHandler);
				}
				while (sourceEventHandler != sourceEventHandler2);
				*/
			}
		}

		[Obsolete("InteractionSourceUpdatedLegacy has been deprecated, and will be removed in a future release. Use InteractionSourceUpdated instead.", false)]
		public static event InteractionManager.SourceEventHandler InteractionSourceUpdatedLegacy
		{
			add
			{
				/*
				InteractionManager.SourceEventHandler sourceEventHandler = InteractionManager.InteractionSourceUpdatedLegacy;
				InteractionManager.SourceEventHandler sourceEventHandler2;
				do
				{
					sourceEventHandler2 = sourceEventHandler;
					sourceEventHandler = Interlocked.CompareExchange<InteractionManager.SourceEventHandler>(ref InteractionManager.InteractionSourceUpdatedLegacy, (InteractionManager.SourceEventHandler)Delegate.Combine(sourceEventHandler2, value), sourceEventHandler);
				}
				while (sourceEventHandler != sourceEventHandler2);
				*/
			}
			remove
			{
				/*
				InteractionManager.SourceEventHandler sourceEventHandler = InteractionManager.InteractionSourceUpdatedLegacy;
				InteractionManager.SourceEventHandler sourceEventHandler2;
				do
				{
					sourceEventHandler2 = sourceEventHandler;
					sourceEventHandler = Interlocked.CompareExchange<InteractionManager.SourceEventHandler>(ref InteractionManager.InteractionSourceUpdatedLegacy, (InteractionManager.SourceEventHandler)Delegate.Remove(sourceEventHandler2, value), sourceEventHandler);
				}
				while (sourceEventHandler != sourceEventHandler2);
				*/
			}
		}

		public static extern int numSourceStates
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		static InteractionManager()
		{
			/*
			if (InteractionManager.<>f__mg$cache0 == null)
			{
				InteractionManager.<>f__mg$cache0 = new InteractionManager.InternalSourceEventHandler(InteractionManager.OnSourceEvent);
			}
			InteractionManager.m_OnSourceEventHandler = InteractionManager.<>f__mg$cache0;
			InteractionManager.Initialize(Marshal.GetFunctionPointerForDelegate(InteractionManager.m_OnSourceEventHandler));
			*/
		}

		public static int GetCurrentReading(InteractionSourceState[] sourceStates)
		{
			if (sourceStates == null)
			{
				throw new ArgumentNullException("sourceStates");
			}
			int result;
			if (sourceStates.Length > 0)
			{
				result = InteractionManager.GetCurrentReading_Internal(sourceStates);
			}
			else
			{
				result = 0;
			}
			return result;
		}

		public static InteractionSourceState[] GetCurrentReading()
		{
			InteractionSourceState[] array = new InteractionSourceState[InteractionManager.numSourceStates];
			if (array.Length > 0)
			{
				InteractionManager.GetCurrentReading_Internal(array);
			}
			return array;
		}

		[MonoPInvokeCallback(typeof(InteractionManager.InternalSourceEventHandler))]
		private static void OnSourceEvent(InteractionManager.EventType eventType, InteractionSourceState state, InteractionSourcePressType pressType)
		{
			/*
			switch (eventType)
			{
			case InteractionManager.EventType.SourceDetected:
			{
				InteractionManager.SourceEventHandler interactionSourceDetectedLegacy = InteractionManager.InteractionSourceDetectedLegacy;
				if (interactionSourceDetectedLegacy != null)
				{
					interactionSourceDetectedLegacy(state);
				}
				Action<InteractionSourceDetectedEventArgs> interactionSourceDetected = InteractionManager.InteractionSourceDetected;
				if (interactionSourceDetected != null)
				{
					interactionSourceDetected(new InteractionSourceDetectedEventArgs(state));
				}
				break;
			}
			case InteractionManager.EventType.SourceLost:
			{
				InteractionManager.SourceEventHandler interactionSourceLostLegacy = InteractionManager.InteractionSourceLostLegacy;
				if (interactionSourceLostLegacy != null)
				{
					interactionSourceLostLegacy(state);
				}
				Action<InteractionSourceLostEventArgs> interactionSourceLost = InteractionManager.InteractionSourceLost;
				if (interactionSourceLost != null)
				{
					interactionSourceLost(new InteractionSourceLostEventArgs(state));
				}
				break;
			}
			case InteractionManager.EventType.SourceUpdated:
			{
				InteractionManager.SourceEventHandler interactionSourceUpdatedLegacy = InteractionManager.InteractionSourceUpdatedLegacy;
				if (interactionSourceUpdatedLegacy != null)
				{
					interactionSourceUpdatedLegacy(state);
				}
				Action<InteractionSourceUpdatedEventArgs> interactionSourceUpdated = InteractionManager.InteractionSourceUpdated;
				if (interactionSourceUpdated != null)
				{
					interactionSourceUpdated(new InteractionSourceUpdatedEventArgs(state));
				}
				break;
			}
			case InteractionManager.EventType.SourcePressed:
			{
				InteractionManager.SourceEventHandler interactionSourcePressedLegacy = InteractionManager.InteractionSourcePressedLegacy;
				if (interactionSourcePressedLegacy != null)
				{
					interactionSourcePressedLegacy(state);
				}
				Action<InteractionSourcePressedEventArgs> interactionSourcePressed = InteractionManager.InteractionSourcePressed;
				if (interactionSourcePressed != null)
				{
					interactionSourcePressed(new InteractionSourcePressedEventArgs(state, pressType));
				}
				break;
			}
			case InteractionManager.EventType.SourceReleased:
			{
				InteractionManager.SourceEventHandler interactionSourceReleasedLegacy = InteractionManager.InteractionSourceReleasedLegacy;
				if (interactionSourceReleasedLegacy != null)
				{
					interactionSourceReleasedLegacy(state);
				}
				Action<InteractionSourceReleasedEventArgs> interactionSourceReleased = InteractionManager.InteractionSourceReleased;
				if (interactionSourceReleased != null)
				{
					interactionSourceReleased(new InteractionSourceReleasedEventArgs(state, pressType));
				}
				break;
			}
			default:
				throw new ArgumentException("OnSourceEvent: Invalid EventType");
			}
			*/
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetCurrentReading_Internal(InteractionSourceState[] sourceStates);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Initialize(IntPtr internalSourceEventHandler);
	}
}
