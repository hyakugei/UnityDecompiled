using System;
using UnityEngine;

namespace UnityEditor.U2D.Interface
{
	internal class UndoSystem : IUndoSystem
	{
		public void RegisterUndoCallback(Undo.UndoRedoCallback undoCallback)
		{
			Undo.undoRedoPerformed = (Undo.UndoRedoCallback)Delegate.Combine(Undo.undoRedoPerformed, undoCallback);
		}

		public void UnregisterUndoCallback(Undo.UndoRedoCallback undoCallback)
		{
			Undo.undoRedoPerformed = (Undo.UndoRedoCallback)Delegate.Remove(Undo.undoRedoPerformed, undoCallback);
		}

		public void RegisterCompleteObjectUndo(ScriptableObject so, string undoText)
		{
			if (so != null)
			{
				Undo.RegisterCompleteObjectUndo(so, undoText);
			}
		}

		public void ClearUndo(ScriptableObject so)
		{
			if (so != null)
			{
				Undo.ClearUndo(so);
			}
		}
	}
}
