using System;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine.Playables;
using UnityEngine.Scripting;

namespace UnityEditor.Playables
{
	public static class Utility
	{
		public static event Action<PlayableGraph> graphCreated
		{
			add
			{
				Action<PlayableGraph> action = Utility.graphCreated;
				Action<PlayableGraph> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action<PlayableGraph>>(ref Utility.graphCreated, (Action<PlayableGraph>)Delegate.Combine(action2, value), action);
				}
				while (action != action2);
			}
			remove
			{
				Action<PlayableGraph> action = Utility.graphCreated;
				Action<PlayableGraph> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action<PlayableGraph>>(ref Utility.graphCreated, (Action<PlayableGraph>)Delegate.Remove(action2, value), action);
				}
				while (action != action2);
			}
		}

		public static event Action<PlayableGraph> destroyingGraph
		{
			add
			{
				Action<PlayableGraph> action = Utility.destroyingGraph;
				Action<PlayableGraph> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action<PlayableGraph>>(ref Utility.destroyingGraph, (Action<PlayableGraph>)Delegate.Combine(action2, value), action);
				}
				while (action != action2);
			}
			remove
			{
				Action<PlayableGraph> action = Utility.destroyingGraph;
				Action<PlayableGraph> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action<PlayableGraph>>(ref Utility.destroyingGraph, (Action<PlayableGraph>)Delegate.Remove(action2, value), action);
				}
				while (action != action2);
			}
		}

		[RequiredByNativeCode]
		private static void OnPlayableGraphCreated(PlayableGraph graph)
		{
			if (Utility.graphCreated != null)
			{
				Utility.graphCreated(graph);
			}
		}

		[RequiredByNativeCode]
		private static void OnDestroyingPlayableGraph(PlayableGraph graph)
		{
			if (Utility.destroyingGraph != null)
			{
				Utility.destroyingGraph(graph);
			}
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern PlayableGraph[] GetAllGraphs();
	}
}
