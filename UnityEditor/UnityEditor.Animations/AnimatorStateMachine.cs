using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngineInternal;

namespace UnityEditor.Animations
{
	public sealed class AnimatorStateMachine : UnityEngine.Object
	{
		internal class StateMachineCache
		{
			private static Dictionary<AnimatorStateMachine, ChildAnimatorStateMachine[]> m_ChildStateMachines;

			private static bool m_Initialized;

			private static void Init()
			{
				if (!AnimatorStateMachine.StateMachineCache.m_Initialized)
				{
					AnimatorStateMachine.StateMachineCache.m_ChildStateMachines = new Dictionary<AnimatorStateMachine, ChildAnimatorStateMachine[]>();
					AnimatorStateMachine.StateMachineCache.m_Initialized = true;
				}
			}

			public static void Clear()
			{
				AnimatorStateMachine.StateMachineCache.Init();
				AnimatorStateMachine.StateMachineCache.m_ChildStateMachines.Clear();
			}

			public static ChildAnimatorStateMachine[] GetChildStateMachines(AnimatorStateMachine parent)
			{
				AnimatorStateMachine.StateMachineCache.Init();
				ChildAnimatorStateMachine[] stateMachines;
				if (!AnimatorStateMachine.StateMachineCache.m_ChildStateMachines.TryGetValue(parent, out stateMachines))
				{
					stateMachines = parent.stateMachines;
					AnimatorStateMachine.StateMachineCache.m_ChildStateMachines.Add(parent, stateMachines);
				}
				return stateMachines;
			}
		}

		private PushUndoIfNeeded undoHandler = new PushUndoIfNeeded(true);

		public extern StateMachineBehaviour[] behaviours
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public extern ChildAnimatorState[] states
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public extern ChildAnimatorStateMachine[] stateMachines
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public extern AnimatorState defaultState
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public Vector3 anyStatePosition
		{
			get
			{
				Vector3 result;
				this.get_anyStatePosition_Injected(out result);
				return result;
			}
			set
			{
				this.set_anyStatePosition_Injected(ref value);
			}
		}

		public Vector3 entryPosition
		{
			get
			{
				Vector3 result;
				this.get_entryPosition_Injected(out result);
				return result;
			}
			set
			{
				this.set_entryPosition_Injected(ref value);
			}
		}

		public Vector3 exitPosition
		{
			get
			{
				Vector3 result;
				this.get_exitPosition_Injected(out result);
				return result;
			}
			set
			{
				this.set_exitPosition_Injected(ref value);
			}
		}

		public Vector3 parentStateMachinePosition
		{
			get
			{
				Vector3 result;
				this.get_parentStateMachinePosition_Injected(out result);
				return result;
			}
			set
			{
				this.set_parentStateMachinePosition_Injected(ref value);
			}
		}

		public extern AnimatorStateTransition[] anyStateTransitions
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public extern AnimatorTransition[] entryTransitions
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		internal extern int transitionCount
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		internal bool pushUndo
		{
			set
			{
				this.undoHandler.pushUndo = value;
			}
		}

		internal List<ChildAnimatorState> statesRecursive
		{
			get
			{
				List<ChildAnimatorState> list = new List<ChildAnimatorState>();
				list.AddRange(this.states);
				for (int i = 0; i < this.stateMachines.Length; i++)
				{
					list.AddRange(this.stateMachines[i].stateMachine.statesRecursive);
				}
				return list;
			}
		}

		internal List<ChildAnimatorStateMachine> stateMachinesRecursive
		{
			get
			{
				List<ChildAnimatorStateMachine> list = new List<ChildAnimatorStateMachine>();
				ChildAnimatorStateMachine[] childStateMachines = AnimatorStateMachine.StateMachineCache.GetChildStateMachines(this);
				list.AddRange(childStateMachines);
				for (int i = 0; i < childStateMachines.Length; i++)
				{
					list.AddRange(childStateMachines[i].stateMachine.stateMachinesRecursive);
				}
				return list;
			}
		}

		internal List<AnimatorStateTransition> anyStateTransitionsRecursive
		{
			get
			{
				List<AnimatorStateTransition> list = new List<AnimatorStateTransition>();
				list.AddRange(this.anyStateTransitions);
				ChildAnimatorStateMachine[] stateMachines = this.stateMachines;
				for (int i = 0; i < stateMachines.Length; i++)
				{
					ChildAnimatorStateMachine childAnimatorStateMachine = stateMachines[i];
					list.AddRange(childAnimatorStateMachine.stateMachine.anyStateTransitionsRecursive);
				}
				return list;
			}
		}

		[Obsolete("stateCount is obsolete. Use .states.Length  instead.", true)]
		private int stateCount
		{
			get
			{
				return 0;
			}
		}

		[Obsolete("stateMachineCount is obsolete. Use .stateMachines.Length instead.", true)]
		private int stateMachineCount
		{
			get
			{
				return 0;
			}
		}

		[Obsolete("uniqueNameHash does not exist anymore.", true)]
		private int uniqueNameHash
		{
			get
			{
				return -1;
			}
		}

		public AnimatorStateMachine()
		{
			AnimatorStateMachine.Internal_CreateAnimatorStateMachine(this);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern MonoScript GetBehaviourMonoScript(int index);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_CreateAnimatorStateMachine([Writable] AnimatorStateMachine self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern AnimatorTransition[] GetStateMachineTransitions(AnimatorStateMachine sourceStateMachine);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetStateMachineTransitions(AnimatorStateMachine sourceStateMachine, AnimatorTransition[] transitions);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void AddBehaviour(int instanceID);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void RemoveBehaviour(int index);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern ScriptableObject ScriptingAddStateMachineBehaviourWithType(Type stateMachineBehaviourType);

		[TypeInferenceRule(TypeInferenceRules.TypeReferencedByFirstArgument)]
		public StateMachineBehaviour AddStateMachineBehaviour(Type stateMachineBehaviourType)
		{
			return (StateMachineBehaviour)this.ScriptingAddStateMachineBehaviourWithType(stateMachineBehaviourType);
		}

		public T AddStateMachineBehaviour<T>() where T : StateMachineBehaviour
		{
			return this.AddStateMachineBehaviour(typeof(T)) as T;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern string MakeUniqueStateName(string name);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern string MakeUniqueStateMachineName(string name);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void Clear();

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void RemoveStateInternal(AnimatorState state);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void RemoveStateMachineInternal(AnimatorStateMachine stateMachine);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void MoveState(AnimatorState state, AnimatorStateMachine target);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void MoveStateMachine(AnimatorStateMachine stateMachine, AnimatorStateMachine target);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern bool HasState(AnimatorState state, bool recursive);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern bool HasStateMachine(AnimatorStateMachine state, bool recursive);

		internal Vector3 GetStatePosition(AnimatorState state)
		{
			ChildAnimatorState[] states = this.states;
			Vector3 result;
			for (int i = 0; i < states.Length; i++)
			{
				if (state == states[i].state)
				{
					result = states[i].position;
					return result;
				}
			}
			result = Vector3.zero;
			return result;
		}

		internal void SetStatePosition(AnimatorState state, Vector3 position)
		{
			ChildAnimatorState[] states = this.states;
			for (int i = 0; i < states.Length; i++)
			{
				if (state == states[i].state)
				{
					states[i].position = position;
					this.states = states;
					break;
				}
			}
		}

		internal Vector3 GetStateMachinePosition(AnimatorStateMachine stateMachine)
		{
			ChildAnimatorStateMachine[] stateMachines = this.stateMachines;
			Vector3 result;
			for (int i = 0; i < stateMachines.Length; i++)
			{
				if (stateMachine == stateMachines[i].stateMachine)
				{
					result = stateMachines[i].position;
					return result;
				}
			}
			result = Vector3.zero;
			return result;
		}

		internal void SetStateMachinePosition(AnimatorStateMachine stateMachine, Vector3 position)
		{
			ChildAnimatorStateMachine[] stateMachines = this.stateMachines;
			for (int i = 0; i < stateMachines.Length; i++)
			{
				if (stateMachine == stateMachines[i].stateMachine)
				{
					stateMachines[i].position = position;
					this.stateMachines = stateMachines;
					break;
				}
			}
		}

		public AnimatorState AddState(string name)
		{
			return this.AddState(name, (this.states.Length <= 0) ? new Vector3(200f, 0f, 0f) : (this.states[this.states.Length - 1].position + new Vector3(35f, 65f)));
		}

		public AnimatorState AddState(string name, Vector3 position)
		{
			AnimatorState animatorState = new AnimatorState();
			animatorState.hideFlags = HideFlags.HideInHierarchy;
			animatorState.name = this.MakeUniqueStateName(name);
			if (AssetDatabase.GetAssetPath(this) != "")
			{
				AssetDatabase.AddObjectToAsset(animatorState, AssetDatabase.GetAssetPath(this));
			}
			this.AddState(animatorState, position);
			return animatorState;
		}

		public void AddState(AnimatorState state, Vector3 position)
		{
			ChildAnimatorState[] states = this.states;
			if (Array.Exists<ChildAnimatorState>(states, (ChildAnimatorState childState) => childState.state == state))
			{
				Debug.LogWarning(string.Format("State '{0}' already exists in state machine '{1}', discarding new state.", state.name, base.name));
			}
			else
			{
				this.undoHandler.DoUndo(this, "State added");
				ArrayUtility.Add<ChildAnimatorState>(ref states, new ChildAnimatorState
				{
					state = state,
					position = position
				});
				this.states = states;
			}
		}

		public void RemoveState(AnimatorState state)
		{
			this.undoHandler.DoUndo(this, "State removed");
			this.undoHandler.DoUndo(state, "State removed");
			this.RemoveStateInternal(state);
		}

		public AnimatorStateMachine AddStateMachine(string name)
		{
			return this.AddStateMachine(name, Vector3.zero);
		}

		public AnimatorStateMachine AddStateMachine(string name, Vector3 position)
		{
			AnimatorStateMachine animatorStateMachine = new AnimatorStateMachine();
			animatorStateMachine.hideFlags = HideFlags.HideInHierarchy;
			animatorStateMachine.name = this.MakeUniqueStateMachineName(name);
			this.AddStateMachine(animatorStateMachine, position);
			if (AssetDatabase.GetAssetPath(this) != "")
			{
				AssetDatabase.AddObjectToAsset(animatorStateMachine, AssetDatabase.GetAssetPath(this));
			}
			return animatorStateMachine;
		}

		public void AddStateMachine(AnimatorStateMachine stateMachine, Vector3 position)
		{
			ChildAnimatorStateMachine[] stateMachines = this.stateMachines;
			if (Array.Exists<ChildAnimatorStateMachine>(stateMachines, (ChildAnimatorStateMachine childStateMachine) => childStateMachine.stateMachine == stateMachine))
			{
				Debug.LogWarning(string.Format("Sub state machine '{0}' already exists in state machine '{1}', discarding new state machine.", stateMachine.name, base.name));
			}
			else
			{
				this.undoHandler.DoUndo(this, "StateMachine " + stateMachine.name + " added");
				ArrayUtility.Add<ChildAnimatorStateMachine>(ref stateMachines, new ChildAnimatorStateMachine
				{
					stateMachine = stateMachine,
					position = position
				});
				this.stateMachines = stateMachines;
			}
		}

		public void RemoveStateMachine(AnimatorStateMachine stateMachine)
		{
			this.undoHandler.DoUndo(this, "StateMachine removed");
			this.undoHandler.DoUndo(stateMachine, "StateMachine removed");
			this.RemoveStateMachineInternal(stateMachine);
		}

		private AnimatorStateTransition AddAnyStateTransition()
		{
			this.undoHandler.DoUndo(this, "AnyState Transition Added");
			AnimatorStateTransition[] anyStateTransitions = this.anyStateTransitions;
			AnimatorStateTransition animatorStateTransition = new AnimatorStateTransition();
			animatorStateTransition.hasExitTime = false;
			animatorStateTransition.hasFixedDuration = true;
			animatorStateTransition.duration = 0.25f;
			animatorStateTransition.exitTime = 0.75f;
			if (AssetDatabase.GetAssetPath(this) != "")
			{
				AssetDatabase.AddObjectToAsset(animatorStateTransition, AssetDatabase.GetAssetPath(this));
			}
			animatorStateTransition.hideFlags = HideFlags.HideInHierarchy;
			ArrayUtility.Add<AnimatorStateTransition>(ref anyStateTransitions, animatorStateTransition);
			this.anyStateTransitions = anyStateTransitions;
			return animatorStateTransition;
		}

		public AnimatorStateTransition AddAnyStateTransition(AnimatorState destinationState)
		{
			AnimatorStateTransition animatorStateTransition = this.AddAnyStateTransition();
			animatorStateTransition.destinationState = destinationState;
			return animatorStateTransition;
		}

		public AnimatorStateTransition AddAnyStateTransition(AnimatorStateMachine destinationStateMachine)
		{
			AnimatorStateTransition animatorStateTransition = this.AddAnyStateTransition();
			animatorStateTransition.destinationStateMachine = destinationStateMachine;
			return animatorStateTransition;
		}

		public bool RemoveAnyStateTransition(AnimatorStateTransition transition)
		{
			bool result;
			if (new List<AnimatorStateTransition>(this.anyStateTransitions).Any((AnimatorStateTransition t) => t == transition))
			{
				this.undoHandler.DoUndo(this, "AnyState Transition Removed");
				AnimatorStateTransition[] anyStateTransitions = this.anyStateTransitions;
				ArrayUtility.Remove<AnimatorStateTransition>(ref anyStateTransitions, transition);
				this.anyStateTransitions = anyStateTransitions;
				if (MecanimUtilities.AreSameAsset(this, transition))
				{
					Undo.DestroyObjectImmediate(transition);
				}
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		internal void RemoveAnyStateTransitionRecursive(AnimatorStateTransition transition)
		{
			if (!this.RemoveAnyStateTransition(transition))
			{
				List<ChildAnimatorStateMachine> stateMachinesRecursive = this.stateMachinesRecursive;
				foreach (ChildAnimatorStateMachine current in stateMachinesRecursive)
				{
					if (current.stateMachine.RemoveAnyStateTransition(transition))
					{
						break;
					}
				}
			}
		}

		public AnimatorTransition AddStateMachineTransition(AnimatorStateMachine sourceStateMachine)
		{
			AnimatorStateMachine destinationStateMachine = null;
			return this.AddStateMachineTransition(sourceStateMachine, destinationStateMachine);
		}

		public AnimatorTransition AddStateMachineTransition(AnimatorStateMachine sourceStateMachine, AnimatorStateMachine destinationStateMachine)
		{
			this.undoHandler.DoUndo(this, "StateMachine Transition Added");
			AnimatorTransition[] stateMachineTransitions = this.GetStateMachineTransitions(sourceStateMachine);
			AnimatorTransition animatorTransition = new AnimatorTransition();
			if (destinationStateMachine)
			{
				animatorTransition.destinationStateMachine = destinationStateMachine;
			}
			if (AssetDatabase.GetAssetPath(this) != "")
			{
				AssetDatabase.AddObjectToAsset(animatorTransition, AssetDatabase.GetAssetPath(this));
			}
			animatorTransition.hideFlags = HideFlags.HideInHierarchy;
			ArrayUtility.Add<AnimatorTransition>(ref stateMachineTransitions, animatorTransition);
			this.SetStateMachineTransitions(sourceStateMachine, stateMachineTransitions);
			return animatorTransition;
		}

		public AnimatorTransition AddStateMachineTransition(AnimatorStateMachine sourceStateMachine, AnimatorState destinationState)
		{
			AnimatorTransition animatorTransition = this.AddStateMachineTransition(sourceStateMachine);
			animatorTransition.destinationState = destinationState;
			return animatorTransition;
		}

		public AnimatorTransition AddStateMachineExitTransition(AnimatorStateMachine sourceStateMachine)
		{
			AnimatorTransition animatorTransition = this.AddStateMachineTransition(sourceStateMachine);
			animatorTransition.isExit = true;
			return animatorTransition;
		}

		public bool RemoveStateMachineTransition(AnimatorStateMachine sourceStateMachine, AnimatorTransition transition)
		{
			this.undoHandler.DoUndo(this, "StateMachine Transition Removed");
			AnimatorTransition[] stateMachineTransitions = this.GetStateMachineTransitions(sourceStateMachine);
			int num = stateMachineTransitions.Length;
			ArrayUtility.Remove<AnimatorTransition>(ref stateMachineTransitions, transition);
			this.SetStateMachineTransitions(sourceStateMachine, stateMachineTransitions);
			if (MecanimUtilities.AreSameAsset(this, transition))
			{
				Undo.DestroyObjectImmediate(transition);
			}
			return num != stateMachineTransitions.Length;
		}

		private AnimatorTransition AddEntryTransition()
		{
			this.undoHandler.DoUndo(this, "Entry Transition Added");
			AnimatorTransition[] entryTransitions = this.entryTransitions;
			AnimatorTransition animatorTransition = new AnimatorTransition();
			if (AssetDatabase.GetAssetPath(this) != "")
			{
				AssetDatabase.AddObjectToAsset(animatorTransition, AssetDatabase.GetAssetPath(this));
			}
			animatorTransition.hideFlags = HideFlags.HideInHierarchy;
			ArrayUtility.Add<AnimatorTransition>(ref entryTransitions, animatorTransition);
			this.entryTransitions = entryTransitions;
			return animatorTransition;
		}

		public AnimatorTransition AddEntryTransition(AnimatorState destinationState)
		{
			AnimatorTransition animatorTransition = this.AddEntryTransition();
			animatorTransition.destinationState = destinationState;
			return animatorTransition;
		}

		public AnimatorTransition AddEntryTransition(AnimatorStateMachine destinationStateMachine)
		{
			AnimatorTransition animatorTransition = this.AddEntryTransition();
			animatorTransition.destinationStateMachine = destinationStateMachine;
			return animatorTransition;
		}

		public bool RemoveEntryTransition(AnimatorTransition transition)
		{
			bool result;
			if (new List<AnimatorTransition>(this.entryTransitions).Any((AnimatorTransition t) => t == transition))
			{
				this.undoHandler.DoUndo(this, "Entry Transition Removed");
				AnimatorTransition[] entryTransitions = this.entryTransitions;
				ArrayUtility.Remove<AnimatorTransition>(ref entryTransitions, transition);
				this.entryTransitions = entryTransitions;
				if (MecanimUtilities.AreSameAsset(this, transition))
				{
					Undo.DestroyObjectImmediate(transition);
				}
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		internal ChildAnimatorState FindState(int nameHash)
		{
			return new List<ChildAnimatorState>(this.states).Find((ChildAnimatorState s) => s.state.nameHash == nameHash);
		}

		internal ChildAnimatorState FindState(string name)
		{
			return new List<ChildAnimatorState>(this.states).Find((ChildAnimatorState s) => s.state.name == name);
		}

		internal bool HasState(AnimatorState state)
		{
			return this.statesRecursive.Any((ChildAnimatorState s) => s.state == state);
		}

		internal bool IsDirectParent(AnimatorStateMachine stateMachine)
		{
			return this.stateMachines.Any((ChildAnimatorStateMachine sm) => sm.stateMachine == stateMachine);
		}

		internal bool HasStateMachine(AnimatorStateMachine child)
		{
			return this.stateMachinesRecursive.Any((ChildAnimatorStateMachine sm) => sm.stateMachine == child);
		}

		internal bool HasTransition(AnimatorState stateA, AnimatorState stateB)
		{
			return stateA.transitions.Any((AnimatorStateTransition t) => t.destinationState == stateB) || stateB.transitions.Any((AnimatorStateTransition t) => t.destinationState == stateA);
		}

		internal AnimatorStateMachine FindParent(AnimatorStateMachine stateMachine)
		{
			AnimatorStateMachine result;
			if (this.stateMachines.Any((ChildAnimatorStateMachine childSM) => childSM.stateMachine == stateMachine))
			{
				result = this;
			}
			else
			{
				result = this.stateMachinesRecursive.Find((ChildAnimatorStateMachine sm) => sm.stateMachine.stateMachines.Any((ChildAnimatorStateMachine childSM) => childSM.stateMachine == stateMachine)).stateMachine;
			}
			return result;
		}

		internal AnimatorStateMachine FindStateMachine(string path)
		{
			AnimatorStateMachine.<FindStateMachine>c__AnonStoreyB <FindStateMachine>c__AnonStoreyB = new AnimatorStateMachine.<FindStateMachine>c__AnonStoreyB();
			<FindStateMachine>c__AnonStoreyB.smNames = path.Split(new char[]
			{
				'.'
			});
			AnimatorStateMachine animatorStateMachine = this;
			int i = 1;
			while (i < <FindStateMachine>c__AnonStoreyB.smNames.Length - 1 && animatorStateMachine != null)
			{
				ChildAnimatorStateMachine[] childStateMachines = AnimatorStateMachine.StateMachineCache.GetChildStateMachines(animatorStateMachine);
				int num = Array.FindIndex<ChildAnimatorStateMachine>(childStateMachines, (ChildAnimatorStateMachine t) => t.stateMachine.name == <FindStateMachine>c__AnonStoreyB.smNames[i]);
				animatorStateMachine = ((num < 0) ? null : childStateMachines[num].stateMachine);
				i++;
			}
			return (!(animatorStateMachine == null)) ? animatorStateMachine : this;
		}

		internal AnimatorStateMachine FindStateMachine(AnimatorState state)
		{
			AnimatorStateMachine result;
			if (this.HasState(state, false))
			{
				result = this;
			}
			else
			{
				List<ChildAnimatorStateMachine> stateMachinesRecursive = this.stateMachinesRecursive;
				int num = stateMachinesRecursive.FindIndex((ChildAnimatorStateMachine sm) => sm.stateMachine.HasState(state, false));
				result = ((num < 0) ? null : stateMachinesRecursive[num].stateMachine);
			}
			return result;
		}

		internal AnimatorStateTransition FindTransition(AnimatorState destinationState)
		{
			return new List<AnimatorStateTransition>(this.anyStateTransitions).Find((AnimatorStateTransition t) => t.destinationState == destinationState);
		}

		[Obsolete("GetTransitionsFromState is obsolete. Use AnimatorState.transitions instead.", true)]
		private AnimatorState GetTransitionsFromState(AnimatorState state)
		{
			return null;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_anyStatePosition_Injected(out Vector3 ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_anyStatePosition_Injected(ref Vector3 value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_entryPosition_Injected(out Vector3 ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_entryPosition_Injected(ref Vector3 value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_exitPosition_Injected(out Vector3 ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_exitPosition_Injected(ref Vector3 value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_parentStateMachinePosition_Injected(out Vector3 ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_parentStateMachinePosition_Injected(ref Vector3 value);
	}
}
