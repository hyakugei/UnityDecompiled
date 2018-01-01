using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityEditorInternal.Profiling
{
	internal class FrameDataView : IDisposable
	{
		public struct MarkerPath
		{
			public readonly List<int> markerIds;

			public MarkerPath(List<int> markerIds)
			{
				this.markerIds = markerIds;
			}

			public override bool Equals(object obj)
			{
				FrameDataView.MarkerPath markerPath = (FrameDataView.MarkerPath)obj;
				bool result;
				if (this.markerIds == markerPath.markerIds)
				{
					result = true;
				}
				else if (this.markerIds == null || markerPath.markerIds == null)
				{
					result = false;
				}
				else if (this.markerIds.Count != markerPath.markerIds.Count)
				{
					result = false;
				}
				else
				{
					int count = this.markerIds.Count;
					for (int i = 0; i < count; i++)
					{
						if (this.markerIds[i] != markerPath.markerIds[i])
						{
							result = false;
							return result;
						}
					}
					result = true;
				}
				return result;
			}

			public override int GetHashCode()
			{
				int result;
				if (this.markerIds == null)
				{
					result = 0;
				}
				else
				{
					int num = 0;
					for (int i = 0; i < this.markerIds.Count; i++)
					{
						num ^= this.markerIds[i].GetHashCode();
					}
					result = num;
				}
				return result;
			}
		}

		private IntPtr m_Ptr;

		public extern bool frameDataReady
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public extern string frameFPS
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public extern string frameTime
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public extern string frameGpuTime
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public extern int frameIndex
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public extern int threadIndex
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public extern ProfilerColumn sortColumn
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public extern bool sortColumnAscending
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public extern ProfilerViewType viewType
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public FrameDataView(ProfilerViewType viewType, int frameIndex, int threadIndex, ProfilerColumn profilerSortColumn, bool sortAscending)
		{
			this.m_Ptr = FrameDataView.Internal_Create(viewType, frameIndex, threadIndex, profilerSortColumn, sortAscending);
		}

		~FrameDataView()
		{
			this.DisposeInternal();
		}

		public void Dispose()
		{
			this.DisposeInternal();
			GC.SuppressFinalize(this);
		}

		private void DisposeInternal()
		{
			if (this.m_Ptr != IntPtr.Zero)
			{
				FrameDataView.Internal_Destroy(this.m_Ptr);
				this.m_Ptr = IntPtr.Zero;
			}
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr Internal_Create(ProfilerViewType viewType, int frameIndex, int threadIndex, ProfilerColumn profilerSortColumn, bool sortAscending);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_Destroy(IntPtr ptr);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern int GetRootItemID();

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern int GetItemMarkerID(int id);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern int GetItemDepth(int id);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern string GetItemFunctionName(int id);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern string GetItemColumnData(int id, ProfilerColumn column);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern float GetItemColumnDataAsSingle(int id, ProfilerColumn column);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern string GetItemTooltip(int id, ProfilerColumn column);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern int GetItemInstanceID(int id);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern int GetItemSamplesCount(int id);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern string[] GetItemColumnDatas(int id, ProfilerColumn column);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern int[] GetItemInstanceIDs(int id);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool HasItemChildren(int id);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern int[] GetItemChildren(int id);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern int[] GetItemAncestors(int id);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern int[] GetItemDescendantsThatHaveChildren(int id);

		public string ResolveItemCallstack(int id)
		{
			return this.ResolveItemCallstack(id, 0);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern string ResolveItemCallstack(int id, int sampleIndex);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void Sort(ProfilerColumn profilerSortColumn, bool sortAscending);

		public FrameDataView.MarkerPath GetItemMarkerIDPath(int id)
		{
			int[] itemAncestors = this.GetItemAncestors(id);
			List<int> list = new List<int>(1 + itemAncestors.Length);
			for (int i = itemAncestors.Length - 1; i >= 0; i--)
			{
				list.Add(this.GetItemMarkerID(itemAncestors[i]));
			}
			list.Add(this.GetItemMarkerID(id));
			return new FrameDataView.MarkerPath(list);
		}

		public static Color32 GetMarkerCategoryColor(int category)
		{
			Color32 result;
			FrameDataView.GetMarkerCategoryColor_Injected(category, out result);
			return result;
		}

		public bool IsValid()
		{
			return !(this.m_Ptr == IntPtr.Zero) && this.GetRootItemID() != -1;
		}

		public override bool Equals(object obj)
		{
			bool result;
			if (this.m_Ptr == IntPtr.Zero)
			{
				result = false;
			}
			else
			{
				FrameDataView frameDataView = obj as FrameDataView;
				result = (frameDataView != null && (this.frameIndex.Equals(frameDataView.frameIndex) && this.threadIndex.Equals(frameDataView.threadIndex)) && this.viewType.Equals(frameDataView.viewType));
			}
			return result;
		}

		public override int GetHashCode()
		{
			return this.frameIndex.GetHashCode() ^ this.threadIndex.GetHashCode() ^ this.viewType.GetHashCode();
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetMarkerCategoryColor_Injected(int category, out Color32 ret);
	}
}
