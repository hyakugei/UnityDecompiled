using System;
using UnityEngine.Bindings;

namespace UnityEngine.Scripting
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false), VisibleToOtherModules]
	internal class GenerateManagedProxyAttribute : Attribute
	{
		public string NativeType
		{
			get;
			set;
		}

		public GenerateManagedProxyAttribute()
		{
		}

		public GenerateManagedProxyAttribute(string nativeType)
		{
			this.NativeType = nativeType;
		}
	}
}
