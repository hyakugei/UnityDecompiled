using System;

namespace UnityEditor.Collaboration
{
	internal enum HistoryState
	{
		Error,
		Offline,
		Maintenance,
		LoggedOut,
		NoSeat,
		Disabled,
		Waiting,
		Ready
	}
}
