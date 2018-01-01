using System;
using UnityEngine.Scripting;

namespace UnityEditor.Collaboration
{
	[UsedByNativeCode]
	internal struct ChangeAction
	{
		private string m_Path;

		private string m_Action;

		public string path
		{
			get
			{
				return this.m_Path;
			}
		}

		public string action
		{
			get
			{
				return this.m_Action;
			}
		}
	}
}
