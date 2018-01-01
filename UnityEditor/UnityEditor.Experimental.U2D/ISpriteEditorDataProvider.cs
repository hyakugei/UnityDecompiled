using System;
using UnityEngine;

namespace UnityEditor.Experimental.U2D
{
	public interface ISpriteEditorDataProvider
	{
		SpriteImportMode spriteImportMode
		{
			get;
		}

		float pixelsPerUnit
		{
			get;
		}

		UnityEngine.Object targetObject
		{
			get;
		}

		SpriteRect[] GetSpriteRects();

		void SetSpriteRects(SpriteRect[] spriteRects);

		void Apply();

		void InitSpriteEditorDataProvider();

		T GetDataProvider<T>() where T : class;

		bool HasDataProvider(Type type);
	}
}
