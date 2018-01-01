using System;
using System.Linq;

namespace UnityEditor.Experimental.U2D
{
	[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
	public class RequireSpriteDataProviderAttribute : Attribute
	{
		private Type[] m_Types;

		public RequireSpriteDataProviderAttribute(params Type[] types)
		{
			this.m_Types = types;
		}

		internal bool ContainsAllType(ISpriteEditorDataProvider provider)
		{
			return provider != null && (from x in this.m_Types
			where provider.HasDataProvider(x)
			select x).Count<Type>() == this.m_Types.Length;
		}
	}
}
