using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEngine.Scripting;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.XR.WSA.Input
{
	[MovedFrom("UnityEngine.VR.WSA.Input")]
	public sealed class GestureRecognizer : IDisposable
	{
		public delegate void HoldCanceledEventDelegate(InteractionSourceKind source, Ray headRay);

		public delegate void HoldCompletedEventDelegate(InteractionSourceKind source, Ray headRay);

		public delegate void HoldStartedEventDelegate(InteractionSourceKind source, Ray headRay);

		public delegate void TappedEventDelegate(InteractionSourceKind source, int tapCount, Ray headRay);

		public delegate void ManipulationCanceledEventDelegate(InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay);

		public delegate void ManipulationCompletedEventDelegate(InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay);

		public delegate void ManipulationStartedEventDelegate(InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay);

		public delegate void ManipulationUpdatedEventDelegate(InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay);

		public delegate void NavigationCanceledEventDelegate(InteractionSourceKind source, Vector3 normalizedOffset, Ray headRay);

		public delegate void NavigationCompletedEventDelegate(InteractionSourceKind source, Vector3 normalizedOffset, Ray headRay);

		public delegate void NavigationStartedEventDelegate(InteractionSourceKind source, Vector3 normalizedOffset, Ray headRay);

		public delegate void NavigationUpdatedEventDelegate(InteractionSourceKind source, Vector3 normalizedOffset, Ray headRay);

		public delegate void RecognitionEndedEventDelegate(InteractionSourceKind source, Ray headRay);

		public delegate void RecognitionStartedEventDelegate(InteractionSourceKind source, Ray headRay);

		public delegate void GestureErrorDelegate([MarshalAs(UnmanagedType.LPStr)] string error, int hresult);

		private IntPtr m_Recognizer;

		public event Action<HoldCanceledEventArgs> HoldCanceled
		{
			add
			{
				Action<HoldCanceledEventArgs> action = this.HoldCanceled;
				Action<HoldCanceledEventArgs> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action<HoldCanceledEventArgs>>(ref this.HoldCanceled, (Action<HoldCanceledEventArgs>)Delegate.Combine(action2, value), action);
				}
				while (action != action2);
			}
			remove
			{
				Action<HoldCanceledEventArgs> action = this.HoldCanceled;
				Action<HoldCanceledEventArgs> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action<HoldCanceledEventArgs>>(ref this.HoldCanceled, (Action<HoldCanceledEventArgs>)Delegate.Remove(action2, value), action);
				}
				while (action != action2);
			}
		}

		public event Action<HoldCompletedEventArgs> HoldCompleted
		{
			add
			{
				Action<HoldCompletedEventArgs> action = this.HoldCompleted;
				Action<HoldCompletedEventArgs> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action<HoldCompletedEventArgs>>(ref this.HoldCompleted, (Action<HoldCompletedEventArgs>)Delegate.Combine(action2, value), action);
				}
				while (action != action2);
			}
			remove
			{
				Action<HoldCompletedEventArgs> action = this.HoldCompleted;
				Action<HoldCompletedEventArgs> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action<HoldCompletedEventArgs>>(ref this.HoldCompleted, (Action<HoldCompletedEventArgs>)Delegate.Remove(action2, value), action);
				}
				while (action != action2);
			}
		}

		public event Action<HoldStartedEventArgs> HoldStarted
		{
			add
			{
				Action<HoldStartedEventArgs> action = this.HoldStarted;
				Action<HoldStartedEventArgs> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action<HoldStartedEventArgs>>(ref this.HoldStarted, (Action<HoldStartedEventArgs>)Delegate.Combine(action2, value), action);
				}
				while (action != action2);
			}
			remove
			{
				Action<HoldStartedEventArgs> action = this.HoldStarted;
				Action<HoldStartedEventArgs> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action<HoldStartedEventArgs>>(ref this.HoldStarted, (Action<HoldStartedEventArgs>)Delegate.Remove(action2, value), action);
				}
				while (action != action2);
			}
		}

		public event Action<TappedEventArgs> Tapped
		{
			add
			{
				Action<TappedEventArgs> action = this.Tapped;
				Action<TappedEventArgs> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action<TappedEventArgs>>(ref this.Tapped, (Action<TappedEventArgs>)Delegate.Combine(action2, value), action);
				}
				while (action != action2);
			}
			remove
			{
				Action<TappedEventArgs> action = this.Tapped;
				Action<TappedEventArgs> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action<TappedEventArgs>>(ref this.Tapped, (Action<TappedEventArgs>)Delegate.Remove(action2, value), action);
				}
				while (action != action2);
			}
		}

		public event Action<ManipulationCanceledEventArgs> ManipulationCanceled
		{
			add
			{
				Action<ManipulationCanceledEventArgs> action = this.ManipulationCanceled;
				Action<ManipulationCanceledEventArgs> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action<ManipulationCanceledEventArgs>>(ref this.ManipulationCanceled, (Action<ManipulationCanceledEventArgs>)Delegate.Combine(action2, value), action);
				}
				while (action != action2);
			}
			remove
			{
				Action<ManipulationCanceledEventArgs> action = this.ManipulationCanceled;
				Action<ManipulationCanceledEventArgs> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action<ManipulationCanceledEventArgs>>(ref this.ManipulationCanceled, (Action<ManipulationCanceledEventArgs>)Delegate.Remove(action2, value), action);
				}
				while (action != action2);
			}
		}

		public event Action<ManipulationCompletedEventArgs> ManipulationCompleted
		{
			add
			{
				Action<ManipulationCompletedEventArgs> action = this.ManipulationCompleted;
				Action<ManipulationCompletedEventArgs> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action<ManipulationCompletedEventArgs>>(ref this.ManipulationCompleted, (Action<ManipulationCompletedEventArgs>)Delegate.Combine(action2, value), action);
				}
				while (action != action2);
			}
			remove
			{
				Action<ManipulationCompletedEventArgs> action = this.ManipulationCompleted;
				Action<ManipulationCompletedEventArgs> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action<ManipulationCompletedEventArgs>>(ref this.ManipulationCompleted, (Action<ManipulationCompletedEventArgs>)Delegate.Remove(action2, value), action);
				}
				while (action != action2);
			}
		}

		public event Action<ManipulationStartedEventArgs> ManipulationStarted
		{
			add
			{
				Action<ManipulationStartedEventArgs> action = this.ManipulationStarted;
				Action<ManipulationStartedEventArgs> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action<ManipulationStartedEventArgs>>(ref this.ManipulationStarted, (Action<ManipulationStartedEventArgs>)Delegate.Combine(action2, value), action);
				}
				while (action != action2);
			}
			remove
			{
				Action<ManipulationStartedEventArgs> action = this.ManipulationStarted;
				Action<ManipulationStartedEventArgs> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action<ManipulationStartedEventArgs>>(ref this.ManipulationStarted, (Action<ManipulationStartedEventArgs>)Delegate.Remove(action2, value), action);
				}
				while (action != action2);
			}
		}

		public event Action<ManipulationUpdatedEventArgs> ManipulationUpdated
		{
			add
			{
				Action<ManipulationUpdatedEventArgs> action = this.ManipulationUpdated;
				Action<ManipulationUpdatedEventArgs> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action<ManipulationUpdatedEventArgs>>(ref this.ManipulationUpdated, (Action<ManipulationUpdatedEventArgs>)Delegate.Combine(action2, value), action);
				}
				while (action != action2);
			}
			remove
			{
				Action<ManipulationUpdatedEventArgs> action = this.ManipulationUpdated;
				Action<ManipulationUpdatedEventArgs> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action<ManipulationUpdatedEventArgs>>(ref this.ManipulationUpdated, (Action<ManipulationUpdatedEventArgs>)Delegate.Remove(action2, value), action);
				}
				while (action != action2);
			}
		}

		public event Action<NavigationCanceledEventArgs> NavigationCanceled
		{
			add
			{
				Action<NavigationCanceledEventArgs> action = this.NavigationCanceled;
				Action<NavigationCanceledEventArgs> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action<NavigationCanceledEventArgs>>(ref this.NavigationCanceled, (Action<NavigationCanceledEventArgs>)Delegate.Combine(action2, value), action);
				}
				while (action != action2);
			}
			remove
			{
				Action<NavigationCanceledEventArgs> action = this.NavigationCanceled;
				Action<NavigationCanceledEventArgs> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action<NavigationCanceledEventArgs>>(ref this.NavigationCanceled, (Action<NavigationCanceledEventArgs>)Delegate.Remove(action2, value), action);
				}
				while (action != action2);
			}
		}

		public event Action<NavigationCompletedEventArgs> NavigationCompleted
		{
			add
			{
				Action<NavigationCompletedEventArgs> action = this.NavigationCompleted;
				Action<NavigationCompletedEventArgs> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action<NavigationCompletedEventArgs>>(ref this.NavigationCompleted, (Action<NavigationCompletedEventArgs>)Delegate.Combine(action2, value), action);
				}
				while (action != action2);
			}
			remove
			{
				Action<NavigationCompletedEventArgs> action = this.NavigationCompleted;
				Action<NavigationCompletedEventArgs> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action<NavigationCompletedEventArgs>>(ref this.NavigationCompleted, (Action<NavigationCompletedEventArgs>)Delegate.Remove(action2, value), action);
				}
				while (action != action2);
			}
		}

		public event Action<NavigationStartedEventArgs> NavigationStarted
		{
			add
			{
				Action<NavigationStartedEventArgs> action = this.NavigationStarted;
				Action<NavigationStartedEventArgs> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action<NavigationStartedEventArgs>>(ref this.NavigationStarted, (Action<NavigationStartedEventArgs>)Delegate.Combine(action2, value), action);
				}
				while (action != action2);
			}
			remove
			{
				Action<NavigationStartedEventArgs> action = this.NavigationStarted;
				Action<NavigationStartedEventArgs> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action<NavigationStartedEventArgs>>(ref this.NavigationStarted, (Action<NavigationStartedEventArgs>)Delegate.Remove(action2, value), action);
				}
				while (action != action2);
			}
		}

		public event Action<NavigationUpdatedEventArgs> NavigationUpdated
		{
			add
			{
				Action<NavigationUpdatedEventArgs> action = this.NavigationUpdated;
				Action<NavigationUpdatedEventArgs> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action<NavigationUpdatedEventArgs>>(ref this.NavigationUpdated, (Action<NavigationUpdatedEventArgs>)Delegate.Combine(action2, value), action);
				}
				while (action != action2);
			}
			remove
			{
				Action<NavigationUpdatedEventArgs> action = this.NavigationUpdated;
				Action<NavigationUpdatedEventArgs> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action<NavigationUpdatedEventArgs>>(ref this.NavigationUpdated, (Action<NavigationUpdatedEventArgs>)Delegate.Remove(action2, value), action);
				}
				while (action != action2);
			}
		}

		public event Action<RecognitionEndedEventArgs> RecognitionEnded
		{
			add
			{
				Action<RecognitionEndedEventArgs> action = this.RecognitionEnded;
				Action<RecognitionEndedEventArgs> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action<RecognitionEndedEventArgs>>(ref this.RecognitionEnded, (Action<RecognitionEndedEventArgs>)Delegate.Combine(action2, value), action);
				}
				while (action != action2);
			}
			remove
			{
				Action<RecognitionEndedEventArgs> action = this.RecognitionEnded;
				Action<RecognitionEndedEventArgs> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action<RecognitionEndedEventArgs>>(ref this.RecognitionEnded, (Action<RecognitionEndedEventArgs>)Delegate.Remove(action2, value), action);
				}
				while (action != action2);
			}
		}

		public event Action<RecognitionStartedEventArgs> RecognitionStarted
		{
			add
			{
				Action<RecognitionStartedEventArgs> action = this.RecognitionStarted;
				Action<RecognitionStartedEventArgs> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action<RecognitionStartedEventArgs>>(ref this.RecognitionStarted, (Action<RecognitionStartedEventArgs>)Delegate.Combine(action2, value), action);
				}
				while (action != action2);
			}
			remove
			{
				Action<RecognitionStartedEventArgs> action = this.RecognitionStarted;
				Action<RecognitionStartedEventArgs> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action<RecognitionStartedEventArgs>>(ref this.RecognitionStarted, (Action<RecognitionStartedEventArgs>)Delegate.Remove(action2, value), action);
				}
				while (action != action2);
			}
		}

		public event Action<GestureErrorEventArgs> GestureError
		{
			add
			{
				Action<GestureErrorEventArgs> action = this.GestureError;
				Action<GestureErrorEventArgs> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action<GestureErrorEventArgs>>(ref this.GestureError, (Action<GestureErrorEventArgs>)Delegate.Combine(action2, value), action);
				}
				while (action != action2);
			}
			remove
			{
				Action<GestureErrorEventArgs> action = this.GestureError;
				Action<GestureErrorEventArgs> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action<GestureErrorEventArgs>>(ref this.GestureError, (Action<GestureErrorEventArgs>)Delegate.Remove(action2, value), action);
				}
				while (action != action2);
			}
		}

		[Obsolete("HoldCanceledEvent is deprecated, and will be removed in a future release. Use OnHoldCanceledEvent instead.", false)]
		public event GestureRecognizer.HoldCanceledEventDelegate HoldCanceledEvent
		{
			add
			{
				GestureRecognizer.HoldCanceledEventDelegate holdCanceledEventDelegate = this.HoldCanceledEvent;
				GestureRecognizer.HoldCanceledEventDelegate holdCanceledEventDelegate2;
				do
				{
					holdCanceledEventDelegate2 = holdCanceledEventDelegate;
					holdCanceledEventDelegate = Interlocked.CompareExchange<GestureRecognizer.HoldCanceledEventDelegate>(ref this.HoldCanceledEvent, (GestureRecognizer.HoldCanceledEventDelegate)Delegate.Combine(holdCanceledEventDelegate2, value), holdCanceledEventDelegate);
				}
				while (holdCanceledEventDelegate != holdCanceledEventDelegate2);
			}
			remove
			{
				GestureRecognizer.HoldCanceledEventDelegate holdCanceledEventDelegate = this.HoldCanceledEvent;
				GestureRecognizer.HoldCanceledEventDelegate holdCanceledEventDelegate2;
				do
				{
					holdCanceledEventDelegate2 = holdCanceledEventDelegate;
					holdCanceledEventDelegate = Interlocked.CompareExchange<GestureRecognizer.HoldCanceledEventDelegate>(ref this.HoldCanceledEvent, (GestureRecognizer.HoldCanceledEventDelegate)Delegate.Remove(holdCanceledEventDelegate2, value), holdCanceledEventDelegate);
				}
				while (holdCanceledEventDelegate != holdCanceledEventDelegate2);
			}
		}

		[Obsolete("HoldCompletedEvent is deprecated, and will be removed in a future release. Use OnHoldCompletedEvent instead.", false)]
		public event GestureRecognizer.HoldCompletedEventDelegate HoldCompletedEvent
		{
			add
			{
				GestureRecognizer.HoldCompletedEventDelegate holdCompletedEventDelegate = this.HoldCompletedEvent;
				GestureRecognizer.HoldCompletedEventDelegate holdCompletedEventDelegate2;
				do
				{
					holdCompletedEventDelegate2 = holdCompletedEventDelegate;
					holdCompletedEventDelegate = Interlocked.CompareExchange<GestureRecognizer.HoldCompletedEventDelegate>(ref this.HoldCompletedEvent, (GestureRecognizer.HoldCompletedEventDelegate)Delegate.Combine(holdCompletedEventDelegate2, value), holdCompletedEventDelegate);
				}
				while (holdCompletedEventDelegate != holdCompletedEventDelegate2);
			}
			remove
			{
				GestureRecognizer.HoldCompletedEventDelegate holdCompletedEventDelegate = this.HoldCompletedEvent;
				GestureRecognizer.HoldCompletedEventDelegate holdCompletedEventDelegate2;
				do
				{
					holdCompletedEventDelegate2 = holdCompletedEventDelegate;
					holdCompletedEventDelegate = Interlocked.CompareExchange<GestureRecognizer.HoldCompletedEventDelegate>(ref this.HoldCompletedEvent, (GestureRecognizer.HoldCompletedEventDelegate)Delegate.Remove(holdCompletedEventDelegate2, value), holdCompletedEventDelegate);
				}
				while (holdCompletedEventDelegate != holdCompletedEventDelegate2);
			}
		}

		[Obsolete("HoldStartedEvent is deprecated, and will be removed in a future release. Use OnHoldStartedEvent instead.", false)]
		public event GestureRecognizer.HoldStartedEventDelegate HoldStartedEvent
		{
			add
			{
				GestureRecognizer.HoldStartedEventDelegate holdStartedEventDelegate = this.HoldStartedEvent;
				GestureRecognizer.HoldStartedEventDelegate holdStartedEventDelegate2;
				do
				{
					holdStartedEventDelegate2 = holdStartedEventDelegate;
					holdStartedEventDelegate = Interlocked.CompareExchange<GestureRecognizer.HoldStartedEventDelegate>(ref this.HoldStartedEvent, (GestureRecognizer.HoldStartedEventDelegate)Delegate.Combine(holdStartedEventDelegate2, value), holdStartedEventDelegate);
				}
				while (holdStartedEventDelegate != holdStartedEventDelegate2);
			}
			remove
			{
				GestureRecognizer.HoldStartedEventDelegate holdStartedEventDelegate = this.HoldStartedEvent;
				GestureRecognizer.HoldStartedEventDelegate holdStartedEventDelegate2;
				do
				{
					holdStartedEventDelegate2 = holdStartedEventDelegate;
					holdStartedEventDelegate = Interlocked.CompareExchange<GestureRecognizer.HoldStartedEventDelegate>(ref this.HoldStartedEvent, (GestureRecognizer.HoldStartedEventDelegate)Delegate.Remove(holdStartedEventDelegate2, value), holdStartedEventDelegate);
				}
				while (holdStartedEventDelegate != holdStartedEventDelegate2);
			}
		}

		[Obsolete("TappedEvent is deprecated, and will be removed in a future release. Use Tapped instead.", false)]
		public event GestureRecognizer.TappedEventDelegate TappedEvent
		{
			add
			{
				GestureRecognizer.TappedEventDelegate tappedEventDelegate = this.TappedEvent;
				GestureRecognizer.TappedEventDelegate tappedEventDelegate2;
				do
				{
					tappedEventDelegate2 = tappedEventDelegate;
					tappedEventDelegate = Interlocked.CompareExchange<GestureRecognizer.TappedEventDelegate>(ref this.TappedEvent, (GestureRecognizer.TappedEventDelegate)Delegate.Combine(tappedEventDelegate2, value), tappedEventDelegate);
				}
				while (tappedEventDelegate != tappedEventDelegate2);
			}
			remove
			{
				GestureRecognizer.TappedEventDelegate tappedEventDelegate = this.TappedEvent;
				GestureRecognizer.TappedEventDelegate tappedEventDelegate2;
				do
				{
					tappedEventDelegate2 = tappedEventDelegate;
					tappedEventDelegate = Interlocked.CompareExchange<GestureRecognizer.TappedEventDelegate>(ref this.TappedEvent, (GestureRecognizer.TappedEventDelegate)Delegate.Remove(tappedEventDelegate2, value), tappedEventDelegate);
				}
				while (tappedEventDelegate != tappedEventDelegate2);
			}
		}

		[Obsolete("ManipulationCanceledEvent is deprecated, and will be removed in a future release. Use OnManipulationCanceledEvent instead.", false)]
		public event GestureRecognizer.ManipulationCanceledEventDelegate ManipulationCanceledEvent
		{
			add
			{
				GestureRecognizer.ManipulationCanceledEventDelegate manipulationCanceledEventDelegate = this.ManipulationCanceledEvent;
				GestureRecognizer.ManipulationCanceledEventDelegate manipulationCanceledEventDelegate2;
				do
				{
					manipulationCanceledEventDelegate2 = manipulationCanceledEventDelegate;
					manipulationCanceledEventDelegate = Interlocked.CompareExchange<GestureRecognizer.ManipulationCanceledEventDelegate>(ref this.ManipulationCanceledEvent, (GestureRecognizer.ManipulationCanceledEventDelegate)Delegate.Combine(manipulationCanceledEventDelegate2, value), manipulationCanceledEventDelegate);
				}
				while (manipulationCanceledEventDelegate != manipulationCanceledEventDelegate2);
			}
			remove
			{
				GestureRecognizer.ManipulationCanceledEventDelegate manipulationCanceledEventDelegate = this.ManipulationCanceledEvent;
				GestureRecognizer.ManipulationCanceledEventDelegate manipulationCanceledEventDelegate2;
				do
				{
					manipulationCanceledEventDelegate2 = manipulationCanceledEventDelegate;
					manipulationCanceledEventDelegate = Interlocked.CompareExchange<GestureRecognizer.ManipulationCanceledEventDelegate>(ref this.ManipulationCanceledEvent, (GestureRecognizer.ManipulationCanceledEventDelegate)Delegate.Remove(manipulationCanceledEventDelegate2, value), manipulationCanceledEventDelegate);
				}
				while (manipulationCanceledEventDelegate != manipulationCanceledEventDelegate2);
			}
		}

		[Obsolete("ManipulationCompletedEvent is deprecated, and will be removed in a future release. Use OnManipulationCompletedEvent instead.", false)]
		public event GestureRecognizer.ManipulationCompletedEventDelegate ManipulationCompletedEvent
		{
			add
			{
				GestureRecognizer.ManipulationCompletedEventDelegate manipulationCompletedEventDelegate = this.ManipulationCompletedEvent;
				GestureRecognizer.ManipulationCompletedEventDelegate manipulationCompletedEventDelegate2;
				do
				{
					manipulationCompletedEventDelegate2 = manipulationCompletedEventDelegate;
					manipulationCompletedEventDelegate = Interlocked.CompareExchange<GestureRecognizer.ManipulationCompletedEventDelegate>(ref this.ManipulationCompletedEvent, (GestureRecognizer.ManipulationCompletedEventDelegate)Delegate.Combine(manipulationCompletedEventDelegate2, value), manipulationCompletedEventDelegate);
				}
				while (manipulationCompletedEventDelegate != manipulationCompletedEventDelegate2);
			}
			remove
			{
				GestureRecognizer.ManipulationCompletedEventDelegate manipulationCompletedEventDelegate = this.ManipulationCompletedEvent;
				GestureRecognizer.ManipulationCompletedEventDelegate manipulationCompletedEventDelegate2;
				do
				{
					manipulationCompletedEventDelegate2 = manipulationCompletedEventDelegate;
					manipulationCompletedEventDelegate = Interlocked.CompareExchange<GestureRecognizer.ManipulationCompletedEventDelegate>(ref this.ManipulationCompletedEvent, (GestureRecognizer.ManipulationCompletedEventDelegate)Delegate.Remove(manipulationCompletedEventDelegate2, value), manipulationCompletedEventDelegate);
				}
				while (manipulationCompletedEventDelegate != manipulationCompletedEventDelegate2);
			}
		}

		[Obsolete("ManipulationStartedEvent is deprecated, and will be removed in a future release. Use OnManipulationStartedEvent instead.", false)]
		public event GestureRecognizer.ManipulationStartedEventDelegate ManipulationStartedEvent
		{
			add
			{
				GestureRecognizer.ManipulationStartedEventDelegate manipulationStartedEventDelegate = this.ManipulationStartedEvent;
				GestureRecognizer.ManipulationStartedEventDelegate manipulationStartedEventDelegate2;
				do
				{
					manipulationStartedEventDelegate2 = manipulationStartedEventDelegate;
					manipulationStartedEventDelegate = Interlocked.CompareExchange<GestureRecognizer.ManipulationStartedEventDelegate>(ref this.ManipulationStartedEvent, (GestureRecognizer.ManipulationStartedEventDelegate)Delegate.Combine(manipulationStartedEventDelegate2, value), manipulationStartedEventDelegate);
				}
				while (manipulationStartedEventDelegate != manipulationStartedEventDelegate2);
			}
			remove
			{
				GestureRecognizer.ManipulationStartedEventDelegate manipulationStartedEventDelegate = this.ManipulationStartedEvent;
				GestureRecognizer.ManipulationStartedEventDelegate manipulationStartedEventDelegate2;
				do
				{
					manipulationStartedEventDelegate2 = manipulationStartedEventDelegate;
					manipulationStartedEventDelegate = Interlocked.CompareExchange<GestureRecognizer.ManipulationStartedEventDelegate>(ref this.ManipulationStartedEvent, (GestureRecognizer.ManipulationStartedEventDelegate)Delegate.Remove(manipulationStartedEventDelegate2, value), manipulationStartedEventDelegate);
				}
				while (manipulationStartedEventDelegate != manipulationStartedEventDelegate2);
			}
		}

		[Obsolete("ManipulationUpdatedEvent is deprecated, and will be removed in a future release. Use OnManipulationUpdatedEvent instead.", false)]
		public event GestureRecognizer.ManipulationUpdatedEventDelegate ManipulationUpdatedEvent
		{
			add
			{
				GestureRecognizer.ManipulationUpdatedEventDelegate manipulationUpdatedEventDelegate = this.ManipulationUpdatedEvent;
				GestureRecognizer.ManipulationUpdatedEventDelegate manipulationUpdatedEventDelegate2;
				do
				{
					manipulationUpdatedEventDelegate2 = manipulationUpdatedEventDelegate;
					manipulationUpdatedEventDelegate = Interlocked.CompareExchange<GestureRecognizer.ManipulationUpdatedEventDelegate>(ref this.ManipulationUpdatedEvent, (GestureRecognizer.ManipulationUpdatedEventDelegate)Delegate.Combine(manipulationUpdatedEventDelegate2, value), manipulationUpdatedEventDelegate);
				}
				while (manipulationUpdatedEventDelegate != manipulationUpdatedEventDelegate2);
			}
			remove
			{
				GestureRecognizer.ManipulationUpdatedEventDelegate manipulationUpdatedEventDelegate = this.ManipulationUpdatedEvent;
				GestureRecognizer.ManipulationUpdatedEventDelegate manipulationUpdatedEventDelegate2;
				do
				{
					manipulationUpdatedEventDelegate2 = manipulationUpdatedEventDelegate;
					manipulationUpdatedEventDelegate = Interlocked.CompareExchange<GestureRecognizer.ManipulationUpdatedEventDelegate>(ref this.ManipulationUpdatedEvent, (GestureRecognizer.ManipulationUpdatedEventDelegate)Delegate.Remove(manipulationUpdatedEventDelegate2, value), manipulationUpdatedEventDelegate);
				}
				while (manipulationUpdatedEventDelegate != manipulationUpdatedEventDelegate2);
			}
		}

		[Obsolete("NavigationCanceledEvent is deprecated, and will be removed in a future release. Use OnNavigationCanceledEvent instead.", false)]
		public event GestureRecognizer.NavigationCanceledEventDelegate NavigationCanceledEvent
		{
			add
			{
				GestureRecognizer.NavigationCanceledEventDelegate navigationCanceledEventDelegate = this.NavigationCanceledEvent;
				GestureRecognizer.NavigationCanceledEventDelegate navigationCanceledEventDelegate2;
				do
				{
					navigationCanceledEventDelegate2 = navigationCanceledEventDelegate;
					navigationCanceledEventDelegate = Interlocked.CompareExchange<GestureRecognizer.NavigationCanceledEventDelegate>(ref this.NavigationCanceledEvent, (GestureRecognizer.NavigationCanceledEventDelegate)Delegate.Combine(navigationCanceledEventDelegate2, value), navigationCanceledEventDelegate);
				}
				while (navigationCanceledEventDelegate != navigationCanceledEventDelegate2);
			}
			remove
			{
				GestureRecognizer.NavigationCanceledEventDelegate navigationCanceledEventDelegate = this.NavigationCanceledEvent;
				GestureRecognizer.NavigationCanceledEventDelegate navigationCanceledEventDelegate2;
				do
				{
					navigationCanceledEventDelegate2 = navigationCanceledEventDelegate;
					navigationCanceledEventDelegate = Interlocked.CompareExchange<GestureRecognizer.NavigationCanceledEventDelegate>(ref this.NavigationCanceledEvent, (GestureRecognizer.NavigationCanceledEventDelegate)Delegate.Remove(navigationCanceledEventDelegate2, value), navigationCanceledEventDelegate);
				}
				while (navigationCanceledEventDelegate != navigationCanceledEventDelegate2);
			}
		}

		[Obsolete("NavigationCompletedEvent is deprecated, and will be removed in a future release. Use OnNavigationCompletedEvent instead.", false)]
		public event GestureRecognizer.NavigationCompletedEventDelegate NavigationCompletedEvent
		{
			add
			{
				GestureRecognizer.NavigationCompletedEventDelegate navigationCompletedEventDelegate = this.NavigationCompletedEvent;
				GestureRecognizer.NavigationCompletedEventDelegate navigationCompletedEventDelegate2;
				do
				{
					navigationCompletedEventDelegate2 = navigationCompletedEventDelegate;
					navigationCompletedEventDelegate = Interlocked.CompareExchange<GestureRecognizer.NavigationCompletedEventDelegate>(ref this.NavigationCompletedEvent, (GestureRecognizer.NavigationCompletedEventDelegate)Delegate.Combine(navigationCompletedEventDelegate2, value), navigationCompletedEventDelegate);
				}
				while (navigationCompletedEventDelegate != navigationCompletedEventDelegate2);
			}
			remove
			{
				GestureRecognizer.NavigationCompletedEventDelegate navigationCompletedEventDelegate = this.NavigationCompletedEvent;
				GestureRecognizer.NavigationCompletedEventDelegate navigationCompletedEventDelegate2;
				do
				{
					navigationCompletedEventDelegate2 = navigationCompletedEventDelegate;
					navigationCompletedEventDelegate = Interlocked.CompareExchange<GestureRecognizer.NavigationCompletedEventDelegate>(ref this.NavigationCompletedEvent, (GestureRecognizer.NavigationCompletedEventDelegate)Delegate.Remove(navigationCompletedEventDelegate2, value), navigationCompletedEventDelegate);
				}
				while (navigationCompletedEventDelegate != navigationCompletedEventDelegate2);
			}
		}

		[Obsolete("NavigationStartedEvent is deprecated, and will be removed in a future release. Use OnNavigationStartedEvent instead.", false)]
		public event GestureRecognizer.NavigationStartedEventDelegate NavigationStartedEvent
		{
			add
			{
				GestureRecognizer.NavigationStartedEventDelegate navigationStartedEventDelegate = this.NavigationStartedEvent;
				GestureRecognizer.NavigationStartedEventDelegate navigationStartedEventDelegate2;
				do
				{
					navigationStartedEventDelegate2 = navigationStartedEventDelegate;
					navigationStartedEventDelegate = Interlocked.CompareExchange<GestureRecognizer.NavigationStartedEventDelegate>(ref this.NavigationStartedEvent, (GestureRecognizer.NavigationStartedEventDelegate)Delegate.Combine(navigationStartedEventDelegate2, value), navigationStartedEventDelegate);
				}
				while (navigationStartedEventDelegate != navigationStartedEventDelegate2);
			}
			remove
			{
				GestureRecognizer.NavigationStartedEventDelegate navigationStartedEventDelegate = this.NavigationStartedEvent;
				GestureRecognizer.NavigationStartedEventDelegate navigationStartedEventDelegate2;
				do
				{
					navigationStartedEventDelegate2 = navigationStartedEventDelegate;
					navigationStartedEventDelegate = Interlocked.CompareExchange<GestureRecognizer.NavigationStartedEventDelegate>(ref this.NavigationStartedEvent, (GestureRecognizer.NavigationStartedEventDelegate)Delegate.Remove(navigationStartedEventDelegate2, value), navigationStartedEventDelegate);
				}
				while (navigationStartedEventDelegate != navigationStartedEventDelegate2);
			}
		}

		[Obsolete("NavigationUpdatedEvent is deprecated, and will be removed in a future release. Use OnNavigationUpdatedEvent instead.", false)]
		public event GestureRecognizer.NavigationUpdatedEventDelegate NavigationUpdatedEvent
		{
			add
			{
				GestureRecognizer.NavigationUpdatedEventDelegate navigationUpdatedEventDelegate = this.NavigationUpdatedEvent;
				GestureRecognizer.NavigationUpdatedEventDelegate navigationUpdatedEventDelegate2;
				do
				{
					navigationUpdatedEventDelegate2 = navigationUpdatedEventDelegate;
					navigationUpdatedEventDelegate = Interlocked.CompareExchange<GestureRecognizer.NavigationUpdatedEventDelegate>(ref this.NavigationUpdatedEvent, (GestureRecognizer.NavigationUpdatedEventDelegate)Delegate.Combine(navigationUpdatedEventDelegate2, value), navigationUpdatedEventDelegate);
				}
				while (navigationUpdatedEventDelegate != navigationUpdatedEventDelegate2);
			}
			remove
			{
				GestureRecognizer.NavigationUpdatedEventDelegate navigationUpdatedEventDelegate = this.NavigationUpdatedEvent;
				GestureRecognizer.NavigationUpdatedEventDelegate navigationUpdatedEventDelegate2;
				do
				{
					navigationUpdatedEventDelegate2 = navigationUpdatedEventDelegate;
					navigationUpdatedEventDelegate = Interlocked.CompareExchange<GestureRecognizer.NavigationUpdatedEventDelegate>(ref this.NavigationUpdatedEvent, (GestureRecognizer.NavigationUpdatedEventDelegate)Delegate.Remove(navigationUpdatedEventDelegate2, value), navigationUpdatedEventDelegate);
				}
				while (navigationUpdatedEventDelegate != navigationUpdatedEventDelegate2);
			}
		}

		[Obsolete("RecognitionEndedEvent is deprecated, and will be removed in a future release. Use OnRecognitionEndedEvent instead.", false)]
		public event GestureRecognizer.RecognitionEndedEventDelegate RecognitionEndedEvent
		{
			add
			{
				GestureRecognizer.RecognitionEndedEventDelegate recognitionEndedEventDelegate = this.RecognitionEndedEvent;
				GestureRecognizer.RecognitionEndedEventDelegate recognitionEndedEventDelegate2;
				do
				{
					recognitionEndedEventDelegate2 = recognitionEndedEventDelegate;
					recognitionEndedEventDelegate = Interlocked.CompareExchange<GestureRecognizer.RecognitionEndedEventDelegate>(ref this.RecognitionEndedEvent, (GestureRecognizer.RecognitionEndedEventDelegate)Delegate.Combine(recognitionEndedEventDelegate2, value), recognitionEndedEventDelegate);
				}
				while (recognitionEndedEventDelegate != recognitionEndedEventDelegate2);
			}
			remove
			{
				GestureRecognizer.RecognitionEndedEventDelegate recognitionEndedEventDelegate = this.RecognitionEndedEvent;
				GestureRecognizer.RecognitionEndedEventDelegate recognitionEndedEventDelegate2;
				do
				{
					recognitionEndedEventDelegate2 = recognitionEndedEventDelegate;
					recognitionEndedEventDelegate = Interlocked.CompareExchange<GestureRecognizer.RecognitionEndedEventDelegate>(ref this.RecognitionEndedEvent, (GestureRecognizer.RecognitionEndedEventDelegate)Delegate.Remove(recognitionEndedEventDelegate2, value), recognitionEndedEventDelegate);
				}
				while (recognitionEndedEventDelegate != recognitionEndedEventDelegate2);
			}
		}

		[Obsolete("RecognitionStartedEvent is deprecated, and will be removed in a future release. Use OnRecognitionStartedEvent instead.", false)]
		public event GestureRecognizer.RecognitionStartedEventDelegate RecognitionStartedEvent
		{
			add
			{
				GestureRecognizer.RecognitionStartedEventDelegate recognitionStartedEventDelegate = this.RecognitionStartedEvent;
				GestureRecognizer.RecognitionStartedEventDelegate recognitionStartedEventDelegate2;
				do
				{
					recognitionStartedEventDelegate2 = recognitionStartedEventDelegate;
					recognitionStartedEventDelegate = Interlocked.CompareExchange<GestureRecognizer.RecognitionStartedEventDelegate>(ref this.RecognitionStartedEvent, (GestureRecognizer.RecognitionStartedEventDelegate)Delegate.Combine(recognitionStartedEventDelegate2, value), recognitionStartedEventDelegate);
				}
				while (recognitionStartedEventDelegate != recognitionStartedEventDelegate2);
			}
			remove
			{
				GestureRecognizer.RecognitionStartedEventDelegate recognitionStartedEventDelegate = this.RecognitionStartedEvent;
				GestureRecognizer.RecognitionStartedEventDelegate recognitionStartedEventDelegate2;
				do
				{
					recognitionStartedEventDelegate2 = recognitionStartedEventDelegate;
					recognitionStartedEventDelegate = Interlocked.CompareExchange<GestureRecognizer.RecognitionStartedEventDelegate>(ref this.RecognitionStartedEvent, (GestureRecognizer.RecognitionStartedEventDelegate)Delegate.Remove(recognitionStartedEventDelegate2, value), recognitionStartedEventDelegate);
				}
				while (recognitionStartedEventDelegate != recognitionStartedEventDelegate2);
			}
		}

		[Obsolete("GestureErrorEvent is deprecated, and will be removed in a future release. Use OnGestureErrorEvent instead.", false)]
		public event GestureRecognizer.GestureErrorDelegate GestureErrorEvent
		{
			add
			{
				GestureRecognizer.GestureErrorDelegate gestureErrorDelegate = this.GestureErrorEvent;
				GestureRecognizer.GestureErrorDelegate gestureErrorDelegate2;
				do
				{
					gestureErrorDelegate2 = gestureErrorDelegate;
					gestureErrorDelegate = Interlocked.CompareExchange<GestureRecognizer.GestureErrorDelegate>(ref this.GestureErrorEvent, (GestureRecognizer.GestureErrorDelegate)Delegate.Combine(gestureErrorDelegate2, value), gestureErrorDelegate);
				}
				while (gestureErrorDelegate != gestureErrorDelegate2);
			}
			remove
			{
				GestureRecognizer.GestureErrorDelegate gestureErrorDelegate = this.GestureErrorEvent;
				GestureRecognizer.GestureErrorDelegate gestureErrorDelegate2;
				do
				{
					gestureErrorDelegate2 = gestureErrorDelegate;
					gestureErrorDelegate = Interlocked.CompareExchange<GestureRecognizer.GestureErrorDelegate>(ref this.GestureErrorEvent, (GestureRecognizer.GestureErrorDelegate)Delegate.Remove(gestureErrorDelegate2, value), gestureErrorDelegate);
				}
				while (gestureErrorDelegate != gestureErrorDelegate2);
			}
		}

		public GestureRecognizer()
		{
			this.m_Recognizer = this.Internal_Create();
		}

		~GestureRecognizer()
		{
			if (this.m_Recognizer != IntPtr.Zero)
			{
				GestureRecognizer.DestroyThreaded(this.m_Recognizer);
				this.m_Recognizer = IntPtr.Zero;
				GC.SuppressFinalize(this);
			}
		}

		public void Dispose()
		{
			if (this.m_Recognizer != IntPtr.Zero)
			{
				GestureRecognizer.Destroy(this.m_Recognizer);
				this.m_Recognizer = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}

		public GestureSettings SetRecognizableGestures(GestureSettings newMaskValue)
		{
			GestureSettings result;
			if (this.m_Recognizer != IntPtr.Zero)
			{
				result = (GestureSettings)this.Internal_SetRecognizableGestures(this.m_Recognizer, (int)newMaskValue);
			}
			else
			{
				result = GestureSettings.None;
			}
			return result;
		}

		public GestureSettings GetRecognizableGestures()
		{
			GestureSettings result;
			if (this.m_Recognizer != IntPtr.Zero)
			{
				result = (GestureSettings)this.Internal_GetRecognizableGestures(this.m_Recognizer);
			}
			else
			{
				result = GestureSettings.None;
			}
			return result;
		}

		public void StartCapturingGestures()
		{
			if (this.m_Recognizer != IntPtr.Zero)
			{
				this.Internal_StartCapturingGestures(this.m_Recognizer);
			}
		}

		public void StopCapturingGestures()
		{
			if (this.m_Recognizer != IntPtr.Zero)
			{
				this.Internal_StopCapturingGestures(this.m_Recognizer);
			}
		}

		public bool IsCapturingGestures()
		{
			return this.m_Recognizer != IntPtr.Zero && this.Internal_IsCapturingGestures(this.m_Recognizer);
		}

		public void CancelGestures()
		{
			if (this.m_Recognizer != IntPtr.Zero)
			{
				this.Internal_CancelGestures(this.m_Recognizer);
			}
		}

		[RequiredByNativeCode]
		private void InvokeHoldCanceled(InteractionSource source, InteractionSourcePose sourcePose, Pose headPose)
		{
			GestureRecognizer.HoldCanceledEventDelegate holdCanceledEvent = this.HoldCanceledEvent;
			if (holdCanceledEvent != null)
			{
				holdCanceledEvent(source.m_SourceKind, new Ray(headPose.position, headPose.rotation * Vector3.forward));
			}
			Action<HoldCanceledEventArgs> holdCanceled = this.HoldCanceled;
			if (holdCanceled != null)
			{
				HoldCanceledEventArgs obj;
				obj.m_Source = source;
				obj.m_SourcePose = sourcePose;
				obj.m_HeadPose = headPose;
				holdCanceled(obj);
			}
		}

		[RequiredByNativeCode]
		private void InvokeHoldCompleted(InteractionSource source, InteractionSourcePose sourcePose, Pose headPose)
		{
			GestureRecognizer.HoldCompletedEventDelegate holdCompletedEvent = this.HoldCompletedEvent;
			if (holdCompletedEvent != null)
			{
				holdCompletedEvent(source.m_SourceKind, new Ray(headPose.position, headPose.rotation * Vector3.forward));
			}
			Action<HoldCompletedEventArgs> holdCompleted = this.HoldCompleted;
			if (holdCompleted != null)
			{
				HoldCompletedEventArgs obj;
				obj.m_Source = source;
				obj.m_SourcePose = sourcePose;
				obj.m_HeadPose = headPose;
				holdCompleted(obj);
			}
		}

		[RequiredByNativeCode]
		private void InvokeHoldStarted(InteractionSource source, InteractionSourcePose sourcePose, Pose headPose)
		{
			GestureRecognizer.HoldStartedEventDelegate holdStartedEvent = this.HoldStartedEvent;
			if (holdStartedEvent != null)
			{
				holdStartedEvent(source.m_SourceKind, new Ray(headPose.position, headPose.rotation * Vector3.forward));
			}
			Action<HoldStartedEventArgs> holdStarted = this.HoldStarted;
			if (holdStarted != null)
			{
				HoldStartedEventArgs obj;
				obj.m_Source = source;
				obj.m_SourcePose = sourcePose;
				obj.m_HeadPose = headPose;
				holdStarted(obj);
			}
		}

		[RequiredByNativeCode]
		private void InvokeTapped(InteractionSource source, InteractionSourcePose sourcePose, Pose headPose, int tapCount)
		{
			GestureRecognizer.TappedEventDelegate tappedEvent = this.TappedEvent;
			if (tappedEvent != null)
			{
				tappedEvent(source.m_SourceKind, tapCount, new Ray(headPose.position, headPose.rotation * Vector3.forward));
			}
			Action<TappedEventArgs> tapped = this.Tapped;
			if (tapped != null)
			{
				TappedEventArgs obj;
				obj.m_Source = source;
				obj.m_SourcePose = sourcePose;
				obj.m_HeadPose = headPose;
				obj.m_TapCount = tapCount;
				tapped(obj);
			}
		}

		[RequiredByNativeCode]
		private void InvokeManipulationCanceled(InteractionSource source, InteractionSourcePose sourcePose, Pose headPose)
		{
			GestureRecognizer.ManipulationCanceledEventDelegate manipulationCanceledEvent = this.ManipulationCanceledEvent;
			if (manipulationCanceledEvent != null)
			{
				manipulationCanceledEvent(source.m_SourceKind, Vector3.zero, new Ray(headPose.position, headPose.rotation * Vector3.forward));
			}
			Action<ManipulationCanceledEventArgs> manipulationCanceled = this.ManipulationCanceled;
			if (manipulationCanceled != null)
			{
				ManipulationCanceledEventArgs obj;
				obj.m_Source = source;
				obj.m_SourcePose = sourcePose;
				obj.m_HeadPose = headPose;
				manipulationCanceled(obj);
			}
		}

		[RequiredByNativeCode]
		private void InvokeManipulationCompleted(InteractionSource source, InteractionSourcePose sourcePose, Pose headPose, Vector3 cumulativeDelta)
		{
			GestureRecognizer.ManipulationCompletedEventDelegate manipulationCompletedEvent = this.ManipulationCompletedEvent;
			if (manipulationCompletedEvent != null)
			{
				manipulationCompletedEvent(source.m_SourceKind, cumulativeDelta, new Ray(headPose.position, headPose.rotation * Vector3.forward));
			}
			Action<ManipulationCompletedEventArgs> manipulationCompleted = this.ManipulationCompleted;
			if (manipulationCompleted != null)
			{
				ManipulationCompletedEventArgs obj;
				obj.m_Source = source;
				obj.m_SourcePose = sourcePose;
				obj.m_HeadPose = headPose;
				obj.m_CumulativeDelta = cumulativeDelta;
				manipulationCompleted(obj);
			}
		}

		[RequiredByNativeCode]
		private void InvokeManipulationStarted(InteractionSource source, InteractionSourcePose sourcePose, Pose headPose)
		{
			GestureRecognizer.ManipulationStartedEventDelegate manipulationStartedEvent = this.ManipulationStartedEvent;
			if (manipulationStartedEvent != null)
			{
				manipulationStartedEvent(source.m_SourceKind, Vector3.zero, new Ray(headPose.position, headPose.rotation * Vector3.forward));
			}
			Action<ManipulationStartedEventArgs> manipulationStarted = this.ManipulationStarted;
			if (manipulationStarted != null)
			{
				ManipulationStartedEventArgs obj;
				obj.m_Source = source;
				obj.m_SourcePose = sourcePose;
				obj.m_HeadPose = headPose;
				manipulationStarted(obj);
			}
		}

		[RequiredByNativeCode]
		private void InvokeManipulationUpdated(InteractionSource source, InteractionSourcePose sourcePose, Pose headPose, Vector3 cumulativeDelta)
		{
			GestureRecognizer.ManipulationUpdatedEventDelegate manipulationUpdatedEvent = this.ManipulationUpdatedEvent;
			if (manipulationUpdatedEvent != null)
			{
				manipulationUpdatedEvent(source.m_SourceKind, cumulativeDelta, new Ray(headPose.position, headPose.rotation * Vector3.forward));
			}
			Action<ManipulationUpdatedEventArgs> manipulationUpdated = this.ManipulationUpdated;
			if (manipulationUpdated != null)
			{
				ManipulationUpdatedEventArgs obj;
				obj.m_Source = source;
				obj.m_SourcePose = sourcePose;
				obj.m_HeadPose = headPose;
				obj.m_CumulativeDelta = cumulativeDelta;
				manipulationUpdated(obj);
			}
		}

		[RequiredByNativeCode]
		private void InvokeNavigationCanceled(InteractionSource source, InteractionSourcePose sourcePose, Pose headPose)
		{
			GestureRecognizer.NavigationCanceledEventDelegate navigationCanceledEvent = this.NavigationCanceledEvent;
			if (navigationCanceledEvent != null)
			{
				navigationCanceledEvent(source.m_SourceKind, Vector3.zero, new Ray(headPose.position, headPose.rotation * Vector3.forward));
			}
			Action<NavigationCanceledEventArgs> navigationCanceled = this.NavigationCanceled;
			if (navigationCanceled != null)
			{
				NavigationCanceledEventArgs obj;
				obj.m_Source = source;
				obj.m_SourcePose = sourcePose;
				obj.m_HeadPose = headPose;
				navigationCanceled(obj);
			}
		}

		[RequiredByNativeCode]
		private void InvokeNavigationCompleted(InteractionSource source, InteractionSourcePose sourcePose, Pose headPose, Vector3 normalizedOffset)
		{
			GestureRecognizer.NavigationCompletedEventDelegate navigationCompletedEvent = this.NavigationCompletedEvent;
			if (navigationCompletedEvent != null)
			{
				navigationCompletedEvent(source.m_SourceKind, normalizedOffset, new Ray(headPose.position, headPose.rotation * Vector3.forward));
			}
			Action<NavigationCompletedEventArgs> navigationCompleted = this.NavigationCompleted;
			if (navigationCompleted != null)
			{
				NavigationCompletedEventArgs obj;
				obj.m_Source = source;
				obj.m_SourcePose = sourcePose;
				obj.m_HeadPose = headPose;
				obj.m_NormalizedOffset = normalizedOffset;
				navigationCompleted(obj);
			}
		}

		[RequiredByNativeCode]
		private void InvokeNavigationStarted(InteractionSource source, InteractionSourcePose sourcePose, Pose headPose)
		{
			GestureRecognizer.NavigationStartedEventDelegate navigationStartedEvent = this.NavigationStartedEvent;
			if (navigationStartedEvent != null)
			{
				navigationStartedEvent(source.m_SourceKind, Vector3.zero, new Ray(headPose.position, headPose.rotation * Vector3.forward));
			}
			Action<NavigationStartedEventArgs> navigationStarted = this.NavigationStarted;
			if (navigationStarted != null)
			{
				NavigationStartedEventArgs obj;
				obj.m_Source = source;
				obj.m_SourcePose = sourcePose;
				obj.m_HeadPose = headPose;
				navigationStarted(obj);
			}
		}

		[RequiredByNativeCode]
		private void InvokeNavigationUpdated(InteractionSource source, InteractionSourcePose sourcePose, Pose headPose, Vector3 normalizedOffset)
		{
			GestureRecognizer.NavigationUpdatedEventDelegate navigationUpdatedEvent = this.NavigationUpdatedEvent;
			if (navigationUpdatedEvent != null)
			{
				navigationUpdatedEvent(source.m_SourceKind, normalizedOffset, new Ray(headPose.position, headPose.rotation * Vector3.forward));
			}
			Action<NavigationUpdatedEventArgs> navigationUpdated = this.NavigationUpdated;
			if (navigationUpdated != null)
			{
				NavigationUpdatedEventArgs obj;
				obj.m_Source = source;
				obj.m_SourcePose = sourcePose;
				obj.m_HeadPose = headPose;
				obj.m_NormalizedOffset = normalizedOffset;
				navigationUpdated(obj);
			}
		}

		[RequiredByNativeCode]
		private void InvokeRecognitionEnded(InteractionSource source, InteractionSourcePose sourcePose, Pose headPose)
		{
			GestureRecognizer.RecognitionEndedEventDelegate recognitionEndedEvent = this.RecognitionEndedEvent;
			if (recognitionEndedEvent != null)
			{
				recognitionEndedEvent(source.m_SourceKind, new Ray(headPose.position, headPose.rotation * Vector3.forward));
			}
			Action<RecognitionEndedEventArgs> recognitionEnded = this.RecognitionEnded;
			if (recognitionEnded != null)
			{
				RecognitionEndedEventArgs obj;
				obj.m_Source = source;
				obj.m_SourcePose = sourcePose;
				obj.m_HeadPose = headPose;
				recognitionEnded(obj);
			}
		}

		[RequiredByNativeCode]
		private void InvokeRecognitionStarted(InteractionSource source, InteractionSourcePose sourcePose, Pose headPose)
		{
			GestureRecognizer.RecognitionStartedEventDelegate recognitionStartedEvent = this.RecognitionStartedEvent;
			if (recognitionStartedEvent != null)
			{
				recognitionStartedEvent(source.m_SourceKind, new Ray(headPose.position, headPose.rotation * Vector3.forward));
			}
			Action<RecognitionStartedEventArgs> recognitionStarted = this.RecognitionStarted;
			if (recognitionStarted != null)
			{
				RecognitionStartedEventArgs obj;
				obj.m_Source = source;
				obj.m_SourcePose = sourcePose;
				obj.m_HeadPose = headPose;
				recognitionStarted(obj);
			}
		}

		[RequiredByNativeCode]
		private void InvokeErrorEvent(string error, int hresult)
		{
			GestureRecognizer.GestureErrorDelegate gestureErrorEvent = this.GestureErrorEvent;
			if (gestureErrorEvent != null)
			{
				gestureErrorEvent(error, hresult);
			}
			Action<GestureErrorEventArgs> gestureError = this.GestureError;
			if (gestureError != null)
			{
				gestureError(new GestureErrorEventArgs(error, hresult));
			}
		}

		private IntPtr Internal_Create()
		{
			IntPtr result;
			GestureRecognizer.INTERNAL_CALL_Internal_Create(this, out result);
			return result;
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_Internal_Create(GestureRecognizer self, out IntPtr value);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Destroy(IntPtr recognizer);

		[GeneratedByOldBindingsGenerator, ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void DestroyThreaded(IntPtr recognizer);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Internal_StartCapturingGestures(IntPtr recognizer);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Internal_StopCapturingGestures(IntPtr recognizer);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern bool Internal_IsCapturingGestures(IntPtr recognizer);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int Internal_SetRecognizableGestures(IntPtr recognizer, int newMaskValue);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int Internal_GetRecognizableGestures(IntPtr recognizer);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Internal_CancelGestures(IntPtr recognizer);
	}
}
