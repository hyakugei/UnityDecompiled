using System;
using UnityEditorInternal.Profiling;

namespace UnityEditorInternal
{
	internal interface IProfilerWindowController
	{
		void SetSelectedPropertyPath(string path);

		void ClearSelectedPropertyPath();

		void SetClearOnPlay(bool enabled);

		bool GetClearOnPlay();

		ProfilerProperty GetRootProfilerProperty(ProfilerColumn sortType);

		FrameDataView GetFrameDataView(ProfilerViewType viewType, ProfilerColumn profilerSortColumn, bool sortAscending);

		int GetActiveVisibleFrameIndex();

		bool IsRecording();

		void Repaint();
	}
}
