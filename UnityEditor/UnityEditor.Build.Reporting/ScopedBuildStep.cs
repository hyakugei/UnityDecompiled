using System;

namespace UnityEditor.Build.Reporting
{
	internal struct ScopedBuildStep : IDisposable
	{
		private readonly BuildReport m_Report;

		private readonly int m_Step;

		public ScopedBuildStep(BuildReport report, string stepName)
		{
			if (report == null)
			{
				throw new ArgumentNullException("report");
			}
			this.m_Report = report;
			this.m_Step = report.BeginBuildStep(stepName);
		}

		public void Resume()
		{
			this.m_Report.ResumeBuildStep(this.m_Step);
		}

		public void Dispose()
		{
			this.m_Report.EndBuildStep(this.m_Step);
		}
	}
}
