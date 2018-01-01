using System;
using UnityEngine;
using UnityEngine.Animations;

namespace UnityEditor
{
	internal class MoveTool : ManipulationTool
	{
		private static MoveTool s_Instance;

		public static void OnGUI(SceneView view)
		{
			if (MoveTool.s_Instance == null)
			{
				MoveTool.s_Instance = new MoveTool();
			}
			MoveTool.s_Instance.OnToolGUI(view);
		}

		public override void ToolGUI(SceneView view, Vector3 handlePosition, bool isStatic)
		{
			TransformManipulator.BeginManipulationHandling(false);
			EditorGUI.BeginChangeCheck();
			Quaternion rotation = Tools.handleRotation;
			AimConstraint component = Selection.activeTransform.GetComponent<AimConstraint>();
			if (component != null && component.constraintActive)
			{
				rotation = TransformManipulator.mouseDownHandleRotation;
			}
			Vector3 vector = Handles.PositionHandle(handlePosition, rotation);
			if (EditorGUI.EndChangeCheck() && !isStatic && TransformManipulator.HandleHasMoved(vector))
			{
				ManipulationToolUtility.SetMinDragDifferenceForPos(handlePosition);
				if (Tools.vertexDragging)
				{
					ManipulationToolUtility.DisableMinDragDifference();
				}
				TransformManipulator.SetPositionDelta(vector, TransformManipulator.mouseDownHandlePosition);
			}
			TransformManipulator.EndManipulationHandling();
		}
	}
}
