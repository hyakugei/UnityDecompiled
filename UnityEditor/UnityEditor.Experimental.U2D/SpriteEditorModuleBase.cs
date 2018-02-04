using System;
using UnityEngine;

namespace UnityEditor.Experimental.U2D
{
	public abstract class SpriteEditorModuleBase
	{
		public ISpriteEditor spriteEditor
		{
			get;
			internal set;
		}

		public abstract string moduleName
		{
			get;
		}

		public abstract bool CanBeActivated();

		public abstract void DoMainGUI();

		public abstract void DoToolbarGUI(Rect drawArea);

		public abstract void OnModuleActivate();

		public abstract void OnModuleDeactivate();

		public abstract void DoPostGUI();

		public abstract bool ApplyRevert(bool apply);
	}
}
