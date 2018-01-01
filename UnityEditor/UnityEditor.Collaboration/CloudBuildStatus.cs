using System;
using UnityEngine.Scripting;

namespace UnityEditor.Collaboration
{
	[UsedByNativeCode]
	internal struct CloudBuildStatus
	{
		private string m_Platform;

		private bool m_Complete;

		private bool m_Successful;

		public string platform
		{
			get
			{
				return this.m_Platform;
			}
		}

		public bool complete
		{
			get
			{
				return this.m_Complete;
			}
		}

		public bool success
		{
			get
			{
				return this.m_Successful;
			}
		}
	}
}
