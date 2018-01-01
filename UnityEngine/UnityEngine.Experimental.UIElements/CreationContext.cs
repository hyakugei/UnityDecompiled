using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace UnityEngine.Experimental.UIElements
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct CreationContext
	{
		public static readonly CreationContext Default = default(CreationContext);

		public VisualElement target
		{
			[CompilerGenerated]
			get
			{
				return this.<target>k__BackingField;
			}
		}

		public VisualTreeAsset visualTreeAsset
		{
			[CompilerGenerated]
			get
			{
				return this.<visualTreeAsset>k__BackingField;
			}
		}

		public Dictionary<string, VisualElement> slotInsertionPoints
		{
			[CompilerGenerated]
			get
			{
				return this.<slotInsertionPoints>k__BackingField;
			}
		}

		internal CreationContext(Dictionary<string, VisualElement> slotInsertionPoints, VisualTreeAsset vta, VisualElement target)
		{
			this.<target>k__BackingField = target;
			this.<slotInsertionPoints>k__BackingField = slotInsertionPoints;
			this.<visualTreeAsset>k__BackingField = vta;
		}
	}
}
