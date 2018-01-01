using System;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Profiling;

namespace UnityEditor
{
	[EditorWindowTitle(title = "Animation", useTypeNameAsIconName = true)]
	internal class AnimationWindow : EditorWindow, IHasCustomMenu
	{
		private static List<AnimationWindow> s_AnimationWindows = new List<AnimationWindow>();

		[SerializeField]
		private AnimEditor m_AnimEditor;

		[SerializeField]
		private EditorGUIUtility.EditorLockTracker m_LockTracker = new EditorGUIUtility.EditorLockTracker();

		[SerializeField]
		private int m_LastSelectedObjectID;

		private GUIStyle m_LockButtonStyle;

		private GUIContent m_DefaultTitleContent;

		private GUIContent m_RecordTitleContent;

		internal AnimationWindowState state
		{
			get
			{
				AnimationWindowState result;
				if (this.m_AnimEditor != null)
				{
					result = this.m_AnimEditor.state;
				}
				else
				{
					result = null;
				}
				return result;
			}
		}

		public static List<AnimationWindow> GetAllAnimationWindows()
		{
			return AnimationWindow.s_AnimationWindows;
		}

		public void ForceRefresh()
		{
			if (this.m_AnimEditor != null)
			{
				this.m_AnimEditor.state.ForceRefresh();
			}
		}

		public void OnEnable()
		{
			if (this.m_AnimEditor == null)
			{
				this.m_AnimEditor = (ScriptableObject.CreateInstance(typeof(AnimEditor)) as AnimEditor);
				this.m_AnimEditor.hideFlags = HideFlags.HideAndDontSave;
			}
			AnimationWindow.s_AnimationWindows.Add(this);
			base.titleContent = base.GetLocalizedTitleContent();
			this.m_DefaultTitleContent = base.titleContent;
			this.m_RecordTitleContent = EditorGUIUtility.TextContentWithIcon(base.titleContent.text, "Animation.Record");
			this.OnSelectionChange();
			Undo.undoRedoPerformed = (Undo.UndoRedoCallback)Delegate.Combine(Undo.undoRedoPerformed, new Undo.UndoRedoCallback(this.UndoRedoPerformed));
		}

		public void OnDisable()
		{
			AnimationWindow.s_AnimationWindows.Remove(this);
			this.m_AnimEditor.OnDisable();
			Undo.undoRedoPerformed = (Undo.UndoRedoCallback)Delegate.Remove(Undo.undoRedoPerformed, new Undo.UndoRedoCallback(this.UndoRedoPerformed));
		}

		public void OnDestroy()
		{
			UnityEngine.Object.DestroyImmediate(this.m_AnimEditor);
		}

		public void Update()
		{
			this.m_AnimEditor.Update();
		}

		public void OnGUI()
		{
			Profiler.BeginSample("AnimationWindow.OnGUI");
			base.titleContent = ((!this.m_AnimEditor.state.recording) ? this.m_DefaultTitleContent : this.m_RecordTitleContent);
			this.m_AnimEditor.OnAnimEditorGUI(this, base.position);
			Profiler.EndSample();
		}

		public void OnSelectionChange()
		{
			if (!(this.m_AnimEditor == null))
			{
				UnityEngine.Object @object = Selection.activeObject;
				bool flag = false;
				if (this.m_LockTracker.isLocked && this.m_AnimEditor.stateDisabled)
				{
					@object = EditorUtility.InstanceIDToObject(this.m_LastSelectedObjectID);
					flag = true;
					this.m_LockTracker.isLocked = false;
				}
				GameObject gameObject = @object as GameObject;
				if (gameObject != null)
				{
					this.EditGameObject(gameObject);
				}
				else
				{
					AnimationClip animationClip = @object as AnimationClip;
					if (animationClip != null)
					{
						this.EditAnimationClip(animationClip);
					}
				}
				if (flag && !this.m_AnimEditor.stateDisabled)
				{
					this.m_LockTracker.isLocked = true;
				}
			}
		}

		public void OnFocus()
		{
			this.OnSelectionChange();
		}

		public void OnControllerChange()
		{
			this.OnSelectionChange();
		}

		public void OnLostFocus()
		{
			if (this.m_AnimEditor != null)
			{
				this.m_AnimEditor.OnLostFocus();
			}
		}

		public bool EditGameObject(GameObject gameObject)
		{
			return !this.state.linkedWithSequencer && this.EditGameObjectInternal(gameObject, null);
		}

		public bool EditAnimationClip(AnimationClip animationClip)
		{
			return !this.state.linkedWithSequencer && this.EditAnimationClipInternal(animationClip, null, null);
		}

		public bool EditSequencerClip(AnimationClip animationClip, UnityEngine.Object sourceObject, IAnimationWindowControl controlInterface)
		{
			bool result;
			if (this.EditAnimationClipInternal(animationClip, sourceObject, controlInterface))
			{
				this.state.linkedWithSequencer = true;
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		public void UnlinkSequencer()
		{
			if (this.state.linkedWithSequencer)
			{
				this.state.linkedWithSequencer = false;
				this.EditAnimationClip(null);
				this.OnSelectionChange();
			}
		}

		private bool EditGameObjectInternal(GameObject gameObject, IAnimationWindowControl controlInterface)
		{
			bool result;
			if (EditorUtility.IsPersistent(gameObject))
			{
				result = false;
			}
			else if ((gameObject.hideFlags & HideFlags.NotEditable) != HideFlags.None)
			{
				result = false;
			}
			else
			{
				GameObjectSelectionItem gameObjectSelectionItem = GameObjectSelectionItem.Create(gameObject);
				if (this.ShouldUpdateGameObjectSelection(gameObjectSelectionItem))
				{
					this.m_AnimEditor.selection = gameObjectSelectionItem;
					this.m_AnimEditor.overrideControlInterface = controlInterface;
					this.m_LastSelectedObjectID = ((!(gameObject != null)) ? 0 : gameObject.GetInstanceID());
					result = true;
				}
				else
				{
					result = false;
				}
			}
			return result;
		}

		private bool EditAnimationClipInternal(AnimationClip animationClip, UnityEngine.Object sourceObject, IAnimationWindowControl controlInterface)
		{
			AnimationClipSelectionItem animationClipSelectionItem = AnimationClipSelectionItem.Create(animationClip, sourceObject);
			bool result;
			if (this.ShouldUpdateSelection(animationClipSelectionItem))
			{
				this.m_AnimEditor.selection = animationClipSelectionItem;
				this.m_AnimEditor.overrideControlInterface = controlInterface;
				this.m_LastSelectedObjectID = ((!(animationClip != null)) ? 0 : animationClip.GetInstanceID());
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		protected virtual void ShowButton(Rect r)
		{
			if (this.m_LockButtonStyle == null)
			{
				this.m_LockButtonStyle = "IN LockButton";
			}
			EditorGUI.BeginChangeCheck();
			this.m_LockTracker.ShowButton(r, this.m_LockButtonStyle, this.m_AnimEditor.stateDisabled);
			if (EditorGUI.EndChangeCheck())
			{
				this.OnSelectionChange();
			}
		}

		private bool ShouldUpdateGameObjectSelection(GameObjectSelectionItem selectedItem)
		{
			bool result;
			if (this.m_LockTracker.isLocked)
			{
				result = false;
			}
			else if (selectedItem.rootGameObject == null)
			{
				result = true;
			}
			else
			{
				AnimationWindowSelectionItem currentSelection = this.m_AnimEditor.selection;
				if (selectedItem.rootGameObject != currentSelection.rootGameObject)
				{
					result = true;
				}
				else if (currentSelection.animationClip == null)
				{
					result = true;
				}
				else
				{
					if (currentSelection.rootGameObject != null)
					{
						AnimationClip[] animationClips = AnimationUtility.GetAnimationClips(currentSelection.rootGameObject);
						if (!Array.Exists<AnimationClip>(animationClips, (AnimationClip x) => x == currentSelection.animationClip))
						{
							result = true;
							return result;
						}
					}
					result = false;
				}
			}
			return result;
		}

		private bool ShouldUpdateSelection(AnimationWindowSelectionItem selectedItem)
		{
			bool result;
			if (this.m_LockTracker.isLocked)
			{
				result = false;
			}
			else
			{
				AnimationWindowSelectionItem selection = this.m_AnimEditor.selection;
				result = (selectedItem.GetRefreshHash() != selection.GetRefreshHash());
			}
			return result;
		}

		private void UndoRedoPerformed()
		{
			base.Repaint();
		}

		public virtual void AddItemsToMenu(GenericMenu menu)
		{
			this.m_LockTracker.AddItemsToMenu(menu, this.m_AnimEditor.stateDisabled);
		}
	}
}
