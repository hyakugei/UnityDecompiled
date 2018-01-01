using System;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Bindings;

namespace UnityEditor.Build.Reporting
{
	[NativeType(Header = "Modules/BuildReportingEditor/Public/BuildReport.h"), NativeClass("BuildReporting::BuildReport")]
	public sealed class BuildReport : UnityEngine.Object
	{
		public extern BuildFile[] files
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public extern BuildStep[] steps
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public BuildSummary summary
		{
			get
			{
				BuildSummary result;
				this.get_summary_Injected(out result);
				return result;
			}
		}

		public StrippingInfo strippingInfo
		{
			get
			{
				return this.GetAppendices<StrippingInfo>().SingleOrDefault<StrippingInfo>();
			}
		}

		private BuildReport()
		{
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void RecordFilesMoved(string originalPathPrefix, string newPathPrefix);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void RecordFileAdded(string path, string role);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void RecordFilesAddedRecursive(string rootDir, string role);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void RecordFileDeleted(string path);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void RecordFilesDeletedRecursive(string rootDir);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern string SummarizeErrors();

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void AddMessage(LogType messageType, string message);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern int BeginBuildStep(string stepName);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void ResumeBuildStep(int depth);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void EndBuildStep(int depth);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void AddAppendix([NotNull] UnityEngine.Object obj);

		internal TAppendix[] GetAppendices<TAppendix>() where TAppendix : UnityEngine.Object
		{
			return this.GetAppendices(typeof(TAppendix)).Cast<TAppendix>().ToArray<TAppendix>();
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern UnityEngine.Object[] GetAppendices([NotNull] Type type);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern UnityEngine.Object[] GetAllAppendices();

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern BuildReport GetLatestReport();

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_summary_Injected(out BuildSummary ret);
	}
}
