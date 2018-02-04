using System;

namespace UnityEngine.Bindings
{
	[AttributeUsage(AttributeTargets.Field), VisibleToOtherModules]
	internal class IgnoreAttribute : Attribute, IBindingsAttribute
	{
	}
}
