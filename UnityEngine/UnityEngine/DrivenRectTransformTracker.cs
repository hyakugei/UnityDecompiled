using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;
using UnityEngine.Scripting;

namespace UnityEngine
{
	public struct DrivenRectTransformTracker
	{
		private Object m_Driver;

		private List<RectTransform> m_Tracked;

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool CanRecordModifications();

		public void Add(Object driver, RectTransform rectTransform, DrivenTransformProperties drivenProperties)
		{
			if (this.m_Tracked == null)
			{
				this.m_Tracked = new List<RectTransform>();
			}
			Debug.AssertFormat(this.m_Driver == driver || this.m_Driver == null, "DrivenRectTransformTracker only supports a single driver.", new object[0]);
			rectTransform.AddDrivenProperties(driver, drivenProperties);
			this.m_Driver = driver;
			this.m_Tracked.Add(rectTransform);
		}

		[ExcludeFromDocs]
		public void Clear()
		{
			bool revertValues = true;
			this.Clear(revertValues);
		}

		public void Clear([DefaultValue("true")] bool revertValues)
		{
			if (this.m_Tracked != null)
			{
				for (int i = 0; i < this.m_Tracked.Count; i++)
				{
					if (this.m_Tracked[i] != null)
					{
						this.m_Tracked[i].ClearDrivenProperties(this.m_Driver, revertValues);
					}
				}
				this.m_Driver = null;
				this.m_Tracked.Clear();
			}
		}
	}
}
