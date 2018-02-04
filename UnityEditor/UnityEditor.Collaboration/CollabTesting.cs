using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor.Collaboration
{
	internal class CollabTesting
	{
		[Flags]
		public enum AsyncState
		{
			NotWaiting = 0,
			WaitForJobComplete = 1,
			WaitForChannelMessageHandled = 2
		}

		private static IEnumerator<CollabTesting.AsyncState> _enumerator = null;

		private static Action _runAfter = null;

		private static CollabTesting.AsyncState _nextState = CollabTesting.AsyncState.NotWaiting;

		public static Func<IEnumerable<CollabTesting.AsyncState>> Tick
		{
			set
			{
				CollabTesting._enumerator = value().GetEnumerator();
			}
		}

		public static Action AfterRun
		{
			set
			{
				CollabTesting._runAfter = value;
			}
		}

		public static bool IsRunning
		{
			get
			{
				return CollabTesting._enumerator != null;
			}
		}

		public static void OnJobsCompleted()
		{
			CollabTesting.OnAsyncSignalReceived(CollabTesting.AsyncState.WaitForJobComplete);
		}

		public static void OnChannelMessageHandled()
		{
			CollabTesting.OnAsyncSignalReceived(CollabTesting.AsyncState.WaitForChannelMessageHandled);
		}

		private static void OnAsyncSignalReceived(CollabTesting.AsyncState stateToRemove)
		{
			if ((CollabTesting._nextState & stateToRemove) != CollabTesting.AsyncState.NotWaiting)
			{
				CollabTesting._nextState &= ~stateToRemove;
				if (CollabTesting._nextState == CollabTesting.AsyncState.NotWaiting)
				{
					CollabTesting.Execute();
				}
			}
		}

		public static void Execute()
		{
			if (CollabTesting._enumerator != null)
			{
				if (!Collab.instance.AnyJobRunning())
				{
					try
					{
						if (!CollabTesting._enumerator.MoveNext())
						{
							CollabTesting.End();
						}
						else
						{
							CollabTesting._nextState = CollabTesting._enumerator.Current;
						}
					}
					catch (Exception)
					{
						Debug.LogError("Something Went wrong with the test framework itself");
						throw;
					}
				}
			}
		}

		public static void End()
		{
			if (CollabTesting._enumerator != null)
			{
				CollabTesting._runAfter();
				CollabTesting._enumerator = null;
			}
		}
	}
}
