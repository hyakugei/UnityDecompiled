using System;

namespace UnityEngine.Bindings
{
	[AttributeUsage(AttributeTargets.Class)]
	internal class NativeAsStructAttribute : Attribute, IBindingsAttribute
	{
		public string StructName
		{
			get;
			set;
		}

		public NativeAsStructAttribute(string structName)
		{
			this.StructName = structName;
		}
	}
}
