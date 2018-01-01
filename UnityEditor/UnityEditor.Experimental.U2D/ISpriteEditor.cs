using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor.Experimental.U2D
{
	public interface ISpriteEditor
	{
		List<SpriteRect> spriteRects
		{
			set;
		}

		SpriteRect selectedSpriteRect
		{
			get;
			set;
		}

		bool enableMouseMoveEvent
		{
			set;
		}

		bool editingDisabled
		{
			get;
		}

		Rect windowDimension
		{
			get;
		}

		T GetDataProvider<T>() where T : class;

		bool HandleSpriteSelection();

		void RequestRepaint();

		void SetDataModified();

		void ApplyOrRevertModification(bool apply);
	}
}
