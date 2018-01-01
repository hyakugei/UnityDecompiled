using System;
using UnityEngine.Experimental.UIElements;

namespace UnityEditor.Experimental.UIElements
{
	internal class UXMLEditorFactories
	{
		private static bool s_Registered;

		internal static void RegisterAll()
		{
			if (!UXMLEditorFactories.s_Registered)
			{
				UXMLEditorFactories.s_Registered = true;
				Factories.RegisterFactory<DoubleField>((IUxmlAttributes bag, CreationContext __) => new DoubleField());
				Factories.RegisterFactory<IntegerField>((IUxmlAttributes bag, CreationContext __) => new IntegerField());
				Factories.RegisterFactory<CurveField>((IUxmlAttributes bag, CreationContext __) => new CurveField());
				Factories.RegisterFactory<ObjectField>((IUxmlAttributes bag, CreationContext __) => new ObjectField());
				Factories.RegisterFactory<ColorField>((IUxmlAttributes bag, CreationContext __) => new ColorField());
				Factories.RegisterFactory<RectField>((IUxmlAttributes bag, CreationContext __) => new RectField());
				Factories.RegisterFactory<Vector2Field>((IUxmlAttributes bag, CreationContext __) => new Vector2Field());
				Factories.RegisterFactory<Vector3Field>((IUxmlAttributes bag, CreationContext __) => new Vector3Field());
				Factories.RegisterFactory<Vector4Field>((IUxmlAttributes bag, CreationContext __) => new Vector4Field());
				Factories.RegisterFactory<BoundsField>((IUxmlAttributes bag, CreationContext __) => new BoundsField());
			}
		}
	}
}
