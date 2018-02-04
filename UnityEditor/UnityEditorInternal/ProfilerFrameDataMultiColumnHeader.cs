using System;
using UnityEditor.IMGUI.Controls;
using UnityEditorInternal.Profiling;
using UnityEngine;

namespace UnityEditorInternal
{
	public class ProfilerFrameDataMultiColumnHeader : MultiColumnHeader
	{
		public struct Column
		{
			public ProfilerColumn profilerColumn;

			public GUIContent headerLabel;
		}

		private ProfilerFrameDataMultiColumnHeader.Column[] m_Columns;

		public ProfilerFrameDataMultiColumnHeader.Column[] columns
		{
			get
			{
				return this.m_Columns;
			}
		}

		public ProfilerColumn sortedProfilerColumn
		{
			get
			{
				return this.GetProfilerColumn(base.sortedColumnIndex);
			}
		}

		public bool sortedProfilerColumnAscending
		{
			get
			{
				return base.IsSortedAscending(base.sortedColumnIndex);
			}
		}

		public ProfilerFrameDataMultiColumnHeader(MultiColumnHeaderState state, ProfilerFrameDataMultiColumnHeader.Column[] columns) : base(state)
		{
			this.m_Columns = columns;
		}

		public int GetMultiColumnHeaderIndex(ProfilerColumn profilerColumn)
		{
			int result;
			for (int i = 0; i < this.m_Columns.Length; i++)
			{
				if (this.m_Columns[i].profilerColumn == profilerColumn)
				{
					result = i;
					return result;
				}
			}
			result = 0;
			return result;
		}

		public static int GetMultiColumnHeaderIndex(ProfilerFrameDataMultiColumnHeader.Column[] columns, ProfilerColumn profilerColumn)
		{
			int result;
			for (int i = 0; i < columns.Length; i++)
			{
				if (columns[i].profilerColumn == profilerColumn)
				{
					result = i;
					return result;
				}
			}
			result = 0;
			return result;
		}

		public ProfilerColumn GetProfilerColumn(int multiColumnHeaderIndex)
		{
			return this.m_Columns[multiColumnHeaderIndex].profilerColumn;
		}
	}
}
