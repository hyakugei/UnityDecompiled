using System;

namespace UnityEngine.Experimental.UIElements
{
	internal interface IDataWatchService
	{
		IDataWatchHandle AddWatch(UnityEngine.Object watched, Action<UnityEngine.Object> onDataChanged);

		void RemoveWatch(IDataWatchHandle handle);

		void ForceDirtyNextPoll(UnityEngine.Object obj);
	}
}
