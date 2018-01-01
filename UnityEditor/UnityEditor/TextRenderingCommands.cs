using System;
using UnityEngine;

namespace UnityEditor
{
	internal static class TextRenderingCommands
	{
		[MenuItem("GameObject/3D Object/3D Text", priority = 4000)]
		private static void Create3DText(MenuCommand command)
		{
			GameObject parent = command.context as GameObject;
			GameObject gameObject = GOCreationCommands.CreateGameObject(parent, "New Text", new Type[]
			{
				typeof(MeshRenderer),
				typeof(TextMesh)
			});
			Font font = (Selection.activeObject as Font) ?? Font.GetDefault();
			TextMesh component = gameObject.GetComponent<TextMesh>();
			component.text = "Hello World";
			component.font = font;
			gameObject.GetComponent<MeshRenderer>().material = font.material;
			GOCreationCommands.Place(gameObject, parent);
		}
	}
}
