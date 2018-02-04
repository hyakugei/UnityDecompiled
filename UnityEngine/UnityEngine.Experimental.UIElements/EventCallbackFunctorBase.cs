using System;

namespace UnityEngine.Experimental.UIElements
{
	internal abstract class EventCallbackFunctorBase
	{
		public CallbackPhase phase
		{
			get;
			private set;
		}

		protected EventCallbackFunctorBase(CallbackPhase phase)
		{
			this.phase = phase;
		}

		public abstract void Invoke(EventBase evt);

		public abstract bool IsEquivalentTo(long eventTypeId, Delegate callback, CallbackPhase phase);

		protected bool PhaseMatches(EventBase evt)
		{
			CallbackPhase phase = this.phase;
			bool result;
			if (phase != CallbackPhase.CaptureAndTarget)
			{
				if (phase == CallbackPhase.TargetAndBubbleUp)
				{
					if (evt.propagationPhase != PropagationPhase.AtTarget && evt.propagationPhase != PropagationPhase.BubbleUp)
					{
						result = false;
						return result;
					}
				}
			}
			else if (evt.propagationPhase != PropagationPhase.Capture && evt.propagationPhase != PropagationPhase.AtTarget)
			{
				result = false;
				return result;
			}
			result = true;
			return result;
		}
	}
}
