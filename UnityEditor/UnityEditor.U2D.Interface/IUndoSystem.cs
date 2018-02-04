using System;
using UnityEngine;

namespace UnityEditor.U2D.Interface
{
	internal interface IUndoSystem
	{
		void RegisterUndoCallback(Undo.UndoRedoCallback undoCallback);

		void UnregisterUndoCallback(Undo.UndoRedoCallback undoCallback);

		void RegisterCompleteObjectUndo(ScriptableObject obj, string undoText);

		void ClearUndo(ScriptableObject obj);
	}
}
