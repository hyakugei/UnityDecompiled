using System;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
	[CustomEditor(typeof(LightProbeGroup))]
	internal class LightProbeGroupInspector : Editor
	{
		private static class Styles
		{
			public static readonly GUIContent showWireframe;

			public static readonly GUIContent selectedProbePosition;

			public static readonly GUIContent addProbe;

			public static readonly GUIContent deleteSelected;

			public static readonly GUIContent selectAll;

			public static readonly GUIContent duplicateSelected;

			public static readonly GUIContent editModeButton;

			static Styles()
			{
				LightProbeGroupInspector.Styles.showWireframe = EditorGUIUtility.TrTextContent("Show Wireframe", "Show the tetrahedron wireframe visualizing the blending between probes.", null);
				LightProbeGroupInspector.Styles.selectedProbePosition = EditorGUIUtility.TrTextContent("Selected Probe Position", "The local position of this probe relative to the parent group.", null);
				LightProbeGroupInspector.Styles.addProbe = EditorGUIUtility.TrTextContent("Add Probe", null, null);
				LightProbeGroupInspector.Styles.deleteSelected = EditorGUIUtility.TrTextContent("Delete Selected", null, null);
				LightProbeGroupInspector.Styles.selectAll = EditorGUIUtility.TrTextContent("Select All", null, null);
				LightProbeGroupInspector.Styles.duplicateSelected = EditorGUIUtility.TrTextContent("Duplicate Selected", null, null);
				LightProbeGroupInspector.Styles.editModeButton = EditorGUIUtility.IconContent("EditCollider");
			}
		}

		private LightProbeGroupEditor m_Editor;

		private bool m_EditingProbes;

		private bool m_ShouldFocus;

		public void OnEnable()
		{
			this.m_Editor = new LightProbeGroupEditor(base.target as LightProbeGroup, this);
			this.m_Editor.PullProbePositions();
			this.m_Editor.DeselectProbes();
			this.m_Editor.PushProbePositions();
			SceneView.onSceneGUIDelegate = (SceneView.OnSceneFunc)Delegate.Combine(SceneView.onSceneGUIDelegate, new SceneView.OnSceneFunc(this.OnSceneGUIDelegate));
			Undo.undoRedoPerformed = (Undo.UndoRedoCallback)Delegate.Combine(Undo.undoRedoPerformed, new Undo.UndoRedoCallback(this.UndoRedoPerformed));
			EditMode.editModeStarted += new Action<IToolModeOwner, EditMode.SceneViewEditMode>(this.OnEditModeStarted);
			EditMode.editModeEnded += new Action<IToolModeOwner>(this.OnEditModeEnded);
		}

		private void OnEditModeEnded(IToolModeOwner owner)
		{
			if (owner == this)
			{
				this.EndEditProbes();
			}
		}

		private void OnEditModeStarted(IToolModeOwner owner, EditMode.SceneViewEditMode mode)
		{
			if (owner == this && mode == EditMode.SceneViewEditMode.LightProbeGroup)
			{
				this.StartEditProbes();
			}
		}

		public void StartEditMode()
		{
			EditMode.ChangeEditMode(EditMode.SceneViewEditMode.LightProbeGroup, this.m_Editor.bounds, this);
		}

		private void StartEditProbes()
		{
			if (!this.m_EditingProbes)
			{
				this.m_EditingProbes = true;
				this.m_Editor.SetEditing(true);
				Tools.s_Hidden = true;
				SceneView.RepaintAll();
			}
		}

		private void EndEditProbes()
		{
			if (this.m_EditingProbes)
			{
				this.m_Editor.drawTetrahedra = true;
				this.m_Editor.DeselectProbes();
				this.m_Editor.SetEditing(false);
				this.m_EditingProbes = false;
				Tools.s_Hidden = false;
				SceneView.RepaintAll();
			}
		}

		public void OnDisable()
		{
			this.EndEditProbes();
			Undo.undoRedoPerformed = (Undo.UndoRedoCallback)Delegate.Remove(Undo.undoRedoPerformed, new Undo.UndoRedoCallback(this.UndoRedoPerformed));
			SceneView.onSceneGUIDelegate = (SceneView.OnSceneFunc)Delegate.Remove(SceneView.onSceneGUIDelegate, new SceneView.OnSceneFunc(this.OnSceneGUIDelegate));
			if (base.target != null)
			{
				this.m_Editor.PushProbePositions();
				this.m_Editor = null;
			}
		}

		private void UndoRedoPerformed()
		{
			this.m_Editor.PullProbePositions();
			this.m_Editor.MarkTetrahedraDirty();
		}

		public override void OnInspectorGUI()
		{
			EditorGUI.BeginChangeCheck();
			this.m_Editor.PullProbePositions();
			EditMode.DoEditModeInspectorModeButton(EditMode.SceneViewEditMode.LightProbeGroup, "Edit Light Probes", LightProbeGroupInspector.Styles.editModeButton, this);
			GUILayout.Space(3f);
			EditorGUI.BeginDisabledGroup(EditMode.editMode != EditMode.SceneViewEditMode.LightProbeGroup);
			this.m_Editor.drawTetrahedra = EditorGUILayout.Toggle(LightProbeGroupInspector.Styles.showWireframe, this.m_Editor.drawTetrahedra, new GUILayoutOption[0]);
			EditorGUI.BeginDisabledGroup(this.m_Editor.SelectedCount == 0);
			Vector3 vector = (this.m_Editor.SelectedCount <= 0) ? Vector3.zero : this.m_Editor.GetSelectedPositions()[0];
			Vector3 vector2 = EditorGUILayout.Vector3Field(LightProbeGroupInspector.Styles.selectedProbePosition, vector, new GUILayoutOption[0]);
			if (vector2 != vector)
			{
				Vector3[] selectedPositions = this.m_Editor.GetSelectedPositions();
				Vector3 b = vector2 - vector;
				for (int i = 0; i < selectedPositions.Length; i++)
				{
					this.m_Editor.UpdateSelectedPosition(i, selectedPositions[i] + b);
				}
			}
			EditorGUI.EndDisabledGroup();
			GUILayout.Space(3f);
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.BeginVertical(new GUILayoutOption[0]);
			if (GUILayout.Button(LightProbeGroupInspector.Styles.addProbe, new GUILayoutOption[0]))
			{
				Vector3 position = Vector3.zero;
				if (SceneView.lastActiveSceneView)
				{
					LightProbeGroup lightProbeGroup = base.target as LightProbeGroup;
					if (lightProbeGroup)
					{
						position = lightProbeGroup.transform.InverseTransformPoint(position);
					}
				}
				this.StartEditProbes();
				this.m_Editor.DeselectProbes();
				this.m_Editor.AddProbe(position);
			}
			if (GUILayout.Button(LightProbeGroupInspector.Styles.deleteSelected, new GUILayoutOption[0]))
			{
				this.StartEditProbes();
				this.m_Editor.RemoveSelectedProbes();
			}
			GUILayout.EndVertical();
			GUILayout.BeginVertical(new GUILayoutOption[0]);
			if (GUILayout.Button(LightProbeGroupInspector.Styles.selectAll, new GUILayoutOption[0]))
			{
				this.StartEditProbes();
				this.m_Editor.SelectAllProbes();
			}
			if (GUILayout.Button(LightProbeGroupInspector.Styles.duplicateSelected, new GUILayoutOption[0]))
			{
				this.StartEditProbes();
				this.m_Editor.DuplicateSelectedProbes();
			}
			GUILayout.EndVertical();
			GUILayout.EndHorizontal();
			EditorGUI.EndDisabledGroup();
			this.m_Editor.HandleEditMenuHotKeyCommands();
			this.m_Editor.PushProbePositions();
			if (EditorGUI.EndChangeCheck())
			{
				this.m_Editor.MarkTetrahedraDirty();
				SceneView.RepaintAll();
			}
		}

		internal override Bounds GetWorldBoundsOfTarget(UnityEngine.Object targetObject)
		{
			return this.m_Editor.bounds;
		}

		private void InternalOnSceneView()
		{
			if (EditorGUIUtility.IsGizmosAllowedForObject(base.target))
			{
				if (SceneView.lastActiveSceneView != null)
				{
					if (this.m_ShouldFocus)
					{
						this.m_ShouldFocus = false;
						SceneView.lastActiveSceneView.FrameSelected();
					}
				}
				this.m_Editor.PullProbePositions();
				LightProbeGroup lightProbeGroup = base.target as LightProbeGroup;
				if (lightProbeGroup != null)
				{
					if (this.m_Editor.OnSceneGUI(lightProbeGroup.transform))
					{
						this.StartEditProbes();
					}
					else
					{
						this.EndEditProbes();
					}
				}
				this.m_Editor.PushProbePositions();
			}
		}

		public void OnSceneGUI()
		{
			if (Event.current.type != EventType.Repaint)
			{
				this.InternalOnSceneView();
			}
		}

		public void OnSceneGUIDelegate(SceneView sceneView)
		{
			if (Event.current.type == EventType.Repaint)
			{
				this.InternalOnSceneView();
			}
		}

		public bool HasFrameBounds()
		{
			return this.m_Editor.SelectedCount > 0;
		}

		public Bounds OnGetFrameBounds()
		{
			return this.m_Editor.selectedProbeBounds;
		}
	}
}
